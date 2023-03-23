using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;
using Tesseract.Core.Utilities;

namespace Tesseract.ImGui.Core {

	/// <summary>
	/// Configuration information for the core ImGui renderer.
	/// </summary>
	public readonly struct ImGuiCoreRenderInfo {

		/// <summary>
		/// The pixel format of the framebuffer passed to the renderer.
		/// </summary>
		public required PixelFormat FramebufferFormat { get; init; }

		/// <summary>
		/// The initial layout of the color attachment at the start of rendering.
		/// </summary>
		public required TextureLayout InitialLayout { get; init; }

		/// <summary>
		/// The final layout of the color attachment at the end of rendering.
		/// </summary>
		public required TextureLayout FinalLayout { get; init; }

		/// <summary>
		/// If the existing contents of the color attachment should be preserved.
		/// </summary>
		public bool PreserveFramebuffer { get; init; } = true;

		public ImGuiCoreRenderInfo() { }

	}

	/// <summary>
	/// ImGui renderer implementation for the core graphics interface.
	/// </summary>
	public static class ImGuiCoreRender {

		// Class for holding rendering resources
		private class Resources : IDisposable {

			// Loads the required shader by name and type for the given graphics instance
			private static IShader LoadShader(IGraphics graphics, string name, ShaderType type) {
				var features = graphics.Features;
				ShaderSourceType sourceType = features.PreferredShaderSourceType;

				string typeSuffix = type switch {
					ShaderType.Vertex => "vert",
					ShaderType.Fragment => "frag",
					_ => throw new NotImplementedException()
				};

				string fileSuffix = sourceType switch {
					ShaderSourceType.GLSL => $"-glsl.{typeSuffix}",
					ShaderSourceType.SPIRV => $".{typeSuffix}.spv",
					_ => throw new NotImplementedException(),
				};

				using Stream resourceStream = typeof(ImGuiCoreRender).Assembly.GetManifestResourceStream($"Tesseract.ImGui.OpenGL.Resources.{name}{fileSuffix}")
					?? throw new IOException($"Could not load shader \"{name}\";{type}");

				return graphics.CreateShader(new ShaderCreateInfo() {
					Source = resourceStream.ReadFully(),
					SourceType = sourceType,
					Type = type
				});
			}

			// The vertex format for ImDrawVert
			private static readonly VertexFormat vertexFormat = new(
				attribs: new VertexAttrib[] {
					// Pos
					new VertexAttrib() {
						Binding = 0,
						Format = PixelFormat.R32G32SFloat,
						Location = 0,
						Offset = (uint)Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.Pos))
					},
					// Col
					new VertexAttrib() {
						Binding = 0,
						Format = PixelFormat.R8G8B8A8UNorm,
						Location = 1,
						Offset = (uint)Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.Col))
					},
					// UV
					new VertexAttrib() {
						Binding = 0,
						Format = PixelFormat.R32G32SFloat,
						Location = 2,
						Offset = (uint)Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.UV))
					}
				},
				bindings: new VertexBinding[] {
					new VertexBinding() {
						Binding = 0,
						InputRate = VertexInputRate.PerVertex,
						Stride = (uint)ImDrawVert.SizeOf
					}
				}
			);

			// Global resources

			public IGraphics Graphics { get; }
			public ImGuiCoreRenderInfo Info { get; }

			public ISync Fence { get; }

			// Pipeline resources

			public IRenderPass RenderPass { get; }

			public IShader VertexShader { get; }
			public IShader FragmentShader { get; }

			public BindSetLayoutBinding BindingGlobals { get; }
			public BindSetLayoutBinding BindingTexture { get; }
			public IBindSetLayout BindSetLayout { get; }

			public IPipelineLayout PipelineLayout { get; }
			public IPipeline Pipeline { get; }

			public IBindPool BindPool { get; }

			public ISampler Sampler { get; }

			public ITexture? FontTexture { get; set; } = null;

			// Buffer resources

			private int vertexCapacity = 10000;
			private int indexCapacity = 10000;

			public IBuffer VertexBuffer { get; private set; }
			public IBuffer IndexBuffer { get; private set; }

			public IPointer<ImDrawVert> Vertices { get; private set; }
			public IPointer<ushort> Indices { get; private set; }

			public IVertexArray VertexArray { get; private set; }

			public IBuffer UniformBuffer { get; }
			public IPointer<Matrix4x4> UniformPtr { get; }


			public Resources(IGraphics graphics, ImGuiCoreRenderInfo info) {
				Graphics = graphics;
				Info = info;
				
				Fence = graphics.CreateSync(new SyncCreateInfo() {
					Direction = SyncDirection.GPUToHost,
					Features = SyncFeatures.GPUWorkSignaling | SyncFeatures.HostWaiting,
					Granularity = SyncGranularity.CommandBuffer
				});

				RenderPass = graphics.CreateRenderPass(new RenderPassCreateInfo() {
					Attachments = new RenderPassAttachment[] {
						new RenderPassAttachment() {
							InitialLayout = info.InitialLayout,
							FinalLayout = info.FinalLayout,
							Format = info.FramebufferFormat,
							LoadOp = info.PreserveFramebuffer ? AttachmentLoadOp.Load : AttachmentLoadOp.Clear,
							StoreOp = AttachmentStoreOp.Store,
							Samples = 1
						}
					},
					Subpasses = new RenderPassSubpass[] {
						new RenderPassSubpass() {
							ColorAttachments = new RenderPassAttachmentReference[] {
								new RenderPassAttachmentReference() {
									Attachment = 0,
									Layout = TextureLayout.ColorAttachment
								}
							}
						}
					}
				});

				VertexShader = LoadShader(graphics, "main", ShaderType.Vertex);
				FragmentShader = LoadShader(graphics, "main", ShaderType.Fragment);

				if (graphics.Properties.Type == GraphicsType.OpenGL) {
					Debug.Assert(VertexShader.TryFindBinding("uGlobals", out var bindingGlobals));
					BindingGlobals = bindingGlobals;
					Debug.Assert(FragmentShader.TryFindBinding("uTexture", out var bindingTexture));
					BindingTexture = bindingTexture;
				} else {
					BindingGlobals = new BindSetLayoutBinding() {
						Binding = 0,
						Stages = ShaderType.Vertex,
						Type = BindType.UniformBuffer
					};
					BindingTexture = new BindSetLayoutBinding() {
						Binding = 1,
						Stages = ShaderType.Fragment,
						Type = BindType.CombinedTextureSampler
					};
				}

				BindSetLayout = graphics.CreateBindSetLayout(new BindSetLayoutCreateInfo() {
					Bindings = new BindSetLayoutBinding[] {
						BindingGlobals,
						BindingTexture
					}
				});

				PipelineLayout = graphics.CreatePipelineLayout(new PipelineLayoutCreateInfo() {
					Layouts = new IBindSetLayout[] { BindSetLayout }
				});

				Pipeline = graphics.CreatePipeline(new PipelineCreateInfo() {
					Layout = PipelineLayout,
					GraphicsInfo = new PipelineGraphicsCreateInfo() {
						DynamicInfo = new PipelineDynamicCreateInfo() {
							VertexFormat = vertexFormat,
							DrawMode = DrawMode.TriangleList
						},
						Attachments = new PipelineColorAttachmentState[] {
							new PipelineColorAttachmentState() {
								BlendEnable = true,
								BlendEquation = BlendEquation.AlphaBlend,
								ColorWriteMask = ColorComponent.All
							}
						},
						RenderPass = RenderPass,
						Subpass = 0,

						DynamicState = new PipelineDynamicState[] {
							PipelineDynamicState.Viewport,
							PipelineDynamicState.Scissor
						}
					}
				});

				BindPool = graphics.CreateBindPool(new BindPoolCreateInfo() {
					BindTypeWeights = new (BindType Type, float Weight)[] {
						(BindType.CombinedTextureSampler, 1000),
						(BindType.UniformBuffer, 1000)
					}
				});

				Sampler = graphics.CreateSampler(new SamplerCreateInfo() { });

				VertexBuffer = graphics.CreateBuffer(new BufferCreateInfo() {
					Size = (ulong)(vertexCapacity * ImDrawVert.SizeOf),
					Usage = BufferUsage.VertexBuffer,
					MapFlags = MemoryMapFlags.Write
				});
				Vertices = VertexBuffer.Map<ImDrawVert>(MemoryMapFlags.Write | MemoryMapFlags.Persistent);

				IndexBuffer = graphics.CreateBuffer(new BufferCreateInfo() {
					Size = (ulong)(indexCapacity * sizeof(ushort)),
					Usage = BufferUsage.IndexBuffer,
					MapFlags = MemoryMapFlags.Write
				});
				Indices = IndexBuffer.Map<ushort>(MemoryMapFlags.Write | MemoryMapFlags.Persistent);

				VertexArray = graphics.CreateVertexArray(new VertexArrayCreateInfo() {
					Format = vertexFormat,
					VertexBuffers = new (BufferBinding Binding, uint Index)[] {
						(new BufferBinding() { Buffer = VertexBuffer }, 0)
					},
					IndexBuffer = (new BufferBinding() { Buffer = IndexBuffer }, IndexType.UInt16)
				});

				UniformBuffer = graphics.CreateBuffer(new BufferCreateInfo() {
					Size = (ulong)Marshal.SizeOf<Matrix4x4>(),
					Usage = BufferUsage.UniformBuffer,
					MapFlags = MemoryMapFlags.Write
				});
				UniformPtr = UniformBuffer.Map<Matrix4x4>(MemoryMapFlags.Write | MemoryMapFlags.Persistent);
			}

			// Prepares the resources required for the given draw data
			public void Prepare(IImDrawData drawData) {
				// Rounds up count to avoid constant reallocation
				static int RoundCount(int count) => (int)BitOperations.RoundUpToPowerOf2((uint)count);

				bool recreateVertArray = false;

				// Check vertex buffer size
				int vertexCount = RoundCount(drawData.TotalVtxCount);
				if (vertexCount > vertexCapacity) {
					VertexBuffer.Unmap();
					VertexBuffer.Dispose();
					VertexBuffer = Graphics.CreateBuffer(new BufferCreateInfo() {
						Size = (ulong)(vertexCount * ImDrawVert.SizeOf),
						Usage = BufferUsage.VertexBuffer,
						MapFlags = MemoryMapFlags.Write
					});
					Vertices = VertexBuffer.Map<ImDrawVert>(MemoryMapFlags.Write | MemoryMapFlags.Persistent);
					vertexCapacity = vertexCount;
					recreateVertArray = true;
				}

				// Check index buffer size
				int indexCount = RoundCount(drawData.TotalIdxCount);
				if (indexCount > indexCapacity) {
					IndexBuffer.Unmap();
					IndexBuffer.Dispose();
					IndexBuffer = Graphics.CreateBuffer(new BufferCreateInfo() {
						Size = (ulong)(indexCount * sizeof(ushort)),
						Usage = BufferUsage.IndexBuffer,
						MapFlags = MemoryMapFlags.Write
					});
					Indices = IndexBuffer.Map<ushort>(MemoryMapFlags.Write | MemoryMapFlags.Persistent);
					indexCapacity = indexCount;
					recreateVertArray = true;
				}

				// Recreate vertex array as needed
				if (recreateVertArray) {
					VertexArray.Dispose();
					VertexArray = Graphics.CreateVertexArray(new VertexArrayCreateInfo() {
						Format = vertexFormat,
						VertexBuffers = new (BufferBinding Binding, uint Index)[] {
							(new BufferBinding() { Buffer = VertexBuffer }, 0)
						},
						IndexBuffer = (new BufferBinding() { Buffer = IndexBuffer }, IndexType.UInt16)
					});
				}
			}

			// Creates and maps the font texture
			public void CreateFontTexture() {
				IImGuiIO io = GImGui.IO;
				var graphics = Graphics;

				// Get pixel data for font atlas
				ReadOnlySpan<byte> pixels = io.Fonts.GetTexDataAsRGBA32(out int w, out int h, out int _);

				// Create texture
				FontTexture = graphics.CreateTexture(new TextureCreateInfo() {
					Size = new Vector3ui((uint)w, (uint)h, 1),
					Type = TextureType.Texture2D,
					Format = PixelFormat.R8G8B8A8UNorm,
					Usage = TextureUsage.Sampled
				});

				// Create a temporary buffer to copy pixel data to texture
				IBuffer copyBuffer = graphics.CreateBuffer(new BufferCreateInfo() {
					Size = (ulong)(w * h * 4),
					Usage = BufferUsage.TransferSrc,
					MapFlags = MemoryMapFlags.Write
				});
				pixels.CopyTo(copyBuffer.Map<byte>(MemoryMapFlags.Write).Span);
				copyBuffer.Unmap();

				// Create temporary fence to know when the copy finishes
				var fence = graphics.CreateSync(new SyncCreateInfo() {
					Direction = SyncDirection.GPUToHost,
					Features = SyncFeatures.GPUWorkSignaling | SyncFeatures.HostWaiting,
					Granularity = SyncGranularity.CommandBuffer
				});

				graphics.RunCommands(cmd => {
					// Transition to transfer dst layout
					cmd.Barrier(new ICommandSink.PipelineBarriers() {
						ProvokingStages = PipelineStage.Top,
						AwaitingStages = PipelineStage.Transfer,
						TextureMemoryBarriers = new ICommandSink.TextureMemoryBarrier[] {
						new ICommandSink.TextureMemoryBarrier() {
							Texture = FontTexture,
							AwaitingAccess = 0,
							ProvokingAccess = MemoryAccess.TransferWrite,
							OldLayout = TextureLayout.Undefined,
							NewLayout = TextureLayout.TransferDst,
							SubresourceRange = new TextureSubresourceRange() {
								Aspects = TextureAspect.Color,
								MipLevelCount = FontTexture.MipLevels
							}
						}
					}
					});

					// Copy to the first mip level
					cmd.CopyBufferToTexture(FontTexture, TextureLayout.TransferDst, copyBuffer, new ICommandSink.CopyBufferTexture() {
						TextureSize = FontTexture.Size,
						TextureSubresource = new TextureSubresourceLayers() { Aspects = TextureAspect.Color }
					});

					// Generate mipmaps for the texture
					cmd.GenerateMipmaps(FontTexture, TextureLayout.TransferDst, TextureLayout.ShaderSampled);
				}, CommandBufferUsage.Graphics, new IGraphics.CommandBufferSubmitInfo() {
					SignalSync = new ISync[] { fence }
				});

				// Wait for the commands to complete
				fence.HostWait(ulong.MaxValue);

				// Dispose of temporary resources
				fence.Dispose();
				copyBuffer.Dispose();

				// Map texture and set the font texture ID
				nuint id = MapTexture(FontTexture);
				io.Fonts.SetTexID(id);
			}

			public void Dispose() {
				GC.SuppressFinalize(this);

				Fence.Dispose();

				RenderPass.Dispose();

				VertexShader.Dispose();
				FragmentShader.Dispose();

				BindSetLayout.Dispose();

				PipelineLayout.Dispose();
				Pipeline.Dispose();

				BindPool.Dispose();

				Sampler.Dispose();

				VertexBuffer.Dispose();
				IndexBuffer.Dispose();
				VertexArray.Dispose();

				UniformBuffer.Dispose();

				FontTexture?.Dispose();
			}
		}

		// The resources used for rendering
		private static Resources? resources = null;

		/// <summary>
		/// The render pass used for rendering.
		/// </summary>
		public static IRenderPass RenderPass => resources?.RenderPass ?? throw new InvalidOperationException("Cannot get render pass before renderer is initialized");

		// If the renderer has rendered at least one frame
		private static bool hasRenderedFrame = false;

		/// <summary>
		/// Initializes the core ImGui renderer using the given information.
		/// </summary>
		/// <param name="graphics">Graphics instance to use</param>
		/// <param name="info">Initialization information</param>
		public static void Init(IGraphics graphics, ImGuiCoreRenderInfo info) {
			IImGuiIO io = GImGui.IO;
			io.BackendRendererName = "Tesseract Core Renderer - " + graphics.Properties.Type;

			resources = new Resources(graphics, info);
		}
		
		/// <summary>
		/// Shuts down the core ImGui renderer, disposing of any used resources.
		/// </summary>
		public static void Shutdown() {
			resources?.Dispose();
			resources = null;

			hasRenderedFrame = false;
		}

		// The list of texture IDs which have been recycled
		private static readonly List<int> recycledTextureIDs = new();
		// The list of mapped textures
		private static readonly List<ITexture?> textures = new();
		// The list of bind sets containing mapped textures
		private static readonly List<IBindSet> bindSets = new();

		/// <summary>
		/// Maps a texture for use with ImGui.
		/// </summary>
		/// <param name="texture">The texture to map</param>
		/// <returns>The ID of the mapped texture</returns>
		public static nuint MapTexture(ITexture texture) {
			int id;
			// Try to use recycled IDs first
			if (recycledTextureIDs.Count > 0) {
				id = recycledTextureIDs[0];
				recycledTextureIDs.RemoveAt(0);
				// Update texture and bind set
				textures[id] = texture;
				bindSets[id].Update(new BindSetWrite() {
					TextureInfo = new TextureBinding() {
						Sampler = resources!.Sampler,
						TexureLayout = TextureLayout.ShaderSampled,
						TextureView = texture.IdentityView
					}
				});
			} else {
				// Else add a new texture and bind set
				id = textures.Count;
				textures.Add(texture);
				var set = resources!.BindPool.AllocSet(new BindSetAllocateInfo() {
					Layouts = new IBindSetLayout[] { resources!.BindSetLayout }
				});
				set.Update(
					new BindSetWrite() {
						Binding = resources.BindingGlobals.Binding,
						Type = BindType.UniformBuffer,
						BufferInfo = new BufferBinding() { Buffer = resources!.UniformBuffer },
					},
					new	BindSetWrite() {
						Binding = resources.BindingTexture.Binding,
						Type = BindType.CombinedTextureSampler,
						TextureInfo = new TextureBinding() {
							Sampler = resources!.Sampler,
							TexureLayout = TextureLayout.ShaderSampled,
							TextureView = texture.IdentityView
						}
					}
				);
				bindSets.Add(set);
			}
			return (nuint)id;
		}

		/// <summary>
		/// Unmaps a texture by its ID.
		/// </summary>
		/// <param name="id">The ID of the texture to unmap</param>
		public static void UnmapTexture(nuint id) {
			int iid = (int)id;
			if (iid < textures.Count) {
				var texture = textures[iid];
				if (texture != null) recycledTextureIDs.Add(iid);
				textures[iid] = null;
			}
		}

		/// <summary>
		/// Attempts to get a texture by its ID.
		/// </summary>
		/// <param name="id">The ID of the texture</param>
		/// <returns>The mapped texture for the ID, or null</returns>
		public static ITexture? LookupTexture(nuint id) {
			int iid = (int)id;
			if (iid >= textures.Count) return null;
			var texture = textures[iid];
			texture ??= textures[0];
			return texture;
		}

		/// <summary>
		/// Begins a new frame for rendering.
		/// </summary>
		/// <exception cref="InvalidOperationException">If the renderer has not been initialized</exception>
		public static void NewFrame() {
			if (resources == null) throw new InvalidOperationException("Cannot begin frame until renderer is initialized");

			// Create font texture if the first frame
			if (!hasRenderedFrame) resources.CreateFontTexture();
		}

		// Clear value if the framebuffer contents should not be preserved
		private static readonly ICommandSink.ClearValue[] clearValues = new ICommandSink.ClearValue[] {
			new ICommandSink.ClearValue() {
				Aspect = TextureAspect.Color,
				Color = default
			}
		};

		// Shared list holding synchronization objects to signal at the end of rendering
		private static readonly List<ISync> signalSyncs = new();

		// The render area of the current frame
		private static Recti renderArea;
		// The clipping rectangle offset and scale for the current frame
		private static Vector2 clipOffset, clipScale;

		// Sets the initial rendering state
		private static void SetupRenderState(ICommandSink cmd) {
			// Set pipeline
			cmd.BindPipeline(resources!.Pipeline);

			// Set viewport
			cmd.SetViewport(new Viewport() {
				Area = renderArea,
				DepthBounds = (0, 0)
			});

			// Bind the vertex array
			cmd.BindVertexArray(resources.VertexArray);
		}

		// Renders the commands in a single draw list to the shared buffers
		private static void RenderDrawList(ICommandSink sink, IImDrawList list, ref int vertexOffset, ref int indexOffset) {
			var vertices = resources!.Vertices.Span[vertexOffset..];
			var indices = resources!.Indices.Span[indexOffset..];

			// Copy vertices and indices to the buffers at the current offset
			list.VtxBuffer.AsSpan().CopyTo(vertices);
			list.IdxBuffer.AsSpan().CopyTo(indices);
			// Flush the buffer ranges
			resources!.VertexBuffer.FlushHostToGPU(new MemoryRange() {
				Offset = (ulong)(vertexOffset * ImDrawVert.SizeOf),
				Length = (ulong)(list.VtxBuffer.Count * ImDrawVert.SizeOf)
			});
			resources!.IndexBuffer.FlushHostToGPU(new MemoryRange() {
				Offset = (ulong)(indexOffset * sizeof(ushort)),
				Length = (ulong)(list.IdxBuffer.Count * sizeof(ushort))
			});

			// For each command in the draw list
			foreach (var cmd in list.CmdBuffer) {
				// Handle user callbacks, else just render the vertices
				if (cmd.UserCallback != null) {
					if (cmd.UserCallback == GImGui.ResetRenderState) SetupRenderState(sink);
					else cmd.UserCallback(list, cmd);
				} else {
					// Compute clip region
					Vector2 clipMin = (new Vector2(cmd.ClipRect.X, cmd.ClipRect.Y) - clipOffset) * clipScale;
					Vector2 clipMax = (new Vector2(cmd.ClipRect.Z, cmd.ClipRect.W) - clipOffset) * clipScale;
					if (clipMax.X <= clipMin.X || clipMax.Y <= clipMin.Y)
						continue;

					// Set scissor to clip area
					sink.SetScissor(new Recti((Vector2i)clipMin, (Vector2i)(clipMax - clipMin)));

					// Bind resource set for texture ID
					var bindSet = bindSets[(int)cmd.TextureID];
					sink.BindResources(PipelineType.Graphics, resources!.PipelineLayout, bindSet);

					// Draw elements
					sink.DrawIndexed(cmd.ElemCount, 1, (uint)(cmd.IdxOffset + indexOffset), (int)(cmd.VtxOffset + vertexOffset), 0);
				}
			}

			// Adjust offsets based on the vertex/index count of the list
			vertexOffset += list.VtxBuffer.Count;
			indexOffset += list.IdxBuffer.Count;
		}

		/// <summary>
		/// Renders ImGui draw data to the given framebuffer.
		/// </summary>
		/// <param name="drawData">The draw data to render</param>
		/// <param name="framebuffer">The framebuffer to render to</param>
		/// <param name="submitInfo">The base command submission information to use</param>
		/// <exception cref="InvalidOperationException">If the renderer is not initialized</exception>
		public static void RenderDrawData(IImDrawData drawData, IFramebuffer framebuffer, IGraphics.CommandBufferSubmitInfo submitInfo) {
			if (resources == null) throw new InvalidOperationException("Cannot render draw data until renderer is initialized");
			
			// Wait for fence indicating we can modify resources
			var fence = resources.Fence;
			if (hasRenderedFrame) fence.HostWait(ulong.MaxValue);
			fence.HostReset();

			// Prepare resources for the given draw data
			resources.Prepare(drawData);

			// Get variables for rendering
			renderArea = new Recti(framebuffer.Size);

			// Setup orthographic projection
			float L = drawData.DisplayPos.X;
			float R = drawData.DisplayPos.X + drawData.DisplaySize.X;
			float T = drawData.DisplayPos.Y;
			float B = drawData.DisplayPos.Y + drawData.DisplaySize.Y;
			Matrix4x4 orthoProjection = new(
				2.0f / (R - L), 0, 0, 0,
				0, 2.0f / (T - B), 0, 0,
				0, 0, -1, 0,
				(R + L) / (L - R), (T + B) / (B - T), 0, 1
			);

			// Setup clipping information
			clipOffset = drawData.DisplayPos;
			clipScale = drawData.FramebufferScale;

			// Initialize list of signal synchronization objects
			signalSyncs.Clear();
			signalSyncs.AddRange(submitInfo.SignalSync);
			signalSyncs.Add(fence);

			// Run the set of commands
			resources.Graphics.RunCommands(cmd => {
				// Begin rendering
				cmd.BeginRenderPass(new ICommandSink.RenderPassBegin() {
					RenderArea = renderArea,
					Framebuffer = framebuffer,
					RenderPass = resources.RenderPass,
					ClearValues = clearValues
				}, SubpassContents.Inline);

				// Setup initial state
				SetupRenderState(cmd);

				// Render each draw list
				int vertexOffset = 0;
				int indexOffset = 0;
				foreach(var list in drawData.CmdLists) RenderDrawList(cmd, list, ref vertexOffset, ref indexOffset);

				// End rendering
				cmd.EndRenderPass();
			}, CommandBufferUsage.Graphics, submitInfo with { SignalSync = signalSyncs });

			hasRenderedFrame = true;
		}

	}

}

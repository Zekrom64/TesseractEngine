using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.OpenGL.Native;
using Tesseract.Core.Numerics;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {

	/// <summary>
	/// Ranged buffer binding state.
	/// </summary>
	public record struct GLBufferRangeBinding {

		/// <summary>
		/// Bound buffer.
		/// </summary>
		public uint Buffer { get; set; }

		/// <summary>
		/// Binding offset.
		/// </summary>
		public nint Offset { get; set; }

		/// <summary>
		/// Binding length.
		/// </summary>
		public nint Length { get; set; }

	}
	
	/// <summary>
	/// State manager for the OpenGL context. This is used to keep track of the current OpenGL state to simplify
	/// state inspection when needed and reduce duplicate calls to OpenGL.
	/// </summary>
	public class GLState : IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		// Individual texture unit state
		private struct TextureUnit {

			// The target currently bound to this unit
			/*   OpenGL technically can bind to different targets within a texture unit but
			 *   it is easier to pretend units only hold one texture with a specific target.
			 */
			public GLTextureTarget Target;
			// The texture bound to this unit
			public uint Texture;
			// The sampler bound to this unit
			public uint Sampler;

		}

		// Texture unit states
		private uint activeTextureUnit;
		private readonly TextureUnit[] textureUnits; 

		// Buffer states
		private uint bufferArray;
		private uint bufferCopyRead;
		private uint bufferCopyWrite;
		private uint bufferDispatchIndirect;
		private uint bufferDrawIndirect;
		private uint bufferElementArray;
		private uint bufferPixelPack;
		private uint bufferPixelUnpack;
		private uint bufferQuery;
		private uint bufferTexture;

		// Ranged buffer states
		private readonly GLBufferRangeBinding[] bufferRangeAtomicCounter;
		private readonly GLBufferRangeBinding[] bufferRangeShaderStorage;
		private readonly GLBufferRangeBinding[] bufferRangeUniform;
		private readonly GLBufferRangeBinding[] bufferRangeTransformFeedback;

		// Program state
		private uint program;

		/// <summary>
		/// A set containing all the supported program binary formats.
		/// </summary>
		public IReadOnlySet<uint> ProgramBinaryFormats { get; }

		// Vertex array state
		/// <summary>
		/// The currently bound vertex array.
		/// </summary>
		public GLVertexArray? VertexArray { get; private set; }

		/// <summary>
		/// The type of indices in the current vertex array.
		/// </summary>
		public GLIndexType IndexType => VertexArray != null ? VertexArray.IndexType!.Value : default;

		/// <summary>
		/// The offset of indices in the current vertex array.
		/// </summary>
		public nint IndexOffset => VertexArray != null ? VertexArray.IndexOffset : 0;

		/// <summary>
		/// The stride in bytes between indices in the current vertex array.
		/// </summary>
		public int IndexStride {
			get {
				if (VertexArray == null) return 0;
				return VertexArray.IndexType switch {
					GLIndexType.UnsignedByte => 1,
					GLIndexType.UnsignedShort => 2,
					GLIndexType.UnsignedInt => 4,
					_ => 0,
				};
			}
		}

		// Framebuffer state
		private uint framebufferDraw;
		private uint framebufferRead;

		// Renderbuffer state
		private uint renderbuffer;

		// Pixel store state
		private int packRowLength;
		private int packImageHeight;
		private int unpackRowLength;
		private int unpackImageHeight;


		// Gets the reference to a buffer binding state
		private ref uint GetBufferBinding(GLBufferTarget target, out bool valid) {
			valid = true;
			switch(target) {
				case GLBufferTarget.Array: return ref bufferArray;
				case GLBufferTarget.CopyRead: return ref bufferCopyRead;
				case GLBufferTarget.CopyWrite: return ref bufferCopyWrite;
				case GLBufferTarget.DispatchIndirect: return ref bufferDispatchIndirect;
				case GLBufferTarget.DrawIndirect: return ref bufferDrawIndirect;
				case GLBufferTarget.ElementArray: return ref bufferElementArray;
				case GLBufferTarget.PixelPack: return ref bufferPixelPack;
				case GLBufferTarget.PixelUnpack: return ref bufferPixelUnpack;
				case GLBufferTarget.Query: return ref bufferQuery;
				case GLBufferTarget.Texture: return ref bufferTexture;
				default:
					valid = false;
					return ref bufferArray;
			}
		}

		// Gets the reference to a ranged buffer binding state
		private ref GLBufferRangeBinding GetBufferRangeBinding(GLBufferRangeTarget target, uint index, out bool valid) {
			valid = true;
			switch(target) {
				case GLBufferRangeTarget.AtomicCounter: return ref bufferRangeAtomicCounter[index];
				case GLBufferRangeTarget.ShaderStorage: return ref bufferRangeShaderStorage[index];
				case GLBufferRangeTarget.Uniform: return ref bufferRangeUniform[index];
				case GLBufferRangeTarget.TransformFeedback: return ref bufferRangeTransformFeedback[index];
				default:
					valid = false;
					return ref bufferRangeUniform[0];
			}
		}

		private ref int GetPixelStoreVar(GLPixelStoreParam param, out bool valid) {
			valid = true;
			switch (param) {
				case GLPixelStoreParam.PackRowLength: return ref packRowLength;
				case GLPixelStoreParam.PackImageHeight: return ref packImageHeight;
				case GLPixelStoreParam.UnpackRowLength: return ref unpackRowLength;
				case GLPixelStoreParam.UnpackImageHeight: return ref unpackImageHeight;
				default:
					valid = false;
					return ref packRowLength;
			}
		}

		public GLState(GLGraphics graphics) {
			Graphics = graphics;
			var gl33 = GL.GL33!;

			// Initialize uniform units
			textureUnits = new TextureUnit[gl33.GetInteger(Native.GLEnums.GL_MAX_TEXTURE_IMAGE_UNITS)];
			
			bufferRangeUniform = new GLBufferRangeBinding[gl33.GetInteger(Native.GLEnums.GL_MAX_UNIFORM_BUFFER_BINDINGS)];
			bufferRangeTransformFeedback = new GLBufferRangeBinding[gl33.GetInteger(Native.GLEnums.GL_MAX_TRANSFORM_FEEDBACK_BUFFERS)];
			if (GL.ARBShaderAtomicCounters != null) {
				bufferRangeAtomicCounter = new GLBufferRangeBinding[gl33.GetInteger(Native.GLEnums.GL_MAX_ATOMIC_COUNTER_BUFFER_BINDINGS)];
			} else {
				bufferRangeAtomicCounter = Array.Empty<GLBufferRangeBinding>();
			}
			if (GL.ARBShaderStorageBufferObject != null) {
				bufferRangeShaderStorage = new GLBufferRangeBinding[gl33.GetInteger(Native.GLEnums.GL_MAX_SHADER_STORAGE_BUFFER_BINDINGS)];
			} else {
				bufferRangeShaderStorage = Array.Empty<GLBufferRangeBinding>();
			}

			// Get list of program binary formats if possible
			HashSet<uint> programBinaryFormats = new();
			if (GL.ARBGetProgramBinary != null) {
				int nformats = gl33.GetInteger(Native.GLEnums.GL_NUM_PROGRAM_BINARY_FORMATS);
				Span<int> formats = gl33.GetInteger(Native.GLEnums.GL_PROGRAM_BINARY_FORMATS, stackalloc int[nformats]);
				foreach (int format in formats) programBinaryFormats.Add((uint)format);
			}
			ProgramBinaryFormats = programBinaryFormats;

			int nbufs = gl33.GetInteger(Native.GLEnums.GL_MAX_DRAW_BUFFERS);
			colorWriteMask = new ColorComponent[nbufs];
			for (int i = 0; i < nbufs; i++) colorWriteMask[i] = ColorComponent.All;

			// Setup OpenGL state to make sure it matches initial state
			gl33.Disable(GLCapability.DepthClamp);
			gl33.PolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);
			gl33.Disable(GLCapability.ColorLogicOp);
			gl33.Disable(GLCapability.PrimitiveRestart);
			gl33.CullFace = false;
			gl33.CullFaceMode = GLFace.Back;
			gl33.FrontFace = GLCullFace.Clockwise;
			gl33.LineWidth = 0.0f;
			gl33.Disable(GLCapability.PolygonOffsetFill);
			gl33.Disable(GLCapability.PolygonOffsetPoint);
			gl33.Disable(GLCapability.PolygonOffsetLine);
			gl33.Disable(GLCapability.DepthTest);
			gl33.DepthMask = false;
			gl33.DepthFunc = GLCompareFunc.Less;
			if (GL.EXTDepthBoundsTest != null) {
				gl33.Disable(GLCapability.DepthBoundsTestEXT);
				GL.EXTDepthBoundsTest.DepthBounds = (0, 0);
			}
			gl33.Disable(GLCapability.StencilTest);
			gl33.StencilFunc(default, default, default);
			gl33.StencilOp(default, default, default);
			gl33.StencilMask = default;
			gl33.LogicOp = GLLogicOp.NoOp;
			colorWriteEnable = new bool[nbufs];
			for (int i = 0; i < nbufs; i++) {
				gl33.ColorMask((uint)i, true, true, true, true);
				colorWriteEnable[i] = true;
			}
		}

		/// <summary>
		/// The active texture unit.
		/// </summary>
		public uint ActiveTextureUnit {
			get => activeTextureUnit;
			set {
				if (value != activeTextureUnit) {
					GL.GL20!.ActiveTexture = (int)value;
					activeTextureUnit = value;
				}
			}
		}

		/// <summary>
		/// Binds a texture to the given target in the current texture unit.
		/// </summary>
		/// <param name="target">Texture target</param>
		/// <param name="texture">Texture to bind</param>
		public void BindTexture(GLTextureTarget target, uint texture) {
			ref TextureUnit texunit = ref textureUnits[ActiveTextureUnit];
			if (texunit.Target == target && texunit.Texture == texture) return;
			GL.GL33!.BindTexture(target, texture);
			texunit.Target = target;
			texunit.Texture = texture;
		}

		/// <summary>
		/// Binds a texture to the given target in the given texture unit.
		/// </summary>
		/// <param name="unit">Texture unit</param>
		/// <param name="target">Texture target</param>
		/// <param name="texture">Texture to bind</param>
		public void BindTextureUnit(uint unit, GLTextureTarget target, uint texture) {
			ref TextureUnit texunit = ref textureUnits[unit];
			if (texunit.Target == target && texunit.Texture == texture) return;
			Graphics.Interface.BindTextureUnit(unit, target, texture);
			texunit.Target = target;
			texunit.Texture = texture;
		}

		/*
		public (uint, GLTextureTarget) BindTextureAny(uint texture, GLTextureTarget defaultTarget) {
			for(uint i = 0; i < textureUnits.Length; i++) {
				TextureUnit texunit = textureUnits[i];
				if (texunit.Texture == texture) {
					return (i, texunit.Target);
				}
			}
			BindTextureUnit(0, defaultTarget, texture);
			return (0, defaultTarget);
		}
		*/

		/// <summary>
		/// Binds a sampler to the given texture unit.
		/// </summary>
		/// <param name="unit">Texture unit</param>
		/// <param name="sampler">Sampler to bind</param>
		public void BindSampler(uint unit, uint sampler) {
			ref TextureUnit texunit = ref textureUnits[unit];
			if (texunit.Sampler == sampler) return;
			GL.GL33!.BindSampler(unit, sampler);
			texunit.Sampler = sampler;
		}

		/// <summary>
		/// Binds a buffer to the given target.
		/// </summary>
		/// <param name="target">Buffer target</param>
		/// <param name="buffer">Buffer to bind</param>
		public void BindBuffer(GLBufferTarget target, uint buffer) {
			ref uint binding = ref GetBufferBinding(target, out bool valid);
			if (valid && binding == buffer) return;
			GL.GL33!.BindBuffer(target, buffer);
			if (valid) binding = buffer;
		}

		/// <summary>
		/// Sets the buffer bound to the given target. This may be used when buffer binding must be
		/// forced to modify the current OpenGL state to keep the state manager in sync.
		/// </summary>
		/// <param name="target">Buffer target</param>
		/// <param name="buffer">Buffer to set</param>
		public void SetBoundBuffer(GLBufferTarget target, uint buffer) {
			ref uint binding = ref GetBufferBinding(target, out bool valid);
			if (valid) binding = buffer;
		}

		/// <summary>
		/// Checks if a buffer is currently bound, binding it to a default target if not, and returns
		/// the target it is bound to.
		/// </summary>
		/// <param name="buffer">Buffer to bind</param>
		/// <param name="defaultTarget">Default buffer target</param>
		/// <returns>Target the buffer is bound to</returns>
		public GLBufferTarget BindBufferAny(uint buffer, GLBufferTarget defaultTarget = GLBufferTarget.Array) {
			if (bufferArray == buffer) return GLBufferTarget.Array;
			if (bufferCopyRead == buffer) return GLBufferTarget.CopyRead;
			if (bufferCopyWrite == buffer) return GLBufferTarget.CopyWrite;
			if (bufferDispatchIndirect == buffer) return GLBufferTarget.DispatchIndirect;
			if (bufferDrawIndirect == buffer) return GLBufferTarget.DrawIndirect;
			if (bufferElementArray == buffer) return GLBufferTarget.ElementArray;
			if (bufferPixelPack == buffer) return GLBufferTarget.PixelPack;
			if (bufferPixelUnpack == buffer) return GLBufferTarget.PixelUnpack;
			if (bufferQuery == buffer) return GLBufferTarget.Query;
			if (bufferTexture == buffer) return GLBufferTarget.Texture;
			GL.GL33!.BindBuffer(defaultTarget, buffer);
			return defaultTarget;
		}

		/// <summary>
		/// Binds a range of a buffer to a ranged buffer target.
		/// </summary>
		/// <param name="target>Buffer target</param>
		/// <param name="index">Index within target to bind to</param>
		/// <param name="binding">Ranged buffer binding</param>
		public void BindBufferRange(GLBufferRangeTarget target, uint index, GLBufferRangeBinding binding) {
			ref GLBufferRangeBinding currentBinding = ref GetBufferRangeBinding(target, index, out bool valid);
			if (valid && currentBinding == binding) return;
			GL.GL33!.BindBufferRange(target, index, binding.Buffer, binding.Offset, binding.Length);
			if (valid) currentBinding = binding;
		}

		/// <summary>
		/// Uses the given program.
		/// </summary>
		/// <param name="program">Program to use</param>
		public void UseProgram(uint program) {
			if (this.program == program) return;
			GL.GL33!.UseProgram(program);
			this.program = program;
		}

		/// <summary>
		/// Binds a vertex array.
		/// </summary>
		/// <param name="vertexArray">Vertex array to bind</param>
		public void BindVertexArray(GLVertexArray vertexArray) {
			if (VertexArray == vertexArray) return;
			var gl33 = GL.GL33!;
			gl33.BindVertexArray(vertexArray.ID);
			VertexArray = vertexArray;
			// If has primitive restart and indices, update primitive restart index for index type
			// The spec doesn't really describe how the comparison is done, so we update it for the index type "just in case"
			if (primitiveRestartEnable && vertexArray.IndexType != null) {
				switch (vertexArray.IndexType.Value) {
					case GLIndexType.UnsignedByte:
						gl33.PrimitiveRestartIndex(0xFF);
						break;
					case GLIndexType.UnsignedShort:
						gl33.PrimitiveRestartIndex(0xFFFF);
						break;
					case GLIndexType.UnsignedInt:
						gl33.PrimitiveRestartIndex(0xFFFFFFFF);
						break;
				}
			}
		}

		/// <summary>
		/// Binds a framebuffer to the given target.
		/// </summary>
		/// <param name="target">Framebuffer target</param>
		/// <param name="framebuffer">Framebuffer to bind</param>
		public void BindFramebuffer(GLFramebufferTarget target, uint framebuffer) {
			var gl33 = GL.GL33!;
			switch(target) {
				case GLFramebufferTarget.Draw:
					if (framebufferDraw == framebuffer) return;
					gl33.BindFramebuffer(GLFramebufferTarget.Draw, framebuffer);
					framebufferDraw = framebuffer;
					break;
				case GLFramebufferTarget.Read:
					if (framebufferRead == framebuffer) return;
					gl33.BindFramebuffer(GLFramebufferTarget.Read, framebuffer);
					framebufferRead = framebuffer;
					break;
				case GLFramebufferTarget.Framebuffer:
					if (framebufferDraw == framebuffer && framebufferRead == framebuffer) return;
					gl33.BindFramebuffer(GLFramebufferTarget.Framebuffer, framebuffer);
					framebufferDraw = framebufferRead = framebuffer;
					break;
			}
		}

		/// <summary>
		/// Binds a renderbuffer to the given target.
		/// </summary>
		/// <param name="target">Renderbuffer target</param>
		/// <param name="renderbuffer">Renderbuffer to bind</param>
		public void BindRenderbuffer(GLRenderbufferTarget target, uint renderbuffer) {
			if (target == GLRenderbufferTarget.Renderbuffer && this.renderbuffer == renderbuffer) return;
			GL.GL33!.BindRenderbuffer(target, renderbuffer);
			if (target == GLRenderbufferTarget.Renderbuffer) this.renderbuffer = renderbuffer;
		}


		private readonly Stack<uint> drawFramebufferStack = new();

		/// <summary>
		/// Pushes the current draw framebuffer, and binds a new one in its place.
		/// </summary>
		/// <param name="framebuffer">Draw framebuffer to bind</param>
		public void PushDrawFramebuffer(uint framebuffer) {
			drawFramebufferStack.Push(framebufferDraw);
			BindFramebuffer(GLFramebufferTarget.Draw, framebuffer);
		}

		/// <summary>
		/// Pops the current draw framebuffer, binding the previously pushed buffer.
		/// </summary>
		public void PopDrawFramebuffer() => BindFramebuffer(GLFramebufferTarget.Draw, drawFramebufferStack.Pop());

		/// <summary>
		/// Sets the given pixel store parameter.
		/// </summary>
		/// <param name="pname">Pixel store parameter</param>
		/// <param name="value">Parameter value</param>
		public void PixelStore(GLPixelStoreParam pname, int value) {
			ref int state = ref GetPixelStoreVar(pname, out bool valid);
			if (valid && state == value) return;
			GL.GL33!.PixelStore(pname, value);
			if (valid) state = value;
		}


		//========================//
		// Render Pass Management //
		//========================//

		/// <summary>
		/// The current render pass used for rendering, or null if there is none.
		/// </summary>
		public GLRenderPass? CurrentRenderPass { get; private set; } = null;

		/// <summary>
		/// The current framebuffer used for rendering, or null if there is none.
		/// </summary>
		public GLFramebuffer? CurrentFramebuffer { get; private set; } = null;

		/// <summary>
		/// The current area being rendered to.
		/// </summary>
		public Recti CurrentRenderArea { get; private set; }

		/// <summary>
		/// The current renderpass' subpass.
		/// </summary>
		public int CurrentSubpass { get; private set; } = -1;

		/// <summary>
		/// Begins a render pass.
		/// </summary>
		/// <param name="beginInfo">Render pass begin information</param>
		public void BeginRenderPass(ICommandSink.RenderPassBegin beginInfo) {
			CurrentRenderPass = (GLRenderPass)beginInfo.RenderPass;
			CurrentFramebuffer = (GLFramebuffer)beginInfo.Framebuffer;
			CurrentRenderArea = beginInfo.RenderArea;
			CurrentSubpass = 0;
			CurrentFramebuffer.BeginRenderPass(beginInfo);
			CurrentFramebuffer.BeginSubpass();
		}

		/// <summary>
		/// Moves to the next subpass in the current render pass.
		/// </summary>
		public void NextSubpass() {
			if (CurrentRenderPass != null) {
				CurrentFramebuffer!.EndSubpass();
				CurrentSubpass++;
				CurrentFramebuffer.BeginSubpass();
			}
		}

		/// <summary>
		/// Ends the current render pass.
		/// </summary>
		public void EndRenderPass() {
			CurrentFramebuffer!.EndRenderPass();
			CurrentRenderPass = null;
			CurrentFramebuffer = null;
			CurrentSubpass = -1;
		}

		//================//
		// Pipeline State //
		//================//

		private const float FloatStateEpsilon = 0.001f;

		public GLPipeline? Pipeline { get; private set; }

		// Statics

		private bool depthClampEnable = false;

		private GLPolygonMode polygonMode = GLPolygonMode.Fill;

		private bool logicOpEnable = false;

		private bool primitiveRestartEnable = false;

		private readonly ColorComponent[] colorWriteMask;

		// Dynamics

		public GLDrawMode DrawMode { get; private set; }

		private uint patchControlPoints = 0;

		private bool rasterizerDiscardEnable = false;

		private bool cullModeEnable = false;

		private GLFace cullMode = GLFace.Back;

		private GLCullFace frontFace = GLCullFace.Clockwise;

		private float lineWidth = 0.0f;

		private bool depthBiasEnable = false;

		private GLPipeline.DepthBiasFactors depthBias = default;

		private bool depthTestEnable = false;

		private bool depthWriteEnable = false;

		private GLCompareFunc depthCompareOp = GLCompareFunc.Less;

		private bool depthBoundsTestEnable = false;

		private bool stencilTestEnable = false;

		private (GLPipeline.StencilOpState, GLPipeline.StencilOpState) stencilOp = default;

		private (uint, uint) stencilCompareMask = default;

		private (uint, uint) stencilWriteMask = default;

		private (int, int) stencilReference = default;

		private (float, float) depthBounds = (0, 0);

		private GLLogicOp logicOp = GLLogicOp.NoOp;

		private Vector4 blendConstants = Vector4.Zero;

		private readonly bool[] colorWriteEnable;


		public void BindPipeline(GLPipeline? pipeline) {
			// Don't update pipeline if they are the same
			if (Pipeline == pipeline) return;
			Pipeline = pipeline;
			if (pipeline != null) {
				var gl33 = GL.GL33!;

				// Setup static state
				UseProgram(pipeline.ShaderProgramID);
				if (pipeline.DepthClampEnable ^ depthClampEnable) {
					depthClampEnable = pipeline.DepthClampEnable;
					if (depthClampEnable) gl33.Enable(GLCapability.DepthClamp);
					else gl33.Disable(GLCapability.DepthClamp);
				}
				if (pipeline.PolygonMode != polygonMode) {
					polygonMode = pipeline.PolygonMode;
					gl33.PolygonMode(GLFace.FrontAndBack, polygonMode);
				}
				if (pipeline.LogicOpEnable ^ logicOpEnable) {
					logicOpEnable = pipeline.LogicOpEnable;
					if (logicOpEnable) gl33.Enable(GLCapability.ColorLogicOp);
					else gl33.Disable(GLCapability.ColorLogicOp);
				}
				// Don't bother checking attachment state against current
				var colorWriteEnable = pipeline.ColorWriteEnable; // Shortcut checking color write enable ahead of time
				for (int i = 0; i < pipeline.Attachments.Length; i++) {
					var attach = pipeline.Attachments[i];
					colorWriteMask[i] = attach.ColorWriteMask;
					if (colorWriteEnable != null && !colorWriteEnable[i]) gl33.ColorMask((uint)i, false, false, false, false); // If no color write enabled, set all to false
					else {
						// Else set based on color write mask
						var mask = attach.ColorWriteMask;
						gl33.ColorMask((uint)i, (mask & ColorComponent.Red) != 0, (mask & ColorComponent.Green) != 0, (mask & ColorComponent.Blue) != 0, (mask & ColorComponent.Alpha) != 0);
					}
				}
				Graphics.Interface.SetAttachmentsBlendState(pipeline.Attachments);

				// Handle dynamic state
				if (pipeline.DrawMode != null) SetDrawMode(pipeline.DrawMode.Value);
				if (pipeline.PrimitiveRestartEnable != null) SetPrimitiveRestartEnable(pipeline.PrimitiveRestartEnable.Value);
				if (pipeline.PatchControlPoints != null) SetPatchControlPoints(pipeline.PatchControlPoints.Value);
				if (pipeline.Viewports != null) SetViewports(pipeline.Viewports);
				if (pipeline.Scissors != null) SetScissors(pipeline.Scissors);
				if (pipeline.RasterizerDiscardEnable != null) SetRasterizerDiscardEnable(pipeline.RasterizerDiscardEnable.Value);
				if (pipeline.CullMode != null) SetCullMode(pipeline.CullMode.Value.Enable, pipeline.CullMode.Value.CullFace);
				if (pipeline.FrontFace != null) SetFrontFace(pipeline.FrontFace.Value);
				if (pipeline.LineWidth != null) SetLineWidth(pipeline.LineWidth.Value);
				if (pipeline.DepthBiasEnable != null) SetDepthBiasEnable(pipeline.DepthBiasEnable.Value);
				if (pipeline.DepthBias != null) SetDepthBias(pipeline.DepthBias.Value);
				if (pipeline.DepthTestEnable != null) SetDepthTestEnable(pipeline.DepthTestEnable.Value);
				if (pipeline.DepthWriteEnable != null) SetDepthWriteEnable(pipeline.DepthWriteEnable.Value);
				if (pipeline.DepthCompareOp != null) SetDepthCompareOp(pipeline.DepthCompareOp.Value);
				if (pipeline.DepthBoundsTestEnable != null) SetDepthBoundsTestEnable(pipeline.DepthBoundsTestEnable.Value);
				if (pipeline.StencilTestEnable != null) SetStencilTestEnable(pipeline.StencilTestEnable.Value);
				// Complex test for setting the stencil state, since OpenGL groups paramters differently for function calls
				// First, try to load reference & compare mask values from the pipeline
				(bool, bool) stencilrefmaskflag = (false, false);
				if (pipeline.StencilReference != null) {
					var refr = pipeline.StencilReference.Value;
					if (refr.Item1 != stencilReference.Item1) {
						stencilReference.Item1 = refr.Item1;
						stencilrefmaskflag.Item1 = true;
					}
					if (refr.Item2 != stencilReference.Item2) {
						stencilReference.Item2 = refr.Item2;
						stencilrefmaskflag.Item2 = true;
					}
				}
				if (pipeline.StencilCompareMask != null) {
					var mask = pipeline.StencilCompareMask.Value;
					if (mask.Item1 != stencilCompareMask.Item1) {
						stencilCompareMask.Item1 = mask.Item1;
						stencilrefmaskflag.Item1 = true;
					}
					if (mask.Item2 != stencilCompareMask.Item2) {
						stencilCompareMask.Item2 = mask.Item2;
						stencilrefmaskflag.Item2 = true;
					}
				}
				// Next, try to set stencil state if there are fixed stencil ops 
				(bool, bool) stencilrefmaskset = (false, false);
				if (pipeline.StencilOp != null) {
					var op = pipeline.StencilOp.Value;
					var frontop = op.Item1;
					if (frontop != stencilOp.Item1) {
						stencilOp.Item1 = frontop;
						gl33.StencilOpSeparate(GLFace.Front, frontop.FailOp, frontop.DepthFailOp, frontop.PassOp);
						gl33.StencilFuncSeparate(GLFace.Front, frontop.CompareOp, stencilReference.Item1, stencilCompareMask.Item1);
						stencilrefmaskset.Item1 = true;
					}
					var backop = op.Item2;
					if (backop != stencilOp.Item2) {
						stencilOp.Item2 = backop;
						gl33.StencilOpSeparate(GLFace.Back, backop.FailOp, backop.DepthFailOp, backop.PassOp);
						gl33.StencilFuncSeparate(GLFace.Back, backop.CompareOp, stencilReference.Item2, stencilCompareMask.Item2);
						stencilrefmaskset.Item2 = true;
					}
				}
				// Finally, if reference/mask values were loaded but not set in the last step, set them now
				if (stencilrefmaskflag.Item1 && !stencilrefmaskset.Item1)
					gl33.StencilFuncSeparate(GLFace.Front, stencilOp.Item1.CompareOp, stencilReference.Item1, stencilCompareMask.Item1);
				if (stencilrefmaskflag.Item2 && !stencilrefmaskset.Item2)
					gl33.StencilFuncSeparate(GLFace.Back, stencilOp.Item2.CompareOp, stencilReference.Item2, stencilCompareMask.Item2);
				if (pipeline.StencilWriteMask != null) {
					var mask = pipeline.StencilWriteMask.Value;
					if (mask.Item1 == mask.Item2) {
						SetStencilWriteMask(GLFace.FrontAndBack, mask.Item1);
					} else {
						SetStencilWriteMask(GLFace.Front, mask.Item1);
						SetStencilWriteMask(GLFace.Back, mask.Item2);
					}
				}
				if (pipeline.DepthBounds != null) {
					var db = pipeline.DepthBounds.Value;
					SetDepthBounds(db.Item1, db.Item2);
				}
				if (pipeline.LogicOp != null) SetLogicOp(pipeline.LogicOp.Value);
				if (pipeline.BlendConstant != null) SetBlendConstants(pipeline.BlendConstant.Value);
			}
		}

		public void SetDrawMode(GLDrawMode mode) => DrawMode = mode;

		public void SetPrimitiveRestartEnable(bool enable) {
			var gl33 = GL.GL33!;
			if (enable ^ primitiveRestartEnable) {
				primitiveRestartEnable = enable;
				if (primitiveRestartEnable) {
					gl33.Enable(GLCapability.PrimitiveRestart);
					if (VertexArray != null && VertexArray.IndexType != null) {
						switch (VertexArray.IndexType.Value) {
							case GLIndexType.UnsignedByte:
								gl33.PrimitiveRestartIndex(0xFF);
								break;
							case GLIndexType.UnsignedShort:
								gl33.PrimitiveRestartIndex(0xFFFF);
								break;
							case GLIndexType.UnsignedInt:
								gl33.PrimitiveRestartIndex(0xFFFFFFFF);
								break;
						}
					}
				} else gl33.Disable(GLCapability.PrimitiveRestart);
			}
		}

		public void SetPatchControlPoints(uint controlPoints) {
			if (patchControlPoints != controlPoints) {
				patchControlPoints = controlPoints;
				Graphics.Interface.SetPatchControlPoints(controlPoints);
			}
		}

		public void SetViewports(in ReadOnlySpan<Viewport> viewports, uint first = 0) => Graphics.Interface.SetViewports(first, viewports);

		public void SetScissors(in ReadOnlySpan<Recti> scissors, uint first = 0) => Graphics.Interface.SetScissors(first, scissors);

		public void SetRasterizerDiscardEnable(bool enable) {
			var gl33 = GL.GL33!;
			if (rasterizerDiscardEnable ^ enable) {
				rasterizerDiscardEnable = enable;
				if (rasterizerDiscardEnable) gl33.Enable(GLCapability.RasterizerDiscard);
				else gl33.Disable(GLCapability.RasterizerDiscard);
			}
		}

		public void SetCullMode(bool enable, GLFace mode) {
			var gl33 = GL.GL33!;
			if (cullModeEnable ^ enable)
					gl33.CullFace = cullModeEnable = enable;
			if (cullMode != mode) {
				cullMode = mode;
				gl33.CullFaceMode = cullMode;
			}
		}

		public void SetFrontFace(GLCullFace face) {
			var gl33 = GL.GL33!;
			if (frontFace != face) {
				frontFace = face;
				gl33.FrontFace = face;
			}
		}

		public void SetLineWidth(float width) {
			var gl33 = GL.GL33!;
			if(!ExMath.EqualsAbout(lineWidth, width, FloatStateEpsilon))
					gl33.LineWidth = lineWidth = width;
		}

		public void SetDepthBiasEnable(bool enable) {
			var gl33 = GL.GL33!;
			if (depthBiasEnable ^ enable) {
				depthBiasEnable = enable;
				GLCapability cap = polygonMode switch {
					GLPolygonMode.Fill => GLCapability.PolygonOffsetFill,
					GLPolygonMode.Line => GLCapability.PolygonOffsetLine,
					GLPolygonMode.Point => GLCapability.PolygonOffsetPoint,
					_ => default
				};
				if (depthBiasEnable) gl33.Enable(cap);
				else gl33.Disable(cap);
			}
		}

		public void SetDepthBias(GLPipeline.DepthBiasFactors db) {
			if (!(
				ExMath.EqualsAbout(db.ConstantFactor, depthBias.ConstantFactor, FloatStateEpsilon) &&
				ExMath.EqualsAbout(db.SlopeFactor, depthBias.SlopeFactor, FloatStateEpsilon) &&
				ExMath.EqualsAbout(db.Clamp, depthBias.Clamp, FloatStateEpsilon)
			)) {
				depthBias = db;
				Graphics.Interface.SetDepthBias(db.ConstantFactor, db.SlopeFactor, db.Clamp);
			}
		}

		public void SetDepthTestEnable(bool enable) {
			var gl33 = GL.GL33!;
			if (depthTestEnable ^ enable) {
				depthTestEnable = enable;
				if (depthTestEnable) gl33.Enable(GLCapability.DepthTest);
				else gl33.Disable(GLCapability.DepthTest);
			}
		}

		public void SetDepthWriteEnable(bool enable) {
			var gl33 = GL.GL33!;
			if (depthWriteEnable ^ enable) {
				depthWriteEnable = enable;
				gl33.DepthMask = depthWriteEnable;
			}
		}

		public void SetDepthCompareOp(GLCompareFunc op) {
			var gl33 = GL.GL33!;
			if (depthCompareOp != op) {
				depthCompareOp = op;
				gl33.DepthFunc = op;
			}
		}

		public void SetDepthBoundsTestEnable(bool enable) {
			if (depthBoundsTestEnable ^ enable) {
				depthBoundsTestEnable = enable;
				Graphics.Interface.SetDepthBoundsTestEnable(enable);
			}
		}

		public void SetStencilTestEnable(bool enable) {
			var gl33 = GL.GL33!;
			if (stencilTestEnable ^ enable) {
				stencilTestEnable = enable;
				if (stencilTestEnable) gl33.Enable(GLCapability.StencilTest);
				else gl33.Disable(GLCapability.StencilTest);
			}
		}

		public void SetStencilOp(GLFace face, GLPipeline.StencilOpState state) {
			var gl33 = GL.GL33!;
			switch(face) {
				case GLFace.Front:
					if (state != stencilOp.Item1) {
						gl33.StencilOpSeparate(GLFace.Front, state.FailOp, state.DepthFailOp, state.PassOp);
						stencilOp.Item1 = state;
					}
					break;
				case GLFace.Back:
					if (state != stencilOp.Item2) {
						gl33.StencilOpSeparate(GLFace.Back, state.FailOp, state.DepthFailOp, state.PassOp);
						stencilOp.Item2 = state;
					}
					break;
				case GLFace.FrontAndBack:
					gl33.StencilOp(state.FailOp, state.DepthFailOp, state.PassOp);
					stencilOp = (state, state);
					break;
			}
		}

		public void SetStencilCompareMask(GLFace face, uint mask) {
			var gl33 = GL.GL33!;
			bool front = false, back = false;
			switch (face) {
				case GLFace.Front:
					front = true;
					break;
				case GLFace.Back:
					back = true;
					break;
				case GLFace.FrontAndBack:
					front = back = true;
					break;
			}
			if (front && mask != stencilCompareMask.Item1) {
				gl33.StencilFuncSeparate(GLFace.Front, stencilOp.Item1.CompareOp, stencilReference.Item1, mask);
				stencilCompareMask.Item1 = mask;
			}
			if (back && mask != stencilCompareMask.Item2) {
				gl33.StencilFuncSeparate(GLFace.Front, stencilOp.Item2.CompareOp, stencilReference.Item2, mask);
				stencilCompareMask.Item2 = mask;
			}
		}

		public void SetStencilWriteMask(GLFace face, uint mask) {
			var gl33 = GL.GL33!;
			switch(face) {
				case GLFace.Front:
					if (stencilWriteMask.Item1 != mask) {
						gl33.StencilMaskSeparate(GLFace.Front, mask);
						stencilWriteMask.Item1 = mask;
					}
					break;
				case GLFace.Back:
					if (stencilWriteMask.Item2 != mask) {
						gl33.StencilMaskSeparate(GLFace.Back, mask);
						stencilWriteMask.Item2 = mask;
					}
					break;
				case GLFace.FrontAndBack:
					if (stencilWriteMask.Item1 != mask || stencilWriteMask.Item2 != mask) {
						gl33.StencilMask = mask;
						stencilWriteMask = (mask, mask);
					}
					break;
			}
		}

		public void SetStencilReference(GLFace face, int reference) {
			var gl33 = GL.GL33!;
			bool front = false, back = false;
			switch (face) {
				case GLFace.Front:
					front = true;
					break;
				case GLFace.Back:
					back = true;
					break;
				case GLFace.FrontAndBack:
					front = back = true;
					break;
			}
			if (front && reference != stencilReference.Item1) {
				gl33.StencilFuncSeparate(GLFace.Front, stencilOp.Item1.CompareOp, reference, stencilCompareMask.Item1);
				stencilReference.Item1 = reference;
			}
			if (back && reference != stencilReference.Item2) {
				gl33.StencilFuncSeparate(GLFace.Front, stencilOp.Item2.CompareOp, reference, stencilCompareMask.Item2);
				stencilReference.Item2 = reference;
			}
		}

		public void SetDepthBounds(float min, float max) {
			if (!ExMath.EqualsAbout(min, depthBounds.Item1, FloatStateEpsilon) || !ExMath.EqualsAbout(max, depthBounds.Item2, FloatStateEpsilon)) {
				depthBounds = (min, max);
				Graphics.Interface.SetDepthBounds(min, max);
			}
		}

		public void SetLogicOp(GLLogicOp op) {
			var gl33 = GL.GL33!;
			if (logicOp != op) {
				logicOp = op;
				gl33.LogicOp = op;
			}
		}

		public void SetBlendConstants(Vector4 constants) {
			var gl33 = GL.GL33!;
			if (blendConstants != constants) {
				blendConstants = constants;
				gl33.BlendColor = constants;
			}
		}

		public void SetColorWriteEnable(in ReadOnlySpan<bool> writeEnable) {
			var gl33 = GL.GL33!;
			for(int i = 0; i < writeEnable.Length; i++) {
				bool en = writeEnable[i];
				if (en ^ colorWriteEnable[i]) {
					colorWriteEnable[i] = en;
					if (!en) gl33.ColorMask((uint)i, false, false, false, false);
					else {
						var mask = colorWriteMask[i];
						gl33.ColorMask((uint)i, (mask & ColorComponent.Red) != 0, (mask & ColorComponent.Green) != 0, (mask & ColorComponent.Blue) != 0, (mask & ColorComponent.Alpha) != 0);
					}
				}
			}
		}

	}

}

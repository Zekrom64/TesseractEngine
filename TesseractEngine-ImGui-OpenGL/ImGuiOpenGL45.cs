using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Numerics;
using Tesseract.ImGui;
using Tesseract.OpenGL;
using Tesseract.OpenGL.Native;
using Tesseract.Core.Native;
using System.Diagnostics;

namespace Tesseract.ImGui.OpenGL {
	
	public static class ImGuiOpenGL45 {

		public static bool PreserveState { get; set; } = true;

		private static GL45 gl = null!;
		private static ARBSync? arbSync = null;

		private const int LocationTexture = 0;
		private const uint LocationUBO = 0;

		private const int InitBufSize = 0x4000;

		private static uint vertexBuffer = 0;
		private static IntPtr vertexBufferPtr;
		private static int vertexBufferSize = 0;

		private static uint indexBuffer = 0;
		private static IntPtr indexBufferPtr;
		private static int indexBufferSize = 0;

		private static uint vertexArray = 0;

		private static uint uniformBuffer;

		private static uint vertexShader, fragmentShader;
		private static uint shaderHandle;

		private static uint textureHandle;

		private static nuint fence = 0;

		public static void Init(GL gl) {
			IImGuiIO io = GImGui.IO;
			io.BackendRendererName = "Tesseract OpenGL 4.5 (Managed)";
			io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

			if (gl.GL45 == null) throw new ArgumentException("OpenGL context does not support GL 4.5", nameof(gl));
			ImGuiOpenGL45.gl = gl.GL45;
			arbSync = gl.ARBSync;
		}

		public static void Shutdown() {
			IImGuiIO io = GImGui.IO;
			io.BackendRendererName = null;

			CheckFence();
			if (vertexArray != 0) {
				gl.DeleteVertexArrays(vertexArray);
				gl.DeleteBuffers(vertexBuffer, indexBuffer, uniformBuffer);
				gl.DeleteProgram(shaderHandle);
				gl.DeleteShader(vertexShader);
				gl.DeleteShader(fragmentShader);
				gl.DeleteTextures(textureHandle);
				vertexArray = 0;
				vertexBuffer = 0;
				vertexBufferSize = 0;
				indexBuffer = 0;
				indexBufferSize = 0;
			}
		}

		private static void CheckFence() {
			if (fence != 0) {
				Debug.Assert(arbSync != null);
				arbSync.ClientWaitSync(fence, GLSyncFlags.FlushCommands, int.MaxValue);
				arbSync.DeleteSync(fence);
				fence = 0;
			} else gl.Finish();
		}

		private static void CheckVertexBuffer(int nVertices) {
			// Round up to multiples of 4096
			if ((nVertices & 0xFFF) != 0) nVertices = (nVertices & ~0xFFF) + 0x1000;
			// If requested size is larger than available
			if (nVertices > vertexBufferSize) {
				if (vertexBuffer != 0) {
					// Make sure buffer is not in use
					CheckFence();
					// Destroy the old buffer
					gl.UnmapNamedBuffer(vertexBuffer);
					gl.DeleteBuffers(vertexBuffer);
				}
				// Create new buffer and assign storage
				vertexBuffer = gl.CreateBuffers();
				nint size = nVertices * ImDrawVert.SizeOf;
				gl.NamedBufferStorage(vertexBuffer, size, GLBufferStorageFlags.MapWrite | GLBufferStorageFlags.MapPersistent);
				vertexBufferSize = nVertices;
				// Map persistent pointer to the buffer
				vertexBufferPtr = gl.MapNamedBufferRange(vertexBuffer, 0, size, GLMapAccessFlags.Write | GLMapAccessFlags.Persistent | GLMapAccessFlags.FlushExplicit);
				// Update the vertex array to use the buffer
				gl.VertexArrayVertexBuffer(vertexArray, 0, vertexBuffer, 0, Marshal.SizeOf<ImDrawVert>());
			}
		}

		private static void CheckIndexBuffer(int nIndices) {
			if ((nIndices & 0xFFF) != 0) nIndices = (nIndices & ~0xFFF) + 0x1000;
			if (nIndices > indexBufferSize) {
				if (indexBuffer != 0) {
					CheckFence();
					gl.UnmapNamedBuffer(indexBuffer);
					gl.DeleteBuffers(indexBuffer);
				}
				indexBuffer = gl.CreateBuffers();
				nint size = nIndices * sizeof(ushort);
				gl.NamedBufferStorage(indexBuffer, size, GLBufferStorageFlags.MapWrite | GLBufferStorageFlags.MapPersistent);
				indexBufferSize = nIndices;
				indexBufferPtr = gl.MapNamedBufferRange(indexBuffer, 0, size, GLMapAccessFlags.Write | GLMapAccessFlags.Persistent | GLMapAccessFlags.FlushExplicit);
				gl.VertexArrayElementBuffer(vertexArray, indexBuffer);
			}
		}

		private static void CreateFontTexture() {
			IImGuiIO io = GImGui.IO;
			ReadOnlySpan<byte> pixels = io.Fonts.GetTexDataAsRGBA32(out int w, out int h, out int _);
			textureHandle = gl.CreateTextures(GLTextureTarget.Texture2D);
			gl.TextureStorage2D(textureHandle, 1, GLInternalFormat.RGBA8, w, h);
			gl.TextureSubImage2D(textureHandle, 0, 0, 0, w, h, GLFormat.RGBA, GLTextureType.UnsignedByte, pixels);
			gl.TextureParameter(textureHandle, GLTexParamter.MinifyFilter, GLEnums.GL_LINEAR);
			gl.TextureParameter(textureHandle, GLTexParamter.MagnifyFilter, GLEnums.GL_LINEAR);
			io.Fonts.SetTexID(textureHandle);
		}

		public static void NewFrame() {
			if (vertexArray == 0) {
				// Create vertex array
				vertexArray = gl.CreateVertexArrays();
				// Setup vertex attributes
				// 0: ImDrawVert.Pos -> vec2
				gl.EnableVertexArrayAttrib(vertexArray, 0);
				gl.VertexArrayAttribFormat(vertexArray, 0, 2, GLType.Float, false, (uint)Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.Pos)));
				gl.VertexArrayAttribBinding(vertexArray, 0, 0);
				// 1: ImDrawPos.Col -> vec4
				gl.EnableVertexArrayAttrib(vertexArray, 1);
				gl.VertexArrayAttribFormat(vertexArray, 1, 4, GLType.UnsignedByte, true, (uint)Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.Col)));
				gl.VertexArrayAttribBinding(vertexArray, 1, 0);
				// 2: ImDrawPos.UV -> vec2
				gl.EnableVertexArrayAttrib(vertexArray, 2);
				gl.VertexArrayAttribFormat(vertexArray, 2, 2, GLType.Float, false, (uint)Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.UV)));
				gl.VertexArrayAttribBinding(vertexArray, 2, 0);

				// Initialize vertex and index buffers
				CheckVertexBuffer(InitBufSize);
				CheckIndexBuffer(InitBufSize);

				// Initialize uniform buffer
				uniformBuffer = gl.CreateBuffers();
				gl.NamedBufferStorage(uniformBuffer, Marshal.SizeOf<Matrix4x4>(), GLBufferStorageFlags.DynamicStorage);

				// Create shader program
				vertexShader = gl.CreateShader(GLShaderType.Vertex);
				gl.ShaderSource(vertexShader,
@"
#version 450
layout(location = 0)
in vec2 inPosition;
layout(location = 1)
in vec4 inColor;
layout(location = 2)
in vec2 inUV;

out vec4 fragColor;
out vec2 fragUV;

layout(binding = 0)
uniform UGlobals {
	mat4 mProj;
} uGlobals;

void main() {
	fragUV = inUV;
	fragColor = inColor;
	gl_Position = vec4(inPosition, 0, 1) * uGlobals.mProj;
}
"
				);
				gl.CompileShader(vertexShader);
				if (gl.GetShader(vertexShader, GLGetShader.CompileStatus) == 0)
					throw new InvalidDataException("Failed to compile shader:\n" + gl.GetShaderInfoLog(vertexShader));

				fragmentShader = gl.CreateShader(GLShaderType.Fragment);
				gl.ShaderSource(fragmentShader,
@"
#version 450

in vec4 fragColor;
in vec2 fragUV;

layout(location = 0)
out vec4 outColor;

layout(binding = 0)
uniform sampler2D uTexture;

void main() {
	outColor = texture(uTexture, fragUV) * fragColor;
}
"
				);
				gl.CompileShader(fragmentShader);
				if (gl.GetShader(fragmentShader, GLGetShader.CompileStatus) == 0)
					throw new InvalidDataException("Failed to compile shader:\n" + gl.GetShaderInfoLog(fragmentShader));

				shaderHandle = gl.CreateProgram();
				gl.AttachShader(shaderHandle, vertexShader);
				gl.AttachShader(shaderHandle, fragmentShader);
				gl.LinkProgram(shaderHandle);
				if (gl.GetProgram(shaderHandle, GLGetProgram.LinkStatus) == 0)
					throw new InvalidDataException("Failed to link shader:\n" + gl.GetProgramInfoLog(shaderHandle));

				CreateFontTexture();
			}
		}

		private static void SetupRenderState(IImDrawData drawData, Vector2i fbSize) {
			// Setup pipeline
			gl.Enable(GLCapability.Blend);
			gl.BlendEquation(GLBlendFunction.Add);
			gl.BlendFuncSeparate(GLBlendFactor.SrcAlpha, GLBlendFactor.OneMinusSrcAlpha, GLBlendFactor.One, GLBlendFactor.OneMinusSrcAlpha);
			gl.Disable(GLCapability.CullFace);
			gl.Disable(GLCapability.DepthTest);
			gl.Disable(GLCapability.StencilTest);
			gl.Enable(GLCapability.ScissorTest);
			gl.Disable(GLCapability.PrimitiveRestart);
			gl.PolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

			// Setup viewport
			gl.Viewport = new Recti(fbSize.X, fbSize.Y);
			float L = drawData.DisplayPos.X;
			float R = drawData.DisplayPos.X + drawData.DisplaySize.X;
			float T = drawData.DisplayPos.Y;
			float B = drawData.DisplayPos.Y + drawData.DisplaySize.Y;
			bool clipOriginLowerLeft = gl.GetInteger(GLEnums.GL_CLIP_ORIGIN) != GLEnums.GL_UPPER_LEFT;
			if (!clipOriginLowerLeft) (T, B) = (B, T);

			// Setup orthographic projection
			Matrix4x4 orthoProjection = new(
				2.0f/(R-L),  0,            0, 0,
				0,           2.0f/(T-B),   0, 0,
				0,           0,           -1, 0,
				(R+L)/(L-R), (T+B)/(B-T),  0, 1
			);

			// Use shader program
			gl.UseProgram(shaderHandle);
			// Set texture uniform to use unit 0
			gl.Uniform(LocationTexture, 0);
			// Update projection matrix uniform and bind
			gl.NamedBufferSubData(uniformBuffer, 0, Matrix4x4.Transpose(orthoProjection));
			gl.BindBufferBase(GLBufferRangeTarget.Uniform, LocationUBO, uniformBuffer);

			// Use default sampler for texture
			gl.BindSampler(0, 0);

			// Use vertex array
			gl.BindVertexArray(vertexArray);
		}

		public static void RenderDrawData(IImDrawData drawData) {
			Vector2i fbSize = (Vector2i)(drawData.DisplaySize * drawData.FramebufferScale);
			if (fbSize.X == 0 || fbSize.Y == 0) return;

			// Save existing state
			uint lastProgram = 0, lastTexture = 0, lastSampler = 0, lastVAO = 0;
			Span<int> lastPolygonMode = stackalloc int[2], lastViewport = stackalloc int[4], lastScissorBox = stackalloc int[4];
			GLBlendFactor lastBlendSrcRGB = default, lastBlendDstRGB = default, lastBlendSrcAlpha = default, lastBlendDstAlpha = default;
			GLBlendFunction lastBlendEquationRGB = default, lastBlendEquationAlpha = default;
			bool lastEnableBlend = default, lastEnableCullFace = default, lastEnableDepthTest = default, lastEnableStencilTest = default, lastEnableScissorTest = default, lastEnablePrimitiveRestart = default;

			if (PreserveState) {
				lastProgram = (uint)gl.GetInteger(GLEnums.GL_CURRENT_PROGRAM);
				lastTexture = (uint)gl.GetInteger(GLEnums.GL_TEXTURE_BINDING_2D, 0);
				lastSampler = (uint)gl.GetInteger(GLEnums.GL_SAMPLER_BINDING, 0);
				lastVAO = (uint)gl.GetInteger(GLEnums.GL_VERTEX_ARRAY_BINDING);
				gl.GetInteger(GLEnums.GL_POLYGON_MODE, lastPolygonMode);
				gl.GetInteger(GLEnums.GL_VIEWPORT, lastViewport);
				gl.GetInteger(GLEnums.GL_SCISSOR_BOX, lastScissorBox);
				lastBlendSrcRGB = (GLBlendFactor)gl.GetInteger(GLEnums.GL_BLEND_SRC_RGB);
				lastBlendDstRGB = (GLBlendFactor)gl.GetInteger(GLEnums.GL_BLEND_DST_RGB);
				lastBlendSrcAlpha = (GLBlendFactor)gl.GetInteger(GLEnums.GL_BLEND_SRC_ALPHA);
				lastBlendDstAlpha = (GLBlendFactor)gl.GetInteger(GLEnums.GL_BLEND_DST_ALPHA);
				lastBlendEquationRGB = (GLBlendFunction)gl.GetInteger(GLEnums.GL_BLEND_EQUATION_RGB);
				lastBlendEquationAlpha = (GLBlendFunction)gl.GetInteger(GLEnums.GL_BLEND_EQUATION_ALPHA);
				lastEnableBlend = gl.IsEnabled(GLCapability.Blend);
				lastEnableCullFace = gl.IsEnabled(GLCapability.CullFace);
				lastEnableDepthTest = gl.IsEnabled(GLCapability.DepthTest);
				lastEnableStencilTest = gl.IsEnabled(GLCapability.StencilTest);
				lastEnableScissorTest = gl.IsEnabled(GLCapability.ScissorTest);
				lastEnablePrimitiveRestart = gl.IsEnabled(GLCapability.PrimitiveRestart);
			}

			// Wait for previously dispatched draws to complete
			CheckFence();
			// Ensure vertex and index buffers have enough space
			CheckVertexBuffer(drawData.TotalVtxCount);
			CheckIndexBuffer(drawData.TotalIdxCount);

			Vector2 clipOff = drawData.DisplayPos;
			Vector2 clipScale = drawData.FramebufferScale;

			int vertexBufferOffset = 0;
			int indexBufferOffset = 0;
			UnmanagedPointer<ImDrawVert> pVertices = new(vertexBufferPtr);
			UnmanagedPointer<ushort> pIndices = new(indexBufferPtr);

			SetupRenderState(drawData, fbSize);

			foreach (IImDrawList drawList in drawData.CmdLists) {
				// Upload vertices and indices
				ReadOnlySpan<ImDrawVert> vertices = drawList.VtxBuffer.AsSpan();
				ReadOnlySpan<ushort> indices = drawList.IdxBuffer.AsSpan();
				MemoryUtil.Copy(pVertices, vertices, (ulong)(vertices.Length * ImDrawVert.SizeOf));
				MemoryUtil.Copy(pIndices, indices, (ulong)(indices.Length * sizeof(ushort)));
				gl.FlushMappedNamedBufferRange(vertexBuffer, vertexBufferOffset * ImDrawVert.SizeOf, vertices.Length * ImDrawVert.SizeOf);
				gl.FlushMappedNamedBufferRange(indexBuffer, indexBufferOffset * sizeof(ushort), indices.Length * sizeof(ushort));

				// For each drawing command
				foreach (ImDrawCmd drawCmd in drawList.CmdBuffer) {
					// Invoke user callback if needed
					if (drawCmd.UserCallback != null) {
						if (drawCmd.UserCallback == GImGui.ResetRenderState) SetupRenderState(drawData, fbSize);
						else drawCmd.UserCallback(drawList, drawCmd);
					} else {
						// Compute clip region
						Vector2 clipMin = (new Vector2(drawCmd.ClipRect.X, drawCmd.ClipRect.Y) - clipOff) * clipScale;
						Vector2 clipMax = (new Vector2(drawCmd.ClipRect.Z, drawCmd.ClipRect.W) - clipOff) * clipScale;
						if (clipMax.X <= clipMin.X || clipMax.Y <= clipMin.Y)
							continue;

						// Set clipping scissor
						gl.Scissor((int)clipMin.X, (int)(fbSize.Y - clipMax.Y), (int)(clipMax.X - clipMin.X), (int)(clipMax.Y - clipMin.Y));

						// Bind texture and draw elements
						gl.BindTextureUnit(0, (uint)drawCmd.TextureID);
						gl.DrawElementsBaseVertex(GLDrawMode.Triangles, (int)drawCmd.ElemCount, GLIndexType.UnsignedShort, ((int)drawCmd.IdxOffset + indexBufferOffset) * sizeof(ushort), (int)drawCmd.VtxOffset + vertexBufferOffset);
					}
				}

				// Increment offsets
				vertexBufferOffset += vertices.Length;
				pVertices += vertices.Length;
				indexBufferOffset += indices.Length;
				pIndices += indices.Length;
			}

			// Generate fence if possible
			if (arbSync != null) {
				gl.MemoryBarrier(GLMemoryBarrier.ClientMappedBuffer);
				fence = arbSync.FenceSync(GLSyncCondition.GPUCommandsComplete);
			}

			// Restore state
			if (PreserveState) {
				gl.UseProgram(lastProgram);
				gl.BindTextureUnit(0, lastTexture);
				gl.BindSampler(0, lastSampler);
				gl.BindVertexArray(lastVAO);
				gl.BlendEquationSeparate(lastBlendEquationRGB, lastBlendEquationAlpha);
				gl.BlendFuncSeparate(lastBlendSrcRGB, lastBlendDstRGB, lastBlendSrcAlpha, lastBlendDstAlpha);
				// FOR SOME REASON, OpenGL returns these values in reverse order the function specifys them...
				// It also likes to return invalid values if not set previously so double-check the implementation isn't stupid
				if (lastPolygonMode[1] != 0) gl.PolygonMode((GLFace)lastPolygonMode[1], (GLPolygonMode)lastPolygonMode[0]);
				gl.Viewport = new(lastViewport[0], lastViewport[1], lastViewport[2], lastViewport[3]);
				gl.Scissor(lastScissorBox[0], lastScissorBox[1], lastScissorBox[2], lastScissorBox[3]);
				if (lastEnableBlend) gl.Enable(GLCapability.Blend);
				else gl.Disable(GLCapability.Blend);
				if (lastEnableCullFace) gl.Enable(GLCapability.CullFace);
				else gl.Disable(GLCapability.CullFace);
				if (lastEnableDepthTest) gl.Enable(GLCapability.DepthTest);
				else gl.Disable(GLCapability.DepthTest);
				if (lastEnableStencilTest) gl.Enable(GLCapability.StencilTest);
				else gl.Disable(GLCapability.StencilTest);
				if (lastEnableScissorTest) gl.Enable(GLCapability.ScissorTest);
				else gl.Disable(GLCapability.ScissorTest);
				if (lastEnablePrimitiveRestart) gl.Enable(GLCapability.PrimitiveRestart);
				else gl.Disable(GLCapability.PrimitiveRestart);
			}
		}

	}

}

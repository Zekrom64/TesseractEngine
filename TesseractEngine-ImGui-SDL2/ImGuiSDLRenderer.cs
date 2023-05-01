using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;
using Tesseract.ImGui;
using Tesseract.SDL;

namespace Tesseract.ImGui.SDL {

	public static class ImGuiSDLRenderer {

		private static SDLRenderer renderer = null!;
		private static SDLTexture? fontTexture = null!;

		public static bool Init(SDLRenderer renderer) {
			ImGuiSDLRenderer.renderer = renderer;

			IImGuiIO io = GImGui.IO;
			io.BackendRendererName = "imgui_impl_sdlrenderer (Managed)";
			io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

			return true;
		}

		public static void Shutdown() {
			IImGuiIO io = GImGui.IO;
			io.BackendRendererName = null;
			io.Fonts.SetTexID(0);
			fontTexture?.Dispose();
			fontTexture = null;
		}

		public static void NewFrame() {
			if (fontTexture == null) {
				IImGuiIO io = GImGui.IO;
				ReadOnlySpan<byte> pixels = io.Fonts.GetTexDataAsRGBA32(out int width, out int height, out int _);
				fontTexture = renderer.CreateTexture(SDLPixelFormatEnum.ABGR8888, SDLTextureAccess.Static, width, height);
				fontTexture.UpdateTexture(null, pixels, 4 * width);
				fontTexture.BlendMode = SDLBlendMode.Blend;
				io.Fonts.SetTexID((nuint)fontTexture.Texture);
			}
		}

		private static void SetupRenderState() {
			renderer.ResetViewport();
			renderer.ResetClipRect();
		}

		private static readonly int PosOffset = (int)Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.Pos));
		private static readonly int UVOffset = (int)Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.UV));
		private static readonly int ColOffset = (int)Marshal.OffsetOf<ImDrawVert>(nameof(ImDrawVert.Col));	
		private static readonly int Stride = Marshal.SizeOf<ImDrawVert>();

		public static void RenderDrawData(IImDrawData drawData) {
			Debug.Assert(renderer != null);
			Debug.Assert(fontTexture != null);

			Vector2 renderScale = renderer.Scale;
			if (renderScale.X == 1) renderScale.X = drawData.FramebufferScale.X;
			if (renderScale.Y == 1) renderScale.Y = drawData.FramebufferScale.Y;

			Vector2i fbSize = (Vector2i)(drawData.DisplaySize * renderScale);
			if (fbSize.X == 0 || fbSize.Y == 0) return;

			bool oldClipEnabled = renderer.IsClipEnabled;
			Recti oldViewport = renderer.Viewport;
			Recti oldClipRect = renderer.ClipRect;
			SDLBlendMode oldBlendMode = renderer.BlendMode;

			Vector2 clipOffset = drawData.DisplayPos;
			Vector2 clipScale = renderScale;

			renderer.BlendMode = SDLBlendMode.Blend;

			foreach (IImDrawList drawList in drawData.CmdLists) {
				Span<ImDrawVert> vtxBuffer = drawList.VtxBuffer.AsSpan();
				Span<ushort> idxBuffer = drawList.IdxBuffer.AsSpan();	
				// We resort to unsafe code because this requires pointer shenanigans to setup the data arrays
				unsafe {
					fixed (ImDrawVert* pVtx = vtxBuffer) {
						fixed (ushort* pIdx = idxBuffer) {
							foreach (ImDrawCmd cmd in drawList.CmdBuffer) {
								if (cmd.UserCallback != null) {
									if (cmd.UserCallback == GImGui.ResetRenderState) SetupRenderState();
									else cmd.UserCallback(drawList, cmd);
								} else {
									Vector2 clipMin = (new Vector2(cmd.ClipRect.X, cmd.ClipRect.Y) - clipOffset) * clipScale;
									Vector2 clipMax = (new Vector2(cmd.ClipRect.Z, cmd.ClipRect.W) - clipOffset) * clipScale;
									if (clipMin.X < 0) clipMin.X = 0;
									if (clipMin.Y < 0) clipMin.Y = 0;
									if (clipMax.X > fbSize.X) clipMax.X = fbSize.X;
									if (clipMax.Y > fbSize.Y) clipMax.Y = fbSize.Y;
									if (clipMax.X <= clipMin.X || clipMax.Y <= clipMin.Y) continue;

									Recti r = new() {
										Position = (Vector2i)clipMin,
										Size = (Vector2i)(clipMax - clipMin)
									};
									renderer.ClipRect = r;

									var pCmdVtx = pVtx + cmd.VtxOffset;
									var pCmdIdx = pIdx + cmd.IdxOffset;

									SDL2.CheckError(SDL2.Functions.SDL_RenderGeometryRaw(
										renderer.Renderer,
										(IntPtr)    cmd.TextureID,

										(float*)    ((nint)pCmdVtx + PosOffset), Stride,	
										(Vector4b*) ((nint)pCmdVtx + ColOffset), Stride,
										(float*)    ((nint)pCmdVtx + UVOffset),  Stride,
										(int)(drawList.VtxBuffer.Count - cmd.VtxOffset),
										
										(IntPtr)    pCmdIdx,
										(int)cmd.ElemCount,
										sizeof(ushort)
									));
								}
							}
						}
					}
				}
			}

			renderer.Viewport = oldViewport;
			if (oldClipEnabled) renderer.ClipRect = oldClipRect;
			else renderer.ResetClipRect();
			renderer.BlendMode = oldBlendMode;
		}

	}

}

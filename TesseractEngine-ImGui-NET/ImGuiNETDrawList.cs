using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.ImGui.Utilities.CLI;

namespace Tesseract.ImGui.NET {

	public class ImGuiNETDrawList : IImDrawList {

		internal readonly ImDrawListPtr drawList;

		private static readonly List<ImDrawCallback> callbacks = new();

		private static readonly Dictionary<IntPtr, ImGuiNETDrawList> drawLists = new();

		private static ImDrawCmd Decode(ImGuiNET.ImDrawCmd cmd) {
			unsafe {
				int icbk = (int)cmd.UserCallbackData;
				ImDrawCallback? cbk = null;
				if (icbk == -1) {
					// Sentinel value for ResetRenderState
					cbk = GImGui.ResetRenderState;
				} else if (icbk > 0) {
					// If positive, must be an index into the custom callbacks
					cbk = callbacks[(int)cmd.UserCallbackData - 1];
				}
				return new ImDrawCmd() {
					ClipRect = cmd.ClipRect,
					ElemCount = cmd.ElemCount,
					VtxOffset = cmd.VtxOffset,
					IdxOffset = cmd.IdxOffset,
					TextureID = (nuint)cmd.TextureId,
					UserCallback = cbk
				};
			}
		}

		private static ImGuiNET.ImDrawCmd Encode(ImDrawCmd cmd) {
			unsafe {
				lock (callbacks) {
					// Default callback is null
					nint cbk = 0;
					if (cmd.UserCallback != null) {
						if (cmd.UserCallback == GImGui.ResetRenderState) {
							// If special ResetRenderState value, use -1
							cbk = -1;
						} else {
							// Else register the custom callback and add the callback trampoline
							callbacks.Add(cmd.UserCallback);
							delegate* unmanaged<IntPtr, ImGuiNET.ImDrawCmd*, void> fptr = &CustomDrawCallback;
							cbk = (nint)fptr;
						}
					}
					return new ImGuiNET.ImDrawCmd() {
						ClipRect = cmd.ClipRect,
						ElemCount = cmd.ElemCount,
						VtxOffset = cmd.VtxOffset,
						IdxOffset = cmd.IdxOffset,
						TextureId = (nint)cmd.TextureID,
						UserCallback = cbk,
						UserCallbackData = (void*)callbacks.Count
					};
				}
			}
		}

		private ImGuiNETDrawList(ImDrawListPtr ptr) {
			drawList = ptr;
			unsafe {
				var pList = ptr.NativePtr;
				lock (drawLists) {
					drawLists[(IntPtr)pList] = this;
				}
				CmdBuffer = new ImGuiNETList<ImDrawCmd, ImGuiNET.ImDrawCmd>(&pList->CmdBuffer, Encode, Decode);
				IdxBuffer = new ImGuiNETVector<ushort>(&pList->IdxBuffer);
				VtxBuffer = new ImGuiNETVector<ImDrawVert>(&pList->VtxBuffer);
			}
		}

		internal static ImGuiNETDrawList Get(ImDrawListPtr ptr) {
			unsafe {
				IntPtr key = (IntPtr)ptr.NativePtr;
				lock (drawLists) {
					if (drawLists.TryGetValue(key, out ImGuiNETDrawList? list)) return list;
					list = new ImGuiNETDrawList(ptr);
					drawLists[key] = list;
					return list;
				}
			}
		}

		internal static void NewFrame() {
			callbacks.Clear();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				lock (drawLists) {
					drawLists.Remove((IntPtr)drawList.NativePtr);
				}
				ImGuiNative.ImDrawList_destroy(drawList.NativePtr);
			}
		}


		public IList<ImDrawCmd> CmdBuffer { get; }

		public IImVector<ushort> IdxBuffer { get; }

		public IImVector<ImDrawVert> VtxBuffer { get; }

		public ImDrawListFlags Flags {
			get => (ImDrawListFlags)drawList.Flags;
			set => drawList.Flags = (ImGuiNET.ImDrawListFlags)value;
		}

		public Vector2 ClipRectMin => drawList.GetClipRectMin();

		public Vector2 ClipRectMax => drawList.GetClipRectMax();

		public void AddBezierCubic(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness, int numSegments = 0) => drawList.AddBezierCubic(p1, p2, p3, p4, col, thickness, numSegments);

		public void AddBezierQuadratic(Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness, int numSegments = 0) => drawList.AddBezierQuadratic(p1, p2, p3, col, thickness, numSegments);

		[UnmanagedCallersOnly]
		private static unsafe void CustomDrawCallback(IntPtr parentList, ImGuiNET.ImDrawCmd* cmd) {
			var cmd2 = Decode(*cmd);
			cmd2.UserCallback?.Invoke(drawLists[parentList], cmd2);
		}

		public void AddCallback(ImDrawCallback callback) {
			lock (callbacks) {
				unsafe {
					delegate* unmanaged<IntPtr, ImGuiNET.ImDrawCmd*, void> cbk = &CustomDrawCallback;
					drawList.AddCallback((nint)cbk, callbacks.Count);
				}
				callbacks.Add(callback);
			}
		}

		public void AddCircle(Vector2 center, float radius, uint col, int numSegments = 0, float thickness = 1) => drawList.AddCircle(center, radius, col, numSegments);

		public void AddCircleFilled(Vector2 center, float radius, uint col, int numSegments = 0) => drawList.AddCircleFilled(center, radius, col, numSegments);

		public void AddConvexPolyFilled(ReadOnlySpan<Vector2> points, uint col) => drawList.AddConvexPolyFilled(ref MemoryMarshal.GetReference(points), points.Length, col);

		public void AddDrawCmd() => drawList.AddDrawCmd();

		public void AddImage(nuint userTextureID, Vector2 pMin, Vector2 pMax, Vector2 uvMin, Vector2 uvMax, uint col = uint.MaxValue) => drawList.AddImage((nint)userTextureID, pMin, pMax, uvMin, uvMax, col);

		public void AddImageQuad(nuint userTextureID, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, uint col = uint.MaxValue) => drawList.AddImageQuad((nint)userTextureID, p1, p2, p3, p4, uv1, uv2, uv3, uv4, col);

		public void AddImageRounded(nuint userTextureID, Vector2 pMin, Vector2 pMax, Vector2 uvMin, Vector2 uvMax, uint col, float rounding, ImDrawFlags flags = ImDrawFlags.None) => drawList.AddImageRounded((nint)userTextureID, pMin, pMax, uvMin, uvMax, col, rounding, (ImGuiNET.ImDrawFlags)flags);

		public void AddLine(Vector2 p1, Vector2 p2, uint col, float thickness = 1) => drawList.AddLine(p1, p2, col, thickness);

		public void AddNgon(Vector2 center, float radius, uint col, int numSegments, float thickness = 1) => drawList.AddNgon(center, radius, col, numSegments, thickness);

		public void AddNgonFilled(Vector2 center, float radius, uint col, int numSegments) => drawList.AddNgonFilled(center, radius, col, numSegments);

		public void AddPolyline(ReadOnlySpan<Vector2> points, uint col, ImDrawFlags flags, float thickness) => drawList.AddPolyline(ref MemoryMarshal.GetReference(points), points.Length, col, (ImGuiNET.ImDrawFlags)flags, thickness);

		public void AddQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness = 1) => drawList.AddQuad(p1, p2, p3, p4, col, thickness);

		public void AddQuadFilled(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col) => drawList.AddQuadFilled(p1, p2, p3, p4, col);

		public void AddRect(Vector2 pMin, Vector2 pMax, uint col, float rounding = 0, ImDrawFlags flags = ImDrawFlags.None, float thickness = 1) => drawList.AddRect(pMin, pMax, col, rounding, (ImGuiNET.ImDrawFlags)flags, thickness);

		public void AddRectFilled(Vector2 pMin, Vector2 pMax, uint col, float rounding = 0, ImDrawFlags flags = ImDrawFlags.None) => drawList.AddRectFilled(pMin, pMax, col, rounding);

		public void AddRectFilledMultiColor(Vector2 pMin, Vector2 pMax, uint colUprLeft, uint colUprRight, uint colBotRight, uint colBotLeft) => drawList.AddRectFilledMultiColor(pMin, pMax, colUprLeft, colUprRight, colBotRight, colBotLeft);

		public void AddText(Vector2 pos, uint col, string text) => drawList.AddText(pos, col, text);

		public void AddText(IImFont font, float fontSize, Vector2 pos, uint col, string text, float wrapWidth, Vector4? cpuFineClipRect = null) {
			// TODO
			throw new NotImplementedException();
		}

		public void AddTriangle(Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness = 1) => drawList.AddTriangle(p1, p2, p3, col, thickness);

		public void AddTriangleFilled(Vector2 p1, Vector2 p2, Vector2 p3, uint col) => drawList.AddTriangleFilled(p1, p2, p3, col);

		public void ChannelsMerge() => drawList.ChannelsMerge();

		public void ChannelsSetCurrent(int n) => drawList.ChannelsSetCurrent(n);

		public void ChannelsSplit(int count) => drawList.ChannelsSplit(count);

		public IImDrawList CloneOutput() => new ImGuiNETDrawList(drawList.CloneOutput());

		public void PathArcTo(Vector2 center, float radius, float aMin, float aMax, int numSegments = 0) => drawList.PathArcTo(center, radius, aMin, aMax, numSegments);

		public void PathArcToFast(Vector2 center, float radius, int aMinOf12, int aMaxOf12) => drawList.PathArcToFast(center, radius, aMinOf12, aMaxOf12);

		public void PathBezierCubicCurveTo(Vector2 p2, Vector2 p3, Vector2 p4, int numSegments = 0) => drawList.PathBezierCubicCurveTo(p2, p3, p4, numSegments);

		public void PathBezierQuadraticCurveTo(Vector2 p2, Vector2 p3, int numSegments = 0) => drawList.PathBezierQuadraticCurveTo(p2, p3, numSegments);

		public void PathClear() => drawList.PathClear();

		public void PathFillConvex(uint col) => drawList.PathFillConvex(col);

		public void PathLineTo(Vector2 pos) => drawList.PathLineTo(pos);

		public void PathLineToMergeDuplicate(Vector2 pos) => drawList.PathLineToMergeDuplicate(pos);

		public void PathRect(Vector2 rectMin, Vector2 rectMax, float rounding = 0, ImDrawFlags flags = ImDrawFlags.None) => drawList.PathRect(rectMin, rectMax);

		public void PathStroke(uint col, ImDrawFlags flags = ImDrawFlags.None, float thickness = 1) => drawList.PathStroke(col, (ImGuiNET.ImDrawFlags)flags, thickness);

		public void PopClipRect() => drawList.PopClipRect();

		public void PopTextureID() => drawList.PopTextureID();

		public void PrimQuadUV(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uvA, Vector2 uvB, Vector2 uvC, Vector2 uvD, uint col) => drawList.PrimQuadUV(a, b, c, d, uvA, uvB, uvC, uvD, col);

		public void PrimRect(Vector2 a, Vector2 b, uint col) => drawList.PrimRect(a, b, col);

		public void PrimRectUV(Vector2 a, Vector2 b, Vector2 uvA, Vector2 uvB, uint col) => drawList.PrimRectUV(a, b, uvA, uvB, col);

		public void PrimReserve(int idxCount, int vtxCount) => drawList.PrimReserve(idxCount, vtxCount);

		public void PrimUnreserve(int idxCount, int vtxCount) => drawList.PrimUnreserve(idxCount, vtxCount);

		public void PrimVtx(Vector2 pos, Vector2 uv, uint col) => drawList.PrimVtx(pos, uv, col);

		public void PrimWriteIdx(ushort idx) => drawList.PrimWriteIdx(idx);

		public void PrimWriteVtx(Vector2 pos, Vector2 uv, uint col) => drawList.PrimWriteVtx(pos, uv, col);

		public void PushClipRect(Vector2 clipRectMin, Vector2 clipRectMax, bool intersectWithCurrentClipRect = false) => drawList.PushClipRect(clipRectMin, clipRectMax, intersectWithCurrentClipRect);

		public void PushClipRectFullScreen() => drawList.PushClipRectFullScreen();

		public void PushTextureID(nuint textureID) => drawList.PushTextureID((nint)textureID);

	}

}

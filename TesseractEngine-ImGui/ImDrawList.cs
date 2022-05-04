using System.Numerics;

namespace Tesseract.ImGui {
	public class ImDrawList {

		public const int TexLinesMaxWidth = 63;

		public readonly List<ImDrawCmd> CmdBuffer = new();
		public readonly List<ushort> IdxBuffer = new();
		public readonly List<ImDrawVert> VtxBuffer = new();
		public ImDrawListFlags Flags;

		internal uint VtxCurrentIdx;
		internal ImDrawListSharedData? Data;
		internal string? OwnerName;
		internal int VtxWritePtr;
		internal int IdxWritePtr;
		internal readonly List<Vector4> ClipRectStack = new();
		internal readonly List<nuint> TextureIdStack = new();
		internal readonly List<Vector2> Path = new();
		internal ImDrawCmdHeader CmdHeader;
		internal ImDrawListSplitter? Splitter;
		internal float FringeScale;

		public ImDrawList(ImDrawListSharedData? sharedData) {
			Data = sharedData;
		}

		public void PushClipRect(Vector2 clipRectMin, Vector2 clipRectMax, bool intersectWithCurrentClipRect = false) {

		}

		public void PushClipRectFullscreen() {

		}
		
		public void PopClipRect() {

		}

		public void PushTextureID(nuint textureID) {

		}

		public void PopTextureID() {

		}

		public Vector2 ClipRectMin {
			get {
				Vector4 clip = ClipRectStack.Last();
				return new(clip.X, clip.Y);
			}
		}

		public Vector2 ClipRectMax {
			get {
				Vector4 clip = ClipRectStack.Last();
				return new(clip.Z, clip.W);
			}
		}

		public void AddLine(Vector2 p1, Vector2 p2, uint col, float thickness = 1.0f) {

		}

		public void AddRect(Vector2 pMin, Vector2 pMax, uint col, float rounding = 0, ImDrawFlags flags = 0, float thickness = 1) {

		}

		public void AddRectFilled(Vector2 pMin, Vector2 pMax, uint col, float rounding = 0, ImDrawFlags flags = 0) {

		}

		public void AddRectFilledMultiColor(Vector2 pMin, Vector2 pMax, uint colUpLeft, uint colUpRight, uint colBottomLeft, uint colBottomRight) {

		}

		public void AddQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness = 1.0f) {

		}

		public void AddQuadFilled(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col) {

		}

		public void AddTriangle(Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness = 1) {

		}

		public void AddTriangleFilled(Vector2 p1, Vector2 p2, Vector2 p3, uint col) {

		}

		public void AddCircle(Vector2 center, float radius, uint col, int numSegments = 0, float thickness = 1) {

		}

		public void AddCircleFilled(Vector2 center, float radius, uint col, int numSegments = 0) {

		}

		public void AddNgon(Vector2 center, float radius, uint col, int numSegments, float thickness = 1) {

		}

		public void AddNgonFilled(Vector2 center, float radius, uint col, int numSegments) {

		}

		public void AddText(Vector2 pos, uint col, string text) {

		}

		public void AddText(ImFont font, float fontSize, Vector2 pos, uint col, string text, float wrapWidth = 0, Vector4? cpuFineClipRect = null) {

		}

		public void AddPolyline(in ReadOnlySpan<Vector2> points, uint col, ImDrawFlags flags, float thickness) {

		}

		public void AddPolyline(IReadOnlyCollection<Vector2> points, uint col, ImDrawFlags flags, float thickness) {

		}

		public void AddConvexPolyFilled(in ReadOnlySpan<Vector2> points, uint col) {

		}

		public void AddConvexPolyFilled(IReadOnlyCollection<Vector2> points, uint col) {

		}

		public void AddBezierCubic(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness, int numSegments = 0) {

		}

		public void AddBezierQuadratic(Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness, int numSegments = 0) {

		}


		public void AddImage(nint userTextureID, Vector2 pMin, Vector2 pMax, Vector2 uvMin, Vector2 uvMax, uint col = 0xFFFFFFFF) {

		}

		public void AddImage(nint userTextureID, Vector2 pMin, Vector2 pMax, Vector2 uvMin = default) => AddImage(userTextureID, pMin, pMax, uvMin, new Vector2(1, 1));

		public void AddImageQuad(nint userTextureID, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, uint col = 0xFFFFFFFF) {

		}

		public void AddImageQuad(nint userTextureID, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3) =>
			AddImageQuad(userTextureID, p1, p2, p3, p4, uv1, uv2, uv3, new Vector2(0, 1));

		public void AddImageQuad(nint userTextureID, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2) =>
			AddImageQuad(userTextureID, p1, p2, p3, p4, uv1, uv2, new Vector2(1, 1));

		public void AddImageQuad(nint userTextureID, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1 = default) =>
			AddImageQuad(userTextureID, p1, p2, p3, p4, uv1, new Vector2(1, 0));

		public void AddImageRounded(nint userTextureID, Vector2 pMin, Vector2 pMax, Vector2 uvMin, Vector2 uvMax, uint col, float rounding, ImDrawFlags flags = 0) {

		}


		public void PathClear() => Path.Clear();

		public void PathLineTo(Vector2 pos) => Path.Add(pos);

		public void PathLineToMergeDuplicate(Vector2 pos) {
			if (Path.Count == 0 || Path.Last() != pos) Path.Add(pos);
		}

		public void PathFillConvex(uint col) {
			AddConvexPolyFilled(Path, col);
			Path.Clear();
		}

		public void PathStroke(uint col, ImDrawFlags flags = 0, float thickness = 1) {
			AddPolyline(Path, col, flags, thickness);
			Path.Clear();
		}

		public void PathArcTo(Vector2 center, float radius, float aMin, float aMax, int numSegments = 0) {

		}

		public void PathArcToFast(Vector2 center, float radius, float aMinOf12, float aMaxOf12) {

		}

		public void PathBezierCubicCurveTo(Vector2 p2, Vector2 p3, Vector2 p4, int numSegments = 0) {

		}

		public void PathBezierQuadraticCurveTo(Vector2 p2, Vector2 p3, int numSegments = 0) {

		}

		public void PathRect(Vector2 rectMin, Vector2 rectMax, float rounding = 0, ImDrawFlags flags = 0) {

		}


		public void AddCallback(ImDrawCallback callback) {

		}

		public void AddDrawCmd() {

		}

		public ImDrawList CloneOutput() {

		}


		public void PrimReserve(int idxCount, int vtxCount) {

		}

		public void PrimUnreserve(int idxCount, int vtxCount) {

		}

		public void PrimRect(Vector2 a, Vector2 b, uint col) {

		}

		public void PrimRectUV(Vector2 a, Vector2 b, Vector2 uvA, Vector2 uvB, uint col) {

		}

		public void PrimQuadUV(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uvA, Vector2 uvB, Vector2 uvC, Vector2 uvD, uint col) {

		}

		public void PrimWriteVtx(Vector2 pos, Vector2 uv, uint col) {
			VtxBuffer[VtxWritePtr++] = new() {
				Pos = pos,
				UV = uv,
				Col = new ImColor(col).Value
			};
		}

		public void PrimWriteIdx(ushort idx) {
			IdxBuffer[IdxWritePtr++] = idx;
		}

		public void PrimVtx(Vector2 pos, Vector2 uv, uint col) {
			PrimWriteIdx((ushort)VtxCurrentIdx);
			PrimWriteVtx(pos, uv, col);
		}

		internal void ResetForNewFrame() {

		}

		internal void ClearFreeMemory() {

		}

		internal void PopUnusedDrawCmd() {

		}

		internal void TryMergeDrawCmds() {

		}

		internal void OnChangedClipRect() {

		}

		internal void OnChangedTextureID() {

		}

		internal void OnChangedVtxOffset() {

		} 

		internal int CalcCircleAutoSegmentCount(float radius) {

		}

		internal void PathArcToFastEx(Vector2 center, float radius, int aMinSample, int aMaxSample, int aStep) {

		}

		internal void PathArcToN(Vector2 center, float radius, float aMin, float aMax, int numSegments) {

		}

	}

}

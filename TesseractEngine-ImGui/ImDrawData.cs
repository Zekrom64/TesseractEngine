using System.Numerics;

namespace Tesseract.ImGui {

	public interface IImDrawData {

		public bool Valid { get; }
		public int TotalIdxCount { get; }
		public int TotalVtxCount { get; }
		public IReadOnlyList<IImDrawList> CmdLists { get; }
		public Vector2 DisplayPos { get; }
		public Vector2 DisplaySize { get; }
		public Vector2 FramebufferScale { get; }

		public void Clear();

		public void DeIndexAllBuffers();

		public void ScaleClipRects(Vector2 fbScale);

	}

	public interface IImDrawList : IDisposable {
	
		public IList<ImDrawCmd> CmdBuffer { get; }

		public IImVector<ushort> IdxBuffer { get; }

		public IImVector<ImDrawVert> VtxBuffer { get; }

		public ImDrawListFlags Flags { get; set; }


		public void PushClipRect(Vector2 clipRectMin, Vector2 clipRectMax, bool intersectWithCurrentClipRect = false);

		public void PushClipRectFullScreen();

		public void PopClipRect();

		public void PushTextureID(nuint textureID);

		public void PopTextureID();

		public Vector2 ClipRectMin { get; }

		public Vector2 ClipRectMax { get; }


		public void AddLine(Vector2 p1, Vector2 p2, uint col, float thickness = 1);

		public void AddRect(Vector2 pMin, Vector2 pMax, uint col, float rounding = 0, ImDrawFlags flags = default, float thickness = 1);

		public void AddRectFilled(Vector2 pMin, Vector2 pMax, uint col, float rounding = 0, ImDrawFlags flags = 0);

		public void AddRectFilledMultiColor(Vector2 pMin, Vector2 pMax, uint colUprLeft, uint colUprRight, uint colBotRight, uint colBotLeft);

		public void AddQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness = 1);

		public void AddQuadFilled(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col);

		public void AddTriangle(Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness = 1);

		public void AddTriangleFilled(Vector2 p1, Vector2 p2, Vector2 p3, uint col);

		public void AddCircle(Vector2 center, float radius, uint col, int numSegments = 0, float thickness = 1);

		public void AddCircleFilled(Vector2 center, float radius, uint col, int numSegments = 0);

		public void AddNgon(Vector2 center, float radius, uint col, int numSegments, float thickness = 1);

		public void AddNgonFilled(Vector2 center, float radius, uint col, int numSegments);

		public void AddText(Vector2 pos, uint col, string text);

		public void AddText(IImFont font, float fontSize, Vector2 pos, uint col, string text, float wrapWidth, Vector4? cpuFineClipRect = null);

		public void AddPolyline(ReadOnlySpan<Vector2> points, uint col, ImDrawFlags flags, float thickness);

		public void AddConvexPolyFilled(ReadOnlySpan<Vector2> points, uint col);

		public void AddBezierCubic(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness, int numSegments = 0);

		public void AddBezierQuadratic(Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness, int numSegments = 0);


		public void AddImage(nuint userTextureID, Vector2 pMin, Vector2 pMax, Vector2 uvMin, Vector2 uvMax, uint col = 0xFFFFFFFF);

		public void AddImage(nuint userTextureID, Vector2 pMin, Vector2 pMax, Vector2 uvMin = default) => AddImage(userTextureID, pMin, pMax, uvMin, Vector2.One);

		public void AddImageQuad(nuint userTextureID, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, uint col = 0xFFFFFFFF);

		public void AddImageQuad(nuint userTextureID, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3) => AddImageQuad(userTextureID, p1, p2, p3, p4, uv1, uv2, uv3, new Vector2(0, 1));

		public void AddImageQuad(nuint userTextureID, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2) => AddImageQuad(userTextureID, p1, p2, p3, p4, uv1, uv2, Vector2.One);

		public void AddImageQuad(nuint userTextureID, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1 = default) => AddImageQuad(userTextureID, p1, p2, p3, p4, uv1, new Vector2(1, 0));

		public void AddImageRounded(nuint userTextureID, Vector2 pMin, Vector2 pMax, Vector2 uvMin, Vector2 uvMax, uint col, float rounding, ImDrawFlags flags = default);


		public void PathClear();

		public void PathLineTo(Vector2 pos);

		public void PathLineToMergeDuplicate(Vector2 pos);

		public void PathFillConvex(uint col);

		public void PathStroke(uint col, ImDrawFlags flags = default, float thickness = 1);

		public void PathArcTo(Vector2 center, float radius, float aMin, float aMax, int numSegments = 0);

		public void PathArcToFast(Vector2 center, float radius, int aMinOf12, int aMaxOf12);

		public void PathBezierCubicCurveTo(Vector2 p2, Vector2 p3, Vector2 p4, int numSegments = 0);

		public void PathBezierQuadraticCurveTo(Vector2 p2, Vector2 p3, int numSegments = 0);

		public void PathRect(Vector2 rectMin, Vector2 rectMax, float rounding = 0, ImDrawFlags flags = default);


		public void AddCallback(ImDrawCallback callback);

		public void AddDrawCmd();

		public IImDrawList CloneOutput();


		public void ChannelsSplit(int count);

		public void ChannelsMerge();

		public void ChannelsSetCurrent(int n);


		public void PrimReserve(int idxCount, int vtxCount);

		public void PrimUnreserve(int idxCount, int vtxCount);

		public void PrimRect(Vector2 a, Vector2 b, uint col);

		public void PrimRectUV(Vector2 a, Vector2 b, Vector2 uvA, Vector2 uvB, uint col);

		public void PrimQuadUV(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uvA, Vector2 uvB, Vector2 uvC, Vector2 uvD, uint col);

		public void PrimWriteVtx(Vector2 pos, Vector2 uv, uint col);

		public void PrimWriteIdx(ushort idx);

		public void PrimVtx(Vector2 pos, Vector2 uv, uint col);

	}

	public interface IImDrawListSharedData { }

}

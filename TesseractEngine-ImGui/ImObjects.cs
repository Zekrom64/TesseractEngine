using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Tesseract.Core;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using System.Runtime.InteropServices;

namespace Tesseract.ImGui {
	
	public class ImFontAtlas {

		public ImFontAtlasFlags Flags;
		public nint TexID { get; set; }
		public int TexDesiredWidth;
		public int TexGlyphPadding;
		public bool Locked;

		internal bool TexReady;
		internal bool TexPixelsUseColors;
		internal byte[]? TexPixelsAlpha8;
		internal uint[]? TexPixelsRGBA32;
		internal int TexWidth;
		internal int TexHeight;
		internal Vector2 TexUVScale;
		internal Vector2 TexUVWhitePixel;
		internal readonly List<ImFont> Fonts = new();
		internal readonly List<ImFontAtlasCustomRect> CustomRects = new();
		internal readonly List<ImFontConfig> ConfigData = new();
		internal readonly Vector4[] TexUVLines = new Vector4[ImDrawList.TexLinesMaxWidth + 1];
		
		internal ImFontBuilderIO FontBuilderIO;
		internal uint FontBuilderFlags;

		internal int PackIdMouseCursors;
		internal int PackIdLines;


		public ImFontAtlas() {

		}

		public ImFont AddFont(ImFontConfig config) {

		}

		public ImFont AddFontDefault(ImFontConfig? config = null) {

		}

		public ImFont AddFontFromFileTTF(string filename, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char,char)>? glyphRanges = null) {

		}

		public ImFont AddFontFromMemoryTTF(byte[] fontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null) {

		}

		public ImFont AddFontFromMemoryCompressedTTF(byte[] compressedFontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null) {

		}

		public ImFont AddFontFromMemoryCompressedBase85TTF(byte[] compressedFontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null) {

		}


		public void ClearInputData() {

		}

		public void ClearTexData() {

		}

		public void ClearFonts() {

		}

		public void Clear() {

		}


		public bool Build() {

		}

		public void GetTexDataAsAlpha8(Span<byte> outPixels, out int outWidth, out int outHeight, out int outBytesPerPixel) {

		}

		public void GetTexDataAsRGBA32(Span<byte> outPixels, out int outWidth, out int outHeight, out int outBytesPerPixel) {

		}

		public bool IsBuilt => Fonts.Count > 0 && TexReady;


		public static IEnumerable<(char, char)> GlyphRangesDefault;
		public static IEnumerable<(char, char)> GlyphRangesKorean;
		public static IEnumerable<(char, char)> GlyphRangesJapanese;
		public static IEnumerable<(char, char)> GlyphRangesChineseFull;
		public static IEnumerable<(char, char)> GlyphRangesChineseSimplifiedCommon;
		public static IEnumerable<(char, char)> GlyphRangesCyrillic;
		public static IEnumerable<(char, char)> GlyphRangesThai;
		public static IEnumerable<(char, char)> GlyphRangesVietnamese;


		public int AddCustomRectRegular(int width, int height) {

		} 

		public int AddCustomRectFontGlyph(ImFont font, char id, int width, int height, int advanceX, Vector2 offset = default) {

		}

		public ImFontAtlasCustomRect GetCustomRectByIndex(int index) {

		}


		public void CalcCustomRectUV(ImFontAtlasCustomRect rect, out Vector2 outUVMin, out Vector2 outUVMax) {

		}

		public bool GetMouseCursorTexData(ImGuiMouseCursor cursor, out Vector2 outOffset, out Vector2 outSize, Span<Vector2> outUVBorder, Span<Vector2> outUVFill) {

		}

	}

	public class ImGuiIO {

		// Configuration

		public ImGuiConfigFlags Flags = 0;
		public ImGuiBackendFlags BackendFlags = 0;
		public Vector2 DisplaySize;
		public float DeltaTime = 1.0f / 60.0f;
		public float IniSavingRate = 0.5f;
		public string IniFilename = "imgui.ini";
		public string LogFilename = "imgui_log.txt";
		public float MouseDoubleClickTime = 0.3f;
		public float MouseDoubleClickMaxDist = 6;
		public float MouseDragThreshold = 6;
		public float KeyRepeatDelay = 0.25f;
		public float KeyRepeatRate = 0.05f;

		public ImFontAtlas? Fonts;
		public float FontGlobalScale = 1;
		public bool FontAllowUserScaling = false;
		public ImFont? FontDefault = null;
		public Vector2 DisplayFramebufferScale = new(1, 1);

		public bool MouseDrawCursor = false;
		public bool ConfigMacOSXBehaviors = Platform.CurrentPlatformType == PlatformType.MacOSX;
		public bool ConfigInputTrickleEventQueue = true;
		public bool ConfigInputTextCursorBlink = true;
		public bool ConfigDragClickToInputText = false;
		public bool ConfigWindowsResizeFromEdges = true;
		public bool ConfigWindowsMoveFromTitleBarOnly = false;
		public float ConfigMemoryCompactTimer = 60.0f;

		// Platform Functions

		public string? BackendPlatformName = null;
		public string? BackendRendererName = null;

		public Func<string>? GetClipboardTextFn = null;
		public Action<string>? SetClipboardTextFn = null;

		public Action<ImGuiViewport, ImGuiPlatformImeData>? SetPlatformImeDataFn = null;
		public IntPtr ImeWindowHandle = IntPtr.Zero;

		// Input

		public void AddKeyEvent(ImGuiKey key, bool down) {

		}

		public void AddKeyAnalogEvent(ImGuiKey key, bool down, float v) {

		}

		public void AddMousePosEvent(float x, float y) {

		}

		public void AddMouseButtonEvent(int button, bool down) {

		} 

		public void AddMouseWheelEvent(float x, float y) {

		}

		public void AddFocusEvent(bool focused) {

		}

		public void AddInputCharacter(int c) {

		}

		public void AddInputCharacterUTF16(char c) {

		}

		public void AddInputCharactersUTF8(in ReadOnlySpan<byte> str) {

		}

		public void AddInputCharactersUTF8(IConstPointer<byte> str) {

		}


		public void ClearInputCharacters() {

		}

		public void ClearInputKeys() {

		}

		public void SetKeyEventNativeData(ImGuiKey key, int nativeKeycode, int nativeScancode, int nativeLegacyIndex = -1) {

		}

		// Output

		public bool WantCaptureMouse { get; internal set; }
		public bool WantCaptureKeyboard { get; internal set; }
		public bool WantTextInput { get; internal set; }
		public bool WantSetMousePos { get; internal set; }
		public bool WantSaveIniSettings { get; internal set; }
		public bool NavActive { get; internal set; }
		public float Framerate { get; internal set; }
		public int MetricsRenderVertices { get; internal set; }
		public int MetricsRenderIndices { get; internal set; }
		public int MetricsRenderWindows { get; internal set; }
		public int MetricsActiveWindows { get; internal set; }
		public int MetricsActiveAllocations { get; internal set; }
		public Vector2 MouseDelta { get; internal set; }

		// Internal

		internal Vector2 MousePos = new(-float.MaxValue);
		internal bool[] MouseDown = new bool[5];
		internal float MouseWheel;
		internal float MouseWheelH;
		internal bool KeyCtrl;
		internal bool KeyShift;
		internal bool KeyAlt;
		internal bool KeySuper;
		internal float[] NavInputs = new float[(int)ImGuiNavInput.Count];

		internal ImGuiKeyModFlags KeyMods;
		internal ImGuiKeyData[] KeysData = new ImGuiKeyData[(int)ImGuiKey.KeysDataSize];
		internal bool WantCaptureMouseUnlessPopupClose;
		internal Vector2 MousePosPrev = new(-float.MaxValue);
		internal struct MouseButtonData {
			public Vector2 ClickedPos = default;
			public double ClickedTime = default;
			public bool Clicked = default;
			public bool DoubleClicked = default;
			public ushort ClickedCount = default;
			public ushort LastClickedCount = default;
			public bool Released = default;
			public bool DownOwned = default;
			public bool DownOwnedUnlessPopupClosed = default;
			public float DownDuration = -1.0f;
			public float DownDurationPrev = default;
			public float DragMaxDistanceSqr = default;
			public MouseButtonData() { }
		}
		internal MouseButtonData[] Mouse = new MouseButtonData[5];
		internal float[] NavInputsDownDuration = new float[(int)ImGuiNavInput.Count];
		internal float[] NavInputsDownDurationPrev = new float[(int)ImGuiNavInput.Count];
		internal float PenPressure;
		internal bool AppFocusLost;
		internal sbyte BackendUsingLegacyKeyArrays = 0;
		internal bool BackendUsingLegacyNavInputArray = false;
		internal char InputQueueSurrogate;
		internal readonly List<char> InputQueueCharacters = new();

	}

	public class ImGuiStyle {

		public float Alpha = 1;
		public float DisabledAlpha = 0.6f;
		public Vector2 WindowPadding = new(8, 8);
		public float WindowRounding = 0;
		public float WindowBorderSize = 1;
		public Vector2 WindowMinSize = new(32, 32);
		public Vector2 WindowTitleAlign = new(0, 0.5f);
		public ImGuiDir WindowMenuButtonPosition = ImGuiDir.Left;
		public float ChildRounding = 0;
		public float ChildBorderSize = 1;
		public float PopupRounding = 0;
		public float PopupBorderSize = 1;
		public Vector2 FramePadding = new(4, 3);
		public float FrameRounding = 0;
		public float FrameBorderSize = 0;
		public Vector2 ItemSpacing = new(8, 4);
		public Vector2 ItemInnerSpacing = new(4, 4);
		public Vector2 CellPadding = new(4, 2);
		public Vector2 TouchExtraPadding = new(0, 0);
		public float IndentSpacing = 21;
		public float ColumnsMinSpacing = 6;
		public float ScrollbarSize = 14;
		public float ScrollbarRounding = 9;
		public float GrabMinSize = 10;
		public float GrabRounding = 0;
		public float LogSliderDeadzone = 4;
		public float TabRounding = 4;
		public float TabBorderSize = 0;
		public float TabMinWidthForCloseButton = 0;
		public ImGuiDir ColorButtonPosition = ImGuiDir.Right;
		public Vector2 ButtonTextAlign = new(0.5f, 0.5f);
		public Vector2 SelectableTextAlign = new(0, 0);
		public Vector2 DisplayWindowPadding = new(19, 19);
		public Vector2 DisplaySafeAreaPadding = new(3, 3);
		public float MouseCursorScale = 1;
		public bool AntiAliasedLines = true;
		public bool AntiAliasedLinesUseTex = true;
		public bool AntiAliasedFill = true;
		public float CurveTessellationTol = 1.25f;
		public float CircleTessellationMaxError = 0.3f;
		public readonly Vector4[] Colors = new Vector4[(int)ImGuiCol.Count];

		public ImGuiStyle() {
			ImGui.StyleColorsDark(this);
		}

		public void ScaleAllSizes(float scaleFactor) {
			WindowPadding = (WindowPadding * scaleFactor).Floor();
			WindowRounding = MathF.Floor(WindowRounding * scaleFactor);
			WindowMinSize = (WindowMinSize * scaleFactor).Floor();
			ChildRounding = MathF.Floor(ChildRounding * scaleFactor);
			PopupRounding = MathF.Floor(PopupRounding * scaleFactor);
			FramePadding = (FramePadding * scaleFactor).Floor();
			FrameRounding = MathF.Floor(FrameRounding * scaleFactor);
			ItemSpacing = (ItemSpacing * scaleFactor).Floor();
			ItemInnerSpacing = (ItemInnerSpacing * scaleFactor).Floor();
			CellPadding = (CellPadding * scaleFactor).Floor();
			TouchExtraPadding = (TouchExtraPadding * scaleFactor).Floor();
			IndentSpacing = MathF.Floor(IndentSpacing * scaleFactor);
			ColumnsMinSpacing = MathF.Floor(ColumnsMinSpacing * scaleFactor);
			ScrollbarSize = MathF.Floor(ScrollbarSize * scaleFactor);
			ScrollbarRounding = MathF.Floor(ScrollbarRounding * scaleFactor);
			GrabMinSize = MathF.Floor(GrabMinSize * scaleFactor);
			GrabRounding = MathF.Floor(GrabRounding * scaleFactor);
			LogSliderDeadzone = MathF.Floor(LogSliderDeadzone * scaleFactor);
			TabRounding = MathF.Floor(TabRounding * scaleFactor);
			TabMinWidthForCloseButton = MathF.Floor(TabMinWidthForCloseButton * scaleFactor);
			DisplayWindowPadding = (DisplayWindowPadding * scaleFactor).Floor();
			DisplaySafeAreaPadding = (DisplaySafeAreaPadding * scaleFactor).Floor();
			MouseCursorScale = MathF.Floor(MouseCursorScale * scaleFactor);
		}

	}

	public class ImDrawData {

		public bool Valid;
		public int TotalIdxCount;
		public int TotalVtxCount;
		public readonly List<ImDrawList> CmdLists = new();
		public Vector2 DisplayPos;
		public Vector2 DisplaySize;
		public Vector2 FramebufferScale;

		public void Clear() {
			Valid = false;
			TotalIdxCount = TotalVtxCount = 0;
			CmdLists.Clear();
			DisplayPos = DisplaySize = FramebufferScale = Vector2.Zero;
		}

		public void DeIndexAllBuffers() {

		}

		public void ScaleClipRects(Vector2 fbScale) {

		}

	}

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

	public class ImFont {

		public readonly List<float> IndexAdvanceX = new();
		public float FallbackAdvanceX { get; }
		public float FontSize;

		public readonly List<char> IndexLookup = new();
		public readonly List<ImFontGlyph> Glyphs = new();
		public ImFontGlyph FallbackGlyph { get; }

		public ImFontAtlas ContainerAtlas { get; }
		public readonly List<ImFontConfig> ConfigData = new();
		public char FallbackChar { get; } = (char)0xFFFD;
		public char EllipsisChar { get; } = '…';
		public char DotChar { get; } = '.';
		public bool DirtyLookupTables { get; }
		public float Scale = 1;
		public float Ascent { get; }
		public float Descent { get; }
		public int MetricsTotalSurface { get; }
		internal byte[] Used4kPagesMap = new byte[0x10000 / 4096 / 8];

		public ImFont() {

		}

		public ImFontGlyph FintGlyph(char c) {

		}

		public ImFontGlyph FindGlyphNoFallback(char c) {

		}

		public float GetCharAdvance(char c) => c < IndexAdvanceX.Count ? IndexAdvanceX[c] : FallbackAdvanceX;

		public bool IsLoaded => ContainerAtlas != null;

		public string DebugName => ConfigData.Count > 0 ? ConfigData[0].Name : "<unknown>";

		public Vector2 CalcTextSizeA(float size, float maxWidth, float wrapWidth, string text, int textBegin = 0) {

		}

		public int CalcWordWrapPositionA(float scale, string text, float wrapWidth, int textBegin = 0) {

		}

		public void RenderChar(ImDrawList drawList, float size, Vector2 pos, uint col, char c) {

		}

		public void RenderText(ImDrawList drawList, float size, Vector2 pos, uint col, Vector4 clipRect, string text, int textBegin = 0, int textEnd = -1, float wrapWidth = 0, bool cpuFineClip = false) {

		}


		internal void BuildLookupTable() {

		}

		internal void ClearOutputData() {

		}

		internal void GrowIndex(int newSize) {

		}

		internal void AddGlyph(ImFontConfig srcConfig, char c, Vector2 xy0, Vector2 xy1, Vector2 uv0, Vector2 uv1, float advanceX) {

		}

		internal void AddRemapChar(char dst, char src, bool overwriteDst = true) {

		}

		internal void SetGlyphVisible(char c, bool visible) {

		}

		internal bool IsGlyphRangeUnused(char cBegin, char cEnd) {

		}

	}

	public class ImGuiTableSortSpecs {

		private IReadOnlyList<ImGuiTableColumnSortSpects> specs = Array.Empty<ImGuiTableColumnSortSpects>();
		public IReadOnlyList<ImGuiTableColumnSortSpects> Specs {
			get => specs;
			set {
				specs = value;
				SpecsDirty = true;
			}
		}
		public bool SpecsDirty;

	}

	public readonly struct ImGuiPayload {

		public IPointer<byte>? Data { get; init; } = null;
		public int SourceId { get; init; } = default;
		public int SourceParentId { get; init; } = default;
		public int DataFrameCount { get; init; } = -1;
		public string DataType { get; init; } = "";
		public bool Preview { get; init; } = default;
		public bool Delivery { get; init; } = default;

		public ImGuiPayload() { }

		public bool IsDataType(string type) => DataFrameCount != -1 && type == DataType;

	}

	public struct ImGuiViewport {

		public ImGuiViewportFlags Flags;
		public Vector2 Pos;
		public Vector2 Size;
		public Vector2 WorkPos;
		public Vector2 WorkSize;

		public IntPtr PlatformHandleRaw;

		public Vector2 Center => Pos + Size * 0.5f;
		public Vector2 WorkCenter => Pos + Size * 0.5f;

	}

	public class ImGuiStorage {

		public struct ImGuiStoragePair {

			public int Key;

			public int AsInt {
				get => (int)AsNInt;
				set => AsNInt = value;
			}
			public float AsFloat {
				get => BitConverter.Int32BitsToSingle((int)AsNInt);
				set => AsNInt = BitConverter.SingleToInt32Bits(value);
			}
			public nint AsNInt;

		}

		public readonly List<ImGuiStoragePair> Data = new();

		public void Clear() => Data.Clear();

		public int GetInt(int key, int defaultVal = 0) { }
		public void SetInt(int key, int val) { }
		public bool GetBool(int key, bool defaultVal = false) { }
		public void SetBool(int key, bool val) { }
		public float GetFloat(int key, float defaultVal = 0) { }
		public void SetFloat(int key, float val) { }
		public nint GetPtr(int key, nint defaultVal = 0) { }
		public void SetPtr(int key, nint val) { }

		public void BuildSortByKey() {

		}

	}

	public class ImGuiMouseCursor {

	}

	public struct ImGuiKeyData {

		public bool Down = default;
		public float DownDuration = -1.0f;
		public float DownDurationPrev = default;
		public float AnalogValue = default;

		public ImGuiKeyData() { }

	}

	public struct ImGuiPlatformImeData {

		public bool WantVisible;
		public Vector2 InputPos;
		public float InputLineHeight;

	}

	public record ImGuiTableColumnSortSpects {

		public int ColumnUserID { get; init; }
		public short ColumnIndex { get; init; }
		public short ColumnOrder { get; init; }
		public ImGuiSortDirection SortDirection { get; init; }

	}

	public class ImGuiOnceUponAFrame {

		public int RefFrame = -1;

		public ImGuiOnceUponAFrame() { }

		public static implicit operator bool(ImGuiOnceUponAFrame uoaf) {
			int currentFrame = ImGui.FrameCount;
			if (uoaf.RefFrame == currentFrame) return false;
			uoaf.RefFrame = currentFrame;
			return true;
		}

	}

	public class ImGuiTextFilter {

		public readonly char[] InputBuf = new char[256];
		public readonly List<string> Filters = new();
		public int CountGrep;

		public ImGuiTextFilter(string defaultFilter = "") {

		}

		public bool Draw(string label = "Filter (inc,-exc)", float width = 0.0f) {

		}

		public bool PassFilter(string text) {

		}

		public void Build() {

		}

		public void Clear() {
			InputBuf[0] = '\0';
			Build();
		}

		public bool IsActive => Filters.Count != 0;

	}

	public class ImGuiListClipper {

		public int DisplayStart;
		public int DisplayEnd;
		public int ItemsCount;
		public float ItemsHeight;
		public float StartPosY;

		public void Begin(int itemsCount, float itemsHeight = -1) {

		}

		public void End() {

		}

		public bool Step() {

		}

		public void ForceDisplayRangeByIndices(int itemMin, int itemMax) {

		}

	}

	public struct ImColor {

		public Vector4 Value;

		public ImColor(float r, float g, float b, float a) {
			Value = new(r, g, b, a);
		}

		public ImColor(Vector4 val) {
			Value = val;
		}

		const float INT_SCALE = 1.0f / 255.0f;

		public ImColor(int r, int g, int b, int a) {
			Value = new Vector4(r, g, b, a) * INT_SCALE;
		}

		public ImColor(uint rgba) {
			Value = new Vector4(
				(rgba >>  0) & 0xFF,
				(rgba >>  8) & 0xFF,
				(rgba >> 16) & 0xFF,
				(rgba >> 24) & 0xFF
			) * INT_SCALE;
		}

		public void SetHSV(float h, float s, float v, float a = 1) {
			Vector3 rgb = ImGui.ColorConvertHSVToRGB(new Vector3(h, s, v));
			Value.X = rgb.X;
			Value.Y = rgb.Y;
			Value.Z = rgb.Z;
			Value.W = a;
		}

		public static ImColor HSV(float h, float s, float v, float a = 1) {
			Vector3 rgb = ImGui.ColorConvertHSVToRGB(new Vector3(h, s, v));
			return new ImColor(rgb.X, rgb.Y, rgb.Z, a);
		}

	}

	public delegate void ImDrawCallback(ImDrawList parentList, ImDrawCmd cmd);

	public struct ImDrawCmd {

		public Vector4 ClipRect;
		public nuint TextureId;
		public uint VtxOffset;
		public uint IdxOffset;
		public uint ElemCount;
		public ImDrawCallback? UserCallback;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImDrawVert {

		public Vector2 Pos;
		public Vector2 UV;
		public Vector4 Col;

	}

	public class ImDrawChannel {

		public readonly List<ImDrawCmd> CmdBuffer = new();
		public readonly List<ushort> IdxBuffer = new();

	}

	public class ImDrawListSplitter {

		private int current;
		private int count;
		private readonly List<ImDrawChannel> channels = new();

		public void Clear() {
			current = 0;
			count = 1;
		}

		public void ClearFreeMemory() {

		}

		public void Split(ImDrawList drawList, int count) {

		}

		public void Merge(ImDrawList drawList) {

		}

		public void SetCurrentChannel(ImDrawList drawList, int channel) {

		}

	}

	public record class ImFontConfig {

		public byte[] FontData { get; init; } = null!;
		public int FontNo { get; init; } = 0;
		public float SizePixels { get; init; }
		public int OversampleH { get; init; } = 3;
		public int OversampleV { get; init; } = 1;
		public bool PixelSnapH { get; init; } = false;
		public Vector2 GlyphExtraSpacing { get; init; } = Vector2.Zero;
		public Vector2 GlyphOffset { get; init; } = Vector2.Zero;
		public readonly List<(char, char)> GlyphRanges = new();
		public float GlyphMinAdvanceX { get; init; } = 0;
		public float GlyphMaxAdvanceX { get; init; } = float.MaxValue;
		public bool MergeMode { get; init; } = false;
		public uint FontBuilderFlags { get; init; } = 0;
		public float RasterizerMultiply { get; init; } = 1;
		public char EllipsisChar { get; init; } = unchecked((char)-1);

		internal string Name = "";
		internal ImFont? DstFont;
	}

	public struct ImFontGlyph {

		public bool Colored;
		public bool Visible;
		public int Codepoint;
		public float AdvanceX;
		public Vector2 XY0;
		public Vector2 XY1;
		public Vector4 UV0;
		public Vector4 UV1;

	}

	public class ImFontGlyphRangesBuilder {

		public readonly List<uint> UsedChars = new();

		public void Clear() {
			UsedChars.EnsureCapacity(0x4000);
			for (int i = 0; i < UsedChars.Count; i++) UsedChars[i] = 0;
		}

		public bool GetBit(int n) {
			uint i = UsedChars[n >> 5];
			return (i & (1 << (n & 0x1F))) != 0;
		}

		public void SetBit(int n) {
			int i = n >> 5;
			uint u = UsedChars[i];
			u |= 1u << (n & 0x1F);
			UsedChars[i] = u;
		}

		public void AddChar(char c) => SetBit((int)c);
		
		public void AddText(string text) {

		}

		public void AddRanges(IEnumerable<(char,char)> ranges) {

		}

		public void BuildRanges(ICollection<(char,char)> outRanges) {

		}

	}

	public class ImFontAtlasCustomRect {

		public ushort Width = 0, Height = 0;
		public ushort X = 0xFFFF, Y = 0xFFFF;
		public uint GlyphID = 0;
		public float GlyphAdvanceX = 0;
		public Vector2 GlyphOffset = Vector2.Zero;
		public ImFont? Font = null;

		public ImFontAtlasCustomRect() { }

		public bool IsPacked => X != 0xFFFF;

	}

	public class ImFontBuilderIO {

	}

}

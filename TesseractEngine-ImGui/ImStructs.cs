using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;
using Tesseract.Core.Util;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.ImGui {

	using ImTextureID = IntPtr;
	using ImDrawIdx = UInt16;
	using ImGuiID = UInt32;
	using ImWchar16 = Char;
	using ImWchar32 = UInt32;
	using ImWchar = Char;
	using ImVec2 = Vector2;
	using ImVec4 = Vector4;

	public struct ImVector {

		public int Size;
		public int Capacity;
		public IntPtr Data;

		public Span<T> AsSpan<T>() where T : unmanaged {
			unsafe {
				return new Span<T>((void*)Data, Size);
			}
		}

		public ImVector<T> AsTVector<T>() where T : struct => new() { RawVector = this };

	}

	public struct ImVector<T> where T : struct {

		public ImVector RawVector;

		public static implicit operator ManagedPointer<T>(ImVector<T> v) {
			return new ManagedPointer<T>(v.RawVector.Data, null, v.RawVector.Size);
		}

		public T this[int index] {
			get => ((ManagedPointer<T>)this)[index];
			set {
				var ptr = ((ManagedPointer<T>)this);
				ptr[index] = value;
			}
		}

	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ImGuiStyle {

		public float Alpha;
		public float DisabledAlpha;
		public ImVec2 WindowPadding;
		public float WindowRounding;
		public float WindowBorderSize;
		public ImVec2 WindowMinSize;
		public ImVec2 WindowTitleAlign;
		public ImGuiDir WindowMenuButtonPosition;
		public float ChildRounding;
		public float ChildBorderSize;
		public float PopupRounding;
		public float PopupBorderSize;
		public ImVec2 FramePadding;
		public float FrameRounding;
		public float FrameBorderSize;
		public ImVec2 ItemSpacing;
		public ImVec2 ItemInnerSpacing;
		public ImVec2 CellPadding;
		public ImVec2 TouchExtraPadding;
		public float IndentSpacing;
		public float ColumnsMinSpacing;
		public float ScrollbarSize;
		public float ScrollbarRounding;
		public float GrabMinSize;
		public float GrabRounding;
		public float LogSliderDeadzone;
		public float TabRounding;
		public float TabBorderSize;
		public float TabMinWidthForCloseButton;
		public ImGuiDir ColorButtonPosition;
		public ImVec2 ButtonTextAlign;
		public ImVec2 SelectableTextAlign;
		public ImVec2 DisplayWindowPadding;
		public ImVec2 DisplaySafeAreaPadding;
		public float MouseCursorScale;
		[MarshalAs(UnmanagedType.U1)]
		public bool AntiAliasedLines;
		[MarshalAs(UnmanagedType.U1)]
		public bool AntiAliasedLinesUseText;
		[MarshalAs(UnmanagedType.U1)]
		public bool AntiAliasedFill;
		public float CurveTessellationTol;
		public float CircleTessellationMaxError;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ImGuiCol.Count)]
		public ImVec4[] Colors;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiIO {
		
		public ImGuiConfigFlags ConfigFlags;
		public ImGuiBackendFlags BackendFlags;
		public ImVec2 DisplaySize;
		public float DeltaTime;
		public float IniSavingRate;
		[MarshalAs(UnmanagedType.LPStr)]
		public string IniFilename;
		[MarshalAs(UnmanagedType.LPStr)]
		public string LogFilename;
		public float MouseDoubleClickTime;
		public float MouseDoubleClickMaxDist;
		public float MouseDragThreshold;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ImGuiKey.Count)]
		public int[] KeyMap;
		public float KeyRepeatDelay;
		public float KeyRepeatRate;
		public IntPtr UserData;
		[NativeType("ImFontAtlas*")]
		public IntPtr FontAtlas;
		public float FontGlobalScale;
		[MarshalAs(UnmanagedType.U1)]
		public bool FontAllowUserScaling;
		[NativeType("ImFont*")]
		public IntPtr FontDefault;
		public ImVec2 DisplayFramebufferScale;
		[MarshalAs(UnmanagedType.U1)]
		public bool MouseDrawCursor;
		[MarshalAs(UnmanagedType.U1)]
		public bool ConfigMacOSXBehaviors;
		[MarshalAs(UnmanagedType.U1)]
		public bool ConfigInputTextCursorBlink;
		[MarshalAs(UnmanagedType.U1)]
		public bool ConfigDragClickToInputText;
		[MarshalAs(UnmanagedType.U1)]
		public bool ConfigWindowsResizeFromEdges;
		[MarshalAs(UnmanagedType.U1)]
		public bool ConfigWindowsMoveFromTitleBarOnly;
		public float ConfigMemoryCompactTimer;
		[MarshalAs(UnmanagedType.LPStr)]
		public string BackendPlatformName;
		[MarshalAs(UnmanagedType.LPStr)]
		public string BackendRendererName;
		public IntPtr BackendPlatformUserData;
		public IntPtr BackendRendererUserData;
		public IntPtr BackendLanguageUserData;
		[return: NativeType("const char*")]
		public delegate IntPtr GetClipboardTextFn(IntPtr userdata);
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public GetClipboardTextFn GetClipboardText;
		public delegate void SetClipboardTextFn(IntPtr userdata, [MarshalAs(UnmanagedType.LPStr)] string text);
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SetClipboardTextFn SetClipboardText;
		public IntPtr ClipboardUserData;
		public delegate void ImeSetInputScreenPosFn(int x, int y);
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ImeSetInputScreenPosFn ImeSetInputScreenPos;
		public IntPtr ImeWindowHandle;
		public ImVec2 MousePos;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public byte[] MouseDown;
		public float MouseWheel;
		public float MouseWheelH;
		[MarshalAs(UnmanagedType.U1)]
		public bool KeyCtrl;
		[MarshalAs(UnmanagedType.U1)]
		public bool KeyShift;
		[MarshalAs(UnmanagedType.U1)]
		public bool KeyAlt;
		[MarshalAs(UnmanagedType.U1)]
		public bool KeySuper;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		public byte[] KeysDown;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ImGuiNavInput.Count)]
		public float[] NavInputs;
		[MarshalAs(UnmanagedType.U1)]
		public bool WantCaptureMouse;
		[MarshalAs(UnmanagedType.U1)]
		public bool WantCaptureKeyboard;
		[MarshalAs(UnmanagedType.U1)]
		public bool WantTextInput;
		[MarshalAs(UnmanagedType.U1)]
		public bool WantSetMousePos;
		[MarshalAs(UnmanagedType.U1)]
		public bool WantSaveIniSettings;
		[MarshalAs(UnmanagedType.U1)]
		public bool NavActive;
		[MarshalAs(UnmanagedType.U1)]
		public bool NavVisible;
		public float Framerate;
		public int MetricsRenderVertices;
		public int MetricsRenderIndices;
		public int MetricsRenderWindows;
		public int MetricsActiveWindows;
		public int MetricsActiveAllocations;
		public ImVec2 MouseDelta;
		[MarshalAs(UnmanagedType.U1)]
		public bool WantCaptureMouseUnlessPopupClose;
		public ImGuiKeyModFlags KeyMods;
		public ImGuiKeyModFlags KeyModsPrev;
		public ImVec2 MousePosPriv;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public ImVec2[] MouseClickedPos;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public double[] MouseClickedTime;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public byte[] MouseClicked;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public byte[] MouseDoubleClicked;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public byte[] MouseReleased;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public byte[] MouseDownOwned;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public byte[] MouseDownOwnedUnlessPopupClose;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public byte[] MouseDownWasDoubleClick;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public float[] MouseDownDuration;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public float[] MouseDownDurationPrev;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public ImVec2[] MouseDragMaxDistanceAbs;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public float[] MouseDragMaxDistanceSqr;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		public float[] KeysDownDuration;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		public float[] KeysDownDurationPrev;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ImGuiNavInput.Count)]
		public float[] NavInputsDownDuration;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ImGuiNavInput.Count)]
		public float[] NavInputsDownDurationPrev;
		public float PenPressure;
		[MarshalAs(UnmanagedType.U1)]
		public bool AppFocusLost;
		public ImWchar InputQueueSurrogate;
		private ImVector inputQueueCharacters;
		public ImVector<ImWchar> InputQueueCharacters {
			get => inputQueueCharacters.AsTVector<ImWchar>();
			set => inputQueueCharacters = value.RawVector;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiInputTextCallbackData {

		public ImGuiInputTextFlags EventFlags;
		public ImGuiInputTextFlags Flags;
		public IntPtr UserData;
		public ImWchar EventChar;
		public ImGuiKey EventKey;
		[NativeType("char*")]
		public IntPtr Buf;
		public int BufTextLen;
		public int BufSize;
		[MarshalAs(UnmanagedType.U1)]
		public bool BufDirty;
		public int CursorPos;
		public int SelectionStart;
		public int SelectionEnd;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiSizeCallbackData {

		public IntPtr UserData;
		public ImVec2 Pos;
		public ImVec2 CurrentSize;
		public ImVec2 DesiredSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiPayload {

		public IntPtr Data;
		public int DataSize;
		public ImGuiID SourceID;
		public ImGuiID SourceParentID;
		public int DataFrameCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 + 1)]
		public byte[] DataType;
		[MarshalAs(UnmanagedType.U1)]
		public bool Preview;
		[MarshalAs(UnmanagedType.U1)]
		public bool Delivery;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiTableColumnSortSpecs {

		public ImGuiID ColumnUserID;
		public short ColumnIndex;
		public short SortOrder;
		[MarshalAs(UnmanagedType.U1)]
		public ImGuiSortDirection SortDirection;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiTableSortSpecs {

		[NativeType("const ImGuiTableColumnSortSpecs*")]
		public IntPtr Specs;
		public int SpecsCount;
		[MarshalAs(UnmanagedType.U1)]
		public bool SpecsDirty;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiOnceUponAFrame {

		public int RefFrame;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiTextRange {

		[NativeType("const char*")]
		public IntPtr B;
		[NativeType("const char*")]
		public IntPtr E;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiTextFilter {

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] InputBuf;
		private ImVector filters;
		public ImVector<ImGuiTextRange> Filters {
			get => filters.AsTVector<ImGuiTextRange>();
			set => filters = value.RawVector;
		}
		public int CountGrep;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiTextBuffer {

		private ImVector buf;
		public ImVector<byte> Buf {
			get => buf.AsTVector<byte>();
			set => buf = value.RawVector;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiStoragePair {

		public ImGuiID Key;

		[StructLayout(LayoutKind.Explicit)]
		public struct Val {

			[FieldOffset(0)]
			public int ValI;
			[FieldOffset(0)]
			public float ValF;
			[FieldOffset(0)]
			public IntPtr ValP;

		}

		public Val Value;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiStorage {

		public ImGuiStoragePair Data;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiListClipper {

		public int DisplayStart;
		public int DisplayEnd;
		public int ItemsCount;
		public int StepNo;
		public int ItemsFrozen;
		public float ItemsHeight;
		public float StartPosY;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImColor {

		public ImVec4 Value;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImDrawCmd {

		public ImVec4 ClipRect;
		public ImTextureID TextureID;
		public uint VtxOffset;
		public uint IdxOffset;
		public uint ElemCount;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ImDrawCallback UserCallback;
		public IntPtr UserCallbackData;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImDrawVert {

		public ImVec2 Pos;
		public ImVec2 UV;
		public uint Col;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImDrawCmdHeader {

		public ImVec4 ClipRect;
		public ImTextureID TextureID;
		public uint VtxOffset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImDrawChannel {

		private ImVector cmdBuffer;
		public ImVector<ImDrawCmd> CmdBuffer {
			get => cmdBuffer.AsTVector<ImDrawCmd>();
			set => cmdBuffer = value.RawVector;
		}
		private ImVector idxBuffer;
		public ImVector<ImDrawIdx> IdxBuffer {
			get => idxBuffer.AsTVector<ImDrawIdx>();
			set => idxBuffer = value.RawVector;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImDrawListSplitter {

		public int Current;
		public int Count;
		private ImVector channels;
		public ImVector<ImDrawChannel> Channels {
			get => channels.AsTVector<ImDrawChannel>();
			set => channels = value.RawVector;
		}

	}



	[StructLayout(LayoutKind.Sequential)]
	public struct ImDrawList {

		private ImVector cmdBuffer;
		public ImVector<ImDrawCmd> CmdBuffer {
			get => cmdBuffer.AsTVector<ImDrawCmd>();
			set => cmdBuffer = value.RawVector;
		}
		private ImVector idxBuffer;
		public ImVector<ImDrawIdx> IdxBuffer {
			get => idxBuffer.AsTVector<ImDrawIdx>();
			set => idxBuffer = value.RawVector;
		}
		private ImVector vtxBuffer;
		public ImVector<ImDrawVert> VtxBuffer {
			get => idxBuffer.AsTVector<ImDrawVert>();
			set => idxBuffer = value.RawVector;
		}
		public ImDrawListFlags Flags;
		public uint VtxCurrentIdx;
		[NativeType("const ImDrawListSharedData*")]
		public IntPtr Data;
		[MarshalAs(UnmanagedType.LPStr)]
		public string OwnerName;
		[NativeType("ImDrawVert*")]
		public IntPtr VtxWritePtr;
		[NativeType("ImDrawIdx*")]
		public IntPtr IdxWritePtr;
		private ImVector clipRectStack;
		public ImVector<ImVec4> ClipRectStack {
			get => clipRectStack.AsTVector<ImVec4>();
			set => clipRectStack = value.RawVector;
		}
		private ImVector textureIdStack;
		public ImVector<ImTextureID> TextureIdStack {
			get => textureIdStack.AsTVector<ImTextureID>();
			set => textureIdStack = value.RawVector;
		}
		private ImVector path;
		public ImVector<ImVec2> Path {
			get => path.AsTVector<ImVec2>();
			set => path = value.RawVector;
		}
		public ImDrawCmdHeader CmdHeader;
		public ImDrawListSplitter Splitter;
		public float FringeScale;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImDrawData {

		[MarshalAs(UnmanagedType.U1)]
		public bool Valid;
		public int CmdListsCount;
		public int TotalIdxCount;
		public int TotalVtxCount;
		[NativeType("ImDrawList**")]
		public IntPtr CmdLists;
		public ImVec2 DisplayPos;
		public ImVec2 DisplaySize;
		public ImVec2 FramebufferScale;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImFontConfig {

		public IntPtr FontData;
		public int FontDataSize;
		[MarshalAs(UnmanagedType.U1)]
		public bool FontDataOwnedByAtlas;
		public int FontNo;
		public float SizePixels;
		public int OversampleH;
		public int OversampleV;
		[MarshalAs(UnmanagedType.U1)]
		public bool PixelSnapH;
		public ImVec2 GlyphExtraSpacing;
		public ImVec2 GlyphOffset;
		[NativeType("const ImWchar*")]
		public IntPtr GlyphRanges;
		public float GlyphMinAdvanceX;
		public float GlyphMaxAdvanceX;
		[MarshalAs(UnmanagedType.U1)]
		public bool MergeMode;
		public uint FontBuilderFlags;
		public float RasterizerMultiply;
		public ImWchar EllipsisChar;
		private unsafe fixed byte name[40];
		public string Name {
			get {
				unsafe {
					fixed(byte* pName = name) {
						return MemoryUtil.GetUTF8((IntPtr)pName, 40);
					}
				}
			}
			set {
				unsafe {
					fixed (byte* pName = name) {
						MemoryUtil.PutUTF8(value, (IntPtr)pName, 40);
					}
				}
			}
		}
		[NativeType("ImFont*")]
		public IntPtr DstFont;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImFontGlyph {

		private const uint MASK_COLORED = 1;
		private const uint MASK_VISIBLE = 1;
		private const uint MASK_CODEPOINT = 0xFFFFFFFC;
		private const int OFFSET_CODEPOINT = 2;

		// Bitfield hell
		private uint cvc;
		public bool Colored {
			get => (cvc & MASK_COLORED) != 0;
			set {
				if (value) cvc |= MASK_COLORED;
				else cvc &= ~MASK_COLORED;
			}
		}
		public bool Visible {
			get => (cvc & MASK_VISIBLE) != 0;
			set {
				if (value) cvc |= MASK_VISIBLE;
				else cvc &= ~MASK_VISIBLE;
			}
		}
		public uint Codepoint {
			get => (cvc & MASK_CODEPOINT) >> OFFSET_CODEPOINT;
			set => cvc = (cvc & ~MASK_CODEPOINT) | ((value << OFFSET_CODEPOINT) & MASK_CODEPOINT);
		}
		public float AdvanceX;
		public float X0, Y0, X1, Y1;
		public float U0, V0, U1, V1;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImFontGlyphRangesBuilder {

		private ImVector usedChars;
		public ImVector<uint> UsedChars {
			get => usedChars.AsTVector<uint>();
			set => usedChars = value.RawVector;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImFontAtlasCustomRect {

		public ushort Width, Height;
		public ushort X, Y;
		public uint GlyphID;
		public float GlyphAdvanceX;
		public ImVec2 GlyphOffset;
		[NativeType("ImFont*")]
		public IntPtr Font;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImFontAtlas {

	}

}

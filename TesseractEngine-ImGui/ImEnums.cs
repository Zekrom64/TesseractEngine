using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui {

	//=========//
	// imgui.h //
	//=========//

	//// Enums/Flags (declared as int for compatibility with old C++, to allow using as flags without overhead, and to not pollute the top of this file)
	//// - Tip: Use your programming IDE navigation facilities on the names in the _central column_ below to find the actual flags/enum lists!
	////   In Visual Studio IDE: CTRL+comma ("Edit.NavigateTo") can follow symbols in comments, whereas CTRL+F12 ("Edit.GoToImplementation") cannot.
	////   With Visual Assist installed: ALT+G ("VAssistX.GoToImplementation") can also follow symbols in comments.
	//typedef int ImGuiCol;               // -> enum ImGuiCol_             // Enum: A color identifier for styling
	//typedef int ImGuiCond;              // -> enum ImGuiCond_            // Enum: A condition for many Set*() functions
	//typedef int ImGuiDataType;          // -> enum ImGuiDataType_        // Enum: A primary data type
	//typedef int ImGuiDir;               // -> enum ImGuiDir_             // Enum: A cardinal direction
	//typedef int ImGuiKey;               // -> enum ImGuiKey_             // Enum: A key identifier
	//typedef int ImGuiNavInput;          // -> enum ImGuiNavInput_        // Enum: An input identifier for navigation
	//typedef int ImGuiMouseButton;       // -> enum ImGuiMouseButton_     // Enum: A mouse button identifier (0=left, 1=right, 2=middle)
	//typedef int ImGuiMouseCursor;       // -> enum ImGuiMouseCursor_     // Enum: A mouse cursor identifier
	//typedef int ImGuiSortDirection;     // -> enum ImGuiSortDirection_   // Enum: A sorting direction (ascending or descending)
	//typedef int ImGuiStyleVar;          // -> enum ImGuiStyleVar_        // Enum: A variable identifier for styling
	//typedef int ImGuiTableBgTarget;     // -> enum ImGuiTableBgTarget_   // Enum: A color target for TableSetBgColor()
	//typedef int ImDrawFlags;            // -> enum ImDrawFlags_          // Flags: for ImDrawList functions
	//typedef int ImDrawListFlags;        // -> enum ImDrawListFlags_      // Flags: for ImDrawList instance
	//typedef int ImFontAtlasFlags;       // -> enum ImFontAtlasFlags_     // Flags: for ImFontAtlas build
	//typedef int ImGuiBackendFlags;      // -> enum ImGuiBackendFlags_    // Flags: for io.BackendFlags
	//typedef int ImGuiButtonFlags;       // -> enum ImGuiButtonFlags_     // Flags: for InvisibleButton()
	//typedef int ImGuiColorEditFlags;    // -> enum ImGuiColorEditFlags_  // Flags: for ColorEdit4(), ColorPicker4() etc.
	//typedef int ImGuiConfigFlags;       // -> enum ImGuiConfigFlags_     // Flags: for io.ConfigFlags
	//typedef int ImGuiComboFlags;        // -> enum ImGuiComboFlags_      // Flags: for BeginCombo()
	//typedef int ImGuiDragDropFlags;     // -> enum ImGuiDragDropFlags_   // Flags: for BeginDragDropSource(), AcceptDragDropPayload()
	//typedef int ImGuiFocusedFlags;      // -> enum ImGuiFocusedFlags_    // Flags: for IsWindowFocused()
	//typedef int ImGuiHoveredFlags;      // -> enum ImGuiHoveredFlags_    // Flags: for IsItemHovered(), IsWindowHovered() etc.
	//typedef int ImGuiInputTextFlags;    // -> enum ImGuiInputTextFlags_  // Flags: for InputText(), InputTextMultiline()
	//typedef int ImGuiKeyModFlags;       // -> enum ImGuiKeyModFlags_     // Flags: for io.KeyMods (Ctrl/Shift/Alt/Super)
	//typedef int ImGuiPopupFlags;        // -> enum ImGuiPopupFlags_      // Flags: for OpenPopup*(), BeginPopupContext*(), IsPopupOpen()
	//typedef int ImGuiSelectableFlags;   // -> enum ImGuiSelectableFlags_ // Flags: for Selectable()
	//typedef int ImGuiSliderFlags;       // -> enum ImGuiSliderFlags_     // Flags: for DragFloat(), DragInt(), SliderFloat(), SliderInt() etc.
	//typedef int ImGuiTabBarFlags;       // -> enum ImGuiTabBarFlags_     // Flags: for BeginTabBar()
	//typedef int ImGuiTabItemFlags;      // -> enum ImGuiTabItemFlags_    // Flags: for BeginTabItem()
	//typedef int ImGuiTableFlags;        // -> enum ImGuiTableFlags_      // Flags: For BeginTable()
	//typedef int ImGuiTableColumnFlags;  // -> enum ImGuiTableColumnFlags_// Flags: For TableSetupColumn()
	//typedef int ImGuiTableRowFlags;     // -> enum ImGuiTableRowFlags_   // Flags: For TableNextRow()
	//typedef int ImGuiTreeNodeFlags;     // -> enum ImGuiTreeNodeFlags_   // Flags: for TreeNode(), TreeNodeEx(), CollapsingHeader()
	//typedef int ImGuiViewportFlags;     // -> enum ImGuiViewportFlags_   // Flags: for ImGuiViewport
	//typedef int ImGuiWindowFlags;       // -> enum ImGuiWindowFlags_     // Flags: for Begin(), BeginChild()

	[Flags]
	public enum ImGuiWindowFlags : int {
		None = 0,
		NoTitleBar = 1 << 0,
		NoResize = 1 << 1,
		NoMove = 1 << 2,
		NoScrollbar = 1 << 3,
		NoScrollbarWithMouse = 1 << 4,
		NoCollapse = 1 << 5,
		AlwaysAutoResize = 1 << 6,
		NoBackground = 1 << 7,
		NoSavedSettings = 1 << 8,
		NoMouseInputs = 1 << 9,
		MenuBar = 1 << 10,
		HorizontalScrollbar = 1 << 11,
		NoFocusOnAppearing = 1 << 12,
		NoBringToFrontOnFocus = 1 << 13,
		AlwaysVerticalScrollbar = 1 << 14,
		AlwaysHorizontalScrollbar = 1 << 15,
		AlwaysUseWindowPadding = 1 << 16,
		NoNavInputs = 1 << 18,
		NoNavFocus = 1 << 19,
		UnsavedDocument = 1 << 20,
		NoNav = NoNavInputs | NoNavFocus,
		NoDecoration = NoTitleBar | NoResize | NoScrollbar | NoCollapse,
		NoInput = NoMouseInputs | NoNavInputs | NoNavFocus,
		NavFlattened = 1 << 23,
		ChildWindow = 1 << 24,
		Tooltip = 1 << 25,
		Popup = 1 << 26,
		Modal = 1 << 27,
		ChildMenu = 1 << 28
	}

	[Flags]
	public enum ImGuiInputTextFlags : int {
		None = 0,
		CharsDecimal = 1 << 0,
		CharsHexadecimal = 1 << 1,
		CharsUppercase = 1 << 2,
		CharsNoBlank = 1 << 3,
		AutoSelectAll = 1 << 4,
		EnterReturnsTrue = 1 << 5,
		CallbackCompletion = 1 << 6,
		CallbackHistory = 1 << 7,
		CallbackAlways = 1 << 8,
		CallbackCharFilter = 1 << 9,
		AllowTabInput = 1 << 10,
		CtrlEnterForNewLine = 1 << 11,
		NoHorizontalScroll = 1 << 12,
		AlwaysOverwrite = 1 << 13,
		ReadOnly = 1 << 14,
		Password = 1 << 15,
		NoUndoRedo = 1 << 16,
		CharsScientific = 1 << 17,
		CallbackResize = 1 << 18,
		CallbackEdit = 1 << 19
	}

	[Flags]
	public enum ImGuiTreeNodeFlags : int {
		None = 0,
		Selected = 1 << 0,
		Framed = 1 << 1,
		AllowItemOverlap = 1 << 2,
		NoTreePushOnOpen = 1 << 3,
		NoAutoOpenLog = 1 << 4,
		DefaultOpen = 1 << 5,
		OpenOnDoubleClick = 1 << 6,
		OpenOnArrow = 1 << 7,
		Leaf = 1 << 8,
		Bullet = 1 << 9,
		FramePadding = 1 << 10,
		SpanAvailWidth = 1 << 11,
		SpanFullWidth = 1 << 12,
		NavLeftJumpsBackHere = 1 << 13,
		CollapsingHeader = Framed | NoTreePushOnOpen | NoAutoOpenLog
	}

	[Flags]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "Values are duplicated as per the source header")]
	public enum ImGuiPopupFlags : int {
		None = 0,
		MouseButtonLeft = 0,
		MouseButtonRight = 1,
		MouseButtonMiddle = 2,
		MouseButtonMask = 0x1F,
		MouseButtonDefault = 1,
		NoOpenOverExistingPopup = 1 << 5,
		NoOpenOverItems = 1 << 6,
		AnyPopupId = 1 << 7,
		AnyPopupLevel = 1 << 8,
		AnyPopup = AnyPopupId | AnyPopupLevel
	}

	[Flags]
	public enum ImGuiSelectableFlags : int {
		None = 0,
		DontClosePopups = 1 << 0,
		SpanAllColumns = 1 << 1,
		AllowDoubleClick = 1 << 2,
		Disabled = 1 << 3,
		AllowItemOverlap = 1 << 4
	}

	[Flags]
	public enum ImGuiComboFlags : int {
		None = 0,
		PopupAlignLeft = 1 << 0,
		HeightSmall = 1 << 1,
		HeightRegular = 1 << 2,
		HeightLarge = 1 << 3,
		HeightLargest = 1 << 4,
		NoArrowButton = 1 << 5,
		NoPreview = 1 << 6,
		HeightMask = HeightSmall | HeightRegular | HeightLarge | HeightLargest
	}

	[Flags]
	public enum ImGuiTabBarFlags : int {
		None = 0,
		Reorderable = 1 << 0,
		AutoSelectNewTabs = 1 << 1,
		TabListPopupButton = 1 << 2,
		NoCloseWithMiddleMouseButton = 1 << 3,
		NoTabListScrollingButtons = 1 << 4,
		NoTooltip = 1 << 5,
		FittingPolicyResizeDown = 1 << 6,
		FittingPolicyScroll = 1 << 7,
		FittingPolicyMask = FittingPolicyResizeDown | FittingPolicyScroll,
		FittingPolicyDefault = FittingPolicyResizeDown
	}

	[Flags]
	public enum ImGuiTabItemFlags : int {
		None = 0,
		UnsavedDocument = 1 << 0,
		SetSelected = 1 << 1,
		NoCloseWithMiddleMouseButton = 1 << 2,
		NoPushId = 1 << 3,
		NoTooltip = 1 << 4,
		NoReorder = 1 << 5,
		Leading = 1 << 6,
		Trailing = 1 << 7
	}

	[Flags]
	public enum ImGuiTableFlags : int {
		None = 0,
		Resizable = 1 << 0,
		Reorderable = 1 << 1,
		Hideable = 1 << 2,
		Sortable = 1 << 3,
		NoSavedSettings = 1 << 4,
		ContextMenuInBody = 1 << 5,
		RowBg = 1 << 6,
		BordersInnerH = 1 << 7,
		BordersOuterH = 1 << 8,
		BordersInnerV = 1 << 9,
		BordersOuterV = 1 << 10,
		BordersH = BordersInnerH | BordersOuterH,
		BordersV = BordersInnerV | BordersOuterV,
		BordersInner = BordersInnerH | BordersInnerV,
		BordersOuter = BordersOuterH | BordersOuterV,
		Border = BordersInner | BordersOuter,
		NoBordersInBody = 1 << 11,
		NoBordersInBodyUntilResize = 1 << 12,
		SizingFixedFit = 1 << 13,
		SizingFixedSame = 2 << 13,
		SizingStrechProp = 3 << 13,
		SizingStrechSame = 4 << 13,
		NoHostExtendX = 1 << 16,
		NoHostExtendY = 1 << 17,
		NoKeepColumnsVisible = 1 << 18,
		PreciseWidths = 1 << 19,
		NoClip = 1 << 20,
		PadOuterX = 1 << 21,
		NoPadOuterX = 1 << 22,
		NoPadInnerX = 1 << 23,
		ScrollX = 1 << 24,
		ScrollY = 1 << 25,
		SortMulti = 1 << 26,
		SortTristate = 1 << 27,
		SizingMask = SizingFixedFit | SizingFixedSame | SizingStrechProp | SizingStrechSame
	}

	[Flags]
	public enum ImGuiTableColumnFlags : int {
		None = 0,
		Disabled = 1 << 0,
		DefaultHide = 1 << 1,
		DefaultSort = 1 << 2,
		WidthStrech = 1 << 3,
		WidthFixed = 1 << 4,
		NoResize = 1 << 5,
		NoReorder = 1 << 6,
		NoHide = 1 << 7,
		NoClip = 1 << 8,
		NoSort = 1 << 9,
		NoSortAscending = 1 << 10,
		NoSortDescending = 1 << 11,
		NoHeaderLabel = 1 << 12,
		NoHeaderWidth = 1 << 13,
		PreferSortAscending = 1 << 14,
		PreferSortDescending = 1 << 15,
		IndentEnable = 1 << 16,
		IndentDisable = 1 << 17,
		IsEnabled = 1 << 24,
		IsVisible = 1 << 25,
		IsSorted = 1 << 26,
		IsHovered = 1 << 27,
		WidthMask = WidthStrech | WidthFixed,
		IndentMask = IndentEnable | IndentDisable,
		StatusMask = IsEnabled | IsVisible | IsSorted | IsHovered,
		NoDirectResize = 1 << 30
	}

	[Flags]
	public enum ImGuiTableRowFlags : int {
		None = 0,
		Headers = 1 << 0
	}

	public enum ImGuiTableBgTarget : int {
		None = 0,
		RowBg0 = 1,
		RowBg1 = 2,
		CellBg = 3
	}

	[Flags]
	public enum ImGuiFocusedFlags : int {
		None = 0,
		ChildWindows = 1 << 0,
		RootWindow = 1 << 1,
		AnyWindow = 1 << 2,
		NoPopupHierarchy = 1 << 3,
		RootAndChildWindows = RootWindow | ChildWindows
	}

	[Flags]
	public enum ImGuiHoveredFlags : int {
		None = 0,
		ChildWindows = 1 << 0,
		RootWindow = 1 << 1,
		AnyWindow = 1 << 2,
		NoPopupHierarchy = 1 << 3,
		AllowWhenBlockedByPopup = 1 << 5,
		AllowWhenBlockedByActiveItem = 1 << 7,
		AllowWhenOverlapped = 1 << 8,
		AllowWhenDisabled = 1 << 9,
		RectOnly = AllowWhenBlockedByPopup | AllowWhenBlockedByActiveItem | AllowWhenOverlapped,
		RootAndChildWindows = RootWindow | ChildWindows
	}

	[Flags]
	public enum ImGuiDragDropFlags : int {
		None,
		SourceNoPreviewTooltip = 1 << 0,
		SourceNoDisableHover = 1 << 1,
		SourceNoHoldToOpenOthers = 1 << 2,
		SourceAllowNullID = 1 << 3,
		SourceExtern = 1 << 4,
		SourceAutoExpirePayload = 1 << 5,
		AcceptBeforeDelivery = 1 << 10,
		AcceptNoDrawDefaultRect = 1 << 11,
		AcceptNoPreviewTooltip = 1 << 12,
		AcceptPeekOnly = AcceptBeforeDelivery | AcceptNoDrawDefaultRect
	}

	public enum ImGuiDataType : int {
		S8,
		U8,
		S16,
		U16,
		S32,
		U32,
		S64,
		U64,
		Float,
		Double,
		COUNT
	}

	public enum ImGuiDir : int {
		None = -1,
		Left,
		Right,
		Up,
		Down
	}

	public enum ImGuiSortDirection : int {
		None,
		Ascending,
		Descending
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "Duplicate values used in source enum")]
	public enum ImGuiKey : int {
		None = 0,
		Tab = 512,
		LeftArrow,
		RightArrow,
		UpArrow,
		DownArrow,
		PageUp,
		PageDown,
		Home,
		End,
		Insert,
		Delete,
		Backspace,
		Space,
		Enter,
		Escape,
		LeftCtrl, LeftShift, LeftAlt, LeftSuper,
		RightCtrl, RightShift, RightAlt, RightSuper,
		Menu,
		_0, _1, _2, _3, _4, _5, _6, _7, _8, _9,
		A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
		F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,
		Apostrophe,
		Comma,
		Minus,
		Period,
		Slash,
		Semicolon,
		Equal,
		LeftBracket,
		Backslash,
		RightBracket,
		GraveAccent,
		CapsLock,
		ScrollLock,
		NumLock,
		PrintScreen,
		Pause,
		Keypad0, Keypad1, Keypad2, Keypad3, Keypad4, Keypad5, Keypad6, Keypad7, Keypad8, Keypad9,
		KeypadDecimal, KeypadDivide,
		KeypadMultiply, KeypadSubtract, KeypadAdd, KeypadEnter, KeypadEqual,

		GamepadStart,
		GamepadBack,
		GamepadFaceUp,
		GamepadFaceDown,
		GamepadFaceLeft,
		GamepadFaceRight,
		GamepadDpadUp,
		GamepadDpadDown,
		GamepadDpadLeft,
		GamepadDpadRight,
		GamepadL1,
		GamepadR1,
		GamepadL2,
		GamepadR2,
		GamepadL3,
		GamepadR3,
		GamepadLStickUp,
		GamepadLStickDown,
		GamepadLStickLeft,
		GamepadLStickRight,
		GamepadRStickUp,
		GamepadRStickDown,
		GamepadRStickLeft,
		GamepadRStickRight,

		ModCtrl,
		ModShift,
		ModAlt,
		ModSuper,

		Count,

		NamedKeyBegin = 512,
		NamedKeyEnd = Count,
		NamedKeyCount = NamedKeyEnd - NamedKeyBegin,

		KeysDataSize = Count
	}

	[Flags]
	public enum ImGuiKeyModFlags : int {
		None = 0,
		Ctrl = 1 << 0,
		Shift = 1 << 1,
		Alt = 1 << 2,
		Super = 1 << 3
	}

	public enum ImGuiNavInput : int {
		Activate,
		Cancel,
		Input,
		Menu,
		DpadLeft,
		DpadRight,
		DpadUp,
		DpadDown,
		LStickLeft,
		LStickRight,
		LStickUp,
		LStickDown,
		FocusPrev,
		FocusNext,
		TweakSlow,
		TweakFast,
		KeyLeft,
		KeyRight,
		KeyUp,
		KeyDown,

		Count
	}

	[Flags]
	public enum ImGuiConfigFlags : int {
		None = 0,
		NavEnableKeyboard = 1 << 0,
		NavEnableGamepad = 1 << 1,
		NavEnableSetMousePos = 1 << 2,
		NavNoCaptureKeyboard = 1 << 3,
		NoMouse = 1 << 4,
		NoMouseCursorChange = 1 << 5,
		IsSRGB = 1 << 20,
		IsTouchScreen = 1 << 21
	}

	[Flags]
	public enum ImGuiBackendFlags : int {
		None = 0,
		HasGamepad = 1 << 0,
		HasMouseCursors = 1 << 1,
		HasSetMousePos = 1 << 2,
		RendererHasVtxOffset = 1 << 3
	}

	public enum ImGuiCol : int {
		Text,
		TextDisabled,
		WindowBg,
		ChildBg,
		PopupBg,
		Border,
		BorderShadow,
		FrameBg,
		FrameBgHovered,
		FrameBgActive,
		TitleBg,
		TitleBgActive,
		TitleBgCollapsed,
		MenuBarBg,
		ScrollbarBg,
		ScrollbarBgGrab,
		ScrollbarBgGrabHovered,
		ScrollbarBgGrabActive,
		CheckMark,
		SliderGrab,
		SliderGrabActive,
		Button,
		ButtonHovered,
		ButtonActive,
		Header,
		HeaderHovered,
		HeaderActive,
		Separator,
		SeparatorHovered,
		SeparatorActive,
		ResizeGrip,
		ResizeGripHovered,
		ResizeGripActive,
		Tab,
		TabHovered,
		TabActive,
		TabUnfocused,
		TabUnfocusedActive,
		PlotLines,
		PlotLinesHovered,
		PlotHistogram,
		PlotHistogramHovered,
		TableHeaderBg,
		TableBorderStrong,
		TableBorderLight,
		TableRowBg,
		TableRowBgAlt,
		TextSelectedBg,
		DragDropTarget,
		NavHighlight,
		NavWindowHighlight,
		NavWindowDimBg,
		ModalWindowDimBg,

		Count
	}

	public enum ImGuiStyleVar : int {
		Alpha,
		DisabledAlpha,
		WindowPadding,
		WindowRounding,
		WindowBorderSize,
		WindowMinSize,
		WindowTitleAlign,
		ChildRounding,
		ChildBorderSize,
		PopupRounding,
		PopupBorderSize,
		FramePadding,
		FrameRounding,
		FrameBorderSize,
		ItemSpacing,
		ItemInnerSpacing,
		IndentSpacing,
		CellPadding,
		ScrollbarSize,
		ScrollbarRounding,
		GrabMinSize,
		GrabRounding,
		TabRounding,
		ButtonTextAlign,
		SelectableTextAlign
	}

	[Flags]
	public enum ImGuiButtonFlags : int {
		None = 0,
		MouseButtonLeft = 1 << 0,
		MouseButtonRight = 1 << 1,
		MouseButtonMiddle = 1 << 2,
		MouseButtonMask = MouseButtonLeft | MouseButtonRight | MouseButtonMiddle,
		MouseButtonDefault = MouseButtonLeft
	}

	[Flags]
	public enum ImGuiColorEditFlags : int {
		None,
		NoAlpha = 1 << 1,
		NoPicker = 1 << 2,
		NoOptions = 1 << 3,
		NoSmallPreview = 1 << 4,
		NoInputs = 1 << 5,
		NoTooltip = 1 << 6,
		NoLabel = 1 << 7,
		NoSidePreview = 1 << 8,
		NoDragDrop = 1 << 9,
		NoBorder = 1 << 10,
		AlphaBar = 1 << 16,
		AlphaPreview = 1 << 17,
		AlphaPreviewHalf = 1 << 18,
		HDR = 1 << 19,
		DisplayRGB = 1 << 20,
		DisplayHSV = 1 << 21,
		DisplayHex = 1 << 22,
		UInt8 = 1 << 23,
		Float = 1 << 24,
		PickerHueBar = 1 << 25,
		PickerHueWheel = 1 << 26,
		InputRGB = 1 << 27,
		InputHSV = 1 << 28,
		DefaultOptions = UInt8 | DisplayRGB | InputRGB | PickerHueBar,
		DisplayMask = DisplayRGB | DisplayHSV | DisplayHex,
		DataTypeMask = UInt8 | Float,
		PickerMask = PickerHueWheel | PickerHueBar,
		InputMask = InputRGB | InputHSV
	}

	[Flags]
	public enum ImGuiSliderFlags : int {
		None = 0,
		AlwaysClamp = 1 << 4,
		Logarithmic = 1 << 5,
		NoRoundToFormat = 1 << 6,
		NoInput = 1 << 7
	}

	public enum ImGuiMouseButton : int {
		Left = 0,
		Right = 1,
		Middle = 2
	}

	public enum ImGuiMouseCursor : int {
		None = -1,
		Arrow = 0,
		TextInput,
		ResizeAll,
		ResizeNS,
		ResizeEW,
		ResizeNESW,
		ResizeNWSE,
		Hand,
		NotAllowed,

		Count
	}

	[Flags]
	public enum ImGuiCond : int {
		None = 0,
		Always = 1 << 0,
		Once = 1 << 1,
		FirstUseEver = 1 << 2,
		Appearing = 1 << 3
	}

	[Flags]
	public enum ImDrawFlags : int {
		None = 0,
		Closed = 1 << 0,
		RoundCornersTopLeft = 1 << 4,
		RoundCornersTopRight = 1 << 5,
		RoundCornersBottomLeft = 1 << 6,
		RoundCornersBottomRight = 1 << 7,
		RoundCornersNone = 1 << 8,
		RoundCornersTop = RoundCornersTopLeft | RoundCornersTopRight,
		RoundCornersBottom = RoundCornersBottomLeft | RoundCornersBottomRight,
		RoundCornersLeft = RoundCornersTopLeft | RoundCornersBottomLeft,
		RoundCornersRight = RoundCornersTopRight | RoundCornersBottomRight,
		RoundCornersAll = RoundCornersBottomLeft | RoundCornersBottomRight | RoundCornersTopLeft | RoundCornersTopRight,
		RoundCornersDefault = RoundCornersAll,
		RoundCornersMask = RoundCornersAll | RoundCornersNone
	}

	[Flags]
	public enum ImDrawListFlags : int {
		None = 0,
		AntiAliasedLines = 1 << 0,
		AntiAliasedLinesUseTex = 1 << 1,
		AntiAliasedFill = 1 << 2,
		AllowVtxOffset = 1 << 3
	}

	[Flags]
	public enum ImFontAtlasFlags : int {
		None = 0,
		NoPowerOfTwoHeight = 1 << 0,
		NoMouseCursors = 1 << 1,
		NoBakedLines = 1 << 2
	}

	[Flags]
	public enum ImGuiViewportFlags : int {
		None = 0,
		IsPlatformWindow = 1 << 0,
		IsPlatformMonitor = 1 << 1,
		OwnedByApp = 1 << 2
	}

	[Flags]
	public enum ImGuiChildFlags {
		None = 0,
		Border = 1,
		AlwaysUseWindowPadding = 2,
		ResizeX = 4,
		ResizeY = 8,
		AutoResizeX = 0x10,
		AutoResizeY = 0x20,
		AlwaysAutoResize = 0x40,
		FrameStyle = 0x80
	}

}

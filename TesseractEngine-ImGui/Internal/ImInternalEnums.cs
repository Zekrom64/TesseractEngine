using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.Internal {
	
	[Flags]
	internal enum ImGuiItemFlags {
		None = 0,
		NoTabStop = 1 << 0,
		ButtonRepeat = 1 << 1,
		Disabled = 1 << 2,
		NoNav = 1 << 3,
		NoNavDefaultFocus = 1 << 4,
		SelectableDontClosePopup = 1 << 5,
		MixedValue = 1 << 6,
		ReadOnly = 1 << 7,
		Inputable = 1 << 8
	}

	[Flags]
	internal enum ImGuiItemStatusFlags {
		None = 0,
		HoveredRect = 1 << 0,
		HasDisplayRect = 1 << 1,
		Edited = 1 << 2,
		ToggledSelection = 1 << 3,
		ToggledOpen = 1 << 4,
		HasDeactivated = 1 << 5,
		Deactivated = 1 << 6,
		HoveredWindow = 1 << 7,
		FocusedByTabbing = 1 << 8
	}

	[Flags]
	internal enum ImGuiInputTextFlagsPrivate {
		Multiline = 1 << 26,
		NoMarkEdited = 1 << 27,
		MergedItem = 1 << 28
	}

	[Flags]
	internal enum ImGuiButtonFlagsPrivate {
		PressedOnClick = 1 << 4,
		PressedOnClickRelease = 1 << 5,
		PressedOnClickReleaseAnywhere = 1 << 6,
		PressedOnRelease = 1 << 7,
		PressedOnDoubleClick = 1 << 8,
		PressedOnDragDropHold = 1 << 9,
		Repeat = 1 << 10,
		FlattenChildren = 1 << 11,
		AllowItemOverlap = 1 << 12,
		DontClosePopups = 1 << 13,
		AlignTextBaseLine = 1 << 15,
		NoKeyModifiers = 1 << 16,
		NoHoldingActiveId = 1 << 17,
		NoNavFocus = 1 << 18,
		NoHoveredOnFocus = 1 << 19,
		PressedOnMask = PressedOnClick | PressedOnClickRelease | PressedOnClickReleaseAnywhere | PressedOnDoubleClick | PressedOnDragDropHold,
		PressedOnDefault = PressedOnRelease
	}

	[Flags]
	internal enum ImGuiComboFlagsPrivate {
		CustomPreview = 1 << 20
	}

	[Flags]
	internal enum ImGuiSliderFlagsPrivate {
		Vertical = 1 << 20,
		ReadOnly = 1 << 21
	}
	
	[Flags]
	internal enum ImGuiSelectableFlagsPrivate {
		NoHoldingActiveID = 1 << 20,
		SelectOnNav = 1 << 21,
		SelectOnClick = 1 << 22,
		SelectOnRelease = 1 << 23,
		SpanAvailWidth = 1 << 24,
		DragHoveredWhenHeld = 1 << 25,
		SetNavIdOnHover = 1 << 26,
		NoPadWithHalfSpacing = 1 << 27
	}

	[Flags]
	internal enum ImGuiTreeNodeFlagsPrivate {
		ClipLabelForTrailingButton = 1 << 20
	}

	[Flags]
	internal enum ImGuiSeparatorFlags {
		None,
		Horizontal = 1 << 0,
		Vertical = 1 << 1,
		SpanAllColumns = 1 << 2
	}

	[Flags]
	internal enum ImGuiTextFlags {
		None = 0,
		NoWidthForLargeClippedText = 1 << 0
	}

	[Flags]
	internal enum ImGuiTooltipFlags {
		None = 0,
		OverridePreviousTooltip = 1 << 0
	}

	internal enum ImGuiLayoutType {
		Horizontal = 0,
		Vertical = 1
	}

	internal enum ImGuiLogType {
		None = 0,
		TTY,
		File,
		Buffer,
		Clipboard
	}

	internal enum ImGuiAxis {
		None = -1,
		X = 0,
		Y = 1
	}

	internal enum ImGuiPlotType {
		Lines,
		Histogram
	}

	internal enum ImGuiPopupPositionPolicy {
		Default,
		ComboBox,
		Tooltip
	}

	internal enum ImGuiDataTypePrivate {
		String = ImGuiDataType.COUNT + 1,
		Pointer,
		ID
	}

	[Flags]
	internal enum ImGuiNextWindowDataFlags {
		None = 0,
		HasPos = 1 << 0,
		HasSize = 1 << 1,
		HasContentSize = 1 << 2,
		HasCollapsed = 1 << 3,
		HasSizeConstraint = 1 << 4,
		HasFocus = 1 << 5,
		HasBgAlpha = 1 << 6,
		HasScroll = 1 << 7
	}

	[Flags]
	internal enum ImGuiNextItemDataFlags {
		None = 0,
		HasWidth = 1 << 0,
		HasOpen = 1 << 1
	}

	internal enum ImGuiInputEventType {
		None,
		MousePos,
		MouseWheel,
		MouseButton,
		Key,
		Text,
		Focus
	}

	internal enum ImGuiInputSource {
		None,
		Mouse,
		Keyboard,
		Gamepad,
		Clipboard,
		Nav
	}

}

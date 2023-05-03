using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.ImGui.NET {

	using IM = ImGuiNET.ImGui;

	public class ImGuiNETCore : IImGui {

		internal static readonly Dictionary<Type, ImGuiNET.ImGuiDataType> typeDataTypes = new() {
			{ typeof(byte), ImGuiNET.ImGuiDataType.U8 },
			{ typeof(ushort), ImGuiNET.ImGuiDataType.U16 },
			{ typeof(uint), ImGuiNET.ImGuiDataType.U32 },
			{ typeof(ulong), ImGuiNET.ImGuiDataType.U64 },
			{ typeof(sbyte), ImGuiNET.ImGuiDataType.S8 },
			{ typeof(short), ImGuiNET.ImGuiDataType.S16 },
			{ typeof(int), ImGuiNET.ImGuiDataType.S32 },
			{ typeof(long), ImGuiNET.ImGuiDataType.S64 },
			{ typeof(float), ImGuiNET.ImGuiDataType.Float },
			{ typeof(double), ImGuiNET.ImGuiDataType.Double }
		};

		internal static ImGuiNET.ImGuiDataType GetDataType<T>() {
			if (typeDataTypes.TryGetValue(typeof(T), out ImGuiNET.ImGuiDataType type)) return type;
			else throw new ArgumentException("Unsupported data type");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ReadOnlySpan<byte> CheckStr(ReadOnlySpan<byte> str, ReadOnlySpan<byte> defaultVal = default) {
			if (str.IsEmpty) return defaultVal;
#if DEBUG
			// Check if the last byte (or just beyond) is null
			if (str[^1] == 0) return str;
			unsafe {
				fixed(byte* pStr = str) {
					if (pStr[str.Length] == 0) return str;
				}
			}
			// Check if any bytes are null
			if (str.Contains((byte)0)) return str;
			// Otherwise, this is not a valid string!
			throw new ArgumentException("String must be null-terminated!");
#else
			// Skip checking in release mode
			return str;
#endif
		}

		/// <summary>
		/// Escapes the formatting '%' character used by C routines such as <c>printf</c> in a UTF-8 or ASCII string.
		/// </summary>
		/// <param name="str">String to escape</param>
		/// <returns>Escaped string</returns>
		internal static ReadOnlySpan<byte> CheckFmtStr(ReadOnlySpan<byte> str, ReadOnlySpan<byte> defaultVal = default) {
			if (str.IsEmpty) return defaultVal;
#if DEBUG
			str = CheckStr(str, defaultVal);
#endif

			int nescape = 0;
			foreach (byte c in str) {
				if (c == '%') nescape++;
			}

			if (nescape > 0) {
				Span<byte> dst = new byte[str.Length + nescape + 1];
				dst[^1] = 0;
				for (int i = 0, j = 0; i < str.Length; i++, j++) {
					byte c = str[i];
					dst[j] = c;
					if (c == '%') dst[++j] = (byte)'%';
				}
				str = dst;
			}

			return str;
		}

		private static IntPtr EnumerateStrings(MemoryStack sp, IEnumerable<string> items, out int itemsCount) {
			// Keep two lists to store string pointers
			Span<IntPtr> shortList = stackalloc IntPtr[256];
			List<IntPtr>? longList = null;

			// For each item
			itemsCount = 0;
			foreach (string item in items) {
				// Try to append to the short list first
				if (itemsCount < shortList.Length) shortList[itemsCount++] = sp.UTF8(item);
				else {
					// Else append to the long list
					if (longList == null) {
						// Initialize the long list using the contents of the short list
						longList = new List<nint>(shortList.Length * 2);
						foreach (IntPtr ptr in shortList) longList.Add(ptr);
					}
					longList.Add(sp.UTF8(item));
					itemsCount++;
				}
			}

			// Use the short or long list
			return longList != null ? sp.Values(longList) : sp.Values((ReadOnlySpan<IntPtr>)shortList);
		}


		private ImGuiNETContext? currentContext = null;

		public IImGuiContext CurrentContext {
			get {
				nint ctx = IM.GetCurrentContext();
				if (currentContext != null && currentContext.Context == ctx) return currentContext;
				currentContext = new ImGuiNETContext(ctx);
				return currentContext;
			}
			set {
				currentContext = (ImGuiNETContext)value;
				IM.SetCurrentContext(currentContext.Context);
			}
		}

		public IImGuiIO IO {
			get {
				var ctx = ((ImGuiNETContext)CurrentContext);
				ctx.IO ??= new ImGuiNETIO(IM.GetIO());
				return ctx.IO;
			}
		}

		public IImGuiStyle Style {
			get => new ImGuiNETStyle(IM.GetStyle());
			set {
				unsafe {
					*IM.GetStyle().NativePtr = *((ImGuiNETStyle)value).style.NativePtr;
				}
			}
		}

		public string Version => IM.GetVersion();

		public bool IsWindowAppearing => IM.IsWindowAppearing();

		public bool IsWindowCollapsed => IM.IsWindowCollapsed();

		public Vector2 WindowPos => IM.GetWindowPos();

		public Vector2 WindowSize => IM.GetWindowSize();

		public float WindowWidth => IM.GetWindowWidth();

		public float WindowHeight => IM.GetWindowHeight();

		public Vector2 ContentRegionAvail => IM.GetContentRegionAvail();

		public Vector2 ContentRegionMax => IM.GetContentRegionMax();

		public Vector2 WindowContentRegionMax => IM.GetWindowContentRegionMax();

		public Vector2 WindowContentRegionMin => IM.GetWindowContentRegionMin();

		public float ScrollX { get => IM.GetScrollX(); set => IM.SetScrollX(value); }
		public float ScrollY { get => IM.GetScrollY(); set => IM.SetScrollY(value); }

		public float ScrollMaxX => IM.GetScrollMaxX();

		public float ScrollMaxY => IM.GetScrollMaxY();

		public IImFont Font => ((ImGuiNETFontAtlas)IO.Fonts).GetFont(IM.GetFont());

		public float FontSize => IM.GetFontSize();

		public Vector2 FontTexUvWhitePixel => IM.GetFontTexUvWhitePixel();

		public Vector2 CursorPos { get => IM.GetCursorPos(); set => IM.SetCursorPos(value); }
		public float CursorPosX { get => IM.GetCursorPosX(); set => IM.SetCursorPosX(value); }
		public float CursorPosY { get => IM.GetCursorPosY(); set => IM.SetCursorPosY(value); }

		public Vector2 CursorStartPos => IM.GetCursorStartPos();

		public Vector2 CursorScreenPos { get => IM.GetCursorScreenPos(); set => IM.SetCursorScreenPos(value); }

		public float TextLineHeight => IM.GetTextLineHeight();

		public float TextLineHeightWithSpacing => IM.GetTextLineHeightWithSpacing();

		public float FrameHeight => IM.GetFrameHeight();

		public float FrameHeightWithSpacing => IM.GetFrameHeightWithSpacing();

		public float TreeNodeToLabelSpacing => IM.GetTreeNodeToLabelSpacing();

		public IImGuiTableSortSpecs? TableSortSpecs {
			get {
				var ptr = IM.TableGetSortSpecs();
				unsafe {
					if (ptr.NativePtr == null) return null;
				}
				return new ImGuiNETTableSortSpecs(ptr);
			}
		}

		public int TableColumnCount => IM.TableGetColumnCount();

		public int TableColumnIndex => IM.TableGetColumnIndex();

		public int TableRowIndex => IM.TableGetRowIndex();

		public int ColumnIndex => IM.GetColumnIndex();

		public int ColumnsCount => IM.GetColumnsCount();

		public IImGuiPayload? DragDropPayload {
			get {
				var ptr = IM.GetDragDropPayload();
				unsafe {
					if (ptr.NativePtr == null) return null;
				}
				return new ImGuiNETPayload(ptr);
			}
		}

		public bool IsItemActive => IM.IsItemActive();

		public bool IsItemFocused => IM.IsItemFocused();

		public bool IsItemVisible => IM.IsItemVisible();

		public bool IsItemEdited => IM.IsItemEdited();

		public bool IsItemActivated => IM.IsItemActivated();

		public bool IsItemDeactivated => IM.IsItemDeactivated();

		public bool IsItemDeactivatedAfterEdit => IM.IsItemDeactivatedAfterEdit();

		public bool IsItemToggledOpen => IM.IsItemToggledOpen();

		public bool IsAnyItemHovered => IM.IsAnyItemHovered();

		public bool IsAnyItemActive => IM.IsAnyItemActive();

		public bool IsAnyItemFocused => IM.IsAnyItemFocused();

		public Vector2 ItemRectMin => IM.GetItemRectMin();

		public Vector2 ItemRectMax => IM.GetItemRectMax();

		public Vector2 ItemRectSize => IM.GetItemRectSize();

		public ImGuiViewport MainViewport {
			get {
				unsafe {
					return *((ImGuiViewport*)ImGuiNative.igGetMainViewport());
				}
			}
			set {
				unsafe {
					*((ImGuiViewport*)ImGuiNative.igGetMainViewport()) = value;
				}
			}
		}

		public double Time => IM.GetTime();

		public int FrameCount => IM.GetFrameCount();

		public IImDrawList BackgroundDrawList => ImGuiNETDrawList.Get(IM.GetBackgroundDrawList());

		public IImDrawList ForegroundDrawList => ImGuiNETDrawList.Get(IM.GetForegroundDrawList());

		public IImDrawListSharedData DrawListSharedData => throw new NotImplementedException();

		private ImGuiNETStorage? stateStorage = null;

		public IImGuiStorage StateStorage {
			get {
				var ptr = IM.GetStateStorage();
				unsafe {
					if (stateStorage != null && ptr.NativePtr == stateStorage.storage.NativePtr) return stateStorage;
				}
				stateStorage = new ImGuiNETStorage(ptr);
				return stateStorage;
			}
			set {
				stateStorage = (ImGuiNETStorage)value;
				IM.SetStateStorage(stateStorage.storage);
			}
		}

		public bool IsAnyMouseDown => IM.IsAnyMouseDown();

		public Vector2 MousePos => IM.GetMousePos();

		public Vector2 MousePosOnOpeningCurrentPopup => IM.GetMousePosOnOpeningCurrentPopup();

		public ImGuiMouseCursor MouseCursor {
			get => (ImGuiMouseCursor)IM.GetMouseCursor();
			set => IM.SetMouseCursor((ImGuiNET.ImGuiMouseCursor)value);
		}

		public string ClipboardText { get => IM.GetClipboardText(); set => IM.SetClipboardText(value); }

		public IImGuiPayload? AcceptDragDropPayload(string type, ImGuiDragDropFlags flags = ImGuiDragDropFlags.None) {
			var ptr = IM.AcceptDragDropPayload(type, (ImGuiNET.ImGuiDragDropFlags)flags);
			unsafe {
				if (ptr.NativePtr == null) return null;
			}
			return new ImGuiNETPayload(ptr);
		}

		public void AlignTextToFramePadding() => IM.AlignTextToFramePadding();

		public bool ArrowButton(ReadOnlySpan<byte> strID, ImGuiDir dir) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igArrowButton(pStrID, (ImGuiNET.ImGuiDir)dir) != 0;
				}
			}
		}

		public bool Begin(ReadOnlySpan<byte> name, ref bool open, ImGuiWindowFlags flags = ImGuiWindowFlags.None) {
			unsafe {
				fixed(byte* pName = CheckStr(name)) {
					byte bopen = (byte)(open ? 1 : 0);
					byte bret = ImGuiNative.igBegin(pName, &bopen, (ImGuiNET.ImGuiWindowFlags)flags);
					open = bopen != 0;
					return bret != 0;
				}
			}
		}

		public void BeginChild(ReadOnlySpan<byte> strId, Vector2 size = default, bool border = false, ImGuiWindowFlags flags = ImGuiWindowFlags.None) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strId)) {
					ImGuiNative.igBeginChild_Str(pStrID, size, (byte)(border ? 1 : 0), (ImGuiNET.ImGuiWindowFlags)flags);
				}
			}
		}

		public void BeginChild(uint id, Vector2 size = default, bool border = false, ImGuiWindowFlags flags = ImGuiWindowFlags.None) => IM.BeginChild(id, size, border, (ImGuiNET.ImGuiWindowFlags)flags);

		public bool BeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags = ImGuiWindowFlags.None) => IM.BeginChildFrame(id, size, (ImGuiNET.ImGuiWindowFlags)flags);

		public bool BeginCombo(ReadOnlySpan<byte> label, ReadOnlySpan<byte> previewValue, ImGuiComboFlags flags = ImGuiComboFlags.None) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label), pPreviewValue = CheckStr(previewValue)) {
					return ImGuiNative.igBeginCombo(pLabel, pPreviewValue, (ImGuiNET.ImGuiComboFlags)flags) != 0;
				}
			}
		}

		public void BeginDisabled(bool disabled = true) => IM.BeginDisabled(disabled);

		public bool BeginDragDropSource(ImGuiDragDropFlags flags = ImGuiDragDropFlags.None) => IM.BeginDragDropSource((ImGuiNET.ImGuiDragDropFlags)flags);

		public bool BeginDragDropTarget() => IM.BeginDragDropTarget();

		public void BeginGroup() => IM.BeginGroup();

		public bool BeginListBox(ReadOnlySpan<byte> label, Vector2 size = default) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					return ImGuiNative.igBeginListBox(pLabel, size) != 0;
				}
			}
		}

		public bool BeginMainMenuBar() => IM.BeginMainMenuBar();

		public bool BeginMenu(ReadOnlySpan<byte> label, bool enabled = true) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					return ImGuiNative.igBeginMenu(pLabel, (byte)(enabled ? 1 : 0)) != 0;
				}
			}
		}

		public bool BeginMenuBar() => IM.BeginMenuBar();

		public bool BeginPopup(ReadOnlySpan<byte> strID, ImGuiWindowFlags flags = ImGuiWindowFlags.None) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igBeginPopup(pStrID, (ImGuiNET.ImGuiWindowFlags)flags) != 0;
				}
			}
		}

		public bool BeginPopupContextItem(ReadOnlySpan<byte> strID = default, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonDefault) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igBeginPopupContextItem(pStrID, (ImGuiNET.ImGuiPopupFlags)popupFlags) != 0;
				}
			}
		}

		public bool BeginPopupContextVoid(ReadOnlySpan<byte> strID = default, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonDefault) {
			unsafe {
				fixed (byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igBeginPopupContextVoid(pStrID, (ImGuiNET.ImGuiPopupFlags)popupFlags) != 0;
				}
			}
		}

		public bool BeginPopupContextWindow(ReadOnlySpan<byte> strID = default, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonDefault) {
			unsafe {
				fixed (byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igBeginPopupContextWindow(pStrID, (ImGuiNET.ImGuiPopupFlags)popupFlags) != 0;
				}
			}
		}

		public bool BeginPopupModal(ReadOnlySpan<byte> name, ref bool pOpen, ImGuiWindowFlags flags = ImGuiWindowFlags.None) {
			unsafe {
				fixed(byte* pName = CheckStr(name)) {
					byte bopen = (byte)(pOpen ? 1 : 0);
					byte bret = ImGuiNative.igBeginPopupModal(pName, &bopen, (ImGuiNET.ImGuiWindowFlags)flags);
					pOpen = bopen != 0;
					return bret != 0;
				}
			}
		}

		public bool BeginTabBar(ReadOnlySpan<byte> strID, ImGuiTabBarFlags flags = ImGuiTabBarFlags.None) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igBeginTabBar(pStrID, (ImGuiNET.ImGuiTabBarFlags)flags) != 0;
				}
			}
		}

		public bool BeginTabItem(ReadOnlySpan<byte> label, ref bool open, ImGuiTabItemFlags flags = ImGuiTabItemFlags.None) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					byte bopen = (byte)(open ? 1 : 0);
					byte bret = ImGuiNative.igBeginTabItem(pLabel, &bopen, (ImGuiNET.ImGuiTabItemFlags)flags);
					open = bopen != 0;
					return bret != 0;
				}
			}
		}

		public bool BeginTable(ReadOnlySpan<byte> strID, int column, ImGuiTableFlags flags = ImGuiTableFlags.None, Vector2 outerSize = default, float innerWidth = 0) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igBeginTable(pStrID, column, (ImGuiNET.ImGuiTableFlags)flags, outerSize, innerWidth) != 0;
				}
			}
		}

		public void BeginTooltip() => IM.BeginTooltip();

		public void Bullet() => IM.Bullet();

		public void BulletText(ReadOnlySpan<byte> fmt) {
			unsafe {
				fixed(byte* pFmt = CheckFmtStr(fmt)) {
					ImGuiNative.igBulletText(pFmt);
				}
			}
		}

		public bool Button(ReadOnlySpan<byte> label, Vector2 size = default) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					return ImGuiNative.igButton(pLabel, size) != 0;
				}
			}
		}

		public float CalcItemWidth() => IM.CalcItemWidth();

		public Vector2 CalcTextSize(ReadOnlySpan<byte> text, bool hideTextAfterDoubleHash = false, float wrapWidth = -1) {
			unsafe {
				fixed(byte* pText = text) {
					Vector2 ret = Vector2.Zero;
					ImGuiNative.igCalcTextSize(&ret, pText, pText + text.Length, (byte)(hideTextAfterDoubleHash ? 1 : 0), wrapWidth);
					return ret;
				}
			}
		}

		public void CaptureKeyboardFromApp(bool wantCaptureKeyboardValue = true) => IM.SetNextFrameWantCaptureKeyboard(wantCaptureKeyboardValue);

		public void CaptureMouseFromApp(bool wantCaptureMouseValue = true) => IM.SetNextFrameWantCaptureMouse(wantCaptureMouseValue);

		public bool Checkbox(ReadOnlySpan<byte> label, ref bool v) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					byte bv = (byte)(v ? 1 : 0);
					byte bret = ImGuiNative.igCheckbox(pLabel, &bv);
					v = bv != 0;
					return bret != 0;
				}
			}
		}

		public bool CheckboxFlags(ReadOnlySpan<byte> label, ref int flags, int flagsValue) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					fixed (int* pFlags = &flags) {
						return ImGuiNative.igCheckboxFlags_IntPtr(pLabel, pFlags, flagsValue) != 0;
					}
				}
			}
		}

		public bool CheckboxFlags(ReadOnlySpan<byte> label, ref uint flags, uint flagsValue) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (uint* pFlags = &flags) {
						return ImGuiNative.igCheckboxFlags_UintPtr(pLabel, pFlags, flagsValue) != 0;
					}
				}
			}
		}

		public void CloseCurrentPopup() => IM.CloseCurrentPopup();

		public bool CollapsingHeader(ReadOnlySpan<byte> label, ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					return ImGuiNative.igCollapsingHeader_TreeNodeFlags(pLabel, (ImGuiNET.ImGuiTreeNodeFlags)flags) != 0;
				}
			}
		}

		public bool CollapsingHeader(ReadOnlySpan<byte> label, ref bool pVisible, ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					byte bvisible = (byte)(pVisible ? 1 : 0);
					byte bret = ImGuiNative.igCollapsingHeader_BoolPtr(pLabel, &bvisible, (ImGuiNET.ImGuiTreeNodeFlags)flags);
					pVisible = bvisible!= 0;
					return bret != 0;
				}
			}
		}

		public bool ColorButton(ReadOnlySpan<byte> descId, Vector4 col, ImGuiColorEditFlags flags = ImGuiColorEditFlags.None, Vector2 size = default) {
			unsafe {
				fixed(byte* pDescID = CheckStr(descId)) {
					return ImGuiNative.igColorButton(pDescID, col, (ImGuiNET.ImGuiColorEditFlags)flags, size) != 0;
				}
			}
		}

		public bool ColorEdit3(ReadOnlySpan<byte> label, ref Vector3 col, ImGuiColorEditFlags flags = ImGuiColorEditFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (Vector3* pCol = &col) {
						return ImGuiNative.igColorEdit3(pLabel, pCol, (ImGuiNET.ImGuiColorEditFlags)flags) != 0;
					}
				}
			}
		}

		public bool ColorEdit4(ReadOnlySpan<byte> label, ref Vector4 col, ImGuiColorEditFlags flags = ImGuiColorEditFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (Vector4* pCol = &col) {
						return ImGuiNative.igColorEdit4(pLabel, pCol, (ImGuiNET.ImGuiColorEditFlags)flags) != 0;
					}
				}
			}
		}

		public bool ColorPicker3(ReadOnlySpan<byte> label, ref Vector3 col, ImGuiColorEditFlags flags = ImGuiColorEditFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (Vector3* pCol = &col) {
						return ImGuiNative.igColorPicker3(pLabel, pCol, (ImGuiNET.ImGuiColorEditFlags)flags) != 0;
					}
				}
			}
		}

		public bool ColorPicker4(ReadOnlySpan<byte> label, ref Vector4 col, ImGuiColorEditFlags flags = ImGuiColorEditFlags.None, Vector4? refCol = null) {
			Vector4 refColv = refCol ?? default;
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (Vector4* pCol = &col) {
						return ImGuiNative.igColorPicker4(pLabel, pCol, (ImGuiNET.ImGuiColorEditFlags)flags, refCol.HasValue ? (float*)(&refColv) : null) != 0;
					}
				}
			}
		}

		public void Columns(int count = 1, ReadOnlySpan<byte> id = default, bool border = true) {
			unsafe {
				fixed(byte* pID = CheckStr(id)) {
					ImGuiNative.igColumns(count, id.IsEmpty ? null : pID, (byte)(border ? 1 : 0));
				}
			}
		}

		public bool Combo(ReadOnlySpan<byte> label, ref int currentItem, IEnumerable<string> items, int popupMaxHeightInItems = -1) {
			using MemoryStack sp = MemoryStack.Push();
			IntPtr pItems;
			int itemsCount;

			if (items is IReadOnlyCollection<string> collection) {
				// If its a collection its really straightforward to allocate
				pItems = sp.UTF8Array(collection);
				itemsCount = collection.Count;
			} else {
				// Else we need to enumerate the items one by one
				pItems = EnumerateStrings(sp, items, out itemsCount);
			}

			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (int* pCurrentItem = &currentItem) {
						return ImGuiNative.igCombo_Str_arr(pLabel, pCurrentItem, (byte**)pItems, itemsCount, popupMaxHeightInItems) != 0;
					}
				}
			}
		}

		public bool Combo(ReadOnlySpan<byte> label, ref int currentItem, ReadOnlySpan<byte> itemsSeparatedByZeros, int popupMaxHeightInItems = -1) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label), pItemsSeparatedByZeros = CheckStr(itemsSeparatedByZeros)) {
					fixed(int* pCurrentItem = &currentItem) {
						return ImGuiNative.igCombo_Str(pLabel, pCurrentItem, pItemsSeparatedByZeros, popupMaxHeightInItems) != 0;
					}
				}
			}
		}

		public bool Combo(ReadOnlySpan<byte> label, ref int currentItem, IImGui.ComboItemsGetter itemsGetter, int itemscount, int popupMaxHeightInItems = -1) {
			using MemoryStack sp = MemoryStack.Push();
			Span<IntPtr> items = stackalloc IntPtr[itemscount];
			for(int i = 0; i < itemscount; i++) {
				if (!itemsGetter(i, out string text)) text = "Unknown item";
				items[i] = sp.UTF8(text);
			}

			unsafe {
				fixed(byte* pLabel = label) {
					fixed (IntPtr* pItems = items) {
						fixed (int* pCurrentItem = &currentItem) {
							return ImGuiNative.igCombo_Str_arr(pLabel, pCurrentItem, (byte**)pItems, itemscount, popupMaxHeightInItems) != 0;
						}
					}
				}
			}
		}

		public IImGuiContext CreateContext(IImFontAtlas? sharedFontAtlas = null) => new ImGuiNETContext(IM.CreateContext(((ImGuiNETFontAtlas?)sharedFontAtlas)?.fontAtlas ?? default));

		public void DestroyContext(IImGuiContext? ctx = null) {
			if (ctx == null) IM.DestroyContext();
			else IM.DestroyContext(((ImGuiNETContext)ctx).Context);
		}

		public bool DragFloat(ReadOnlySpan<byte> label, ref float v, float vSpeed = 1, float vMin = 0, float vMax = 0, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (float* pV = &v) {
						return ImGuiNative.igDragFloat(pLabel, pV, vSpeed, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool DragFloat2(ReadOnlySpan<byte> label, ref Vector2 v, float vSpeed = 1, float vMin = 0, float vMax = 0, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (Vector2* pV = &v) {
						return ImGuiNative.igDragFloat2(pLabel, pV, vSpeed, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool DragFloat3(ReadOnlySpan<byte> label, ref Vector3 v, float vSpeed = 1, float vMin = 0, float vMax = 0, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (Vector3* pV = &v) {
						return ImGuiNative.igDragFloat3(pLabel, pV, vSpeed, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool DragFloat4(ReadOnlySpan<byte> label, ref Vector4 v, float vSpeed = 1, float vMin = 0, float vMax = 0, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (Vector4* pV = &v) {
						return ImGuiNative.igDragFloat4(pLabel, pV, vSpeed, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool DragFloatRange2(ReadOnlySpan<byte> label, ref float vCurrentMin, ref float vCurrentMax, float vSpeed = 1, float vMin = 0, float vMax = 0, ReadOnlySpan<byte> format = default, ReadOnlySpan<byte> formatMax = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8), pFormatMax = CheckStr(format)) {
					fixed(float* pCurrentMin = &vCurrentMin, pCurrentMax = &vCurrentMax) {
						return ImGuiNative.igDragFloatRange2(pLabel, pCurrentMin, pCurrentMax, vSpeed, vMin, vMax, pFormat, formatMax.IsEmpty ? null : pFormatMax, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool DragInt(ReadOnlySpan<byte> label, ref int v, float vSpeed = 1, int vMin = 0, int vMax = 0, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%d"u8)) {
					fixed (int* pV = &v) {
						return ImGuiNative.igDragInt(pLabel, pV, vSpeed, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool DragInt2(ReadOnlySpan<byte> label, Span<int> v, float vSpeed = 1, int vMin = 0, int vMax = 0, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			if (v.Length < 2) throw new ArgumentException("Span must have at least 2 elements", nameof(v));
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%d"u8)) {
					fixed (int* pV = v) {
						return ImGuiNative.igDragInt2(pLabel, pV, vSpeed, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool DragInt3(ReadOnlySpan<byte> label, Span<int> v, float vSpeed = 1, int vMin = 0, int vMax = 0, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			if (v.Length < 3) throw new ArgumentException("Span must have at least 3 elements", nameof(v));
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%d"u8)) {
					fixed (int* pV = v) {
						return ImGuiNative.igDragInt3(pLabel, pV, vSpeed, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool DragInt4(ReadOnlySpan<byte> label, Span<int> v, float vSpeed = 1, int vMin = 0, int vMax = 0, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			if (v.Length < 4) throw new ArgumentException("Span must have at least 4 elements", nameof(v));
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%d"u8)) {
					fixed (int* pV = v) {
						return ImGuiNative.igDragInt4(pLabel, pV, vSpeed, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool DragIntRange2(ReadOnlySpan<byte> label, ref int vCurrentMin, ref int vCurrentMax, float vSpeed = 1, int vMin = 0, int vMax = 0, ReadOnlySpan<byte> format = default, ReadOnlySpan<byte> formatMax = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%d"u8), pFormatMax = CheckStr(formatMax)) {
					fixed (int* pCurrentMin = &vCurrentMin, pCurrentMax = &vCurrentMax) {
						return ImGuiNative.igDragIntRange2(pLabel, pCurrentMin, pCurrentMax, vSpeed, vMin, vMax, pFormat, formatMax.IsEmpty ? null : pFormatMax, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool DragScalar<T>(ReadOnlySpan<byte> label, ref T data, float vSpeed = 1, ImNullable<T> min = default, ImNullable<T> max = default, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) where T : struct {
			MemoryUtil.AssertUnmanaged<T>();
#pragma warning disable 8500
			T vmin = min.HasValue ? min.Value : default;
			T vmax = max.HasValue ? max.Value : default;
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format)) {
					fixed (T* pData = &data) {
						return ImGuiNative.igDragScalar(pLabel, GetDataType<T>(), pData, vSpeed, min.HasValue ? &vmin : null, max.HasValue ? &vmax : null, format.IsEmpty ? null : pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
#pragma warning restore
		}

		public bool DragScalarN<T>(ReadOnlySpan<byte> label, Span<T> data, float vSpeed = 1, ImNullable<T> min = default, ImNullable<T> max = default, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) where T : struct {
			MemoryUtil.AssertUnmanaged<T>();
#pragma warning disable 8500
			T vmin = min.HasValue ? min.Value : default;
			T vmax = max.HasValue ? max.Value : default;
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format)) {
					fixed (T* pData = data) {
						return ImGuiNative.igDragScalarN(pLabel, GetDataType<T>(), pData, data.Length, vSpeed, min.HasValue ? &vmin : null, max.HasValue ? &vmax : null, format.IsEmpty ? null : pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
#pragma warning restore
		}

		public void Dummy(Vector2 size) => IM.Dummy(size);

		public void End() => IM.End();

		public void EndChild() => IM.EndChild();

		public void EndCombo() => IM.EndCombo();

		public void EndDisabled() => IM.EndDisabled();

		public void EndDragDropSource() => IM.EndDragDropSource();

		public void EndDragDropTarget() => IM.EndDragDropTarget();

		public void EndFrame() => IM.EndFrame();

		public void EndGroup() => IM.EndGroup();

		public void EndListBox() => IM.EndListBox();

		public void EndMainMenuBar() => IM.EndMainMenuBar();

		public void EndMenu() => IM.EndMenu();

		public void EndMenuBar() => IM.EndMenuBar();

		public void EndPopup() => IM.EndPopup();

		public void EndTabBar() => IM.EndTabBar();

		public void EndTabItem() => IM.EndTabItem();

		public void EndTable() => IM.EndTable();

		public void EndTooltip() => IM.EndTooltip();

		public uint GetColorU32(ImGuiCol idx, float alphaMul = 1) => IM.GetColorU32((ImGuiNET.ImGuiCol)idx, alphaMul);

		public uint GetColorU32(Vector4 col) => IM.GetColorU32(col);

		public uint GetColorU32(uint col) => IM.GetColorU32(col);

		public float GetColumnOffset(int columnIndex = -1) => IM.GetColumnOffset(columnIndex);

		public float GetColumnWidth(int columnIndex = -1) => IM.GetColumnWidth(columnIndex);

		public IImDrawData GetDrawData() {
			var ptr = IM.GetDrawData();
			unsafe {
				if (ptr.NativePtr == (ImDrawData*)0) throw new InvalidOperationException("Draw data not yet available, did you call Render()?");
			}
			return new ImGuiNETDrawData(ptr);
		}

		public uint GetID(ReadOnlySpan<byte> strID) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igGetID_Str(pStrID);
				}
			}
		}

		public uint GetID(nint ptrID) => IM.GetID(ptrID);

		public string GetKeyName(ImGuiKey key) => IM.GetKeyName((ImGuiNET.ImGuiKey)key);

		public int GetKeyPressedAmount(ImGuiKey key, float repeatDelay, float rate) => IM.GetKeyPressedAmount((ImGuiNET.ImGuiKey)key, repeatDelay, rate);

		public int GetMouseClickedCount(ImGuiMouseButton button) => IM.GetMouseClickedCount((ImGuiNET.ImGuiMouseButton)button);

		public Vector2 GetMouseDragDelta(ImGuiMouseButton button = ImGuiMouseButton.Left, float lockThreshold = -1) => IM.GetMouseDragDelta((ImGuiNET.ImGuiMouseButton)button, lockThreshold);

		public string GetStyleColorName(ImGuiCol idx) => IM.GetStyleColorName((ImGuiNET.ImGuiCol)idx);

		public Vector4 GetStyleColorVec4(ImGuiCol idx) {
			unsafe {
				return *IM.GetStyleColorVec4((ImGuiNET.ImGuiCol)idx);
			}
		}

		public IImDrawList GetWindowDrawList() => ImGuiNETDrawList.Get(IM.GetWindowDrawList());

		public void Image(nuint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tintCol, Vector4 borderCol = default) => IM.Image((nint)userTextureID, size, uv0, uv1, tintCol, borderCol);

		public bool ImageButton(ReadOnlySpan<byte> strID, nuint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 bgCol, Vector4 tintCol) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igImageButton(pStrID, (nint)userTextureID, size, uv0, uv1, bgCol, tintCol) != 0;
				}
			}
		}

		public void Indent(float indentW = 0) => IM.Indent(indentW);

		public bool InputDouble(ReadOnlySpan<byte> label, ref double v, double step = 0, double stepFast = 0, ReadOnlySpan<byte> format = default, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.6f"u8)) {
					fixed(double* pV = &v) {
						return ImGuiNative.igInputDouble(pLabel, pV, step, stepFast, pFormat, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
		}

		public bool InputFloat(ReadOnlySpan<byte> label, ref float v, float step = 0, float stepFast = 0, ReadOnlySpan<byte> format = default, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (float* pV = &v) {
						return ImGuiNative.igInputFloat(pLabel, pV, step, stepFast, pFormat, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
		}

		public bool InputFloat2(ReadOnlySpan<byte> label, ref Vector2 v, ReadOnlySpan<byte> format = default, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (Vector2* pV = &v) {
						return ImGuiNative.igInputFloat2(pLabel, pV, pFormat, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
		}

		public bool InputFloat3(ReadOnlySpan<byte> label, ref Vector3 v, ReadOnlySpan<byte> format = default, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (Vector3* pV = &v) {
						return ImGuiNative.igInputFloat3(pLabel, pV, pFormat, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
		}

		public bool InputFloat4(ReadOnlySpan<byte> label, ref Vector4 v, ReadOnlySpan<byte> format = default, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (Vector4* pV = &v) {
						return ImGuiNative.igInputFloat4(pLabel, pV, pFormat, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
		}

		public bool InputInt(ReadOnlySpan<byte> label, ref int v, int step = 0, int stepFast = 0, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (int* pV = &v) {
						return ImGuiNative.igInputInt(pLabel, pV, step, stepFast, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
		}

		public bool InputInt2(ReadOnlySpan<byte> label, Span<int> v, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) {
			if (v.Length < 2) throw new ArgumentException("Span must have at least 2 elements", nameof(v));
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (int* pV = v) {
						return ImGuiNative.igInputInt2(pLabel, pV, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
		}

		public bool InputInt3(ReadOnlySpan<byte> label, Span<int> v, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) {
			if (v.Length < 3) throw new ArgumentException("Span must have at least 3 elements", nameof(v));
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (int* pV = v) {
						return ImGuiNative.igInputInt3(pLabel, pV, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
		}

		public bool InputInt4(ReadOnlySpan<byte> label, Span<int> v, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) {
			if (v.Length < 4) throw new ArgumentException("Span must have at least 4 elements", nameof(v));
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (int* pV = v) {
						return ImGuiNative.igInputInt4(pLabel, pV, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
		}

		public bool InputScalar<T>(ReadOnlySpan<byte> label, ref T data, ImNullable<T> pStep = default, ImNullable<T> pStepFast = default, ReadOnlySpan<byte> format = default, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) where T : struct {
			MemoryUtil.AssertUnmanaged<T>();
#pragma warning disable 8500
			T vstep = pStep.HasValue ? pStep.Value : default;
			T vstepfast = pStepFast.HasValue ? pStepFast.Value : default;
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format)) {
					fixed (T* pData = &data) {
						return ImGuiNative.igInputScalar(pLabel, GetDataType<T>(), pData, pStep.HasValue ? &vstep : null, pStepFast.HasValue ? &vstepfast : null, format.IsEmpty ? null : pFormat, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
#pragma warning restore
		}

		public bool InputScalarN<T>(ReadOnlySpan<byte> label, Span<T> data, ImNullable<T> pStep = default, ImNullable<T> pStepFast = default, ReadOnlySpan<byte> format = default, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) where T : struct {
			MemoryUtil.AssertUnmanaged<T>();
#pragma warning disable 8500
			T vstep = pStep.HasValue ? pStep.Value : default;
			T vstepfast = pStepFast.HasValue ? pStepFast.Value : default;
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format)) {
					fixed (T* pData = data) {
						return ImGuiNative.igInputScalarN(pLabel, GetDataType<T>(), pData, data.Length, pStep.HasValue ? &vstep : null, pStepFast.HasValue ? &vstepfast : null, format.IsEmpty ? null : pFormat, (ImGuiNET.ImGuiInputTextFlags)flags) != 0;
					}
				}
			}
#pragma warning restore
		}


		private static readonly List<ImGuiInputTextCallback> textCallbacks = new();

		private unsafe class TextCallbackData : IImGuiInputTextCallbackData {

			internal ImGuiInputTextCallbackDataPtr callbackData;

			public ImGuiInputTextFlags EventFlag => (ImGuiInputTextFlags)callbackData.EventFlag;

			public ImGuiInputTextFlags Flags => (ImGuiInputTextFlags)callbackData.Flags;

			public char EventChar => (char)callbackData.EventChar;

			public ImGuiKey EventKey => (ImGuiKey)callbackData.EventKey;

			public Span<byte> Buf {
				get {
					return new Span<byte>((void*)callbackData.Buf, callbackData.BufSize);
				}
			}

			public int BufSize => callbackData.BufSize;

			public int BufTextLen => callbackData.BufTextLen;

			public bool BufDirty { get => callbackData.BufDirty; set => callbackData.BufDirty = value; }

			public int CursorPos { get => callbackData.CursorPos; set => callbackData.CursorPos = value; }

			public int SelectionStart { get => callbackData.SelectionStart; set => callbackData.SelectionStart = value; }

			public int SelectionEnd { get => callbackData.SelectionEnd; set => callbackData.SelectionEnd = value; }

			public void ClearSelection() => callbackData.ClearSelection();

			public void DeleteChars(int pos, int bytesCount) => callbackData.DeleteChars(pos, bytesCount);

			public void InsertChars(int pos, string text) => callbackData.InsertChars(pos, text);

			public void SelectAll() => callbackData.SelectAll();

		}

		private static readonly TextCallbackData textCallbackData = new();

		private static unsafe int InputTextCallback(ImGuiInputTextCallbackData* data) {
			var cbk = textCallbacks[(int)data->UserData];
			textCallbackData.callbackData = new ImGuiInputTextCallbackDataPtr(data);
			return cbk(textCallbackData);
		}

		public bool InputText(ReadOnlySpan<byte> label, ImGuiTextBuffer buf, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None, ImGuiInputTextCallback? callback = null) {
			callback ??= buf.Callback;
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pBuf = buf.Buf) {
					int index = textCallbacks.Count;
					textCallbacks.Add(callback);
					return ImGuiNative.igInputText(pLabel, pBuf, (uint)buf.Capacity, (ImGuiNET.ImGuiInputTextFlags)flags, InputTextCallback, (void*)index) != 0;
				}
			}
		}

		public bool InputTextMultiline(ReadOnlySpan<byte> label, ImGuiTextBuffer buf, Vector2 size = default, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None, ImGuiInputTextCallback? callback = null) {
			callback ??= buf.Callback;
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pBuf = buf.Buf) {
					int index = textCallbacks.Count;
					textCallbacks.Add(callback);
					return ImGuiNative.igInputTextMultiline(pLabel, pBuf, (uint)buf.Capacity, size, (ImGuiNET.ImGuiInputTextFlags)flags, InputTextCallback, (void*)index) != 0;
				}
			}
		}

		public bool InputTextWithHint(ReadOnlySpan<byte> label, ReadOnlySpan<byte> hint, ImGuiTextBuffer buf, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None, ImGuiInputTextCallback? callback = null) {
			callback ??= buf.Callback;
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pHint = CheckStr(hint), pBuf = buf.Buf) {
					int index = textCallbacks.Count;
					textCallbacks.Add(callback);
					return ImGuiNative.igInputTextWithHint(pLabel, pHint, pBuf, (uint)buf.Capacity, (ImGuiNET.ImGuiInputTextFlags)flags, InputTextCallback, (void*)index) != 0;
				}
			}
		}


		public bool InvisibleButton(ReadOnlySpan<byte> strID, Vector2 size, ImGuiButtonFlags flags = ImGuiButtonFlags.None) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igInvisibleButton(pStrID, size, (ImGuiNET.ImGuiButtonFlags)flags) != 0;
				}
			}
		}

		public bool IsItemClicked(ImGuiMouseButton mouseButton = ImGuiMouseButton.Left) => IM.IsItemClicked((ImGuiNET.ImGuiMouseButton)mouseButton);

		public bool IsItemHovered(ImGuiHoveredFlags flags = ImGuiHoveredFlags.None) => IM.IsItemHovered((ImGuiNET.ImGuiHoveredFlags)flags);

		public bool IsKeyDown(ImGuiKey key) => IM.IsKeyDown((ImGuiNET.ImGuiKey)key);

		public bool IsKeyPressed(ImGuiKey key, bool repeat = true) => IM.IsKeyPressed((ImGuiNET.ImGuiKey)key, repeat);

		public bool IsKeyReleased(ImGuiKey key) => IM.IsKeyReleased((ImGuiNET.ImGuiKey)key);

		public bool IsMouseClicked(ImGuiMouseButton button, bool repeat = false) => IM.IsMouseClicked((ImGuiNET.ImGuiMouseButton)button, repeat);

		public bool IsMouseDoubleClicked(ImGuiMouseButton button) => IM.IsMouseDoubleClicked((ImGuiNET.ImGuiMouseButton)button);

		public bool IsMouseDown(ImGuiMouseButton button) => IM.IsMouseDown((ImGuiNET.ImGuiMouseButton)button);

		public bool IsMouseDragging(ImGuiMouseButton button, float lockThreshold = -1) => IM.IsMouseDragging((ImGuiNET.ImGuiMouseButton)button, lockThreshold);

		public bool IsMouseHoveringRect(Vector2 rMin, Vector2 rMax, bool clip = true) => IM.IsMouseHoveringRect(rMin, rMax, clip);

		public bool IsMousePosValid(Vector2? mousePos = null) {
			if (mousePos == null) return IM.IsMousePosValid();
			else {
				Vector2 vmousePos = mousePos.Value;
				return IM.IsMousePosValid(ref vmousePos);
			}
		}

		public bool IsMouseReleased(ImGuiMouseButton button) => IM.IsMouseReleased((ImGuiNET.ImGuiMouseButton)button);

		public bool IsPopupOpen(ReadOnlySpan<byte> strID, ImGuiPopupFlags flags = ImGuiPopupFlags.MouseButtonLeft) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					return ImGuiNative.igIsPopupOpen_Str(pStrID, (ImGuiNET.ImGuiPopupFlags)flags) != 0;
				}
			}
		}

		public bool IsRectVisible(Vector2 size) => IM.IsRectVisible(size);

		public bool IsRectVisible(Vector2 rectMin, Vector2 rectMax) => IM.IsRectVisible(rectMin, rectMax);

		public bool IsWindowFocused(ImGuiFocusedFlags flags = ImGuiFocusedFlags.None) => IM.IsWindowFocused((ImGuiNET.ImGuiFocusedFlags)flags);

		public bool IsWindowHovered(ImGuiHoveredFlags flags = ImGuiHoveredFlags.None) => IM.IsWindowHovered((ImGuiNET.ImGuiHoveredFlags)flags);

		public void LabelText(ReadOnlySpan<byte> label, ReadOnlySpan<byte> fmt) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label), pFmt = CheckFmtStr(fmt)) {
					ImGuiNative.igLabelText(pLabel, pFmt);
				}
			}
		}

		public bool ListBox(ReadOnlySpan<byte> label, ref int currentItem, IEnumerable<string> items, int heightInItems = -1) {
			using MemoryStack sp = MemoryStack.Push();
			IntPtr pItems;
			int itemCount;
			
			if (items is IReadOnlyCollection<string> collection) {
				pItems = sp.UTF8Array(collection);
				itemCount = collection.Count;
			} else {
				pItems = EnumerateStrings(sp, items, out itemCount);
			}

			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (int* pCurrentItem = &currentItem) {
						return ImGuiNative.igListBox_Str_arr(pLabel, pCurrentItem, (byte**)pItems, itemCount, heightInItems) != 0;
					}
				}
			}
		}

		public bool ListBox(ReadOnlySpan<byte> label, ref int currentItem, IImGui.ListBoxItemsGetter itemsGetter, int itemsCount, int heightInItems = -1) {
			using MemoryStack sp = MemoryStack.Push();
			Span<IntPtr> items = stackalloc IntPtr[itemsCount];
			for(int i = 0; i < itemsCount; i++) {
				if (!itemsGetter(i, out string text)) text = "Unknown item";
				items[i] = sp.UTF8(text);
			}

			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					fixed(IntPtr* pItems = items) {
						fixed(int* pCurrentItem = &currentItem) {
							return ImGuiNative.igListBox_Str_arr(pLabel, pCurrentItem, (byte**)pItems, itemsCount, heightInItems) != 0;
						}
					}
				}
			}
		}

		public void LoadIniSettingsFromDisk(string iniFilename) => IM.LoadIniSettingsFromDisk(iniFilename);

		public void LoadIniSettingsFromMemory(ReadOnlySpan<byte> iniData) {
			unsafe {
				fixed(byte* pIniData = iniData) {
					ImGuiNative.igLoadIniSettingsFromMemory(pIniData, (uint)iniData.Length);
				}
			}
		}

		public void LogButtons() => IM.LogButtons();

		public void LogFinish() => IM.LogFinish();

		public void LogText(string fmt) => IM.LogText(fmt);

		public void LogToClipboard(int autoOpenDepth = -1) => IM.LogToClipboard(autoOpenDepth);

		public void LogToFile(int autoOpenDepth = -1, string? filename = null) => IM.LogToFile(autoOpenDepth, filename);

		public void LogToTTY(int autoOpenDepth = -1) => IM.LogToTTY(autoOpenDepth);

		public bool MenuItem(ReadOnlySpan<byte> label, ReadOnlySpan<byte> shortcut = default, bool selected = false, bool enabled = true) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label), pShortcut = CheckStr(shortcut)) {
					return ImGuiNative.igMenuItem_Bool(pLabel, shortcut.IsEmpty ? null : pShortcut, (byte)(selected ? 1 : 0), (byte)(enabled ? 1 : 0)) != 0;
				}
			}
		}

		public bool MenuItem(ReadOnlySpan<byte> label, ReadOnlySpan<byte> shortcut, ref bool selected, bool enabled = true) {
			unsafe {
				fixed(byte* pLabel = CheckStr(shortcut), pShortcut = CheckStr(shortcut)) {
					byte bselected = (byte)(selected ? 1 : 0);
					byte bret = ImGuiNative.igMenuItem_BoolPtr(pLabel, pShortcut, &bselected, (byte)(enabled ? 1 : 0));
					selected = bselected != 0;
					return bret != 0;
				}
			}
		}

		public IImFontAtlas NewFontAtlas() => new ImGuiNETFontAtlas();

		public void NewFrame() {
			IM.NewFrame();
			ImGuiNETDrawList.NewFrame();
			sizeCallbacks.Clear();
		}

		public void NewLine() => IM.NewLine();

		public IImGuiStorage NewStorage() => new ImGuiNETStorage();

		public IImGuiStyle NewStyle() => new ImGuiNETStyle();

		public void NextColumn() => IM.NextColumn();

		public void OpenPopup(ReadOnlySpan<byte> strID, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonLeft) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					ImGuiNative.igOpenPopup_Str(pStrID, (ImGuiNET.ImGuiPopupFlags)popupFlags);
				}
			}
		}

		public void OpenPopup(uint id, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonLeft) => IM.OpenPopup(id, (ImGuiNET.ImGuiPopupFlags)popupFlags);

		public void OpenPopupOnItemClick(ReadOnlySpan<byte> strID = default, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonDefault) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					ImGuiNative.igOpenPopupOnItemClick(pStrID, (ImGuiNET.ImGuiPopupFlags)popupFlags);
				}
			}
		}

		public void PlotHistogram(ReadOnlySpan<byte> label, ReadOnlySpan<float> values, int valuesCount = -1, ReadOnlySpan<byte> overlayText = default, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default, int stride = 1) {
			if (valuesCount < 0) valuesCount = values.Length / stride;
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pOverlayText = CheckStr(overlayText, stackalloc byte[0])) {
					fixed (float* pValues = values) {
						ImGuiNative.igPlotHistogram_FloatPtr(pLabel, pValues, valuesCount, 0, overlayText.IsEmpty ? null : pOverlayText, scaleMin, scaleMax, graphSize, stride);
					}
				}
			}
		}

		public void PlotHistogram(ReadOnlySpan<byte> label, Func<int, float> valuesGetter, int valuesCount, int valuesOffset = 0, ReadOnlySpan<byte> overlayText = default, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default) {
			Span<float> values = stackalloc float[valuesCount];
			for (int i = 0; i < valuesCount; i++) values[i] = valuesGetter(i);
			PlotHistogram(label, values, valuesCount, overlayText, scaleMin, scaleMax, graphSize);
		}

		public void PlotLines(ReadOnlySpan<byte> label, ReadOnlySpan<float> values, int valuesCount = -1, ReadOnlySpan<byte> overlayText = default, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default, int stride = 1) {
			if (valuesCount < 0) valuesCount = values.Length / stride;
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pOverlayText = CheckStr(overlayText, stackalloc byte[0])) {
					fixed (float* pValues = values) {
						ImGuiNative.igPlotLines_FloatPtr(pLabel, pValues, valuesCount, 0, overlayText.IsEmpty ? null : pOverlayText, scaleMin, scaleMax, graphSize, stride);
					}
				}
			}
		}

		public void PlotLines(ReadOnlySpan<byte> label, Func<int, float> valuesGetter, int valuesCount, int valuesOffset = 0, ReadOnlySpan<byte> overlayText = default, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default) {
			Span<float> values = stackalloc float[valuesCount];
			for (int i = 0; i < valuesCount; i++) values[i] = valuesGetter(i);
			PlotLines(label, values, valuesCount, overlayText, scaleMin, scaleMax, graphSize);
		}

		public void PopTabStop() => IM.PopTabStop();

		public void PopButtonRepeat() => IM.PopButtonRepeat();

		public void PopClipRect() => IM.PopClipRect();

		public void PopFont() => IM.PopFont();

		public void PopID() => IM.PopID();

		public void PopItemWidth() => IM.PopItemWidth();

		public void PopStyleColor(int count = 1) => IM.PopStyleColor(count);

		public void PopStyleVar(int count = 1) => IM.PopStyleVar(count);

		public void PopTextWrapPos() => IM.PopTextWrapPos();

		public void ProgressBar(float fraction, Vector2 sizeArg, ReadOnlySpan<byte> overlay = default) {
			unsafe {
				fixed(byte* pOverlay = CheckStr(overlay)) {
					ImGuiNative.igProgressBar(fraction, sizeArg, overlay.IsEmpty ? null : pOverlay);
				}
			}
		}

		public void PushTabStop(bool tabStop) => IM.PushTabStop(tabStop);

		public void PushButtonRepeat(bool repeat) => IM.PushButtonRepeat(repeat);

		public void PushClipRect(Vector2 clipRectMin, Vector2 clipRectMax, bool intersectWithCurrentClipRect) => IM.PushClipRect(clipRectMin, clipRectMax, intersectWithCurrentClipRect);

		public void PushFont(IImFont font) => IM.PushFont(((ImGuiNETFont)font).font);

		public void PushID(ReadOnlySpan<byte> strID) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					ImGuiNative.igPushID_Str(pStrID);
				}
			}
		}

		public void PushID(nint ptrID) => IM.PushID(ptrID);

		public void PushID(int id) => IM.PushID(id);

		public void PushItemWidth(float itemWidth) => IM.PushItemWidth(itemWidth);

		public void PushStyleColor(ImGuiCol idx, uint col) => IM.PushStyleColor((ImGuiNET.ImGuiCol)idx, col);

		public void PushStyleColor(ImGuiCol idx, Vector4 col) => IM.PushStyleColor((ImGuiNET.ImGuiCol)idx, col);

		public void PushStyleVar(ImGuiStyleVar idx, float val) => IM.PushStyleVar((ImGuiNET.ImGuiStyleVar)idx, val);

		public void PushStyleVar(ImGuiStyleVar idx, Vector2 val) => IM.PushStyleVar((ImGuiNET.ImGuiStyleVar)idx, val);

		public void PushTextWrapPos(float wrapLocalPosX = 0) => IM.PushTextWrapPos(wrapLocalPosX);

		public bool RadioButton(ReadOnlySpan<byte> label, bool active) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					return ImGuiNative.igRadioButton_Bool(pLabel, (byte)(active ? 1 : 0)) != 0;
				}
			}
		}

		public bool RadioButton(ReadOnlySpan<byte> label, ref int v, int vButton) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					fixed (int* pV = &v) {
						return ImGuiNative.igRadioButton_IntPtr(pLabel, pV, vButton) != 0;
					}
				}
			}
		}

		public void Render() => IM.Render();

		public void ResetMouseDragDelta(ImGuiMouseButton button = ImGuiMouseButton.Left) => IM.ResetMouseDragDelta((ImGuiNET.ImGuiMouseButton)button);

		public void SameLine(float offsetFromStartX = 0, float spacing = -1) => IM.SameLine(offsetFromStartX, spacing);

		public void SaveIniSettingsToDisk(string iniFilename) => IM.SaveIniSettingsToDisk(iniFilename);

		public ReadOnlySpan<byte> SaveIniSettingsToMemory() {
			unsafe {
				uint count = 0;
				byte* ptr = ImGuiNative.igSaveIniSettingsToMemory(&count);
				byte[] data = new byte[count];
				fixed(byte* pData = data) {
					Buffer.MemoryCopy(ptr, pData, count, count);
				}
				return data;
			}
		}

		public bool Selectable(ReadOnlySpan<byte> label, bool selected = false, ImGuiSelectableFlags flags = ImGuiSelectableFlags.None, Vector2 size = default) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					return ImGuiNative.igSelectable_Bool(pLabel, (byte)(selected ? 1 : 0), (ImGuiNET.ImGuiSelectableFlags)flags, size) != 0;
				}
			}
		}

		public bool Selectable(ReadOnlySpan<byte> label, ref bool selected, ImGuiSelectableFlags flags = ImGuiSelectableFlags.None, Vector2 size = default) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					byte bselected = (byte)(selected ? 1 : 0);
					byte bret = ImGuiNative.igSelectable_BoolPtr(pLabel, &bselected, (ImGuiNET.ImGuiSelectableFlags)flags, size);
					selected = bselected != 0;
					return bret != 0;
				}
			}
		}

		public void Separator() => IM.Separator();

		public void SetColorEditOptions(ImGuiColorEditFlags flags) => IM.SetColorEditOptions((ImGuiNET.ImGuiColorEditFlags)flags);

		public void SetColumnOffset(int columnIndex, float width) => IM.SetColumnOffset(columnIndex, width);

		public void SetColumnWidth(int columnIndex, float width) => IM.SetColumnWidth(columnIndex, width);

		public bool SetDragDropPayload(string type, ReadOnlySpan<byte> data, ImGuiCond cond = ImGuiCond.None) {
			unsafe {
				fixed(byte* pData = data) {
					return IM.SetDragDropPayload(type, (IntPtr)pData, (uint)data.Length, (ImGuiNET.ImGuiCond)cond);
				}
			}
		}

		public void SetItemAllowOverlap() => IM.SetItemAllowOverlap();

		public void SetItemDefaultFocus() => IM.SetItemDefaultFocus();

		public void SetKeyboardFocusHere(int offset = 0) => IM.SetKeyboardFocusHere(offset);

		public void SetNextItemOpen(bool isOpen, ImGuiCond cond = ImGuiCond.None) => IM.SetNextItemOpen(isOpen, (ImGuiNET.ImGuiCond)cond);

		public void SetNextItemWidth(float itemWidth) => IM.SetNextItemWidth(itemWidth);

		public void SetNextWindowBgAlpha(float alpha) => IM.SetNextWindowBgAlpha((float)alpha);

		public void SetNextWindowCollapsed(bool collapsed, ImGuiCond cond = ImGuiCond.None) => IM.SetNextWindowCollapsed(collapsed, (ImGuiNET.ImGuiCond)cond);

		public void SetNextWindowContentSize(Vector2 size) => IM.SetNextWindowContentSize(size);

		public void SetNextWindowFocus() => IM.SetNextWindowFocus();

		public void SetNextWindowPos(Vector2 pos, ImGuiCond cond = ImGuiCond.None, Vector2 pivot = default) => IM.SetNextWindowPos(pos, (ImGuiNET.ImGuiCond)cond, pivot);

		public void SetNextWindowSize(Vector2 size, ImGuiCond cond = ImGuiCond.None) => IM.SetNextWindowSize(size, (ImGuiNET.ImGuiCond)cond);


		private static readonly List<ImGuiSizeCallback> sizeCallbacks = new();

		private static unsafe void SizeCallback(ImGuiNET.ImGuiSizeCallbackData* data) {
			ImGuiSizeCallbackData data2 = new() {
				CurrentSize = data->CurrentSize,
				DesiredSize = data->DesiredSize,
				Pos = data->Pos
			};
			sizeCallbacks[(int)data->UserData].Invoke(ref data2);
			data->CurrentSize = data2.CurrentSize;
			data->DesiredSize = data2.DesiredSize;
			data->Pos = data2.Pos;
		}

		public void SetNextWindowSizeConstraints(Vector2 sizeMin, Vector2 sizeMax, ImGuiSizeCallback? customCallback = null) {
			if (customCallback != null) {
				unsafe {
					int index = sizeCallbacks.Count;
					sizeCallbacks.Add(customCallback);
					IM.SetNextWindowSizeConstraints(sizeMin, sizeMax, SizeCallback, index);
				}
			} else IM.SetNextWindowSizeConstraints(sizeMin, sizeMax);
		}


		public void SetScrollFromPosX(float localX, float centerXRatio = 0.5F) => IM.SetScrollFromPosX(localX, centerXRatio);

		public void SetScrollFromPosY(float localY, float centerYRatio = 0.5F) => IM.SetScrollFromPosY(localY, centerYRatio);

		public void SetScrollHereX(float centerXRatio = 0.5F) => IM.SetScrollHereX(centerXRatio);

		public void SetScrollHereY(float centerYRatio = 0.5F) => IM.SetScrollHereY(centerYRatio);

		public void SetTabItemClosed(ReadOnlySpan<byte> tabOrDockedWindowLabel) {
			unsafe {
				fixed(byte* pTabOrDockedWindowLabel = CheckStr(tabOrDockedWindowLabel)) {
					ImGuiNative.igSetTabItemClosed(pTabOrDockedWindowLabel);
				}
			}
		}

		public void SetTooltip(ReadOnlySpan<byte> fmt) {
			unsafe {
				fixed(byte* pFmt = CheckFmtStr(fmt)) {
					ImGuiNative.igSetTooltip(pFmt);
				}
			}
		}

		public void SetWindowCollapsed(bool collapsed, ImGuiCond cond = ImGuiCond.None) => IM.SetWindowCollapsed(collapsed, (ImGuiNET.ImGuiCond)cond);

		public void SetWindowCollapsed(ReadOnlySpan<byte> name, bool collapsed, ImGuiCond cond = ImGuiCond.None) {
			unsafe {
				fixed(byte* pName = CheckStr(name)) {
					ImGuiNative.igSetWindowCollapsed_Str(pName, (byte)(collapsed ? 1 : 0), (ImGuiNET.ImGuiCond)cond);
				}
			}
		}

		public void SetWindowFocus() => IM.SetWindowFocus();

		public void SetWindowFocus(ReadOnlySpan<byte> name) {
			unsafe {
				fixed(byte* pName = CheckStr(name)) {
					ImGuiNative.igSetWindowFocus_Str(pName);
				}
			}
		}

		public void SetWindowFontScale(float scale) => IM.SetWindowFontScale(scale);

		public void SetWindowPos(Vector2 pos, ImGuiCond cond = ImGuiCond.None) => IM.SetWindowPos(pos, (ImGuiNET.ImGuiCond)cond);

		public void SetWindowPos(ReadOnlySpan<byte> name, Vector2 pos, ImGuiCond cond = ImGuiCond.None) {
			unsafe {
				fixed(byte* pName = CheckStr(name)) {
					ImGuiNative.igSetWindowPos_Str(pName, pos, (ImGuiNET.ImGuiCond)cond);
				}
			}
		}

		public void SetWindowSize(Vector2 size, ImGuiCond cond = ImGuiCond.None) => IM.SetWindowSize(size, (ImGuiNET.ImGuiCond)cond);

		public void SetWindowSize(ReadOnlySpan<byte> name, Vector2 size, ImGuiCond cond = ImGuiCond.None) {
			unsafe {
				fixed(byte* pName = CheckStr(name)) {
					ImGuiNative.igSetWindowSize_Str(pName, size, (ImGuiNET.ImGuiCond)cond);
				}
			}
		}

		public void ShowAboutWindow(ref bool open) => IM.ShowAboutWindow(ref open);

		public void ShowDemoWindow(ref bool open) => IM.ShowDemoWindow(ref open);

		public void ShowFontSelector(ReadOnlySpan<byte> label) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					ImGuiNative.igShowFontSelector(pLabel);
				}
			}
		}

		public void ShowMetricsWindow(ref bool open) => IM.ShowMetricsWindow(ref open);

		public void ShowStackToolWindow(ref bool open) => IM.ShowStackToolWindow(ref open);

		public void ShowStyleEditor(IImGuiStyle? style = null) {
			if (style != null) IM.ShowStyleEditor(((ImGuiNETStyle)style).style);
			else IM.ShowStyleEditor();
		}

		public void ShowStyleSelector(ReadOnlySpan<byte> label) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					ImGuiNative.igShowStyleSelector(pLabel);
				}
			}
		}

		public void ShowUserGuide() => IM.ShowUserGuide();

		public bool SliderAngle(ReadOnlySpan<byte> label, ref float vRad, float vDegreesMin = -360, float vDegreesMax = 360, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.0f"u8)) {
					fixed (float* pV = &vRad) {
						return ImGuiNative.igSliderFloat(pLabel, pV, vDegreesMin, vDegreesMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool SliderFloat(ReadOnlySpan<byte> label, ref float v, float vMin, float vMax, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (float* pV = &v) {
						return ImGuiNative.igSliderFloat(pLabel, pV, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool SliderFloat2(ReadOnlySpan<byte> label, ref Vector2 v, float vMin, float vMax, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (Vector2* pV = &v) {
						return ImGuiNative.igSliderFloat2(pLabel, pV, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool SliderFloat3(ReadOnlySpan<byte> label, ref Vector3 v, float vMin, float vMax, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (Vector3* pV = &v) {
						return ImGuiNative.igSliderFloat3(pLabel, pV, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool SliderFloat4(ReadOnlySpan<byte> label, ref Vector4 v, float vMin, float vMax, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (Vector4* pV = &v) {
						return ImGuiNative.igSliderFloat4(pLabel, pV, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool SliderInt(ReadOnlySpan<byte> label, ref int v, int vMin, int vMax, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%d"u8)) {
					fixed (int* pV = &v) {
						return ImGuiNative.igSliderInt(pLabel, pV, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool SliderInt2(ReadOnlySpan<byte> label, Span<int> v, int vMin, int vMax, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			if (v.Length < 2) throw new ArgumentException("Span must have at least 2 elements", nameof(v));
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%d"u8)) {
					fixed (int* pV = v) {
						return ImGuiNative.igSliderInt2(pLabel, pV, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool SliderInt3(ReadOnlySpan<byte> label, Span<int> v, int vMin, int vMax, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			if (v.Length < 3) throw new ArgumentException("Span must have at least 3 elements", nameof(v));
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%d"u8)) {
					fixed (int* pV = v) {
						return ImGuiNative.igSliderInt3(pLabel, pV, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool SliderInt4(ReadOnlySpan<byte> label, Span<int> v, int vMin, int vMax, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			if (v.Length < 4) throw new ArgumentException("Span must have at least 4 elements", nameof(v));
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%d"u8)) {
					fixed (int* pV = v) {
						return ImGuiNative.igSliderInt4(pLabel, pV, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool SliderScalar<T>(ReadOnlySpan<byte> label, ref T data, T min, T max, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) where T : struct {
			MemoryUtil.AssertUnmanaged<T>();
#pragma warning disable 8500
			unsafe {
				fixed(byte* pLabel = CheckStr(label), pFormat = CheckStr(format)) {
					fixed(T* pData = &data) {
						return ImGuiNative.igSliderScalar(pLabel, GetDataType<T>(), pData, &min, &max, format.IsEmpty ? null : pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
#pragma warning restore
		}

		public bool SliderScalarN<T>(ReadOnlySpan<byte> label, Span<T> data, T min, T max, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) where T : struct {
			MemoryUtil.AssertUnmanaged<T>();
#pragma warning disable 8500
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format)) {
					fixed (T* pData = data) {
						return ImGuiNative.igSliderScalarN(pLabel, GetDataType<T>(), pData, data.Length, &min, &max, format.IsEmpty ? null : pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
#pragma warning restore
		}

		public bool SmallButton(ReadOnlySpan<byte> label) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					return ImGuiNative.igSmallButton(pLabel) != 0;
				}
			}
		}

		public void Spacing() => IM.Spacing();

		public void StyleColorsClassic(IImGuiStyle? dst = null) {
			if (dst != null) IM.StyleColorsClassic(((ImGuiNETStyle)dst).style);
			else IM.StyleColorsClassic();
		}

		public void StyleColorsDark(IImGuiStyle? dst = null) {
			if (dst != null) IM.StyleColorsDark(((ImGuiNETStyle)dst).style);
			else IM.StyleColorsDark();
		}

		public void StyleColorsLight(IImGuiStyle? dst = null) {
			if (dst != null) IM.StyleColorsLight(((ImGuiNETStyle)dst).style);
			else IM.StyleColorsLight();
		}

		public bool TabItemButton(ReadOnlySpan<byte> label, ImGuiTabItemFlags flags = ImGuiTabItemFlags.None) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					return ImGuiNative.igTabItemButton(pLabel, (ImGuiNET.ImGuiTabItemFlags)flags) != 0;
				}
			}
		}

		public ImGuiTableColumnFlags TableGetColumnFlags(int columnN = -1) => (ImGuiTableColumnFlags)IM.TableGetColumnFlags(columnN);

		public string TableGetColumnName(int columnN = -1) => IM.TableGetColumnName(columnN);

		public void TableHeader(ReadOnlySpan<byte> label) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					ImGuiNative.igTableHeader(pLabel);
				}
			}
		}

		public void TableHeadersRow() => IM.TableHeadersRow();

		public bool TableNextColumn() => IM.TableNextColumn();

		public void TableNextRow(ImGuiTableRowFlags rowFlags = ImGuiTableRowFlags.None, float minRowHeight = 0) => IM.TableNextRow((ImGuiNET.ImGuiTableRowFlags)rowFlags);

		public void TableSetBgColor(ImGuiTableBgTarget target, uint color, int columnN = -1) => IM.TableSetBgColor((ImGuiNET.ImGuiTableBgTarget)target, color, columnN);

		public void TableSetColumnEnabled(int columnN, bool v) => IM.TableSetColumnEnabled(columnN, v);

		public bool TableSetColumnIndex(int columnN) => IM.TableSetColumnIndex(columnN);

		public void TableSetupColumn(ReadOnlySpan<byte> label, ImGuiTableColumnFlags flags = ImGuiTableColumnFlags.None, float initWidthOrWeight = 0, uint userID = 0) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label)) {
					ImGuiNative.igTableSetupColumn(pLabel, (ImGuiNET.ImGuiTableColumnFlags)flags, initWidthOrWeight, userID);
				}
			}
		}

		public void TableSetupScrollFreeze(int cols, int rows) => IM.TableSetupScrollFreeze(cols, rows);

		public void Text(ReadOnlySpan<byte> fmt) {
			unsafe {
				fixed(byte* pFmt = CheckFmtStr(fmt)) {
					ImGuiNative.igText(pFmt);
				}
			}
		}

		public void TextColored(Vector4 col, ReadOnlySpan<byte> fmt) {
			unsafe {
				fixed(byte* pFmt = CheckFmtStr(fmt)) {
					ImGuiNative.igTextColored(col, pFmt);
				}
			}
		}

		public void TextDisabled(ReadOnlySpan<byte> fmt) {
			unsafe {
				fixed (byte* pFmt = CheckFmtStr(fmt)) {
					ImGuiNative.igTextDisabled(pFmt);
				}
			}
		}

		public void TextWrapped(ReadOnlySpan<byte> fmt) {
			unsafe {
				fixed (byte* pFmt = CheckFmtStr(fmt)) {
					ImGuiNative.igTextWrapped(pFmt);
				}
			}
		}

		public bool TreeNode(ReadOnlySpan<byte> label) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					return ImGuiNative.igTreeNode_Str(pLabel) != 0;
				}
			}
		}

		public bool TreeNode(ReadOnlySpan<byte> strID, ReadOnlySpan<byte> text) {
			unsafe {
				fixed (byte* pStrID = CheckStr(strID), pText = CheckFmtStr(text)) {
					return ImGuiNative.igTreeNode_StrStr(pStrID, pText) != 0;
				}
			}
		}

		public bool TreeNode(nint ptrID, ReadOnlySpan<byte> text) {
			unsafe {
				fixed (byte* pText = CheckStr(text)) {
					return ImGuiNative.igTreeNode_Ptr((void*)ptrID, pText) != 0;
				}
			}
		}

		public bool TreeNodeEx(ReadOnlySpan<byte> label, ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label)) {
					return ImGuiNative.igTreeNodeEx_Str(pLabel, (ImGuiNET.ImGuiTreeNodeFlags)flags) != 0;
				}
			}
		}

		public bool TreeNodeEx(ReadOnlySpan<byte> strID, ImGuiTreeNodeFlags flags, ReadOnlySpan<byte> text) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID), pText = CheckFmtStr(text)) {
					return ImGuiNative.igTreeNodeEx_StrStr(pStrID, (ImGuiNET.ImGuiTreeNodeFlags)flags, pText) != 0;
				}
			}
		}

		public bool TreeNodeEx(nint ptrID, ImGuiTreeNodeFlags flags, ReadOnlySpan<byte> text) {
			unsafe {
				fixed(byte* pText = CheckFmtStr(text)) {
					return ImGuiNative.igTreeNodeEx_Ptr((void*)ptrID, (ImGuiNET.ImGuiTreeNodeFlags)flags, pText) != 0;
				}
			}
		}

		public void TreePop() => IM.TreePop();

		public void TreePush(ReadOnlySpan<byte> strID) {
			unsafe {
				fixed(byte* pStrID = CheckStr(strID)) {
					ImGuiNative.igTreePush_Str(pStrID);
				}
			}
		}

		public void TreePush(nint ptrID = 0) => IM.TreePush(ptrID);

		public void Unindent(float indentW = 0) => IM.Unindent(indentW);

		public void Value(ReadOnlySpan<byte> prefix, bool b) {
			unsafe {
				fixed(byte* pPrefix = CheckStr(prefix)) {
					ImGuiNative.igValue_Bool(pPrefix, (byte)(b ? 1 : 0));
				}
			}
		}

		public void Value(ReadOnlySpan<byte> prefix, int v) {
			unsafe {
				fixed(byte* pPrefix = CheckStr(prefix)) {
					ImGuiNative.igValue_Int(pPrefix, v);
				}
			}
		}

		public void Value(ReadOnlySpan<byte> prefix, uint v) {
			unsafe {
				fixed (byte* pPrefix = CheckStr(prefix)) {
					ImGuiNative.igValue_Uint(pPrefix, v);
				}
			}
		}

		public void Value(ReadOnlySpan<byte> prefix, float v, ReadOnlySpan<byte> floatFormat = default) {
			unsafe {
				fixed (byte* pPrefix = CheckStr(prefix), pFormat = CheckStr(floatFormat, "%.3f"u8)) {
					ImGuiNative.igValue_Float(pPrefix, v, pFormat);
				}
			}
		}

		public bool VSliderFloat(ReadOnlySpan<byte> label, Vector2 size, ref float v, float vMin, float vMax, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed(byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed(float* pV = &v) {
						return ImGuiNative.igVSliderFloat(pLabel, size, pV, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool VSliderInt(ReadOnlySpan<byte> label, Vector2 size, ref int v, int vMin, int vMax, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) {
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (int* pV = &v) {
						return ImGuiNative.igVSliderInt(pLabel, size, pV, vMin, vMax, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
		}

		public bool VSliderScalar<T>(ReadOnlySpan<byte> label, Vector2 size, ref T data, T min, T max, ReadOnlySpan<byte> format = default, ImGuiSliderFlags flags = ImGuiSliderFlags.None) where T : struct {
			MemoryUtil.AssertUnmanaged<T>();
#pragma warning disable 8500
			unsafe {
				fixed (byte* pLabel = CheckStr(label), pFormat = CheckStr(format, "%.3f"u8)) {
					fixed (T* pV = &data) {
						return ImGuiNative.igVSliderScalar(pLabel, size, GetDataType<T>(), pV, &min, &max, pFormat, (ImGuiNET.ImGuiSliderFlags)flags) != 0;
					}
				}
			}
#pragma warning restore
		}
	}
}

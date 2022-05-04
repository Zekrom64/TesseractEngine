using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using Tesseract.Core.Math;

namespace Tesseract.ImGui.Internal {

	//==============================//
	// Publicly-facing opaque types //
	//==============================//

	public class ImGuiContext { }

	public class ImDrawListSharedData {

		internal Vector2 TexUvWhitePixel;
		internal ImFont Font;
		internal float FontSize;
		internal float CurveTessellationTol;
		internal float CircleSegmentMaxError;
		internal Vector4 ClipRectFullscreen;
		internal ImDrawListFlags InitialFlags;

		internal Vector2[] ArcFastVtx = new Vector2[ImGui.DrawListArcFastTableSize];
		internal float ArcFastRadiusCutoff;
		internal byte[] CircleSegmentCounts = new byte[64];
		internal Vector4 TexUvLines;

	}

	//======================//
	// Fully internal types //
	//======================//

	internal struct ImDrawCmdHeader {

		public Vector4 ClipRect;
		public nuint TextureId;
		public uint VtxOffset;

	}

	/*
	public class ImPool<T> where T : class, new() {

		public readonly List<T?> Buf = new();
		public readonly ImGuiStorage Map = new();
		public int FreeIdx;
		public int AliveCount;

		public T? GetByKey(int key) {
			int idx = Map.GetInt(key, -1);
			return idx != -1 ? Buf[idx] : default;
		}

		public T GetByIndex(int index) {
			T? t = Buf[index];
			if (t == null) throw new NullReferenceException();
			return t;
		}

		public int GetIndex(T t) => Buf.IndexOf(t);

		public T GetOrAddByKey(int key) {
			int idx = Map.GetInt(key, -1);
			if (idx != -1) return GetByIndex(idx);
			Map.SetInt(key, FreeIdx);
			return Add();
		}

		public bool Contains(T t) => Buf.Contains(t);

		public void Clear() {
			Map.Clear();
			Buf.Clear();
			FreeIdx = AliveCount = 0;
		}

		public T Add() {
			int idx = FreeIdx;
			if (idx == Buf.Count || idx == -1) {
				Buf.Add(new T());
			} else {
				FreeIdx = Buf.IndexOf(null);
				Buf[idx] = new T();
			}
			AliveCount++;
			return Buf[idx]!;
		}

		public void Remove()
	}
	*/

	internal struct ImGuiDataTypeTempStorage {

		public ulong Data;

	}

	internal struct ImGuiDataTypeInfo {

		public int Size;
		public string Name;
		public string PrintFmt;
		public string ScanFmt;

	}

	internal struct ImGuiColorMod {

		public uint Col;
		public Vector4 BackupValue;

	}

	internal struct ImGuiStyleMod {

		public ImGuiStyleVar VarIdx;
		public Vector2 BackupFloat;
		public Vector2i BackupInt {
			get => new(BitConverter.SingleToInt32Bits(BackupFloat.X), BitConverter.SingleToInt32Bits(BackupFloat.Y));
			set => BackupFloat = new Vector2(BitConverter.Int32BitsToSingle(value.X), BitConverter.Int32BitsToSingle(value.Y));
		}

		public ImGuiStyleMod(ImGuiStyleVar idx, int v) {
			VarIdx = idx;
			BackupFloat = new(BitConverter.Int32BitsToSingle(v), 0);
		}

		public ImGuiStyleMod(ImGuiStyleVar idx, float v) {
			VarIdx = idx;
			BackupFloat = new(v, 0);
		}

		public ImGuiStyleMod(ImGuiStyleVar idx, Vector2 v) {
			VarIdx = idx;
			BackupFloat = v;
		}

	}

	internal struct ImGuiComboPreviewData {

		public Rectf PreviewRect;
		public Vector2 BackupCursorPos;
		public Vector2 BackupCursorMaxPos;
		public Vector2 BackupCursorPosPrevLine;
		public float BackupPrevLineTextBaseOffset;
		public ImGuiLayoutType BackupLayout;

	}

	internal struct ImGuiGroupData {

		public int WindowID;
		public Vector2 BackupCursorPos;
		public Vector2 BackupCursorMaxPos;
		public float BackupIndent;
		public float BackupGroupOffset;
		public Vector2 BackupCurrLineSize;
		public float BackupCurrLineTextBaseOffset;
		public int BackupActiveIdIsAlive;
		public bool BackupActiveIdPreviousFrameIsAlive;
		public bool BackupHoveredIdIsAlive;
		public bool EmitItem;

	}

	internal struct ImGuiMenuColumns {

		public uint TotalWidth;
		public uint NextTotalWidth;
		public ushort Spacing;
		public ushort OffsetIcon;
		public ushort OffsetLabel;
		public ushort OffsetShortcut;
		public ushort OffsetMark;
		public ushort Widths0;
		public ushort Widths1;
		public ushort Widths2;
		public ushort Widths3;

		public void Update(float spacing, bool windowReappearing) {

		}

		public float DeclColumns(float wIcon, float wLabel, float wShortcut, float wMark) {

		}

		public void CalcNextTotalWidth(bool updateOffsets) {

		}

	}

	internal struct ImGuiPopupData {

		public int PopupId = default;

		public ImGuiWindow? Window = default;

		public ImGuiWindow? SourceWindow = default;

		public int OpenFrameCount = -1;

		public int OpenParentId = default;

		public Vector2 OpenPopupPos = default;

		public Vector2 OpenMousePos = default;

		public ImGuiPopupData() { }

	}

	internal struct ImGuiNextWindowData {

		public ImGuiNextWindowDataFlags Flags;

		public ImGuiCond PosCond;

		public ImGuiCond SizeCond;

		public ImGuiCond CollapsedCond;

		public Vector2 PosVal;

		public Vector2 PosPivotVal;

		public Vector2 SizeVal;

		public Vector2 ContentSizeVal;

		public Vector2 ScrollVal;

		public bool CollapsedVal;

		public Rectf SizeConstraintRect;

		public ImGuiSizeCallback SizeCallback;

		public float BgAlphaVal;

		public Vector2 MenuBarOffsetMinVal;

		public void ClearFlags() {
			Flags = ImGuiNextWindowDataFlags.None;
		}

	}

	internal struct ImGuiNextItemData {

		public ImGuiNextItemDataFlags Flags;

		public float Width;

		public int FocusScopeID;

		public ImGuiCond OpenCond;

		public bool OpenVal;

		public void ClearFlags() {
			Flags = ImGuiNextItemDataFlags.None;
		}

	}

	internal struct ImGuiLastItemData {

		public int ID;

		public ImGuiItemFlags InFlags;

		public ImGuiItemStatusFlags StatusFlags;

		public Rectf Rect;

		public Rectf NavRect;

		public Rectf DisplayRect;

	}

	internal struct ImGuiStackSizes {

		public short SizeOfIDStack;

		public short SizeOfColorStack;

		public short SizeOfStyleVarStack;

		public short SizeOfFontStack;

		public short SizeOfFocusScopeStack;

		public short SizeOfGroupStack;

		public short SizeOfItemFlagsStack;

		public short SizeOfBeginPopupStack;

		public short SizeOfDisabledStack;

		public void SetToCurrentState() {

		}

		public void CompareWithTopState() {

		}

	}

	internal struct ImGuiWindowStackData {

		public ImGuiWindow? Window;

		public ImGuiLastItemData ParentLastItemDataBackup;

		public ImGuiStackSizes StackSizesOnBegin;

	}

	internal struct ImGuiShrinkWidthItem {

		public int Index;

		public float Width;

	}

	public struct ImGuiPtrOrIndex {

		public object Ptr;

		public int Index;

	}

}

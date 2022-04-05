using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Tesseract.Core.Math;

namespace Tesseract.ImGui {

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

	internal class ImBitVector {

		public readonly List<uint> Storage = new();

		public void Create(int sz) {
			sz = (sz + 31) >> 5;
			Storage.EnsureCapacity(sz);
			Storage.AddRange(new uint[Storage.Count - sz]);
		}

		public void Clear() => Storage.Clear();

		public bool TestBit(int n) => (Storage[n >> 5] & (1 << (n & 0x1F))) != 0;

		public void SetBit(int n) {
			int i = n >> 5;
			uint u = Storage[i];
			u |= 1u << (n & 0x1F);
			Storage[i] = u;
		}

		public void ClearBit(int n) {
			int i = n >> 5;
			uint u = Storage[i];
			u &= ~(1u << (n & 0x1F));
			Storage[i] = u;
		}

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

	internal struct ImGuiInputTextState {

		public int ID;
		public StringBuilder Text;
		public float ScrollX;
		public 

	}

	//==========================//
	// Internal ImGui functions //
	//==========================//

	public static partial class ImGui {

		internal static int HashData(byte[] data, uint seed = 0) {

		}

		internal static int HashStr(string data, uint seed = 0) {

		}

		internal static void Qsort<T>(IList<T> elements, Comparison<T> compareFunc) {

		}

		internal static void Qsort<T>(IList<T> elements) where T : IComparable<T> {

		}

		internal static bool IsPowerOfTwo(int v) => BitOperations.IsPow2(v);

		internal static bool IsPowerOfTwo(ulong v) => BitOperations.IsPow2(v);

		internal static int UpperPowerOfTwo(int v) => (int)BitOperations.RoundUpToPowerOf2((uint)v);

		internal static string FormatString(string fmt, params object[] args) {

		}

		internal static int ParseFormatFindStart(string fmt) {

		}

		internal static int ParseFormatFindEnd(string fmt) {

		}

		internal static string ParseFormatTrimDecorations(string fmt) {

		}

		internal static int ParseFormatPrecision(string format, int defaultValue) {

		}

		internal static bool IsCharBlank(char c) => c == ' ' || c == '\t' || c == 0x3000;


		internal static Vector2 BezierCubicCalc(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float t) {

		}

		internal static Vector2 BezierCubicClosestPoint(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, int numSegments) {

		}

		internal static Vector2 BezierCubicClosestPointCastlejau(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float tessTol) {

		}

		internal static Vector2 BezierQuadraticCalc(Vector2 p1, Vector2 p2, Vector2 p3, float t) {

		}

		internal static Vector2 LineClosestPoint(Vector2 a, Vector2 b, Vector2 p) {

		}

		internal static int RoundUpEven(int i) => ((i + 1) << 1) >> 1;

		internal const int DrawListCircleAutoSegmentMin = 4;
		internal const int DrawListCircleAutoSegmentMax = 512;

		internal static int DrawListCircleAutoSegmentCalc(float rad, int max) {
			int nseg = (int)Math.Ceiling(Math.PI / Math.Acos(1 - Math.Min(float.Epsilon, rad) / rad));
			return Math.Clamp(RoundUpEven(nseg), DrawListCircleAutoSegmentMin, DrawListCircleAutoSegmentMax);
		}

		internal const int DrawListArcFastTableSize = 48;

	}

}

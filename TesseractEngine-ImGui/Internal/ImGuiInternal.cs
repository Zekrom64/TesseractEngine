using System.Numerics;

namespace Tesseract.ImGui.Internal {
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

using System.Numerics;
using System.Runtime.InteropServices;

namespace Tesseract.ImGui {

	[StructLayout(LayoutKind.Sequential)]
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

	/// <summary>
	/// Storage used by <see cref="GImGui.IsKeyDown(ImGuiKey)"/>, <see cref="GImGui.IsKeyPressed(ImGuiKey, bool)"/> etc functions.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiKeyData {

		/// <summary>
		/// True for if key is down.
		/// </summary>
		public bool Down = default;
		/// <summary>
		/// Duration the key has been down (<0.0f: not pressed, 0.0f: just pressed, >0.0f: time held).
		/// </summary>
		public float DownDuration = -1.0f;
		/// <summary>
		/// Last frame duration the key has been down.
		/// </summary>
		public float DownDurationPrev = -1.0f;
		/// <summary>
		/// 0.0f..1.0f for gamepad values.
		/// </summary>
		public float AnalogValue = default;

		public ImGuiKeyData() { }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiPlatformImeData {

		public bool WantVisible;
		public Vector2 InputPos;
		public float InputLineHeight;

	}

	[StructLayout(LayoutKind.Sequential)]
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
			GImGui.ColorConvertHSVToRGB(h, s, v, out Value.X, out Value.Y, out Value.Z);
			Value.W = a;
		}

		public static ImColor HSV(float h, float s, float v, float a = 1) {
			ImColor color = new();
			color.SetHSV(h, s, v, a);
			return color;
		}

	}

	public delegate void ImDrawCallback(IImDrawList parentList, in ImDrawCmd cmd);

	public struct ImDrawCmd {

		public Vector4 ClipRect;
		public nuint TextureID;
		public uint VtxOffset;
		public uint IdxOffset;
		public uint ElemCount;
		public ImDrawCallback UserCallback;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImDrawVert {

		public static readonly int SizeOf = Marshal.SizeOf<ImDrawVert>();

		public Vector2 Pos;
		public Vector2 UV;
		public uint Col;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImDrawCmdHeader {

		public Vector4 ClipRect;
		public nuint TextureID;
		public uint VtxOffset;

	}

	public struct ImFontGlyph {

		public bool Colored;
		public bool Visible;
		public int Codepoint;
		public float AdvanceX;
		public Vector2 XY0;
		public Vector2 XY1;
		public Vector2 UV0;
		public Vector2 UV1;

	}

	public interface IImVector<T> : IList<T>, IReadOnlyList<T> {

		public Span<T> AsSpan();

		public void Resize(int newSize);

	}

}

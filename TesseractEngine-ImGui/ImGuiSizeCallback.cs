using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui {

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiSizeCallbackData {

		private readonly IntPtr userData;

		public Vector2 Pos;

		public Vector2 CurrentSize;

		public Vector2 DesiredSize;

	}

	public delegate void ImGuiSizeCallback(ref ImGuiSizeCallbackData data);

}

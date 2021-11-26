using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.ImGui {

	public delegate int ImGuiInputTextCallback(in ImGuiInputTextCallbackData data);
	public delegate void ImGuiSizeCallback(in ImGuiSizeCallbackData data);
	public delegate IntPtr ImGuiMemAllocFunc(nuint sz, IntPtr userData);
	public delegate void ImGuiMemFreeFunc(IntPtr ptr, IntPtr userData);
	public delegate void ImDrawCallback(in ImDrawList parentList, in ImDrawCmd cmd);

}

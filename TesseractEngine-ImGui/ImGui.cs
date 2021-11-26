using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.ImGui {

	public class ImGuiFunctions {

		[return: NativeType("ImGuiContext*")]
		public delegate IntPtr PFN_igCreateContext([NativeType("ImFontAtlas*")] IntPtr sharedFontAtlas);
		public delegate void PFN_igDestroyContext([NativeType("ImGuiContext*")] IntPtr ctx);
		[return: NativeType("ImGuiContext*")]
		public delegate IntPtr PFN_igGetCurrentContext();
		public delegate void PFN_igSetCurrentContext([NativeType("ImGuiContext*")] IntPtr ctx);

		[return: NativeType("ImGuiIO*")]
		public delegate IntPtr PFN_igGetIO();
		[return: NativeType("ImGuiStyle*")]
		public delegate IntPtr PFN_igGetStyle();

		public delegate void PFN_igNewFrame();
		public delegate void PFN_igEndFrame();
		public delegate void PFN_igRender();
		[return: NativeType("ImDrawData*")]
		public delegate IntPtr PFN_igGetDrawData();

		public delegate void PFN_igShowDemoWindow(ref bool open);
		public delegate void PFN_igShowMetricsWindow(ref bool open);
		public delegate void PFN_igShowStackToolWindow(ref bool open);
		public delegate void PFN_igShowAboutWindow(ref bool open);
		public delegate void PFN_igShowStyleEditor([NativeType("ImGuiStyle*")] IntPtr style);
		public delegate void PFN_igShowStyleSelector([MarshalAs(UnmanagedType.LPStr)] string label);
		public delegate void PFN_igShowFontSelector([MarshalAs(UnmanagedType.LPStr)] string label);
		public delegate void PFN_igShowUserGuide();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_igGetVersion();

		public delegate void PFN_igStyleColorsDark([NativeType("ImGuiStyle*")] IntPtr style);

	}

	public class ImGui {

	}
}

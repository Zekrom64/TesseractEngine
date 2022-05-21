using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class WGLFunctions {

		public delegate bool PFN_wglCopyContext([NativeType("HGLRC")] IntPtr hglrcSource, [NativeType("HGLRC")] IntPtr hglrcDest, uint attribs);
		[return: NativeType("HGLRC")]
		public delegate IntPtr PFN_wglCreateContext([NativeType("HDC")] IntPtr hDC);
		[return: NativeType("HGLRC")]
		public delegate IntPtr PFN_wglCreateLayerContext([NativeType("HDC")] IntPtr hDC, int layerPlane);
		public delegate bool PFN_wglDeleteContext([NativeType("HGLRC")] IntPtr hglrc);
		public delegate bool PFN_wglDescribeLayerPlane([NativeType("HDC")] IntPtr hDC, int layerPlanes, int overlayUnderlayPlane, uint plpdSize, IntPtr plpd);
		[return: NativeType("HGLRC")]
		public delegate IntPtr PFN_wglGetCurrentContext();
		[return: NativeType("HDC")]
		public delegate IntPtr PFN_wglGetCurrentDC();
		public delegate int PFN_wglGetLayerPaletteEntries([NativeType("HDC")] IntPtr hDC, int layerPlane, int firstEntry, int numEntries, IntPtr pColors);
		public delegate IntPtr PFN_wglGetProcAddress([MarshalAs(UnmanagedType.LPStr)] string name);
		public delegate bool PFN_wglMakeCurrent([NativeType("HDC")] IntPtr hDC, [NativeType("HGLRC")] IntPtr hglrc);
		public delegate bool PFN_wglRealizeLayerPalette([NativeType("HDC")] IntPtr hDC, int layerPlane, bool realize);
		public delegate int PFN_wglSetLayerPaletteEntries([NativeType("HDC")] IntPtr hDC, int layerPlane, int firstEntry, int numEntries, IntPtr pColors);
		public delegate bool PFN_wglShareLists([NativeType("HGLRC")] IntPtr hglrc1, [NativeType("HGLRC")] IntPtr hglrc2);
		public delegate bool PFN_wglSwapLayerBuffers([NativeType("HDC")] IntPtr hDC, uint planes);
		public delegate bool PFN_wglUseFontBitmapsA([NativeType("HDC")] IntPtr hDC, uint firstGlyph, uint numGlyphs, uint displayList);
		public delegate bool PFN_wglUseFontOutlinesA([NativeType("HDC")] IntPtr hDC, uint firstGlyph, uint numGlyphs, uint displayList, float deviation, float extrusion, int format, IntPtr lpgmf);

		public PFN_wglCopyContext wglCopyContext;
		public PFN_wglCreateContext wglCreateContext;
		public PFN_wglCreateLayerContext wglCreateLayerContext;
		public PFN_wglDeleteContext wglDeleteContext;
		public PFN_wglDescribeLayerPlane wglDescribeLayerPlane;
		public PFN_wglGetCurrentContext wglGetCurrentContext;
		public PFN_wglGetCurrentDC wglGetCurrentDC;
		public PFN_wglGetLayerPaletteEntries wglGetLayerPaletteEntries;
		public PFN_wglGetProcAddress wglGetProcAddress;
		public PFN_wglMakeCurrent wglMakeCurrent;
		public PFN_wglRealizeLayerPalette wglRealizeLayerPalette;
		public PFN_wglSetLayerPaletteEntries wglSetLayerPaletteEntries;
		public PFN_wglShareLists wglShareLists;
		public PFN_wglSwapLayerBuffers wglSwapLayerBuffers;
		public PFN_wglUseFontBitmapsA wglUseFontBitmapsA;
		public PFN_wglUseFontOutlinesA wglUseFontOutlinesA;

	}
#nullable restore

	public class WGL : IGLObject {

		public GL GL { get; }

		public WGLFunctions Functions { get; } = new();

		public WGL(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[NativeType("HGLRC")]
		public IntPtr CurrentContext => Functions.wglGetCurrentContext();

		[NativeType("HDC")]
		public IntPtr CurrentDC => Functions.wglGetCurrentDC();

	}
}

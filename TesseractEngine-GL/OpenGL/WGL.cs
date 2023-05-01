using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	using HGLRC = IntPtr;
	using HDC = IntPtr;

	public unsafe class WGLFunctions {

		[NativeType("BOOL wglCopyContext(HGLRC hglrcSource, HGLRC hglrcDest, UINT attribs)")]
		public delegate* unmanaged<HGLRC, HGLRC, uint, bool> wglCopyContext;
		[NativeType("HGLRC wglCreateContext(HDC hDC)")]
		public delegate* unmanaged<HDC, HGLRC> wglCreateContext;
		[NativeType("HGLRC wglCreateLayerContext(HDC hDC, int layerPlane)")]
		public delegate* unmanaged<HDC, int, HGLRC> wglCreateLayerContext;
		[NativeType("BOOL wglDeleteContext(HGLRC hglrc)")]
		public delegate* unmanaged<HGLRC, bool> wglDeleteContext;
		[NativeType("BOOL wglDescribeLayerPlane(HDC hDC, int layerPlanes, int overlayUnderlayPlane, UINT plpdSize, PLLAYERPLANEDESCRIPTOR plpd)")]
		public delegate* unmanaged<HDC, int, int, uint, IntPtr, bool> wglDescribeLayerPlane;
		[NativeType("HGLRC wglGetCurrentContext()")]
		public delegate* unmanaged<HGLRC> wglGetCurrentContext;
		[NativeType("HDC wglGetCurrentDC()")]
		public delegate* unmanaged<HDC> wglGetCurrentDC;
		[NativeType("int wglGetLayerPaletteEntries(HDC hDC, int layerPlane, int firstEntry, int numEntries, COLORREF* pColors)")]
		public delegate* unmanaged<HDC, int, int, int, IntPtr, int> wglGetLayerPaletteEntries;
		[NativeType("void* wglGetProcAddress(LPCSTR name)")]
		public delegate* unmanaged<byte*, IntPtr> wglGetProcAddress;
		[NativeType("BOOL wglMakeCurrent(HDC hDC, HGLRC hglrc)")]
		public delegate* unmanaged<HDC, HGLRC, bool> wglMakeCurrent;
		[NativeType("BOOL wglRealizeLayerPalette(HDC hDC, int layerPlane, BOOL realize)")]
		public delegate* unmanaged<HDC, int, bool, bool> wglRealizeLayerPalette;
		[NativeType("int wglSetLayerPaletteEntries(HDC hDC, int layerPlane, int firstEntry, int numEntries, COLORREF* pColors)")]
		public delegate* unmanaged<HDC, int, int, int, IntPtr, int> wglSetLayerPaletteEntries;
		[NativeType("BOOL wglShareLists(HGLRC hglrc1, HGLRC hglrc2)")]
		public delegate* unmanaged<HGLRC, HGLRC, bool> wglShareLists;
		[NativeType("BOOL wglSwapLayerBuffers(HDC hDC, UINT planes)")]
		public delegate* unmanaged<HDC, uint, bool> wglSwapLayerBuffers;
		[NativeType("BOOL wglUseFontBitmapsA(HDC hDC, DWORD firstGlyph, DWORD numGlyphs, DWORD displayList)")]
		public delegate* unmanaged<HDC, uint, uint, uint, bool> wglUseFontBitmapsA;
		[NativeType("BOOL wglUseFontOutlinesA(HDC hDC, DWORD firstGlyph, DWORD numGlyphs, DWORD displayList, FLOAT deviation, FLOAT extrusion, int format, LPGLYPHMETRICSFLOAT lpgmf)")]
		public delegate* unmanaged<HDC, uint, uint, uint, float, float, int, IntPtr, bool> wglUseFontOutlinesA;

	}

	public class WGL : IGLObject {

		public GL GL { get; }

		public WGLFunctions Functions { get; } = new();

		public WGL(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		public HGLRC CurrentContext {
			get {
				unsafe {
					return Functions.wglGetCurrentContext();
				}
			}
		}

		public HDC CurrentDC {
			get {
				unsafe {
					return Functions.wglGetCurrentDC();
				}
			}
		}
	}
}

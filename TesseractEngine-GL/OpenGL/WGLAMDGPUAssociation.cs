using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	using HGLRC = IntPtr;

	public enum GLGetGPUInfoWGLAMD : int {
		Vendor = 0x1F00,
		/// <summary>
		/// Name of the GPU.
		/// </summary>
		RendererString = 0x1F01,
		/// <summary>
		/// String describing the highest supported OpenGL version.
		/// </summary>
		OpenGLVersionString = 0x1F02,
		//Not included for now. Returns an array of GPU IDs sorted by fastest blit rates for wglBlitContextFramebufferAMD
		//FastestTargetGPUs = 0x21A2,
		/// <summary>
		/// The amount of RAM available to the GPU in MB.
		/// </summary>
		RAM = 0x21A3,
		/// <summary>
		/// The GPU clock speed in MHz.
		/// </summary>
		Clock = 0x21A4,
		/// <summary>
		/// The number of 3D pipelines.
		/// </summary>
		NumPipes = 0x21A5,
		/// <summary>
		/// The number of SIMD ALUs in each shader pipe.
		/// </summary>
		NumSIMD = 0x21A6,
		/// <summary>
		/// The number of render backends.
		/// </summary>
		NumRB = 0x21A7,
		/// <summary>
		/// The number of shader parameter interpolators.
		/// </summary>
		NumSPI = 0x21A8
	}

	public unsafe class WGLAMDGPUAssociationFunctions {

		public delegate uint PFN_wglGetGPUIDsAMD(uint maxCount, [NativeType("UINT*")] IntPtr ids);
		public delegate int PFN_wglGetGPUInfoAMD(uint id, GLGetGPUInfoWGLAMD property, GLType dataType, uint size, IntPtr data);
		public delegate uint PFN_wglGetContextGPUIDAMD([NativeType("HGLRC")] IntPtr hglrc);
		[return: NativeType("HGLRC")]
		public delegate IntPtr PFN_wglCreateAssociatedContextAMD(uint id);
		[return: NativeType("HGLRC")]
		public delegate IntPtr PFN_wglCreateAssocatedContextAttribsAMD(uint id, [NativeType("HGLRC")] IntPtr hShareContext, [NativeType("const int*")] IntPtr attribList);
		public delegate bool PFN_wglDeleteAssociatedContextAMD([NativeType("HGLRC")] IntPtr hglrc);
		public delegate bool PFN_wglMakeAssociatedContextCurrentAMD([NativeType("HGLRC")] IntPtr hglrc);
		[return: NativeType("HGLRC")]
		public delegate IntPtr PFN_wglGetCurrentAssociatedContextAMD();
		public delegate void PFN_wglBlitContextFramebufferAMD([NativeType("HGLRC")] IntPtr dstCtx, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, GLBufferMask mask, GLFilter filter);

		[NativeType("UINT wglGetGPUIDsAMD(UINT maxCount, UINT* pIds)")]
		public delegate* unmanaged<uint, uint*, uint> wglGetGPUIDsAMD;
		[NativeType("int wglGetGPUInfoAMD(UINT id, int property, GLenum dataType, UINT size, void* pData)")]
		public delegate* unmanaged<uint, int, uint, uint, IntPtr, int> wglGetGPUInfoAMD;
		[NativeType("UINT wglGetContextGPUIDAMD(HGLRC hglrc)")]
		public delegate* unmanaged<HGLRC, uint> wglGetContextGPUIDAMD;
		[NativeType("HGLRC wglCreateAssociatedContextAMD(UINT id)")]
		public delegate* unmanaged<uint, HGLRC> wglCreateAssociatedContextAMD;
		[NativeType("HGLRC wglCreateAssociatedContextAttribsAMD(UINT id, HGLRC hShareContext, const int* pAttribList)")]
		public delegate* unmanaged<uint, HGLRC, int*, HGLRC> wglCreateAssocatedContextAttribsAMD;
		[NativeType("BOOL wglDeleteAssociatedContextAMD(HGLRC hglrc)")]
		public delegate* unmanaged<HGLRC, bool> wglDeleteAssociatedContextAMD;
		[NativeType("BOOL wglMakeAssociatedContextCurrentAMD(HGLRC hglrc)")]
		public delegate* unmanaged<HGLRC, bool> wglMakeAssociatedContextCurrentAMD;
		[NativeType("HGLRC wglGetCurrentAssociatedContextAMD()")]
		public delegate* unmanaged<HGLRC> wglGetCurrentAssociatedContextAMD;
		[NativeType("void wglBlitContextFramebufferAMD(HGLRC dstCtx, GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, GLbitfield mask, GLenum filter)")]
		public delegate* unmanaged<HGLRC, int, int, int, int, int, int, int, int, uint, uint, void> wglBlitContextFramebufferAMD;

	}

	public class WGLAMDGPUAssociation : IGLObject {

		public GL GL { get; }

		public WGLAMDGPUAssociationFunctions Functions { get; } = new();

		public WGLAMDGPUAssociation(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		public uint[] GPUIDs {
			get {
				unsafe {
					Span<uint> tmp = stackalloc uint[16];
					uint n;
					uint[] ids;
					fixed (uint* ptmp = tmp) {
						n = Functions.wglGetGPUIDsAMD((uint)tmp.Length, ptmp);
						if (n <= tmp.Length) {
							ids = new uint[n];
							for (int i = 0; i < n; i++) ids[i] = tmp[i];
							return ids;
						}
					}
					tmp = stackalloc uint[(int)n];
					fixed (uint* ptmp = tmp) {
						n = Functions.wglGetGPUIDsAMD((uint)tmp.Length, ptmp);
					}
					ids = new uint[n];
					for (int i = 0; i < n; i++) ids[i] = tmp[i];
					return ids;
				}
			}
		}

		public int GetGPUInfo(uint id, GLGetGPUInfoWGLAMD property, GLType dataType, uint size, IntPtr data) {
			unsafe {
				return Functions.wglGetGPUInfoAMD(id, (int)property, (uint)dataType, size, data);
			}
		}

		public T GetGPUInfo<T>(uint id, GLGetGPUInfoWGLAMD property) where T : unmanaged {
			Type t = typeof(T);
			unsafe {
				if (t == typeof(int)) {
					int i = 0;
					GetGPUInfo(id, property, GLType.Int, 1, (IntPtr)(&i));
					return (T)(object)i;
				} else if (t == typeof(uint)) {
					uint i = 0;
					GetGPUInfo(id, property, GLType.UnsignedInt, 1, (IntPtr)(&i));
					return (T)(object)i;
				} else if (t == typeof(float)) {
					float f = 0;
					GetGPUInfo(id, property, GLType.Float, 1, (IntPtr)(&f));
					return (T)(object)f;
				} else if (t == typeof(string)) {
					using MemoryStack sp = MemoryStack.Push();
					int bp = sp.Pointer;
					int n = 128;
					int ret = 0;
					UnmanagedPointer<byte> buf;
					do {
						n <<= 1; // Double buffer size every iteration
						sp.Pointer = bp; // "Free" any existing buffer
						buf = sp.Alloc<byte>(n);
						MemoryUtil.ZeroMemory(buf);
						ret = GetGPUInfo(id, property, GLType.Byte, (uint)n, buf);
					} while (ret >= n); // Repeat until the returned number of bytes is less than the buffer size
					return (T)(object)MemoryUtil.GetUTF8(buf.Ptr)!;
				} else throw new ArgumentException("Unsupported info type", nameof(T));
			}
		}

		public uint GetContextGPUID(HGLRC hglrc) {
			unsafe {
				return Functions.wglGetContextGPUIDAMD(hglrc);
			}
		}

		public HGLRC CreateAssociatedContext(uint id) {
			unsafe {
				return Functions.wglCreateAssociatedContextAMD(id);
			}
		}

		public HGLRC CreateAssociatedContextAttribs(uint id, HGLRC hShareContext, in ReadOnlySpan<int> attribs) {
			unsafe {
				fixed (int* pAttribs = attribs) {
					return Functions.wglCreateAssocatedContextAttribsAMD(id, hShareContext, pAttribs);
				}
			}
		}

		public bool DeleteAssociatedContext(HGLRC hglrc) {
			unsafe {
				return Functions.wglDeleteAssociatedContextAMD(hglrc);
			}
		}

		public bool MakeAssociatedContextCurrent(HGLRC hglrc) {
			unsafe {
				return Functions.wglMakeAssociatedContextCurrentAMD(hglrc);
			}
		}

		public HGLRC CurrentAssociatedContext {
			get {
				unsafe {
					return Functions.wglGetCurrentAssociatedContextAMD();
				}
			}
		}

		public void BlitContextFramebuffer(HGLRC dstCtx, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, GLBufferMask mask, GLFilter filter) {
			unsafe {
				Functions.wglBlitContextFramebufferAMD(dstCtx, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, (uint)mask, (uint)filter);
			}
		}
	}
}

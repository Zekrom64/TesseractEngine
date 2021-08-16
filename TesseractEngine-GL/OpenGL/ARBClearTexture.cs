using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBClearTextureFunctions {

		public delegate void PFN_glClearTexImage(uint texture, int level, uint format, uint type, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glClearTexImageARB" })]
		public PFN_glClearTexImage glClearTexImage;
		public delegate void PFN_glClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glClearTexSubImageARB" })]
		public PFN_glClearTexSubImage glClearTexSubImage;

	}

	public class ARBClearTexture : IGLObject {

		public GL GL { get; }
		public ARBClearTextureFunctions Functions { get; } = new();

		public ARBClearTexture(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearTexImage<T>(uint texture, int level, GLFormat format, GLType type, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					Functions.glClearTexImage(texture, level, (uint)format, (uint)type, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearTexSubImage<T>(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLType type, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glClearTexSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, (uint)format, (uint)type, (IntPtr)pData);
				}
			}
		}

	}
}

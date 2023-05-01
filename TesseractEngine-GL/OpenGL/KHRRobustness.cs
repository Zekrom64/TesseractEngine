using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class KHRRobustnessFunctions {

		[ExternFunction(AltNames = new string[] { "glGetGraphicsResetStatusKHR" })]
		[NativeType("GLenum glGetGraphicsResetStatus()")]
		public delegate* unmanaged<uint> glGetGraphicsResetStatus;
		[ExternFunction(AltNames = new string[] { "glReadnPixelsARB" })]
		[NativeType("void glReadnPixels(GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, GLsizei bufSize, void* pData)")]
		public delegate* unmanaged<int, int, int, int, uint, uint, int, IntPtr, void> glReadnPixels;
		// glGetnUniformfv
		// glGetnUniformiv
		// glGetnUniformuiv

	}

	public class KHRRobustness : IGLObject {

		public GL GL { get; }
		public KHRRobustnessFunctions Functions { get; } = new();

		public KHRRobustness(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		public GLGraphicsResetStatus GraphicsResetStatus {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					return (GLGraphicsResetStatus)Functions.glGetGraphicsResetStatus();
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> ReadnPixels<T>(Recti area, GLFormat format, GLType type, Span<T> data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					Functions.glReadnPixels(area.Position.X, area.Position.Y, area.Size.X, area.Size.Y, (uint)format, (uint)type, data.Length * sizeof(T), (IntPtr)pData);
				}
			}
			return data;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ReadnPixels(Recti area, GLFormat format, GLType type, int bufSize, IntPtr data) {
			unsafe {
				Functions.glReadnPixels(area.Position.X, area.Position.Y, area.Size.X, area.Size.Y, (uint)format, (uint)type, bufSize, data);
			}
		}
	}
}

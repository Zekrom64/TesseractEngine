using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class KHRRobustnessFunctions {

		public delegate uint PFN_glGetGraphicsResetStatus();
		[ExternFunction(AltNames = new string[] { "glGetGraphicsResetStatusKHR" })]
		public PFN_glGetGraphicsResetStatus glGetGraphicsResetStatus;
		public delegate void PFN_glReadnPixels(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glReadnPixelsARB" })]
		public PFN_glReadnPixels glReadnPixels;
		// glGetnUniformfv
		// glGetnUniformiv
		// glGetnUniformuiv

	}
#nullable restore

	public class KHRRobustness : IGLObject {

		public GL GL { get; }
		public KHRRobustnessFunctions Functions { get; } = new();

		public KHRRobustness(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		public GLGraphicsResetStatus GraphicsResetStatus {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLGraphicsResetStatus) Functions.glGetGraphicsResetStatus();
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
		public void ReadnPixels(Recti area, GLFormat format, GLType type, int bufSize, IntPtr data) =>
			Functions.glReadnPixels(area.Position.X, area.Position.Y, area.Size.X, area.Size.Y, (uint)format, (uint)type, bufSize, data);

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBCopyImageFunctions {

		[ExternFunction(AltNames = new string[] { "glCopyImageSubDataARB" })]
		[NativeType("void glCopyImageSubData(GLuint srcName, GLenum srcTarget, GLint srcLevel, GLint srcX, GLint srcY, GLint srcZ, GLuint dstName, GLenum dstTarget, GLint dstLevel, GLint dstX, GLint dstY, GLint dstZ, GLint width, GLint height, GLint depth)")]
		public delegate* unmanaged<uint, uint, int, int, int, int, uint, uint, int, int, int, int, int, int, int, void> glCopyImageSubData;

	}

	public class ARBCopyImage : IGLObject {

		public GL GL { get; }
		public ARBCopyImageFunctions Functions { get; } = new();

		public ARBCopyImage(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImageSubData(uint srcName, GLCopyImageTarget srcTarget, int srcLevel, Vector3i src, uint dstName, GLCopyImageTarget dstTarget, int dstLevel, Vector3i dst, Vector3i size) {
			unsafe {
				Functions.glCopyImageSubData(srcName, (uint)srcTarget, srcLevel, src.X, src.Y, src.Z, dstName, (uint)dstTarget, dstLevel, dst.X, dst.Y, dst.Z, size.X, size.Y, size.Z);
			}
		}
	}
}

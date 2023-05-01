using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBShaderImageLoadStoreFunctions {

		[ExternFunction(AltNames = new string[] { "glBindImageTextureARB" })]
		[NativeType("void glBindImageTexture(GLuint unit, GLuint texture, GLint level, GLboolean layered, GLint layer, GLbitfield access, GLenum format)")]
		public delegate* unmanaged<uint, uint, int, byte, int, uint, uint, void> glBindImageTexture;
		[ExternFunction(AltNames = new string[] { "glMemoryBarrierARB" })]
		[NativeType("void glMemoryBarrier(GLbitfield barriers)")]
		public delegate* unmanaged<uint, void> glMemoryBarrier;

	}

	public class ARBShaderImageLoadStore {

		public GL GL { get; }
		public ARBShaderImageLoadStoreFunctions Functions { get; } = new();

		public ARBShaderImageLoadStore(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindImageTexture(uint unit, uint texture, int level, bool layered, int layer, GLAccess access, GLInternalFormat format) {
			unsafe {
				Functions.glBindImageTexture(unit, texture, level, (byte)(layered ? 1 : 0), layer, (uint)access, (uint)format);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MemoryBarrier(GLMemoryBarrier barrier) {
			unsafe {
				Functions.glMemoryBarrier((uint)barrier);
			}
		}
	}

}

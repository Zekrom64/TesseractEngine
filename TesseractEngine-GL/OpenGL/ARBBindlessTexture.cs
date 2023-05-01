using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBBindlessTextureFunctions {

		[NativeType("GLuint64 glGetTextureHandleARB(GLuint texture)")]
		public delegate* unmanaged<uint, ulong> glGetTextureHandleARB;
		[NativeType("GLuint64 glGetTextureSamplerHandleARB(GLuint texture, GLuint sampler)")]
		public delegate* unmanaged<uint, uint, ulong> glGetTextureSamplerHandleARB;

		[NativeType("void glMakeTextureHandleResidentARB(GLuint64 handle)")]
		public delegate* unmanaged<ulong, void> glMakeTextureHandleResidentARB;
		[NativeType("void glMakeTextureHandleNonResidentARB(GLuint64 handle)")]
		public delegate* unmanaged<ulong, void> glMakeTextureHandleNonResidentARB;

		[NativeType("GLuint64 glGetImageHandleARB(GLuint texture, GLint level, GLboolean layered, GLint layer, GLenum format)")]
		public delegate* unmanaged<uint, int, byte, int, uint, ulong> glGetImageHandleARB;

		[NativeType("void glMakeImageHandleResidentARB(GLuint64 handle, GLbitfield access)")]
		public delegate* unmanaged<ulong, uint, void> glMakeImageHandleResidentARB;
		[NativeType("void glMakeImageHandleNonResidentARB(GLuint64 handle)")]
		public delegate* unmanaged<ulong, void> glMakeImageHandleNonResidentARB;

		[NativeType("void glUniformHandleui64ARB(GLint location, GLuint64 value)")]
		public delegate* unmanaged<int, ulong, void> glUniformHandleui64ARB;
		[NativeType("void glProgramUniformHandleui64ARB(GLuint program, GLint location, GLuint64 value)")]
		public delegate* unmanaged<uint, int, ulong, void> glProgramUniformHandleui64ARB;

		[NativeType("GLboolean glIsTextureHandleResidentARB(GLuint64 handle)")]
		public delegate* unmanaged<ulong, byte> glIsTextureHandleResidentARB;
		[NativeType("GLboolean glIsImageHandleResidentARB(GLuint64 handle)")]
		public delegate* unmanaged<ulong, byte> glIsImageHandleResidentARB;

	}

	public class ARBBindlessTexture : IGLObject {

		public GL GL { get; }
		public ARBBindlessTextureFunctions Functions { get; } = new();

		public ARBBindlessTexture(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetTextureHandle(uint texture) {
			unsafe {
				return Functions.glGetTextureHandleARB(texture);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetTextureSamplerHandle(uint texture, uint sampler) {
			unsafe {
				return Functions.glGetTextureSamplerHandleARB(texture, sampler);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MakeTextureHandleResident(ulong handle) {
			unsafe {
				Functions.glMakeTextureHandleResidentARB(handle);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MakeTextureHandleNonResident(ulong handle) {
			unsafe {
				Functions.glMakeTextureHandleNonResidentARB(handle);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetImageHandle(uint texture, int level, bool layered, int layer, GLInternalFormat format) {
			unsafe {
				return Functions.glGetImageHandleARB(texture, level, (byte)(layered ? 1 : 0), layer, (uint)format);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MakeImageHandleResident(ulong handle, GLAccess access) {
			unsafe {
				Functions.glMakeImageHandleResidentARB(handle, (uint)access);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MakeImageHandleNonResident(ulong handle) {
			unsafe {
				Functions.glMakeImageHandleNonResidentARB(handle);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformHandle(int location, ulong value) {
			unsafe {
				Functions.glUniformHandleui64ARB(location, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramUniformHandle(uint program, int location, ulong value) {
			unsafe {
				Functions.glProgramUniformHandleui64ARB(program, location, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsTextureHandleResident(ulong handle) {
			unsafe {
				return Functions.glIsTextureHandleResidentARB(handle) != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsImageHandleResident(ulong handle) {
			unsafe {
				return Functions.glIsImageHandleResidentARB(handle) != 0;
			}
		}
	}
}

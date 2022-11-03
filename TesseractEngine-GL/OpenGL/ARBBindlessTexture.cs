using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBBindlessTextureFunctions {

		public delegate ulong PFN_glGetTextureHandleARB(uint texture);
		public delegate ulong PFN_glGetTextureSamplerHandleARB(uint texture, uint sampler);

		public delegate void PFN_glMakeTextureHandleResidentARB(ulong handle);
		public delegate void PFN_glMakeTextureHandleNonResidentARB(ulong handle);

		public delegate ulong PFN_glGetImageHandleARB(uint texture, int level, byte layered, int layer, uint format);

		public delegate void PFN_glMakeImageHandleResidentARB(ulong handle, uint access);
		public delegate void PFN_glMakeImageHandleNonResidentARB(ulong handle);

		public delegate void PFN_glUniformHandleui64ARB(int location, ulong value);
		public delegate void PFN_glProgramUniformHandleui64ARB(uint program, int location, ulong value);

		public delegate byte PFN_glIsTextureHandleResidentARB(ulong handle);
		public delegate byte PFN_glIsImageHandleResidentARB(ulong handle);

		public PFN_glGetTextureHandleARB glGetTextureHandleARB;
		public PFN_glGetTextureSamplerHandleARB glGetTextureSamplerHandleARB;

		public PFN_glMakeTextureHandleResidentARB glMakeTextureHandleResidentARB;
		public PFN_glMakeTextureHandleNonResidentARB glMakeTextureHandleNonResidentARB;

		public PFN_glGetImageHandleARB glGetImageHandleARB;

		public PFN_glMakeImageHandleResidentARB glMakeImageHandleResidentARB;
		public PFN_glMakeImageHandleNonResidentARB glMakeImageHandleNonResidentARB;

		public PFN_glUniformHandleui64ARB glUniformHandleui64ARB;
		public PFN_glProgramUniformHandleui64ARB glProgramUniformHandleui64ARB;

		public PFN_glIsTextureHandleResidentARB glIsTextureHandleResidentARB;
		public PFN_glIsImageHandleResidentARB glIsImageHandleResidentARB;

	}
#nullable restore

	public class ARBBindlessTexture : IGLObject {

		public GL GL { get; }
		public ARBBindlessTextureFunctions Functions { get; } = new();

		public ARBBindlessTexture(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetTextureHandle(uint texture) => Functions.glGetTextureHandleARB(texture);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetTextureSamplerHandle(uint texture, uint sampler) => Functions.glGetTextureSamplerHandleARB(texture, sampler);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MakeTextureHandleResident(ulong handle) => Functions.glMakeTextureHandleResidentARB(handle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MakeTextureHandleNonResident(ulong handle) => Functions.glMakeTextureHandleNonResidentARB(handle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetImageHandle(uint texture, int level, bool layered, int layer, GLInternalFormat format) =>
			Functions.glGetImageHandleARB(texture, level, (byte)(layered ? 1 : 0), layer, (uint)format);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MakeImageHandleResident(ulong handle, GLAccess access) => Functions.glMakeImageHandleResidentARB(handle, (uint)access);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MakeImageHandleNonResident(ulong handle) => Functions.glMakeImageHandleNonResidentARB(handle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformHandle(int location, ulong value) => Functions.glUniformHandleui64ARB(location, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramUniformHandle(uint program, int location, ulong value) => Functions.glProgramUniformHandleui64ARB(program, location, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsTextureHandleResident(ulong handle) => Functions.glIsTextureHandleResidentARB(handle) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsImageHandleResident(ulong handle) => Functions.glIsImageHandleResidentARB(handle) != 0;

	}
}

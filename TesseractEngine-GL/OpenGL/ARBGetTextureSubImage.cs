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
	public class ARBGetTextureSubImageFunctions {

		public delegate void PFN_glGetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, int bufSize, IntPtr pixels);
		[ExternFunction(AltNames = new string[] { "glGetTextureSubImageARB" })]
		public PFN_glGetTextureSubImage glGetTextureSubImage;
		public delegate void PFN_glGetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, IntPtr pixels);
		[ExternFunction(AltNames = new string[] { "glGetCompressedTextureSubImageARB" })]
		public PFN_glGetCompressedTextureSubImage glGetCompressedTextureSubImage;

	}
#nullable restore

	public class ARBGetTextureSubImage : IGLObject {

		public GL GL { get; }
		public ARBGetTextureSubImageFunctions Functions { get; } = new();

		public ARBGetTextureSubImage(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetTextureSubImage<T>(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLTextureType type, Span<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					Functions.glGetTextureSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, (uint)format, (uint)type, pixels.Length * sizeof(T), (IntPtr)pPixels);
				}
			}
			return pixels;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetTextureSubImage(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLTextureType type, int bufSize, IntPtr pixels) =>
				Functions.glGetTextureSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, (uint)format, (uint)type, bufSize, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetCompressedTextureSubImage<T>(uint texture, int level, Vector3i offset, Vector3i size, Span<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					Functions.glGetCompressedTextureSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, pixels.Length * sizeof(T), (IntPtr)pPixels);
				}
			}
			return pixels;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetCompressedTextureSubImage(uint texture, int level, Vector3i offset, Vector3i size, int bufSize, IntPtr pixels) =>
			Functions.glGetCompressedTextureSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, bufSize, pixels);

	}
}

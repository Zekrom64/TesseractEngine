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
	public class ARBMultiBindFunctions {

		public delegate void PFN_glBindBuffersBase(uint target, uint first, int count, [NativeType("const GLuint*")] IntPtr buffers);
		[ExternFunction(AltNames = new string[] { "glBindBuffersBaseARB" })]
		public PFN_glBindBuffersBase glBindBuffersBase;
		public delegate void PFN_glBindBuffersRange(uint target, uint first, int count, [NativeType("const GLuint*")] IntPtr buffers, [NativeType("const GLintptr*")] IntPtr offsets, [NativeType("const GLsizeiptr*")] IntPtr sizes);
		[ExternFunction(AltNames = new string[] { "glBindBuffersRangeARB" })]
		public PFN_glBindBuffersRange glBindBuffersRange;
		public delegate void PFN_glBindTextures(uint first, int count, [NativeType("const GLuint*")] IntPtr textures);
		[ExternFunction(AltNames = new string[] { "glBindTexturesARB" })]
		public PFN_glBindTextures glBindTextures;
		public delegate void PFN_glBindSamplers(uint first, int count, [NativeType("const GLuint*")] IntPtr samples);
		[ExternFunction(AltNames = new string[] { "glBindSamplersARB" })]
		public PFN_glBindSamplers glBindSamplers;
		public delegate void PFN_glBindImageTextures(uint first, int count, [NativeType("const GLuint*")] IntPtr textures);
		[ExternFunction(AltNames = new string[] { "glBindImageTexturesARB" })]
		public PFN_glBindImageTextures glBindImageTextures;
		public delegate void PFN_glBindVertexBuffers(uint first, int count, [NativeType("const GLuint*")] IntPtr buffers, [NativeType("const GLintptr*")] IntPtr offsets, [NativeType("const GLsizei*")] IntPtr strides);
		[ExternFunction(AltNames = new string[] { "glBindVertexBuffersARB" })]
		public PFN_glBindVertexBuffers glBindVertexBuffers;

	}
#nullable restore

	public class ARBMultiBind : IGLObject {

		public GL GL { get; }
		public ARBMultiBindFunctions Functions { get; } = new();

		public ARBMultiBind(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffersBase(GLBufferRangeTarget target, uint first, in ReadOnlySpan<uint> buffers) {
			unsafe {
				fixed(uint* pBuffers = buffers) {
					Functions.glBindBuffersBase((uint)target, first, buffers.Length, (IntPtr)pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffersBase(GLBufferRangeTarget target, uint first, params uint[] buffers) {
			unsafe {
				fixed (uint* pBuffers = buffers) {
					Functions.glBindBuffersBase((uint)target, first, buffers.Length, (IntPtr)pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffersRange(GLBufferRangeTarget target, uint first, in ReadOnlySpan<uint> buffers, in ReadOnlySpan<nint> offsets, in ReadOnlySpan<nint> sizes) {
			int count = ExMath.Min(buffers.Length, offsets.Length, sizes.Length);
			unsafe {
				fixed(uint* pBuffers = buffers) {
					fixed(nint* pOffsets = offsets, pSizes = sizes) {
						Functions.glBindBuffersRange((uint)target, first, count, (IntPtr)pBuffers, (IntPtr)pOffsets, (IntPtr)pSizes);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindImageTextures(uint first, in ReadOnlySpan<uint> textures) {
			unsafe {
				fixed(uint* pTextures = textures) {
					Functions.glBindImageTextures(first, textures.Length, (IntPtr)pTextures);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindImageTextures(uint first, params uint[] textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					Functions.glBindImageTextures(first, textures.Length, (IntPtr)pTextures);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSamplers(uint first, in ReadOnlySpan<uint> samplers) {
			unsafe {
				fixed(uint* pSamplers = samplers) {
					Functions.glBindSamplers(first, samplers.Length, (IntPtr)pSamplers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSamplers(uint first, params uint[] samplers) {
			unsafe {
				fixed (uint* pSamplers = samplers) {
					Functions.glBindSamplers(first, samplers.Length, (IntPtr)pSamplers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTextures(uint first, in ReadOnlySpan<uint> textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					Functions.glBindTextures(first, textures.Length, (IntPtr)pTextures);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTextures(uint first, params uint[] textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					Functions.glBindTextures(first, textures.Length, (IntPtr)pTextures);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexBuffers(uint first, in ReadOnlySpan<uint> buffers, in ReadOnlySpan<nint> offsets, in ReadOnlySpan<int> strides) {
			int count = ExMath.Min(buffers.Length, offsets.Length, strides.Length);
			unsafe {
				fixed(uint* pBuffers = buffers) {
					fixed(nint* pOffsets = offsets) {
						fixed(int* pStrides = strides) {
							Functions.glBindVertexBuffers(first, count, (IntPtr)pBuffers, (IntPtr)pOffsets, (IntPtr)pStrides);
						}
					}
				}
			}
		}

	}
}

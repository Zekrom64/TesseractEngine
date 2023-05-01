using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBMultiBindFunctions {

		[ExternFunction(AltNames = new string[] { "glBindBuffersBaseARB" })]
		[NativeType("void glBindBuffersBase(GLenum target, GLuint first, GLsizei count, const GLuint* pBuffers)")]
		public delegate* unmanaged<uint, uint, int, uint*, void> glBindBuffersBase;
		[ExternFunction(AltNames = new string[] { "glBindBuffersRangeARB" })]
		[NativeType("void glBindBuffersRange(GLuint target, GLuint first, GLsizei count, const GLuint* pBuffers, const GLintptr* pOffsets, const GLsizeiptr* pSizes)")]
		public delegate* unmanaged<uint, uint, int, uint*, nint*, nint*, void> glBindBuffersRange;
		[ExternFunction(AltNames = new string[] { "glBindTexturesARB" })]
		[NativeType("void glBindTextures(GLuint first, GLsizei count, const GLuint* pTextures)")]
		public delegate* unmanaged<uint, int, uint*, void> glBindTextures;
		[ExternFunction(AltNames = new string[] { "glBindSamplersARB" })]
		[NativeType("void glBindSamplers(GLuint first, GLsizei count, const GLuint* pSamplers)")]
		public delegate* unmanaged<uint, int, uint*, void> glBindSamplers;
		[ExternFunction(AltNames = new string[] { "glBindImageTexturesARB" })]
		[NativeType("void glBindImageTextures(GLuint first, GLsizei count, const GLuint* pTextures)")]
		public delegate* unmanaged<uint, int, uint*, void> glBindImageTextures;
		[ExternFunction(AltNames = new string[] { "glBindVertexBuffersARB" })]
		[NativeType("void glBindVertexBuffers(GLuint first, GLsizei count, const GLuint* pBuffers, const GLintptr* pOffsets, const GLsizei* pStrides)")]
		public delegate* unmanaged<uint, int, uint*, nint*, int*, void> glBindVertexBuffers;

	}

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
					Functions.glBindBuffersBase((uint)target, first, buffers.Length, pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffersBase(GLBufferRangeTarget target, uint first, params uint[] buffers) {
			unsafe {
				fixed (uint* pBuffers = buffers) {
					Functions.glBindBuffersBase((uint)target, first, buffers.Length, pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffersRange(GLBufferRangeTarget target, uint first, in ReadOnlySpan<uint> buffers, in ReadOnlySpan<nint> offsets, in ReadOnlySpan<nint> sizes) {
			int count = ExMath.Min(buffers.Length, offsets.Length, sizes.Length);
			unsafe {
				fixed(uint* pBuffers = buffers) {
					fixed(nint* pOffsets = offsets, pSizes = sizes) {
						Functions.glBindBuffersRange((uint)target, first, count, pBuffers, pOffsets, pSizes);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindImageTextures(uint first, in ReadOnlySpan<uint> textures) {
			unsafe {
				fixed(uint* pTextures = textures) {
					Functions.glBindImageTextures(first, textures.Length, pTextures);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindImageTextures(uint first, params uint[] textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					Functions.glBindImageTextures(first, textures.Length, pTextures);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSamplers(uint first, in ReadOnlySpan<uint> samplers) {
			unsafe {
				fixed(uint* pSamplers = samplers) {
					Functions.glBindSamplers(first, samplers.Length, pSamplers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSamplers(uint first, params uint[] samplers) {
			unsafe {
				fixed (uint* pSamplers = samplers) {
					Functions.glBindSamplers(first, samplers.Length, pSamplers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTextures(uint first, in ReadOnlySpan<uint> textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					Functions.glBindTextures(first, textures.Length, pTextures);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTextures(uint first, params uint[] textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					Functions.glBindTextures(first, textures.Length, pTextures);
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
							Functions.glBindVertexBuffers(first, count, pBuffers, pOffsets, pStrides);
						}
					}
				}
			}
		}

	}
}

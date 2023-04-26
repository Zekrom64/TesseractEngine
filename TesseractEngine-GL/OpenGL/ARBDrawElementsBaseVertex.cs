using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBDrawElementsBaseVertexFunctions {

		[ExternFunction(AltNames = new string[] { "glDrawElementsBaseVertexARB" })]
		[NativeType("void glDrawElementsBaseVertex(GLenum mode, GLsizei count, GLenum type, void* pIndices, GLint baseVertex)")]
		public delegate* unmanaged<uint, int, uint, IntPtr, int, void> glDrawElementsBaseVertex;
		[ExternFunction(AltNames = new string[] { "glDrawRangeElementsBaseVertexARB" })]
		[NativeType("void glDrawRangeElementsBaseVertex(GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, void* pIndices, GLint baseVertex)")]
		public delegate* unmanaged<uint, uint, uint, int, uint, IntPtr, int, void> glDrawRangeElementsBaseVertex;
		[ExternFunction(AltNames = new string[] { "glDrawElementsInstancedBaseVertexARB" })]
		[NativeType("void glDrawElementsInstancedBaseVertex(GLenum mode, GLsizei count, GLenum type, void* pIndices, GLsizei instanceCount, GLint baseVertex)")]
		public delegate* unmanaged<uint, int, uint, IntPtr, int, int, void> glDrawElementsInstancedBaseVertex;
		[ExternFunction(AltNames = new string[] { "glMultiDrawElementsBaseVertexARB" })]
		[NativeType("void glMultiDrawElementsBaseVertex(GLenum mode, const GLsizei* pCount, GLenum type, const void* const* ppIndices, GLsizei drawCount, const GLint* pBaseVertex)")]
		public delegate* unmanaged<uint, int*, uint, IntPtr*, int, int*, void> glMultiDrawElementsBaseVertex;

	}

	public class ARBDrawElementsBaseVertex : IGLObject {

		public GL GL { get; }
		public ARBDrawElementsBaseVertexFunctions Functions { get; } = new();

		public ARBDrawElementsBaseVertex(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsBaseVertex(GLDrawMode mode, int count, GLIndexType type, nint offset, int basevertex) {
			unsafe {
				Functions.glDrawElementsBaseVertex((uint)mode, count, (uint)type, offset, basevertex);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawRangeElementsBaseVertex(GLDrawMode mode, uint start, uint end, int count, GLIndexType type, nint offset, int basevertex) {
			unsafe {
				Functions.glDrawRangeElementsBaseVertex((uint)mode, start, end, count, (uint)type, offset, basevertex);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstancedBaseVertex(GLDrawMode mode, int count, GLIndexType type, nint offset, int instancecount, int basevertex) {
			unsafe {
				Functions.glDrawElementsInstancedBaseVertex((uint)mode, count, (uint)type, offset, instancecount, basevertex);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElementsBaseVertex(GLDrawMode mode, in ReadOnlySpan<int> count, GLIndexType type, in ReadOnlySpan<nint> offsets, int drawCount, in ReadOnlySpan<int> baseVertex) {
			unsafe {
				fixed(int* pCount = count) {
					fixed(nint* pOffsets = offsets) {
						fixed(int* pBaseVertices = baseVertex) {
							Functions.glMultiDrawElementsBaseVertex((uint)mode, pCount, (uint)type, pOffsets, drawCount, pBaseVertices);
						}
					}
				}
			}
		}

	}

}

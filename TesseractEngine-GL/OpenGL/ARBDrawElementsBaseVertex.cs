using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBDrawElementsBaseVertexFunctions {

		public delegate void PFN_glDrawElementsBaseVertex(uint mode, int count, uint type, IntPtr indices, int basevertex);
		[ExternFunction(AltNames = new string[] { "glDrawElementsBaseVertexARB" })]
		public PFN_glDrawElementsBaseVertex glDrawElementsBaseVertex;
		public delegate void PFN_glDrawRangeElementsBaseVertex(uint mode, uint start, uint end, int count, uint type, IntPtr indices, int basevertex);
		[ExternFunction(AltNames = new string[] { "glDrawRangeElementsBaseVertexARB" })]
		public PFN_glDrawRangeElementsBaseVertex glDrawRangeElementsBaseVertex;
		public delegate void PFN_glDrawElementsInstancedBaseVertex(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex);
		[ExternFunction(AltNames = new string[] { "glDrawElementsInstancedBaseVertexARB" })]
		public PFN_glDrawElementsInstancedBaseVertex glDrawElementsInstancedBaseVertex;
		public delegate void PFN_glMultiDrawElementsBaseVertex(uint mode, [NativeType("const GLsizei*")] IntPtr count, uint type, [NativeType("const void* const*")] IntPtr indices, int drawcount, [NativeType("const GLint*")] IntPtr basevertex);
		[ExternFunction(AltNames = new string[] { "glMultiDrawElementsBaseVertexARB" })]
		public PFN_glMultiDrawElementsBaseVertex glMultiDrawElementsBaseVertex;

	}
#nullable restore

	public class ARBDrawElementsBaseVertex : IGLObject {

		public GL GL { get; }
		public ARBDrawElementsBaseVertexFunctions Functions { get; } = new();

		public ARBDrawElementsBaseVertex(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsBaseVertex(GLDrawMode mode, int count, GLIndexType type, nint offset, int basevertex) => Functions.glDrawElementsBaseVertex((uint)mode, count, (uint)type, offset, basevertex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawRangeElementsBaseVertex(GLDrawMode mode, uint start, uint end, int count, GLIndexType type, nint offset, int basevertex) => Functions.glDrawRangeElementsBaseVertex((uint)mode, start, end, count, (uint)type, offset, basevertex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstancedBaseVertex(GLDrawMode mode, int count, GLIndexType type, nint offset, int instancecount, int basevertex) => Functions.glDrawElementsInstancedBaseVertex((uint)mode, count, (uint)type, offset, instancecount, basevertex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElementsBaseVertex(GLDrawMode mode, in ReadOnlySpan<int> count, GLIndexType type, in ReadOnlySpan<nint> offsets, int drawCount, in ReadOnlySpan<int> baseVertex) {
			unsafe {
				fixed(int* pCount = count) {
					fixed(nint* pOffsets = offsets) {
						fixed(int* pBaseVertices = baseVertex) {
							Functions.glMultiDrawElementsBaseVertex((uint)mode, (IntPtr)pCount, (uint)type, (IntPtr)pOffsets, drawCount, (IntPtr)pBaseVertices);
						}
					}
				}
			}
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTransformFeedbackInstancedFunctions {

		[ExternFunction(AltNames = new string[] { "glDrawTransformFeedbackInstancedARB" })]
		[NativeType("void glDrawTransformFeedbackInstanced(GLenum mode, GLuint id, GLsizei primCount)")]
		public delegate* unmanaged<uint, uint, int, void> glDrawTransformFeedbackInstanced;
		[ExternFunction(AltNames = new string[] { "glDrawTransformFeedbackStreamInstancedARB" })]
		[NativeType("void glDrawTransformFeedbackStreamInstanced(GLenum mode, GLuint id, GLuint stream, GLsizei primCount)")]
		public delegate* unmanaged<uint, uint, uint, int, void> glDrawTransformFeedbackStreamInstanced;

	}

	public class ARBTransformFeedbackInstanced : IGLObject {

		public GL GL { get; }
		public ARBTransformFeedbackInstancedFunctions Functions { get; } = new();

		public ARBTransformFeedbackInstanced(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedbackInstanced(GLDrawMode mode, uint id, int primCount) {
			unsafe {
				Functions.glDrawTransformFeedbackInstanced((uint)mode, id, primCount);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedbackStreamInstanced(GLDrawMode mode, uint id, uint stream, int primCount) {
			unsafe {
				Functions.glDrawTransformFeedbackStreamInstanced((uint)mode, id, stream, primCount);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTransformFeedback3Functions {

		[ExternFunction(AltNames = new string[] { "glDrawTransformFeedbackStreamARB" })]
		[NativeType("void glDrawTransformFeedbackStream(GLenum mode, GLuint id, GLuint stream)")]
		public delegate* unmanaged<uint, uint, uint, void> glDrawTransformFeedbackStream;
		[ExternFunction(AltNames = new string[] { "glBeginQueryIndexedARB" })]
		[NativeType("void glBeginQueryIndexed(GLenum target, GLuint index, GLuint id)")]
		public delegate* unmanaged<uint, uint, uint, void> glBeginQueryIndexed;
		[ExternFunction(AltNames = new string[] { "glEndQueryIndexedARB" })]
		[NativeType("void glEndQueryIndexed(GLenum target, GLuint index)")]
		public delegate* unmanaged<uint, uint, void> glEndQueryIndexed;
		[ExternFunction(AltNames = new string[] { "gGetQueryIndexedivARB" })]
		[NativeType("void glGetQueryIndexediv(GLenum target, GLuint index, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, uint, int*, void> glGetQueryIndexediv;

	}

	public class ARBTransformFeedback3 : IGLObject {

		public GL GL { get; }
		public ARBTransformFeedback3Functions Functions { get; } = new();

		public ARBTransformFeedback3(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedbackStream(GLDrawMode mode, uint id, uint stream) {
			unsafe {
				Functions.glDrawTransformFeedbackStream((uint)mode, id, stream);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginQueryIndexed(GLQueryTarget target, uint index, uint id) {
			unsafe {
				Functions.glBeginQueryIndexed((uint)target, index, id);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndQueryIndexed(GLQueryTarget target, uint index) {
			unsafe {
				Functions.glEndQueryIndexed((uint)target, index);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetQueryIndexed(GLQueryTarget target, uint index, GLGetQuery pname) {
			int value = 0;
			unsafe {
				Functions.glGetQueryIndexediv((uint)target, index, (uint)pname, &value);
			}
			return value;
		}

	}

}

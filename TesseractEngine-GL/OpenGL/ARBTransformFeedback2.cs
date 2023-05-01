using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTransformFeedback2Functions {

		[ExternFunction(AltNames = new string[] { "glBindTransformFeedbackARB" })]
		[NativeType("void glBindTransformFeedback(GLenum target, GLuint feedback)")]
		public delegate* unmanaged<uint, uint, void> glBindTransformFeedback;
		[ExternFunction(AltNames = new string[] { "glDeleteTransformFeedbacksARB" })]
		[NativeType("void glDeleteTransformFeedbacks(GLsizei n, const GLuint* pFeedbacks)")]
		public delegate* unmanaged<int, uint*, void> glDeleteTransformFeedbacks;
		[ExternFunction(AltNames = new string[] { "glGenTransformFeedbacksARB" })]
		[NativeType("void glGenTransformFeedbacks(GLsizei n, GLuint* pFeedbacks)")]
		public delegate* unmanaged<int, uint*, void> glGenTransformFeedbacks;
		[ExternFunction(AltNames = new string[] { "glIsTransformFeedbackARB" })]
		[NativeType("GLboolean glIsTransformFeedback(GLuint feedback)")]
		public delegate* unmanaged<uint, byte> glIsTransformFeedback;
		[ExternFunction(AltNames = new string[] { "glPauseTransformFeedbackARB" })]
		[NativeType("void glPauseTransformFeedback()")]
		public delegate* unmanaged<void> glPauseTransformFeedback;
		[ExternFunction(AltNames = new string[] { "glResumeTransformFeedbackARB" })]
		[NativeType("void glResumeTransformFeedback()")]
		public delegate* unmanaged<void> glResumeTransformFeedback;
		[ExternFunction(AltNames = new string[] { "glDrawTransformFeedbackARB" })]
		[NativeType("void glDrawTransformFeedback(GLenum mode, GLuint id)")]
		public delegate* unmanaged<uint, uint, void> glDrawTransformFeedback;

	}

	public class ARBTransformFeedback2 : IGLObject {

		public GL GL { get; }
		public ARBTransformFeedback2Functions Functions { get; } = new();

		public ARBTransformFeedback2(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTransformFeedback(GLTransformFeedbackTarget target, uint id) {
			unsafe {
				Functions.glBindTransformFeedback((uint)target, id);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTransformFeedbacks(in ReadOnlySpan<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glDeleteTransformFeedbacks(ids.Length, pIds);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTransformFeedbacks(params uint[] ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glDeleteTransformFeedbacks(ids.Length, pIds);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTransformFeedbacks(uint id) {
			unsafe {
				Functions.glDeleteTransformFeedbacks(1, &id);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenTransformFeedbacks(Span<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glGenTransformFeedbacks(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenTransformFeedbacks(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glGenTransformFeedbacks(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenTransformFeedbacks() {
			uint id = 0;
			unsafe {
				Functions.glGenTransformFeedbacks(1, &id);
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsTransformFeedback(uint id) {
			unsafe {
				return Functions.glIsTransformFeedback(id) != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PauseTransformFeedback() {
			unsafe {
				Functions.glPauseTransformFeedback();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResumeTransformFeedback() {
			unsafe {
				Functions.glResumeTransformFeedback();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedback(GLDrawMode mode, uint id) {
			unsafe {
				Functions.glDrawTransformFeedback((uint)mode, id);
			}
		}
	}
}

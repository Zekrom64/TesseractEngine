using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.GL {

	public class ARBTransformFeedback2Functions {

		public delegate void PFN_glBindTransformFeedback(uint target, uint id);
		[ExternFunction(AltNames = new string[] { "glBindTransformFeedbackARB" })]
		public PFN_glBindTransformFeedback glBindTransformFeedback;
		public delegate void PFN_glDeleteTransformFeedbacks(int n, [NativeType("const GLuint*")] IntPtr ids);
		[ExternFunction(AltNames = new string[] { "glDeleteTransformFeedbacksARB" })]
		public PFN_glDeleteTransformFeedbacks glDeleteTransformFeedbacks;
		public delegate void PFN_glGenTransformFeedbacks(int n, [NativeType("GLuint*")] IntPtr ids);
		[ExternFunction(AltNames = new string[] { "glGenTransformFeedbacksARB" })]
		public PFN_glGenTransformFeedbacks glGenTransformFeedbacks;
		public delegate byte PFN_glIsTransformFeedback(uint id);
		[ExternFunction(AltNames = new string[] { "glIsTransformFeedbackARB" })]
		public PFN_glIsTransformFeedback glIsTransformFeedback;
		public delegate void PFN_glPauseTransformFeedback();
		[ExternFunction(AltNames = new string[] { "glPauseTransformFeedbackARB" })]
		public PFN_glPauseTransformFeedback glPauseTransformFeedback;
		public delegate void PFN_glResumeTransformFeedback();
		[ExternFunction(AltNames = new string[] { "glResumeTransformFeedbackARB" })]
		public PFN_glResumeTransformFeedback glResumeTransformFeedback;
		public delegate void PFN_glDrawTransformFeedback(uint mode, uint id);
		[ExternFunction(AltNames = new string[] { "glDrawTransformFeedbackARB" })]
		public PFN_glDrawTransformFeedback glDrawTransformFeedback;

	}

	public class ARBTransformFeedback2 : IGLObject {

		public GL GL { get; }
		public ARBTransformFeedback2Functions Functions { get; } = new();

		public ARBTransformFeedback2(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTransformFeedback(GLTransformFeedbackTarget target, uint id) => Functions.glBindTransformFeedback((uint)target, id);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTransformFeedbacks(in ReadOnlySpan<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glDeleteTransformFeedbacks(ids.Length, (IntPtr)pIds);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTransformFeedbacks(params uint[] ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glDeleteTransformFeedbacks(ids.Length, (IntPtr)pIds);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTransformFeedbacks(uint id) {
			unsafe {
				Functions.glDeleteTransformFeedbacks(1, (IntPtr)(&id));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenTransformFeedbacks(Span<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glGenTransformFeedbacks(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenTransformFeedbacks(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glGenTransformFeedbacks(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenTransformFeedbacks() {
			uint id = 0;
			unsafe {
				Functions.glGenTransformFeedbacks(1, (IntPtr)(&id));
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsTransformFeedback(uint id) => Functions.glIsTransformFeedback(id) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PauseTransformFeedback() => Functions.glPauseTransformFeedback();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResumeTransformFeedback() => Functions.glResumeTransformFeedback();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedback(GLDrawMode mode, uint id) => Functions.glDrawTransformFeedback((uint)mode, id);

	}
}

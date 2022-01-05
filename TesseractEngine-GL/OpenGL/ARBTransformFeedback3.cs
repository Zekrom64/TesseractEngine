using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBTransformFeedback3Functions {

		public delegate void PFN_glDrawTransformFeedbackStream(uint mode, uint id, uint stream);
		[ExternFunction(AltNames = new string[] { "glDrawTransformFeedbackStreamARB" })]
		public PFN_glDrawTransformFeedbackStream glDrawTransformFeedbackStream;
		public delegate void PFN_glBeginQueryIndexed(uint target, uint index, uint id);
		[ExternFunction(AltNames = new string[] { "glBeginQueryIndexedARB" })]
		public PFN_glBeginQueryIndexed glBeginQueryIndexed;
		public delegate void PFN_glEndQueryIndexed(uint target, uint index);
		[ExternFunction(AltNames = new string[] { "glEndQueryIndexedARB" })]
		public PFN_glEndQueryIndexed glEndQueryIndexed;
		public delegate void PFN_glGetQueryIndexediv(uint target, uint index, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "gGetQueryIndexedivARB" })]
		public PFN_glGetQueryIndexediv glGetQueryIndexediv;

	}
#nullable restore

	public class ARBTransformFeedback3 : IGLObject {

		public GL GL { get; }
		public ARBTransformFeedback3Functions Functions { get; } = new();

		public ARBTransformFeedback3(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedbackStream(GLDrawMode mode, uint id, uint stream) => Functions.glDrawTransformFeedbackStream((uint)mode, id, stream);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginQueryIndexed(GLQueryTarget target, uint index, uint id) => Functions.glBeginQueryIndexed((uint)target, index, id);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndQueryIndexed(GLQueryTarget target, uint index) => Functions.glEndQueryIndexed((uint)target, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetQueryIndexed(GLQueryTarget target, uint index, GLGetQuery pname) {
			int value = 0;
			unsafe {
				Functions.glGetQueryIndexediv((uint)target, index, (uint)pname, (IntPtr)(&value));
			}
			return value;
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBTransformFeedbackInstancedFunctions {

		public delegate void PFN_glDrawTransformFeedbackInstanced(uint mode, uint id, int primcount);
		[ExternFunction(AltNames = new string[] { "glDrawTransformFeedbackInstancedARB" })]
		public PFN_glDrawTransformFeedbackInstanced glDrawTransformFeedbackInstanced;
		public delegate void PFN_glDrawTransformFeedbackStreamInstanced(uint mode, uint id, uint stream, int primcount);
		[ExternFunction(AltNames = new string[] { "glDrawTransformFeedbackStreamInstancedARB" })]
		public PFN_glDrawTransformFeedbackStreamInstanced glDrawTransformFeedbackStreamInstanced;

	}

	public class ARBTransformFeedbackInstanced : IGLObject {

		public GL GL { get; }
		public ARBTransformFeedbackInstancedFunctions Functions { get; } = new();

		public ARBTransformFeedbackInstanced(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedbackInstanced(GLDrawMode mode, uint id, int primCount) => Functions.glDrawTransformFeedbackInstanced((uint)mode, id, primCount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedbackStreamInstanced(GLDrawMode mode, uint id, uint stream, int primCount) => Functions.glDrawTransformFeedbackStreamInstanced((uint)mode, id, stream, primCount);

	}
}

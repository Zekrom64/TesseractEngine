using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBBaseInstanceFunctions {

		public delegate void PFN_glDrawArraysInstancedBaseInstance(uint mode, int first, int count, int primcount, uint baseinstance);
		[ExternFunction(AltNames = new string[] { "glDrawArraysInstancedBaseInstanceARB" })]
		public PFN_glDrawArraysInstancedBaseInstance glDrawArraysInstancedBaseInstance;
		public delegate void PFN_glDrawElementsInstancedBaseInstance(uint mode, int count, uint type, IntPtr indices, int primcount, uint baseinstance);
		[ExternFunction(AltNames = new string[] { "glDrawElementsInstancedBaseInstanceARB" })]
		public PFN_glDrawElementsInstancedBaseInstance glDrawElementsInstancedBaseInstance;
		public delegate void PFN_glDrawElementsInstancedBaseVertexBaseInstance(uint mode, int count, uint type, IntPtr indices, int primcount, int basevertex, uint baseinstance);
		[ExternFunction(AltNames = new string[] { "glDrawElementsInstancedBaseVertexBaseInstanceARB" })]
		public PFN_glDrawElementsInstancedBaseVertexBaseInstance glDrawElementsInstancedBaseVertexBaseInstance;

	}

	public class ARBBaseInstance : IGLObject {

		public GL GL { get; }
		public ARBBaseInstanceFunctions Functions { get; } = new();

		public ARBBaseInstance(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArraysInstancedBaseInstance(GLDrawMode mode, int first, int count, int primCount, uint baseInstance) => Functions.glDrawArraysInstancedBaseInstance((uint)mode, first, count, primCount, baseInstance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstancedBaseInstance(GLDrawMode mode, int count, GLIndexType type, nint offset, int primCount, uint baseInstance) => Functions.glDrawElementsInstancedBaseInstance((uint)mode, count, (uint)type, offset, primCount, baseInstance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstancedBaseVertexBaseInstance(GLDrawMode mode, int count, GLIndexType type, nint offset, int primCount, int baseVertex, uint baseInstance) => Functions.glDrawElementsInstancedBaseVertexBaseInstance((uint)mode, count, (uint)type, offset, primCount, baseVertex, baseInstance);

	}

}

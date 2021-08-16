using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBVertexAttribBindingFunctions {

		public delegate void PFN_glBindVertexBuffer(uint bindingIndex, uint buffer, nint offset, int stride);
		[ExternFunction(AltNames = new string[] { "glBindVertexBufferARB" })]
		public PFN_glBindVertexBuffer glBindVertexBuffer;
		public delegate void PFN_glVertexAttribFormat(uint attribIndex, int size, uint type, byte normalized, uint relativeOffset);
		[ExternFunction(AltNames = new string[] { "glVertexAttribFormat" })]
		public PFN_glVertexAttribFormat glVertexAttribFormat;
		public delegate void PFN_glVertexAttribIFormat(uint attribIndex, int size, uint type, uint relativeOffset);
		[ExternFunction(AltNames = new string[] { "glVertexAttribIFormat" })]
		public PFN_glVertexAttribIFormat glVertexAttribIFormat;
		public delegate void PFN_glVertexAttribLFormat(uint attribIndex, int size, uint type, uint relativeOffset);
		[ExternFunction(AltNames = new string[] { "glVertexAttribLFormat" })]
		public PFN_glVertexAttribLFormat glVertexAttribLFormat;
		public delegate void PFN_glVertexAttribBinding(uint attribIndex, uint bindingIndex);
		[ExternFunction(AltNames = new string[] { "glVertexAttribBindingARB" })]
		public PFN_glVertexAttribBinding glVertexAttribBinding;
		public delegate void PFN_glVertexBindingDivisor(uint bindingIndex, uint divisor);
		[ExternFunction(AltNames = new string[] { "glVertexBindingDivisorARB" })]
		public PFN_glVertexBindingDivisor glVertexBindingDivisor;

	}

	public class ARBVertexAttribBinding : IGLObject {

		public GL GL { get; }
		public ARBVertexAttribBindingFunctions Functions { get; } = new();

		public ARBVertexAttribBinding(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexBuffer(uint bindingIndex, uint buffer, nint offset, int stride) => Functions.glBindVertexBuffer(bindingIndex, buffer, offset, stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribFormat(uint attribIndex, int size, GLType type, bool normalized, uint relativeOffset) => Functions.glVertexAttribFormat(attribIndex, size, (uint)type, (byte)(normalized ? 1 : 0), relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribIFormat(uint attribIndex, int size, GLType type, uint relativeOffset) => Functions.glVertexAttribIFormat(attribIndex, size, (uint)type, relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribLFormat(uint attribIndex, int size, GLType type, uint relativeOffset) => Functions.glVertexAttribLFormat(attribIndex, size, (uint)type, relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribBinding(uint attribIndex, uint bindingIndex) => Functions.glVertexAttribBinding(attribIndex, bindingIndex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexBindingDivisor(uint bindingIndex, uint divisor) => Functions.glVertexBindingDivisor(bindingIndex, divisor);

	}
}

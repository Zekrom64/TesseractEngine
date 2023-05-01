using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {
	
	public unsafe class ARBComputeShaderFunctions {

		[ExternFunction(AltNames = new string[] { "glDispatchComputeARB" })]
		[NativeType("void glDispatchCompute(GLuint numGroupsX, GLuint numGroupsY, GLuint numGroupsZ)")]
		public delegate* unmanaged<uint, uint, uint, void> glDispatchCompute;
		[ExternFunction(AltNames = new string[] { "glDispatchComputeIndiretARB" })]
		[NativeType("void glDispatchComputeIndirect(GLintptr indirect)")]
		public delegate* unmanaged<IntPtr, void> glDispatchComputeIndirect;

	}

	public class ARBComputeShader : IGLObject {

		public GL GL { get; }
		public ARBComputeShaderFunctions Functions { get; } = new();

		public ARBComputeShader(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DispatchCompute(Vector3ui numGroups) {
			unsafe {
				Functions.glDispatchCompute(numGroups.X, numGroups.Y, numGroups.Y);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DispatchComputeIndirect(nint offset) {
			unsafe {
				Functions.glDispatchComputeIndirect(offset);
			}
		}
	}
}

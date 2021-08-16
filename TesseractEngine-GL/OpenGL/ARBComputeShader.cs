using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBComputeShaderFunctions {

		public delegate void PFN_glDispatchCompute(uint numGroupsX, uint numGroupsY, uint numGroupsZ);
		[ExternFunction(AltNames = new string[] { "glDispatchComputeARB" })]
		public PFN_glDispatchCompute glDispatchCompute;
		public delegate void PFN_glDispatchComputeIndirect(IntPtr indirect);
		[ExternFunction(AltNames = new string[] { "glDispatchComputeIndiretARB" })]
		public PFN_glDispatchComputeIndirect glDispatchComputeIndirect;

	}

	public class ARBComputeShader : IGLObject {

		public GL GL { get; }
		public ARBComputeShaderFunctions Functions { get; } = new();

		public ARBComputeShader(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DispatchCompute(Vector3ui numGroups) => Functions.glDispatchCompute(numGroups.X, numGroups.Y, numGroups.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DispatchComputeIndirect(nint offset) => Functions.glDispatchComputeIndirect(offset);

	}
}

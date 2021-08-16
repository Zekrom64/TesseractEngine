using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBShaderStorageBufferObjectFunctions {

		public delegate void PFN_glShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding);
		[ExternFunction(AltNames = new string[] { "glShaderStorageBlockBindingARB" })]
		public PFN_glShaderStorageBlockBinding glShaderStorageBlockBinding;

	}

	public class ARBShaderStorageBufferObject : IGLObject {

		public GL GL { get; }
		public ARBShaderStorageBufferObjectFunctions Functions { get; } = new();

		public ARBShaderStorageBufferObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding) => Functions.glShaderStorageBlockBinding(program, storageBlockIndex, storageBlockBinding);

	}
}

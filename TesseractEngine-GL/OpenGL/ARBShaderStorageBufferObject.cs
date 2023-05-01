using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBShaderStorageBufferObjectFunctions {

		[ExternFunction(AltNames = new string[] { "glShaderStorageBlockBindingARB" })]
		[NativeType("void glShaderStorageBlockBinding(GLuint program, GLuint storageBlockIndex, GLuint storageBlockBinding)")]
		public delegate* unmanaged<uint, uint, uint, void> glShaderStorageBlockBinding;

	}

	public class ARBShaderStorageBufferObject : IGLObject {

		public GL GL { get; }
		public ARBShaderStorageBufferObjectFunctions Functions { get; } = new();

		public ARBShaderStorageBufferObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding) {
			unsafe {
				Functions.glShaderStorageBlockBinding(program, storageBlockIndex, storageBlockBinding);
			}
		}
	}
}

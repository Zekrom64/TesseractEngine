using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL.Graphics {
	
	public class GLShader : IShader, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public uint ID { get; }

		public GLShader(GLGraphics graphics, ShaderCreateInfo createInfo) {
			Graphics = graphics;

			var glspv = GL.ARBGLSPIRV;
			var es2c = GL.ARBES2Compatbility;
			switch(createInfo.SourceType) {
				case ShaderSourceType.GLSL: break;
				case ShaderSourceType.SPIRV:
					if (glspv == null) throw new GLException("Attempted to use SPIR-V shader source when ARB_gl_spirv is not present");
					break;
				default:
					throw new GLException($"Unsupported shader source type \"{createInfo.SourceType}\"");
			}

			var gl33 = GL.GL33!;
			ID = gl33.CreateShader(GLEnums.Convert(createInfo.Type));

			bool compileFlag = false;
			try {
				switch(createInfo.SourceType) {
					case ShaderSourceType.GLSL: {
						if (createInfo.Source is IConstPointer<byte> ptr) gl33.ShaderSource(ID, ptr);
						else gl33.ShaderSource(ID, createInfo.Source.ToString()!);
					} break;
					case ShaderSourceType.SPIRV: {
						if (es2c == null) throw new GLException("Cannot load SPIR-V shader source without GL_ARB_ES2_compatibility");
						ReadOnlySpan<int> asspan = default;
						IConstPointer<int>? asptr = default;
						if (createInfo.Source is int[] asarr) asspan = asarr;
						else if (createInfo.Source is IReadOnlyList<int> aslist) asspan = aslist.ToArray();
						else if (createInfo.Source is ReadOnlyMemory<int> asmem) asspan = asmem.Span;
						else if (createInfo.Source is IConstPointer<int> ascptr) asptr = ascptr;
						else throw new GLException("Shader source is of unsupported type for source type SPIR-V");

						if (!asspan.IsEmpty) {
							unsafe {
								fixed(int* pBinary = asspan) {
									es2c.ShaderBinary(stackalloc uint[] { ID }, Native.GLEnums.GL_SHADER_BINARY_FORMAT_SPIR_V_ARB, new Span<byte>(pBinary, asspan.Length * 4));
								}
							}
						} else es2c.ShaderBinary(stackalloc uint[] { ID }, Native.GLEnums.GL_SHADER_BINARY_FORMAT_SPIR_V_ARB, MemoryUtil.RecastAs<int, byte>(asptr!).ReadOnlySpan);

						// Since its a binary we don't need to compile it
						compileFlag = true;
					} break;
				}

				if (!compileFlag) {
					gl33.CompileShader(ID);
					if (gl33.GetShader(ID, GLGetShader.CompileStatus) == 0)
						throw new GLException("Failed to compile shader, error log: \n" + gl33.GetShaderInfoLog(ID));
					compileFlag = true;
				}

			} finally {
				if (!compileFlag) gl33.DeleteShader(ID);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			GL.GL33!.DeleteShader(ID);
		}

		public bool TryFindBinding(string name, out BindSetLayoutBinding binding) {
			// TODO: Use ARB_program_interface_query or older functions
			binding = default;
			return false;
		}

	}

}

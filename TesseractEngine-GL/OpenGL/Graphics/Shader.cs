using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;

namespace Tesseract.OpenGL.Graphics {
	
	public class GLShader : IShader, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public uint ID { get; }

		public GLShaderType Type { get; }

		public ulong SourceHash { get; }

		public GLShader(GLGraphics graphics, ShaderCreateInfo createInfo) {
			Graphics = graphics;
			Type = GLEnums.Convert(createInfo.Type);

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
			ID = gl33.CreateShader(Type);

			bool compileFlag = false;
			ReadOnlySpan<byte> byteSource;
			try {
				switch(createInfo.SourceType) {
					case ShaderSourceType.GLSL:
						byteSource = ShaderSourceUtil.GetGLSL(createInfo.Source);
						SourceHash = XXHash64.Compute(byteSource);
						gl33.ShaderSource(ID, byteSource);
						break;
					case ShaderSourceType.SPIRV:
						if (es2c == null) throw new GLException("Cannot load SPIR-V shader source without GL_ARB_ES2_compatibility");
						byteSource = MemoryMarshal.Cast<int, byte>(ShaderSourceUtil.GetSPIRV(createInfo.Source));
						SourceHash = XXHash64.Compute(byteSource);
						es2c.ShaderBinary(stackalloc uint[] { ID }, Native.GLEnums.GL_SHADER_BINARY_FORMAT_SPIR_V_ARB, byteSource);

						// Since its a binary we don't need to compile it
						compileFlag = true;
						break;
					default:
						throw new ArgumentException($"Unsupported shader source type {createInfo.SourceType}", nameof(createInfo));
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

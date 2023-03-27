using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;

namespace Tesseract.OpenGL.Graphics {
	
	public class GLShader : IShader, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public uint ID { get; }

		public ShaderType Type { get; }

		public GLShaderType GLType { get; }

		public ulong SourceHash { get; }

		public GLShader(GLGraphics graphics, ShaderCreateInfo createInfo) {
			Graphics = graphics;
			Type = createInfo.Type;
			GLType = GLEnums.Convert(createInfo.Type);

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
			ID = gl33.CreateShader(GLType);

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

	}

	public class GLShaderProgram : IShaderProgram, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public uint ID { get; }

		internal ShaderType Types { get; }

		public GLShaderProgram(GLGraphics graphics, ShaderProgramCreateInfo createInfo) {
			Graphics = graphics;
			var gl33 = GL.GL33!;
			
			ID = gl33.CreateProgram();

			ShaderType types = default;
			foreach(var module in createInfo.Modules) {
				types |= module.Type;
				gl33.AttachShader(ID, ((GLShader)module).ID);
			}

			gl33.LinkProgram(ID);
			if (gl33.GetProgram(ID, GLGetProgram.LinkStatus) != Native.GLEnums.GL_TRUE) {
				string log = gl33.GetProgramInfoLog(ID);
				throw new GLException("Failed to link shader program:\n" + log);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			GL.GL33!.DeleteProgram(ID);
			Graphics.State.InvalidateProgramID(ID);
		}

		private readonly Dictionary<string, BindSetLayoutBinding> bindingInfo = new();
		private bool hasAllBindings = false;

		private static readonly GLShaderUniformType[] sampledTextureTypes = new GLShaderUniformType[] {
			GLShaderUniformType.Sampler1D,
			GLShaderUniformType.Sampler2D,
			GLShaderUniformType.Sampler3D,
			GLShaderUniformType.SamplerCubemap,
			GLShaderUniformType.Sampler2DRect,
			GLShaderUniformType.Sampler1DArray,
			GLShaderUniformType.Sampler2DArray,
			GLShaderUniformType.SamplerCubemapArray,
			GLShaderUniformType.SamplerBuffer,
			GLShaderUniformType.Sampler2DMultisample,
			GLShaderUniformType.Sampler2DMultisampleArray,
			GLShaderUniformType.IntSampler1D,
			GLShaderUniformType.IntSampler2D,
			GLShaderUniformType.IntSampler3D,
			GLShaderUniformType.IntSamplerCubemap,
			GLShaderUniformType.IntSampler2DRect,
			GLShaderUniformType.IntSampler1DArray,
			GLShaderUniformType.IntSampler2DArray,
			GLShaderUniformType.IntSamplerCubemapArray,
			GLShaderUniformType.IntSamplerBuffer,
			GLShaderUniformType.IntSampler2DMultisample,
			GLShaderUniformType.IntSampler2DMultisampleArray,
			GLShaderUniformType.UIntSampler1D,
			GLShaderUniformType.UIntSampler2D,
			GLShaderUniformType.UIntSampler3D,
			GLShaderUniformType.UIntSamplerCubemap,
			GLShaderUniformType.UIntSampler2DRect,
			GLShaderUniformType.UIntSampler1DArray,
			GLShaderUniformType.UIntSampler2DArray,
			GLShaderUniformType.UIntSamplerCubemapArray,
			GLShaderUniformType.UIntSamplerBuffer,
			GLShaderUniformType.UIntSampler2DMultisample,
			GLShaderUniformType.UIntSampler2DMultisampleArray
		};

		private static readonly GLShaderUniformType[] storageTextureTypes = new GLShaderUniformType[] {
			GLShaderUniformType.Image1D,
			GLShaderUniformType.Image2D,
			GLShaderUniformType.Image3D,
			GLShaderUniformType.ImageCubemap,
			GLShaderUniformType.Image2DRect,
			GLShaderUniformType.Image1DArray,
			GLShaderUniformType.Image2DArray,
			GLShaderUniformType.ImageCubemapArray,
			GLShaderUniformType.ImageBuffer,
			GLShaderUniformType.Image2DMultisample,
			GLShaderUniformType.Image2DMultisampleArray,
			GLShaderUniformType.IntImage1D,
			GLShaderUniformType.IntImage2D,
			GLShaderUniformType.IntImage3D,
			GLShaderUniformType.IntImageCubemap,
			GLShaderUniformType.IntImage2DRect,
			GLShaderUniformType.IntImage1DArray,
			GLShaderUniformType.IntImage2DArray,
			GLShaderUniformType.IntImageCubemapArray,
			GLShaderUniformType.IntImageBuffer,
			GLShaderUniformType.IntImage2DMultisample,
			GLShaderUniformType.IntImage2DMultisampleArray,
			GLShaderUniformType.UIntImage1D,
			GLShaderUniformType.UIntImage2D,
			GLShaderUniformType.UIntImage3D,
			GLShaderUniformType.UIntImageCubemap,
			GLShaderUniformType.UIntImage2DRect,
			GLShaderUniformType.UIntImage1DArray,
			GLShaderUniformType.UIntImage2DArray,
			GLShaderUniformType.UIntImageCubemapArray,
			GLShaderUniformType.UIntImageBuffer,
			GLShaderUniformType.UIntImage2DMultisample,
			GLShaderUniformType.UIntImage2DMultisampleArray
		};

		private void GatherAllBindings(ARBProgramInterfaceQuery piq) {
			ShaderType GatherStages(GLProgramInterface iface, uint i) {
				var piq = GL.ARBProgramInterfaceQuery!;
				ShaderType stages = default;
				if (piq.GetProgramResource<int>(ID, iface, i, GLGetProgramResource.ReferencedByVertexShader) != 0) stages |= ShaderType.Vertex;
				if (piq.GetProgramResource<int>(ID, iface, i, GLGetProgramResource.ReferencedByGeometryShader) != 0) stages |= ShaderType.Geometry;
				if (piq.GetProgramResource<int>(ID, iface, i, GLGetProgramResource.ReferencedByTessControlShader) != 0) stages |= ShaderType.TessellationControl;
				if (piq.GetProgramResource<int>(ID, iface, i, GLGetProgramResource.ReferencedByTessEvaluationShader) != 0) stages |= ShaderType.TessellationEvaluation;
				if (piq.GetProgramResource<int>(ID, iface, i, GLGetProgramResource.ReferencedByFragmentShader) != 0) stages |= ShaderType.Fragment;
				if (piq.GetProgramResource<int>(ID, iface, i, GLGetProgramResource.ReferencedByComputeShader) != 0) stages |= ShaderType.Compute;
				return stages;
			}

			int numUniforms = piq.GetProgramInterface(ID, GLProgramInterface.Uniform, GLGetProgramInterface.ActiveResources);
			for (uint i = 0; i < numUniforms; i++) {
				string name = piq.GetProgramResourceName(ID, GLProgramInterface.Uniform, i);
				var type = piq.GetProgramResource<GLShaderUniformType>(ID, GLProgramInterface.Uniform, i, GLGetProgramResource.Type);

				BindType bindType;
				if (sampledTextureTypes.Contains(type)) bindType = BindType.CombinedTextureSampler;
				else if (storageTextureTypes.Contains(type)) bindType = BindType.StorageTexture;
				else continue;

				bindingInfo[name] = new BindSetLayoutBinding() {
					Type = bindType,
					Binding = piq.GetProgramResource<uint>(ID, GLProgramInterface.Uniform, i, GLGetProgramResource.Location),
					Stages = GatherStages(GLProgramInterface.Uniform, i)
				};
			}

			int numUniformBlocks = piq.GetProgramInterface(ID, GLProgramInterface.UniformBlock, GLGetProgramInterface.ActiveResources);
			for(uint i = 0; i < numUniformBlocks; i++) {
				string name = piq.GetProgramResourceName(ID, GLProgramInterface.UniformBlock, i);

				bindingInfo[name] = new BindSetLayoutBinding() {
					Type = BindType.UniformBuffer,
					Binding = piq.GetProgramResource<uint>(ID, GLProgramInterface.UniformBlock, i, GLGetProgramResource.BufferBinding),
					Stages = GatherStages(GLProgramInterface.UniformBlock, i)
				};
			}

			int numStorageBlocks = piq.GetProgramInterface(ID, GLProgramInterface.ShaderStorageBlock, GLGetProgramInterface.ActiveResources);
			for(uint i = 0; i < numStorageBlocks; i++) {
				string name = piq.GetProgramResourceName(ID, GLProgramInterface.ShaderStorageBlock, i);

				bindingInfo[name] = new BindSetLayoutBinding() {
					Type = BindType.StorageBuffer,
					Binding = piq.GetProgramResource<uint>(ID, GLProgramInterface.ShaderStorageBlock, i, GLGetProgramResource.BufferBinding),
					Stages = GatherStages(GLProgramInterface.ShaderStorageBlock, i)
				};
			}
		}

		private void GatherAllBindings(GL33 gl33) {
			int numUniforms = gl33.GetProgram(ID, GLGetProgram.ActiveUniforms);
			for(uint i = 0; i < numUniforms; i++) {
				// Get the type and check if it is a sampled or storage texture
				gl33.GetActiveUniform(ID, i, out int sz, out GLShaderUniformType type, out string name);
				BindType bindType;
				if (sampledTextureTypes.Contains(type)) bindType = BindType.CombinedTextureSampler;
				else if (storageTextureTypes.Contains(type)) bindType = BindType.StorageTexture;
				else continue; // Fast return, the uniform was found but is not of an accepted type

				// Register the binding
				bindingInfo[name] = new BindSetLayoutBinding() {
					Binding = (uint)gl33.GetUniformLocation(ID, name),
					Type = bindType
				};
			}

			int numUniformBlocks = gl33.GetProgram(ID, GLGetProgram.ActiveUniformBlocks);
			for(uint i = 0; i < numUniformBlocks; i++) {
				string name = gl33.GetActiveUniformBlockName(ID, i);
				bindingInfo[name] = new BindSetLayoutBinding() {
					Binding = (uint)gl33.GetActiveUniformBlock(ID, i, GLGetActiveUniformBlock.Binding),
					Type = BindType.UniformBuffer
				};
			}
		}

		public bool TryGetBinding(string name, out BindSetLayoutBinding binding) {
			if (!hasAllBindings) {
				var piq = GL.ARBProgramInterfaceQuery;
				if (piq != null) GatherAllBindings(piq);
				else GatherAllBindings(GL.GL33!);
				hasAllBindings = true;
			}
			return bindingInfo.TryGetValue(name, out binding);
		}

	}

}

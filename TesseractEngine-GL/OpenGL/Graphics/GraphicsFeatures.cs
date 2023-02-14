using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Collections;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {

    public class GLGraphicsFeatures : IGraphicsFeatures {

		public static uint GetRequiredContextFlags(GraphicsHardwareFeatures features) {
			uint flags = 0;
			if (features.RobustBufferAccess) flags |= Native.GLEnums.GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT;
			return flags;
		}

		public GraphicsHardwareFeatures HardwareFeatures { get; }

		public bool StandardSampleLocations => false; // *Could* be determined by ARB_sample_locations but for now this is too complex

		public bool StrictLines => true; // Line drawing is hidden in OpenGL

		// All dynamic states are supported
		public IReadOnlyIndexer<PipelineDynamicState, bool> SupportedDynamicStates { get; } = new FuncReadOnlyIndexer<PipelineDynamicState, bool>((PipelineDynamicState _) => true);

		public bool PushConstants => false; // No push constants

		public bool TextureSubView { get; } // Supported by ARB_texture_view

		public bool SamplerCustomBorderColor => true;

		private readonly HashSet<ShaderSourceType> sourceTypes = new() { ShaderSourceType.GLSL };
		public IReadOnlyIndexer<ShaderSourceType, bool> SupportedShaderSourceTypes { get; }

		public ShaderSourceType PreferredShaderSourceType => ShaderSourceType.GLSL;

		public bool FramebufferBlitTextureSubView => true;

		public bool LimitedTextureBlit => true;

		public bool LimitedTextureCopy { get; }

		public bool LimitedTextureCopyToBuffer { get; }

		private static bool CheckExtendedStorageFormats(GL gl) {
			var ifq = gl.ARBInternalFormatQuery;
			if (ifq == null) return false;

			GLInternalFormat[] formats = new GLInternalFormat[] {
				GLInternalFormat.RG16F,
				GLInternalFormat.R11FG11FB10F,
				GLInternalFormat.R16F,
				GLInternalFormat.RGBA16,
				GLInternalFormat.RGB10A2,
				GLInternalFormat.RG16,
				GLInternalFormat.RG8,
				GLInternalFormat.R16,
				GLInternalFormat.R8,
				GLInternalFormat.RGBA16SNorm,
				GLInternalFormat.RG16SNorm,
				GLInternalFormat.RG8SNorm,
				GLInternalFormat.R16SNorm,
				GLInternalFormat.R8SNorm,
				GLInternalFormat.RG16I,
				GLInternalFormat.RG8I,
				GLInternalFormat.R16I,
				GLInternalFormat.R8I,
				GLInternalFormat.RGB10A2UI,
				GLInternalFormat.RG16UI,
				GLInternalFormat.RG8UI,
				GLInternalFormat.R16UI,
				GLInternalFormat.R8UI
			};

			Span<int> prop = stackalloc int[1];
			foreach (GLInternalFormat format in formats) {
				ifq.GetInternalFormat(GLInternalFormatTarget.Texture2D, format, GLGetInternalFormat.ShaderImageLoad, prop);
				if (prop[0] != Native.GLEnums.GL_FULL_SUPPORT) return false;
				ifq.GetInternalFormat(GLInternalFormatTarget.Texture2D, format, GLGetInternalFormat.ShaderImageStore, prop);
				if (prop[0] != Native.GLEnums.GL_FULL_SUPPORT) return false;
			}

			return true;
		}

		public static GraphicsHardwareFeatures GatherHardwareFeatures(GL gl) {
			int contextFlags = gl.GL11.GetInteger(Native.GLEnums.GL_CONTEXT_FLAGS);

			Span<float> lineWidthRange = gl.GL11.GetFloat(Native.GLEnums.GL_LINE_WIDTH_RANGE, (stackalloc float[2]));
			Span<float> pointSizeRange = gl.GL11.GetFloat(Native.GLEnums.GL_POINT_SIZE_RANGE, (stackalloc float[2]));
			bool shaderStoresAndAtomics = gl.ARBShaderStorageBufferObject != null && gl.ARBShaderImageLoadStore != null && gl.ARBShaderAtomicCounters != null;
			bool dynamicUniforms = gl.GL40 != null;

			return new() {
				RobustBufferAccess = ((contextFlags & Native.GLEnums.GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT) != 0),
				FullDrawIndexUInt32 = true, // Always supported by OpenGL
				CubeMapArray = gl.ARBTextureCubeMapArray,
				IndependentBlend = gl.ARBDrawBuffersBlend != null,
				GeometryShader = true, // Always supported by OpenGL >= 3.2
				TessellationShader = gl.ARBTessellationShader != null,
				SampleRateShading = gl.ARBSampleShading != null,
				DualSrcBlend = true, // Always supported by OpenGL >= 3.3
				LogicOp = true, // Always supported by OpenGL
				MultiDrawIndirect = gl.ARBMultiDrawIndirect != null,
				DrawIndirectFirstInstance = gl.ARBBaseInstance != null,
				DepthClamp = gl.ARBDepthClamp,
				DepthBiasClamp = gl.ARBPolygonOffsetClamp != null,
				FillModeNonSolid = true, // Always supported by OpenGL
				DepthBounds = gl.EXTDepthBoundsTest != null,
				WideLines = lineWidthRange[1] > 1.0f,
				LargePoints = pointSizeRange[1] > 1.0f,
				AlphaToOne = true, // Always supported by OpenGL >= 1.3, *may* have issues on non-compliant implementations
				MultiViewport = gl.ARBViewportArray != null,
				SamplerAnisotropy = gl.ARBTextureFilterAnisotropic || gl.EXTTextureFilterAnisotropic,
				TextureCompressionETC2 = gl.ARBES3Compatibility,
				TextureCompressionASTC_LDR = gl.KHRTextureCompressionASTC_LDR,
				/* Note: Because of old extensions, naming for BC formats isn't regular. Translations are:
				 *     S3TC_DXT1 -> BC1
				 *     S3TC_DXT3 -> BC2
				 *     S3TC_DXT5 -> BC3
				 *     RED_RGTC1 -> BC4
				 *     RG_RGTC2 -> BC5
				 *     BPTC_(UN)SIGNED_FLOAT -> BC6
				 *     BPTC_UNORM -> BC7
				 * Note: Because this feature implies support for BC1-7, *all* extensions must be present.
				 */
				TextureCompressionBC = gl.EXTTextureCompressionS3TC && gl.EXTTextureCompressionRGTC && gl.ARBTextureCompressionBPTC,
				OcclusionQueryPrecise = true, // Will be >1 on OpenGL >= 3.3 unless the implementation lies about query object support
				PipelineStatisticsQuery = gl.ARBPipelineStatisticsQuery,
				VertexPipelineStoresAndAtomics = shaderStoresAndAtomics,
				FragmentStoresAndAtomics = shaderStoresAndAtomics,
				ShaderTessellationAndGeometryPointSize = gl.GL40 != null, // Geometry/Tess Eval support in GLSL >= 4.0
				ShaderImageGatherExtended = false, // No known extension?
				ShaderStorageImageExtendedFormats = CheckExtendedStorageFormats(gl),
				ShaderStorageImageMultisample = gl.ARBShaderImageLoadStore != null,
				ShaderStorageImageReadWithoutFormat = false, // Not suppored
				ShaderStorageImageWriteWithoutFormat = false,
				ShaderUniformBufferArrayDynamicIndexing = dynamicUniforms,
				ShaderSampledImageArrayDynamicIndexing = dynamicUniforms,
				ShaderStorageBufferArrayDynamicIndexing = dynamicUniforms,
				ShaderStorageImageArrayDynamicIndexing = dynamicUniforms,
				ShaderClipDistance = true, // Always supported by OpenGL >= 3.0
				ShaderCullDistance = gl.GL45 != null, // Support in GLSL >= 4.5
				ShaderFloat64 = gl.ARBGPUShaderFP64,
				ShaderInt64 = gl.ARBGPUShaderInt64 != null,
				ShaderInt16 = gl.AMDGPUShaderInt16,
				ShaderResourceResidency = false, // No sparse resource support in OpenGL
				ShaderResourceMinLOD = true, // Caveat support in GLSL >= 1.3 (OpenGL 3.0) except for certain sampler types until GLSL >= 4.0
				SparseBinding = false, // No sparse resource support in OpenGL
				SparseResidencyBuffer = false, // No sparse resource support in OpenGL
				SparseResidencyImage2D = false, // No sparse resource support in OpenGL
				SparseResidencyImage3D = false, // No sparse resource support in OpenGL
				SparseResidency2Samples = false, // No sparse resource support in OpenGL
				SparseResidency4Samples = false, // No sparse resource support in OpenGL
				SparseResidency8Samples = false, // No sparse resource support in OpenGL
				SparseResidency16Samples = false, // No sparse resource support in OpenGL
				SparseResidencyAliased = false, // No sparse resource support in OpenGL
				DrawIndirect = gl.ARBDrawIndirect != null, // Minimum required extension for draw indirection, even if multi-draw indirect isn't supported
			};
		}

		internal GLGraphicsFeatures(GL gl, GraphicsHardwareFeatures? features) {
			if (features == null) features = new();
			HardwareFeatures = features;

			TextureSubView = gl.ARBTextureView != null;
			SupportedShaderSourceTypes = new FuncReadOnlyIndexer<ShaderSourceType, bool>(type => sourceTypes.Contains(type));

			if (gl.ARBGLSPIRV != null) sourceTypes.Add(ShaderSourceType.SPIRV);

			LimitedTextureCopy = gl.ARBCopyImage == null;
			LimitedTextureCopyToBuffer = gl.ARBGetTextureSubImage == null;
		}

	}

}

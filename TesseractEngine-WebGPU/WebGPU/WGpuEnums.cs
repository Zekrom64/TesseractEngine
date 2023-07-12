using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.WebGPU {

	[Flags]
	public enum WGpuFeatures : int {
		DepthClipControl = 0x01,
		Depth32FloatStencil8 = 0x02,
		TextureCompressionBC = 0x04,
		TextureCompressionETC2 = 0x08,
		TextureCompressionASTC = 0x10,
		TimestampQuery = 0x20,
		IndirectFirstInstance = 0x40,
		ShaderF16 = 0x80,
		RG11B10UFloatRenderable = 0x100
	}

	public enum HTMLPredefinedColorSpace : int {
		Invalid = 0,
		SRGB = 1,
		DisplayP3 = 2
	}

	public enum WGpuPowerPreference : int {
		Invalid = 0,
		LowPower = 1,
		HighPerformance = 2
	}

	public enum WGpuTextureFormat : int {
		Invalid = 0,
		
		R8UNorm = 1,
		R8SNorm = 2,
		R8UInt = 3,
		R8SInt = 4,

		R16UInt = 5,
		R16SInt = 6,
		R16Float = 7,
		RG8UNorm = 8,
		RG8SNorm = 9,
		RG8UInt = 10,
		RG8SInt = 11,

		R32UInt = 12,
		R32SInt = 13,
		R32Float = 14,
		RG16UInt = 15,
		RG16SInt = 16,
		RG16Float = 17,
		RGBA8UNorm = 18,
		RGBA8UNormSRGB = 19,
		RGBA8SNorm = 20,
		RGBA8UInt = 21,
		RGBA8SInt = 22,
		BGRA8UNorm = 23,
		BGRA8UNormSRGB = 24,

		RGB9E5UFloat = 25,
		RGB10A2UNorm = 26,
		RG11B10UFloat = 27,

		RG32UInt = 28,
		RG32SInt = 29,
		RG32Float = 30,
		RGBA16UInt = 31,
		RGBA16SInt = 32,
		RGBA16Float = 33,

		RGBA32UInt = 34,
		RGBA32SInt = 35,
		RGBA32Float = 36,

		Stencil8 = 37,
		Depth16UNorm = 38,
		Depth24Plus = 39,
		Depth34PlusStencil8 = 40,
		Depth32Float = 41,
		Depth32FloatStencil8 = 42,

		BC1RGBAUNorm = 43,
		BC1RGBAUNormSRGB = 44,
		BC2RGBAUNorm = 45,
		BC2RGBAUNormSRGB = 46,
		BC3RGBAUNorm = 47,
		BC3RGBAUNormSRGB = 48,
		BC4RUNorm = 49,
		BC4RSNorm = 50,
		BC5RGUNorm = 51,
		BC5RGSNorm = 52,
		BC6HRGBUFloat = 53,
		BC6HRGBSFloat = 54,
		BC7RGBAUNorm = 55,
		BC7RGBAUNormSRGB = 56,

		ETC2RGB8UNorm = 57,
		ETC2RGB8UNormSRGB = 58,
		ETC2RGB8A1UNorm = 59,
		ETC2RGB8A1UNormSRGB = 60,
		ETC2RGBA8UNorm = 61,
		ETC2RGBA8UNormSRGB = 62,
		EACR11UNorm = 63,
		EACR11SNorm = 64,
		EACRG11UNorm = 65,
		EACRG11SNorm = 66,

		ASTC4X4UNorm = 67,
		ASTC4X4UNormSRGB = 68,
		ASTC5X4UNorm = 69,
		ASTC5X4UNormSRGB = 70,
		ASTC5X5UNorm = 71,
		ASTC5X5UNormSRGB = 72,
		ASTC6X5UNorm = 73,
		ASTC6X5UNormSRGB = 74,
		ASTC6X6UNorm = 75,
		ASTC6X6UNormSRGB = 76,
		ASTC8X5UNorm = 77,
		ASTC8X5UNormSRGB = 78,
		ASTC8X6UNorm = 79,
		ASTC8X6UNormSRGB = 80,
		ASTC8X8UNorm = 81,
		ASTC8X8UNormSRGB = 82,
		ASTC10X5UNorm = 83,
		ASTC10X5UNormSRGB = 84,
		ASTC10X6UNorm = 85,
		ASTC10X6UNormSRGB = 86,
		ASTC10X8UNorm = 87,
		ASTC10X8UNormSRGB = 88,
		ASTC10X10UNorm = 89,
		ASTC10X10UNormSRGB = 90,
		ASTC12X10UNorm = 91,
		ASTC12X10UNormSRGB = 92,
		ASTC12X12UNorm = 93,
		ASTC12X12UNormSRGB = 94
	}

	public enum WGpuBufferMapState : int {
		Invalid = 0,
		Unmapped = 1,
		Pending = 2,
		Mapped = 3
	}

	[Flags]
	public enum WGpuBufferUsageFlags : int {
		MapRead = 0x0001,
		MapWrite = 0x0002,
		CopySrc = 0x0004,
		CopyDst = 0x0008,
		Index = 0x0010,
		Vertex = 0x0020,
		Uniform = 0x0040,
		Storage = 0x0080,
		Indirect = 0x0100,
		QueryResolve = 0x0200
	}

	[Flags]
	public enum WGpuMapModeFlags : int {
		Read = 0x1,
		Write = 0x2
	}

	public enum WGpuTextureDimension : int {
		Invalid = 0,
		Dimension1D = 1,
		Dimension2D = 2,
		Dimension3D = 3
	}

	[Flags]
	public enum WGpuTextureUsageFlags : int {
		CopySrc = 0x01,
		CopyDst = 0x02,
		TextureBinding = 0x04,
		StorageBinding = 0x08,
		RenderAttachment = 0x10
	}

	public enum WGpuTextureViewDimension : int {
		Invalid = 0,
		Dimension1D = 1,
		Dimension2D = 2,
		Dimension2DArray = 3,
		DimensionCube = 4,
		DimensionCubeArray = 5,
		Dimension3D = 6
	}

	public enum WGpuTextureAspect : int {
		Invalid = 0,
		All = 1,
		StencilOnly = 2,
		DepthOnly = 3
	}

	public enum WGpuAddressMode : int {
		Invalid = 0,
		ClampToEdge = 1,
		Repeat = 2,
		MirrorRepeat = 3
	}

	public enum WGpuFilterMode : int {
		Invalid = 0,
		Nearest = 1,
		Linear = 2
	}

	public enum WGpuMipmapFilterMode : int {
		Invalid = 0,
		Nearest = 1,
		Linear = 2
	}

	public enum WGpuCompareFunction : int {
		Invalid = 0,
		Never = 1,
		Less = 2,
		Equal = 3,
		LessEqual = 4,
		Greater = 5,
		NotEqual = 6,
		GreaterEqual = 7,
		Always = 8
	}

	[Flags]
	public enum WGpuShaderStageFlags : int {
		Vertex = 0x1,
		Fragment = 0x2,
		Compute = 0x4
	}

	public enum WGpuBindGroupLayoutType : int {
		Invalid = 0,
		Buffer = 1,
		Sampler = 2,
		Texture = 3,
		StorageTexture = 4,
		ExternalTexture = 5
	}

	public enum WGpuBufferBindingType : int {
		Invalid = 0,
		Uniform = 1,
		Storage = 2
	}

	public enum WGpuSamplerBindingType : int {
		Invalid = 0,
		Filtering = 1,
		NonFiltering = 2,
		Comparison = 3
	}

	public enum WGpuTextureSampleType : int {
		Invalid = 0,
		Float = 1,
		UnfilteredFloat = 2,
		Depth = 3,
		SInt = 4,
		UInt = 5
	}

	public enum WGpuStorageTextureAccess : int {
		Invalid = 0,
		WriteOnly = 1
	}

	public enum WGpuCompilationMessageType : int {
		Error = 0,
		Warning = 1,
		Info = 2
	}

	public enum WGpuAutoLayoutMode : int {
		NoHint = 0,
		Auto = 1
	}

	public enum WGpuPrimitiveTopology : int {
		Invalid = 0,
		PointList = 1,
		LineList = 2,
		LineStrip = 3,
		TriangleList = 4,
		TriangleStrip = 5
	}

	public enum WGpuFrontFace : int {
		Invalid = 0,
		ConterClockwise = 1,
		Clockwise = 2
	}

	public enum WGpuCullMode : int {
		Invalid = 0,
		None = 1,
		Front = 2,
		Back = 3
	}

	[Flags]
	public enum WGpuColorWriteFlags : int {
		Read = 0x01,
		Green = 0x02,
		Blue = 0x04,
		Alpha = 0x08,
		All = 0xF
	}

	public enum WGpuBlendFactor : int {
		Invalid = 0,
		Zero = 1,
		One = 2,
		Src = 3,
		OneMinusSrc = 4,
		SrcAlpha = 5,
		OneMinusSrcAlpha = 6,
		Dst = 7,
		OneMinusDst = 8,
		DstAlpha = 9,
		OneMinusDstAlpha = 10,
		SrcAlphaSaturated = 11,
		Constant = 12,
		OneMinusConstant = 13
	}

	public enum WGpuBlendOperation : int {
		Disabled = 0,
		Add = 1,
		Subtract = 2,
		ReverseSubtract = 3,
		Min = 4,
		Max = 5
	}

	public enum WGpuStencilOperation : int {
		Invalid = 0,
		Keep = 1,
		Zero = 2,
		Replace = 3,
		Invert = 4,
		IncrementClamp = 5,
		DecrementClamp = 6,
		IncrementWrap = 7,
		DecrementWrap = 8
	}

	public enum WGpuIndexFormat : int {
		Invalid = 0,
		UInt16 = 1,
		UInt32 = 2
	}

	public enum WGpuVertexFormat : int {
		Invalid = 0,
		UInt8x2 = 95,
		UInt8x4 = 96,
		SInt8x2 = 97,
		SInt8x4 = 98,
		UNorm8x2 = 99,
		UNorm8x4 = 100,
		SNorm8x2 = 101,
		SNorm8x4 = 102,
		UInt16x2 = 103,
		UInt16x4 = 104,
		SInt16x2 = 105,
		SInt16x4 = 106,
		UNorm16x2 = 107,
		UNorm16x4 = 108,
		SNorm16x2 = 109,
		SNorm16x4 = 110,
		Float16x2 = 111,
		Float16x4 = 112,
		Float32 = 113,
		Float32x2 = 114,
		Float32x3 = 115,
		Float32x4 = 116,
		UInt32 = 117,
		UInt32x2 = 118,
		UInt32x3 = 119,
		UInt32x4 = 120,
		SInt32 = 121,
		SInt32x2 = 122,
		SInt32x3 = 123,
		SInt32x4 = 124
	}

	public enum WGpuVertexStepMode : int {
		Invalid = 0,
		Vertex = 1,
		Instance = 2
	}

	public enum WGpuComputePassTimestampLocation : int {
		Beginning = 0,
		End = 1
	}

	public enum WGpuLoadOp : int {
		Undefined = 0,
		Load = 1,
		Clear = 2
	}

	public enum WGpuStoreOp : int {
		Undefined = 0,
		Store = 1,
		Discard = 2
	}

	public enum WGpuQueryType : int {
		Invalid = 0,
		Occlusion = 1,
		Timestamp = 2
	}

	public enum WGpuPipelineStatisticName : int {
		Invalid = 0,
		Timestamp = 1
	}

	public enum WGpuCanvasAlphaMode : int {
		Invalid = 0,
		Opaque = 1,
		Premultiplied = 2
	}

	public enum WGpuDeviceLostReason : int {
		Invalid = 0,
		Destroyed = 1
	}

	public enum WGpuErrorType : int {
		NoError = 0,
		OutOfMemory = 1,
		Validation = 2,
		UnknownError = 3
	}

	public enum WGpuErrorFilter : int {
		Invalid = 0,
		OutOfMemory = 1,
		Validation = 2,
		Internal = 3
	}

	public enum WGpuRenderPassTimestampLocation : int {
		Beginning = 0,
		End = 1
	}

}

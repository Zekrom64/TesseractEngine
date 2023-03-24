using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {
	
	public enum GLTextureTarget : uint {
		Texture1D = GLEnums.GL_TEXTURE_1D,
		Texture1DArray = GLEnums.GL_TEXTURE_1D_ARRAY,
		Texture2D = GLEnums.GL_TEXTURE_2D,
		Texture2DArray = GLEnums.GL_TEXTURE_2D_ARRAY,
		Texture2DMultisample = GLEnums.GL_TEXTURE_2D_MULTISAMPLE,
		Texture2DMultisampleArray = GLEnums.GL_TEXTURE_2D_MULTISAMPLE_ARRAY,
		Texture3D = GLEnums.GL_TEXTURE_3D,
		CubeMap = GLEnums.GL_TEXTURE_CUBE_MAP,
		CubeMapArray = GLEnums.GL_TEXTURE_CUBE_MAP_ARRAY,
		Rectangle = GLEnums.GL_TEXTURE_RECTANGLE,
		CubeMapPositiveX = GLEnums.GL_TEXTURE_CUBE_MAP_POSITIVE_X,
		CubeMapNegativeX = GLEnums.GL_TEXTURE_CUBE_MAP_NEGATIVE_X,
		CubeMapPositiveY = GLEnums.GL_TEXTURE_CUBE_MAP_POSITIVE_Y,
		CubeMapNegativeY = GLEnums.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y,
		CubeMapPositiveZ = GLEnums.GL_TEXTURE_CUBE_MAP_POSITIVE_Z,
		CubeMapNegativeZ = GLEnums.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z
	}

	public enum GLDrawMode : uint {
		Points = GLEnums.GL_POINTS,
		LineStrip = GLEnums.GL_LINE_STRIP,
		LineLoop = GLEnums.GL_LINE_LOOP,
		Lines = GLEnums.GL_LINES,
		LineStripAdjacency = GLEnums.GL_LINE_STRIP_ADJACENCY,
		LinesAdjacency = GLEnums.GL_LINES_ADJACENCY,
		TriangleStrip = GLEnums.GL_TRIANGLE_STRIP,
		TriangleFan = GLEnums.GL_TRIANGLE_FAN,
		Triangles = GLEnums.GL_TRIANGLES,
		TriangleStripAdjacency = GLEnums.GL_TRIANGLE_STRIP_ADJACENCY,
		TrianglesAdjacency = GLEnums.GL_TRIANGLES_ADJACENCY,
		Patches = GLEnums.GL_PATCHES
	}

	public enum GLType : uint {
		Byte = GLEnums.GL_BYTE,
		UnsignedByte = GLEnums.GL_UNSIGNED_BYTE,
		Short = GLEnums.GL_SHORT,
		UnsignedShort = GLEnums.GL_UNSIGNED_SHORT,
		Int = GLEnums.GL_INT,
		UnsignedInt = GLEnums.GL_UNSIGNED_INT,
		Float = GLEnums.GL_FLOAT,
		Double = GLEnums.GL_DOUBLE
	}

	public enum GLIndexType : uint {
		UnsignedByte = GLEnums.GL_UNSIGNED_BYTE,
		UnsignedShort = GLEnums.GL_UNSIGNED_SHORT,
		UnsignedInt = GLEnums.GL_UNSIGNED_INT
	}

	public enum GLTextureType : uint {
		Byte = GLEnums.GL_BYTE,
		UnsignedByte = GLEnums.GL_UNSIGNED_BYTE,
		Short = GLEnums.GL_SHORT,
		UnsignedShort = GLEnums.GL_UNSIGNED_SHORT,
		Int = GLEnums.GL_INT,
		UnsignedInt = GLEnums.GL_UNSIGNED_INT,
		HalfFloat = GLEnums.GL_HALF_FLOAT,
		Float = GLEnums.GL_FLOAT,
		Double = GLEnums.GL_DOUBLE,
		UnsignedByte_3_3_2 = GLEnums.GL_UNSIGNED_BYTE_3_3_2,
		UnsignedByte_2_3_3_Rev = GLEnums.GL_UNSIGNED_BYTE_2_3_3_REV,
		UnsignedShort_5_6_5 = GLEnums.GL_UNSIGNED_SHORT_5_6_5,
		UnsignedShort_5_6_5_Rev = GLEnums.GL_UNSIGNED_SHORT_5_6_5_REV,
		UnsignedShort_4_4_4_4 = GLEnums.GL_UNSIGNED_SHORT_4_4_4_4,
		UnsignedShort_4_4_4_4_Rev = GLEnums.GL_UNSIGNED_SHORT_4_4_4_4_REV,
		UnsignedShort_5_5_5_1 = GLEnums.GL_UNSIGNED_SHORT_5_5_5_1,
		UnsignedShort_1_5_5_5_Rev = GLEnums.GL_UNSIGNED_SHORT_1_5_5_5_REV,
		UnsignedInt_8_8_8_8 = GLEnums.GL_UNSIGNED_INT_8_8_8_8,
		UnsignedInt_8_8_8_8_Rev = GLEnums.GL_UNSIGNED_INT_8_8_8_8_REV,
		UnsignedInt_10_10_10_2 = GLEnums.GL_UNSIGNED_INT_10_10_10_2,
		UnsignedInt_2_10_10_10_Rev = GLEnums.GL_UNSIGNED_INT_2_10_10_10_REV,
		UnsignedInt_10F_11F_11F_Rev = GLEnums.GL_UNSIGNED_INT_10F_11F_11F_REV,
		UnsignedInt_24_8 = GLEnums.GL_UNSIGNED_INT_24_8,
		UnsignedInt_5_9_9_9_Rev = GLEnums.GL_UNSIGNED_INT_5_9_9_9_REV,
		Float32UnsignedInt_24_8_Rev = GLEnums.GL_FLOAT_32_UNSIGNED_INT_24_8_REV
	}

	public enum GLFormat : uint {
		R = GLEnums.GL_RED,
		RG = GLEnums.GL_RG,
		RGB = GLEnums.GL_RGB,
		BGR = GLEnums.GL_BGR,
		RGBA = GLEnums.GL_RGBA,
		BGRA = GLEnums.GL_BGRA,
		DepthComponent = GLEnums.GL_DEPTH_COMPONENT,
		StencilIndex = GLEnums.GL_STENCIL_INDEX,
		DepthStencil = GLEnums.GL_DEPTH_STENCIL
	}

	public enum GLInternalFormat : uint {
		R = GLEnums.GL_RED,
		RG = GLEnums.GL_RG,
		RGB = GLEnums.GL_RGB,
		BGR = GLEnums.GL_BGR,
		RGBA = GLEnums.GL_RGBA,
		BGRA = GLEnums.GL_BGRA,

		R8 = GLEnums.GL_R8,
		R8SNorm = GLEnums.GL_R8_SNORM,
		R16 = GLEnums.GL_R16,
		R16SNorm = GLEnums.GL_R16_SNORM,
		RG8 = GLEnums.GL_RG8,
		RG8SNorm = GLEnums.GL_RG8_SNORM,
		RG16 = GLEnums.GL_RG16,
		RG16SNorm = GLEnums.GL_RG16_SNORM,
		R3G3B2 = GLEnums.GL_R3_G3_B2,
		RGB4 = GLEnums.GL_RGB4,
		RGB5 = GLEnums.GL_RGB5,
		RGB8 = GLEnums.GL_RGB8,
		RGB8SNorm = GLEnums.GL_RGB8_SNORM,
		RGB10 = GLEnums.GL_RGB10,
		RGB12 = GLEnums.GL_RGB12,
		RGB16 = GLEnums.GL_RGB16,
		RGB16SNorm = GLEnums.GL_RGB16_SNORM,
		RGBA2 = GLEnums.GL_RGBA2,
		RGBA4 = GLEnums.GL_RGBA4,
		RGB5A1 = GLEnums.GL_RGB5_A1,
		RGBA8 = GLEnums.GL_RGBA8,
		RGBA8SNorm = GLEnums.GL_RGBA8_SNORM,
		RGB10A2 = GLEnums.GL_RGB10_A2,
		RGB10A2UI = GLEnums.GL_RGB10_A2UI,
		RGBA12 = GLEnums.GL_RGBA12,
		RGBA16 = GLEnums.GL_RGBA16,
		RGBA16SNorm = GLEnums.GL_RGBA16_SNORM,
		SRGB8 = GLEnums.GL_SRGB8,
		SRGB8A8 = GLEnums.GL_SRGB8_ALPHA8,
		R16F = GLEnums.GL_R16F,
		RG16F = GLEnums.GL_RG16F,
		RGB16F = GLEnums.GL_RGB16F,
		RGBA16F = GLEnums.GL_RGBA16F,
		R32F = GLEnums.GL_R32F,
		RG32F = GLEnums.GL_RG32F,
		RGB32F = GLEnums.GL_RGB32F,
		RGBA32F = GLEnums.GL_RGBA32F,
		R11FG11FB10F = GLEnums.GL_R11F_G11F_B10F,
		RGB9E5 = GLEnums.GL_RGB9_E5,
		R8I = GLEnums.GL_R8I,
		R8UI = GLEnums.GL_R8UI,
		R16I = GLEnums.GL_R16I,
		R16UI = GLEnums.GL_R16UI,
		R32I = GLEnums.GL_R32I,
		R32UI = GLEnums.GL_R32UI,
		RG8I = GLEnums.GL_RG8I,
		RG8UI = GLEnums.GL_RG8UI,
		RG16I = GLEnums.GL_RG16I,
		RG16UI = GLEnums.GL_RG16UI,
		RG32I = GLEnums.GL_RG32I,
		RG32UI = GLEnums.GL_RG32UI,
		RGB8I = GLEnums.GL_RGB8I,
		RGB8UI = GLEnums.GL_RGB8UI,
		RGB16I = GLEnums.GL_RGB16I,
		RGB16UI = GLEnums.GL_RGB16UI,
		RGB32I = GLEnums.GL_RGB32I,
		RGB32UI = GLEnums.GL_RGB32UI,
		RGBA8I = GLEnums.GL_RGBA8I,
		RGBA8UI = GLEnums.GL_RGBA8UI,
		RGBA16I = GLEnums.GL_RGBA16I,
		RGBA16UI = GLEnums.GL_RGBA16UI,
		RGBA32I = GLEnums.GL_RGBA32I,
		RGBA32UI = GLEnums.GL_RGBA32UI,

		CompressedR = GLEnums.GL_COMPRESSED_RED,
		CompressedRG = GLEnums.GL_COMPRESSED_RG,
		CompressedRGB = GLEnums.GL_COMPRESSED_RGB,
		CompressedRGBA = GLEnums.GL_COMPRESSED_RGBA,
		CompressedSRGBAlpha = GLEnums.GL_COMPRESSED_SRGB_ALPHA,
		CompressedR_RGTC1 = GLEnums.GL_COMPRESSED_RED_RGTC1,
		CompressedSignedR_RGTC1 = GLEnums.GL_COMPRESSED_SIGNED_RED_RGTC1,
		CompressedRG_RGTC2 = GLEnums.GL_COMPRESSED_RG_RGTC2,
		CompressedSignedRG_RGTC2 = GLEnums.GL_COMPRESSED_SIGNED_RG_RGTC2,
		CompressedRGBA_BPTC_UNorm = GLEnums.GL_COMPRESSED_RGBA_BPTC_UNORM,
		CompressedRGB_BPTC_SFloat = GLEnums.GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT,
		CompressedRGB_BPTC_UFloat = GLEnums.GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT,

		DepthComponent16 = GLEnums.GL_DEPTH_COMPONENT16,
		DepthComponent24 = GLEnums.GL_DEPTH_COMPONENT24,
		DepthComponent32F = GLEnums.GL_DEPTH_COMPONENT32F,
		Depth24Stencil8 = GLEnums.GL_DEPTH24_STENCIL8,
		Depth32FStencil8 = GLEnums.GL_DEPTH32F_STENCIL8,
		StencilIndex8 = GLEnums.GL_STENCIL_INDEX8
	}

	public enum GLQueryTarget : uint {
		SamplesPassed = GLEnums.GL_SAMPLES_PASSED,
		AnySamplesPassed = GLEnums.GL_ANY_SAMPLES_PASSED,
		AnySamplesPassedConservative = GLEnums.GL_ANY_SAMPLES_PASSED_CONSERVATIVE,
		PrimitivesGenerated = GLEnums.GL_PRIMITIVES_GENERATED,
		TransformFeedbackPrimitivesWritten = GLEnums.GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN,
		TimeElapsed = GLEnums.GL_TIME_ELAPSED,
		VerticesSubmitted = GLEnums.GL_VERTICES_SUBMITTED_ARB,
		PrimitivesSubmitted = GLEnums.GL_PRIMITIVES_SUBMITTED_ARB,
		VertexShaderInvocations = GLEnums.GL_VERTEX_SHADER_INVOCATIONS_ARB,
		TessControlShaderPatches = GLEnums.GL_TESS_CONTROL_SHADER_PATCHES_ARB,
		TessEvaluationShaderInvocations = GLEnums.GL_TESS_EVALUATION_SHADER_INVOCATIONS_ARB,
		GeometryShaderInvocations = GLEnums.GL_GEOMETRY_SHADER_INVOCATIONS,
		GeometryShaderPrimitivesEmitted = GLEnums.GL_GEOMETRY_SHADER_PRIMITIVES_EMITTED_ARB,
		FragmentShaderInvocations = GLEnums.GL_FRAGMENT_SHADER_INVOCATIONS_ARB,
		ComputeShaderInvocations = GLEnums.GL_COMPUTE_SHADER_INVOCATIONS_ARB,
		ClippingInputPrimitives = GLEnums.GL_CLIPPING_INPUT_PRIMITIVES_ARB,
		ClippingOutputPrimitives = GLEnums.GL_CLIPPING_OUTPUT_PRIMITIVES_ARB,
		TransformFeedbackOverflow = GLEnums.GL_TRANSFORM_FEEDBACK_OVERFLOW_ARB,
		TransformFeedbackStreamOverflow = GLEnums.GL_TRANSFORM_FEEDBACK_STREAM_OVERFLOW_ARB
	}

	public enum GLBufferTarget : uint {
		Array = GLEnums.GL_ARRAY_BUFFER,
		AtomicCounter = GLEnums.GL_ATOMIC_COUNTER_BUFFER,
		CopyRead = GLEnums.GL_COPY_READ_BUFFER,
		CopyWrite = GLEnums.GL_COPY_WRITE_BUFFER,
		DispatchIndirect = GLEnums.GL_DISPATCH_INDIRECT_BUFFER,
		DrawIndirect = GLEnums.GL_DRAW_INDIRECT_BUFFER,
		ElementArray = GLEnums.GL_ELEMENT_ARRAY_BUFFER,
		PixelPack = GLEnums.GL_PIXEL_PACK_BUFFER,
		PixelUnpack = GLEnums.GL_PIXEL_UNPACK_BUFFER,
		Query = GLEnums.GL_QUERY_BUFFER,
		ShaderStorage = GLEnums.GL_SHADER_STORAGE_BUFFER,
		Texture = GLEnums.GL_TEXTURE_BUFFER,
		TransformFeedback = GLEnums.GL_TRANSFORM_FEEDBACK_BUFFER,
		Uniform = GLEnums.GL_UNIFORM_BUFFER,
		Parameter = GLEnums.GL_PARAMETER_BUFFER_ARB
	}

	public enum GLBufferRangeTarget : uint {
		AtomicCounter = GLEnums.GL_ATOMIC_COUNTER_BUFFER,
		TransformFeedback = GLEnums.GL_TRANSFORM_FEEDBACK_BUFFER,
		Uniform = GLEnums.GL_UNIFORM_BUFFER,
		ShaderStorage = GLEnums.GL_SHADER_STORAGE_BUFFER
	}

	public enum GLBufferUsage : uint {
		StreamDraw = GLEnums.GL_STREAM_DRAW,
		StreamRead = GLEnums.GL_STREAM_READ,
		StreamCopy = GLEnums.GL_STREAM_COPY,
		StaticDraw = GLEnums.GL_STATIC_DRAW,
		StaticRead = GLEnums.GL_STATIC_READ,
		StaticCopy = GLEnums.GL_STATIC_COPY,
		DynamicDraw = GLEnums.GL_DYNAMIC_DRAW,
		DynamicRead = GLEnums.GL_DYNAMIC_READ,
		DynamicCopy = GLEnums.GL_DYNAMIC_COPY
	}

	public enum GLGetBufferParameter : uint {
		Access = GLEnums.GL_BUFFER_ACCESS,
		AccessFlags = GLEnums.GL_BUFFER_ACCESS_FLAGS,
		ImmutableStorage = GLEnums.GL_BUFFER_IMMUTABLE_STORAGE,
		Mapped = GLEnums.GL_BUFFER_MAPPED,
		MapLength = GLEnums.GL_BUFFER_MAP_LENGTH,
		MapOffset = GLEnums.GL_BUFFER_MAP_OFFSET,
		Size = GLEnums.GL_BUFFER_SIZE,
		StorageFlags = GLEnums.GL_BUFFER_STORAGE_FLAGS,
		Usage = GLEnums.GL_BUFFER_USAGE
	}

	public enum GLGetBufferPointer : uint {
		MapPointer = GLEnums.GL_BUFFER_MAP_POINTER
	}

	public enum GLGetQueryObject : uint {
		Result = GLEnums.GL_QUERY_RESULT,
		ResultNoWait = GLEnums.GL_QUERY_NO_WAIT,
		ResultAvailable = GLEnums.GL_QUERY_RESULT_AVAILABLE,
	}

	public enum GLGetQueryTarget : uint {
		SamplesPassed = GLEnums.GL_SAMPLES_PASSED,
		AnySamplesPassed = GLEnums.GL_ANY_SAMPLES_PASSED,
		AnySamplesPassedConservative = GLEnums.GL_ANY_SAMPLES_PASSED_CONSERVATIVE,
		PrimitivesGenerated = GLEnums.GL_PRIMITIVES_GENERATED,
		TransformFeedbackPrimitivesWritten = GLEnums.GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN,
		TimeElapsed = GLEnums.GL_TIME_ELAPSED
	}

	public enum GLGetQuery : uint {
		CurrentQuery = GLEnums.GL_CURRENT_QUERY,
		CounterBits = GLEnums.GL_QUERY_COUNTER_BITS,
		Timestamp = GLEnums.GL_TIMESTAMP
	}

	public enum GLAccess : uint {
		ReadOnly = GLEnums.GL_READ_ONLY,
		WriteOnly = GLEnums.GL_WRITE_ONLY,
		ReadWrite = GLEnums.GL_READ_WRITE
	}

	public enum GLCompareFunc : uint {
		Never = GLEnums.GL_NEVER,
		Less = GLEnums.GL_LESS,
		Equal = GLEnums.GL_EQUAL,
		LessOrEqual = GLEnums.GL_LEQUAL,
		Greater = GLEnums.GL_GREATER,
		NotEqual = GLEnums.GL_NOTEQUAL,
		GreaterOrEqual = GLEnums.GL_GEQUAL,
		Always = GLEnums.GL_ALWAYS
	}

	public enum GLBlendFactor : uint {
		Zero = GLEnums.GL_ZERO,
		One = GLEnums.GL_ONE,
		SrcColor = GLEnums.GL_SRC_COLOR,
		OneMinusSrcColor = GLEnums.GL_ONE_MINUS_SRC_COLOR,
		DstColor = GLEnums.GL_DST_COLOR,
		OneMinusDstColor = GLEnums.GL_ONE_MINUS_DST_COLOR,
		SrcAlpha = GLEnums.GL_SRC_ALPHA,
		OneMinusSrcAlpha = GLEnums.GL_ONE_MINUS_SRC_ALPHA,
		DstAlpha = GLEnums.GL_DST_ALPHA,
		OneMinusDstAlpha = GLEnums.GL_ONE_MINUS_DST_ALPHA,
		ConstantColor = GLEnums.GL_CONSTANT_COLOR,
		OneMinusConstantColor = GLEnums.GL_ONE_MINUS_CONSTANT_COLOR,
		ConstantAlpha = GLEnums.GL_CONSTANT_ALPHA,
		OneMinusConstantAlpha = GLEnums.GL_ONE_MINUS_CONSTANT_ALPHA,
		SrcAlphaSaturate = GLEnums.GL_SRC_ALPHA_SATURATE,
		Src1Color = GLEnums.GL_SRC1_COLOR,
		OneMinusSrc1Color = GLEnums.GL_ONE_MINUS_SRC1_COLOR,
		Src1Alpha = GLEnums.GL_SRC1_ALPHA,
		OneMinusSrc1Alpha = GLEnums.GL_ONE_MINUS_SRC1_ALPHA
	}

	public enum GLBlendFunction : uint {
		Add = GLEnums.GL_FUNC_ADD,
		Subtract = GLEnums.GL_FUNC_SUBTRACT,
		ReverseSubtract = GLEnums.GL_FUNC_REVERSE_SUBTRACT,
		Min = GLEnums.GL_MIN,
		Max = GLEnums.GL_MAX
	}

	public enum GLBufferMask : uint {
		Color = GLEnums.GL_COLOR_BUFFER_BIT,
		Depth = GLEnums.GL_DEPTH_BUFFER_BIT,
		Stencil = GLEnums.GL_STENCIL_BUFFER_BIT
	}

	public enum GLFace : uint {
		Front = GLEnums.GL_FRONT,
		Back = GLEnums.GL_BACK,
		FrontAndBack = GLEnums.GL_FRONT_AND_BACK
	}

	public enum GLCapability : uint {
		Blend = GLEnums.GL_BLEND,
		ClipDistance0 = GLEnums.GL_CLIP_DISTANCE0,
		ClipDistance1 = GLEnums.GL_CLIP_DISTANCE1,
		ClipDistance2 = GLEnums.GL_CLIP_DISTANCE2,
		ClipDistance3 = GLEnums.GL_CLIP_DISTANCE3,
		ClipDistance4 = GLEnums.GL_CLIP_DISTANCE4,
		ClipDistance5 = GLEnums.GL_CLIP_DISTANCE5,
		ColorLogicOp = GLEnums.GL_COLOR_LOGIC_OP,
		CullFace = GLEnums.GL_CULL_FACE,
		DebugOutput = GLEnums.GL_DEBUG_OUTPUT,
		DebugOutputSynchronous = GLEnums.GL_DEBUG_OUTPUT_SYNCHRONOUS,
		DepthClamp = GLEnums.GL_DEPTH_CLAMP,
		DepthTest = GLEnums.GL_DEPTH_TEST,
		Dither = GLEnums.GL_DITHER,
		FramebufferSRGB = GLEnums.GL_FRAMEBUFFER_SRGB,
		LineSmooth = GLEnums.GL_LINE_SMOOTH,
		Multisample = GLEnums.GL_MULTISAMPLE,
		PolygonOffsetFill = GLEnums.GL_POLYGON_OFFSET_FILL,
		PolygonOffsetLine = GLEnums.GL_POLYGON_OFFSET_LINE,
		PolygonOffsetPoint = GLEnums.GL_POLYGON_OFFSET_POINT,
		PolygonSmooth = GLEnums.GL_POLYGON_SMOOTH,
		PrimitiveRestart = GLEnums.GL_PRIMITIVE_RESTART,
		PrimitiveRestartFixedIndex = GLEnums.GL_PRIMITIVE_RESTART_FIXED_INDEX,
		RasterizerDiscard = GLEnums.GL_RASTERIZER_DISCARD,
		SampleAlphaToCoverage = GLEnums.GL_SAMPLE_ALPHA_TO_COVERAGE,
		SampleAlphaToOne = GLEnums.GL_SAMPLE_ALPHA_TO_ONE,
		SampleCoverage = GLEnums.GL_SAMPLE_COVERAGE,
		SampleShading = GLEnums.GL_SAMPLE_SHADING,
		SampleMask = GLEnums.GL_SAMPLE_MASK,
		ScissorTest = GLEnums.GL_SCISSOR_TEST,
		StencilTest = GLEnums.GL_STENCIL_TEST,
		TextureCubeMapSeamless = GLEnums.GL_TEXTURE_CUBE_MAP_SEAMLESS,
		ProgramPointSize = GLEnums.GL_PROGRAM_POINT_SIZE,
		DepthBoundsTestEXT = GLEnums.GL_DEPTH_BOUNDS_TEST_EXT
	}

	public enum GLIndexedCapability : uint {
		Blend = GLEnums.GL_BLEND,
		ScissorTest = GLEnums.GL_SCISSOR_TEST
	}

	public enum GLLogicOp : uint {
		Clear = GLEnums.GL_CLEAR,
		Set = GLEnums.GL_SET,
		Copy = GLEnums.GL_COPY,
		CopyInverted = GLEnums.GL_COPY_INVERTED,
		NoOp = GLEnums.GL_NOOP,
		Invert = GLEnums.GL_INVERT,
		And = GLEnums.GL_AND,
		NAnd = GLEnums.GL_NAND,
		Or = GLEnums.GL_OR,
		NOr = GLEnums.GL_NOR,
		XOr = GLEnums.GL_XOR,
		Equiv = GLEnums.GL_EQUIV,
		AndReverse = GLEnums.GL_AND_REVERSE,
		AndInverted = GLEnums.GL_AND_INVERTED,
		OrReverse = GLEnums.GL_OR_REVERSE,
		OrInverted = GLEnums.GL_OR_INVERTED
	}

	public enum GLDrawBuffer : uint {
		None = GLEnums.GL_NONE,
		FrontLeft = GLEnums.GL_FRONT_LEFT,
		FrontRight = GLEnums.GL_FRONT_RIGHT,
		BackLeft = GLEnums.GL_BACK_LEFT,
		BackRight = GLEnums.GL_BACK_RIGHT,
		Front = GLEnums.GL_FRONT,
		Back = GLEnums.GL_BACK,
		Left = GLEnums.GL_LEFT,
		Right = GLEnums.GL_RIGHT,
		FrontAndBack = GLEnums.GL_FRONT_AND_BACK
	}

	public enum GLCullFace : uint {
		Clockwise = GLEnums.GL_CW,
		CounterClockwise = GLEnums.GL_CCW
	}

	public enum GLError : uint {
		NoError = GLEnums.GL_NO_ERROR,
		InvalidEnum = GLEnums.GL_INVALID_ENUM,
		InvalidValue = GLEnums.GL_INVALID_VALUE,
		InvalidOperation = GLEnums.GL_INVALID_OPERATION,
		StackOverflow = GLEnums.GL_STACK_OVERFLOW,
		StackUnderflow = GLEnums.GL_STACK_UNDERFLOW,
		OutOfMemory = GLEnums.GL_OUT_OF_MEMORY,
		TableTooLarge = GLEnums.GL_TABLE_TOO_LARGE,
		InvalidFramebufferOperation = GLEnums.GL_INVALID_FRAMEBUFFER_OPERATION
	}

	public enum GLPolygonMode : uint {
		Point = GLEnums.GL_POINT,
		Line = GLEnums.GL_LINE,
		Fill = GLEnums.GL_FILL
	}

	public enum GLStencilFunc : uint {
		Never = GLEnums.GL_NEVER,
		Less = GLEnums.GL_LESS,
		LessOrEqual = GLEnums.GL_LEQUAL,
		Greater = GLEnums.GL_GREATER,
		GreaterOrEqual = GLEnums.GL_GEQUAL,
		Equal = GLEnums.GL_EQUAL,
		NotEqual = GLEnums.GL_NOTEQUAL,
		Always = GLEnums.GL_ALWAYS
	}

	public enum GLStencilOp : uint {
		Keep = GLEnums.GL_KEEP,
		Zero = GLEnums.GL_ZERO,
		Replace = GLEnums.GL_REPLACE,
		Increment = GLEnums.GL_INCR,
		IncrementAndWrap = GLEnums.GL_INCR_WRAP,
		Decrement = GLEnums.GL_DECR,
		DecrementAndWrap = GLEnums.GL_DECR_WRAP,
		Invert = GLEnums.GL_INVERT
	}

	public enum GLTexParamter : uint {
		DepthStencilTextureMode = GLEnums.GL_DEPTH_STENCIL_TEXTURE_MODE,
		BaseLevel = GLEnums.GL_TEXTURE_BASE_LEVEL,
		BorderColor = GLEnums.GL_TEXTURE_BORDER_COLOR,
		CompareFunc = GLEnums.GL_TEXTURE_COMPARE_FUNC,
		CompareMode = GLEnums.GL_TEXTURE_COMPARE_MODE,
		LodBias = GLEnums.GL_TEXTURE_LOD_BIAS,
		MinifyFilter = GLEnums.GL_TEXTURE_MIN_FILTER,
		MagnifyFilter = GLEnums.GL_TEXTURE_MAG_FILTER,
		MinLod = GLEnums.GL_TEXTURE_MIN_LOD,
		MaxLod = GLEnums.GL_TEXTURE_MAX_LOD,
		MaxLevel = GLEnums.GL_TEXTURE_MAX_LEVEL,
		SwizzleR = GLEnums.GL_TEXTURE_SWIZZLE_R,
		SwizzleG = GLEnums.GL_TEXTURE_SWIZZLE_G,
		SwizzleB = GLEnums.GL_TEXTURE_SWIZZLE_B,
		SwizzleA = GLEnums.GL_TEXTURE_SWIZZLE_A,
		SwizzleRGBA = GLEnums.GL_TEXTURE_SWIZZLE_RGBA,
		WrapS = GLEnums.GL_TEXTURE_WRAP_S,
		WrapT = GLEnums.GL_TEXTURE_WRAP_T,
		WrapR = GLEnums.GL_TEXTURE_WRAP_R
	}

	public enum GLOrigin : uint {
		LowerLeft = GLEnums.GL_LOWER_LEFT,
		UpperLeft = GLEnums.GL_UPPER_LEFT
	}

	public enum GLShaderType : uint {
		Vertex = GLEnums.GL_VERTEX_SHADER,
		TessellationControl = GLEnums.GL_TESS_CONTROL_SHADER,
		TessellationEvaluation = GLEnums.GL_TESS_EVALUATION_SHADER,
		Geometry = GLEnums.GL_GEOMETRY_SHADER,
		Fragment = GLEnums.GL_FRAGMENT_SHADER,
		Compute = GLEnums.GL_COMPUTE_SHADER
	}

	public enum GLShaderAttribType : uint {
		Float = GLEnums.GL_FLOAT,
		Vector2 = GLEnums.GL_FLOAT_VEC2,
		Vector3 = GLEnums.GL_FLOAT_VEC3,
		Vector4 = GLEnums.GL_FLOAT_VEC4,
		Matrix2x2 = GLEnums.GL_FLOAT_MAT2,
		Matrix3x3 = GLEnums.GL_FLOAT_MAT3,
		Matrix4x4 = GLEnums.GL_FLOAT_MAT4,
		Matrix2x3 = GLEnums.GL_FLOAT_MAT2x3,
		Matrix2x4 = GLEnums.GL_FLOAT_MAT2x4,
		Matrix3x2 = GLEnums.GL_FLOAT_MAT3x2,
		Matrix3x4 = GLEnums.GL_FLOAT_MAT3x4,
		Matrix4x2 = GLEnums.GL_FLOAT_MAT4x2,
		Matrix4x3 = GLEnums.GL_FLOAT_MAT4x3,
		Integer = GLEnums.GL_INT,
		Vector2i = GLEnums.GL_INT_VEC2,
		Vector3i = GLEnums.GL_INT_VEC3,
		Vector4i = GLEnums.GL_INT_VEC4,
		UnsignedInteger = GLEnums.GL_UNSIGNED_INT,
		Vector2ui = GLEnums.GL_UNSIGNED_INT_VEC2,
		Vector3ui = GLEnums.GL_UNSIGNED_INT_VEC3,
		Vector4ui = GLEnums.GL_UNSIGNED_INT_VEC4,
		Double = GLEnums.GL_DOUBLE,
		Vector2d = GLEnums.GL_DOUBLE_VEC2,
		Vector3d = GLEnums.GL_DOUBLE_VEC3,
		Vector4d = GLEnums.GL_DOUBLE_VEC4,
		Matrix2x2d = GLEnums.GL_DOUBLE_MAT2,
		Matrix3x3d = GLEnums.GL_DOUBLE_MAT3,
		Matrix4x4d = GLEnums.GL_DOUBLE_MAT4,
		Matrix2x3d = GLEnums.GL_DOUBLE_MAT2x3,
		Matrix2x4d = GLEnums.GL_DOUBLE_MAT2x4,
		Matrix3x2d = GLEnums.GL_DOUBLE_MAT3x2,
		Matrix3x4d = GLEnums.GL_DOUBLE_MAT3x4,
		Matrix4x2d = GLEnums.GL_DOUBLE_MAT4x2,
		Matrix4x3d = GLEnums.GL_DOUBLE_MAT4x3
	}

	public enum GLShaderUniformType : uint {
		Float = GLEnums.GL_FLOAT,
		Vector2 = GLEnums.GL_FLOAT_VEC2,
		Vector3 = GLEnums.GL_FLOAT_VEC3,
		Vector4 = GLEnums.GL_FLOAT_VEC4,
		Matrix2x2 = GLEnums.GL_FLOAT_MAT2,
		Matrix3x3 = GLEnums.GL_FLOAT_MAT3,
		Matrix4x4 = GLEnums.GL_FLOAT_MAT4,
		Matrix2x3 = GLEnums.GL_FLOAT_MAT2x3,
		Matrix2x4 = GLEnums.GL_FLOAT_MAT2x4,
		Matrix3x2 = GLEnums.GL_FLOAT_MAT3x2,
		Matrix3x4 = GLEnums.GL_FLOAT_MAT3x4,
		Matrix4x2 = GLEnums.GL_FLOAT_MAT4x2,
		Matrix4x3 = GLEnums.GL_FLOAT_MAT4x3,
		Integer = GLEnums.GL_INT,
		Vector2i = GLEnums.GL_INT_VEC2,
		Vector3i = GLEnums.GL_INT_VEC3,
		Vector4i = GLEnums.GL_INT_VEC4,
		UnsignedInteger = GLEnums.GL_UNSIGNED_INT,
		Vector2ui = GLEnums.GL_UNSIGNED_INT_VEC2,
		Vector3ui = GLEnums.GL_UNSIGNED_INT_VEC3,
		Vector4ui = GLEnums.GL_UNSIGNED_INT_VEC4,
		Double = GLEnums.GL_DOUBLE,
		Vector2d = GLEnums.GL_DOUBLE_VEC2,
		Vector3d = GLEnums.GL_DOUBLE_VEC3,
		Vector4d = GLEnums.GL_DOUBLE_VEC4,
		Matrix2x2d = GLEnums.GL_DOUBLE_MAT2,
		Matrix3x3d = GLEnums.GL_DOUBLE_MAT3,
		Matrix4x4d = GLEnums.GL_DOUBLE_MAT4,
		Matrix2x3d = GLEnums.GL_DOUBLE_MAT2x3,
		Matrix2x4d = GLEnums.GL_DOUBLE_MAT2x4,
		Matrix3x2d = GLEnums.GL_DOUBLE_MAT3x2,
		Matrix3x4d = GLEnums.GL_DOUBLE_MAT3x4,
		Matrix4x2d = GLEnums.GL_DOUBLE_MAT4x2,
		Matrix4x3d = GLEnums.GL_DOUBLE_MAT4x3,
		Sampler1D = GLEnums.GL_SAMPLER_1D,
		Sampler2D = GLEnums.GL_SAMPLER_2D,
		Sampler3D = GLEnums.GL_SAMPLER_3D,
		SamplerCubemap = GLEnums.GL_SAMPLER_CUBE,
		SamplerCubemapArray = GLEnums.GL_SAMPLER_CUBE_MAP_ARRAY,
		Sampler1DShadow = GLEnums.GL_SAMPLER_1D_SHADOW,
		Sampler2DShadow = GLEnums.GL_SAMPLER_2D_SHADOW,
		Sampler1DArray = GLEnums.GL_SAMPLER_1D_ARRAY,
		Sampler2DArray = GLEnums.GL_SAMPLER_2D_ARRAY,
		Sampler1DArrayShadow = GLEnums.GL_SAMPLER_1D_ARRAY_SHADOW,
		Sampler2DArrayShadow = GLEnums.GL_SAMPLER_2D_ARRAY_SHADOW,
		Sampler2DMultisample = GLEnums.GL_SAMPLER_2D_MULTISAMPLE,
		Sampler2DMultisampleArray = GLEnums.GL_SAMPLER_2D_MULTISAMPLE_ARRAY,
		SamplerCubemapShadow = GLEnums.GL_SAMPLER_CUBE_SHADOW,
		SamplerBuffer = GLEnums.GL_SAMPLER_BUFFER,
		Sampler2DRect = GLEnums.GL_SAMPLER_2D_RECT,
		Sampler2DRectShadow = GLEnums.GL_SAMPLER_2D_RECT_SHADOW,
		IntSampler1D = GLEnums.GL_INT_SAMPLER_1D,
		IntSampler2D = GLEnums.GL_INT_SAMPLER_2D,
		IntSampler3D = GLEnums.GL_INT_SAMPLER_3D,
		IntSamplerCubemap = GLEnums.GL_INT_SAMPLER_CUBE,
		IntSamplerCubemapArray = GLEnums.GL_INT_SAMPLER_CUBE_MAP_ARRAY,
		IntSampler1DArray = GLEnums.GL_INT_SAMPLER_1D_ARRAY,
		IntSampler2DArray = GLEnums.GL_INT_SAMPLER_2D_ARRAY,
		IntSampler2DMultisample = GLEnums.GL_INT_SAMPLER_2D_MULTISAMPLE,
		IntSampler2DMultisampleArray = GLEnums.GL_INT_SAMPLER_2D_MULTISAMPLE_ARRAY,
		IntSamplerBuffer = GLEnums.GL_INT_SAMPLER_BUFFER,
		IntSampler2DRect = GLEnums.GL_INT_SAMPLER_2D_RECT,
		UIntSampler1D = GLEnums.GL_UNSIGNED_INT_SAMPLER_1D,
		UIntSampler2D = GLEnums.GL_UNSIGNED_INT_SAMPLER_2D,
		UIntSampler3D = GLEnums.GL_UNSIGNED_INT_SAMPLER_3D,
		UIntSamplerCubemap = GLEnums.GL_UNSIGNED_INT_SAMPLER_CUBE,
		UIntSamplerCubemapArray = GLEnums.GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY,
		UIntSampler1DArray = GLEnums.GL_UNSIGNED_INT_SAMPLER_1D_ARRAY,
		UIntSampler2DArray = GLEnums.GL_UNSIGNED_INT_SAMPLER_2D_ARRAY,
		UIntSampler2DMultisample = GLEnums.GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE,
		UIntSampler2DMultisampleArray = GLEnums.GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY,
		UIntSamplerBuffer = GLEnums.GL_UNSIGNED_INT_SAMPLER_BUFFER,
		UIntSampler2DRect = GLEnums.GL_UNSIGNED_INT_SAMPLER_2D_RECT,
		Image1D = GLEnums.GL_IMAGE_1D,
		Image2D = GLEnums.GL_IMAGE_2D,
		Image3D = GLEnums.GL_IMAGE_3D,
		Image2DRect = GLEnums.GL_IMAGE_2D_RECT,
		ImageCubemap = GLEnums.GL_IMAGE_CUBE,
		ImageCubemapArray = GLEnums.GL_IMAGE_CUBE_MAP_ARRAY,
		ImageBuffer = GLEnums.GL_IMAGE_BUFFER,
		Image1DArray = GLEnums.GL_IMAGE_1D_ARRAY,
		Image2DArray = GLEnums.GL_IMAGE_2D_ARRAY,
		Image2DMultisample = GLEnums.GL_IMAGE_2D_MULTISAMPLE,
		Image2DMultisampleArray = GLEnums.GL_IMAGE_2D_MULTISAMPLE_ARRAY,
		IntImage1D = GLEnums.GL_INT_IMAGE_1D,
		IntImage2D = GLEnums.GL_INT_IMAGE_2D,
		IntImage3D = GLEnums.GL_INT_IMAGE_3D,
		IntImage2DRect = GLEnums.GL_INT_IMAGE_2D_RECT,
		IntImageCubemap = GLEnums.GL_INT_IMAGE_CUBE,
		IntImageCubemapArray = GLEnums.GL_INT_IMAGE_CUBE_MAP_ARRAY,
		IntImageBuffer = GLEnums.GL_INT_IMAGE_BUFFER,
		IntImage1DArray = GLEnums.GL_INT_IMAGE_1D_ARRAY,
		IntImage2DArray = GLEnums.GL_INT_IMAGE_2D_ARRAY,
		IntImage2DMultisample = GLEnums.GL_INT_IMAGE_2D_MULTISAMPLE,
		IntImage2DMultisampleArray = GLEnums.GL_INT_IMAGE_2D_MULTISAMPLE_ARRAY,
		UIntImage1D = GLEnums.GL_UNSIGNED_INT_IMAGE_1D,
		UIntImage2D = GLEnums.GL_UNSIGNED_INT_IMAGE_2D,
		UIntImage3D = GLEnums.GL_UNSIGNED_INT_IMAGE_3D,
		UIntImage2DRect = GLEnums.GL_UNSIGNED_INT_IMAGE_2D_RECT,
		UIntImageCubemap = GLEnums.GL_UNSIGNED_INT_IMAGE_CUBE,
		UIntImageCubemapArray = GLEnums.GL_UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY,
		UIntImageBuffer = GLEnums.GL_UNSIGNED_INT_IMAGE_BUFFER,
		UIntImage1DArray = GLEnums.GL_UNSIGNED_INT_IMAGE_1D_ARRAY,
		UIntImage2DArray = GLEnums.GL_UNSIGNED_INT_IMAGE_2D_ARRAY,
		UIntImage2DMultisample = GLEnums.GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE,
		UIntImage2DMultisampleArray = GLEnums.GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE_ARRAY
	}

	public enum GLGetProgram : uint {
		DeleteStatus = GLEnums.GL_DELETE_STATUS,
		LinkStatus = GLEnums.GL_LINK_STATUS,
		ValidateStatus = GLEnums.GL_VALIDATE_STATUS,
		InfoLogLength = GLEnums.GL_INFO_LOG_LENGTH,
		AttachedShaders = GLEnums.GL_ATTACHED_SHADERS,
		ActiveAtomicCounterBuffers = GLEnums.GL_ACTIVE_ATOMIC_COUNTER_BUFFERS,
		ActiveAttributes = GLEnums.GL_ACTIVE_ATTRIBUTES,
		ActiveAttributeMaxLength = GLEnums.GL_ACTIVE_ATTRIBUTE_MAX_LENGTH,
		ActiveUniforms = GLEnums.GL_ACTIVE_UNIFORMS,
		ActiveUniformMaxLength = GLEnums.GL_ACTIVE_UNIFORM_MAX_LENGTH,
		ProgramBinaryLength = GLEnums.GL_PROGRAM_BINARY_LENGTH,
		ComputeWorkGroupSize = GLEnums.GL_COMPUTE_WORK_GROUP_SIZE,
		TransformFeedbackBufferMode = GLEnums.GL_TRANSFORM_FEEDBACK_BUFFER_MODE,
		TransformFeedbackVaryings = GLEnums.GL_TRANSFORM_FEEDBACK_VARYINGS,
		TransformFeedbackVaryingMaxLength = GLEnums.GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH,
		GeometryVerticesOut = GLEnums.GL_GEOMETRY_VERTICES_OUT,
		GeometryInputType = GLEnums.GL_GEOMETRY_INPUT_TYPE,
		GeometryOutputType = GLEnums.GL_GEOMETRY_OUTPUT_TYPE,
		ActiveUniformBlocks = GLEnums.GL_ACTIVE_UNIFORM_BLOCKS,
		ActiveUniformBlockMaxNameLength = GLEnums.GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH
	}

	public enum GLGetShader : uint {
		ShaderType = GLEnums.GL_SHADER_TYPE,
		DeleteStatus = GLEnums.GL_DELETE_STATUS,
		CompileStatus = GLEnums.GL_COMPILE_STATUS,
		InfoLogLength = GLEnums.GL_INFO_LOG_LENGTH,
		ShaderSourceLength = GLEnums.GL_SHADER_SOURCE_LENGTH
	}

	public enum GLQueryMode : uint {
		Wait = GLEnums.GL_QUERY_WAIT,
		NoWait = GLEnums.GL_QUERY_NO_WAIT,
		ByRegionWait = GLEnums.GL_QUERY_BY_REGION_WAIT,
		ByRegionNoWait = GLEnums.GL_QUERY_BY_REGION_NO_WAIT
	}

	public enum GLClearBuffer : uint {
		Color = GLEnums.GL_COLOR,
		Depth = GLEnums.GL_DEPTH,
		Stencil = GLEnums.GL_STENCIL,
		DepthStencil = GLEnums.GL_DEPTH_STENCIL
	}

	public enum GLGetVertexAttrib : uint {
		ArrayBufferBinding = GLEnums.GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING,
		ArrayEnabled = GLEnums.GL_VERTEX_ATTRIB_ARRAY_ENABLED,
		ArraySize = GLEnums.GL_VERTEX_ATTRIB_ARRAY_SIZE,
		ArrayStride = GLEnums.GL_VERTEX_ATTRIB_ARRAY_STRIDE,
		ArrayType = GLEnums.GL_VERTEX_ATTRIB_ARRAY_TYPE,
		ArrayNormalized = GLEnums.GL_VERTEX_ATTRIB_ARRAY_NORMALIZED,
		ArrayInteger = GLEnums.GL_VERTEX_ATTRIB_ARRAY_INTEGER,
		ArrayLong = GLEnums.GL_VERTEX_ATTRIB_ARRAY_LONG,
		ArrayDivisor = GLEnums.GL_VERTEX_ATTRIB_ARRAY_DIVISOR,
		Binding = GLEnums.GL_VERTEX_ATTRIB_BINDING,
		RelativeOffset = GLEnums.GL_VERTEX_ATTRIB_RELATIVE_OFFSET,
		CurrentVertexAttrib = GLEnums.GL_CURRENT_VERTEX_ATTRIB
	}

	public enum GLMapAccessFlags : uint {
		Read = GLEnums.GL_MAP_READ_BIT,
		Write = GLEnums.GL_MAP_WRITE_BIT,
		Persistent = GLEnums.GL_MAP_PERSISTENT_BIT,
		Coherent = GLEnums.GL_MAP_COHERENT_BIT,
		InvalidateRange = GLEnums.GL_MAP_INVALIDATE_RANGE_BIT,
		InvalidateBuffer = GLEnums.GL_MAP_INVALIDATE_BUFFER_BIT,
		FlushExplicit = GLEnums.GL_MAP_FLUSH_EXPLICIT_BIT,
		Unsynchronized = GLEnums.GL_MAP_UNSYNCHRONIZED_BIT
	}

	public enum GLPatchParamteri : uint {
		Vertices = GLEnums.GL_PATCH_VERTICES
	}

	public enum GLPatchParamterfv : uint {
		DefaultInnerLevel = GLEnums.GL_PATCH_DEFAULT_INNER_LEVEL,
		DefaultOuterLevel = GLEnums.GL_PATCH_DEFAULT_OUTER_LEVEL
	}

	public enum GLTransformFeedbackBufferMode : uint {
		InterleavedAttribs = GLEnums.GL_INTERLEAVED_ATTRIBS,
		SeparateAttribs = GLEnums.GL_SEPARATE_ATTRIBS
	}

	public enum GLClampColorTarget : uint {
		VertexColor = GLEnums.GL_CLAMP_VERTEX_COLOR,
		FragmentColor = GLEnums.GL_CLAMP_FRAGMENT_COLOR,
		ReadColor = GLEnums.GL_CLAMP_READ_COLOR
	}

	public enum GLClampColorMode : uint {
		FixedOnly = GLEnums.GL_FIXED_ONLY,
		False = GLEnums.GL_FALSE,
		True = GLEnums.GL_TRUE
	}

	public enum GLRenderbufferTarget : uint {
		Renderbuffer = GLEnums.GL_RENDERBUFFER
	}

	public enum GLFramebufferTarget : uint {
		Draw = GLEnums.GL_DRAW_FRAMEBUFFER,
		Read = GLEnums.GL_READ_FRAMEBUFFER,
		Framebuffer = GLEnums.GL_FRAMEBUFFER
	}

	public enum GLGetRenderbuffer : uint {
		Width = GLEnums.GL_RENDERBUFFER_WIDTH,
		Height = GLEnums.GL_RENDERBUFFER_HEIGHT,
		InternalFormat = GLEnums.GL_RENDERBUFFER_INTERNAL_FORMAT,
		RedSize = GLEnums.GL_RENDERBUFFER_RED_SIZE,
		GreenSize = GLEnums.GL_RENDERBUFFER_GREEN_SIZE,
		BlueSize = GLEnums.GL_RENDERBUFFER_BLUE_SIZE,
		AlphaSize = GLEnums.GL_RENDERBUFFER_ALPHA_SIZE,
		DepthSize = GLEnums.GL_RENDERBUFFER_DEPTH_SIZE,
		StencilSize = GLEnums.GL_RENDERBUFFER_STENCIL_SIZE,
		Samples = GLEnums.GL_RENDERBUFFER_SAMPLES
	}

	public enum GLFramebufferStatus : uint {
		Complete = GLEnums.GL_FRAMEBUFFER_COMPLETE,
		Undefined = GLEnums.GL_FRAMEBUFFER_UNDEFINED,
		IncompleteAttachment = GLEnums.GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT,
		IncompleteMissingAttachment = GLEnums.GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT,
		Unsupported = GLEnums.GL_FRAMEBUFFER_UNSUPPORTED,
		IncompleteMultisample = GLEnums.GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE
	}

	public enum GLFramebufferAttachment : uint {
		Color0 = GLEnums.GL_COLOR_ATTACHMENT0,
		Color1 = GLEnums.GL_COLOR_ATTACHMENT1,
		Color2 = GLEnums.GL_COLOR_ATTACHMENT2,
		Color3 = GLEnums.GL_COLOR_ATTACHMENT3,
		Color4 = GLEnums.GL_COLOR_ATTACHMENT4,
		Color5 = GLEnums.GL_COLOR_ATTACHMENT5,
		Color6 = GLEnums.GL_COLOR_ATTACHMENT6,
		Color7 = GLEnums.GL_COLOR_ATTACHMENT7,
		Color8 = GLEnums.GL_COLOR_ATTACHMENT8,
		Color9 = GLEnums.GL_COLOR_ATTACHMENT9,
		Color10 = GLEnums.GL_COLOR_ATTACHMENT10,
		Color11 = GLEnums.GL_COLOR_ATTACHMENT11,
		Color12 = GLEnums.GL_COLOR_ATTACHMENT12,
		Color13 = GLEnums.GL_COLOR_ATTACHMENT13,
		Color14 = GLEnums.GL_COLOR_ATTACHMENT14,
		Color15 = GLEnums.GL_COLOR_ATTACHMENT15,
		Depth = GLEnums.GL_DEPTH_ATTACHMENT,
		Stencil = GLEnums.GL_STENCIL_ATTACHMENT,
		DepthStencil = GLEnums.GL_DEPTH_STENCIL_ATTACHMENT
	}

	public enum GLGetFramebufferAttachment : uint {
		RedSize = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_RED_SIZE,
		GreenSize = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_GREEN_SIZE,
		BlueSize = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_BLUE_SIZE,
		AlphaSize = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE,
		DepthSize = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE,
		ComponentType = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE,
		ColorEncoding = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING,
		TextureLevel = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL,
		TextureCubeMapFace = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE,
		Layered = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_LAYERED,
		TextureLayer = GLEnums.GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER
	}

	public enum GLFilter : uint {
		Linear = GLEnums.GL_LINEAR,
		Nearest = GLEnums.GL_NEAREST
	}

	public enum GLDebugSource : uint {
		API = GLEnums.GL_DEBUG_SOURCE_API_ARB,
		WindowSystem = GLEnums.GL_DEBUG_SOURCE_WINDOW_SYSTEM_ARB,
		ShaderCompiler = GLEnums.GL_DEBUG_SOURCE_SHADER_COMPILER_ARB,
		ThirdParty = GLEnums.GL_DEBUG_SOURCE_THIRD_PARTY_ARB,
		Application = GLEnums.GL_DEBUG_SOURCE_APPLICATION_ARB,
		Other = GLEnums.GL_DEBUG_SOURCE_OTHER
	}

	public enum GLDebugType : uint {
		Error = GLEnums.GL_DEBUG_TYPE_ERROR_ARB,
		DeprecatedBehavior = GLEnums.GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR_ARB,
		UndefinedBehavior = GLEnums.GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR_ARB,
		Portability = GLEnums.GL_DEBUG_TYPE_PORTABILITY_ARB,
		Performance = GLEnums.GL_DEBUG_TYPE_PERFORMANCE_ARB,
		Other = GLEnums.GL_DEBUG_TYPE_OTHER_ARB
	}

	public enum GLDebugSeverity : uint {
		High = GLEnums.GL_DEBUG_SEVERITY_HIGH_ARB,
		Medium = GLEnums.GL_DEBUG_SEVERITY_MEDIUM_ARB,
		Low = GLEnums.GL_DEBUG_SEVERITY_LOW_ARB
	}

	public enum GLGetActiveUniform : uint {
		Type = GLEnums.GL_UNIFORM_TYPE,
		Size = GLEnums.GL_UNIFORM_SIZE,
		NameLength = GLEnums.GL_UNIFORM_NAME_LENGTH,
		BlockIndex = GLEnums.GL_UNIFORM_BLOCK_INDEX,
		Offset = GLEnums.GL_UNIFORM_OFFSET,
		ArrayStride = GLEnums.GL_UNIFORM_ARRAY_STRIDE,
		MatrixStride = GLEnums.GL_UNIFORM_MATRIX_STRIDE,
		IsRowMajor = GLEnums.GL_UNIFORM_IS_ROW_MAJOR,
		AtomicCounterBufferIndex = GLEnums.GL_UNIFORM_ATOMIC_COUNTER_BUFFER_INDEX
	}

	public enum GLGetActiveUniformBlock : uint {
		Binding = GLEnums.GL_UNIFORM_BLOCK_BINDING,
		DataSize = GLEnums.GL_UNIFORM_BLOCK_DATA_SIZE,
		NameLength = GLEnums.GL_UNIFORM_BLOCK_NAME_LENGTH,
		ActiveUniforms = GLEnums.GL_UNIFORM_BLOCK_ACTIVE_UNIFORMS,
		ActiveUniformIndices = GLEnums.GL_UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES,
		ReferencedByVertexShader = GLEnums.GL_UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER,
		ReferencedByTessControlShader = GLEnums.GL_UNIFORM_BLOCK_REFERENCED_BY_TESS_CONTROL_SHADER,
		ReferencedByTessEvaluationShader = GLEnums.GL_UNIFORM_BLOCK_REFERENCED_BY_TESS_EVALUATION_SHADER,
		ReferencedByGeometryShader = GLEnums.GL_UNIFORM_BLOCK_REFERENCED_BY_GEOMETRY_SHADER,
		ReferencedByFragmentShader = GLEnums.GL_UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER,
		ReferencedByComputeShader = GLEnums.GL_UNIFORM_BLOCK_REFERENCED_BY_COMPUTE_SHADER
	}

	public enum GLProvokingVertexConvention : uint {
		FirstVertex = GLEnums.GL_FIRST_VERTEX_CONVENTION,
		LastVertex = GLEnums.GL_LAST_VERTEX_CONVENTION
	}

	public enum GLGetMutlisample : uint {
		SamplePosition = GLEnums.GL_SAMPLE_POSITION
	}

	public enum GLSyncCondition : uint {
		GPUCommandsComplete = GLEnums.GL_SYNC_GPU_COMMANDS_COMPLETE
	}

	public enum GLSyncFlags : uint {
		FlushCommands = GLEnums.GL_SYNC_FLUSH_COMMANDS_BIT
	}

	public enum GLGetSync : uint {
		ObjectType = GLEnums.GL_OBJECT_TYPE,
		SyncStatus = GLEnums.GL_SYNC_STATUS,
		SyncCondition = GLEnums.GL_SYNC_CONDITION,
		SyncFlags = GLEnums.GL_SYNC_FLAGS
	}

	public enum GLProgramParameter : uint {
		GeometryVerticesOut = GLEnums.GL_GEOMETRY_VERTICES_OUT,
		GeometryInputType = GLEnums.GL_GEOMETRY_INPUT_TYPE,
		GeometryOutputType = GLEnums.GL_GEOMETRY_OUTPUT_TYPE,
		BinaryRetrievableHint = GLEnums.GL_PROGRAM_BINARY_RETRIEVABLE_HINT,
		Separable = GLEnums.GL_PROGRAM_SEPARABLE
	}

	public enum GLCubeMapFace : uint {
		PositiveX = GLEnums.GL_TEXTURE_CUBE_MAP_POSITIVE_X,
		NegativeX = GLEnums.GL_TEXTURE_CUBE_MAP_NEGATIVE_X,
		PositiveY = GLEnums.GL_TEXTURE_CUBE_MAP_POSITIVE_Y,
		NegativeY = GLEnums.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y,
		PositiveZ = GLEnums.GL_TEXTURE_CUBE_MAP_POSITIVE_Z,
		NegativeZ = GLEnums.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z
	}

	public enum GLQueryCounterTarget : uint {
		Timestamp = GLEnums.GL_TIMESTAMP
	}

	public enum GLSamplerParameter : uint {
		WrapS = GLEnums.GL_TEXTURE_WRAP_S,
		WrapT = GLEnums.GL_TEXTURE_WRAP_T,
		WrapR = GLEnums.GL_TEXTURE_WRAP_R,
		MinFilter = GLEnums.GL_TEXTURE_MIN_FILTER,
		MagFilter = GLEnums.GL_TEXTURE_MAG_FILTER,
		BorderColor = GLEnums.GL_TEXTURE_BORDER_COLOR,
		MinLOD = GLEnums.GL_TEXTURE_MIN_LOD,
		MaxLOD = GLEnums.GL_TEXTURE_MAX_LOD,
		LODBias = GLEnums.GL_TEXTURE_LOD_BIAS,
		CompareMode = GLEnums.GL_TEXTURE_COMPARE_MODE,
		CompareFunc = GLEnums.GL_TEXTURE_COMPARE_FUNC,
		MaxAnisotropy = GLEnums.GL_TEXTURE_MAX_ANISOTROPY_EXT
	}

	public enum GLGetProgramStage : uint {
		ActiveSubroutines = GLEnums.GL_ACTIVE_SUBROUTINES,
		ActiveSubroutineUniforms = GLEnums.GL_ACTIVE_SUBROUTINE_UNIFORMS,
		ActiveSubroutineUniformLocations = GLEnums.GL_ACTIVE_SUBROUTINE_UNIFORM_LOCATIONS,
		ActiveSubroutineMaxLength = GLEnums.GL_ACTIVE_SUBROUTINE_MAX_LENGTH,
		ActiveSubroutineUniformMaxLength = GLEnums.GL_ACTIVE_SUBROUTINE_UNIFORM_MAX_LENGTH
	}

	public enum GLGetActiveSubroutineUniform : uint {
		NumCompatibleSubroutines = GLEnums.GL_NUM_COMPATIBLE_SUBROUTINES,
		CompatibleSubroutines = GLEnums.GL_COMPATIBLE_SUBROUTINES,
		UniformSize = GLEnums.GL_UNIFORM_SIZE,
		UniformNameLength = GLEnums.GL_UNIFORM_NAME_LENGTH
	}

	public enum GLTransformFeedbackTarget : uint {
		TransformFeedback = GLEnums.GL_TRANSFORM_FEEDBACK
	}

	public enum GLShaderStages : uint {
		Vertex = GLEnums.GL_VERTEX_SHADER_BIT,
		TessellationControl = GLEnums.GL_TESS_CONTROL_SHADER_BIT,
		TessellationEvaluation = GLEnums.GL_TESS_EVALUATION_SHADER_BIT,
		Geometry = GLEnums.GL_GEOMETRY_SHADER_BIT,
		Fragment = GLEnums.GL_FRAGMENT_SHADER_BIT,
		Compute = GLEnums.GL_COMPUTE_SHADER_BIT
	}

	public enum GLGetProgramPipeline : uint {
		ActiveProgram = GLEnums.GL_ACTIVE_PROGRAM,
		VertexShader = GLEnums.GL_VERTEX_SHADER,
		TessellationControlShader = GLEnums.GL_TESS_CONTROL_SHADER,
		TessellationEvaluationShader = GLEnums.GL_TESS_EVALUATION_SHADER,
		GeometryShader = GLEnums.GL_GEOMETRY_SHADER,
		FragmentShader = GLEnums.GL_FRAGMENT_SHADER,
		InfoLogLength = GLEnums.GL_INFO_LOG_LENGTH
	}

	public enum GLPrecisionType : uint {
		LowFloat = GLEnums.GL_LOW_FLOAT,
		MediumFloat = GLEnums.GL_MEDIUM_FLOAT,
		HighFloat = GLEnums.GL_HIGH_FLOAT,
		LowInt = GLEnums.GL_LOW_INT,
		MediumInt = GLEnums.GL_MEDIUM_INT,
		HighInt = GLEnums.GL_HIGH_INT
	}

	public enum GLMemoryBarrier : uint {
		VertexAttribArray = GLEnums.GL_VERTEX_ATTRIB_ARRAY_BARRIER_BIT,
		ElementArray = GLEnums.GL_ELEMENT_ARRAY_BARRIER_BIT,
		Uniform = GLEnums.GL_UNIFORM_BARRIER_BIT,
		TextureFetch = GLEnums.GL_TEXTURE_FETCH_BARRIER_BIT,
		ShaderImageAccess = GLEnums.GL_SHADER_IMAGE_ACCESS_BARRIER_BIT,
		Command = GLEnums.GL_COMMAND_BARRIER_BIT,
		PixelBuffer = GLEnums.GL_PIXEL_BUFFER_BARRIER_BIT,
		TextureUpdate = GLEnums.GL_TEXTURE_UPDATE_BARRIER_BIT,
		BufferUpdate = GLEnums.GL_BUFFER_UPDATE_BARRIER_BIT,
		ClientMappedBuffer = GLEnums.GL_CLIENT_MAPPED_BUFFER_BARRIER_BIT,
		Framebuffer = GLEnums.GL_FRAMEBUFFER_BARRIER_BIT,
		TransformFeedback = GLEnums.GL_TRANSFORM_FEEDBACK_BARRIER_BIT,
		AtomicCounter = GLEnums.GL_ATOMIC_COUNTER_BARRIER_BIT,
		ShaderStorage = GLEnums.GL_SHADER_STORAGE_BARRIER_BIT,
		QueryBuffer = GLEnums.GL_QUERY_BUFFER_BARRIER_BIT,
		All = GLEnums.GL_ALL_BARRIER_BITS
	}

	public enum GLGetActiveAtomicCounterBuffer : uint {
		Binding = GLEnums.GL_ATOMIC_COUNTER_BUFFER_BINDING,
		DataSize = GLEnums.GL_ATOMIC_COUNTER_BUFFER_DATA_SIZE,
		ActiveAtomicCounters = GLEnums.GL_ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTERS,
		ActiveAtomicCounterIndices = GLEnums.GL_ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTER_INDICES,
		ReferencedByVertexShader = GLEnums.GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_VERTEX_SHADER,
		ReferencedByTessControlShader = GLEnums.GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_CONTROL_SHADER,
		ReferencedByTessEvaluationShader = GLEnums.GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_EVALUATION_SHADER,
		ReferencedByGeometryShader = GLEnums.GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_GEOMETRY_SHADER,
		ReferencedByFragmentShader = GLEnums.GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_FRAGMENT_SHADER,
		ReferencedByComputeShader = GLEnums.GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_COMPUTE_SHADER
	}

	public enum GLInternalFormatTarget : uint {
		Texture1D = GLEnums.GL_TEXTURE_1D,
		Texture1DArray = GLEnums.GL_TEXTURE_1D_ARRAY,
		Texture2D = GLEnums.GL_TEXTURE_2D,
		Texture2DArray = GLEnums.GL_TEXTURE_2D_ARRAY,
		Texture3D = GLEnums.GL_TEXTURE_3D,
		TextureCubeMap = GLEnums.GL_TEXTURE_CUBE_MAP,
		TextureCubeMapArray = GLEnums.GL_TEXTURE_CUBE_MAP_ARRAY,
		TextureRectangle = GLEnums.GL_TEXTURE_RECTANGLE,
		TextureBuffer = GLEnums.GL_TEXTURE_BUFFER,
		Renderbuffer = GLEnums.GL_RENDERBUFFER,
		Texture2DMultisample = GLEnums.GL_TEXTURE_2D_MULTISAMPLE,
		Texture2DMultisampleArray = GLEnums.GL_TEXTURE_2D_MULTISAMPLE_ARRAY
	}

	public enum GLGetInternalFormat : uint {
		SampleCounts = GLEnums.GL_NUM_SAMPLE_COUNTS,
		Samples = GLEnums.GL_SAMPLES,
		InternalFormatSupported = GLEnums.GL_INTERNALFORMAT_SUPPORTED,
		InternalFormatPreferred = GLEnums.GL_INTERNALFORMAT_PREFERRED,
		InternalFormatRedSize = GLEnums.GL_INTERNALFORMAT_RED_SIZE,
		InternalFormatGreenSize = GLEnums.GL_INTERNALFORMAT_GREEN_SIZE,
		InternalFormatBlueSize = GLEnums.GL_INTERNALFORMAT_BLUE_SIZE,
		InternalFormatAlphaSize = GLEnums.GL_INTERNALFORMAT_ALPHA_SIZE,
		InternalFormatDepthSize = GLEnums.GL_INTERNALFORMAT_DEPTH_SIZE,
		InternalFormatStencilSize = GLEnums.GL_INTERNALFORMAT_STENCIL_SIZE,
		InternalFormatSharedSize = GLEnums.GL_INTERNALFORMAT_SHARED_SIZE,
		InternalFormatRedType = GLEnums.GL_INTERNALFORMAT_RED_TYPE,
		InternalFormatGreenType = GLEnums.GL_INTERNALFORMAT_GREEN_TYPE,
		InternalFormatBlueType = GLEnums.GL_INTERNALFORMAT_BLUE_TYPE,
		InternalFormatAlphaType = GLEnums.GL_INTERNALFORMAT_ALPHA_TYPE,
		InternalFormatDepthType = GLEnums.GL_INTERNALFORMAT_DEPTH_TYPE,
		InternalFormatStencilType = GLEnums.GL_INTERNALFORMAT_STENCIL_TYPE,
		MaxWidth = GLEnums.GL_MAX_WIDTH,
		MaxHeight = GLEnums.GL_MAX_HEIGHT,
		MaxLayers = GLEnums.GL_MAX_LAYERS,
		MaxCombinedDimensions = GLEnums.GL_MAX_COMBINED_DIMENSIONS,
		ColorComponents = GLEnums.GL_COLOR_COMPONENTS,
		DepthComponents = GLEnums.GL_DEPTH_COMPONENTS,
		StencilComponents = GLEnums.GL_STENCIL_COMPONENTS,
		ColorRenderable = GLEnums.GL_COLOR_RENDERABLE,
		DepthRenderable = GLEnums.GL_DEPTH_RENDERABLE,
		StencilRenderable = GLEnums.GL_STENCIL_RENDERABLE,
		FramebufferRenderable = GLEnums.GL_FRAMEBUFFER_RENDERABLE,
		FramebufferRenderableLayered = GLEnums.GL_FRAMEBUFFER_RENDERABLE_LAYERED,
		FramebufferBlend = GLEnums.GL_FRAMEBUFFER_BLEND,
		ReadPixels = GLEnums.GL_READ_PIXELS,
		ReadPixelsFormat = GLEnums.GL_READ_PIXELS_FORMAT,
		ReadPixelsType = GLEnums.GL_READ_PIXELS_TYPE,
		TextureImageFormat = GLEnums.GL_TEXTURE_IMAGE_FORMAT,
		TextureImageType = GLEnums.GL_TEXTURE_IMAGE_TYPE,
		GetTextureImageFormat = GLEnums.GL_GET_TEXTURE_IMAGE_FORMAT,
		GetTextureImageType = GLEnums.GL_GET_TEXTURE_IMAGE_TYPE,
		Mipmap = GLEnums.GL_MIPMAP,
		GenerateMipmap = GLEnums.GL_GENERATE_MIPMAP,
		AutoGenerateMipmap = GLEnums.GL_AUTO_GENERATE_MIPMAP,
		ColorEncoding = GLEnums.GL_COLOR_ENCODING,
		SRGBRead = GLEnums.GL_SRGB_READ,
		SRGBWrite = GLEnums.GL_SRGB_WRITE,
		Filter = GLEnums.GL_FILTER,
		VertexTexture = GLEnums.GL_VERTEX_TEXTURE,
		TessControlTexture = GLEnums.GL_TESS_CONTROL_TEXTURE,
		TessEvaluationTexture = GLEnums.GL_TESS_EVALUATION_TEXTURE,
		GeometryTexture = GLEnums.GL_GEOMETRY_TEXTURE,
		FragmentTexture = GLEnums.GL_FRAGMENT_TEXTURE,
		ComputeTexture = GLEnums.GL_COMPUTE_TEXTURE,
		TextureShadow = GLEnums.GL_TEXTURE_SHADOW,
		TextureGather = GLEnums.GL_TEXTURE_GATHER,
		TextureGatherShadow = GLEnums.GL_TEXTURE_GATHER_SHADOW,
		ShaderImageLoad = GLEnums.GL_SHADER_IMAGE_LOAD,
		ShaderImageStore = GLEnums.GL_SHADER_IMAGE_STORE,
		ShaderImageAtomic = GLEnums.GL_SHADER_IMAGE_ATOMIC,
		ImageTexelSize = GLEnums.GL_IMAGE_TEXEL_SIZE,
		ImageCompatibilityClass = GLEnums.GL_IMAGE_COMPATIBILITY_CLASS,
		ImagePixelFormat = GLEnums.GL_IMAGE_PIXEL_FORMAT,
		ImagePixelType = GLEnums.GL_IMAGE_PIXEL_TYPE,
		ImageFormatCompatibilityType = GLEnums.GL_IMAGE_FORMAT_COMPATIBILITY_TYPE,
		SimultaneousTextureAndDepthTest = GLEnums.GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_TEST,
		SimultaneousTextureAndStencilTest = GLEnums.GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_TEST,
		SimultaneousTextureAndDepthWrite = GLEnums.GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_WRITE,
		SimultaneousTextureAndStencilWrite = GLEnums.GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_WRITE,
		TextureCompressed = GLEnums.GL_TEXTURE_COMPRESSED,
		TextureCompressedBlockWidth = GLEnums.GL_TEXTURE_COMPRESSED_BLOCK_WIDTH,
		TextureCompressedBlockHeight = GLEnums.GL_TEXTURE_COMPRESSED_BLOCK_HEIGHT,
		TextureCompressedBlockSize = GLEnums.GL_TEXTURE_COMPRESSED_BLOCK_SIZE,
		ClearBuffer = GLEnums.GL_CLEAR_BUFFER,
		TextureView = GLEnums.GL_TEXTURE_VIEW,
		ViewCompatibilityClass = GLEnums.GL_VIEW_COMPATIBILITY_CLASS,
		ClearTexture = GLEnums.GL_CLEAR_TEXTURE
	}

	public enum GLIdentifier : uint {
		Buffer = GLEnums.GL_BUFFER,
		Shader = GLEnums.GL_SHADER,
		Program = GLEnums.GL_PROGRAM,
		VertexArray = GLEnums.GL_VERTEX_ARRAY,
		Query = GLEnums.GL_QUERY,
		ProgramPipeline = GLEnums.GL_PROGRAM_PIPELINE,
		TransformFeedback = GLEnums.GL_TRANSFORM_FEEDBACK,
		Sampler = GLEnums.GL_SAMPLER,
		Texture = GLEnums.GL_TEXTURE,
		Renderbuffer = GLEnums.GL_RENDERBUFFER,
		Framebuffer = GLEnums.GL_FRAMEBUFFER
	}

	public enum GLFramebufferParameter : uint {
		DefaultWidth = GLEnums.GL_FRAMEBUFFER_DEFAULT_WIDTH,
		DefaultHeight = GLEnums.GL_FRAMEBUFFER_DEFAULT_HEIGHT,
		DefaultLayers = GLEnums.GL_FRAMEBUFFER_DEFAULT_LAYERS,
		DefaultSamples = GLEnums.GL_FRAMEBUFFER_DEFAULT_SAMPLES,
		DefaultFixedSampleLocations = GLEnums.GL_FRAMEBUFFER_DEFAULT_FIXED_SAMPLE_LOCATIONS
	}

	public enum GLProgramInterface : uint {
		Uniform = GLEnums.GL_UNIFORM,
		UniformBlock = GLEnums.GL_UNIFORM_BLOCK,
		ProgramInput = GLEnums.GL_PROGRAM_INPUT,
		ProgramOutput = GLEnums.GL_PROGRAM_OUTPUT,
		BufferVariable = GLEnums.GL_BUFFER_VARIABLE,
		ShaderStorageBlock = GLEnums.GL_SHADER_STORAGE_BLOCK,
		AtomicCounterBuffer = GLEnums.GL_ATOMIC_COUNTER_BUFFER,
		VertexSubroutine = GLEnums.GL_VERTEX_SUBROUTINE,
		TessControlSubroutine = GLEnums.GL_TESS_CONTROL_SUBROUTINE,
		TessEvaluationSubroutine = GLEnums.GL_TESS_EVALUATION_SUBROUTINE,
		GeometrySubroutine = GLEnums.GL_GEOMETRY_SUBROUTINE,
		FragmentSubroutine = GLEnums.GL_FRAGMENT_SUBROUTINE,
		ComputeSubroutine = GLEnums.GL_COMPUTE_SUBROUTINE,
		VertexSubroutineUniform = GLEnums.GL_VERTEX_SUBROUTINE_UNIFORM,
		TessControlSubroutineUniform = GLEnums.GL_TESS_CONTROL_SUBROUTINE_UNIFORM,
		TessEvaluationSubroutineUniform = GLEnums.GL_TESS_EVALUATION_SUBROUTINE_UNIFORM,
		GeometrySubroutineUniform = GLEnums.GL_GEOMETRY_SUBROUTINE_UNIFORM,
		FragmentSubroutineUniform = GLEnums.GL_FRAGMENT_SUBROUTINE_UNIFORM,
		ComputeSubroutineUniform = GLEnums.GL_COMPUTE_SUBROUTINE_UNIFORM,
		TransformFeedbackVarying = GLEnums.GL_TRANSFORM_FEEDBACK_VARYING
	}

	public enum GLGetProgramInterface : uint {
		ActiveResources = GLEnums.GL_ACTIVE_RESOURCES,
		MaxNameLength = GLEnums.GL_MAX_NAME_LENGTH,
		MaxNumActiveVariables = GLEnums.GL_MAX_NUM_ACTIVE_VARIABLES,
		MaxNumCompatibleSubroutines = GLEnums.GL_MAX_NUM_COMPATIBLE_SUBROUTINES
	}

	public enum GLGetProgramResource : uint {
		NameLength = GLEnums.GL_NAME_LENGTH,
		Type = GLEnums.GL_TYPE,
		ArraySize = GLEnums.GL_ARRAY_SIZE,
		Offset = GLEnums.GL_OFFSET,
		BlockIndex = GLEnums.GL_BLOCK_INDEX,
		ArrayStride = GLEnums.GL_ARRAY_STRIDE,
		MatrixStride = GLEnums.GL_MATRIX_STRIDE,
		IsRowMajor = GLEnums.GL_IS_ROW_MAJOR,
		AtomicCounterBufferIndex = GLEnums.GL_ATOMIC_COUNTER_BUFFER_INDEX,
		BufferBinding = GLEnums.GL_BUFFER_BINDING,
		BufferDataSize = GLEnums.GL_BUFFER_DATA_SIZE,
		NumActiveVariables = GLEnums.GL_NUM_ACTIVE_VARIABLES,
		ActiveVariables = GLEnums.GL_ACTIVE_VARIABLES,
		ReferencedByVertexShader = GLEnums.GL_REFERENCED_BY_VERTEX_SHADER,
		ReferencedByTessControlShader = GLEnums.GL_REFERENCED_BY_TESS_CONTROL_SHADER,
		ReferencedByTessEvaluationShader = GLEnums.GL_REFERENCED_BY_TESS_EVALUATION_SHADER,
		ReferencedByGeometryShader = GLEnums.GL_REFERENCED_BY_GEOMETRY_SHADER,
		ReferencedByFragmentShader = GLEnums.GL_REFERENCED_BY_FRAGMENT_SHADER,
		ReferencedByComputeShader = GLEnums.GL_REFERENCED_BY_COMPUTE_SHADER,
		TopLevelArraySize = GLEnums.GL_TOP_LEVEL_ARRAY_SIZE,
		TopLevelArrayStride = GLEnums.GL_TOP_LEVEL_ARRAY_STRIDE,
		Location = GLEnums.GL_LOCATION,
		LocationIndex = GLEnums.GL_LOCATION_INDEX,
		IsPerPatch = GLEnums.GL_IS_PER_PATCH,
		NumCompatibleSubroutines = GLEnums.GL_NUM_COMPATIBLE_SUBROUTINES,
		CompatibleSubroutines = GLEnums.GL_COMPATIBLE_SUBROUTINES
	}

	public enum GLBufferStorageFlags : uint {
		MapRead = GLEnums.GL_MAP_READ_BIT,
		MapWrite = GLEnums.GL_MAP_WRITE_BIT,
		MapPersistent = GLEnums.GL_MAP_PERSISTENT_BIT,
		MapCoherent = GLEnums.GL_MAP_COHERENT_BIT,
		DynamicStorage = GLEnums.GL_DYNAMIC_STORAGE_BIT,
		ClientStage = GLEnums.GL_CLIENT_STORAGE_BIT
	}

	public enum GLClipDepth : uint {
		NegativeOneToOne = GLEnums.GL_NEGATIVE_ONE_TO_ONE,
		ZeroToOne = GLEnums.GL_ZERO_TO_ONE
	}

	public enum GLGraphicsResetStatus : uint {
		NoError = GLEnums.GL_NO_ERROR,
		GuiltyContextReset = GLEnums.GL_GUILTY_CONTEXT_RESET,
		InnocentContextReset = GLEnums.GL_INNOCENT_CONTEXT_RESET,
		UnknownContextReset = GLEnums.GL_UNKNOWN_CONTEXT_RESET
	}

	public enum GLGetTransformFeedback : uint {
		BufferBinding = GLEnums.GL_TRANSFORM_FEEDBACK_BUFFER_BINDING,
		BufferStart = GLEnums.GL_TRANSFORM_FEEDBACK_BUFFER_START,
		BufferSize = GLEnums.GL_TRANSFORM_FEEDBACK_BUFFER_SIZE,
		Paused = GLEnums.GL_TRANSFORM_FEEDBACK_BUFFER_PAUSED,
		Active = GLEnums.GL_TRANSFORM_FEEDBACK_ACTIVE
	}

	public enum GLGetTexLevelParameter : uint {
		Width = GLEnums.GL_TEXTURE_WIDTH,
		Height = GLEnums.GL_TEXTURE_HEIGHT,
		Depth = GLEnums.GL_TEXTURE_DEPTH,
		InternalFormat = GLEnums.GL_TEXTURE_INTERNAL_FORMAT,
		RedSize = GLEnums.GL_TEXTURE_RED_SIZE,
		GreenSize = GLEnums.GL_TEXTURE_GREEN_SIZE,
		BlueSize = GLEnums.GL_TEXTURE_BLUE_SIZE,
		AlphaSize = GLEnums.GL_TEXTURE_ALPHA_SIZE,
		DepthSize = GLEnums.GL_TEXTURE_DEPTH_SIZE,
		Compressed = GLEnums.GL_TEXTURE_COMPRESSED,
		CompressedImageSize = GLEnums.GL_TEXTURE_COMPRESSED_IMAGE_SIZE,
		BufferOffset = GLEnums.GL_TEXTURE_BUFFER_OFFSET
	}

	public enum GLGetVertexArray : uint {
		ElementArrayBufferBinding = GLEnums.GL_ELEMENT_ARRAY_BUFFER_BINDING
	}

	public enum GLGetVertexArrayIndexed : uint {
		AttribArrayEnabled = GLEnums.GL_VERTEX_ATTRIB_ARRAY_ENABLED,
		AttribArraySize = GLEnums.GL_VERTEX_ATTRIB_ARRAY_SIZE,
		AttribArrayStride = GLEnums.GL_VERTEX_ATTRIB_ARRAY_STRIDE,
		AttribArrayType = GLEnums.GL_VERTEX_ATTRIB_ARRAY_TYPE,
		AttribArrayNormalized = GLEnums.GL_VERTEX_ATTRIB_ARRAY_NORMALIZED,
		AttribArrayInteger = GLEnums.GL_VERTEX_ATTRIB_ARRAY_INTEGER,
		AttribArrayLong = GLEnums.GL_VERTEX_ATTRIB_ARRAY_LONG,
		AttribArrayDivisor = GLEnums.GL_VERTEX_ATTRIB_ARRAY_DIVISOR,
		AttribRelativeOffset = GLEnums.GL_VERTEX_ATTRIB_RELATIVE_OFFSET,
		BindingOffset = GLEnums.GL_VERTEX_BINDING_OFFSET
	}

	public enum GLTextureWrap : uint {
		Repeat = GLEnums.GL_REPEAT,
		MirroredRepeat = GLEnums.GL_MIRRORED_REPEAT,
		ClampToEdge = GLEnums.GL_CLAMP_TO_EDGE,
		ClampToBorder = GLEnums.GL_CLAMP_TO_BORDER,
		MirrorClampToEdge = GLEnums.GL_MIRROR_CLAMP_TO_EDGE
	}

	public enum GLTextureSwizzle : uint {
		Red = GLEnums.GL_RED,
		Green = GLEnums.GL_GREEN,
		Blue = GLEnums.GL_BLUE,
		Alpha = GLEnums.GL_ALPHA,
		One = GLEnums.GL_ONE,
		Zero = GLEnums.GL_ZERO
	}

	public enum GLWaitResult : uint {
		AlreadySignaled = GLEnums.GL_ALREADY_SIGNALED,
		TimeoutExpired = GLEnums.GL_TIMEOUT_EXPIRED,
		ConditionSatisfied = GLEnums.GL_CONDITION_SATISFIED,
		WaitFailed = GLEnums.GL_WAIT_FAILED
	}

	public enum GLCopyImageTarget : uint {
		Renderbuffer = GLEnums.GL_RENDERBUFFER,

		Texture1D = GLEnums.GL_TEXTURE_1D,
		Texture1DArray = GLEnums.GL_TEXTURE_1D_ARRAY,
		Texture2D = GLEnums.GL_TEXTURE_2D,
		Texture2DArray = GLEnums.GL_TEXTURE_2D_ARRAY,
		Texture2DMultisample = GLEnums.GL_TEXTURE_2D_MULTISAMPLE,
		Texture2DMultisampleArray = GLEnums.GL_TEXTURE_2D_MULTISAMPLE_ARRAY,
		Texture3D = GLEnums.GL_TEXTURE_3D,
		CubeMap = GLEnums.GL_TEXTURE_CUBE_MAP,
		CubeMapArray = GLEnums.GL_TEXTURE_CUBE_MAP_ARRAY,
		Rectangle = GLEnums.GL_TEXTURE_RECTANGLE,
		CubeMapPositiveX = GLEnums.GL_TEXTURE_CUBE_MAP_POSITIVE_X,
		CubeMapNegativeX = GLEnums.GL_TEXTURE_CUBE_MAP_NEGATIVE_X,
		CubeMapPositiveY = GLEnums.GL_TEXTURE_CUBE_MAP_POSITIVE_Y,
		CubeMapNegativeY = GLEnums.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y,
		CubeMapPositiveZ = GLEnums.GL_TEXTURE_CUBE_MAP_POSITIVE_Z,
		CubeMapNegativeZ = GLEnums.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z
	}

	public enum GLPixelStoreParam : uint {
		PackSwapBytes = GLEnums.GL_PACK_SWAP_BYTES,
		PackLSBFirst = GLEnums.GL_PACK_LSB_FIRST,
		PackRowLength = GLEnums.GL_PACK_ROW_LENGTH,
		PackImageHeight = GLEnums.GL_PACK_IMAGE_HEIGHT,
		PackSkipPixels = GLEnums.GL_PACK_SKIP_PIXELS,
		PackSkipRows = GLEnums.GL_PACK_SKIP_ROWS,
		PackSkipImages = GLEnums.GL_PACK_SKIP_IMAGES,
		PackAlignment = GLEnums.GL_PACK_ALIGNMENT,
		UnpackSwapBytes = GLEnums.GL_UNPACK_SWAP_BYTES,
		UnpackLSBFirst = GLEnums.GL_UNPACK_LSB_FIRST,
		UnpackRowLength = GLEnums.GL_UNPACK_ROW_LENGTH,
		UnpackImageHeight = GLEnums.GL_UNPACK_IMAGE_HEIGHT,
		UnpackSkipPixels = GLEnums.GL_UNPACK_SKIP_PIXELS,
		UnpackSkipRows = GLEnums.GL_UNPACK_SKIP_ROWS,
		UnpackSkipImages = GLEnums.GL_UNPACK_SKIP_IMAGES,
		UnpackAlignment = GLEnums.GL_UNPACK_ALIGNMENT
	}

}

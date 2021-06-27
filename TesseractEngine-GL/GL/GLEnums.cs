using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.GL.Native;

namespace Tesseract.GL {
	
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
		Rectangle = GLEnums.GL_TEXTURE_RECTANGLE
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
		Float = GLEnums.GL_FLOAT
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
		UnsignedInt_2_10_10_10_Rev = GLEnums.GL_UNSIGNED_INT_2_10_10_10_REV
	}

	public enum GLFormat : uint {
		R = GLEnums.GL_RED,
		RG = GLEnums.GL_RG,
		RGB = GLEnums.GL_RGB,
		BGR = GLEnums.GL_BGR,
		RGBA = GLEnums.GL_RGBA,
		BGRA = GLEnums.GL_BGRA
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
		CompressedRGB_BPTC_UFloat = GLEnums.GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT
	}

	public enum GLQueryTarget : uint {
		SamplesPassed = GLEnums.GL_SAMPLES_PASSED,
		AnySamplesPassed = GLEnums.GL_ANY_SAMPLES_PASSED,
		AnySamplesPassedConservative = GLEnums.GL_ANY_SAMPLES_PASSED_CONSERVATIVE,
		PrimitivesGenerated = GLEnums.GL_PRIMITIVES_GENERATED,
		TransformFeedbackPrimitivesWritten = GLEnums.GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN,
		TimeElapsed = GLEnums.GL_TIME_ELAPSED
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
		Uniform = GLEnums.GL_UNIFORM_BUFFER
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

	public enum GLBufferParameter : uint {
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
		ResultAvailable = GLEnums.GL_QUERY_RESULT_AVAILABLE
	}

	public enum GLGetQueryTarget : uint {
		SamplesPassed = GLEnums.GL_SAMPLES_PASSED,
		AnySamplesPassed = GLEnums.GL_ANY_SAMPLES_PASSED,
		AnySamplesPassedConservative = GLEnums.GL_ANY_SAMPLES_PASSED_CONSERVATIVE,
		PrimitivesGenerated = GLEnums.GL_PRIMITIVES_GENERATED,
		TransformFeedbackPrimitivesWritten = GLEnums.GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN,
		TimeElapsed = GLEnums.GL_TIME_ELAPSED,
		Timestamp = GLEnums.GL_TIMESTAMP
	}

	public enum GLGetQuery : uint {
		CurrentQuery = GLEnums.GL_CURRENT_QUERY,
		CounterBits = GLEnums.GL_QUERY_COUNTER_BITS
	}

	public enum GLMapAccess : uint {
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

	public enum GLClearMask : uint {
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
		ProgramPointSize = GLEnums.GL_PROGRAM_POINT_SIZE
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

	public enum GLPointOrigin : uint {
		LowerLeft = GLEnums.GL_LOWER_LEFT,
		UpperLeft = GLEnums.GL_UPPER_LEFT
	}

	public enum GLShaderType : uint {
		Vertex = GLEnums.GL_VERTEX_SHADER,
		TessellationControl = GLEnums.GL_TESS_CONTROL_SHADER,
		TessellationEvaluation = GLEnums.GL_TESS_EVALUATION_SHADER,
		Geometry = GLEnums.GL_GEOMETRY_SHADER,
		Fragment = GLEnums.GL_FRAGMENT_SHADER
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

	public enum GLClampColorTarget : uint {
		ClampReadColor = GLEnums.GL_CLAMP_READ_COLOR
	}

	public enum GLClearBuffer : uint {
		Color = GLEnums.GL_COLOR,
		Depth = GLEnums.GL_DEPTH,
		Stencil = GLEnums.GL_STENCIL
	}

	public enum GLIndexedCapability : uint {
		Blend = GLEnums.GL_BLEND,
		ScissorTest = GLEnums.GL_SCISSOR_TEST
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

	public enum GLFeedbackBufferMode : uint {
		InterleavedAttribs = GLEnums.GL_INTERLEAVED_ATTRIBS,
		SeparateAttribs = GLEnums.GL_SEPARATE_ATTRIBS
	}

}

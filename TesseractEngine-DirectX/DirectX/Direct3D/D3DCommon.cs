using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Windows;

namespace Tesseract.DirectX.Direct3D {
	
	public enum D3DDriverType {
		Unknown,
		Hardware,
		Reference,
		Null,
		Software,
		Warp
	}

	[Flags]
	public enum D3DFeatureLevel {
		Level1_0Core = 0x1000,
		Level9_1 = 0x9100,
		Level9_2 = 0x9200,
		Level9_3 = 0x9300,
		Level10_0 = 0xA000,
		Level10_1 = 0xA100,
		Level11_0 = 0xB000,
		Level11_1 = 0xB100,
		Level12_0 = 0xC000,
		Level12_1 = 0xC100,
		Level12_2 = 0xC200
	}

	public enum D3DPrimitiveTopology {
		Undefined = 0,
		PointList,
		LineList,
		LineStrip,
		TriangleList,
		TriangleStrip,
		TriangleFan,

		LineListAdj = 10,
		LineStripAdj,
		TriangleListAdj,
		TriangleStripAdj,

		_1ControlPointPatchList = 33,
		_2ControlPointPatchList,
		_3ControlPointPatchList,
		_4ControlPointPatchList,
		_5ControlPointPatchList,
		_6ControlPointPatchList,
		_7ControlPointPatchList,
		_8ControlPointPatchList,
		_9ControlPointPatchList,
		_10ControlPointPatchList,
		_11ControlPointPatchList,
		_12ControlPointPatchList,
		_13ControlPointPatchList,
		_14ControlPointPatchList,
		_15ControlPointPatchList,
		_16ControlPointPatchList,
		_17ControlPointPatchList,
		_18ControlPointPatchList,
		_19ControlPointPatchList,
		_20ControlPointPatchList,
		_21ControlPointPatchList,
		_22ControlPointPatchList,
		_23ControlPointPatchList,
		_24ControlPointPatchList,
		_25ControlPointPatchList,
		_26ControlPointPatchList,
		_27ControlPointPatchList,
		_28ControlPointPatchList,
		_29ControlPointPatchList,
		_30ControlPointPatchList,
		_31ControlPointPatchList,
		_32ControlPointPatchList
	}

	public enum D3DPrimitive {
		Undefined = 0,
		Point,
		Line,
		Triangle,

		LineAdj = 6,
		TriangleAdj,

		_1ControlPointPatch,
		_2ControlPointPatchList,
		_3ControlPointPatchList,
		_4ControlPointPatchList,
		_5ControlPointPatchList,
		_6ControlPointPatchList,
		_7ControlPointPatchList,
		_8ControlPointPatchList,
		_9ControlPointPatchList,
		_10ControlPointPatchList,
		_11ControlPointPatchList,
		_12ControlPointPatchList,
		_13ControlPointPatchList,
		_14ControlPointPatchList,
		_15ControlPointPatchList,
		_16ControlPointPatchList,
		_17ControlPointPatchList,
		_18ControlPointPatchList,
		_19ControlPointPatchList,
		_20ControlPointPatchList,
		_21ControlPointPatchList,
		_22ControlPointPatchList,
		_23ControlPointPatchList,
		_24ControlPointPatchList,
		_25ControlPointPatchList,
		_26ControlPointPatchList,
		_27ControlPointPatchList,
		_28ControlPointPatchList,
		_29ControlPointPatchList,
		_30ControlPointPatchList,
		_31ControlPointPatchList,
		_32ControlPointPatchList
	}

	public enum D3DSrvDimension {
		Unknown = 0,
		Buffer,
		Texture1D,
		Texture1DArray,
		Texture2D,
		Texture2DArray,
		Texture2DMS,
		Texture2DMSArray,
		Texture3D,
		TextureCube,
		TextureCubeArray,
		BufferEx
	}

	[Flags]
	public enum D3DShaderFeature {
		Doubles = 0x00001,
		ComputeShadersPlusRawAndUnstructuredBuffersViaShader4X = 0x00002,
		UAVsAtEveryStage = 0x00004,
		_64UAVs = 0x00008,
		MinimumPrecision = 0x00010,
		_11_1_DoubleExtensions = 0x00020,
		_11_1_ShaderExtensions = 0x00040,
		Level9ComparisonFiltering = 0x00080,
		TiledResources = 0x00100,
		StencilRef = 0x00200,
		InnerCoverage = 0x00400,
		TypedUAVLoadAdditionalFormats = 0x00800,
		ROVs = 0x01000,
		ViewportAndRTArrayIndexFromAnyShaderFeedingRasterizer = 0x02000,
		WaveOps = 0x04000,
		Int64Ops = 0x08000,
		ViewID = 0x10000,
		Barycentrics = 0x20000,
		Native16BitOps = 0x40000,
		ShadingRate = 0x80000,
		RaytracingTier1_1 = 0x100000,
		SamplerFeedback = 0x200000,
		AtomicInt64OnTypedResource = 0x400000,
		AtomicInt64OnGroupShared = 0x800000,
		DerivativesInMeshAndAmplificationShaders = 0x1000000,
		ResourceDescriptorHeapIndexing = 0x2000000,
		SamplerDescriptorHeapIndexing = 0x4000000,
		WaveMMA = 0x8000000,
		AtomicInt64OnDescriptorHeapResource = 0x10000000,
		AdvancedTextureOps = 0x20000000,
		WritableMSAATextures = 0x40000000
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D3DShaderMacro {

		[MarshalAs(UnmanagedType.LPStr)]
		public string Name;
		[MarshalAs(UnmanagedType.LPStr)]
		public string Description;

	}

	[ComImport, Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID3D10Blob : IUnknown {

		[PreserveSig]
		public IntPtr GetBufferPointer();

		[PreserveSig]
		public nuint GetBufferSize();

	}

	[ComImport, Guid("a06eb39a-50da-425b-8c31-4eecd6c270f3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID3DDestructionNotifier : IUnknown {

		public uint RegisterDestructionCallback([MarshalAs(UnmanagedType.FunctionPtr)] Releaser callbackFn, IntPtr data);

		public void UnregisterDestructionCallback(uint callbackId);

	}

	public enum D3DIncludeType : int {
		Local,
		System
	}

	// TODO: Not declared with a GUID, does this need anything special to marshal as a CCW correctly?
	public interface ID3DInclude {

		public void Open(D3DIncludeType includeType, [MarshalAs(UnmanagedType.LPStr)] string fileName, IntPtr pParentData, [NativeType("LPCVOID*")] IntPtr ppData, out uint bytes);

		public void Close(IntPtr data);

	}

	public enum D3DShaderVariableClass : int {
		Scalar,
		Vector,
		MatrixRows,
		MatrixColumns,
		Object,
		Struct,
		InterfaceClass,
		InterfacePointer
	}

	[Flags]
	public enum D3DShaderVariableFlags : int {
		UserPacked = 1,
		Used = 2,
		InterfacePointer = 4,
		InterfaceParameter = 8
	}

	public enum D3DShaderVariableType : int {
		Void = 0,
		Bool,
		Int,
		Float,
		String,
		Texture,
		Texture1D,
		Texture2D,
		Texture3D,
		TextureCube,
		Sampler,
		Sampler1D,
		Sampler2D,
		Sampler3D,
		SamplerCube,
		PixelShader,
		VertexShader,
		PixelFragment,
		VertexFragment,
		UInt,
		UInt8,
		GeometryShader,
		Rasterizer,
		DepthStencil,
		Blend,
		Buffer,
		CBuffer,
		TBuffer,
		Texture1DArray,
		Texture2DArray,
		RenderTargetView,
		DepthStencilView,
		Texture2DMS,
		Texture2DMSArray,
		TextureCubeArray,
		HullShader,
		DomainShader,
		InterfacePointer,
		ComputeShader,
		Double,
		RWTexture1D,
		RWTexture1DArray,
		RWTexture2D,
		RWTexture2DArray,
		RWTexture3D,
		RWBuffer,
		ByteAddressBuffer,
		RWByteAddressBuffer,
		StructuredBuffer,
		RWStructuredBuffer,
		AppendStructuredBuffer,
		ConsumeStructuredBuffer,
		Min8Float,
		Min10Float,
		Min16Float,
		Min12Int,
		Min16Int,
		Min16UInt,
		Int16,
		UInt16,
		Float16,
		Int64,
		UInt64
	}

	[Flags]
	public enum D3DShaderInputFlags : int {
		UserPacked = 0x01,
		ComparisonShader = 0x02,
		TextureComponent0 = 0x04,
		TextureComponent1 = 0x08,
		TextureComponents = 0x0C,
		Unused = 0x10
	}

	public enum D3DShaderInputType {
		CBuffer,
		TBuffer,
		Texture,
		Sampler,
		UAV_RWTyped,
		Structured,
		UAV_RWStructured,
		ByteAddress,
		UAV_RWByteAddress,
		UAVAppendStructured,
		UAVConsumeStructured,
		UAV_RWStructuredWithCounter,
		RTAccelerationStructure,
		UAVFeedbackTexture
	}

	[Flags]
	public enum D3DShaderCBufferFlags : int {
		UserPacked = 1
	}

	public enum D3DCBufferType {
		CBuffer,
		TBuffer,
		InterfacePointers,
		ResourceBindInfo
	}

	public enum D3DName {
		Undefined = 0,
		Position,
		ClipDistance,
		CullDistance,
		RenderTargetArrayIndex,
		ViewportArrayIndex,
		VertexID,
		PrimitiveID,
		InstanceID,
		IsFrontFace,
		SampleIndex,
		FinalQuadEdgeTessFactor,
		FinalQuadInsideTessFactor,
		TriEdgeTessFactor,
		TriInsideTessFactor,
		LineDetailTessFactor,
		LineDensityTessFactor,
		Barycentrics = 23,
		ShadingRate,
		CullPrimitive,
		Target = 64,
		Depth,
		Coverage,
		DepthGreaterEqual,
		DepthLessEqual,
		StencilRef,
		InnerCoverage
	}

	public enum D3DResourceReturnType {
		UNorm = 1,
		SNorm,
		SInt,
		UInt,
		Float,
		Mixed,
		Double,
		Continued
	}

	public enum D3DRegisterComponentType {
		Unknown = 0,
		UInt32,
		SInt32,
		Float32
	}

	public enum D3DTesselatorDomain {
		Undefined = 0,
		IsoLine,
		Tri,
		Quad
	}

	public enum D3DTessellatorPartitioning {
		Undefined = 0,
		Integer,
		Pow2,
		FractionalOdd,
		FractionalEven
	}

	public enum D3DTessellatorOutputPrimitive {
		Undefined = 0,
		Point,
		Line,
		TriangleCW,
		TriangleCCW
	}

	public enum D3DMinPrecision {
		Default = 0,
		Float16,
		Float2_8,
		Reserved,
		SInt16,
		UInt16,
		PrecisionAny16 = 0xF0,
		PrecisionAny10 = 0xF1
	}

	public enum D3DInterpolationMode : int {
		Undefined = 0,
		Constant,
		Linear,
		LinearCentroid,
		LinearNoPerspective,
		LinearNoPerspectiveCentroid,
		LinearSample,
		LinearNoPerspectiveSample
	}

	[Flags]
	public enum D3DParameterFlags : int {
		None = 0,
		In = 1,
		Out = 2
	}

	public enum D3DFormatLayout {
		Standard = 0,
		Custom = -1
	}

	public enum D3DFormatTypeLevel {
		NoType = 0,
		PartialType = -2,
		FullType = -1
	}

	public enum D3DFormatComponentName {
		R = -4,
		G = -3,
		B = -2,
		A = -1,
		D = 0,
		S = 1,
		X = 2
	}

	public enum D3DFormatComponentInterpretation {
		Typeless = 0,
		Float = -4,
		SNorm = -3,
		UNorm = -2,
		SInt = -1,
		UInt = 1,
		UNormSRGB = 2,
		BiasedFixed2_8 = 3
	}

	[Flags]
	public enum D3DComponentMask {
		X = 1,
		Y = 2,
		Z = 4,
		W = 8
	}

	public static class D3D {

		public const int FL9_1ReqTexture1DUDimension = 2048;
		public const int FL9_3ReqTexture1DUDimension = 4096;
		public const int FL9_1ReqTexture2DUOrVDimension = 2048;
		public const int FL9_3ReqTexture2DUOrVDimension = 4096;
		public const int FL9_1ReqtextureCubeDimension = 512;
		public const int FL9_3ReqTextureCubeDimension = 4096;
		public const int FL9_1ReqTexture3DUVOrWDimension = 256;
		public const int FL9_1DefaultMaxAnisotropy = 2;
		public const int FL9_1IAPrimitiveMaxCount = 65535;
		public const int FL9_2IAPrimitiveMaxCount = 1048575;
		public const int FL9_1SimultaneousRenderTargetCount = 1;
		public const int FL9_3SimultaneousRenderTargetCount = 4;
		public const int FL9_1MaxTextureRepeat = 128;
		public const int FL9_2MaxTextureRepeat = 2048;
		public const int FL9_3MaxTextureRepeat = 8192;

		public static readonly Guid WKPDIDDebugObjectName = new(0x429b8c22, 0x9188, 0x4b0c, 0x87, 0x42, 0xac, 0xb0, 0xbf, 0x85, 0xc2, 0x00);
		public static readonly Guid WKPDIDDebugObjectNameW = new(0x4cca5fd8, 0x921f, 0x42c8, 0x85, 0x66, 0x70, 0xca, 0xf2, 0xa9, 0xb7, 0x41);
		public static readonly Guid WKPDIDCommentStringW = new(0xd0149dc0, 0x90e8, 0x4ec8, 0x81, 0x44, 0xe9, 0x00, 0xad, 0x26, 0x6b, 0xb2);
		public static readonly Guid WKPDID_D3D12UniqueObjectID = new(0x1b39de15, 0xec04, 0x4bae, 0xba, 0x4d, 0x8c, 0xef, 0x79, 0xfc, 0x04, 0xc1);

		public static readonly Guid TextureLayoutRowMajor = new(0xb5dc234f, 0x72bb, 0x4bec, 0x97, 0x05, 0x8c, 0xf2, 0x58, 0xdf, 0x6b, 0x6c);
		public static readonly Guid TextureLayout64KBStandardSwizzle = new(0x4c0f29e3, 0x3f5f, 0x4d35, 0x84, 0xc9, 0xbc, 0x09, 0x83, 0xb6, 0x2c, 0x28);

	}

}

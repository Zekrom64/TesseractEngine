using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;

namespace Tesseract.WebGPU {


	// Note: Typedef buried in emscripten/em_types.h, it uses a 32-bit int as a boolean.

	[StructLayout(LayoutKind.Sequential)]
	public struct EMBOOL {

		public int Value;

		public EMBOOL(int x) { Value = x; }

		public static implicit operator EMBOOL(int value) => new(value);

		public static implicit operator int(EMBOOL value) => value.Value;

		public static implicit operator EMBOOL(bool value) => new(value ? 1 : 0);

		public static implicit operator bool(EMBOOL value) => value.Value != 0;

	}

	// Note: Empscripten can only pass at maximum 53-bit integers through functions via
	// doubles so we use a special type to enforce constraints on this. It is currently
	// unlikely that GPUs will use memory beyond the 9EB limit of this :P

	[StructLayout(LayoutKind.Sequential)]
	public struct DoubleInt53 {

		public const ulong MaxValue = (1UL << 53) - 1;

		private readonly double value;

		private DoubleInt53(double value) { this.value = value; }

		public static implicit operator DoubleInt53(ulong x) {
#if DEBUG
			if (x > MaxValue) throw new ArgumentOutOfRangeException(nameof(x), "Integer value exceeds the range of a double");
#endif
			return new DoubleInt53(x);
		}

		public static implicit operator ulong(DoubleInt53 x) => (ulong)x.value;

	}

	// Note: WebGPU uses some (currently) empty structs that need non-null pointers, so
	// we pass this dummy structure instead.

	public readonly struct DummyStruct {

		[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Padding member added to match the layout described in Emscripten")]
		private readonly uint _padding;
	
	}


	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuObjectDescriptorBase {

		private unsafe fixed byte label[WGpu.ObjectLabelMaxLength];

		public static implicit operator string(WGpuObjectDescriptorBase x) {
			unsafe {
				return MemoryUtil.GetUTF8((IntPtr)x.label, WGpu.ObjectLabelMaxLength)!;
			}
		}

		public static implicit operator WGpuObjectDescriptorBase(string s) {
			WGpuObjectDescriptorBase desc = new();
			unsafe {
				MemoryUtil.PutUTF8(s, (IntPtr)desc.label, WGpu.ObjectLabelMaxLength);
			}
			return desc;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuExtent3D {

		public int Width;

		public int Height;

		public int DepthOrArrayLayers;

		public WGpuExtent3D() {
			Width = 0;
			Height = 1;
			DepthOrArrayLayers = 1;
		}

		public static implicit operator WGpuExtent3D(Vector3i v) => new() { Width = v.X, Height = v.Y, DepthOrArrayLayers = v.Z };

		public static implicit operator Vector3i(WGpuExtent3D v) => new(v.Width, v.Height, v.DepthOrArrayLayers);

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct  WGpuSupportedLimits {

		public ulong MaxUniformBufferBindingSize;

		public ulong MaxStorageBufferBindingSize;

		public ulong MaxBufferSize;

		public uint MaxTextureDimension1D;

		public uint MaxTextureDimension2D;

		public uint MaxTextureDimension3D;

		public uint MaxTextureArrayLayers;

		public uint MaxBindGroups;

		public uint MaxBindingsPerBindGroup;

		public uint MaxDynamicUniformBuffersPerPipelineLayout;

		public uint maxDynamicStorageBuffersPerPipelineLayout;

		public uint MaxSampledTexturesPerShaderStage;

		public uint MaxSamplersPerShaderStage;

		public uint MaxStorageBuffersPerShaderStage;

		public uint MaxStorageTexturesPerShaderStage;

		public uint MaxUniformBuffersPerShaderStage;

		public uint MinUniformBufferOffsetAlignment;

		public uint MinStorageBufferOffsetAlignment;

		public uint MaxVertexBuffers;

		public uint MaxVertexAttributes;

		public uint MaxVertexBufferArrayStride;

		public uint MaxInterStageShaderComponents;

		public uint MaxInterStageShaderVariables;

		public uint MaxColorAttachments;

		public uint MaxColorAttachmentBytesPerSample;

		public uint MaxComputeWorkgroupStorageSize;

		public uint MaxComputeInvocationsPerWorkgroup;

		public uint MaxComputeWorkgroupSizeX;

		public uint MaxComputeWorkgroupSizeY;

		public uint MaxComputeWorkgroupSizeZ;

		private readonly uint _padding;
	
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuAdapterInfo {

		private unsafe fixed byte vendor[512];

		public readonly string Vendor {
			get {
				unsafe {
					fixed(byte* pVendor = vendor) {
						return MemoryUtil.GetUTF8((IntPtr)pVendor, 512)!;
					}
				}
			}
		}

		private unsafe fixed byte architecture[512];

		public readonly string Architecture {
			get {
				unsafe {
					fixed (byte* pArch = architecture) {
						return MemoryUtil.GetUTF8((IntPtr)pArch, 512)!;
					}
				}
			}
		}

		private unsafe fixed byte device[512];

		public readonly string Device {
			get {
				unsafe {
					fixed (byte* pDevice = device) {
						return MemoryUtil.GetUTF8((IntPtr)pDevice, 512)!;
					}
				}
			}
		}

		private unsafe fixed byte description[512];

		public readonly string Description {
			get {
				unsafe {
					fixed (byte* pDesc = description) {
						return MemoryUtil.GetUTF8((IntPtr)pDesc, 512)!;
					}
				}
			}
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuRequestAdapterOptions {

		public WGpuPowerPreference PowerPreference;

		public EMBOOL ForceFallbackAdapter;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuQueueDescriptor {

		[NativeType("const char*")]
		public IntPtr Label;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuDeviceDescriptor {

		public WGpuFeatures RequiredFeatures;

		private readonly uint _padding0;

		public WGpuSupportedLimits RequiredLimits;

		public WGpuQueueDescriptor DefaultQueue;

		private readonly uint _padding1;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuBufferDescriptor {

		public ulong Size;

		public WGpuBufferUsageFlags Usage;

		public EMBOOL MappedAtCreation;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuTextureDescriptor {

		public uint Width;

		public uint Height;

		public uint DepthOrArrayLayers;

		public uint MipLevelCount;

		public uint SampleCount;

		public WGpuTextureDimension Dimension;

		public WGpuTextureFormat Format;

		public WGpuTextureUsageFlags Usage;

		public int NumViewFormats;

		[NativeType("WGPU_TEXTURE_FORMAT*")]
		public IntPtr ViewFormats;

		public WGpuTextureDescriptor() {
			Width = default;
			Height = 1;
			DepthOrArrayLayers = 1;
			MipLevelCount = 1;
			SampleCount = 1;
			Dimension = WGpuTextureDimension.Dimension2D;
			Format = default;
			Usage = default;
			NumViewFormats = default;
			ViewFormats = default;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuTextureViewDescriptor {

		public WGpuTextureFormat Format;

		public WGpuTextureViewDimension Dimension;

		public WGpuTextureAspect Aspect;

		public uint BaseMipLevel;

		public uint MipLevelCount;

		public uint BaseArrayLayer;

		public uint ArrayLayerCount;

		public WGpuTextureViewDescriptor() {
			Format = default;
			Dimension = default;
			Aspect = WGpuTextureAspect.All;
			BaseMipLevel = 0;
			MipLevelCount = default;
			BaseArrayLayer = 0;
			ArrayLayerCount = default;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuExternalTextureDescriptor {

		public IntPtr Source;

		public HTMLPredefinedColorSpace ColorSpace;

		public WGpuExternalTextureDescriptor() {
			Source = default;
			ColorSpace = HTMLPredefinedColorSpace.SRGB;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuSamplerDescriptor {

		public WGpuAddressMode AddressModeU;

		public WGpuAddressMode AddressModeV;

		public WGpuAddressMode AddressModeW;

		public WGpuFilterMode MagFilter;

		public WGpuFilterMode MinFilter;

		public WGpuMipmapFilterMode MipmapFilter;

		public float LodMinClamp;

		public float LodMaxClamp;

		public WGpuCompareFunction Compare;

		public uint MaxAnisotropy;

		public WGpuSamplerDescriptor() {
			AddressModeU = WGpuAddressMode.ClampToEdge;
			AddressModeV = WGpuAddressMode.ClampToEdge;
			AddressModeW = WGpuAddressMode.ClampToEdge;
			MagFilter = WGpuFilterMode.Nearest;
			MinFilter = WGpuFilterMode.Nearest;
			MipmapFilter = WGpuMipmapFilterMode.Nearest;
			LodMinClamp = 0;
			LodMaxClamp = 32;
			Compare = WGpuCompareFunction.Invalid;
			MaxAnisotropy = 1;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuBufferBindingLayout {

		public WGpuBufferBindingType Type;

		public EMBOOL HasDynamicOffset;

		public ulong MinBindingSize;

		public WGpuBufferBindingLayout() {
			Type = WGpuBufferBindingType.Uniform;
			HasDynamicOffset = false;
			MinBindingSize = 0;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuSamplerBindingLayout {

		public WGpuSamplerBindingType Type;

		public WGpuSamplerBindingLayout() {
			Type = WGpuSamplerBindingType.Filtering;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuTextureBindingLayout {

		public WGpuTextureSampleType SampleType;

		public WGpuTextureViewDimension ViewDimension;

		public EMBOOL Multisampled;

		public WGpuTextureBindingLayout() {
			SampleType = WGpuTextureSampleType.Float;
			ViewDimension = WGpuTextureViewDimension.Dimension2D;
			Multisampled = default;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuStorageTextureBindingLayout {

		public WGpuStorageTextureAccess Access;

		public WGpuTextureFormat Format;

		public WGpuTextureViewDimension ViewDimension;

		public WGpuStorageTextureBindingLayout() {
			Access = WGpuStorageTextureAccess.WriteOnly;
			Format = default;
			ViewDimension = WGpuTextureViewDimension.Dimension2D;
		}

	}

	// typedef DummyStruct WGpuExternalTextureBindingLayout;

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuBindGroupEntry {

		public uint Binding;

		public IntPtr Resource;

		public ulong BufferBindOffset;

		public ulong BufferBindSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuShaderModuleCompilationHint {

		[NativeType("const char*")]
		public IntPtr EntryPointName;

		[NativeType("WGpuPipelineLayout")]
		public IntPtr Layout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuShaderModuleDescriptor {

		[NativeType("const char*")]
		public IntPtr Code;

		public int NumHints;

		[NativeType("const WGpuShaderModuleCompilationHint*")]
		public IntPtr Hints;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuCompilationMessage {

		[NativeType("char*")]
		public IntPtr Message;

		public WGpuCompilationMessageType Type;

		public uint LineNum;

		public uint LinePos;

		public uint Offset;

		public uint Length;

	}

	// Note: WGpuCompilationInfo is just an int message count followed in memory by the list of messages
	// This doesn't marshal well and its easier to just do it manually with raw memory accesses

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuPipelineConstant {

		[NativeType("const char*")]
		public IntPtr Name;

		private readonly uint _padding;

		public double Value;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuPrimitiveState {

		public WGpuPrimitiveTopology Topology;

		public WGpuIndexFormat StripIndexFormat;

		public WGpuFrontFace FrontFace;

		public WGpuCullMode CullMode;

		public EMBOOL UnclippedDepth;

		public WGpuPrimitiveState() {
			Topology = WGpuPrimitiveTopology.TriangleList;
			StripIndexFormat = WGpuIndexFormat.Invalid;
			FrontFace = WGpuFrontFace.ConterClockwise;
			CullMode = WGpuCullMode.None;
			UnclippedDepth = false;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuMultisampleState {

		public uint Count;

		public uint Mask;

		public EMBOOL AlphaToCoverageEnabled;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuFragmentState {

		[NativeType("WGpuShaderModule")]
		public IntPtr Module;

		[NativeType("const char*")]
		public IntPtr EntryPoint;

		public int NumTargets;

		[NativeType("const WGpuColorTargetState*")]
		public IntPtr Targets;

		public int NumConstants;

		[NativeType("const WGpuPipelineConstant*")]
		public IntPtr Constants;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuBlendComponent {

		public WGpuBlendOperation Operation;

		public WGpuBlendFactor SrcFactor;

		public WGpuBlendFactor DstFactor;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuStencilFaceState {

		public WGpuCompareFunction Compare;

		public WGpuStencilOperation FailOp;

		public WGpuStencilOperation DepthFailOp;

		public WGpuStencilOperation PassOp;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuVertexState {

		[NativeType("WGpuShaderModule")]
		public IntPtr Module;

		[NativeType("const char*")]
		public IntPtr EntryPoint;

		public int NumBuffers;

		[NativeType("const WGpuVertexBufferLayout*")]
		public IntPtr Buffers;

		public int NumConstants;

		[NativeType("const WGpuPipelineConstant*")]
		public IntPtr Constants;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuVertexBufferLayout {

		public int NumAttributes;

		[NativeType("const WGpuVertexAttribute*")]
		public IntPtr Attributes;

		public ulong ArrayStride;

		public WGpuVertexStepMode StepMode;

		private readonly uint _padding;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuVertexAttribute {

		public ulong Offset;

		public uint ShaderLocation;

		public WGpuVertexFormat Format;

	}

	// typedef DummyStruct WGpuCommandBufferDescriptor;

	// typedef DummyStruct WGpuCommandEncoderDescriptor;

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuImageCopyBuffer {

		public ulong Offset;

		public uint BytesPerRow;

		public uint RowsPerImage;

		[NativeType("WGpuBuffer")]
		public IntPtr Buffer;

		private readonly uint _padding;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuComputePassTimestampWrite {

		[NativeType("WGpuQuerySet")]
		public IntPtr QuerySet;

		public uint QueryIndex;

		public WGpuComputePassTimestampLocation Location;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuComputePassDescriptor {

		public uint NumTimestampWrites;

		[NativeType("WGpuComputePassTimestampWrite*")]
		public IntPtr TimestampWrites;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuRenderPassDepthStencilAttachment {

		[NativeType("WGpuTextureView")]
		public IntPtr View;

		public WGpuLoadOp DepthLoadOp;

		public float DepthClearValue;

		public WGpuStoreOp DepthStoreOp;

		public EMBOOL DepthReadOnly;

		public WGpuLoadOp StencilLoadOp;

		public uint StencilClearValue;

		public WGpuStoreOp StencilStoreOp;

		public EMBOOL StencilReadOnly;

	}

	// typedef DummyStruct WGpuRenderBundleDescriptor

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuRenderBundleEncoderDescriptor {

		public int NumColorFormats;

		[NativeType("const WGPU_TEXTURE_FORMAT*")]
		public IntPtr ColorFormats;

		public WGpuTextureFormat DepthStencilFormat;

		public uint SampleCount;

		public EMBOOL DepthReadOnly;

		public EMBOOL StencilReadOnly;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuQuerySetDescriptor {

		public WGpuQueryType Type;

		public uint Count;

	}

	// typedef Vector4d WGpuColor

	// typedef Vector2i WGpuOrigin2D

	// typedef Vector3i WGpuOrigin3D

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuCanvasConfiguration {

		[NativeType("WGpuDevice")]
		public IntPtr Device;

		public WGpuTextureFormat Format;

		public WGpuTextureUsageFlags Usage;

		public int NumViewFormats;

		[NativeType("WGPU_TEXTURE_FORMAT*")]
		public IntPtr ViewFormats;

		public HTMLPredefinedColorSpace ColorSpace;

		public WGpuCanvasAlphaMode AlphaMode;

		public WGpuCanvasConfiguration() {
			Device = default;
			Format = default;
			Usage = WGpuTextureUsageFlags.RenderAttachment;
			NumViewFormats = default;
			ViewFormats = default;
			ColorSpace = HTMLPredefinedColorSpace.SRGB;
			AlphaMode = WGpuCanvasAlphaMode.Opaque;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuRenderPassTimestampWrite {

		[NativeType("WGpuQuerySet")]
		public IntPtr QuerySet;

		public uint QueryIndex;

		public WGpuRenderPassTimestampLocation Location;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuRenderPassDescriptor {

		public int NumColorAttachments;

		[NativeType("const WGpuRenderPassColorAttachment*")]
		public IntPtr ColorAttachments;

		public WGpuRenderPassDepthStencilAttachment DepthStencilAttachment;

		[NativeType("WGpuQuerySet")]
		public IntPtr OcclusionQuerySet;

		public DoubleInt53 MaxDrawCount;

		public uint NumTimestampWrites;

		[NativeType("WGpuRenderPassTimestampWrite*")]
		public IntPtr TimestampWrites;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuRenderPassColorAttachment {

		[NativeType("WGpuTextureView")]
		public IntPtr View;

		[NativeType("WGpuTextureView")]
		public IntPtr ResolveTarget;

		public WGpuStoreOp StoreOp;

		public WGpuLoadOp LoadOp;

		// TODO
		// public Vector4d ClearValue;

		public WGpuRenderPassColorAttachment() {
			View = default;
			ResolveTarget = default;
			StoreOp = WGpuStoreOp.Store;
			LoadOp = WGpuLoadOp.Load;
			// ClearValue = new Vector4d(0.0, 0.0, 0.0, 1.0);
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuImageCopyExternalImage {

		[NativeType("WGpuObjectBase")]
		public IntPtr Source;

		public Vector2i Origin;

		public EMBOOL FlipY;

		public WGpuImageCopyExternalImage() {
			Source = default;
			Origin = new Vector2i(0, 0);
			FlipY = default;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuImageCopyTexture {

		public IntPtr Texture;

		public uint MipLevel;

		public Vector3i Origin;

		public WGpuTextureAspect Aspect;

		public WGpuImageCopyTexture() {
			Texture = default;
			MipLevel = 0;
			Origin = Vector3i.Zero;
			Aspect = WGpuTextureAspect.All;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuImageCopyTextureTagged {

		public WGpuImageCopyTexture ImageCopy;

		public HTMLPredefinedColorSpace ColorSpace;

		public EMBOOL PremultipliedAlpha;

		public WGpuImageCopyTextureTagged() {
			ImageCopy = new();
			ColorSpace = HTMLPredefinedColorSpace.SRGB;
			PremultipliedAlpha = false;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuDepthStencilState {

		public WGpuTextureFormat Format;

		public EMBOOL DepthWriteEnabled;

		public WGpuCompareFunction DepthCompare;

		public uint StencilReadMask;

		public uint StencilWriteMask;

		public int DepthBias;

		public float DepthBiasSlopeScale;

		public float DepthBiasClamp;

		public WGpuStencilFaceState StencilFront;

		public WGpuStencilFaceState StencilBack;

		public EMBOOL ClampDepth;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuBlendState {

		public WGpuBlendComponent Color;

		public WGpuBlendComponent Alpha;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuColorTargetState {

		public WGpuTextureFormat Format;

		public WGpuBlendState Blend;

		public WGpuColorWriteFlags WriteMask;

		public WGpuColorTargetState() {
			Format = default;
			Blend = new WGpuBlendState() {
				Color = new WGpuBlendComponent() {
					Operation = WGpuBlendOperation.Disabled,
					SrcFactor = WGpuBlendFactor.Src,
					DstFactor = WGpuBlendFactor.OneMinusSrc
				},
				Alpha = new WGpuBlendComponent() {
					Operation = WGpuBlendOperation.Add,
					SrcFactor = WGpuBlendFactor.One,
					DstFactor = WGpuBlendFactor.Zero
				}
			};
			WriteMask = WGpuColorWriteFlags.All;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuRenderPipelineDescriptor {

		public WGpuVertexState Vertex;

		public WGpuPrimitiveState Primitive;

		public WGpuDepthStencilState DepthStencil;

		public WGpuMultisampleState Multisample;

		public WGpuFragmentState Fragment;

		[NativeType("WGpuPipelineLayout")]
		public IntPtr Layout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WGpuBindGroupLayoutEntry {

		public uint Binding;

		public WGpuShaderStageFlags Visibility;

		public WGpuBindGroupLayoutType Type;

		private readonly uint _padding;

		[StructLayout(LayoutKind.Explicit)]
		public struct EntryLayout {

			[FieldOffset(0)]
			public WGpuBufferBindingLayout Buffer;

			[FieldOffset(0)]
			public WGpuSamplerBindingLayout Sampler;

			[FieldOffset(0)]
			public WGpuTextureBindingLayout Texture;

			[FieldOffset(0)]
			public WGpuStorageTextureBindingLayout StorageTexture;

		}

		public EntryLayout Layout;

	}

}

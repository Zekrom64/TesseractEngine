using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.DirectX.GI {

	// dxgicommon.h

	public enum DXGIColorSpaceType {
		RGBFullG22NoneP709 = 0,
		RGBFullG10NoneP709,
		RGBStudioG22NoneP709,
		RGBStudioG22NoneP2020,
		Reserved,
		YCbCrFullG22NoneP709_X601,
		YCbCrStudioG22LeftP601,
		YCbCrFullG22LeftP601,
		YCbCrStudioG22LeftP709,
		YCbCrFullG22LeftP709,
		YCbCrStudioG22LeftP2020,
		YCbCrFullG22LeftP2020,
		RGBFullG2084NoneP2020,
		YCbCrStudioG2084LeftP2020,
		RGBStudioG2084NoneP2020,
		YCbCrStudioG22TopLeftP2020,
		YCbCrStudioG2084TopLeftP2020,
		RGBFullG22NoneP2020,
		YCbCrStudioGHLGTopLeftP2020,
		YCbCrFullGHLGTopLeftP2020,
		RGBStudioG24NoneP709,
		RGBStudioG24NoneP2020,
		YCbCrStudioG24LeftP709,
		YCbCrStudioG24LeftP2020,
		YCbCrStudioG24TopLeftP2020,
		Custom = unchecked((int)0xFFFFFFFF)
	}

	// dxgiformat.h

	public enum DXGIFormat : uint {
		Unknown,

		R32G32B32A32Typeless,
		R32G32B32A32Float,
		R32G32B32A32UInt,
		R32G32B32A32SInt,

		R32G32B32Typeless,
		R32G32B32Float,
		R32G32B32UInt,
		R32G32B32SInt,

		R16G16B16A16Typeless,
		R16G16B16A16Float,
		R16G16B16A16UNorm,
		R16G16B16A16UInt,
		R16G16B16A16SNorm,
		R16G16B16A16SInt,

		R32G32Typeless,
		R32G32Float,
		R32G32UInt,
		R32G32SInt,

		R32G8X24Typeless,
		D32FloatS8X24UInt,
		R32FloatX8X24Typeless,
		X32TypelessG8X24UInt,

		R10G10B10A2Typeless,
		R10G10B10A2UNorm,
		R10G10B10A2UInt,
		R11G11B10Float,

		R8G8B8A8Typeless,
		R8G8B8A8UNorm,
		R8G8B8A8UNormSRGB,
		R8G8B8A8UInt,
		R8G8B8A8SNorm,
		R8G8B8A8SInt,

		R16G16Typeless,
		R16G16Float,
		R16G16UNorm,
		R16G16UInt,
		R16G16SNorm,
		R16G16SInt,

		R32Typeless,
		D32Float,
		R32Float,
		R32UInt,
		R32SInt,

		R24G8Typeless,
		D24UNormS8UInt,
		R24UNormX8Typeless,
		X24TypelessG8UInt,

		R8G8Typeless,
		R8G8UNorm,
		R8G8UInt,
		R8G8SNorm,
		R8G8SInt,

		R16Typeless,
		R16Float,
		D16Float,
		R16UNorm,
		R16UInt,
		R16SNorm,
		R16SInt,

		R8Typeless,
		R8UNorm,
		R8UInt,
		R8SNorm,
		R8SInt,

		A8UNorm,
		R1UNorm,
		R9G9B9E5SharedExp,
		R8G8_B8G8UNorm,
		G8R8_G8B8UNorm,

		BC1Typeless,
		BC1UNorm,
		BC1UnormSRGB,
		BC2Typeless,
		BC2UNorm,
		BC2UnormSRGB,
		BC3Typeless,
		BC3UNorm,
		BC3UnormSRGB,
		BC4Typeless,
		BC4UNorm,
		BC4UnormSRGB,
		BC5Typeless,
		BC5UNorm,
		BC5UnormSRGB,

		B5G6R5UNorm,
		B5G5R5A1UNorm,
		B8G8R8A8UNorm,
		B8G8R8X8UNorm,
		R10G10B10XRBiasA2UNorm,
		B8G8R8A8Typeless,
		B8G8R8A8UNormSRGB,
		B8G8R8X8Typeless,
		B8G8R8X8UNormSRGB,

		BC6HTypeless,
		BC6HUFloat16,
		BC6HSFloat16,
		BC7Typeless,
		BC7UNorm,
		BC7UNormSRGB,

		AYUV,
		Y410,
		Y416,
		NV12,
		P010,
		P016,
		_420Opaque,
		YUV2,
		Y210,
		Y216,
		NV11,
		AI44,
		IA44,

		P8,
		A8P8,
		B4G4R4A4UNorm,

		P208 = 130,
		V208,
		V408,

		SamplerFeedbackMinMipOpaque = 189,
		SamplerFeedbackMipRegionUsedOpaque
	}

	// dxgitype.h

	public enum DXGIModeRotation {
		Unspecified,
		Identity,
		Rotate90,
		Rotate180,
		Rotate270
	}

	public enum DXGIModeScanlineOrder {
		Unspecified,
		Progressive,
		UpperFieldFirst,
		LowerFieldFirst
	}

	public enum DXGIModeScaling {
		Unspecified,
		Centered,
		Streched
	}

	// dxgi.h

	[Flags]
	public enum DXGIUsageFlags {
		CPUAccessNone = 0,
		CPUAccessDynamic = 1,
		CPUAccessReadWrite = 2,
		CPUAccessScratch = 3,
		CPUAccessField = 15,
		ShaderInput = 0x10,
		RenderTargetOutput = 0x20,
		BackBuffer = 0x40,
		Shared = 0x80,
		ReadOnly = 0x100,
		DiscardOnPresent = 0x200,
		UnorderedAccess = 0x400
	}

	[Flags]
	public enum DXGIEnumModesFlags {
		Interlaced = 1,
		Scaling = 2,
		Stereo = 0x4,
		DisabledStereo = 0x8
	}

	public enum DXGIResourcePriority : uint {
		Minimum = 0x28000000,
		Low = 0x50000000,
		Normal = 0x78000000,
		High = 0xA0000000,
		Maximum = 0xC8000000
	}

	[Flags]
	public enum DXGIMapFlags {
		Read = 0x1,
		Write = 0x2,
		Discard = 0x4
	}
	
	public enum DXGISwapEffect {
		Discard,
		Sequential,
		FlipSequential,
		FlipDiscard
	}

	public enum DXGIResidency {
		FullyResident = 1,
		ResidentInSharedMemory,
		EvictedToDisk
	}

	[Flags]
	public enum DXGISwapchainFlags {
		NonPrerotated = 0x1,
		AllowModeSwitch = 0x2,
		GDICompatible = 0x4,
		RestrictedContent = 0x8,
		RestrictSharedResourceDriver = 0x10,
		DisplayOnly = 0x20,
		FrameLatencyWaitableObject = 0x40,
		ForegroundLayer = 0x80,
		FullscreenVideo = 0x100,
		YUVVideo = 0x200,
		HWProtected = 0x400,
		AllowTearing = 0x800,
		RestrictedToAllHolographicDisplays = 0x1000
	}

	[Flags]
	public enum DXGIPresentFlags {
		Test = 0x1,
		DoNotSequence = 0x2,
		Restart = 0x4,
		DoNotWait = 0x8,
		StereoPreferRight =	0x10,
		StereoTemporaryMono = 0x20,
		RestrictToOutput = 0x40,
		UseDuration = 0x100,
		AllowTearing = 0x200
	}

	[Flags]
	public enum DXGIMakeWindowAssociationFlags {
		NoWindowChanges = 0x1,
		NoAltEnter = 0x2,
		NoPrintScreen = 0x4
	}

	[Flags]
	public enum DXGIAdapterFlags {
		None = 0,
		Remote = 1,
		Software = 2
	}
	
	// dxgi1_2.h

	public enum DXGIOutDuplPointerShapeType {
		Monochrome = 0x1,
		Color = 0x2,
		MaskedColor = 0x4
	}

	public enum DXGIAlphaMode {
		Unspecified,
		Premultiplied,
		Straight,
		Ignore
	}

	[Flags]
	public enum DXGISharedHandleAccessFlags : uint {
		GenericRead = 0x80000000,
		GenericWrite = 0x40000000,
		GenericExecute = 0x20000000,
		GenericAll = 0x10000000,
		SharedResourceRead = GenericRead,
		SharedResourceWrite = 1
	}

	public enum DXGIOfferResourcePriority {
		Low = 1,
		Normal,
		High
	}

	public enum DXGIScaling {
		Strech = 0,
		None = 1,
		AspectRatioStrech = 2
	}

	public enum DXGIGraphicsPreemptionGranularity {
		DmaBufferBoundary = 0,
		PrimitiveBoundary,
		TriangleBoundary,
		PixelBoundary,
		InstructionBoundary
	}

	public enum DXGIComputePreemptionGranularity {
		DmaBufferBoundary = 0,
		DispatchBoundary,
		ThreadGroupBoundary,
		ThreadBoundary,
		InstructionBoundary
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.DirectX.GI {

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

		P208 = 0x82,
		V208,
		V408,

		SamplerFeedbackMinMipOpaque = 189,
		SamplerFeedbackMipRegionUsedOpaque
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.DirectX.GI {

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

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIRational {
		public uint Numerator;
		public uint Denominator;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGISampleDesc {
		public uint Count;
		public uint Quality;
	}

	public static partial class DXGI {

		public const uint StandardMultisampleQuality = 0xFFFFFFFF;
		public const uint CenterMultisampleQuality = 0xFFFFFFFE;

	}

}

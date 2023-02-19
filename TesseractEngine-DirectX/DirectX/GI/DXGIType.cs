using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;
using Tesseract.Core.Utilities;

namespace Tesseract.DirectX.GI {

	using DXGIRGBA = Vector4;
	using DXGIRGB = Vector3;

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

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIModeDesc {
		public uint Width;
		public uint Height;
		public DXGIRational RefreshRate;
		public DXGIFormat Format;
		public DXGIModeScanlineOrder ScanlineOrdering;
		public DXGIModeScaling Scaling;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIGammaControlCapabilities {
		public bool ScaleAndOffsetSupported;
		public float MaxConvertedValue;
		public float MinConvertedValue;
		public uint NumGammaControlPoints;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025)]
		public float[] ControlPointPositions;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIGammaControl {
		public DXGIRGB Scale;
		public DXGIRGB Offset;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025)]
		public DXGIRGB[] GammaCurve;
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Numerics;
using Tesseract.Core.Native;
using Tesseract.Windows;

namespace Tesseract.DirectX.GI {

	// dxgicommon.h

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

	// dxgitype.h

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
	public struct DXGIRGB {
		public float R;
		public float G;
		public float B;

		public static implicit operator DXGIRGB(Vector3 v) => new() { R = v.X, G = v.Y, B = v.Z };
		public static implicit operator Vector3(DXGIRGB c) => new() { X = c.R, Y = c.G, Z = c.B };
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIRGBA {
		public float R;
		public float G;
		public float B;
		public float A;

		public static implicit operator DXGIRGBA(Vector4 v) => new() { R = v.X, G = v.Y, B = v.Z, A = v.W };
		public static implicit operator Vector4(DXGIRGBA c) => new() { X = c.R, Y = c.G, Z = c.B, W = c.A };
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIGammaControl {
		public DXGIRGB Scale;
		public DXGIRGB Offset;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025)]
		public DXGIRGB[] GammaCurve;
	}

	// dxgi.h

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGISurfaceDesc {
		public uint Width;
		public uint Height;
		public DXGIFormat Format;
		public DXGISampleDesc SampleDesc;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIMappedRect {
		public int Pitch;
		[NativeType("BYTE*")]
		public IntPtr Bits;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DXGIOutputDesc {
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public char[] DeviceName;
		public RECT DesktopCoordinates;
		public bool AttachedToDesktop;
		public DXGIModeRotation Rotation;
		[NativeType("HMONITOR")]
		public IntPtr Monitor;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIFrameStatistics {
		public uint PresentCount;
		public uint PresentRefreshCount;
		public uint SyncRefreshCount;
		public LARGE_INTEGER SyncQPCTime;
		public LARGE_INTEGER SyncGPUTime;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DXGIAdapterDesc {
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] Description;
		public uint VendorID;
		public uint DeviceID;
		public uint SubSysID;
		public uint Revision;
		public nuint DedicatedVideoMemory;
		public nuint DedicatedSystemMemory;
		public nuint SharedSystemMemory;
		public LUID AdapterLuid;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGISwapChainDesc {
		public DXGIModeDesc BufferDesc;
		public DXGISampleDesc SampleDesc;
		public DXGIUsageFlags BufferUsage;
		public uint BufferCount;
		[NativeType("HWND")]
		public IntPtr OutputWindow;
		public bool Windowed;
		public DXGISwapEffect SwapEffect;
		public DXGISwapchainFlags Flags;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGISharedResource {
		[NativeType("HANDLE")]
		public IntPtr Handle;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIAdapterDesc1 {
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] Description;
		public uint VendorId;
		public uint DeviceID;
		public uint SubSysID;
		public uint Revision;
		public nuint DedicatedVideoMemory;
		public nuint DedicatedSystemMemory;
		public nuint SharedSystemMemory;
		public LUID AdapterLuid;
		public DXGIAdapterFlags Flags;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIDisplayColorSpace {
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public Vector2[] PrimaryCoordinates;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public Vector2[] WhitePoints;
	}

	// dxgi1_2.h

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIOutDuplMoveRect {

		public POINT SourcePoint;

		public RECT DestinationRect;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIOutDuplDesc {

		public DXGIModeDesc ModeDesc;
		public DXGIModeRotation Rotation;
		public bool DesktopImageInSystemMemory;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIOutDuplPointerPosition {
		public POINT Position;
		public bool Visible;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIOutDuplPointerShapeInfo {
		public DXGIOutDuplPointerShapeType Type;
		public uint Width;
		public uint Height;
		public uint Pitch;
		public POINT HotSpot;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIOutDuplFrameInfo {
		public LARGE_INTEGER LastPresentTime;
		public LARGE_INTEGER LastMouseUpdateTime;
		public uint AccumulatedFrames;
		public bool RectsCoalesced;
		public bool ProtectedContentMaskedOut;
		public DXGIOutDuplPointerPosition PointerPosition;
		public uint TotalMetadataBufferSize;
		public uint PointerShapedBufferSize;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIModeDesc1 {
		public uint Width;
		public uint Height;
		public DXGIRational RefreshRate;
		public DXGIFormat Format;
		public DXGIModeScanlineOrder ScanlineOrdering;
		public DXGIModeScaling Scaling;
		public bool Stereo;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGISwapChainDesc1 {
		public uint Width;
		public uint Height;
		public DXGIFormat Format;
		public bool Stereo;
		public DXGISampleDesc SampleDesc;
		public DXGIUsageFlags BufferUsage;
		public uint BufferCount;
		public DXGIScaling Scaling;
		public DXGISwapEffect SwapEffect;
		public DXGIAlphaMode AlphaMode;
		public DXGISwapchainFlags Flags;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGISwapChainFullscreenDesc {
		public DXGIRational RefreshRate;
		public DXGIModeScanlineOrder ScanlineOrdering;
		public DXGIModeScaling Scaling;
		public bool Windowed;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIPresentParameters {
		public uint DirtyRectsCount;
		[NativeType("RECT*")]
		public IntPtr DirtyRects;
		[NativeType("POINT*")]
		public IntPtr ScrollOffset;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIAdapterDesc2 {
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] Description;
		public uint VendorId;
		public uint DeviceId;
		public uint SubSysId;
		public uint Revision;
		public nuint DedicatedVideoMemory;
		public nuint DedicatedSystemMemory;
		public nuint SharedSystemMemory;
		public LUID AdapterLuid;
		public DXGIAdapterFlags Flags;
		public DXGIGraphicsPreemptionGranularity GraphicsPreemptionGranularity;
		public DXGIComputePreemptionGranularity ComputePreemptionGranularity;
	}

}

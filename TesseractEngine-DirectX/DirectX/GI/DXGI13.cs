using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Windows;

namespace Tesseract.DirectX.GI {

	using DXGIMatrix3x2F = System.Numerics.Matrix3x2;

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIDecodeSwapChainDesc {

		public uint Flags;

	}

	[Flags]
	public enum DXGIMultiplaneOverlayYCbCrFlags {
		NominalRange = 1,
		BT709 = 2,
		XVYCC = 0x4
	}

	public enum DXGIFramePresentationMode {
		Composed = 0,
		Overlay,
		None,
		CompositionFailure
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIFrameStatisticsMedia {

		public uint PresentCount;
		public uint PresentRefereshCount;
		public uint SyncRefreshCount;
		public LARGE_INTEGER SyncQPCTime;
		public LARGE_INTEGER SyncGPUTime;
		public DXGIFramePresentationMode CompositionMode;
		public uint ApprovedPresentDuration;

	}

	[Flags]
	public enum DXGIOverlaySupportFlag {
		Direct = 1,
		Scaling = 2
	}

	[ComImport, Guid("6007896c-3244-4afd-bf18-a6d3beda5023")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIDevice3 : IDXGIDevice2 {

		[PreserveSig]
		public void Trim();

	}

	[ComImport, Guid("a8be2ac4-199f-4946-b331-79599fb98de7")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGISwapChain2 : IDXGISwapChain1 {

		public void SetSourceSize(uint width, uint height);

		public void GetSourceSize(out uint width, out uint height);

		public void SetMaximumFrameLatency(uint maxLatency);

		public void GetMaximumFrameLatency(out uint maxLatency);

		[PreserveSig]
		public IntPtr GetFrameLatencyWaitableObject();

		public void SetMatrixTransform(in DXGIMatrix3x2F matrix);

		public void GetMatrixTransform(out DXGIMatrix3x2F matrix);

	}

	[ComImport, Guid("595e39d1-2724-4663-99b1-da969de28364")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIOutput2 : IDXGIOutput1 {

		[PreserveSig]
		public bool SupportsOverlays();

	}

	[ComImport, Guid("25483823-cd46-4c7d-86ca-47aa95b837bd")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIFactory3 : IDXGIFactory2 {

		[PreserveSig]
		public DXGICreateFactoryFlags GetCreationFlags();

	}

	[ComImport, Guid("2633066b-4514-4c7a-8fd8-12ea98059d18")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIDecodeSwapChain : IUnknown {

		public void PresentBuffer(uint bufferToRepeat, uint syncInterval, DXGIPresentFlags flags);

		public void SetSourceRect(in RECT rect);

		public void SetTargetRect(in RECT rect);

		public void SetDestSize(uint width, uint height);

		public void GetSourceRect(out RECT rect);

		public void GetTargetRect(out RECT rect);

		public void GetDestSize(out uint width, out uint height);

		public void SetColorSpace(DXGIMultiplaneOverlayYCbCrFlags colorspace);

		[PreserveSig]
		public DXGIMultiplaneOverlayYCbCrFlags GetColorSpace();

	}

	[ComImport, Guid("41e7d1f2-a591-4f7b-a2e5-fa9c843e1c12")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIFactoryMedia : IUnknown {

		[return: MarshalAs(UnmanagedType.Interface)]
		public void CreateSwapChainForCompositionSurfaceHandle(
			[MarshalAs(UnmanagedType.IUnknown)] object device,
			[NativeType("HANDLE")] IntPtr surface,
			in DXGISwapChainDesc1 desc,
			[MarshalAs(UnmanagedType.Interface)] IDXGIOutput? restrictToOutput,
			[MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 swapchain
		);

		[return: MarshalAs(UnmanagedType.Interface)]
		public void CreateDecodeSwapChainForCompositionSurfaceHandle(
			[MarshalAs(UnmanagedType.IUnknown)] object device,
			[NativeType("HANDLE")] IntPtr surface,
			in DXGIDecodeSwapChainDesc desc,
			[MarshalAs(UnmanagedType.Interface)] IDXGIResource yuvDecodeBuffers,
			[MarshalAs(UnmanagedType.Interface)] IDXGIOutput? restrictToOutput,
			[MarshalAs(UnmanagedType.Interface)] out IDXGIDecodeSwapChain swapchain
		);

	}

	[ComImport, Guid("dd95b90b-f05f-4f6a-bd65-25bfb264bd84")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGISwapChainMedia : IUnknown {

		public void GetFrameStatisticsMedia(out DXGIFrameStatisticsMedia stats);

		public void SetPresentDuration(uint duration);

		public void CheckPresentDurationSupport(uint desiredPresentDuration, out uint closestSmallerPresentDuration, out uint closestLargerPresentDuration);

	}

	[ComImport, Guid("8a6bb301-7e7e-41F4-a8e0-5b32f7f99b18")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIOutput3 : IDXGIOutput2 {

		public void CheckOverlaySupport(DXGIFormat enumFormat, [MarshalAs(UnmanagedType.IUnknown)] out IUnknown concernedDevice, out DXGIOverlaySupportFlag flags);

	}

	[Flags]
	public enum DXGICreateFactoryFlags : uint {
		Debug = 1
	}

	public static class DXGI13 {

		[DllImport("dxgi.dll", PreserveSig = false)]
		private static extern IntPtr CreateDXGIFactory2(DXGICreateFactoryFlags flags, in Guid iid);

		public static T CreateDXGIFactory2<T>(DXGICreateFactoryFlags flags) where T : class => COMHelpers.GetObjectForCOMPointer<T>(CreateDXGIFactory2(flags, COMHelpers.GetCOMID<T>()))!;

		[DllImport("dxgi.dll", PreserveSig = false)]
		private static extern IntPtr DXGIGetDebugInterface1(uint flags, in Guid riid);

		public static T GetDebugInterface1<T>() where T : class => COMHelpers.GetObjectForCOMPointer<T>(DXGIGetDebugInterface1(0, COMHelpers.GetCOMID<T>()))!;

	}

}

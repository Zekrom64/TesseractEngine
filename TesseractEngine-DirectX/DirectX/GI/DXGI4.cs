using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Windows;

namespace Tesseract.DirectX.GI {

	[Flags]
	public enum DXGISwapChainColorSpaceSupportFlag {
		Present = 1,
		OverlayPresent = 2
	}

	[Flags]
	public enum DXGIOverlayColorSpaceSupportFlag {
		Present = 1
	}

	public enum DXGIMemorySegmentGroup {
		Local = 0,
		NonLocal = 1
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIQueryVideoMemoryInfo {

		public ulong Budget;
		public ulong CurrentUsage;
		public ulong AvailableForReservation;
		public ulong CurrentReservation;

	}

	[ComImport, Guid("94d99bdb-f1f8-4ab0-b236-7da0170edab1")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGISwapChain3 : IDXGISwapChain2 {

		[PreserveSig]
		public uint GetCurrentBackBufferIndex();

		public void CheckColorSpaceSupport(DXGIColorSpaceType colorSpace, out DXGISwapChainColorSpaceSupportFlag flags);

		public void SetColorSpace1(DXGIColorSpaceType colorSpace);

		public void ResizeBuffers1(uint bufferCount, uint width, uint height, DXGIFormat format, DXGISwapChainFlags flags, [NativeType("const UINT*")] IntPtr pNodeMask, [NativeType("IUnknown* const*")] IntPtr pPresentQueue);

	}

	[ComImport, Guid("dc7dca35-2196-414d-9F53-617884032a60")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIOutput4 : IDXGIOutput3 {

		public void CheckOverlayColorSpaceSupport(DXGIFormat format, DXGIColorSpaceType colorSpace, [MarshalAs(UnmanagedType.Interface)] object device, out DXGIOverlayColorSpaceSupportFlag flags);

	}

	[ComImport, Guid("1bc6ea02-ef36-464f-bf0c-21ca39e5168a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIFactory4 : IDXGIFactory3 {

		public void EnumAdapterByLuid(LUID luid, Guid iid, out IntPtr adapter);

		public void EnumWarpAdapter(Guid iid, out IntPtr adapter);

	}

	[ComImport, Guid("645967a4-1392-4310-a798-8053ce3e93fd")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIAdapter3 : IDXGIAdapter2 {

		public void RegisterHardwareContentProtectionTeardownStatusEvent([NativeType("HANDLE")] IntPtr _event, out uint cookie);

		[PreserveSig]
		public void UnregisterHardwareContentProtectionTeardownStatus(uint cookie);

		public void QueryVideoMemoryInfo(uint nodeIndex, DXGIMemorySegmentGroup segmentGroup, out DXGIQueryVideoMemoryInfo memoryInfo);

		public void SetVideoMemoryReservation(uint nodeIndex, DXGIMemorySegmentGroup segmentGroup, ulong reservation);

		public void RegisterVideoMemoryBudgetChangeNotificationEvent([NativeType("HANDLE")] IntPtr _event, out uint cookie);

		[PreserveSig]
		public void UnregisterVideoMemoryBudgetChangeNotification(uint cookie);

	}

}

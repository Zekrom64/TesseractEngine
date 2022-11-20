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
	public enum DXGIAdapterFlag3 : uint {
		None = 0,
		Remote = 0x1,
		Software = 0x2,
		ACGCompatible = 0x4,
		SupportMonitoredFences = 0x8,
		SupportNonMonitoredFences = 0x10,
		KeyedMutexConformance = 0x20
	}

	[Flags]
	public enum DXGIHardwareCompositionSupportFlags {
		Fullscreen = 0x1,
		Windowed = 0x2,
		CursorStreched = 0x4
	}

	[Flags]
	public enum DXGI_GPUPreference {
		Unspecified = 0,
		MinimumPower = 0x1,
		HighPerformance = 0x2
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIAdapterDesc3 {

		private unsafe fixed char description[128];
		public string Description {
			get {
				unsafe {
					fixed(char* pDescription = description) {
						return MemoryUtil.GetUTF16(new ReadOnlySpan<char>(pDescription, 128));
					}
				}
			}
		}

		public uint VendorId;
		public uint DeviceId;
		public uint SubSysId;
		public uint Revision;
		public nuint DedicatedVideoMemory;
		public nuint DedicatedSystemMemory;
		public nuint SharedSystemMemory;
		public LUID AdapterLuid;
		public DXGIAdapterFlag3 Flags;
		public DXGIGraphicsPreemptionGranularity GraphicsPreemptionGranularity;
		public DXGIComputePreemptionGranularity ComputePreemptionGranularity;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGIOutputDesc1 {

		private unsafe fixed char deviceName[32];
		public string DeviceName {
			get {
				unsafe {
					fixed(char* pDeviceName = deviceName) {
						return MemoryUtil.GetUTF16(new ReadOnlySpan<char>(pDeviceName, 32));
					}
				}
			}
		}

		public RECT DesktopCoordinates;
		public bool AttachedToDesktop;
		public DXGIModeRotation Rotation;
		[NativeType("HMONITOR")]
		public IntPtr Monitor;
		public uint BitsPerColor;
		public DXGIColorSpaceType ColorSpace;
		public float RedPrimary0;
		public float RedPrimary1;
		public float GreenPrimary0;
		public float GreenPrimary1;
		public float BluePrimary0;
		public float BluePrimary1;
		public float WhitePoint0;
		public float WhitePoint1;
		public float MinLuminance;
		public float MaxLuminance;
		public float MaxFullFrameLuminance;

	}

	[ComImport, Guid("3c8d99d1-4fbf-4181-a82c-af66bf7bd24e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIAdapter4 : IDXGIAdapter3 {

		public void GetDesc3(out DXGIAdapterDesc3 desc);

	}

	[ComImport, Guid("068346e8-aaec-4b84-add7-137f513f77a1")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIOutput6 : IDXGIOutput5 {

		public void GetDesc1(out DXGIOutputDesc1 desc);

		public void CheckHardwareCompositionSupport(out DXGIHardwareCompositionSupportFlags flags);

	}

	[ComImport, Guid("c1b6694f-ff09-44a9-b03c-77900a0a1d17")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIFactory6 : IDXGIFactory5 {

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U4)]
		public HRESULT EnumAdapterByGpuPreference(uint adapterIdx, DXGI_GPUPreference gpuPreference, in Guid iid, out IntPtr adapter);

	}

	[ComImport, Guid("a4966eed-76db-44da-84c1-ee9a7afb20a8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIFactory7 : IDXGIFactory6 {

		public void RegisterAdaptersChangedEvent([NativeType("HANDLE")] IntPtr _event, out uint cookie);

		public void UnregisterAdaptersChangedEvent(uint cookie);

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Windows;

namespace Tesseract.DirectX.GI {

	[ComImport, Guid("aec22fb8-76f3-4639-9be0-28eb43a67a2e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIObject : IUnknown {

		public void SetPrivateData(in Guid guid, uint dataSize, IntPtr data);

		public void SetPrivateDataInterface(in Guid guid, [MarshalAs(UnmanagedType.IUnknown)] object _object);

		public void GetPrivateData(in Guid guid, out uint dataSize, IntPtr data);

		public IntPtr GetParent(in Guid riid);

	}

	[ComImport, Guid("3d3e0379-f9de-4d58-bb6c-18d62992f1a6")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIDeviceSubObject : IDXGIObject {

		public IntPtr GetDevice(in Guid riid);

	}

	[ComImport, Guid("035f3ab4-482e-4e50-b41f-8a7f8bd8960b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIResource : IDXGIDeviceSubObject {

		[return: NativeType("HANDLE")]
		public IntPtr GetSharedHandle();

		public DXGIUsageFlags GetUsage();

		public void SetEvictionPriority(uint evictionPriority);

		public uint GetEvictionPriority();

	}

	[ComImport, Guid("9d8e1289-d7b3-465f-8126-250e349af85d")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIKeyedMutex : IDXGIDeviceSubObject {

		public void AcquireSync(ulong key, uint dwMilliseconds);

		public void ReleaseSync(ulong key);
			
	}

	[ComImport, Guid("cafcb56c-6ac3-4889-bf47-9e23bbd260ec")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGISurface : IDXGIDeviceSubObject {

		public DXGISurfaceDesc GetDesc();

		public void Map(out DXGIMappedRect mappedRect, DXGIMapFlags flags);

		public void Unmap();

	}

	[ComImport, Guid("4ae63092-6327-4c1b-80ae-bfe12ea32b86")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGISurface1 : IDXGISurface {

		[return: NativeType("HDC")]
		public IntPtr GetDC(bool discard);

		public void ReleaseDC([NativeType("RECT*")] IntPtr dirtyRect);

	}

	[ComImport, Guid("ae02eedb-c735-4690-8d52-5a8dc20213aa")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIOutput : IDXGIObject {

		public DXGIOutputDesc GetDesc();

		public void GetDisplayModeList(DXGIFormat format, DXGIEnumModesFlags flags, ref uint modeCount, [NativeType("DXGI_MODE_DESC*")] IntPtr desc);

		/*
		public void GetDisplayModeList(DXGIFormat format, DXGIEnumModesFlags flags, out DXGIModeDesc[] desc) {
			unsafe {
				uint modeCount = 0;
				GetDisplayModeList(format, flags, ref modeCount, IntPtr.Zero);
				desc = new DXGIModeDesc[modeCount];
				fixed (DXGIModeDesc* descPtr = desc) {
					GetDisplayModeList(format, flags, ref modeCount, (IntPtr)descPtr);
				}
			}
		}
		*/

		public void FindClosestMatchingMode(in DXGIModeDesc mode, out DXGIModeDesc closestMatch, [MarshalAs(UnmanagedType.IUnknown)] object device);

		public void WaitForVBlank();

		public void TakeOwnership([MarshalAs(UnmanagedType.IUnknown)] object device, bool exclusive);

		public void ReleaseOwnership();

		public DXGIGammaControlCapabilities GetGammaControlCapabilities();

		public void SetGammaControl(in DXGIGammaControl gammaControl);

		public DXGIGammaControl GetGammaControl();

		public void SetDisplaySurface(IDXGISurface surface);

		public void GetDisplaySurfaceData(IDXGISurface surface);

		public DXGIFrameStatistics GetFrameStatistics();

	}

	[ComImport, Guid("2411e7e1-12ac-4ccf-bd14-9798e8534dc0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIAdapter : IDXGIObject {

		[return: MarshalAs(UnmanagedType.U4)]
		[PreserveSig]
		public HRESULT EnumOutputs(uint outputIdx, out IDXGIOutput ppOutput);

		/*
		public List<IDXGIOutput> EnumOutputs() {
			List<IDXGIOutput> outputs = new();
			uint index = 0;
			while(EnumOutputs(index++, out IDXGIOutput output).Succeeded) outputs.Add(output);
			return outputs;
		}
		*/

		public DXGIAdapterDesc GetDesc();

		[return: MarshalAs(UnmanagedType.U4)]
		[PreserveSig]
		public HRESULT CheckInterfaceSupport(in Guid riid, out LARGE_INTEGER umdVersion);

		//public bool CheckInterfaceSupport<T>(out LARGE_INTEGER umdVersion) => CheckInterfaceSupport(typeof(T).GUID, out umdVersion).Succeeded;

	}

	[ComImport, Guid("310d36a0-d2e7-4c0a-aa04-6a9d23b8886a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGISwapChain : IDXGIDeviceSubObject {

		public void Present(uint syncInterval, DXGIPresentFlags flags);

		public void GetBuffer(uint bufferIdx, in Guid riid, out IntPtr ppSurface);

		/*
		public T GetBuffer<T>(uint bufferIdx) {
			GetBuffer(bufferIdx, typeof(T).GUID, out IntPtr ppSurface);
			return COMHelpers.GetObjectForCOMPointer<T>(ppSurface);
		}
		*/

		public void SetFullscreenState(bool fullscreen, IDXGIOutput target);

		public void GetFullscreenState(out bool fullscreen, out IDXGIOutput target);

		public DXGISwapChainDesc GetDesc();

		public void ResizeBuffers(uint bufferCount, uint width, uint height, DXGIFormat format, DXGISwapchainFlags flags);

		public void ResizeTarget(in DXGIModeDesc targetModeDesc);

		[return: MarshalAs(UnmanagedType.Interface)]
		public IDXGIOutput GetContainingOutput();

		public void GetFrameStatistics(out DXGIFrameStatistics stats);

		public void GetLastPresentCount(out uint lastPresentCount);

	}

	[ComImport, Guid("7b7166ec-21c7-44ae-b21a-c9ae321ae369")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIFactory : IDXGIObject {

		[return: MarshalAs(UnmanagedType.U4)]
		[PreserveSig]
		public HRESULT EnumAdapters(uint adapterIndex, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter adapter);

		/*
		public List<IDXGIAdapter> EnumAdapters() {
			List<IDXGIAdapter> adapters = new();
			uint index = 0;
			while (EnumAdapters(index++, out IDXGIAdapter adapter).Succeeded) adapters.Add(adapter);
			return adapters;
		}
		*/

		public void MakeWindowAssociation([NativeType("HWND")] IntPtr window, DXGIMakeWindowAssociationFlags flags);

		[return: NativeType("HWND")]
		public IntPtr GetWindowAssocation();

		public IDXGISwapChain CreateSwapchain([MarshalAs(UnmanagedType.IUnknown)] object device, in DXGISwapChainDesc desc);

		public IDXGIAdapter CreateSoftwareAdapter([NativeType("HMODULE")] IntPtr swrast);

	}

	[ComImport, Guid("54ec77fa-1377-44e6-8c32-88fd5f44c84c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIDevice : IDXGIObject {

		[return: MarshalAs(UnmanagedType.Interface)]
		public IDXGIAdapter GetAdapter();

		public IDXGISurface CreateSurface(in DXGISurfaceDesc desc, uint numSurfaces, DXGIUsageFlags usage, [NativeType("const DXGI_SHARED_RESOURCE*")] IntPtr pSharedResource);

		/*
		public IDXGISurface CreateSurface(in DXGISurfaceDesc desc, uint numSurfaces, DXGIUsageFlags usage, DXGISharedResource? sharedResource = null) {
			unsafe {
				IntPtr pSharedResource = IntPtr.Zero;
				DXGISharedResource sr;
				if (sharedResource.HasValue) {
					sr = sharedResource.Value;
					pSharedResource = (IntPtr)(&sr);
				}
				return CreateSurface(desc, numSurfaces, usage, pSharedResource);
			}
		}
		*/

		public void QueryResourceResidency([NativeType("IUnknown* const*")] IntPtr ppResources, [NativeType("DXGI_RESIDENCY*")] IntPtr pResidencyStatus, uint numResources);

		/*
		public DXGIResidency[] QueryResourceResidency(IEnumerable<IUnknown> resources) {
			Span<IntPtr> ppResources = stackalloc IntPtr[resources.Count()];
			int i = 0;
			foreach(IUnknown res in resources) ppResources[i++] = COMHelpers.GetCOMPointerForObject<IUnknown>(res);
			DXGIResidency[] residencies = new DXGIResidency[ppResources.Length];
			unsafe {
				fixed (IntPtr* ppResourcesPtr = ppResources) {
					fixed(DXGIResidency* ppResidencyPtr = residencies) {
						QueryResourceResidency((IntPtr)ppResourcesPtr, (IntPtr)ppResidencyPtr, (uint)ppResources.Length);
					}
				}
			}
			return residencies;
		}
		*/

		public void SetGPUThreadPriority(int priority);

		public int GetGPUThreadPriority();

	}

	[ComImport, Guid("770aae78-f26f-4dba-a829-253c83d1b387")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIFactory1 : IDXGIFactory {

		[return: MarshalAs(UnmanagedType.U4)]
		[PreserveSig]
		public HRESULT EnumAdapters1(uint adapterIndex, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter1 adapter);

		/*
		public List<IDXGIAdapter1> EnumAdapters1() {
			List<IDXGIAdapter1> adapters = new();
			uint index = 0;
			while (EnumAdapters1(index++, out IDXGIAdapter1 adapter).Succeeded) adapters.Add(adapter);
			return adapters;
		}
		*/

		[PreserveSig]
		public bool IsCurrent();

	}

	[ComImport, Guid("29038f61-3839-4626-91fd-086879011a05")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIAdapter1 : IDXGIAdapter {

		public DXGIAdapterDesc1 GetDesc1();

	}

	[ComImport, Guid("77db970f-6276-48ba-ba28-070143b4392c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIDevice1 : IDXGIDevice {

		public void SetMaximumFrameLatency(uint maxLatency);

		public uint GetMaximumFrameLatency();

	}

	public static class DXGI {

		// winerror.h

		public const int ErrorAccessDenied = unchecked((int)0x887A002B);
		public const int ErrorAccessLost = unchecked((int)0x887A0026);
		public const int ErrorAlreadyExists = unchecked((int)0x887A0036);
		public const int ErrorCannotProtectContent = unchecked((int)0x887A002A);
		public const int ErrorDeviceHung = unchecked((int)0x887A0006);
		public const int ErrorDeviceRemoved = unchecked((int)0x887A0005);
		public const int ErrorDeviceReset = unchecked((int)0x887A0007);
		public const int ErrorDriverInternalError = unchecked((int)0x887A0020);
		public const int ErrorFrameStatisticsDisjoint = unchecked((int)0x887A000B);
		public const int ErrorGraphicsVidpnSourceInUse = unchecked((int)0x887A000C);
		public const int ErrorInvalidCall = unchecked((int)0x887A0001);
		public const int ErrorMoreData = unchecked((int)0x887A0003);
		public const int ErrorNameAlreadyExists = unchecked((int)0x887A002C);
		public const int ErrorNonExclusive = unchecked((int)0x887A0021);
		public const int ErrorNotCurrentlyAvailable = unchecked((int)0x887A0022);
		public const int ErrorNotFound = unchecked((int)0x887A0002);
		public const int ErrorRemoteClientDisconnected = unchecked((int)0x887A0023);
		public const int ErrorRemoteOutOfMemory = unchecked((int)0x887A0024);
		public const int ErrorRestrictToOutputStale = unchecked((int)0x887A0029);
		public const int ErrorSDKComponentMissing = unchecked((int)0x887A002D);
		public const int ErrorSessionDisconnected = unchecked((int)0x887A0028);
		public const int ErrorUnsupported = unchecked((int)0x887A0004);
		public const int ErrorWaitTimeout = unchecked((int)0x887A0027);
		public const int ErrorWasStillDrawing = unchecked((int)0x887A000A);

		// dxgicommon.h

		public const uint StandardMultisampleQuality = 0xFFFFFFFF;
		public const uint CenterMultisampleQuality = 0xFFFFFFFE;

		// dxgi.h

		public const int MaxSwapChainBuffers = 16;

		[DllImport("dxgi.dll", PreserveSig = false)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "DllImport required for PreserveSig attribute")]
		private static extern IntPtr CreateDXGIFactory(in Guid riid);

		/// <summary>
		/// <para>
		/// Creates a DXGI 1.0 factory that you can use to generate other DXGI objects.
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// Use a DXGI factory to generate objects that
		/// <see href="https://docs.microsoft.com/en-us/windows/desktop/api/dxgi/nf-dxgi-idxgifactory-enumadapters">enumerate adapters</see>,
		/// <see href="https://docs.microsoft.com/en-us/windows/desktop/api/dxgi/nf-dxgi-idxgifactory-createswapchain">create swap chains</see>, and 
		/// <see href="https://docs.microsoft.com/en-us/windows/desktop/api/dxgi/nf-dxgi-idxgifactory-makewindowassociation">associate a window</see>
		/// with the alt+enter key sequence for toggling to and from the fullscreen display mode.
		/// </para>
		/// 
		/// <para>
		/// If the <b>CreateDXGIFactory</b> function succeeds, the reference count on the IDXGIFactory interface is incremented. To avoid a memory leak, when you finish using the interface, call the IDXGIFactory::Release method to release the interface.
		/// </para>
		/// 
		/// <para>
		/// <b>Note</b>  Do not mix the use of DXGI 1.0 (<b>IDXGIFactory</b>) and DXGI 1.1 (<b>IDXGIFactory1</b>) in an application.
		/// Use <b>IDXGIFactory</b> or <b>IDXGIFactory1</b>, but not both in an application.
		/// </para>
		/// </remarks>
		/// 
		/// <typeparam name="T">The COM interface type</typeparam>
		/// <returns>A <see cref="IDXGIFactory"/> object</returns>
		public static T CreateDXGIFactory<T>() where T : class => COMHelpers.GetObjectFromCOMGetter<T>(CreateDXGIFactory)!;

		[DllImport("dxgi.dll", PreserveSig = false)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "DllImport required for PreserveSig attribute")]
		private static extern IntPtr CreateDXGIFactory1(in Guid riid);

		public static T CreateDXGIFactory1<T>() where T : class => COMHelpers.GetObjectFromCOMGetter<T>(CreateDXGIFactory1)!;

	}

}

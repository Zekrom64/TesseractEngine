using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Windows;
using System.Diagnostics.CodeAnalysis;

namespace Tesseract.DirectX.GI {
	
	[ComImport, Guid("ea9dbf1a-c88e-4486-854a-98aa0138f30c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIDisplayControl : IUnknown {

		[PreserveSig]
		public bool IsStereoEnabled();

		public void SetStereoEnabled(bool enabled);

	}

	[ComImport, Guid("191cfac3-a341-470d-b26e-a864f428319c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIOutputDuplication : IDXGIObject {

		public DXGIOutputDesc GetDesc();

		[return: MarshalAs(UnmanagedType.Interface)]
		public IDXGIResource AcquireNextFrame(uint timeoutMilliseconds, out DXGIOutDuplFrameInfo frameInfo);
		
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U4)]
		public HRESULT GetFrameDirtyRects(uint dirtyRectsBufferSize, [NativeType("RECT*")] IntPtr pDirtyRectsBuffer, out uint dirtyRectsBufferSizeRequired);

		/*
		public RECT[] GetFrameDirtyRects() {
			RECT[] rects = new RECT[16];
			HRESULT hr;
			int nrects = 0;
			do {
				unsafe {
					fixed (RECT* pRects = rects) {
						hr = GetFrameDirtyRects((uint)(rects.Length * sizeof(RECT)), (IntPtr)pRects, out uint reqSize);
						nrects = (int)(reqSize / sizeof(RECT));
					}
				}
				if (hr == DXGI.ErrorMoreData) rects = new RECT[rects.Length * 2];
			} while (hr == DXGI.ErrorMoreData);
			hr.DoThrowException();
			return rects[0..nrects];
		}
		*/

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U4)]
		public HRESULT GetFrameMoveRects(uint moveRectsBufferSize, [NativeType("DXGI_OUTDUPL_MOVE_RECT*")] IntPtr pMoveRectsBuffer, out uint moveRectsBufferSizeRequired);

		/*
		public DXGIOutDuplMoveRect[] GetFrameMoveRects() {
			DXGIOutDuplMoveRect[] rects = new DXGIOutDuplMoveRect[16];
			HRESULT hr;
			int nrects = 0;
			do {
				unsafe {
					fixed (DXGIOutDuplMoveRect* pRects = rects) {
						hr = GetFrameMoveRects((uint)(rects.Length * sizeof(DXGIOutDuplMoveRect)), (IntPtr)pRects, out uint reqSize);
						nrects = (int)(reqSize / sizeof(DXGIOutDuplMoveRect));
					}
				}
				if (hr == DXGI.ErrorMoreData) rects = new DXGIOutDuplMoveRect[rects.Length * 2];
			} while (hr == DXGI.ErrorMoreData);
			hr.DoThrowException();
			return rects[0..nrects];
		}
		*/

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U4)]
		public HRESULT GetFramePointerShape(uint pointerShapeBufferSize, IntPtr pPointerShapeBuffer, out uint pointerShapeBufferSizeRequired, out DXGIOutDuplPointerShapeInfo pointerShapeInfo);

		/*
		public byte[] GetFramePointerShape(out DXGIOutDuplPointerShapeInfo pointerShapeInfo) {
			byte[] data = new byte[4096];
			HRESULT hr;
			int ndata;
			do {
				unsafe {
					fixed (byte* pData = data) {
						hr = GetFramePointerShape((uint)data.Length, (IntPtr)pData, out uint reqSize, out pointerShapeInfo);
						ndata = (int)reqSize;
					}
				}
				if (hr == DXGI.ErrorMoreData) data = new byte[ndata];
			} while (hr == DXGI.ErrorMoreData);
			hr.DoThrowException();
			return data[0..ndata];
		}
		*/

		public DXGIMappedRect MapDesktopSurface();

		public void UnMapDesktopSurface();

		public void ReleaseFrame();

	}

	[ComImport, Guid("aba496dd-b617-4cb8-a866-bc44d7eb1fa2")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGISurface2 : IDXGISurface1 {

		public void GetResource(in Guid riid, out IntPtr ppParentResource, out uint subresourceIndex);

		/*
		public T GetResource<T>(out uint subresourceIndex) {
			GetResource(typeof(T).GUID, out IntPtr ppParentResource, out subresourceIndex);
			return COMHelpers.GetObjectForCOMPointer<T>(ppParentResource);
		}
		*/

	}

	[ComImport, Guid("30961379-4609-4a41-998e-54fe567ee0c1")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIResource1 : IDXGIResource {

		[return: MarshalAs(UnmanagedType.Interface)]
		public IDXGISurface2 CreateSubresourceSurface(uint index);

		[return: NativeType("HANDLE")]
		public IntPtr CreateSharedHandle([NativeType("const SECURITY_ATTRIBUTES*")] IntPtr pAttributes, DXGISharedHandleAccessFlags access, [MarshalAs(UnmanagedType.LPWStr)] string name);

	}

	[ComImport, Guid("05008617-fbfd-4051-a790-144884b4f6a9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIDevice2 : IDXGIDevice1 {

		public void OfferResources(uint numResources, [NativeType("IDXGIResource* const*")] IntPtr ppResources, DXGIOfferResourcePriority priority);

		/*
		public void OfferResources(IEnumerable<IDXGIResource> resources, DXGIOfferResourcePriority priority) {
			Span<IntPtr> pResources = stackalloc IntPtr[resources.Count()];
			int i =	0;
			foreach (IDXGIResource resource in resources) pResources[i++] = COMHelpers.GetCOMPointerForObject<IDXGIResource>(resource);
			unsafe {
				fixed(IntPtr* ppResources = pResources) {
					OfferResources((uint)pResources.Length, (IntPtr)ppResources, priority);
				}
			}
		}
		*/

		public void ReclaimResources(uint numResources, [NativeType("IDXGIResource* const*")] IntPtr ppResources, [NativeType("BOOL*")] IntPtr pDiscarded);

		/*
		public bool[] ReclaimResources(IEnumerable<IDXGIResource> resources) {
			Span<IntPtr> pResources = stackalloc IntPtr[resources.Count()];
			int i = 0;
			foreach (IDXGIResource resource in resources) pResources[i++] = COMHelpers.GetCOMPointerForObject<IDXGIResource>(resource);
			Span<int> discarded = stackalloc int[pResources.Length];
			unsafe {
				fixed (IntPtr* ppResources = pResources) {
					fixed(int* ppDiscarded = discarded) {
						ReclaimResources((uint)pResources.Length, (IntPtr)ppResources, (IntPtr)ppDiscarded);
					}
				}
			}
			bool[] bdiscarded = new bool[pResources.Length];
			for (i = 0; i < pResources.Length; i++) bdiscarded[i] = discarded[i] != 0;
			return bdiscarded;
		}
		*/

		public void EnqueueSetEvent([NativeType("HANDLE")] IntPtr hEvent);

	}

	[ComImport, Guid("790a45f7-0d42-4876-983a-0a55cfe6f4aa")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGISwapChain1 : IDXGISwapChain {

		public DXGISwapChainDesc1 GetDesc1();

		public DXGISwapChainFullscreenDesc GetFullscreenDesc();

		[return: NativeType("HWND")]
		public IntPtr GetHwnd();

		public IntPtr GetCoreWindow(in Guid riid);

		//public T GetCoreWindow<T>() => COMHelpers.GetObjectFromCOMGetter<T>(GetCoreWindow);

		public void Present(uint syncInterval, DXGIPresentFlags flags, in DXGIPresentParameters parameters);

		[PreserveSig]
		public bool IsTemporaryMonoSupported();

		[return: MarshalAs(UnmanagedType.Interface)]
		public IDXGIOutput GetRestrictToOutput();

		public void SetBackgroundColor(DXGIRGBA color);

		public DXGIRGBA GetBackgroundColor();

		public void SetRotation(DXGIModeRotation rotation);

		public DXGIModeRotation GetRotation();

	}

	[ComImport, Guid("50c83a1c-e072-4c48-87b0-3630fa36a6d0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIFactory2 : IDXGIFactory1 {

		[PreserveSig]
		public bool IsWindowStereoEnabled();

		public IDXGISwapChain1 CreateSwapchainForHwnd([MarshalAs(UnmanagedType.IUnknown)] object device, [NativeType("HWND")] IntPtr hWnd, in DXGISwapChainDesc1 desc, [NativeType("const DXGI_SWAP_CHAIN_FULLSCREEN_DESC*")] IntPtr pFullscreenDesc, [MarshalAs(UnmanagedType.Interface)] [AllowNull] IDXGIOutput restrictToOutput);

		/*
		public IDXGISwapChain1 CreateSwapchainForHwnd(object device, [NativeType("HWND")] IntPtr hWnd, in DXGISwapChainDesc1 desc, DXGISwapChainFullscreenDesc? fullscreenDesc = null, [AllowNull] IDXGIOutput restrictToOutput = null) {
			unsafe {
				IntPtr pfd = IntPtr.Zero;
				DXGISwapChainFullscreenDesc fd;
				if (fullscreenDesc.HasValue) {
					fd = fullscreenDesc.Value;
					pfd = (IntPtr)(&fd);
				}
				return CreateSwapchainForHwnd(device, hWnd, desc, pfd, restrictToOutput);
			}
		}
		*/

		public IDXGISwapChain1 CreateSwapchainForCoreWindow([MarshalAs(UnmanagedType.IUnknown)] object device, [MarshalAs(UnmanagedType.IUnknown)] object window, in DXGISwapChainDesc1 desc, [MarshalAs(UnmanagedType.Interface)][AllowNull] IDXGIOutput restrictToOutput);

		public LUID GetSharedResourceAdapterLuid([NativeType("HANDLE")] IntPtr hResource);

		public uint RegisterStereoStatusWindow([NativeType("HWND")] IntPtr hWnd, uint msg);

		public uint RegisterStereoStatusEvent([NativeType("HANDLE")] IntPtr hEvent);

		[PreserveSig]
		public void UnregisterStereoStatus(uint cookie);

		public uint RegisterOcclusionStatusWindow([NativeType("HWND")] IntPtr hWnd, uint msg);

		public uint RegisterOcclusionStatusEvent([NativeType("HANDLE")] IntPtr hEvent);

		[PreserveSig]
		public void UnregisterOcclusionStatus(uint cookie);

		public IDXGISwapChain1 CreateSwapChainForComposition([MarshalAs(UnmanagedType.IUnknown)] object device, in DXGISwapChainDesc1 desc, [MarshalAs(UnmanagedType.Interface)][AllowNull] IDXGIOutput restrictToOutput);

	}

	[ComImport, Guid("0AA1AE0A-FA0E-4B84-8644-E05FF8E5ACB5")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIAdapter2 : IDXGIAdapter1 {

		public DXGIAdapterDesc2 GetDesc2();

	}

	[ComImport, Guid("00cddea8-939b-4b83-a340-a685226666cc")]
	public interface IDXGIOutput1 : IDXGIOutput {

		public void GetDisplayModeList1(DXGIFormat format, DXGIEnumModesFlags flags, ref uint numModes, [NativeType("DXGI_MODE_DESC1*")] IntPtr pDesc);

		/*
		public DXGIModeDesc1[] GetDisplayModeList1(DXGIFormat format, DXGIEnumModesFlags flags) {
			unsafe {
				uint nmodes = 0;
				GetDisplayModeList1(format, flags, ref nmodes, IntPtr.Zero);
				DXGIModeDesc1[] modes = new DXGIModeDesc1[nmodes];
				fixed(DXGIModeDesc1* pModes = modes) {
					GetDisplayModeList1(format, flags, ref nmodes, (IntPtr)pModes);
				}
				return modes;
			}
		}
		*/

		public void FindClosestMatchingMode1(in DXGIModeDesc1 modeToMatch, out DXGIModeDesc1 closestMatch, [MarshalAs(UnmanagedType.IUnknown)] object concernedDevice);

		public void GetDisplaySurfaceData([MarshalAs(UnmanagedType.Interface)] IDXGIResource destination);

		public IDXGIOutputDuplication DuplicateOutput([MarshalAs(UnmanagedType.IUnknown)] object device);

	}

}

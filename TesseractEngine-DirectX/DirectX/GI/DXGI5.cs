using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.DirectX.GI {

	[Flags]
	public enum DXGIOutDuplFlag {
		CompositedUICaptureOnly = 1
	}

	public enum DXGI_HDRMetaDataType {
		None = 0,
		HDR10,
		HDR10Plus
	}

	[Flags]
	public enum DXGIOfferResourceFlags {
		AllowDecommit = 1
	}

	public enum DXGIReclaimResourceResults {
		Ok = 0,
		Discarded,
		NotCommitted
	}

	public enum DXGIFeature {
		PresentAllowTearing = 0
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGI_HDRMetaDataHDR10 {

		public ushort RedPrimary0;
		public ushort RedPrimary1;
		public ushort GreenPrimary0;
		public ushort GreenPrimary1;
		public ushort BluePrimary0;
		public ushort BluePrimary1;
		public ushort WhitePoint0;
		public ushort WhitePoint1;
		public uint MaxMasteringLuminance;
		public uint MinMasteringLuminance;
		public ushort MaxContentLightLevel;
		public ushort MaxFrameAverageLightLevel;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXGI_HDRMetaDataHDR10Plus {

		public unsafe fixed byte Data[72];

	}

	[ComImport, Guid("80a07424-ab52-42eb-833c-0c42fd282d98")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIOutput5 : IDXGIOutput4 {

		public void DuplicateOutput1([MarshalAs(UnmanagedType.IUnknown)] object device, DXGIOutDuplFlag flags, uint formatCount, [NativeType("const DXGI_FORMAT*")] IntPtr formats, [MarshalAs(UnmanagedType.Interface)] out IDXGIOutputDuplication duplication);

	}

	[ComImport, Guid("3d585d5a-bd4a-489e-b1f4-3dbcb6452ffb")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGISwapChain4 : IDXGISwapChain3 {

		public void SetHDRMetaData(DXGI_HDRMetaDataType type, uint size, IntPtr metadata);

	}

	[ComImport, Guid("95b4f95f-d8da-4ca4-9ee6-3b76d5968a10")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIDevice4 : IDXGIDevice3 {

		public void OfferResources1(uint resourceCount, [NativeType("IDXGIResource* const*")] IntPtr resources, DXGIOfferResourcePriority priority, DXGIOfferResourceFlags flags);

		public void ReclaimResources1(uint resourceCount, [NativeType("IDXGIResource* const*")] IntPtr resources, out DXGIReclaimResourceResults results);

	}

	[ComImport, Guid("7632e1f5-ee65-4dca-87fd-84cd75f8838d")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXGIFactory5 : IDXGIFactory4 {

		public void CheckFeatureSupport(DXGIFeature feature, IntPtr supportData, uint supportDataSize);

	}

}

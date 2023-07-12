using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.WebGPU.Native;

namespace Tesseract.WebGPU {

	public class WGpuAdapter : IWGpuObject {

		public nint PrimitiveHandle { get; }

		public WGpuFeatures Features => WGpuFunctions.wgpu_adapter_or_device_get_features(PrimitiveHandle);

		public WGpuSupportedLimits Limits {
			get {
				WGpuFunctions.wgpu_adapter_or_device_get_limits(PrimitiveHandle, out var limits);
				return limits;
			}
		}

		public bool IsFallback => WGpuFunctions.wgpu_adapter_is_fallback_adapter(PrimitiveHandle);

		public WGpuAdapter(nint adapter) {
			PrimitiveHandle = adapter;
		}

		public WGpuDevice? RequestDevice(WGpuDeviceDescriptor? descriptor = null) {
			nint device;
			if (descriptor != null) {
				device = WGpuFunctions.wgpu_adapter_request_device_sync(PrimitiveHandle, descriptor.Value);
			} else {
				device = WGpuFunctions.wgpu_adapter_request_device_sync_simple(PrimitiveHandle);
			}
			return device == 0 ? null : new WGpuDevice(device);
		}

		public Task<WGpuDevice?> RequestDeviceAsync(WGpuDeviceDescriptor? descriptor = null) {
			TaskCompletionSource<WGpuDevice?> tcs = new();
			void Callback(nint device, nint _) => tcs.SetResult(device == 0 ? null : new WGpuDevice(device));
			if (descriptor != null) {
				WGpuFunctions.wgpu_adapter_request_device_async(PrimitiveHandle, descriptor.Value, Callback, 0);
			} else {
				WGpuFunctions.wgpu_adapter_request_device_async_simple(PrimitiveHandle, Callback);
			}
			return tcs.Task;
		}

		public Task<WGpuAdapterInfo> RequestAdapterInfoAsync() {
			TaskCompletionSource<WGpuAdapterInfo> tcs = new();
			using MemoryStack sp = MemoryStack.Push();
			var hints = sp.Values<nint>(sp.ASCII("vendor"), sp.ASCII("architecture"), sp.ASCII("device"), sp.ASCII("description"), 0);
			void Callback(nint adapter, in WGpuAdapterInfo info, nint _) => tcs.SetResult(info);
			WGpuFunctions.wgpu_adapter_request_adapter_info_async(PrimitiveHandle, hints, Callback, 0);
			return tcs.Task;
		}
	}

}

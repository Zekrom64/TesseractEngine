using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.WebGPU.Native {

	// Note: WASM is (for now) 32-bit only but the *correct* interpretation of these handles is as pointers
	// Therefore we declare as such and cross our fingers hoping Blazor translates these correctly

	using WGpuObjectBase = nint;

	using WGpuAdapter = nint;
	using WGpuDevice = nint;
	using WGpuBuffer = nint;
	using WGpuTexture = nint;
	using WGpuTextureView = nint;
	using WGpuExternalTexture = nint;
	using WGpuSampler = nint;
	using WGpuBindGroupLayout = nint;
	using WGpuBindGroup = nint;
	using WGpuPipelineLayout = nint;
	using WGpuShaderModule = nint;
	using WGpuComputePipeline = nint;
	using WGpuRenderPipeline = nint;
	using WGpuPipelineBase = nint;
	using WGpuCommandBuffer = nint;
	using WGpuCommandEncoder = nint;
	using WGpuBindingCommandsMixin = nint;
	using WGpuComputePassEncoder = nint;
	using WGpuRenderCommandsMixin = nint;
	using WGpuRenderPassEncoder = nint;
	using WGpuRenderBundle = nint;
	using WGpuRenderBundleEncoder = nint;
	using WGpuQueue = nint;
	using WGpuQuerySet = nint;
	using WGpuCanvasContext = nint;
	using WGpuImageBitmap = nint;

	public delegate void WGpuRequestAdapterCallback(WGpuAdapter adapter, IntPtr userData);

	public delegate void WGpuRequestDeviceCallback(WGpuDevice device, IntPtr userData);

	public delegate void WGpuRequestAdapterInfoCallback(WGpuAdapter adapter, in WGpuAdapterInfo adapterInfo, IntPtr userData);

	internal static partial class WGpuFunctions {

		public const string LibName = "libwebgpu";

		[LibraryImport(LibName)]
		public static partial uint wgpu_get_num_live_objects();

		[LibraryImport(LibName)]
		public static partial void wgpu_object_destroy(WGpuObjectBase wgpuObject);

		[LibraryImport(LibName)]
		public static partial void wgpu_destroy_all_objects();

		[LibraryImport(LibName)]
		public static partial WGpuCanvasContext wgpu_canvas_get_webgpu_context([MarshalAs(UnmanagedType.LPUTF8Str)] string canvasSelector);

		[LibraryImport(LibName)]
		public static partial EMBOOL wgpu_is_valid_object(WGpuObjectBase obj);

		[LibraryImport(LibName)]
		public static partial void wgpu_object_set_label(WGpuObjectBase obj, [MarshalAs(UnmanagedType.LPUTF8Str)] string label);

		[LibraryImport(LibName)]
		public static partial int wgpu_object_get_label(WGpuObjectBase obj, [NativeType("char*")] IntPtr dstLabel, uint dstLabelSize);


		[LibraryImport(LibName)]
		public static partial EMBOOL navigator_gpu_available();

		[LibraryImport(LibName)]
		public static partial void navigator_delete_webgpu_api_access();

		[LibraryImport(LibName)]
		public static partial EMBOOL navigator_gpu_request_adapter_async(in WGpuRequestAdapterOptions options, [MarshalAs(UnmanagedType.FunctionPtr)] WGpuRequestAdapterCallback adapterCallback, IntPtr userData);

		[LibraryImport(LibName)]
		public static partial WGpuAdapter navigator_gpu_request_adapter_sync(in WGpuRequestAdapterOptions options);

		[LibraryImport(LibName)]
		public static partial void navigator_gpu_request_adapter_async_simple([MarshalAs(UnmanagedType.FunctionPtr)] WGpuRequestAdapterCallback adapterCallback);

		[LibraryImport(LibName)]
		public static partial WGpuAdapter navigator_gpu_request_adapter_sync_simple();

		[LibraryImport(LibName)]
		public static partial WGpuTextureFormat navigator_gpu_get_preferred_canvas_format();

		[LibraryImport(LibName)]
		public static partial EMBOOL wgpu_is_adapter(WGpuObjectBase obj);

		[LibraryImport(LibName)]
		public static partial WGpuFeatures wgpu_adapter_or_device_get_features(WGpuAdapter adapter);

		[LibraryImport(LibName)]
		public static partial EMBOOL wgpu_adapter_or_device_supports_feature(WGpuAdapter adapter, WGpuFeatures feature);

		[LibraryImport(LibName)]
		public static partial void wgpu_adapter_or_device_get_limits(WGpuAdapter adapter, out WGpuSupportedLimits limits);

		[LibraryImport(LibName)]
		public static partial EMBOOL wgpu_adapter_is_fallback_adapter(WGpuAdapter adapter);

		[LibraryImport(LibName)]
		public static partial void wgpu_adapter_request_device_async(WGpuAdapter adapter, in WGpuDeviceDescriptor descriptor, [MarshalAs(UnmanagedType.FunctionPtr)] WGpuRequestDeviceCallback deviceCallback, IntPtr userData);

		[LibraryImport(LibName)]
		public static partial WGpuDevice wgpu_adapter_request_device_sync(WGpuAdapter adapter, in WGpuDeviceDescriptor descriptor);

		[LibraryImport(LibName)]
		public static partial void wgpu_adapter_request_device_async_simple(WGpuAdapter adapter, WGpuRequestDeviceCallback deviceCallback);

		[LibraryImport(LibName)]
		public static partial WGpuDevice wgpu_adapter_request_device_sync_simple(WGpuAdapter adapter);

		[LibraryImport(LibName)]
		public static partial void wgpu_adapter_request_adapter_info_async(WGpuAdapter adapter, [NativeType("const char**")] IntPtr unmaskHints, WGpuRequestAdapterInfoCallback callback, IntPtr userData);

		[LibraryImport(LibName)]
		public static partial EMBOOL wgpu_is_device(WGpuObjectBase obj);

		[LibraryImport(LibName)]
		public static partial WGpuQueue wgpu_get_device_queue(WGpuDevice device);

		[LibraryImport(LibName)]
		public static partial WGpuBuffer wgpu_device_create_buffer(WGpuDevice device, in WGpuBufferDescriptor descriptor);

	}


}

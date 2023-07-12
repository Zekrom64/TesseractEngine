using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.WebGPU.Native;

namespace Tesseract.WebGPU {

	public static class WGpu {

		public const int ObjectLabelMaxLength = 256;


		/// <summary>
		/// The number of WebGPU objects referenced by the WebGPU JS library.
		/// </summary>
		public static uint NumLiveObjects => WGpuFunctions.wgpu_get_num_live_objects();

		/// <summary>
		/// If the browser advertises that it supports WebGPU. Note that this does not necessarily
		/// mean that the underlying graphics device is supported.
		/// </summary>
		public static bool NavigatorGpuAvailable => WGpuFunctions.navigator_gpu_available() != 0;

		/// <summary>
		/// The preferred format for textures presented to the canvas.
		/// </summary>
		public static WGpuTextureFormat PreferredCanvasFormat => WGpuFunctions.navigator_gpu_get_preferred_canvas_format();


		/// <summary>
		/// Deinitializes all initialized WebGPU objects.
		/// </summary>
		public static void DestroyAllObjects() => WGpuFunctions.wgpu_destroy_all_objects();

		/// <summary>
		/// Acquires a canvas context from a canvas by calling <c>canvas.getCanvasContext()</c>.
		/// </summary>
		/// <param name="selector">The DOM selector to find the canvas by</param>
		/// <returns>The WebGPU context for the canvas, or null if there was an error</returns>
		public static WGpuCanvasContext? CanvasGetWebGPUContext(string selector) {
			IntPtr retn = WGpuFunctions.wgpu_canvas_get_webgpu_context(selector);
			return retn == IntPtr.Zero ? null : new WGpuCanvasContext(retn);
		}

		/// <summary>
		/// Removes access to the WebGPU API on the current JS page, effectively the same as
		/// doing <c>delete navigator.gpu</c> in Javascript.
		/// </summary>
		public static void DeleteApiAccess() => WGpuFunctions.navigator_delete_webgpu_api_access();

		/// <summary>
		/// Synchronously requests an adapter from the user agent.
		/// </summary>
		/// <param name="options">Adapter options, or null if nothing is requested</param>
		/// <returns>The requested adapter, or null if there was an error</returns>
		public static WGpuAdapter? RequestAdapter(WGpuRequestAdapterOptions? options = null) {
			nint adapter;
			if (options != null) {
				adapter = WGpuFunctions.navigator_gpu_request_adapter_sync(options.Value);
			} else {
				adapter = WGpuFunctions.navigator_gpu_request_adapter_sync_simple();
			}
			return adapter == 0 ? null : new WGpuAdapter(adapter);
		}

		/// <summary>
		/// Asynchronously requests an adapter from the user agent.
		/// </summary>
		/// <param name="options">Adapter options, or null if nothing is requested</param>
		/// <returns>Task that completes with the requested adapter, or null if there was an error</returns>
		public static Task<WGpuAdapter?> RequestAdapterAsync(WGpuRequestAdapterOptions? options = null) {
			TaskCompletionSource<WGpuAdapter?> tcs = new();
			void Callback(nint adapter, nint _) => tcs.TrySetResult(adapter == 0 ? null : new WGpuAdapter(adapter));
			if (options != null) {
				if (!WGpuFunctions.navigator_gpu_request_adapter_async(options.Value, Callback, 0)) tcs.TrySetResult(null);
			} else {
				WGpuFunctions.navigator_gpu_request_adapter_async_simple(Callback);
			}
			return tcs.Task;
		}

	}

}

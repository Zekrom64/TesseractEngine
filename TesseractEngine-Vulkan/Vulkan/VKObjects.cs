using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Vulkan {

	public class VulkanException : Exception {

		public static string GetResultString(VKResult result) => result switch {
			VKResult.Success => "Success (Non-Error)",
			VKResult.NotReady => "Not ready (Non-Error)",
			VKResult.Timeout => "Timeout (Non-Error)",
			VKResult.EventSet => "Event set (Non-Error)",
			VKResult.EventReset => "Event reset (Non-Error)",
			VKResult.Incomplete => "Incomplete, return array too small (Non-Error)",
			VKResult.ErrorOutOfHostMemory => "Out of host memory",
			VKResult.ErrorOutOfDeviceMemory => "Out of device memory",
			VKResult.ErrorInitializationFailed => "Initialization failed",
			VKResult.ErrorDeviceLost => "Device lost",
			VKResult.ErrorMemoryMapFailed => "Memory map failed",
			VKResult.ErrorLayerNotPresent => "Layer not present",
			VKResult.ErrorExtensionNotPresent => "Extension not present",
			VKResult.ErrorFeatureNotPresent => "Feature not present",
			VKResult.ErrorIncompatibleDriver => "Incompatible driver",
			VKResult.ErrorTooManyObjects => "Too many objects",
			VKResult.ErrorFormatNotSupported => "Format not supported",
			VKResult.ErrorFragmentedPool => "Fragmented pool",
			VKResult.ErrorUnknown => "Uknown error in Vulkan API",
			VKResult.ErrorOutOfPoolMemory => "Out of pool memory",
			VKResult.ErrorInvalidExternalHandle => "Invalid external handle",
			VKResult.ErrorFragmentation => "Fragmentation of descriptor pool",
			VKResult.ErrorInvalidOpaqueCaptureAddress => "Invalid opaque capture address",
			VKResult.ErrorSurfaceLostKHR => "Surface Lost",
			VKResult.ErrorNativeWindowInUseKHR => "Native Window in Use",
			VKResult.SuboptimalKHR => "Suboptimal swapchain (Non-Error)",
			VKResult.ErrorOutOfDateKHR => "Out of date swapchain",
			VKResult.ErrorIncompatibleDisplayKHR => "Incompatible display",
			VKResult.ErrorValidationFailedEXT => "Validation failed",
			VKResult.ErrorInvalidShaderNV => "Invalid shader",
			VKResult.ErrorIncompatibleVersionKHR => "VK_ERROR_INCOMPATIBLE_VERSION_KHR",
			VKResult.ErrorInvalidDRMFormatModifierPlaneLayoutEXT => "VK_ERROR_INVALID_DRM_FORMAT_MODIFIER_PLANE_LAYOUT_EXT",
			VKResult.ErrorNotPermittedEXT => "VK_ERROR_NOT_PERMITTED_EXT",
			VKResult.ErrorFullScreenExclusiveLostEXT => "Exclusive fullscreen access lost",
			VKResult.ThreadIdleKHR => "Thread idle (Non-Error)",
			VKResult.ThreadDoneKHR => "Thread done (Non-Error)",
			VKResult.OperationDeferredKHR => "Operation deferred (Non-Error)",
			VKResult.OperationNotDeferredKHR => "Operation not deferred (Non-Error)",
			VKResult.PipelineCompileRequiredEXT => "Pipeline compile required (Non-Error)",
			_ => "Unknown VkResult : 0x" + result.ToString("X"),
		};

		public VulkanException(VKResult result) : base(GetResultString(result)) { }

		public VulkanException(string msg, VKResult result) : base(msg + GetResultString(result)) { }

		public VulkanException(string msg) : base(msg) { }

	}

	public class VulkanAllocationCallbacks : IDisposable {

		public IntPtr Ptr { get; }

		public VulkanAllocationCallbacks(in VKAllocationCallbacks callbacks) {
			Ptr = Marshal.AllocHGlobal(Marshal.SizeOf<VKAllocationCallbacks>());
			Marshal.StructureToPtr(callbacks, Ptr, false);
		}

		~VulkanAllocationCallbacks() {
			Dispose();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Marshal.DestroyStructure<VKAllocationCallbacks>(Ptr);
			Marshal.FreeHGlobal(Ptr);
		}

		public static implicit operator VulkanAllocationCallbacks(in VKAllocationCallbacks callbacks) => new(callbacks);

		public static implicit operator IntPtr(VulkanAllocationCallbacks callbacks) => callbacks != null ? callbacks.Ptr : IntPtr.Zero;

	}

}

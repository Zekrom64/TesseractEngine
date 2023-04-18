using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public static class KHRDeferredHostOperations {

		public const string ExtensionName = "VK_KHR_deferred_host_operations";

	}

	public unsafe class KHRDeferredHostOperationsFunctions {

		[NativeType("VkResult vkCreateDeferredOperationKHR(VkDevice device, const VkAllocationCallbacks* pAllocator, VkDeferredOperationKHR* pDeferredOperation)")]
		public delegate* unmanaged<IntPtr, VKAllocationCallbacks*, out ulong, VKResult> vkCreateDeferredOperationKHR;
		[NativeType("VkResult vkDeferredOperationJoinKHR(VkDevice device, VkDeferredOperationKHR operation)")]
		public delegate* unmanaged<IntPtr, ulong, VKResult> vkDeferredOperationJoinKHR;
		[NativeType("void vkDestroyDeferredOperationKHR(VkDevice device, VkDeferredOperationKHR operation, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<IntPtr, ulong, VKAllocationCallbacks*, void> vkDestroyDeferredOperationKHR;
		[NativeType("uint32_t vkGetDeferredOperationMaxConcurrencyKHR(VkDevice device, VkDeferredOperationKHR operation)")]
		public delegate* unmanaged<IntPtr, ulong, uint> vkGetDeferredOperationMaxConcurrencyKHR;
		[NativeType("VkResult vkGetDeferredOperationResultKHR(VkDevice device, VkDeferredOperationKHR operation)")]
		public delegate* unmanaged<IntPtr, ulong, VKResult> vkGetDeferredOperationResultKHR;

	}

}

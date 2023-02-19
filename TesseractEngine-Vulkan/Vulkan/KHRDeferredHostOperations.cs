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

#nullable disable
	public class KHRDeferredHostOperationsFunctions {

		public delegate VKResult PFN_vkCreateDeferredOperationKHR([NativeType("VkDevice")] IntPtr device, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, [NativeType("VkDeferredOperationKHR")] out ulong deferredOperation);
		public delegate VKResult PFN_vkDeferredOperationJoinKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDeferredOperationKHR")] ulong operation);
		public delegate void PFN_vkDestroyDeferredOperationKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDeferredOperationKHR")] ulong operation, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate uint PFN_vkGetDeferredOperationMaxConcurrencyKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDeferredOperationKHR")] ulong operation);
		public delegate VKResult PFN_vkGetDeferredOperationResultKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDeferredOperationKHR")] ulong operation);

		public PFN_vkCreateDeferredOperationKHR vkCreateDeferredOperationKHR;
		public PFN_vkDeferredOperationJoinKHR vkDeferredOperationJoinKHR;
		public PFN_vkDestroyDeferredOperationKHR vkDestroyDeferredOperationKHR;
		public PFN_vkGetDeferredOperationMaxConcurrencyKHR vkGetDeferredOperationMaxConcurrencyKHR;
		public PFN_vkGetDeferredOperationResultKHR vkGetDeferredOperationResultKHR;

	}
#nullable restore

}

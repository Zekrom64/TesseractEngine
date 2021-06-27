using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.Vulkan {

	using VkBool32 = Boolean;
	using VkDeviceSize = UInt64;
	using VkSampleMask = UInt32;
	using VkInstance = IntPtr;
	using VkPhysicalDevice = IntPtr;
	using VkDevice = IntPtr;
	using VkQueue = IntPtr;
	using VkSemaphore = UInt64;
	using VkCommandBuffer = IntPtr;
	using VkFence = UInt64;
	using VkDeviceMemory = UInt64;
	using VkBuffer = UInt64;
	using VkImage = UInt64;
	using VkEvent = UInt64;
	using VkQueryPool = UInt64;
	using VkBufferView = UInt64;
	using VkImageView = UInt64;
	using VkShaderModule = UInt64;
	using VkPipelineCache = UInt64;
	using VkPipelineLayout = UInt64;
	using VkRenderPass = UInt64;
	using VkPipeline = UInt64;
	using VkDescriptorSetLayout = UInt64;
	using VkSampler = UInt64;
	using VkDescriptorPool = UInt64;
	using VkDescriptorSet = UInt64;
	using VkFramebuffer = UInt64;
	using VkCommandPool = UInt64;

	public delegate IntPtr VKAllocationFunction(IntPtr userData, nuint size, nuint alignment, VKSystemAllocationScope allocationScope);

	public delegate IntPtr VKReallocationFunction(IntPtr userData, IntPtr original, nuint size, nuint alignment, VKSystemAllocationScope allocationScope);

	public delegate void VKFreeFunction(IntPtr userdata, IntPtr memory);

	public delegate IntPtr VKInternalAllocationNotification(IntPtr userData, nuint size, VKInternalAllocationType allocationType, VKSystemAllocationScope allocationScope);

	public delegate void VKInternalFreeNotification(IntPtr userData, nuint size, VKInternalAllocationType allocationType, VKSystemAllocationScope allocationScope);


	public delegate IntPtr VKGetInstanceProcAddr(VkInstance instance, [MarshalAs(UnmanagedType.LPStr)] string name);

	public delegate IntPtr VKGetDeviceProcAddr(VkDevice device, [MarshalAs(UnmanagedType.LPStr)] string name);


}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {
	
	[Flags]
	public enum VKDebugReportFlagBitsEXT : int {
		Information = 0x00000001,
		Warning = 0x00000002,
		PerformanceWarning = 0x00000004,
		Error = 0x00000008,
		Debug = 0x00000010
	}

	public enum VKDebugReportObjectTypeEXT : int {
		Unknown = 0,
		Instance = 1,
		PhysicalDevice = 2,
		Device = 3,
		Queue = 4,
		Semaphore = 5,
		CommandBuffer = 6,
		Fence = 7,
		DeviceMemory = 8,
		Buffer = 9,
		Image = 10,
		Event = 11,
		QueryPool = 12,
		BufferView = 13,
		ImageView = 14,
		ShaderModule = 15,
		PipelineCache = 16,
		PipelineLayout = 17,
		RenderPass = 18,
		Pipeline = 19,
		DescriptorSetLayout = 20,
		Sampler = 21,
		DescriptorPool = 22,
		DescriptorSet = 23,
		Framebuffer = 24,
		CommandPool = 25,
		SurfaceKHR = 26,
		SwapchainKHR = 27,
		DebugReportCallbackEXT = 28,
		DisplayKHR = 29,
		DisplayModeKHR = 30,
		ValidationCacheEXT = 33,
		// Vulkan 1.1
		SamplerYCbCrConversion = 1000156000,
		DescriptorUpdateTemplate = 1000085000,
		// VK_NVX_binary_import
		CUModuleNVX = 1000029000,
		CUFunctionNVX = 1000029001,
		// VK_KHR_acceleration_structure
		AccelerationStructureKHR = 1000150000,
		// VK_NV_ray_tracing
		AccelerationStructureNV = 1000165000,
		// VK_FUCHSIA_buffer_collection
		BufferCollectionFUCHSIA = 1000366000,
	}

	public delegate bool VKDebugReportCallback(VKDebugReportFlagBitsEXT flags, VKDebugReportObjectTypeEXT objectType, ulong obj, nuint location, int messageCode, [MarshalAs(UnmanagedType.LPUTF8Str)] string layerPrefix, [MarshalAs(UnmanagedType.LPUTF8Str)] string message, IntPtr userData);

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKDebugReportCallbackCreateInfoEXT {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VKDebugReportFlagBitsEXT flags;
		public VKDebugReportFlagBitsEXT Flags { get => flags; init => flags = value; }
		private readonly IntPtr callback;
		public VKDebugReportCallback Callback { init => callback = Marshal.GetFunctionPointerForDelegate(value); }
		private readonly IntPtr userData;
		public IntPtr UserData { get => userData; init => userData = value; }

	}

	public unsafe class EXTDebugReportInstanceFunctions {

		[NativeType("VkResult vkCreateDebugReportCallbackEXT(VkInstance instance, const VkDebugReportCallbackCreateInfoEXT* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDebugReportCallbackEXT* pCallback)")]
		public delegate* unmanaged<IntPtr, in VKDebugReportCallbackCreateInfoEXT, VKAllocationCallbacks*, out ulong, VKResult> vkCreateDebugReportCallbackEXT;
		[NativeType("void vkDebugReportMessageEXT(VkInstance instance, VkDebugReportFlagsEXT flags, VkDebugReportObjectTypeEXT objectType, uint64_t obj, size_t location, int32_t messageCode, const char* layerPrefix, const char* message)")]
		public delegate* unmanaged<IntPtr, VKDebugReportFlagBitsEXT, VKDebugReportObjectTypeEXT, ulong, nuint, int, IntPtr, IntPtr, void> vkDebugReportMessageEXT;
		[NativeType("void vkDestroyDebugReportCallbackEXT(VkInstance instance, VkDebugReportCallbackEXT callback, const VkAllocationCallbacks* allocator)")]
		public delegate* unmanaged<IntPtr, ulong, IntPtr, VKAllocationCallbacks*> vkDestroyDebugReportCallbackEXT;

	}

	public static class EXTDebugReport {

		public const string ExtensionName = "VK_EXT_debug_report";

	}

	public class VKDebugReportCallbackEXT : IDisposable, IVKInstanceObject, IVKAllocatedObject, IPrimitiveHandle<ulong> {

		[NativeType("VkDebugReportCallbackEXT")]
		public ulong DebugReportCallback { get; }

		public VKInstance Instance { get; }

		public VulkanAllocationCallbacks? Allocator { get; }

		public ulong PrimitiveHandle => DebugReportCallback;

		public VKDebugReportCallbackEXT(VKInstance instance, ulong callback, VulkanAllocationCallbacks? allocator) {
			Instance = instance;
			DebugReportCallback = callback;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				Instance.EXTDebugReportFunctions!.vkDestroyDebugReportCallbackEXT(Instance, DebugReportCallback, Allocator);
			}
		}

	}

}

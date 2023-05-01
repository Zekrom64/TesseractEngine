using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	[Flags]
	public enum VKDebugUtilsMessageSeverityFlagBigsEXT : uint {
		Verbose = 0x00000001,
		Info = 0x00000010,
		Warning = 0x00000100,
		Error = 0x00001000
	}

	[Flags]
	public enum VKDebugUtilsMessageTypeFlagBitsEXT : uint {
		General = 0x00000001,
		Validation = 0x00000002,
		Performance = 0x00000004
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKDebugUtilsLabelEXT {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly IntPtr labelName;
		[NativeType("const char*")]
		public IntPtr LabelName { get => labelName; init => labelName = value; }
		private readonly Vector4 color;
		public Vector4 Color { get => color; init => color = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKDebugUtilsMessengerCallbackDataEXT {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly uint flags;
		public uint Flags { get => flags; init => flags = value; }
		private readonly IntPtr messageIdName;
		[NativeType("const char*")]
		public IntPtr MessageIdName { get => messageIdName; init => messageIdName = value; }
		private readonly int messageIdNumber;
		public int MessageIdNumber { get => messageIdNumber; init => messageIdNumber = value; }
		private readonly IntPtr message;
		[NativeType("const char*")]
		public IntPtr Message { get => message; init => message = value; }
		private readonly uint queueLabelCount;
		public uint QueueLabelCount { get => queueLabelCount; init => queueLabelCount = value; }
		private readonly IntPtr queueLabels;
		[NativeType("const VkDebugUtilsLabelEXT*")]
		public IntPtr QueueLabels { get => queueLabels; init => queueLabels = value; }
		private readonly uint cmdBufLabelCount;
		public uint CmdBufLabelCount { get => cmdBufLabelCount; init => cmdBufLabelCount = value; }
		private readonly IntPtr cmdBufLabels;
		[NativeType("const VkDebugUtilsLabelEXT*")]
		public IntPtr CmdBufLabels { get => cmdBufLabels; init => cmdBufLabels = value; }
		private readonly uint objectCount;
		public uint ObjectCount { get => objectCount; init => objectCount = value; }
		private readonly IntPtr objects;
		[NativeType("const VkDebugUtilsObjectNameInfoEXT*")]
		public IntPtr Objects { get => objects; init => objects = value; }

	}
	
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKDebugUtilsObjectNameInfoEXT {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VKObjectType objectType;
		public VKObjectType ObjectType { get => objectType; init => objectType = value; }
		private readonly ulong objectHandle;
		public ulong ObjectHandle { get => objectHandle; init => objectHandle = value; }
		private readonly IntPtr objectName;
		[NativeType("const char*")]
		public IntPtr ObjectName { get => objectName; init => objectName = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKDebugUtilsObjectTagInfoEXT {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VKObjectType objectType;
		public VKObjectType ObjectType { get => objectType; init => objectType = value; }
		private readonly ulong objectHandle;
		public ulong ObjectHandle { get => objectHandle; init => objectHandle = value; }
		private readonly ulong tagName;
		public ulong TagName { get => tagName; init => tagName = value; }
		private readonly nuint tagSize;
		public nuint TagSize { get => tagSize; init => tagSize = value; }
		private readonly IntPtr tag;
		public IntPtr Tag { get => tag; init => tag = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKDebugUtilsMessengerCreateInfoEXT {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly uint flags;
		public uint Flags { get => flags; init => flags = value; }
		private readonly VKDebugUtilsMessageSeverityFlagBigsEXT messageSeverity;
		public VKDebugUtilsMessageSeverityFlagBigsEXT MessageSeverity { get => messageSeverity; init => messageSeverity = value; }
		private readonly VKDebugUtilsMessageTypeFlagBitsEXT messageType;
		public VKDebugUtilsMessageTypeFlagBitsEXT MessageType { get => messageType; init => messageType = value; }
		private readonly IntPtr userCallback;
		public VKDebugUtilsMessengerCallbackEXT UserCallback { init => userCallback = Marshal.GetFunctionPointerForDelegate(value); }
		private readonly IntPtr userData;
		public IntPtr UserData { get => userData; init => userData = value; }

	}

	public delegate VKBool32 VKDebugUtilsMessengerCallbackEXT(VKDebugUtilsMessageSeverityFlagBigsEXT messageSeverity, VKDebugUtilsMessageTypeFlagBitsEXT messageTypes, in VKDebugUtilsMessengerCallbackDataEXT callbackData, IntPtr userData);

	public unsafe class EXTDebugUtilsInstanceFunctions {

		[NativeType("void vkCmdBeginDebugUtilsLabelEXT(VkCommandBuffer cmdbuf, const VkDebugUtilsLabelEXT* pLabelInfo)")]
		public delegate* unmanaged<IntPtr, in VKDebugUtilsLabelEXT, void> vkCmdBeginDebugUtilsLabelEXT;
		[NativeType("void vkCmdEndDebugUtilsLabelEXT(VkCommandBuffer cmdbuf)")]
		public delegate* unmanaged<IntPtr, void> vkCmdEndDebugUtilsLabelEXT;
		[NativeType("void vkCmdInsertDebugUtilsLabelEXT(VkCommandBuffer cmdbuf, const VkDebugUtilsLabelEXT* pLabelInfo)")]
		public delegate* unmanaged<IntPtr, in VKDebugUtilsLabelEXT, void> vkCmdInsertDebugUtilsLabelEXT;
		[NativeType("VkResult vkCreateDebugUtilsMessengerEXT(VkInstance instance, const VkDebugUtilsMessengerCreateInfoEXT* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDebugUtilsMessengerEXT* pMessenger)")]
		public delegate* unmanaged<IntPtr, in VKDebugUtilsMessengerCreateInfoEXT, VKAllocationCallbacks*, out ulong, VKResult> vkCreateDebugUtilsMessengerEXT;
		[NativeType("void vkDestroyDebugUtilsMessengerEXT(VkInstance instance, VkDebugUtilsMessengerEXT messenger, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<IntPtr, ulong, VKAllocationCallbacks*, void> vkDestroyDebugUtilsMessengerEXT;
		[NativeType("void vkQueueBeginDebugUtilsLabelEXT(VkQueue queue, const VkDebugUtilsLabelEXT* pLabelInfo)")]
		public delegate* unmanaged<IntPtr, in VKDebugUtilsLabelEXT, void> vkQueueBeginDebugUtilsLabelEXT;
		[NativeType("void vkQueueEndDebugUtilsLabelEXT(VkQueue queue)")]
		public delegate* unmanaged<IntPtr, void> vkQueueEndDebugUtilsLabelEXT;
		[NativeType("void vkQueueInsertDebugUtilsLabelEXT(VkQueue queue, const VkDebugUtilsLabelEXT* pLabelInfo)")]
		public delegate* unmanaged<IntPtr, in VKDebugUtilsLabelEXT, void> vkQueueInsertDebugUtilsLabelEXT;
		[NativeType("VkResult vkSetDebugUtilsObjectNameEXT(VkDevice device, const VKDebugUtilsObjectNameInfoEXT* pNameInfo)")]
		public delegate* unmanaged<IntPtr, in VKDebugUtilsObjectNameInfoEXT, VKResult> vkSetDebugUtilsObjectNameEXT;
		[NativeType("VkResult vkSetDebugUtilsObjectTagEXT(VkDevice device, const VkDebugUtilsObjectTagInfoEXT* pTagInfo)")]
		public delegate* unmanaged<IntPtr, in VKDebugUtilsObjectTagInfoEXT, VKResult> vkSetDebugUtilsObjectTagEXT;
		[NativeType("void vkSubmitDebugUtilsMessageEXT(VkInstance instance, VkDebugUtilsMessageSeverityFlagsEXT messageSeverity, VkDebugUtilsMessageTypeFlags messageTypes, const VkDebugUtilsMessangerCallbackDataEXT* pCallbackData)")]
		public delegate* unmanaged<IntPtr, VKDebugUtilsMessageSeverityFlagBigsEXT, VKDebugUtilsMessageTypeFlagBitsEXT, in VKDebugUtilsMessengerCallbackDataEXT, void> vkSubmitDebugUtilsMessageEXT;

	}

	public class EXTDebugUtils {

		public const string ExtensionName = "VK_EXT_debug_utils";

	}

	public class VKDebugUtilsMessengerEXT : IVKInstanceObject, IVKAllocatedObject, IDisposable {

		public VKInstance Instance { get; }

		public VulkanAllocationCallbacks? Allocator { get; }

		public ulong Messenger { get; }

		public VKDebugUtilsMessengerEXT(VKInstance instance, ulong messenger, VulkanAllocationCallbacks? allocator = null) {
			Instance = instance;
			Allocator = allocator;
			Messenger = messenger;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				Instance.EXTDebugUtilsFunctions!.vkDestroyDebugUtilsMessengerEXT(Instance, Messenger, Allocator);
			}
		}

		public static implicit operator ulong(VKDebugUtilsMessengerEXT messenger) => messenger.Messenger; 

	}
}

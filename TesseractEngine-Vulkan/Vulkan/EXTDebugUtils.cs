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
		[MarshalAs(UnmanagedType.LPUTF8Str)]
		private readonly string labelName;
		public string LabelName { get => labelName; init => labelName = value; }
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
		[MarshalAs(UnmanagedType.LPUTF8Str)]
		private readonly string messageIdName;
		public string MessageIdName { get => messageIdName; init => messageIdName = value; }
		private readonly int messageIdNumber;
		public int MessageIdNumber { get => messageIdNumber; init => messageIdNumber = value; }
		[MarshalAs(UnmanagedType.LPUTF8Str)]
		private readonly string message;
		public string Message { get => message; init => message = value; }
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
		[MarshalAs(UnmanagedType.LPUTF8Str)]
		private readonly string objectName;
		public string ObjectName { get => objectName; init => objectName = value; }

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
		[MarshalAs(UnmanagedType.FunctionPtr)]
		private readonly VKDebugUtilsMessengerCallbackEXT userCallback;
		public VKDebugUtilsMessengerCallbackEXT UserCallback { get => userCallback; init => userCallback = value; }
		private readonly IntPtr userData;
		public IntPtr UserData { get => userData; init => userData = value; }

	}

	public delegate VKBool32 VKDebugUtilsMessengerCallbackEXT(VKDebugUtilsMessageSeverityFlagBigsEXT messageSeverity, VKDebugUtilsMessageTypeFlagBitsEXT messageTypes, in VKDebugUtilsMessengerCallbackDataEXT callbackData, IntPtr userData);

#nullable disable
	public class EXTDebugUtilsInstanceFunctions {

		public delegate void PFN_vkCmdBeginDebugUtilsLabelEXT(IntPtr commandBuffer, in VKDebugUtilsLabelEXT labelInfo);
		public delegate void PFN_vkCmdEndDebugUtilsLabelEXT(IntPtr commandBuffer);
		public delegate void PFN_vkCmdInsertDebugUtilsLabelEXT(IntPtr commandBuffer, in VKDebugUtilsLabelEXT labelInfo);
		public delegate VKResult PFN_vkCreateDebugUtilsMessengerEXT(IntPtr instance, in VKDebugUtilsMessengerCreateInfoEXT createInfo, IntPtr allocator, out ulong messenger);
		public delegate void PFN_vkDestroyDebugUtilsMessengerEXT(IntPtr instance, ulong messenger, IntPtr allocator);
		public delegate void PFN_vkQueueBeginDebugUtilsLabelEXT(IntPtr queue, in VKDebugUtilsLabelEXT labelInfo);
		public delegate void PFN_vkQueueEndDebugUtilsLabelEXT(IntPtr queue);
		public delegate void PFN_vkQueueInsertDebugUtilsLabelEXT(IntPtr queue, in VKDebugUtilsLabelEXT labelInfo);
		public delegate VKResult PFN_vkSetDebugUtilsObjectNameEXT(IntPtr device, in VKDebugUtilsObjectNameInfoEXT nameInfo);
		public delegate VKResult PFN_vkSetDebugUtilsObjectTagEXT(IntPtr device, in VKDebugUtilsObjectTagInfoEXT tagInfo);
		public delegate void PFN_vkSubmitDebugUtilsMessageEXT(IntPtr instance, VKDebugUtilsMessageSeverityFlagBigsEXT messageSeverity, VKDebugUtilsMessageTypeFlagBitsEXT messageTypes, in VKDebugUtilsMessengerCallbackDataEXT callbackData);

		public PFN_vkCmdBeginDebugUtilsLabelEXT vkCmdBeginDebugUtilsLabelEXT;
		public PFN_vkCmdEndDebugUtilsLabelEXT vkCmdEndDebugUtilsLabelEXT;
		public PFN_vkCmdInsertDebugUtilsLabelEXT vkCmdInsertDebugUtilsLabelEXT;
		public PFN_vkCreateDebugUtilsMessengerEXT vkCreateDebugUtilsMessengerEXT;
		public PFN_vkDestroyDebugUtilsMessengerEXT vkDestroyDebugUtilsMessengerEXT;
		public PFN_vkQueueBeginDebugUtilsLabelEXT vkQueueBeginDebugUtilsLabelEXT;
		public PFN_vkQueueEndDebugUtilsLabelEXT vkQueueEndDebugUtilsLabelEXT;
		public PFN_vkQueueInsertDebugUtilsLabelEXT vkQueueInsertDebugUtilsLabelEXT;
		public PFN_vkSetDebugUtilsObjectNameEXT vkSetDebugUtilsObjectNameEXT;
		public PFN_vkSetDebugUtilsObjectTagEXT vkSetDebugUtilsObjectTagEXT;
		public PFN_vkSubmitDebugUtilsMessageEXT vkSubmitDebugUtilsMessageEXT;

	}
#nullable restore

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
			Instance.EXTDebugUtilsFunctions!.vkDestroyDebugUtilsMessengerEXT(Instance, Messenger, Allocator);
		}

		public static implicit operator ulong(VKDebugUtilsMessengerEXT messenger) => messenger.Messenger; 

	}
}

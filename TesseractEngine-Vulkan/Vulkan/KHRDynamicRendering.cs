using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	[Flags]
	public enum VKRenderingFlagBits : uint {
		ContentsSecondaryCommandBuffers = 0x1,
		Suspending = 0x2,
		Resuming = 0x4
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRenderingAttachmentInfoKHR {

		public VKStructureType Type;

		public IntPtr Next;

		[NativeType("VkImageView")]
		public ulong ImageView;

		public VKImageLayout ImageLayout;

		public VKResolveModeFlagBits ResolveMode;

		[NativeType("VkImageView")]
		public ulong ResolveImageView;

		public VKImageLayout ResolveImageLayout;

		public VKAttachmentLoadOp LoadOp;

		public VKAttachmentStoreOp StoreOp;

		public VKClearValue ClearValue;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRenderingInfoKHR {

		public VKStructureType Type;

		public IntPtr Next;

		public VKRenderingFlagBits Flags;

		public VKRect2D RenderArea;

		public uint LayerCount;

		public uint ViewMask;

		public uint ColorAttachmentCount;

		[NativeType("const VkRenderingAttachmentInfoKHR*")]
		public IntPtr ColorAttachments;

		[NativeType("const VkRenderingAttachmentInfoKHR*")]
		public IntPtr DepthAttachment;

		[NativeType("const VkRenderingAttachmentInfoKHR*")]
		public IntPtr StencilAttachment;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCommandBufferInheritanceRenderingInfo {

		public VKStructureType Type;

		public IntPtr Next;

		public VKRenderingFlagBits Flags;

		public uint ViewMask;

		public uint ColorAttachmentCount;

		[NativeType("const VkFormat*")]
		public IntPtr ColorAttachmentFormats;

		public VKFormat DepthAttachmentFormat;

		public VKFormat StencilAttachmentFormat;

		public VKSampleCountFlagBits RasterizationSamples;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineRenderingCreateInfo {

		public VKStructureType Type;

		public IntPtr Next;

		public uint ViewMask;

		public uint ColorAttachmentCount;

		[NativeType("const VkFormat*")]
		public IntPtr ColorAttachmentFormats;

		public VKFormat DepthAttachmentFormat;

		public VKFormat StencilAttachmentFormat;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceDynamicRenderingFeatures {

		public VKStructureType Type;

		public IntPtr Next;

		public VKBool32 DynamicRendering;

	}

#nullable disable
	public class KHRDynamicRenderingDeviceFunctions {

		public delegate void PFN_vkCmdBeginRenderingKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, in VKRenderingInfoKHR renderingInfo);
		public delegate void PFN_vkCmdEndRenderingKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer);

		public PFN_vkCmdBeginRenderingKHR vkCmdBeginRenderingKHR;
		public PFN_vkCmdEndRenderingKHR vkCmdEndRenderingKHR;

	}
#nullable restore

	public static class KHRDynamicRendering {

		public const string ExtensionName = "VK_KHR_dynamic_rendering";

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKDescriptorUpdateTemplate : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.DescriptorUpdateTemplate;

		public VKDevice Device { get; }

		public VulkanAllocationCallbacks? Allocator { get; }

		[NativeType("VkDescriptorUpdateTemplate")]
		public ulong DescriptorUpdateTemplate { get; }

		public ulong PrimitiveHandle => DescriptorUpdateTemplate;

		public VKDescriptorUpdateTemplate(VKDevice device, ulong descriptorUpdateTemplate, VulkanAllocationCallbacks? allocator = null) {
			Device = device;
			DescriptorUpdateTemplate = descriptorUpdateTemplate;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				if (Device.VK11Functions) Device.VK11Functions!.vkDestroyDescriptorUpdateTemplate(Device, DescriptorUpdateTemplate, Allocator);
				else Device.KHRDescriptorUpdateTemplate!.vkDestroyDescriptorUpdateTemplateKHR(Device, DescriptorUpdateTemplate, Allocator);
			}
		}

		public void UpdateDescriptorSet(VKDescriptorSet descriptorSet, IntPtr data) {
			unsafe {
				if (Device.VK11Functions) Device.VK11Functions!.vkUpdateDescriptorSetWithTemplate(Device, descriptorSet, DescriptorUpdateTemplate, data);
				else Device.KHRDescriptorUpdateTemplate!.vkUpdateDescriptorSetWithTemplateKHR(Device, descriptorSet, DescriptorUpdateTemplate, data);
			}
		}

		public void UpdateDescriptorSet<T>(VKDescriptorSet descriptorSet, params T[] data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					UpdateDescriptorSet(descriptorSet, (IntPtr)pData);
				}
			}
		}

		public void UpdateDescriptorSet<T>(VKDescriptorSet descriptorSet, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					UpdateDescriptorSet(descriptorSet, (IntPtr)pData);
				}
			}
		}

		public static implicit operator ulong(VKDescriptorUpdateTemplate? template) => template != null ? template.PrimitiveHandle : 0;

	}
}

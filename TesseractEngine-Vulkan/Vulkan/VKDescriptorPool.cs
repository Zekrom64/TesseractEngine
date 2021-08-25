using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKDescriptorPool : IVKDeviceObject, IVKAllocatedObject, IDisposable {

		public VKDevice Device { get; }

		[NativeType("VkDescriptorPool")]
		public ulong DescriptorPool { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public VKDescriptorPool(VKDevice device, ulong descriptorPool, VulkanAllocationCallbacks allocator) {
			Device = device;
			DescriptorPool = descriptorPool;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyDescriptorPool(Device, DescriptorPool, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Reset(VKDescriptorPoolResetFlagBits flags = 0) => VK.CheckError(Device.VK10Functions.vkResetDescriptorPool(Device, DescriptorPool, flags));

		public VKDescriptorSet[] Allocate(in VKDescriptorSetAllocateInfo allocateInfo) {
			Span<ulong> descriptorSets = stackalloc ulong[(int)allocateInfo.DescriptorSetCount];
			unsafe {
				fixed(ulong* pDescriptorSets = descriptorSets) {
					VK.CheckError(Device.VK10Functions.vkAllocateDescriptorSets(Device, allocateInfo, (IntPtr)pDescriptorSets), "Failed to allocate descriptor set(s)");
				}
			}
			VKDescriptorSet[] sets = new VKDescriptorSet[descriptorSets.Length];
			for (int i = 0; i < sets.Length; i++) sets[i] = new VKDescriptorSet(this, descriptorSets[i]);
			return sets;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Finalization done in bulk during freeing")]
		public void Free(params VKDescriptorSet[] sets) {
			Span<ulong> descriptorSets = stackalloc ulong[sets.Length];
			for (int i = 0; i < sets.Length; i++) {
				var set = sets[i];
				GC.SuppressFinalize(set);
				descriptorSets[i] = set;
			}
			unsafe {
				fixed(ulong* pDescriptorSets = descriptorSets) {
					VK.CheckError(Device.VK10Functions.vkFreeDescriptorSets(Device, DescriptorPool, (uint)sets.Length, (IntPtr)pDescriptorSets), "Failed to free descriptor sets");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKDescriptorPool pool) => pool != null ? pool.DescriptorPool : 0;

	}

}

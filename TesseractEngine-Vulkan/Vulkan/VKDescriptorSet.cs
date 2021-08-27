using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKDescriptorSet : IVKDeviceObject, IDisposable {

		public VKDescriptorPool DescriptorPool { get; }

		public VKDevice Device => DescriptorPool.Device;

		[NativeType("VkDescriptorSet")]
		public ulong DescriptorSet { get; }

		public VKDescriptorSet(VKDescriptorPool descriptorPool, ulong descriptorSet) {
			DescriptorPool = descriptorPool;
			DescriptorSet = descriptorSet;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			ulong set = DescriptorSet;
			unsafe {
				VK.CheckError(Device.VK10Functions.vkFreeDescriptorSets(Device, DescriptorPool, 1, (IntPtr)(&set)), "Failed to free descriptor set");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKDescriptorSet descriptorSet) => descriptorSet != null ? descriptorSet.DescriptorSet : 0;

	}

}

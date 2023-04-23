using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Vulkan.Native;

namespace Tesseract.Vulkan {
	public class VK11 {

		public static readonly uint ApiVersion = VK10.MakeApiVersion(0, 1, 1, 0);

		public const int MaxDeviceGroupSize = 32;
		public const int LUIDSize = 8;
		public const uint QueueFamilyExternal = (~0U) - 1;

		public VK VK { get; }
		public VK11Functions Functions { get; } = new();

		public VK11(VK vk) {
			VK = vk;
			Library.LoadFunctions(name => vk.InstanceGetProcAddr(IntPtr.Zero, name), Functions);
		}

		public uint EnumerateInstanceVersion() {
			unsafe {
				VK.CheckError(Functions.vkEnumerateInstanceVersion(out uint apiVersion));
				return apiVersion;
			}
		}

	}
}

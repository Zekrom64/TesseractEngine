using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Vulkan {
	public class VK11 {

		public static readonly int ApiVersion = VK10.MakeVersion(1, 1, 0);

		public const int MaxDeviceGroupSize = 32;
		public const int LUIDSize = 8;
		public const uint QueueFamilyExternal = (~0U) - 1;

	}
}

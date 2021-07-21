using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Vulkan {

	public interface IVKAllocatedObject {

		public VulkanAllocationCallbacks Allocator { get; }

	}

	public interface IVK10DeviceObject {

		public VK10Device Device { get; }

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Vulkan {

	public interface IVKLoader {

		/// <summary>
		/// Gets the address of a Vulkan API function given its name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IntPtr GetVKProcAddress(string name);

	}

	public interface IVKAllocatedObject {

		public VulkanAllocationCallbacks Allocator { get; }

	}

	public interface IVKInstanceObject {

		public VKInstance Instance { get; }

	}

	public interface IVKDeviceObject {

		public VKDevice Device { get; }

	}

}

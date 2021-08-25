using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;

namespace Tesseract.Vulkan.Graphics {

	/// <summary>
	/// Vulkan-specific window attributes.
	/// </summary>
	public static class VKWindowAttributes {

		/// <summary>
		/// Attribute specifying if Vulkan graphics will be used with a window.
		/// </summary>
		public static readonly IWindowAttribute<bool> VulkanWindow = new OpaqueWindowAttribute<bool>();

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Services;

namespace Tesseract.Vulkan.Graphics {
	
	public static class VKServices {

		public static readonly IService<IVKSurfaceProvider> SurfaceProvider = new OpaqueService<IVKSurfaceProvider>();

	}

}

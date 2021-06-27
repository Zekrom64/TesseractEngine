using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;

namespace Tesseract.Core.Services {
	public static class GraphicsServices {

		public static readonly IService<IWindowSystem> WindowSystem = new OpaqueService<IWindowSystem>();

		public static readonly IService<IGammaRampObject> GammaRampObject = new OpaqueService<IGammaRampObject>();

		public static readonly IService<IImageIO> ImageIO = new OpaqueService<IImageIO>();

		public static readonly IService<IImageProcessing> ImageProcessing = new OpaqueService<IImageProcessing>();
		
	}
}

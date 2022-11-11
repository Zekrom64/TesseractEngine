using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Vulkan {
	public class VK12 {

		public static readonly uint ApiVersion = VK10.MakeApiVersion(0, 1, 2, 0);

		public const int MaxDriverNameSize = 256;
		public const int MaxDriverInfoSize = 256;

	}
}

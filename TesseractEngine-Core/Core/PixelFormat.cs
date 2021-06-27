using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core {

	public enum PixelComponent {
		None,
		Red,
		Green,
		Blue,
		Alpha,
		Depth,
		Stencil
	}

	public struct PixelChannel {

		public PixelComponent Component { get; init; }
		public int Offset { get; init; }
		public int Size { get; init; }

	}

	public class PixelFormat {



	}
}

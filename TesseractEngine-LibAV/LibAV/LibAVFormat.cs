using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.LibAV.Native;

namespace Tesseract.LibAV {
	
	public static class LibAVFormat {

		public static LibrarySpec LibrarySpec { get; } = new() { Name = "avformat" };
		public static Library Library { get; } = LibraryManager.Load(LibrarySpec);
		public static LibAVFormatFunctions Functions { get; } = new();

		static LibAVFormat() {
			Library.LoadFunctions(Functions);
		}

		public const int SeekSize = 0x10000;
		public const int SeekForce = 0x20000;

	}


}

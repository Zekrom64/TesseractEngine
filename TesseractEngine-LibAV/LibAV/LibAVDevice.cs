using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.LibAV.Native;

namespace Tesseract.LibAV {
	
	public static class LibAVDevice {

		public static readonly LibrarySpec LibrarySpec = new() { Name = "avdevice" };
		public static Library Library { get; } = LibraryManager.Load(LibrarySpec);
		public static LibAVDeviceFunctions Functions { get; } = new();

		static LibAVDevice() {
			Library.LoadFunctions(Functions);
		}

		public static int Version => (int)Functions.avdevice_version();

		public static string Configuration => MemoryUtil.GetASCII(Functions.avdevice_configuration());

		public static string License => MemoryUtil.GetASCII(Functions.avdevice_license());


		public static void RegisterAll() => Functions.avdevice_register_all();

	}

}

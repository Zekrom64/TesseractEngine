using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV.Native {

	public class LibAVDeviceFunctions {

		public delegate uint PFN_avdevice_version();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_avdevice_configuration();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_avdevice_license();

		public PFN_avdevice_version avdevice_version;
		public PFN_avdevice_configuration avdevice_configuration;
		public PFN_avdevice_license avdevice_license;

		public delegate void PFN_avdevice_register_all();

		public PFN_avdevice_register_all avdevice_register_all;

	}

}

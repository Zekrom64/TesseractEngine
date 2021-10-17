using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.OpenAL {

	public enum ALCSampleType : int {
		Byte = 0x1400,
		UnsignedByte = 0x1401,
		Short = 0x1402,
		UnsignedShort = 0x1403,
		Int = 0x1404,
		UnsignedInt = 0x1405,
		Float = 0x1406
	}

	public enum ALCChannelConfiguration : int {
		Mono = 0x1500,
		Stereo = 0x1501,
		Quad = 0x1503,
		_5Point1 = 0x1504,
		_6Point1 = 0x1505,
		_7Point1 = 0x1506
	}
	
	public class SOFTLoopbackFunctions {

		[return: NativeType("ALCdevice*")]
		public delegate IntPtr PFN_alcLoopbackOpenDeviceSOFT([MarshalAs(UnmanagedType.LPStr)] string deviceName);
		public delegate byte PFN_alcIsRenderFormatSupportedSOFT([NativeType("ALCdevice*")] IntPtr device, int freq, ALCChannelConfiguration channels, ALCSampleType type);
		public delegate void PFN_alcRenderSamplesSOFT([NativeType("ALCdevice*")] IntPtr device, IntPtr buffer, int samples);

		public PFN_alcLoopbackOpenDeviceSOFT alcLoopbackOpenDeviceSOFT;
		public PFN_alcIsRenderFormatSupportedSOFT alcIsRenderFormatSupportedSOFT;
		public PFN_alcRenderSamplesSOFT alcRenderSamplesSOFT;

	}

	public class SOFTLoopback {

		public const string ExtensionName = "ALC_SOFT_loopback";

		public SOFTLoopbackFunctions Functions { get; } = new();

		public SOFTLoopback() {
			Library.LoadFunctions(ALC.GetProcAddress, Functions);
		}

	}
}

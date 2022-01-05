using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.OpenAL {
	
	public enum ALChannelConfigurationSOFT : int {
		Mono = 0x1500,
		Stereo = 0x1501,
		Rear = 0x1502,
		Quad = 0x1503,
		_5Point1 = 0x1504,
		_6Point1 = 0x1505,
		_7Point1 = 0x1506
	}

	public enum ALSampleTypeSOFT : int {
		Byte = 0x1400,
		UnsignedByte = 0x1401,
		Short = 0x1402,
		UnsignedShort = 0x1403,
		Int = 0x1404,
		UnsignedInt = 0x1405,
		Float = 0x1406,
		Double = 0x1407,
		Byte3 = 0x1408,
		UnsignedByte3 = 0x1409
	}

	public enum ALStorageFormatSOFT : int {
		Mono8 = 0x1100,
		Mono16 = 0x1101,
		Mono32f = 0x10010,
		Stereo8 = 0x1102,
		Stereo16 = 0x1103,
		Stereo32f = 0x10011,
		Quad8 = 0x1204,
		Quad16 = 0x1205,
		Quad32f = 0x1206,
		Rear8 = 0x1207,
		Rear16 = 0x1208,
		Rear32f = 0x1209,
		_5Point1_8 = 0x120A,
		_5Point1_16 = 0x120B,
		_5Point1_32f = 0x120C,
		_6Point1_8 = 0x120D,
		_6Point1_16 = 0x120E,
		_6Point1_32f = 0x120F,
		_7Point1_8 = 0x1210,
		_7Point1_16 = 0x1211,
		_7Point1_32f = 0x1212,
	}

#nullable disable
	public class SOFTBufferSamplesFunctions {

		public delegate void PFN_alBufferSamplesSOFT(uint buffer, uint sampleRate, ALStorageFormatSOFT internalFormat, int samples, ALChannelConfigurationSOFT channels, ALSampleTypeSOFT type, IntPtr data);
		public delegate void PFN_alBufferSubSamplesSOFT(uint buffer, int offset, int samples, ALChannelConfigurationSOFT channels, ALSampleTypeSOFT type, IntPtr data);
		public delegate void PFN_alGetBufferSamplesSOFT(uint buffer, int offset, int samples, ALChannelConfigurationSOFT channels, ALSampleTypeSOFT type, IntPtr data);
		public delegate byte PFN_alIsBufferFormatSupportedSOFT(ALStorageFormatSOFT format);

		public PFN_alBufferSamplesSOFT alBufferSamplesSOFT;
		public PFN_alBufferSubSamplesSOFT alBufferSubSamplesSOFT;
		public PFN_alGetBufferSamplesSOFT alGetBufferSamplesSOFT;
		public PFN_alIsBufferFormatSupportedSOFT alIsBufferFormatSupportedSOFT;

	}
#nullable restore

	public class SOFTBufferSamples {

		public const string ExtensionName = "AL_SOFT_buffer_samples";

		public SOFTBufferSamplesFunctions Functions { get; } = new();

		public SOFTBufferSamples(AL al) {
			Library.LoadFunctions(al.GetProcAddress, Functions);
		}

		public bool IsBufferFormatSupported(ALStorageFormatSOFT format) => Functions.alIsBufferFormatSupportedSOFT(format) != 0;

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.OpenAL {

	using ALenum = Int32;
	using ALint = Int32;

	public enum ALSourceAttrib : ALenum {
		Relative = 0x202,
		ConeInnerAngle = 0x1001,
		ConeOuterAngle = 0x1002,
		Pitch = 0x1003,
		Position = 0x1004,
		Direction = 0x1005,
		Velocity = 0x1006,
		Looping = 0x1007,
		Buffer = 0x1009,
		Gain = 0x100A,
		MinGain = 0x100D,
		MaxGain = 0x100E,
		State = 0x100F,
		BuffersQueued = 0x1015,
		BuffersProcessed = 0x1016,
		ReferenceDistance = 0x1020,
		RolloffFactor = 0x1021,
		ConeOuterGain = 0x1022,
		MaxDistance = 0x1023,
		SecOffset = 0x1024,
		SampleOffset = 0x1025,
		ByteOffset = 0x1026,
		Type = 0x1027,

		DirectFilter = 0x20005,
		AuxiliarySendFilter = 0x20006,
		AirAbsorptionFactor = 0x20007,
		RoomRolloffFactor = 0x20008,
		ConeOuterGainHF = 0x20009,
		DirectFilterGainHFAuto = 0x2000A,
		AuxiliarySendFilterGainAuto = 0x2000B,
		AuxiliarySendFilterGainHFAuto = 0x2000C
	}

	public enum ALListenerAttrib : ALenum {
		Position = 0x1004,
		Velocity = 0x1006,
		Gain = 0x100A,
		Orientation = 0x100F,

		MetersPerUnit = 0x20004
	}

	public enum ALSourceState : ALint {
		Initial = 0x1011,
		Playing = 0x1012,
		Paused = 0x1013,
		Stopped = 0x1014
	}

	public enum ALSourceType : ALint {
		Static = 0x1028,
		Streaming = 0x1029,
		Undetermined = 0x1030
	}

	public enum ALFormat : ALint {
		Mono8 = 0x1100,
		Mono16 = 0x1101,
		Stereo8 = 0x1102,
		Stereo16 = 0x1103,

		// AL_EXT_float32
		MonoFloat32 = 0x10010,
		StereoFloat32 = 0x10011,

		// AL_EXT_MCFORMATS
		Quad8 = 0x1204,
		Quad16 = 0x1205,
		Quad32 = 0x1206,
		Rear8 = 0x1207,
		Rear16 = 0x1208,
		Rear32 = 0x1209,
		_5_1Channel8 = 0x120A,
		_5_1Channel16 = 0x120B,
		_5_1Channel32 = 0x120C,
		_6_1Channel8 = 0x120D,
		_6_1Channel16 = 0x120E,
		_6_1Channel32 = 0x120F,
		_7_1Channel8 = 0x1210,
		_7_1Channel16 = 0x1211,
		_7_1Channel32 = 0x1212,

		// AL_EXT_MULAW_MCFORMATS
		MonoMulaw = 0x10014,
		StereoMulaw = 0x10015,
		QuadMulaw = 0x10021,
		RearMulaw = 0x10022,
		_5_1ChannelMulaw = 0x10023,
		_6_1ChannelMulaw = 0x10024,
		_7_1ChannelMulaw = 0x10025
	}

	public enum ALBufferAttrib : ALenum {
		Frequency = 0x2001,
		Bits = 0x2002,
		Channels = 0x2003,
		Size = 0x2004,

		InternalFormatSOFT = 0x2008,
		ByteLengthSOFT = 0x2009,
		SampleLengthSOFT = 0x200A,
		SecLengthSOFT = 0x200B
	}

	public enum ALBufferState : ALint {
		Unused = 0x2010,
		Pending = 0x2011,
		Processed = 0x2012
	}

	public enum ALError : ALenum {
		NoError = 0,
		InvalidName = 0xA001,
		InvalidEnum = 0xA002,
		InvalidValue = 0xA003,
		InvalidOperation = 0xA004,
		OutOfMemory = 0xA005
	}

	public enum ALGetString : ALenum {
		Vendor = 0xB001,
		Version = 0xB002,
		Renderer = 0xB003,
		Extensions = 0xB004
	}

	public enum ALGetFloat : ALenum {
		DopplerFactor = 0xC000,
		DopplerVelocity = 0xC001,
		SpeedOfSound = 0xC003
	}

	public enum ALDistanceModel : ALenum {
		InverseDistance = 0xD001,
		InverseDistanceClamped = 0xD002,
		LinearDistance = 0xD003,
		LinearDistanceClamped = 0xD004,
		ExponentDistance = 0xD005,
		ExponentDistanceClamped = 0xD006
	}

	public enum ALGetInteger : ALenum {
		DistanceModel = 0xD000
	}

}

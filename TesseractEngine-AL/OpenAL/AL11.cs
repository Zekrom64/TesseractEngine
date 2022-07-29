using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;

namespace Tesseract.OpenAL {

	using ALboolean = Byte;
	using ALchar = Byte;
	using ALbyte = SByte;
	using ALubyte = Byte;
	using ALshort = Int16;
	using ALushort = UInt16;
	using ALint = Int32;
	using ALuint = UInt32;
	using ALsizei = Int32;
	using ALenum = Int32;
	using ALfloat = Single;
	using ALdouble = Double;

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

#nullable disable
	public class AL11Functions {

		public delegate void PFN_alDopplerFactor(float value);
		public delegate void PFN_alDopplerVelocity(float value);
		public delegate void PFN_alSpeedOfSound(float value);
		public delegate void PFN_alDistanceModel(ALDistanceModel distanceModel);

		public PFN_alDopplerFactor alDopplerFactor;
		public PFN_alDopplerVelocity alDopplerVelocity;
		public PFN_alSpeedOfSound alSpeedOfSound;
		public PFN_alDistanceModel alDistanceModel;

		public delegate void PFN_alEnable(ALenum capability);
		public delegate void PFN_alDisable(ALenum capability);
		public delegate ALboolean PFN_alIsEnabled(ALenum capability);

		public PFN_alEnable alEnable;
		public PFN_alDisable alDisable;
		public PFN_alIsEnabled alIsEnabled;

		[return: NativeType("const ALchar*")]
		public delegate IntPtr PFN_alGetString(ALGetString param);
		public delegate void PFN_alGetBooleanv(ALenum param, [NativeType("ALboolean*")] IntPtr values);
		public delegate void PFN_alGetIntegerv(ALGetInteger param, [NativeType("ALint*")] IntPtr values);
		public delegate void PFN_alGetFloatv(ALGetFloat param, [NativeType("ALfloat*")] IntPtr values);
		public delegate void PFN_alGetDoublev(ALGetFloat param, [NativeType("ALdouble*")] IntPtr values);
		public delegate ALboolean PFN_alGetBoolean(ALenum param);
		public delegate ALint PFN_alGetInteger(ALGetInteger param);
		public delegate ALfloat PFN_alGetFloat(ALGetFloat param);
		public delegate ALdouble PFN_alGetDouble(ALGetFloat param);

		public PFN_alGetString alGetString;
		public PFN_alGetBooleanv alGetBooleanv;
		public PFN_alGetIntegerv alGetIntegerv;
		public PFN_alGetFloatv alGetFloatv;
		public PFN_alGetDoublev alGetDoublev;
		public PFN_alGetBoolean alGetBoolean;
		public PFN_alGetInteger alGetInteger;
		public PFN_alGetFloat alGetFloat;
		public PFN_alGetDouble alGetDouble;

		public delegate ALError PFN_alGetError();
		public delegate ALboolean PFN_alIsExtensionPresent([MarshalAs(UnmanagedType.LPStr)] string extname);
		public delegate IntPtr PFN_alGetProcAddress([MarshalAs(UnmanagedType.LPStr)] string fname);
		public delegate ALenum PFN_alGetEnumValue([MarshalAs(UnmanagedType.LPStr)] string ename);

		public PFN_alGetError alGetError;
		public PFN_alIsExtensionPresent alIsExtensionPresent;
		public PFN_alGetProcAddress alGetProcAddress;
		public PFN_alGetEnumValue alGetEnumValue;

		public delegate void PFN_alListenerf(ALListenerAttrib param, ALfloat value);
		public delegate void PFN_alListener3f(ALListenerAttrib param, ALfloat value1, ALfloat value2, ALfloat value3);
		public delegate void PFN_alListenerfv(ALListenerAttrib param, [NativeType("const ALfloat*")] IntPtr values);
		public delegate void PFN_alListeneri(ALListenerAttrib param, ALint value);
		public delegate void PFN_alListener3i(ALListenerAttrib param, ALint value1, ALint value2, ALint value3);
		public delegate void PFN_alListeneriv(ALListenerAttrib param, [NativeType("const ALint*")] IntPtr values);

		public PFN_alListenerf alListenerf;
		public PFN_alListener3f alListener3f;
		public PFN_alListenerfv alListenerfv;
		public PFN_alListeneri alListeneri;
		public PFN_alListener3i alListener3i;
		public PFN_alListeneriv alListeneriv;

		public delegate void PFN_alGetListenerf(ALListenerAttrib param, out ALfloat value);
		public delegate void PFN_alGetListener3f(ALListenerAttrib param, out ALfloat value1, out ALfloat value2, out ALfloat value3);
		public delegate void PFN_alGetListenerfv(ALListenerAttrib param, [NativeType("ALfloat*")] IntPtr values);
		public delegate void PFN_alGetListeneri(ALListenerAttrib param, out ALint value);
		public delegate void PFN_alGetListener3i(ALListenerAttrib param, out ALint value1, out ALint value2, out ALint value3);
		public delegate void PFN_alGetListeneriv(ALListenerAttrib param, [NativeType("ALint*")] IntPtr values);

		public PFN_alGetListenerf alGetListenerf;
		public PFN_alGetListener3f alGetListener3f;
		public PFN_alGetListenerfv alGetListenerfv;
		public PFN_alGetListeneri alGetListeneri;
		public PFN_alGetListener3i alGetListener3i;
		public PFN_alGetListeneriv alGetListeneriv;

		public delegate void PFN_alGenSources(ALsizei n, [NativeType("ALuint*")] IntPtr sources);
		public delegate void PFN_alDeleteSources(ALsizei n, [NativeType("const ALuint*")] IntPtr sources);
		public delegate ALboolean PFN_alIsSource(ALuint source);

		public PFN_alGenSources alGenSources;
		public PFN_alDeleteSources alDeleteSources;
		public PFN_alIsSource alIsSource;

		public delegate void PFN_alSourcef(ALuint source, ALSourceAttrib param, ALfloat value);
		public delegate void PFN_alSource3f(ALuint source, ALSourceAttrib param, ALfloat value1, ALfloat value2, ALfloat value3);
		public delegate void PFN_alSourcefv(ALuint source, ALSourceAttrib param, [NativeType("const ALfloat*")] IntPtr values);
		public delegate void PFN_alSourcei(ALuint source, ALSourceAttrib param, ALint value);
		public delegate void PFN_alSource3i(ALuint source, ALSourceAttrib param, ALint value1, ALint value2, ALint value3);
		public delegate void PFN_alSourceiv(ALuint source, ALSourceAttrib param, [NativeType("const ALint*")] IntPtr values);

		public PFN_alSourcef alSourcef;
		public PFN_alSource3f alSource3f;
		public PFN_alSourcefv alSourcefv;
		public PFN_alSourcei alSourcei;
		public PFN_alSource3i alSource3i;
		public PFN_alSourceiv alSourceiv;

		public delegate void PFN_alGetSourcef(ALuint source, ALSourceAttrib param, out ALfloat value);
		public delegate void PFN_alGetSource3f(ALuint source, ALSourceAttrib param, out ALfloat value1, out ALfloat value2, out ALfloat value3);
		public delegate void PFN_alGetSourcefv(ALuint source, ALSourceAttrib param, [NativeType("ALfloat*")] IntPtr values);
		public delegate void PFN_alGetSourcei(ALuint source, ALSourceAttrib param, out ALint value);
		public delegate void PFN_alGetSource3i(ALuint source, ALSourceAttrib param, out ALint value1, out ALint value2, out ALint value3);
		public delegate void PFN_alGetSourceiv(ALuint source, ALSourceAttrib param, [NativeType("ALint*")] IntPtr values);

		public PFN_alGetSourcef alGetSourcef;
		public PFN_alGetSource3f alGetSource3f;
		public PFN_alGetSourcefv alGetSourcefv;
		public PFN_alGetSourcei alGetSourcei;
		public PFN_alGetSource3i alGetSource3i;
		public PFN_alGetSourceiv alGetSourceiv;

		public delegate void PFN_alSourcePlayv(ALsizei n, [NativeType("const ALuint*")] IntPtr sources);
		public delegate void PFN_alSourceStopv(ALsizei n, [NativeType("const ALuint*")] IntPtr sources);
		public delegate void PFN_alSourceRewindv(ALsizei n, [NativeType("const ALuint*")] IntPtr sources);
		public delegate void PFN_alSourcePausev(ALsizei n, [NativeType("const ALuint*")] IntPtr sources);

		public PFN_alSourcePlayv alSourcePlayv;
		public PFN_alSourceStopv alSourceStopv;
		public PFN_alSourceRewindv alSourceRewindv;
		public PFN_alSourcePausev alSourcePausev;

		public delegate void PFN_alSourcePlay(ALuint source);
		public delegate void PFN_alSourceStop(ALuint source);
		public delegate void PFN_alSourceRewind(ALuint source);
		public delegate void PFN_alSourcePause(ALuint source);

		public PFN_alSourcePlay alSourcePlay;
		public PFN_alSourceStop alSourceStop;
		public PFN_alSourceRewind alSourceRewind;
		public PFN_alSourcePause alSourcePause;

		public delegate void PFN_alSourceQueueBuffers(ALuint source, ALsizei nb, [NativeType("const ALuint*")] IntPtr buffers);
		public delegate void PFN_alSourceUnqueueBuffers(ALuint source, ALsizei nb, [NativeType("ALuint*")] IntPtr buffers);

		public PFN_alSourceQueueBuffers alSourceQueueBuffers;
		public PFN_alSourceUnqueueBuffers alSourceUnqueueBuffers;

		public delegate void PFN_alGenBuffers(ALsizei n, [NativeType("ALuint*")] IntPtr buffers);
		public delegate void PFN_alDeleteBuffers(ALsizei n, [NativeType("const ALuint*")] IntPtr buffers);
		public delegate ALboolean PFN_alIsBuffer(ALuint buffer);

		public PFN_alGenBuffers alGenBuffers;
		public PFN_alDeleteBuffers alDeleteBuffers;
		public PFN_alIsBuffer alIsBuffer;

		public delegate void PFN_alBufferData(ALuint buffer, ALFormat format, IntPtr data, ALsizei size, ALsizei freq);

		public PFN_alBufferData alBufferData;

		public delegate void PFN_alBufferf(ALuint buffer, ALBufferAttrib param, ALfloat value);
		public delegate void PFN_alBuffer3f(ALuint buffer, ALBufferAttrib param, ALfloat value1, ALfloat value2, ALfloat value3);
		public delegate void PFN_alBufferfv(ALuint buffer, ALBufferAttrib param, [NativeType("const ALfloat*")] IntPtr values);
		public delegate void PFN_alBufferi(ALuint buffer, ALBufferAttrib param, ALint value);
		public delegate void PFN_alBuffer3i(ALuint buffer, ALBufferAttrib param, ALint value1, ALint value2, ALint value3);
		public delegate void PFN_alBufferiv(ALuint buffer, ALBufferAttrib param, [NativeType("const ALint*")] IntPtr values);

		public PFN_alBufferf alBufferf;
		public PFN_alBuffer3f alBuffer3f;
		public PFN_alBufferfv alBufferfv;
		public PFN_alBufferi alBufferi;
		public PFN_alBuffer3i alBuffer3i;
		public PFN_alBufferiv alBufferiv;

		public delegate void PFN_alGetBufferf(ALuint buffer, ALBufferAttrib param, out ALfloat value);
		public delegate void PFN_alGetBuffer3f(ALuint buffer, ALBufferAttrib param, out ALfloat value1, out ALfloat value2, out ALfloat value3);
		public delegate void PFN_alGetBufferfv(ALuint buffer, ALBufferAttrib param, [NativeType("ALfloat*")] IntPtr values);
		public delegate void PFN_alGetBufferi(ALuint buffer, ALBufferAttrib param, out ALint value);
		public delegate void PFN_alGetBuffer3i(ALuint buffer, ALBufferAttrib param, out ALint value1, out ALint value2, out ALint value3);
		public delegate void PFN_alGetBufferiv(ALuint buffer, ALBufferAttrib param, [NativeType("ALint*")] IntPtr values);

		public PFN_alGetBufferf alGetBufferf;
		public PFN_alGetBuffer3f alGetBuffer3f;
		public PFN_alGetBufferfv alGetBufferfv;
		public PFN_alGetBufferi alGetBufferi;
		public PFN_alGetBuffer3i alGetBuffer3i;
		public PFN_alGetBufferiv alGetBufferiv;

	}
#nullable restore

	public class AL11 {

		public AL AL { get; }
		public AL11Functions Functions { get; } = new();

		public AL11(AL al) {
			AL = al;
			Library.LoadFunctions(al.GetProcAddress, Functions);
		}

		public float DopplerFactor {
			get => Functions.alGetFloat(ALGetFloat.DopplerFactor);
			set => Functions.alDopplerFactor(value);
		}

		public float DopplerVelocity {
			get => Functions.alGetFloat(ALGetFloat.DopplerVelocity);
			set => Functions.alDopplerVelocity(value);
		}

		public float SpeedOfSound {
			get => Functions.alGetFloat(ALGetFloat.SpeedOfSound);
			set => Functions.alSpeedOfSound(value);
		}

		public ALDistanceModel DistanceModel {
			get => (ALDistanceModel)Functions.alGetInteger(ALGetInteger.DistanceModel);
			set => Functions.alDistanceModel(value);
		}

		public void Enable(ALenum capability) => Functions.alEnable(capability);

		public void Disable(ALenum capability) => Functions.alDisable(capability);

		public bool IsEnabled(ALenum capability) => Functions.alIsEnabled(capability) != 0;

		public string? GetString(ALGetString param) => MemoryUtil.GetASCII(Functions.alGetString(param));

		public ALError GetError() => Functions.alGetError();

		public bool IsExtensionPresent(string extname) => Functions.alIsExtensionPresent(extname) != 0;

		public IntPtr GetProcAddress(string fname) => Functions.alGetProcAddress(fname);

		public ALenum GetEnumValue(string ename) => Functions.alGetEnumValue(ename);

		public Vector3 ListenerPosition {
			get {
				Functions.alGetListener3f(ALListenerAttrib.Position, out float x, out float y, out float z);
				return new Vector3(x, y, z);
			}
			set => Functions.alListener3f(ALListenerAttrib.Position, value.X, value.Y, value.Z);
		}

		public Vector3 ListenerVelocity {
			get {
				Functions.alGetListener3f(ALListenerAttrib.Velocity, out float x, out float y, out float z);
				return new Vector3(x, y, z);
			}
			set => Functions.alListener3f(ALListenerAttrib.Velocity, value.X, value.Y, value.Z);
		}

		public float ListenerGain {
			get {
				Functions.alGetListenerf(ALListenerAttrib.Gain, out float value);
				return value;
			}
			set => Functions.alListenerf(ALListenerAttrib.Gain, value);
		}

		public (Vector3, Vector3) ListenerOrientation {
			get {
				Span<float> values = stackalloc float[6];
				unsafe {
					fixed (float* pValues = values) {
						Functions.alGetListenerfv(ALListenerAttrib.Orientation, (IntPtr)pValues);
					}
				}
				return (
					new Vector3().ReadFrom(values),
					new Vector3().ReadFrom(values, 3)
				);
			}
			set {
				Span<float> values = stackalloc float[6];
				value.Item1.CopyTo(values);
				value.Item2.CopyTo(values, 3);
				unsafe {
					fixed(float* pValues = values) {
						Functions.alListenerfv(ALListenerAttrib.Orientation, (IntPtr)pValues);
					}
				}
			}
		}

		private static void SourceOpVector(IEnumerable<ALSource> sources, Action<ALsizei, IntPtr> fn) {
			Span<ALuint> srcs = stackalloc ALuint[sources.Count()];
			int i = 0;
			foreach (ALSource source in sources) srcs[i++] = source.Source;
			unsafe {
				fixed(ALuint* pSrcs = srcs) {
					fn(srcs.Length, (IntPtr)pSrcs);
				}
			}
		}

		public void Play(params ALSource[] sources) => SourceOpVector(sources, new(Functions.alSourcePlayv));

		public void Pause(params ALSource[] sources) => SourceOpVector(sources, new(Functions.alSourcePausev));

		public void Stop(params ALSource[] sources) => SourceOpVector(sources, new(Functions.alSourceStopv));

		public void Rewind(params ALSource[] sources) => SourceOpVector(sources, new(Functions.alSourceRewindv));

		public void Play(IEnumerable<ALSource> sources) => SourceOpVector(sources, new(Functions.alSourcePlayv));

		public void Pause(IEnumerable<ALSource> sources) => SourceOpVector(sources, new(Functions.alSourcePausev));

		public void Stop(IEnumerable<ALSource> sources) => SourceOpVector(sources, new(Functions.alSourceStopv));

		public void Rewind(IEnumerable<ALSource> sources) => SourceOpVector(sources, new(Functions.alSourceRewindv));

		// AL_EXT_EFX

		public float ListenerMetersPerUnit {
			get {
				Functions.alGetListenerf(ALListenerAttrib.MetersPerUnit, out float value);
				return value;
			}
			set => Functions.alListenerf(ALListenerAttrib.MetersPerUnit, value);
		}

	}

	public class ALSource : IDisposable, IALObject {

		public AL11 AL11 { get; }
		public AL AL => AL11.AL;
		public ALuint Source { get; }

		public bool Relative {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.Relative, out int val);
				return val != 0;
			}
			set => AL11.Functions.alSourcei(Source, ALSourceAttrib.Relative, value ? 1 : 0);
		}

		public ALSourceType Type {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.Type, out int val);
				return (ALSourceType)val;
			}
		}

		public bool Looping {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.Looping, out int val);
				return val != 0;
			}
			set => AL11.Functions.alSourcei(Source, ALSourceAttrib.Looping, value ? 1 : 0);
		}

		public int BuffersQueued {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.BuffersQueued, out int val);
				return val;
			}
		}

		public int BuffersProcessed {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.BuffersProcessed, out int val);
				return val;
			}
		}

		public float MinGain {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.MinGain, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.MinGain, value);
		}

		public float MaxGain {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.MaxGain, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.MaxGain, value);
		}

		public float ReferenceDistance {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.ReferenceDistance, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.ReferenceDistance, value);
		}

		public float RolloffFactor {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.RolloffFactor, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.RolloffFactor, value);
		}

		public float MaxDistance {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.MaxDistance, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.MaxDistance, value);
		}

		public float Pitch {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.Pitch, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.Pitch, value);
		}

		public Vector3 Direction {
			get {
				AL11.Functions.alGetSource3f(Source, ALSourceAttrib.Direction, out float x, out float y, out float z);
				return new Vector3(x, y, z);
			}
			set => AL11.Functions.alSource3f(Source, ALSourceAttrib.Direction, value.X, value.Y, value.Z);
		}

		public float ConeInnerAngle {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.ConeInnerAngle, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.ConeInnerAngle, value);
		}

		public float ConeOuterAngle {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.ConeOuterAngle, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.ConeOuterAngle, value);
		}

		public float ConeOuterGain {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.ConeOuterGain, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.ConeOuterGain, value);
		}

		public float SecOffset {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.SecOffset, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.SecOffset, value);
		}

		public int SampleOffset {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.SampleOffset, out int val);
				return val;
			}
			set => AL11.Functions.alSourcei(Source, ALSourceAttrib.SampleOffset, value);
		}

		public int ByteOffset {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.ByteOffset, out int val);
				return val;
			}
			set => AL11.Functions.alSourcei(Source, ALSourceAttrib.ByteOffset, value);
		}

		public ALSource(AL11 al11, ALuint source) {
			AL11 = al11;
			Source = source;
		}

		public ALSource(AL11 al11) {
			AL11 = al11;
			ALuint source = 0;
			unsafe {
				AL11.Functions.alGenSources(1, (IntPtr)(&source));
				Source = source;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			ALuint source = Source;
			unsafe {
				AL11.Functions.alDeleteSources(1, (IntPtr)(&source));
			}
		}

		public void QueueBuffers(params ALBuffer[] buffers) {
			Span<ALuint> bufs = stackalloc ALuint[buffers.Length];
			for (int i = 0; i < buffers.Length; i++) bufs[i] = buffers[i].Buffer;
			unsafe {
				fixed (ALuint* pBufs = bufs) {
					AL11.Functions.alSourceQueueBuffers(Source, buffers.Length, (IntPtr)pBufs);
				}
			}
		}

		public void QueueBuffers(IEnumerable<ALBuffer> buffers) {
			Span<ALuint> bufs = stackalloc ALuint[buffers.Count()];
			int i = 0;
			foreach (ALBuffer buffer in buffers) bufs[i++] = buffer.Buffer;
			unsafe {
				fixed (ALuint* pBufs = bufs) {
					AL11.Functions.alSourceQueueBuffers(Source, bufs.Length, (IntPtr)pBufs);
				}
			}
		}

		public ALBuffer?[] UnqueueBuffers() {
			int n = BuffersProcessed;
			Span<ALuint> bufs = stackalloc ALuint[n];
			unsafe {
				fixed (ALuint* pBufs = bufs) {
					AL11.Functions.alSourceUnqueueBuffers(Source, n, (IntPtr)pBufs);
				}
			}
			ALBuffer?[] buffers = new ALBuffer?[n];
			for (int i = 0; i < n; i++) {
				ALuint buf = bufs[i];
				buffers[i] = buf != 0 ? new ALBuffer(AL11, buf) : null;
			}
			return buffers;
		}

		public void Play() => AL11.Functions.alSourcePlay(Source);

		public void Pause() => AL11.Functions.alSourcePause(Source);

		public void Stop() => AL11.Functions.alSourceStop(Source);

		public void Rewind() => AL11.Functions.alSourceRewind(Source);

		// AL_EXT_EFX

		public ALFilter? DirectFilter {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.DirectFilter, out ALint filter);
				return filter != 0 ? new ALFilter(AL, (uint)filter) : null;
			}
			set => AL11.Functions.alSourcei(Source, ALSourceAttrib.DirectFilter, value != null ? (int)value.Filter : 0);
		}

		public void SetAuxiliarySendFilter(int index, ALAuxiliaryEffectSlot fxslot, ALFilter filter) =>
			AL11.Functions.alSource3i(Source, ALSourceAttrib.AuxiliarySendFilter, fxslot != null ? (int)fxslot.AuxiliaryEffectSlot : 0, index, filter != null ? (int)filter.Filter : 0);

		public float AirAbsorptionFactor {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.AirAbsorptionFactor, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.AirAbsorptionFactor, value);
		}

		public float RoomRolloffFactor {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.RoomRolloffFactor, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.RoomRolloffFactor, value);
		}

		public float ConeOuterGainHF {
			get {
				AL11.Functions.alGetSourcef(Source, ALSourceAttrib.ConeOuterGainHF, out float val);
				return val;
			}
			set => AL11.Functions.alSourcef(Source, ALSourceAttrib.ConeOuterGainHF, value);
		}

		public bool DirectFilterGainHFAuto {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.DirectFilterGainHFAuto, out int val);
				return val != 0;
			}
			set => AL11.Functions.alSourcei(Source, ALSourceAttrib.DirectFilterGainHFAuto, value ? 1 : 0);
		}

		public bool AuxiliarySendFilterGainAuto {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.AuxiliarySendFilterGainAuto, out int val);
				return val != 0;
			}
			set => AL11.Functions.alSourcei(Source, ALSourceAttrib.AuxiliarySendFilterGainAuto, value ? 1 : 0);
		}

		public bool AuxiliarySendFilterGainHFAuto {
			get {
				AL11.Functions.alGetSourcei(Source, ALSourceAttrib.AuxiliarySendFilterGainHFAuto, out int val);
				return val != 0;
			}
			set => AL11.Functions.alSourcei(Source, ALSourceAttrib.AuxiliarySendFilterGainHFAuto, value ? 1 : 0);
		}

	}

	public class ALBuffer : IDisposable, IALObject {

		public AL11 AL11 { get; }
		public AL AL => AL11.AL;
		public ALuint Buffer { get; }

		public int Frequency {
			get {
				AL11.Functions.alGetBufferi(Buffer, ALBufferAttrib.Frequency, out int value);
				return value;
			}
		}

		public int Size {
			get {
				AL11.Functions.alGetBufferi(Buffer, ALBufferAttrib.Size, out int value);
				return value;
			}
		}

		public int Bits {
			get {
				AL11.Functions.alGetBufferi(Buffer, ALBufferAttrib.Bits, out int value);
				return value;
			}
		}

		public int Channels {
			get {
				AL11.Functions.alGetBufferi(Buffer, ALBufferAttrib.Channels, out int value);
				return value;
			}
		}

		public ALBuffer(AL11 al11, ALuint buffer) {
			AL11 = al11;
			Buffer = buffer;
		}

		public ALBuffer(AL11 al11) {
			AL11 = al11;
			ALuint buffer = 0;
			unsafe {
				AL11.Functions.alGenBuffers(1, (IntPtr)(&buffer));
			}
			Buffer = buffer;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			ALuint buffer = Buffer;
			unsafe {
				AL11.Functions.alDeleteBuffers(1, (IntPtr)(&buffer));
			}
		}

		public void BufferData<T>(ALFormat format, in ReadOnlySpan<T> data, ALsizei frequency) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					AL11.Functions.alBufferData(Buffer, format, (IntPtr)pData, data.Length * sizeof(T), frequency);
				}
			}
		}

		public void BufferData<T>(ALFormat format, IConstPointer<T> data, ALsizei size, ALsizei frequency) where T : unmanaged =>
			AL11.Functions.alBufferData(Buffer, format, data.Ptr, size, frequency);

#nullable disable
		// AL_SOFT_buffer_samples

		public void BufferSamplesSOFT<T>(ALuint sampleRate, ALStorageFormatSOFT internalFormat, ALsizei samples, ALChannelConfigurationSOFT channels, ALSampleTypeSOFT type, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					AL.SOFTBufferSamples.Functions.alBufferSamplesSOFT(Buffer, sampleRate, internalFormat, samples, channels, type, (IntPtr)pData);
				}
			}
		}

		public void BufferSamplesSOFT<T>(ALuint sampleRate, ALStorageFormatSOFT internalFormat, ALsizei samples, ALChannelConfigurationSOFT channels, ALSampleTypeSOFT type, IConstPointer<T> data) where T : unmanaged =>
					AL.SOFTBufferSamples.Functions.alBufferSamplesSOFT(Buffer, sampleRate, internalFormat, samples, channels, type, data.Ptr);

		public void BufferSubSamplesSOFT<T>(ALsizei offset, ALsizei samples, ALChannelConfigurationSOFT channels, ALSampleTypeSOFT type, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					AL.SOFTBufferSamples.Functions.alBufferSubSamplesSOFT(Buffer, offset, samples, channels, type, (IntPtr)pData);
				}
			}
		}

		public void BufferSubSamplesSOFT<T>(ALsizei offset, ALsizei samples, ALChannelConfigurationSOFT channels, ALSampleTypeSOFT type, IConstPointer<T> data) where T : unmanaged =>
			AL.SOFTBufferSamples.Functions.alBufferSubSamplesSOFT(Buffer, offset, samples, channels, type, data.Ptr);

		public void GetBufferSamplesSOFT<T>(ALsizei offset, ALsizei samples, ALChannelConfigurationSOFT channels, ALSampleTypeSOFT type, Span<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					AL.SOFTBufferSamples.Functions.alGetBufferSamplesSOFT(Buffer, offset, samples, channels, type, (IntPtr)pData);
				}
			}
		}

		public void GetBufferSamplesSOFT<T>(ALsizei offset, ALsizei samples, ALChannelConfigurationSOFT channels, ALSampleTypeSOFT type, IPointer<T> data) where T : unmanaged =>
			AL.SOFTBufferSamples.Functions.alGetBufferSamplesSOFT(Buffer, offset, samples, channels, type, data.Ptr);
#nullable restore

	}
}

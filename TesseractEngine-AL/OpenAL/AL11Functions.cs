using System;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenAL {

	using ALboolean = Byte;
	using ALint = Int32;
	using ALuint = UInt32;
	using ALsizei = Int32;
	using ALenum = Int32;
	using ALfloat = Single;
	using ALdouble = Double;

#nullable disable
	public unsafe class AL11Functions {

		public delegate* unmanaged<float, void> alDopplerFactor;
		public delegate* unmanaged<float, void> alDopplerVelocity;
		public delegate* unmanaged<float, void> alSpeedOfSound;
		public delegate* unmanaged<ALDistanceModel, void> alDistanceModel;

		public delegate* unmanaged<ALenum, void> alEnable;
		public delegate* unmanaged<ALenum, void> alDisable;
		public delegate* unmanaged<ALenum, ALboolean> alIsEnabled;

		public delegate* unmanaged<ALGetString, byte*> alGetString;
		public delegate* unmanaged<ALenum, ALboolean*, void> alGetBooleanv;
		public delegate* unmanaged<ALGetInteger, ALint*, void> alGetIntegerv;
		public delegate* unmanaged<ALGetFloat, ALfloat*, void> alGetFloatv;
		public delegate* unmanaged<ALGetFloat, ALdouble*, void> alGetDoublev;
		public delegate* unmanaged<ALenum, ALboolean> alGetBoolean;
		public delegate* unmanaged<ALGetInteger, ALint> alGetInteger;
		public delegate* unmanaged<ALGetFloat, ALfloat> alGetFloat;
		public delegate* unmanaged<ALGetFloat, ALdouble> alGetDouble;

		public delegate* unmanaged<ALError> alGetError;
		public delegate* unmanaged<byte*, ALboolean> alIsExtensionPresent;
		public delegate* unmanaged<byte*, nint> alGetProcAddress;
		public delegate* unmanaged<byte*, ALenum> alGetEnumValue;


		public delegate* unmanaged<ALListenerAttrib, ALfloat, void> alListenerf;
		public delegate* unmanaged<ALListenerAttrib, ALfloat, ALfloat, ALfloat, void> alListener3f;
		public delegate* unmanaged<ALListenerAttrib, ALfloat*, void> alListenerfv;
		public delegate* unmanaged<ALListenerAttrib, ALint, void> alListeneri;
		public delegate* unmanaged<ALListenerAttrib, ALint, ALint, ALint, void> alListener3i;
		public delegate* unmanaged<ALListenerAttrib, ALint*, void> alListeneriv;

		public delegate* unmanaged<ALListenerAttrib, ref ALfloat, void> alGetListenerf;
		public delegate* unmanaged<ALListenerAttrib, ref ALfloat, ref ALfloat, ref ALfloat, void> alGetListener3f;
		public delegate* unmanaged<ALListenerAttrib, ALfloat*, void> alGetListenerfv;
		public delegate* unmanaged<ALListenerAttrib, ref ALint, void> alGetListeneri;
		public delegate* unmanaged<ALListenerAttrib, ref ALint, ref ALint, ref ALint, void> alGetListener3i;
		public delegate* unmanaged<ALListenerAttrib, ALint*, void> alGetListeneriv;


		public delegate* unmanaged<ALsizei, ALuint*, void> alGenSources;
		public delegate* unmanaged<ALsizei, ALuint*, void> alDeleteSources;
		public delegate* unmanaged<ALuint, ALboolean> alIsSource;

		public delegate* unmanaged<ALuint, ALSourceAttrib, ALfloat, void> alSourcef;
		public delegate* unmanaged<ALuint, ALSourceAttrib, ALfloat, ALfloat, ALfloat, void> alSource3f;
		public delegate* unmanaged<ALuint, ALSourceAttrib, ALfloat*, void> alSourcefv;
		public delegate* unmanaged<ALuint, ALSourceAttrib, ALint, void> alSourcei;
		public delegate* unmanaged<ALuint, ALSourceAttrib, ALint, ALint, ALint, void> alSource3i;
		public delegate* unmanaged<ALuint, ALSourceAttrib, ALint*, void> alSourceiv;

		public delegate* unmanaged<ALuint, ALSourceAttrib, ref ALfloat, void> alGetSourcef;
		public delegate* unmanaged<ALuint, ALSourceAttrib, ref ALfloat, ref ALfloat, ref ALfloat, void> alGetSource3f;
		public delegate* unmanaged<ALuint, ALSourceAttrib, ALfloat*, void> alGetSourcefv;
		public delegate* unmanaged<ALuint, ALSourceAttrib, ref ALint, void> alGetSourcei;
		public delegate* unmanaged<ALuint, ALSourceAttrib, ref ALint, ref ALint, ref ALint, void> alGetSource3i;
		public delegate* unmanaged<ALuint, ALSourceAttrib, ALint*, void> alGetSourceiv;

		public delegate* unmanaged<ALsizei, ALuint*, void> alSourcePlayv;
		public delegate* unmanaged<ALsizei, ALuint*, void> alSourceStopv;
		public delegate* unmanaged<ALsizei, ALuint*, void> alSourceRewindv;
		public delegate* unmanaged<ALsizei, ALuint*, void> alSourcePausev;

		public delegate* unmanaged<ALuint, void> alSourcePlay;
		public delegate* unmanaged<ALuint, void> alSourceStop;
		public delegate* unmanaged<ALuint, void> alSourceRewind;
		public delegate* unmanaged<ALuint, void> alSourcePause;

		public delegate* unmanaged<ALuint, ALsizei, ALuint*, void> alSourceQueueBuffers;
		public delegate* unmanaged<ALuint, ALsizei, ALuint*, void> alSourceUnqueueBuffers;


		public delegate* unmanaged<ALsizei, ALuint*, void> alGenBuffers;
		public delegate* unmanaged<ALsizei, ALuint*, void> alDeleteBuffers;
		public delegate* unmanaged<ALuint, ALboolean> alIsBuffer;

		public delegate* unmanaged<ALuint, ALFormat, nint, ALsizei, ALsizei, void> alBufferData;

		public delegate* unmanaged<ALuint, ALBufferAttrib, ALfloat, void> alBufferf;
		public delegate* unmanaged<ALuint, ALBufferAttrib, ALfloat, ALfloat, ALfloat, void> alBuffer3f;
		public delegate* unmanaged<ALuint, ALBufferAttrib, ALfloat*, void> alBufferfv;
		public delegate* unmanaged<ALuint, ALBufferAttrib, ALint, void> alBufferi;
		public delegate* unmanaged<ALuint, ALBufferAttrib, ALint, ALint, ALint, void> alBuffer3i;
		public delegate* unmanaged<ALuint, ALBufferAttrib, ALint*, void> alBufferiv;

		public delegate* unmanaged<ALuint, ALBufferAttrib, ref ALfloat, void> alGetBufferf;
		public delegate* unmanaged<ALuint, ALBufferAttrib, ref ALfloat, ref ALfloat, ref ALfloat, void> alGetBuffer3f;
		public delegate* unmanaged<ALuint, ALBufferAttrib, ALfloat*, void> alGetBufferfv;
		public delegate* unmanaged<ALuint, ALBufferAttrib, ref ALint, void> alGetBufferi;
		public delegate* unmanaged<ALuint, ALBufferAttrib, ref ALint, ref ALint, ref ALint, void> alGetBuffer3i;
		public delegate* unmanaged<ALuint, ALBufferAttrib, ALint*, void> alGetBufferiv;

	}
#nullable restore

}

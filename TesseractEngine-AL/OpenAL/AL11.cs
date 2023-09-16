using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;
using Tesseract.Core.Utilities;

namespace Tesseract.OpenAL {

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

	public class AL11 {

		public AL AL { get; }
		public AL11Functions Functions { get; } = new();

		public AL11(AL al) {
			AL = al;
			Library.LoadFunctions(al.GetProcAddress, Functions);
		}



		public ALfloat DopplerFactor {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetFloat(ALGetFloat.DopplerFactor);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					Functions.alDopplerFactor(value);
				}
			}
		}

		public ALfloat DopplerVelocity {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetFloat(ALGetFloat.DopplerVelocity);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					Functions.alDopplerVelocity(value);
				}
			}
		}

		public ALfloat SpeedOfSound {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetFloat(ALGetFloat.SpeedOfSound);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					Functions.alSpeedOfSound(value);
				}
			}
		}

		public ALDistanceModel DistanceModel {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (ALDistanceModel)GetInteger(ALGetInteger.DistanceModel);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					Functions.alDistanceModel(value);
				}
			}
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Enable(ALenum capability) {
			unsafe {
				Functions.alEnable(capability);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Disable(ALenum capability) {
			unsafe {
				Functions.alDisable(capability);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsEnabled(ALenum capability) {
			unsafe {
				return Functions.alIsEnabled(capability) != 0;
			}
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string? GetString(ALGetString param) {
			unsafe {
				return MemoryUtil.GetASCII((IntPtr)Functions.alGetString(param));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALint> GetIntegerv(ALGetInteger param, Span<ALint> values) {
			unsafe {
				fixed(ALint* pValues = values) {
					Functions.alGetIntegerv(param, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALfloat> GetFloatv(ALGetFloat param, Span<ALfloat> values) {
			unsafe {
				fixed (ALfloat* pValues = values) {
					Functions.alGetFloatv(param, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALdouble> GetDoublev(ALGetFloat param, Span<ALdouble> values) {
			unsafe {
				fixed (ALdouble* pValues = values) {
					Functions.alGetDoublev(param, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALint GetInteger(ALGetInteger param) {
			unsafe {
				return Functions.alGetInteger(param);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALfloat GetFloat(ALGetFloat param) {
			unsafe {
				return Functions.alGetFloat(param);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALdouble GetDouble(ALGetFloat param) {
			unsafe {
				return Functions.alGetDouble(param);
			}
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALError GetError() {
			unsafe {
				return Functions.alGetError();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsExtensionPresent(string extname) {
			unsafe {
				fixed (byte* pExtName = MemoryUtil.StackallocASCII(extname, stackalloc byte[256])) {
					return Functions.alIsExtensionPresent(pExtName) != 0;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr GetProcAddress(string fname) {
			unsafe {
				fixed (byte* pFname = MemoryUtil.StackallocASCII(fname, stackalloc byte[256])) {
					return Functions.alGetProcAddress(pFname);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALenum GetEnumValue(string ename) {
			unsafe {
				fixed (byte* pEname = MemoryUtil.StackallocASCII(ename, stackalloc byte[256])) {
					return Functions.alGetEnumValue(pEname);
				}
			}
		}




		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Listenerf(ALListenerAttrib attrib, ALfloat value) {
			unsafe {
				Functions.alListenerf(attrib, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Listener3f(ALListenerAttrib attrib, Vector3 value) {
			unsafe {
				Functions.alListener3f(attrib, value.X, value.Y, value.Z);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Listener3f(ALListenerAttrib attrib, ALfloat x, ALfloat y, ALfloat z) {
			unsafe {
				Functions.alListener3f(attrib, x, y, z);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Listenerfv(ALListenerAttrib attrib, ReadOnlySpan<ALfloat> values) {
			unsafe {
				fixed(ALfloat* pValues = values) {
					Functions.alListenerfv(attrib, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALfloat GetListenerf(ALListenerAttrib attrib) {
			unsafe {
				ALfloat result = default;
				Functions.alGetListenerf(attrib, ref result);
				return result;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 GetListener3f(ALListenerAttrib attrib) {
			unsafe {
				Vector3 result = default;
				Functions.alGetListener3f(attrib, ref result.X, ref result.Y, ref result.Z);
				return result;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALfloat> GetListenerfv(ALListenerAttrib attrib, Span<ALfloat> values) {
			unsafe {
				fixed(ALfloat* pValues = values) {
					Functions.alGetListenerfv(attrib, pValues);
				}
			}
			return values;
		}

		/// <summary>
		/// The orientation of the listener defined by two orthogonal 'forward' and 'up' vectors.
		/// </summary>
		public (Vector3 Forward, Vector3 Up) ListenerOrientation {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Span<float> values = stackalloc float[6];
				GetListenerfv(ALListenerAttrib.Orientation, values);
				return (
					new Vector3().ReadFrom(values),
					new Vector3().ReadFrom(values, 3)
				);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				Span<float> values = stackalloc float[6];
				value.Forward.CopyTo(values);
				value.Up.CopyTo(values, 3);
				Listenerfv(ALListenerAttrib.Orientation, values);
			}
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALuint GenSources() {
			unsafe {
				ALuint source;
				Functions.alGenSources(1, &source);
				return source;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALuint> GenSources(Span<ALuint> sources) {
			unsafe {
				fixed(ALuint* pSources = sources) {
					Functions.alGenSources(sources.Length, pSources);
				}
			}
			return sources;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALuint[] GenSources(int count) {
			ALuint[] sources = new ALuint[count];
			unsafe {
				fixed(ALuint* pSources = sources) {
					Functions.alGenSources(count, pSources);
				}
			}
			return sources;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSources(ALuint source) {
			unsafe {
				Functions.alDeleteSources(1, &source);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSources(in ReadOnlySpan<ALuint> sources) {
			unsafe {
				fixed(ALuint* pSources = sources) {
					Functions.alDeleteSources(sources.Length, pSources);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSource(ALuint id) {
			unsafe {
				return Functions.alIsSource(id) != 0;
			}
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Sourcef(ALuint source, ALSourceAttrib attrib, ALfloat value) {
			unsafe {
				Functions.alSourcef(source, attrib, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Source3f(ALuint source, ALSourceAttrib attrib, Vector3 value) {
			unsafe {
				Functions.alSource3f(source, attrib, value.X, value.Y, value.Z);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Source3f(ALuint source, ALSourceAttrib attrib, ALfloat x, ALfloat y, ALfloat z) {
			unsafe {
				Functions.alSource3f(source, attrib, x, y, z);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Sourcefv(ALuint source, ALSourceAttrib attrib, in ReadOnlySpan<ALfloat> value) {
			unsafe {
				fixed (ALfloat* pValue = value) {
					Functions.alSourcefv(source, attrib, pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Sourcei(ALuint source, ALSourceAttrib attrib, ALint value) {
			unsafe {
				Functions.alSourcei(source, attrib, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Source3i(ALuint source, ALSourceAttrib attrib, Vector3i value) {
			unsafe {
				Functions.alSource3i(source, attrib, value.X, value.Y, value.Z);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Sourceiv(ALuint source, ALSourceAttrib attrib, in ReadOnlySpan<ALint> value) {
			unsafe {
				fixed (ALint* pValue = value) {
					Functions.alSourceiv(source, attrib, pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALfloat GetSourcef(ALuint source, ALSourceAttrib attrib) {
			unsafe {
				ALfloat value = default;
				Functions.alGetSourcef(source, attrib, ref value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 GetSource3f(ALuint source, ALSourceAttrib attrib) {
			unsafe {
				Vector3 value = default;
				Functions.alGetSource3f(source, attrib, ref value.X, ref value.Y, ref value.Z);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALfloat> GetSourcefv(ALuint source, ALSourceAttrib attrib, Span<ALfloat> values) {
			unsafe {
				fixed(ALfloat* pValues = values) {
					Functions.alGetSourcefv(source, attrib, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALint GetSourcei(ALuint source, ALSourceAttrib attrib) {
			unsafe {
				ALint value = default;
				Functions.alGetSourcei(source, attrib, ref value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i GetSource3i(ALuint source, ALSourceAttrib attrib) {
			unsafe {
				Vector3i value = default;
				Functions.alGetSource3i(source, attrib, ref value.X, ref value.Y, ref value.Z);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALint> GetSourceiv(ALuint source, ALSourceAttrib attrib, Span<ALint> values) {
			unsafe {
				fixed (ALint* pValues = values) {
					Functions.alGetSourceiv(source, attrib, pValues);
				}
			}
			return values;
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourcePlay(ALuint source) {
			unsafe {
				Functions.alSourcePlay(source);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourcePlay(in ReadOnlySpan<ALuint> sources) {
			unsafe {
				fixed(ALuint* pSources = sources) {
					Functions.alSourcePlayv(sources.Length, pSources);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourceStop(ALuint source) {
			unsafe {
				Functions.alSourceStop(source);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourceStop(in ReadOnlySpan<ALuint> sources) {
			unsafe {
				fixed (ALuint* pSources = sources) {
					Functions.alSourceStopv(sources.Length, pSources);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourceRewind(ALuint source) {
			unsafe {
				Functions.alSourceRewind(source);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourceRewind(in ReadOnlySpan<ALuint> sources) {
			unsafe {
				fixed (ALuint* pSources = sources) {
					Functions.alSourceRewindv(sources.Length, pSources);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourcePause(ALuint source) {
			unsafe {
				Functions.alSourcePause(source);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourcePause(in ReadOnlySpan<ALuint> sources) {
			unsafe {
				fixed (ALuint* pSources = sources) {
					Functions.alSourcePausev(sources.Length, pSources);
				}
			}
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourceQueueBuffers(ALuint source, ALuint buffer) {
			unsafe {
				Functions.alSourceQueueBuffers(source, 1, &buffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SourceQueueBuffers(ALuint source, in ReadOnlySpan<ALuint> buffers) {
			unsafe {
				fixed(ALuint* pBuffers = buffers) {
					Functions.alSourceQueueBuffers(source, buffers.Length, pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALuint SourceUnqueueBuffers(ALuint source) {
			unsafe {
				ALuint buffer = default;
				Functions.alSourceUnqueueBuffers(source, 1, &buffer);
				return buffer;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALuint> SourceUnqueueBuffers(ALuint source, Span<ALuint> buffers) {
			unsafe {
				fixed(ALuint* pBuffers = buffers) {
					Functions.alSourceUnqueueBuffers(source, buffers.Length, pBuffers);
				}
			}
			return buffers;
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALuint GenBuffers() {
			unsafe {
				ALuint buffer;
				Functions.alGenBuffers(1, &buffer);
				return buffer;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALuint> GenBuffers(Span<ALuint> buffers) {
			unsafe {
				fixed(ALuint* pBuffers = buffers) {
					Functions.alGenBuffers(buffers.Length, pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ALuint[] GenBuffers(int count) {
			ALuint[] buffers = new ALuint[count];
			unsafe {
				fixed(ALuint* pBuffers = buffers) {
					Functions.alGenBuffers(count, pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(ALuint buffer) {
			unsafe {
				Functions.alDeleteBuffers(1, &buffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(in ReadOnlySpan<ALuint> buffers) {
			unsafe {
				fixed(ALuint* pBuffers = buffers) {
					Functions.alDeleteBuffers(buffers.Length, pBuffers);
				}
			}
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferData<T>(ALuint buffer, ALFormat format, in ReadOnlySpan<T> data, ALsizei freq) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					Functions.alBufferData(buffer, format, (nint)pData, data.Length * sizeof(T), freq);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferData(ALuint buffer, ALFormat format, IntPtr data, ALsizei size, ALsizei freq) {
			unsafe {
				Functions.alBufferData(buffer, format, data, size, freq);
			}
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Bufferf(ALuint buffer, ALBufferAttrib attrib, ALfloat value) {
			unsafe {
				Functions.alBufferf(buffer, attrib, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Buffer3f(ALuint buffer, ALBufferAttrib attrib, Vector3 value) {
			unsafe {
				Functions.alBuffer3f(buffer, attrib, value.X, value.Y, value.Z);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Buffer3f(ALuint buffer, ALBufferAttrib attrib, ALfloat x, ALfloat y, ALfloat z) {
			unsafe {
				Functions.alBuffer3f(buffer, attrib, x, y, z);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Bufferfv(ALuint buffer, ALBufferAttrib attrib, in ReadOnlySpan<ALfloat> values) {
			unsafe {
				fixed (ALfloat* pValues = values) {
					Functions.alBufferfv(buffer, attrib, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Bufferi(ALuint buffer, ALBufferAttrib attrib, ALint value) {
			unsafe {
				Functions.alBufferi(buffer, attrib, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Buffer3i(ALuint buffer, ALBufferAttrib attrib, Vector3i value) {
			unsafe {
				Functions.alBuffer3i(buffer, attrib, value.X, value.Y, value.Z);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Bufferiv(ALuint buffer, ALBufferAttrib attrib, in ReadOnlySpan<ALint> values) {
			unsafe {
				fixed (ALint* pValues = values) {
					Functions.alBufferiv(buffer, attrib, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetBufferf(ALuint buffer, ALBufferAttrib attrib) {
			unsafe {
				ALfloat value = default;
				Functions.alGetBufferf(buffer, attrib, ref value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 GetBuffer3f(ALuint buffer, ALBufferAttrib attrib) {
			unsafe {
				Vector3 value = default;
				Functions.alGetBuffer3f(buffer, attrib, ref value.X, ref value.Y, ref value.Z);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALfloat> GetBufferfv(ALuint buffer, ALBufferAttrib attrib, Span<ALfloat> values) {
			unsafe {
				fixed(ALfloat* pValues = values) {
					Functions.alGetBufferfv(buffer, attrib, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetBufferi(ALuint buffer, ALBufferAttrib attrib) {
			unsafe {
				ALint value = default;
				Functions.alGetBufferi(buffer, attrib, ref value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i GetBuffer3i(ALuint buffer, ALBufferAttrib attrib) {
			unsafe {
				Vector3i value = default;
				Functions.alGetBuffer3i(buffer, attrib, ref value.X, ref value.Y, ref value.Z);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<ALint> GetBufferiv(ALuint buffer, ALBufferAttrib attrib, Span<ALint> values) {
			unsafe {
				fixed (ALint* pValues = values) {
					Functions.alGetBufferiv(buffer, attrib, pValues);
				}
			}
			return values;
		}

	}
}

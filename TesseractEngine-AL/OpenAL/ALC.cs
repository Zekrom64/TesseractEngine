using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.OpenAL {

	using ALCboolean = Byte;
	using ALCchar = Byte;
	using ALCbyte = SByte;
	using ALCubyte = Byte;
	using ALCshort = Int16;
	using ALCushort = UInt16;
	using ALCint = Int32;
	using ALCuint = UInt32;
	using ALCsizei = Int32;
	using ALCenum = Int32;
	using ALCfloat = Single;
	using ALCdouble = Double;

	public enum ALCContextAttrib : ALCenum {
		Frequency = 0x1007,
		Refresh = 0x1008,
		Sync = 0x1009,
		MonoSources = 0x1010,
		StereoSources = 0x1011
	}

	public enum ALCError : ALCenum {
		NoError = 0,
		InvalidDevice = 0xA001,
		InvalidContext = 0xA002,
		InvalidEnum = 0xA003,
		InvalidValue = 0xA004,
		OutOfMemory = 0xA005
	}

	public enum ALCGetInteger : ALCenum {
		MajorVersion = 0x1000,
		MinorVersion = 0x1001,
		AttributesSize = 0x1002,
		AllAttributes = 0x1003,

		CaptureSamples = 0x312
	}

	public enum ALCGetString : ALCenum {
		DefaultDeviceSpecifier = 0x1004,
		DeviceSpecifier = 0x1005,
		Extensions = 0x1006,

		CaptureDeviceSpecifier = 0x310,
		CaptureDefaultDeviceSpecifier = 0x311,

		DefaultAllDevicesSpecifier = 0x1012,
		AllDevicesSpecifier = 0x1013
	}

	public class ALCFunctions {

		[return: NativeType("ALCcontext*")]
		public delegate IntPtr PFN_alcCreateContext([NativeType("ALCdevice*")] IntPtr device, [NativeType("const ALCint*")] IntPtr attrlist);
		public PFN_alcCreateContext alcCreateContext;

		public delegate ALCboolean PFN_alcMakeContextCurrent([NativeType("ALCcontext*")] IntPtr context);
		public delegate void PFN_alcProcessContext([NativeType("ALCcontext*")] IntPtr context);
		public delegate void PFN_alcSuspendContext([NativeType("ALCcontext*")] IntPtr context);
		public delegate void PFN_alcDestroyContext([NativeType("ALCcontext*")] IntPtr context);
		[return: NativeType("ALCcontext*")]
		public delegate IntPtr PFN_alcGetCurrentContext();
		[return: NativeType("ALCdevice*")]
		public delegate IntPtr PFN_alcGetContextsDevice([NativeType("ALCcontext*")] IntPtr context);

		public PFN_alcMakeContextCurrent alcMakeContextCurrent;
		public PFN_alcProcessContext alcProcessContext;
		public PFN_alcSuspendContext alcSuspendContext;
		public PFN_alcDestroyContext alcDestroyContext;
		public PFN_alcGetCurrentContext alcGetCurrentContext;
		public PFN_alcGetContextsDevice alcGetContextsDevice;

		[return: NativeType("ALCdevice*")]
		public delegate IntPtr PFN_alcOpenDevice([MarshalAs(UnmanagedType.LPStr)] string deviceName);
		public delegate ALCboolean PFN_alcCloseDevice([NativeType("ALCdevice*")] IntPtr device);

		public PFN_alcOpenDevice alcOpenDevice;
		public PFN_alcCloseDevice alcCloseDevice;

		public delegate ALCenum PFN_alcGetError([NativeType("ALCdevice*")] IntPtr device);
		public PFN_alcGetError alcGetError;

		public delegate ALCboolean PFN_alcIsExtensionPresent([NativeType("ALCdevice*")] IntPtr device, [MarshalAs(UnmanagedType.LPStr)] string extname);
		public delegate IntPtr PFN_alcGetProcAddress([NativeType("ALCdevice*")] IntPtr device, [MarshalAs(UnmanagedType.LPStr)] string funcname);
		public delegate ALCenum PFN_alcGetEnumValue([NativeType("ALCdevice*")] IntPtr device, [MarshalAs(UnmanagedType.LPStr)] string enumname);

		public PFN_alcIsExtensionPresent alcIsExtensionPresent;
		public PFN_alcGetProcAddress alcGetProcAddress;
		public PFN_alcGetEnumValue alcGetEnumValue;

		[return: NativeType("const ALCchar*")]
		public delegate IntPtr PFN_alcGetString([NativeType("ALCdevice*")] IntPtr device, ALCenum param);
		public delegate void PFN_alcGetIntegerv([NativeType("ALCdevice*")] IntPtr device, ALCenum param, ALCsizei size, [NativeType("ALCint*")] IntPtr values);

		public PFN_alcGetString alcGetString;
		public PFN_alcGetIntegerv alcGetIntegerv;

		[return: NativeType("ALCdevice*")]
		public delegate IntPtr PFN_alcCaptureOpenDevice([NativeType("const ALCchar*")] IntPtr devicename, ALCuint frequency, ALCenum format, ALCsizei buffersize);
		public delegate ALCboolean PFN_alcCaptureCloseDevice([NativeType("ALCdevice*")] IntPtr device);
		public delegate void PFN_alcCaptureStart([NativeType("ALCdevice*")] IntPtr device);
		public delegate void PFN_alcCaptureStop([NativeType("ALCdevice*")] IntPtr device);
		public delegate void PFN_alcCaptureSamples([NativeType("ALCdevice*")] IntPtr device, IntPtr buffer, ALCsizei samples);

		public PFN_alcCaptureOpenDevice alcCaptureOpenDevice;
		public PFN_alcCaptureCloseDevice alcCaptureCloseDevice;
		public PFN_alcCaptureStart alcCaptureStart;
		public PFN_alcCaptureStop alcCaptureStop;
		public PFN_alcCaptureSamples alcCaptureSamples;

	}

	public static class ALC {

		public static LibrarySpec LibrarySpec { get; } = new() { Name = "OpenAL32" };
		public static Library Library { get; } = LibraryManager.Load(LibrarySpec);
		public static ALCFunctions Functions { get; } = new();

		static ALC() {
			Library.LoadFunctions(Functions);
		}

	}

	public class ALCDevice : IDisposable {

		[NativeType("ALCdevice*")]
		public IntPtr Device { get; private set; }

		public bool IsCapture { get; }

		public ALCDevice([NativeType("ALCdevice*")] IntPtr device, bool isCapture) {
			Device = device;
			IsCapture = isCapture;
		}

		public ALCDevice(string name) {
			IntPtr device = ALC.Functions.alcOpenDevice(name);
			if (device == IntPtr.Zero) throw new ALException($"Failed to open ALC device \"{name}\": {(ALCError)ALC.Functions.alcGetError(IntPtr.Zero)}");
			Device = device;
			IsCapture = false;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Device != IntPtr.Zero) {
				if (IsCapture) ALC.Functions.alcCaptureCloseDevice(Device);
				else ALC.Functions.alcCloseDevice(Device);
				Device = IntPtr.Zero;
			}
		}

		public ALCContext CreateContext(in ReadOnlySpan<int> attrlist) {
			IntPtr pContext;
			if (attrlist[^1] != 0) {
				Span<int> attrlist2 = stackalloc int[attrlist.Length + 1];
				attrlist.CopyTo(attrlist2);
				attrlist2[^1] = 0;
				unsafe {
					fixed(int* pAttr = attrlist2) {
						pContext = ALC.Functions.alcCreateContext(Device, (IntPtr)pAttr);
					}
				}
			} else {
				unsafe {
					fixed (int* pAttr = attrlist) {
						pContext = ALC.Functions.alcCreateContext(Device, (IntPtr)pAttr);
					}
				}
			}
			if (pContext == IntPtr.Zero) throw new ALException("Failed to create AL context: " + (ALCError)ALC.Functions.alcGetError(Device));
			return new ALCContext(pContext);
		}

		public string GetString(ALCGetString pname) => MemoryUtil.GetASCII(ALC.Functions.alcGetString(Device, (ALCenum)pname));

		public int GetInteger(ALCGetInteger pname) {
			int val = 0;
			unsafe {
				ALC.Functions.alcGetIntegerv(Device, (int)pname, 1, (IntPtr)(&val));
			}
			return val;
		}

		public void GetInteger(ALCGetInteger pname, Span<int> vals) {
			unsafe {
				fixed(int* pVals = vals) {
					ALC.Functions.alcGetIntegerv(Device, (int)pname, vals.Length, (IntPtr)pVals);
				}
			}
		}

		public ALCError GetError() => (ALCError)ALC.Functions.alcGetError(Device);

		public bool IsExtensionPresent(string name) => ALC.Functions.alcIsExtensionPresent(Device, name) != 0;

		public IntPtr GetProcAddress(string funcname) => ALC.Functions.alcGetProcAddress(Device, funcname);

		public ALCenum GetEnumValue(string enumname) => ALC.Functions.alcGetEnumValue(Device, enumname);

		public void CaptureStart() => ALC.Functions.alcCaptureStart(Device);

		public void CaptureStop() => ALC.Functions.alcCaptureStop(Device);

		public void CaptureSamples<T>(Span<T> buffer) where T : unmanaged {
			unsafe {
				fixed(T* pBuffer = buffer) {
					ALC.Functions.alcCaptureSamples(Device, (IntPtr)pBuffer, buffer.Length * sizeof(T));
				}
			}
		}

	}

	public class ALCContext : IDisposable {

		public static ALCContext Current {
			get {
				IntPtr pContext = ALC.Functions.alcGetCurrentContext();
				return pContext == IntPtr.Zero ? null : new(pContext);
			}
		}

		[NativeType("ALCcontext*")]
		public IntPtr Context { get; private set; }

		public ALCDevice Device => new(ALC.Functions.alcGetContextsDevice(Context), false);

		public ALCContext([NativeType("ALCcontext*")] IntPtr pContext) {
			Context = pContext;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Context != IntPtr.Zero) {
				ALC.Functions.alcDestroyContext(Context);
				Context = IntPtr.Zero;
			}
		}

		public bool MakeContextCurrent() => ALC.Functions.alcMakeContextCurrent(Context) != 0;

		public void ProcessContext() => ALC.Functions.alcProcessContext(Context);

		public void SuspendContext() => ALC.Functions.alcSuspendContext(Context);

	}

}

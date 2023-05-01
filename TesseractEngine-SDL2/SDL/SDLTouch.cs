using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {
	
	public enum SDLTouchDeviceType {
		Invalid = -1,
		Direct,
		IndirectAbsolute,
		IndirectRelative
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLFinger {
		public long ID;
		public float X;
		public float Y;
		public float Pressure;
	}

	public readonly struct SDLTouchDevice {

		public int DeviceIndex { get; init; }

		public SDLTouchID TouchID {
			get {
				unsafe {
					return new() { TouchID = SDL2.Functions.SDL_GetTouchDevice(DeviceIndex) };
				}
			}
		}
	}

	public readonly struct SDLTouchID {

		public long TouchID { get; init; }

		public SDLTouchDeviceType Type {
			get {
				unsafe {
					return SDL2.Functions.SDL_GetTouchDeviceType(TouchID);
				}
			}
		}

		public int NumFingers {
			get {
				unsafe {
					return SDL2.Functions.SDL_GetNumTouchFingers(TouchID);
				}
			}
		}

		public IPointer<SDLFinger> GetTouchFinger(int index) {
			unsafe {
				return new UnmanagedPointer<SDLFinger>((IntPtr)SDL2.Functions.SDL_GetTouchFinger(TouchID, index));
			}
		}

		public void RecordGesture() {
			unsafe {
				SDL2.Functions.SDL_RecordGesture(TouchID);
			}
		}

		public void LoadDollarTemplates(SDLRWOps rwops) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_LoadDollarTemplates(TouchID, (SDL_RWops*)rwops.RWOps.Ptr));
			}
		}

		public void LoadDollarTemplates(SDLSpanRWOps rwops) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_LoadDollarTemplates(TouchID, (SDL_RWops*)rwops.RWOps.Ptr));
			}
		}
	}

}

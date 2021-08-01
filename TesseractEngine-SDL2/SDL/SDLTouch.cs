using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

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

	public struct SDLTouchDevice {

		public int DeviceIndex { get; init; }

		public SDLTouchID TouchID => new() { TouchID = SDL2.Functions.SDL_GetTouchDevice(DeviceIndex) };

	}

	public struct SDLTouchID {

		public long TouchID { get; init; }

		public SDLTouchDeviceType Type => SDL2.Functions.SDL_GetTouchDeviceType(TouchID);

		public int NumFingers => SDL2.Functions.SDL_GetNumTouchFingers(TouchID);

		public IPointer<SDLFinger> GetTouchFinger(int index) => new UnmanagedPointer<SDLFinger>(SDL2.Functions.SDL_GetTouchFinger(TouchID, index));

		public void RecordGesture() => SDL2.Functions.SDL_RecordGesture(TouchID);

		public void LoadDollarTemplates(SDLRWOps rwops) => SDL2.CheckError(SDL2.Functions.SDL_LoadDollarTemplates(TouchID, rwops.RWOps.Ptr));

		public void LoadDollarTemplates(SDLSpanRWOps rwops) => SDL2.CheckError(SDL2.Functions.SDL_LoadDollarTemplates(TouchID, rwops.RWOps.Ptr));

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {
	
	public enum SDLHapticType : ushort {
		Constant = 0x0001,
		Sine = 0x0002,
		LeftRight = 0x0004,
		Triangle = 0x0008,
		SawtoothUp = 0x0010,
		SawtoothDown = 0x0020,
		Ramp = 0x0040,
		Spring = 0x0080,
		Damper = 0x0100,
		Inertia = 0x0200,
		Friction = 0x0400,
		Custom = 0x0800,
		Gain = 0x1000,
		AutoCenter = 0x2000,
		Status = 0x4000,
		Pause = 0x8000
	}

	public enum SDLHapticDirectionType : byte {
		Polar = 0,
		Cartesian,
		Spherical,
		SteeringAxis
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLHapticDirection {
		public SDLHapticDirectionType Type;
		public Vector3i Dir;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLHapticConstant {
		public SDLHapticType Type;
		public SDLHapticDirection Direction;
		public uint Length;
		public ushort Delay;
		public ushort Button;
		public ushort Interval;
		public short Level;
		public ushort AttackLength;
		public ushort AttachLevel;
		public ushort FadeLength;
		public ushort FadeLevel;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLHapticPeriodic {
		public SDLHapticType Type;
		public SDLHapticDirection Direction;
		public uint Length;
		public ushort Delay;
		public ushort Button;
		public ushort Interval;
		public ushort Period;
		public short Magnitude;
		public short Offset;
		public ushort Phase;
		public ushort AttackLength;
		public ushort AttachLevel;
		public ushort FadeLength;
		public ushort FadeLevel;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLHapticCondition {
		public SDLHapticType Type;
		public SDLHapticDirection Direction;
		public uint Length;
		public ushort Delay;
		public ushort Button;
		public ushort Interval;
		public Vector3us RightSat;
		public Vector3us LeftSat;
		public Vector3s RightCoeff;
		public Vector3s LeftCoeff;
		public Vector3us Deadband;
		public Vector3s Center;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLHapticRamp {
		public SDLHapticType Type;
		public SDLHapticDirection Direction;
		public uint Length;
		public ushort Delay;
		public ushort Button;
		public ushort Interval;
		public short Start;
		public short End;
		public ushort AttackLength;
		public ushort AttachLevel;
		public ushort FadeLength;
		public ushort FadeLevel;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLHapticLeftRight {
		public SDLHapticType Type;
		public uint Length;
		public ushort LargeMagnitude;
		public ushort SmallMagnitude;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLHapticCustom {
		public SDLHapticType Type;
		public SDLHapticDirection Direction;
		public uint Length;
		public ushort Delay;
		public ushort Button;
		public ushort Interval;
		public byte Channels;
		public ushort Period;
		public ushort Samples;
		[NativeType("Uint16*")]
		public IntPtr Data;
		public ushort AttackLength;
		public ushort AttachLevel;
		public ushort FadeLength;
		public ushort FadeLevel;
	}
	
	[StructLayout(LayoutKind.Explicit)]
	public struct SDLHapticEffect {
		[FieldOffset(0)]
		public SDLHapticType Type;
		[FieldOffset(0)]
		public SDLHapticConstant Constant;
		[FieldOffset(0)]
		public SDLHapticPeriodic Periodic;
		[FieldOffset(0)]
		public SDLHapticCondition Condition;
		[FieldOffset(0)]
		public SDLHapticRamp Ramp;
		[FieldOffset(0)]
		public SDLHapticLeftRight LeftRight;
		[FieldOffset(0)]
		public SDLHapticCustom Custom;
	}

	public struct SDLHapticDevice {

		public int DeviceIndex { get; set; }

		public string Name => MemoryUtil.GetASCII(SDL2.Functions.SDL_HapticName(DeviceIndex))!;

		public bool Opened => SDL2.Functions.SDL_HapticOpened(DeviceIndex) != 0;

		public SDLHaptic Open() {
			IntPtr device = SDL2.Functions.SDL_HapticOpen(DeviceIndex);
			if (device == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			return new SDLHaptic(device);
		}

	}

	public class SDLHaptic : IDisposable {

		public IPointer<SDL_Haptic> Haptic { get; private set; }

		public SDLHapticDevice Device => new() { DeviceIndex = SDL2.Functions.SDL_HapticIndex(Haptic.Ptr) };

		public int NumEffects => SDL2.Functions.SDL_HapticNumEffects(Haptic.Ptr);

		public int NumEffectsPlaying => SDL2.Functions.SDL_HapticNumEffectsPlaying(Haptic.Ptr);

		public SDLHapticType Query => (SDLHapticType)SDL2.Functions.SDL_HapticQuery(Haptic.Ptr);

		public int NumAxes => SDL2.Functions.SDL_HapticNumAxes(Haptic.Ptr);

		public SDLHaptic(IntPtr pHaptic) {
			Haptic = new UnmanagedPointer<SDL_Haptic>(pHaptic);
		}

		public SDLHaptic(SDLJoystick joystick) {
			IntPtr pHaptic = SDL2.Functions.SDL_HapticOpenFromJoystick(joystick.Joystick.Ptr);
			if (pHaptic == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			Haptic = new UnmanagedPointer<SDL_Haptic>(pHaptic);
		}

		public bool EffectSupported(SDLHapticEffect effect) => SDL2.Functions.SDL_HapticEffectSupported(Haptic.Ptr, effect) > 0;

		public int NewEffect(SDLHapticEffect effect) => SDL2.CheckError(SDL2.Functions.SDL_HapticNewEffect(Haptic.Ptr, effect));

		public void UpdateEffect(int effect, SDLHapticEffect data) => SDL2.CheckError(SDL2.Functions.SDL_HapticUpdateEffect(Haptic.Ptr, effect, data));

		public void RunEffect(int effect, uint iterations = 1) => SDL2.CheckError(SDL2.Functions.SDL_HapticRunEffect(Haptic.Ptr, effect, iterations));

		public void StopEffect(int effect) => SDL2.Functions.SDL_HapticStopEffect(Haptic.Ptr, effect);

		public void DestroyEffect(int effect) => SDL2.Functions.SDL_HapticDestroyEffect(Haptic.Ptr, effect);

		public bool GetEffectStatus(int effect) => SDL2.CheckError(SDL2.Functions.SDL_HapticGetEffectStatus(Haptic.Ptr, effect)) != 0;

		public int Gain {
			set => SDL2.CheckError(SDL2.Functions.SDL_HapticSetGain(Haptic.Ptr, value));
		}

		public int AutoCenter {
			set => SDL2.CheckError(SDL2.Functions.SDL_HapticSetAutocenter(Haptic.Ptr, value));
		}

		public void Pause() => SDL2.CheckError(SDL2.Functions.SDL_HapticPause(Haptic.Ptr));

		public void Unpause() => SDL2.CheckError(SDL2.Functions.SDL_HapticUnpause(Haptic.Ptr));

		public void StopAll() => SDL2.CheckError(SDL2.Functions.SDL_HapticStopAll(Haptic.Ptr));

		public bool RumbleSupported => SDL2.CheckError(SDL2.Functions.SDL_HapticRumbleSupported(Haptic.Ptr)) != 0;

		public void RumbleInit() => SDL2.CheckError(SDL2.Functions.SDL_HapticRumbleInit(Haptic.Ptr));

		public void RumblePlay(float strength, uint length) => SDL2.CheckError(SDL2.Functions.SDL_HapticRumblePlay(Haptic.Ptr, strength, length));

		public void RumbleStop() => SDL2.CheckError(SDL2.Functions.SDL_HapticRumbleStop(Haptic.Ptr));

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Haptic != null) {
				SDL2.Functions.SDL_HapticClose(Haptic.Ptr);
				Haptic = new NullPointer<SDL_Haptic>();
			}
		}

	}

}

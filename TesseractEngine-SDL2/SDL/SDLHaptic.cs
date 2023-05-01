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

		public string Name {
			get {
				unsafe {
					return MemoryUtil.GetASCII(SDL2.Functions.SDL_HapticName(DeviceIndex))!;
				}
			}
		}

		public bool Opened {
			get {
				unsafe {
					return SDL2.Functions.SDL_HapticOpened(DeviceIndex) != 0;
				}
			}
		}

		public SDLHaptic Open() {
			unsafe {
				IntPtr device = SDL2.Functions.SDL_HapticOpen(DeviceIndex);
				if (device == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				return new SDLHaptic(device);
			}
		}

	}

	public class SDLHaptic : IDisposable {

		[NativeType("SDL_Haptic*")]
		public IntPtr Haptic { get; private set; }

		public SDLHapticDevice Device {
			get {
				unsafe {
					return new() { DeviceIndex = SDL2.Functions.SDL_HapticIndex(Haptic) };
				}
			}
		}

		public int NumEffects {
			get {
				unsafe {
					return SDL2.Functions.SDL_HapticNumEffects(Haptic);
				}
			}
		}

		public int NumEffectsPlaying {
			get {
				unsafe {
					return SDL2.Functions.SDL_HapticNumEffectsPlaying(Haptic);
				}
			}
		}

		public SDLHapticType Query {
			get {
				unsafe {
					return (SDLHapticType)SDL2.Functions.SDL_HapticQuery(Haptic);
				}
			}
		}

		public int NumAxes {
			get {
				unsafe {
					return SDL2.Functions.SDL_HapticNumAxes(Haptic);
				}
			}
		}

		public SDLHaptic([NativeType("SDL_Haptic*")] IntPtr pHaptic) {
			Haptic = pHaptic;
		}

		public SDLHaptic(SDLJoystick joystick) {
			unsafe {
				IntPtr pHaptic = SDL2.Functions.SDL_HapticOpenFromJoystick(joystick.Joystick);
				if (pHaptic == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				Haptic = pHaptic;
			}
		}

		public bool EffectSupported(SDLHapticEffect effect) {
			unsafe {
				return SDL2.Functions.SDL_HapticEffectSupported(Haptic, effect) > 0;
			}
		}

		public int NewEffect(SDLHapticEffect effect) {
			unsafe {
				return SDL2.CheckError(SDL2.Functions.SDL_HapticNewEffect(Haptic, effect));
			}
		}

		public void UpdateEffect(int effect, SDLHapticEffect data) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_HapticUpdateEffect(Haptic, effect, data));
			}
		}

		public void RunEffect(int effect, uint iterations = 1) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_HapticRunEffect(Haptic, effect, iterations));
			}
		}

		public void StopEffect(int effect) {
			unsafe {
				SDL2.Functions.SDL_HapticStopEffect(Haptic, effect);
			}
		}

		public void DestroyEffect(int effect) {
			unsafe {
				SDL2.Functions.SDL_HapticDestroyEffect(Haptic, effect);
			}
		}

		public bool GetEffectStatus(int effect) {
			unsafe {
				return SDL2.CheckError(SDL2.Functions.SDL_HapticGetEffectStatus(Haptic, effect)) != 0;
			}
		}

		public int Gain {
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_HapticSetGain(Haptic, value));
				}
			}
		}

		public int AutoCenter {
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_HapticSetAutocenter(Haptic, value));
				}
			}
		}

		public void Pause() {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_HapticPause(Haptic));
			}
		}

		public void Unpause() {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_HapticUnpause(Haptic));
			}
		}

		public void StopAll() {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_HapticStopAll(Haptic));
			}
		}

		public bool RumbleSupported {
			get {
				unsafe {
					return SDL2.CheckError(SDL2.Functions.SDL_HapticRumbleSupported(Haptic)) != 0;
				}
			}
		}

		public void RumbleInit() {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_HapticRumbleInit(Haptic));
			}
		}

		public void RumblePlay(float strength, uint length) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_HapticRumblePlay(Haptic, strength, length));
			}
		}

		public void RumbleStop() {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_HapticRumbleStop(Haptic));
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Haptic != IntPtr.Zero) {
				unsafe {
					SDL2.Functions.SDL_HapticClose(Haptic);
				}
				Haptic = IntPtr.Zero;
			}
		}

	}

}

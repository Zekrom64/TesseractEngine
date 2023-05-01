using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {
	
	public enum SDLSensorType {
		Invalid = -1,
		Unknown,
		Accelerometer,
		Gyroscope
	}

	public struct SDLSensorDevice {

		public int DeviceIndex { get; set; }

		public string Name {
			get {
				unsafe {
					return MemoryUtil.GetASCII(SDL2.Functions.SDL_SensorGetDeviceName(DeviceIndex))!;
				}
			}
		}

		public SDLSensorType Type {
			get {
				unsafe {
					return SDL2.Functions.SDL_SensorGetDeviceType(DeviceIndex);
				}
			}
		}

		public int NonPortableType {
			get {
				unsafe {
					return SDL2.Functions.SDL_SensorGetDeviceNonPortableType(DeviceIndex);
				}
			}
		}

		public int InstanceID {
			get {
				unsafe {
					return SDL2.Functions.SDL_SensorGetDeviceInstanceID(DeviceIndex);
				}
			}
		}

		public SDLSensor Open() {
			unsafe {
				IntPtr pSensor = SDL2.Functions.SDL_SensorOpen(DeviceIndex);
				if (pSensor == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				return new SDLSensor(pSensor);
			}
		}

	}

	public class SDLSensor : IDisposable {

		[NativeType("SDL_Sensor*")]
		public IntPtr Sensor { get; private set; }

		public string Name {
			get {
				unsafe {
					return MemoryUtil.GetASCII(SDL2.Functions.SDL_SensorGetName(Sensor))!;
				}
			}
		}

		public SDLSensorType Type {
			get {
				unsafe {
					return SDL2.Functions.SDL_SensorGetType(Sensor);
				}
			}
		}

		public int NonPortableType {
			get {
				unsafe {
					return SDL2.Functions.SDL_SensorGetNonPortableType(Sensor);
				}
			}
		}

		public int InstanceID {
			get {
				unsafe {
					return SDL2.Functions.SDL_SensorGetInstanceID(Sensor);
				}
			}
		}

		public SDLSensor(IntPtr pSensor) {
			Sensor = pSensor;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Sensor != IntPtr.Zero) {
				unsafe {
					SDL2.Functions.SDL_SensorClose(Sensor);
				}
				Sensor = IntPtr.Zero;
			}
		}

		public Span<float> GetData(Span<float> v) {
			unsafe {
				fixed(float* pV = v) {
					SDL2.Functions.SDL_SensorGetData(Sensor, pV, v.Length);
				}
			}
			return v;
		}
	}

}

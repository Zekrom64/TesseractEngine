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

		public string Name => MemoryUtil.GetASCII(SDL2.Functions.SDL_SensorGetDeviceName(DeviceIndex))!;

		public SDLSensorType Type => SDL2.Functions.SDL_SensorGetDeviceType(DeviceIndex);

		public int NonPortableType => SDL2.Functions.SDL_SensorGetDeviceNonPortableType(DeviceIndex);

		public int InstanceID => SDL2.Functions.SDL_SensorGetDeviceInstanceID(DeviceIndex);

		public SDLSensor Open() {
			IntPtr pSensor = SDL2.Functions.SDL_SensorOpen(DeviceIndex);
			if (pSensor == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			return new SDLSensor(pSensor);
		}

	}

	public class SDLSensor : IDisposable {

		public IPointer<SDL_Sensor> Sensor { get; private set; }

		public string Name => MemoryUtil.GetASCII(SDL2.Functions.SDL_SensorGetName(Sensor.Ptr))!;

		public SDLSensorType Type => SDL2.Functions.SDL_SensorGetType(Sensor.Ptr);

		public int NonPortableType => SDL2.Functions.SDL_SensorGetNonPortableType(Sensor.Ptr);

		public int InstanceID => SDL2.Functions.SDL_SensorGetInstanceID(Sensor.Ptr);

		public SDLSensor(IntPtr pSensor) {
			Sensor = new UnmanagedPointer<SDL_Sensor>(pSensor);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Sensor != null) {
				SDL2.Functions.SDL_SensorClose(Sensor.Ptr);
				Sensor = new NullPointer<SDL_Sensor>();
			}
		}

		public Span<float> GetData(Span<float> v) {
			unsafe {
				fixed(float* pV = v) {
					SDL2.Functions.SDL_SensorGetData(Sensor.Ptr, (IntPtr)pV, v.Length);
				}
			}
			return v;
		}
	}

}

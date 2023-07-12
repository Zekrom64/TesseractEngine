using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.WebGPU {

	public class WGpuDevice : IWGpuObject {

		public nint PrimitiveHandle { get; }

		public WGpuDevice(nint device) {
			PrimitiveHandle = device;
		}

	}

}

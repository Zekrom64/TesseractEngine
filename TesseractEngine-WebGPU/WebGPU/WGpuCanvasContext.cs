using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.WebGPU {

	public class WGpuCanvasContext : IWGpuObject {

		public nint PrimitiveHandle { get; }

		public WGpuCanvasContext(nint gpuCanvasContext) {
			PrimitiveHandle = gpuCanvasContext;
		}

	}

}

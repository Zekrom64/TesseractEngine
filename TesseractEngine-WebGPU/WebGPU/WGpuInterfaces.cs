using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.WebGPU.Native;

namespace Tesseract.WebGPU {

	public interface IWGpuObject : IPrimitiveHandle<IntPtr> {

		/// <summary>
		/// The label associated with the WebGPU object.
		/// </summary>
		public string Label {
			get {
				Span<byte> buffer = stackalloc byte[WGpu.ObjectLabelMaxLength];
				unsafe {
					fixed(byte* pBuffer = buffer) {
						int len = WGpuFunctions.wgpu_object_get_label(PrimitiveHandle, (IntPtr)pBuffer, (uint)buffer.Length);
						return MemoryUtil.GetUTF8(buffer[..len]);
					}
				}
			}
			set => WGpuFunctions.wgpu_object_set_label(PrimitiveHandle, value);
		}

	}

}

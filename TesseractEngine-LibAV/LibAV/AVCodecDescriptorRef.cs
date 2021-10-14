using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class AVCodecDescriptorRef : IDisposable {

		public ManagedPointer<AVCodecDescriptor> CodecDescriptor { get; }

		public AVCodecDescriptorRef(ManagedPointer<AVCodecDescriptor> codecDesc) {
			CodecDescriptor = codecDesc;
		}

		public AVCodecDescriptorRef Next {
			get {
				IntPtr pNext = LibAVCodec.Functions.avcodec_descriptor_next(CodecDescriptor);
				return pNext == IntPtr.Zero ? null : new AVCodecDescriptorRef(new(pNext));
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			CodecDescriptor.Dispose();
		}



	}

}

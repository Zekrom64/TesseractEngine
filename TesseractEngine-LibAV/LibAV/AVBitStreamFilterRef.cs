using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class AVBitStreamFilterRef : IDisposable {

		public ManagedPointer<AVBitStreamFilter> BitStreamFilter { get; private set; }

		public AVBitStreamFilterRef(ManagedPointer<AVBitStreamFilter> bsf) {
			BitStreamFilter = bsf;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (BitStreamFilter) {
				BitStreamFilter.Dispose();
				BitStreamFilter = default;
			}
		}

		public static AVBitStreamFilterRef GetByName(string name) {
			IntPtr pBsf = LibAVCodec.Functions.av_bsf_get_by_name(name);
			if (pBsf == IntPtr.Zero) return null;
			return new AVBitStreamFilterRef(new(pBsf));
		}

	}

}

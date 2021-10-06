using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {

	public class SWSFilter : IDisposable {

		public IPointer<SwsFilter> Filter { get; }

		public SWSFilter(IPointer<SwsFilter> filter) {
			Filter = filter;
		}

		public SWSFilter() {
			Filter = new ManagedPointer<SwsFilter>();
		}

		public static SWSFilter GetDefaultFilter(float lumaGBlur, float chromaGBlur, float lumaSharpen, float chromaSharpen, float chromaHShift, float chromaVShift, bool verbose = false) {
			IntPtr pFilter = SWScale.Functions.sws_getDefaultFilter(lumaGBlur, chromaGBlur, lumaSharpen, chromaSharpen, chromaHShift, chromaVShift, verbose);
			return new(new ManagedPointer<SwsFilter>(pFilter, ptr => SWScale.Functions.sws_freeFilter(ptr)));
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Filter is IDisposable d) d.Dispose();
		}

		public static implicit operator IntPtr(SWSFilter filter) => filter.Filter.Ptr;

	}

}

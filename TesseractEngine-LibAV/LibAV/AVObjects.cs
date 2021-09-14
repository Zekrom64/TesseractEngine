using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.LibAV {
	
	public class AVException : Exception {

		public static string ErrorToString(AVError error) {
			Span<byte> errbuf = stackalloc byte[1024];
			int ret;
			unsafe {
				fixed(byte* pErrbuf = errbuf) {
					ret = LibAVUtil.Functions.av_strerror((int)error, (IntPtr)pErrbuf, (nuint)errbuf.Length);
				}
			}
			if (ret < 0) return "Unknown error";
			return Encoding.ASCII.GetString(errbuf);
		}

		public AVException(string msg) : base(msg) { }

		public AVException(string msg, AVError error) : base(msg + ": " + ErrorToString(error)) { }

	}

}

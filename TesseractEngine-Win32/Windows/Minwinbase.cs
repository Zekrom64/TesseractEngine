using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Windows {

	[StructLayout(LayoutKind.Sequential)]
	public struct FILETIME {

		public uint LowDateTime;
		public uint HighDateTime;

		public static implicit operator ulong(FILETIME ft) => ((ulong)ft.HighDateTime) << 32 | ft.LowDateTime;
		public static implicit operator FILETIME(ulong v) {
			return new FILETIME() {
				LowDateTime = (uint)v,
				HighDateTime = (uint)(v >> 32)
			};
		}

	}

}

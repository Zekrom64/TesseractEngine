using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Windows {

	/// <summary>
	/// Contains a 64-bit value representing the number of 100-nanosecond intervals since January 1, 1601 (UTC).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct FILETIME {

		/// <summary>
		/// The low-order part of the file time.
		/// </summary>
		public uint LowDateTime;

		/// <summary>
		/// The high-order part of the file time.
		/// </summary>
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

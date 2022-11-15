using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.Windows {

	using DWORD = UInt32;
	using LONG = Int32;
	using LONGLONG = Int64;

	/// <summary>
	/// Describes a local identifier for an adapter.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct LUID {

		/// <summary>
		/// Specifies a DWORD that contains the unsigned lower numbers of the id.
		/// </summary>
		public readonly DWORD LowPart;

		/// <summary>
		/// Specifies a LONG that contains the signed high numbers of the id.
		/// </summary>
		public readonly LONG HighPart;

		public LUID(DWORD lowPart, LONG highPart) {
			LowPart = lowPart;
			HighPart = highPart;
		}

	}

	/// <summary>
	/// <para>Represents a 64-bit signed integer value.</para>
	/// 
	/// <para>
	/// The <b>LARGE_INTEGER</b> structure is actually a union. If your compiler has built-in support for 64-bit integers, use the
	/// <see cref="QuadPart">QuadPart</see> member to store the 64-bit integer. Otherwise, use the <see cref="LowPart">LowPart</see>
	/// and <see cref="HighPart">HighPart</see> members to store the 64-bit integer.
	/// </para>
	/// 
	/// <para>
	/// In the CLR this type supports implict conversion with <see cref="Int64">long</see>.
	/// </para>
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct LARGE_INTEGER {

		[FieldOffset(0)]
		public LONGLONG QuadPart;

		[FieldOffset(0)]
		public DWORD LowPart;

		[FieldOffset(4)]
		public LONG HighPart;

		public static implicit operator long(LARGE_INTEGER value) => value.QuadPart;
		public static implicit operator LARGE_INTEGER(long value) => new() { QuadPart = value };

	}

}

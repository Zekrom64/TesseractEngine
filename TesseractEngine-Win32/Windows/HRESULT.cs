using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace Tesseract.Windows {

	using LONG = Int32;

	/// <summary>
	/// <para>A 32-bit value that is used to describe an error or warning.</para>
	/// 
	/// <para>
	/// An HRESULT value consists of the following fields:
	/// <list type="bullet">
	///		<item>A 1-bit code indicating severity, where zero represents success and 1 represents failure.</item>
	///		<item>A 4-bit reserved value.</item>
	///		<item>An 11-bit code indicating responsibility for the error or warning, also known as a facility code.</item>
	///		<item>A 16-bit code describing the error or warning.</item>.
	/// </list>
	/// Most MAPI interface methods and functions return <b>HRESULT</b> values to provide detailed cause formation. <b>HRESULT</b>
	/// values are also used widely in OLE interface methods. OLE provides several macros for converting between <b>HRESULT</b>
	/// values and <b>SCODE</b> values, another common data type for error handling.
	/// </para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct HRESULT : IEquatable<HRESULT> {

		/// <summary>
		/// The underlying <b>HRESULT</b> value.
		/// </summary>
		public LONG Value;

		/// <summary>
		/// The value of the severity bit.
		/// </summary>
		public bool Severity {
			get => (Value & 0x80000000) != 0;
			set {
				if (value) Value |= unchecked((int)0x80000000);
				else Value &= ~unchecked((int)0x80000000);
			}
		}

		/// <summary>
		/// The value of the facility field.
		/// </summary>
		public int Facility {
			get => (Value >> 16) & 0x7FF;
			set => Value = (Value & unchecked((int)0xF800FFFF)) | ((value & 0x7FF) << 16);
		}

		/// <summary>
		/// The value of the error code field.
		/// </summary>
		public int Code {
			get => Value & 0xFFFF;
			set => Value = (Value & unchecked((int)0xFFFF0000)) | (value & 0xFFFF);
		}

		/// <summary>
		/// If this <b>HRESULT</b> indicates success (ie. it is equal to <b>S_OK</b>).
		/// </summary>
		public bool Succeeded => Value == 0;

		/// <summary>
		/// If this <b>HRESULT</b> indicates some kind of failure (ie. it is not equal to <b>S_OK</b>).
		/// </summary>
		public bool Failed => Value != 0;

		/// <summary>
		/// If this <b>HRESULT</b> indicates a failure throws an exception via <see cref="Marshal.ThrowExceptionForHR(LONG)"/>.
		/// </summary>
		public void DoThrowException() {
			if (Failed) Marshal.ThrowExceptionForHR(Value);
		}

		public bool Equals(HRESULT other) => Value == other.Value;

		public override string ToString() {
			Exception? ex = Marshal.GetExceptionForHR(Value);
			if (ex != null) return ex.Message;
			else return string.Empty;
		}

		public override bool Equals([NotNullWhen(true)] object? obj) => obj is HRESULT hr && Equals(hr);

		public override int GetHashCode() => Value;

		public static implicit operator LONG(HRESULT hr) => hr.Value;
		public static implicit operator HRESULT(LONG l) => new() { Value = l };

		public static bool operator ==(HRESULT hr1, HRESULT hr2) => hr1.Value == hr2.Value;
		public static bool operator !=(HRESULT hr1, HRESULT hr2) => hr1.Value != hr2.Value;


	}
}

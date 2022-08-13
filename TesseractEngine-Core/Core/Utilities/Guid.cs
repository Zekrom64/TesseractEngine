using System;
using System.Text;

namespace Tesseract.Core.Utilities {

	/// <summary>
	/// <para>
	/// A GUID digester assists in converting arbitrary data into the byte array which defines a GUID.
	/// </para>
	/// <para>
	/// The digesting algorithm starts with an initial 16-byte state representing the GUID data. For every
	/// digested byte this state is rotated forward by one byte, and the digested byte is exlusive-or'd with
	/// the first byte in the state. For larger data types this is repeated for every byte in the value. Note
	/// that this algorithm is not cryptographically secure, and could leak the source information for small
	/// digests.
	/// </para>
	/// </summary>
	/// <remarks>
	/// <para>
	/// A GUID digester will produce the same resulting GUID if the initial state is the same and every subsequent
	/// digested byte is the same (ie. the digested data is provided in the same form and order). This makes the digester
	/// useful for generating a unique ID based on certain parameters.
	/// </para>
	/// <para>
	///	The digester should be provided with a unique but constant initial state to differentiate between GUIDs generated for
	///	different purposes or by different implementations.
	/// </para>
	/// <para>
	///	Numeric types larger than one byte are digested in little-endian order. Strings must be converted using a particular
	///	encoding to convert them to a known sequence of bytes. UTF-8 encoding should be preferred.
	/// </para>
	/// </remarks>
	public class GuidDigester {

		private byte[] guidData = new byte[16];

		/// <summary>
		/// The current GUID created from the digester's state.
		/// </summary>
		public Guid CurrentGuid {
			get => new(guidData);
			set => guidData = value.ToByteArray();
		}

		/// <summary>
		/// Creates a new digester with an initial state.
		/// </summary>
		/// <param name="initial">The initial GUID state</param>
		public GuidDigester(Guid initial = default) {
			CurrentGuid = initial;
		}

		/// <summary>
		/// Digests a stream of arbitrary bytes.
		/// </summary>
		/// <param name="data">Bytes to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(in ReadOnlySpan<byte> data) {
			foreach (byte b in data) {
				byte val = (byte)(b ^ guidData[15]);
				for (int i = 14; i >= 0; i--) guidData[i + 1] = guidData[i];
				guidData[0] = val;
			}
			return this;
		}

		/// <summary>
		/// Digests an individual byte.
		/// </summary>
		/// <param name="val">Byte to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(byte val) {
			val ^= guidData[15];
			for (int i = 14; i >= 0; i--) guidData[i + 1] = guidData[i];
			guidData[0] = val;
			return this;
		}

		/// <summary>
		/// Digests an individual byte.
		/// </summary>
		/// <param name="val">Byte to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(sbyte val) => Digest((byte)val);

		/// <summary>
		/// Digests a 16-bit integer.
		/// </summary>
		/// <param name="val">Value to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(short val) {
			Digest((byte)(val >> 8));
			return Digest((byte)val);
		}

		/// <summary>
		/// Digests a 16-bit integer.
		/// </summary>
		/// <param name="val">Value to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(ushort val) => Digest((short)val);

		/// <summary>
		/// Digests a 32-bit integer.
		/// </summary>
		/// <param name="val">Value to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(int val) {
			Digest((byte)(val >> 24));
			Digest((byte)(val >> 16));
			Digest((byte)(val >> 8));
			return Digest((byte)val);
		}

		/// <summary>
		/// Digests a 32-bit integer.
		/// </summary>
		/// <param name="val">Value to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(uint val) => Digest((int)val);

		/// <summary>
		/// Digests a 64-bit integer.
		/// </summary>
		/// <param name="val">Value to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(long val) {
			Digest((byte)(val >> 56));
			Digest((byte)(val >> 48));
			Digest((byte)(val >> 40));
			Digest((byte)(val >> 32));
			Digest((byte)(val >> 24));
			Digest((byte)(val >> 16));
			Digest((byte)(val >> 8));
			return Digest((byte)val);
		}

		/// <summary>
		/// Digests a 64-bit integer.
		/// </summary>
		/// <param name="val">Value to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(ulong val) => Digest((long)val);

		/// <summary>
		/// Digests a single-precision floating point value.
		/// </summary>
		/// <param name="val">Value to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(float val) => Digest(BitConverter.SingleToInt32Bits(val));

		/// <summary>
		/// Digests a double-precision floating point value.
		/// </summary>
		/// <param name="val">Value to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester Digest(double val) => Digest(BitConverter.DoubleToInt64Bits(val));

		/// <summary>
		/// Digests a string of UTF-8 encoded characters.
		/// </summary>
		/// <param name="str">String to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester DigestUTF8(string str) => Digest(Encoding.UTF8.GetBytes(str));

		/// <summary>
		/// Digests a string of ASCII encoded characters.
		/// </summary>
		/// <param name="str">String to digest</param>
		/// <returns>This digester</returns>
		public GuidDigester DigestASCII(string str) => Digest(Encoding.ASCII.GetBytes(str));

		public GuidDigester Digest(Guid guid) => Digest(guid.ToByteArray());

	}

	/// <summary>
	/// An unmanaged type that stores the raw bytes backing a <see cref="Guid"/>. Because the internal
	/// representation of a normal Guid is opaque it cannot be assumed that the memory layout backing
	/// it will be the same. This type guarentees the layout of the bytes composing the Guid and therefore
	/// is safe to store in binary form.
	/// </summary>
	public struct GuidValue {

		private unsafe fixed byte guid[16];

		/// <summary>
		/// The Guid value being stored.
		/// </summary>
		public Guid Guid {
			get {
				unsafe {
					fixed (byte* pGuid = guid) {
						return new Guid(new ReadOnlySpan<byte>(pGuid, 16));
					}
				}
			}
			set {
				byte[] bytes = value.ToByteArray();
				unsafe {
					for (int i = 0; i < 16; i++) guid[i] = bytes[i];
				}
			}
		}

		public GuidValue(Guid guid) {
			byte[] bytes = guid.ToByteArray();
			unsafe {
				for (int i = 0; i < 16; i++) this.guid[i] = bytes[i];
			}
		}

		public static implicit operator GuidValue(Guid value) => new(value);

		public static implicit operator Guid(GuidValue value) => value.Guid;

	}

}

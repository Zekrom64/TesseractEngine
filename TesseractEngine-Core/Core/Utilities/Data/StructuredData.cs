using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Core.Collections;

namespace Tesseract.Core.Utilities.Data {

	internal static class StructuredData {

		// Dictionaries for converting between type and ID values

		internal static readonly ConverseDictionary<Type, DataType> TypeToID = new() {
			{ typeof(bool), DataType.Boolean },

			{ typeof(byte), DataType.Byte },
			{ typeof(int), DataType.Int },
			{ typeof(long), DataType.Long },
			{ typeof(float), DataType.Float },
			{ typeof(double), DataType.Double },

			{ typeof(string), DataType.String },
			{ typeof(DataList), DataType.List },
			{ typeof(DataObject), DataType.Object }
		};

		internal static readonly IDictionary<DataType, Type> IDToType = TypeToID.Reverse;

		// Type ID values

		internal const byte TypeNone = default;
		internal const byte TypeBoolean = (byte)'B';

		internal const byte TypeTrue =   (byte)'y';
		internal const byte TypeFalse =  (byte)'n';

		internal const byte TypeByte =   (byte)'b';
		internal const byte TypeInt =    (byte)'i';
		internal const byte TypeLong =   (byte)'l';
		internal const byte TypeFloat =  (byte)'f';
		internal const byte TypeDouble = (byte)'d';

		internal const byte TypeString = (byte)'S';
		internal const byte TypeList =   (byte)'L';
		internal const byte TypeObject = (byte)'O';
		internal const byte TypeObjectStreaming = (byte)'o';

		// Floating-point equality with tolerance

		private const float EpsilonFloat = 0.0001f;
		private const double EpsilonDouble = 0.0000000001;

		internal static bool Equal(float a, float b) {
			float tolerace = Math.Abs(a * EpsilonFloat);
			return Math.Abs(a - b) <= tolerace;
		}

		internal static bool Equal(double a, double b) {
			double tolerace = Math.Abs(a * EpsilonDouble);
			return Math.Abs(a - b) <= tolerace;
		}

		// String encoding
		// Although BinaryReader/Writer have methods for strings this is used explicitly so
		// the format is known and compatible with other implementations

		internal static string ReadString(BinaryReader br) {
			// Read length
			int length = br.Read7BitEncodedInt();
			// Read data into buffer
			Span<byte> buffer = length > 4096 ? new byte[length] : stackalloc byte[length];
			br.Read(buffer);
			// Convert from UTF8
			return Encoding.UTF8.GetString(buffer);
		}

		internal static void WriteString(string str, BinaryWriter bw) {
			Span<byte> buffer = stackalloc byte[4096];
			// Try to encode using the stack allocated buffer first
			var encoder = Encoding.UTF8.GetEncoder();
			encoder.Convert(str, buffer, flush: true, out _, out int byteCount, out bool complete);
			// If unable to complete then encode into a new byte array and write
			if (!complete) {
				buffer = Encoding.UTF8.GetBytes(str);
				byteCount = buffer.Length;
			}
			// Write length and data
			bw.Write7BitEncodedInt(byteCount);
			bw.Write(buffer[..byteCount]);
		}

		// String parsing/escaping

		internal static string EscapeString(string str) {
			StringBuilder sb = new(str.Length);
			foreach(char c in str) {
				switch (c) {
					case '\'':
						sb.Append("\\'");
						break;
					case '\"':
						sb.Append("\\\"");
						break;
					case '\\':
						sb.Append("\\\\");
						break;
					case '\b':
						sb.Append("\\b");
						break;
					case '\f':
						sb.Append("\\f");
						break;
					case '\n':
						sb.Append("\\n");
						break;
					case '\r':
						sb.Append("\\r");
						break;
					case '\t':
						sb.Append("\\t");
						break;
					default:
						if (char.IsAscii(c)) sb.Append(c);
						else sb.Append($"\\u{(ushort)c:X4}");
						break;
				}
			}
			return sb.ToString();
		}
		
		internal static string ParseString(ReadOnlySpan<char> chars, out int numRead, bool simulate) {
			int i = 0;
			StringBuilder? sb = null;
			if (!simulate) sb = new();

			char Next(ReadOnlySpan<char> str) {
				if (i == str.Length) throw new FormatException("Unexpected end of text");
				return str[i++];
			}

			if (Next(chars) != '\"') throw new FormatException("Expected \'\"\' at the start of a string");
			do {
				char c = Next(chars);
				if (c == '\"') break;
				else if (c == '\\') {
					c = Next(chars);
					switch(c) {
						case '\'':
							sb?.Append('\'');
							break;
						case '\"':
							sb?.Append('\"');
							break;
						case '\\':
							sb?.Append('\\');
							break;
						case 'b':
							sb?.Append('\b');
							break;
						case 'f':
							sb?.Append('\f');
							break;
						case 'n':
							sb?.Append('\n');
							break;
						case 'r':
							sb?.Append('\r');
							break;
						case 't':
							sb?.Append('\t');
							break;
						case 'u':
							if (chars.Length - i < 4) throw new FormatException("Unexpected end of text");
							sb?.Append(int.Parse(chars[i..(i + 4)], NumberStyles.HexNumber));
							break;
						default:
							throw new FormatException($"Unexpected escape sequence at {i - 1}");
					}
				} else sb?.Append(c);
			} while (true);

			numRead = i;
			return sb != null ? sb.ToString() : string.Empty;
		}

	}

}

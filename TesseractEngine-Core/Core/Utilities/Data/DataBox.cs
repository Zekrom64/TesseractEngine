using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Utilities.Data {

	public enum DataParseMarker {
		Undefined,
		Boolean,
		Number,
		String,
		Object,
		List,

	}

	/// <summary>
	/// Custom box for data values that tracks their data type as well.
	/// </summary>
	public readonly struct DataBox : IEquatable<DataBox>, IConstructibleData<DataBox> {
	
		/// <summary>
		/// The boxed value.
		/// </summary>
		public object Value { get; }

		/// <summary>
		/// The data type of the value.
		/// </summary>
		public DataType Type { get; }

		internal DataBox(object? value, DataType type) {
			Value = value!;
			Type = value == null ? DataType.None : type;
		}

		/// <summary>
		/// Boxes a value, checking if it is an allowed data type.
		/// </summary>
		/// <param name="value">Value to box</param>
		/// <exception cref="ArgumentException">If the value is not of a valid type</exception>
		public DataBox(object value) {
			if (!StructuredData.TypeToID.TryGetValue(value.GetType(), out DataType type))
				throw new ArgumentException("Unsupported type of object for data boxing", nameof(value));
			Value = value;
			Type = type;
		}

		/// <summary>
		/// Gets this value as a specific type, potentially performing numeric conversion if required.
		/// </summary>
		/// <typeparam name="T">The target tpe</typeparam>
		/// <returns>This value as the target type</returns>
		/// <exception cref="InvalidCastException">If the conversion could not be performed</exception>
		public T As<T>() {
			// Direct conversion
			if (Value is T tval) return tval;
			// Indirect conversion of numeric types
			else if (typeof(T).IsPrimitive) {
				if (Value is IConvertible cvt) {
					return (T)cvt.ToType(typeof(T), null);
				}
			}
			throw new InvalidCastException($"Value is of incompatible type (expected {typeof(T)}, got {Value.GetType()})");
		}

		public bool Is<T>([NotNullWhen(true)] out T? value) {
			if (Value is T tval) {
				value = tval;
				return true;
			} else {
				value = default;
				return false;
			}
		}


		public bool Equals(DataBox other) => Type == other.Type && Type switch {
			// Special case checking for float equality with tolerance
			DataType.Float => StructuredData.Equal((float)Value, (float)other.Value),
			DataType.Double => StructuredData.Equal((double)Value, (double)other.Value),
			_ => Equals(Value, other.Value),
		};

		public override bool Equals([NotNullWhen(true)] object? obj) => obj is DataBox box && Equals(box);

		public override int GetHashCode() => Value?.GetHashCode() ?? 0;

		public override string ToString() => Type switch {
			DataType.Boolean => (bool)Value ? "true" : "false",
			DataType.Byte => $"{(byte)Value}b",
			DataType.Int => $"{(int)Value}i",
			DataType.Long => $"{(long)Value}l",
			DataType.Float => $"{(float)Value}f",
			DataType.Double => $"{(double)Value}d",
			DataType.String => $"\"{StructuredData.EscapeString((string)Value)}\"",
			DataType.List or DataType.Object => Value.ToString()!,
			_ => "?"
		};

		/// <summary>
		/// Parses a boxed data value from a span of characters.
		/// </summary>
		/// <param name="str">String to parse</param>
		/// <returns>Boxed data value</returns>
		public static DataBox Parse(ReadOnlySpan<char> str) => Parse(str, out _);

		/// <summary>
		/// Parses a boxed data value from a span of characters.
		/// </summary>
		/// <param name="str">String to parse</param>
		/// <param name="length">The length of the value in characters</param>
		/// <param name="simulate">If parsing should only be simulated instead of returning a complete value</param>
		/// <param name="markers">A list to fill with markers indicating what is being parsed</param>
		/// <returns>Boxed data value</returns>
		public static DataBox Parse(ReadOnlySpan<char> str, out int length, bool simulate = false, List<(int Position, DataParseMarker Marker)>? markers = null) =>
			Parse(str, out length, simulate, markers, nestedMarkers: null);

		private static DataBox Parse(ReadOnlySpan<char> str, out int length, bool simulate, List<(int, DataParseMarker)>? markers, Stack<DataParseMarker>? nestedMarkers) {
			int i = 0;

			// Initialize marker information as required
			if (markers != null && nestedMarkers == null) nestedMarkers = new();
			markers?.Add((0, DataParseMarker.Undefined));

			void MarkLastChar(DataParseMarker marker) => markers?.Add((i - 1, marker));
			void PopMark() {
				DataParseMarker mark = DataParseMarker.Undefined;
				nestedMarkers?.TryPop(out mark);
				markers?.Add((i, mark));
			}

			char Next(ReadOnlySpan<char> str) {
				if (i == str.Length) throw new FormatException("Unexpected end of text");
				return str[i++];
			}
			char NextSymbol(ReadOnlySpan<char> str) {
				char c;
				do {
					c = Next(str);
				} while (char.IsWhiteSpace(c));
				return c;
			}
			void SkipWhiteSpace(ReadOnlySpan<char> str) {
				while (char.IsWhiteSpace(str[i])) i++;
			}
			string NextString(ReadOnlySpan<char> str) {
				SkipWhiteSpace(str);
				int start = i;
				string s = StructuredData.ParseString(str[i..], out int len, simulate);
				i += len;
				markers?.Add((start, DataParseMarker.String));
				PopMark();
				return s;
			}
			void MatchNext(string text, ReadOnlySpan<char> str) {
				foreach(char c in text) {
					if (c != Next(str)) throw new FormatException($"Unexpected symbol at {i-1}");
				}
			}

			char c = NextSymbol(str);
			// Check if we are starting a number
			if (char.IsDigit(c) || c == '-' || c == '+') {
				MarkLastChar(DataParseMarker.Number);
				int start = i - 1;

				DataType? explicitType = null;
				bool isFloating = false;
				bool parsing = true;
				while(parsing) {
					c = Next(str);
					if (char.IsDigit(c)) continue;
					switch (c) {
							// Accept parts of floating-point numbers
						case '.':
						case 'e':
						case 'E':
							isFloating = true;
							continue;
							// Accept certain letters as type qualifiers
						case 'b':
							explicitType = DataType.Byte;
							parsing = false;
							break;
						case 'i':
							explicitType = DataType.Int;
							parsing = false;
							break;
						case 'l':
							explicitType = DataType.Long;
							parsing = false;
							break;
						case 'f':
							explicitType = DataType.Float;
							parsing = false;
							break;
						case 'd':
							explicitType = DataType.Double;
							parsing = false;
							break;
							// Else we overran the end of the number
						default:
							i--;
							parsing = false;
							break;
					}
				}
				PopMark();

				// If no explicit type was defined guess if it is floating-point or integer
				if (explicitType == null) {
					if (isFloating) explicitType = DataType.Double;
					else explicitType = DataType.Int;
				}

				// Now try to parse the number from the string
				// Do this even if simulating to check for a number format error
				ReadOnlySpan<char> numText = str[start..(i + 1)];
				length = i;
				try {
					return explicitType switch {
						DataType.Byte => byte.Parse(numText),
						DataType.Int => int.Parse(numText),
						DataType.Long => long.Parse(numText),
						DataType.Float => float.Parse(numText),
						DataType.Double => double.Parse(numText),
						_ => throw new InvalidOperationException(),// How did we get here?
					};
				} catch (FormatException ex) {
					throw new FormatException($"Invalid number format at {start}", ex);
				}
			} else {
				// Else not a number, handle based on the first character
				switch (c) {
					case '{': {
							// Mark the next bit of text as part of an object
							MarkLastChar(DataParseMarker.Object);
							nestedMarkers?.Push(DataParseMarker.Object);
							DataObject? obj = null;
							if (!simulate) obj = new DataObject();

							for(int j = 0; true; j++) {
								// Check if the object has ended or validate a comma before the next item
								c = NextSymbol(str);
								if (c == '}') break;
								else if (!(c == ',' && j != 0)) throw new FormatException($"Invalid character in object at {i-1}");

								// Get the next field name
								string name = NextString(str);

								// Validate that the next symbol is a colon
								c = NextSymbol(str);
								if (c != ':') throw new FormatException($"Invalid character in object at {i-1}");

								// Get the value and store
								DataBox value = Parse(str[i..], out int len, simulate, markers, nestedMarkers);
								if (obj != null) obj[name] = value;

								i += len;
							}

							PopMark();
							length = i;
							return obj ?? default(DataBox);
						}
					case '[': {
							// Mark the next bit of text as part of a list
							MarkLastChar(DataParseMarker.List);
							nestedMarkers?.Push(DataParseMarker.List);
							DataList? lst = null;

							for (int j = 0; true; j++) {
								// Check if the list has ended or validate a comma before the next item
								c = NextSymbol(str);
								if (c == ']') break;
								else if (!(c == ',' && j != 0)) throw new FormatException($"Invalid character in object at {i - 1}");

								// Get the value and append
								DataBox value = Parse(str[i..], out int len, simulate, markers, nestedMarkers);
								lst?.Add(value);

								i += len;
							}

							PopMark();
							length = i;
							return lst ?? default(DataBox);
						}
					case '\"': {
							// Quotes indicate a string
							i--;
							string value = NextString(str);
							length = i;
							return value;
						}
					case 't':
						// Must be 't'rue
						MarkLastChar(DataParseMarker.Boolean);
						MatchNext("rue", str);
						PopMark();
						length = i;
						return true;
					case 'f':
						// Must be 'f'alse
						MatchNext("false", str);
						length = i;
						PopMark();
						return false;
					case 'n':
						// Must be 'n'ull
						MarkLastChar(DataParseMarker.Undefined);
						MatchNext("ull", str);
						length = i;
						PopMark();
						return default;
					default:
						throw new FormatException($"Unexpected character \'{c}\' at {i - 1}");
				}
			}
		}

		public static DataBox Construct(BinaryReader br) {
			byte type = br.ReadByte();
			if (type == StructuredData.TypeTrue) return new DataBox(true, DataType.Boolean);
			if (type == StructuredData.TypeFalse) return new DataBox(false, DataType.Boolean);
			DataType dtype = (DataType)type;
			object value = dtype switch {
				DataType.None => null!,
				DataType.Byte => br.ReadByte(),
				DataType.Int => br.Read7BitEncodedInt(),
				DataType.Long => br.Read7BitEncodedInt64(),
				DataType.Float => br.ReadSingle(),
				DataType.Double => br.ReadDouble(),
				DataType.String => StructuredData.ReadString(br),
				DataType.List => DataList.Construct(br),
				DataType.Object => DataObject.Construct(br),
				DataType.ObjectStreaming => DataObjectStream.Construct(br),
				_ => throw new InvalidDataException($"Unknown data type 0x{type:X2}"),
			};
			return new DataBox(value, (DataType)type);
		}

		public void Write(BinaryWriter bw) {
			if (Type == default) throw new InvalidOperationException("DataBox does not contain valid value");
			if (Type == DataType.Boolean) {
				bw.Write((bool)Value ? StructuredData.TypeTrue : StructuredData.TypeFalse);
			} else {
				switch (Type) {
					case DataType.Byte:
						bw.Write(StructuredData.TypeByte);
						bw.Write((byte)Value);
						break;
					case DataType.Int:
						bw.Write(StructuredData.TypeInt);
						bw.Write7BitEncodedInt((int)Value);
						break;
					case DataType.Long:
						bw.Write(StructuredData.TypeLong);
						bw.Write7BitEncodedInt64((long)Value);
						break;
					case DataType.Float:
						bw.Write(StructuredData.TypeFloat);
						bw.Write((float)Value);
						break;
					case DataType.Double:
						bw.Write(StructuredData.TypeDouble);
						bw.Write((double)Value);
						break;
					case DataType.String:
						StructuredData.WriteString((string)Value, bw);
						break;
					case DataType.List:
						((DataList)Value).Write(bw);
						break;
					case DataType.Object:
						((DataObject)Value).Write(bw);
						break;
				}
			}
		}

		public static bool operator ==(DataBox left, DataBox right) => left.Equals(right);

		public static bool operator !=(DataBox left, DataBox right) => !(left == right);

		public static implicit operator DataBox(bool value) => new(value, DataType.Boolean);

		public static implicit operator DataBox(byte value) => new(value, DataType.Byte);

		public static implicit operator DataBox(int value) => new(value, DataType.Int);

		public static implicit operator DataBox(long value) => new(value, DataType.Long);

		public static implicit operator DataBox(float value) => new(value, DataType.Float);

		public static implicit operator DataBox(double value) => new(value, DataType.Double);

		public static implicit operator DataBox(string value) => new(value, DataType.String);

		public static implicit operator DataBox(DataList value) => new(value, DataType.List);

		public static implicit operator DataBox(DataObject value) => new(value, DataType.Object);

		public static explicit operator bool(DataBox box) => box.As<bool>();

		public static explicit operator byte(DataBox box) => box.As<byte>();

		public static explicit operator int(DataBox box) => box.As<int>();

		public static explicit operator long(DataBox box) => box.As<long>();

		public static explicit operator float(DataBox box) => box.As<float>();

		public static explicit operator double(DataBox box) => box.As<double>();

		public static explicit operator string(DataBox box) => box.As<string>();

		public static explicit operator DataList(DataBox box) => box.As<DataList>();

		public static explicit operator DataObject(DataBox box) => box.As<DataObject>();

	}

}

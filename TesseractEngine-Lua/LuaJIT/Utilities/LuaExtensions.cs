using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tesseract.LuaJIT.Utilities {

	public static class LuaExtensions {

		/// <summary>
		/// Tests if the value on the stack is a proper Lua array. The value is an array if:
		/// <list type="bullet">
		/// <item>It is a table</item>
		/// <item>The table is non-empty</item>
		/// <item>All keys are integers</item>
		/// <item>The keys are all in the range of 1 to the size of the table, inclusive</item>
		/// </list>
		/// Note that this is an O(n) operation as the entire table must be iterated to check
		/// all possible keys due to how Lua tables operate internally.
		/// </summary>
		/// <param name="lua">This Lua runtime</param>
		/// <param name="index">Stack index of value to test</param>
		/// <returns>If the value is a proper array</returns>
		public static bool IsArray(this LuaBase lua, int index) {
			// Array must be a table
			if (!lua.IsTable(index)) return false;

			// Track the index count and number of indices
			int count = 0, maxn = 0;
			lua.PushNil();
			while(lua.Next(index)) {
				// All keys must be numbers
				if (!lua.IsNumber(-2)) {
					lua.Pop(2);
					return false;
				}
				double nd = lua.ToNumber(-2);
				// All keys must be integers
				if (nd % 1 < double.Epsilon) {
					lua.Pop(2);
					return false;
				}
				int n = (int)nd;
				// All keys must be >0
				if (n < 1) {
					lua.Pop(2);
					return false;
				}
				lua.Pop(1);
				// Track max key value and total item count
				maxn = Math.Max(maxn, n);
				count++;
			}

			// Array must have one or more elements and they must all be at integer indices
			return count != 0 && count == maxn;
		}

		/// <summary>
		/// Dupliates the top value of the stack <i>n</i> times.
		/// </summary>
		/// <param name="lua">This Lua state</param>
		/// <param name="n">The number of times to duplicate the value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Dup(this LuaBase lua, int n = 1) {
			while (n-- > 0) lua.PushValue(-1);
		}

		/// <summary>
		/// Registers a function with the given name under the given package, creating the package if it does not exist.
		/// </summary>
		/// <param name="lua">This Lua state</param>
		/// <param name="package">The package name</param>
		/// <param name="name">The function name</param>
		/// <param name="func">The function to register</param>
		public static void Register(this LuaBase lua, ReadOnlySpan<byte> package, ReadOnlySpan<byte> name, LuaFunction func) {
			lua.GetGlobal(package);
			if (lua.IsNil(-1)) {
				lua.Pop(1);
				lua.CreateTable();
				lua.Dup();
				lua.SetGlobal(package);
			}
			lua.PushFunction(func);
			lua.SetField(-2, name);
			lua.Pop(1);
		}

		/// <summary>
		/// Registers a function with the given name under the given package, creating the package if it does not exist.
		/// </summary>
		/// <param name="lua">This Lua state</param>
		/// <param name="package">The package name</param>
		/// <param name="name">The function name</param>
		/// <param name="func">The function to register</param>
		public static void Register(this LuaBase lua, ReadOnlySpan<char> package, ReadOnlySpan<char> name, LuaFunction func) {
			lua.GetGlobal(package);
			if (lua.IsNil(-1)) {
				lua.Pop(1);
				lua.CreateTable();
				lua.Dup();
				lua.SetGlobal(package);
			}
			lua.PushFunction(func);
			lua.SetField(-2, name);
			lua.Pop(1);
		}

		/// <summary>
		/// Registers a set of named functions under the given package, creating the package if it does not exist.
		/// </summary>
		/// <param name="lua">This Lua state</param>
		/// <param name="package">The package name</param>
		/// <param name="funcs">The functions to register</param>
		public static void Register(this LuaBase lua, ReadOnlySpan<byte> package, IEnumerable<KeyValuePair<string, LuaFunction>> funcs) {
			lua.GetGlobal(package);
			if (lua.IsNil(-1)) {
				lua.Pop(1);
				lua.CreateTable();
				lua.Dup();
				lua.SetGlobal(package);
			}
			foreach (var func in funcs) {
				lua.PushFunction(func.Value);
				lua.SetField(-2, func.Key);
			}
			lua.Pop(1);
		}

		/// <summary>
		/// Registers a set of named functions under the given package, creating the package if it does not exist.
		/// </summary>
		/// <param name="lua">This Lua state</param>
		/// <param name="package">The package name</param>
		/// <param name="funcs">The functions to register</param>
		public static void Register(this LuaBase lua, ReadOnlySpan<char> package, IEnumerable<KeyValuePair<string, LuaFunction>> funcs) {
			lua.GetGlobal(package);
			if (lua.IsNil(-1)) {
				lua.Pop(1);
				lua.CreateTable();
				lua.Dup();
				lua.SetGlobal(package);
			}
			foreach (var func in funcs) {
				lua.PushFunction(func.Value);
				lua.SetField(-2, func.Key);
			}
			lua.Pop(1);
		}

		private static int JsonDecode(LuaBase lua, JsonElement element) {
			switch (element.ValueKind) {
				case JsonValueKind.Object: {
						lua.CreateTable();
						foreach(var elem in element.EnumerateObject()) {
							lua.PushString(elem.Name);
							JsonDecode(lua, elem.Value);
							lua.RawSet(-3);
						}
					}
					break;
				case JsonValueKind.Array: {
						lua.CreateTable(narr: element.GetArrayLength());
						int index = 1;
						foreach(var elem in element.EnumerateArray()) {
							JsonDecode(lua, element);
							lua.RawSet(-1, index++);
						}
					}
					break;
				case JsonValueKind.String:
					lua.PushString(element.GetString());
					break;
				case JsonValueKind.Number:
					lua.PushNumber(element.GetDouble());
					break;
				case JsonValueKind.True:
					lua.PushBoolean(true);
					break;
				case JsonValueKind.False:
					lua.PushBoolean(false);
					break;
				case JsonValueKind.Undefined:
				case JsonValueKind.Null:
					break;
			}
			return 1;
		}

		private static void JsonEncode(LuaBase lua, int value, Utf8JsonWriter writer) {
			LuaType type = lua.Type(value);
			switch (type) {
				case LuaType.None:
				case LuaType.Nil:
					writer.WriteNullValue();
					break;
				case LuaType.Boolean:
					writer.WriteBooleanValue(lua.ToBoolean(value));
					break;
				case LuaType.Number:
					writer.WriteNumberValue(lua.ToNumber(value));
					break;
				case LuaType.String:
					writer.WriteStringValue(lua.ToString(value));
					break;
				case LuaType.Table:
					value = lua.ToAbsoluteIndex(value);
					if (lua.IsArray(value)) {
						writer.WriteStartArray();
						lua.PushNil();
						while (lua.Next(value)) {
							JsonEncode(lua, -1, writer);
							lua.Pop(1);
						}
						writer.WriteEndArray();
					} else {
						writer.WriteStartObject();
						lua.PushNil();
						while(lua.Next(value)) {
							if (!lua.IsString(-2)) throw new LuaException("Cannot encode object with non-string key");
							writer.WritePropertyName(lua.ToString(-2)!);
							JsonEncode(lua, -1, writer);
							lua.Pop(1);
						}
						writer.WriteEndObject();
					}
					break;
				case LuaType.LightUserData:
				case LuaType.Function:
				case LuaType.UserData:
				case LuaType.Thread:
				default:
					throw new LuaException($"Encountered unencodable value of type {type}");
			}
		}

		/// <summary>
		/// Adds a JSON package for encoding and decoding Lua objects:
		/// <list type="table">
		/// 
		/// <item>
		/// <term><c>json.encode(value: any, [options: string...]) -> string</c></term>
		/// <description>
		/// Encodes a Lua value as a string of JSON text. Any additional arguments must be strings
		/// which specify how to encode the JSON, recognized arguments are:
		/// <list type="table">
		/// <item><term>pretty</term><description>The JSON will be "pretty printed" with more readable formatting.</description></item>
		/// </list>
		/// </description>
		/// </item>
		/// 
		/// <item>
		/// <term><c>json.decode(text: string) -> any</c></term>
		/// <description>Decodes a string of JSON text into a corresponding Lua value.</description>
		/// </item>
		/// 
		/// </list>
		/// </summary>
		/// <param name="lua">This Lua state</param>
		public static void OpenJSON(this LuaBase lua) {
			Register(lua, "json"u8, "encode"u8, state => {
				JsonWriterOptions opts = default;
				int argc = state.Top;
				for(int i = 2; i < argc; i++) {
					string opt = lua.CheckString(i);
					switch (opt) {
						case "pretty":
							opts = opts with { Indented = true };
							break;
						default:
							throw new LuaException($"Unrecognized encoding option \"{opt}\"");
					}
				}

				using MemoryStream ms = new();
				Utf8JsonWriter writer = new(ms, opts);

				state.PushString(ms.ToArray());
				return 1;
			});

			Register(lua, "json"u8, "decode"u8, state => {
				ReadOnlySpan<byte> jsonString = state.CheckStringBytes(1);
				Utf8JsonReader reader = new(jsonString);
				if (JsonElement.TryParseValue(ref reader, out JsonElement? json) && json != null) {
					JsonDecode(state, json.Value);
					return 1;
				} else throw new LuaException("Unexpected end of JSON");
			});
		}

		/// <summary>
		/// Enumeration of tokens for serialized data.
		/// </summary>
		public enum SerialToken : byte {
			/// <summary>
			/// End token, indicates the end of the data stream or the end of a table.
			/// </summary>
			End =       (byte)'e',
			/// <summary>
			/// Nil value token.
			/// </summary>
			Nil =       (byte)'n',
			/// <summary>
			/// True boolean value token.
			/// </summary>
			True =      (byte)'t',
			/// <summary>
			/// False boolean value token.
			/// </summary>
			False =     (byte)'f',
			/// <summary>
			/// Token for a number packed as a 7-bit integer.
			/// </summary>
			PackedInt = (byte)'i',
			/// <summary>
			/// Token for a number stored as a double.
			/// </summary>
			Double =    (byte)'d',
			/// <summary>
			/// String value token.
			/// </summary>
			String =    (byte)'s',
			/// <summary>
			/// Table value token.
			/// </summary>
			Table =     (byte)'T',
			/// <summary>
			/// Custom value token.
			/// </summary>
			Custom =    (byte)'C'
		}

		/// <summary>
		/// The maximum size a piece of binary data is allowed to be for extension functions. This can be
		/// used to prevent a malicious user from trying to allocate an arbitrary amount of memory by crafting
		/// a specific formatting or serialized string.
		/// </summary>
		public static int MaxDataSize { get; set; } = int.MaxValue;

		/// <summary>
		/// Serializes the Lua value at the given stack index into binary data using a <see cref="BinaryWriter"/>.
		/// </summary>
		/// <param name="lua">The Lua interface</param>
		/// <param name="bw">The writer to serialize data to</param>
		/// <param name="index">The stack index of the value to serialize</param>
		/// <exception cref="LuaException">If there was an error during serialization</exception>
		public static void Serialize(this LuaBase lua, BinaryWriter bw, int index) {
			bool TrySerializeCustom(int index) {
				bool isCustom = false;
				if (lua.GetMetatable(index)) {
					lua.GetField(-1, "__serialize"u8);
					if (!(isCustom = lua.IsFunction(-1))) lua.Pop();
				}

				if (isCustom) {
					// Call the __serialize metamethod with the object, returns a key and data string
					lua.PushValue(index);
					lua.Call(1, 2);
					if (!(lua.IsString(-2) && lua.IsString(-1)))
						throw new LuaException("Custom serialization function must return key and data as strings");
					// Write data to the stream
					bw.Write((byte)SerialToken.Custom);
					ReadOnlySpan<byte> key = lua.ToStringBytes(-2);
					if (key.Length > MaxDataSize) throw new LuaException("Custom value key too large");
					bw.Write7BitEncodedInt(key.Length);
					bw.Write(key);
					ReadOnlySpan<byte> data = lua.ToStringBytes(-1);
					if (data.Length > MaxDataSize) throw new LuaException("Custom value data too large");
					bw.Write7BitEncodedInt(data.Length);
					bw.Write(data);
					return true;
				} else return false;
			}

			switch (lua.Type(index)) {
				case LuaType.Nil:
					bw.Write((byte)SerialToken.Nil);
					break;
				case LuaType.Boolean:
					if (lua.ToBoolean(index)) bw.Write((byte)SerialToken.True);
					else bw.Write((byte)SerialToken.False);
					break;
				case LuaType.Number: {
						double num = lua.ToNumber(index);
						if (double.IsInteger(num) && num >= int.MinValue && num <= int.MaxValue) {
							bw.Write((byte)SerialToken.PackedInt);
							bw.Write7BitEncodedInt((int)num);
						} else {
							bw.Write((byte)SerialToken.Double);
							bw.Write(num);
						}
					}
					break;
				case LuaType.String: {
						ReadOnlySpan<byte> strBytes = lua.ToStringBytes(index);
						if (strBytes.Length > MaxDataSize) throw new LuaException("String too large");
						// <Token:B> <Length:7bI> <Data:nB>
						bw.Write((byte)SerialToken.String);
						bw.Write7BitEncodedInt(strBytes.Length);
						bw.Write(strBytes);
					}
					break;
				case LuaType.Table: {
						index = lua.ToAbsoluteIndex(index);
						int top = lua.Top;
						try {
							// Try to use the custom serialization function, else fall back to default
							// method for serializing tables
							if (!TrySerializeCustom(index)) {
								// <Token:B> [<Key:?> <Value:?]... <End:B>
								bw.Write((byte)SerialToken.Table);
								lua.PushNil();
								while (lua.Next(index)) {
									Serialize(lua, bw, -2);
									Serialize(lua, bw, -1);
									lua.Pop();
								}
								bw.Write((byte)SerialToken.End);
							}
						} finally {
							lua.Top = top;
						}
					}
					break;
				case LuaType.UserData: {
						index = lua.ToAbsoluteIndex(index);
						int top = lua.Top;
						try {
							if (!TrySerializeCustom(index))
								throw new LuaException("Cannot serialize userdata without a custom serialization function");
						} finally {
							lua.Top = top;
						}
					} 
					break;
				case LuaType.None:
				case LuaType.LightUserData:
				case LuaType.Function:
				case LuaType.Thread:
				default:
					throw new LuaException("Encountered unserializable value");
			}
			// Write a trailing end token just in case
			bw.Write((byte)SerialToken.End);
		}

		/// <summary>
		/// Deserializes a value from serialized binary data, pushing it onto the stack And
		/// returning true. If the end of data is reached, nothing is pushed and false is returned.
		/// </summary>
		/// <param name="lua">The Lua interface</param>
		/// <param name="br">The reader for serialized data</param>
		/// <returns>If a value was deserialized and pushed</returns>
		/// <exception cref="IOException">If there is an error reading the serialized data</exception>
		/// <exception cref="LuaException">If there is an error decoding the serialized data</exception>
		public static bool Deserialize(this LuaBase lua, BinaryReader br) {
			void ReadFully(Span<byte> span) {
				int offset = 0;
				do {
					int nread = br.Read(span[offset..]);
					if (nread == 0) throw new IOException("Unexpected end of serialized data");
					offset += nread;
				} while (offset < span.Length);
			}

			switch ((SerialToken)br.ReadByte()) {
				case SerialToken.Nil:
					lua.PushNil();
					break;
				case SerialToken.True:
					lua.PushBoolean(true);
					break;
				case SerialToken.False:
					lua.PushBoolean(false);
					break;
				case SerialToken.PackedInt:
					lua.PushInteger(br.Read7BitEncodedInt());
					break;
				case SerialToken.Double:
					lua.PushNumber(br.ReadDouble());
					break;
				case SerialToken.String: {
						int length = br.Read7BitEncodedInt();
						if (length > MaxDataSize) throw new LuaException("String has excessively large size");
						Span<byte> buffer = length > 4096 ? new byte[length] : stackalloc byte[length];
						ReadFully(buffer);
						lua.PushString(buffer);
					}
					break;
				case SerialToken.Table: {
						int top = lua.Top;
						try {
							lua.CreateTable();
							while (Deserialize(lua, br)) {
								if (!Deserialize(lua, br)) throw new IOException("Table ends prematurely");
								lua.RawSet(-3);
							}
							top++;
						} finally {
							lua.Top = top;
						}
					}
					break;
				case SerialToken.Custom: {
						int top = lua.Top;
						try {
							int keyLen = br.Read7BitEncodedInt();
							if (keyLen > MaxDataSize) throw new LuaException("Custom data has excessively large key");
							Span<byte> key = keyLen > 4096 ? new byte[keyLen] : stackalloc byte[keyLen];
							ReadFully(key);

							bool hasDeserial = false;

							lua.GetField(Lua.RegistryIndex, "__deserial"u8);
							if (lua.IsTable(-1)) {
								lua.PushString(key);
								lua.RawGet(-2);
								hasDeserial = lua.IsFunction(-1);
							}

							if (!hasDeserial) throw new LuaException($"Missing deserializer for custom key \"{Encoding.UTF8.GetString(key)}\"");

							int dataLen = br.Read7BitEncodedInt();
							if (dataLen > MaxDataSize) throw new LuaException("Custom data is excessively large");
							Span<byte> data = dataLen > 4096 ? new byte[dataLen] : stackalloc byte[dataLen];
							ReadFully(data);

							lua.Remove(-2);
							lua.PushString(data);
							lua.Call(1, 1);
						} finally {
							lua.Top = top;
						}
					}
					break;
				case SerialToken.End:
				default:
					return false;
			}
			return true;
		}

		/// <summary>
		/// Adds a custom deserialization handler like with <c>serialize.newdecoder()</c> (see
		/// <see cref="OpenSerialize(LuaBase)"/>).
		/// </summary>
		/// <param name="lua">The Lua interface</param>
		/// <param name="func">The serialization function</param>
		public static void AddCustomDeserializer(LuaBase lua, string key, LuaFunction func) {
			lua.GetField(Lua.RegistryIndex, "__deserial"u8);
			if (lua.IsNil(-1)) {
				lua.Pop();
				lua.CreateTable();
				lua.Dup();
				lua.SetField(Lua.RegistryIndex, "__deserial"u8);
			}
			lua.PushFunction(func);
			lua.SetField(-2, key);
			lua.Pop();
		}

		/// <summary>
		/// Adds a serialization package for converting between Lua values and binary data:
		/// <list type="table">
		/// 
		/// <item>
		/// <term><c>serialize.encode(value: any) -> string</c></term>
		/// <description>Encodes a Lua value as serialized binary data. Userdata and tables
		/// may support custom serialization via the <c>__serialize</c> metamethod. If present,
		/// this is called with the object being serialized and must return a unique key string
		/// followed by the serialized data packed in a string.</description>
		/// </item>
		/// 
		/// <item>
		/// <term><c>serialize.decode(data: string) -> any</c></term>
		/// <description>Decodes a string of JSON text into a corresponding Lua value.</description>
		/// </item>
		/// 
		/// <item>
		/// <term><c>serialize.newdecoder(key: string, func: function)</c></term>
		/// <description>Adds a custom deserializer for custom data with the given key. The deserialization
		/// function is called with a string storing the custom data and must return the deserialized
		/// data type.</description>
		/// </item>
		/// 
		/// </list>
		/// </summary>
		/// <param name="lua">This Lua state</param>
		public static void OpenSerialize(this LuaBase lua) {
			Register(lua, "serialize"u8, "encode"u8, state => {
				using MemoryStream ms = new();
				Serialize(state, new BinaryWriter(ms), 1);
				state.PushString(ms.GetBuffer());
				return 1;
			});
			Register(lua, "serialize"u8, "decode"u8, state => {
				ReadOnlySpan<byte> data = state.CheckStringBytes(1);
				using MemoryStream ms = new(data.ToArray());
				if (!Deserialize(lua, new BinaryReader(ms))) state.PushNil();
				return 1;
			});
			Register(lua, "serialize"u8, "newdecoder"u8, state => {
				state.CheckStringBytes(1);
				if (state.IsFunction(2)) state.TypeError(2, "function");
				state.GetField(Lua.RegistryIndex, "__deserial"u8);
				if (lua.IsNil(-1)) {
					lua.CreateTable();
					lua.Dup();
					lua.SetField(Lua.RegistryIndex, "__deserial"u8);
				}
				lua.PushValue(1);
				lua.PushValue(2);
				lua.RawSet(-3);
				return 0;
			});
		}

		private static int StructPack(LuaBase lua) {
			string format = lua.CheckString(1);
			using MemoryStream ms = new();
			BinaryWriter bw = new(ms);

			// The index of the next argument to pack
			int arg = 2;
			// If the next value should be written in little endian
			bool isLittleEndian = BitConverter.IsLittleEndian;
			// If the next value should be aligned to its size
			bool isAligned = false;
			// String buffer for count digits
			StringBuilder countDigits = new();

			// Processes alignment of values
			void AlignTo(int size) {
				bw.Flush();
				long offset = ms.Position;
				if (isAligned) {
					int diff = (int)(offset % size);
					if (diff > 0) {
						Span<byte> zeros = stackalloc byte[size - diff];
						zeros.Clear();
						bw.Write(zeros);
					}
				}
			}

			// Processes count digits
			int CheckCount() {
				// If no digits, only a single value
				if (countDigits.Length == 0) return 1;
				// Try to parse the count
				string str = countDigits.ToString();
				countDigits.Clear();
				int count = int.Parse(str);
				if (count > MaxDataSize) throw new ArgumentException("Format string has excessively large count specifier");
				return count;
			}

			// Fetches the next number
			double NextNumber() => lua.CheckNumber(arg++);

			// Fetches the next signed integer
			long NextInt(long min, long max, string name) {
				double x = NextNumber();
				if (!double.IsInteger(x)) throw new LuaException($"Value #{arg - 1} is not an integer");
				if (x > max || x < min) throw new LuaException($"Value #{arg - 1} does not fit in the range of a {name}");
				return (long)x;
			}

			// Fetches the next unsigned integer 
			ulong NextUInt(ulong max, string name) {
				double x = NextNumber();
				if (!double.IsInteger(x)) throw new LuaException($"Value #{arg - 1} is not an integer");
				if (x > max || x < 0) throw new LuaException($"Value #{arg - 1} does not fit in the range of a {name}");
				return (ulong)x;
			}

			Span<byte> zeros = stackalloc byte[256];
			zeros.Clear();

			foreach (char c in format) {
				switch (c) {
					case '@': // Native byte order, native size & align
						isLittleEndian = BitConverter.IsLittleEndian;
						isAligned = true;
						countDigits.Clear();
						break;
					case '=': // Native byte order, standard size, no align
						isLittleEndian = BitConverter.IsLittleEndian;
						isAligned = false;
						countDigits.Clear();
						break;
					case '<': // Little endian, standard size, no align
						isLittleEndian = true;
						isAligned = false;
						countDigits.Clear();
						break;
					case '>': // Big endian, standard size, no align
					case '!': // Network (= big endian)
						isLittleEndian = false;
						isAligned = false;
						countDigits.Clear();
						break;
					case 'b': // signed char (byte)
						for (int i = CheckCount(); i > 0; i--)
							bw.Write((byte)NextInt(0, byte.MaxValue, "unsigned byte"));
						break;
					case 'B': // unsigned char (byte)
						for (int i = CheckCount(); i > 0; i--)
							bw.Write((byte)NextUInt(byte.MaxValue, "unsigned byte"));
						break;
					case '?': // _Bool (bool)
						for (int i = CheckCount(); i > 0; i--)
							bw.Write((byte)(lua.CheckBoolean(arg++) ? 1 : 0));
						break;
					case 'h': // short
						AlignTo(2);
						for (int i = CheckCount(); i > 0; i--) {
							short value = (short)NextInt(short.MinValue, short.MaxValue, "signed short");
							if (!isLittleEndian) value = BinaryPrimitives.ReverseEndianness(value);
							bw.Write(value);
						}
						break;
					case 'H': // unsigned short
						AlignTo(2);
						for (int i = CheckCount(); i > 0; i--) {
							ushort value = (ushort)NextUInt(ushort.MaxValue, "unsigned short");
							if (!isLittleEndian) value = BinaryPrimitives.ReverseEndianness(value);
							bw.Write(value);
						}
						break;
					case 'i': // int
						AlignTo(4);
						for (int i = CheckCount(); i > 0; i--) {
							int value = (int)NextInt(int.MinValue, int.MaxValue, "signed int");
							if (!isLittleEndian) value = BinaryPrimitives.ReverseEndianness(value);
							bw.Write(value);
						}
						break;
					case 'I': // unsigned int
						AlignTo(4);
						for (int i = CheckCount(); i > 0; i--) {
							uint value = (uint)NextUInt(uint.MaxValue, "unsigned int");
							if (!isLittleEndian) value = BinaryPrimitives.ReverseEndianness(value);
							bw.Write(value);
						}
						break;
					case 'q': // long long
						AlignTo(8);
						for (int i = CheckCount(); i > 0; i--) {
							long value = NextInt(long.MinValue, long.MaxValue, "signed long");
							if (!isLittleEndian) value = BinaryPrimitives.ReverseEndianness(value);
							bw.Write(value);
						}
						break;
					case 'Q': // unsigned long long
						AlignTo(8);
						for (int i = CheckCount(); i > 0; i--) {
							ulong value = NextUInt(ulong.MaxValue, "unsigned long");
							if (!isLittleEndian) value = BinaryPrimitives.ReverseEndianness(value);
							bw.Write(value);
						}
						break;
					case 'f': // float
						AlignTo(4);
						for (int i = CheckCount(); i > 0; i--) {
							double x = NextNumber();
							if (x > float.MaxValue || x < float.MinValue)
								throw new LuaException($"Value #{arg - 1} does not fit in the range of a float");
							int value = BitConverter.SingleToInt32Bits((float)x);
							if (!isLittleEndian) value = BinaryPrimitives.ReverseEndianness(value);
							bw.Write(value);
						}
						break;
					case 'd': // double
						AlignTo(8);
						for (int i = CheckCount(); i > 0; i--) {
							long value = BitConverter.DoubleToInt64Bits(NextNumber());
							if (!isLittleEndian) value = BinaryPrimitives.ReverseEndianness(value);
							bw.Write(value);
						}
						break;
					case 'x': // padding byte
						for(int i = CheckCount(); i > 0; i--)
							bw.Write(0);
						break;
					case 's': { // fixed-size string
							int count = CheckCount();
							ReadOnlySpan<byte> str = lua.CheckStringBytes(arg++);
							if (str.Length < count) {
								bw.Write(str);
								int diff = count - str.Length;
								while(diff > 0) {
									int nwrite = Math.Min(diff, zeros.Length);
									bw.Write(zeros[..nwrite]);
									diff -= nwrite;
								}
							} else bw.Write(str[..count]);
						}
						break;
					default:
						// Skip any whitespace
						if (char.IsWhiteSpace(c)) continue;
						if (char.IsAsciiDigit(c)) {
							countDigits.Append(c);
							continue;
						}
						throw new LuaException($"Invalid struct format character '{c}'");
				}
			}

			lua.PushString(ms.ToArray());
			return 1;
		}

		private static int StructUnpack(LuaBase lua) {
			string format = lua.CheckString(1);
			ReadOnlySpan<byte> data = lua.CheckStringBytes(2);
			int offset = 0;
			int top = lua.Top;

			// If the next value should be written in little endian
			bool isLittleEndian = BitConverter.IsLittleEndian;
			// If the next value should be aligned to its size
			bool isAligned = false;
			// String buffer for count digits
			StringBuilder countDigits = new();

			// Processes alignment of values
			void AlignTo(int size) {
				if (isAligned) {
					int diff = offset % size;
					if (diff > 0) offset += size - diff;
				}
			}

			// Processes count digits
			int CheckCount() {
				// If no digits, only a single value
				if (countDigits.Length == 0) return 1;
				// Try to parse the count
				string str = countDigits.ToString();
				countDigits.Clear();
				int count = int.Parse(str);
				if (count > MaxDataSize) throw new ArgumentException("Format string has excessively large count specifier");
				return count;
			}

			foreach (char c in format) {
				switch (c) {
					case '@': // Native byte order, native size & align
						isLittleEndian = BitConverter.IsLittleEndian;
						isAligned = true;
						countDigits.Clear();
						break;
					case '=': // Native byte order, standard size, no align
						isLittleEndian = BitConverter.IsLittleEndian;
						isAligned = false;
						countDigits.Clear();
						break;
					case '<': // Little endian, standard size, no align
						isLittleEndian = true;
						isAligned = false;
						countDigits.Clear();
						break;
					case '>': // Big endian, standard size, no align
					case '!': // Network (= big endian)
						isLittleEndian = false;
						isAligned = false;
						countDigits.Clear();
						break;
					case 'b': // signed char (byte)
						for (int i = CheckCount(); i > 0; i--) lua.PushInteger((sbyte)data[offset++]);
						break;
					case 'B': // unsigned char (byte)
						for (int i = CheckCount(); i > 0; i--) lua.PushInteger(data[offset++]);
						break;
					case '?': // _Bool (bool)
						for (int i = CheckCount(); i > 0; i--) lua.PushBoolean(data[offset++] != 0);
						break;
					case 'h': // short
						AlignTo(2);
						for (int i = CheckCount(); i > 0; i--, offset += 2)
							lua.PushInteger(isLittleEndian ? BinaryPrimitives.ReadInt16LittleEndian(data[offset..]) : BinaryPrimitives.ReadInt16BigEndian(data[offset..]));
						break;
					case 'H': // unsigned short
						AlignTo(2);
						for (int i = CheckCount(); i > 0; i--, offset += 2)
							lua.PushInteger(isLittleEndian ? BinaryPrimitives.ReadUInt16LittleEndian(data[offset..]) : BinaryPrimitives.ReadUInt16BigEndian(data[offset..]));
						break;
					case 'i': // int
						AlignTo(4);
						for (int i = CheckCount(); i > 0; i--, offset += 4)
							lua.PushInteger(isLittleEndian ? BinaryPrimitives.ReadInt32LittleEndian(data[offset..]) : BinaryPrimitives.ReadInt32BigEndian(data[offset..]));
						break;
					case 'I': // unsigned int
						AlignTo(4);
						for (int i = CheckCount(); i > 0; i--, offset += 4) {
							uint x = isLittleEndian ? BinaryPrimitives.ReadUInt32LittleEndian(data[offset..]) : BinaryPrimitives.ReadUInt32BigEndian(data[offset..]);
							if (x > nint.MaxValue) lua.PushNumber(x);
							else lua.PushInteger((nint)x);
						}
						break;
					case 'q': // long long
						AlignTo(8);
						for (int i = CheckCount(); i > 0; i--, offset += 8) {
							long x = isLittleEndian ? BinaryPrimitives.ReadInt64LittleEndian(data[offset..]) : BinaryPrimitives.ReadInt64BigEndian(data[offset..]);
							if (x < nint.MinValue || x > nint.MaxValue) lua.PushNumber(x);
							else lua.PushInteger((nint)x);
						}
						break;
					case 'Q': // unsigned long long
						AlignTo(8);
						for (int i = CheckCount(); i > 0; i--, offset += 8) {
							ulong x = isLittleEndian ? BinaryPrimitives.ReadUInt64LittleEndian(data[offset..]) : BinaryPrimitives.ReadUInt64BigEndian(data[offset..]);
							if (x > (ulong)nint.MaxValue) lua.PushNumber(x);
							else lua.PushInteger((nint)x);
						}
						break;
					case 'f': // float
						AlignTo(4);
						for (int i = CheckCount(); i > 0; i--, offset += 4) {
							int x = isLittleEndian ? BinaryPrimitives.ReadInt32LittleEndian(data[offset..]) : BinaryPrimitives.ReadInt32BigEndian(data[offset..]);
							lua.PushNumber(BitConverter.Int32BitsToSingle(x));
						}
						break;
					case 'd': // double
						AlignTo(8);
						for (int i = CheckCount(); i > 0; i--, offset += 8) {
							long x = isLittleEndian ? BinaryPrimitives.ReadInt64LittleEndian(data[offset..]) : BinaryPrimitives.ReadInt64BigEndian(data[offset..]);
							lua.PushNumber(BitConverter.Int64BitsToDouble(x));
						}
						break;
					case 'x': // padding byte
						offset += CheckCount();
						break;
					case 's': { // fixed-size string
							int count = CheckCount();
							lua.PushString(data[offset..(offset + count)]);
							offset += count;
						}
						break;
					default:
						// Skip any whitespace
						if (char.IsWhiteSpace(c)) continue;
						if (char.IsAsciiDigit(c)) {
							countDigits.Append(c);
							continue;
						}
						throw new LuaException($"Invalid struct format character '{c}'");
				}
			}

			return lua.Top - top;
		}

		/// <summary>
		/// Adds a structure (un)packing package for converting between Lua values and packed binary data. This is
		/// heavily based on the <c>struct</c> Python library with the exception that the <b>c</b>, <b>l</b>, <b>L</b>,
		/// <b>n</b>, <b>N</b>, <b>p</b>, and <b>P</b> formats are not supported. <b>c</b> is redundant as Lua uses
		/// strings as its byte array object. <b>l</b>, <b>L</b>, <b>n</b>, <b>N</b>, and <b>P</b> are not supported due to
		/// potential complications with the <see cref="nint"/> type (this may change in the future). <b>p</b> is not supported as
		/// Pascal strings are not common enough to warrant use. See the <see href="https://docs.python.org/3/library/struct.html">
		/// Python documentation</see> for apropriate usage.
		/// <list type="table">
		/// 
		/// <item>
		/// <term><c>struct.pack(format: string, ...) -> string</c></term>
		/// <description>Packs the trailing arguments into the binary structure described by the format string. The passed values
		/// are validated to be the correct type and within the apropriate range for their corresponding binary type.</description>
		/// </item>
		/// 
		/// <item>
		/// <term><c>struct.unpack(format: string, data: string) -> ...</c></term>
		/// <description>Unpacks the binary data stored in the data string according to the format string, returning as a variable
		/// number of values.</description>
		/// </item>
		/// 
		/// </list>
		/// </summary>
		/// <param name="lua"></param>
		public static void OpenStruct(this LuaBase lua) {
			lua.Register("struct"u8, "pack"u8, StructPack);
			lua.Register("struct"u8, "unpack"u8, StructUnpack);
		}

	}

}

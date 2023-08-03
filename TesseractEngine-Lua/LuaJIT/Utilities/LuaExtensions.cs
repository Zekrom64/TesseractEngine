using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
		public static bool IsArray(this LuaState lua, int index) {
			// Array must be a table
			if (lua.Type(index) != LuaType.Table) return false;

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
		public static void Dup(this LuaState lua, int n = 1) {
			while (n-- > 0) lua.PushValue(-1);
		}

		/// <summary>
		/// Registers a function with the given name under the given package, creating the package if it does not exist.
		/// </summary>
		/// <param name="lua">This Lua state</param>
		/// <param name="package">The package name</param>
		/// <param name="name">The function name</param>
		/// <param name="func">The function to register</param>
		public static void Register(this LuaState lua, ReadOnlySpan<byte> package, ReadOnlySpan<byte> name, LuaFunction func) {
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
		public static void Register(this LuaState lua, ReadOnlySpan<char> package, ReadOnlySpan<char> name, LuaFunction func) {
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
		public static void Register(this LuaState lua, ReadOnlySpan<byte> package, IEnumerable<KeyValuePair<string, LuaFunction>> funcs) {
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
		public static void Register(this LuaState lua, ReadOnlySpan<char> package, IEnumerable<KeyValuePair<string, LuaFunction>> funcs) {
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

		private static int JsonDecode(LuaState lua, JsonElement element) {
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

		private static void JsonEncode(LuaState lua, int value, Utf8JsonWriter writer) {
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
		/// <param name="lua"></param>
		public static void OpenJSON(this LuaState lua) {
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

	}

}

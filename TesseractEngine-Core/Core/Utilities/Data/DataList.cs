using SixLabors.Fonts.Unicode;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Schema;
using Tesseract.Core.Native;

namespace Tesseract.Core.Utilities.Data {

	/// <summary>
	/// <para>A list of generic data, compatible with <see cref="DataBox"/>.</para>
	/// <para>
	/// Lists may be <i>uniform</i> (all elements are the same type) or <i>non-uniform</i>
	/// (there are mixed types of elements). This can be reflected using <see cref="UniformType"/>
	/// and <see cref="IsUniformType"/>. If the list is uniform it can be retrieved as
	/// a <see cref="Span{T}"/> of the apropriate type. If it is non-uniform it can only
	/// be manipulated with the standard methods of <see cref="IList{T}"/>.
	/// </para>
	/// </summary>
	public class DataList :
		IEquatable<DataList>,
		IEnumerable<DataBox>,
		IList<DataBox>,
		IReadOnlyList<DataBox>,
		IConstructibleData<DataList> {


		// List of non-uniform entries
		private List<DataBox>? nonUniformList = null;

		// Array of uniform entries
		private Array? uniformList = null;
		// The real number of values in the uniform array
		private int uniformListCount = 0;

		// The type of uniform values in the list
		// If None, values are known non-uniform, if null the state is not known
		private DataType? uniformType = null;

		/// <summary>
		/// The data type of all of the list elements if they are all of the same type, otherwise
		/// the value is <see cref="DataType.None"/>. 
		/// </summary>
		public DataType UniformType {
			get {
				if (uniformType == null) {
					// Only check if all entries in the non-uniform list are the same type
					if (nonUniformList != null && nonUniformList.Count > 0) {
						DataType type = nonUniformList[0].Type;
						for (int i = 1; i < nonUniformList.Count; i++) {
							if (nonUniformList[i].Type != type) {
								type = DataType.None;
								break;
							}
						}
						uniformType = type;
					} else uniformType = DataType.None;
				}
				return uniformType.Value;
			}
		}

		/// <summary>
		/// Gets if the list has a uniform data type for all elements.
		/// </summary>
		public bool IsUniformType => UniformType != DataType.None;

		public int Count => nonUniformList != null ? nonUniformList.Count : uniformListCount;

		public bool IsReadOnly => false;


		public DataBox this[int index] {
			get {
				if (uniformList != null) {
					if (index >= uniformListCount) throw new IndexOutOfRangeException();
					return new DataBox(uniformList.GetValue(index), UniformType);
				} else if (nonUniformList != null) {
					return nonUniformList[index];
				} else throw new IndexOutOfRangeException();
			}
			set {
				hashCode = null;
				asString = null;

				// Copy to list if the value would break uniformity
				if (uniformList != null && UniformType != value.Type) MakeNonUniform();

				if (uniformList != null) {
					if (index >= uniformListCount) throw new IndexOutOfRangeException();
					uniformList.SetValue(value.Value, index);
				} else if (nonUniformList != null) {
					nonUniformList[index] = value;
					// Invalidate uniformity if known
					if (uniformType != null && value.Type != uniformType) uniformType = null;
				} else throw new IndexOutOfRangeException();
			}
		}


		public bool Equals(DataList? other) {
			if (other == null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (Count != other.Count) return false;
			if (hashCode != null && other.hashCode != null && hashCode == other.hashCode) return true;

			// Possible fast check between uniform types
			if (uniformType != null && other.uniformType != null) {
				if (uniformType != other.UniformType) return false;

				switch(UniformType) {
						// Check for underlying memory equality for integer types
					case DataType.Boolean:
						return MemoryUtil.MemoryEqual<bool>((bool[])uniformList!, (bool[])other.uniformList!);
					case DataType.Byte:
						return MemoryUtil.MemoryEqual<byte>((byte[])uniformList!, (byte[])other.uniformList!);
					case DataType.Int:
						return MemoryUtil.MemoryEqual<int>((int[])uniformList!, (int[])other.uniformList!);
					case DataType.Long:
						return MemoryUtil.MemoryEqual<long>((long[])uniformList!, (long[])other.uniformList!);
						// Check using tolerance method for float types
					case DataType.Float: {
							float[] left = (float[])uniformList!, right = (float[])other.uniformList!;
							for(int i = 0; i < left.Length; i++) {
								if (!StructuredData.Equal(left[i], right[i])) return false;
							}
							return true;
						}
					case DataType.Double: {
							double[] left = (double[])uniformList!, right = (double[])other.uniformList!;
							for (int i = 0; i < left.Length; i++) {
								if (!StructuredData.Equal(left[i], right[i])) return false;
							}
							return true;
						}
						// Otherwise use the default method
					default:
						break;
				}
			}

			// Slow check using indexing accessor
			for (int i = 0; i < Count; i++) {
				if (this[i] != other[i]) return false;
			}
			return true;
		}

		public override bool Equals(object? obj) => obj is DataList data && Equals(data);



		private int? hashCode = 0;

		public override int GetHashCode() {
			if (hashCode == null) {
				uint hash = 0;
				for (int i = 0; i < Count; i++) {
					hash ^= (uint)this[i].GetHashCode();
					hash = BitOperations.RotateLeft(hash, 3);
				}
				hashCode = (int)hash;
			}
			return hashCode.Value;
		}

		private string? asString = null;

		public override string ToString() {
			if (asString == null) {
				StringBuilder sb = new();
				sb.Append('[');
				for (int i = 0; i < Count; i++) {
					if (i != 0) sb.Append(',');
					sb.Append(this[i].ToString());
				}
				sb.Append(']');
				asString = sb.ToString();
			}
			return asString;
		}


		private void MakeNonUniform() {
			nonUniformList = new(uniformListCount);
			DataType oldType = UniformType;
			for (int i = 0; i < uniformListCount; i++) nonUniformList.Add(new DataBox(uniformList!.GetValue(i), oldType));
			uniformList = null;
		}

		private bool TryMakeUniform() {
			// If impossible to make uniform then return false
			if (!IsUniformType) return false;
			// Convert any existing non-uniform list to a uniform one
			if (nonUniformList != null) {
				Array newArr = Array.CreateInstance(
					StructuredData.IDToType[UniformType],
					(int)BitOperations.RoundUpToPowerOf2((uint)nonUniformList.Count)
				);
				for (int i = 0; i < nonUniformList.Count; i++) newArr.SetValue(nonUniformList[i].Value, i);

				uniformList = newArr;
				uniformListCount = nonUniformList.Count;
				nonUniformList = null;
			}
			return true;
		}

		public Span<T> AsSpan<T>() {
			if (!TryMakeUniform()) throw new InvalidOperationException("Cannot convert non-uniform data list to a span");

			if (uniformList is T[] arr) return arr.AsSpan()[..uniformListCount];
			else throw new InvalidCastException($"Type mismatch when casting data list (expected \"{typeof(T)}\", got \"{uniformList!.GetType()}\")");
		}

		public static DataList Construct(BinaryReader br) {
			DataType dtype = (DataType)br.ReadByte();
			int count = br.Read7BitEncodedInt();
			DataList list = new();
			if (dtype == DataType.None) {
				// If uniform type is not defined each element must be boxed in the binary
				list.nonUniformList = new List<DataBox>(count);
				for(int i = 0; i < count; i++) list.Add(DataBox.Construct(br));
			} else {
				list.uniformType = dtype;
				list.uniformListCount = count;
				// Else the list is just the raw data for the type
				switch (dtype) {
					case DataType.Boolean: {
							// Boolean lists are packed into individual bits
							int bytes = count >> 3;
							if ((count & 0x7) != 0) bytes++;

							Span<byte> byteData = bytes > 4096 ? new byte[bytes] : stackalloc byte[bytes];
							br.Read(byteData);

							bool[] listData = new bool[count];
							for(int i = 0; i < count; i++) {
								byte b = byteData[i >> 3];
								listData[i] = ((b >> (i & 0x7)) & 1) != 0;
							}

							list.uniformList = listData;
						}
						break;
					case DataType.Byte:
						list.uniformList = br.ReadBytes(count);
						break;
						// Integer types are not 7-bit packed in lists to make reading/writing a simple memory copy
					case DataType.Int: {
							int[] intData = new int[count];
							if (BitConverter.IsLittleEndian) br.Read(MemoryMarshal.Cast<int, byte>(intData));
							else {
								for(int i = 0; i < count; i++) intData[i] = br.ReadInt32();
							}
							list.uniformList = intData;
						}
						break;
					case DataType.Long: {
							long[] longData = new long[count];
							if (BitConverter.IsLittleEndian) br.Read(MemoryMarshal.Cast<long, byte>(longData));
							else {
								for(int i = 0; i < count; i++) longData[i] = br.ReadInt64();
							}
							list.uniformList = longData;
						}
						break;
					case DataType.Float: {
							float[] floatData = new float[count];
							if (BitConverter.IsLittleEndian) br.Read(MemoryMarshal.Cast<float, byte>(floatData));
							else {
								for (int i = 0; i < count; i++) floatData[i] = br.ReadSingle();
							}
							list.uniformList = floatData;
						}
						break;
					case DataType.Double: {
							double[] doubleData = new double[count];
							if (BitConverter.IsLittleEndian) br.Read(MemoryMarshal.Cast<double, byte>(doubleData));
							else {
								for(int i = 0; i < count; i++) doubleData[i] = br.ReadDouble();
							}
							list.uniformList = doubleData;
						}
						break;
					case DataType.String: {
							string[] stringData = new string[count];
							for(int i = 0; i < count; i++) stringData[i] = StructuredData.ReadString(br);
							list.uniformList = stringData;
						}
						break;
					case DataType.List: {
							DataList[] listData = new DataList[count];
							for(int i = 0; i < count; i++) listData[i] = Construct(br);
							list.uniformList = listData;
						}
						break;
					case DataType.Object: {
							DataObject[] objectData = new DataObject[count];
							for(int i = 0; i < count; i++) objectData[i] = DataObject.Construct(br);
							list.uniformList = objectData;
						}
						break;
				}
			}
			return list;
		}

		public void Write(BinaryWriter bw) {
			bw.Write((byte)(TryMakeUniform() ? UniformType : DataType.None));
			bw.Write7BitEncodedInt(Count);
			if (uniformList != null) {
				switch (UniformType) {
					case DataType.Boolean: {
							int byteCount = Count >> 3;
							if ((Count & 0x7) != 0) byteCount++;
							Span<byte> bytes = byteCount > 4096 ? new byte[byteCount] : stackalloc byte[byteCount];
							bytes.Clear();
							bool[] array = (bool[])uniformList;
							for(int i = 0; i < Count; i++) {
								if (array[i]) bytes[i >> 3] |= (byte)(1 << (i & 0x7));
							}
							bw.Write(bytes);
						}
						break;
					case DataType.Byte:
						bw.Write((byte[])uniformList);
						break;
					case DataType.Int:
						if (BitConverter.IsLittleEndian) bw.Write(MemoryMarshal.Cast<int, byte>((int[])uniformList));
						else {
							foreach (int value in (int[])uniformList) bw.Write(value);
						}
						break;
					case DataType.Long:
						if (BitConverter.IsLittleEndian) bw.Write(MemoryMarshal.Cast<long, byte>((long[])uniformList));
						else {
							foreach (long value in (long[])uniformList) bw.Write(value);
						}
						break;
					case DataType.Float:
						if (BitConverter.IsLittleEndian) bw.Write(MemoryMarshal.Cast<float, byte>((float[])uniformList));
						else {
							foreach (float value in (float[])uniformList) bw.Write(value);
						}
						break;
					case DataType.Double:
						if (BitConverter.IsLittleEndian) bw.Write(MemoryMarshal.Cast<double, byte>((double[])uniformList));
						else {
							foreach (double value in (double[])uniformList) bw.Write(value);
						}
						break;
					case DataType.String:
						foreach (string value in (string[])uniformList) StructuredData.WriteString(value, bw);
						break;
					case DataType.List:
						foreach (DataList value in (DataList[])uniformList) value.Write(bw);
						break;
					case DataType.Object:
						foreach (DataObject value in (DataObject[])uniformList) value.Write(bw);
						break;
					default:
						break;
				}
			} else if (nonUniformList != null) {
				foreach (DataBox value in nonUniformList) value.Write(bw);
			}
		}

		//==========================//
		// Non-Modifying Operations //
		//==========================//

		public IEnumerator<DataBox> GetEnumerator() {
			for (int i = 0; i < Count; i++) yield return this[i];
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int IndexOf(DataBox item) {
			if (uniformList != null) {
				if (item.Type != UniformType) return -1;
				return Array.IndexOf(uniformList, item.Value);
			} else if (nonUniformList != null) {
				for(int i = 0; i < Count; i++) {
					if (nonUniformList[i] == item) return i;
				}
			}
			return -1;
		}

		public bool Contains(DataBox item) => IndexOf(item) != -1;

		public void CopyTo(DataBox[] array, int arrayIndex) {
			if (uniformList != null) {
				DataType type = UniformType;
				for (int i = 0; i < uniformListCount; i++) array[arrayIndex++] = new DataBox(uniformList.GetValue(i), type);
			} else nonUniformList?.CopyTo(array, arrayIndex);
		}

		//=========================//
		// Modification Operations //
		//=========================//

		public void Insert(int index, DataBox item) {
			if (index < 0 || index > Count) throw new IndexOutOfRangeException();
			hashCode = null;
			asString = null;

			// Process the underlying storage if it is an array
			if (uniformList != null) {
				if (item.Type == UniformType) {
					// Expand the array if required
					if (uniformList.Length == uniformListCount) {
						Array newArr = Array.CreateInstance(StructuredData.IDToType[UniformType], uniformList.Length * 2);
						uniformList.CopyTo(newArr, 0);
						uniformList = newArr;
					}
					// Shift any later items in the array
					if (index < Count) Array.Copy(uniformList, index, uniformList, index + 1, Count - index);
					// Insert the item
					uniformList.SetValue(item.Value, Count);
					uniformListCount++;
					return;
				} else MakeNonUniform();
			}
			
			if (nonUniformList != null) {
				// Insert into the list if it exists
				nonUniformList.Insert(index, item);
				// Clear the uniform type if known and not matching
				if (uniformType != null && uniformType != item.Type) uniformType = DataType.None;
			} else {
				// The list is empty so initialize as an array
				uniformList = Array.CreateInstance(StructuredData.IDToType[UniformType], 16);
				uniformList.SetValue(item.Value, 0);
				uniformListCount = 1;
				uniformType = item.Type;
			}
		}

		public void Add(DataBox item) => Insert(Count, item);

		public void RemoveAt(int index) {
			if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
			hashCode = null;
			asString = null;

			if (uniformList != null) {
				// Shift later items down if they exist
				if (index < uniformListCount - 1) Array.Copy(uniformList, index + 1, uniformList, index, uniformListCount - (index + 1));
				uniformListCount--;
				// Clear any lost items to null to avoid dangling references
				if (!StructuredData.IDToType[UniformType].IsValueType) uniformList.SetValue(null, uniformListCount);
			} else if (nonUniformList != null) {
				nonUniformList.RemoveAt(index);
				// Invalidate type if the list is known to be non-uniform
				if (uniformType == DataType.None) uniformType = null;
			} else throw new IndexOutOfRangeException();
		}

		public bool Remove(DataBox item) {
			for (int i = 0; i < Count; i++) {
				if (this[i] == item) {
					RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public void Clear() {
			uniformType = null;
			hashCode = 0;
			asString = "[]";

			uniformList = null;
			uniformListCount = 0;
			nonUniformList = null;
		}
	}

}

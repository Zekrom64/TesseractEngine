using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.NET {

	internal struct ImVectorT<T> where T : unmanaged {

		private static readonly int tsize = Marshal.SizeOf<T>();

		public int Size;

		public int Capacity;

		public unsafe T* Data;

		private int GrowCapacity(int sz) {
			int newCapacity = Capacity != 0 ? (Capacity + Capacity / 2) : 8;
			return newCapacity > sz ? newCapacity : sz;
		}

		public void Reserve(int size) {
			if (size > Capacity) {
				unsafe {
					long newSize = tsize * size;
					T* newData = (T*)ImGuiNative.igMemAlloc((uint)newSize);
					if (Data != (T*)0) {
						Buffer.MemoryCopy(Data, newData, newSize, Size * tsize);
						ImGuiNative.igMemFree(Data);
					}
					Data = newData;
				}
				Capacity = size;
			}
		}

		public void Resize(int size) {
			Reserve(GrowCapacity(size));
			Size = size;
		}

		public void PushBack(T value) => Insert(Size, value);

		public void Insert(int index, T value) {
			Reserve(Size + 1);
			if (index < Size) {
				unsafe {
					Buffer.MemoryCopy(Data + index, Data + index + 1, (Capacity - index) * tsize, (Size - index) * tsize);
					Data[index] = value;
				}
			}
		}

		public void Erase(int index) {
			if (index < Size - 1) {
				unsafe {
					Buffer.MemoryCopy(Data + index + 1, Data + index, (Capacity - index) * tsize, (Size - index - 1) * tsize);
				}
			}
			Size--;
		}

	}

}

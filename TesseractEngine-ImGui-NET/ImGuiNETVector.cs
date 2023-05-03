using ImGuiNET;
using System.Collections;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.ImGui.NET {

	internal class ImGuiNETVector<T> : ImGuiNETListBase<T>, IImVector<T> where T : unmanaged {

		private unsafe readonly ImVectorT<T>* pVector;

		public unsafe ImGuiNETVector(ImVector* pVector) {
			this.pVector = (ImVectorT<T>*)pVector;
		}

		public override T this[int index] {
			get {
				if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
				unsafe {
					return pVector->Data[index];
				}
			}
			set {
				if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
				unsafe {
					pVector->Data[index] = value;
				}
			}
		}

		public override int Count {
			get {
				unsafe {
					return pVector->Size;
				}
			}
		}

		public Span<T> AsSpan() {
			unsafe {
				return new Span<T>(pVector->Data, pVector->Size);
			}
		}

		public override void Clear() {
			unsafe {
				pVector->Size = 0;
			}
		}

		public override void Insert(int index, T item) {
			if (index < 0 || index > Count) throw new IndexOutOfRangeException();
			unsafe {
				pVector->Insert(index, item);
			}
		}

		public override void RemoveAt(int index) {
			if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
			unsafe {
				pVector->Erase(index);
			}
		}

		public void Resize(int newSize) {
			unsafe {
				pVector->Resize(newSize);
			}
		}
	}

}

using ImGuiNET;
using System.Collections;

namespace Tesseract.ImGui.NET {

	internal class ImGuiNETList<T, TNative> : ImGuiNETListBase<T> where TNative : unmanaged {

		private readonly ImGuiNETVector<TNative> vector;

		private readonly Func<T, TNative> encodeFunc;

		private readonly Func<TNative, T> decodeFunc;

		public unsafe ImGuiNETList(ImVector* vector, Func<T, TNative> encodeFunc, Func<TNative, T> decodeFunc) {
			this.vector = new ImGuiNETVector<TNative>(vector);
			this.encodeFunc = encodeFunc;
			this.decodeFunc = decodeFunc;
		}

		public override T this[int index] {
			get => decodeFunc(vector[index]);
			set => vector[index] = encodeFunc(value);
		}

		public override int Count => vector.Count;

		public override void Clear() => vector.Clear();

		public override void Insert(int index, T item) => vector.Insert(index, encodeFunc(item));

		public override void RemoveAt(int index) => vector.RemoveAt(index);

	}

}

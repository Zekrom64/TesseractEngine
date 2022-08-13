using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui {

	// In the continuing saga of how retarded can the C++/CLI compiler be; it apparently gets confused about generic instances of System.Nullable<T>
	// So, we use our own nullable type as a workaround that is apparently simple enough for the compiler to understand

	public struct ImNullable<T> {

		private readonly T value;
		public T Value {
			get {
				if (!HasValue) throw new NullReferenceException("Attempted to retrieve value from nullable without value");
				return value;
			}
		}

		public bool HasValue { get; }

		public ImNullable() {
			value = default!;
			HasValue = false;
		}

		public ImNullable(T value) {
			this.value = value;
			HasValue = true;
		}

		public static implicit operator ImNullable<T>(T value) => new(value);

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui {

	// Helper: Growable text buffer for logging/accumulating text
	// (this could be called 'ImGuiTextBuilder' / 'ImGuiStringBuilder')

	public class ImGuiTextBuffer {

		private byte[] buf = Array.Empty<byte>();

		public Span<byte> Buf => buf.AsSpan()[..Size];

		private int size;
		public int Size {
			get => size;
			set {
				Reserve(value);
				size = value;
			}
		}

		public int Capacity => buf.Length;

		public bool Empty => size <= 1;

		public void Clear() {
			buf = Array.Empty<byte>();
			size = 0;
		}

		public void Reserve(int capacity) {
			if (buf.Length < capacity) Array.Resize(ref buf, capacity);
		}

		public void Append(string str) {
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			Reserve(size + bytes.Length + 1);
			bytes.CopyTo(buf, size);
			size += bytes.Length;
			buf[size] = 0;
		}

		public static implicit operator string(ImGuiTextBuffer buf) => Encoding.UTF8.GetString(buf.buf, 0, buf.size);

	}

}

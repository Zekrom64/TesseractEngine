using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesseract.ImGui {

	// Helper: Growable text buffer for logging/accumulating text
	// (this could be called 'ImGuiTextBuilder' / 'ImGuiStringBuilder')

	public class ImGuiTextBuffer {

		private byte[] buf;

		public Span<byte> Buf => buf;

		public int Capacity => buf.Length;

		public int Length {
			get {
				int len = Array.IndexOf<byte>(buf, 0);
				return len < 0 ? Capacity : len;
			}
		}

		public ImGuiTextBuffer(int? capacity = null) {
			buf = new byte[capacity ?? 256];
		}

		public void Clear() {
			buf = Array.Empty<byte>();
		}

		public void ClearText() => Buf.Fill(0);

		public void Reserve(int capacity) {
			if (buf.Length < capacity) Array.Resize(ref buf, capacity);
		}

		public int Callback(IImGuiInputTextCallbackData data) {
			if ((data.EventFlag & ImGuiInputTextFlags.CallbackResize) != 0) Reserve(data.BufTextLen + 1);
			return 0;
		}

		public void Append(string str) {
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			int len = Length;
			Reserve(len + bytes.Length + 1);
			bytes.CopyTo(buf, len);
			buf[len + bytes.Length] = 0;
		}

		public static implicit operator string(ImGuiTextBuffer buf) {
			return Encoding.UTF8.GetString(buf.buf, 0, buf.Length);
		}

	}

}

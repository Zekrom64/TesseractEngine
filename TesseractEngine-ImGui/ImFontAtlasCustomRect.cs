using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui {

	public interface IImFontAtlasCustomRect {

		public ushort Width { get; set; }

		public ushort Height { get; set; }

		public ushort X { get; }

		public ushort Y { get; }

		public uint GlyphID { get; set; }

		public float GlyphAdvanceX { get; set; }

		public Vector2 GlyphOffset { get; set; }

		public IImFont Font { get; set; }

		public bool IsPacked => X != 0xFFFF;

	}

}

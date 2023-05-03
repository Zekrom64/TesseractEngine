using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.NET {

	public record class ImGuiNETContext : IImGuiContext {

		internal nint Context { get; init; }

		internal ImGuiNETIO? IO { get; set; } = null;

		internal ImGuiNETFontAtlas? FontAtlas { get; set; } = null;

		public ImGuiNETContext(nint context) {
			Context = context;
		}
	}

}

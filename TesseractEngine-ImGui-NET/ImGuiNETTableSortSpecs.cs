using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.NET {

	public class ImGuiNETTableSortSpecs : IImGuiTableSortSpecs {

		private readonly ImGuiTableSortSpecsPtr specs;

		public ImGuiNETTableSortSpecs(ImGuiTableSortSpecsPtr specs) {
			this.specs = specs;
		}

		public ReadOnlySpan<ImGuiTableColumnSortSpecs> Specs {
			get {
				unsafe {
					var ptr = specs.Specs;
					return new ReadOnlySpan<ImGuiTableColumnSortSpecs>(ptr.NativePtr, specs.SpecsCount);
				}
			}
		}

		public bool SpecsDirty => specs.SpecsDirty;

	}

}

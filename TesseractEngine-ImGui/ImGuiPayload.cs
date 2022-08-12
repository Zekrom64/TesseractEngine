using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui {
	
	public interface IImGuiPayload {

		public ReadOnlySpan<byte> Data { get; }

		public bool IsDataType(string type);

		public bool IsPreview { get; }

		public bool IsDelivery { get; }

	}

}

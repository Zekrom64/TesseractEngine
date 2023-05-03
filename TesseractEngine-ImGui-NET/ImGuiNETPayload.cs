using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.NET {

	public class ImGuiNETPayload : IImGuiPayload {

		private readonly ImGuiPayloadPtr payload;

		public ImGuiNETPayload(ImGuiPayloadPtr payload) {
			this.payload = payload;
		}

		public ReadOnlySpan<byte> Data {
			get {
				unsafe {
					return new ReadOnlySpan<byte>((void*)payload.Data, payload.DataSize);
				}
			}
		}

		public bool IsPreview => payload.IsPreview();

		public bool IsDelivery => payload.IsDelivery();

		public bool IsDataType(string type) => payload.IsDataType(type);

	}

}

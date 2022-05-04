namespace Tesseract.ImGui {
	public class ImDrawChannel {

		public readonly List<ImDrawCmd> CmdBuffer = new();
		public readonly List<ushort> IdxBuffer = new();

	}

}

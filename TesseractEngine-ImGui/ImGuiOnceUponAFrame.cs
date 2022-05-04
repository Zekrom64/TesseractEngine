namespace Tesseract.ImGui {
	public class ImGuiOnceUponAFrame {

		public int RefFrame = -1;

		public ImGuiOnceUponAFrame() { }

		public static implicit operator bool(ImGuiOnceUponAFrame uoaf) {
			int currentFrame = ImGui.FrameCount;
			if (uoaf.RefFrame == currentFrame) return false;
			uoaf.RefFrame = currentFrame;
			return true;
		}

	}

}

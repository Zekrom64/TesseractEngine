using System.Numerics;

namespace Tesseract.ImGui {
	public class ImDrawData {

		public bool Valid;
		public int TotalIdxCount;
		public int TotalVtxCount;
		public readonly List<ImDrawList> CmdLists = new();
		public Vector2 DisplayPos;
		public Vector2 DisplaySize;
		public Vector2 FramebufferScale;

		public void Clear() {
			Valid = false;
			TotalIdxCount = TotalVtxCount = 0;
			CmdLists.Clear();
			DisplayPos = DisplaySize = FramebufferScale = Vector2.Zero;
		}

		public void DeIndexAllBuffers() {

		}

		public void ScaleClipRects(Vector2 fbScale) {

		}

	}

}

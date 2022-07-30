using System.Numerics;

namespace Tesseract.ImGui {

	public interface IImDrawData {

		public bool Valid { get; }
		public int TotalIdxCount { get; }
		public int TotalVtxCount { get; }
		public IList<IImDrawList> CmdLists { get; }
		public Vector2 DisplayPos { get; }
		public Vector2 DisplaySize { get; }
		public Vector2 FramebufferScale { get; }

		public void Clear();

		public void DeIndexAllBuffers();

		public void ScaleClipRects(Vector2 fbScale);

	}

	public interface IImDrawList { }

}

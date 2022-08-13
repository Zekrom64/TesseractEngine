namespace Tesseract.ImGui {

	public interface IImGuiListClipper {

		public int DisplayStart { get; }
		public int DisplayEnd { get; }
		public int ItemsCount { get; }
		public float ItemsHeight { get; }
		public float StartPosY { get; }

		public void Begin(int itemsCount, float itemsHeight = -1);

		public void End();

		public bool Step();

		public void ForceDisplayRangeByIndices(int itemMin, int itemMax);

	}

}

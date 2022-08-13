namespace Tesseract.ImGui {

	public interface IImGuiStorage : IDisposable {

		public void Clear();

		public int GetInt(uint key, int defaultVal = 0);
		public void SetInt(uint key, int val);
		public bool GetBool(uint key, bool defaultVal = false);
		public void SetBool(uint key, bool val);
		public float GetFloat(uint key, float defaultVal = 0);
		public void SetFloat(uint key, float val);
		public nint GetPtr(uint key, nint defaultVal = 0);
		public void SetPtr(uint key, nint val);

		public void BuildSortByKey();

	}

}

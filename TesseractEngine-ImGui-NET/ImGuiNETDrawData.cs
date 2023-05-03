using ImGuiNET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.NET {

	public class ImGuiNETDrawData : IImDrawData {

		private class CmdList : IReadOnlyList<IImDrawList> {

			private readonly ImDrawDataPtr drawData;

			public CmdList(ImDrawDataPtr drawData) {
				this.drawData = drawData;
			}

			public IImDrawList this[int index] => ImGuiNETDrawList.Get(drawData.CmdListsRange[index]);

			public int Count => drawData.CmdListsCount;

			public IEnumerator<IImDrawList> GetEnumerator() {
				for(int i = 0; i < Count; i++) yield return this[i];
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		}

		private readonly ImDrawDataPtr drawData;

		internal ImGuiNETDrawData(ImDrawDataPtr drawData) {
			this.drawData = drawData;
			this.CmdLists = new CmdList(drawData);
		}

		public bool Valid => drawData.Valid;

		public int TotalIdxCount => drawData.TotalIdxCount;

		public int TotalVtxCount => drawData.TotalVtxCount;

		public IReadOnlyList<IImDrawList> CmdLists { get; }

		public Vector2 DisplayPos => drawData.DisplayPos;

		public Vector2 DisplaySize => drawData.DisplaySize;

		public Vector2 FramebufferScale => drawData.FramebufferScale;

		public void Clear() => drawData.Clear();

		public void DeIndexAllBuffers() => drawData.DeIndexAllBuffers();

		public void ScaleClipRects(Vector2 fbScale) => drawData.ScaleClipRects(fbScale);

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;

namespace Tesseract.Core.Input {
	
	/// <summary>
	/// Interface for accessing a system's clipboard contents. This will be provided by
	/// either <see cref="IInputSystem"/> or <see cref="IWindow"/>.
	/// </summary>
	public interface IClipboard {

		/// <summary>
		/// The current text of the clipboard.
		/// </summary>
		public string? ClipboardText { get; set; }

	}

}

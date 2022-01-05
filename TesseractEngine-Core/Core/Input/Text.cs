using System;

namespace Tesseract.Core.Input {

	/// <summary>
	/// A text input event is fired when text input is to be appended.
	/// </summary>
	public struct TextInputEvent {

		/// <summary>
		/// The text from the input event.
		/// </summary>
		public string Text;

	}

	/// <summary>
	/// A text edit event is fired when text input is to be modified.
	/// </summary>
	public struct TextEditEvent {

		/// <summary>
		/// The character index of the start of modification.
		/// </summary>
		public int Start;

		/// <summary>
		/// The number of characters to modify.
		/// </summary>
		public int Length;

		/// <summary>
		/// The modified text to replace with.
		/// </summary>
		public string Text;

	}

	/// <summary>
	/// A text input provides actual text as distinct from key presses. Text input must
	/// be manually started and stopped, and text input events will be fired to either
	/// append or modify a buffer of input text.
	/// </summary>
	public interface ITextInput {

		/// <summary>
		/// Event fired when text input is received.
		/// </summary>
		public event Action<TextInputEvent> OnTextInput;

		/// <summary>
		/// Event fired when text is edited during input.
		/// </summary>
		public event Action<TextEditEvent> OnTextEdit;

		/// <summary>
		/// Starts text input.
		/// </summary>
		public void StartTextInput();

		/// <summary>
		/// Ends text input.
		/// </summary>
		public void EndTextInput();

	}

}

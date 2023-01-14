using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.Core.Graphics.Font {

	/// <summary>
	/// Font renderer creation information.
	/// </summary>
	public record FontRendererCreateInfo {

		/// <summary>
		/// The name of the font family to create the renderer from.
		/// </summary>
		public required string Family { get; init; }

		/// <summary>
		/// The point (pt) size to render the font at.
		/// </summary>
		public required float Size { get; init; }

		/// <summary>
		/// The style of the rendered font.
		/// </summary>
		public FontStyle Style { get; init; } = FontStyle.Normal;

	}

	/// <summary>
	/// A font renderer provides methods for rendering a font to a pixel image and computing the layout.
	/// </summary>
	public interface IFontRenderer : IDisposable {

		/// <summary>
		/// Renders the given string of text, generating an image with the rendered text on a transparent background.
		/// </summary>
		/// <param name="text">The text to render</param>
		/// <param name="color">The color of text to render</param>
		/// <returns>An image containing the rendered text</returns>
		public IImage Render(string text, Vector4 color);

		/// <summary>
		/// Lays out the given text, generating a corresponding list of rectangles that determine the location
		/// and sizes of the characters.
		/// </summary>
		/// <param name="text">String of text</param>
		/// <param name="positions">Span that will receive the character locations</param>
		public void Layout(string text, Span<Recti> positions) => Layout(text.AsSpan(), positions);

		/// <summary>
		/// Lays out the given text, generating a corresponding list of rectangles that determine the location
		/// and sizes of the characters.
		/// </summary>
		/// <param name="text">Span of characters in the text</param>
		/// <param name="positions">Span that will receive the character locations</param>
		public void Layout(in ReadOnlySpan<char> text, Span<Recti> positions);

	}

}

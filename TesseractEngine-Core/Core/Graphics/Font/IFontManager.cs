using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Graphics.Font {

	/// <summary>
	/// Bitmask of font styles.
	/// </summary>
	[Flags]
	public enum FontStyle {
		/// <summary>
		/// Normal style.
		/// </summary>
		Normal = 0,
		/// <summary>
		/// Bold style.
		/// </summary>
		Bold = 0x1,
		/// <summary>
		/// Italic style.
		/// </summary>
		Italic = 0x2
	}

	/// <summary>
	/// A font manager tracks a collection of loaded fonts that can be rendered.
	/// </summary>
	public interface IFontManager : IDisposable {

		/// <summary>
		/// The collection of font family names that the manager can provide.
		/// </summary>
		public IReadOnlyCollection<string> Families { get; }

		/// <summary>
		/// Adds a font to the manager using a stream as the source of font data.
		/// </summary>
		/// <param name="source">Font data source</param>
		/// <returns>Family name of the loaded font</returns>
		public string AddFont(Stream source);

		/// <summary>
		/// Adds a font to the manager using a span as the source of font data.
		/// </summary>
		/// <param name="source">Font data source</param>
		/// <returns>Family name of the loaded font</returns>
		public string AddFont(in ReadOnlySpan<byte> source);

		/// <summary>
		/// Creates a font renderer using a font from this manager.
		/// </summary>
		/// <param name="createInfo">Font renderer creation information</param>
		/// <returns>The created font renderer</returns>
		public IFontRenderer CreateRenderer(FontRendererCreateInfo createInfo);

	}

}

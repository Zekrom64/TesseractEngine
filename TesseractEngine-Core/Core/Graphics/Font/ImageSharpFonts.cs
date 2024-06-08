using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.Core.Graphics.Font {

	/// <summary>
	/// Font manager using ImageSharp.
	/// </summary>
	public class ImageSharpFontManager : IFontManager {

		private readonly FontCollection fontCollection = new();

		public IReadOnlyCollection<string> Families => throw new NotImplementedException();

		public string AddFont(Stream source) {
			return fontCollection.Add(source).Name;
		}

		public string AddFont(in ReadOnlySpan<byte> source) {
			return fontCollection.Add(new MemoryStream(source.ToArray())).Name;
		}

		private static SixLabors.Fonts.FontStyle ConvertStyle(FontStyle style) {
			SixLabors.Fonts.FontStyle style2 = default;
			if ((style & FontStyle.Bold) != 0) style2 |= SixLabors.Fonts.FontStyle.Bold;
			if ((style & FontStyle.Italic) != 0) style2 |= SixLabors.Fonts.FontStyle.Italic;
			return style2;
		}

		public IFontRenderer CreateRenderer(FontRendererCreateInfo createInfo) {
			FontFamily family = fontCollection.Get(createInfo.Family);
			return new ImageSharpFontRenderer(family.CreateFont(createInfo.Size, ConvertStyle(createInfo.Style)));
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}

	/// <summary>
	/// Font renderer using ImageSharp.
	/// </summary>
	public class ImageSharpFontRenderer : IFontRenderer {

		private readonly SixLabors.Fonts.Font font;

		private readonly TextOptions textOptions;

		public ImageSharpFontRenderer(SixLabors.Fonts.Font font) {
			this.font = font;
			textOptions = new(font);
		}

		public void Layout(in ReadOnlySpan<char> text, Span<Recti> positions) {
			ReadOnlySpan<GlyphBounds> bounds;
			TextMeasurer.TryMeasureCharacterBounds(text, textOptions, out bounds);
			for(int i = 0; i < bounds.Length; i++) {
				var area = bounds[i].Bounds;
				positions[i] = new Recti() {
					Position = new Vector2i((int)area.Top, (int)area.Left),
					Size = new Vector2i((int)area.Width, (int)area.Height)
				};
			}
		}

		public IImage Render(string text, Vector4 color) {
			var rect = TextMeasurer.MeasureSize(text, textOptions);
			Image<Rgba32> image = new((int)rect.Width, (int)rect.Height);
			image.Mutate(x => x.DrawText(text, font, new Color(color), new PointF()));
			return new ImageSharpImage<Rgba32>(image);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}

}

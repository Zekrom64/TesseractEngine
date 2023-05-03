using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.ImGui.NET {

	public class ImGuiNETFontAtlas : IImFontAtlas {

		private static readonly uint sizeofFontAtlas = (uint)Marshal.SizeOf<ImFontAtlas>();

		internal readonly ImFontAtlasPtr fontAtlas;

		private readonly Dictionary<IntPtr, ImGuiNETFont> fonts = new();

		public ImGuiNETFontAtlas(ImFontAtlasPtr fontAtlas) {
			this.fontAtlas = fontAtlas;
		}

		public ImGuiNETFontAtlas() {
			unsafe {
				fontAtlas = ImGuiNative.ImFontAtlas_ImFontAtlas();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				ImGuiNative.ImFontAtlas_destroy(fontAtlas.NativePtr);
			}
		}

		internal ImGuiNETFont GetFont(ImFontPtr font) {
			unsafe {
				IntPtr ptr = (IntPtr)fontAtlas.NativePtr;
				if (fonts.TryGetValue(ptr, out ImGuiNETFont? cfont)) return cfont;
				cfont = new ImGuiNETFont(this, font);
				fonts[ptr] = cfont;
				return cfont;
			}
		}


		public bool IsBuilt => fontAtlas.IsBuilt();

		private static readonly Dictionary<IntPtr, int> glyphRangeLengths = new();

		private static ReadOnlySpan<char> GetGlyphRange(IntPtr ptr) {
			// Try to get the cached version first
			if (!glyphRangeLengths.TryGetValue(ptr, out int length)) {
				// Else scan through characters until null is found
				UnmanagedPointer<char> uptr = new(ptr);
				length = 0;
				while (uptr[length++] != 0) ;
				// Truncate to a multiple of two
				length &= ~1;
				// Cache result
				glyphRangeLengths[ptr] = length;
			}
			unsafe {
				return new ReadOnlySpan<char>((void*)ptr, length);
			}
		}

		public ReadOnlySpan<char> GlyphRangesDefault => GetGlyphRange(fontAtlas.GetGlyphRangesDefault());

		public ReadOnlySpan<char> GlyphRangesKorean => GetGlyphRange(fontAtlas.GetGlyphRangesKorean());

		public ReadOnlySpan<char> GlyphRangesJapanese => GetGlyphRange(fontAtlas.GetGlyphRangesJapanese());

		public ReadOnlySpan<char> GlyphRangesChineseFull => GetGlyphRange(fontAtlas.GetGlyphRangesChineseFull());

		public ReadOnlySpan<char> GlyphRangesChineseSimplifiedCommon => GetGlyphRange(fontAtlas.GetGlyphRangesChineseSimplifiedCommon());

		public ReadOnlySpan<char> GlyphRangesCyrillic => GetGlyphRange(fontAtlas.GetGlyphRangesCyrillic());

		public ReadOnlySpan<char> GlyphRangesThai => GetGlyphRange(fontAtlas.GetGlyphRangesThai());

		public ReadOnlySpan<char> GlyphRangesVietnamese => GetGlyphRange(fontAtlas.GetGlyphRangesVietnamese());

		public ImFontAtlasFlags Flags { get => (ImFontAtlasFlags)fontAtlas.Flags; set => fontAtlas.Flags = (ImGuiNET.ImFontAtlasFlags)value; }

		public nuint TexID { get => (nuint)fontAtlas.TexID; set => fontAtlas.TexID = (nint)value; }

		public int TexDesiredWidth { get => fontAtlas.TexDesiredWidth; set => fontAtlas.TexDesiredWidth = value; }

		public int TexGlyphPadding { get => fontAtlas.TexGlyphPadding; set => fontAtlas.TexGlyphPadding = value; }

		public bool Locked => fontAtlas.Locked;

		public int AddCustomRectFontGlyph(IImFont font, char id, int width, int height, float advanceX, Vector2 offset = default) =>
			fontAtlas.AddCustomRectFontGlyph(((ImGuiNETFont)font).font, (ushort)id, width, height, advanceX, offset);

		public int AddCustomRectRegular(int width, int height) => fontAtlas.AddCustomRectRegular(width, height);


		private unsafe class FontConfig : IDisposable {

			private readonly ManagedPointer<byte> fontData;
			private readonly ManagedPointer<char> glyphRanges;

			public readonly ImFontConfigPtr ptr;

			public FontConfig(ImFontConfig config) {
				ptr = ImGuiNative.ImFontConfig_ImFontConfig();

				fontData = new ManagedPointer<byte>(config.FontData);

				if (config.GlyphRanges != null) {
					var ranges = config.GlyphRanges;
					glyphRanges = new ManagedPointer<char>(ranges.Count * 2 + 1);
					int i = 0;
					foreach(var range in ranges) {
						glyphRanges[i++] = range.Item1;
						glyphRanges[i++] = range.Item2;
					}
					glyphRanges[i] = '\0';
				}

				ptr.FontData = fontData;
				ptr.FontNo = config.FontNo;
				ptr.SizePixels = config.SizePixels;
				ptr.OversampleH = config.OversampleH;
				ptr.OversampleV = config.OversampleV;
				ptr.PixelSnapH = config.PixelSnapH;
				ptr.GlyphExtraSpacing = config.GlyphExtraSpacing;
				ptr.GlyphOffset = config.GlyphOffset;
				ptr.GlyphRanges = glyphRanges;
				ptr.GlyphMinAdvanceX = config.GlyphMinAdvanceX;
				ptr.GlyphMaxAdvanceX = config.GlyphMaxAdvanceX;
				ptr.MergeMode = config.MergeMode;
				ptr.FontBuilderFlags = config.FontBuilderFlags;
				ptr.RasterizerMultiply = config.RasterizerMultiply;
				ptr.EllipsisChar = config.EllipsisChar;
			}

			public void Dispose() {
				GC.SuppressFinalize(this);
				ptr.Destroy();
				fontData.Dispose();
				glyphRanges.Dispose();
			}

		}

		private static IntPtr StackallocRanges(MemoryStack sp, IReadOnlyCollection<(char, char)>? glyphRanges) {
			if (glyphRanges == null) return IntPtr.Zero;
			var ptr = sp.Alloc<char>(glyphRanges.Count * 2);
			int i = 0;
			foreach(var range in glyphRanges) {
				ptr[i++] = range.Item1;
				ptr[i++] = range.Item2;
			}
			return ptr;
		}

		public IImFont AddFont(ImFontConfig config) {
			using FontConfig fc = new(config);
			return GetFont(fontAtlas.AddFont(fc.ptr));

		}

		public IImFont AddFontDefault(ImFontConfig? config = null) {
			using FontConfig? fc = config != null ? new(config) : null;
			return GetFont(fontAtlas.AddFontDefault(fc?.ptr ?? default));
		}

		public IImFont AddFontFromFileTTF(string filename, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null) {
			using MemoryStack sp = MemoryStack.Push();
			using FontConfig? fc = config != null ? new(config) : null;
			return GetFont(fontAtlas.AddFontFromFileTTF(filename, sizePixels, fc?.ptr ?? default, StackallocRanges(sp, glyphRanges)));
		}

		public IImFont AddFontFromMemoryCompressedBase85TTF(byte[] compressedFontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null) {
			using MemoryStack sp = MemoryStack.Push();
			using FontConfig? fc = config != null ? new(config) : null;
			unsafe {
				fixed (byte* pCompressedFontData = compressedFontData) {
					return GetFont(ImGuiNative.ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(fontAtlas, pCompressedFontData, sizePixels, fc != null ? fc.ptr.NativePtr : null, (ushort*)StackallocRanges(sp, glyphRanges)));
				}
			}
		}

		public IImFont AddFontFromMemoryCompressedTTF(byte[] compressedFontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null) {
			using MemoryStack sp = MemoryStack.Push();
			using FontConfig? fc = config != null ? new(config) : null;
			unsafe {
				fixed (byte* pCompressedFontData = compressedFontData) {
					return GetFont(ImGuiNative.ImFontAtlas_AddFontFromMemoryCompressedTTF(fontAtlas, pCompressedFontData, compressedFontData.Length, sizePixels, fc != null ? fc.ptr.NativePtr : null, (ushort*)StackallocRanges(sp, glyphRanges)));
				}
			}
		}

		public IImFont AddFontFromMemoryTTF(byte[] fontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null) {
			using MemoryStack sp = MemoryStack.Push();
			using FontConfig? fc = config != null ? new(config) : null;
			unsafe {
				fixed(byte* pFontData = fontData) {
					return GetFont(fontAtlas.AddFontFromMemoryTTF((IntPtr)pFontData, fontData.Length, sizePixels, fc?.ptr ?? default, StackallocRanges(sp, glyphRanges)));
				}
			}
		}

		public bool Build() => fontAtlas.Build();


		private unsafe class FontAtlasCustomRect : IImFontAtlasCustomRect {

			internal readonly ImFontAtlasCustomRectPtr ptr;

			internal readonly ImGuiNETFontAtlas fontAtlas;

			internal FontAtlasCustomRect(ImGuiNETFontAtlas fontAtlas, ImFontAtlasCustomRectPtr ptr) {
				this.fontAtlas = fontAtlas;
				this.ptr = ptr;
			}

			public ushort Width { get => ptr.Width; set => ptr.Width = value; }

			public ushort Height { get => ptr.Height; set => ptr.Height = value; }

			public ushort X => ptr.X;

			public ushort Y => ptr.Y;

			public uint GlyphID { get => ptr.GlyphID; set => ptr.GlyphID = value; }

			public float GlyphAdvanceX { get => ptr.GlyphAdvanceX; set => ptr.GlyphAdvanceX = value; }

			public Vector2 GlyphOffset { get => ptr.GlyphOffset; set => ptr.GlyphOffset = value; }

			public IImFont Font {
				get => fontAtlas.GetFont(ptr.Font);
				set {
					unsafe {
						ptr.NativePtr->Font = ((ImGuiNETFont)value).font;
					}
				}
			}

		}


		public void CalcCustomRectUV(IImFontAtlasCustomRect rect, out Vector2 outUVMin, out Vector2 outUVMax) =>
			fontAtlas.CalcCustomRectUV(((FontAtlasCustomRect)rect).ptr, out outUVMin, out outUVMax);

		public void Clear() => fontAtlas.Clear();

		public void ClearFonts() => fontAtlas.ClearFonts();

		public void ClearInputData() => fontAtlas.ClearInputData();

		public void ClearTexData() => fontAtlas.ClearTexData();

		public IImFontAtlasCustomRect GetCustomRectByIndex(int index) => new FontAtlasCustomRect(this, fontAtlas.GetCustomRectByIndex(index));

		public bool GetMouseCursorTexData(ImGuiMouseCursor cursor, out Vector2 outOffset, out Vector2 outSize, Span<Vector2> outUVBorder, Span<Vector2> outUVFill) =>
			fontAtlas.GetMouseCursorTexData((ImGuiNET.ImGuiMouseCursor)cursor, out outOffset, out outSize, out outUVBorder[0], out outUVFill[0]);

		public ReadOnlySpan<byte> GetTexDataAsAlpha8(out int outWidth, out int outHeight, out int outBytesPerPixel) {
			unsafe {
				fontAtlas.GetTexDataAsAlpha8(out byte* pixels, out outWidth, out outHeight, out outBytesPerPixel);
				return new ReadOnlySpan<byte>(pixels, outWidth * outHeight * outBytesPerPixel);
			}
		}

		public ReadOnlySpan<byte> GetTexDataAsRGBA32(out int outWidth, out int outHeight, out int outBytesPerPixel) {
			unsafe {
				fontAtlas.GetTexDataAsRGBA32(out byte* pixels, out outWidth, out outHeight, out outBytesPerPixel);
				return new ReadOnlySpan<byte>(pixels, outWidth * outHeight * outBytesPerPixel);
			}
		}

		public void SetTexID(nuint id) => fontAtlas.SetTexID((nint)id);

	}

}

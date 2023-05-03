using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.ImGui.Utilities.CLI;

namespace Tesseract.ImGui.NET {

	public class ImGuiNETFont : IImFont {

		internal readonly ImFontPtr font;

		internal ImGuiNETFont(ImGuiNETFontAtlas fontAtlas, ImFontPtr font) {
			this.font = font;
			unsafe {
				IndexAdvanceX = new ImGuiNETVector<float>(&font.NativePtr->IndexAdvanceX);
				IndexLookup = new ImGuiNETVector<char>(&font.NativePtr->IndexLookup);
				Glyphs = new ImGuiNETVector<ImFontGlyph>(&font.NativePtr->Glyphs);
			}
			ContainerAtlas = fontAtlas;
		}

		private static ImFontGlyph Decode(ImGuiNET.ImFontGlyphPtr glyph) {
			unsafe {
				return *((ImFontGlyph*)glyph.NativePtr);
			}
		}


		public IReadOnlyList<float> IndexAdvanceX { get; }

		public float FallbackAdvanceX => font.FallbackAdvanceX;

		public float FontSize { get => font.FontSize; set => font.FontSize = value; }

		public IReadOnlyList<char> IndexLookup { get; }

		public IReadOnlyList<ImFontGlyph> Glyphs { get; }

		public ImFontGlyph FallbackGlyph => Decode(font.FallbackGlyph);

		public IImFontAtlas ContainerAtlas { get; }

		public char FallbackChar => (char)font.FallbackChar;

		public char EllipsisChar => (char)font.EllipsisChar;

		public bool DirtyLookupTables => font.DirtyLookupTables;

		public float Scale { get => font.Scale; set => font.Scale = value; }

		public float Ascent => font.Ascent;

		public float Descent => font.Descent;

		public int MetricsTotalSurface => font.MetricsTotalSurface;

		public bool IsLoaded => font.IsLoaded();

		public string DebugName => font.GetDebugName();

		public Vector2 CalcTextSizeA(float size, float maxWidth, float wrapWidth, string text, int textBegin = 0) {
			StringParam vtext = text.AsSpan()[textBegin..];
			Vector2 ret = default;
			unsafe {
				fixed(byte* pText = vtext.Bytes) {
					ImGuiNative.ImFont_CalcTextSizeA(&ret, font.NativePtr, size, maxWidth, wrapWidth, pText, pText + vtext.Bytes.Length, (byte**)0);
				}
			}
			return ret;
		}

		public int CalcWordWrapPositionA(float scale, string text, float wrapWidth, int textBegin = 0) {
			StringParam vtext = text.AsSpan()[textBegin..];
			int numBytes;
			unsafe {
				fixed(byte* pText = vtext.Bytes) {
					byte* pSplit = ImGuiNative.ImFont_CalcWordWrapPositionA(font.NativePtr, scale, pText, pText + vtext.Bytes.Length, wrapWidth);
					numBytes = (int)(pSplit - pText);
				}
			}
			return Encoding.UTF8.GetCharCount(vtext.Bytes[..numBytes]);
		}

		public ImFontGlyph? FindGlyphNoFallback(char c) {
			var ptr = font.FindGlyphNoFallback(c);
			unsafe {
				if (ptr.NativePtr == (ImGuiNET.ImFontGlyph*)0) return null;
				else return Decode(ptr);
			}
		}

		public ImFontGlyph FintGlyph(char c) => Decode(font.FindGlyph(c));

		public float GetCharAdvance(char c) => font.GetCharAdvance(c);

		public void RenderChar(IImDrawList drawList, float size, Vector2 pos, uint col, char c) => font.RenderChar(((ImGuiNETDrawList)drawList).drawList, size, pos, col, c);

		public void RenderText(IImDrawList drawList, float size, Vector2 pos, uint col, Vector4 clipRect, string text, int textBegin = 0, int textEnd = -1, float wrapWidth = 0, bool cpuFineClip = false) {
			if (textEnd < 0) textEnd = text.Length;
			StringParam vtext = text.AsSpan()[textBegin..textEnd];
			unsafe {
				fixed(byte* pText = vtext.Bytes) {
					ImGuiNative.ImFont_RenderText(font.NativePtr, ((ImGuiNETDrawList)drawList).drawList.NativePtr, size, pos, col, clipRect, pText, pText + vtext.Bytes.Length, wrapWidth, (byte)(cpuFineClip ? 1 : 0));
				}
			}
		}

	}

}

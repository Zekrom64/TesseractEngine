using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Windows {

	#region Defines

	/// <summary>
	/// Enumeration of <c>*_CHARSET</c> values from the <c>wingdi.h</c> header.
	/// </summary>
	public enum WingdiCharSet : byte {
		Ansi = 0,
		Default = 1,
		Symbol = 2,
		ShiftJIS = 128,
		HanGUL = 129,
		GB2312 = 134,
		ChineseBig5 = 136,
		Oem = 255,
		Johab = 130,
		Hebrew = 177,
		Arabic = 178,
		Greek = 161,
		Turkish = 162,
		Vietnamese = 163,
		Thai = 222,
		EastEurope = 238,
		Russian = 204
	}

	/// <summary>
	/// Enumeration of <c>OUT_*_PRECIS</c> values from the <c>wingdi.h</c> header.
	/// </summary>
	public enum WingdiOutPrecision : byte {
		/// <summary>
		/// Specifies the default font mapper behavior.
		/// </summary>
		Default = 0,
		/// <summary>
		/// This value is not used by the font mapper, but it is returned when raster fonts are enumerated.
		/// </summary>
		String = 1,
		[Obsolete("Not used")]
		Character = 2,
		/// <summary>
		/// This value is not used by the font mapper, but it is returned when TrueType, other outline-based fonts, and
		/// vector fonts are enumerated.
		/// </summary>
		Stroke = 3,
		/// <summary>
		/// Instructs the font mapper to choose a TrueType font when the system contains multiple fonts with the same name.
		/// </summary>
		TT = 4,
		/// <summary>
		/// Instructs the font mapper to choose a Device font when the system contains multiple fonts with the same name.
		/// </summary>
		Device = 5,
		/// <summary>
		/// Instructs the font mapper to choose a raster font when the system contains multiple fonts with the same name.
		/// </summary>
		Raster = 6,
		/// <summary>
		/// Instructs the font mapper to choose from only TrueType fonts. If there are no TrueType fonts installed in the
		/// system, the font mapper returns to default behavior.
		/// </summary>
		TTOnly = 7,
		/// <summary>
		/// This value instructs the font mapper to choose from TrueType and other outline-based fonts.
		/// </summary>
		Outline = 8,
		ScreenOutline = 9,
		/// <summary>
		/// Instructs the font mapper to choose from only PostScript fonts. If there are no PostScript fonts installed in
		/// the system, the font mapper returns to default behavior.
		/// </summary>
		PSOnly = 10
	}

	/// <summary>
	/// Enumeration of <c>TMPF_*</c> values from the <c>wingdi.h</c> header.
	/// </summary>
	[Flags]
	public enum WingdiTMPF : byte {
		/// <summary>
		/// If this bit is set the font is a variable pitch font. If this bit is clear the font is a fixed pitch font. Note
		/// very carefully that those meanings are the opposite of what the constant name implies.
		/// </summary>
		FixedPitch = 0x01,
		/// <summary>
		/// If this bit is set the font is a vector font.
		/// </summary>
		Vector = 0x02,
		/// <summary>
		/// If this bit is set the font is a TrueType font.
		/// </summary>
		TrueType = 0x04,
		/// <summary>
		/// If this bit is set the font is a device font.
		/// </summary>
		Device = 0x08
	}

	/// <summary>
	/// Enumeration of <c>FF_*</c> values from the <c>wingdi.h</c> header.
	/// </summary>
	public enum WingdiFontFamily : byte {
		/// <summary>
		/// Use default font.
		/// </summary>
		DontCare = 0,
		/// <summary>
		/// Fonts with variable stroke width (proportional) and with serifs. MS Serif is an example.
		/// </summary>
		Roman = 1 << 4,
		/// <summary>
		/// Fonts with variable stroke width (proportional) and without serifs. MS Sans Serif is an example.
		/// </summary>
		Swiss = 2 << 4,
		/// <summary>
		/// Fonts with constant stroke width (monospace), with or without serifs. Monospace fonts are usually
		/// modern. Pica, Elite, and CourierNew are examples.
		/// </summary>
		Modern = 3 << 4,
		/// <summary>
		/// Fonts designed to look like handwriting. Script and Cursive are examples.
		/// </summary>
		Script = 4 << 4,
		/// <summary>
		/// Novelty fonts. Old English is an example.
		/// </summary>
		Decorative = 5 << 4
	}

	/// <summary>
	/// Enumeration of <c>*_FONTTYPE</c> values from the <c>wingdi.h</c> header.
	/// </summary>
	[Flags]
	public enum WingdiFontType : uint {
		Raster = 0x0001,
		Device = 0x0002,
		TrueType = 0x0004
	}

	/// <summary>
	/// Enumeration of <c>CLIP_*_PRECIS</c> values from the <c>wingdi.h</c> header.
	/// </summary>
	[Flags]
	public enum WingdiClipPrecision : byte {
		/// <summary>
		/// Specifies default clipping behavior.
		/// </summary>
		Default = 0,
		Character = 1,
		/// <summary>
		/// Not used by the font mapper, but is returned when raster, vector, or TrueType fonts
		/// are enumerated. For compatibility, this value is always returned when enumerating fonts.
		/// </summary>
		Stroke = 2,
		/// <summary>
		/// When this value is used, the rotation for all fonts depends on whether the orientation
		/// of the coordinate system is left-handed or right-handed.If not used, device fonts always
		/// rotate counterclockwise, but the rotation of other fonts is dependent on the orientation
		/// of the coordinate system.
		/// </summary>
		LHAngles = 1 << 4,
		TTAlways = 2 << 4,
		/// <summary>
		/// <b>Windows XP SP1</b>: Turns off font association for the font. Note that this flag is not
		/// guaranteed to have any effect on any platform after Windows Server 2003.
		/// </summary>
		DFADisable = 4 << 4,
		/// <summary>
		/// You must specify this flag to use an embedded read-only font.
		/// </summary>
		Embedded = 8 << 4,
	}

	/// <summary>
	/// Enumeration of <c>*_QUALITY</c> values from the <c>wingdi.h</c> header.
	/// </summary>
	public enum WingdiQuality : byte {
		/// <summary>
		/// 
		/// </summary>
		Default = 0,
		/// <summary>
		/// 
		/// </summary>
		Draft = 1,
		/// <summary>
		/// 
		/// </summary>
		Proof = 2,
		/// <summary>
		/// 
		/// </summary>
		NonAntialiased = 3,
		/// <summary>
		/// 
		/// </summary>
		Antialiased = 4
	}

	#endregion

	#region Structures

	/// <summary>
	/// Structure describing the attributes of a font. See the
	/// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-logfontw">MSDN</seealso>
	/// for extensive details.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LOGFONTW {

		/// <summary>
		/// The height, in logical units, of the font's character cell or character.
		/// The character height value (also known as the em height) is the character
		/// cell height value minus the internal-leading value.
		/// </summary>
		public int Height;

		/// <summary>
		/// The average width, in logical units, of characters in the font. If the width is
		/// zero, the aspect ratio of the device is matched against the digitization aspect
		/// ratio of the available fonts to find the closest match, determined by the absolute
		/// value of the difference.
		/// </summary>
		public int Width;

		/// <summary>
		/// The angle, in tenths of degrees, between the escapement vector and the x-axis of the device. The escapement
		/// vector is parallel to the base line of a row of text.
		/// </summary>
		public int Escapement;

		/// <summary>
		/// The angle, in tenths of degrees, between each character's base line and the x-axis of the device.
		/// </summary>
		public int Orientation;

		/// <summary>
		/// The weight of the font in the range 0 through 1000. For example, 400 is normal and 700 is bold. If this
		/// value is zero, a default weight is used.
		/// </summary>
		public int Weight;

		private byte italic;

		/// <summary>
		/// An italic font if set to true.
		/// </summary>
		public bool Italic {
			get => italic != 0;
			set => italic = (byte)(value ? 1 : 0);
		}

		private byte underline;

		/// <summary>
		/// An underlined font if set to true.
		/// </summary>
		public bool Underline {
			get => underline != 0;
			set => underline = (byte)(value ? 1 : 0);
		}

		private byte strikeOut;

		/// <summary>
		/// A strikeout font if set to true.
		/// </summary>
		public bool StrikeOut {
			get => strikeOut != 0;
			set => strikeOut = (byte)(value ? 1 : 0);
		}

		/// <summary>
		/// The character set.
		/// </summary>
		public WingdiCharSet CharSet;

		/// <summary>
		/// The output precision. The output precision defines how closely the output must match the requested font's
		/// height, width, character orientation, escapement, pitch, and font type.
		/// </summary>
		public WingdiOutPrecision OutPrecision;

		/// <summary>
		/// The clipping precision. The clipping precision defines how to clip characters that are partially outside
		/// the clipping region. It can be one or more of the following values.
		/// </summary>
		public WingdiClipPrecision ClipPrecision;

		/// <summary>
		/// The output quality. The output quality defines how carefully the graphics device interface (GDI) must attempt
		/// to match the logical-font attributes to those of an actual physical font.
		/// </summary>
		public WingdiQuality Quality;

		private byte pitchAndFamily;

		/// <summary>
		/// The pitch and family of the font.
		/// </summary>
		public (WingdiTMPF Pitch, WingdiFontFamily Family) PitchAndFamily {
			get => ((WingdiTMPF)(pitchAndFamily & 0xF), (WingdiFontFamily)(pitchAndFamily & 0xF0));
			set => pitchAndFamily = (byte)((int)value.Pitch | (int)value.Family);
		}

		private unsafe fixed char faceName[32];

		/// <summary>
		/// A null-terminated string that specifies the typeface name of the font. The length of
		/// this string must not exceed 32 <see cref="char"/> values, including the terminating null character.
		/// The EnumFontFamiliesEx function can be used to enumerate the typeface names of all currently available fonts. If lfFaceName is an empty string, GDI uses the first font that matches the other specified attributes.
		/// </summary>
		public string FaceName {
			get {
				unsafe {
					fixed(char* pFaceName = faceName) {
						return MemoryUtil.GetUTF16(new ReadOnlySpan<char>(pFaceName, 32));
					}
				}
			}
			set {
				unsafe {
					fixed(char* pFaceName = faceName) {
						MemoryUtil.PutUTF16(value, new Span<byte>(pFaceName, 32*sizeof(char)));
						// Always null terminate
						pFaceName[31] = '\0';
					}
				}
			}
		}

	}

	/// <summary>
	/// The <c>TEXTMETRIC</c> structure contains basic information about a physical font. All sizes are specified
	/// in logical units; that is, they depend on the current mapping mode of the display context. See the
	/// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-textmetricw">MSDN</seealso>
	/// for extensive details.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct TEXTMETRICW {

		/// <summary>
		/// The height (ascent + descent) of characters.
		/// </summary>
		public int Height;

		/// <summary>
		/// The ascent (units above the base line) of characters.
		/// </summary>
		public int Ascent;

		/// <summary>
		/// The descent (units below the base line) of characters.
		/// </summary>
		public int Descent;

		/// <summary>
		/// The amount of leading (space) inside the bounds set by the tmHeight member. Accent marks and other diacritical
		/// characters may occur in this area. The designer may set this member to zero.
		/// </summary>
		public int InternalLeading;

		/// <summary>
		/// The amount of extra leading (space) that the application adds between rows. Since this area is outside the font,
		/// it contains no marks and is not altered by text output calls in either OPAQUE or TRANSPARENT mode. The designer
		/// may set this member to zero.
		/// </summary>
		public int ExternalLeading;

		/// <summary>
		/// The average width of characters in the font (generally defined as the width of the letter x ). This value does not
		/// include the overhang required for bold or italic characters.
		/// </summary>
		public int AveCharWidth;

		/// <summary>
		/// The width of the widest character in the font.
		/// </summary>
		public int MaxCharWidth;

		/// <summary>
		/// The weight of the font.
		/// </summary>
		public int Weight;

		/// <summary>
		/// The extra width per string that may be added to some synthesized fonts. When synthesizing some attributes, such as
		/// bold or italic, graphics device interface (GDI) or a device may have to add width to a string on both a per-character
		/// and per-string basis.
		/// </summary>
		public int Overhang;

		/// <summary>
		/// The horizontal aspect of the device for which the font was designed.
		/// </summary>
		public int DigitizedAspectX;

		/// <summary>
		/// The vertical aspect of the device for which the font was designed. The ratio of the <see cref="DigitizedAspectX"/> and
		/// <see cref="DigitizedAspectY"/> members is the aspect ratio of the device for which the font was designed.
		/// </summary>
		public int DigitizedAspectY;

		/// <summary>
		/// The value of the first character defined in the font.
		/// </summary>
		public char FirstChar;

		/// <summary>
		/// The value of the last character defined in the font.
		/// </summary>
		public char LastChar;

		/// <summary>
		/// The value of the character to be substituted for characters not in the font.
		/// </summary>
		public char DefaultChar;

		/// <summary>
		/// The value of the character that will be used to define word breaks for text justification.
		/// </summary>
		public char BreakChar;

		private byte tmItalic;

		/// <summary>
		/// Specifies an italic font if it is true.
		/// </summary>
		public bool Italic {
			get => tmItalic != 0;
			set => tmItalic = (byte)(value ? 1 : 0);
		}

		private byte tmUnderlined;

		/// <summary>
		/// Specifies an underlined font if it is true.
		/// </summary>
		public bool Underlined {
			get => tmUnderlined != 0;
			set => tmUnderlined = (byte)(value ? 1 : 0);
		}

		private byte tmStruckOut;

		/// <summary>
		/// A strikeout font if it is true.
		/// </summary>
		public bool StruckOut {
			get => tmStruckOut != 0;
			set => tmStruckOut = (byte)(value ? 1 : 0);
		}

		private byte tmPitchAndFamily;

		/// <summary>
		/// Specifies information about the pitch, the technology, and the family of a physical font.
		/// </summary>
		public (WingdiTMPF Pitch, WingdiFontFamily Family) PitchAndFamily {
			get => ((WingdiTMPF)(tmPitchAndFamily & 0xF), (WingdiFontFamily)(tmPitchAndFamily & 0xF0));
			set => tmPitchAndFamily = (byte)((int)value.Pitch | (int)value.Family);
		}

		/// <summary>
		/// The character set of the font.
		/// </summary>
		public WingdiCharSet CharSet;

	}

	#endregion

	#region Functions

	/// <summary>
	/// 
	/// </summary>
	/// <param name="logFont">Structure containing the logical attributes of the font</param>
	/// <param name="textMetric">Structure containing the physical attributes of the font</param>
	/// <param name="fontType">The type of the font</param>
	/// <param name="lParam">Application-defined data passed to the enumeration function</param>
	/// <returns>If enumeration should be continued</returns>
	public delegate bool FONTENUMPROCW(in LOGFONTW logFont, in TEXTMETRICW textMetric, WingdiFontType fontType, nint lParam);

	#endregion

	public static partial class Wingdi {

		[LibraryImport("gdi32.dll")]
		private static partial int EnumFontFamiliesExW(IntPtr hDC, ref LOGFONTW logFont, FONTENUMPROCW proc, nint lParam, uint dwFlags);

		public static int EnumFontFamiliesEx(IntPtr hDC, ref LOGFONTW logFont, FONTENUMPROCW proc, nint lParam) =>
			EnumFontFamiliesExW(hDC, ref logFont, proc, lParam, 0);

	}

}

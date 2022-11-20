using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;
using Tesseract.DirectX.Direct2D;
using Tesseract.Windows;

namespace Tesseract.DirectX.DirectWrite {

	using IDWriteGeometrySink = ID2D1SimplifiedGeometrySink;

	using COLORREF = UInt32;
	using SIZE = Vector2i;

	// usp10.h

	public enum SCRIPT_JUSTIFY {
		None = 0,
		ArabicBlank,
		Character,
		Blank = 4,
		ArabicNormal = 7,
		ArabicKashida,
		ArabicAlef,
		ArabicHA,
		ArabicRA,
		ArabicBA,
		ArabicBARA,
		ArabicSeen,
		ArabicSeenM
	}

	// dwrite.h

	public enum DWriteFactoryType {
		Shared,
		Isolated
	}

	public enum DWriteFontFileType {
		Unknown,
		CFF,
		TrueType,
		OpenTypeCollection,
		Type1PFM,
		Type1PFB,
		Vector,
		Bitmap
	}

	public enum DWriteFontFaceType {
		CFF,
		TrueType,
		OpenTypeCollection,
		Type1,
		Vector,
		Bitmap,
		Unknown,
		RawCFF
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "Duplicate values match those in the source header")]
	public enum DWriteFontWeight {
		Thin = 100,
		ExtraLight = 200,
		UltraLight = 200,
		Light = 300,
		SemiLight = 350,
		Normal = 400,
		Regular = 400,
		Medium = 500,
		DemiBold = 600,
		SemiBold = 600,
		Bold = 700,
		ExtraBold = 800,
		UltraBold = 800,
		Black = 900,
		Heavy = 900,
		ExtraBlack = 950,
		UltraBlack = 950
	}

	public enum DWriteFontStretch {
		Undefined = 0,
		UltraCondensed,
		ExtraCondensed,
		Condensed,
		SemiCondensed,
		Normal,
		Medium = Normal,
		SemiExpanded,
		Expanded,
		ExtraExpanded,
		UltraExpanded
	}

	public enum DWriteFontStyle {
		Normal,
		Oblique,
		Italic
	}

	public enum DWriteInformationalStringID {
		None,
		CopyrightNotice,
		VersionStrings,
		Trademark,
		Manufacturer,
		Designer,
		DesignerURL,
		Description,
		FontVendorURL,
		LicenseDescription,
		LicenseInfoURL,
		Win32FamilyNames,
		Win32SubFamilyNames,
		TypographicFamilyNames,
		TypographicSubFamilyNames,
		SampleText,
		FullName,
		PostScriptName,
		PostScriptCIDName,
		WeightStretchStyleFamilyName,
		DesignScriptLanguageTag,
		SupportedScriptLanguageTag,
		PreferredFamilyNames = TypographicFamilyNames,
		PreferredSubFamilyNames = TypographicSubFamilyNames,
		WWSFamilyName = WeightStretchStyleFamilyName
	}

	public enum DWriteFontSimulations {
		None,
		Bold,
		Oblique
	}

	public enum DWritePixelGeometry {
		Flat,
		RGB,
		BGR
	}

	public enum DWriteRenderingMode {
		Default,
		Aliased,
		GDIClassic,
		GDINatural,
		Natural,
		NaturalSymmetric,
		Outline
	}

	public enum DWriteTextAlignment {
		Leading,
		Trailing,
		Center,
		Justified
	}

	public enum DWriteParagraphAlignment {
		Near,
		Far,
		Center
	}

	public enum DWriteWordWrapping {
		Wrap,
		NoWrap,
		EmergencyBreak,
		WholeWord,
		Character
	}

	public enum DWriteReadingDirection {
		LeftToRight,
		RightToLeft,
		TopToBottom,
		BottomToTop
	}

	public enum DWriteFlowDirection {
		TopToBottom,
		BottomToTop,
		LeftToRight,
		RightToLeft
	}

	public enum DWriteTrimmingGranularity {
		None,
		Character,
		Word
	}

	public enum DWriteBreakCondition {
		Neutral,
		CanBreak,
		MayNotBreak,
		MustBreak
	}

	public enum DWriteLineSpacingMethod {
		Default,
		Uniform,
		Proportional
	}

	public static partial class DWrite {

		public static uint MakeOpenTypeTag(char a, char b, char c, char d) =>
			(uint)(((byte)a) | ((byte)b << 8) | ((byte)c << 16) | ((byte)d << 24));

		public static uint MakeOpenTypeTag(string abcd) {
			if (abcd.Length != 4) throw new ArgumentException("Tag string must have 4 characters", nameof(abcd));
			return MakeOpenTypeTag(abcd[0], abcd[1], abcd[2], abcd[3]);
		}

	}

	public enum DWriteFontFeatureTag : uint {
		AlternativeFractions = 0x63726661, /* 'afrc' */
		PetiteCapitalsFromCapitals = 0x63703263, /* 'c2pc' */
		SmallCapitalsFromCapitals = 0x63733263, /* 'c2sc' */
		ContextualAlternates = 0x746c6163, /* 'calt' */
		CaseSensitiveForms = 0x65736163, /* 'case' */
		GlyphCompositionDecomposition = 0x706d6363, /* 'ccmp' */
		ContextualLigatures = 0x67696c63, /* 'clig' */
		CapitalSpacing = 0x70737063, /* 'cpsp' */
		ContextualSwash = 0x68777363, /* 'cswh' */
		CursivePositioning = 0x73727563, /* 'curs' */
		Default = 0x746c6664, /* 'dflt' */
		DiscretionaryLigatures = 0x67696c64, /* 'dlig' */
		ExportForms = 0x74707865, /* 'expt' */
		Fractions = 0x63617266, /* 'frac' */
		FullWidth = 0x64697766, /* 'fwid' */
		HalfForms = 0x666c6168, /* 'half' */
		HalantForms = 0x6e6c6168, /* 'haln' */
		AlternateHalfWidth = 0x746c6168, /* 'halt' */
		HistoricalForms = 0x74736968, /* 'hist' */
		HorizontalKanaAlternates = 0x616e6b68, /* 'hkna' */
		HistoricalLigatures = 0x67696c68, /* 'hlig' */
		HalfWidth = 0x64697768, /* 'hwid' */
		HojoKanjiForms = 0x6f6a6f68, /* 'hojo' */
		JIS04Forms = 0x3430706a, /* 'jp04' */
		JIS78Forms = 0x3837706a, /* 'jp78' */
		JIS83Forms = 0x3338706a, /* 'jp83' */
		JIS90Forms = 0x3039706a, /* 'jp90' */
		Kerning = 0x6e72656b, /* 'kern' */
		StandardLigatures = 0x6167696c, /* 'liga' */
		LiningFeatures = 0x6d756e6c, /* 'lnum' */
		LocalizedForms = 0x6c636f6c, /* 'locl' */
		MarkPositioning = 0x6b72616d, /* 'mark' */
		MathematicalGreek = 0x6b72676d, /* 'mgrk' */
		MarkToMarkPositioning = 0x6b6d6b6d, /* 'mkmk' */
		AlternateAnnotationForms = 0x746c616e, /* 'nalt' */
		NLCKanjiForms = 0x6b636c6e, /* 'nlck' */
		OldStyleFigures = 0x6d756e6f, /* 'onum' */
		Orignals = 0x6e64726f, /* 'ordn' */
		ProportionalAlternateWidth = 0x746c6170, /* 'palt' */
		PetiteCapitals = 0x70616370, /* 'pcap' */
		ProportionalFigures = 0x6d756e70, /* 'pnum' */
		ProportionalWidths = 0x64697770, /* 'pwid' */
		QuarterWidths = 0x64697771, /* 'qwid' */
		RequiredLigatures = 0x67696c72, /* 'rlig' */
		RubyNotationForms = 0x79627572, /* 'ruby' */
		StylisticAlternates = 0x746c6173, /* 'salt' */
		ScientificInferiors = 0x666e6973, /* 'sinf' */
		SmallCapitals = 0x70636d73, /* 'smcp' */
		SimplifiedForms = 0x6c706d73, /* 'smpl' */
		StylisticSet1 = 0x31307373, /* 'ss01' */
		StylisticSet2 = 0x32307373, /* 'ss02' */
		StylisticSet3 = 0x33307373, /* 'ss03' */
		StylisticSet4 = 0x34307373, /* 'ss04' */
		StylisticSet5 = 0x35307373, /* 'ss05' */
		StylisticSet6 = 0x36307373, /* 'ss06' */
		StylisticSet7 = 0x37307373, /* 'ss07' */
		StylisticSet8 = 0x38307373, /* 'ss08' */
		StylisticSet9 = 0x39307373, /* 'ss09' */
		StylisticSet10 = 0x30317373, /* 'ss10' */
		StylisticSet11 = 0x31317373, /* 'ss11' */
		StylisticSet12 = 0x32317373, /* 'ss12' */
		StylisticSet13 = 0x33317373, /* 'ss13' */
		StylisticSet14 = 0x34317373, /* 'ss14' */
		StylisticSet15 = 0x35317373, /* 'ss15' */
		StylisticSet16 = 0x36317373, /* 'ss16' */
		StylisticSet17 = 0x37317373, /* 'ss17' */
		StylisticSet18 = 0x38317373, /* 'ss18' */
		StylisticSet19 = 0x39317373, /* 'ss19' */
		StylisticSet20 = 0x30327373, /* 'ss20' */
		Subscript = 0x73627573, /* 'subs' */
		Superscript = 0x73707573, /* 'sups' */
		Swash = 0x68737773, /* 'swsh' */
		Tiling = 0x6c746974, /* 'titl' */
		TraditionalNameForms = 0x6d616e74, /* 'tnam' */
		TabularFigures = 0x6d756e74, /* 'tnum' */
		TraditionalForms = 0x64617274, /* 'trad' */
		ThirdWidths = 0x64697774, /* 'twid' */
		Unicase = 0x63696e75, /* 'unic' */
		VerticalWriting = 0x74726576, /* 'vert' */
		VerticalAlternatesAndRotation = 0x32747276, /* 'vrt2' */
		SlashedZero = 0x6f72657a, /* 'zero' */
	}

	public enum DWriteScriptShapes {
		Default = 0,
		NoVisual
	}

	public enum DWriteNumberSubstitutionMethod {
		FromCulture,
		Contextual,
		None,
		National,
		Traditional
	}

	public enum DWriteTextureType {
		Aliased1x1,
		ClearType3x1
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteFontMetrics {

		public ushort DesignUnitsPerEm;
		public ushort Ascent;
		public ushort Descent;
		public short LineGap;
		public ushort CapHeight;
		public ushort XHeight;
		public short UnderlinePosition;
		public ushort UnderlineThickness;
		public short StrikethroughPosition;
		public ushort StrikethroughThickness;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteGlyphMetrics {

		public int LeftSideBearing;
		public uint AdvanceWidth;
		public int RightSideBearing;
		public int TopSideBearing;
		public uint AdvanceHeight;
		public int BottomSideBearing;
		public int VerticalOriginY;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteGlyphOffset {

		public float AdvanceOffset;
		public float AscenderOffset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteMatrix {

		public float M11;
		public float M12;
		public float M21;
		public float M22;
		public float DX;
		public float DY;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteTrimming {

		public DWriteTrimmingGranularity Granularity;
		public uint Delimiter;
		public uint DelimiterCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteGlyphRun {

		[NativeType("IDWriteFontFace*")]
		public IntPtr FontFace;
		public float FontEmSize;
		public uint GlyphCount;
		[NativeType("UINT16 const*")]
		public IntPtr GlyphIndices;
		[NativeType("FLOAT const*")]
		public IntPtr GlyphAdvances;
		[NativeType("DWRITE_GLYPH_OFFSET const*")]
		public IntPtr GlyphOffsets;
		public bool IsSideways;
		public uint BidiLevel;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteGlyphRunDescription {

		[NativeType("WCHAR const*")]
		public IntPtr LocaleName;
		[NativeType("WCHAR const*")]
		public IntPtr String;
		public uint StringLength;
		[NativeType("UINT16 const*")]
		public IntPtr ClusterMap;
		public uint TextPosition;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteUnderline {

		public float Width;
		public float Thickness;
		public float Offset;
		public float RunHeight;
		public DWriteReadingDirection ReadingDirection;
		public DWriteFlowDirection FlowDirection;
		[NativeType("WCHAR const*")]
		public IntPtr LocaleName;
		public DWriteMeasuringMode MeasuringMode;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteStrikethrough {

		public float Width;
		public float Thickness;
		public float Offset;
		public DWriteReadingDirection ReadingDirection;
		public DWriteFlowDirection FlowDirection;
		[NativeType("WCHAR const*")]
		public IntPtr LocaleName;
		public DWriteMeasuringMode MeasuringMode;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteInlineObjectMetrics {

		public float Width;
		public float Height;
		public float Baseline;
		public bool SupportsSideways;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteOverhangMetrics {

		public float Left;
		public float Top;
		public float Right;
		public float Bottom;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteFontFeature {

		public DWriteFontFeatureTag NameTag;
		public uint Parameter;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteTextRange {

		public uint StartPosition;
		public uint Length;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteLineMetrics {

		public uint Length;
		public uint TrailingWhitespaceLength;
		public uint NewlineLength;
		public float Height;
		public float Baseline;
		public bool IsTrimmed;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteTextMetrics {

		public float Left;
		public float Top;
		public float Width;
		public float WidthIncludingTrailingWhitespace;
		public float Height;
		public float LayoutWidth;
		public float LayoutHeight;
		public uint MaxBidiReorderingDepth;
		public uint LineCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteClusterMetrics {

		public float Width;
		public ushort Length;
		private ushort pack0;
		public bool CanWrapLineAfter {
			get => (pack0 & 0x0001) != 0;
			set {
				if (value) pack0 |= 0x0001;
				else pack0 &= 0xFFFE;
			}
		}
		public bool IsWhitespace {
			get => (pack0 & 0x0002) != 0;
			set {
				if (value) pack0 |= 0x0002;
				else pack0 &= 0xFFFD;
			}
		}
		public bool IsNewline {
			get => (pack0 & 0x0004) != 0;
			set {
				if (value) pack0 |= 0x0004;
				else pack0 &= 0xFFFB;
			}
		}
		public bool IsSoftHyphen {
			get => (pack0 & 0x0008) != 0;
			set {
				if (value) pack0 |= 0x0008;
				else pack0 &= 0xFFF7;
			}
		}
		public bool IsRightToLeft {
			get => (pack0 & 0x0010) != 0;
			set {
				if (value) pack0 |= 0x0010;
				else pack0 &= 0xFFEF;
			}
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteHitTestMetrics {

		public uint TextPosition;
		public uint Length;
		public float Left;
		public float Top;
		public float Width;
		public float Height;
		public uint BidiLevel;
		public bool IsText;
		public bool IsTrimmed;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteScriptAnalysis {

		public ushort Script;
		public DWriteScriptShapes Shape;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteLineBreakpoint {

		private byte pack0;
		public DWriteBreakCondition BreakConditionBefore {
			get => (DWriteBreakCondition)(pack0 & 0x03);
			set => pack0 = (byte)((pack0 & 0xFC) | ((int)value & 0x3));
		}
		public DWriteBreakCondition BreakConditionAfter {
			get => (DWriteBreakCondition)((pack0 >> 2) & 0x3);
			set => pack0 = (byte)((pack0 & 0xF3) | (((int)value & 0x3) << 2));
		}
		public bool IsWhitespace {
			get => (pack0 & 0x10) != 0;
			set {
				if (value) pack0 |= 0x10;
				else pack0 &= 0xEF;
			}
		}
		public bool IsSoftHyphen {
			get => (pack0 & 0x20) != 0;
			set {
				if (value) pack0 |= 0x20;
				else pack0 &= 0xDF;
			}
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteTypographicFeatures {

		[NativeType("DWRITE_FONT_FEATURE*")]
		public IntPtr Features;
		public uint FeatureCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteShapingTextProperties {

		private ushort pack0;
		public bool IsShapedAlone {
			get => (pack0 & 0x0001) != 0;
			set {
				if (value) pack0 |= 0x0001;
				else pack0 &= 0xFFFE;
			}
		}
		public bool CanBreakShapingAfter {
			get => (pack0 & 0x0004) != 0;
			set {
				if (value) pack0 |= 0x0004;
				else pack0 &= 0xFFFB;
			}
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteShapingGlyphProperties {

		private ushort pack0;
		public SCRIPT_JUSTIFY Justification {
			get => (SCRIPT_JUSTIFY)(pack0 & 0xF);
			set => pack0 = (ushort)((pack0 & 0xFFF0) | ((int)value & 0xF));
		}
		public bool IsClusterStart {
			get => (pack0 & 0x0010) != 0;
			set {
				if (value) pack0 |= 0x0010;
				else pack0 &= 0xFFEF;
			}
		}
		public bool IsDiacritic {
			get => (pack0 & 0x0020) != 0;
			set {
				if (value) pack0 |= 0x0020;
				else pack0 &= 0xFFDF;
			}
		}
		public bool IsZeroWidthSpace {
			get => (pack0 & 0x0040) != 0;
			set {
				if (value) pack0 |= 0x0040;
				else pack0 &= 0xFFBF;
			}
		}

	}

	[ComImport, Guid("6d4865fe-0ab8-4d91-8f62-5dd6be34a3e0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontFileStream : IUnknown {

		public void ReadFileFragment(out IntPtr fragmentStart, ulong offset, ulong fragmentSize, out IntPtr fragmentContext);

		[PreserveSig]
		public void ReleaseFileFragment(IntPtr fragmentContext);

		public void GetFileSize(out ulong size);

		public void GetLastWriteTime(out ulong lastWriteTime);

	}

	[ComImport, Guid("727cad4e-d6af-4c9e-8a08-d695b11caa49")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontFileLoader : IUnknown {

		public void CreateStreamFromKey(IntPtr key, uint keySize, [MarshalAs(UnmanagedType.Interface)] out IDWriteFontFileStream stream);

	}

	[ComImport, Guid("b2d9f3ec-c9fe-4a11-a2ec-d86208f7c0a2")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteLocalFontFileLoader : IDWriteFontFileLoader {

		public void GetFilePathLengthFromKey(IntPtr key, uint keySize, out uint length);

		public void GetFilePathFromKey(IntPtr key, uint keySize, [NativeType("WCHAR*")] IntPtr path, uint length);

		public void GetLastWriteTimeFromKey(IntPtr key, uint keySize, out FILETIME writeTime);

	}

	[ComImport, Guid("739d886a-cef5-47dc-8769-1a8b41bebbb0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontFile : IUnknown {

		public void GetReferenceKey(out IntPtr key, out uint keySize);

		public void GetLoader([MarshalAs(UnmanagedType.Interface)] out IDWriteFontFileLoader loader);

		public void Analyze(out bool isSupportedFontType, out DWriteFontFileType fileType, out DWriteFontFaceType faceType, out uint facesNum);

	}

	[ComImport, Guid("72755049-5ff7-435d-8348-4be97cfa6c7c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontFileEnumerator : IUnknown {

		public void MoveNext(out bool hasCurrentFile);

		public void GetCurrentFontFile([MarshalAs(UnmanagedType.Interface)] out IDWriteFontFile fontFile);

	}

	[ComImport, Guid("cca920e4-52f0-492b-bfa8-29c72ee0a468")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontCollectionLoader : IUnknown {

		public void CreateEnumeratorFromKey([MarshalAs(UnmanagedType.Interface)] IDWriteFactory factory, IntPtr key, uint keySize, [MarshalAs(UnmanagedType.Interface)] out IDWriteFontFileEnumerator enumerator);

	}

	[ComImport, Guid("08256209-099a-4b34-b86d-c22b110e7771")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteLocalizedStrings : IUnknown {

		[PreserveSig]
		public uint GetCount();

		public void FindLocaleName([MarshalAs(UnmanagedType.LPWStr)] string localeName, out uint index, out bool exists);

		public void GetLocaleNameLength(uint index, out uint length);

		public void GetLocaleName(uint index, [NativeType("WCHAR*")] IntPtr localeName, out uint size);

		public void GetStringLength(uint index, out uint length);

		public void GetString(uint index, [NativeType("WCHAR*")] IntPtr buffer, uint size);

	}

	[ComImport, Guid("2f0da53a-2add-47cd-82ee-d9ec34688e75")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteRenderingParams : IUnknown {

		[PreserveSig]
		public float GetGamma();

		[PreserveSig]
		public float GetEnhancedContrast();

		[PreserveSig]
		public float GetClearTypeLevel();

		[PreserveSig]
		public DWritePixelGeometry GetPixelGeometry();

		[PreserveSig]
		public DWriteRenderingMode GetRenderingMode();

	}

	[ComImport, Guid("5f49804d-7024-4d43-bfa9-d25984f53849")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontFace : IUnknown {

		[PreserveSig]
		public DWriteFontFaceType GetType();

		public void GetFiles(out uint numberOfFiles, [NativeType("IDWriteFontFile**")] IntPtr fontFiles);

		[PreserveSig]
		public uint GetIndex();

		[PreserveSig]
		public DWriteFontSimulations GetSimulations();

		[PreserveSig]
		public bool IsSymbolFont();

		[PreserveSig]
		public void GetMetrics(out DWriteFontMetrics metrics);

		[PreserveSig]
		public ushort GetGlyphCount();

		public void GetDesignGlyphMetrics([NativeType("UINT16 const*")] IntPtr glyphIndices, uint glyphCount, out DWriteGlyphMetrics metrics, bool isSideways = false);

		public void GetGlyphIndices([NativeType("UINT32 const*")] IntPtr codepoints, uint count, [NativeType("UINT16*")] IntPtr glyphIndices);

		public void TryGetFontTable(uint tableTag, out IntPtr tableData, out uint tableSize, out IntPtr context, out bool exists);

		[PreserveSig]
		public void ReleaseFontTable(IntPtr tableContext);

		public void GetGlyphRunOutline(float emSize, [NativeType("UINT16 const*")] IntPtr glyphIndices, [NativeType("FLOAT const*")] IntPtr glyphAdvances, [NativeType("DWRITE_GLYPH_OFFSET const*")] IntPtr glyphOffsets, uint glyphCount, bool isSideways, bool isRtl, [MarshalAs(UnmanagedType.Interface)] out IDWriteGeometrySink geometrySink);

		public void GetRecommendedRenderingMode(float emSize, float pixelsPerDip, DWriteMeasuringMode mode, [MarshalAs(UnmanagedType.Interface)] IDWriteRenderingParams _params, out DWriteRenderingMode renderingMode);

		public void GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, in DWriteMatrix transform, out DWriteFontMetrics metrics);

		public void GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, in DWriteMatrix transform, bool useGdiNatural, [NativeType("UINT16 const*")] IntPtr glyphIndices, uint glyphCount, out DWriteGlyphMetrics metrics, bool isSideways = false);

	}

	[ComImport, Guid("acd16696-8c14-4f5d-877e-fe3fc1d32737")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFont : IUnknown {

		public void GetFontFamily([MarshalAs(UnmanagedType.Interface)] out IDWriteFontFamily family);

		[PreserveSig]
		public DWriteFontWeight GetWeight();

		[PreserveSig]
		public DWriteFontStretch GetStretch();

		[PreserveSig]
		public DWriteFontStyle GetStyle();

		[PreserveSig]
		public bool IsSymbolFont();

		public void GetFaceNames([MarshalAs(UnmanagedType.Interface)] out IDWriteLocalizedStrings names);

		public void GetInformationalStrings(DWriteInformationalStringID stringId, [MarshalAs(UnmanagedType.Interface)] out IDWriteLocalizedStrings strings, out bool exists);

		[PreserveSig]
		public void GetMetrics(out DWriteFontMetrics metrics);

		public void HasCharacter(uint value, out bool exists);

		public void CreateFontFace([MarshalAs(UnmanagedType.Interface)] out IDWriteFontFace face);

	}

	[ComImport, Guid("1a0d8438-1d97-4ec1-aef9-a2fb86ed6acb")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontList : IUnknown {

		public void GetFontCollection([MarshalAs(UnmanagedType.Interface)] out IDWriteFontCollection collection);

		[PreserveSig]
		public uint GetFontCount();

		public void GetFont(uint index, [MarshalAs(UnmanagedType.Interface)] out IDWriteFont font);

	}

	[ComImport, Guid("da20d8ef-812a-4c43-9802-62ec4abd7add")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontFamily : IDWriteFontList {

		public void GetFamilyNames([MarshalAs(UnmanagedType.Interface)] out IDWriteLocalizedStrings names);

		public void GetFirstMatchingFont(DWriteFontWeight weight, DWriteFontStretch stretch, DWriteFontStyle style, [MarshalAs(UnmanagedType.Interface)] out IDWriteFont font);

		public void GetMatchingFonts(DWriteFontWeight weight, DWriteFontStretch stretch, DWriteFontStyle style, [MarshalAs(UnmanagedType.Interface)] out IDWriteFontList fonts);

	}

	[ComImport, Guid("a84cee02-3eea-4eee-a827-87c1a02a0fcc")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontCollection : IUnknown {

		[PreserveSig]
		public uint GetFontFamilyCount();

		public void GetFontFamily(uint index, [MarshalAs(UnmanagedType.Interface)] out IDWriteFontFamily family);

		public void FindFamilyName([MarshalAs(UnmanagedType.LPWStr)] string name, out uint index, out bool exists);

		public void GetFontFromFontFace([MarshalAs(UnmanagedType.Interface)] IDWriteFontFace face, [MarshalAs(UnmanagedType.Interface)] out IDWriteFont font);

	}

	[ComImport, Guid("eaf3a2da-ecf4-4d24-b644-b34f6842024b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWritePixelSnapping : IUnknown {

		public void IsPixelSnappingDisabled(IntPtr clientDrawingContext, out bool disabled);

		public void GetCurrentTransform(IntPtr clientDrawingContext, out DWriteMatrix transform);

		public void GetPixelsPerDip(IntPtr clientDrawingContext, out float pixelsPerDip);

	}

	[ComImport, Guid("ef8a8135-5cc6-45fe-8825-c5a0724eb819")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextRenderer : IDWritePixelSnapping {

		public void DrawGlyphRun(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWriteMeasuringMode mode, in DWriteGlyphRun glyphRun, in DWriteGlyphRunDescription runDescr, [MarshalAs(UnmanagedType.IUnknown)] object? drawingEffect);

		public void DrawUnderline(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, in DWriteUnderline underline, [MarshalAs(UnmanagedType.IUnknown)] object? drawingEffect);

		public void DrawStrikethrough(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, in DWriteStrikethrough strikethrough, [MarshalAs(UnmanagedType.IUnknown)] object? drawingEffect);

		public void DrawInlineObject(IntPtr clientDrawingContext, float originX, float originY, [MarshalAs(UnmanagedType.Interface)] IDWriteInlineObject _object, bool isSideways, bool isRtl, [MarshalAs(UnmanagedType.IUnknown)] object? drawingEffect);

	}

	[ComImport, Guid("8339fde3-106f-47ab-8373-1c6295eb10b3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteInlineObject : IUnknown {

		public void Draw(IntPtr clientDrawingContext, [MarshalAs(UnmanagedType.Interface)] IDWriteTextRenderer renderer, float originX, float originY, bool isSideways, bool isRtl, [MarshalAs(UnmanagedType.IUnknown)] object? drawingEffect);

		public void GetMetrics(out DWriteInlineObjectMetrics metrics);

		public void GetOverhangMetrics(out DWriteOverhangMetrics overhangs);

		public void GetBreakConditions(out DWriteBreakCondition conditionBefore, out DWriteBreakCondition conditionAfter);

	}

	[ComImport, Guid("9c906818-31d7-4fd3-a151-7c5e225db55a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextFormat : IUnknown {

		public void SetTextAlignment(DWriteTextAlignment alignment);

		public void SetParagraphAlignment(DWriteParagraphAlignment alignment);

		public void SetWordWrapping(DWriteWordWrapping wordWrapping);

		public void SetReadingDirection(DWriteReadingDirection direction);

		public void SetFlowDirection(DWriteFlowDirection direction);

		public void SetIncrementalTabStop(float tabstop);

		public void SetTrimming(in DWriteTrimming trimming, [MarshalAs(UnmanagedType.Interface)] IDWriteInlineObject? trimmingSign);

		public void SetLineSpacing(DWriteLineSpacingMethod spacing, float lineSpacing, float baseline);

		[PreserveSig]
		public DWriteTextAlignment GetTextAlignment();

		[PreserveSig]
		public DWriteParagraphAlignment GetParagraphAlignment();

		[PreserveSig]
		public DWriteWordWrapping GetWordWrapping();

		[PreserveSig]
		public DWriteReadingDirection GetReadingDirection();

		[PreserveSig]
		public DWriteFlowDirection GetFlowDirection();

		[PreserveSig]
		public float GetIncrementalTabStop();

		public void GetTrimming(out DWriteTrimming options, [MarshalAs(UnmanagedType.Interface)] out IDWriteInlineObject trimmingSign);

		public void GetLineSpacing(out DWriteLineSpacingMethod method, out float spacing, out float baseline);

		public void GetFontCollection([MarshalAs(UnmanagedType.Interface)] out IDWriteFontCollection collection);

		[PreserveSig]
		public uint GetFontFamilyNameLength();

		public void GetFontFamilyName([NativeType("WCHAR*")] IntPtr name, uint size);

		[PreserveSig]
		public DWriteFontWeight GetFontWeight();

		[PreserveSig]
		public DWriteFontStyle GetFontStyle();

		[PreserveSig]
		public DWriteFontStretch GetFontStretch();

		[PreserveSig]
		public float GetFontSize();

		[PreserveSig]
		public uint GetLocaleNameLength();

		public void GetLocaleName([NativeType("WCHAR*")] IntPtr name, uint size);

	}

	[ComImport, Guid("55f1112b-1dc2-4b3c-9541-f46894ed85b6")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTypography {

		public void AddFontFeature(DWriteFontFeature feature);

		[PreserveSig]
		public uint GetFontFeatureCount();

		public void GetFontFeature(uint index, out DWriteFontFeature feature);

	}

	[ComImport, Guid("5e5a32a3-8dff-4773-9ff6-0696eab77267")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteBitmapRenderTarget : IUnknown {

		public void DrawGlyphRun(float baselineOriginX, float baselineOriginY, DWriteMeasuringMode measuringMode, in DWriteGlyphRun glyphRun, [MarshalAs(UnmanagedType.Interface)] IDWriteRenderingParams _params, COLORREF textColor, [NativeType("RECT*")] IntPtr blackboxRect = default);

		[PreserveSig]
		[return: NativeType("HDC")]
		public IntPtr GetMemoryDC();

		public float GetPixelsPerDip();

		public void SetPixelsPerDip(float pixelsPerDip);

		public void GetCurrentTransform(out DWriteMatrix transform);

		public void SetCurrentTransform(in DWriteMatrix transform);

		public void GetSize(out SIZE size);

		public void Resize(uint width, uint height);

	}

	[ComImport, Guid("1edd9491-9853-4299-898f-6432983b6f3a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteGdiIterop : IUnknown {

		public void CreateFontFromLOGFONT(in LOGFONTW logfont, [MarshalAs(UnmanagedType.Interface)] out IDWriteFont font);

		public void ConvertFontToLOGFONT([MarshalAs(UnmanagedType.Interface)] IDWriteFont font, out LOGFONTW logfont, out bool isSystemFont);

		public void ConvertFontFaceToLOGFONT([MarshalAs(UnmanagedType.Interface)] IDWriteFontFace font, out LOGFONTW logfont);

		public void CreateFontFaceFromHdc([NativeType("HDC")] IntPtr hdc, [MarshalAs(UnmanagedType.Interface)] out IDWriteFontFace fontFace);

		public void CreateBitmapRenderTarget([NativeType("HDC")] IntPtr hdc, uint width, uint height, [MarshalAs(UnmanagedType.Interface)] out IDWriteBitmapRenderTarget target);

	}

	[ComImport, Guid("53737037-6d14-410b-9bfe-0b182bb70961")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextLayout : IDWriteTextFormat {

		public void SetMaxWidth(float maxWidth);

		public void SetMaxHeight(float maxHeight);

		public void SetFontCollection([MarshalAs(UnmanagedType.Interface)] IDWriteFontCollection collection, DWriteTextRange range);

		public void SetFontFamilyName([MarshalAs(UnmanagedType.LPWStr)] string name, DWriteTextRange range);

		public void SetFontWeight(DWriteFontWeight weight, DWriteTextRange range);

		public void SetFontStyle(DWriteFontStyle style, DWriteTextRange range);

		public void SetFontStretch(DWriteFontStretch stretch, DWriteTextRange range);

		public void SetFontSize(float size, DWriteTextRange range);

		public void SetUnderline(bool underline, DWriteTextRange range);

		public void SetStrikethrough(bool strikethrough, DWriteTextRange range);

		public void SetDrawingEffect([MarshalAs(UnmanagedType.IUnknown)] object effect, DWriteTextRange range);

		public void SetInlineObject([MarshalAs(UnmanagedType.Interface)] IDWriteInlineObject _object, DWriteTextRange range);

		public void SetTypography([MarshalAs(UnmanagedType.Interface)] IDWriteTypography typography, DWriteTextRange range);

		public void SetLocaleName([MarshalAs(UnmanagedType.LPWStr)] string locale, DWriteTextRange range);

		[PreserveSig]
		public float GetMaxWidth();

		[PreserveSig]
		public float GetMaxHeight();

		public void GetFontCollection(uint pos, [MarshalAs(UnmanagedType.Interface)] out IDWriteFontCollection collection, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetFontFamilyNameLength(uint pos, out uint len, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetFontFamilyName(uint position, [NativeType("WCHAR*")] IntPtr name, uint nameSize, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetFontWeight(uint position, out DWriteFontWeight weight, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetFontStyle(uint currentPosition, out DWriteFontStyle style, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetFontStretch(uint position, out DWriteFontStretch stretch, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetFontSize(uint position, out float size, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetUnderline(uint position, out bool hasUnderline, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetStrikethrough(uint position, out bool hasStrikethrough, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetDrawingEffect(uint position, [MarshalAs(UnmanagedType.IUnknown)] out IUnknown effect, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetInlineObject(uint position, [MarshalAs(UnmanagedType.Interface)] out IDWriteInlineObject _object, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetTypography(uint position, [MarshalAs(UnmanagedType.Interface)] out IDWriteTypography typography, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetLocaleNameLength(uint position, out uint length, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void GetLocaleName(uint position, [NativeType("WCHAR*")] IntPtr name, uint nameSize, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

		public void Draw(IntPtr context, [MarshalAs(UnmanagedType.Interface)] IDWriteTextRenderer renderer, float originX, float originY);

		public void GetLineMetrics([NativeType("DWRITE_LINE_METRICS*")] IntPtr metrics, uint maxCount, out uint actualCount);

		public void GetMetrics(out DWriteTextMetrics metrics);

		public void GetOverhangMetrics(out DWriteOverhangMetrics overhangs);

		public void GetClusterMetrics([NativeType("DWRITE_CLUSTER_METRICS*")] IntPtr metrics, uint maxCount, out uint actualCount);

		public void DetermineMinWidth(out float minWidth);

		public void HitTestPoint(float pointX, float pointY, out bool isTrailingHit, out bool isInside, out DWriteHitTestMetrics metrics);

		public void HitTestTextPosition(uint textPosition, bool isTrailingHit, out float pointX, out float pointY, out DWriteHitTestMetrics metrics);

		public void HitTestTextRange(uint textPosition, uint textLength, float originX, float originY, [NativeType("DWRITE_HIT_TEST_METRICS*")] IntPtr metrics, uint maxMetricsCount, out uint actualMetricsCount);

	}

	[ComImport, Guid("14885cc9-bab0-4f90-b6ed-5c366a2cd03d")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteNumberSubstitution : IUnknown {

	}

	[ComImport, Guid("688e1a58-5094-47c8-adc8-fbcea60ae92b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextAnalysisSource : IUnknown {

		public void GetTextAtPosition(uint position, [NativeType("WCHAR const*")] out IntPtr text, out uint textLength);

		public void GetTextBeforePosition(uint position, [NativeType("WCHAR const*")] out IntPtr text, out uint textLength);

		[PreserveSig]
		public DWriteReadingDirection GetParagraphReadingDirection();

		public void GetLocaleName(uint position, out uint textLen, [NativeType("WCHAR const*")] out IntPtr locale);

		public void GetNumberSubsitution(uint position, out uint textLen, [MarshalAs(UnmanagedType.Interface)] out IDWriteNumberSubstitution substitution);

	}

	[ComImport, Guid("5810cd44-0ca0-4701-b3fa-bec5182ae4f6")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextAnalysisSink : IUnknown {

		public void SetScriptAnalysis(uint position, uint length, [NativeType("DWRITE_SCRIPT_ANALYSIS const*")] IntPtr scriptAnalysis);

		public void SetScriptBreakpoints(uint position, uint length, [NativeType("DWRITE_LINE_BREAKPOINT const*")] IntPtr scriptAnalysis);

		public void SetBidiLevel(uint position, uint length, byte explicitLevel, byte resolvedLevel);

		public void SetNumberSubstitution(uint position, uint length, [MarshalAs(UnmanagedType.Interface)] IDWriteNumberSubstitution substitution);

	}

	[ComImport, Guid("b7e6163e-7f46-43b4-84b3-e4e6249c365d")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextAnalyzer {

		public void AnalyzeScript([MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSource source, uint position, uint length, [MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSink sink);

		public void AnalyzeBidi([MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSource source, uint position, uint length, [MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSink sink);

		public void AnalyzeNumberSubstitution([MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSource source, uint position, uint length, [MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSink sink);

		public void AnalyzeLineBreakpoints([MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSource source, uint position, uint length, [MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSink sink);

		public void GetGlyphs(
			[MarshalAs(UnmanagedType.LPWStr)] string text,
			uint length,
			[MarshalAs(UnmanagedType.Interface)] IDWriteFontFace fontFace,
			bool isSideways,
			bool isRtl,
			in DWriteScriptAnalysis analysis,
			[MarshalAs(UnmanagedType.LPWStr)] string? locale,
			[MarshalAs(UnmanagedType.Interface)] IDWriteNumberSubstitution? substitution,
			[NativeType("DWRITE_TYPOGRAPHIC_FEATURES const**")] IntPtr features,
			[NativeType("UINT32 const*")] IntPtr featureRangeLen,
			uint featureRanges,
			uint maxGlyphCount,
			[NativeType("UINT16*")] IntPtr clustermap,
			[NativeType("DWRITE_SHAPING_TEXT_PROPERTIES*")] IntPtr textProps,
			[NativeType("UINT16*")] IntPtr glyphIndices,
			[NativeType("DWRITE_SHAPING_GLYPH_PROPERTIES*")] IntPtr glyphProps,
			out uint actualGlyphCount
		);

		public void GetGlyphPlacements(
			[MarshalAs(UnmanagedType.LPWStr)] string text,
			[NativeType("UINT16 const*")] IntPtr clustermap,
			[NativeType("DWRITE_SHAPING_TEXT_PROPERTIES*")] IntPtr props,
			uint textLen,
			[NativeType("UINT16 const*")] IntPtr glyphIndices,
			[NativeType("DWRITE_SHAPING_GLYPH_PROPERTIES const*")] IntPtr glyphProps,
			uint glyphCount,
			[MarshalAs(UnmanagedType.Interface)] IDWriteFontFace fontFace,
			float fontEmSize,
			bool isSideways,
			bool isRtl,
			in DWriteScriptAnalysis analysis,
			[MarshalAs(UnmanagedType.LPWStr)] string? locale,
			[NativeType("DWRITE_TYPOGRAPHIC_FEATURES const**")] IntPtr features,
			[NativeType("UINT32 const*")] IntPtr featureRangeLen,
			uint featureRanges,
			[NativeType("FLOAT*")] IntPtr glyphAdvances,
			[NativeType("DWRITE_GLYPH_OFFSET*")] IntPtr glyphOffsets
		);

		public void GetGdiCompatibleGlyphPlacements(
			[MarshalAs(UnmanagedType.LPWStr)] string text,
			[NativeType("UINT16 const*")] IntPtr clustermap,
			[NativeType("DWRITE_SHAPING_TEXT_PROPERTIES*")] IntPtr props,
			uint textLen,
			[NativeType("UINT16 const*")] IntPtr glyphIndices,
			[NativeType("DWRITE_SHAPING_GLYPH_PROPERTIES const*")] IntPtr glyphProps,
			uint glyphCount,
			[MarshalAs(UnmanagedType.Interface)] IDWriteFontFace fontFace,
			float fontEmSize,
			float pixelsPerDip,
			in DWriteMatrix transform,
			bool useGdiNatural,
			bool isSideways,
			bool isRtl,
			in DWriteScriptAnalysis analysis,
			[MarshalAs(UnmanagedType.LPWStr)] string? locale,
			[NativeType("DWRITE_TYPOGRAPHIC_FEATURES const**")] IntPtr features,
			[NativeType("UINT32 const*")] IntPtr featureRangeLen,
			uint featureRanges,
			[NativeType("FLOAT*")] IntPtr glyphAdvances,
			[NativeType("DWRITE_GLYPH_OFFSET*")] IntPtr glyphOffsets
		);

	}

	[ComImport, Guid("7d97dbf7-e085-42d4-81e3-6a883bded118")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteGlyphRunAnalysis : IUnknown {

		public void GetAlphaTextureBounds(DWriteTextureType type, out RECT bounds);

		public void CreateAlphaTexture(DWriteTextureType type, in RECT bounds, [NativeType("BYTE*")] IntPtr alphaValues, uint bufferSize);

		public void GetAlphaBlendParams([MarshalAs(UnmanagedType.Interface)] IDWriteRenderingParams renderingParams, [NativeType("FLOAT*")] IntPtr blendGamma, [NativeType("FLOAT*")] IntPtr blendEnhancedContrast, [NativeType("FLOAT*")] IntPtr blendClearTypeLevel);

	}

	[ComImport, Guid("b859ee5a-d838-4b5b-a2e8-1adc7d93db48")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFactory {

		public void GetSystemFontCollection([MarshalAs(UnmanagedType.Interface)] out IDWriteFontCollection collection, bool checkForUpdates = false);

		public void CreateCustomFontCollection([MarshalAs(UnmanagedType.Interface)] IDWriteFontCollectionLoader loader, IntPtr key, uint keySize, [MarshalAs(UnmanagedType.Interface)] out IDWriteFontCollection collection);

		public void RegisterFontCollectionLoader([MarshalAs(UnmanagedType.Interface)] IDWriteFontCollectionLoader loader);

		public void UnregisterFontCollectionLoader([MarshalAs(UnmanagedType.Interface)] IDWriteFontCollectionLoader loader);

		public void CreateFontFileReference([MarshalAs(UnmanagedType.LPWStr)] string path, out FILETIME writetime, [MarshalAs(UnmanagedType.Interface)] out IDWriteFontFile fontFile);

		public void CreateCustomFontFileReference(IntPtr referenceKey, uint keySize, [MarshalAs(UnmanagedType.Interface)] IDWriteFontFileLoader loader, [MarshalAs(UnmanagedType.Interface)] out IDWriteFontFile fontFile);

		public void CreateFontFace(DWriteFontFaceType faceType, uint filesNumber, [NativeType("IDWriteFontFile* const*")] IntPtr fontFiles, uint index, DWriteFontSimulations simFlags, [MarshalAs(UnmanagedType.Interface)] out IDWriteFontFace fontFace);

		public void CreateRenderingParams([MarshalAs(UnmanagedType.Interface)] out IDWriteRenderingParams _params);

		public void CreateMonitorRenderingParams([NativeType("HMONITOR")] IntPtr monitor, [MarshalAs(UnmanagedType.Interface)] out IDWriteRenderingParams _params);

		public void CreateCustomRenderingParams(float gamma, float enhancedContrast, float cleartypeLevel, DWritePixelGeometry geometry, DWriteRenderingMode mode, [MarshalAs(UnmanagedType.Interface)] out IDWriteRenderingParams _params);

		public void RegisterFontFileLoader([MarshalAs(UnmanagedType.Interface)] IDWriteFontFileLoader loader);

		public void UnregisterFontFileLoader([MarshalAs(UnmanagedType.Interface)] IDWriteFontFileLoader loader);

		public void CreateTextFormat([MarshalAs(UnmanagedType.LPWStr)] string familyName, [MarshalAs(UnmanagedType.Interface)] IDWriteFontCollection collection, DWriteFontWeight weight, DWriteFontStyle style, DWriteFontStretch stretch, float size, [MarshalAs(UnmanagedType.LPWStr)] string? locale, [MarshalAs(UnmanagedType.Interface)] out IDWriteTextFormat format);

		public void CreateTypography([MarshalAs(UnmanagedType.Interface)] out IDWriteTypography typography);

		public void GetGdiInterop([MarshalAs(UnmanagedType.Interface)] out IDWriteGdiIterop gdiInterop);

		public void CreateTextLayout([MarshalAs(UnmanagedType.LPWStr)] string _string, uint len, [MarshalAs(UnmanagedType.Interface)] IDWriteTextFormat format, float maxWidth, float maxHeight, [MarshalAs(UnmanagedType.Interface)] out IDWriteTextLayout layout);

		public void CreateGdiCompatibleTextLayout([MarshalAs(UnmanagedType.LPWStr)] string _string, uint len, [MarshalAs(UnmanagedType.Interface)] IDWriteTextFormat format, float layoutWidth, float layoutHeight, float pixelsPerDip, in DWriteMatrix transform, bool useGdiNatural, [MarshalAs(UnmanagedType.Interface)] out IDWriteTextLayout layout);

		public void CreateEllipsisTrimmingSign([MarshalAs(UnmanagedType.Interface)] IDWriteTextFormat format, [MarshalAs(UnmanagedType.Interface)] out IDWriteInlineObject trimmingSign);

		public void CreateTextAnalyzer([MarshalAs(UnmanagedType.Interface)] out IDWriteTextAnalyzer analyzer);

		public void CreateNumberSubstitution(DWriteNumberSubstitutionMethod method, [MarshalAs(UnmanagedType.LPWStr)] string? locale, bool ignoreUserOverride, [MarshalAs(UnmanagedType.Interface)] out IDWriteNumberSubstitution substitution);

		public void CreateGlyphRunAnalysis(in DWriteGlyphRun glyphRun, float pixelsPerDip, in DWriteMatrix transform, DWriteRenderingMode renderingMode, DWriteMeasuringMode measuringMode, float baselineX, float baselineY, [MarshalAs(UnmanagedType.Interface)] out IDWriteGlyphRunAnalysis analysis);

	}

	public static partial class DWrite {

		[DllImport("DWrite.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IUnknown DWriteCreateFactory(DWriteFactoryType type, Guid riid);

		public static T CreateFactory<T>(DWriteFactoryType type) where T : class =>
			COMHelpers.GetObjectFromCOMGetter<T>((in Guid riid) => DWriteCreateFactory(type, riid).QueryInterface(riid))!;

	}

}

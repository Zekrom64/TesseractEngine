using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.DirectX.GI;

namespace Tesseract.DirectX.DirectWrite {
	
	public enum DWritePanoseFamily : byte {
		Any,
		NoFit,
		TextDisplay,
		Script,
		Decorative,
		Symbol,
		Pictoral = Symbol
	}

	public enum DWritePanoseSerifStyle : byte {
		Any,
		NoFit,
		Cove,
		ObtuseCove,
		SquareCove,
		ObtuseSquareCove,
		Square,
		Thin,
		Oval,
		Exaggerated,
		Triangle,
		NormalSans,
		ObtuseSans,
		PerpendicularSans,
		Flared,
		Rounded,
		Script,
		Bone = Oval
	}

	public enum DWritePanoseWeight : byte {
		Any,
		NoFit,
		VeryLight,
		Light,
		Thin,
		Book,
		Medium,
		Demi,
		Bold,
		Heavy,
		Black,
		ExtraBlack,
		Nord = ExtraBlack
	}

	public enum DWritePanoseProportion : byte {
		Any,
		NoFit,
		OldStyle,
		Modern,
		EvenWidth,
		Expanded,
		Condensed,
		VeryExpanded,
		VeryCondensed,
		Monospaced
	}

	public enum DWritePanoseContrast : byte {
		Any,
		NoFit,
		None,
		VeryLow,
		Low,
		MediumLow,
		Medium,
		MediumHigh,
		High,
		VeryHigh,
		HorizontalLow,
		HorizontalMedium,
		HorizontalHigh
	}

	public enum DWritePanoseStrokeVariation : byte {
		Any,
		NoFit,
		NoVariation,
		GradualDiagonal,
		GradualTransitional,
		GradualVertical,
		GradualHorizontal,
		RapidVertical,
		RapidHorizontal,
		InstantVertical,
		InstantHorizontal
	}

	public enum DWritePanoseArmStyle : byte {
		Any,
		NoFit,
		StraightArmsHorizontal,
		StraightArmsWedge,
		StraightArmsVertical,
		StraightArmsSingleSerif,
		StraightArmsDoubleSerif,
		NonStraightArmsHorizontal,
		NonStraightArmsWedge,
		NonStraightArmsVertical,
		NonStraightArmsSingleSerif,
		NonStraightArmsDoubleSerif
	}

	public enum DWritePanoseLetterForm : byte {
		Any,
		NoFit,
		NormalContact,
		NormalWeighted,
		NormalBoxed,
		NormalFlattened,
		NormalRounded,
		NormalOffCenter,
		NormalSquare,
		ObliqueContact,
		ObliqueWeighted,
		ObliqueBoxed,
		ObliqueFlattened,
		ObliqueRounded,
		ObliqueOffCenter,
		ObliqueSquare
	}

	public enum DWritePanoseMidline : byte {
		Any,
		NoFit,
		StandardTrimmed,
		StandardPointed,
		StandardSerifed,
		HighTrimmed,
		HighPointed,
		HighSerifed,
		ConstantTrimmed,
		ConstantPointed,
		ConstantSerifed,
		LowTrimmed,
		LowPointed,
		LowSerifed
	}

	public enum DWritePanoseXHeight : byte {
		Any,
		NoFit,
		ConstantSmall,
		ConstantStandard,
		ConstantLarge,
		DuckingSmall,
		DuckingStandard,
		DuckingLarge
	}

	public enum DWritePanoseToolKind : byte {
		Any,
		NoFit,
		FlatNib,
		PressurePoint,
		Engraved,
		Ball,
		Brush,
		Rough,
		FeltPenBrushTip,
		WildBrush
	}

	public enum DWritePanoseSpacing : byte {
		Any,
		NoFit,
		ProprotionalSpaced,
		Monospaced
	}

	public enum DWritePanoseAspectRatio : byte {
		Any,
		NoFit,
		VeryCondensed,
		Condensed,
		Normal,
		Expanded,
		VeryExpanded
	}

	public enum DWritePanoseScriptTopology : byte {
		Any,
		NoFit,
		RomanDisconnected,
		RomanTrailing,
		RomanConnected,
		CursiveDisconnected,
		CursiveTrailing,
		CursiveConnected,
		BlackletterDisconnected,
		BlackletterRailing,
		BlackletterConnected
	}

	public enum DWritePanoseScriptForm : byte {
		Any,
		NoFit,
		UprightNoWrapping,
		UprightSomeWrapping,
		UprightMoreWrapping,
		UprightExtremeWrapping,
		ObliqueNoWrapping,
		ObliqueSomeWrapping,
		ObliqueMoreWrapping,
		ObliqueExtremeWrapping,
		ExaggeratedNoWrapping,
		ExaggeratedSomeWrapping,
		ExaggeratedMoreWrapping,
		ExaggeratedExtremeWrapping
	}

	public enum DWritePanoseFinials : byte {
		Any,
		NoFit,
		NoneNoLoops,
		NoneClosedLoops,
		NoneOpenLoops,
		SharpNoLoops,
		SharpClosedLoops,
		SharpOpenLoops,
		TaperedNoLoops,
		TaperedClosedLoops,
		TaperedOpenLoops,
		RoundNoLoops,
		RoundClosedLoops,
		RoundOpenLoops
	}

	public enum DWritePanoseXAscent : byte {
		Any,
		NoFit,
		VeryLow,
		Low,
		Medium,
		High,
		VeryHigh
	}

	public enum DWritePanoseDecorativeClass : byte {
		Any,
		NoFit,
		Derivative,
		NonstandardTopology,
		NonstandardElements,
		NonstandardAspect,
		Initials,
		Cartoon,
		PictureStems,
		Ornamented,
		TextAndBackground,
		Collage,
		Montage
	}

	public enum DWritePanoseAspect : byte {
		Any,
		NoFit,
		SuperCondensed,
		VeryCondensed,
		Condensed,
		Normal,
		Extended,
		VeryExtended,
		SuperExtended,
		Monospaced
	}

	public enum DWritePanoseFill : byte {
		Any,
		NoFit,
		StandardSolidFill,
		NoFill,
		PatternedFill,
		ComplexFill,
		ShapedFill,
		DrawnDistressed
	}

	public enum DWritePanoseLining : byte {
		Any,
		NoFit,
		None,
		Inline,
		Outline,
		Engraved,
		Shadow,
		Relief,
		Backdrop
	}

	public enum DWritePanoseDecorativeTopology : byte {
		Any,
		NoFit,
		Standard,
		Square,
		MultipleSegment,
		ArtDeco,
		UnevenWeighting,
		DiverseArms,
		DiverseForms,
		LombardicForms,
		UpperCaseInLowerCase,
		ImpliedTopology,
		HorseshoeEAndA,
		Cursive,
		Blackletter,
		SwashVariance
	}

	public enum DWritePanoseCharacterRanges : byte {
		Any,
		NoFit,
		ExtendedCollection,
		Literals,
		NoLowerCase,
		SmallCaps
	}

	public enum DWritePanoseSymbolKind : byte {
		Any,
		NoFit,
		Montages,
		Pictures,
		Shapes,
		Scientific,
		Music,
		Expert,
		Patterns,
		Boarders,
		Icons,
		Logos,
		IndustrySpecific
	}

	public enum DWritePanoseSymbolAspectRatio : byte {
		Any,
		NoFit,
		NoWidth,
		ExceptionallyWide,
		SuperWide,
		VeryWide,
		Wide,
		Normal,
		Narrow,
		VeryNarrow
	}

	public enum DWriteOutlineThreshold {
		Antialiased,
		Aliased
	}

	public enum DWriteBaseline {
		Default,
		Roman,
		Central,
		Math,
		Hanging,
		IdeographicBottom,
		IdeographicTop,
		Minimum,
		Maximum
	}

	public enum DWriteVerticalGlyphOrientation {
		Default,
		Stacked
	}

	public enum DWriteGlyphOrientationAngle {
		Angle0Degrees,
		Angle90Degrees,
		Angle180Degrees,
		Angle270Degrees
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteFontMetrics1 {

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
		public short GlyphBoxLeft;
		public short GlyphBoxTop;
		public short GlyphBoxRight;
		public short GlyphBoxBottom;
		public short SubscriptPositionX;
		public short SubscriptPositionY;
		public short SubscriptSizeX;
		public short SubscriptSizeY;
		public short SuperscriptPositionX;
		public short SuperscriptPositionY;
		public short SuperscriptSizeX;
		public short SuperscriptSizeY;
		public bool HasTypographicMetrics;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteCaretMetrics {

		public short SlopeRise;
		public short SlopeRun;
		public short Offset;

	}

	[StructLayout(LayoutKind.Explicit)]
	public struct DWritePanose {

		[FieldOffset(0)]
		private unsafe fixed byte Values[10];
		[FieldOffset(10)]
		public DWritePanoseFamily FamilyKind;

		[StructLayout(LayoutKind.Sequential)]
		public struct DWritePanoseText {

			public DWritePanoseFamily FamilyKind;
			public DWritePanoseSerifStyle SerifStyle;
			public DWritePanoseWeight Weight;
			public DWritePanoseProportion Proportion;
			public DWritePanoseStrokeVariation StrokeVariation;
			public DWritePanoseArmStyle ArmStyle;
			public DWritePanoseLetterForm LetterForm;
			public DWritePanoseMidline Midline;
			public DWritePanoseXHeight XHeight;

		}

		[FieldOffset(0)]
		public DWritePanoseText Text;

		[StructLayout(LayoutKind.Sequential)]
		public struct DWritePanoseScript {

			public DWritePanoseFamily FamilyKind;
			public DWritePanoseToolKind ToolKind;
			public DWritePanoseWeight Weight;
			public DWritePanoseSpacing Spacing;
			public DWritePanoseAspectRatio AspectRatio;
			public DWritePanoseContrast Contrast;
			public DWritePanoseScriptTopology ScriptTopology;
			public DWritePanoseScriptForm ScriptForm;
			public DWritePanoseFinials Finials;
			public DWritePanoseXAscent XAscent;

		}

		[FieldOffset(0)]
		public DWritePanoseScript Script;

		[StructLayout(LayoutKind.Sequential)]
		public struct DWritePanoseDecorative {

			public DWritePanoseFamily FamilyKind;
			public DWritePanoseDecorativeClass DecorativeClass;
			public DWritePanoseWeight Weight;
			public DWritePanoseAspect Aspect;
			public DWritePanoseContrast Contrast;
			public DWritePanoseSerifStyle SerifVariant;
			public DWritePanoseFill Fill;
			public DWritePanoseLining Linig;
			public DWritePanoseDecorativeTopology DecorativeTopology;
			public DWritePanoseCharacterRanges CharacterRange;

		}

		[FieldOffset(0)]
		public DWritePanoseDecorative Decorative;

		[StructLayout(LayoutKind.Sequential)]
		public struct DWritePanoseSymbol {

			public DWritePanoseFamily FamilyKind;
			public DWritePanoseSymbolKind SymbolKind;
			public DWritePanoseWeight Weight;
			public DWritePanoseSpacing Spacing;
			public DWritePanoseSymbolAspectRatio AspectRatioAndContrast;
			public DWritePanoseSymbolAspectRatio AspectRatio94;
			public DWritePanoseSymbolAspectRatio AspectRatio119;
			public DWritePanoseSymbolAspectRatio AspectRatio157;
			public DWritePanoseSymbolAspectRatio AspectRatio163;
			public DWritePanoseSymbolAspectRatio AspectRatio211;

		}

		[FieldOffset(0)]
		public DWritePanoseSymbol Symbol;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteUnicodeRange {

		public uint First;
		public uint Last;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteScriptProperties {

		public uint IsoScriptCode;
		public uint IsoScriptNumber;
		public uint ClusterLookahead;
		public uint JustificationCharacter;
		private uint pack0;
		public bool RestrictCaretToClusters {
			get => (pack0 & 0x01) != 0;
			set {
				if (value) pack0 |= 0x01;
				else pack0 &= ~0x1u;
			}
		}
		public bool UsesWordDividers {
			get => (pack0 & 0x02) != 0;
			set {
				if (value) pack0 |= 0x02;
				else pack0 &= ~0x02u;
			}
		}
		public bool IsDiscreteWriting {
			get => (pack0 & 0x04) != 0;
			set {
				if (value) pack0 |= 0x04;
				else pack0 &= ~0x04u;
			}
		}
		public bool IsBlockWriting {
			get => (pack0 & 0x08) != 0;
			set {
				if (value) pack0 |= 0x08;
				else pack0 &= ~0x08u;
			}
		}
		public bool IsDistributedWithinCluster {
			get => (pack0 & 0x10) != 0;
			set {
				if (value) pack0 |= 0x10;
				else pack0 &= ~0x10u;
			}
		}
		public bool IsConnectedWriting {
			get => (pack0 & 0x20) != 0;
			set {
				if (value) pack0 |= 0x20;
				else pack0 &= ~0x20u;
			}
		}
		public bool IsCursiveWriting {
			get => (pack0 & 0x40) != 0;
			set {
				if (value) pack0 |= 0x40;
				else pack0 &= ~0x40u;
			}
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteJustificationOpportunity {

		public float ExpansionMinimum;
		public float ExpansionMaximum;
		public float CompressionMaximum;
		private uint pack0;
		public byte ExpansionPriority {
			get => (byte)pack0;
			set => pack0 = (pack0 & 0xFFFFFF00) | value;
		}
		public byte CompressionPriority {
			get => (byte)(pack0 >> 8);
			set => pack0 = (pack0 & 0xFFFF00FF) | ((uint)value << 8);
		}
		public bool AllowResidualExpansion {
			get => (pack0 & 0x10000) != 0;
			set {
				if (value) pack0 |= 0x10000;
				else pack0 &= ~0x10000u;
			}
		}
		public bool AllowResidualCompression {
			get => (pack0 & 0x20000) != 0;
			set {
				if (value) pack0 |= 0x20000;
				else pack0 &= ~0x20000u;
			}
		}
		public bool ApplyToLeadingEdge {
			get => (pack0 & 0x40000) != 0;
			set {
				if (value) pack0 |= 0x40000;
				else pack0 &= ~0x40000u;
			}
		}
		public bool ApplyToTrailingEdge {
			get => (pack0 & 0x80000) != 0;
			set {
				if (value) pack0 |= 0x80000;
				else pack0 &= ~0x80000u;
			}
		}

	}

	[ComImport, Guid("30572f99-dac6-41db-a16e-0486307e606a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFactory1 : IDWriteFactory {

		public void GetEudcFontCollection([MarshalAs(UnmanagedType.Interface)] out IDWriteFontCollection collection, bool checkForUpdates = false);

		public void CreateCustomRenderingParams(float gamma, float enhcontrast, float enhcontrastGrayscale, float cleartypeLevel, DWritePixelGeometry geometry, DWriteRenderingMode mode, [MarshalAs(UnmanagedType.Interface)] out IDWriteRenderingParams1 _params);

	}

	[ComImport, Guid("a71efdb4-9fdb-4838-ad90-cfc3be8c3daf")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontFace1 : IDWriteFontFace {

		[PreserveSig]
		public void GetMetrics(out DWriteFontMetrics1 metrics);

		public void GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, in DWriteMatrix transform, out DWriteFontMetrics1 metrics);

		[PreserveSig]
		public void GetCaretMetrics(out DWriteCaretMetrics metrics);

		public void GetUnicodeRanges(uint maxCount, [NativeType("DWRITE_UNICODE_RANGE*")] IntPtr range, out uint count);

		[PreserveSig]
		public bool IsMonospacedFont();

		public void GetDesignGlyphAdvances(uint glyphCount, [NativeType("UINT16 const*")] IntPtr indices, [NativeType("INT32*")] IntPtr advances, bool isSideways = false);

		public void GetGdiCompatibleGlyphAdvances(float emSize, float pixelsPerDip, in DWriteMatrix transform, bool useGdiNatural, bool isSideways, uint glyphCount, [NativeType("const UINT16*")] IntPtr indices, [NativeType("INT32*")] IntPtr advances);

		public void GetKerningPairAdjustments(uint glyphCount, [NativeType("const UINT16*")] IntPtr indices, [NativeType("INT32*")] IntPtr adjustments);

		[PreserveSig]
		public bool HasKerningPairs();

		public void DetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, in DWriteMatrix transform, bool isSideways, DWriteOutlineThreshold threshold, DWriteMeasuringMode measuringMode, out DWriteRenderingMode renderingMode);

		public void GetVerticalGlyphVariants(uint glyphCount, [NativeType("const UINT16*")] IntPtr nominalIndices, [NativeType("UINT16*")] IntPtr verticalIndices);

		[PreserveSig]
		public bool HasVerticalGlyphVariants();

	}

	[ComImport, Guid("acd16696-8c14-4f5d-877e-fe3fc1d32738")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFont1 : IDWriteFont {

		[PreserveSig]
		public void GetMetrics(out DWriteFontMetrics1 metrics);

		[PreserveSig]
		public void GetPanose(out DWritePanose panose);

		public void GetUnicodeRanges(uint maxCount, [NativeType("DWRITE_UNICODE_RANGE*")] IntPtr range, out uint count);

		[PreserveSig]
		public bool IsMonospacedFont();

	}

	[ComImport, Guid("94413cf4-a6fc-4248-8b50-6674348fcad3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteRenderingParams1 : IDWriteRenderingParams {

		[PreserveSig]
		public float GetGrayscaleEnhancedContrast();

	}

	[ComImport, Guid("80dad800-e21f-4e83-96ce-bfcce500db7c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextAnalyzer1 : IDWriteTextAnalyzer {

		public void ApplyCharacterSpacing(float leadingSpacing, float trailingSpacing, float minAdvanceWidth, uint len, uint glyphCount, [NativeType("UINT16 const*")] IntPtr clustermap, [NativeType("FLOAT const*")] IntPtr advances, [NativeType("DWRITE_GLYPH_OFFSET const*")] IntPtr offsets, [NativeType("DWRITE_SHAPING_GLYPH_PROPERTIES const*")] IntPtr props, [NativeType("FLOAT*")] IntPtr modifiedAdvances, [NativeType("DWRITE_GLYPH_OFFSET*")] IntPtr modifiedOffsets);

		public void GetBaseline([MarshalAs(UnmanagedType.Interface)] IDWriteFontFace face, DWriteBaseline baseline, bool vertical, bool isSimulationAllowed, DWriteScriptAnalysis sa, [MarshalAs(UnmanagedType.LPWStr)] string? localeName, [NativeType("INT32*")] IntPtr baselineCoord, out bool exists);

		public void AnalyzeVerticalGlyphOrientation([MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSource1 source, uint textPos, uint len, [MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSink1 sink);

		public void GetGlyphOrientationTransform(DWriteGlyphOrientationAngle angle, bool isSideways, out DWriteMatrix transform);

		public void GetScriptProperties(DWriteScriptAnalysis sa, out DWriteScriptProperties props);

		public void GetTextComplexity([MarshalAs(UnmanagedType.LPWStr)] string text, uint len, [MarshalAs(UnmanagedType.Interface)] IDWriteFontFace face, out bool isSimple, out uint lenRead, [NativeType("UINT16*")] IntPtr indices);

		public void GetJustificationOpportunities([MarshalAs(UnmanagedType.Interface)] IDWriteFontFace face, float fontEmSize, DWriteScriptAnalysis sa, uint length, uint glyphCount, [MarshalAs(UnmanagedType.LPWStr)] string text, [NativeType("const UINT16*")] IntPtr clustermap, [NativeType("DWRITE_SHAPING_GLYPH_PROPERTIES*")] IntPtr prop, [NativeType("DWRITE_JUSTIFICATION_OPPORTUNITY*")] IntPtr jo);

		public void JustifyGlyphAdvances(float width, uint glyphCount, [NativeType("const DWRITE_JUSTIFICATION_OPPORTUNITY*")] IntPtr po, [NativeType("const FLOAT*")] IntPtr advances, [NativeType("const DWRITE_GLYPH_OFFSET*")] IntPtr offsets, [NativeType("FLOAT*")] IntPtr justifiedAdvances, [NativeType("DWRITE_GLYPH_OFFSET*")] IntPtr justifiedOffsets);

		public void GetJustifiedGlyphs([MarshalAs(UnmanagedType.Interface)] IDWriteFontFace face, float fontEmSize, DWriteScriptAnalysis sa, uint length, uint glyphCount, uint maxGlyphCount, [NativeType("const UINT16*")] IntPtr clustermap, [NativeType("const UINT16*")] IntPtr indices, [NativeType("const FLOAT*")] IntPtr advances, [NativeType("const FLOAT*")] IntPtr justifiedAdvances, [NativeType("const DWRITE_GLYPH_OFFSET*")] IntPtr justifiedOffsets, [NativeType("const DWRITE_SHAPING_GLYPH_PROPERTIES*")] IntPtr prop, out uint actualCount, [NativeType("UINT16*")] IntPtr modifiedClustermap, [NativeType("UINT16*")] IntPtr modifiedIndices, [NativeType("FLOAT*")] IntPtr modifiedAdvances, [NativeType("DWRITE_GLYPH_OFFSET*")] IntPtr modifiedOffsets);

	}

	[ComImport, Guid("639cfad8-0fb4-4b21-a58a-067920120009")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextAnalysisSource1 : IDWriteTextAnalysisSource {

		public void GetVerticalGlyphOrientation(uint pos, out uint length, out DWriteVerticalGlyphOrientation orientation, out byte bidiLevel);

	}

	[ComImport, Guid("b0d941a0-85e7-4d8b-9fd3-5ced9934482a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextAnalysisSink1 : IDWriteTextAnalysisSink {

		public void SetGlyphOrientation(uint pos, uint length, DWriteGlyphOrientationAngle angle, byte adjustedBidiLevel, bool isSideways, bool isRtl);

	}

	[ComImport, Guid("9064d822-80a7-465c-a986-df65f78b8feb")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextLayout1 : IDWriteTextLayout {

		public void SetPairKerning(bool isPairKerningEnabled, DWriteTextRange range);

		public void GetPairKerning(uint position, out uint isPairKerningEnabled, out DWriteTextRange range);

		public void SetCharacterSpacing(float leadingSpacing, float trailingSpacing, float minimumAdvanceWidth, DWriteTextRange range);

		public void GetCharacterSpacing(uint position, out float leadingSpacing, out float trailingSpacing, out float minimumAdvanceWidth, [NativeType("DWRITE_TEXT_RANGE*")] IntPtr range = default);

	}

	public enum DWriteTextAntialiasMode {
		ClearType,
		Grayscale
	}

	[ComImport, Guid("791e8298-3ef3-4230-9880-c9bdecc42064")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteBitmapRenderTarget1 : IDWriteBitmapRenderTarget {

		[PreserveSig]
		public DWriteTextAntialiasMode GetTextAntialiasMode();

		public void SetTextAntialiasMode(DWriteTextAntialiasMode mode);

	}

}

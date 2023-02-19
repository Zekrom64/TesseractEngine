using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Windows;

namespace Tesseract.DirectX.DirectWrite {

	using DWRITE_COLOR_F = Vector4;
	
	public enum DWriteOpticalAlignment {
		None,
		NoSideBearings
	}

	public enum DWriteGridFitMode {
		Default,
		Disabled,
		Enabled
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteTextMetrics1 {

		public float Left;
		public float Top;
		public float Width;
		public float WdithIncludingTrailingWhitespace;
		public float Height;
		public float LayoutWidth;
		public float LayoutHeight;
		public uint MaxBidiReorderingDepth;
		public uint LineCount;
		public float HeightIncludingTrailingWhitesapce;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWriteColorGlyphRun {

		public DWriteGlyphRun GlyphRun;
		[NativeType("DWRITE_GLYPH_RUN_DESCRIPTION*")]
		private IntPtr GlyphRunDescription;
		public float BaselineOriginX;
		public float BaselineOriginY;
		public DWRITE_COLOR_F RunColor;
		public ushort PaletteIndex;

	}

	[ComImport, Guid("d3e0e934-22a0-427e-aae4-7d9574b59db1")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextRenderer1 : IDWriteTextRenderer {

		public void DrawGlyphRun(IntPtr context, float originX, float originY, DWriteGlyphOrientationAngle angle, DWriteMeasuringMode mode, in DWriteGlyphRun run, in DWriteGlyphRunDescription rundesc, [MarshalAs(UnmanagedType.IUnknown)] IUnknown effect);

		public void DrawUnderline(IntPtr context, float originX, float originY, DWriteGlyphOrientationAngle angle, in DWriteUnderline underline, [MarshalAs(UnmanagedType.IUnknown)] IUnknown effect);

		public void DrawStrikethrough(IntPtr context, float originX, float originY, DWriteGlyphOrientationAngle angle, in DWriteUnderline underline, [MarshalAs(UnmanagedType.IUnknown)] IUnknown effect);

		public void DrawInlineObject(IntPtr context, float originX, float originY, DWriteGlyphOrientationAngle angle, [MarshalAs(UnmanagedType.Interface)] IDWriteInlineObject inlineObject, bool isSideways, bool isRtl, [MarshalAs(UnmanagedType.IUnknown)] IUnknown effect);

	}

	[ComImport, Guid("efa008f9-f7a1-48bf-b05c-f224713cc0ff")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontFallback : IUnknown {

		public void MapCharacters([MarshalAs(UnmanagedType.Interface)] IDWriteTextAnalysisSource source, uint position, uint length, [MarshalAs(UnmanagedType.Interface)] IDWriteFontCollection baseCollection, [MarshalAs(UnmanagedType.LPWStr)] string baseFamilyName, DWriteFontWeight baseWeight, DWriteFontStyle baseStyle, DWriteFontStretch baseStretch, out uint mappedLength, [MarshalAs(UnmanagedType.Interface)] out IDWriteFont mappedFont, out float scale);

	}

	[ComImport, Guid("5f174b49-0d8b-4cfb-8bca-f1cce9d06c67")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextFormat1 : IDWriteTextFormat {

		public void SetVerticalGlyphOrientation(DWriteVerticalGlyphOrientation orientation);

		[PreserveSig]
		public DWriteVerticalGlyphOrientation GetVerticalGlyphOrientation();

		public void SetLastLineWrapping(bool lastLineWrappingEnabled);

		[PreserveSig]
		public bool GetLastLineWrapping();

		public void SetOpticalAlignment(DWriteOpticalAlignment alignment);

		[PreserveSig]
		public bool GetOpticalAlignment();

		public void SetFontFallback([MarshalAs(UnmanagedType.Interface)] IDWriteFontFallback fallback);

		public void GetFontFallback([MarshalAs(UnmanagedType.Interface)] out IDWriteFontFallback fallback);

	}

	[ComImport, Guid("1093c18f-8d5e-43f0-b064-0917311b525e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextLayout2 : IDWriteTextLayout1 {

		public void GetMetrics(out DWriteTextMetrics1 metrics);

		public void SetVerticalGlyphOrientation(DWriteVerticalGlyphOrientation orientation);

		[PreserveSig]
		public DWriteVerticalGlyphOrientation GetVerticalGlyphOrientation();

		public void SetLastLineWrapping(bool lastLineWrappingEnabled);

		[PreserveSig]
		public bool GetLastLineWrapping();

		public void SetOpticalAlignment(DWriteOpticalAlignment alignment);

		[PreserveSig]
		public bool GetOpticalAlignment();

		public void SetFontFallback([MarshalAs(UnmanagedType.Interface)] IDWriteFontFallback fallback);

		public void GetFontFallback([MarshalAs(UnmanagedType.Interface)] out IDWriteFontFallback fallback);

	}

	[ComImport, Guid("553a9ff3-5693-4df7-b52b-74806f7f2eb9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteTextAnalyzer2 : IDWriteTextAnalyzer1 {

		public void GetGlyphOrientationTransform(DWriteGlyphOrientationAngle angle, bool isSideways, float originX, float originY, out DWriteMatrix transform);

		public void GetTypographicFeatures([MarshalAs(UnmanagedType.Interface)] IDWriteFontFace fontFace, DWriteScriptAnalysis analysis, [MarshalAs(UnmanagedType.LPWStr)] string localeName, uint maxTagCount, out uint actualTagCount, [NativeType("DWRITE_FONT_FEATURE_TAG*")] IntPtr tags);

		public void CheckTypographicFeature([MarshalAs(UnmanagedType.Interface)] IDWriteFontFace fontFace, DWriteScriptAnalysis analysis, [MarshalAs(UnmanagedType.LPWStr)] string localeName, DWriteFontFeatureTag feature, uint glyphCount, [NativeType("const UINT16*")] IntPtr indices, [NativeType("UINT8*")] IntPtr featureApplies);

	}

	[ComImport, Guid("fd882d06-8aba-4fb8-b849-8be8b73e14de")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontFallbackBuilder : IUnknown {

		public void AddMapping([NativeType("const DWRITE_UNICODE_RANGE*")] IntPtr ranges, uint rangesCount, [NativeType("WCHAR const**")] IntPtr targetFamilyNames, uint targetFamilyNamesCount, [MarshalAs(UnmanagedType.Interface)] IDWriteFontCollection? collection = null, [MarshalAs(UnmanagedType.LPWStr)] string? localeName = null, [MarshalAs(UnmanagedType.LPWStr)] string? baseFamilyName = null, float scale = 1);

		public void AddMappings([MarshalAs(UnmanagedType.Interface)] IDWriteFontFallback fallback);

		public void CreateFontFallback([MarshalAs(UnmanagedType.Interface)] out IDWriteFontFallback fallback);

	}

	[ComImport, Guid("29748ed6-8c9c-4a6a-be0b-d912e8538944")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFont2 : IDWriteFont1 {

		[PreserveSig]
		public bool IsColorFont();

	}

	[ComImport, Guid("d8b768ff-64bc-4e66-982b-ec8e87f693f7")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFontFace2 : IDWriteFontFace1 {

		[PreserveSig]
		public bool IsColorFontFace();

		[PreserveSig]
		public uint GetColorPaletteCount();

		[PreserveSig]
		public uint GetPaletteEntryCount();

		public void GetPaletteEntries(uint paletteIndex, uint firstEntryIndex, uint entryCount, [NativeType("DWRITE_COLOR_F*")] IntPtr entries);

		public void GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, in DWriteMatrix transform, bool isSideways, DWriteOutlineThreshold threshold, DWriteMeasuringMode measuringMode, [MarshalAs(UnmanagedType.Interface)] IDWriteRenderingParams _params, out DWriteRenderingMode renderingMode, out DWriteGridFitMode gridFitMode);

	}

	[ComImport, Guid("d31fbe17-f157-41a2-8d24-cb779e0560e8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteColorGlyphRunEnumerator : IUnknown {

		public void MoveNext(out bool hasRun);

		public void GetCurrentRun([NativeType("DWRITE_COLOR_GLYPH_RUN const*")] out IntPtr run);

	}

	[ComImport, Guid("f9d711c3-9777-40ae-87e8-3e5aF9bf0948")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteRenderingParams2 : IDWriteRenderingParams1 {

		[PreserveSig]
		public DWriteGridFitMode GetGridFitMode();

	}

	[ComImport, Guid("0439fc60-ca44-4994-8dee-3a9af7b732ec")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDWriteFactory2 : IDWriteFactory1 {

		public void GetSystemFontFallback([MarshalAs(UnmanagedType.Interface)] out IDWriteFontFallback fallback);

		public void CreateFontFallbackBuilder([MarshalAs(UnmanagedType.Interface)] out IDWriteFontFallbackBuilder fallbackBuilder);

		public void TranslateColorGlyphRun(float originX, float originY, in DWriteGlyphRun run, in DWriteGlyphRunDescription rundescr, DWriteMeasuringMode mode, in DWriteMatrix transform, uint paletteIndex, [MarshalAs(UnmanagedType.Interface)] out IDWriteColorGlyphRunEnumerator colorLayers);

		public void CreateCustomRenderingParams(float gamma, float contrast, float grayscaleContrast, float cleartypeLevel, DWritePixelGeometry pixelGeometry, DWriteRenderingMode renderingMode, DWriteGridFitMode gridFitMode, [MarshalAs(UnmanagedType.Interface)] out IDWriteRenderingParams2 _params);

		public void CreateGlyphRunAnalysis(in DWriteGlyphRun run, in DWriteMatrix transform, DWriteRenderingMode renderingMode, DWriteMeasuringMode measuringMode, DWriteGridFitMode gridFitMode, DWriteTextAntialiasMode antialiasMode, float originX, float originY, [MarshalAs(UnmanagedType.Interface)] out IDWriteGlyphRunAnalysis analysis);

	}

}

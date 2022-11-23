using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;
using Tesseract.DirectX.Direct3D;
using Tesseract.DirectX.DirectWrite;
using Tesseract.Windows;

namespace Tesseract.DirectX.Direct2D {

	using D2D1_RECT_F = D2DRectF;
	using D2D1_TAG = UInt64;
	using D2D1_RECT_U = D2DRectU;

	public enum D2D1InterpolationMode {
		NearestNeighbor = 0,
		Linear,
		Cubic,
		MultiSampleLinear,
		Anisotropic,
		HighQualityCubic,
		Fant,
		MipmapLinear
	}

	public enum D2D1DebugLevel : uint {
		None = 0,
		Error,
		Warning,
		Information
	}

	public enum D2D1FactoryType : uint {
		SingleThreaded = 0,
		MultiThreaded
	}

	public enum D2D1FillMode : uint {
		Alternate = 0,
		Winding
	}

	public enum D2D1PathSegment : uint {
		None = 0,
		ForceUnstroked,
		ForceRoundLineJoin
	}

	public enum D2D1FigureBegin : uint {
		Filled = 0,
		Hollow
	}

	public enum D2D1FigureEnd : uint {
		Open = 0,
		Closed
	}

	public enum D2D1CapStyle : uint {
		Flat = 0,
		Square,
		Round,
		Triangle
	}

	public enum D2D1LineJoin : uint {
		Miter = 0,
		Bevel,
		Round,
		MiterOrBevel
	}

	public enum D2D1DashStyle : uint {
		Solid = 0,
		Dash,
		Dot,
		DashDot,
		DashDotDot,
		Custom
	}

	public enum D2D1GeometryRelation : uint {
		Unknown = 0,
		Disjoint,
		IsContained,
		Contains,
		Overlap
	}

	public enum D2D1GeometrySimplificationOption : uint {
		CubicsAndLines = 0,
		Lines
	}

	public enum D2D1CombineMode : uint {
		Union = 0,
		Intersect,
		Xor,
		Exclude
	}

	public enum D2D1SweepDirection : uint {
		CounterClockwise = 0,
		Clockwise
	}

	public enum D2D1ArcSize : uint {
		Small = 0,
		Large
	}

	public enum D2D1AntialiasMode : uint {
		Default = 0,
		ClearType,
		Grayscale,
		Aliased
	}

	public enum D2D1TextAntialiasMode : uint {
		Default = 0,
		ClearType,
		Grayscale,
		Aliased
	}

	public enum D2D1ExtendMode : uint {
		Clamp = 0,
		Wrap,
		Mirror
	}

	public enum D2D1BitmapInterpolationMode : uint {
		NearestNeighbor = D2D1InterpolationMode.NearestNeighbor,
		Linear = D2D1InterpolationMode.Linear
	}

	public enum D2D1Gamma : uint {
		Gamma2_2 = 0,
		Gamma1_0
	}

	[Flags]
	public enum D2D1CompatibleRenderTargetOptions : uint {
		None = 0x0,
		GdiCompatible = 0x1
	}

	public enum D2D1OpacityMaskContent : uint {
		Graphics = 0,
		TextNatural,
		TextGdiCompatible
	}

	[Flags]
	public enum D2D1DrawTextOptions : uint {
		NoSnap = 0x01,
		Clip = 0x02,
		EnableColorFont = 0x04,
		DisableColorBitmapSnapping = 0x08,
		None = 0
	}

	[Flags]
	public enum D2D1LayerOptions : uint {
		None = 0,
		InitializeForClearType = 0x1
	}

	public enum D2D1RenderTargetType : uint {
		Default = 0,
		Software,
		Hardware
	}

	[Flags]
	public enum D2D1RenderTargetUsage : uint {
		None = 0,
		ForceBitmapRemoting = 0x1,
		GdiCompatible = 0x2
	}

	public enum D2D1FeatureLevel : uint {
		Default = 0,
		Level9 = D3DFeatureLevel.Level9_1,
		Level10 = D3DFeatureLevel.Level10_0
	}

	public enum D2D1WindowState : uint {
		None = 0,
		Occluded
	}

	public enum D2D1DCInitializeMode : uint {
		Copy = 0,
		Clear
	}

	[Flags]
	public enum D2D1PresentOptions : uint {
		None = 0,
		RetainContents = 0x1,
		Immediately = 0x2
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1BezierSegment {

		public Vector2 Point1;
		public Vector2 Point2;
		public Vector2 Point3;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1FactoryOptions {

		public D2D1DebugLevel DebugLevel;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1Triangle {

		public Vector2 Point1;
		public Vector2 Point2;
		public Vector2 Point3;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1RoundedRect {

		public D2D1_RECT_F Rect;
		public float RadiusX;
		public float RadiusY;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1Ellipse {

		public Vector2 Point;
		public float RadiusX;
		public float RadiusY;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1QuadraticBezierSegment {

		public Vector2 Point1;
		public Vector2 Point2;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1ArcSegment {

		public Vector2 Point;
		public Vector2 Size;
		public float RotationAngle;
		public D2D1SweepDirection SweepDirection;
		public D2D1ArcSize ArcSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1DrawingStateDescription {

		public D2D1AntialiasMode AntialiasedMode;
		public D2D1TextAntialiasMode TextAntialiasMode;
		public D2D1_TAG Tag1;
		public D2D1_TAG Tag2;
		public Matrix3x2 Transform;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1GradientStop {

		public float Position;
		public Vector4 Color;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1BitmapProperties {

		public D2D1PixelFormat PixelFormat;
		public float DpiX;
		public float DpiY;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1BitmapBrushProperties {

		public D2D1ExtendMode ExtendModeX;
		public D2D1ExtendMode ExtendModeY;
		public D2D1BitmapInterpolationMode InterpolationMode;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1BrushProperties {

		public float Opacity;
		public Matrix3x2 Transform;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1LinearGradientBrushProperties {

		public Vector2 StartPoint;
		public Vector2 EndPoint;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1RadialGradientBrushProperties {

		public Vector2 Center;
		public Vector2 GradientOriginOffset;
		public float RadiusX;
		public float RadiusY;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1LayerParameters {

		public D2D1_RECT_F ContentBounds;
		[NativeType("ID2D1Geometry*")]
		public IntPtr GeometricMask;
		public D2D1AntialiasMode MaskAntialiasMode;
		public Matrix3x2 MaskTransform;
		public float Opacity;
		[NativeType("ID2D1Brush*")]
		public IntPtr OpacityBrush;
		public D2D1LayerOptions LayerOptions;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1RenderTargetProperties {

		public D2D1RenderTargetType Type;
		public D2D1PixelFormat PixelFormat;
		public float DpiX;
		public float DpiY;
		public D2D1RenderTargetUsage Usage;
		public D2D1FeatureLevel MinLevel;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1StrokeStyleProperties {

		public D2D1CapStyle StartCap;
		public D2D1CapStyle EndCap;
		public D2D1CapStyle DashCap;
		public D2D1LineJoin LineJoin;
		public float MiterLimit;
		public D2D1DashStyle DashStyle;
		public float DashOffset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1HWNDRenderTargetProperties {

		[NativeType("HWND")]
		public IntPtr HWnd;
		public Vector2ui PixelSize;
		public D2D1PresentOptions PresentOptions;

	}

	[ComImport, Guid("2cd90691-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1Resource : IUnknown {

		public void GetFactory([MarshalAs(UnmanagedType.Interface)] out ID2D1Factory factory);

	}

	[ComImport, Guid("2cd9069d-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1StrokeStyle : ID2D1Resource {

		[PreserveSig]
		public D2D1CapStyle GetStartCap();

		[PreserveSig]
		public D2D1CapStyle GetEndCap();

		[PreserveSig]
		public D2D1CapStyle GetDashCap();

		[PreserveSig]
		public float GetMiterLimit();

		[PreserveSig]
		public D2D1LineJoin GetLineJoin();

		[PreserveSig]
		public float GetDashOffset();

		[PreserveSig]
		public D2D1DashStyle GetDashStyle();

		[PreserveSig]
		public uint GetDashesCount();

		[PreserveSig]
		public void GetDashes([NativeType("float*")] IntPtr dashes, uint count);

	}

	[ComImport, Guid("2cd9069e-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1SimplifiedGeometrySink : IUnknown {

		[PreserveSig]
		public void SetFillMode(D2D1FillMode mode);

		[PreserveSig]
		public void SetSegmentFlags(D2D1PathSegment vertexFlags);

		[PreserveSig]
		public void BeginFigure(Vector2 startPoint, D2D1FigureBegin figureBegin);

		[PreserveSig]
		public void AddLines([NativeType("const D2D1_POINT_2F*")] IntPtr points, uint count);

		[PreserveSig]
		public void AddBeziers([NativeType("const D2D1_BEZIER_SEGMENT*")] IntPtr beziers, uint count);

		[PreserveSig]
		public void EndFigure(D2D1FigureEnd figureEnd);

		public void Close();
	
	}

	[ComImport, Guid("2cd906c1-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1TessellationSink : IUnknown {

		[PreserveSig]
		public void AddTriangles([NativeType("const D2D1_TRIANGLE*")] IntPtr triangles, uint count);

		public void Close();

	}

	[ComImport, Guid("2cd906a1-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1Geometry : ID2D1Resource {

		public void GetBounds(in Matrix3x2 transform, out D2D1_RECT_F bounds);

		public void GetWidenedBounds(float strokeWidth, [MarshalAs(UnmanagedType.Interface)] ID2D1StrokeStyle strokeStyle, in Matrix3x2 transform, float tolerance, out D2D1_RECT_F bounds);

		public void StrokeContainsPoint(Vector2 point, float strokeWidth, [MarshalAs(UnmanagedType.Interface)] ID2D1StrokeStyle strokeStyle, in Matrix3x2 transform, float tolerance, out bool contains);

		public void FillContainsPoint(Vector2 point, Matrix3x2 transform, float tolerance, out D2D1GeometryRelation relation);

		public void CompareWithGeometry([MarshalAs(UnmanagedType.Interface)] ID2D1Geometry geometry, in Matrix3x2 transform, float tolerance, out D2D1GeometryRelation relation);

		public void Simplify(D2D1GeometrySimplificationOption option, in Matrix3x2 transform, float tolerance, [MarshalAs(UnmanagedType.Interface)] ID2D1SimplifiedGeometrySink sink);

		public void Tessellate(in Matrix3x2 transform, float tolerance, [MarshalAs(UnmanagedType.Interface)] ID2D1TessellationSink sink);

		public void CombineWithGeometry([MarshalAs(UnmanagedType.Interface)] ID2D1Geometry geometry, D2D1CombineMode combineMode, in Matrix3x2 transform, float tolerance, [MarshalAs(UnmanagedType.Interface)] ID2D1SimplifiedGeometrySink sink);

		public void Outline(in Matrix3x2 transform, float tolerance, [MarshalAs(UnmanagedType.Interface)] ID2D1SimplifiedGeometrySink sink);

		public void ComputeArea(in Matrix3x2 transform, float tolerance, out float area);

		public void ComputeLength(in Matrix3x2 transform, float tolerance, out float length);

		public void ComputePointAtLength(float length, in Matrix3x2 transform, float tolerance, out Vector2 point, out Vector2 tangent);

		public void Widen(float strokeWidth, [MarshalAs(UnmanagedType.Interface)] ID2D1StrokeStyle strokeStyle, in Matrix3x2 transform, float tolerance, [MarshalAs(UnmanagedType.Interface)] ID2D1SimplifiedGeometrySink sink);

	}

	[ComImport, Guid("2cd906a2-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1RectangeGeometry : ID2D1Geometry {

		[PreserveSig]
		public void GetRect(out D2D1_RECT_F rect);

	}

	[ComImport, Guid("2cd906a3-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1RoundedRectangleGeometry : ID2D1Geometry {

		[PreserveSig]
		public void GetRoundedRect(out D2D1RoundedRect rect);

	}

	[ComImport, Guid("2cd906a4-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1EllipseGeometry : ID2D1Geometry {

		[PreserveSig]
		public void GetEllipse(out D2D1Ellipse ellipse);

	}

	[ComImport, Guid("2cd906a6-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1GeometryGroup : ID2D1Geometry {

		[PreserveSig]
		public D2D1FillMode GetFillMode();

		[PreserveSig]
		public uint GetSourceGeometryCount();

		[PreserveSig]
		public void GetSourceGeometries([NativeType("ID2D1Geometry**")] IntPtr geometry, uint geometryCount);

	}

	[ComImport, Guid("2cd906bb-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1TransformedGeometry : ID2D1Geometry {

		[PreserveSig]
		public void GetSourceGeometry([MarshalAs(UnmanagedType.Interface)] out ID2D1Geometry geometry);

		[PreserveSig]
		public void GetTransform(out Matrix3x2 transform);

	}

	[ComImport, Guid("2cd9069f-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1GeometrySink : ID2D1SimplifiedGeometrySink {

		[PreserveSig]
		public void AddLine(Vector2 point);

		[PreserveSig]
		public void AddBezier(in D2D1BezierSegment bezier);

		[PreserveSig]
		public void AddQuadraticBezier(in D2D1QuadraticBezierSegment bezier);

		[PreserveSig]
		public void AddQuadraticBeziers([NativeType("const D2D1_QUADRATIC_BEZIER_SEGMENT*")] IntPtr beziers, uint bezierCount);

		[PreserveSig]
		public void AddArc(in D2D1ArcSegment arc);

	}

	[ComImport, Guid("2cd906a5-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1PathGeometry : ID2D1Geometry {

		public void Open([MarshalAs(UnmanagedType.Interface)] out ID2D1GeometrySink sink);

		public void Stream([MarshalAs(UnmanagedType.Interface)] ID2D1GeometrySink sink);

		public void GetSegmentCount(out uint count);

		public void GetFigureCount(out uint count);

	}

	[ComImport, Guid("28506e39-ebf6-46a1-bb47-fd85565ab957")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1DrawingStateBlock : ID2D1Resource {

		public void GetDescription(out D2D1DrawingStateDescription desc);

		public void SetDescription(in D2D1DrawingStateDescription desc);

		public void SetTextRenderingParams([MarshalAs(UnmanagedType.Interface)] IDWriteRenderingParams textRenderingParams);

		public void GetTextRenderingParams([MarshalAs(UnmanagedType.Interface)] out IDWriteRenderingParams textRenderingParams);

	}

	[ComImport, Guid("65019f75-8da2-497c-b32c-dfa34e48ede6")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1Image : ID2D1Resource {

	}

	[ComImport, Guid("a2296057-ea42-4099-983b-539fb6505426")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1Bitmap : ID2D1Image {

		[PreserveSig]
		public Vector2 GetSize();

		[PreserveSig]
		public Vector2ui GetPixelSize();

		[PreserveSig]
		public D2D1PixelFormat GetPixelFormat();

		[PreserveSig]
		public void GetDpi(out float dpiX, out float dpiY);

		public void CopyFromBitmap(in Vector2ui dstPoint, [MarshalAs(UnmanagedType.Interface)] ID2D1Bitmap bitmap, in D2D1_RECT_U srcRect);

		public void CopyFromRenderTarget(in Vector2ui dstPoint, [MarshalAs(UnmanagedType.Interface)] ID2D1RenderTarget renderTarget, in D2D1_RECT_U srcRect);

		public void CopyFromMemory(in D2D1_RECT_U dstRect, IntPtr data, uint pitch);

	}

	[ComImport, Guid("2cd906a8-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1Brush : ID2D1Resource {

		[PreserveSig]
		public void SetOpacity(float opacity);

		[PreserveSig]
		public void SetTransform(in Matrix3x2 transform);

		[PreserveSig]
		public float GetOpacity();

		[PreserveSig]
		public void GetTransform(out Matrix3x2 transform);

	}

	[ComImport, Guid("2cd906aa-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1BitmapBrush : ID2D1Brush {

		[PreserveSig]
		public void SetExtendModeX(D2D1ExtendMode mode);

		[PreserveSig]
		public void SetExtendModeY(D2D1ExtendMode mode);

		[PreserveSig]
		public void SetInterpolationMode(D2D1BitmapInterpolationMode mode);

		[PreserveSig]
		public void SetBitmap([MarshalAs(UnmanagedType.Interface)] ID2D1Bitmap bitmap);

		[PreserveSig]
		public D2D1ExtendMode GetExtendModeX();

		[PreserveSig]
		public D2D1ExtendMode GetExtendModeY();

		[PreserveSig]
		public D2D1BitmapInterpolationMode GetInterpolationMode();

		[PreserveSig]
		public void GetBitmap([MarshalAs(UnmanagedType.Interface)] out ID2D1Bitmap bitmap);

	}

	[ComImport, Guid("2cd906a9-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1SolidColorBrush : ID2D1Brush {

		[PreserveSig]
		public void SetColor(in Vector4 color);

		[PreserveSig]
		public void GetColor(out Vector4 color);

	}

	[ComImport, Guid("2cd906a7-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1GradientStopCollection : ID2D1Resource {

		[PreserveSig]
		public uint GetGradientStopCount();

		[PreserveSig]
		public void GetGradientStops([NativeType("D2D1_GRADIENT_STOP")] IntPtr stops, uint stopCount);

		[PreserveSig]
		public D2D1Gamma GetColorInterpolationGamma();

		[PreserveSig]
		public D2D1ExtendMode GetExtendMode();

	}

	[ComImport, Guid("2cd906ab-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1LinearGradientBrush : ID2D1Brush {

		[PreserveSig]
		public void SetStartPoint(Vector2 startPoint);

		[PreserveSig]
		public void SetEndPoint(Vector2 endPoint);

		[PreserveSig]
		public Vector2 GetStartPoint();

		[PreserveSig]
		public Vector2 GetEndPoint();

		[PreserveSig]
		public void GetGradientStopCollection([MarshalAs(UnmanagedType.Interface)] out ID2D1GradientStopCollection gradient);

	}

	[ComImport, Guid("2cd906ac-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1RadialGradientBrush : ID2D1Brush {

		[PreserveSig]
		public void SetCenter(Vector2 center);

		[PreserveSig]
		public void SetGradientOrigin(Vector2 offset);

		[PreserveSig]
		public void SetRadiusX(float radius);

		[PreserveSig]
		public void SetRadiusY(float radius);

		[PreserveSig]
		public Vector2 GetCenter();

		[PreserveSig]
		public Vector2 GetGradientOriginOffset();

		[PreserveSig]
		public float GetRadiusX();

		[PreserveSig]
		public float GetRadiusY();

		[PreserveSig]
		public void GetGradientStopCollection([MarshalAs(UnmanagedType.Interface)] out ID2D1GradientStopCollection gradient);

	}

	[ComImport, Guid("2cd9069b-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1Layer : ID2D1Resource {

		[PreserveSig]
		public Vector2 GetSize();

	}

	[ComImport, Guid("2cd906c2-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1Mesh : ID2D1Resource {

		public void Open([MarshalAs(UnmanagedType.Interface)] out ID2D1TessellationSink sink);

	}

	[ComImport, Guid("2cd90694-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1RenderTarget : ID2D1Resource {

		public void CreateBitmap(Vector2ui size, IntPtr srcData, uint pitch, in D2D1BitmapProperties desc, [MarshalAs(UnmanagedType.Interface)] out ID2D1Bitmap bitmap);

		public void CreateBitmapFromWicBitmap([NativeType("IWICBitmapSource*")] IntPtr bitmapSource, in D2D1BitmapProperties desc, [MarshalAs(UnmanagedType.Interface)] out ID2D1Bitmap bitmap);

		public void CreateSharedBitmap(in Guid iid, IntPtr data, in D2D1BitmapProperties desc, [MarshalAs(UnmanagedType.Interface)] out ID2D1Bitmap bitmap);

		public void CreateBitmapBrush([MarshalAs(UnmanagedType.Interface)] ID2D1Bitmap bitmap, in D2D1BitmapBrushProperties bitmapBrushDesc, in D2D1BrushProperties brushDesc, [MarshalAs(UnmanagedType.Interface)] out ID2D1BitmapBrush brush);

		public void CreateSolidColorBrush(in Vector4 color, in D2D1BrushProperties desc, [MarshalAs(UnmanagedType.Interface)] out ID2D1SolidColorBrush brush);

		public void CreateGradientStopCollection([NativeType("const D2D1_GRADIENT_STOP*")] IntPtr stops, uint stopCount, D2D1Gamma gamma, D2D1ExtendMode extendMode, [MarshalAs(UnmanagedType.Interface)] out ID2D1GradientStopCollection gradient);

		public void CreateLinearGradientBrush(in D2D1LinearGradientBrushProperties gradientBrushDesc, in D2D1BrushProperties brushDesc, [MarshalAs(UnmanagedType.Interface)] ID2D1GradientStopCollection gradient, [MarshalAs(UnmanagedType.Interface)] out ID2D1LinearGradientBrush brush);

		public void CreateRadialGradientBrush(in D2D1RadialGradientBrushProperties gradientBrushDesc, in D2D1BrushProperties brushDesc, [MarshalAs(UnmanagedType.Interface)] ID2D1GradientStopCollection gradient, [MarshalAs(UnmanagedType.Interface)] out ID2D1RadialGradientBrush brush);

		public void CreateCompatibleRendertarget(in Vector2 size, in Vector2ui pixelSize, in D2D1PixelFormat format, D2D1CompatibleRenderTargetOptions options, [MarshalAs(UnmanagedType.Interface)] out ID2D1BitmapRenderTarget renderTarget);

		public void CreateLayer(in Vector2 size, [MarshalAs(UnmanagedType.Interface)] out ID2D1Layer layer);

		public void CreateMesh([MarshalAs(UnmanagedType.Interface)] out ID2D1Mesh mesh);

		[PreserveSig]
		public void DrawLine(Vector2 p0, Vector2 p1, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush, float strokeWidth, [MarshalAs(UnmanagedType.Interface)] ID2D1StrokeStyle strokeStyle);

		[PreserveSig]
		public void DrawRectangle(in D2D1_RECT_F rect, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush, float strokeWdith, [MarshalAs(UnmanagedType.Interface)] ID2D1StrokeStyle strokeStyle);

		[PreserveSig]
		public void FillRectangle(in D2D1_RECT_F rect, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush);

		[PreserveSig]
		public void DrawRoundedRectangle(in D2D1RoundedRect rect, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush, float strokeWidth, [MarshalAs(UnmanagedType.Interface)] ID2D1StrokeStyle strokeStyle);

		[PreserveSig]
		public void FillRoundedRectangle(in D2D1RoundedRect rect, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush);

		[PreserveSig]
		public void DrawEllipse(in D2D1Ellipse ellipse, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush, float strokeWidth, [MarshalAs(UnmanagedType.Interface)] ID2D1StrokeStyle strokeStyle);

		[PreserveSig]
		public void FillEllipse(in D2D1Ellipse ellipse, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush);

		[PreserveSig]
		public void DrawGeometry([MarshalAs(UnmanagedType.Interface)] ID2D1Geometry geometry, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush, float strokeWidth, [MarshalAs(UnmanagedType.Interface)] ID2D1StrokeStyle strokeStyle);

		[PreserveSig]
		public void FillGeometry([MarshalAs(UnmanagedType.Interface)] ID2D1Geometry geometry, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush opacityBrush);

		[PreserveSig]
		public void FillMesh([MarshalAs(UnmanagedType.Interface)] ID2D1Mesh mesh, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush);

		[PreserveSig]
		public void FillOpacityMask([MarshalAs(UnmanagedType.Interface)] ID2D1Bitmap mask, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush, D2D1OpacityMaskContent content, in D2D1_RECT_F dstRect, in D2D1_RECT_F srcRect);

		[PreserveSig]
		public void DrawBitmap([MarshalAs(UnmanagedType.Interface)] ID2D1Bitmap bitmap, in D2D1_RECT_F dstRect, float opacity, D2D1BitmapInterpolationMode interpolationMode, in D2D1_RECT_F srcRect);

		[PreserveSig]
		public void DrawText([MarshalAs(UnmanagedType.LPWStr)] string text, uint stringLen, [MarshalAs(UnmanagedType.Interface)] IDWriteTextFormat textFormat, in D2D1_RECT_F layoutRect, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush, D2D1DrawTextOptions options, DWriteMeasuringMode measuringMode);

		[PreserveSig]
		public void DrawTextLayout(Vector2 origin, [MarshalAs(UnmanagedType.Interface)] IDWriteTextLayout layout, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush, D2D1DrawTextOptions options);

		[PreserveSig]
		public void DrawGlyphRun(Vector2 baselineOrigin, in DWriteGlyphRun glyphRun, [MarshalAs(UnmanagedType.Interface)] ID2D1Brush brush, DWriteMeasuringMode measuringMode);

		[PreserveSig]
		public void SetTransform(in Matrix3x2 transform);

		[PreserveSig]
		public void GetTransform(out Matrix3x2 transform);

		[PreserveSig]
		public void SetAntialiasMode(D2D1AntialiasMode antialiaseMode);

		[PreserveSig]
		public D2D1AntialiasMode GetAntialiasMode();

		[PreserveSig]
		public void SetTextAntialiasMode(D2D1TextAntialiasMode antialiasMode);

		[PreserveSig]
		public D2D1TextAntialiasMode GetTextAntialiasMode();

		[PreserveSig]
		public void SetTextRenderingParams([MarshalAs(UnmanagedType.Interface)] IDWriteRenderingParams textRenderingParams);

		[PreserveSig]
		public void GetTextRenderingParams([MarshalAs(UnmanagedType.Interface)] out IDWriteRenderingParams textRenderingParams);

		[PreserveSig]
		public void SetTags(D2D1_TAG tag1, D2D1_TAG tag2);

		[PreserveSig]
		public void GetTags(out D2D1_TAG tag1, out D2D1_TAG tag2);

		[PreserveSig]
		public void PushLayer(in D2D1LayerParameters layerParameters, [MarshalAs(UnmanagedType.Interface)] ID2D1Layer layer);

		[PreserveSig]
		public void PopLayer();

		public void Flush(out D2D1_TAG tag1, out D2D1_TAG tag2);

		[PreserveSig]
		public void SaveDrawingState([MarshalAs(UnmanagedType.Interface)] ID2D1DrawingStateBlock stateBlock);

		[PreserveSig]
		public void RestoreDrawingState([MarshalAs(UnmanagedType.Interface)] ID2D1DrawingStateBlock stateBlock);

		[PreserveSig]
		public void PushAxisAlignedClip(in D2D1_RECT_F clipRect, D2D1AntialiasMode antialiasMode);

		[PreserveSig]
		public void PopAxisAlignedClip();

		[PreserveSig]
		public void Clear(in Vector4 color);

		[PreserveSig]
		public void BeginDraw();

		public void EndDraw(out D2D1_TAG tag1, out D2D1_TAG tag2);

		[PreserveSig]
		public D2D1PixelFormat GetPixelFormat();

		[PreserveSig]
		public void SetDpi(float dpiX, float dpiY);

		[PreserveSig]
		public void GetDpi(out float dpiX, out float dpiY);

		[PreserveSig]
		public Vector2 GetSize();

		[PreserveSig]
		public Vector2ui GetPixelSize();

		[PreserveSig]
		public uint GetMaximumBitmapSize();

		[PreserveSig]
		public bool IsSupported(in D2D1RenderTargetProperties desc);

	}

	[ComImport, Guid("2cd90695-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1BitmapRenderTarget : ID2D1RenderTarget {

		public void GetBitmap([MarshalAs(UnmanagedType.Interface)] out ID2D1Bitmap bitmap);

	}

	[ComImport, Guid("2cd90698-12e2-11dc-9fed-001143a055f9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1HwndRenderTarget : ID2D1RenderTarget {

		[PreserveSig]
		public D2D1WindowState CheckWindowState();

		public void Resize(in Vector2ui size);

		[PreserveSig]
		[return: NativeType("HWND")]
		public IntPtr GetHwnd();

	}

	[ComImport, Guid("1c51bc64-de61-46fd-9899-63a5d8f03950")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1CRenderTarget : ID2D1RenderTarget {

		public void BindDC([NativeType("HDC")] IntPtr dc, in RECT rect);

	}

	[ComImport, Guid("e0db51c3-6f77-4bae-b3d5-e47509b35838")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1GdiInteropRenderTarget : IUnknown {

		public void GetDC(D2D1DCInitializeMode mode, [NativeType("HDC")] out IntPtr dc);

		public void ReleaseDC(in RECT update);

	}

	[ComImport, Guid("06152247-6f50-465a-9245-118bfd3b6007")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID2D1Factory {

	}

	public static partial class D2D1 {

		[DllImport("d2d1.dll", PreserveSig = false)]
		private static extern void D2D1CreateFactory(D2D1FactoryType factoryType, in Guid iid, in D2D1FactoryOptions factoryOptions, out IntPtr factory);

		public static T CreateFactory<T>(D2D1FactoryType factoryType, D2D1FactoryOptions factoryOptions) where T : class =>
			COMHelpers.GetObjectFromCOMGetter<T>((in Guid riid, out IntPtr ptr) => D2D1CreateFactory(factoryType, riid, factoryOptions, out ptr))!;

	}

}

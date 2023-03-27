using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {

	public record GLPixelFormat {

		public required GLInternalFormat InternalFormat { get; init; }

		public required GLFormat Format { get; init; }

		public required GLTextureType Type { get; init; }

		public GLBufferMask Buffers { get; init; } = GLBufferMask.Color;

		private int count = 0;
		public int Count {
			get {
				if (count == 0) {
					count = Format switch {
						GLFormat.R => 1,
						GLFormat.RG => 2,
						GLFormat.RGB or GLFormat.BGR => 3,
						GLFormat.RGBA or GLFormat.BGRA => 4,
						_ => 0
					};
				}
				return count;
			}
			init => count = value;
		}

		public bool Normalized { get; init; } = true;

		public int SizeOf => GLEnums.SizeOf(Type) * Count;

	}

	public static class GLEnums {

		public static int SizeOf(GLTextureType type) => type switch {
			GLTextureType.Byte => 1,
			GLTextureType.UnsignedByte => 1,
			GLTextureType.Short => 2,
			GLTextureType.UnsignedShort => 2,
			GLTextureType.Int => 4,
			GLTextureType.UnsignedInt => 4,
			GLTextureType.HalfFloat => 2,
			GLTextureType.Float => 4,
			GLTextureType.UnsignedByte_3_3_2 => 1,
			GLTextureType.UnsignedByte_2_3_3_Rev => 1,
			GLTextureType.UnsignedShort_5_6_5 => 2,
			GLTextureType.UnsignedShort_5_6_5_Rev => 2,
			GLTextureType.UnsignedShort_4_4_4_4 => 2,
			GLTextureType.UnsignedShort_4_4_4_4_Rev => 2,
			GLTextureType.UnsignedShort_5_5_5_1 => 2,
			GLTextureType.UnsignedShort_1_5_5_5_Rev => 2,
			GLTextureType.UnsignedInt_8_8_8_8 => 4,
			GLTextureType.UnsignedInt_8_8_8_8_Rev => 4,
			GLTextureType.UnsignedInt_10_10_10_2 => 4,
			GLTextureType.UnsignedInt_2_10_10_10_Rev => 4,
			GLTextureType.Double => 8,
			_ => 0
		};

		public static int SizeOf(GLType type) => SizeOf((GLTextureType)type);

		public static int SizeOf(GLIndexType type) => SizeOf((GLTextureType)type);

		private static readonly Dictionary<PixelFormat, GLPixelFormat> stdToGLFormat = new() {
			// R8x
			{ PixelFormat.R8UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R8,
				Format = GLFormat.R,
				Type = GLTextureType.UnsignedByte
			} },
			{ PixelFormat.R8SNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R8SNorm,
				Format = GLFormat.R,
				Type = GLTextureType.Byte
			} },
			{ PixelFormat.R8UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R8UI,
				Format = GLFormat.R,
				Type = GLTextureType.UnsignedByte,
				Normalized = false
			} },
			{ PixelFormat.R8SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R8I,
				Format = GLFormat.R,
				Type = GLTextureType.Byte,
				Normalized = false
			} },
			// RG8x
			{ PixelFormat.R8G8UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG8,
				Format = GLFormat.RG,
				Type = GLTextureType.UnsignedByte
			} },
			{ PixelFormat.R8G8SNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG8SNorm,
				Format = GLFormat.RG,
				Type = GLTextureType.Byte
			} },
			{ PixelFormat.R8G8UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG8UI,
				Format = GLFormat.RG,
				Type = GLTextureType.UnsignedByte,
				Normalized = false
			} },
			{ PixelFormat.R8G8SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG8I,
				Format = GLFormat.RG,
				Type = GLTextureType.Byte,
				Normalized = false
			} },
			// RGB8x
			{ PixelFormat.R8G8B8UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB8,
				Format = GLFormat.RGB,
				Type = GLTextureType.UnsignedByte
			} },
			{ PixelFormat.R8G8B8SNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB8SNorm,
				Format = GLFormat.RGB,
				Type = GLTextureType.Byte
			} },
			{ PixelFormat.R8G8B8UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB8UI,
				Format = GLFormat.RGB,
				Type = GLTextureType.UnsignedByte,
				Normalized = false
			} },
			{ PixelFormat.R8G8B8SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB8I,
				Format = GLFormat.RGB,
				Type = GLTextureType.Byte,
				Normalized = false
			} },
			{ PixelFormat.R8G8B8SRGB, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.SRGB8,
				Format = GLFormat.RGB,
				Type = GLTextureType.UnsignedByte
			} },
			// BGR8x
			{ PixelFormat.B8G8R8UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB8,
				Format = GLFormat.BGR,
				Type = GLTextureType.UnsignedByte
			} },
			{ PixelFormat.B8G8R8SNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB8SNorm,
				Format = GLFormat.BGR,
				Type = GLTextureType.Byte
			} },
			{ PixelFormat.B8G8R8UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB8UI,
				Format = GLFormat.BGR,
				Type = GLTextureType.UnsignedByte,
				Normalized = false
			} },
			{ PixelFormat.B8G8R8SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB8I,
				Format = GLFormat.BGR,
				Type = GLTextureType.Byte,
				Normalized = false
			} },
			{ PixelFormat.B8G8R8SRGB, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.SRGB8,
				Format = GLFormat.BGR,
				Type = GLTextureType.UnsignedByte
			} },
			// RGBA8x
			{ PixelFormat.R8G8B8A8UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedByte
			} },
			{ PixelFormat.R8G8B8A8SNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8SNorm,
				Format = GLFormat.RGBA,
				Type = GLTextureType.Byte
			} },
			{ PixelFormat.R8G8B8A8UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8UI,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedByte,
				Normalized = false
			} },
			{ PixelFormat.R8G8B8A8SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8I,
				Format = GLFormat.RGBA,
				Type = GLTextureType.Byte,
				Normalized = false
			} },
			{ PixelFormat.R8G8B8A8SRGB, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.SRGB8A8,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedByte
			} },
			// BGRA8x
			{ PixelFormat.B8G8R8A8UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8,
				Format = GLFormat.BGRA,
				Type = GLTextureType.UnsignedByte
			} },
			{ PixelFormat.B8G8R8A8SNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8SNorm,
				Format = GLFormat.BGRA,
				Type = GLTextureType.Byte
			} },
			{ PixelFormat.B8G8R8A8UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8UI,
				Format = GLFormat.BGRA,
				Type = GLTextureType.UnsignedByte,
				Normalized = false
			} },
			{ PixelFormat.B8G8R8A8SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8I,
				Format = GLFormat.BGRA,
				Type = GLTextureType.Byte,
				Normalized = false
			} },
			{ PixelFormat.B8G8R8A8SRGB, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.SRGB8A8,
				Format = GLFormat.BGRA,
				Type = GLTextureType.UnsignedByte
			} },
			// ABGR8x
			{ PixelFormat.A8B8G8R8UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8,
				Format = GLFormat.RGBA,
				Type = BitConverter.IsLittleEndian ? GLTextureType.UnsignedInt_8_8_8_8 : GLTextureType.UnsignedInt_8_8_8_8_Rev
			} },
			// ARGB8x
			{ PixelFormat.A8R8G8B8UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8,
				Format = GLFormat.BGRA,
				Type = BitConverter.IsLittleEndian ? GLTextureType.UnsignedInt_8_8_8_8 : GLTextureType.UnsignedInt_8_8_8_8_Rev
			} },
			// R16x
			{ PixelFormat.R16UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R16,
				Format = GLFormat.R,
				Type = GLTextureType.UnsignedShort
			} },
			{ PixelFormat.R16SNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R16SNorm,
				Format = GLFormat.R,
				Type = GLTextureType.Short
			} },
			{ PixelFormat.R16UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R16UI,
				Format = GLFormat.R,
				Type = GLTextureType.UnsignedShort,
				Normalized = false
			} },
			{ PixelFormat.R16SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R16I,
				Format = GLFormat.R,
				Type = GLTextureType.Short,
				Normalized = false
			} },
			{ PixelFormat.R16SFloat, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R16F,
				Format = GLFormat.R,
				Type = GLTextureType.HalfFloat,
				Normalized = false
			} },
			// RG16x
			{ PixelFormat.R16G16UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG16,
				Format = GLFormat.RG,
				Type = GLTextureType.UnsignedShort
			} },
			{ PixelFormat.R16G16SNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG16SNorm,
				Format = GLFormat.RG,
				Type = GLTextureType.Short
			} },
			{ PixelFormat.R16G16UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG16UI,
				Format = GLFormat.RG,
				Type = GLTextureType.UnsignedShort,
				Normalized = false
			} },
			{ PixelFormat.R16G16SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG16I,
				Format = GLFormat.RG,
				Type = GLTextureType.Short,
				Normalized = false
			} },
			{ PixelFormat.R16G16SFloat, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG16F,
				Format = GLFormat.RG,
				Type = GLTextureType.HalfFloat,
				Normalized = false
			} },
			// RGB16x
			{ PixelFormat.R16G16B16UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB16,
				Format = GLFormat.RGB,
				Type = GLTextureType.UnsignedShort
			} },
			{ PixelFormat.R16G16B16SNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB16SNorm,
				Format = GLFormat.RGB,
				Type = GLTextureType.Short
			} },
			{ PixelFormat.R16G16B16UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB16UI,
				Format = GLFormat.RGB,
				Type = GLTextureType.UnsignedShort,
				Normalized = false
			} },
			{ PixelFormat.R16G16B16SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB16I,
				Format = GLFormat.RGB,
				Type = GLTextureType.Short,
				Normalized = false
			} },
			{ PixelFormat.R16G16B16SFloat, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB16F,
				Format = GLFormat.RGB,
				Type = GLTextureType.HalfFloat,
				Normalized = false
			} },
			// RGBA16x
			{ PixelFormat.R16G16B16A16UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA16,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedShort
			} },
			{ PixelFormat.R16G16B16A16SNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA16SNorm,
				Format = GLFormat.RGBA,
				Type = GLTextureType.Short
			} },
			{ PixelFormat.R16G16B16A16UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA16UI,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedShort,
				Normalized = false
			} },
			{ PixelFormat.R16G16B16A16SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA16I,
				Format = GLFormat.RGBA,
				Type = GLTextureType.Short,
				Normalized = false
			} },
			{ PixelFormat.R16G16B16A16SFloat, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA16F,
				Format = GLFormat.RGBA,
				Type = GLTextureType.HalfFloat,
				Normalized = false
			} },
			// R32x
			{ PixelFormat.R32UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R32UI,
				Format = GLFormat.R,
				Type = GLTextureType.UnsignedInt,
				Normalized = false
			} },
			{ PixelFormat.R32SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R32I,
				Format = GLFormat.R,
				Type = GLTextureType.Int,
				Normalized = false
			} },
			{ PixelFormat.R32SFloat, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R32F,
				Format = GLFormat.R,
				Type = GLTextureType.Float,
				Normalized = false
			} },
			// RG32x
			{ PixelFormat.R32G32UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG32UI,
				Format = GLFormat.RG,
				Type = GLTextureType.UnsignedInt,
				Normalized = false
			} },
			{ PixelFormat.R32G32SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG32I,
				Format = GLFormat.RG,
				Type = GLTextureType.Int,
				Normalized = false
			} },
			{ PixelFormat.R32G32SFloat, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RG32F,
				Format = GLFormat.RG,
				Type = GLTextureType.Float,
				Normalized = false
			} },
			// RGB32x
			{ PixelFormat.R32G32B32UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB32UI,
				Format = GLFormat.RGB,
				Type = GLTextureType.UnsignedInt,
				Normalized = false
			} },
			{ PixelFormat.R32G32B32SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB32I,
				Format = GLFormat.RGB,
				Type = GLTextureType.Int,
				Normalized = false
			} },
			{ PixelFormat.R32G32B32SFloat, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB32F,
				Format = GLFormat.RGB,
				Type = GLTextureType.Float,
				Normalized = false
			} },
			// RGBA32x
			{ PixelFormat.R32G32B32A32UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA32UI,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedInt,
				Normalized = false
			} },
			{ PixelFormat.R32G32B32A32SInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA32I,
				Format = GLFormat.RGBA,
				Type = GLTextureType.Int,
				Normalized = false
			} },
			{ PixelFormat.R32G32B32A32SFloat, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA32F,
				Format = GLFormat.RGBA,
				Type = GLTextureType.Float,
				Normalized = false
			} },
			// RGBA4
			{ PixelFormat.R4G4B4A4UNormPack16, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA4,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedShort_4_4_4_4
			} },
			// BGRA4
			{ PixelFormat.B4G4R4A4UNormPack16, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA4,
				Format = GLFormat.BGRA,
				Type = GLTextureType.UnsignedShort_4_4_4_4
			} },
			// ARGB4
			{ PixelFormat.A4R4G4B4UNormPack16, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA4,
				Format = GLFormat.BGRA,
				Type = GLTextureType.UnsignedShort_4_4_4_4_Rev
			} },
			// R5G6A5
			{ PixelFormat.R5G6B5UNormPack16, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB5,
				Format = GLFormat.RGB,
				Type = GLTextureType.UnsignedShort_5_6_5
			} },
			// B5G6R5
			{ PixelFormat.B5G6R5UNormPack16, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB5,
				Format = GLFormat.BGR,
				Type = GLTextureType.UnsignedShort_5_6_5
			} },
			// RGB5A1
			{ PixelFormat.R5G5B5A1UNormPack16, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB5A1,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedShort_5_5_5_1
			} },
			// BGR5A1
			{ PixelFormat.B5G5R5A1UNormPack16, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB5A1,
				Format = GLFormat.BGRA,
				Type = GLTextureType.UnsignedShort_5_5_5_1
			} },
			// A1RGB5
			{ PixelFormat.A1R5G5B5UNormPack16, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB5A1,
				Format = GLFormat.BGRA,
				Type = GLTextureType.UnsignedShort_1_5_5_5_Rev
			} },
			// ABGR8-Pack32
			{ PixelFormat.A8B8G8R8UNormPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedInt_8_8_8_8_Rev
			} },
			{ PixelFormat.A8B8G8R8SNormPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8SNorm,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedInt_8_8_8_8_Rev
			} },
			{ PixelFormat.A8B8G8R8UIntPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8UI,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedInt_8_8_8_8_Rev,
				Normalized = false
			} },
			{ PixelFormat.A8B8G8R8SIntPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA8I,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedInt_8_8_8_8_Rev,
				Normalized = false
			} },
			{ PixelFormat.A8B8G8R8SRGBPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.SRGB8A8,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedInt_8_8_8_8_Rev
			} },
			// A2RGB10
			{ PixelFormat.A2R10G10B10UNormPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB10A2,
				Format = GLFormat.BGRA,
				Type = GLTextureType.UnsignedInt_2_10_10_10_Rev
			} },
			{ PixelFormat.A2R10G10B10UIntPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB10A2UI,
				Format = GLFormat.BGRA,
				Type = GLTextureType.UnsignedInt_2_10_10_10_Rev
			} },
			// A2BGR10
			{ PixelFormat.A2B10G10R10UNormPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB10A2,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedInt_2_10_10_10_Rev
			} },
			{ PixelFormat.A2B10G10R10UIntPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB10A2UI,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedInt_2_10_10_10_Rev
			} },
			// B10GR11
			{ PixelFormat.B10G11R11UFloatPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.R11FG11FB10F,
				Format = GLFormat.RGB,
				Type = GLTextureType.UnsignedInt_10F_11F_11F_Rev
			} },
			// D16
			{ PixelFormat.D16UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.DepthComponent16,
				Format = GLFormat.DepthComponent,
				Type = GLTextureType.HalfFloat,
				Buffers = GLBufferMask.Depth
			} },
			// D24
			{ PixelFormat.X8D24UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.DepthComponent24,
				Format = GLFormat.DepthComponent,
				Type = GLTextureType.UnsignedInt_24_8,
				Buffers = GLBufferMask.Depth
			} },
			// D32
			{ PixelFormat.D32SFloat, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.DepthComponent32F,
				Format = GLFormat.DepthComponent,
				Type = GLTextureType.Float,
				Buffers = GLBufferMask.Depth
			} },
			// S8
			{ PixelFormat.S8UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.StencilIndex8,
				Format = GLFormat.StencilIndex,
				Type = GLTextureType.UnsignedByte,
				Buffers = GLBufferMask.Stencil
			} },
			// D24S8
			{ PixelFormat.D24UNormS8UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.Depth24Stencil8,
				Format = GLFormat.DepthStencil,
				Type = GLTextureType.UnsignedInt_24_8,
				Buffers = GLBufferMask.Depth | GLBufferMask.Stencil
			} },
			// D32S8
			{ PixelFormat.D32SFloatS8UInt, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.Depth32FStencil8,
				Format = GLFormat.DepthStencil,
				Type = GLTextureType.Float32UnsignedInt_24_8_Rev,
				Buffers = GLBufferMask.Depth | GLBufferMask.Stencil
			} },
			// E5B9G9R9
			{ PixelFormat.E5B9G9R9UFloatPack32, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGB10,
				Format = GLFormat.RGB,
				Type = GLTextureType.UnsignedInt_5_9_9_9_Rev
			} }
		};

		public static GLPixelFormat? StdToGLFormat(PixelFormat format) {
			if (stdToGLFormat.TryGetValue(format, out GLPixelFormat? glpxformat)) return glpxformat;
			else return null;
		}


		public static GLTextureWrap Convert(SamplerAddressMode addressMode) => addressMode switch {
			SamplerAddressMode.Repeat => GLTextureWrap.Repeat,
			SamplerAddressMode.MirroredRepeat => GLTextureWrap.MirroredRepeat,
			SamplerAddressMode.ClampToBorder => GLTextureWrap.ClampToBorder,
			SamplerAddressMode.ClampToEdge => GLTextureWrap.ClampToEdge,
			SamplerAddressMode.MirrorClampToEdge => GLTextureWrap.MirrorClampToEdge,
			_ => default
		};

		public static GLTextureSwizzle Convert(ComponentSwizzle swizzle, GLTextureSwizzle def) => swizzle switch {
			ComponentSwizzle.Red => GLTextureSwizzle.Red,
			ComponentSwizzle.Green => GLTextureSwizzle.Green,
			ComponentSwizzle.Blue => GLTextureSwizzle.Blue,
			ComponentSwizzle.Alpha => GLTextureSwizzle.Alpha,
			ComponentSwizzle.One => GLTextureSwizzle.One,
			ComponentSwizzle.Zero => GLTextureSwizzle.Zero,
			_ => def
		};

		public static GLFilter Convert(TextureFilter filter) => filter switch {
			TextureFilter.Nearest => GLFilter.Nearest,
			TextureFilter.Linear => GLFilter.Linear,
			_ => default
		};

		public static GLCompareFunc Convert(CompareOp op) => op switch {
			CompareOp.Always => GLCompareFunc.Always,
			CompareOp.Equal => GLCompareFunc.Equal,
			CompareOp.Greater => GLCompareFunc.Greater,
			CompareOp.GreaterOrEqual => GLCompareFunc.GreaterOrEqual,
			CompareOp.Less => GLCompareFunc.Less,
			CompareOp.LessOrEqual => GLCompareFunc.LessOrEqual,
			CompareOp.Never => GLCompareFunc.Never,
			CompareOp.NotEqual => GLCompareFunc.NotEqual,
			_ => default
		};

		public static GLShaderType Convert(ShaderType type) => type switch {
			ShaderType.Vertex => GLShaderType.Vertex,
			ShaderType.TessellationControl => GLShaderType.TessellationControl,
			ShaderType.TessellationEvaluation => GLShaderType.TessellationEvaluation,
			ShaderType.Geometry => GLShaderType.Geometry,
			ShaderType.Fragment => GLShaderType.Fragment,
			ShaderType.Compute => GLShaderType.Compute,
			_ => default
		};

		public static GLBufferMask Convert(TextureAspect aspect) {
			GLBufferMask mask = 0;
			if ((aspect & TextureAspect.Color) != 0) mask |= GLBufferMask.Color;
			if ((aspect & TextureAspect.Depth) != 0) mask |= GLBufferMask.Depth;
			if ((aspect & TextureAspect.Stencil) != 0) mask |= GLBufferMask.Stencil;
			return mask;
		}

		public static GLIndexType Convert(IndexType type) => type switch {
			IndexType.UInt8 => GLIndexType.UnsignedByte,
			IndexType.UInt16 => GLIndexType.UnsignedShort,
			IndexType.UInt32 => GLIndexType.UnsignedInt,
			_ => default
		};

		public static GLPolygonMode Convert(PolygonMode mode) => mode switch {
			PolygonMode.Fill => GLPolygonMode.Fill,
			PolygonMode.Line => GLPolygonMode.Line,
			PolygonMode.Point => GLPolygonMode.Point,
			_ => default
		};

		public static GLBlendFactor Convert(BlendFactor blendFactor) => blendFactor switch {
			BlendFactor.Zero => GLBlendFactor.Zero,
			BlendFactor.One => GLBlendFactor.One,
			BlendFactor.SrcColor => GLBlendFactor.SrcColor,
			BlendFactor.OneMinusSrcColor => GLBlendFactor.OneMinusSrcColor,
			BlendFactor.DstColor => GLBlendFactor.DstColor,
			BlendFactor.OneMinusDstColor => GLBlendFactor.OneMinusDstColor,
			BlendFactor.SrcAlpha => GLBlendFactor.SrcAlpha,
			BlendFactor.OneMinusSrcAlpha => GLBlendFactor.OneMinusSrcAlpha,
			BlendFactor.DstAlpha => GLBlendFactor.DstAlpha,
			BlendFactor.OneMinusDstAlpha => GLBlendFactor.OneMinusDstAlpha,
			BlendFactor.ConstantColor => GLBlendFactor.ConstantColor,
			BlendFactor.OneMinusConstantColor => GLBlendFactor.OneMinusConstantColor,
			BlendFactor.ConstantAlpha => GLBlendFactor.ConstantAlpha,
			BlendFactor.OneMinusConstantAlpha => GLBlendFactor.OneMinusConstantAlpha,
			BlendFactor.Src1Color => GLBlendFactor.Src1Color,
			BlendFactor.OneMinusSrc1Color => GLBlendFactor.OneMinusSrc1Color,
			BlendFactor.Src1Alpha => GLBlendFactor.Src1Alpha,
			BlendFactor.OneMinusSrc1Alpha => GLBlendFactor.OneMinusSrc1Alpha,
			_ => default,
		};

		public static GLBlendFunction Convert(BlendOp blendEquation) => blendEquation switch {
			BlendOp.Add => GLBlendFunction.Add,
			BlendOp.Subtract => GLBlendFunction.Subtract,
			BlendOp.ReverseSubtract => GLBlendFunction.ReverseSubtract,
			BlendOp.Min => GLBlendFunction.Min,
			BlendOp.Max => GLBlendFunction.Max,
			_ => default,
		};

		public static GLDrawMode Convert(DrawMode mode) => mode switch {
			DrawMode.PointList => GLDrawMode.Points,
			DrawMode.LineList => GLDrawMode.Lines,
			DrawMode.LineStrip => GLDrawMode.LineStrip,
			DrawMode.TriangleList => GLDrawMode.Triangles,
			DrawMode.TriangleStrip => GLDrawMode.TriangleStrip,
			DrawMode.TriangleFan => GLDrawMode.TriangleFan,
			DrawMode.LineListWithAdjacency => GLDrawMode.LinesAdjacency,
			DrawMode.LineStripWithAdjacency => GLDrawMode.LineStripAdjacency,
			DrawMode.TriangleListWithAdjacency => GLDrawMode.TrianglesAdjacency,
			DrawMode.TriangleStripWithAdjacency => GLDrawMode.TriangleStripAdjacency,
			DrawMode.PatchList => GLDrawMode.Patches,
			_ => default,
		};

		public static GLFace Convert(CullFace face, out bool enable) {
			enable = face != CullFace.None;
			return face switch {
				CullFace.FrontAndBack => GLFace.FrontAndBack,
				CullFace.Back => GLFace.Back,
				CullFace.Front => GLFace.Front,
				_ => default
			};
		}

		public static GLCullFace Convert(FrontFace face) => face switch {
			FrontFace.Clockwise => GLCullFace.Clockwise,
			FrontFace.CounterClockwise => GLCullFace.CounterClockwise,
			_ => default
		};

		public static GLStencilOp Convert(StencilOp op) => op switch {
			StencilOp.IncrementAndClamp => GLStencilOp.Increment,
			StencilOp.IncrementAndWrap => GLStencilOp.IncrementAndWrap,
			StencilOp.DecrementAndClamp => GLStencilOp.Decrement,
			StencilOp.DecrementAndWrap => GLStencilOp.DecrementAndWrap,
			StencilOp.Invert => GLStencilOp.Invert,
			StencilOp.Keep => GLStencilOp.Keep,
			StencilOp.Replace => GLStencilOp.Replace,
			StencilOp.Zero => GLStencilOp.Zero,
			_ => default
		};

		public static GLStencilFunc ConvertStencilFunc(CompareOp op) => op switch {
			CompareOp.Always => GLStencilFunc.Always,
			CompareOp.Equal => GLStencilFunc.Equal,
			CompareOp.Greater => GLStencilFunc.Greater,
			CompareOp.Less => GLStencilFunc.Less,
			CompareOp.NotEqual => GLStencilFunc.NotEqual,
			CompareOp.GreaterOrEqual => GLStencilFunc.GreaterOrEqual,
			CompareOp.LessOrEqual => GLStencilFunc.LessOrEqual,
			CompareOp.Never => GLStencilFunc.Never,
			_ => default
		};

		public static GLLogicOp Convert(LogicOp op) => op switch {
			LogicOp.And => GLLogicOp.And,
			LogicOp.AndInverted => GLLogicOp.AndInverted,
			LogicOp.AndReverse => GLLogicOp.AndReverse,
			LogicOp.Clear => GLLogicOp.Clear,
			LogicOp.Copy => GLLogicOp.Copy,
			LogicOp.CopyInverted => GLLogicOp.CopyInverted,
			LogicOp.Invert => GLLogicOp.Invert,
			LogicOp.Nand => GLLogicOp.NAnd,
			LogicOp.NoOp => GLLogicOp.NoOp,
			LogicOp.Nor => GLLogicOp.NOr,
			LogicOp.Or => GLLogicOp.Or,
			LogicOp.OrInverted => GLLogicOp.OrInverted,
			LogicOp.OrReverse => GLLogicOp.OrReverse,
			LogicOp.Set => GLLogicOp.Set,
			LogicOp.XNor => GLLogicOp.Equiv,
			LogicOp.Xor => GLLogicOp.XOr,
			_ => default
		};

		public static GLFace Convert(CullFace face) => face switch {
			CullFace.Front => GLFace.Front,
			CullFace.Back => GLFace.Back,
			CullFace.FrontAndBack => GLFace.FrontAndBack,
			_ => default
		};

		public static GLMemoryBarrier Convert(ICommandSink.BufferMemoryBarrier barrier) {
			GLMemoryBarrier glbarrier = 0;
			IBuffer buffer = barrier.Buffer;
			if ((barrier.ProvokingAccess & MemoryAccess.IndirectCommandRead) != 0) glbarrier |= GLMemoryBarrier.Command;
			if ((barrier.ProvokingAccess & MemoryAccess.IndexRead) != 0) glbarrier |= GLMemoryBarrier.ElementArray;
			if ((barrier.ProvokingAccess & MemoryAccess.VertexAttributeRead) != 0) glbarrier |= GLMemoryBarrier.VertexAttribArray;
			if ((barrier.ProvokingAccess & MemoryAccess.UniformRead) != 0) glbarrier |= GLMemoryBarrier.Uniform;
			if ((barrier.ProvokingAccess & MemoryAccess.ShaderRead) != 0) {
				if ((buffer.Usage & BufferUsage.StorageBuffer) != 0) glbarrier |= GLMemoryBarrier.ShaderStorage;
				if ((buffer.Usage & BufferUsage.UniformTexelBuffer) != 0) glbarrier |= GLMemoryBarrier.TextureFetch;
				if ((buffer.Usage & BufferUsage.StorageTexelBuffer) != 0) glbarrier |= GLMemoryBarrier.ShaderImageAccess;
			}
			if ((barrier.ProvokingAccess & MemoryAccess.ShaderWrite) != 0) {
				if ((buffer.Usage & BufferUsage.StorageBuffer) != 0) glbarrier |= GLMemoryBarrier.ShaderStorage;
				if ((buffer.Usage & BufferUsage.StorageTexelBuffer) != 0) glbarrier |= GLMemoryBarrier.ShaderImageAccess;
			}
			if ((barrier.ProvokingAccess & (MemoryAccess.TransferRead | MemoryAccess.TransferWrite)) != 0) glbarrier |= GLMemoryBarrier.BufferUpdate;
			if ((barrier.ProvokingAccess & (MemoryAccess.HostRead | MemoryAccess.HostWrite)) != 0) glbarrier |= GLMemoryBarrier.ClientMappedBuffer;
			return glbarrier;
		}

		public static GLMemoryBarrier Convert(ICommandSink.TextureMemoryBarrier barrier) {
			GLMemoryBarrier glbarrier = 0;
			ITexture texture = barrier.Texture;
			if ((barrier.ProvokingAccess & MemoryAccess.ShaderRead) != 0) {
				if ((texture.Usage & TextureUsage.Sampled) != 0) glbarrier |= GLMemoryBarrier.TextureFetch;
				if ((texture.Usage & TextureUsage.Storage) != 0) glbarrier |= GLMemoryBarrier.ShaderImageAccess;
			}
			if ((barrier.ProvokingAccess & MemoryAccess.ShaderWrite) != 0) glbarrier |= GLMemoryBarrier.ShaderImageAccess;
			if ((barrier.ProvokingAccess & (
				MemoryAccess.ColorAttachmentRead | MemoryAccess.ColorAttachmentWrite |
				MemoryAccess.DepthStencilAttachmentRead | MemoryAccess.DepthStencilAttachmentWrite
			)) != 0) glbarrier |= GLMemoryBarrier.Framebuffer;
			if ((barrier.ProvokingAccess & (MemoryAccess.TransferRead | MemoryAccess.TransferWrite)) != 0) glbarrier |= GLMemoryBarrier.TextureUpdate;
			return glbarrier;
		}

		public static GLProgramInterface Convert(BindType type) => type switch {
			BindType.CombinedTextureSampler or BindType.StorageTexture => GLProgramInterface.Uniform,
			BindType.UniformBuffer => GLProgramInterface.UniformBlock,
			BindType.StorageBuffer => GLProgramInterface.ShaderStorageBlock,
			_ => throw new ArgumentException("Bind type not supported in OpenGL", nameof(type))
		};

	}

}

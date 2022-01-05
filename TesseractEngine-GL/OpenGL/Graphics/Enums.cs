using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {

	public record GLPixelFormat {

		public GLInternalFormat InternalFormat { get; init; }

		public GLFormat Format { get; init; }

		public GLTextureType Type { get; init; }

		public int Count { get; init; }

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
			_ => 0
		};

		public static int SizeOf(GLType type) => SizeOf((GLTextureType)type);

		public static int SizeOf(GLIndexType type) => SizeOf((GLTextureType)type);

		private static readonly Dictionary<GLInternalFormat, GLPixelFormat> internalFormats = new();

		private static readonly Dictionary<PixelFormat, GLInternalFormat> stdToInternalFormat = new() {
			{ PixelFormat.R8G8B8UNorm, GLInternalFormat.RGB8 },
			{ PixelFormat.R8G8B8A8UNorm, GLInternalFormat.RGBA8 },
			{ PixelFormat.R32G32B32A32SFloat, GLInternalFormat.RGBA32F },
			{ PixelFormat.D16UNorm, GLInternalFormat.DepthComponent16 },
			{ PixelFormat.X8D24UNorm, GLInternalFormat.DepthComponent24 },
			{ PixelFormat.D32SFloat, GLInternalFormat.DepthComponent32F },
			{ PixelFormat.D24UNormS8UInt, GLInternalFormat.Depth24Stencil8 },
			{ PixelFormat.D32SFloatS8UInt, GLInternalFormat.Depth32FStencil8 },
			{ PixelFormat.S8UInt, GLInternalFormat.StencilIndex8 }
		};

		private static void AddFormat(GLInternalFormat internalFormat, GLFormat format, GLTextureType type, int count) =>
			internalFormats[internalFormat] = new GLPixelFormat() { InternalFormat = internalFormat, Format = format, Type = type, Count = count };

		private static readonly Dictionary<PixelFormat, GLPixelFormat> stdToGLFormat = new() {
			{ PixelFormat.A8B8G8R8UNorm, new GLPixelFormat() {
				InternalFormat = GLInternalFormat.RGBA,
				Format = GLFormat.RGBA,
				Type = GLTextureType.UnsignedInt_8_8_8_8_Rev,
				Count = 4
			} }
		};

		static GLEnums() {
			AddFormat(GLInternalFormat.R8, GLFormat.R, GLTextureType.UnsignedByte, 1);
			AddFormat(GLInternalFormat.R8SNorm, GLFormat.R, GLTextureType.Byte, 1);
			AddFormat(GLInternalFormat.R8UI, GLFormat.R, GLTextureType.UnsignedByte, 1);
			AddFormat(GLInternalFormat.R8I, GLFormat.R, GLTextureType.Byte, 1);
			AddFormat(GLInternalFormat.RGBA8, GLFormat.RGBA, GLTextureType.UnsignedByte, 4);
		}

		private static bool TryGetFormat(PixelFormat format, out GLFormat glformat) {
			glformat = default;
			return false;
		}

		private static bool TryGetType(PixelFormat format, out GLTextureType type) {
			if (format.NumberFormat != ChannelNumberFormat.Undefined) {
				
			}
			type = default;
			return false;
		}

		public static GLPixelFormat StdToGLFormat(PixelFormat format) {
			if (stdToInternalFormat.TryGetValue(format, out GLInternalFormat glformat)) return internalFormats[glformat];
			if (stdToGLFormat.TryGetValue(format, out GLPixelFormat glpxformat)) return glpxformat;
			/* TODO: OpenGL supports more formats, but how to convert from PixelFormat correctly?
			foreach(GLPixelFormat glfmt in internalFormats.Values) {
				if (format.SizeOf != glfmt.SizeOf) continue;
			}
			*/
			return null;
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

		public static (GLType, int, bool) Convert(VertexAttribFormat format) => format switch {
			VertexAttribFormat.X32SFloat => (GLType.Float, 1, true),
			VertexAttribFormat.X32Y32SFloat => (GLType.Float, 2, true),
			VertexAttribFormat.X32Y32Z32SFloat => (GLType.Float, 3, true),
			VertexAttribFormat.X32Y32Z32W32SFloat => (GLType.Float, 4, true),
			VertexAttribFormat.X32SInt => (GLType.Int, 1, false),
			VertexAttribFormat.X32Y32SInt => (GLType.Int, 2, false),
			VertexAttribFormat.X32Y32Z32SInt => (GLType.Int, 3, false),
			VertexAttribFormat.X32Y32Z32W32SInt => (GLType.Int, 4, false),
			_ => default
		};

		public static GLIndexType Convert(IndexType type) => type switch {
			IndexType.UInt8 => GLIndexType.UnsignedByte,
			IndexType.UInt16 => GLIndexType.UnsignedShort,
			IndexType.UInt32 => GLIndexType.UnsignedInt,
			_ => default
		};

	}

}

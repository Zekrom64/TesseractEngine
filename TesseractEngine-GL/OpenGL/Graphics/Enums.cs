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

		public GLBufferMask Buffers { get; init; }

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

		public static GLPixelFormat? StdToGLFormat(PixelFormat format) {
			if (stdToInternalFormat.TryGetValue(format, out GLInternalFormat glformat)) return internalFormats[glformat];
			if (stdToGLFormat.TryGetValue(format, out GLPixelFormat? glpxformat)) return glpxformat;
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

	}

}

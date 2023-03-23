using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;
using Tesseract.Core.Collections;

namespace Tesseract.Vulkan.Services.Objects {

    public class VulkanConverter {

		private static readonly Dictionary<PixelFormat, VKFormat> stdToVk = new() {
			{ PixelFormat.R4G4UNormPack8, VKFormat.R4G4UnormPack8 },
			{ PixelFormat.R4G4B4A4UNormPack16, VKFormat.R4G4B4A4UNormPack16 },
			{ PixelFormat.B4G4R4A4UNormPack16, VKFormat.B4G4R4A4UNormPack16 },
			{ PixelFormat.R5G6B5UNormPack16, VKFormat.R5G6B5UNormPack16 },
			{ PixelFormat.B5G6R5UNormPack16, VKFormat.B5G6R5UNormPack16 },
			{ PixelFormat.R5G5B5A1UNormPack16, VKFormat.R5G5B5A1UNormPack16 },
			{ PixelFormat.B5G5R5A1UNormPack16, VKFormat.B5G5R5A1UNormPack16 },
			{ PixelFormat.A1R5G5B5UNormPack16, VKFormat.A1R5G5B5UNormPack16 },

			{ PixelFormat.R8UNorm, VKFormat.R8UNorm },
			{ PixelFormat.R8SNorm, VKFormat.R8SNorm },
			{ PixelFormat.R8UScaled, VKFormat.R8UScaled },
			{ PixelFormat.R8SScaled, VKFormat.R8SScaled },
			{ PixelFormat.R8UInt, VKFormat.R8UInt },
			{ PixelFormat.R8SInt, VKFormat.R8SInt },
			{ PixelFormat.R8SRGB, VKFormat.R8SRGB },
			{ PixelFormat.R8G8UNorm, VKFormat.R8G8UNorm },
			{ PixelFormat.R8G8SNorm, VKFormat.R8G8SNorm },
			{ PixelFormat.R8G8UScaled, VKFormat.R8G8UScaled },
			{ PixelFormat.R8G8SScaled, VKFormat.R8G8SScaled },
			{ PixelFormat.R8G8UInt, VKFormat.R8G8UInt },
			{ PixelFormat.R8G8SInt, VKFormat.R8G8SInt },
			{ PixelFormat.R8G8SRGB, VKFormat.R8G8SRGB },
			{ PixelFormat.R8G8B8UNorm, VKFormat.R8G8B8UNorm },
			{ PixelFormat.R8G8B8SNorm, VKFormat.R8G8B8SNorm },
			{ PixelFormat.R8G8B8UScaled, VKFormat.R8G8B8UScaled },
			{ PixelFormat.R8G8B8SScaled, VKFormat.R8G8B8SScaled },
			{ PixelFormat.R8G8B8UInt, VKFormat.R8G8B8UInt },
			{ PixelFormat.R8G8B8SInt, VKFormat.R8G8B8SInt },
			{ PixelFormat.R8G8B8SRGB, VKFormat.R8G8B8SRGB },
			{ PixelFormat.B8G8R8UNorm, VKFormat.B8G8R8UNorm },
			{ PixelFormat.B8G8R8SNorm, VKFormat.B8G8R8SNorm },
			{ PixelFormat.B8G8R8UScaled, VKFormat.B8G8R8UScaled },
			{ PixelFormat.B8G8R8SScaled, VKFormat.B8G8R8SScaled },
			{ PixelFormat.B8G8R8UInt, VKFormat.B8G8R8UInt },
			{ PixelFormat.B8G8R8SInt, VKFormat.B8G8R8SInt },
			{ PixelFormat.B8G8R8SRGB, VKFormat.B8G8R8SRGB },
			{ PixelFormat.R8G8B8A8UNorm, VKFormat.R8G8B8A8UNorm },
			{ PixelFormat.R8G8B8A8SNorm, VKFormat.R8G8B8A8SNorm },
			{ PixelFormat.R8G8B8A8UScaled, VKFormat.R8G8B8A8UScaled },
			{ PixelFormat.R8G8B8A8SScaled, VKFormat.R8G8B8A8SScaled },
			{ PixelFormat.R8G8B8A8UInt, VKFormat.R8G8B8A8UInt },
			{ PixelFormat.R8G8B8A8SInt, VKFormat.R8G8B8A8SInt },
			{ PixelFormat.R8G8B8A8SRGB, VKFormat.R8G8B8A8SRGB },
			{ PixelFormat.B8G8R8A8UNorm, VKFormat.B8G8R8A8UNorm },
			{ PixelFormat.B8G8R8A8SNorm, VKFormat.B8G8R8A8SNorm },
			{ PixelFormat.B8G8R8A8UScaled, VKFormat.B8G8R8A8UScaled },
			{ PixelFormat.B8G8R8A8SScaled, VKFormat.B8G8R8A8SScaled },
			{ PixelFormat.B8G8R8A8UInt, VKFormat.B8G8R8A8UInt },
			{ PixelFormat.B8G8R8A8SInt, VKFormat.B8G8R8A8SInt },
			{ PixelFormat.B8G8R8A8SRGB, VKFormat.B8G8R8A8SRGB },

			{ PixelFormat.A8B8G8R8UNormPack32, VKFormat.A8B8G8R8UNormPack32 },
			{ PixelFormat.A8B8G8R8SNormPack32, VKFormat.A8B8G8R8SNormPack32 },
			{ PixelFormat.A8B8G8R8UScaledPack32, VKFormat.A8B8G8R8UScaledPack32 },
			{ PixelFormat.A8B8G8R8SScaledPack32, VKFormat.A8B8G8R8SScaledPack32 },
			{ PixelFormat.A8B8G8R8UIntPack32, VKFormat.A8B8G8R8UIntPack32 },
			{ PixelFormat.A8B8G8R8SIntPack32, VKFormat.A8B8G8R8SIntPack32 },
			{ PixelFormat.A8B8G8R8SRGBPack32, VKFormat.A8B8G8R8SRGBPack32 },
			{ PixelFormat.A2R10G10B10UNormPack32, VKFormat.A2R10G10B10UNormPack32 },
			{ PixelFormat.A2R10G10B10SNormPack32, VKFormat.A2R10G10B10SNormPack32 },
			{ PixelFormat.A2R10G10B10UScaledPack32, VKFormat.A2R10G10B10UScaledPack32 },
			{ PixelFormat.A2R10G10B10SScaledPack32, VKFormat.A2R10G10B10SScaledPack32 },
			{ PixelFormat.A2R10G10B10UIntPack32, VKFormat.A2R10G10B10UIntPack32 },
			{ PixelFormat.A2R10G10B10SIntPack32, VKFormat.A2R10G10B10SIntPack32 },
			{ PixelFormat.A2B10G10R10UNormPack32, VKFormat.A2B10G10R10UNormPack32 },
			{ PixelFormat.A2B10G10R10SNormPack32, VKFormat.A2B10G10R10SNormPack32 },
			{ PixelFormat.A2B10G10R10UScaledPack32, VKFormat.A2B10G10R10UScaledPack32 },
			{ PixelFormat.A2B10G10R10SScaledPack32, VKFormat.A2B10G10R10SScaledPack32 },
			{ PixelFormat.A2B10G10R10UIntPack32, VKFormat.A2B10G10R10UIntPack32 },
			{ PixelFormat.A2B10G10R10SIntPack32, VKFormat.A2B10G10R10SIntPack32 },

			{ PixelFormat.R16UNorm, VKFormat.R16UNorm },
			{ PixelFormat.R16SNorm, VKFormat.R16SNorm },
			{ PixelFormat.R16UScaled, VKFormat.R16UScaled },
			{ PixelFormat.R16SScaled, VKFormat.R16SScaled },
			{ PixelFormat.R16UInt, VKFormat.R16UInt },
			{ PixelFormat.R16SInt, VKFormat.R16SInt },
			{ PixelFormat.R16SFloat, VKFormat.R16SFloat },
			{ PixelFormat.R16G16UNorm, VKFormat.R16G16UNorm },
			{ PixelFormat.R16G16SNorm, VKFormat.R16G16SNorm },
			{ PixelFormat.R16G16UScaled, VKFormat.R16G16UScaled },
			{ PixelFormat.R16G16SScaled, VKFormat.R16G16SScaled },
			{ PixelFormat.R16G16UInt, VKFormat.R16G16UInt },
			{ PixelFormat.R16G16SInt, VKFormat.R16G16SInt },
			{ PixelFormat.R16G16SFloat, VKFormat.R16G16SFloat },
			{ PixelFormat.R16G16B16UNorm, VKFormat.R16G16B16UNorm },
			{ PixelFormat.R16G16B16SNorm, VKFormat.R16G16B16SNorm },
			{ PixelFormat.R16G16B16UScaled, VKFormat.R16G16B16UScaled },
			{ PixelFormat.R16G16B16SScaled, VKFormat.R16G16B16SScaled },
			{ PixelFormat.R16G16B16UInt, VKFormat.R16G16B16UInt },
			{ PixelFormat.R16G16B16SInt, VKFormat.R16G16B16SInt },
			{ PixelFormat.R16G16B16SFloat, VKFormat.R16G16B16SFloat },
			{ PixelFormat.R16G16B16A16UNorm, VKFormat.R16G16B16A16UNorm },
			{ PixelFormat.R16G16B16A16SNorm, VKFormat.R16G16B16A16SNorm },
			{ PixelFormat.R16G16B16A16UScaled, VKFormat.R16G16B16A16UScaled },
			{ PixelFormat.R16G16B16A16SScaled, VKFormat.R16G16B16A16SScaled },
			{ PixelFormat.R16G16B16A16UInt, VKFormat.R16G16B16A16UInt },
			{ PixelFormat.R16G16B16A16SInt, VKFormat.R16G16B16A16SInt },
			{ PixelFormat.R16G16B16A16SFloat, VKFormat.R16G16B16A16SFloat },

			{ PixelFormat.R32UInt, VKFormat.R32UInt },
			{ PixelFormat.R32SInt, VKFormat.R32SInt },
			{ PixelFormat.R32SFloat, VKFormat.R32SFloat },
			{ PixelFormat.R32G32UInt, VKFormat.R32G32UInt },
			{ PixelFormat.R32G32SInt, VKFormat.R32G32SInt },
			{ PixelFormat.R32G32SFloat, VKFormat.R32G32SFloat },
			{ PixelFormat.R32G32B32UInt, VKFormat.R32G32B32UInt },
			{ PixelFormat.R32G32B32SInt, VKFormat.R32G32B32SInt },
			{ PixelFormat.R32G32B32SFloat, VKFormat.R32G32B32SFloat },
			{ PixelFormat.R32G32B32A32UInt, VKFormat.R32G32B32A32UInt },
			{ PixelFormat.R32G32B32A32SInt, VKFormat.R32G32B32A32SInt },
			{ PixelFormat.R32G32B32A32SFloat, VKFormat.R32G32B32A32SFloat },

			{ PixelFormat.R64UInt, VKFormat.R64UInt },
			{ PixelFormat.R64SInt, VKFormat.R64SInt },
			{ PixelFormat.R64SFloat, VKFormat.R64SFloat },
			{ PixelFormat.R64G64UInt, VKFormat.R64G64UInt },
			{ PixelFormat.R64G64SInt, VKFormat.R64G64SInt },
			{ PixelFormat.R64G64SFloat, VKFormat.R64G64SFloat },
			{ PixelFormat.R64G64B64UInt, VKFormat.R64G64B64UInt },
			{ PixelFormat.R64G64B64SInt, VKFormat.R64G64B64SInt },
			{ PixelFormat.R64G64B64SFloat, VKFormat.R64G64B64SFloat },
			{ PixelFormat.R64G64B64A64UInt, VKFormat.R64G64B64A64UInt },
			{ PixelFormat.R64G64B64A64SInt, VKFormat.R64G64B64A64SInt },
			{ PixelFormat.R64G64B64A64SFloat, VKFormat.R64G64B64A64SFloat },

			{ PixelFormat.B10G11R11UFloatPack32, VKFormat.B10G11R11UFloatPack32 },
			{ PixelFormat.E5B9G9R9UFloatPack32, VKFormat.E5B9G9R9UFloatPack32 },

			{ PixelFormat.D16UNorm, VKFormat.D16UNorm },
			{ PixelFormat.X8D24UNorm, VKFormat.X8D24UNormPack32 },
			{ PixelFormat.D32SFloat, VKFormat.D32SFloat },
			{ PixelFormat.S8UInt, VKFormat.S8UInt },
			{ PixelFormat.D16UNormS8UInt, VKFormat.D16UNormS8UInt },
			{ PixelFormat.D24UNormS8UInt, VKFormat.D24UNormS8UInt },
			{ PixelFormat.D32SFloatS8UInt, VKFormat.D32SFloatS8UInt }
		};

		private static readonly Dictionary<VKFormat, PixelFormat?> vkToStd = stdToVk.ToDictionary(item => item.Value, item => (PixelFormat?)item.Key);

		public static VKFormat Convert(PixelFormat format) => stdToVk.GetValueOrDefault(format, VKFormat.Undefined);

		public static PixelFormat? Convert(VKFormat format) => vkToStd.GetValueOrDefault(format, null);

		public static VKFilter ConvertFilter(TextureFilter filter) => filter switch {
			TextureFilter.Linear => VKFilter.Linear,
			TextureFilter.Nearest => VKFilter.Nearest,
			_ => default
		};

		public static VKSamplerMipmapMode ConvertMipmapMode(TextureFilter filter) => filter switch {
			TextureFilter.Nearest => VKSamplerMipmapMode.Nearest,
			TextureFilter.Linear => VKSamplerMipmapMode.Linear,
			_ => default,
		};

		public static VKSamplerAddressMode Convert(SamplerAddressMode mode) => mode switch {
			SamplerAddressMode.Repeat => VKSamplerAddressMode.Repeat,
			SamplerAddressMode.MirroredRepeat => VKSamplerAddressMode.MirroredRepeat,
			SamplerAddressMode.ClampToEdge => VKSamplerAddressMode.ClampToEdge,
			SamplerAddressMode.ClampToBorder => VKSamplerAddressMode.ClampToBorder,
			SamplerAddressMode.MirrorClampToEdge => VKSamplerAddressMode.MirrorClampToEdge,
			_ => default,
		};

		public static VKCompareOp Convert(CompareOp op) => op switch {
			CompareOp.Never => VKCompareOp.Never,
			CompareOp.Less => VKCompareOp.Less,
			CompareOp.Equal => VKCompareOp.Equal,
			CompareOp.LessOrEqual => VKCompareOp.LessOrEqual,
			CompareOp.Greater => VKCompareOp.Greater,
			CompareOp.NotEqual => VKCompareOp.NotEqual,
			CompareOp.GreaterOrEqual => VKCompareOp.GreaterOrEqual,
			CompareOp.Always => VKCompareOp.Always,
			_ => default,
		};

		public static VKBorderColor Convert(SamplerBorderColor color) {
			return color switch {
				SamplerBorderColor.TransparentBlackNorm => VKBorderColor.FloatTransparentBlack,
				SamplerBorderColor.OpaqueBlackNorm => VKBorderColor.FloatOpaqueBlack,
				SamplerBorderColor.OpaqueWhiteNorm => VKBorderColor.FloatOpaqueWhite,
				SamplerBorderColor.CustomNorm => VKBorderColor.FloatCustomEXT,
				SamplerBorderColor.TransparentBlackInt => VKBorderColor.IntTransparentBlack,
				SamplerBorderColor.OpaqueBlackInt => VKBorderColor.IntOpaqueBlack,
				SamplerBorderColor.OpaqueWhiteInt => VKBorderColor.IntOpaqueWhite,
				SamplerBorderColor.CustomInt => VKBorderColor.IntCustomEXT,
				_ => default,
			};
		}

		public static VKClearColorValue ConvertClearColor(object o) {
			VKClearColorValue clear = new();
			if (o is Vector4 v4) clear.Float32 = v4;
			else if (o is Vector4i v4i) clear.Int32 = v4i;
			else if (o is Vector4ui v4u) clear.UInt32 = v4u;
			else if (o is IReadOnlyTuple4<float> t4f) clear.Float32 = new Vector4(t4f.X, t4f.Y, t4f.Z, t4f.W);
			else if (o is IReadOnlyTuple4<int> t4i) clear.Int32 = new Vector4i(t4i.X, t4i.Y, t4i.Z, t4i.W);
			else if (o is IReadOnlyTuple4<uint> t4u) clear.UInt32 = new Vector4ui(t4u.X, t4u.Y, t4u.Z, t4u.W);
			return clear;
		}

		public static VKSampleCountFlagBits ConvertSampleCount(uint samples) => (VKSampleCountFlagBits)(1 << BitOperations.Log2(samples));

		public static VKComponentSwizzle Convert(ComponentSwizzle swizzle) => swizzle switch {
			ComponentSwizzle.Identity => VKComponentSwizzle.Identity,
			ComponentSwizzle.Zero => VKComponentSwizzle.Zero,
			ComponentSwizzle.One => VKComponentSwizzle.One,
			ComponentSwizzle.Red => VKComponentSwizzle.R,
			ComponentSwizzle.Green => VKComponentSwizzle.G,
			ComponentSwizzle.Blue => VKComponentSwizzle.B,
			ComponentSwizzle.Alpha => VKComponentSwizzle.A,
			_ => default,
		};

		public static VKComponentMapping Convert(ComponentMapping mapping) => new() {
			R = Convert(mapping.Red),
			G = Convert(mapping.Green),
			B = Convert(mapping.Blue),
			A = Convert(mapping.Alpha)
		};

		public static VKImageViewType ConvertImageViewType(TextureType type) => type switch {
			TextureType.Texture1D => VKImageViewType.Type1D,
			TextureType.Texture1DArray => VKImageViewType.Array1D,
			TextureType.Texture2D => VKImageViewType.Type2D,
			TextureType.Texture2DCube => VKImageViewType.Cube,
			TextureType.Texture2DArray => VKImageViewType.Array2D,
			TextureType.Texture2DCubeArray => VKImageViewType.CubeArray,
			TextureType.Texture3D => VKImageViewType.Type3D,
			_ => default,
		};

		public static VKImageAspectFlagBits Convert(TextureAspect aspect) {
			VKImageAspectFlagBits flags = 0;
			if ((aspect & TextureAspect.Color) != 0) flags |= VKImageAspectFlagBits.Color;
			if ((aspect & TextureAspect.Depth) != 0) flags |= VKImageAspectFlagBits.Depth;
			if ((aspect & TextureAspect.Stencil) != 0) flags |= VKImageAspectFlagBits.Stencil;
			return flags;
		}

		public static VKImageSubresourceRange Convert(TextureSubresourceRange range) => new() {
			AspectMask = Convert(range.Aspects),
			BaseArrayLayer = range.BaseArrayLayer,
			BaseMipLevel = range.BaseMipLevel,
			LayerCount = range.ArrayLayerCount,
			LevelCount = range.MipLevelCount
		};

		public static VKIndexType Convert(IndexType type) => type switch {
			IndexType.UInt8 => VKIndexType.UInt8EXT,
			IndexType.UInt16 => VKIndexType.UInt16,
			IndexType.UInt32 => VKIndexType.UInt32,
			_ => default
		};

		public static VKImageLayout Convert(TextureLayout layout) => layout switch {
			TextureLayout.Undefined => VKImageLayout.Undefined,
			TextureLayout.General => VKImageLayout.General,
			TextureLayout.ColorAttachment => VKImageLayout.ColorAttachmentOptimal,
			TextureLayout.DepthStencilAttachment => VKImageLayout.DepthStencilAttachmentOptimal,
			TextureLayout.DepthStencilSampled => VKImageLayout.DepthStencilReadOnlyOptimal,
			TextureLayout.ShaderSampled => VKImageLayout.ShaderReadOnlyOptimal,
			TextureLayout.TransferSrc => VKImageLayout.TransferSrcOptimal,
			TextureLayout.TransferDst => VKImageLayout.TransferDstOptimal,
			TextureLayout.PresentSrc => VKImageLayout.PresentSrcKHR,
			_ => default
		};

		public static VKImageBlit Convert(ICommandSink.BlitTextureRegion region, VulkanTexture src, VulkanTexture dst) => new() {
			SrcOffsets = new((Vector3i)region.SrcOffset0, (Vector3i)region.SrcOffset1),
			SrcSubresource = new() {
				AspectMask = Convert(region.Aspect),
				BaseArrayLayer = src.Type switch {
					TextureType.Texture1DArray => region.SrcOffset0.Y,
					TextureType.Texture2DArray or TextureType.Texture2DCube or TextureType.Texture2DCubeArray => region.SrcOffset0.Z,
					_ => 0
				},
				LayerCount = src.Type switch {
					TextureType.Texture1DArray => (region.SrcOffset1.Y - region.SrcOffset0.Y),
					TextureType.Texture2DArray or TextureType.Texture2DCube or TextureType.Texture2DCubeArray => (region.SrcOffset1.Z - region.SrcOffset0.Z),
					_ => 1
				},
				MipLevel = region.SrcLevel
			},
			DstOffsets = new((Vector3i)region.DstOffset0, (Vector3i)region.DstOffset1),
			DstSubresource = new() {
				AspectMask = Convert(region.Aspect),
				BaseArrayLayer = dst.Type switch {
					TextureType.Texture1DArray => region.DstOffset0.Y,
					TextureType.Texture2DArray or TextureType.Texture2DCube or TextureType.Texture2DCubeArray => region.DstOffset0.Z,
					_ => 0
				},
				LayerCount = dst.Type switch {
					TextureType.Texture1DArray => (region.SrcOffset1.Y - region.DstOffset0.Y),
					TextureType.Texture2DArray or TextureType.Texture2DCube or TextureType.Texture2DCubeArray => (region.DstOffset1.Z - region.DstOffset0.Z),
					_ => 1
				},
				MipLevel = region.DstLevel
			}
		};

		public static VKFilter Convert(TextureFilter filter) => filter switch {
			TextureFilter.Nearest => VKFilter.Nearest,
			TextureFilter.Linear => VKFilter.Linear,
			_ => default,
		};

		public static VKStencilFaceFlagBits ConvertToStencilFace(CullFace face) {
			VKStencilFaceFlagBits flags = 0;
			if ((face & CullFace.Front) != 0) flags |= VKStencilFaceFlagBits.Front;
			if ((face & CullFace.Back) != 0) flags |= VKStencilFaceFlagBits.Back;
			return flags;
		}

		public static VKPipelineStageFlagBits Convert(PipelineStage stage) {
			VKPipelineStageFlagBits flags = 0;
			if ((stage & PipelineStage.AllCommands) != 0) flags |= VKPipelineStageFlagBits.AllCommands;
			if ((stage & PipelineStage.AllGraphics) != 0) flags |= VKPipelineStageFlagBits.AllGraphics;
			if ((stage & PipelineStage.Bottom) != 0) flags |= VKPipelineStageFlagBits.BottomOfPipe;
			if ((stage & PipelineStage.ColorAttachmentOutput) != 0) flags |= VKPipelineStageFlagBits.ColorAttachmentOutput;
			if ((stage & PipelineStage.ComputeShader) != 0) flags |= VKPipelineStageFlagBits.ComputeShader;
			if ((stage & PipelineStage.DrawIndirect) != 0) flags |= VKPipelineStageFlagBits.DrawIndirect;
			if ((stage & PipelineStage.EarlyFragmentTests) != 0) flags |= VKPipelineStageFlagBits.EarlyFragmentTests;
			if ((stage & PipelineStage.FragmentShader) != 0) flags |= VKPipelineStageFlagBits.FragmentShader;
			if ((stage & PipelineStage.GeometryShader) != 0) flags |= VKPipelineStageFlagBits.GeometryShader;
			if ((stage & PipelineStage.Host) != 0) flags |= VKPipelineStageFlagBits.Host;
			if ((stage & PipelineStage.LateFragmentTests) != 0) flags |= VKPipelineStageFlagBits.LateFragmentTests;
			if ((stage & PipelineStage.TessellationControlShader) != 0) flags |= VKPipelineStageFlagBits.TessellationControlShader;
			if ((stage & PipelineStage.TessellationEvaluationShader) != 0) flags |= VKPipelineStageFlagBits.TessellationEvaluationShader;
			if ((stage & PipelineStage.Top) != 0) flags |= VKPipelineStageFlagBits.TopOfPipe;
			if ((stage & PipelineStage.Transfer) != 0) flags |= VKPipelineStageFlagBits.Transfer;
			if ((stage & PipelineStage.VertexInput) != 0) flags |= VKPipelineStageFlagBits.VertexInput;
			if ((stage & PipelineStage.VertexShader) != 0) flags |= VKPipelineStageFlagBits.VertexShader;
			return flags;
		}

		public static VKSubpassContents Convert(SubpassContents contents) => contents switch {
			SubpassContents.Inline => VKSubpassContents.Inline,
			SubpassContents.SecondaryCommandBuffers => VKSubpassContents.SecondaryCommandBuffers,
			_ => default
		};

		public static VKAccessFlagBits Convert(MemoryAccess access) {
			VKAccessFlagBits flags = 0;
			if ((access & MemoryAccess.ColorAttachmentRead) != 0) flags |= VKAccessFlagBits.ColorAttachmentRead;
			if ((access & MemoryAccess.ColorAttachmentWrite) != 0) flags |= VKAccessFlagBits.ColorAttachmentWrite;
			if ((access & MemoryAccess.DepthStencilAttachmentRead) != 0) flags |= VKAccessFlagBits.DepthStencilAttachmentRead;
			if ((access & MemoryAccess.DepthStencilAttachmentWrite) != 0) flags |= VKAccessFlagBits.DepthStencilAttachmentWrite;
			if ((access & MemoryAccess.HostRead) != 0) flags |= VKAccessFlagBits.HostRead;
			if ((access & MemoryAccess.HostWrite) != 0) flags |= VKAccessFlagBits.HostWrite;
			if ((access & MemoryAccess.IndexRead) != 0) flags |= VKAccessFlagBits.IndexRead;
			if ((access & MemoryAccess.IndirectCommandRead) != 0) flags |= VKAccessFlagBits.IndirectCommandRead;
			if ((access & MemoryAccess.InputAttachmentRead) != 0) flags |= VKAccessFlagBits.InputAttachmentRead;
			if ((access & MemoryAccess.MemoryRead) != 0) flags |= VKAccessFlagBits.MemoryRead;
			if ((access & MemoryAccess.MemoryWrite) != 0) flags |= VKAccessFlagBits.MemoryWrite;
			if ((access & MemoryAccess.ShaderRead) != 0) flags |= VKAccessFlagBits.ShaderRead;
			if ((access & MemoryAccess.ShaderWrite) != 0) flags |= VKAccessFlagBits.ShaderWrite;
			if ((access & MemoryAccess.TransferRead) != 0) flags |= VKAccessFlagBits.TransferRead;
			if ((access & MemoryAccess.TransferWrite) != 0) flags |= VKAccessFlagBits.TransferWrite;
			if ((access & MemoryAccess.UniformRead) != 0) flags |= VKAccessFlagBits.UniformRead;
			if ((access & MemoryAccess.VertexAttributeRead) != 0) flags |= VKAccessFlagBits.VertexAttributeRead;
			return flags;
		}

		public static VKMemoryBarrier Convert(ICommandSink.MemoryBarrier barrier) => new() {
			Type = VKStructureType.MemoryBarrier,
			SrcAccessMask = Convert(barrier.ProvokingAccess),
			DstAccessMask = Convert(barrier.AwaitingAccess)
		};

		public static VKBufferMemoryBarrier Convert(ICommandSink.BufferMemoryBarrier barrier) {
			MemoryRange range = barrier.Range.Constrain(barrier.Buffer.Size);
			return new VKBufferMemoryBarrier() {
				Type = VKStructureType.BufferMemoryBarrier,
				Buffer = ((VulkanBuffer)barrier.Buffer).Buffer,
				SrcAccessMask = Convert(barrier.ProvokingAccess),
				DstAccessMask = Convert(barrier.AwaitingAccess),
				SrcQueueFamilyIndex = VK10.QueueFamilyIgnored,
				DstQueueFamilyIndex = VK10.QueueFamilyIgnored,
				Offset = range.Offset,
				Size = range.Length
			};
		}

		public static VKImageMemoryBarrier Convert(ICommandSink.TextureMemoryBarrier barrier) => new() {
			Type = VKStructureType.ImageMemoryBarrier,
			Image = ((VulkanTexture)barrier.Texture).Image,
			SrcAccessMask = Convert(barrier.ProvokingAccess),
			DstAccessMask = Convert(barrier.AwaitingAccess),
			SrcQueueFamilyIndex = VK10.QueueFamilyIgnored,
			DstQueueFamilyIndex = VK10.QueueFamilyIgnored,
			OldLayout = Convert(barrier.OldLayout),
			NewLayout = Convert(barrier.NewLayout),
			SubresourceRange = Convert(barrier.SubresourceRange)
		};

		public static VKCommandBufferLevel Convert(CommandBufferType type) => type switch {
			CommandBufferType.Primary => VKCommandBufferLevel.Primary,
			CommandBufferType.Secondary => VKCommandBufferLevel.Secondary,
			_ => default
		};

		public static VKShaderStageFlagBits Convert(ShaderType type) => type switch {
			ShaderType.Vertex => VKShaderStageFlagBits.Vertex,
			ShaderType.TessellationControl => VKShaderStageFlagBits.TessellationControl,
			ShaderType.TessellationEvaluation => VKShaderStageFlagBits.TessellationEvaluation,
			ShaderType.Geometry => VKShaderStageFlagBits.Geometry,
			ShaderType.Fragment => VKShaderStageFlagBits.Fragment,
			ShaderType.Compute => VKShaderStageFlagBits.Compute,
			_ => default
		};

		public static VKVertexInputRate Convert(VertexInputRate rate) => rate switch {
			VertexInputRate.PerVertex => VKVertexInputRate.Vertex,
			VertexInputRate.PerInstance => VKVertexInputRate.Instance,
			_ => default
		};

		public static VKPipelineVertexInputStateCreateInfo Convert(MemoryStack sp, VertexFormat format) => new() {
			Type = VKStructureType.PipelineVertexInputStateCreateInfo,
			VertexAttributeDescriptionCount = (uint)format.Attributes.Count,
			VertexAttributeDescriptions = sp.Values(Collection.ConvertAll(format.Attributes, attrib => new VKVertexInputAttributeDescription() {
				Binding = attrib.Binding,
				Format = Convert(attrib.Format),
				Location = attrib.Location,
				Offset = attrib.Offset
			})),
			VertexBindingDescriptionCount = (uint)format.Bindings.Count,
			VertexBindingDescriptions = sp.Values(Collection.ConvertAll(format.Bindings, bind => new VKVertexInputBindingDescription() {
				Binding = bind.Binding,
				InputRate = Convert(bind.InputRate),
				Stride = bind.Stride
			}))
		};

		public static VKPrimitiveTopology Convert(DrawMode mode) => mode switch {
			DrawMode.PointList => VKPrimitiveTopology.PointList,
			DrawMode.LineList => VKPrimitiveTopology.LineList,
			DrawMode.LineStrip => VKPrimitiveTopology.LineStrip,
			DrawMode.TriangleList => VKPrimitiveTopology.TriangleList,
			DrawMode.TriangleStrip => VKPrimitiveTopology.TriangleStrip,
			DrawMode.TriangleFan => VKPrimitiveTopology.TriangleFan,
			DrawMode.LineListWithAdjacency => VKPrimitiveTopology.LineListWithAdjacency,
			DrawMode.LineStripWithAdjacency => VKPrimitiveTopology.LineStripWithAdjacency,
			DrawMode.TriangleListWithAdjacency => VKPrimitiveTopology.TriangleListWithAdjacency,
			DrawMode.TriangleStripWithAdjacency => VKPrimitiveTopology.TriangleStripWithAdjacency,
			DrawMode.PatchList => VKPrimitiveTopology.PatchList,
			_ => default
		};

		public static VKPolygonMode Convert(PolygonMode mode) => mode switch {
			PolygonMode.Fill => VKPolygonMode.Fill,
			PolygonMode.Line => VKPolygonMode.Line,
			PolygonMode.Point => VKPolygonMode.Point,
			_ => default
		};

		public static VKCullModeFlagBits ConvertCullMode(CullFace cull) {
			VKCullModeFlagBits flags = 0;
			if ((cull & CullFace.Back) != 0) flags |= VKCullModeFlagBits.Back;
			if ((cull & CullFace.Front) != 0) flags |= VKCullModeFlagBits.Front;
			return flags;
		}

		public static VKFrontFace Convert(FrontFace face) => face switch {
			FrontFace.Clockwise => VKFrontFace.Clockwise,
			FrontFace.CounterClockwise => VKFrontFace.CounterClockwise,
			_ => default
		};

		public static VKStencilOp Convert(StencilOp op) => op switch {
			StencilOp.Keep => VKStencilOp.Keep,
			StencilOp.Zero => VKStencilOp.Zero,
			StencilOp.Replace => VKStencilOp.Replace,
			StencilOp.IncrementAndClamp => VKStencilOp.IncrementAndClamp,
			StencilOp.DecrementAndClamp => VKStencilOp.DecrementAndClamp,
			StencilOp.Invert => VKStencilOp.Invert,
			StencilOp.IncrementAndWrap => VKStencilOp.IncrementAndWrap,
			StencilOp.DecrementAndWrap => VKStencilOp.DecrementAndWrap,
			_ => default
		};

		public static VKStencilOpState Convert(PipelineStencilState state) => new() {
			CompareMask = state.CompareMask,
			WriteMask = state.WriteMask,
			Reference = state.Reference,
			CompareOp = Convert(state.CompareOp),
			FailOp = Convert(state.FailOp),
			PassOp = Convert(state.PassOp),
			DepthFailOp = Convert(state.DepthFailOp)
		};

		public static VKLogicOp Convert(LogicOp op) => op switch {
			LogicOp.Clear => VKLogicOp.Clear,
			LogicOp.And => VKLogicOp.And,
			LogicOp.AndReverse => VKLogicOp.AndReverse,
			LogicOp.Copy => VKLogicOp.Copy,
			LogicOp.AndInverted => VKLogicOp.AndInverted,
			LogicOp.NoOp => VKLogicOp.NoOp,
			LogicOp.Xor => VKLogicOp.Xor,
			LogicOp.Or => VKLogicOp.Or,
			LogicOp.Nor => VKLogicOp.Nor,
			LogicOp.XNor => VKLogicOp.Equivalent,
			LogicOp.Invert => VKLogicOp.Invert,
			LogicOp.OrReverse => VKLogicOp.OrReverse,
			LogicOp.CopyInverted => VKLogicOp.CopyInverted,
			LogicOp.OrInverted => VKLogicOp.OrInverted,
			LogicOp.Nand => VKLogicOp.Nand,
			LogicOp.Set => VKLogicOp.Set,
			_ => default
		};

		public static VKBlendFactor Convert(BlendFactor factor) => factor switch {
			BlendFactor.Zero => VKBlendFactor.Zero,
			BlendFactor.One => VKBlendFactor.One,
			BlendFactor.SrcColor => VKBlendFactor.SrcColor,
			BlendFactor.OneMinusSrcColor => VKBlendFactor.OneMinusSrcColor,
			BlendFactor.DstColor => VKBlendFactor.DstColor,
			BlendFactor.OneMinusDstColor => VKBlendFactor.OneMinusDstColor,
			BlendFactor.SrcAlpha => VKBlendFactor.SrcAlpha,
			BlendFactor.OneMinusSrcAlpha => VKBlendFactor.OneMinusSrcAlpha,
			BlendFactor.DstAlpha => VKBlendFactor.DstAlpha,
			BlendFactor.OneMinusDstAlpha => VKBlendFactor.OneMinusDstAlpha,
			BlendFactor.ConstantColor => VKBlendFactor.ConstantColor,
			BlendFactor.OneMinusConstantColor => VKBlendFactor.OneMinusConstantColor,
			BlendFactor.ConstantAlpha => VKBlendFactor.ConstantAlpha,
			BlendFactor.OneMinusConstantAlpha => VKBlendFactor.OneMinusConstantAlpha,
			BlendFactor.Src1Color => VKBlendFactor.Src1Color,
			BlendFactor.OneMinusSrc1Color => VKBlendFactor.OneMinusSrc1Color,
			BlendFactor.Src1Alpha => VKBlendFactor.Src1Alpha,
			BlendFactor.OneMinusSrc1Alpha => VKBlendFactor.OneMinusSrc1Alpha,
			_ => default
		};

		public static VKBlendOp Convert(BlendOp op) => op switch {
			BlendOp.Add => VKBlendOp.Add,
			BlendOp.Subtract => VKBlendOp.Subtract,
			BlendOp.ReverseSubtract => VKBlendOp.ReverseSubtract,
			BlendOp.Min => VKBlendOp.Min,
			BlendOp.Max => VKBlendOp.Max,
			_ => default,
		};

		public static VKColorComponentFlagBits Convert(ColorComponent comp) {
			VKColorComponentFlagBits flags = 0;
			if ((comp & ColorComponent.Red) != 0) flags |= VKColorComponentFlagBits.R;
			if ((comp & ColorComponent.Green) != 0) flags |= VKColorComponentFlagBits.G;
			if ((comp & ColorComponent.Blue) != 0) flags |= VKColorComponentFlagBits.B;
			if ((comp & ColorComponent.Alpha) != 0) flags |= VKColorComponentFlagBits.A;
			return flags;
		}

		public static VKPipelineColorBlendAttachmentState Convert(PipelineColorAttachmentState state) => new() {
			BlendEnable = state.BlendEnable,
			SrcColorBlendFactor = Convert(state.BlendEquation.SrcRGB),
			DstColorBlendFactor = Convert(state.BlendEquation.DstRGB),
			ColorBlendOp = Convert(state.BlendEquation.RGBOp),
			SrcAlphaBlendFactor = Convert(state.BlendEquation.SrcAlpha),
			DstAlphaBlendFactor = Convert(state.BlendEquation.DstAlpha),
			AlphaBlendOp = Convert(state.BlendEquation.AlphaOp),
			ColorWriteMask = Convert(state.ColorWriteMask)
		};

		public static VKDynamicState Convert(PipelineDynamicState state) => state switch {
			PipelineDynamicState.Viewport => VKDynamicState.Viewport,
			PipelineDynamicState.Scissor => VKDynamicState.Scissor,
			PipelineDynamicState.LineWidth => VKDynamicState.LineWidth,
			PipelineDynamicState.DepthBias => VKDynamicState.DepthBias,
			PipelineDynamicState.BlendConstants => VKDynamicState.BlendConstants,
			PipelineDynamicState.DepthBounds => VKDynamicState.DepthBounds,
			PipelineDynamicState.StencilCompareMask => VKDynamicState.StencilCompareMask,
			PipelineDynamicState.StencilWriteMask => VKDynamicState.StencilWriteMask,
			PipelineDynamicState.StencilReference => VKDynamicState.StencilReference,
			PipelineDynamicState.CullMode => VKDynamicState.CullMode,
			PipelineDynamicState.FrontFace => VKDynamicState.FrontFace,
			PipelineDynamicState.DrawMode => VKDynamicState.PrimitiveTopology,
			PipelineDynamicState.DepthTestEnable => VKDynamicState.DepthTestEnable,
			PipelineDynamicState.DepthWriteEnable => VKDynamicState.DepthWriteEnable,
			PipelineDynamicState.DepthCompareOp => VKDynamicState.DepthCompareOp,
			PipelineDynamicState.DepthBoundsTestEnable => VKDynamicState.DepthBoundsTestEnable,
			PipelineDynamicState.StencilTestEnable => VKDynamicState.StencilTestEnable,
			PipelineDynamicState.StencilOp => VKDynamicState.StencilOp,
			PipelineDynamicState.PatchControlPoints => VKDynamicState.PatchControlPointsEXT,
			PipelineDynamicState.RasterizerDiscardEnable => VKDynamicState.RasterizerDiscardEnable,
			PipelineDynamicState.DepthBiasEnable => VKDynamicState.DepthBiasEnable,
			PipelineDynamicState.LogicOp => VKDynamicState.LogicOpEXT,
			PipelineDynamicState.PrimitiveRestartEnable => VKDynamicState.PrimitiveRestartEnable,
			PipelineDynamicState.VertexFormat => VKDynamicState.VertexInputEXT,
			PipelineDynamicState.ColorWrite => VKDynamicState.ColorWriteEnableEXT,
			PipelineDynamicState.ViewportCount => VKDynamicState.ViewportWithCount,
			PipelineDynamicState.ScissorCount => VKDynamicState.ScissorWithCount,
			_ => default
		};

		public static VKGraphicsPipelineCreateInfo ConvertGraphicsPipeline(MemoryStack sp, PipelineCreateInfo createInfo, List<IDisposable> disposables) {
			var gfxInfo = createInfo.GraphicsInfo;
			var dynInfo = gfxInfo!.DynamicInfo;
			
			ManagedPointer<VKPipelineShaderStageCreateInfo> pStages = new(Collection.ConvertAll(gfxInfo.Shaders, Convert));
			disposables.Add(pStages);
			
			bool hasTessState = false;
			foreach (var shader in gfxInfo.Shaders)
				if (shader.Type == ShaderType.TessellationControl || shader.Type == ShaderType.TessellationEvaluation)
					hasTessState = true;

			List<VKDynamicState> states = new();
			foreach (PipelineDynamicState dyn in gfxInfo.DynamicState) states.Add(Convert(dyn));
			UnmanagedPointer<VKDynamicState> pStates = sp.Values(states);

			VKPipelineCreateFlagBits flags = VKPipelineCreateFlagBits.AllowDerivatives;
			if (createInfo.BasePipeline != null || createInfo.BasePipelineIndex.HasValue) flags |= VKPipelineCreateFlagBits.Derivative;

			return new() {
				Type = VKStructureType.GraphicsPipelineCreateInfo,
				Flags = flags,
				StageCount = (uint)gfxInfo.Shaders.Count,
				Stages = pStages,
				VertexInputState = sp.Values(Convert(sp, dynInfo.VertexFormat)),
				InputAssemblyState = sp.Values(new VKPipelineInputAssemblyStateCreateInfo() {
					Type = VKStructureType.PipelineInputAssemblyStateCreateInfo,
					PrimitiveRestartEnable = dynInfo.PrimitiveRestartEnable,
					Topology = Convert(dynInfo.DrawMode)
				}),
				TessellationState = hasTessState ? sp.Values(new VKPipelineTessellationStateCreateInfo() {
					Type = VKStructureType.PipelineTesselationStateCreateInfo,
					PatchControlPoints = dynInfo.PatchControlPoints
				}) : IntPtr.Zero,
				ViewportState = sp.Values(new VKPipelineViewportStateCreateInfo() {
					Type = VKStructureType.PipelineViewportStateCreateInfo,
					ViewportCount = gfxInfo.ViewportCount,
					Viewports = sp.Values(dynInfo.Viewports),
					ScissorCount = gfxInfo.ScissorCount,
					Scissors = sp.Values(dynInfo.Scissors)
				}),
				RasterizationState = sp.Values(new VKPipelineRasterizationStateCreateInfo() {
					Type = VKStructureType.PipelineRasterizationStateCreateInfo,
					DepthClampEnable = gfxInfo.DepthClampEnable,
					RasterizerDiscardEnable = dynInfo.RasterizerDiscardEnable,
					PolygonMode = Convert(gfxInfo.PolygonMode),
					CullMode = ConvertCullMode(dynInfo.CullMode),
					FrontFace = Convert(dynInfo.FrontFace),
					DepthBiasEnable = dynInfo.DepthBiasEnable,
					DepthBiasConstantFactor = dynInfo.DepthBiasConstantFactor,
					DepthBiasClamp = dynInfo.DepthBiasClamp,
					DepthBiasSlopeFactor = dynInfo.DepthBiasSlopeFactor,
					LineWidth = dynInfo.LineWidth
				}),
				MultisampleState = IntPtr.Zero,
				DepthStencilState = sp.Values(new VKPipelineDepthStencilStateCreateInfo() {
					Type = VKStructureType.PipelineDepthStencilStateCreateInfo,
					DepthTestEnable = dynInfo.DepthTestEnable,
					DepthWriteEnable = dynInfo.DepthWriteEnable,
					DepthCompareOp = Convert(dynInfo.DepthCompareOp),
					DepthBoundsTestEnable = dynInfo.DepthBoundsTestEnable,
					StencilTestEnable = dynInfo.StencilTestEnable,
					Front = Convert(dynInfo.FrontStencilState),
					Back = Convert(dynInfo.BackStencilState),
					MinDepthBounds = dynInfo.DepthBounds.Min,
					MaxDepthBounds = dynInfo.DepthBounds.Max
				}),
				ColorBlendState = sp.Values(new VKPipelineColorBlendStateCreateInfo() {
					Type = VKStructureType.PipelineColorBlendStateCreateInfo,
					LogicOpEnable = gfxInfo.LogicOpEnable,
					LogicOp = Convert(dynInfo.LogicOp),
					AttachmentCount = (uint)gfxInfo.Attachments.Count,
					Attachments = sp.Values(Collection.ConvertAll(gfxInfo.Attachments, Convert)),
					BlendConstant = dynInfo.BlendConstant
				}),
				DynamicState = sp.Values(new VKPipelineDynamicStateCreateInfo() {
					Type = VKStructureType.PipelineDynamicStateCreateInfo,
					DynamicStateCount = (uint)states.Count,
					DynamicStates = pStates
				}),
				Layout = ((VulkanPipelineLayout)createInfo.Layout).Layout,
				RenderPass = ((VulkanRenderPass)gfxInfo.RenderPass).RenderPass,
				Subpass = gfxInfo.Subpass,
				BasePipelineHandle = (createInfo.BasePipeline is VulkanPipeline basePipeline) ? basePipeline.Pipeline : 0,
				BasePipelineIndex = createInfo.BasePipelineIndex.GetValueOrDefault(-1)
			};
		}

		public static VKImageSubresourceLayers Convert(TextureSubresourceLayers layers) => new() {
			AspectMask = Convert(layers.Aspects),
			BaseArrayLayer = layers.BaseArrayLayer,
			LayerCount = layers.LayerCount,
			MipLevel = layers.MipLevel
		};

		public static VKImageResolve ConvertImageResolve(ICommandSink.CopyTextureRegion region) => new() {
			DstOffset = (Vector3i)region.DstOffset,
			DstSubresource = Convert(region.DstSubresource),
			SrcOffset = (Vector3i)region.SrcOffset,
			SrcSubresource = Convert(region.SrcSubresource),
			Extent = region.Size
		};

		public static VKClearColorValue Convert(ICommandSink.ClearColorValue value) => new() { Float32 = value.AsFloat32 };

		public static VKClearValue Convert(ICommandSink.ClearValue value) {
			VKClearValue vkvalue = new();
			if ((value.Aspect & TextureAspect.Color) != 0) vkvalue.Color = Convert(value.Color);
			else vkvalue.DepthStencil = new() {
				Depth = value.Depth,
				Stencil = (uint)value.Stencil
			};
			return vkvalue;
		}

		public static VKRenderPassBeginInfo Convert(MemoryStack sp, ICommandSink.RenderPassBegin begin) {
			VulkanRenderPass renderPass = (VulkanRenderPass)begin.RenderPass;
			UnmanagedPointer<VKClearValue> clearValues = default;
			int clearValuesCount = begin.ClearValues.Count;
			if (clearValuesCount > 0) {
				clearValues = sp.Alloc<VKClearValue>(clearValuesCount);
				for (int i = 0; i < clearValuesCount; i++) clearValues[i] = Convert(begin.ClearValues[i]);
			}
			return new() {
				Type = VKStructureType.RenderPassBeginInfo,
				RenderPass = renderPass.RenderPass,
				RenderArea = begin.RenderArea,
				Framebuffer = ((VulkanFramebuffer)begin.Framebuffer).Framebuffer,
				ClearValueCount = (uint)begin.ClearValues.Count,
				ClearValues = clearValues
			};
		}

		public static VKBufferImageCopy Convert(ICommandSink.CopyBufferTexture copy) => new() {
			BufferOffset = copy.BufferOffset,
			BufferRowLength = copy.BufferRowLength,
			BufferImageHeight = copy.BufferImageHeight,
			ImageOffset = (Vector3i)copy.TextureOffset,
			ImageExtent = copy.TextureSize,
			ImageSubresource = Convert(copy.TextureSubresource)
		};

		public static VKImageCopy ConvertImageCopy(ICommandSink.CopyTextureRegion region) => new() {
			SrcOffset = (Vector3i)region.SrcOffset,
			SrcSubresource = Convert(region.SrcSubresource),
			DstOffset = (Vector3i)region.DstOffset,
			DstSubresource = Convert(region.DstSubresource),
			Extent = region.Size
		};

		public static VKBufferCopy Convert(ICommandSink.CopyBufferRegion region) => new() {
			SrcOffset = region.SrcOffset,
			DstOffset = region.DstOffset,
			Size = region.Length
		};

		public static VKClearAttachment Convert(ICommandSink.ClearAttachment attachment) => new() {
			AspectMask = Convert(attachment.Value.Aspect),
			ColorAttachment = (uint)attachment.Attachment,
			ClearValue = Convert(attachment.Value)
		};

		public static VKClearRect Convert(ICommandSink.ClearRect rect) => new() {
			Rect = rect.Rect,
			BaseArrayLayer = rect.BaseArrayLayer,
			LayerCount = rect.LayerCount
		};

		public static VKImageType ConvertImageType(TextureType type) => type switch {
			TextureType.Texture1D => VKImageType.Type1D,
			TextureType.Texture1DArray => VKImageType.Type1D,
			TextureType.Texture2D => VKImageType.Type2D,
			TextureType.Texture2DCube => VKImageType.Type2D,
			TextureType.Texture2DArray => VKImageType.Type2D,
			TextureType.Texture2DCubeArray => VKImageType.Type2D,
			TextureType.Texture3D => VKImageType.Type3D,
			_ => default
		};

		public static VKImageCreateFlagBits ConvertImageCreateFlags(TextureType type) {
			if (type == TextureType.Texture2DCube || type == TextureType.Texture2DCubeArray) return VKImageCreateFlagBits.CubeCompatible;
			return 0;
		}

		public static VKImageUsageFlagBits Convert(TextureUsage usage) {
			VKImageUsageFlagBits flags = 0;
			if ((usage & TextureUsage.ColorAttachment) != 0) flags |= VKImageUsageFlagBits.ColorAttachment;
			if ((usage & TextureUsage.DepthStencilAttachment) != 0) flags |= VKImageUsageFlagBits.DepthStencilAttachment;
			if ((usage & TextureUsage.InputAttachment) != 0) flags |= VKImageUsageFlagBits.InputAttachment;
			if ((usage & TextureUsage.Sampled) != 0) flags |= VKImageUsageFlagBits.Sampled;
			if ((usage & TextureUsage.Storage) != 0) flags |= VKImageUsageFlagBits.Storage;
			if ((usage & TextureUsage.TransferDst) != 0) flags |= VKImageUsageFlagBits.TransferDst;
			if ((usage & TextureUsage.TransferSrc) != 0) flags |= VKImageUsageFlagBits.TransferSrc;
			if ((usage & TextureUsage.TransientAttachment) != 0) flags |= VKImageUsageFlagBits.TransientAttachment;
			return flags;
		}

		public static TextureUsage Convert(VKImageUsageFlagBits usage) {
			TextureUsage flags = 0;
			if ((usage & VKImageUsageFlagBits.ColorAttachment) != 0) flags |= TextureUsage.ColorAttachment;
			if ((usage & VKImageUsageFlagBits.DepthStencilAttachment) != 0) flags |= TextureUsage.DepthStencilAttachment;
			if ((usage & VKImageUsageFlagBits.InputAttachment) != 0) flags |= TextureUsage.InputAttachment;
			if ((usage & VKImageUsageFlagBits.Sampled) != 0) flags |= TextureUsage.Sampled;
			if ((usage & VKImageUsageFlagBits.Storage) != 0) flags |= TextureUsage.Storage;
			if ((usage & VKImageUsageFlagBits.TransferDst) != 0) flags |= TextureUsage.TransferDst;
			if ((usage & VKImageUsageFlagBits.TransferSrc) != 0) flags |= TextureUsage.TransferSrc;
			if ((usage & VKImageUsageFlagBits.TransientAttachment) != 0) flags |= TextureUsage.TransientAttachment;
			return flags;
		}

		public static VKAttachmentLoadOp Convert(AttachmentLoadOp op) => op switch {
			AttachmentLoadOp.Load => VKAttachmentLoadOp.Load,
			AttachmentLoadOp.Clear => VKAttachmentLoadOp.Clear,
			AttachmentLoadOp.DontCare => VKAttachmentLoadOp.DontCare,
			_ => default
		};

		public static VKAttachmentStoreOp Convert(AttachmentStoreOp op) => op switch {
			AttachmentStoreOp.Store => VKAttachmentStoreOp.Store,
			AttachmentStoreOp.DontCare => VKAttachmentStoreOp.DontCare,
			_ => default
		};

		public static VKAttachmentDescription Convert(RenderPassAttachment attachment) => new() {
			Format = Convert(attachment.Format),
			InitialLayout = Convert(attachment.InitialLayout),
			FinalLayout = Convert(attachment.FinalLayout),
			LoadOp = Convert(attachment.LoadOp),
			StoreOp = Convert(attachment.StoreOp),
			StencilLoadOp = Convert(attachment.StencilLoadOp),
			StencilStoreOp = Convert(attachment.StencilStoreOp),
			Samples = ConvertSampleCount(attachment.Samples)
		};

		public static VKAttachmentReference Convert(RenderPassAttachmentReference refer) => new() {
			Attachment = refer.Attachment,
			Layout = Convert(refer.Layout)
		};

		public static VKPipelineBindPoint Convert(PipelineType type) => type switch {
			PipelineType.Graphics => VKPipelineBindPoint.Graphics,
			PipelineType.Compute => VKPipelineBindPoint.Compute,
			_ => default
		};

		public static VKSubpassDescription Convert(MemoryStack sp, RenderPassSubpass subpass) => new() {
			PipelineBindPoint = Convert(subpass.PipelineBindType),
			ColorAttachmentCount = (uint)(subpass.ColorAttachments != null ? subpass.ColorAttachments.Length : 0),
			ColorAttachments = subpass.ColorAttachments != null ? sp.Values(subpass.ColorAttachments.ConvertAll(Convert)) : IntPtr.Zero,
			DepthStencilAttachment = subpass.DepthStencilAttachment.HasValue ? sp.Values(Convert(subpass.DepthStencilAttachment.Value)) : IntPtr.Zero,
			InputAttachmentCount = (uint)(subpass.InputAttachments != null ? subpass.InputAttachments.Length : 0),
			InputAttachments = subpass.InputAttachments != null ? sp.Values(subpass.InputAttachments.ConvertAll(Convert)) : IntPtr.Zero,
			PreserveAttachmentCount = (uint)(subpass.PreserveAttachments != null ? subpass.PreserveAttachments.Length : 0),
			PreserveAttachments = subpass.PreserveAttachments != null ? sp.Values(subpass.PreserveAttachments.ConvertAll(Convert)) : IntPtr.Zero,
			ResolveAttachments = subpass.ResolveAttachments != null ? sp.Values(subpass.ResolveAttachments.ConvertAll(Convert)) : IntPtr.Zero
		};

		public static VKSubpassDependency Convert(RenderPassDependency dep) => new() {
			SrcAccessMask = Convert(dep.SrcAccess),
			DstAccessMask = Convert(dep.DstAccess),
			SrcStageMask = Convert(dep.SrcStages),
			DstStageMask = Convert(dep.DstStages),
			SrcSubpass = dep.SrcSubpass,
			DstSubpass = dep.DstSubpass
		};

		public static VKRenderPassCreateInfo Convert(MemoryStack sp, RenderPassCreateInfo createInfo) => new() {
			Type = VKStructureType.RenderPassCreateInfo,
			AttachmentCount = (uint)createInfo.Attachments.Count,
			Attachments = sp.Values(createInfo.Attachments.ConvertAll(Convert)),
			SubpassCount = (uint)createInfo.Subpasses.Count,
			Subpasses = sp.Values(createInfo.Subpasses.ConvertAll(subpass => Convert(sp, subpass))),
			DependencyCount = (uint)createInfo.Dependencies.Count,
			Dependencies = sp.Values(createInfo.Dependencies.ConvertAll(Convert))
		};

		public static VKPipelineShaderStageCreateInfo Convert(PipelineShaderStageInfo stage) => new() {
			Type = VKStructureType.PipelineShaderStageCreateInfo,
			Stage = Convert(stage.Type),
			Module = ((VulkanShader)stage.Shader).ShaderModule,
			Name = stage.EntryPoint
		};

		public static VKComputePipelineCreateInfo ConvertComputePipeline(PipelineCreateInfo createInfo) {
			PipelineComputeCreateInfo computeInfo = createInfo.ComputeInfo!;

			VKPipelineCreateFlagBits flags = VKPipelineCreateFlagBits.AllowDerivatives;
			if (createInfo.BasePipeline != null || createInfo.BasePipelineIndex.HasValue) flags |= VKPipelineCreateFlagBits.Derivative;

			return new VKComputePipelineCreateInfo() {
				Type = VKStructureType.ComputePipelineCreateInfo,
				Flags = flags,
				Stage = Convert(computeInfo.Shader),
				BasePipelineHandle = ((VulkanPipeline?)createInfo.BasePipeline)?.Pipeline,
				BasePipelineIndex = createInfo.BasePipelineIndex.GetValueOrDefault(-1),
				Layout = ((VulkanPipelineLayout)createInfo.Layout).Layout
			};
		}

		public static VKDescriptorType Convert(BindType type) => type switch {
			BindType.UniformBuffer => VKDescriptorType.UniformBuffer,
			BindType.StorageBuffer => VKDescriptorType.StorageBuffer,
			BindType.CombinedTextureSampler => VKDescriptorType.CombinedImageSampler,
			BindType.StorageTexture => VKDescriptorType.StorageImage,
			BindType.InputAttachment => VKDescriptorType.InputAttachment,
			_ => default
		};

		public static VKDescriptorImageInfo Convert(TextureBinding binding) => new() {
			Sampler = ((VulkanSampler)binding.Sampler)?.Sampler,
			ImageView = ((VulkanTextureView)binding.TextureView)?.ImageView,
			ImageLayout = Convert(binding.TexureLayout)
		};

		public static VKDescriptorBufferInfo Convert(BufferBinding binding) {
			MemoryRange range = binding.Range.Constrain(binding.Buffer.Size);
			return new() {
				Buffer = ((VulkanBuffer)binding.Buffer).Buffer,
				Offset = range.Offset,
				Range = range.Length
			};
		}

		public static VKWriteDescriptorSet Convert(MemoryStack sp, BindSetWrite write, VKDescriptorSet set) => new() {
			Type = VKStructureType.WriteDescriptorSet,
			DstSet = set,
			DstBinding = write.Binding,
			DstArrayElement = 0,
			DescriptorCount = 1,
			DescriptorType = Convert(write.Type),
			BufferInfo = sp.Values(Convert(write.BufferInfo)),
			ImageInfo = sp.Values(Convert(write.TextureInfo))
		};

		public static VKDescriptorSetLayoutBinding Convert(BindSetLayoutBinding binding) => new() {
			Binding = binding.Binding,
			DescriptorType = Convert(binding.Type),
			DescriptorCount = 1,
			StageFlags = Convert(binding.Stages)
		};

		public static VKPushConstantRange Convert(PushConstantRange range) => new() {
			StageFlags = Convert(range.Stages),
			Offset = range.Offset,
			Size = range.Size
		};

		public static SwapchainPresentMode Convert(VKPresentModeKHR mode) => mode switch {
			VKPresentModeKHR.FIFO => SwapchainPresentMode.FIFO,
			VKPresentModeKHR.FIFORelaxed => SwapchainPresentMode.RelaxedFIFO,
			VKPresentModeKHR.Immediate => SwapchainPresentMode.Immediate,
			VKPresentModeKHR.Mailbox => SwapchainPresentMode.Mailbox,
			_ => default
		};

		public static VKPresentModeKHR Convert(SwapchainPresentMode mode) => mode switch {
			SwapchainPresentMode.FIFO => VKPresentModeKHR.FIFO,
			SwapchainPresentMode.RelaxedFIFO => VKPresentModeKHR.FIFORelaxed,
			SwapchainPresentMode.Immediate => VKPresentModeKHR.Immediate,
			SwapchainPresentMode.Mailbox => VKPresentModeKHR.Mailbox,
			_ => default
		};

		public static VKResolveModeFlagBits Convert(ResolveMode mode) {
			VKResolveModeFlagBits vkmode = 0;
			if ((mode & ResolveMode.First) != 0) vkmode |= VKResolveModeFlagBits.SampleZero;
			if ((mode & (ResolveMode.Average | ResolveMode.Default)) != 0) vkmode |= VKResolveModeFlagBits.Average;
			if ((mode & ResolveMode.Min) != 0) vkmode |= VKResolveModeFlagBits.Min;
			if ((mode & ResolveMode.Max) != 0) vkmode |= VKResolveModeFlagBits.Max;
			return vkmode;
		}

		public static VKRenderingAttachmentInfo Convert(in ICommandSink.RenderingAttachmentInfo info) {
			return new VKRenderingAttachmentInfo() {
				Type = VKStructureType.RenderingAttachmentInfo,
				ImageView = ((VulkanTextureView)info.TextureView).ImageView.ImageView,
				ImageLayout = Convert(info.TextureLayout),
				ResolveMode = Convert(info.ResolveMode),
				ResolveImageView = ((VulkanTextureView?)info.ResolveTextureView)?.ImageView?.ImageView ?? 0,
				ResolveImageLayout = Convert(info.ResolveTextureLayout),
				LoadOp = Convert(info.LoadOp),
				StoreOp = Convert(info.StoreOp),
				ClearValue = Convert(info.ClearValue)
			};
		}

		public static VKVertexInputBindingDescription2EXT Convert(VertexBinding binding) {
			return new VKVertexInputBindingDescription2EXT() {
				Type = VKStructureType.VertexInputBindingDescription2EXT,
				Binding = binding.Binding,
				InputRate = Convert(binding.InputRate),
				Stride = binding.Stride,
				Divisor = 1
			};
		}

		public static VKVertexInputAttributeDescription2EXT Convert(VertexAttrib attrib) {
			return new VKVertexInputAttributeDescription2EXT() {
				Type = VKStructureType.VertexInputAttributeDescription2EXT,
				Binding = attrib.Binding,
				Location = attrib.Location,
				Format = Convert(attrib.Format),
				Offset = attrib.Offset
			};
		}

	}

}

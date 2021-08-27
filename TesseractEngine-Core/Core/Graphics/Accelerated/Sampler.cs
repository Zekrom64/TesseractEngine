using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// Enumeration of sampler addressing modes.
	/// </summary>
	public enum SamplerAddressMode {
		/// <summary>
		/// Samples outside of the range [0,1] will be clamped to coordinates inside this range by wrapping around at the limits of 0 and 1.
		/// This effectively repeats the texture for values outside the normalized range.
		/// </summary>
		Repeat,
		/// <summary>
		/// Similar to <see cref="Repeat"/>, but every odd wrap around will mirror the coordinates in the regular texture space.
		/// </summary>
		MirroredRepeat,
		/// <summary>
		/// Samples outside of the range [0,1] will be clamped to coordinates inside this range (at the limits of 0 and 1). This
		/// effectively samples the color at the closest edge of the texture.
		/// </summary>
		ClampToEdge,
		/// <summary>
		/// Samples outside of the range [0,1] will return a sampler-defined border color.
		/// </summary>
		ClampToBorder,
		/// <summary>
		/// Similar to <see cref="ClampToEdge"/>, but instead clamps in the range [-1, 1], where negative coordinates will sample
		/// its mirrored equivalent position in the positive space.
		/// </summary>
		MirrorClampToEdge
	}

	/// <summary>
	/// Enumeration of sampler border color types.
	/// </summary>
	public enum SamplerBorderColor {
		/// <summary>
		/// Transparent black, with a normalized value of (0.0, 0.0, 0.0, 0.0).
		/// </summary>
		TransparentBlack,
		/// <summary>
		/// Opaque black, with a normalized value of (0.0, 0.0, 0.0, 1.0).
		/// </summary>
		OpaqueBlack,
		/// <summary>
		/// Opaque white, with a normalized value of (1.0, 1.0, 1.0, 1.0).
		/// </summary>
		OpaqueWhite,
		/// <summary>
		/// A custom border color defined during sampler creation.
		/// </summary>
		Custom
	}
	
	/// <summary>
	/// <para>A sampler controls how texels are actually fetched from a texture.</para>
	/// <para>
	/// Some functionality provided by samplers:
	/// <list type="bullet">
	/// <item><b>Filtering</b><para>
	/// Filtering determines how texels are sampled at different scales. The magnification filter is applied
	/// when the texture is being sampled at a scale larger than its native size, and the minification filter
	/// is applied when the texture is sampled a a smaller scale. Samplers may also apply anisotropic
	/// filtering if supported.
	/// </para></item>
	/// <item><b>Address Modes</b><para>
	/// Addressing modes control how coordinates outside of the [0,1] range are handled. They can
	/// apply repetition, mirroring, or clamping to the coordinates. Clamping can be done to the edge
	/// of the texture or to a 'border' color. The border color can be transparent black, opaque black
	/// or white, or a custom color if supported.
	/// </para></item>
	/// </list>
	/// </para>
	/// </summary>
	public interface ISampler : IDisposable { }

	/// <summary>
	/// Sampler creation information.
	/// </summary>
	public record SamplerCreateInfo {

		/// <summary>
		/// The pixel format of the textures the sampler will be used with. If <see cref="IGraphicsFeatures.SamplerNoFormat"/>
		/// is set this value may be null.
		/// </summary>
		public PixelFormat Format { get; init; } = null;

		/// <summary>
		/// The magnification filter to use during sampling.
		/// </summary>
		public TextureFilter MagnifyFilter { get; init; } = TextureFilter.Nearest;

		/// <summary>
		/// The minification filter to use during sampling.
		/// </summary>
		public TextureFilter MinifyFilter { get; init; } = TextureFilter.Nearest;

		/// <summary>
		/// The filtering to apply when sampling between mipmaps.
		/// </summary>
		public TextureFilter MipmapMode { get; init; } = TextureFilter.Nearest;

		/// <summary>
		/// The address modes to use for each dimension of the texture.
		/// </summary>
		public Tuple3<SamplerAddressMode> AddressMode { get; init; } = new(SamplerAddressMode.Repeat);

		/// <summary>
		/// The bias to add to level-of-detail values when selecting mipmap levels.
		/// </summary>
		public float MipLODBias { get; init; } = 0.0f;

		/// <summary>
		/// If anisotropic filtering should be applied during sampling.
		/// </summary>
		public bool AnisotropyEnable { get; init; } = false;

		/// <summary>
		/// The maximum level of anisotropy to use if enabled.
		/// </summary>
		public float MaxAnisotropy { get; init; } = 0.0f;

		/// <summary>
		/// If the sampler operates in comparison mode, for use with depth textures.
		/// </summary>
		public bool CompareEnable { get; init; } = false;

		/// <summary>
		/// The compare operation to perform, if enabled.
		/// </summary>
		public CompareOp CompareOp { get; init; } = default;

		/// <summary>
		/// The range of values that level-of-detail values will be clamped between.
		/// </summary>
		public (float, float) LODRange { get; init; } = (0.0f, 1000.0f);

		/// <summary>
		/// The border color to use for sampling operations.
		/// </summary>
		public SamplerBorderColor BorderColor { get; init; } = SamplerBorderColor.TransparentBlack;

		/// <summary>
		/// The custom border color of the sampler if <see cref="BorderColor"/> is set to
		/// <see cref="SamplerBorderColor.Custom"/>. The format of the color (signed/unsigned
		/// integer, float) is interpreted from the type provided. The value must be an
		/// instance of <see cref="Vector4"/> or <see cref="ITuple4{T}"/>.
		/// </summary>
		public object CustomBorderColor { get; init; } = null;

	}

}

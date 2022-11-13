using System;
using Tesseract.Core.Numerics;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// A viewport specifies the region of 3D space that will be captured during rasterization. This is done
	/// using a rectangle describing the 2D area in the X and Y axes, and a pair of depth bounds values
	/// describing the area in the Z axis.
	/// </summary>
	public record struct Viewport {

		// Note for OpenGL: glViewport takes ints in pixel coordinates, this can be computed knowing the current FB size and converting area based on NDC

		/// <summary>
		/// A rectangle describing the 2D area that will be captured by the framebuffer.
		/// </summary>
		public required Rectf Area { get; set; }

		/// <summary>
		/// The minimum and maximum depth bounds values.
		/// </summary>
		public required (float Min, float Max) DepthBounds { get; set; }

	}

	/// <summary>
	/// The polygon mode describes how polygons (triangles) are rasterized.
	/// </summary>
	public enum PolygonMode {
		/// <summary>
		/// Fragments are generated for each sample inside the bounds of the polygon.
		/// </summary>
		Fill,
		/// <summary>
		/// Lines are drawn between each vertex of the polygon's triangles.
		/// </summary>
		Line,
		/// <summary>
		/// Points are drawn at eachvertex of the polygon.
		/// </summary>
		Point
	}

	/// <summary>
	/// The culling mode determines if geometry can be discarded based on the direction it is facing, either "front" or "back" facing.
	/// </summary>
	[Flags]
	public enum CullFace {
		/// <summary>
		/// All geometry is kept.
		/// </summary>
		None = 0,
		/// <summary>
		/// Front-facing geometry is discarded, back-facing geometry is kept.
		/// </summary>
		Front = 0x01,
		/// <summary>
		/// Back-facing geometry is discarded, front-facing geometry is kept.
		/// </summary>
		Back = 0x02,
		/// <summary>
		/// Front and back-facing geometry is discarded, the same as if rasterizer discard was enabled.
		/// </summary>
		FrontAndBack = Front | Back
	}

	/// <summary>
	/// The front face for culling is determined based on vertex order, either "clockwise" or "counter-clockwise".
	/// </summary>
	public enum FrontFace {
		/// <summary>
		/// A polygon is facing "forwards" if the vertices are drawn in order "clockwise".
		/// </summary>
		Clockwise,
		/// <summary>
		/// A polygon is facing "forwards" if the vertices are drawn in order "counter-clockwise".
		/// </summary>
		CounterClockwise
	}

	/// <summary>
	/// A compare op describes how two values are compared, producing a boolean value.
	/// </summary>
	public enum CompareOp {
		/// <summary>
		/// The comparison will always be false.
		/// </summary>
		Never,
		/// <summary>
		/// If the target value is less than a reference value.
		/// </summary>
		Less,
		/// <summary>
		/// If the target value is equal to a reference value.
		/// </summary>
		Equal,
		/// <summary>
		/// If the target value is less or equal to a reference value.
		/// </summary>
		LessOrEqual,
		/// <summary>
		/// If the target value is greater than a reference value.
		/// </summary>
		Greater,
		/// <summary>
		/// If the target value is not equal to a reference value.
		/// </summary>
		NotEqual,
		/// <summary>
		/// If the target value is greater or equal to a reference value.
		/// </summary>
		GreaterOrEqual,
		/// <summary>
		/// The comparison will always be true.
		/// </summary>
		Always
	}

	/// <summary>
	/// A stencil op describes how stencil values are modified based under certain conditions.
	/// </summary>
	public enum StencilOp {
		/// <summary>
		/// The value currently in the stencil buffer is kept.
		/// </summary>
		Keep,
		/// <summary>
		/// The stencil value is cleared to zero.
		/// </summary>
		Zero,
		/// <summary>
		/// The stencil value is set to a supplied reference value.
		/// </summary>
		Replace,
		/// <summary>
		/// The stencil value is incremented if it is lower than the maximum value.
		/// </summary>
		IncrementAndClamp,
		/// <summary>
		/// The stencil value is decremented if it is higher than the minimum value.
		/// </summary>
		DecrementAndClamp,
		/// <summary>
		/// The stencil value is inverted bitwise.
		/// </summary>
		Invert,
		/// <summary>
		/// The stencil value is incremented, wrapping from the maximum value to the minimum value.
		/// </summary>
		IncrementAndWrap,
		/// <summary>
		/// The stencil value is decremented, wrapping from the minimum value to the maximum value.
		/// </summary>
		DecrementAndWrap
	}

	/// <summary>
	/// A logic op describes a logical operation that is optionally performed in lieu of color blending.
	/// </summary>
	public enum LogicOp {
		/// <summary>
		/// A value with all bits clear is written.
		/// </summary>
		Clear,
		/// <summary>
		/// The bitwise AND of the source and destination is written.
		/// </summary>
		And,
		/// <summary>
		/// The bitwise AND of the source and inverted destination is written.
		/// </summary>
		AndReverse,
		/// <summary>
		/// The source is written.
		/// </summary>
		Copy,
		/// <summary>
		/// The bitwise AND of the inverted source and destination is written.
		/// </summary>
		AndInverted,
		/// <summary>
		/// The destination is left unmodified.
		/// </summary>
		NoOp,
		/// <summary>
		/// The bitwise XOR of the source and destination is written.
		/// </summary>
		Xor,
		/// <summary>
		/// The bitwise OR of the source and destination is written.
		/// </summary>
		Or,
		/// <summary>
		/// The bitwise NOR of the source and destination is written.
		/// </summary>
		Nor,
		/// <summary>
		/// The bitwise XNOR of the source and destination is written.
		/// </summary>
		XNor,
		/// <summary>
		/// The destination is inverted.
		/// </summary>
		Invert,
		/// <summary>
		/// The bitwise OR of the source and inverted destination is written.
		/// </summary>
		OrReverse,
		/// <summary>
		/// The inverted source is written.
		/// </summary>
		CopyInverted,
		/// <summary>
		/// The bitwise OR of the inverted source and destination is written.
		/// </summary>
		OrInverted,
		/// <summary>
		/// The bitwise NAND of the source and destination is written.
		/// </summary>
		Nand,
		/// <summary>
		/// A value with all bits set is written.
		/// </summary>
		Set
	}

	/// <summary>
	/// A blend factor is multiplied into a source or destination value before color blending. The blend factors
	/// may be specified independently for the color and alpha channels.
	/// </summary>
	public enum BlendFactor {
		/// <summary>
		/// The factor values are all 0.0.
		/// </summary>
		Zero,
		/// <summary>
		/// The factor values are all 1.0.
		/// </summary>
		One,
		/// <summary>
		/// The factor values are loaded from the source color, i.e. (Rs, Gs, Bs, As).
		/// </summary>
		SrcColor,
		/// <summary>
		/// The factor values are set to 1.0 minus the values from the source color, i.e. (1-Rs, 1-Gs, 1-Bs, 1-As).
		/// </summary>
		OneMinusSrcColor,
		/// <summary>
		/// The factor values are loaded from the destination color, i.e. (Rd, Gd, Bd, Ad).
		/// </summary>
		DstColor,
		/// <summary>
		/// The factor values are set to 1.0 minus the values from the destination color, i.e. (1-Rd, 1-Gd, 1-Bd, 1-Ad).
		/// </summary>
		OneMinusDstColor,
		/// <summary>
		/// The factor values are loaded from the source color's alpha channel, i.e. (As, As, As, As).
		/// </summary>
		SrcAlpha,
		/// <summary>
		/// The factor values are set to 1.0 minus the values from the source color's alpha channel, i.e. (1-As, 1-As, 1-As, 1-As).
		/// </summary>
		OneMinusSrcAlpha,
		/// <summary>
		/// The factor values are loaded from the destination color's alpha channel, i.e. (Ad, Ad, Ad, Ad).
		/// </summary>
		DstAlpha,
		/// <summary>
		/// The factor values are set to 1.0 minus the values from the destination color's alpha channel, i.e. (1-Ad, 1-Ad, 1-Ad, 1-Ad).
		/// </summary>
		OneMinusDstAlpha,
		/// <summary>
		/// The factor values are loaded from the constant color, i.e. (Rc, Gc, Bc, Ac).
		/// </summary>
		ConstantColor,
		/// <summary>
		/// The factor values are set to 1.0 minus the values from the constant color, i.e. (1-Rc, 1-Gc, 1-Bc, 1-Ac).
		/// </summary>
		OneMinusConstantColor,
		/// <summary>
		/// The factor values are loaded from the constant alpha channel, i.e. (Ac, Ac, Ac, Ac).
		/// </summary>
		ConstantAlpha,
		/// <summary>
		/// The factor values are set to 1.0 minus the values from the constant alpha, i.e. (1-Ac, 1-Ac, 1-Ac, 1-Ac).
		/// </summary>
		OneMinusConstantAlpha,
		/// <summary>
		/// The factor values are loaded from the secondary source color, i.e. (Rs1, Gs1, Bs1, As1).
		/// </summary>
		Src1Color,
		/// <summary>
		/// The factor values are set to 1.0 minus the values from the secondary source color, i.e. (1-Rs1, 1-Gs1, 1-Bs1, 1-As1).
		/// </summary>
		OneMinusSrc1Color,
		/// <summary>
		/// The factor values are loaded from the secondary source color's alpha channel, i.e. (As1, As1, As1, As1).
		/// </summary>
		Src1Alpha,
		/// <summary>
		/// The factor values are set to 1.0 minus the values from the secondary source color's alpha channel, i.e. (1-As1, 1-As1, 1-As1, 1-As1).
		/// </summary>
		OneMinusSrc1Alpha
	}

	/// <summary>
	/// A blend equation combines settings for blend factors and operations for both RGB and alpha components
	/// to create a complete blending function. The equivalent equation is:
	/// <para>
	/// <c>D = (S * s) &lt;op&gt; (D * d)</c>
	/// </para>
	/// Where D and S are the destination and source color values and 'd' and 's' are the destination and source
	/// blend factors.
	/// </summary>
	public record struct BlendEquation {

		/// <summary>
		/// Implements a passthrough blend equation, i.e. <c>D = S</c>
		/// </summary>
		public static readonly BlendEquation Passthrough = new() {
			SrcRGB = BlendFactor.One,
			DstRGB = BlendFactor.Zero,
			RGBOp = BlendOp.Add,
			SrcAlpha = BlendFactor.One,
			DstAlpha = BlendFactor.Zero,
			AlphaOp = BlendOp.Add
		};

		/// <summary>
		/// Implements an additive blend equation, i.e. <c>D = S + D</c>
		/// </summary>
		public static readonly BlendEquation AddBlend = new() {
			SrcRGB = BlendFactor.One,
			DstRGB = BlendFactor.One,
			RGBOp = BlendOp.Add,
			SrcAlpha = BlendFactor.One,
			DstAlpha = BlendFactor.One,
			AlphaOp = BlendOp.Add
		};

		/// <summary>
		/// Implements a multiplicative blend equation, i.e. <c>D = S * D</c>
		/// </summary>
		public static readonly BlendEquation MultBlend = new() {
			SrcRGB = BlendFactor.DstColor,
			DstRGB = BlendFactor.Zero,
			RGBOp = BlendOp.Add,
			SrcAlpha = BlendFactor.DstColor,
			DstAlpha = BlendFactor.Zero,
			AlphaOp = BlendOp.Add
		};

		/// <summary>
		/// Implements an alpha blend equation, i.e.:
		/// <para>
		/// <c>D[RGB] = (S[RGB] * (1-D[A])) + (D[RGB] * D[A])</c><br/>
		/// <c>D[A] = max(S[A], D[A])</c>
		/// </para>
		/// </summary>
		public static readonly BlendEquation AlphaBlend = new() {
			SrcRGB = BlendFactor.OneMinusDstAlpha,
			DstRGB = BlendFactor.DstAlpha,
			RGBOp = BlendOp.Add,
			SrcAlpha = BlendFactor.One,
			DstAlpha = BlendFactor.One,
			AlphaOp = BlendOp.Max
		};

		/// <summary>
		/// The blending factor for the source RGB channels.
		/// </summary>
		public BlendFactor SrcRGB;

		/// <summary>
		/// The blending factor for the destination RGB channels.
		/// </summary>
		public BlendFactor DstRGB;

		/// <summary>
		/// The blend operation to apply to the RGB channels.
		/// </summary>
		public BlendOp RGBOp;

		/// <summary>
		/// The blending factor for the source alpha channel.
		/// </summary>
		public BlendFactor SrcAlpha;

		/// <summary>
		/// The blending factor for the destination alpha channel.
		/// </summary>
		public BlendFactor DstAlpha;

		/// <summary>
		/// The blend operation to apply to the alpha channel.
		/// </summary>
		public BlendOp AlphaOp;

	}

	/// <summary>
	/// A blend operation describes how components are combined during color blending.
	/// </summary>
	public enum BlendOp {
		/// <summary>
		/// The source value is added to the destination.
		/// </summary>
		Add,
		/// <summary>
		/// The destination value is subtracted from the source.
		/// </summary>
		Subtract,
		/// <summary>
		/// The source value is subtracted from the destination.
		/// </summary>
		ReverseSubtract,
		/// <summary>
		/// The minimum of the source and destination is used.
		/// </summary>
		Min,
		/// <summary>
		/// The maximum of the source and destination is used.
		/// </summary>
		Max
	}

	/// <summary>
	/// A color component specifies the channels of an RGBA color value.
	/// </summary>
	[Flags]
	public enum ColorComponent {
		/// <summary>
		/// Red color component.
		/// </summary>
		Red = 0x01,
		/// <summary>
		/// Green color component.
		/// </summary>
		Green = 0x02,
		/// <summary>
		/// Blue color component.
		/// </summary>
		Blue = 0x04,
		/// <summary>
		/// Alpha color component.
		/// </summary>
		Alpha = 0x08,

		/// <summary>
		/// Convenience field specifying all components.
		/// </summary>
		All = Red | Green | Blue | Alpha
	}

	/// <summary>
	/// A component swizzle specifies how a color channel component is mapped.
	/// </summary>
	public enum ComponentSwizzle {
		/// <summary>
		/// The color component is mapped to the identity swizzle (ie. it will map to whatever the same component is).
		/// </summary>
		Identity,
		/// <summary>
		/// The color component is mapped to a constant zero.
		/// </summary>
		Zero,
		/// <summary>
		/// The color component is mapped to a constant one.
		/// </summary>
		One,
		/// <summary>
		/// The color component is mapped to the red component.
		/// </summary>
		Red,
		/// <summary>
		/// The color component is mapped to the green component.
		/// </summary>
		Green,
		/// <summary>
		/// The color component is mapped to the blue component.
		/// </summary>
		Blue,
		/// <summary>
		/// The color component is mapped to the alpha component.
		/// </summary>
		Alpha
	}

	/// <summary>
	/// A component mapping provides a mapping for color components based on component swizzles.
	/// </summary>
	public struct ComponentMapping {

		/// <summary>
		/// The component mapping for the red component.
		/// </summary>
		public ComponentSwizzle Red = ComponentSwizzle.Identity;

		/// <summary>
		/// The component mapping for the green component.
		/// </summary>
		public ComponentSwizzle Green = ComponentSwizzle.Identity;

		/// <summary>
		/// The component mapping for the blue component.
		/// </summary>
		public ComponentSwizzle Blue = ComponentSwizzle.Identity;

		/// <summary>
		/// The component mapping for the alpha component.
		/// </summary>
		public ComponentSwizzle Alpha = ComponentSwizzle.Identity;

		/// <summary>
		/// If the mapping is an identity mapping (ie. every component maps to itself).
		/// </summary>
		public bool IsIdentity =>
			(Red == ComponentSwizzle.Identity || Red == ComponentSwizzle.Red) &&
			(Green == ComponentSwizzle.Identity || Green == ComponentSwizzle.Green) &&
			(Blue == ComponentSwizzle.Identity || Blue == ComponentSwizzle.Blue) &&
			(Alpha == ComponentSwizzle.Identity || Alpha == ComponentSwizzle.Alpha);

		public ComponentMapping() { }

	}

	/// <summary>
	/// Enumeration of coordinate systems used for graphics.
	/// </summary>
	public enum CoordinateSystem {
		/// <summary>
		/// "Left-handed", ie. positive Y is towards the top.
		/// </summary>
		LeftHanded,
		/// <summary>
		/// "Right-handed, ie. positive Y is towards the bottom.
		/// </summary>
		RightHanded
	}

	/// <summary>
	/// Enumeration of multisample resolution modes.
	/// </summary>
	[Flags]
	public enum ResolveMode : uint {
		/// <summary>
		/// No resolution is done.
		/// </summary>
		None = 0,
		/// <summary>
		/// The "first" sample for a single resolved pixel is used for the resulting value.
		/// </summary>
		First = 0x1,
		/// <summary>
		/// The average of all samples is used.
		/// </summary>
		Average = 0x2,
		/// <summary>
		/// The minimum of all samples is used.
		/// </summary>
		Min = 0x4,
		/// <summary>
		/// The maximum of all samples is used.
		/// </summary>
		Max = 0x8,
		/// <summary>
		/// The default backend-specific resolution mode is used.
		/// </summary>
		Default = 0x80000000
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;

namespace Tesseract.Core.Graphics.Accelerated {
	
	public struct Viewport {

		public Rectf Area;

		public float MinDepth;

		public float MaxDepth;

	}

	public enum PolygonMode {
		Fill,
		Line,
		Point
	}

	public enum CullMode {
		Front = 0x01,
		Back = 0x02,
		FrontAndBack = Front | Back
	}

	public enum FrontFace {
		Clockwise,
		CounterClockwise
	}

	public enum CompareOp {
		Never,
		Less,
		Equal,
		LessOrEqual,
		Greater,
		NotEqual,
		GreaterOrEqual,
		Always
	}

	public enum StencilOp {
		Keep,
		Zero,
		Replace,
		IncrementAndClamp,
		DecrementAndClamp,
		Invert,
		IncrementAndWrap,
		DecrementAndWrap
	}

	public enum LogicOp {
		Clear,
		And,
		AndReverse,
		Copy,
		AndInverted,
		NoOp,
		Xor,
		Or,
		Nor,
		XNor,
		Invert,
		OrReverse,
		CopyInverted,
		OrInverted,
		Nand,
		Set
	}

	public enum BlendFactor {
		Zero,
		One,
		SrcColor,
		OneMinusSrcColor,
		DstColor,
		OneMinusDstColor,
		SrcAlpha,
		OneMinusSrcAlpha,
		DstAlpha,
		OneMinusDstAlpha,
		ConstantColor,
		OneMinusConstantColor,
		ConstantAlpha,
		OneMinusConstantAlpha,
		Src1Color,
		OneMinusSrc1Color,
		Src1Alpha,
		OneMinusSrc1Alpha
	}

	public enum BlendOp {
		Add,
		Subtract,
		ReverseSubtract,
		Min,
		Max
	}

	public enum ColorComponent {
		Red = 0x01,
		Green = 0x02,
		Blue = 0x04,
		Alpha = 0x08
	}

}

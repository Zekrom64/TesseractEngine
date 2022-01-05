using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Math;

namespace Tesseract.OpenGL.Graphics {

	public class GLPipeline : IPipeline {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		//==============//
		// Static State //
		//==============//

		// Shader state

		public uint ShaderProgramID { get; }

		// Rasterization state

		public bool DepthClampEnable { get; }

		public GLPolygonMode PolygonMode { get; }

		// Color blend state

		public bool LogicOpEnable { get; }

		public struct ColorAttachmentState {

			public bool BlendEnable { get; init; }

			public GLBlendFactor SrcRGB { get; }

			public GLBlendFactor DstRGB { get; }

			public GLBlendFunction RGBOp { get; }

			public GLBlendFactor SrcAlpha { get; }

			public GLBlendFactor DstAlpha { get; }

			public GLBlendFunction AlphaOp { get; }

			public bool ColorWriteMaskR { get; }

			public bool ColorWriteMaskG { get; }

			public bool ColorWriteMaskB { get; }

			public bool ColorWriteMaskA { get; }

		}

		public ColorAttachmentState[] Attachments { get; }

		//===============//
		// Dynmaic state //
		//===============//

		// Input assembly state

		public GLDrawMode? DrawMode { get; }

		public bool? PrimitiveRestartEnable { get; }

		// Tessellation state

		public uint? PatchControlPoints { get; }

		// Viewport state

		public Viewport[] Viewports { get; }

		public Recti[] Scissors { get; }

		// Rasterization state

		public bool? RasterizerDiscardEnable { get; }

		public GLCullFace? CullMode { get; }
		
		public GLFace? FrontFace { get; }

		public float? LineWidth { get; }

		public bool? DepthBiasEnable { get; }

		public float? DepthBiasConstantFactor { get; }

		public float? DepthBiasClamp { get; }

		public float? DepthBiasSlopeFactor { get; }

		// Depth/stencil state

		public bool? DepthTestEnable { get; }

		public bool? DepthWriteEnable { get; }

		public GLCompareFunc? DepthCompareOp { get; }

		public bool? DepthBoundsTestEnable { get; }

		public bool? StencilTestEnable { get; }

		public struct StencilState {

			public GLStencilOp FailOp { get; }

			public GLStencilOp PassOp { get; }

			public GLStencilOp DepthFailOp { get; }

			public GLStencilFunc CompareOp { get; }

			public uint CompareMask { get; }

			public uint WriteMask { get; }

			public uint Reference { get; }

		}

		public StencilState? FrontStencilState { get; }

		public StencilState? BackStencilState { get; }

		public GLPipeline(GLGraphics graphics, PipelineCreateInfo createInfo) {

		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			GL.GL33!.DeleteProgram(ShaderProgramID);
		}

	}

}

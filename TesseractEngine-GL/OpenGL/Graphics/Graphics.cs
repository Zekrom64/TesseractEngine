using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Util;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL.Graphics {

	public class GLGraphics : IGraphics {

		public IGraphicsProperites Properties => Provider.Properties;

		public IGraphicsFeatures Features { get; }

		public IGraphicsLimits Limits => Provider.Limits;

		public IGLContext Context => Provider.Context;

		public GL GL => Provider.GL;

		public GLGraphicsProvider Provider { get; }

		public GLState State { get; }

		public GLInterface Interface { get; }

		private readonly GLCommandSink immediateCommandSink;

		public GLGraphics(GLGraphicsProvider provider, GraphicsCreateInfo createInfo) {
			Provider = provider;
			State = new GLState(this);
			Interface = new GLInterface(this);

			GraphicsHardwareFeatures? hwfeatures = createInfo.EnabledFeatures;
			if (hwfeatures != null) hwfeatures = hwfeatures.Mask(Provider.Features.HardwareFeatures);
			Features = new GLGraphicsFeatures(GL, hwfeatures);

			immediateCommandSink = new GLCommandSink(this);
		}

		public IBuffer CreateBuffer(BufferCreateInfo createInfo) => new GLBuffer(this, createInfo);

		public ICommandBuffer CreateCommandBuffer(CommandBufferCreateInfo createInfo) {
			// TODO
			throw new NotImplementedException();
		}

		public IPipeline CreatePipeline(PipelineCreateInfo createInfo) => new GLPipeline(this, createInfo);

		public IPipelineCache CreatePipelineCache(PipelineCacheCreateInfo createInfo) => new GLPipelineCache(this, createInfo);

		public IPipelineLayout CreatePipelineLayout(PipelineLayoutCreateInfo createInfo) {
			// TODO
			throw new NotImplementedException();
		}

		public ITexture CreateTexture(TextureCreateInfo createInfo) => new GLTexture(this, createInfo);

		public ITextureView CreateTextureView(TextureViewCreateInfo createInfo) => new GLTextureView(this, createInfo);

		public ISampler CreateSampler(SamplerCreateInfo createInfo) => new GLSampler(this, createInfo);

		public IShader CreateShader(ShaderCreateInfo createInfo) => new GLShader(this, createInfo);

		public IPipelineSet CreatePipelineSet(PipelineSetCreateInfo createInfo) {
			// TODO
			throw new NotImplementedException();
		}

		public IRenderPass CreateRenderPass(RenderPassCreateInfo createInfo) {
			// TODO
			throw new NotImplementedException();
		}

		public IFramebuffer CreateFramebuffer(FramebufferCreateInfo createInfo) => new GLFramebuffer(this, createInfo);

		public ISync CreateSync(SyncCreateInfo createInfo) {
			// TODO
			throw new NotImplementedException();
		}

		public IVertexArray CreateVertexArray(VertexArrayCreateInfo createInfo) => new GLVertexArray(this, createInfo);

		public IBindSetLayout CreateBindSetLayout(BindSetLayoutCreateInfo createInfo) {
			// TODO
			throw new NotImplementedException();
		}

		public IBindPool CreateBindPool(BindPoolCreateInfo createInfo) {
			// TODO
			throw new NotImplementedException();
		}

		public void SubmitCommands(in IGraphics.CommandBufferSubmitInfo submitInfo) {
			// TODO
			throw new NotImplementedException();
		}

		public void TrimCommandBufferMemory() { } // No-op

		public void RunCommands(Action<ICommandSink> cmdSink, CommandBufferUsage usage, in IGraphics.CommandBufferSubmitInfo submitInfo) {
			// TODO: Handle command syncing
			cmdSink(immediateCommandSink);
		}

		public void WaitIdle() {
			GL.GL11.Finish();
		}
	}

}

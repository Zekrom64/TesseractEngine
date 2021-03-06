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

		public ICommandBuffer CreateCommandBuffer(CommandBufferCreateInfo createInfo) => new GLCommandBuffer(this, createInfo);

		public IPipeline CreatePipeline(PipelineCreateInfo createInfo) => new GLPipeline(this, createInfo);

		public IPipelineCache CreatePipelineCache(PipelineCacheCreateInfo createInfo) => new GLPipelineCache(this, createInfo);

		public IPipelineLayout CreatePipelineLayout(PipelineLayoutCreateInfo createInfo) => new GLPipelineLayout();

		public ITexture CreateTexture(TextureCreateInfo createInfo) => new GLTexture(this, createInfo);

		public ITextureView CreateTextureView(TextureViewCreateInfo createInfo) => new GLTextureView(this, createInfo);

		public ISampler CreateSampler(SamplerCreateInfo createInfo) => new GLSampler(this, createInfo);

		public IShader CreateShader(ShaderCreateInfo createInfo) => new GLShader(this, createInfo);

		public IPipelineSet CreatePipelineSet(PipelineSetCreateInfo createInfo) => new GLPipelineSet(this, createInfo);

		public IRenderPass CreateRenderPass(RenderPassCreateInfo createInfo) => new GLRenderPass(this, createInfo);

		public IFramebuffer CreateFramebuffer(FramebufferCreateInfo createInfo) => new GLFramebuffer(this, createInfo);

		public ISync CreateSync(SyncCreateInfo createInfo) => new GLSync(this, createInfo);

		public IVertexArray CreateVertexArray(VertexArrayCreateInfo createInfo) => new GLVertexArray(this, createInfo);

		public IBindSetLayout CreateBindSetLayout(BindSetLayoutCreateInfo createInfo) => new GLBindSetLayout();

		public IBindPool CreateBindPool(BindPoolCreateInfo createInfo) {
			// TODO
			throw new NotImplementedException();
		}

		public void SubmitCommands(in IGraphics.CommandBufferSubmitInfo submitInfo) {
			foreach (var sync in submitInfo.WaitSync)
				if (sync.Item1 is GLSync glsync && glsync.IsFence) glsync.HostWait(ulong.MaxValue);
			foreach (ICommandBuffer buffer in submitInfo.CommandBuffer)
				if (buffer is GLCommandBuffer glbuffer) glbuffer.RunCommands();
			foreach (var sync in submitInfo.SignalSync)
				if (sync is GLSync glsync && glsync.IsFence) glsync.GenerateFence();
			throw new NotImplementedException();
		}

		public void TrimCommandBufferMemory() { } // No-op

		public void RunCommands(Action<ICommandSink> cmdSink, CommandBufferUsage usage, in IGraphics.CommandBufferSubmitInfo submitInfo) {
			foreach (var sync in submitInfo.WaitSync)
				if (sync.Item1 is GLSync glsync && glsync.IsFence) glsync.HostWait(ulong.MaxValue);
			cmdSink(immediateCommandSink);
			foreach (var sync in submitInfo.SignalSync)
				if (sync is GLSync glsync && glsync.IsFence) glsync.GenerateFence();
		}

		public void WaitIdle() {
			GL.GL11.Finish();
		}
	}

}

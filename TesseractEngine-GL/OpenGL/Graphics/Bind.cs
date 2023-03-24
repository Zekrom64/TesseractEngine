using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {

	public class GLBindSetLayout : IBindSetLayout {

		public IReadOnlyList<BindSetLayoutBinding> Bindings { get; }

		public GLBindSetLayout(BindSetLayoutCreateInfo createInfo) {
			Bindings = createInfo.Bindings;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}

	public class GLBindPool : IBindPool {

		public GLGraphics Graphics { get; }

		public GLBindPool(GLGraphics graphics) {
			Graphics = graphics;
		}

		public IBindSet AllocSet(BindSetAllocateInfo allocateInfo) => new GLBindSet(this, allocateInfo);

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}

	public class GLBindSet : IBindSet {

		private readonly GLGraphics graphics;

		private readonly TextureBinding?[] textures;
		private readonly BufferBinding?[] uniformBuffers;
		private readonly BufferBinding?[] storageBuffers;

		public GLBindSet(GLBindPool pool, BindSetAllocateInfo allocateInfo) {
			graphics = pool.Graphics;
			var limits = graphics.Limits;
			textures = new TextureBinding?[limits.MaxPerStageSamplers];
			uniformBuffers = new BufferBinding?[limits.MaxPerStageUniformBuffers];
			storageBuffers = new BufferBinding?[limits.MaxPerStageStorageBuffers];

			foreach(var layout in allocateInfo.Layouts) {
				foreach(var binding in layout.Bindings) {
					switch(binding.Type) {
						case BindType.UniformBuffer:
							uniformBuffers[binding.Binding] = default(BufferBinding);
							break;
						case BindType.StorageBuffer:
							storageBuffers[binding.Binding] = default(BufferBinding);
							break;
						case BindType.CombinedTextureSampler:
						case BindType.StorageTexture:
							textures[binding.Binding] = default(TextureBinding);
							break;
					}
				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		internal void Bind() {
			var state = graphics.State;

			// Bind textures
			for(uint i = 0; i < textures.Length; i++) {
				var tex = textures[i];
				if (tex != null) {
					var tex2 = tex.Value;
					var texobj = (IGLTexture)tex2.TextureView;
					state.BindTextureUnit(i, texobj.GLTarget, texobj.ID);
					var samplerobj = (GLSampler?)tex2.Sampler;
					if (samplerobj != null) state.BindSampler(i, samplerobj.ID);
				}
			}

			void BindBuffers(GLBufferRangeTarget target, BufferBinding?[] bindings) {
				for(uint i = 0; i < bindings.Length; i++) {
					var buf = bindings[i];
					if (buf != null) {
						var buf2 = buf.Value;
						state.BindBufferRange(target, i, new GLBufferRangeBinding() {
							Buffer = ((GLBuffer)buf2.Buffer).ID,
							Offset = (nint)buf2.Range.Offset,
							Length = (nint)buf2.Range.Length
						});
					}
				}
			}

			// Bind buffers
			BindBuffers(GLBufferRangeTarget.Uniform, uniformBuffers);
			BindBuffers(GLBufferRangeTarget.ShaderStorage, storageBuffers);
		}

		public void Update(IReadOnlyList<BindSetWrite> writes) {
			foreach(BindSetWrite write in writes) {
				switch(write.Type) {
					case BindType.UniformBuffer:
						if (write.BufferInfo != null) {
							var info = write.BufferInfo.Value;
							uniformBuffers[write.Binding] = info with { Range = info.Range.Constrain(info.Buffer.Size) };

						} else uniformBuffers[write.Binding] = null;
						break;
					case BindType.StorageBuffer:
						if (write.BufferInfo != null) {
							var info = write.BufferInfo.Value;
							storageBuffers[write.Binding] = info with { Range = info.Range.Constrain(info.Buffer.Size) };

						} else storageBuffers[write.Binding] = null;
						break;
					case BindType.CombinedTextureSampler:
					case BindType.StorageTexture:
						textures[write.Binding] = write.TextureInfo;
						break;
				}
			}
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.GL.Native;

namespace Tesseract.GL.Graphics {

	public struct GLBufferRangeBinding {

		public uint Buffer;

		public nint Offset;

		public nint Length;

		public static bool operator ==(GLBufferRangeBinding b1, GLBufferRangeBinding b2) => b1.Buffer == b2.Buffer && b1.Offset == b2.Offset && b1.Length == b2.Length;

		public static bool operator !=(GLBufferRangeBinding b1, GLBufferRangeBinding b2) => !(b1 == b2);

		public override bool Equals(object obj) => obj is GLBufferRangeBinding b && b == this;

		public override int GetHashCode() => (int)(Buffer ^ Offset ^ Length);
	}
	
	public class GLState : IGLObject {

		public GL GL { get; }

		private uint texture1D;
		private uint texture1DArray;
		private uint texture2D;
		private uint texture2DArray;
		private uint texture2DMultisample;
		private uint texture2DMultisampleArray;
		private uint texture3D;
		private uint textureCubeMap;
		private uint textureCubeMapArray;
		private uint textureRectangle;

		private uint bufferArray;
		private uint bufferCopyRead;
		private uint bufferCopyWrite;
		private uint bufferDispatchIndirect;
		private uint bufferDrawIndirect;
		private uint bufferElementArray;
		private uint bufferPixelPack;
		private uint bufferPixelUnpack;
		private uint bufferQuery;
		private uint bufferTexture;

		private readonly GLBufferRangeBinding[] bufferRangeAtomicCounter;
		private readonly GLBufferRangeBinding[] bufferRangeShaderStorage;
		private readonly GLBufferRangeBinding[] bufferRangeUniform;
		private readonly GLBufferRangeBinding[] bufferRangeTransformFeedback;

		private uint vertexArray;

		private uint framebufferDraw;
		private uint framebufferRead;

		private uint renderbuffer;

		private ref uint GetTextureBinding(GLTextureTarget target, out bool valid) {
			valid = true;
			switch(target) {
				case GLTextureTarget.Texture1D: return ref texture1D;
				case GLTextureTarget.Texture1DArray: return ref texture1DArray;
				case GLTextureTarget.Texture2D: return ref texture2D;
				case GLTextureTarget.Texture2DArray: return ref texture2DArray;
				case GLTextureTarget.Texture2DMultisample: return ref texture2DMultisample;
				case GLTextureTarget.Texture2DMultisampleArray: return ref texture2DMultisampleArray;
				case GLTextureTarget.Texture3D: return ref texture3D;
				case GLTextureTarget.CubeMap: return ref textureCubeMap;
				case GLTextureTarget.CubeMapArray: return ref textureCubeMapArray;
				case GLTextureTarget.Rectangle: return ref textureRectangle;
				default:
					valid = false;
					return ref texture1D;
			}
		}

		private ref uint GetBufferBinding(GLBufferTarget target, out bool valid) {
			valid = true;
			switch(target) {
				case GLBufferTarget.Array: return ref bufferArray;
				case GLBufferTarget.CopyRead: return ref bufferCopyRead;
				case GLBufferTarget.CopyWrite: return ref bufferCopyWrite;
				case GLBufferTarget.DispatchIndirect: return ref bufferDispatchIndirect;
				case GLBufferTarget.DrawIndirect: return ref bufferDrawIndirect;
				case GLBufferTarget.ElementArray: return ref bufferElementArray;
				case GLBufferTarget.PixelPack: return ref bufferPixelPack;
				case GLBufferTarget.PixelUnpack: return ref bufferPixelUnpack;
				case GLBufferTarget.Query: return ref bufferQuery;
				case GLBufferTarget.Texture: return ref bufferTexture;
				default:
					valid = false;
					return ref bufferArray;
			}
		}

		private ref GLBufferRangeBinding GetBufferRangeBinding(GLBufferRangeTarget target, uint index, out bool valid) {
			valid = true;
			switch(target) {
				case GLBufferRangeTarget.AtomicCounter: return ref bufferRangeAtomicCounter[index];
				case GLBufferRangeTarget.ShaderStorage: return ref bufferRangeShaderStorage[index];
				case GLBufferRangeTarget.Uniform: return ref bufferRangeUniform[index];
				case GLBufferRangeTarget.TransformFeedback: return ref bufferRangeTransformFeedback[index];
				default:
					valid = false;
					return ref bufferRangeUniform[0];
			}
		}

		public GLState(GL gl) {
			GL = gl;
			bufferRangeUniform = new GLBufferRangeBinding[gl.GL11.GetInteger(GLEnums.GL_MAX_UNIFORM_BUFFER_BINDINGS)];
			bufferRangeTransformFeedback = new GLBufferRangeBinding[gl.GL11.GetInteger(GLEnums.GL_MAX_TRANSFORM_FEEDBACK_BUFFERS)];
		}

		public void BindTexture(GLTextureTarget target, uint texture) {
			ref uint binding = ref GetTextureBinding(target, out bool valid);
			if (valid && binding == texture) return;
			GL.GL15.BindTexture(target, texture);
			if (valid) binding = texture;
		}

		public void BindBuffer(GLBufferTarget target, uint buffer) {
			ref uint binding = ref GetBufferBinding(target, out bool valid);
			if (valid && binding == buffer) return;
			GL.GL15.BindBuffer(target, buffer);
			if (valid) binding = buffer;
		}

		public GLBufferTarget BindBufferAny(uint buffer, GLBufferTarget defaultTarget = GLBufferTarget.Array) {
			if (bufferArray == buffer) return GLBufferTarget.Array;
			if (bufferCopyRead == buffer) return GLBufferTarget.CopyRead;
			if (bufferCopyWrite == buffer) return GLBufferTarget.CopyWrite;
			if (bufferDispatchIndirect == buffer) return GLBufferTarget.DispatchIndirect;
			if (bufferDrawIndirect == buffer) return GLBufferTarget.DrawIndirect;
			if (bufferElementArray == buffer) return GLBufferTarget.ElementArray;
			if (bufferPixelPack == buffer) return GLBufferTarget.PixelPack;
			if (bufferPixelUnpack == buffer) return GLBufferTarget.PixelUnpack;
			if (bufferQuery == buffer) return GLBufferTarget.Query;
			if (bufferTexture == buffer) return GLBufferTarget.Texture;
			GL.GL15.BindBuffer(defaultTarget, buffer);
			return defaultTarget;
		}

		public void BindBufferRange(GLBufferRangeTarget target, uint index, GLBufferRangeBinding binding) {
			ref GLBufferRangeBinding currentBinding = ref GetBufferRangeBinding(target, index, out bool valid);
			if (valid && currentBinding == binding) return;
			GL.GL30.BindBufferRange(target, index, binding.Buffer, binding.Offset, binding.Length);
			if (valid) currentBinding = binding;
		}

		public void BindVertexArray(uint vertexArray) {
			if (this.vertexArray == vertexArray) return;
			GL.GL30.BindVertexArray(vertexArray);
			this.vertexArray = vertexArray;
		}

		public void BindFramebuffer(GLFramebufferTarget target, uint framebuffer) {
			switch(target) {
				case GLFramebufferTarget.Draw:
					if (framebufferDraw == framebuffer) return;
					GL.GL30.BindFramebuffer(GLFramebufferTarget.Draw, framebuffer);
					framebufferDraw = framebuffer;
					break;
				case GLFramebufferTarget.Read:
					if (framebufferRead == framebuffer) return;
					GL.GL30.BindFramebuffer(GLFramebufferTarget.Read, framebuffer);
					framebufferRead = framebuffer;
					break;
				case GLFramebufferTarget.Framebuffer:
					if (framebufferDraw == framebuffer && framebufferRead == framebuffer) return;
					GL.GL30.BindFramebuffer(GLFramebufferTarget.Framebuffer, framebuffer);
					framebufferDraw = framebufferRead = framebuffer;
					break;
			}
		}

		public void BindRenderbuffer(GLRenderbufferTarget target, uint renderbuffer) {
			if (target == GLRenderbufferTarget.Renderbuffer && this.renderbuffer == renderbuffer) return;
			GL.GL30.BindRenderbuffer(target, renderbuffer);
			if (target == GLRenderbufferTarget.Renderbuffer) this.renderbuffer = renderbuffer;
		}

	}

}

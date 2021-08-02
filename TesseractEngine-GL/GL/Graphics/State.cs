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

		private struct TextureUnit {

			// The target currently bound to this unit
			/*   OpenGL technically can bind to different targets within a texture unit but
			 *   it is easier to pretend units only hold one texture with a specific target.
			 */
			public GLTextureTarget Target;
			// The texture bound to this unit
			public uint Texture;
			// The sampler bound to this unit
			public uint Sampler;

		}

		private uint activeTextureUnit;
		private readonly TextureUnit[] textureUnits; 

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

		private uint program;

		private uint vertexArray;

		private uint framebufferDraw;
		private uint framebufferRead;

		private uint renderbuffer;

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
			textureUnits = new TextureUnit[gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TEXTURE_IMAGE_UNITS)];
			bufferRangeUniform = new GLBufferRangeBinding[gl.GL11.GetInteger(Native.GLEnums.GL_MAX_UNIFORM_BUFFER_BINDINGS)];
			bufferRangeTransformFeedback = new GLBufferRangeBinding[gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TRANSFORM_FEEDBACK_BUFFERS)];
		}

		public uint ActiveTextureUnit {
			get => activeTextureUnit;
			set {
				if (value != activeTextureUnit) {
					GL.GL20.ActiveTexture = (int)value;
					activeTextureUnit = value;
				}
			}
		}

		public void BindTexture(GLTextureTarget target, uint texture) {
			ref TextureUnit texunit = ref textureUnits[ActiveTextureUnit];
			if (texunit.Target == target && texunit.Texture == texture) return;
			GL.GL15.BindTexture(target, texture);
			texunit.Target = target;
			texunit.Texture = texture;
		}

		public void BindTextureUnit(uint unit, GLTextureTarget target, uint texture) {
			ref TextureUnit texunit = ref textureUnits[unit];
			if (texunit.Target == target && texunit.Texture == texture) return;
			if (activeTextureUnit != unit) {
				GL.GL20.ActiveTexture = (int)unit;
				activeTextureUnit = unit;
			}
			GL.GL15.BindTexture(target, texture);
			texunit.Target = target;
			texunit.Texture = texture;
		}

		/*
		public (uint, GLTextureTarget) BindTextureAny(uint texture, GLTextureTarget defaultTarget) {
			for(uint i = 0; i < textureUnits.Length; i++) {
				TextureUnit texunit = textureUnits[i];
				if (texunit.Texture == texture) {
					return (i, texunit.Target);
				}
			}
			BindTextureUnit(0, defaultTarget, texture);
			return (0, defaultTarget);
		}
		*/

		public void BindSampler(uint unit, uint sampler) {
			ref TextureUnit texunit = ref textureUnits[unit];
			if (texunit.Sampler == sampler) return;
			GL.GL33.BindSampler(unit, sampler);
			texunit.Sampler = sampler;
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

		public void UseProgram(uint program) {
			if (this.program == program) return;
			GL.GL20.UseProgram(program);
			this.program = program;
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

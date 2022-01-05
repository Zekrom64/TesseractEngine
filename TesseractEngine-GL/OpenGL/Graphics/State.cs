using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.OpenGL.Native;
using Tesseract.Core.Math;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {

	/// <summary>
	/// Ranged buffer binding state.
	/// </summary>
	public record struct GLBufferRangeBinding {

		/// <summary>
		/// Bound buffer.
		/// </summary>
		public uint Buffer { get; set; }

		/// <summary>
		/// Binding offset.
		/// </summary>
		public nint Offset { get; set; }

		/// <summary>
		/// Binding length.
		/// </summary>
		public nint Length { get; set; }

	}
	
	/// <summary>
	/// State manager for the OpenGL context. This is used to keep track of the current OpenGL state to simplify
	/// state inspection when needed and reduce duplicate calls to OpenGL.
	/// </summary>
	public class GLState : IGLObject {

		public GL GL { get; }

		// Individual texture unit state
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

		// Texture unit states
		private uint activeTextureUnit;
		private readonly TextureUnit[] textureUnits; 

		// Buffer states
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

		// Ranged buffer states
		private readonly GLBufferRangeBinding[] bufferRangeAtomicCounter;
		private readonly GLBufferRangeBinding[] bufferRangeShaderStorage;
		private readonly GLBufferRangeBinding[] bufferRangeUniform;
		private readonly GLBufferRangeBinding[] bufferRangeTransformFeedback;

		// Program state
		private uint program;

		// Vertex array state
		private uint vertexArray;

		// Framebuffer state
		private uint framebufferDraw;
		private uint framebufferRead;

		// Renderbuffer state
		private uint renderbuffer;

		// Gets the reference to a buffer binding state
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

		// Gets the reference to a ranged buffer binding state
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
			if (gl.ARBShaderAtomicCounters != null) {
				bufferRangeAtomicCounter = new GLBufferRangeBinding[gl.GL11.GetInteger(Native.GLEnums.GL_MAX_ATOMIC_COUNTER_BUFFER_BINDINGS)];
			} else {
				bufferRangeAtomicCounter = Array.Empty<GLBufferRangeBinding>();
			}
			if (gl.ARBShaderStorageBufferObject != null) {
				bufferRangeShaderStorage = new GLBufferRangeBinding[gl.GL11.GetInteger(Native.GLEnums.GL_MAX_SHADER_STORAGE_BUFFER_BINDINGS)];
			} else {
				bufferRangeShaderStorage = Array.Empty<GLBufferRangeBinding>();
			}
		}

		/// <summary>
		/// The active texture unit.
		/// </summary>
		public uint ActiveTextureUnit {
			get => activeTextureUnit;
			set {
				if (value != activeTextureUnit) {
					GL.GL20!.ActiveTexture = (int)value;
					activeTextureUnit = value;
				}
			}
		}

		/// <summary>
		/// Binds a texture to the given target in the current texture unit.
		/// </summary>
		/// <param name="target">Texture target</param>
		/// <param name="texture">Texture to bind</param>
		public void BindTexture(GLTextureTarget target, uint texture) {
			ref TextureUnit texunit = ref textureUnits[ActiveTextureUnit];
			if (texunit.Target == target && texunit.Texture == texture) return;
			GL.GL15!.BindTexture(target, texture);
			texunit.Target = target;
			texunit.Texture = texture;
		}

		/// <summary>
		/// Binds a texture to the given target in the given texture unit.
		/// </summary>
		/// <param name="unit">Texture unit</param>
		/// <param name="target">Texture target</param>
		/// <param name="texture">Texture to bind</param>
		public void BindTextureUnit(uint unit, GLTextureTarget target, uint texture) {
			ref TextureUnit texunit = ref textureUnits[unit];
			if (texunit.Target == target && texunit.Texture == texture) return;
			var dsa = GL.ARBDirectStateAccess;
			if (dsa != null) {
				dsa.BindTextureUnit(unit, texture);
			} else {
				if (activeTextureUnit != unit) {
					GL.GL20!.ActiveTexture = (int)unit;
					activeTextureUnit = unit;
				}
				GL.GL15!.BindTexture(target, texture);
			}
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

		/// <summary>
		/// Binds a sampler to the given texture unit.
		/// </summary>
		/// <param name="unit">Texture unit</param>
		/// <param name="sampler">Sampler to bind</param>
		public void BindSampler(uint unit, uint sampler) {
			ref TextureUnit texunit = ref textureUnits[unit];
			if (texunit.Sampler == sampler) return;
			GL.GL33!.BindSampler(unit, sampler);
			texunit.Sampler = sampler;
		}

		/// <summary>
		/// Binds a buffer to the given target.
		/// </summary>
		/// <param name="target">Buffer target</param>
		/// <param name="buffer">Buffer to bind</param>
		public void BindBuffer(GLBufferTarget target, uint buffer) {
			ref uint binding = ref GetBufferBinding(target, out bool valid);
			if (valid && binding == buffer) return;
			GL.GL15!.BindBuffer(target, buffer);
			if (valid) binding = buffer;
		}

		/// <summary>
		/// Sets the buffer bound to the given target. This may be used when buffer binding must be
		/// forced to modify the current OpenGL state to keep the state manager in sync.
		/// </summary>
		/// <param name="target">Buffer target</param>
		/// <param name="buffer">Buffer to set</param>
		public void SetBoundBuffer(GLBufferTarget target, uint buffer) {
			ref uint binding = ref GetBufferBinding(target, out bool valid);
			if (valid) binding = buffer;
		}

		/// <summary>
		/// Checks if a buffer is currently bound, binding it to a default target if not, and returns
		/// the target it is bound to.
		/// </summary>
		/// <param name="buffer">Buffer to bind</param>
		/// <param name="defaultTarget">Default buffer target</param>
		/// <returns>Target the buffer is bound to</returns>
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
			GL.GL15!.BindBuffer(defaultTarget, buffer);
			return defaultTarget;
		}

		/// <summary>
		/// Binds a range of a buffer to a ranged buffer target.
		/// </summary>
		/// <param name="target>Buffer target</param>
		/// <param name="index">Index within target to bind to</param>
		/// <param name="binding">Ranged buffer binding</param>
		public void BindBufferRange(GLBufferRangeTarget target, uint index, GLBufferRangeBinding binding) {
			ref GLBufferRangeBinding currentBinding = ref GetBufferRangeBinding(target, index, out bool valid);
			if (valid && currentBinding == binding) return;
			GL.GL30!.BindBufferRange(target, index, binding.Buffer, binding.Offset, binding.Length);
			if (valid) currentBinding = binding;
		}

		/// <summary>
		/// Uses the given program.
		/// </summary>
		/// <param name="program">Program to use</param>
		public void UseProgram(uint program) {
			if (this.program == program) return;
			GL.GL20!.UseProgram(program);
			this.program = program;
		}

		/// <summary>
		/// Binds a vertex array.
		/// </summary>
		/// <param name="vertexArray">Vertex array to bind</param>
		public void BindVertexArray(uint vertexArray) {
			if (this.vertexArray == vertexArray) return;
			GL.GL30!.BindVertexArray(vertexArray);
			this.vertexArray = vertexArray;
		}

		/// <summary>
		/// Binds a framebuffer to the given target.
		/// </summary>
		/// <param name="target">Framebuffer target</param>
		/// <param name="framebuffer">Framebuffer to bind</param>
		public void BindFramebuffer(GLFramebufferTarget target, uint framebuffer) {
			switch(target) {
				case GLFramebufferTarget.Draw:
					if (framebufferDraw == framebuffer) return;
					GL.GL30!.BindFramebuffer(GLFramebufferTarget.Draw, framebuffer);
					framebufferDraw = framebuffer;
					break;
				case GLFramebufferTarget.Read:
					if (framebufferRead == framebuffer) return;
					GL.GL30!.BindFramebuffer(GLFramebufferTarget.Read, framebuffer);
					framebufferRead = framebuffer;
					break;
				case GLFramebufferTarget.Framebuffer:
					if (framebufferDraw == framebuffer && framebufferRead == framebuffer) return;
					GL.GL30!.BindFramebuffer(GLFramebufferTarget.Framebuffer, framebuffer);
					framebufferDraw = framebufferRead = framebuffer;
					break;
			}
		}

		/// <summary>
		/// Binds a renderbuffer to the given target.
		/// </summary>
		/// <param name="target">Renderbuffer target</param>
		/// <param name="renderbuffer">Renderbuffer to bind</param>
		public void BindRenderbuffer(GLRenderbufferTarget target, uint renderbuffer) {
			if (target == GLRenderbufferTarget.Renderbuffer && this.renderbuffer == renderbuffer) return;
			GL.GL30!.BindRenderbuffer(target, renderbuffer);
			if (target == GLRenderbufferTarget.Renderbuffer) this.renderbuffer = renderbuffer;
		}


		private readonly Stack<uint> drawFramebufferStack = new();

		/// <summary>
		/// Pushes the current draw framebuffer, and binds a new one in its place.
		/// </summary>
		/// <param name="framebuffer">Draw framebuffer to bind</param>
		public void PushDrawFramebuffer(uint framebuffer) {
			drawFramebufferStack.Push(framebufferDraw);
			BindFramebuffer(GLFramebufferTarget.Draw, framebuffer);
		}

		/// <summary>
		/// Pops the current draw framebuffer, binding the previously pushed buffer.
		/// </summary>
		public void PopDrawFramebuffer() => BindFramebuffer(GLFramebufferTarget.Draw, drawFramebufferStack.Pop());


		//========================//
		// Render Pass Management //
		//========================//

		/// <summary>
		/// The current render pass used for rendering, or null if there is none.
		/// </summary>
		public GLRenderPass? CurrentRenderPass { get; private set; } = null;

		/// <summary>
		/// The current framebuffer used for rendering, or null if there is none.
		/// </summary>
		public GLFramebuffer? CurrentFramebuffer { get; private set; } = null;

		/// <summary>
		/// The current area being rendered to.
		/// </summary>
		public Recti CurrentRenderArea { get; private set; }

		/// <summary>
		/// The current renderpass' subpass.
		/// </summary>
		public int CurrentSubpass { get; private set; } = -1;

		/// <summary>
		/// Begins a render pass.
		/// </summary>
		/// <param name="beginInfo">Render pass begin information</param>
		public void BeginRenderPass(ICommandSink.RenderPassBegin beginInfo) {
			CurrentRenderPass = (GLRenderPass)beginInfo.RenderPass;
			CurrentFramebuffer = (GLFramebuffer)beginInfo.Framebuffer;
			CurrentRenderArea = beginInfo.RenderArea;
			CurrentSubpass = 0;
			CurrentFramebuffer.BeginRenderPass(beginInfo);
			CurrentFramebuffer.BeginSubpass();
		}

		/// <summary>
		/// Moves to the next subpass in the current render pass.
		/// </summary>
		public void NextSubpass() {
			if (CurrentRenderPass != null) {
				CurrentFramebuffer!.EndSubpass();
				CurrentSubpass++;
				CurrentFramebuffer.BeginSubpass();
			}
		}

		/// <summary>
		/// Ends the current render pass.
		/// </summary>
		public void EndRenderPass() {
			CurrentFramebuffer!.EndRenderPass();
			CurrentRenderPass = null;
			CurrentFramebuffer = null;
			CurrentSubpass = -1;
		}

		//================//
		// Pipeline State //
		//================//

		public GLDrawMode DrawMode { get; private set; }


		public void BindPipeline(GLPipeline pipeline) {

		}

	}

}

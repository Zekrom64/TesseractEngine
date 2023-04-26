using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBGeometryShader4Functions {

		[ExternFunction(AltNames = new string[] { "glProgramParameteriARB" })]
		[NativeType("void glProgramParameter(GLuint program, GLenum pname, GLint value)")]
		public delegate* unmanaged<uint, uint, int, void> glProgramParameteri;
		[ExternFunction(AltNames = new string[] { "glFramebufferTextureARB" })]
		[NativeType("void glFramebufferTexture(GLenum target, GLenum attachment, GLuint texture, GLint level)")]
		public delegate* unmanaged<uint, uint, uint, int, void> glFramebufferTexture;
		[ExternFunction(AltNames = new string[] { "glFramebufferTextureLayerARB" })]
		[NativeType("void glFramebufferTextureLayer(GLenum target, GLenum attachment, GLuint texture, GLint level, GLint layer)")]
		public delegate* unmanaged<uint, uint, uint, int, int, void> glFramebufferTextureLayer;
		[ExternFunction(AltNames = new string[] { "glFramebufferTextureFaceARB" })]
		[NativeType("void glFramebufferTextureFace(GLenum target, GLenum attachment, GLuint texture, GLint level, GLenum face)")]
		public delegate* unmanaged<uint, uint, uint, int, uint, void> glFramebufferTextureFace;

	}

	public class ARBGeometryShader4 : IGLObject {

		public GL GL { get; }
		public ARBGeometryShader4Functions Functions { get; } = new();

		public ARBGeometryShader4(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramParamter(uint program, GLProgramParameter pname, int val) {
			unsafe {
				Functions.glProgramParameteri(program, (uint)pname, val);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture(GLFramebufferTarget target, GLFramebufferAttachment attachment, uint texture, int level) {
			unsafe {
				Functions.glFramebufferTexture((uint)target, (uint)attachment, texture, level);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTextureLayer(GLFramebufferTarget target, GLFramebufferAttachment attachment, uint texture, int level, int layer) {
			unsafe {
				Functions.glFramebufferTextureLayer((uint)target, (uint)attachment, texture, level, layer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTextureFace(GLFramebufferTarget target, GLFramebufferAttachment attachment, uint texture, int level, GLCubeMapFace face) {
			unsafe {
				Functions.glFramebufferTextureFace((uint)target, (uint)attachment, texture, level, (uint)face);
			}
		}
	}

}

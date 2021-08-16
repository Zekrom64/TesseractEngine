using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBGeometryShader4Functions {

		public delegate void PFN_glProgramParameteri(uint program, uint pname, int value);
		[ExternFunction(AltNames = new string[] { "glProgramParameteriARB" })]
		public PFN_glProgramParameteri glProgramParameteri;
		public delegate void PFN_glFramebufferTexture(uint target, uint attachment, uint texture, int level);
		[ExternFunction(AltNames = new string[] { "glFramebufferTextureARB" })]
		public PFN_glFramebufferTexture glFramebufferTexture;
		public delegate void PFN_glFramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer);
		[ExternFunction(AltNames = new string[] { "glFramebufferTextureLayerARB" })]
		public PFN_glFramebufferTextureLayer glFramebufferTextureLayer;
		public delegate void PFN_glFramebufferTextureFace(uint target, uint attachment, uint texture, int level, uint face);
		[ExternFunction(AltNames = new string[] { "glFramebufferTextureFaceARB" })]
		public PFN_glFramebufferTextureFace glFramebufferTextureFace;

	}

	public class ARBGeometryShader4 : IGLObject {

		public GL GL { get; }
		public ARBGeometryShader4Functions Functions { get; } = new();

		public ARBGeometryShader4(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramParamter(uint program, GLProgramParameter pname, int val) => Functions.glProgramParameteri(program, (uint)pname, val);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture(GLFramebufferTarget target, GLFramebufferAttachment attachment, uint texture, int level) => Functions.glFramebufferTexture((uint)target, (uint)attachment, texture, level);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTextureLayer(GLFramebufferTarget target, GLFramebufferAttachment attachment, uint texture, int level, int layer) => Functions.glFramebufferTextureLayer((uint)target, (uint)attachment, texture, level, layer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTextureFace(GLFramebufferTarget target, GLFramebufferAttachment attachment, uint texture, int level, GLCubeMapFace face) => Functions.glFramebufferTextureFace((uint)target, (uint)attachment, texture, level, (uint)face);

	}

}

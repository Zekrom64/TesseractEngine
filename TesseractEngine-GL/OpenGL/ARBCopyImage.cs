using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBCopyImageFunctions {

		public delegate void PFN_glCopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth);
		[ExternFunction(AltNames = new string[] { "glCopyImageSubDataARB" })]
		public PFN_glCopyImageSubData glCopyImageSubData;

	}

	public class ARBCopyImage : IGLObject {

		public GL GL { get; }
		public ARBCopyImageFunctions Functions { get; } = new();

		public ARBCopyImage(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImageSubData(uint srcName, GLTextureTarget srcTarget, int srcLevel, Vector3i src, uint dstName, GLTextureTarget dstTarget, int dstLevel, Vector3i dst, Vector3i size) =>
			Functions.glCopyImageSubData(srcName, (uint)srcTarget, srcLevel, src.X, src.Y, src.Z, dstName, (uint)dstTarget, dstLevel, dst.X, dst.Y, dst.Z, size.X, size.Y, size.Z);

	}
}

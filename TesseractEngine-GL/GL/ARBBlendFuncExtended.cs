using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.GL {
	
	public class ARBBlendFuncExtendedFunctions {

		public delegate void PFN_glBindFragDataLocationIndexed(uint program, uint colorNumber, uint index, [MarshalAs(UnmanagedType.LPStr)] string name);
		[ExternFunction(AltNames = new string[] { "glBindFragDataLocationIndexedARB" })]
		public PFN_glBindFragDataLocationIndexed glBindFragDataLocationIndexed;
		public delegate int PFN_glGetFragDataIndex(uint program, [MarshalAs(UnmanagedType.LPStr)] string name);
		[ExternFunction(AltNames = new string[] { "glGetFragDataIndexARB" })]
		public PFN_glGetFragDataIndex glGetFragDataIndex;

	}
	
	public class ARBBlendFuncExtended : IGLObject {

		public GL GL { get; }
		public ARBBlendFuncExtendedFunctions Functions { get; } = new();

		public ARBBlendFuncExtended(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFragDataLocationIndexed(uint program, uint colorNumber, uint index, string name) => Functions.glBindFragDataLocationIndexed(program, colorNumber, index, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFragDataIndex(uint program, string name) => Functions.glGetFragDataIndex(program, name);
	
	}

}

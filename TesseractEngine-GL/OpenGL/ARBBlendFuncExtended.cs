using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBBlendFuncExtendedFunctions {

		[ExternFunction(AltNames = new string[] { "glBindFragDataLocationIndexedARB" })]
		[NativeType("void glBindFragDataLocationIndexed(GLuint program, GLuint colorNumber, GLuint index, const char* name)")]
		public delegate* unmanaged<uint, uint, uint, byte*, void> glBindFragDataLocationIndexed;
		[ExternFunction(AltNames = new string[] { "glGetFragDataIndexARB" })]
		[NativeType("int glGetFragDataIndex(GLuint program, const char* name)")]
		public delegate* unmanaged<uint, byte*, int> glGetFragDataIndex;

	}

	public class ARBBlendFuncExtended : IGLObject {

		public GL GL { get; }
		public ARBBlendFuncExtendedFunctions Functions { get; } = new();

		public ARBBlendFuncExtended(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFragDataLocationIndexed(uint program, uint colorNumber, uint index, string name) {
			unsafe {
				fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					Functions.glBindFragDataLocationIndexed(program, colorNumber, index, pName);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFragDataIndex(uint program, string name) {
			unsafe {
				fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					return Functions.glGetFragDataIndex(program, pName);
				}
			}
		}
	}

}

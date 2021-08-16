using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class EXTGPUShader4Functions {

		public delegate void PFN_glVertexAttribIPointer(uint index, int size, uint type, int stride, IntPtr pointer);
		[ExternFunction(AltNames = new string[] { "glVertexAttribIPointerEXT" })]
		public PFN_glVertexAttribIPointer glVertexAttribIPointer;
		public delegate void PFN_glGetVertexAttribIiv(uint index, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetVertexAttribIivEXT" })]
		public PFN_glGetVertexAttribIiv glGetVertexAttribIiv;
		public delegate void PFN_glGetVertexAttribIuiv(uint index, uint pname, [NativeType("GLuint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetVertexAttribIuivEXT" })]
		public PFN_glGetVertexAttribIuiv glGetVertexAttribIuiv;
		public delegate void PFN_glBindFragDataLocation(uint program, uint colorNumber, [MarshalAs(UnmanagedType.LPStr)] string name);
		[ExternFunction(AltNames = new string[] { "glBindFragDataLocationEXT" })]
		public PFN_glBindFragDataLocation glBindFragDataLocation;
		public delegate int PFN_glGetFragDataLocation(uint program, [MarshalAs(UnmanagedType.LPStr)] string name);
		[ExternFunction(AltNames = new string[] { "glGetFragDataLocationEXT" })]
		public PFN_glGetFragDataLocation glGetFragDataLocation;

	}

	public class EXTGPUShader4 : IGLObject {

		public GL GL { get; }
		public EXTGPUShader4Functions Functions { get; } = new();

		public EXTGPUShader4(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribIPointer(uint index, int size, GLType type, int stride, nint offset) => Functions.glVertexAttribIPointer(index, size, (uint)type, stride, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetVertexAttrib(uint index, GLGetVertexAttrib pname, Span<int> v) {
			unsafe {
				fixed(int* pV = v) {
					Functions.glGetVertexAttribIiv(index, (uint)pname, (IntPtr)pV);
				}
			}
			return v;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetVertexAttrib(uint index, GLGetVertexAttrib pname, Span<uint> v) {
			unsafe {
				fixed(uint* pV = v) {
					Functions.glGetVertexAttribIuiv(index, (uint)pname, (IntPtr)pV);
				}
			}
			return v;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFragDataLocation(uint program, uint colorNumber, string name) => Functions.glBindFragDataLocation(program, colorNumber, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFragDataLocation(uint program, string name) => Functions.glGetFragDataLocation(program, name);

	}

}

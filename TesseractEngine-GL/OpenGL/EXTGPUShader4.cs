using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class EXTGPUShader4Functions {

		[ExternFunction(AltNames = new string[] { "glVertexAttribIPointerEXT" })]
		[NativeType("void glVertexAttribIPointer(GLuint index, GLint size, GLenum type, GLsizei stride, void* pointer)")]
		public delegate* unmanaged<uint, int, uint, int, IntPtr, void> glVertexAttribIPointer;

		[ExternFunction(AltNames = new string[] { "glGetVertexAttribIivEXT" })]
		[NativeType("void glGetVertexAttribIiv(GLuint index, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetVertexAttribIiv;

		[ExternFunction(AltNames = new string[] { "glGetVertexAttribIuivEXT" })]
		[NativeType("void glGetVertexAttribIuiv(GLuint index, GLenum pname, GLuint* pParams)")]
		public delegate* unmanaged<uint, uint, uint*, void> glGetVertexAttribIuiv;

		[ExternFunction(AltNames = new string[] { "glBindFragDataLocationEXT" })]
		[NativeType("void glBindFragDataLocation(GLuint program, GLuint colorNumber, const char* name)")]
		public delegate* unmanaged<uint, uint, byte*, void> glBindFragDataLocation;

		[ExternFunction(AltNames = new string[] { "glGetFragDataLocationEXT" })]
		[NativeType("GLint glGetFragDataLocation(GLuint program, const char* name)")]
		public delegate* unmanaged<uint, byte*, int> glGetFragDataLocation;

	}

	public class EXTGPUShader4 : IGLObject {

		public GL GL { get; }
		public EXTGPUShader4Functions Functions { get; } = new();

		public EXTGPUShader4(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribIPointer(uint index, int size, GLTextureType type, int stride, nint offset) {
			unsafe {
				Functions.glVertexAttribIPointer(index, size, (uint)type, stride, offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetVertexAttrib(uint index, GLGetVertexAttrib pname, Span<int> v) {
			unsafe {
				fixed(int* pV = v) {
					Functions.glGetVertexAttribIiv(index, (uint)pname, pV);
				}
			}
			return v;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetVertexAttrib(uint index, GLGetVertexAttrib pname, Span<uint> v) {
			unsafe {
				fixed(uint* pV = v) {
					Functions.glGetVertexAttribIuiv(index, (uint)pname, pV);
				}
			}
			return v;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFragDataLocation(uint program, uint colorNumber, string name) {
			unsafe {
				fixed(byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					Functions.glBindFragDataLocation(program, colorNumber, pName);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFragDataLocation(uint program, string name) {
			unsafe {
				fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					return Functions.glGetFragDataLocation(program, pName);
				}
			}
		}
	}

}

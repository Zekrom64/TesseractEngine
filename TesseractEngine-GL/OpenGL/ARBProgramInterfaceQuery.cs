using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBProgramInterfaceQueryFunctions {

		[ExternFunction(AltNames = new string[] { "glGetProgramInterfaceivARB" })]
		[NativeType("void glGetProgramInterfaceiv(GLuint program, GLenum programInterface, GLenum pname, GLint* pValues)")]
		public delegate* unmanaged<uint, uint, uint, int*, void> glGetProgramInterfaceiv;
		[ExternFunction(AltNames = new string[] { "glGetProgramResourceIndexARB" })]
		[NativeType("GLuint glGetProgramResourceIndex(GLuint program, GLenum programInterface, const char* name)")]
		public delegate* unmanaged<uint, uint, byte*, uint> glGetProgramResourceIndex;
		[ExternFunction(AltNames = new string[] { "glGetProgramResourceName" })]
		[NativeType("void glGetProgramResourceName(GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize, GLsizei* pLength, char* pName)")]
		public delegate* unmanaged<uint, uint, uint, int, out int, byte*, void> glGetProgramResourceName;
		[ExternFunction(AltNames = new string[] { "glGetProgramResourceivARB" })]
		[NativeType("void glGetProgramResourceiv(GLuint program, GLenum programInterface, GLuint index, GLsizei propCount, const GLenum* pProps, GLsizei bufSize, GLint* pLength, GLint* pValues)")]
		public delegate* unmanaged<uint, uint, uint, int, uint*, int, out int, int*, void> glGetProgramResourceiv;
		[ExternFunction(AltNames = new string[] { "glGetProgramResourceLocationARB" })]
		[NativeType("GLint glGetProgramResourceLocation(GLuint program, GLenum programInterface, const char* name)")]
		public delegate* unmanaged<uint, uint, byte*, int> glGetProgramResourceLocation;
		[ExternFunction(AltNames = new string[] { "glGetProgramResourceLocationIndexARB" })]
		[NativeType("GLint glGetProgramResourceLocationIndex(GLuint program, GLenum programInterface, const char* name)")]
		public delegate* unmanaged<uint, uint, byte*, int> glGetProgramResourceLocationIndex;

	}

	public class ARBProgramInterfaceQuery : IGLObject {

		public GL GL { get; }
		public ARBProgramInterfaceQueryFunctions Functions { get; } = new();

		public ARBProgramInterfaceQuery(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetProgramInterface(uint program, GLProgramInterface programInterface, GLGetProgramInterface pname, Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetProgramInterfaceiv(program, (uint)programInterface, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramInterface(uint program, GLProgramInterface programInterface, GLGetProgramInterface pname) {
			int retn = 0;
			unsafe {
				Functions.glGetProgramInterfaceiv(program, (uint)programInterface, (uint)pname, &retn);
			}
			return retn;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetProgramResourceIndex(uint program, GLProgramInterface programInterface, string name) {
			unsafe {
				fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					return Functions.glGetProgramResourceIndex(program, (uint)programInterface, pName);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetProgramResourceName(uint program, GLProgramInterface programInterface, uint index) {
			int length = GetProgramInterface(program, programInterface, GLGetProgramInterface.MaxNameLength, stackalloc int[1])[0];
			Span<byte> name = stackalloc byte[length];
			unsafe {
				fixed(byte* pName = name) {
					Functions.glGetProgramResourceName(program, (uint)programInterface, index, length, out length, pName);
				}
			}
			return MemoryUtil.GetASCII(name[..length]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetProgramResource(uint program, GLProgramInterface programInterface, uint index, in ReadOnlySpan<GLGetProgramResource> props, Span<int> values) {
			unsafe {
				fixed(GLGetProgramResource* pProps = props) {
					fixed(int* pValues = values) {
						Functions.glGetProgramResourceiv(program, (uint)programInterface, index, props.Length, (uint*)pProps, values.Length, out int _, pValues);
					}
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T GetProgramResource<T>(uint program, GLProgramInterface programInterface, uint index,  GLGetProgramResource prop) where T : unmanaged {
			T retn;
			unsafe {
				Functions.glGetProgramResourceiv(program, (uint)programInterface, index, 1, (uint*)&prop, 1, out int _, (int*)&retn);
			}
			return retn;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramResourceLocation(uint program, GLProgramInterface programInterface, string name) {
			unsafe {
				fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					return Functions.glGetProgramResourceLocation(program, (uint)programInterface, pName);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramResourceLocationIndex(uint program, GLProgramInterface programInterface, string name) {
			unsafe {
				fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					return Functions.glGetProgramResourceLocationIndex(program, (uint)programInterface, pName);
				}
			}
		}
	}
}

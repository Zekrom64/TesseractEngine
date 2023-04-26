using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBUniformBufferObjectFunctions {

		[ExternFunction(AltNames = new string[] { "glGetUniformIndicesARB" })]
		[NativeType("void glGetUniformIndices(GLuint program, GLsizei uniformCount, const char* const* pUniformNames, GLuint* pUniformIndices)")]
		public delegate* unmanaged<uint, int, byte**, uint*, void> glGetUniformIndices;
		[ExternFunction(AltNames = new string[] { "glGetActiveUniformsivARB" })]
		[NativeType("void glGetActiveUniformsiv(GLuint program, GLsizei uniformCount, const GLuint* pUniformIndices, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, int, uint*, uint, int*, void> glGetActiveUniformsiv;
		[ExternFunction(AltNames = new string[] { "glGetActiveUniformNameARB" })]
		[NativeType("void glGetActiveUniformName(GLuint program, GLuint uniformIndex, GLsizei bufSize, GLsizei* pLength, char* pUniformName)")]
		public delegate* unmanaged<uint, uint, int, out int, byte*, void> glGetActiveUniformName;
		[ExternFunction(AltNames = new string[] { "glGetUniformBlockIndexARB" })]
		[NativeType("GLuint glGetUniformBlockIndex(GLuint program, const char* uniformBlockName)")]
		public delegate* unmanaged<uint, byte*, uint> glGetUniformBlockIndex;
		[ExternFunction(AltNames = new string[] { "glGetActuveUniformBlockivARB" })]
		[NativeType("void glGetActiveUniformBlockiv(GLuint program, GLuint uniformBlockIndex, GLenum pname, GLint* pValue)")]
		public delegate* unmanaged<uint, uint, uint, int*, void> glGetActiveUniformBlockiv;
		[ExternFunction(AltNames = new string[] { "glGetActiveUniformBlockNameARB" })]
		[NativeType("void glGetActiveUniformBlockName(GLuint program, GLuint uniformBlockIndex, GLsizei bufSize, GLsizei* pLength, char* pUniformBlockName)")]
		public delegate* unmanaged<uint, uint, int, out int, byte*, void> glGetActiveUniformBlockName;
		[ExternFunction(AltNames = new string[] { "glBindBufferRangeARB" })]
		[NativeType("void glBindBufferRange(GLenum target, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size)")]
		public delegate* unmanaged<uint, uint, uint, nint, nint, void> glBindBufferRange;
		[ExternFunction(AltNames = new string[] { "glBindBufferBaseARB" })]
		[NativeType("void glBindBufferBase(GLenum target, GLuint index, GLuint buffer)")]
		public delegate* unmanaged<uint, uint, uint, void> glBindBufferBase;
		[ExternFunction(AltNames = new string[] { "glGetIntegeri_vARB" })]
		[NativeType("void glGetIntegeri_v(GLenum target, GLuint index, GLint* pData)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetIntegeri_v;
		[ExternFunction(AltNames = new string[] { "glUniformBlockBindingARB" })]
		[NativeType("void glUniformBlockBinding(GLuint program, GLuint uniformBlockIndex, GLuint uniformBlockBinding)")]
		public delegate* unmanaged<uint, uint, uint, void> glUniformBlockBinding;

	}

	public class ARBUniformBufferObject : IGLObject {

		public GL GL { get; }
		public ARBUniformBufferObjectFunctions Functions { get; } = new();

		public ARBUniformBufferObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		public uint GetUniformIndices(uint program, ReadOnlySpan<char> name) {
			unsafe {
				uint index = 0;
				fixed(byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					Functions.glGetUniformIndices(program, 1, &pName, &index);
				}
				return index;
			}
		}

		public Span<uint> GetUniformIndices(uint program, Span<uint> indices, params string[] names) {
			using MemoryStack sp = MemoryStack.Push();
			unsafe {
				fixed(uint* pIndices = indices) {
					fixed (IntPtr* ppNames = MemoryUtil.StackallocUTF8Array(names, stackalloc IntPtr[names.Length], sp)) {
						Functions.glGetUniformIndices(program, names.Length, (byte**)ppNames, pIndices);
					}
				}
			}
			return indices;
		}

		public int[] GetActiveUniforms(uint program, in ReadOnlySpan<uint> uniformIndices, GLGetActiveUniform pname) {
			int[] vals = new int[uniformIndices.Length];
			unsafe {
				fixed(int* pVals = vals) {
					fixed(uint* pUniformIndices = uniformIndices) {
						Functions.glGetActiveUniformsiv(program, uniformIndices.Length, pUniformIndices, (uint)pname, pVals);
					}
				}
			}
			return vals;
		}

		public int[] GetActiveUniforms(uint program, GLGetActiveUniform pname, params uint[] uniformIndices) {
			int[] vals = new int[uniformIndices.Length];
			unsafe {
				fixed (int* pVals = vals) {
					fixed (uint* pUniformIndices = uniformIndices) {
						Functions.glGetActiveUniformsiv(program, uniformIndices.Length, pUniformIndices, (uint)pname, pVals);
					}
				}
			}
			return vals;
		}

		public string GetActiveUniformName(uint program, uint uniformIndex) {
			int len = GL.GL20!.GetProgram(program, GLGetProgram.ActiveUniformMaxLength);
			Span<byte> name = stackalloc byte[len];
			unsafe {
				fixed(byte* pName = name) {
					Functions.glGetActiveUniformName(program, uniformIndex, len, out len, pName);
				}
			}
			return MemoryUtil.GetASCII(name[0..len]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetUniformBlockIndex(uint program, string uniformBlockName) {
			unsafe { 
				fixed(byte* pUniformBlockName = MemoryUtil.StackallocUTF8(uniformBlockName, stackalloc byte[256])) {
					return Functions.glGetUniformBlockIndex(program, pUniformBlockName);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetActiveUniformBlock(uint program, uint uniformBlockIndex, GLGetActiveUniformBlock pname) {
			int val = 0;
			unsafe {
				Functions.glGetActiveUniformBlockiv(program, uniformBlockIndex, (uint)pname, &val);
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetActiveUniformBlock(uint program, uint uniformBlockIndex, GLGetActiveUniformBlock pname, Span<int> vals) {
			unsafe {
				fixed(int* pVals = vals) {
					Functions.glGetActiveUniformBlockiv(program, uniformBlockIndex, (uint)pname, pVals);
				}
			}
			return vals;
		}

		public string GetActiveUniformBlockName(uint program, uint uniformBlockIndex) {
			int len = GL.GL20!.GetProgram(program, GLGetProgram.ActiveUniformBlockMaxNameLength);
			Span<byte> name = stackalloc byte[len];
			unsafe {
				fixed(byte* pName = name) {
					Functions.glGetActiveUniformBlockName(program, uniformBlockIndex, len, out len, pName);
				}
			}
			return MemoryUtil.GetASCII(name[0..len]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferRange(GLBufferRangeTarget target, uint index, uint buffer, nint offset, nint size) {
			unsafe {
				Functions.glBindBufferRange((uint)target, index, buffer, offset, size);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferBase(GLBufferRangeTarget target, uint index, uint buffer) {
			unsafe {
				Functions.glBindBufferBase((uint)target, index, buffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetInteger(uint pname, uint index, Span<int> val) {
			unsafe {
				fixed(int* pVal = val) {
					Functions.glGetIntegeri_v(pname, index, pVal);
				}
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetInteger(uint pname, uint index) {
			int val = 0;
			unsafe {
				Functions.glGetIntegeri_v(pname, index, &val);
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) {
			unsafe {
				Functions.glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);
			}
		}
	}
}

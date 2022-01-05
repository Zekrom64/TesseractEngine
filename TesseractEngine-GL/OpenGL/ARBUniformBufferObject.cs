using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBUniformBufferObjectFunctions {

		public delegate void PFN_glGetUniformIndices(uint program, int uniformCount, [NativeType("const char* const*")] IntPtr uniformNames, [NativeType("GLuint*")] IntPtr uniformIndices);
		[ExternFunction(AltNames = new string[] { "glGetUniformIndicesARB" })]
		public PFN_glGetUniformIndices glGetUniformIndices;
		public delegate void PFN_glGetActiveUniformsiv(uint program, int uniformCount, [NativeType("const GLuint*")] IntPtr uniformIndices, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetActiveUniformsivARB" })]
		public PFN_glGetActiveUniformsiv glGetActiveUniformsiv;
		public delegate void PFN_glGetActiveUniformName(uint program, uint uniformIndex, int bufSize, out int length, [NativeType("char*")] IntPtr uniformName);
		[ExternFunction(AltNames = new string[] { "glGetActiveUniformNameARB" })]
		public PFN_glGetActiveUniformName glGetActiveUniformName;
		public delegate uint PFN_glGetUniformBlockIndex(uint program, [MarshalAs(UnmanagedType.LPStr)] string uniformBlockName);
		[ExternFunction(AltNames = new string[] { "glGetUniformBlockIndexARB" })]
		public PFN_glGetUniformBlockIndex glGetUniformBlockIndex;
		public delegate void PFN_glGetActiveUniformBlockiv(uint program, uint uniformBlockIndex, uint pname, [NativeType("GLint*")] IntPtr value);
		[ExternFunction(AltNames = new string[] { "glGetActuveUniformBlockivARB" })]
		public PFN_glGetActiveUniformBlockiv glGetActiveUniformBlockiv;
		public delegate void PFN_glGetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, out int length, [NativeType("char*")] IntPtr uniformBlockName);
		[ExternFunction(AltNames = new string[] { "glGetActiveUniformBlockNameARB" })]
		public PFN_glGetActiveUniformBlockName glGetActiveUniformBlockName;
		public delegate void PFN_glBindBufferRange(uint target, uint index, uint buffer, nint offset, nint size);
		[ExternFunction(AltNames = new string[] { "glBindBufferRangeARB" })]
		public PFN_glBindBufferRange glBindBufferRange;
		public delegate void PFN_glBindBufferBase(uint target, uint index, uint buffer);
		[ExternFunction(AltNames = new string[] { "glBindBufferBaseARB" })]
		public PFN_glBindBufferBase glBindBufferBase;
		public delegate void PFN_glGetIntegeri_v(uint target, uint index, [NativeType("GLint*")] IntPtr data);
		[ExternFunction(AltNames = new string[] { "glGetIntegeri_vARB" })]
		public PFN_glGetIntegeri_v glGetIntegeri_v;
		public delegate void PFN_glUniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding);
		[ExternFunction(AltNames = new string[] { "glUniformBlockBindingARB" })]
		public PFN_glUniformBlockBinding glUniformBlockBinding;

	}
#nullable restore

	public class ARBUniformBufferObject : IGLObject {

		public GL GL { get; }
		public ARBUniformBufferObjectFunctions Functions { get; } = new();

		public ARBUniformBufferObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		public uint[] GetUniformIndices(uint program, params string[] names) {
			using MemoryStack sp = MemoryStack.Push();
			uint[] indices = new uint[names.Length];
			unsafe {
				fixed (uint* pIndices = indices) {
					Functions.glGetUniformIndices(program, names.Length, sp.ASCIIArray(names), (IntPtr)pIndices);
				}
			}
			return indices;
		}

		public int[] GetActiveUniforms(uint program, in ReadOnlySpan<uint> uniformIndices, GLGetActiveUniform pname) {
			int[] vals = new int[uniformIndices.Length];
			unsafe {
				fixed(int* pVals = vals) {
					fixed(uint* pUniformIndices = uniformIndices) {
						Functions.glGetActiveUniformsiv(program, uniformIndices.Length, (IntPtr)pUniformIndices, (uint)pname, (IntPtr)pVals);
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
						Functions.glGetActiveUniformsiv(program, uniformIndices.Length, (IntPtr)pUniformIndices, (uint)pname, (IntPtr)pVals);
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
					Functions.glGetActiveUniformName(program, uniformIndex, len, out len, (IntPtr)pName);
				}
			}
			return MemoryUtil.GetASCII(name[0..len]);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetUniformBlockIndex(uint program, string uniformBlockName) => Functions.glGetUniformBlockIndex(program, uniformBlockName);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetActiveUniformBlock(uint program, uint uniformBlockIndex, GLGetActiveUniformBlock pname) {
			int val = 0;
			unsafe {
				Functions.glGetActiveUniformBlockiv(program, uniformBlockIndex, (uint)pname, (IntPtr)(&val));
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetActiveUniformBlock(uint program, uint uniformBlockIndex, GLGetActiveUniformBlock pname, Span<int> vals) {
			unsafe {
				fixed(int* pVals = vals) {
					Functions.glGetActiveUniformBlockiv(program, uniformBlockIndex, (uint)pname, (IntPtr)pVals);
				}
			}
			return vals;
		}

		public string GetActiveUniformBlockName(uint program, uint uniformBlockIndex) {
			int len = GL.GL20!.GetProgram(program, GLGetProgram.ActiveUniformBlockMaxNameLength);
			Span<byte> name = stackalloc byte[len];
			unsafe {
				fixed(byte* pName = name) {
					Functions.glGetActiveUniformBlockName(program, uniformBlockIndex, len, out len, (IntPtr)pName);
				}
			}
			return MemoryUtil.GetASCII(name[0..len]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferRange(GLBufferRangeTarget target, uint index, uint buffer, nint offset, nint size) => Functions.glBindBufferRange((uint)target, index, buffer, offset, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferBase(GLBufferRangeTarget target, uint index, uint buffer) => Functions.glBindBufferBase((uint)target, index, buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetInteger(uint pname, uint index, Span<int> val) {
			unsafe {
				fixed(int* pVal = val) {
					Functions.glGetIntegeri_v(pname, index, (IntPtr)pVal);
				}
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetInteger(uint pname, uint index) {
			int val = 0;
			unsafe {
				Functions.glGetIntegeri_v(pname, index, (IntPtr)(&val));
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) => Functions.glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);

	}
}

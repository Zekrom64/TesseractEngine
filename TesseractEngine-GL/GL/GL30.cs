using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.GL.Native;

namespace Tesseract.GL {

	public class GL30 : IGLObject {

		public GL GL { get; }
		public GL30Functions Functions { get; }

		public GL30(GL gl, IGLContext context) {
			GL = gl;
			Functions = new GL30Functions();
			Library.LoadFunctions(name => context.GetGLProcAddress(name), Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginConditionalRender(uint id, GLQueryMode mode) => Functions.glBeginConditionalRender(id, (uint)mode);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginTransformFeedback(GLDrawMode mode) => Functions.glBeginTransformFeedback((uint)mode);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFragDataLocation(uint program, uint colorNumber, string name) => Functions.glBindFragDataLocation(program, colorNumber, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClampColor(GLClampColorTarget target, bool clamp) => Functions.glClampColor((uint)target, clamp ? GLEnums.GL_TRUE : GLEnums.GL_FALSE);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBuffer(GLClearBuffer buffer, int drawbuffer, float depth, int stencil) => Functions.glClearBufferfi((uint)buffer, drawbuffer, depth, stencil);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBuffer(GLClearBuffer buffer, int drawbuffer, in Span<int> value) {
			unsafe {
				fixed(int* pValue = value) {
					Functions.glClearBufferiv((uint)buffer, drawbuffer, (IntPtr)pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBuffer(GLClearBuffer buffer, int drawbuffer, in Span<float> value) {
			unsafe {
				fixed (float* pValue = value) {
					Functions.glClearBufferfv((uint)buffer, drawbuffer, (IntPtr)pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBuffer(GLClearBuffer buffer, int drawbuffer, in Span<uint> value) {
			unsafe {
				fixed (uint* pValue = value) {
					Functions.glClearBufferuiv((uint)buffer, drawbuffer, (IntPtr)pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ColorMask(uint buf, bool red, bool green, bool blue, bool alpha) => Functions.glColorMaski(buf, (byte)(red ? 1 : 0), (byte)(green ? 1 : 0), (byte)(blue ? 1 : 0), (byte)(alpha ? 1 : 0));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Disable(GLIndexedCapability cap, uint index) => Functions.glDisablei((uint)cap, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Enable(GLIndexedCapability cap, uint index) => Functions.glEnablei((uint)cap, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndConditionalRender() => Functions.glEndConditionalRender();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndTransformFeedback() => Functions.glEndTransformFeedback();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetBoolean(uint pname, uint index) {
			Functions.glGetBooleani_v(pname, index, out byte data);
			return data != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFragDataLocation(uint program, string name) => Functions.glGetFragDataLocation(program, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetString(uint pname, uint index) => MemoryUtil.GetStringASCII(Functions.glGetStringi(pname, index));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTexParameter(GLTextureTarget target, GLTexParamter pname, in Span<int> param) {
			unsafe {
				fixed(int* pParams = param) {
					Functions.glGetTexParameteriiv((uint)target, (uint)pname, (IntPtr)pParams);
				}
			}
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetTexParameter(GLTextureTarget target, GLTexParamter pname, in Span<uint> param) {
			unsafe {
				fixed (uint* pParams = param) {
					Functions.glGetTexParameteriiv((uint)target, (uint)pname, (IntPtr)pParams);
				}
			}
			return param;
		}

		public void GetTransformFeedbackVarying(uint program, uint index, out int size, out GLShaderAttribType type, out string name) {
			int maxLen = GL.GL20.GetProgram(program, GLGetProgram.TransformFeedbackVaryingMaxLength);
			Span<byte> nameBytes = stackalloc byte[maxLen];
			uint gltype = 0;
			unsafe {
				fixed(byte* pName = nameBytes) {
					Functions.glGetTransformFeedbackVarying(program, index, maxLen, out maxLen, out size, out gltype, (IntPtr)pName);
				}
			}
			type = (GLShaderAttribType)gltype;
			name = MemoryUtil.GetStringASCII(nameBytes);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetVertexAttrib(uint index, GLGetVertexAttrib pname, in Span<int> param) {
			unsafe {
				fixed(int* pParam = param) {
					Functions.glGetVertexAttribiiv(index, (uint)pname, (IntPtr)pParam);
				}
			}
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetVertexAttrib(uint index, GLGetVertexAttrib pname, in Span<uint> param) {
			unsafe {
				fixed (uint* pParam = param) {
					Functions.glGetVertexAttribiuiv(index, (uint)pname, (IntPtr)pParam);
				}
			}
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, in Span<int> param) {
			unsafe {
				fixed(int* pParam = param) {
					Functions.glTexParameteriiv((uint)target, (uint)pname, (IntPtr)pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, in Span<uint> param) {
			unsafe {
				fixed (uint* pParam = param) {
					Functions.glTexParameteriuiv((uint)target, (uint)pname, (IntPtr)pParam);
				}
			}
		}

		public void TransformFeedbackVaryings(uint program, in Span<string> varyings, GLFeedbackBufferMode bufferMode) {
			using MemoryStack sp = MemoryStack.Current;
			UnmanagedPointer<IntPtr> pVaryings = sp.ASCIIArray(varyings);
			Functions.glTransformFeedbackVaryings(program, varyings.Length, pVaryings.Ptr, (uint)bufferMode);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribIPointer(uint index, int size, GLType type, int stride, nint offset) => Functions.glVertexAttribiPointer(index, size, (uint)type, stride, offset);

	}

}

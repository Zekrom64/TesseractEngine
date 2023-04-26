using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBGetProgramBinaryFunctions {

		[ExternFunction(AltNames = new string[] { "glGetProgramBinaryARB" })]
		[NativeType("void glGetProgramBinary(GLuint program, GLsizei bufSize, GLsizei* pLength, GLenum* pBinaryFormat, void* pBinary)")]
		public delegate* unmanaged<uint, int, out int, out uint, IntPtr, void> glGetProgramBinary;
		[ExternFunction(AltNames = new string[] { "glProgramBinaryARB" })]
		[NativeType("void glProgramBinary(GLuint program, GLenum binaryFormat, void* pBinary)")]
		public delegate* unmanaged<uint, uint, IntPtr, void> glProgramBinary;
		[ExternFunction(AltNames = new string[] { "glProgramParameteriARB" })]
		[NativeType("void glProgramParameteri(GLuint program, GLenum pname, GLint value)")]
		public delegate* unmanaged<uint, uint, int, void> glProgramParameteri;

	}

	public class ARBGetProgramBinary : IGLObject {

		public GL GL { get; }
		public ARBGetProgramBinaryFunctions Functions { get; } = new();

		public ARBGetProgramBinary(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<byte> GetProgramBinary(uint program, out uint binaryFormat, Span<byte> binary) {
			int length = 0;
			unsafe {
				fixed(byte* pBinary = binary) {
					Functions.glGetProgramBinary(program, binary.Length, out length, out binaryFormat, (IntPtr)pBinary);
				}
			}
			return binary[..length];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte[] GetProgramBinary(uint program, out uint binaryFormat) {
			int length = GL.GL20!.GetProgram(program, GLGetProgram.ProgramBinaryLength);
			byte[] binary = new byte[length];
			unsafe {
				fixed(byte* pBinary = binary) {
					Functions.glGetProgramBinary(program, length, out int _, out binaryFormat, (IntPtr)pBinary);
				}
			}
			return binary;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramBinary(uint program, uint binaryFormat, in ReadOnlySpan<byte> binary) {
			unsafe {
				fixed(byte* pBinary = binary) {
					Functions.glProgramBinary(program, binaryFormat, (IntPtr)pBinary);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramParameter(uint program, GLProgramParameter pname, int value) {
			unsafe {
				Functions.glProgramParameteri(program, (uint)pname, value);
			}
		}
	}

}

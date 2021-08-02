using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.GL {

	public class ARBGetProgramBinaryFunctions {

		public delegate void PFN_glGetProgramBinary(uint program, int bufSize, out int length, out uint binaryFormat, IntPtr binary);
		[ExternFunction(AltNames = new string[] { "glGetProgramBinaryARB" })]
		public PFN_glGetProgramBinary glGetProgramBinary;
		public delegate void PFN_glProgramBinary(uint program, uint binaryFormat, IntPtr binary);
		[ExternFunction(AltNames = new string[] { "glProgramBinaryARB" })]
		public PFN_glProgramBinary glProgramBinary;
		public delegate void PFN_glProgramParameteri(uint program, uint pname, int value);
		[ExternFunction(AltNames = new string[] { "glProgramParameteriARB" })]
		public PFN_glProgramParameteri glProgramParameteri;

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
			int length = GL.GL20.GetProgram(program, GLGetProgram.ProgramBinaryLength);
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
		public void ProgramParameter(uint program, GLProgramParameter pname, int value) => Functions.glProgramParameteri(program, (uint)pname, value);

	}

}

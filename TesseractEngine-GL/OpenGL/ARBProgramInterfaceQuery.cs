using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBProgramInterfaceQueryFunctions {

		public delegate void PFN_glGetProgramInterfaceiv(uint program, uint programInterface, uint pname, [NativeType("GLint*")] IntPtr values);
		[ExternFunction(AltNames = new string[] { "glGetProgramInterfaceivARB" })]
		public PFN_glGetProgramInterfaceiv glGetProgramInterfaceiv;
		public delegate uint PFN_glGetProgramResourceIndex(uint program, uint programInterface, [MarshalAs(UnmanagedType.LPStr)] string name);
		[ExternFunction(AltNames = new string[] { "glGetProgramResourceIndexARB" })]
		public PFN_glGetProgramResourceIndex glGetProgramResourceIndex;
		public delegate void PFN_glGetProgramResourceName(uint program, uint programInterface, uint index, int bufSize, out int length, [NativeType("char*")] IntPtr name);
		[ExternFunction(AltNames = new string[] { "glGetProgramResourceName" })]
		public PFN_glGetProgramResourceName glGetProgramResourceName;
		public delegate void PFN_glGetProgramResourceiv(uint program, uint programInterface, uint index, int propCount, [NativeType("const GLenum*")] IntPtr props, int bufSize, out int length, [NativeType("GLint*")] IntPtr values);
		[ExternFunction(AltNames = new string[] { "glGetProgramResourceivARB" })]
		public PFN_glGetProgramResourceiv glGetProgramResourceiv;
		public delegate int PFN_glGetProgramResourceLocation(uint program, uint programInterface, [MarshalAs(UnmanagedType.LPStr)] string name);
		[ExternFunction(AltNames = new string[] { "glGetProgramResourceLocationARB" })]
		public PFN_glGetProgramResourceLocation glGetProgramResourceLocation;
		public delegate int PFN_glGetProgramResourceLocationIndex(uint program, uint programInterface, [MarshalAs(UnmanagedType.LPStr)] string name);
		[ExternFunction(AltNames = new string[] { "glGetProgramResourceLocationIndexARB" })]
		public PFN_glGetProgramResourceLocationIndex glGetProgramResourceLocationIndex;

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
					Functions.glGetProgramInterfaceiv(program, (uint)programInterface, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetProgramResourceIndex(uint program, GLProgramInterface programInterface, string name) => Functions.glGetProgramResourceIndex(program, (uint)programInterface, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetProgramResourceName(uint program, GLProgramInterface programInterface, uint index) {
			int length = GetProgramInterface(program, programInterface, GLGetProgramInterface.MaxNameLength, stackalloc int[1])[0];
			Span<byte> name = stackalloc byte[length];
			unsafe {
				fixed(byte* pName = name) {
					Functions.glGetProgramResourceName(program, (uint)programInterface, index, length, out length, (IntPtr)pName);
				}
			}
			return MemoryUtil.GetASCII(name[..length]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetProgramResource(uint program, GLProgramInterface programInterface, uint index, in ReadOnlySpan<GLGetProgramResource> props, Span<int> values) {
			unsafe {
				fixed(GLGetProgramResource* pProps = props) {
					fixed(int* pValues = values) {
						Functions.glGetProgramResourceiv(program, (uint)programInterface, index, props.Length, (IntPtr)pProps, values.Length, out int _, (IntPtr)pValues);
					}
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramResourceLocation(uint program, GLProgramInterface programInterface, string name) => Functions.glGetProgramResourceLocation(program, (uint)programInterface, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramResourceLocationIndex(uint program, GLProgramInterface programInterface, string name) => Functions.glGetProgramResourceLocationIndex(program, (uint)programInterface, name);

	}
}

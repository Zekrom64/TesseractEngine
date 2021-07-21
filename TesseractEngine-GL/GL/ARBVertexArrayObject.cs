using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.GL {
	
	public class ARBVertexArrayObjectFunctions {

		public delegate void PFN_glBindVertexArray(uint array);
		[ExternFunction(AltNames = new string[] { "glBindVertexArrayARB" })]
		public PFN_glBindVertexArray glBindVertexArray;
		public delegate void PFN_glDeleteVertexArrays(int n, [NativeType("const GLuint*")] IntPtr arrays);
		[ExternFunction(AltNames = new string[] { "glDeleteVertexArraysARB" })]
		public PFN_glDeleteVertexArrays glDeleteVertexArrays;
		public delegate void PFN_glGenVertexArrays(int n, [NativeType("GLuint*")] IntPtr arrays);
		[ExternFunction(AltNames = new string[] { "glGenVertexArraysARB" })]
		public PFN_glGenVertexArrays glGenVertexArrays;
		public delegate byte PFN_glIsVertexArray(uint array);
		[ExternFunction(AltNames = new string[] { "glIsVertexArrayARB" })]
		public PFN_glIsVertexArray glIsVertexArray;

	}

	public class ARBVertexArrayObject {

		public GL GL { get; }
		public ARBVertexArrayObjectFunctions Functions { get; } = new();

		public ARBVertexArrayObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexArray(uint array) => Functions.glBindVertexArray(array);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteVertexArrays(in ReadOnlySpan<uint> arrays) {
			unsafe {
				fixed(uint* pArrays = arrays) {
					Functions.glDeleteVertexArrays(arrays.Length, (IntPtr)pArrays);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteVertexArrays(params uint[] arrays) {
			unsafe {
				fixed (uint* pArrays = arrays) {
					Functions.glDeleteVertexArrays(arrays.Length, (IntPtr)pArrays);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteVertexArrays(uint array) {
			unsafe {
				Functions.glDeleteVertexArrays(1, (IntPtr)(&array));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenVertexArrays(Span<uint> arrays) {
			unsafe {
				fixed(uint* pArrays = arrays) {
					Functions.glGenVertexArrays(arrays.Length, (IntPtr)pArrays);
				}
			}
			return arrays;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenVertexArrays(int n) {
			uint[] arrays = new uint[n];
			unsafe {
				fixed (uint* pArrays = arrays) {
					Functions.glGenVertexArrays(n, (IntPtr)pArrays);
				}
			}
			return arrays;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenVertexArrays() {
			uint array = 0;
			unsafe {
				Functions.glGenVertexArrays(1, (IntPtr)(&array));
			}
			return array;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsVertexArray(uint array) => Functions.glIsVertexArray(array) != 0;

	}

}

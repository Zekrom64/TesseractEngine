using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBVertexArrayObjectFunctions {

		[ExternFunction(AltNames = new string[] { "glBindVertexArrayARB" })]
		[NativeType("void glBindVertexArray(GLuint vertexArray)")]
		public delegate* unmanaged<uint, void> glBindVertexArray;
		[ExternFunction(AltNames = new string[] { "glDeleteVertexArraysARB" })]
		[NativeType("void glDeleteVertexArrays(GLsizei n, const GLuint* pArrays)")]
		public delegate* unmanaged<int, uint*, void> glDeleteVertexArrays;
		[ExternFunction(AltNames = new string[] { "glGenVertexArraysARB" })]
		[NativeType("void glGenVertexArrays(GLsizei n, GLuint* pArrays)")]
		public delegate* unmanaged<int, uint*, void> glGenVertexArrays;
		[ExternFunction(AltNames = new string[] { "glIsVertexArrayARB" })]
		[NativeType("GLboolean glIsVertexArray(GLuint vertexArray)")]
		public delegate* unmanaged<uint, byte> glIsVertexArray;

	}

	public class ARBVertexArrayObject {

		public GL GL { get; }
		public ARBVertexArrayObjectFunctions Functions { get; } = new();

		public ARBVertexArrayObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexArray(uint array) {
			unsafe {
				Functions.glBindVertexArray(array);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteVertexArrays(in ReadOnlySpan<uint> arrays) {
			unsafe {
				fixed(uint* pArrays = arrays) {
					Functions.glDeleteVertexArrays(arrays.Length, pArrays);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteVertexArrays(params uint[] arrays) {
			unsafe {
				fixed (uint* pArrays = arrays) {
					Functions.glDeleteVertexArrays(arrays.Length, pArrays);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteVertexArrays(uint array) {
			unsafe {
				Functions.glDeleteVertexArrays(1, &array);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenVertexArrays(Span<uint> arrays) {
			unsafe {
				fixed(uint* pArrays = arrays) {
					Functions.glGenVertexArrays(arrays.Length, pArrays);
				}
			}
			return arrays;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenVertexArrays(int n) {
			uint[] arrays = new uint[n];
			unsafe {
				fixed (uint* pArrays = arrays) {
					Functions.glGenVertexArrays(n, pArrays);
				}
			}
			return arrays;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenVertexArrays() {
			uint array = 0;
			unsafe {
				Functions.glGenVertexArrays(1, &array);
			}
			return array;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsVertexArray(uint array) {
			unsafe {
				return Functions.glIsVertexArray(array) != 0;
			}
		}
	}

}

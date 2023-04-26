using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class EXTDrawBuffers2Functions {

		[ExternFunction(AltNames = new string[] { "glColorMaskIndexedEXT" })]
		[NativeType("void glColorMaskIndexedEXT(GLenum buf, GLboolean r, GLboolean g, GLboolean b, GLboolean a)")]
		public delegate* unmanaged<uint, byte, byte, byte, byte, void> glColorMaski;
		[ExternFunction(AltNames = new string[] { "glGetBooleanIndexedvEXT" })]
		[NativeType("void glGetBooleani_v(GLenum target, GLuint index, GLboolean* pData)")]
		public delegate* unmanaged<uint, uint, byte*, void> glGetBooleani_v;
		[ExternFunction(AltNames = new string[] { "glGetIntegerIndexedvEXT" })]
		[NativeType("void glGetIntegerIndexedvEXT(GLenum target, GLuint index, GLint* pData)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetIntegeri_v;
		[ExternFunction(AltNames = new string[] { "glEnableIndexedEXT" })]
		[NativeType("void glEnableIndexedEXT(GLenum target, GLuint index)")]
		public delegate* unmanaged<uint, uint, void> glEnablei;
		[ExternFunction(AltNames = new string[] { "glDisableIndexedEXT" })]
		[NativeType("void glDisableIndexedEXT(GLenum target, GLuint index)")]
		public delegate* unmanaged<uint, uint, void> glDisablei;
		[ExternFunction(AltNames = new string[] { "glIsEnabledIndexedEXT" })]
		[NativeType("GLboolean glIsEnabledIndexedEXT(GLenum target, GLuint index)")]
		public delegate* unmanaged<uint, uint, byte> glIsEnabledi;

	}

	public class EXTDrawBuffers2 : IGLObject {

		public GL GL { get; }
		public EXTDrawBuffers2Functions Functions { get; } = new();

		public EXTDrawBuffers2(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ColorMaskIndexed(uint buffer, bool r, bool g, bool b, bool a) {
			unsafe {
				Functions.glColorMaski(buffer, (byte)(r ? 1 : 0), (byte)(g ? 1 : 0), (byte)(b ? 1 : 0), (byte)(a ? 1 : 0));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetBoolean(GLIndexedCapability cap, uint index) {
			unsafe {
				byte data = 0;
				Functions.glGetBooleani_v((uint)cap, index, &data);
				return data != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Enable(GLIndexedCapability cap, uint index) {
			unsafe {
				Functions.glEnablei((uint)cap, index);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Disable(GLIndexedCapability cap, uint index) {
			unsafe {
				Functions.glDisablei((uint)cap, index);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsEnabled(GLIndexedCapability cap, uint index) {
			unsafe {
				return Functions.glIsEnabledi((uint)cap, index) != 0;
			}
		}
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class EXTDrawBuffers2Functions {

		public delegate void PFN_glColorMaski(uint buf, byte r, byte g, byte b, byte a);
		[ExternFunction(AltNames = new string[] { "glColorMaskIndexedEXT" })]
		public PFN_glColorMaski glColorMaski;
		public delegate void PFN_glGetBooleani_v(uint target, uint index, [NativeType("GLboolean*")] IntPtr data);
		[ExternFunction(AltNames = new string[] { "glGetBooleanIndexedvEXT" })]
		public PFN_glGetBooleani_v glGetBooleani_v;
		public delegate void PFN_glGetIntegeri_v(uint target, uint index, [NativeType("GLboolean*")] IntPtr data);
		[ExternFunction(AltNames = new string[] { "glGetIntegerIndexedvEXT" })]
		public PFN_glGetIntegeri_v glGetIntegeri_v;
		public delegate void PFN_glEnablei(uint target, uint index);
		[ExternFunction(AltNames = new string[] { "glEnableIndexedEXT" })]
		public PFN_glEnablei glEnablei;
		public delegate void PFN_glDisablei(uint target, uint index);
		[ExternFunction(AltNames = new string[] { "glDisableIndexedEXT" })]
		public PFN_glDisablei glDisablei;
		public delegate byte PFN_glIsEnabledi(uint target, uint index);
		[ExternFunction(AltNames = new string[] { "glIsEnabledIndexedEXT" })]
		public PFN_glIsEnabledi glIsEnabledi;

	}
#nullable restore

	public class EXTDrawBuffers2 : IGLObject {

		public GL GL { get; }
		public EXTDrawBuffers2Functions Functions { get; } = new();

		public EXTDrawBuffers2(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ColorMaskIndexed(uint buffer, bool r, bool g, bool b, bool a) => Functions.glColorMaski(buffer, (byte)(r ? 1 : 0), (byte)(g ? 1 : 0), (byte)(b ? 1 : 0), (byte)(a ? 1 : 0));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetBoolean(GLIndexedCapability cap, uint index) {
			unsafe {
				byte data = 0;
				Functions.glGetBooleani_v((uint)cap, index, (IntPtr)(&data));
				return data != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Enable(GLIndexedCapability cap, uint index) => Functions.glEnablei((uint)cap, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Disable(GLIndexedCapability cap, uint index) => Functions.glDisablei((uint)cap, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsEnabled(GLIndexedCapability cap, uint index) => Functions.glIsEnabledi((uint)cap, index) != 0;

	}

}

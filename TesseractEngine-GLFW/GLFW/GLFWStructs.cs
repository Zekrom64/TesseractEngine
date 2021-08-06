using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.GLFW {
	
	[StructLayout(LayoutKind.Sequential)]
	public struct GLFWVidMode {

		public int Width;

		public int Height;

		public int RedBits;

		public int GreenBits;

		public int BlueBits;

		public int RefreshRate;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct GLFWGammaRamp {

		[NativeType("unsigned short*")]
		public IntPtr Red;

		[NativeType("unsigned short*")]
		public IntPtr Green;

		[NativeType("unsigned short*")]
		public IntPtr Blue;

		public uint Size;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct GLFWImage {

		public int Width;

		public int Height;

		[NativeType("const char*")]
		public IntPtr Pixels;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct GLFWGamepadState {

		public byte Button0;
		public byte Button1;
		public byte Button2;
		public byte Button3;
		public byte Button4;
		public byte Button5;
		public byte Button6;
		public byte Button7;
		public byte Button8;
		public byte Button9;
		public byte Button10;
		public byte Button11;
		public byte Button12;
		public byte Button13;
		public byte Button14;

		public ReadOnlySpan<GLFWButtonState> Buttons {
			get {
				GLFWButtonState[] buttons = new GLFWButtonState[15];
				ref byte button0 = ref Button0;
				unsafe {
					fixed (byte* pButton0 = &button0) {
						for (int i = 0; i < 15; i++) buttons[i] = (GLFWButtonState)pButton0[i];
					}
				}
				return buttons;
			}
			set {
				ref byte button0 = ref Button0;
				unsafe {
					fixed (byte* pButton0 = &button0) {
						for (int i = 0; i < Math.Max(15, value.Length); i++)
							pButton0[i] = (byte)value[i];
					}
				}
			}
		}

		public GLFWButtonState this[GLFWGamepadButton button] {
			get {
				int i = (int)button;
				if (i < 0 || i > 14) throw new IndexOutOfRangeException();
				ref byte button0 = ref Button0;
				unsafe {
					fixed(byte* pButton0 = &button0) {
						return (GLFWButtonState)pButton0[i];
					}
				}
			}
			set {
				int i = (int)button;
				if (i < 0 || i > 14) throw new IndexOutOfRangeException();
				ref byte button0 = ref Button0;
				unsafe {
					fixed (byte* pButton0 = &button0) {
						pButton0[i] = (byte)value;
					}
				}
			}
		}

		public float Axis0;
		public float Axis1;
		public float Axis2;
		public float Axis3;
		public float Axis4;
		public float Axis5;

	}

}

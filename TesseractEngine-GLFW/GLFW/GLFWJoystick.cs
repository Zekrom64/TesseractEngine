using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.GLFW {
	
	public struct GLFWJoystick {

		public int ID { get; init; }

		public bool Present {
			get {
				unsafe {
					return GLFW3.Functions.glfwJoystickPresent(ID);
				}
			}
		}

		public ReadOnlySpan<float> Axes {
			get {
				unsafe {
					IntPtr pAxes = GLFW3.Functions.glfwGetJoystickAxes(ID, out int count);
					return new ReadOnlySpan<float>((void*)pAxes, count);
				}
			}
		}

		public ReadOnlySpan<GLFWButtonState> Buttons {
			get {
				unsafe {
					IntPtr pButtons = GLFW3.Functions.glfwGetJoystickButtons(ID, out int count);
					return new ReadOnlySpan<GLFWButtonState>((void*)pButtons, count);
				}
			}
		}

		public ReadOnlySpan<GLFWHat> Hats {
			get {
				unsafe {
					IntPtr pHats = GLFW3.Functions.glfwGetJoystickHats(ID, out int count);
					return new ReadOnlySpan<GLFWHat>((void*)pHats, count);
				}
			}
		}

		public string? Name {
			get {
				unsafe {
					return MemoryUtil.GetUTF8(GLFW3.Functions.glfwGetJoystickName(ID));
				}
			}
		}

		public Guid? GUID {
			get {
				unsafe {
					string? str = MemoryUtil.GetUTF8(GLFW3.Functions.glfwGetJoystickGUID(ID));
					if (str == null) return null;
					return Guid.Parse(str);
				}
			}
		}

		public IntPtr UserPointer {
			get {
				unsafe {
					return GLFW3.Functions.glfwGetJoystickUserPointer(ID);
				}
			}
			set {
				unsafe {
					GLFW3.Functions.glfwSetJoystickUserPointer(ID, value);
				}
			}
		}

		public bool IsGamepad {
			get {
				unsafe {
					return GLFW3.Functions.glfwJoystickIsGamepad(ID);
				}
			}
		}

		public string? GamepadName {
			get {
				unsafe {
					return MemoryUtil.GetUTF8(GLFW3.Functions.glfwGetGamepadName(ID));
				}
			}
		}

		public GLFWGamepadState? GamepadState {
			get {
				unsafe {
					if (!GLFW3.Functions.glfwGetGamepadState(ID, out GLFWGamepadState state)) return null;
					return state;
				}
			}
		}

	}

}

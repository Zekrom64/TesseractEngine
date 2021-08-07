using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.GLFW {
	
	public struct GLFWJoystick {

		public int ID { get; init; }

		public bool Present => GLFW3.Functions.glfwJoystickPresent(ID);

		public ReadOnlySpan<float> Axes {
			get {
				IntPtr pAxes = GLFW3.Functions.glfwGetJoystickAxes(ID, out int count);
				unsafe {
					return new ReadOnlySpan<float>((void*)pAxes, count);
				}
			}
		}

		public ReadOnlySpan<GLFWButtonState> Buttons {
			get {
				IntPtr pButtons = GLFW3.Functions.glfwGetJoystickButtons(ID, out int count);
				unsafe {
					return new ReadOnlySpan<GLFWButtonState>((void*)pButtons, count);
				}
			}
		}

		public ReadOnlySpan<GLFWHat> Hats {
			get {
				IntPtr pHats = GLFW3.Functions.glfwGetJoystickHats(ID, out int count);
				unsafe {
					return new ReadOnlySpan<GLFWHat>((void*)pHats, count);
				}
			}
		}

		public string Name => MemoryUtil.GetStringUTF8(GLFW3.Functions.glfwGetJoystickName(ID));

		public Guid GUID => Guid.Parse(MemoryUtil.GetStringUTF8(GLFW3.Functions.glfwGetJoystickGUID(ID)));

		public IntPtr UserPointer {
			get => GLFW3.Functions.glfwGetJoystickUserPointer(ID);
			set => GLFW3.Functions.glfwSetJoystickUserPointer(ID, value);
		}

		public bool IsGamepad => GLFW3.Functions.glfwJoystickIsGamepad(ID);

		public string GamepadName => MemoryUtil.GetStringUTF8(GLFW3.Functions.glfwGetGamepadName(ID));

		public GLFWGamepadState? GamepadState {
			get {
				if (!GLFW3.Functions.glfwGetGamepadState(ID, out GLFWGamepadState state)) return null;
				return state;
			}
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.GLFW {

	public struct GLFWMonitor : IEquatable<GLFWMonitor> {

		[NativeType("GLFWmonitor*")]
		public IntPtr Monitor { get; init; }

		public Vector2i Pos {
			get {
				GLFW3.Functions.glfwGetMonitorPos(Monitor, out int x, out int y);
				return new(x, y);
			}
		}

		public Recti WorkArea {
			get {
				GLFW3.Functions.glfwGetMonitorWorkArea(Monitor, out int x, out int y, out int w, out int h);
				return new(x, y, w, h);
			}
		}

		public Vector2i PhysicalSize {
			get {
				GLFW3.Functions.glfwGetMonitorPhysicalSize(Monitor, out int w, out int h);
				return new(w, h);
			}
		}

		public Vector2 ContentScale {
			get {
				GLFW3.Functions.glfwGetMonitorContentScale(Monitor, out float x, out float y);
				return new(x, y);
			}
		}

		public string Name => MemoryUtil.GetStringUTF8(GLFW3.Functions.glfwGetMonitorName(Monitor));

		public IntPtr UserPointer {
			get => GLFW3.Functions.glfwGetMonitorUserPointer(Monitor);
			set => GLFW3.Functions.glfwSetMonitorUserPointer(Monitor, value);
		}

		public ReadOnlySpan<GLFWVidMode> VideoModes {
			get {
				IntPtr pModes = GLFW3.Functions.glfwGetVideoModes(Monitor, out int count);
				unsafe {
					return new((void*)pModes, count);
				}
			}
		}

		public GLFWVidMode VideoMode => new UnmanagedPointer<GLFWVidMode>(GLFW3.Functions.glfwGetVideoMode(Monitor)).Value;

		public float Gamma {
			set => GLFW3.Functions.glfwSetGamma(Monitor, value);
		}

		public GLFWGammaRamp GammaRamp {
			get => new UnmanagedPointer<GLFWGammaRamp>(GLFW3.Functions.glfwGetGammaRamp(Monitor)).Value;
			set => GLFW3.Functions.glfwSetGammaRamp(Monitor, value);
		}

		public static bool operator ==(GLFWMonitor m1, GLFWMonitor m2) => m1.Monitor == m2.Monitor;

		public static bool operator !=(GLFWMonitor m1, GLFWMonitor m2) => m1.Monitor != m2.Monitor;

		public bool Equals(GLFWMonitor m) => Monitor == m.Monitor;

		public override bool Equals(object o) => o is GLFWMonitor m && Equals(m);

		public override int GetHashCode() => Monitor.GetHashCode();

		public static implicit operator bool(GLFWMonitor monitor) => monitor.Monitor != IntPtr.Zero;

	}

}

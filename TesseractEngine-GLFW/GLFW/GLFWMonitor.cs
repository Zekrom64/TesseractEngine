using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.GLFW {

	public record struct GLFWMonitor : IEquatable<GLFWMonitor> {

		[NativeType("GLFWmonitor*")]
		public IntPtr Monitor { get; init; }

		public Vector2i Pos {
			get {
				if (Monitor == IntPtr.Zero) return default;
				GLFW3.Functions.glfwGetMonitorPos(Monitor, out int x, out int y);
				return new(x, y);
			}
		}

		public Recti WorkArea {
			get {
				if (Monitor == IntPtr.Zero) return default;
				GLFW3.Functions.glfwGetMonitorWorkarea(Monitor, out int x, out int y, out int w, out int h);
				return new(x, y, w, h);
			}
		}

		public Vector2i PhysicalSize {
			get {
				if (Monitor == IntPtr.Zero) return default;
				GLFW3.Functions.glfwGetMonitorPhysicalSize(Monitor, out int w, out int h);
				return new(w, h);
			}
		}

		public Vector2 ContentScale {
			get {
				if (Monitor == IntPtr.Zero) return default;
				GLFW3.Functions.glfwGetMonitorContentScale(Monitor, out float x, out float y);
				return new(x, y);
			}
		}

		public string? Name => Monitor == IntPtr.Zero ? null : MemoryUtil.GetUTF8(GLFW3.Functions.glfwGetMonitorName(Monitor));

		public IntPtr UserPointer {
			get => Monitor == IntPtr.Zero ? IntPtr.Zero : GLFW3.Functions.glfwGetMonitorUserPointer(Monitor);
			set {
				if (Monitor != IntPtr.Zero) GLFW3.Functions.glfwSetMonitorUserPointer(Monitor, value);
			}
		}

		public ReadOnlySpan<GLFWVidMode> VideoModes {
			get {
				if (Monitor == IntPtr.Zero) return default;
				IntPtr pModes = GLFW3.Functions.glfwGetVideoModes(Monitor, out int count);
				unsafe {
					return new((void*)pModes, count);
				}
			}
		}

		public GLFWVidMode VideoMode => Monitor == IntPtr.Zero ? default : new UnmanagedPointer<GLFWVidMode>(GLFW3.Functions.glfwGetVideoMode(Monitor)).Value;

		public float Gamma {
			set {
				if (Monitor != IntPtr.Zero) GLFW3.Functions.glfwSetGamma(Monitor, value);
			}
		}

		public GLFWGammaRamp GammaRamp {
			get => Monitor == IntPtr.Zero ? default : new UnmanagedPointer<GLFWGammaRamp>(GLFW3.Functions.glfwGetGammaRamp(Monitor)).Value;
			set {
				if (Monitor != IntPtr.Zero) GLFW3.Functions.glfwSetGammaRamp(Monitor, value);
			}
		}

		public static implicit operator bool(GLFWMonitor monitor) => monitor.Monitor != IntPtr.Zero;

	}

}

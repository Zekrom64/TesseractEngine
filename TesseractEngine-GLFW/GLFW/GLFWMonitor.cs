using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.GLFW {

	public readonly record struct GLFWMonitor : IEquatable<GLFWMonitor> {

		[NativeType("GLFWmonitor*")]
		public IntPtr Monitor { get; init; }

		public Vector2i Pos {
			get {
				if (Monitor == IntPtr.Zero) return default;
				unsafe {
					GLFW3.Functions.glfwGetMonitorPos(Monitor, out int x, out int y);
					return new(x, y);
				}
			}
		}

		public Recti WorkArea {
			get {
				if (Monitor == IntPtr.Zero) return default;
				unsafe {
					GLFW3.Functions.glfwGetMonitorWorkarea(Monitor, out int x, out int y, out int w, out int h);
					return new(x, y, w, h);
				}
			}
		}

		public Vector2i PhysicalSize {
			get {
				if (Monitor == IntPtr.Zero) return default;
				unsafe {
					GLFW3.Functions.glfwGetMonitorPhysicalSize(Monitor, out int w, out int h);
					return new(w, h);
				}
			}
		}

		public Vector2 ContentScale {
			get {
				if (Monitor == IntPtr.Zero) return default;
				unsafe {
					GLFW3.Functions.glfwGetMonitorContentScale(Monitor, out float x, out float y);
					return new(x, y);
				}
			}
		}

		public string? Name {
			get {
				unsafe {
					return Monitor == IntPtr.Zero ? null : MemoryUtil.GetUTF8(GLFW3.Functions.glfwGetMonitorName(Monitor));
				}
			}
		}

		public IntPtr UserPointer {
			get {
				unsafe {
					return Monitor == IntPtr.Zero ? IntPtr.Zero : GLFW3.Functions.glfwGetMonitorUserPointer(Monitor);
				}
			}
			set {
				if (Monitor != IntPtr.Zero) {
					unsafe {
						GLFW3.Functions.glfwSetMonitorUserPointer(Monitor, value);
					}
				}
			}
		}

		public ReadOnlySpan<GLFWVidMode> VideoModes {
			get {
				if (Monitor == IntPtr.Zero) return default;
				unsafe {
					GLFWVidMode* pModes = GLFW3.Functions.glfwGetVideoModes(Monitor, out int count);
					return new((void*)pModes, count);
				}
			}
		}

		public GLFWVidMode VideoMode {
			get {
				unsafe {
					return Monitor == IntPtr.Zero ? default : *GLFW3.Functions.glfwGetVideoMode(Monitor);
				}
			}
		}

		public float Gamma {
			set {
				if (Monitor != IntPtr.Zero) {
					unsafe {
						GLFW3.Functions.glfwSetGamma(Monitor, value);
					}
				}
			}
		}

		public GLFWGammaRamp GammaRamp {
			get {
				unsafe {
					return Monitor == IntPtr.Zero ? default : *GLFW3.Functions.glfwGetGammaRamp(Monitor);
				}
			}
			set {
				if (Monitor != IntPtr.Zero) {
					unsafe {
						GLFW3.Functions.glfwSetGammaRamp(Monitor, value);
					}
				}
			}
		}

		public static implicit operator bool(GLFWMonitor monitor) => monitor.Monitor != IntPtr.Zero;

	}

}

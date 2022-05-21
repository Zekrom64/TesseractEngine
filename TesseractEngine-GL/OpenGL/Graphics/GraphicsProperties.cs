using System;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Util;

namespace Tesseract.OpenGL.Graphics {

	/// <summary>
	/// Interface providing memory information for OpenGL graphics instances. There is no
	/// standard way to get memory usage or even available memory and only some statistics
	/// are provided by a mix of vendor-specific extensions.
	/// </summary>
	public interface IGLGraphicsMemory {

		public ulong TotalVideoMemory { get; }

		public ulong TotalDeviceMemory { get; }

		public ulong TotalCommittedMemory { get; }

	}

	public class UnknownGLGraphicsMemory : IGLGraphicsMemory {

		public ulong TotalVideoMemory => 0;

		public ulong TotalDeviceMemory => 0;

		public ulong TotalCommittedMemory => 0;

	}

	public class NVXGLGraphicsMemory : IGLObject, IGLGraphicsMemory {

		private const ulong ScaleFactor = 1024; // Units are in kb

		public GL GL { get; }

		public ulong TotalVideoMemory { get; }

		public ulong TotalDeviceMemory { get; }

		public ulong TotalCommittedMemory => TotalVideoMemory - ((ulong)GL.GL11.GetInteger(Native.GLEnums.GL_GPU_MEMORY_INFO_CURRENT_AVAILABLE_VIDMEM_NVX) * ScaleFactor);

		public NVXGLGraphicsMemory(GL gl) {
			GL = gl;
			TotalVideoMemory = (ulong)gl.GL11.GetInteger(Native.GLEnums.GL_GPU_MEMORY_INFO_TOTAL_AVAILABLE_MEMORY_NVX) * ScaleFactor;
			TotalDeviceMemory = (ulong)gl.GL11.GetInteger(Native.GLEnums.GL_GPU_MEMORY_INFO_DEDICATED_VIDMEM_NVX) * ScaleFactor;
		}

	}

	public class WGLAMDGLGraphicsMemory : IGLGraphicsMemory {

		private const ulong ScaleFactor = 1000000; // Units are in MB

		public ulong TotalVideoMemory { get; }

		public ulong TotalDeviceMemory => 0;

		public ulong TotalCommittedMemory => 0;

		public WGLAMDGLGraphicsMemory(GL gl) {
			IntPtr hglrc = gl.WGL!.CurrentContext;
			uint gpuid = gl.WGLAMDGPUAssociation!.GetContextGPUID(hglrc);
			TotalVideoMemory = ScaleFactor * gl.WGLAMDGPUAssociation!.GetGPUInfo<uint>(gpuid, GLGetGPUInfoWGLAMD.RAM);
		}

	}

	public class GLGraphicsProperties : IGraphicsProperites {

		public GraphicsType Type => GraphicsType.OpenGL;

		public string TypeInfo => "Tesseract GLCore Graphics";

		public string RendererName { get; }

		public string VendorName { get; }

		public ThreadSafetyLevel APIThreadSafety => ThreadSafetyLevel.SingleThread;

		private readonly IGLGraphicsMemory memory;

		public ulong TotalVideoMemory => memory.TotalVideoMemory;

		public ulong TotalDeviceMemory => memory.TotalDeviceMemory;

		public ulong TotalCommittedMemory => memory.TotalCommittedMemory;

		public CoordinateSystem CoordinateSystem => CoordinateSystem.LeftHanded;

		public GLGraphicsProperties(GL gl) {
			RendererName = gl.GL11.GetString(Native.GLEnums.GL_RENDERER)!;
			VendorName = gl.GL11.GetString(Native.GLEnums.GL_VENDOR)!;

			if (gl.WGLAMDGPUAssociation != null) memory = new WGLAMDGLGraphicsMemory(gl);
			else if (gl.NVXGPUMemoryInfo) memory = new NVXGLGraphicsMemory(gl);
			else memory = new UnknownGLGraphicsMemory();
		}

	}

}

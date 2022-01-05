using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Util;

namespace Tesseract.OpenGL.Graphics {

	public class GLGraphicsProperties : IGraphicsProperites {

		public GraphicsType Type => GraphicsType.OpenGL;

		public string TypeInfo => "Tesseract GLCore Graphics";

		public string RendererName { get; }

		public string VendorName { get; }

		public ThreadSafetyLevel APIThreadSafety => ThreadSafetyLevel.SingleThread;

		// TODO: Use extensions to reflect video memory info
		public ulong TotalVideoMemory => 0;

		public ulong TotalDeviceMemory => 0;

		public ulong TotalCommittedMemory => 0;

		public GLGraphicsProperties(GL gl) {
			RendererName = gl.GL11.GetString(Native.GLEnums.GL_RENDERER)!;
			VendorName = gl.GL11.GetString(Native.GLEnums.GL_VENDOR)!;
		}

	}

}

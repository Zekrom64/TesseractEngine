using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.GL.Native;

namespace Tesseract.GL {

	public delegate void GLDebugProc(GLDebugSource source, GLDebugType type, uint id, GLDebugSeverity severity, int length, [MarshalAs(UnmanagedType.LPStr)] string message, IntPtr userParam);

}

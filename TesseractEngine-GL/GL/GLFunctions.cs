using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.GL.Native;

namespace Tesseract.GL {

	public delegate void GLDebugProc(GLDebugSource source, GLDebugType type, uint id, GLDebugSeverity severity, int length, [NativeType("const GLchar*")] IntPtr message, IntPtr userParam);

}

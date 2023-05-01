using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	// We assume that the message is encoded in null-terminated UTF-8 in the managed implementation
	public delegate void GLDebugProc(GLDebugSource source, GLDebugType type, uint id, GLDebugSeverity severity, int length, [MarshalAs(UnmanagedType.LPUTF8Str)] string message, IntPtr userParam);

	public delegate void PFN_glGetInteger(uint name, out int value);

	[return: NativeType("const GLubyte*")]
	public delegate IntPtr PFN_glGetString(uint name);
	[return: NativeType("const GLubyte*")]
	public delegate IntPtr PFN_glGetStringi(uint name, uint index);

}

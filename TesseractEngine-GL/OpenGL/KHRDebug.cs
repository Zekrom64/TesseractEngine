using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class KHRDebugFunctions {

		public delegate void PFN_glDebugMessageControl(uint source, uint type, uint severity, int count, [NativeType("const GLuint*")] IntPtr ids, byte enabled);
		public delegate void PFN_glDebugMessageInsert(uint source, uint type, uint id, uint severity, int length, [NativeType("const GLchar*")] IntPtr buf);
		public delegate void PFN_glDebugMessageCallback([MarshalAs(UnmanagedType.FunctionPtr)] GLDebugProc callback, IntPtr userParam);
		public delegate uint PFN_glGetDebugMessageLog(uint count, int bufSize, [NativeType("GLenum*")] IntPtr sources, [NativeType("GLenum*")] IntPtr types, [NativeType("GLuint*")] IntPtr ids, [NativeType("GLenum*")] IntPtr severities, [NativeType("GLsizei*")] IntPtr lengths, [NativeType("char*")] IntPtr messageLog);
		public delegate void PFN_glGetPointerv(uint pname, [NativeType("void**")] IntPtr _params);
		public delegate void PFN_glPushDebugGroup(uint source, uint id, int length, [MarshalAs(UnmanagedType.LPStr)] string message);
		public delegate void PFN_glPopDebugGroup();
		public delegate void PFN_glObjectLabel(uint identifier, uint name, int length, [MarshalAs(UnmanagedType.LPStr)] string label);
		public delegate void PFN_glGetObjectLabel(uint identifier, uint name, int bufSize, out int length, [NativeType("char*")] IntPtr label);
		public delegate void PFN_glObjectPtrLabel(IntPtr ptr, int length, [MarshalAs(UnmanagedType.LPStr)] string label);
		public delegate void PFN_glGetObjectPtrLabel(IntPtr ptr, int bufSize, out int length, [NativeType("char*")] IntPtr label);

		[ExternFunction(AltNames = new string[] { "glDebugMessageControlARB" })]
		public PFN_glDebugMessageControl glDebugMessageControl;
		[ExternFunction(AltNames = new string[] { "glDebugMessageInsertARB" })]
		public PFN_glDebugMessageInsert glDebugMessageInsert;
		[ExternFunction(AltNames = new string[] { "glDebugMessageCallbackARB" })]
		public PFN_glDebugMessageCallback glDebugMessageCallback;
		public PFN_glGetDebugMessageLog glGetDebugMessageLog;
		public PFN_glGetPointerv glGetPointerv;
		public PFN_glPushDebugGroup glPushDebugGroup;
		public PFN_glPopDebugGroup glPopDebugGroup;
		public PFN_glObjectLabel glObjectLabel;
		public PFN_glGetObjectLabel glGetObjectLabel;
		public PFN_glObjectPtrLabel glObjectPtrLabel;
		public PFN_glGetObjectPtrLabel glGetObjectPtrLabel;

	}

	public class KHRDebug : IGLObject {

		public GL GL { get; }
		public KHRDebugFunctions Functions { get; } = new();

		public KHRDebug(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		public void DebugMessageControl(GLDebugSource source, GLDebugType type, GLDebugSeverity severity, in ReadOnlySpan<uint> ids, bool enabled) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glDebugMessageControl((uint)source, (uint)type, (uint)severity, ids.Length, (IntPtr)pIds, (byte)(enabled ? 1 : 0));
				}
			}
		}

		public void DebugMessageInsert(GLDebugSource source, GLDebugType type, uint id, GLDebugSeverity severity, string message) {
			//Functions.glDebugMessageInsert((uint)source, (uint)type, id, (uint)severity, )
		}

	}

}

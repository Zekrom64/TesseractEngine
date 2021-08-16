using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	public class KHRDebugFunctions {

		public delegate void PFN_glDebugMessageControl(uint source, uint type, uint severity, int count, [NativeType("const GLuint*")] IntPtr ids, byte enabled);
		public delegate void PFN_glDebugMessageInsert(uint source, uint type, uint id, uint severity, int length, [NativeType("const char*")] IntPtr buf);
		public delegate void PFN_glDebugMessageCallback([MarshalAs(UnmanagedType.FunctionPtr)] GLDebugProc callback, IntPtr userParam);
		public delegate uint PFN_glGetDebugMessageLog(uint count, int bufSize, [NativeType("GLenum*")] IntPtr sources, [NativeType("GLenum*")] IntPtr types, [NativeType("GLuint*")] IntPtr ids, [NativeType("GLenum*")] IntPtr severities, [NativeType("GLsizei*")] IntPtr lengths, [NativeType("char*")] IntPtr messageLog);
		public delegate void PFN_glGetPointerv(uint pname, [NativeType("void**")] IntPtr _params);
		public delegate void PFN_glPushDebugGroup(uint source, uint id, int length, [NativeType("const char*")] IntPtr message);
		public delegate void PFN_glPopDebugGroup();
		public delegate void PFN_glObjectLabel(uint identifier, uint name, int length, [NativeType("const char*")] IntPtr label);
		public delegate void PFN_glGetObjectLabel(uint identifier, uint name, int bufSize, out int length, [NativeType("char*")] IntPtr label);
		public delegate void PFN_glObjectPtrLabel(IntPtr ptr, int length, [NativeType("const char*")] IntPtr label);
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DebugMessageControl(GLDebugSource source, GLDebugType type, GLDebugSeverity severity, in ReadOnlySpan<uint> ids, bool enabled) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glDebugMessageControl((uint)source, (uint)type, (uint)severity, ids.Length, (IntPtr)pIds, (byte)(enabled ? 1 : 0));
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DebugMessageInsert(GLDebugSource source, GLDebugType type, uint id, GLDebugSeverity severity, string message) {
			byte[] utf8 = Encoding.UTF8.GetBytes(message + "\0");
			unsafe {
				fixed(byte* pUtf8 = utf8) {
					Functions.glDebugMessageInsert((uint)source, (uint)type, id, (uint)severity, utf8.Length - 1, (IntPtr)pUtf8);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DebugMessageCallback(GLDebugProc callback, IntPtr userParam) => Functions.glDebugMessageCallback(callback, userParam);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<IntPtr> GetPointer(uint pname, Span<IntPtr> values) {
			unsafe {
				fixed(IntPtr* pValues = values) {
					Functions.glGetPointerv(pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushDebugGroup(GLDebugSource source, uint id, string message) {
			byte[] utf8 = Encoding.UTF8.GetBytes(message + "\0");
			unsafe {
				fixed(byte* pUtf8 = utf8) {
					Functions.glPushDebugGroup((uint)source, id, utf8.Length - 1, (IntPtr)pUtf8);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PopDebugGroup() => Functions.glPopDebugGroup();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ObjectLabel(GLObjectLabelIdentifier identifier, uint name, string label) {
			byte[] utf8 = Encoding.UTF8.GetBytes(label + "\0");
			unsafe {
				fixed(byte* pUtf8 = utf8) {
					Functions.glObjectLabel((uint)identifier, name, utf8.Length - 1, (IntPtr)pUtf8);
				}
			}
		}

		public string GetObjectLabel(GLObjectLabelIdentifier identifier, uint name) {
			int length = GL.GL11.GetInteger(GLEnums.GL_MAX_LABEL_LENGTH);
			Span<byte> lblBytes = stackalloc byte[length];
			unsafe {
				fixed(byte* pBytes = lblBytes) {
					Functions.glGetObjectLabel((uint)identifier, name, length, out length, (IntPtr)pBytes);
				}
			}
			return MemoryUtil.GetStringUTF8(lblBytes[..length]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ObjectPtrLabel(IntPtr ptr, string label) {
			byte[] utf8 = Encoding.UTF8.GetBytes(label + "\0");
			unsafe {
				fixed (byte* pUtf8 = utf8) {
					Functions.glObjectPtrLabel(ptr, utf8.Length - 1, (IntPtr)pUtf8);
				}
			}
		}

		public string GetObjectPtrLabel(IntPtr ptr) {
			int length = GL.GL11.GetInteger(GLEnums.GL_MAX_LABEL_LENGTH);
			Span<byte> lblBytes = stackalloc byte[length];
			unsafe {
				fixed (byte* pBytes = lblBytes) {
					Functions.glGetObjectPtrLabel(ptr, length, out length, (IntPtr)pBytes);
				}
			}
			return MemoryUtil.GetStringUTF8(lblBytes[..length]);
		}

	}

}

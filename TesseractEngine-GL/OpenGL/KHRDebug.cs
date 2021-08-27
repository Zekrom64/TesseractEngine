using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	public class KHRDebugFunctions {

		public delegate void PFN_glDebugMessageControl(uint source, uint type, uint severity, int count, [NativeType("const GLuint*")] IntPtr ids, byte enabled);
		public delegate void PFN_glDebugMessageInsert(uint source, uint type, uint id, uint severity, int length, [NativeType("const GLchar*")] IntPtr buf);
		public delegate void PFN_glDebugMessageCallback([MarshalAs(UnmanagedType.FunctionPtr)] GLDebugProc callback, IntPtr userParam);
		public delegate uint PFN_glGetDebugMessageLog(uint count, int bufSize, [NativeType("GLenum*")] IntPtr sources, [NativeType("GLenum*")] IntPtr types, [NativeType("GLuint*")] IntPtr ids, [NativeType("GLenum*")] IntPtr severities, [NativeType("GLsizei*")] IntPtr lengths, [NativeType("char*")] IntPtr messageLog);
		public delegate void PFN_glGetPointerv(uint pname, [NativeType("void**")] IntPtr _params);
		public delegate void PFN_glPushDebugGroup(uint source, uint id, int length, [NativeType("const GLuint*")] IntPtr message);
		public delegate void PFN_glPopDebugGroup();
		public delegate void PFN_glObjectLabel(uint identifier, uint name, int length, [NativeType("const GLuint*")] IntPtr label);
		public delegate void PFN_glGetObjectLabel(uint identifier, uint name, int bufSize, out int length, [NativeType("char*")] IntPtr label);
		public delegate void PFN_glObjectPtrLabel(IntPtr ptr, int length, [NativeType("const GLuint*")] IntPtr label);
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

		/* This extension is a bit tricky for managed code because it operates on arbitrary byte strings. For
		 * the best compatibility we implicitly use UTF-8 strings because they use byte-based characters,
		 * have some compatibility with ANSI/ASCII strings that are normally used, and can store extended
		 * Unicode characters.
		 */

		public void DebugMessageControl(GLDebugSource source, GLDebugType type, GLDebugSeverity severity, in ReadOnlySpan<uint> ids, bool enabled) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glDebugMessageControl((uint)source, (uint)type, (uint)severity, ids.Length, (IntPtr)pIds, (byte)(enabled ? 1 : 0));
				}
			}
		}

		public void DebugMessageInsert(GLDebugSource source, GLDebugType type, uint id, GLDebugSeverity severity, string message) {
			using ManagedPointer<byte> pMessage = new(Encoding.UTF8.GetBytes(message + "\0"));
			Functions.glDebugMessageInsert((uint)source, (uint)type, id, (uint)severity, pMessage.Count - 1, pMessage);
		}

		public void DebugMessageCallback(GLDebugProc callback, IntPtr userParam) => Functions.glDebugMessageCallback(callback, userParam);

		// We ignore the lengths paramter in GetDebugMessageLog because it is too much work to convert from UTF-8 byte lengths to character offsets

		public uint GetDebugMessageLog(int bufSize, Span<GLDebugSource> sources, Span<GLDebugType> types, Span<uint> ids, Span<GLDebugSeverity> severities, out string messageLog) {
			int count = ExMath.Min(sources.Length, types.Length, ids.Length, severities.Length);
			uint ret;
			ManagedPointer<byte> msglog = new(bufSize);
			unsafe {
				fixed(GLDebugSource* pSources = sources) {
					fixed(GLDebugType* pTypes = types) {
						fixed(uint* pIds = ids) {
							fixed(GLDebugSeverity* pSeverities = severities) {
								ret = Functions.glGetDebugMessageLog((uint)count, bufSize, (IntPtr)pSources, (IntPtr)pTypes, (IntPtr)pIds, (IntPtr)pSeverities, IntPtr.Zero, msglog.Ptr);
							}
						}
					}
				}
			}
			messageLog = MemoryUtil.GetStringUTF8(msglog.Ptr);
			return ret;
		}

		public Span<IntPtr> GetPointer(uint pname, Span<IntPtr> values) {
			unsafe {
				fixed(IntPtr* pValues = values) {
					Functions.glGetPointerv(pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		public void PushDebugGroup(GLDebugSource source, uint id, string message) {
			using ManagedPointer<byte> pMessage = new(Encoding.UTF8.GetBytes(message + "\0"));
			Functions.glPushDebugGroup((uint)source, id, pMessage.Count - 1, pMessage);
		}

		public void PopDebugGroup() => Functions.glPopDebugGroup();

		public void ObjectLabel(GLIdentifier identifier, uint name, string label) {
			using ManagedPointer<byte> pLabel = new(Encoding.UTF8.GetBytes(label + "\0"));
			Functions.glObjectLabel((uint)identifier, name, pLabel.Count - 1, pLabel);
		}

		public string GetObjectLabel(GLIdentifier identifier, uint name) {
			int length = GL.GL11.GetInteger(GLEnums.GL_MAX_LABEL_LENGTH);
			Span<byte> nameBytes = stackalloc byte[length];
			unsafe {
				fixed(byte* pName = nameBytes) {
					Functions.glGetObjectLabel((uint)identifier, name, length, out length, (IntPtr)pName);
				}
			}
			return Encoding.UTF8.GetString(nameBytes[..length]);
		}

		public void ObjectPtrLabel(IntPtr ptr, string label) {
			using ManagedPointer<byte> pLabel = new(Encoding.UTF8.GetBytes(label + "\0"));
			Functions.glObjectPtrLabel(ptr, pLabel.Count - 1, pLabel);
		}

		public string GetObjectPtrLabel(IntPtr ptr) {
			int length = GL.GL11.GetInteger(GLEnums.GL_MAX_LABEL_LENGTH);
			Span<byte> nameBytes = stackalloc byte[length];
			unsafe {
				fixed (byte* pName = nameBytes) {
					Functions.glGetObjectPtrLabel(ptr, length, out length, (IntPtr)pName);
				}
			}
			return Encoding.UTF8.GetString(nameBytes[..length]);
		}

	}

}

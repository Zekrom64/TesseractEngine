using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	public unsafe class KHRDebugFunctions {

		[ExternFunction(AltNames = new string[] { "glDebugMessageControlARB" })]
		[NativeType("void glDebugMessageControl(GLenum source, GLenum type, GLenum severity, GLint count, const GLuint* pIDs, GLboolean enabled)")]
		public delegate* unmanaged<uint, uint, uint, int, uint*, byte, void> glDebugMessageControl;
		[ExternFunction(AltNames = new string[] { "glDebugMessageInsertARB" })]
		[NativeType("void glDebugMessageInsert(GLenum source, GLenum type, GLuint id, GLenum severity, GLint length, const GLchar* pBuf)")]
		public delegate* unmanaged<uint, uint, uint, uint, int, byte*, void> glDebugMessageInsert;
		[ExternFunction(AltNames = new string[] { "glDebugMessageCallbackARB" })]
		[NativeType("void glDebugMessageCallback(GLDEBUGPROC pCallback, void* pUserParam)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> glDebugMessageCallback;
		[NativeType("GLenum glGetDebugMessageLog(GLuint count, GLint bufSize, GLenum* pSources, GLenum* pTypes, GLuint* pIDs, GLenum* pSeverities, GLsizei* pLengths, char* pMessageLog)")]
		public delegate* unmanaged<uint, int, uint*, uint*, uint*, uint*, nint*, byte*, uint> glGetDebugMessageLog;
		[NativeType("void glGetPointerv(GLenum pname, void** pParams)")]
		public delegate* unmanaged<uint, IntPtr*, void> glGetPointerv;
		[NativeType("void glPushDebugGroup(GLenum source, GLuint id, GLint length, const char* pMessage)")]
		public delegate* unmanaged<uint, uint, int, byte*, void> glPushDebugGroup;
		[NativeType("void glPopDebugGroup()")]
		public delegate* unmanaged<void> glPopDebugGroup;
		[NativeType("void glObjectLabel(GLenum identifier, GLuint name, GLint length, const char* label)")]
		public delegate* unmanaged<uint, uint, int, byte*, void> glObjectLabel;
		[NativeType("void glGetObjectLabel(GLenum identifier, GLuint name, GLsizei bufSize, GLsizei* pLength, char* pLabel)")]
		public delegate* unmanaged<uint, uint, int, out int, byte*, void> glGetObjectLabel;
		[NativeType("void glObjectPtrLabel(void* ptr, GLsizei length, const char* label)")]
		public delegate* unmanaged<IntPtr, int, byte*, void> glObjectPtrLabel;
		[NativeType("void glGetObjectPtrLabel(void* ptr, GLsizei bufSize, GLsizei* pLength, char* pLabel)")]
		public delegate* unmanaged<IntPtr, int, out int, byte*, void>  glGetObjectPtrLabel;

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
					Functions.glDebugMessageControl((uint)source, (uint)type, (uint)severity, ids.Length, pIds, (byte)(enabled ? 1 : 0));
				}
			}
		}

		public void DebugMessageInsert(GLDebugSource source, GLDebugType type, uint id, GLDebugSeverity severity, string message) {
			unsafe {
				Span<byte> strMessage = MemoryUtil.StackallocUTF8(message, stackalloc byte[1024]);
				fixed(byte* pMessage = strMessage) {
					Functions.glDebugMessageInsert((uint)source, (uint)type, id, (uint)severity, strMessage.Length - 1, pMessage);
				}
			}
		}

		public void DebugMessageCallback(GLDebugProc callback, IntPtr userParam) {
			unsafe {
				Functions.glDebugMessageCallback(Marshal.GetFunctionPointerForDelegate(callback), userParam);
			}
		}

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
								ret = Functions.glGetDebugMessageLog((uint)count, bufSize, (uint*)pSources, (uint*)pTypes, pIds, (uint*)pSeverities, (nint*)0, (byte*)msglog.Ptr);
							}
						}
					}
				}
			}
			messageLog = MemoryUtil.GetUTF8(msglog.Ptr)!;
			return ret;
		}

		public Span<IntPtr> GetPointer(uint pname, Span<IntPtr> values) {
			unsafe {
				fixed(IntPtr* pValues = values) {
					Functions.glGetPointerv(pname, pValues);
				}
			}
			return values;
		}

		public void PushDebugGroup(GLDebugSource source, uint id, string message) {
			Span<byte> strMessage = MemoryUtil.StackallocUTF8(message, stackalloc byte[4096]);
			unsafe {
				fixed(byte* pMessage = strMessage) {
					Functions.glPushDebugGroup((uint)source, id, strMessage.Length - 1, pMessage);
				}
			}
		}

		public void PopDebugGroup() {
			unsafe {
				Functions.glPopDebugGroup();
			}
		}

		public void ObjectLabel(GLIdentifier identifier, uint name, string label) {
			Span<byte> strLabel = MemoryUtil.StackallocUTF8(label, stackalloc byte[256]);
			unsafe {
				fixed(byte* pLabel = strLabel) {
					Functions.glObjectLabel((uint)identifier, name, strLabel.Length - 1, pLabel);
				}
			}
		}

		public string GetObjectLabel(GLIdentifier identifier, uint name) {
			int length = GL.GL11.GetInteger(GLEnums.GL_MAX_LABEL_LENGTH);
			Span<byte> nameBytes = stackalloc byte[length];
			unsafe {
				fixed(byte* pName = nameBytes) {
					Functions.glGetObjectLabel((uint)identifier, name, length, out length, pName);
				}
			}
			return Encoding.UTF8.GetString(nameBytes[..length]);
		}

		public void ObjectPtrLabel(IntPtr ptr, string label) {
			Span<byte> strLabel = MemoryUtil.StackallocUTF8(label, stackalloc byte[256]);
			unsafe {
				fixed (byte* pLabel = strLabel) {
					Functions.glObjectPtrLabel(ptr, strLabel.Length - 1, pLabel);
				}
			}
		}

		public string GetObjectPtrLabel(IntPtr ptr) {
			int length = GL.GL11.GetInteger(GLEnums.GL_MAX_LABEL_LENGTH);
			Span<byte> nameBytes = stackalloc byte[length];
			unsafe {
				fixed (byte* pName = nameBytes) {
					Functions.glGetObjectPtrLabel(ptr, length, out length, pName);
				}
			}
			return Encoding.UTF8.GetString(nameBytes[..length]);
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	using GLenum = UInt32;
	using GLbitfield = UInt32;
	using GLuint = UInt32;
	using GLint = Int32;
	using GLsizei = Int32;
	using GLboolean = Byte;
	using GLbyte = SByte;
	using GLshort = Int16;
	using GLubyte = Byte;
	using GLushort = UInt16;
	using GLulong = UInt64;
	using GLfloat = Single;
	using GLclampf = Single;
	using GLdouble = Double;
	using GLclampd = Double;
	using GLint64 = Int64;
	using GLuint64 = UInt64;
	using GLintptr = IntPtr;
	using GLsizeiptr = IntPtr;

	public unsafe class GL15Functions {

		[NativeType("void glBeginQuery(GLenum target, GLuint id)")]
		public delegate* unmanaged<GLenum, GLuint, void> glBeginQuery;
		[NativeType("void glBindBuffer(GLenum target, GLuint buffer)")]
		public delegate* unmanaged<GLenum, GLuint, void> glBindBuffer;
		[NativeType("void glBufferData(GLenum target, GLsizeiptr size, void* data, GLenum usage)")]
		public delegate* unmanaged<GLenum, GLsizeiptr, IntPtr, GLenum, void> glBufferData;
		[NativeType("void glBufferSubData(GLenum target, GLintptr offset, GLsizeiptr size, void* data)")]
		public delegate* unmanaged<GLenum, GLintptr, GLsizeiptr, IntPtr, void> glBufferSubData;
		[NativeType("void glDeleteBuffers(GLsizei n, const GLuint* pBuffers)")]
		public delegate* unmanaged<GLsizei, GLuint*, void> glDeleteBuffers;
		[NativeType("void glDeleteQueries(GLsizei n, const GLuint* pQueries)")]
		public delegate* unmanaged<GLsizei, GLuint*, void> glDeleteQueries;
		[NativeType("void glEndQuery(GLenum target)")]
		public delegate* unmanaged<GLenum, void> glEndQuery;
		[NativeType("void glGenBuffers(GLsizei n, GLuint* pBuffers)")]
		public delegate* unmanaged<GLsizei, GLuint*, void> glGenBuffers;
		[NativeType("void glGenQueries(GLsizei n, GLuint* pQueries)")]
		public delegate* unmanaged<GLsizei, GLuint*, void> glGenQueries;
		[NativeType("void glGetBufferParameteriv(GLenum taregt, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<GLenum, GLenum, GLint*, void> glGetBufferParameteriv;
		[NativeType("void glGetBufferPointerv(GLenum target, GLenum pname, void** pParams)")]
		public delegate* unmanaged<GLenum, GLenum, IntPtr*, void> glGetBufferPointerv;
		[NativeType("void glGetBufferSubData(GLenum target, GLintptr offset, GLsizeiptr size, void* data)")]
		public delegate* unmanaged<GLenum, GLintptr, GLsizeiptr, IntPtr, void> glGetBufferSubData;
		[NativeType("void glGetQueryObjectiv(GLuint id, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<GLuint, GLenum, GLint*, void> glGetQueryObjectiv;
		[NativeType("void glGetQueryObjectuiv(GLuint id, GLenum pname, GLuint* pParams)")]
		public delegate* unmanaged<GLuint, GLenum, GLuint*, void> glGetQueryObjectuiv;
		[NativeType("void glGetQueryiv(GLenum target, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<GLenum, GLenum, GLint*, void> glGetQueryiv;
		[NativeType("GLboolean glIsBuffer(GLuint buffer)")]
		public delegate* unmanaged<GLuint, GLboolean> glIsBuffer;
		[NativeType("GLboolean glIsQuery(GLuint query)")]
		public delegate* unmanaged<GLuint, GLboolean> glIsQuery;
		[NativeType("void* glMapBuffer(GLenum target, GLenum access)")]
		public delegate* unmanaged<GLenum, GLenum, IntPtr> glMapBuffer;
		[NativeType("GLboolean glUnmapBuffer(GLenum target)")]
		public delegate* unmanaged<GLenum, GLboolean> glUnmapBuffer;

	}

	public class GL15 : GL14 {

		public GL15Functions FunctionsGL15 { get; } = new();

		public GL15(GL gl, IGLContext context) : base(gl, context) {
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL15);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginQuery(GLQueryTarget target, uint id) {
			unsafe {
				FunctionsGL15.glBeginQuery((uint)target, id);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffer(GLBufferTarget target, uint id) {
			unsafe {
				FunctionsGL15.glBindBuffer((uint)target, id);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferData(GLBufferTarget target, nint size, IntPtr data, GLBufferUsage usage) {
			unsafe {
				FunctionsGL15.glBufferData((uint)target, size, data, (uint)usage);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferData<T>(GLBufferTarget target, GLBufferUsage usage, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					BufferData(target, (IntPtr)(data.Length * sizeof(T)), (IntPtr)pData, usage);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferData<T>(GLBufferTarget target, GLBufferUsage usage, params T[] data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					BufferData(target, (IntPtr)(data.Length * sizeof(T)), (IntPtr)pData, usage);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferSubData(GLBufferTarget target, nint offset, nint size, IntPtr data) {
			unsafe {
				FunctionsGL15.glBufferSubData((uint)target, offset, size, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferSubData<T>(GLBufferTarget target, nint offset, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					BufferSubData(target, offset, data.Length * (nint)sizeof(T), (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferSubData<T>(GLBufferTarget target, nint offset, params T[] data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					BufferSubData(target, offset, data.Length * (nint)sizeof(T), (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(int n, IntPtr buffers) {
			unsafe {
				FunctionsGL15.glDeleteBuffers(n, (GLuint*)buffers);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(in ReadOnlySpan<uint> buffers) {
			unsafe {
				fixed(uint* pBuffers = buffers) {
					FunctionsGL15.glDeleteBuffers(buffers.Length, pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(params uint[] buffers) {
			unsafe {
				fixed (uint* pBuffers = buffers) {
					FunctionsGL15.glDeleteBuffers(buffers.Length, pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(uint buffer) {
			unsafe {
				FunctionsGL15.glDeleteBuffers(1, &buffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(int n, IntPtr ids) {
			unsafe {
				FunctionsGL15.glDeleteQueries(n, (GLuint*)ids);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(in ReadOnlySpan<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					FunctionsGL15.glDeleteQueries(ids.Length, pIds);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(params uint[] ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					FunctionsGL15.glDeleteQueries(ids.Length, pIds);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(uint query) {
			unsafe {
				FunctionsGL15.glDeleteQueries(1, &query);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndQuery(GLQueryTarget target) {
			unsafe {
				FunctionsGL15.glEndQuery((uint)target);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenBuffers(int n, IntPtr buffers) {
			unsafe {
				FunctionsGL15.glGenBuffers(n, (GLuint*)buffers);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenBuffers(in Span<uint> buffers) {
			unsafe {
				fixed (uint* pBuffers = buffers) {
					FunctionsGL15.glGenBuffers(buffers.Length, pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenBuffers(int n) {
			uint[] buffers = new uint[n];
			unsafe {
				fixed (uint* pBuffers = buffers) {
					FunctionsGL15.glGenBuffers(buffers.Length, pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenBuffers() => GenBuffers(stackalloc uint[1])[0];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenQueries(int n, IntPtr ids) {
			unsafe {
				FunctionsGL15.glGenQueries(n, (GLuint*)ids);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenQueries(in Span<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					FunctionsGL15.glGenQueries(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenQueries(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					FunctionsGL15.glGenQueries(n, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenQueries() => GenQueries(stackalloc uint[1])[0];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetBufferParameteri(GLBufferTarget target, GLGetBufferParameter pname) {
			unsafe {
				int param = 0;
				FunctionsGL15.glGetBufferParameteriv((uint)target, (uint)pname, &param);
				return param;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr GetBufferPointer(GLBufferTarget target, GLGetBufferPointer pname) {
			unsafe {
				IntPtr param = IntPtr.Zero;
				FunctionsGL15.glGetBufferPointerv((uint)target, (uint)pname, &param);
				return param;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetBufferSubData(GLBufferTarget target, nint offset, nint size, IntPtr data) {
			unsafe {
				FunctionsGL15.glGetBufferSubData((uint)target, offset, size, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetBufferSubData<T>(GLBufferTarget target, nint offset, in Span<T> data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					FunctionsGL15.glGetBufferSubData((uint)target, offset, (nint)sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
			return data;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] GetBufferSubData<T>(GLBufferTarget target, nint offset, T[] data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					FunctionsGL15.glGetBufferSubData((uint)target, offset, (nint)sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
			return data;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] GetBufferSubData<T>(GLBufferTarget target, nint offset, int count) where T : unmanaged => GetBufferSubData(target, offset, new T[count]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetQueryObjecti(uint id, GLGetQueryObject pname) {
			unsafe {
				int value = 0;
				FunctionsGL15.glGetQueryObjectiv(id, (uint)pname, &value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjecti(uint id, GLGetQueryObject pname, nint offset) {
			unsafe {
				FunctionsGL15.glGetQueryObjectiv(id, (uint)pname, (GLint*)offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetQueryObjectui(uint id, GLGetQueryObject pname) {
			unsafe {
				uint value = 0;
				FunctionsGL15.glGetQueryObjectuiv(id, (uint)pname, &value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjectui(uint id, GLGetQueryObject pname, nint offset) {
			unsafe {
				FunctionsGL15.glGetQueryObjectuiv(id, (uint)pname, (GLuint*)offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetQueryi(GLGetQueryTarget target, GLGetQuery pname) {
			unsafe {
				int value = 0;
				FunctionsGL15.glGetQueryiv((uint)target, (uint)pname, &value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsBuffer(uint buffer) {
			unsafe {
				return FunctionsGL15.glIsBuffer(buffer) != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsQuery(uint id) {
			unsafe {
				return FunctionsGL15.glIsQuery(id) != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapBuffer(GLBufferTarget target, GLAccess access) {
			unsafe {
				return FunctionsGL15.glMapBuffer((uint)target, (uint)access);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool UnmapBuffer(GLBufferTarget target) {
			unsafe {
				return FunctionsGL15.glUnmapBuffer((uint)target) != 0;
			}
		}
	}
}

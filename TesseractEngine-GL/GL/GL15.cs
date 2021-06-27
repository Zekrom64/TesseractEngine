using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.GL.Native;

namespace Tesseract.GL {
	public class GL15 : IGLObject {

		public GL GL { get; }
		public GL15Functions Functions { get; }

		public GL15(GL gl, IGLContext context) {
			GL = gl;
			Functions = new GL15Functions();
			Library.LoadFunctions(name => context.GetGLProcAddress(name), Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginQuery(GLQueryTarget target, uint id) => Functions.glBeginQuery((uint)target, id);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffer(GLBufferTarget target, uint id) => Functions.glBindBuffer((uint)target, id);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferData(GLBufferTarget target, nint size, IntPtr data, GLBufferUsage usage) => Functions.glBufferData((uint)target, size, data, (uint)usage);

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
		public void BufferSubData(GLBufferTarget target, nint offset, nint size, IntPtr data) => Functions.glBufferSubData((uint)target, offset, size, data);

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
		public void DeleteBuffers(int n, IntPtr buffers) => Functions.glDeleteBuffers(n, buffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(in ReadOnlySpan<uint> buffers) {
			unsafe {
				fixed(uint* pBuffers = buffers) {
					Functions.glDeleteBuffers(buffers.Length, (IntPtr)pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(params uint[] buffers) {
			unsafe {
				fixed (uint* pBuffers = buffers) {
					Functions.glDeleteBuffers(buffers.Length, (IntPtr)pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(int n, IntPtr ids) => Functions.glDeleteQueries(n, ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(in ReadOnlySpan<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glDeleteQueries(ids.Length, (IntPtr)pIds);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(params uint[] ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glDeleteQueries(ids.Length, (IntPtr)pIds);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndQuery(GLQueryTarget target) => Functions.glEndQuery((uint)target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenBuffers(int n, IntPtr buffers) => Functions.glGenBuffers(n, buffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenBuffers(in Span<uint> buffers) {
			unsafe {
				fixed (uint* pBuffers = buffers) {
					Functions.glGenBuffers(buffers.Length, (IntPtr)pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenBuffers(int n) {
			uint[] buffers = new uint[n];
			unsafe {
				fixed (uint* pBuffers = buffers) {
					Functions.glGenBuffers(buffers.Length, (IntPtr)pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenBuffer() => GenBuffers(stackalloc uint[1])[0];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenQueries(int n, IntPtr ids) => Functions.glGenQueries(n, ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenQueries(in Span<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glGenQueries(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenQueries(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glGenQueries(n, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenQuery() => GenQueries(stackalloc uint[1])[0];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetBufferParameteri(GLBufferTarget target, GLBufferParameter pname) {
			unsafe {
				int param = 0;
				Functions.glGetBufferParamteriv((uint)target, (uint)pname, (IntPtr)(&param));
				return param;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr GetBufferPointer(GLBufferTarget target, GLGetBufferPointer pname) {
			unsafe {
				IntPtr param = IntPtr.Zero;
				Functions.glGetBufferPointerv((uint)target, (uint)pname, (IntPtr)(&param));
				return param;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetBufferSubData(GLBufferTarget target, nint offset, nint size, IntPtr data) => Functions.glGetBufferSubData((uint)target, offset, size, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetBufferSubData<T>(GLBufferTarget target, nint offset, in Span<T> data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					Functions.glGetBufferSubData((uint)target, offset, (nint)sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
			return data;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] GetBufferSubData<T>(GLBufferTarget target, nint offset, T[] data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glGetBufferSubData((uint)target, offset, (nint)sizeof(T) * data.Length, (IntPtr)pData);
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
				Functions.glGetQueryObjectiv(id, (uint)pname, (IntPtr)(&value));
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjecti(uint id, GLGetQueryObject pname, nint offset) => Functions.glGetQueryObjectiv(id, (uint)pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetQueryObjectui(uint id, GLGetQueryObject pname) {
			unsafe {
				uint value = 0;
				Functions.glGetQueryObjectuiv(id, (uint)pname, (IntPtr)(&value));
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjectui(uint id, GLGetQueryObject pname, nint offset) => Functions.glGetQueryObjectuiv(id, (uint)pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetQueryi(GLGetQueryTarget target, GLGetQuery pname) {
			unsafe {
				int value = 0;
				Functions.glGetQueryiv((uint)target, (uint)pname, (IntPtr)(&value));
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsBuffer(uint buffer) => Functions.glIsBuffer(buffer) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsQuery(uint id) => Functions.glIsQuery(id) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapBuffer(GLBufferTarget target, GLMapAccess access) => Functions.glMapBuffer((uint)target, (uint)access);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool UnmapBuffer(GLBufferTarget target) => Functions.glUnmapBuffer((uint)target) != 0;

	}
}

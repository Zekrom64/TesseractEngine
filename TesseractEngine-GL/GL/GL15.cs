using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.GL.Native;

namespace Tesseract.GL {

	public class GL15 : GL14 {

		public GL15Functions FunctionsGL15 { get; } = new();

		public GL15(GL gl, IGLContext context) : base(gl, context) {
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL15);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginQuery(GLQueryTarget target, uint id) => FunctionsGL15.glBeginQuery((uint)target, id);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffer(GLBufferTarget target, uint id) => FunctionsGL15.glBindBuffer((uint)target, id);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferData(GLBufferTarget target, nint size, IntPtr data, GLBufferUsage usage) => FunctionsGL15.glBufferData((uint)target, size, data, (uint)usage);

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
		public void BufferSubData(GLBufferTarget target, nint offset, nint size, IntPtr data) => FunctionsGL15.glBufferSubData((uint)target, offset, size, data);

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
		public void DeleteBuffers(int n, IntPtr buffers) => FunctionsGL15.glDeleteBuffers(n, buffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(in ReadOnlySpan<uint> buffers) {
			unsafe {
				fixed(uint* pBuffers = buffers) {
					FunctionsGL15.glDeleteBuffers(buffers.Length, (IntPtr)pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(params uint[] buffers) {
			unsafe {
				fixed (uint* pBuffers = buffers) {
					FunctionsGL15.glDeleteBuffers(buffers.Length, (IntPtr)pBuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffers(uint buffer) {
			unsafe {
				FunctionsGL15.glDeleteBuffers(1, (IntPtr)(&buffer));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(int n, IntPtr ids) => FunctionsGL15.glDeleteQueries(n, ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(in ReadOnlySpan<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					FunctionsGL15.glDeleteQueries(ids.Length, (IntPtr)pIds);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(params uint[] ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					FunctionsGL15.glDeleteQueries(ids.Length, (IntPtr)pIds);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQueries(uint query) {
			unsafe {
				FunctionsGL15.glDeleteQueries(1, (IntPtr)(&query));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndQuery(GLQueryTarget target) => FunctionsGL15.glEndQuery((uint)target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenBuffers(int n, IntPtr buffers) => FunctionsGL15.glGenBuffers(n, buffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenBuffers(in Span<uint> buffers) {
			unsafe {
				fixed (uint* pBuffers = buffers) {
					FunctionsGL15.glGenBuffers(buffers.Length, (IntPtr)pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenBuffers(int n) {
			uint[] buffers = new uint[n];
			unsafe {
				fixed (uint* pBuffers = buffers) {
					FunctionsGL15.glGenBuffers(buffers.Length, (IntPtr)pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenBuffers() => GenBuffers(stackalloc uint[1])[0];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenQueries(int n, IntPtr ids) => FunctionsGL15.glGenQueries(n, ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenQueries(in Span<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					FunctionsGL15.glGenQueries(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenQueries(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					FunctionsGL15.glGenQueries(n, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenQueries() => GenQueries(stackalloc uint[1])[0];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetBufferParameteri(GLBufferTarget target, GLBufferParameter pname) {
			unsafe {
				int param = 0;
				FunctionsGL15.glGetBufferParamteriv((uint)target, (uint)pname, (IntPtr)(&param));
				return param;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr GetBufferPointer(GLBufferTarget target, GLGetBufferPointer pname) {
			unsafe {
				IntPtr param = IntPtr.Zero;
				FunctionsGL15.glGetBufferPointerv((uint)target, (uint)pname, (IntPtr)(&param));
				return param;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetBufferSubData(GLBufferTarget target, nint offset, nint size, IntPtr data) => FunctionsGL15.glGetBufferSubData((uint)target, offset, size, data);

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
				FunctionsGL15.glGetQueryObjectiv(id, (uint)pname, (IntPtr)(&value));
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjecti(uint id, GLGetQueryObject pname, nint offset) => FunctionsGL15.glGetQueryObjectiv(id, (uint)pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetQueryObjectui(uint id, GLGetQueryObject pname) {
			unsafe {
				uint value = 0;
				FunctionsGL15.glGetQueryObjectuiv(id, (uint)pname, (IntPtr)(&value));
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjectui(uint id, GLGetQueryObject pname, nint offset) => FunctionsGL15.glGetQueryObjectuiv(id, (uint)pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetQueryi(GLGetQueryTarget target, GLGetQuery pname) {
			unsafe {
				int value = 0;
				FunctionsGL15.glGetQueryiv((uint)target, (uint)pname, (IntPtr)(&value));
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsBuffer(uint buffer) => FunctionsGL15.glIsBuffer(buffer) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsQuery(uint id) => FunctionsGL15.glIsQuery(id) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapBuffer(GLBufferTarget target, GLMapAccess access) => FunctionsGL15.glMapBuffer((uint)target, (uint)access);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool UnmapBuffer(GLBufferTarget target) => FunctionsGL15.glUnmapBuffer((uint)target) != 0;

	}
}

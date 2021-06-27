using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Native {

	/// <summary>
	/// A constant pointer stores a memory reference to a read-only object.
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public interface IConstPointer<T> {

		/// <summary>
		/// The underlying memory pointer.
		/// </summary>
		public IntPtr Ptr { get; }

		/// <summary>
		/// If the underlying memory pointer is null.
		/// </summary>
		public bool IsNull => Ptr == IntPtr.Zero;

		/// <summary>
		/// The value stored at the memory reference.
		/// </summary>
		public T Value { get; }

	}

	/// <summary>
	/// A pointer stores a memory reference to a modifiable object.
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public interface IPointer<T> : IConstPointer<T> {

		/// <summary>
		/// The value stored at the memory reference.
		/// </summary>
		public new T Value { get; set; }

	}
	
	/// <summary>
	/// A releaser handles releasing memory held by a managed pointer.
	/// </summary>
	/// <param name="ptr">Pointer to release</param>
	public delegate void Releaser(IntPtr ptr);

	/// <summary>
	/// A managed pointer references memory that is manually managed by the developer.
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public struct ManagedPointer<T> : IDisposable, IPointer<T> where T : struct {

		private readonly IntPtr ptr;
		private readonly Releaser release;

		public T Value {
			get => Marshal.PtrToStructure<T>(ptr);
			set => Marshal.StructureToPtr(value, ptr, true);
		}

		public IntPtr Ptr => ptr;

		/// <summary>
		/// Creates a new managed pointer from an existing raw pointer and an optional releaser.
		/// </summary>
		/// <param name="ptr">Raw pointer</param>
		/// <param name="release">Releaser for pointer, or null</param>
		public ManagedPointer(IntPtr ptr, Releaser release = null) {
			this.ptr = ptr;
			this.release = release;
		}

		/// <summary>
		/// Allocates a pointer to a value and marshals the given value to the new memory.
		/// </summary>
		/// <param name="value">Value to store in memory</param>
		public ManagedPointer(T value) {
			ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
			Marshal.StructureToPtr(value, ptr, false);
			release = Marshal.FreeHGlobal;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			release?.Invoke(ptr);
		}

		public static implicit operator IntPtr(ManagedPointer<T> mptr) => mptr.ptr;

		public T this[int index] {
			get => Marshal.PtrToStructure<T>(ptr + Marshal.SizeOf<T>() * index);
			set => Marshal.StructureToPtr(value, ptr + Marshal.SizeOf<T>() * index, true);
		}

	}

	/// <summary>
	/// An unmanaged pointer references memory that is externally managed.
	/// </summary>
	/// <typeparam name="T">Type reference</typeparam>
	public struct UnmanagedPointer<T> : IPointer<T> where T : struct {

		private readonly IntPtr ptr;

		public T Value {
			get => Marshal.PtrToStructure<T>(ptr);
			set => Marshal.StructureToPtr(value, ptr, false);
		}

		public IntPtr Ptr => ptr;

		public UnmanagedPointer(IntPtr ptr) {
			this.ptr = ptr;
		}

		public static implicit operator IntPtr(UnmanagedPointer<T> uptr) => uptr.ptr;

		public static explicit operator UnmanagedPointer<T>(IntPtr ptr) => new(ptr);

		public T this[int index] {
			get => Marshal.PtrToStructure<T>(ptr + Marshal.SizeOf<T>() * index);
			set => Marshal.StructureToPtr(value, ptr + Marshal.SizeOf<T>() * index, false);
		}

	}

	/// <summary>
	/// <para>
	/// An object pointer stores a reference to a managed object.
	/// </para>
	/// <para>
	/// Creating a new object pointer from an object will pin the object in memory until explicitly
	/// disposed of. An object pointer can retrieve the pinned value from the raw pointer as well.
	/// Note that the pointer is merely a reference to the object, not a pointer to the object's raw memory.
	/// Additionaly, because this is a reference to an object the value referenced by the pointer cannot be
	/// directly changed (though the object may still be modifiable).
	/// </para>
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public struct ObjectPointer<T> : IDisposable, IConstPointer<T> where T : class {

		private readonly IntPtr ptr;

		public IntPtr Ptr => ptr;

		public T Value => (T)GCHandle.FromIntPtr(ptr).Target;

		/// <summary>
		/// Creates an object pointer from a raw pointer.
		/// </summary>
		/// <param name="ptr">Raw pointer value</param>
		public ObjectPointer(IntPtr ptr) {
			this.ptr = ptr;
		}

		/// <summary>
		/// Creates a new object pointer referencing the given value.
		/// </summary>
		/// <param name="value">Value to reference</param>
		public ObjectPointer(T value) {
			ptr = GCHandle.ToIntPtr(GCHandle.Alloc(value));
		}

		public void Dispose() {
			GCHandle.FromIntPtr(ptr).Free();
		}

		public static implicit operator IntPtr(ObjectPointer<T> optr) => optr.Ptr;

	}

}

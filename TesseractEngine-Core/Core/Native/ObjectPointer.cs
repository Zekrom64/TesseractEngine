using System;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Native {

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
	public readonly struct ObjectPointer<T> : IDisposable, IConstPointer<T?> where T : class {

		public IntPtr Ptr { get; }

		public int ArraySize => 1;

		public T? Value {
			get {
				object? target = Handle.Target;
				return (T?)target;
			}
		}

		/// <summary>
		/// The corresponding <see cref="GCHandle"/> for this pointer.
		/// </summary>
		public GCHandle Handle => GCHandle.FromIntPtr(Ptr);

		public ReadOnlySpan<T?> ReadOnlySpan => throw new InvalidOperationException("Cannot get an object pointer as a span, underlying memory is opaque");

		public T? this[int key] {
			get {
				if (key == 0) return Value;
				else throw new IndexOutOfRangeException();
			}
		}

		/// <summary>
		/// Creates an object pointer from a raw pointer.
		/// </summary>
		/// <param name="ptr">Raw pointer value</param>
		public ObjectPointer(IntPtr ptr) {
			Ptr = ptr;
		}

		/// <summary>
		/// Creates a new object pointer referencing the given value.
		/// </summary>
		/// <param name="value">Value to reference</param>
		public ObjectPointer(T value) {
			Ptr = GCHandle.ToIntPtr(GCHandle.Alloc(value));
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Handle.Free();
		}

		public static implicit operator IntPtr(ObjectPointer<T> optr) => optr.Ptr;

		/// <summary>
		/// Checks if this pointer is valid (ie. not a null pointer).
		/// </summary>
		/// <param name="optr">The pointer to check</param>
		public static implicit operator bool(ObjectPointer<T> optr) => optr.Ptr != IntPtr.Zero;

	}

}

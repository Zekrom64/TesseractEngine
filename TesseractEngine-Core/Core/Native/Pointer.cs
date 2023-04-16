using System;
using Tesseract.Core.Collections;

namespace Tesseract.Core.Native {

	/// <summary>
	/// A constant pointer stores a memory reference to a read-only object.
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public interface IConstPointer<T> : IReadOnlyIndexer<int, T> {

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

		/// <summary>
		/// The size of the array pointed to by this value. If only a single value this is 1. If the size
		/// is unknown this is -1.
		/// </summary>
		public int ArraySize { get; }

		/// <summary>
		/// Gets the pointer represented as a read-only span.
		/// </summary>
		public ReadOnlySpan<T> ReadOnlySpan { get; }

	}

	/// <summary>
	/// A pointer stores a memory reference to a modifiable object.
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public interface IPointer<T> : IConstPointer<T>, IIndexer<int, T> {

		/// <summary>
		/// The value stored at the memory reference.
		/// </summary>
		public new T Value { get; set; }

		/// <summary>
		/// Gets the pointer represented as a span.
		/// </summary>
		public Span<T> Span { get; }

		ReadOnlySpan<T> IConstPointer<T>.ReadOnlySpan => Span;

	}

	/// <summary>
	/// A releaser handles releasing memory held by a managed pointer.
	/// </summary>
	/// <param name="ptr">Pointer to release</param>
	public delegate void Releaser(IntPtr ptr);

}

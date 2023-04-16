using System;
using Tesseract.Core.Collections;

namespace Tesseract.Core.Native {

	/// <summary>
	/// This structure will always act like a null pointer, and can be used for any pointer element type.
	/// </summary>
	/// <typeparam name="T">Pointer element type</typeparam>
	public struct NullPointer<T> : IPointer<T> {

		public T this[int key] {
			get => throw new NullReferenceException("Attempted to dereference a null pointer");
			set => throw new NullReferenceException("Attempted to dereference a null pointer");
		}

		T IReadOnlyIndexer<int, T>.this[int key] => throw new NullReferenceException("Attempted to dereference a null pointer");

		T IConstPointer<T>.Value => throw new NullReferenceException("Attempted to dereference a null pointer");

		public T Value {
			get => throw new NullReferenceException("Attempted to dereference a null pointer");
			set => throw new NullReferenceException("Attempted to dereference a null pointer");
		}

		public Span<T> Span => Span<T>.Empty;

		public nint Ptr => 0;

		public int ArraySize => -1;

	}

}

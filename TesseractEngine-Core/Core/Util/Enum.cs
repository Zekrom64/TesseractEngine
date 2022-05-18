using System;

namespace Tesseract.Core.Util {

	/// <summary>
	/// A valued enum is a type which behaves like an enumeration but is a true
	/// class or struct instead of a simple numeric value.
	/// </summary>
	/// <typeparam name="T">The correspoding unique enum value</typeparam>
	public interface IValuedEnum<T> : IEquatable<IValuedEnum<T>> {

		/// <summary>
		/// The underlying unique enumeration value.
		/// </summary>
		public T Value { get; }

	}

}

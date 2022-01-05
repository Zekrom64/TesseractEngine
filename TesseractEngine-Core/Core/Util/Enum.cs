using System;

namespace Tesseract.Core.Util {

	public interface IValuedEnum<T> : IEquatable<IValuedEnum<T>> {

		public T Value { get; }

	}

}

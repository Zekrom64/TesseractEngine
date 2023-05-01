namespace Tesseract.Core.Native {

	/// <summary>
	/// A primitive handle is an object whose native representation is that
	/// of a handle of a simple primitive type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IPrimitiveHandle<T> where T : unmanaged {

		/// <summary>
		/// The primitive handle value.
		/// </summary>
		public T PrimitiveHandle { get; }

	}

}

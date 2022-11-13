using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Numerics {

	/// <summary>
	/// Interface for 4-component vector types.
	/// </summary>
	/// <typeparam name="T">Vector element type</typeparam>
	/// <typeparam name="TSelf">Vector type</typeparam>
	public interface IVector4<TSelf, T> :
		IVector<TSelf, T>, ITuple4<T>
		where TSelf : IVector4<TSelf, T>
		where T : struct, INumber<T> {

		/// <summary>
		/// A vector containing all zeros.
		/// </summary>
		public static TSelf Zero => TSelf.Create(T.Zero, T.Zero, T.Zero, T.Zero);

		/// <summary>
		/// A vector containing all ones.
		/// </summary>
		public static TSelf One => TSelf.Create(T.MultiplicativeIdentity, T.MultiplicativeIdentity, T.MultiplicativeIdentity, T.MultiplicativeIdentity);

		/// <summary>
		/// Creates a new instance of this vector type.
		/// </summary>
		/// <param name="x">Vector X component</param>
		/// <param name="y">Vector Y component</param>
		/// <param name="z">Vector Z component</param>
		/// <param name="w">Vector W component</param>
		/// <returns>Constructed vector</returns>
		public static abstract TSelf Create(T x, T y, T z, T w);

		/// <summary>
		/// Performs a vector swizzle using the components of this vector.
		/// </summary>
		/// <param name="x">New X component</param>
		/// <param name="y">New Y component</param>
		/// <param name="z">New Z component</param>
		/// <param name="w">New W component</param>
		/// <returns>Swizzled vector</returns>
		public TSelf Swizzle(int x, int y, int z, int w);

	}

	/// <summary>
	/// Interface for 4-component floating point vector types.
	/// </summary>
	/// <typeparam name="T">Vector element type</typeparam>
	/// <typeparam name="TSelf">Vector type</typeparam>
	public interface IVector4Float<TSelf, T> :
		IVector4<TSelf, T>, IVectorFloat<TSelf, T>
		where TSelf : IVector4Float<TSelf, T>
		where T : struct, IFloatingPointIeee754<T> {

	}

	/// <summary>
	/// Interface for 4-component integer vector types.
	/// </summary>
	/// <typeparam name="T">Vector element type</typeparam>
	/// <typeparam name="TSelf">Vector type</typeparam>
	public interface IVector4Int<TSelf, T> :
		IVector4<TSelf, T>, IVectorInt<TSelf, T>
		where TSelf : IVector4Int<TSelf, T>
		where T : struct, IBinaryInteger<T> {

	}

}

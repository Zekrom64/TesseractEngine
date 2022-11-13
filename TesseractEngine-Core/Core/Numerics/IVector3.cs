using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Numerics {

	/// <summary>
	/// Interface for 3-component vector types.
	/// </summary>
	/// <typeparam name="T">Vector element type</typeparam>
	/// <typeparam name="TSelf">Vector type</typeparam>
	public interface IVector3<TSelf, T> :
		IVector<TSelf, T>, ITuple3<T>
		where TSelf : IVector3<TSelf, T>
		where T : struct, INumber<T> {

		/// <summary>
		/// A vector containing all zeros.
		/// </summary>
		public static TSelf Zero => TSelf.Create(T.Zero, T.Zero, T.Zero);

		/// <summary>
		/// A vector containing all ones.
		/// </summary>
		public static TSelf One => TSelf.Create(T.MultiplicativeIdentity, T.MultiplicativeIdentity, T.MultiplicativeIdentity);

		/// <summary>
		/// Creates a new instance of this vector type.
		/// </summary>
		/// <param name="x">Vector X component</param>
		/// <param name="y">Vector Y component</param>
		/// <param name="z">Vector Z component</param>
		/// <returns>Constructed vector</returns>
		public static abstract TSelf Create(T x, T y, T z);

		/// <summary>
		/// Performs a vector swizzle using the components of this vector.
		/// </summary>
		/// <param name="x">New X component</param>
		/// <param name="y">New Y component</param>
		/// <param name="z">New Z component</param>
		/// <returns>Swizzled vector</returns>
		public TSelf Swizzle(int x, int y, int z);

	}

	/// <summary>
	/// Interface for 3-component floating point vector types.
	/// </summary>
	/// <typeparam name="T">Vector element type</typeparam>
	/// <typeparam name="TSelf">Vector type</typeparam>
	public interface IVector3Float<TSelf, T> :
		IVector3<TSelf, T>, IVectorFloat<TSelf, T>
		where TSelf : IVector3Float<TSelf, T>
		where T : struct, IFloatingPointIeee754<T> {

	}

	/// <summary>
	/// Interface for 3-component integer vector types.
	/// </summary>
	/// <typeparam name="T">Vector element type</typeparam>
	/// <typeparam name="TSelf">Vector type</typeparam>
	public interface IVector3Int<TSelf, T> :
		IVector3<TSelf, T>, IVectorInt<TSelf, T>
		where TSelf : IVector3Int<TSelf, T>
		where T : struct, IBinaryInteger<T> {

	}

}

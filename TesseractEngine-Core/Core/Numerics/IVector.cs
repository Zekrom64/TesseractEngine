using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Numerics {
	
	/// <summary>
	/// Interface for numeric vector types.
	/// </summary>
	/// <typeparam name="T">Vector element type</typeparam>
	public interface IVector<T> :
		IIndexer<int, T>
		where T : struct, INumber<T> {

		/// <summary>
		/// Gets the elements of this vector as a span.
		/// </summary>
		public Span<T> AsSpan { get; }

		/// <summary>
		/// Tests if this vector has a zero length (ie. all its components are zero).
		/// </summary>
		public bool IsZeroLength { get; }

		/// <summary>
		/// Gets the length squared of this vector.
		/// </summary>
		public T LengthSquared { get; }

	}

	/// <summary>
	/// Extended version of <see cref="IVector{T}"/> which defines operators on itself.
	/// </summary>
	/// <typeparam name="TSelf">Vector type</typeparam>
	/// <typeparam name="T">Vector element type</typeparam>
	public interface IVector<TSelf,T> :
		IVector<T>, IEquatable<TSelf>,
		IEqualityOperators<TSelf, TSelf, bool>,
		IAdditionOperators<TSelf, TSelf, TSelf>,
		ISubtractionOperators<TSelf, TSelf, TSelf>,
		IMultiplyOperators<TSelf, TSelf, TSelf>,
		IDivisionOperators<TSelf, TSelf, TSelf>,
		IUnaryNegationOperators<TSelf, TSelf>,
		IUnaryPlusOperators<TSelf, TSelf>,
		IIncrementOperators<TSelf>,
		IDecrementOperators<TSelf>
		where TSelf : IVector<TSelf, T>
		where T : struct, INumber<T> {

		/// <summary>
		/// Componentwise computes the minimum and maximum of two vectors, storing the values back
		/// into the respective vectors.
		/// </summary>
		/// <param name="min">Vector to store minimums into</param>
		/// <param name="max">Vector to store maximums into</param>
		public static abstract void MinMax(ref TSelf min, ref TSelf max);

		/// <summary>
		/// Gets the absolute value of this vector.
		/// </summary>
		/// <returns>Absolute vector</returns>
		public TSelf Abs();

		/// <summary>
		/// Gets the minimum value between this vector and a second.
		/// </summary>
		/// <param name="v2">Second vector</param>
		/// <returns>Minimum vector</returns>
		public TSelf Min(TSelf v2);

		/// <summary>
		/// Gets the maximum value between this vector and a second.
		/// </summary>
		/// <param name="v2">Second vector</param>
		/// <returns>Maximum vector</returns>
		public TSelf Max(TSelf v2);

		/// <summary>
		/// Computes the horizontal sum of this vector, ie. the sum of all elements in the vector.
		/// </summary>
		/// <returns>Vector horizontal sum</returns>
		public T Sum();

		/// <summary>
		/// Computes the dot product between this vector and a second.
		/// </summary>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector dot product</returns>
		public T Dot(TSelf v2);

		/// <summary>
		/// Computes the distance squared between this vector and a second.
		/// </summary>
		/// <param name="v2">Second vector</param>
		/// <returns>Distance squared between vectors</returns>
		public T DistanceSquared(TSelf v2);

		/// <summary>
		/// Computes the sum of a vector and a scalar.
		/// </summary>
		/// <param name="left">Vector value</param>
		/// <param name="right">Scalar value</param>
		/// <returns>Vector sum</returns>
		public static abstract TSelf operator +(TSelf left, T right);

		/// <summary>
		/// Computes the difference of a vector and a scalar.
		/// </summary>
		/// <param name="left">Vector value</param>
		/// <param name="right">Scalar value</param>
		/// <returns>Vector difference</returns>
		public static abstract TSelf operator -(TSelf left, T right);

		/// <summary>
		/// Computes the product of a vector and a scalar.
		/// </summary>
		/// <param name="left">Vector value</param>
		/// <param name="right">Scalar value</param>
		/// <returns>Vector product</returns>
		public static abstract TSelf operator *(TSelf left, T right);

		/// <summary>
		/// Computes the quotient of a vector and a scalar.
		/// </summary>
		/// <param name="left">Vector value</param>
		/// <param name="right">Scalar value</param>
		/// <returns>Vector quotient</returns>
		public static abstract TSelf operator /(TSelf left, T right);

	}

	/// <summary>
	/// Variant of <see cref="IVector{TSelf, T}"/> for vectors with floating-point components.
	/// </summary>
	/// <typeparam name="TSelf">Vector type</typeparam>
	/// <typeparam name="T">Vector element type</typeparam>
	public interface IVectorFloat<TSelf, T> :
		IVector<TSelf, T>
		where TSelf : IVectorFloat<TSelf, T>
		where T : struct, IFloatingPointIeee754<T> {

		/// <summary>
		/// Gets the length of this vector.
		/// </summary>
		public T Length { get; }

		/// <summary>
		/// Componentwise rounds this vector to the nearest integer value.
		/// </summary>
		/// <returns>Rounded vector</returns>
		public TSelf Round();

		/// <summary>
		/// Componentwise rounds this vector down to an integer value.
		/// </summary>
		/// <returns>Rounded vector</returns>
		public TSelf Floor();

		/// <summary>
		/// Componentwise rounds this vector up to an integer value.
		/// </summary>
		/// <returns>Rounded vector</returns>
		public TSelf Ceiling();

		/// <summary>
		/// Computes the distance between this vector and a second.
		/// </summary>
		/// <param name="v2">Second vector</param>
		/// <returns>Distance between vectors</returns>
		public T Distance(TSelf v2);

		/// <summary>
		/// Computes the normalized form of this vector by dividing by its <see cref="Length"/>.
		/// </summary>
		/// <returns>Normalized vector</returns>
		public TSelf Normalize();

		/// <summary>
		/// Componentwise computes the square root of the vector.
		/// </summary>
		/// <returns>Vector square root</returns>
		public TSelf Sqrt();

	}

	/// <summary>
	/// Variant of <see cref="IVector{TSelf, T}"/> for vectors with integer components.
	/// </summary>
	/// <typeparam name="TSelf">Vector type</typeparam>
	/// <typeparam name="T">Vector element type</typeparam>
	public interface IVectorInt<TSelf, T> :
		IVector<TSelf, T>,
		IModulusOperators<TSelf, TSelf, TSelf>,
		IShiftOperators<TSelf, int, TSelf>,
		IBitwiseOperators<TSelf, TSelf, TSelf>
		where TSelf : IVectorInt<TSelf, T>
		where T : struct, IBinaryInteger<T> {

		/// <summary>
		/// Computes the modulus of a vector and a scalar.
		/// </summary>
		/// <param name="left">Vector value</param>
		/// <param name="right">Scalar value</param>
		/// <returns>Vector modulus</returns>
		public static abstract TSelf operator %(TSelf left, T right);

		/// <summary>
		/// Computes the bitwise AND of a vector and a scalar.
		/// </summary>
		/// <param name="left">Vector value</param>
		/// <param name="right">Scalar value</param>
		/// <returns>Vector bitwise AND</returns>
		public static abstract TSelf operator &(TSelf left, T right);

		/// <summary>
		/// Computes the bitwise OR of a vector and a scalar.
		/// </summary>
		/// <param name="left">Vector value</param>
		/// <param name="right">Scalar value</param>
		/// <returns>Vector bitwise OR</returns>
		public static abstract TSelf operator |(TSelf left, T right);

		/// <summary>
		/// Computes the bitwise XOR of a vector and a scalar.
		/// </summary>
		/// <param name="left">Vector value</param>
		/// <param name="right">Scalar value</param>
		/// <returns>Vector bitwise XOR</returns>
		public static abstract TSelf operator ^(TSelf left, T right);

	}

}

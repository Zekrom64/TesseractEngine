using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tesseract.Core.Numerics {

	/// <summary>
	/// Contains utilities for vector mathematics.
	/// </summary>
	public static class Vecmath {

		public const int X = 0;

		public const int Y = 1;

		public const int Z = 2;

		public const int W = 3;

		/// <summary>
		/// Converts a Structure-of-Arrays data structure to an Array-of-Structures data structure.
		/// </summary>
		/// <typeparam name="V">Structure in Array-of-Structures</typeparam>
		/// <typeparam name="T1">First element type in Structure-of-Arrays</typeparam>
		/// <typeparam name="T2">Second element type in Structure-of-Arrays</typeparam>
		/// <param name="a1">First array from Structure-of-Arrays</param>
		/// <param name="a2">Second array from Structure-of-Arrays</param>
		/// <returns>Array-of-Structures</returns>
		public static V[] SOAToAOS<V, T1, T2>(in ReadOnlySpan<T1> a1, in ReadOnlySpan<T2> a2) where V : ITuple<T1, T2>, new() {
			int length = ExMath.Min(a1.Length, a2.Length);
			V[] aos = new V[length];
			for (int i = 0; i < length; i++) aos[length] = new V() { X = a1[i], Y = a2[i] };
			return aos;
		}

		/// <summary>
		/// Converts a Structure-of-Arrays data structure to an Array-of-Structures data structure.
		/// </summary>
		/// <typeparam name="V">Structure in Array-of-Structures</typeparam>
		/// <typeparam name="T1">First element type in Structure-of-Arrays</typeparam>
		/// <typeparam name="T2">Second element type in Structure-of-Arrays</typeparam>
		/// <param name="aos">Array-of-Structures to store into</param>
		/// <param name="a1">First array from Structure-of-Arrays</param>
		/// <param name="a2">Second array from Structure-of-Arrays</param>
		/// <returns>Array-of-Structures</returns>
		public static Span<V> SOAToAOS<V, T1, T2>(Span<V> aos, in ReadOnlySpan<T1> a1, in ReadOnlySpan<T2> a2) where V : ITuple<T1, T2>, new() {
			int length = ExMath.Min(aos.Length, a1.Length, a2.Length);
			for (int i = 0; i < length; i++) aos[length] = new V() { X = a1[i], Y = a2[i] };
			return aos;
		}

		/// <summary>
		/// Converts a Structure-of-Arrays data structure to an Array-of-Structures data structure.
		/// </summary>
		/// <typeparam name="V">Structure in Array-of-Structures</typeparam>
		/// <typeparam name="T1">First element type in Structure-of-Arrays</typeparam>
		/// <typeparam name="T2">Second element type in Structure-of-Arrays</typeparam>
		/// <typeparam name="T3">Third element type in Structure-of-Arrays</typeparam>
		/// <param name="a1">First array from Structure-of-Arrays</param>
		/// <param name="a2">Second array from Structure-of-Arrays</param>
		/// <param name="a3">Third array from Structure-of-Arrays</param>
		/// <returns>Array-of-Structures</returns>
		public static V[] SOAToAOS<V, T1, T2, T3>(in ReadOnlySpan<T1> a1, in ReadOnlySpan<T2> a2, in ReadOnlySpan<T3> a3) where V : ITuple<T1, T2, T3>, new() {
			int length = ExMath.Min(a1.Length, a2.Length, a3.Length);
			V[] aos = new V[length];
			for (int i = 0; i < length; i++) aos[length] = new V() { X = a1[i], Y = a2[i], Z = a3[i] };
			return aos;
		}

		/// <summary>
		/// Converts a Structure-of-Arrays data structure to an Array-of-Structures data structure.
		/// </summary>
		/// <typeparam name="V">Structure in Array-of-Structures</typeparam>
		/// <typeparam name="T1">First element type in Structure-of-Arrays</typeparam>
		/// <typeparam name="T2">Second element type in Structure-of-Arrays</typeparam>
		/// <typeparam name="T3">Third element type in Structure-of-Arrays</typeparam>
		/// <param name="a1">First array from Structure-of-Arrays</param>
		/// <param name="a2">Second array from Structure-of-Arrays</param>
		/// <param name="a3">Third array from Structure-of-Arrays</param>
		/// <returns>Array-of-Structures</returns>
		public static Span<V> SOAToAOS<V, T1, T2, T3>(Span<V> aos, in ReadOnlySpan<T1> a1, in ReadOnlySpan<T2> a2, in ReadOnlySpan<T3> a3) where V : ITuple<T1, T2, T3>, new() {
			int length = ExMath.Min(aos.Length, a1.Length, a2.Length, a3.Length);
			for (int i = 0; i < length; i++) aos[length] = new V() { X = a1[i], Y = a2[i], Z = a3[i] };
			return aos;
		}


		/// <summary>
		/// Computes the absolute value of this vector.
		/// </summary>
		/// <param name="v">Vector value</param>
		/// <returns>Absolute vector value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Abs(this Vector2 v) => Vector2.Abs(v);

		/// <summary>
		/// Computes the absolute value of this vector.
		/// </summary>
		/// <param name="v">Vector value</param>
		/// <returns>Absolute vector value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Abs(this Vector3 v) => Vector3.Abs(v);

		/// <summary>
		/// Computes the absolute value of this vector.
		/// </summary>
		/// <param name="v">Vector value</param>
		/// <returns>Absolute vector value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Abs(this Vector4 v) => Vector4.Abs(v);


		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Min(this Vector2 v1, Vector2 v2) => Vector2.Min(v1, v2);

		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Min(this Vector3 v1, Vector3 v2) => Vector3.Min(v1, v2);

		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Min(this Vector4 v1, Vector4 v2) => Vector4.Min(v1, v2);


		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Max(this Vector2 v1, Vector2 v2) => Vector2.Max(v1, v2);

		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Max(this Vector3 v1, Vector3 v2) => Vector3.Max(v1, v2);

		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Max(this Vector4 v1, Vector4 v2) => Vector4.Max(v1, v2);


		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i Min(this Vector2i v1, Vector2i v2) => new(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y));

		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i Min(this Vector3i v1, Vector3i v2) => new(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y), Math.Min(v1.Z, v2.Z));

		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i Min(this Vector4i v1, Vector4i v2) => new(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y), Math.Min(v1.Y, v2.Y), Math.Min(v1.W, v2.W));


		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i Max(this Vector2i v1, Vector2i v2) => new(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y));

		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i Max(this Vector3i v1, Vector3i v2) => new(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y), Math.Max(v1.Z, v2.Z));

		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i Max(this Vector4i v1, Vector4i v2) => new(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y), Math.Max(v1.Y, v2.Y), Math.Max(v1.W, v2.W));


		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui Min(this Vector2ui v1, Vector2ui v2) => new(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y));

		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui Min(this Vector3ui v1, Vector3ui v2) => new(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y), Math.Min(v1.Z, v2.Z));

		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui Min(this Vector4ui v1, Vector4ui v2) => new(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y), Math.Min(v1.Y, v2.Y), Math.Min(v1.W, v2.W));


		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui Max(this Vector2ui v1, Vector2ui v2) => new(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y));

		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui Max(this Vector3ui v1, Vector3ui v2) => new(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y), Math.Max(v1.Z, v2.Z));

		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui Max(this Vector4ui v1, Vector4ui v2) => new(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y), Math.Max(v1.Y, v2.Y), Math.Max(v1.W, v2.W));


		/// <summary>
		/// Component-wise determines the minimum and maximum value between two vectors,
		/// storing the minimums and maximums back into each respective vector.
		/// </summary>
		/// <param name="vmin">Vector to receive minimum values</param>
		/// <param name="vmax">Vector to receive maximum values</param>
		public static void MinMax(ref Vector2 vmin, ref Vector2 vmax) {
			ExMath.MinMax(ref vmin.X, ref vmax.X);
			ExMath.MinMax(ref vmin.Y, ref vmax.Y);
		}

		/// <summary>
		/// Component-wise determines the minimum and maximum value between two vectors,
		/// storing the minimums and maximums back into each respective vector.
		/// </summary>
		/// <param name="vmin">Vector to receive minimum values</param>
		/// <param name="vmax">Vector to receive maximum values</param>
		public static void MinMax(ref Vector3 vmin, ref Vector3 vmax) {
			ExMath.MinMax(ref vmin.X, ref vmax.X);
			ExMath.MinMax(ref vmin.Y, ref vmax.Y);
			ExMath.MinMax(ref vmin.Z, ref vmax.Z);
		}

		/// <summary>
		/// Component-wise determines the minimum and maximum value between two vectors,
		/// storing the minimums and maximums back into each respective vector.
		/// </summary>
		/// <param name="vmin">Vector to receive minimum values</param>
		/// <param name="vmax">Vector to receive maximum values</param>
		public static void MinMax(ref Vector4 vmin, ref Vector4 vmax) {
			ExMath.MinMax(ref vmin.X, ref vmax.X);
			ExMath.MinMax(ref vmin.Y, ref vmax.Y);
			ExMath.MinMax(ref vmin.Z, ref vmax.Z);
			ExMath.MinMax(ref vmin.W, ref vmax.W);
		}


		/// <summary>
		/// Tests if a vector is zero-length (i.e. all of its components are zero).
		/// </summary>
		/// <param name="v">Vector to test</param>
		/// <returns>If the vector has a length of zero</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsZeroLength(Vector2 v) => v.X == 0 && v.Y == 0;

		/// <summary>
		/// Tests if a vector is zero-length (i.e. all of its components are zero).
		/// </summary>
		/// <param name="v">Vector to test</param>
		/// <returns>If the vector has a length of zero</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsZeroLength(Vector3 v) => v.X == 0 && v.Y == 0 && v.Z == 0;

		/// <summary>
		/// Tests if a vector is zero-length (i.e. all of its components are zero).
		/// </summary>
		/// <param name="v">Vector to test</param>
		/// <returns>If the vector has a length of zero</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsZeroLength(Vector4 v) => v.X == 0 && v.Y == 0 && v.Z == 0 && v.W == 0;


		/// <summary>
		/// Sums every component in the vector, returning the total.
		/// </summary>
		/// <param name="v">Vector to sum</param>
		/// <returns>Sum of vector components</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sum(this Vector2 v) => v.X + v.Y;

		/// <summary>
		/// Sums every component in the vector, returning the total.
		/// </summary>
		/// <param name="v">Vector to sum</param>
		/// <returns>Sum of vector components</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sum(this Vector3 v) => v.X + v.Y + v.Z;

		/// <summary>
		/// Sums every component in the vector, returning the total.
		/// </summary>
		/// <param name="v">Vector to sum</param>
		/// <returns>Sum of vector components</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sum(this Vector4 v) => v.X + v.Y + v.Z + v.W;


		/// <summary>
		/// Computes the dot product between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector dot product</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(this Vector2 v1, Vector2 v2) => Vector2.Dot(v1, v2);

		/// <summary>
		/// Computes the dot product between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector dot product</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(this Vector3 v1, Vector3 v2) => Vector3.Dot(v1, v2);

		/// <summary>
		/// Computes the dot product between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector dot product</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(this Vector4 v1, Vector4 v2) => Vector4.Dot(v1, v2);

		/// <summary>
		/// Computes the floor of every component of this vector.
		/// </summary>
		/// <param name="v">This vector</param>
		/// <returns>Floor vector</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Floor(this Vector2 v) => new(MathF.Floor(v.X), MathF.Floor(v.Y));

		/// <summary>
		/// Computes the floor of every component of this vector.
		/// </summary>
		/// <param name="v">This vector</param>
		/// <returns>Floor vector</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Floor(this Vector3 v) => new(MathF.Floor(v.X), MathF.Floor(v.Y), MathF.Floor(v.Z));

		/// <summary>
		/// Computes the floor of every component of this vector.
		/// </summary>
		/// <param name="v">This vector</param>
		/// <returns>Floor vector</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Floor(this Vector4 v) => new(MathF.Floor(v.X), MathF.Floor(v.Y), MathF.Floor(v.Z), MathF.Floor(v.W));

		/// <summary>
		/// Computes the ceiling of every component of this vector.
		/// </summary>
		/// <param name="v">This vector</param>
		/// <returns>Ceiling vector</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Ceiling(this Vector2 v) => new(MathF.Ceiling(v.X), MathF.Ceiling(v.Y));

		/// <summary>
		/// Computes the ceiling of every component of this vector.
		/// </summary>
		/// <param name="v">This vector</param>
		/// <returns>Ceiling vector</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Ceiling(this Vector3 v) => new(MathF.Ceiling(v.X), MathF.Ceiling(v.Y), MathF.Ceiling(v.Z));

		/// <summary>
		/// Computes the ceiling of every component of this vector.
		/// </summary>
		/// <param name="v">This vector</param>
		/// <returns>Ceiling vector</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Ceiling(this Vector4 v) => new(MathF.Ceiling(v.X), MathF.Ceiling(v.Y), MathF.Ceiling(v.Z), MathF.Ceiling(v.W));

		/// <summary>
		/// Normalizes a vector by dividing each component by its length.
		/// </summary>
		/// <param name="v">Vector to normalize</param>
		/// <returns>Normalized vector</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Normalize(this Vector2 v) => Vector2.Normalize(v);

		/// <summary>
		/// Normalizes a vector by dividing each component by its length.
		/// </summary>
		/// <param name="v">Vector to normalize</param>
		/// <returns>Normalized vector</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Normalize(this Vector3 v) => Vector3.Normalize(v);

		/// <summary>
		/// Normalizes a vector by dividing each component by its length.
		/// </summary>
		/// <param name="v">Vector to normalize</param>
		/// <returns>Normalized vector</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Normalize(this Vector4 v) => Vector4.Normalize(v);

		/// <summary>
		/// Gets the distance squared between this and a second vector.
		/// </summary>
		/// <param name="a">First position vector</param>
		/// <param name="b">Second position vector</param>
		/// <returns>Distance squared between vectors</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceSquared(this Vector2 a, Vector2 b) => Vector2.DistanceSquared(a, b);

		/// <summary>
		/// Gets the distance squared between this and a second vector.
		/// </summary>
		/// <param name="a">First position vector</param>
		/// <param name="b">Second position vector</param>
		/// <returns>Distance squared between vectors</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceSquared(this Vector3 a, Vector3 b) => Vector3.DistanceSquared(a, b);

		/// <summary>
		/// Gets the distance squared between this and a second vector.
		/// </summary>
		/// <param name="a">First position vector</param>
		/// <param name="b">Second position vector</param>
		/// <returns>Distance squared between vectors</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceSquared(this Vector4 a, Vector4 b) => Vector4.DistanceSquared(a, b);

		/// <summary>
		/// Gets the distance between this and a second vector.
		/// </summary>
		/// <param name="a">First position vector</param>
		/// <param name="b">Second position vector</param>
		/// <returns>Distance between vectors</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(this Vector2 a, Vector2 b) => Vector2.Distance(a, b);

		/// <summary>
		/// Gets the distance between this and a second vector.
		/// </summary>
		/// <param name="a">First position vector</param>
		/// <param name="b">Second position vector</param>
		/// <returns>Distance between vectors</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(this Vector3 a, Vector3 b) => Vector3.Distance(a, b);

		/// <summary>
		/// Gets the distance between this and a second vector.
		/// </summary>
		/// <param name="a">First position vector</param>
		/// <param name="b">Second position vector</param>
		/// <returns>Distance between vectors</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(this Vector4 a, Vector4 b) => Vector4.Distance(a, b);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Get(this Vector2 v, int idx) => idx switch { 0 => v.X, 1 => v.Y, _ => 0 };
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Get(this Vector3 v, int idx) => idx switch { 0 => v.X, 1 => v.Y, 2 => v.Z, _ => 0 };
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Get(this Vector4 v, int idx) => idx switch { 0 => v.X, 1 => v.Y, 2 => v.Z, 3 => v.W, _ => 0 };

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Set(this Vector2 v, int idx, float value) {
			switch(idx) {
				case 0:
					v.X = value;
					break;
				case 1:
					v.Y = value;
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Set(this Vector3 v, int idx, float value) {
			switch (idx) {
				case 0:
					v.X = value;
					break;
				case 1:
					v.Y = value;
					break;
				case 2:
					v.Z = value;
					break;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Set(this Vector4 v, int idx, float value) {
			switch (idx) {
				case 0:
					v.X = value;
					break;
				case 1:
					v.Y = value;
					break;
				case 2:
					v.Z = value;
					break;
				case 3:
					v.W = value;
					break;
			}
		}

		public static Vector2 Swizzle(this Vector2 v, int x, int y) => new(v.Get(x), v.Get(y));

		public static Vector3 Swizzle(this Vector2 v, int x, int y, int z) => new(v.Get(x), v.Get(y), v.Get(z));

		public static Vector4 Swizzle(this Vector2 v, int x, int y, int z, int w) => new(v.Get(x), v.Get(y), v.Get(z), v.Get(w));

		public static Vector2 Swizzle(this Vector3 v, int x, int y) => new(v.Get(x), v.Get(y));

		public static Vector3 Swizzle(this Vector3 v, int x, int y, int z) => new(v.Get(x), v.Get(y), v.Get(z));

		public static Vector4 Swizzle(this Vector3 v, int x, int y, int z, int w) => new(v.Get(x), v.Get(y), v.Get(z), v.Get(w));

		public static Vector2 Swizzle(this Vector4 v, int x, int y) => new(v.Get(x), v.Get(y));

		public static Vector3 Swizzle(this Vector4 v, int x, int y, int z) => new(v.Get(x), v.Get(y), v.Get(z));

		public static Vector4 Swizzle(this Vector4 v, int x, int y, int z, int w) => new(v.Get(x), v.Get(y), v.Get(z), v.Get(w));

	}

}

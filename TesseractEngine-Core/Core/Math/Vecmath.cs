﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.Core.Math {

	/// <summary>
	/// Contains utilities for vector mathematics.
	/// </summary>
	public static class Vecmath {

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
		public static Vector2 Abs(this Vector2 v) => new(MathF.Abs(v.X), MathF.Abs(v.Y));

		/// <summary>
		/// Computes the absolute value of this vector.
		/// </summary>
		/// <param name="v">Vector value</param>
		/// <returns>Absolute vector value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Abs(this Vector3 v) => new(MathF.Abs(v.X), MathF.Abs(v.Y), MathF.Abs(v.Z));

		/// <summary>
		/// Computes the absolute value of this vector.
		/// </summary>
		/// <param name="v">Vector value</param>
		/// <returns>Absolute vector value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Abs(this Vector4 v) => new(MathF.Abs(v.X), MathF.Abs(v.Y), MathF.Abs(v.Z), MathF.Abs(v.W));


		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		public static Vector2 Min(this Vector2 v1, Vector2 v2) => new(MathF.Min(v1.X, v2.X), MathF.Min(v1.Y, v2.Y));

		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		public static Vector3 Min(this Vector3 v1, Vector3 v2) => new(MathF.Min(v1.X, v2.X), MathF.Min(v1.Y, v2.Y), MathF.Min(v1.Z, v2.Z));

		/// <summary>
		/// Computes the minimum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of minimum values</returns>
		public static Vector4 Min(this Vector4 v1, Vector4 v2) => new(MathF.Min(v1.X, v2.X), MathF.Min(v1.Y, v2.Y), MathF.Min(v1.Y, v2.Y), MathF.Min(v1.W, v2.W));


		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		public static Vector2 Max(this Vector2 v1, Vector2 v2) => new(MathF.Max(v1.X, v2.X), MathF.Max(v1.Y, v2.Y));

		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		public static Vector3 Max(this Vector3 v1, Vector3 v2) => new(MathF.Max(v1.X, v2.X), MathF.Max(v1.Y, v2.Y), MathF.Max(v1.Z, v2.Z));

		/// <summary>
		/// Computes the maximum of each component between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector of maximum values</returns>
		public static Vector4 Max(this Vector4 v1, Vector4 v2) => new(MathF.Max(v1.X, v2.X), MathF.Max(v1.Y, v2.Y), MathF.Max(v1.Y, v2.Y), MathF.Max(v1.W, v2.W));


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
		public static float Dot(this Vector2 v1, Vector2 v2) => (v1 * v2).Sum();

		/// <summary>
		/// Computes the dot product between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector dot product</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(this Vector3 v1, Vector3 v2) => (v1 * v2).Sum();

		/// <summary>
		/// Computes the dot product between this and a second vector.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Vector dot product</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(this Vector4 v1, Vector4 v2) => (v1 * v2).Sum();

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	}

}

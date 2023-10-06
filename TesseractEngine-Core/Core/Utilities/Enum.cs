using System;
using System.Collections.Generic;

namespace Tesseract.Core.Utilities {

	/// <summary>
	/// Utilities for enumeration types.
	/// </summary>
	public static class EnumUtils {

		/// <summary>
		/// Enumeration information record.
		/// </summary>
		public readonly struct EnumInfo {

			/// <summary>
			/// The maximum known value in the enumeration, cast as a signed 64-bit integer.
			/// </summary>
			public long MaxValue { get; init; }

		}

		// Cache of information for enum types
		private static readonly Dictionary<Type, EnumInfo> infos = new();

		/// <summary>
		/// Gets the information for the given enumeration type.
		/// </summary>
		/// <typeparam name="T">Enumeration type</typeparam>
		/// <returns>Enumeration info</returns>
		public static EnumInfo GetInfo<T>() where T : struct, Enum {
			lock (infos) {
				if (infos.TryGetValue(typeof(T), out var info)) return info;

				T[] vals = Enum.GetValues<T>();
				long maxval = 0;
				foreach(T val in vals) {
					long lval = (long)(object)val;
					maxval = Math.Max(maxval, lval);
				}

				info = new() {
					MaxValue = maxval
				};
				infos[typeof(T)] = info;
				return info;
			}
		}

		public static long GetMaxValue<T>() where T : struct, Enum => GetInfo<T>().MaxValue;

	}

	/// <summary>
	/// Interface for types that act as enumerations of the given value type.
	/// </summary>
	/// <typeparam name="T">Enumeration value type</typeparam>
	public interface IValuedEnum<T> : IEquatable<IValuedEnum<T>> {

		/// <summary>
		/// The underlying unique enumeration value.
		/// </summary>
		public T Value { get; }

	}

}

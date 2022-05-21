using System;
using System.Collections.Generic;

namespace Tesseract.Core.Util {

	public static class EnumUtils {

		public readonly struct EnumInfo {

			public long MaxValue { get; init; }

		}

		private static readonly Dictionary<Type, EnumInfo> infos = new();

		public static EnumInfo GetInfo<T>() where T : struct, Enum {
			lock (infos) {
				if (infos.TryGetValue(typeof(T), out var info)) return info;

				T[] vals = Enum.GetValues<T>();
				long maxval = 0;
				foreach(T val in vals) {
					long lval = (long)(object)val;
					maxval = System.Math.Max(maxval, lval);
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

	public interface IValuedEnum<T> : IEquatable<IValuedEnum<T>> {

		public T Value { get; }

	}

}

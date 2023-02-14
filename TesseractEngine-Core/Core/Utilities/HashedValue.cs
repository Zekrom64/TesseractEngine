using System;

namespace Tesseract.Core.Utilities {
	/// <summary>
	/// <para>
	/// A hashed value is a value type that encapsulates a value and a cached hash code for
	/// that value. The hashed value overrides hash code and equality functions such that
	/// the hash code of the hashed value is the cached hash code and hash equality is
	/// checked before regular object equality. Note that because the hash code is cached
	/// the type of the underlying value should be immutable/unmodifiable to prevent
	/// the hash code from changing once initialized.
	/// </para>
	/// <para>The primary use for hashed values is to improve performance of hash-based
	/// containers (dictionaries, sets) when using complex keys that may have significant
	/// overhead in generating hash codes.</para>
	/// </summary>
	/// <typeparam name="T">Underlying value type</typeparam>
	public struct HashedValue<T> : IEquatable<HashedValue<T>>, IEquatable<T> {

		/// <summary>
		/// Computes the hash code for any type. This will return 0 if the value is null for
		/// nullable types, otherwise it returns the regular <see cref="GetHashCode"/> result.
		/// </summary>
		/// <param name="val">Type value</param>
		/// <returns>Value hash code</returns>
		public static int Hash(T? val) {
			if (Equals(null, val)) return 0;
			else return val.GetHashCode();
		}

		private bool flag;
		private T? value;
		/// <summary>
		/// The underlying value being hashed.
		/// </summary>
		public T? Value {
			get => value;
			set {
				flag = true;
				this.value = value;
			}
		}

		private int hash;
		/// <summary>
		/// The cached hash code for the corresponding value.
		/// </summary>
		public int HashValue {
			get {
				if (flag) hash = Hash(value);
				return hash;
			}
		}

		/// <summary>
		/// Creates a new hashed value.
		/// </summary>
		/// <param name="val">Initial value</param>
		public HashedValue(T? val) {
			value = val;
			hash = Hash(val);
			flag = false;
		}

		public bool Equals(HashedValue<T> other) => HashValue == other.HashValue && Equals(Value, other.Value);

		public bool Equals(T? val) => HashValue == Hash(val) && Equals(Value, val);

		public override bool Equals(object? obj) =>
			(obj is HashedValue<T> hv && Equals(hv)) ||
			(obj is T val && Equals(val));

		public override int GetHashCode() => HashValue;

		public static bool operator ==(HashedValue<T> hv1, HashedValue<T> hv2) => hv1.Equals(hv2);

		public static bool operator !=(HashedValue<T> hv1, HashedValue<T> hv2) => !hv1.Equals(hv2);

		public static implicit operator HashedValue<T>(T val) => new(val);

		public static implicit operator T?(HashedValue<T> hv) => hv.Value;

	}

}

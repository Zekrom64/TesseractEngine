using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Math {
	
	/// <summary>
	/// A two-component vector of 32-bit integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2i : ITuple2<int>, IEquatable<IReadOnlyTuple2<int>> {

		/// <summary>
		/// Vector X value.
		/// </summary>
		public int X;
		/// <summary>
		/// Vector Y value.
		/// </summary>
		public int Y;

		int ITuple2<int>.X { get => X; set => X = value; }

		int IReadOnlyTuple2<int>.X => X;

		int ITuple2<int>.Y { get => Y; set => X = value; }

		int IReadOnlyTuple2<int>.Y => Y;

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector2i(int s) {
			X = Y = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		public Vector2i(int x, int y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector2i(IReadOnlyTuple2<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		public bool Equals(IReadOnlyTuple2<int> other) => X == other.X && Y == other.Y;

		public override bool Equals(object obj) => obj is IReadOnlyTuple2<int> other && Equals(other);

		public override int GetHashCode() => X ^ (Y << 16);

		public override string ToString() => $"({X},{Y})";

		/// <summary>
		/// Indexes the values in this vector.
		/// </summary>
		/// <param name="index">Index into vector</param>
		/// <returns>Value at index in vector</returns>
		public int this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
				}
			}
		}

		// Equality operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2i left, Vector2i right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2i left, Vector2i right) => !(left == right);

		// Arithmetic operators
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator +(Vector2i left, Vector2i right) => new(left.X + right.X, left.Y + right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator +(Vector2i left, int right) => new(left.X + right, left.Y + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator -(Vector2i left, Vector2i right) => new(left.X - right.X, left.Y - right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator -(Vector2i left, int right) => new(left.X - right, left.Y - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator *(Vector2i left, Vector2i right) => new(left.X * right.X, left.Y * right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator *(Vector2i left, int right) => new(left.X * right, left.Y * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator /(Vector2i left, Vector2i right) => new(left.X / right.X, left.Y / right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator /(Vector2i left, int right) => new(left.X / right, left.Y / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator %(Vector2i left, Vector2i right) => new(left.X % right.X, left.Y % right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator %(Vector2i left, int right) => new(left.X % right, left.Y % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator -(Vector2i value) => new(-value.X, -value.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator +(Vector2i value) => new(+value.X, +value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator ++(Vector2i value) => new(value.X + 1, value.Y + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator --(Vector2i value) => new(value.X - 1, value.Y - 1);

		// Bitwise operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator >>(Vector2i left, int right) => new(left.X >> right, left.Y >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator <<(Vector2i left, int right) => new(left.X << right, left.Y << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator ~(Vector2i value) => new(~value.X, ~value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator &(Vector2i left, Vector2i right) => new(left.X & right.X, left.Y & right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator &(Vector2i left, int right) => new(left.X & right, left.Y & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator |(Vector2i left, Vector2i right) => new(left.X | right.X, left.Y | right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator |(Vector2i left, int right) => new(left.X | right, left.Y | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator ^(Vector2i left, Vector2i right) => new(left.X ^ right.X, left.Y ^ right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator ^(Vector2i left, int right) => new(left.X ^ right, left.Y ^ right);

		public static implicit operator Vector<int>(Vector2i v) => new(stackalloc[] { v.X, v.Y });
		public static implicit operator Vector2i(Vector<int> v) => new(v[0], v[1]);

	}

	/// <summary>
	/// A two-component vector of 32-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2ui : ITuple2<uint>, IEquatable<IReadOnlyTuple2<uint>> {

		/// <summary>
		/// Vector X value.
		/// </summary>
		public uint X;
		/// <summary>
		/// Vector Y value.
		/// </summary>
		public uint Y;

		uint ITuple2<uint>.X { get => X; set => X = value; }

		uint IReadOnlyTuple2<uint>.X => X;

		uint ITuple2<uint>.Y { get => Y; set => X = value; }

		uint IReadOnlyTuple2<uint>.Y => Y;

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector2ui(uint s) {
			X = Y = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		public Vector2ui(uint x, uint y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector2ui(IReadOnlyTuple2<uint> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		public bool Equals(IReadOnlyTuple2<uint> other) => X == other.X && Y == other.Y;

		public override bool Equals(object obj) => obj is IReadOnlyTuple2<uint> other && Equals(other);

		public override int GetHashCode() => (int)(X ^ (Y << 16));

		public override string ToString() => $"({X},{Y})";

		/// <summary>
		/// Indexes the values in this vector.
		/// </summary>
		/// <param name="index">Index uinto vector</param>
		/// <returns>Value at index in vector</returns>
		public uint this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
				}
			}
		}

		// Equality operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2ui left, Vector2ui right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2ui left, Vector2ui right) => !(left == right);

		// Arithmetic operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator +(Vector2ui left, Vector2ui right) => new(left.X + right.X, left.Y + right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator +(Vector2ui left, uint right) => new(left.X + right, left.Y + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator -(Vector2ui left, Vector2ui right) => new(left.X - right.X, left.Y - right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator -(Vector2ui left, uint right) => new(left.X - right, left.Y - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator *(Vector2ui left, Vector2ui right) => new(left.X * right.X, left.Y * right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator *(Vector2ui left, uint right) => new(left.X * right, left.Y * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator /(Vector2ui left, Vector2ui right) => new(left.X / right.X, left.Y / right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator /(Vector2ui left, uint right) => new(left.X / right, left.Y / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator %(Vector2ui left, Vector2ui right) => new(left.X % right.X, left.Y % right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator %(Vector2ui left, uint right) => new(left.X % right, left.Y % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator -(Vector2ui value) => new((uint)-value.X, (uint)-value.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator +(Vector2ui value) => new(+value.X, +value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator ++(Vector2ui value) => new(value.X + 1, value.Y + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator --(Vector2ui value) => new(value.X - 1, value.Y - 1);

		// Bitwise operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator >>(Vector2ui left, int right) => new(left.X >> right, left.Y >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator <<(Vector2ui left, int right) => new(left.X << right, left.Y << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator ~(Vector2ui value) => new(~value.X, ~value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator &(Vector2ui left, Vector2ui right) => new(left.X & right.X, left.Y & right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator &(Vector2ui left, uint right) => new(left.X & right, left.Y & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator |(Vector2ui left, Vector2ui right) => new(left.X | right.X, left.Y | right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator |(Vector2ui left, uint right) => new(left.X | right, left.Y | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator ^(Vector2ui left, Vector2ui right) => new(left.X ^ right.X, left.Y ^ right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator ^(Vector2ui left, uint right) => new(left.X ^ right, left.Y ^ right);

		public static implicit operator Vector<uint>(Vector2ui v) => new(stackalloc[] { v.X, v.Y });
		public static implicit operator Vector2ui(Vector<uint> v) => new(v[0], v[1]);

	}

	/// <summary>
	/// A two-component vector of 64-bit floating point values.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2d : ITuple2<double>, IEquatable<IReadOnlyTuple2<double>> {

		/// <summary>
		/// Vector X value.
		/// </summary>
		public double X;
		/// <summary>
		/// Vector Y value.
		/// </summary>
		public double Y;

		double ITuple2<double>.X { get => X; set => X = value; }

		double IReadOnlyTuple2<double>.X => X;

		double ITuple2<double>.Y { get => Y; set => X = value; }

		double IReadOnlyTuple2<double>.Y => Y;

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector2d(double s) {
			X = Y = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		public Vector2d(double x, double y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector2d(IReadOnlyTuple2<double> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		public bool Equals(IReadOnlyTuple2<double> other) => X == other.X && Y == other.Y;

		public override bool Equals(object obj) => obj is IReadOnlyTuple2<double> other && Equals(other);

		public override int GetHashCode() => (int)(BitConverter.DoubleToInt64Bits(X) ^ BitConverter.DoubleToInt64Bits(Y));

		public override string ToString() => $"({X},{Y})";

		/// <summary>
		/// Indexes the values in this vector.
		/// </summary>
		/// <param name="index">Index doubleo vector</param>
		/// <returns>Value at index in vector</returns>
		public double this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
				}
			}
		}

		// Equality operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2d left, Vector2d right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2d left, Vector2d right) => !(left == right);

		// Arithmetic operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator +(Vector2d left, Vector2d right) => new(left.X + right.X, left.Y + right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator +(Vector2d left, double right) => new(left.X + right, left.Y + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator -(Vector2d left, Vector2d right) => new(left.X - right.X, left.Y - right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator -(Vector2d left, double right) => new(left.X - right, left.Y - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator *(Vector2d left, Vector2d right) => new(left.X * right.X, left.Y * right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator *(Vector2d left, double right) => new(left.X * right, left.Y * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator /(Vector2d left, Vector2d right) => new(left.X / right.X, left.Y / right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator /(Vector2d left, double right) => new(left.X / right, left.Y / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator %(Vector2d left, Vector2d right) => new(left.X % right.X, left.Y % right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator %(Vector2d left, double right) => new(left.X % right, left.Y % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator -(Vector2d value) => new(-value.X, -value.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator +(Vector2d value) => new(+value.X, +value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator ++(Vector2d value) => new(value.X + 1, value.Y + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator --(Vector2d value) => new(value.X - 1, value.Y - 1);

	}

	/// <summary>
	/// A three-component of 32-bit integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3i : ITuple3<int>, IEquatable<IReadOnlyTuple3<int>> {

		/// <summary>
		/// Vector X component.
		/// </summary>
		public int X;
		/// <summary>
		/// Vector Y component.
		/// </summary>
		public int Y;
		/// <summary>
		/// Vector Z component.
		/// </summary>
		public int Z;

		int ITuple2<int>.X { get => X; set => X = value; }

		int IReadOnlyTuple2<int>.X => X;

		int ITuple2<int>.Y { get => Y; set => X = value; }

		int IReadOnlyTuple2<int>.Y => Y;

		int ITuple3<int>.Z { get => Z; set => Z = value; }

		int IReadOnlyTuple3<int>.Z => Z;

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector3i(int s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		public Vector3i(int x, int y, int z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector3i(IReadOnlyTuple3<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public bool Equals(IReadOnlyTuple3<int> other) => X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object obj) => obj is IReadOnlyTuple3<int> other && Equals(other);

		public override int GetHashCode() => X ^ (Y << 10) ^ (Z << 20);

		public override string ToString() => $"({X},{Y},{Z})";

		public int this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				2 => Z,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3i left, Vector3i right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3i left, Vector3i right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator +(Vector3i left, Vector3i right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator +(Vector3i left, int right) => new(left.X + right, left.Y + right, left.Z + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator -(Vector3i left, Vector3i right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator -(Vector3i left, int right) => new(left.X - right, left.Y - right, left.Z - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator *(Vector3i left, Vector3i right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator *(Vector3i left, int right) => new(left.X * right, left.Y * right, left.Z * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator /(Vector3i left, Vector3i right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator /(Vector3i left, int right) => new(left.X / right, left.Y / right, left.Z / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator %(Vector3i left, Vector3i right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator %(Vector3i left, int right) => new(left.X % right, left.Y % right, left.Z % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator -(Vector3i value) => new(-value.X, -value.Y, -value.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator +(Vector3i value) => new(+value.X, +value.Y, +value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator ++(Vector3i value) => new(value.X + 1, value.Y + 1, value.Z + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator --(Vector3i value) => new(value.X - 1, value.Y - 1, value.Z - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator >>(Vector3i left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator <<(Vector3i left, int right) => new(left.X << right, left.Y << right, left.Z << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator ~(Vector3i value) => new(~value.X, ~value.Y, ~value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator &(Vector3i left, Vector3i right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator &(Vector3i left, int right) => new(left.X & right, left.Y & right, left.Z & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator |(Vector3i left, Vector3i right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator |(Vector3i left, int right) => new(left.X | right, left.Y | right, left.Z | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator ^(Vector3i left, Vector3i right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator ^(Vector3i left, int right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);

		public static implicit operator Vector<int>(Vector3i v) => new(stackalloc[] { v.X, v.Y, v.Z });
		public static implicit operator Vector3i(Vector<int> v) => new(v[0], v[1], v[2]);

	}

	/// <summary>
	/// A three-component of 32-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3ui : ITuple3<uint>, IEquatable<IReadOnlyTuple3<uint>> {

		/// <summary>
		/// Vector X component.
		/// </summary>
		public uint X;
		/// <summary>
		/// Vector Y component.
		/// </summary>
		public uint Y;
		/// <summary>
		/// Vector Z component.
		/// </summary>
		public uint Z;

		uint ITuple2<uint>.X { get => X; set => X = value; }

		uint IReadOnlyTuple2<uint>.X => X;

		uint ITuple2<uint>.Y { get => Y; set => X = value; }

		uint IReadOnlyTuple2<uint>.Y => Y;

		uint ITuple3<uint>.Z { get => Z; set => Z = value; }

		uint IReadOnlyTuple3<uint>.Z => Z;

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector3ui(uint s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		public Vector3ui(uint x, uint y, uint z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector3ui(IReadOnlyTuple3<uint> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public bool Equals(IReadOnlyTuple3<uint> other) => X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object obj) => obj is IReadOnlyTuple3<uint> other && Equals(other);

		public override int GetHashCode() => (int)(X ^ (Y << 10) ^ (Z << 20));

		public override string ToString() => $"({X},{Y},{Z})";

		public uint this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				2 => Z,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3ui left, Vector3ui right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3ui left, Vector3ui right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator +(Vector3ui left, Vector3ui right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator +(Vector3ui left, uint right) => new(left.X + right, left.Y + right, left.Z + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator -(Vector3ui left, Vector3ui right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator -(Vector3ui left, uint right) => new(left.X - right, left.Y - right, left.Z - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator *(Vector3ui left, Vector3ui right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator *(Vector3ui left, uint right) => new(left.X * right, left.Y * right, left.Z * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator /(Vector3ui left, Vector3ui right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator /(Vector3ui left, uint right) => new(left.X / right, left.Y / right, left.Z / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator %(Vector3ui left, Vector3ui right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator %(Vector3ui left, uint right) => new(left.X % right, left.Y % right, left.Z % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator -(Vector3ui value) => new((uint)-value.X, (uint)-value.Y, (uint)-value.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator +(Vector3ui value) => new(+value.X, +value.Y, +value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator ++(Vector3ui value) => new(value.X + 1, value.Y + 1, value.Z + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator --(Vector3ui value) => new(value.X - 1, value.Y - 1, value.Z - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator >>(Vector3ui left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator <<(Vector3ui left, int right) => new(left.X << right, left.Y << right, left.Z << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator ~(Vector3ui value) => new(~value.X, ~value.Y, ~value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator &(Vector3ui left, Vector3ui right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator &(Vector3ui left, uint right) => new(left.X & right, left.Y & right, left.Z & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator |(Vector3ui left, Vector3ui right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator |(Vector3ui left, uint right) => new(left.X | right, left.Y | right, left.Z | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator ^(Vector3ui left, Vector3ui right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator ^(Vector3ui left, uint right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);

		public static implicit operator Vector<uint>(Vector3ui v) => new(stackalloc[] { v.X, v.Y, v.Z });
		public static implicit operator Vector3ui(Vector<uint> v) => new(v[0], v[1], v[2]);

	}

	/// <summary>
	/// A three-component of 16-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3us : ITuple3<ushort>, IEquatable<IReadOnlyTuple3<ushort>> {

		/// <summary>
		/// Vector X component.
		/// </summary>
		public ushort X;
		/// <summary>
		/// Vector Y component.
		/// </summary>
		public ushort Y;
		/// <summary>
		/// Vector Z component.
		/// </summary>
		public ushort Z;

		ushort ITuple2<ushort>.X { get => X; set => X = value; }

		ushort IReadOnlyTuple2<ushort>.X => X;

		ushort ITuple2<ushort>.Y { get => Y; set => X = value; }

		ushort IReadOnlyTuple2<ushort>.Y => Y;

		ushort ITuple3<ushort>.Z { get => Z; set => Z = value; }

		ushort IReadOnlyTuple3<ushort>.Z => Z;

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector3us(ushort s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector3us(int s) {
			X = Y = Z = (ushort)s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		public Vector3us(ushort x, ushort y, ushort z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		public Vector3us(int x, int y, int z) {
			X = (ushort)x;
			Y = (ushort)y;
			Z = (ushort)z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector3us(IReadOnlyTuple3<ushort> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public bool Equals(IReadOnlyTuple3<ushort> other) => X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object obj) => obj is IReadOnlyTuple3<ushort> other && Equals(other);

		public override int GetHashCode() => X ^ (Y << 10) ^ (Z << 20);

		public override string ToString() => $"({X},{Y},{Z})";

		public ushort this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				2 => Z,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3us left, Vector3us right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3us left, Vector3us right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator +(Vector3us left, Vector3us right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator +(Vector3us left, int right) => new(left.X + right, left.Y + right, left.Z + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator -(Vector3us left, Vector3us right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator -(Vector3us left, int right) => new(left.X - right, left.Y - right, left.Z - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator *(Vector3us left, Vector3us right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator *(Vector3us left, int right) => new(left.X * right, left.Y * right, left.Z * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator /(Vector3us left, Vector3us right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator /(Vector3us left, int right) => new(left.X / right, left.Y / right, left.Z / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator %(Vector3us left, Vector3us right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator %(Vector3us left, int right) => new(left.X % right, left.Y % right, left.Z % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator -(Vector3us value) => new(-value.X, -value.Y, -value.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator +(Vector3us value) => new(+value.X, +value.Y, +value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator ++(Vector3us value) => new(value.X + 1, value.Y + 1, value.Z + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator --(Vector3us value) => new(value.X - 1, value.Y - 1, value.Z - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator >>(Vector3us left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator <<(Vector3us left, int right) => new(left.X << right, left.Y << right, left.Z << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator ~(Vector3us value) => new(~value.X, ~value.Y, ~value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator &(Vector3us left, Vector3us right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator &(Vector3us left, int right) => new(left.X & right, left.Y & right, left.Z & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator |(Vector3us left, Vector3us right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator |(Vector3us left, int right) => new(left.X | right, left.Y | right, left.Z | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator ^(Vector3us left, Vector3us right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator ^(Vector3us left, int right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);

	}

	/// <summary>
	/// A three-component of 32-bit integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4i : ITuple4<int>, IEquatable<IReadOnlyTuple4<int>> {

		/// <summary>
		/// Vector X component.
		/// </summary>
		public int X;
		/// <summary>
		/// Vector Y component.
		/// </summary>
		public int Y;
		/// <summary>
		/// Vector Z component.
		/// </summary>
		public int Z;
		/// <summary>
		/// Vector W component.
		/// </summary>
		public int W;

		int ITuple2<int>.X { get => X; set => X = value; }

		int IReadOnlyTuple2<int>.X => X;

		int ITuple2<int>.Y { get => Y; set => X = value; }

		int IReadOnlyTuple2<int>.Y => Y;

		int ITuple3<int>.Z { get => Z; set => Z = value; }

		int IReadOnlyTuple3<int>.Z => Z;

		int ITuple4<int>.W { get => W; set => W = value; }

		int IReadOnlyTuple4<int>.W => W;

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector4i(int s) {
			X = Y = Z = W = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		/// <param name="w">W component value</param>
		public Vector4i(int x, int y, int z, int w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector4i(IReadOnlyTuple4<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = tuple.W;
		}

		public bool Equals(IReadOnlyTuple4<int> other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

		public override bool Equals(object obj) => obj is IReadOnlyTuple4<int> other && Equals(other);

		public override int GetHashCode() => X ^ (Y << 8) ^ (Z << 16) ^ (W << 24);

		public override string ToString() => $"({X},{Y},{Z},{W})";

		public int this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				2 => Z,
				3 => W,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					case 3: W = value; break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector4i left, Vector4i right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4i left, Vector4i right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator +(Vector4i left, Vector4i right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator +(Vector4i left, int right) => new(left.X + right, left.Y + right, left.Z + right, left.W + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator -(Vector4i left, Vector4i right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator -(Vector4i left, int right) => new(left.X - right, left.Y - right, left.Z - right, left.W - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator *(Vector4i left, Vector4i right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator *(Vector4i left, int right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator /(Vector4i left, Vector4i right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator /(Vector4i left, int right) => new(left.X / right, left.Y / right, left.Z / right, left.W / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator %(Vector4i left, Vector4i right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z, left.W % right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator %(Vector4i left, int right) => new(left.X % right, left.Y % right, left.Z % right, left.W % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator -(Vector4i value) => new(-value.X, -value.Y, -value.Z, -value.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator +(Vector4i value) => new(+value.X, +value.Y, +value.Z, +value.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator ++(Vector4i value) => new(value.X + 1, value.Y + 1, value.Z + 1, value.W + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator --(Vector4i value) => new(value.X - 1, value.Y - 1, value.Z - 1, value.W - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator >>(Vector4i left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right, left.W >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator <<(Vector4i left, int right) => new(left.X << right, left.Y << right, left.Z << right, left.W << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator ~(Vector4i value) => new(~value.X, ~value.Y, ~value.Z, ~value.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator &(Vector4i left, Vector4i right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z, left.W & right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator &(Vector4i left, int right) => new(left.X & right, left.Y & right, left.Z & right, left.W & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator |(Vector4i left, Vector4i right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z, left.W | right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator |(Vector4i left, int right) => new(left.X | right, left.Y | right, left.Z | right, left.W | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator ^(Vector4i left, Vector4i right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z, left.W ^ right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator ^(Vector4i left, int right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right, left.W ^ right);

		public static implicit operator Vector<int>(Vector4i v) => new(stackalloc[] { v.X, v.Y, v.Z, v.W });
		public static implicit operator Vector4i(Vector<int> v) => new(v[0], v[1], v[2], v[3]);

	}

	/// <summary>
	/// A three-component of 32-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4ui : ITuple4<uint>, IEquatable<IReadOnlyTuple4<uint>> {

		/// <summary>
		/// Vector X component.
		/// </summary>
		public uint X;
		/// <summary>
		/// Vector Y component.
		/// </summary>
		public uint Y;
		/// <summary>
		/// Vector Z component.
		/// </summary>
		public uint Z;
		/// <summary>
		/// Vector W component.
		/// </summary>
		public uint W;

		uint ITuple2<uint>.X { get => X; set => X = value; }

		uint IReadOnlyTuple2<uint>.X => X;

		uint ITuple2<uint>.Y { get => Y; set => X = value; }

		uint IReadOnlyTuple2<uint>.Y => Y;

		uint ITuple3<uint>.Z { get => Z; set => Z = value; }

		uint IReadOnlyTuple3<uint>.Z => Z;

		uint ITuple4<uint>.W { get => W; set => W = value; }

		uint IReadOnlyTuple4<uint>.W => W;

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector4ui(uint s) {
			X = Y = Z = W = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		/// <param name="w">W component value</param>
		public Vector4ui(uint x, uint y, uint z, uint w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector4ui(IReadOnlyTuple4<uint> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = tuple.W;
		}

		public bool Equals(IReadOnlyTuple4<uint> other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

		public override bool Equals(object obj) => obj is IReadOnlyTuple4<uint> other && Equals(other);

		public override int GetHashCode() => (int)(X ^ (Y << 8) ^ (Z << 16) ^ (W << 24));

		public override string ToString() => $"({X},{Y},{Z},{W})";

		public uint this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				2 => Z,
				3 => W,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					case 3: W = value; break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector4ui left, Vector4ui right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4ui left, Vector4ui right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator +(Vector4ui left, Vector4ui right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator +(Vector4ui left, uint right) => new(left.X + right, left.Y + right, left.Z + right, left.W + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator -(Vector4ui left, Vector4ui right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator -(Vector4ui left, uint right) => new(left.X - right, left.Y - right, left.Z - right, left.W - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator *(Vector4ui left, Vector4ui right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator *(Vector4ui left, uint right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator /(Vector4ui left, Vector4ui right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator /(Vector4ui left, uint right) => new(left.X / right, left.Y / right, left.Z / right, left.W / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator %(Vector4ui left, Vector4ui right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z, left.W % right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator %(Vector4ui left, uint right) => new(left.X % right, left.Y % right, left.Z % right, left.W % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator -(Vector4ui value) => new((uint)-value.X, (uint)-value.Y, (uint)-value.Z, (uint)-value.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator +(Vector4ui value) => new(+value.X, +value.Y, +value.Z, +value.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator ++(Vector4ui value) => new(value.X + 1, value.Y + 1, value.Z + 1, value.W + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator --(Vector4ui value) => new(value.X - 1, value.Y - 1, value.Z - 1, value.W - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator >>(Vector4ui left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right, left.W >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator <<(Vector4ui left, int right) => new(left.X << right, left.Y << right, left.Z << right, left.W << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator ~(Vector4ui value) => new(~value.X, ~value.Y, ~value.Z, ~value.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator &(Vector4ui left, Vector4ui right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z, left.W & right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator &(Vector4ui left, uint right) => new(left.X & right, left.Y & right, left.Z & right, left.W & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator |(Vector4ui left, Vector4ui right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z, left.W | right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator |(Vector4ui left, uint right) => new(left.X | right, left.Y | right, left.Z | right, left.W | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator ^(Vector4ui left, Vector4ui right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z, left.W ^ right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator ^(Vector4ui left, uint right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right, left.W ^ right);

		public static implicit operator Vector<uint>(Vector4ui v) => new(stackalloc[] { v.X, v.Y, v.Z, v.W });
		public static implicit operator Vector4ui(Vector<uint> v) => new(v[0], v[1], v[2], v[3]);

	}

	/// <summary>
	/// Contains utilities for vector mathematics.
	/// </summary>
	public static class Vecmath {

		/// <summary>
		/// Converts a Structure-of-Arrays data structure to an Array-of-Structures data structure.
		/// </summary>
		/// <typeparam name="V">Structure in Array-of-Structures</typeparam>
		/// <typeparam name="T">Array element type in Structure-of-Arrays</typeparam>
		/// <param name="a1">First array from Structure-of-Arrays</param>
		/// <param name="a2">Second array from Structure-of-Arrays</param>
		/// <param name="ctor">Constructor for structure type in Array-of-Structures</param>
		/// <returns>Array-of-Structures</returns>
		public static V[] SOAToAOS<V,T>(T[] a1, T[] a2) where V : ITuple2<T>, new() {
			int length = System.Math.Min(a1.Length, a2.Length);
			V[] aos = new V[length];
			for (int i = 0; i < length; i++) aos[length] = new V() { X = a1[i], Y = a2[i] };
			return aos;
		}

		/// <summary>
		/// Converts a Structure-of-Arrays data structure to an Array-of-Structures data structure.
		/// </summary>
		/// <typeparam name="V">Structure in Array-of-Structures</typeparam>
		/// <typeparam name="T">Array element type in Structure-of-Arrays</typeparam>
		/// <param name="a1">First array from Structure-of-Arrays</param>
		/// <param name="a2">Second array from Structure-of-Arrays</param>
		/// <param name="ctor">Constructor for structure type in Array-of-Structures</param>
		/// <returns>Array-of-Structures</returns>
		public static Span<V> SOAToAOS<V, T>(Span<T> a1, Span<T> a2) where V : ITuple2<T>, new() {
			int length = System.Math.Min(a1.Length, a2.Length);
			V[] aos = new V[length];
			for (int i = 0; i < length; i++) aos[length] = new V() { X = a1[i], Y = a2[i] };
			return aos;
		}

		/// <summary>
		/// Converts a Structure-of-Arrays data structure to an Array-of-Structures data structure.
		/// </summary>
		/// <typeparam name="V">Structure in Array-of-Structures</typeparam>
		/// <typeparam name="T">Array element type in Structure-of-Arrays</typeparam>
		/// <param name="aos">Array-of-Structures to store into</param>
		/// <param name="a1">First array from Structure-of-Arrays</param>
		/// <param name="a2">Second array from Structure-of-Arrays</param>
		/// <param name="ctor">Constructor for structure type in Array-of-Structures</param>
		/// <returns>Array-of-Structures</returns>
		public static Span<V> SOAToAOS<V, T>(Span<V> aos, Span<T> a1, Span<T> a2) where V : ITuple2<T>, new() {
			int length = System.Math.Min(aos.Length, System.Math.Min(a1.Length, a2.Length));
			for (int i = 0; i < length; i++) aos[length] = new V() { X = a1[i], Y = a2[i] };
			return aos;
		}

		public static V[] SOAToAOS<V, T>(T[] a1, T[] a2, T[] a3) where V : ITuple3<T>, new() {
			int length = System.Math.Min(System.Math.Min(a1.Length, a2.Length), a3.Length);
			V[] aos = new V[length];
			for (int i = 0; i < length; i++) aos[length] = new V() { X = a1[i], Y = a2[i], Z = a3[i] };
			return aos;
		}

	}

}

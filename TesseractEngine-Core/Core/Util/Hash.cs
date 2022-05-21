using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.Core.Util {
	
	/// <summary>
	/// A pure C# implementation of the xxHash64 hashing algorithm.
	/// </summary>
	public struct XXHash64 {

		private const ulong Prime1 = 11400714785074694791;
		private const ulong Prime2 = 14029467366897019727;
		private const ulong Prime3 = 1609587929392839161;
		private const ulong Prime4 = 9650029242287828579;
		private const ulong Prime5 = 2870177450012600261;

		private const int MaxBufferSize = 32;

		private unsafe fixed ulong state[4]; // State variables
		private unsafe fixed byte buffer[MaxBufferSize]; // Input buffer
		private int bufferSize;
		private int totalLength;

		/// <summary>
		/// The 64-bit hash value.
		/// </summary>
		public ulong Hash {
			get {
				unsafe {
					ulong result;

					if (totalLength >= MaxBufferSize) {
						result =
							BitOperations.RotateLeft(state[0], 1) +
							BitOperations.RotateLeft(state[1], 7) +
							BitOperations.RotateLeft(state[2], 12) +
							BitOperations.RotateLeft(state[3], 18);
						result = (result ^ ProcessSingle(0, state[0])) * Prime1 + Prime4;
						result = (result ^ ProcessSingle(0, state[1])) * Prime1 + Prime4;
						result = (result ^ ProcessSingle(0, state[2])) * Prime1 + Prime4;
						result = (result ^ ProcessSingle(0, state[3])) * Prime1 + Prime4;
					} else {
						result = state[2] + Prime5;
					}

					result += (uint)totalLength;

					fixed(byte* pbuffer = buffer) {
						byte* data = pbuffer;
						byte* stop = data + bufferSize;

						for (; data + 8 <= stop; data += 8)
							result = BitOperations.RotateLeft(result ^ ProcessSingle(0, *(ulong*)data), 27) * Prime1 + Prime4;

						if (data + 4 <= stop) {
							result = BitOperations.RotateLeft(result ^ (*(uint*)data) * Prime1, 23) * Prime2 + Prime3;
							data += 4;
						}

						while (data != stop)
							result = BitOperations.RotateLeft(result ^ (*data++) * Prime5, 11) * Prime1;
					}

					result ^= result >> 33;
					result *= Prime2;
					result ^= result >> 29;
					result *= Prime3;
					result ^= result >> 32;
					return result;
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the xxHash64 algorithm, initialized with the given seed.
		/// </summary>
		/// <param name="seed">Seed value</param>
		public XXHash64(ulong seed = 0) {
			unsafe {
				state[0] = seed + Prime1 + Prime2;
				state[1] = seed + Prime2;
				state[2] = seed;
				state[3] = seed - Prime1;
			}
			bufferSize = totalLength = 0;
		}
		
		/// <summary>
		/// Adds a sequence of bytes to the hash.
		/// </summary>
		/// <param name="input">Bytes to add to hash</param>
		/// <returns>This hash</returns>
		public XXHash64 Add(in ReadOnlySpan<byte> input) {
			unsafe {
				int length = input.Length;
				if (length == 0) return this;
				totalLength += length;

				fixed(byte* pinput = input, pbuffer = buffer) {
					byte* data = pinput;

					if (bufferSize + input.Length < MaxBufferSize) {
						while (length-- > 0) buffer[bufferSize++] = *data++;
						return this;
					}

					byte* stop = data + length;
					byte* stopBlock = stop - MaxBufferSize;

					if (bufferSize > 0) {
						while (bufferSize < MaxBufferSize) buffer[bufferSize++] = *data++;
						Process((ulong*)pbuffer, ref state[0], ref state[1], ref state[2], ref state[3]);
					}

					ulong s0 = state[0], s1 = state[1], s2 = state[2], s3 = state[3];
					while(data <= stopBlock) {
						Process((ulong*)data, ref s0, ref s1, ref s2, ref s3);
						data += 32;
					}
					state[0] = s0;
					state[1] = s1;
					state[2] = s2;
					state[3] = s3;

					bufferSize = (int)(stop - data);
					for (int i = 0; i < bufferSize; i++)
						buffer[i] = data[i];
				}
			}
			return this;
		}

		/// <summary>
		/// Adds the byte representation of an unmanaged value to the hash.
		/// </summary>
		/// <typeparam name="T">Unmanaged type</typeparam>
		/// <param name="val">Value to add to hash</param>
		/// <returns>This hash</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public XXHash64 Add<T>(T val) where T : unmanaged {
			unsafe {
				return Add(new ReadOnlySpan<byte>(&val, sizeof(T)));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong ProcessSingle(ulong previous, ulong input) =>
			BitOperations.RotateLeft(previous + input * Prime2, 31) * Prime1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void Process(ulong* block, ref ulong state0, ref ulong state1, ref ulong state2, ref ulong state3) {
			state0 = ProcessSingle(state0, block[0]);
			state1 = ProcessSingle(state1, block[1]);
			state2 = ProcessSingle(state2, block[2]);
			state3 = ProcessSingle(state3, block[3]);
		}

		public override int GetHashCode() => (int)Hash;

		/// <summary>
		/// Computes the hash of a byte sequence.
		/// </summary>
		/// <param name="input">Byte sequence to hash</param>
		/// <param name="seed">Seed value</param>
		/// <returns>Hash value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong Compute(in ReadOnlySpan<byte> input, ulong seed = 0) {
			XXHash64 hash = new(seed);
			hash.Add(input);
			return hash;
		}

		/// <summary>
		/// Computes the hash the byte representation of an unmanaged value.
		/// </summary>
		/// <param name="input">Value to hash</param>
		/// <param name="seed">Seed value</param>
		/// <returns>Hash value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong Compute<T>(T input, ulong seed = 0) where T : unmanaged {
			XXHash64 hash = new(seed);
			hash.Add(input);
			return hash;
		}

		public static implicit operator ulong(XXHash64 hash) => hash.Hash;

	}

}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.Core.Collections {

	/// <summary>
	/// <para>Maps a collection of objects to integer coordinates on a 2D grid.</para>
	/// <para>
	/// The grid is stored using smaller chunks of entries that are an integer power of two in size. Within a chunk
	/// entries are indexed by 'minor' coordinates which are the coordinates of the entry bitwise ANDded with the
	/// <see cref="ChunkMask"/> value. All chunks in the grid are stored using a hash table with key values of
	/// 'major' coordinates, which are the coordinates of any particular entry for a chunk shifted right by
	/// the <see cref="ChunkShift"/> value.
	/// </para>
	/// </summary>
	/// <typeparam name="T">The type of entries stored in the grid</typeparam>
	public class Grid2D<T> {

		private readonly Vector2i chunkSize;
		private readonly Vector2i chunkMask, chunkShift;
		private readonly Dictionary<Vector2i, T[,]> chunks;

		/// <summary>
		/// The size of each chunk of entries.
		/// </summary>
		public Vector2i ChunkSize => chunkSize;

		public Vector2i ChunkMask => chunkMask;

		public Vector2i ChunkShift => chunkShift;

		public Grid2D(Vector2i chunkSize = default) {
			if (chunkSize == default) chunkSize = new(16);
			this.chunkSize = new(
				(int)BitOperations.RoundUpToPowerOf2((uint)chunkSize.X),
				(int)BitOperations.RoundUpToPowerOf2((uint)chunkSize.Y)
			);
			chunkMask = chunkSize - 1;
			chunkShift = new(
				BitOperations.Log2((uint)chunkSize.X),
				BitOperations.Log2((uint)chunkSize.Y)
			);
			chunks = new Dictionary<Vector2i, T[,]>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryGetChunk(Vector2i point, [MaybeNullWhen(false)] out T[,] chunk) => chunks.TryGetValue(point >> chunkShift, out chunk);

		public T[,] GetChunk(Vector2i point) {
			if (TryGetChunk(point, out T[,]? chunk)) return chunk;
			chunk = new T[chunkSize.X, chunkSize.Y];
			chunks[point >> chunkShift] = chunk;
			return chunk;
		}

		public void Add(Vector2i point, T item) => this[point] = item;

		public T this[Vector2i point] {
			get {
				Vector2i minorpos = point & chunkMask;
				return GetChunk(point)[minorpos.X, minorpos.Y];
			}
			set {
				Vector2i minorpos = point & chunkMask;
				GetChunk(point)[minorpos.X, minorpos.Y] = value;
			}
		}

	}

}

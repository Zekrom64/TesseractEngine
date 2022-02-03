﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Util {

	/// <summary>
	/// A "first-in first-out" stream, which will store all written bytes to a buffer
	/// which can then be read. Note that this stream is not seekable, and its length
	/// and position have slightly different meaning for <see cref="Length"/> and
	/// <see cref="Position"/>. The length corresponds to the number of bytes currently
	/// in the buffer (effectively an "available" value) and the position corresponds
	/// to the total number of bytes that have been read from the stream.
	/// </summary>
	public class FIFOStream : Stream {

		// The size of each block in the FIFO list
		private const int BlockSize = 4096;

		/* The FIFO is implemented by lists of blocks containing the actual data, with
		 * a variable keeping track of the actual number of bytes in the FIFO. Blocks
		 * are divided between "live" (containing actual data) and "dead" (no longer
		 * in use). Dead blocks are recycled to reduce GC overhead from constantly
		 * discarding and createding new blocks.
		 */

		// The list of "dead" blocks to be reused
		private LinkedList<byte[]> deadBlocks = new();
		// The list of "live" blocks in use
		private LinkedList<byte[]> liveBlocks = new();
		// The number of actual bytes in the live buffer
		private long liveLength = 0;
		// The offset into the firstmost live buffer that data actually starts at
		private int liveOffset = 0;
		// The total number of bytes that have been extracted from the FIFO
		private long walkedBytes = 0;

		public override bool CanRead => true;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length => liveLength;

		public override long Position { get => walkedBytes; set => throw new NotSupportedException(); }

		public override void Flush() {
			// Clear the dead blocks 
			deadBlocks.Clear();
		}

		public override int Read(byte[] buffer, int offset, int count) {
			if (count <= 0) return 0;
			// Clamp count to available length
			count = (int)System.Math.Min(count, liveLength);
			// While there are still bytes to read
			while(count > 0) {
				// Get next block
				byte[] block = liveBlocks.First!.Value;
				// Compute available bytes in block
				int avail = block.Length - liveOffset;
				// Compute number of bytes to read
				int len = System.Math.Min(avail, count);
				// Perform array copy
				Array.Copy(block, liveOffset, buffer, offset, len);
				// Adjust destination offset
				offset += len;
				// Adjust count
				count -= len;
				// Adjust read offset
				liveOffset += len;
				// If the block is now empty
				if (liveOffset == block.Length) {
					// Make the block "dead" and step to the next block
					liveBlocks.RemoveFirst();
					deadBlocks.AddLast(block);
					liveOffset = 0;
				}
			}
			// Increment walked bytes
			walkedBytes += count;
			liveLength -= count;
			return count;
		}

		public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

		public override void SetLength(long value) => throw new NotSupportedException();

		// Generates a new block, either from the dead list or allocated
		private byte[] NewBlock() {
			if (deadBlocks.Count > 0) {
				byte[] block = deadBlocks.First!.Value;
				deadBlocks.RemoveFirst();
				return block;
			} else return new byte[BlockSize];
		}

		public override void Write(byte[] buffer, int offset, int count) {
			if (count <= 0) return;
			// The live buffer *must* have at least one block in it
			if (liveBlocks.Count == 0) liveBlocks.AddLast(NewBlock());
			// Compute the write offset based on the current live offset and live length
			// The write offset will always be in the lastmost block
			int writeOffset = (int)((liveOffset + liveLength) % BlockSize);
			while(count > 0) {
				// Get last block
				byte[] block = liveBlocks.Last!.Value;
				// Compute available bytes
				int avail = block.Length - writeOffset;
				// Compute bytes to write
				int len = System.Math.Min(avail, count);
				// Copy bytes to the block
				Array.Copy(buffer, offset, block, writeOffset, len);
				// Adjust source offset
				offset += len;
				// Adjust count
				count -= len;
				// If there is still data to be written, we need to add a new block and reset
				if (count > 0) {
					liveBlocks.AddLast(NewBlock());
					writeOffset = 0;
				}
			}
			liveLength += count;
		}
	}

}

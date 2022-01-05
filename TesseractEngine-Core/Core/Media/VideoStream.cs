using System;
using Tesseract.Core.Graphics;
using Tesseract.Core.Native;

namespace Tesseract.Core.Media {

	/// <summary>
	/// <para>A video stream manages the streaming of uncompressed video frames.</para>
	/// <para>
	/// While the average framerate as specified by <see cref="MediaInfo.VideoFramerate"/> should be
	/// used as a target for encoding video, decoding video should use a "presentation timestamp"
	/// supplied when a frame is decoded. This timestamp is the time at which a frame should start
	/// being presented, and the frame with the most recent timestamp should be used. This
	/// timestamp is monotonically increasing; newly decoded frames will always have a timestamp
	/// greater than that of previous frames (ie. frames are supplied in presentation order instead
	/// of decoding order).
	/// </para>
	/// </summary>
	public interface IVideoStream {

		/// <summary>
		/// The pixel format used by uncompressed video frames.
		/// </summary>
		public PixelFormat Format { get; }

		/// <summary>
		/// Reads a single uncompressed frame to memory specified by the given pointer, returning
		/// if a frame was able to be read.
		/// </summary>
		/// <param name="pFramebuffer">Pointer to memory to copy the uncompressed frame into</param>
		/// <param name="pts">The presentation timestamp of the decoded frame</param>
		/// <returns>If a frame was read from the stream</returns>
		public bool ReadFrame(IPointer<byte> pFramebuffer, out TimeSpan pts);

	}

}

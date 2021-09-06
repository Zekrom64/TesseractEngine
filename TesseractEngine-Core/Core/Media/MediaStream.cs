using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Audio;

namespace Tesseract.Core.Media {
	
	/// <summary>
	/// A media stream handles decoding/encoding of different multimedia formats, handling
	/// audio and video primarily.
	/// </summary>
	public interface IMediaStream {

		/// <summary>
		/// Information about the media accessed by this stream.
		/// </summary>
		public MediaInfo Info { get; }

		/// <summary>
		/// The video component of this media stream, or null if there is no video.
		/// </summary>
		public IVideoStream VideoStream { get; }

		/// <summary>
		/// The audio component of this media stream, or null if there is no audio.
		/// </summary>
		public IAudioStream AudioStrema { get; }

		/// <summary>
		/// If the media stream's position can be changed.
		/// </summary>
		public bool Seekable { get; }

		/// <summary>
		/// The current position of the media stream.
		/// </summary>
		public MediaPosition Position { get; set; }

		/// <summary>
		/// <para>
		/// Attempts to advance the media stream, returning if the stream was able to advance. A media position
		/// may be supplied to suggest how much the stream should attempt to advance. Some implementations
		/// may choose to ignore this value.
		/// </para>
		/// <para>
		/// A media stream may be unable to advance due to several issues (eg. not enough data provided to decode
		/// more audio/video, internal buffers for some component being full).
		/// </para>
		/// </summary>
		/// <returns>If the media stream was able to advance</returns>
		public bool Advance(MediaPosition? amount = null);

		/// <summary>
		/// Flushes any data buffered by this media stream.
		/// </summary>
		public void Flush();

	}

}

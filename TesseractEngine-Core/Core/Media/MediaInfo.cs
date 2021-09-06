using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Audio;
using Tesseract.Core.Graphics;
using Tesseract.Core.Math;

namespace Tesseract.Core.Media {

	/// <summary>
	/// Media info describes details about how data is stored in a media format and accessed via media streams.
	/// </summary>
	public record MediaInfo {

		/// <summary>
		/// <para>
		/// The MIME type of the media format, or null if undefined.
		/// </para>
		/// </summary>
		public string MIMEType { get; init; } = null;

		/// <summary>
		/// <para>The framerate of video in frames per second supplied by this format, otherwise 0.</para>
		/// <para>
		/// Note: This is mostly a nominal value, as video formats may have variable framerates and the value
		/// that matters is the presentation timestamp on each decoded frame.
		/// </para>
		/// </summary>
		public double VideoFramerate { get; init; } = 0;

		/// <summary>
		/// The resolution of the uncompressed video frames in this format, otherwise the default value.
		/// </summary>
		public Vector2i VideoResolution { get; init; } = default;

		/// <summary>
		/// The pixel format of decompressed video frames that will be used by this media, otherwise null.
		/// </summary>
		public PixelFormat PixelFormat { get; init; } = null;

		/// <summary>
		/// The format of audio supplied by this format, otherwise null.
		/// </summary>
		public AudioFormat AudioFormat { get; init; } = null;

	}

}

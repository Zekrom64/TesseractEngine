using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Media {

	/// <summary>
	/// Enumeration of media position types.
	/// </summary>
	public enum MediaPositionType {
		/// <summary>
		/// The media position is specified by a timestamp.
		/// </summary>
		Timestamp,
		/// <summary>
		/// The media position is specified by a frame number.
		/// </summary>
		Frame
	}
	
	/// <summary>
	/// A media position determines what the current read/write position is
	/// in a piece of media.
	/// </summary>
	public readonly struct MediaPosition {

		/// <summary>
		/// The type of media position specified.
		/// </summary>
		public MediaPositionType Type { get; init; }

		/// <summary>
		/// A time-based media position, otherwise default.
		/// </summary>
		public TimeSpan Time { get; init; }

		/// <summary>
		/// A frame-based media position, otherwise default.
		/// </summary>
		public int Frame { get; init; }

		/// <summary>
		/// Creates a media position from a timestamp.
		/// </summary>
		/// <param name="offset">Time offset from the beginning of the media</param>
		/// <returns>Media position</returns>
		public static MediaPosition FromTime(TimeSpan offset) {
			return new MediaPosition() {
				Type = MediaPositionType.Timestamp,
				Time = offset
			};
		}

		/// <summary>
		/// Creates a media position from a frame number.
		/// </summary>
		/// <param name="frame">Frame number</param>
		/// <returns>Media position</returns>
		public static MediaPosition FromFrame(int frame) {
			return new MediaPosition() {
				Type = MediaPositionType.Frame,
				Frame = frame
			};
		}

	}

}

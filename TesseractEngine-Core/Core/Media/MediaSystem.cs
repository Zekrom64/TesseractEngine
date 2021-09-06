using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Media {
	
	/// <summary>
	/// A media system controlls encoding & decoding of multimedia files.
	/// </summary>
	public interface IMediaSystem {

		/// <summary>
		/// Tests if the media system can decode media stored in the given MIME type.
		/// </summary>
		/// <param name="mimeType">Media MIME type</param>
		/// <returns>If this media system can decode this type of media</returns>
		public bool CanDecode(string mimeType);

		//public bool CanEncode(string mimeType);
		
		/// <summary>
		/// Opens a stream in decoding mode using a standard IO stream as a source.
		/// </summary>
		/// <param name="source">Source stream</param>
		/// <param name="preferenceInfo">The preferred media info to use during decoding, or null to use the defaults</param>
		/// <returns>Media stream</returns>
		public IMediaStream Decode(Stream source, MediaInfo preferenceInfo = null);

		//public IMediaStream Encode(Stream destination, MediaInfo info);

	}

}

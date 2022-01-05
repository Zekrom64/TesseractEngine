using System.Collections.Generic;

namespace Tesseract.Core.Resource {

	/// <summary>
	/// Resource metadata provides additional information about a resource
	/// without inspecting its contents.
	/// </summary>
	public struct ResourceMetadata {

		/// <summary>
		/// The MIME type of the resource, or null if the MIME type could not be determined. This
		/// may be guessed from other properties such as file names via <see cref="MIME.TryGuessFromExtension(string, out string)"/>.
		/// </summary>
		public string? MIMEType { get; init; }

		/// <summary>
		/// The size of the resource, or -1 if the size cannot be determined.
		/// </summary>
		public long Size { get; init; }

		/// <summary>
		/// If the resource is local to the host system. Local resources are generally stored
		/// in the host's file system, either as a raw file or as part of a compressed archive.
		/// </summary>
		public bool Local { get; init; }

	}

	/// <summary>
	/// MIME types are strings that describe how the contents of a file are actually formatted. MIME
	/// types can be guessed from the extension at the end of a file name, but in some cases such
	/// as audio/video files the exact type can only be determined from the file contents itself.
	/// </summary>
	public static class MIME {

		/// <summary>
		/// Javascript code.
		/// </summary>
		public const string Javascript = "application/javascript";
		/// <summary>
		/// JSON data.
		/// </summary>
		public const string JSON = "application/json";
		/// <summary>
		/// PDF document.
		/// </summary>
		public const string PDF = "application/pdf";
		/// <summary>
		/// Zip archive.
		/// </summary>
		public const string Zip = "application/zip";

		/// <summary>
		/// MPEG4 audio.
		/// </summary>
		public const string MP4Audio = "audio/mp4";
		/// <summary>
		/// Ogg-Vorbis audio.
		/// </summary>
		public const string OGGAudio = "audio/ogg";

		/// <summary>
		/// TrueType font.
		/// </summary>
		public const string TTF = "font/ttf";

		/// <summary>
		/// Bitmap image.
		/// </summary>
		public const string BMP = "image/bmp";
		/// <summary>
		/// GIF image.
		/// </summary>
		public const string GIF = "image/gif";
		/// <summary>
		/// JPEG image.
		/// </summary>
		public const string JPEG = "image/jpeg";
		/// <summary>
		/// PNG image.
		/// </summary>
		public const string PNG = "image/png";

		/// <summary>
		/// HTTP plaintext.
		/// </summary>
		public const string HTTP = "message/http";

		/// <summary>
		/// Wavefront Material Template Library file.
		/// </summary>
		public const string MTL = "model/mtl";
		/// <summary>
		/// Wavefront 3D object file.
		/// </summary>
		public const string OBJ = "model/obj";
		/// <summary>
		/// STL model file.
		/// </summary>
		public const string STL = "model/stl";

		/// <summary>
		/// Cascading Style Sheet.
		/// </summary>
		public const string CSS = "test/css";
		/// <summary>
		/// Comma-separated values.
		/// </summary>
		public const string CSV = "text/csv";
		/// <summary>
		/// HTML document.
		/// </summary>
		public const string HTML = "text/html";
		/// <summary>
		/// Plain text.
		/// </summary>
		public const string PlainText = "text/plain";
		/// <summary>
		/// XML data.
		/// </summary>
		public const string XML = "text/xml";

		/// <summary>
		/// MPEG4 video.
		/// </summary>
		public const string MP4Video = "video/mp4";
		/// <summary>
		/// Ogg-Theora video.
		/// </summary>
		public const string OGGVideo = "video/ogg";

		// Dictionary of file extensions to the assumed MIME types
		private static readonly Dictionary<string, string> mimeExtensions = new() {
			{ "js", Javascript },
			{ "cjs", Javascript },
			{ "mjs", Javascript },
			{ "json", JSON },
			{ "pdf", PDF },
			{ "zip", Zip },
			{ "zipx", Zip },
			{ "mp4", MP4Video }, // MP4s are assumed to be video by default, m4a and m4v specify audio/video only
			{ "m4a", MP4Audio },
			{ "m4v", MP4Video },
			{ "ogg", OGGAudio }, // Oggs are assumed to be audio by default, oga and ogv specify audio/video only
			{ "oga", OGGAudio },
			{ "ogv", OGGVideo },
			{ "ttf", TTF },
			{ "tte", TTF },
			{ "dfont", TTF },
			{ "bmp", BMP },
			{ "dib", BMP },
			{ "gif", GIF },
			{ "jpg", JPEG },
			{ "jpeg", JPEG },
			{ "jpe", JPEG },
			{ "jif", JPEG },
			{ "jfif", JPEG },
			{ "jfi", JPEG },
			{ "png", PNG },
			{ "mtl", MTL },
			{ "obj", OBJ },
			{ "stl", STL },
			{ "css", CSS },
			{ "csv", CSV },
			{ "html", HTML },
			{ "htm", HTML },
			{ "txt", PlainText },
			{ "xml", XML }
		};

		/// <summary>
		/// Attempts to guess the MIME type of a resource based on the extension part of its name.
		/// </summary>
		/// <param name="extension">The extension part of the resource's name</param>
		/// <param name="mime">The MIME type if found</param>
		/// <returns>If a MIME type was found for the extension</returns>
		public static bool TryGuessFromExtension(string? extension, out string? mime) {
			if (extension == null) {
				mime = null;
				return false;
			}
			return mimeExtensions.TryGetValue(extension, out mime);
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Resource {

	public struct ResourceMetadata {

		public string MIMEType { get; init; }

		public long Size { get; init; }

		public bool Local { get; init; }

	}
	
	public static class MIME {

		public const string Javascript = "application/javascript";
		public const string JSON = "application/json";
		public const string PDF = "application/pdf";
		public const string Zip = "application/zip";

		public const string MP4Audio = "audio/mp4";
		public const string OGGAudio = "audio/ogg";

		public const string TTF = "font/ttf";

		public const string BMP = "image/bmp";
		public const string GIF = "image/gif";
		public const string JPEG = "image/jpeg";
		public const string PNG = "image/png";
		
		public const string HTTP = "message/http";

		public const string MTL = "model/mtl";
		public const string OBJ = "model/obj";
		public const string STL = "model/stl";

		public const string CSS = "test/css";
		public const string CSV = "text/csv";
		public const string HTML = "text/html";
		public const string PlainText = "text/plain";
		public const string XML = "text/xml";

		public const string MP4Video = "video/mp4";
		public const string OGGVideo = "video/ogg";

		private static readonly Dictionary<string, string> mimeExtensions = new() {
			{ "js", Javascript },
			{ "cjs", Javascript },
			{ "mjs", Javascript },
			{ "json", JSON },
			{ "pdf", PDF },
			{ "zip", Zip },
			{ "zipx", Zip },
			{ "mp4", MP4Video },
			{ "m4a", MP4Audio },
			{ "m4v", MP4Video },
			{ "ogg", OGGAudio },
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

		public static bool TryGuessFromExtension(string extension, out string mime) => mimeExtensions.TryGetValue(extension, out mime);

	}

}

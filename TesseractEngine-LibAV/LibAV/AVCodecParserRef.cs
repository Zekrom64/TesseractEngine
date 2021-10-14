using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class AVCodecParserRef : IDisposable {

		public ManagedPointer<AVCodecParser> CodecParser { get; }

		public AVCodecParserRef(ManagedPointer<AVCodecParser> codecParser) {
			CodecParser = codecParser;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			CodecParser.Dispose();
		}

		public AVCodecParserRef Next {
			get {
				IntPtr pNext = LibAVCodec.Functions.av_parser_next(CodecParser);
				return pNext != IntPtr.Zero ? new AVCodecParserRef(new(pNext)) : null;
			}
		}

		public void Register() => LibAVCodec.Functions.av_register_codec_parser(CodecParser);

	}

}

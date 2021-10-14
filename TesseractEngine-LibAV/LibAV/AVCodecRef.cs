using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class AVCodecRef : IDisposable {

		public ManagedPointer<AVCodec> Codec { get; }

		public bool IsEncoder => LibAVCodec.Functions.av_codec_is_encoder(Codec);

		public bool IsDecoder => LibAVCodec.Functions.av_codec_is_decoder(Codec);

		public AVCodecRef([NativeType("AVCodec*")] IntPtr codec) {
			Codec = new(codec);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Codec.Dispose();
		}

		public AVCodecRef Next {
			get {
				IntPtr pNext = LibAVCodec.Functions.av_codec_next(Codec);
				return pNext == IntPtr.Zero ? null : new AVCodecRef(pNext);
			}
		}

		public void Register() => LibAVCodec.Functions.avcodec_register(Codec);

		public string GetProfileName(int profile) => MemoryUtil.GetASCII(LibAVCodec.Functions.av_get_profile_name(Codec, profile));

	}

}

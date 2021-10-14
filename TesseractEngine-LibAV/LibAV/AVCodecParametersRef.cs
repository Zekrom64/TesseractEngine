using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class AVCodecParametersRef : IDisposable {

		public ManagedPointer<AVCodecParameters> CodecParameters { get; private set; }

		public AVCodecParametersRef(ManagedPointer<AVCodecParameters> ptr) {
			CodecParameters = ptr;
		}

		public AVCodecParametersRef() {
			CodecParameters = new(LibAVCodec.Functions.avcodec_parameters_alloc(), new(LibAVCodec.Functions.avcodec_parameters_free));
		}

		public void Copy(AVCodecParametersRef src) {
			AVError err = LibAVCodec.Functions.avcodec_parameters_copy(CodecParameters, src.CodecParameters);
			if (err != AVError.None) throw new AVException("Failed to copy codec parameters", err);
		}

		public void FromContext(AVCodecContextRef ctxt) {
			AVError err = LibAVCodec.Functions.avcodec_parameters_from_context(CodecParameters, ctxt.Context);
			if (err != AVError.None) throw new AVException("Failed to copy codec parameters from context", err);
		}

		public void ToContext(AVCodecContextRef ctxt) {
			AVError err = LibAVCodec.Functions.avcodec_parameters_to_context(ctxt.Context, CodecParameters);
			if (err != AVError.None) throw new AVException("Failed to copy codec parameters to context", err);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (CodecParameters) {
				CodecParameters.Dispose();
				CodecParameters = default;
			}
		}

		public int GetAudioFrameDuration(int frameBytes = 0) => LibAVCodec.Functions.av_get_audio_frame_duration2(CodecParameters, frameBytes);

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV.Native {
	
	public class LibAVResampleFunctions {

		public delegate uint PFN_avresample_version();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_avresample_configuration();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_avresample_license();

		public PFN_avresample_version avresample_version;
		public PFN_avresample_configuration avresample_configuration;
		public PFN_avresample_license avresample_license;

		[return: NativeType("const AVClass*")]
		public delegate IntPtr PFN_avresample_get_class();

		public PFN_avresample_get_class avresample_get_class;

		[return: NativeType("AVAudioResampleContext*")]
		public delegate IntPtr PFN_avresample_alloc_context();
		public delegate AVError PFN_avresample_open([NativeType("AVAudioResampleContext*")] IntPtr ctx);
		public delegate bool PFN_avresample_is_open([NativeType("AVAudioResampleContext*")] IntPtr ctx);
		public delegate void PFN_avresample_close([NativeType("AVAudioResampleContext*")] IntPtr ctx);
		public delegate void PFN_avresample_free([NativeType("AVAudioResampleContext**")] IntPtr ctx);

		public PFN_avresample_alloc_context avresample_alloc_context;
		public PFN_avresample_open avresample_open;
		public PFN_avresample_is_open avresample_is_open;
		public PFN_avresample_close avresample_close;
		public PFN_avresample_free avresample_free;

		public delegate AVError PFN_avresample_build_matrix(AVChannelMask inLayout, AVChannelMask outLayout, double centerMixLevel, double surroundMixLevel, double lfeMixLevel, bool normalize, [NativeType("double*")] IntPtr matrix, int stride, AVMatrixEncoding encoding);
		public delegate AVError PFN_avresample_get_matrix([NativeType("AVAudioResampleContext*")] IntPtr ctx, [NativeType("double*")] IntPtr matrix, int stride);
		public delegate AVError PFN_avresample_set_matrix([NativeType("AVAudioResampleContext*")] IntPtr ctx, [NativeType("const double*")] IntPtr matrix, int stride);
		public delegate AVError PFN_avresample_set_channel_mapping([NativeType("AVAudioResampleContext*")] IntPtr ctx, [NativeType("const int*")] IntPtr channelMap);
		public delegate AVError PFN_avresample_set_compensation([NativeType("AVAudioResampleContext*")] IntPtr ctx, int sampleDelta, int compensationDistance);
		public delegate int PFN_avresample_get_out_samples([NativeType("AVAudioResampleContext*")] IntPtr ctx, int nbSamples);

		public PFN_avresample_build_matrix avresample_build_matrix;
		public PFN_avresample_get_matrix avresample_get_matrix;
		public PFN_avresample_set_matrix avresample_set_matrix;
		public PFN_avresample_set_channel_mapping avresample_set_channel_mapping;
		public PFN_avresample_set_compensation avresample_set_compensation;
		public PFN_avresample_get_out_samples avresample_get_out_samples;

		public delegate int PFN_avresample_convert([NativeType("AVAudioResampleContext*")] IntPtr ctx, [NativeType("uint8_t**")] IntPtr output, int outPlaneSize, int outSamples, [NativeType("uint8_t* const*")] IntPtr input, int inPlaneSize, int inSamples);
		public delegate int PFN_avresample_get_delay([NativeType("AVAudioResampleContext*")] IntPtr ctx);
		public delegate int PFN_avresample_available([NativeType("AVAudioResampleContext*")] IntPtr ctx);
		public delegate int PFN_avresample_read([NativeType("AVAudioResampleContext*")] IntPtr ctx, [NativeType("uint8_t**")] IntPtr output, int nbSamples);
		public delegate AVError PFN_avresample_convert_frame([NativeType("AVAudioResampleContext*")] IntPtr ctx, [NativeType("AVFrame*")] IntPtr output, [NativeType("AVFrame*")] IntPtr input);
		public delegate AVError PFN_avresample_config([NativeType("AVAudioResampleContext*")] IntPtr ctx, [NativeType("AVFrame*")] IntPtr output, [NativeType("AVFrame*")] IntPtr input);

		public PFN_avresample_convert avresample_convert;
		public PFN_avresample_get_delay avresample_get_delay;
		public PFN_avresample_available avresample_available;
		public PFN_avresample_read avresample_read;
		public PFN_avresample_convert_frame avresample_convert_frame;
		public PFN_avresample_config avresample_config;

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.LibAV.Native {

	public class LibAVCodecFunctions {

		[return: NativeType("const AVCodecHWConfig*")]
		public delegate IntPtr PFN_avcodec_get_hw_config([NativeType("const AVCodec*")] IntPtr codec, int index);

		public PFN_avcodec_get_hw_config avcodec_get_hw_config;

		[return: NativeType("AVCodec*")]
		public delegate IntPtr PFN_av_codec_next([NativeType("const AVCodec*")] IntPtr c);

		public PFN_av_codec_next av_codec_next;

		public delegate uint PFN_avcodec_version();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_avcodec_configuration();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_avcodec_license();
		public delegate void PFN_avcodec_register([NativeType("AVCodec*")] IntPtr codec);
		public delegate void PFN_avcodec_register_all();
		[return: NativeType("AVCodecContext*")]
		public delegate IntPtr PFN_avcodec_alloc_context3([NativeType("const AVCodec*")] IntPtr codec);
		public delegate void PFN_avcodec_free_context([NativeType("AVCodecContext**")] IntPtr avctx);

		public PFN_avcodec_version avcodec_version;
		public PFN_avcodec_configuration avcodec_configuration;
		public PFN_avcodec_license avcodec_license;
		public PFN_avcodec_register avcodec_register;
		public PFN_avcodec_register_all avcodec_register_all;
		public PFN_avcodec_alloc_context3 avcodec_alloc_context3;
		public PFN_avcodec_free_context avcodec_free_context;

#if LIBAVCODEC_VERSION_58
		public delegate int PFN_avcodec_get_context_defaults3([NativeType("AVCodecContext*")] IntPtr s, [NativeType("const AVCodec*")] IntPtr codec);
		public PFN_avcodec_get_context_defaults3 avcodec_get_context_defaults3;
#endif

		[return: NativeType("const AVClass*")]
		public delegate IntPtr PFN_avcodec_get_class();
		public PFN_avcodec_get_class avcodec_get_class;

#if LIBAVCODEC_VERSION_58
		public delegate AVError PFN_avcodec_copy_context([NativeType("AVCodecContext*")] IntPtr dst, [NativeType("const AVCodecContext*")] IntPtr src);
		public PFN_avcodec_copy_context avcodec_copy_context;
#endif

		[return: NativeType("AVCodecParameters*")]
		public delegate IntPtr PFN_avcodec_parameters_alloc();
		public delegate void PFN_avcodec_parameters_free([NativeType("AVCodecParameters**")] IntPtr par);
		public delegate AVError PFN_avcodec_parameters_copy([NativeType("AVCodecParameters*")] IntPtr dst, [NativeType("const AVCodecParameters*")] IntPtr src);
		public delegate AVError PFN_avcodec_parameters_from_context([NativeType("AVCodecParameters*")] IntPtr par, [NativeType("const AVCodecContext*")] IntPtr codec);
		public delegate AVError PFN_avcodec_parameters_to_context([NativeType("AVCodecContext*")] IntPtr codec, [NativeType("const AVCodecParameters*")] IntPtr par);
		public delegate AVError PFN_avcodec_open2([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("const AVCodec*")] IntPtr codec, [NativeType("AVDictionary**")] IntPtr options);
		[Obsolete("The functionality provided by this function is no longer supported by libav")]
		public delegate int PFN_avcodec_close([NativeType("AVCodecContext*")] IntPtr avctx);
		public delegate void PFN_avsubtitle_free([NativeType("AVSubtitle*")] IntPtr sub);
		[return: NativeType("AVPacket*")]
		public delegate IntPtr PFN_av_packet_alloc();
		[return: NativeType("AVPacket*")]
		public delegate IntPtr PFN_av_packet_clone([NativeType("const AVPacket*")] IntPtr src);
		public delegate void PFN_av_packet_free([NativeType("AVPacket**")] IntPtr pkt);
		public delegate void PFN_av_init_packet([NativeType("AVPacket*")] IntPtr pkt);
		public delegate AVError PFN_av_new_packet([NativeType("AVPacket*")] IntPtr pkt, int size);
		public delegate void PFN_av_shrink_packet([NativeType("AVPacket*")] IntPtr pkt, int size);
		public delegate int PFN_av_grow_packet([NativeType("AVPacket*")] IntPtr pkt, int growBy);
		public delegate AVError PFN_av_packet_from_data([NativeType("AVPacket*")] IntPtr pkt, [NativeType("uint8_t*")] IntPtr data, int size);

		public PFN_avcodec_parameters_alloc avcodec_parameters_alloc;
		public PFN_avcodec_parameters_free avcodec_parameters_free;
		public PFN_avcodec_parameters_copy avcodec_parameters_copy;
		public PFN_avcodec_parameters_from_context avcodec_parameters_from_context;
		public PFN_avcodec_parameters_to_context avcodec_parameters_to_context;
		public PFN_avcodec_open2 avcodec_open2;
		[Obsolete("The functionality provided by this function is no longer supported by libav")]
		public PFN_avcodec_close avcodec_close;
		public PFN_avsubtitle_free avsubtitle_free;
		public PFN_av_packet_alloc av_packet_alloc;
		public PFN_av_packet_clone av_packet_clone;
		public PFN_av_packet_free av_packet_free;
		public PFN_av_init_packet av_init_packet;
		public PFN_av_new_packet av_new_packet;
		public PFN_av_shrink_packet av_shrink_packet;
		public PFN_av_grow_packet av_grow_packet;
		public PFN_av_packet_from_data av_packet_from_data;

#if LIBAVCODEC_VERSION_58
		[Obsolete("The functionality provided by this function is no longer supported by libav")]
		public delegate int PFN_av_dup_packet([NativeType("AVPacket*")] IntPtr pkt);
		[Obsolete("The functionality provided by this function is no longer supported by libav")]
		public delegate int PFN_av_free_packet([NativeType("AVPacket*")] IntPtr pkt);

		[Obsolete("The functionality provided by this function is no longer supported by libav")]
		public PFN_av_dup_packet av_dup_packet;
		[Obsolete("The functionality provided by this function is no longer supported by libav")]
		public PFN_av_free_packet av_free_packet;
#endif

		[return: NativeType("uint8_t*")]
		public delegate IntPtr PFN_av_packet_new_side_data([NativeType("AVPacket*")] IntPtr pkt, AVPacketSideDataType type, int size);
		public delegate int PFN_av_packet_add_side_data([NativeType("AVPacket*")] IntPtr pkt, AVPacketSideDataType type, [NativeType("uint8_t*")] IntPtr data, nuint size);
		public delegate AVError PFN_av_packet_shrink_side_data([NativeType("AVPacket*")] IntPtr pkt, AVPacketSideDataType type, int size);
		[return: NativeType("uint8_t*")]
		public delegate IntPtr PFN_av_packet_get_side_data([NativeType("AVPacket*")] IntPtr pkt, AVPacketSideDataType type, [NativeType("int*")] IntPtr size);
		public delegate void PFN_av_packet_free_side_data([NativeType("AVPacket*")] IntPtr pkt);
		public delegate AVError PFN_av_packet_ref([NativeType("AVPacket*")] IntPtr dst, [NativeType("const AVPacket*")] IntPtr src);
		public delegate void PFN_av_packet_unref([NativeType("AVPacket*")] IntPtr packet);
		public delegate void PFN_av_packet_move_ref([NativeType("AVPacket*")] IntPtr dst, [NativeType("AVPacket*")] IntPtr src);
		public delegate AVError PFN_av_packet_copy_props([NativeType("AVPacket*")] IntPtr dst, [NativeType("const AVPacket*")] IntPtr src);
		public delegate void PFN_av_packet_rescale_ts([NativeType("AVPacket*")] IntPtr pkt, AVRational tbSrc, AVRational tbDst);
		[return: NativeType("AVCodec*")]
		public delegate IntPtr PFN_avcodec_find_decoder(AVCodecID id);
		[return: NativeType("AVCodec*")]
		public delegate IntPtr PFN_avcodec_find_decoder_by_name([MarshalAs(UnmanagedType.LPStr)] string name);
		public delegate int PFN_avcodec_default_get_buffer2([NativeType("AVCodecContext*")] IntPtr s, [NativeType("AVFrame*")] IntPtr frame, int flags);
		public delegate void PFN_avcodec_align_dimensions([NativeType("AVCodecContext*")] IntPtr s, ref int width, ref int height);
		public delegate void PFN_avcodec_align_dimensions2([NativeType("AVCodecContext*")] IntPtr s, ref int width, ref int height, [NativeType("int[AV_NUM_DATA_POINTERS]")] IntPtr linesizeAlign);
		[Obsolete("Use avcodec_send_packet() and avcodec_receive_frame()")]
		public delegate int PFN_avcodec_decode_audio4([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVFrame*")] IntPtr frame, out int gotFramePtr, [NativeType("AVPacket*")] IntPtr avpkt);
		[Obsolete("Use avcodec_send_packet() and avcodec_receive_frame()")]
		public delegate int PFN_avcodec_decode_video2([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVFrame*")] IntPtr picture, out int gotPicturePtr, [NativeType("AVPacket*")] IntPtr avpkt);
		[Obsolete("Use avcodec_send_packet() and avcodec_receive_frame()")]
		public delegate int PFN_avcodec_decode_subtitle2([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVSubtitle*")] IntPtr sub, out int getSubPtr, [NativeType("AVPacket*")] IntPtr avpkt);
		public delegate AVError PFN_avcodec_send_packet([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("const AVPacket*")] IntPtr avpkt);
		public delegate AVError PFN_avcodec_receive_frame([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVFrame*")] IntPtr frame);
		public delegate AVError PFN_avcodec_send_frame([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("const AVFrame*")] IntPtr frame);
		public delegate AVError PFN_avcodec_receive_packet([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVPacket*")] IntPtr avpkt);
		public delegate AVError PFN_avcodec_get_hw_frames_parameters([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVBufferRef*")] IntPtr deviceRef, AVPixelFormat hwPixFmt, [NativeType("AVBufferRef**")] IntPtr outFramesRef);

		public PFN_av_packet_new_side_data av_packet_new_side_data;
		public PFN_av_packet_add_side_data av_packet_add_side_data;
		public PFN_av_packet_shrink_side_data av_packet_shrink_side_data;
		public PFN_av_packet_get_side_data av_packet_get_side_data;
		public PFN_av_packet_free_side_data av_packet_free_side_data;
		public PFN_av_packet_ref av_packet_ref;
		public PFN_av_packet_unref av_packet_unref;
		public PFN_av_packet_move_ref av_packet_move_ref;
		public PFN_av_packet_copy_props av_packet_copy_props;
		public PFN_av_packet_rescale_ts av_packet_rescale_ts;
		public PFN_avcodec_find_decoder avcodec_find_decoder;
		public PFN_avcodec_find_decoder_by_name avcodec_find_decoder_by_name;
		public PFN_avcodec_default_get_buffer2 avcodec_default_get_buffer2;
		public PFN_avcodec_align_dimensions avcodec_align_dimensions;
		public PFN_avcodec_align_dimensions2 avcodec_align_dimensions2;
		[Obsolete("Use avcodec_send_packet() and avcodec_receive_frame()")]
		public PFN_avcodec_decode_audio4 avcodec_decode_audio4;
		[Obsolete("Use avcodec_send_packet() and avcodec_receive_frame()")]
		public PFN_avcodec_decode_video2 avcodec_decode_video2;
		[Obsolete("Use avcodec_send_packet() and avcodec_receive_frame()")]
		public PFN_avcodec_decode_subtitle2 avcodec_decode_subtitle2;
		public PFN_avcodec_send_packet avcodec_send_packet;
		public PFN_avcodec_receive_frame avcodec_receive_frame;
		public PFN_avcodec_send_frame avcodec_send_frame;
		public PFN_avcodec_receive_packet avcodec_receive_packet;
		public PFN_avcodec_get_hw_frames_parameters avcodec_get_hw_frames_parameters;

		[return: NativeType("AVCodecParser*")]
		public delegate IntPtr PFN_av_parser_next([NativeType("const AVCodecParser*")] IntPtr c);
		public delegate void PFN_av_register_codec_parser([NativeType("AVCodecParser*")] IntPtr parser);
		[return: NativeType("AVCodecParserContext*")]
		public delegate IntPtr PFN_av_parser_init(int codecID);
		public delegate int PFN_av_parser_parse2([NativeType("AVCodecParserContext*")] IntPtr s, [NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("uint8_t**")] IntPtr pOutBuf, [NativeType("int*")] IntPtr outBufSize, [NativeType("const uint8_t*")] IntPtr buf, int bufSize, long pts, long dts, long pos);
		[Obsolete("Use AVBitstreamFilter")]
		public delegate bool PFN_av_parser_change([NativeType("AVCodecParserContext*")] IntPtr s, [NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("uint8_t**")] IntPtr pOutBuf, [NativeType("int*")] IntPtr pOutBufSize, [NativeType("const uint8_t*")] IntPtr buf, int bufSize, int keyframe);
		public delegate void PFN_av_parser_close([NativeType("AVCodecParserContext*")] IntPtr s);
		[return: NativeType("AVCodec*")]
		public delegate IntPtr PFN_avcodec_find_encoder(AVCodecID id);
		[return: NativeType("AVCodec*")]
		public delegate IntPtr PFN_avcodec_find_encoder_by_name([MarshalAs(UnmanagedType.LPStr)] string name);
		[Obsolete("Use avcodec_send_frame()/avcodec_receive_packet() instead")]
		public delegate int PFN_avcodec_encode_audio2([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVPacket*")] IntPtr avpkt, [NativeType("const AVFrame*")] IntPtr frame, out int gotPacketPtr);
		[Obsolete("Use avcodec_send_frame()/avcodec_receive_packet() instead")]
		public delegate int PFN_avcodec_encode_video2([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVPacket*")] IntPtr avpkt, [NativeType("const AVFrame*")] IntPtr frame, out int gotPacketPtr);
		public delegate int PFN_avcodec_encode_subtitle([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("uint8_t*")] IntPtr buf, int bufSize, [NativeType("const AVSubtitle*")] IntPtr sub);

		public PFN_av_parser_next av_parser_next;
		public PFN_av_register_codec_parser av_register_codec_parser;
		public PFN_av_parser_init av_parser_init;
		public PFN_av_parser_parse2 av_parser_parse2;
		[Obsolete("Use AVBitstreamFilter")]
		public PFN_av_parser_change av_parser_change;
		public PFN_av_parser_close av_parser_close;
		public PFN_avcodec_find_encoder avcodec_find_encoder;
		public PFN_avcodec_find_encoder_by_name avcodec_find_encoder_by_name;
		[Obsolete("Use avcodec_send_frame()/avcodec_receive_packet() instead")]
		public PFN_avcodec_encode_audio2 avcodec_encode_audio2;
		[Obsolete("Use avcodec_send_frame()/avcodec_receive_packet() instead")]
		public PFN_avcodec_encode_video2 avcodec_encode_video2;
		public PFN_avcodec_encode_subtitle avcodec_encode_subtitle;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate int PFN_avpicture_alloc([NativeType("AVPicture*")] IntPtr picture, AVPixelFormat pixFmt, int width, int height);
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate void PFN_avpicture_free([NativeType("AVPicture*")] IntPtr picture);
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate int PFN_avpicture_fill([NativeType("AVPicture*")] IntPtr picture, [NativeType("uint8_t*")] IntPtr ptr, AVPixelFormat pixFmt, int width, int height);
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate int PFN_avpicture_layout([NativeType("AVPicture*")] IntPtr picture, AVPixelFormat pixFmt, int width, int height, [NativeType("unsigned char*")] IntPtr dest, int destSize);
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate int PFN_avpicture_get_size(AVPixelFormat pixFmt, int width, int height);
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate void PFN_av_picture_copy([NativeType("AVPicture*")] IntPtr dst, [NativeType("const AVPicture*")] IntPtr src, AVPixelFormat pixFmt, int width, int height);
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate int PFN_av_picture_crop([NativeType("AVPicture*")] IntPtr dst, [NativeType("const AVPicture*")] IntPtr src, AVPixelFormat pixFmt, int topBand, int leftBand);
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate int PFN_av_picture_pad([NativeType("AVPicture*")] IntPtr dst, [NativeType("const AVPicture*")] IntPtr src, int height, int width, AVPixelFormat format, int padtop, int padbottom, int padleft, int padright, [NativeType("int*")] IntPtr color);

		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_avpicture_alloc avpicture_alloc;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_avpicture_free avpicture_free;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_avpicture_fill avpicture_fill;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_avpicture_layout avpicture_layout;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_avpicture_get_size avpicture_get_size;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_av_picture_copy avpicture_copy;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_av_picture_crop avpicture_crop;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_av_picture_pad avpicture_pad;
#endif

		public delegate uint PFN_avcodec_pix_fmt_to_codec_tag(AVPixelFormat format);
		public delegate AVLossFlags PFN_avcodec_get_pix_fmt_loss(AVPixelFormat dstFormat, AVPixelFormat srcFormat, bool hasAlpha);
		public delegate AVPixelFormat PFN_avcodec_find_best_pix_fmt2([NativeType("AVPixelFormat*")] IntPtr pixelFormatList, AVPixelFormat srcPixelFormat, bool hasAlpha, out AVLossFlags loss);
		public delegate AVPixelFormat PFN_avcodec_default_get_format([NativeType("AVCodecContext*")] IntPtr s, [NativeType("const AVPixelFormat")] IntPtr format);

		public PFN_avcodec_pix_fmt_to_codec_tag avcodec_pix_fmt_to_codec_tag;
		public PFN_avcodec_get_pix_fmt_loss avcodec_get_pix_fmt_loss;
		public PFN_avcodec_find_best_pix_fmt2 avcodec_find_best_pix_fmt2;
		public PFN_avcodec_default_get_format avcodec_default_get_format;

		public delegate nuint PFN_av_get_codec_tag_string([NativeType("char*")] IntPtr buf, nuint bufSize, uint codecTag);
		public delegate void PFN_avcodec_string([NativeType("char*")] IntPtr buf, int bufSize, [NativeType("AVCodecContext*")] IntPtr enc, bool encode);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_av_get_profile_name([NativeType("AVCodec*")] IntPtr codec, int profile);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_avcodec_profile_name(AVCodecID codec, int profile);

		public PFN_av_get_codec_tag_string av_get_codec_tag_string;
		public PFN_avcodec_string avcodec_string;
		public PFN_av_get_profile_name av_get_profile_name;
		public PFN_avcodec_profile_name avcodec_profile_name;

		public delegate int PFN_avcodec_default_execute([NativeType("AVCodecContext*")] IntPtr c, [MarshalAs(UnmanagedType.FunctionPtr)] AVDefaultExecuteFunc func, IntPtr arg, out int ret, int count, int size);
		public delegate int PFN_avcodec_default_execute2([NativeType("AVCodecContext*")] IntPtr c, [MarshalAs(UnmanagedType.FunctionPtr)] AVDefaultExecuteFunc2 func, IntPtr arg, out int ret, int count);

		public PFN_avcodec_default_execute avcodec_default_execute;
		public PFN_avcodec_default_execute2 avcodec_default_execute2;

		public delegate int PFN_avcodec_fill_audio_frame([NativeType("AVFrame*")] IntPtr frame, int nbChannels, AVSampleFormat sampleFormat, [NativeType("const uint8_t*")] IntPtr buf, int bufSize, int align);
		public delegate void PFN_avcodec_flush_buffers([NativeType("AVCodecContext*")] IntPtr c);

		public PFN_avcodec_fill_audio_frame avcodec_fill_audio_frame;
		public PFN_avcodec_flush_buffers avcodec_flush_buffers;

		public delegate int PFN_av_get_bits_per_sample(AVCodecID codecID);
		public delegate int PFN_av_get_exact_bits_per_sample(AVCodecID codecID);
		public delegate int PFN_av_get_audio_frame_duration([NativeType("AVCodecContext*")] IntPtr c, int frameBytes);
		public delegate int PFN_av_get_audio_frame_duration2([NativeType("AVCodecParameters*")] IntPtr par, int frameBytes);

		public PFN_av_get_bits_per_sample av_get_bits_per_sample;
		public PFN_av_get_exact_bits_per_sample av_get_exact_bits_per_sample;
		public PFN_av_get_audio_frame_duration av_get_audio_frame_duration;
		public PFN_av_get_audio_frame_duration2 av_get_audio_frame_duration2;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate void PFN_av_register_bitstream_filter([NativeType("AVBitStreamFilter*")] IntPtr filter);
		[Obsolete("Deprecated in libavcodec > 58")]
		[return: NativeType("AVBitStreamFilterContext*")]
		public delegate IntPtr PFN_av_bitstream_filter_init([MarshalAs(UnmanagedType.LPStr)] string name);
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate int PFN_av_bitstream_filter_filter([NativeType("AVBitStreamFilterContext*")] IntPtr bsfc, [NativeType("AVCodecContext*")] IntPtr ctx, [MarshalAs(UnmanagedType.LPStr)] string args, [NativeType("uint8_t**")] out IntPtr outBuf, out int outbufSize, [NativeType("const uint8_t*")] IntPtr buf, int bufSize, bool keyframe);
		[Obsolete("Deprecated in libavcodec > 58")]
		public delegate void PFN_av_bitstream_filter_close([NativeType("AVBitStreamFilterContext*")] IntPtr bsfc);
		[Obsolete("Deprecated in libavcodec > 58")]
		[return: NativeType("AVBitStreamFilter*")]
		public delegate IntPtr PFN_av_bitstream_filter_next([NativeType("AVBitStreamFilter*")] IntPtr filter);

		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_av_register_bitstream_filter av_register_bitstream_filter;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_av_bitstream_filter_init av_bitstream_filter_init;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_av_bitstream_filter_filter av_bitstream_filter_filter;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_av_bitstream_filter_close av_bitstream_filter_close;
		[Obsolete("Deprecated in libavcodec > 58")]
		public PFN_av_bitstream_filter_next av_bitstream_filter_next;
#endif

		[return: NativeType("const AVBitStreamFilter*")]
		public delegate IntPtr PFN_av_bsf_get_by_name([MarshalAs(UnmanagedType.LPStr)] string name);
		[return: NativeType("const AVBitStreamFilter*")]
		public delegate IntPtr PFN_av_bsf_next([NativeType("void**")] IntPtr opaque);
		public delegate int PFN_av_bsf_alloc([NativeType("AVBitStreamFilter*")] IntPtr filter, [NativeType("AVBSFContext**")] out IntPtr context);
		public delegate int PFN_av_bsf_init([NativeType("AVBSFContext*")] IntPtr bsfc);
		public delegate AVError PFN_av_bsf_send_packet([NativeType("AVBSFContext*")] IntPtr bsfc, [NativeType("AVPacket*")] IntPtr packet);
		public delegate AVError PFN_av_bsf_receive_packet([NativeType("AVBSFContext*")] IntPtr bsfc, [NativeType("AVPacket*")] IntPtr packet);
		public delegate void PFN_av_bsf_flush([NativeType("AVBSFContext*")] IntPtr bsfc);
		public delegate void PFN_av_bsf_free([NativeType("AVBSFContext*")] IntPtr bsfc);
		[return: NativeType("AVClass*")]
		public delegate IntPtr PFN_av_bsf_get_class();

		public PFN_av_bsf_get_by_name av_bsf_get_by_name;
		public PFN_av_bsf_next av_bsf_next;
		public PFN_av_bsf_alloc av_bsf_alloc;
		public PFN_av_bsf_init av_bsf_init;
		public PFN_av_bsf_send_packet av_bsf_send_packet;
		public PFN_av_bsf_receive_packet av_bsf_receive_packet;
		public PFN_av_bsf_flush av_bsf_flush;
		public PFN_av_bsf_free av_bsf_free;
		public PFN_av_bsf_get_class av_bsf_get_class;

		public delegate void PFN_av_fast_padded_malloc(IntPtr ptr, out uint size, nuint minSize);
		public delegate uint PFN_av_xiphlacing([NativeType("unsigned char*")] IntPtr s, uint v);

		public PFN_av_fast_padded_malloc av_fast_padded_malloc;
		public PFN_av_xiphlacing av_xiphlacing;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec > 59")]
		public delegate void PFN_av_register_hwaccel([NativeType("AVHWAccel*")] IntPtr hwaccel);
		[Obsolete("Deprecated in libavcodec > 59")]
		[return: NativeType("AVHWAccel*")]
		public delegate IntPtr PFN_av_hwaccel_next([NativeType("const AVHWAccel*")] IntPtr hwaccel);

		[Obsolete("Deprecated in libavcodec > 59")]
		public PFN_av_register_hwaccel av_register_hwaccel;
		[Obsolete("Deprecated in libavcodec > 59")]
		public PFN_av_hwaccel_next av_hwaccel_next;
#endif

		public delegate int PFN_av_lockmgr_register([MarshalAs(UnmanagedType.FunctionPtr)] AVLockMgrCB cb);
		public PFN_av_lockmgr_register av_lockmgr_register;

		public delegate AVMediaType PFN_avcodec_get_type(AVCodecID codecID);
		public delegate bool PFN_avcodec_is_open([NativeType("AVCodecContext*")] IntPtr ctx);
		public delegate bool PFN_av_codec_is_encoder([NativeType("AVCodec*")] IntPtr codec);
		public delegate bool PFN_av_codec_is_decoder([NativeType("AVCodec*")] IntPtr codec);
		[return: NativeType("const AVCodecDescriptor*")]
		public delegate IntPtr PFN_avcodec_descriptor_get(AVCodecID codecID);
		[return: NativeType("const AVCodecDescriptor*")]
		public delegate IntPtr PFN_avcodec_descriptor_next([NativeType("const AVCodecDescriptor*")] IntPtr prev);
		[return: NativeType("const AVCodecDescriptor*")]
		public delegate IntPtr PFN_avcodec_descriptor_get_by_name([MarshalAs(UnmanagedType.LPStr)] string name);
		[return: NativeType("AVCPBProperties*")]
		public delegate IntPtr PFN_av_cpb_properties_alloc([NativeType("size_t*")] IntPtr size);

		public PFN_avcodec_get_type avcodec_get_type;
		public PFN_avcodec_is_open avcodec_is_open;
		public PFN_av_codec_is_encoder av_codec_is_encoder;
		public PFN_av_codec_is_decoder av_codec_is_decoder;
		public PFN_avcodec_descriptor_get avcodec_descriptor_get;
		public PFN_avcodec_descriptor_next avcodec_descriptor_next;
		public PFN_avcodec_descriptor_get_by_name avcodec_descriptor_get_by_name;
		public PFN_av_cpb_properties_alloc av_cpb_properties_alloc;

	}

}

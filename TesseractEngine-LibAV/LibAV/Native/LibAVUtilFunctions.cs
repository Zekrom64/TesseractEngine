using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV.Native {

	public class LibAVUtilFunctions {

		// samplefmt.h

		[return: NativeType("const char*")]
		public delegate IntPtr PFN_av_get_sample_fmt_name(AVSampleFormat sampleFormat);
		public delegate AVSampleFormat PFN_av_get_sample_fmt([MarshalAs(UnmanagedType.LPStr)] string name);
		public delegate AVSampleFormat PFN_av_get_packed_sample_fmt(AVSampleFormat sampleFormat);
		public delegate AVSampleFormat PFN_av_get_planar_sample_fmt(AVSampleFormat sampleFormat);
		[return: NativeType("char*")]
		public delegate IntPtr PFN_av_get_sample_fmt_string([NativeType("char*")] IntPtr buf, int bufSize, AVSampleFormat sampleFormat);
		public delegate int PFN_av_get_bytes_per_sample(AVSampleFormat sampleFormat);
		public delegate bool PFN_av_sample_fmt_is_planar(AVSampleFormat sampleFormat);
		public delegate int PFN_av_samples_get_buffer_size(out int linesize, int nbChannels, int nbSamples, AVSampleFormat sampleFormat, int align);
		public delegate int PFN_av_samples_fill_arrays([NativeType("uint8_t**")] out IntPtr audioData, out int linesize, [NativeType("const uint8_t*")] IntPtr buf, int nbChannels, int nbSamples, AVSampleFormat sampleFormat, int align);
		public delegate int PFN_av_samples_alloc([NativeType("uint8_t**")] out IntPtr audioData, out int linesize, int nbChannels, int nbSamples, AVSampleFormat sampleFormat, int align);
		public delegate int PFN_av_samples_copy([NativeType("uint8_t**")] out IntPtr dst, [NativeType("uint8_t* const*")] IntPtr src, int dstOffset, int srcOffset, int nbSamples, int nbChannels, AVSampleFormat sampleFormat);
		public delegate int PFN_av_samples_set_silence([NativeType("uint8_t**")] IntPtr audioData, int offset, int nbSamples, int nbChannels, AVSampleFormat sampleFormat);

		public PFN_av_get_sample_fmt_name av_get_sample_fmt_name;
		public PFN_av_get_sample_fmt av_get_sample_fmt;
		public PFN_av_get_packed_sample_fmt av_get_packed_sample_fmt;
		public PFN_av_get_planar_sample_fmt av_get_planar_sample_fmt;
		public PFN_av_get_sample_fmt_string av_get_sample_fmt_string;
		public PFN_av_get_bytes_per_sample av_get_bytes_per_sample;
		public PFN_av_sample_fmt_is_planar av_sample_fmt_is_planar;
		public PFN_av_samples_get_buffer_size av_samples_get_buffer_size;
		public PFN_av_samples_fill_arrays av_samples_fill_arrays;
		public PFN_av_samples_alloc av_samples_alloc;
		public PFN_av_samples_copy av_samples_copy;
		public PFN_av_samples_set_silence av_samples_set_silence;

		// attributes.h

		// avutil.h

		public delegate uint PFN_avutil_version();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_av_version_info();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_avutil_configuration();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_avutil_license();
		public delegate byte PFN_av_get_picture_type_char(AVPictureType pictureType);
		public delegate AVRational PFN_av_get_time_base_q();

		public PFN_avutil_version avutil_version;
		public PFN_av_version_info av_version_info;
		public PFN_avutil_configuration avutil_configuration;
		public PFN_avutil_license avutil_license;
		public PFN_av_get_picture_type_char av_get_picture_type_char;
		public PFN_av_get_time_base_q av_get_time_base_q;

		// error.h

		public delegate int PFN_av_strerror(int errnum, [NativeType("char*")] IntPtr errbuf, nuint errbufSize);

		public PFN_av_strerror av_strerror;

		// rational.h

		public delegate AVRational PFN_av_reduce(out int dstNum, out int dstDen, long num, long den, long max);
		public delegate AVRational PFN_av_mul_q(AVRational b, AVRational c);
		public delegate AVRational PFN_av_div_q(AVRational b, AVRational c);
		public delegate AVRational PFN_av_add_q(AVRational b, AVRational c);
		public delegate AVRational PFN_av_sub_q(AVRational b, AVRational c);
		public delegate AVRational PFN_av_d2q(double d, int max);
		public delegate int PFN_av_nearer_q(AVRational q, AVRational q1, AVRational q2);
		public delegate int PFN_av_find_nearest_q_idx(AVRational q, [NativeType("const AVRational*")] IntPtr qList);

		public PFN_av_reduce av_reduce;
		public PFN_av_mul_q av_mul_q;
		public PFN_av_div_q av_div_q;
		public PFN_av_add_q av_add_q;
		public PFN_av_sub_q av_sub_q;
		public PFN_av_d2q av_d2q;
		public PFN_av_nearer_q av_nearer_q;
		public PFN_av_find_nearest_q_idx av_find_nearest_q_idx;

		// buffer.h

		[return: NativeType("AVBufferRef*")]
		public delegate IntPtr PFN_av_buffer_alloc(int size);
		[return: NativeType("AVBufferRef*")]
		public delegate IntPtr PFN_av_buffer_allocz(int size);
		[return: NativeType("AVBufferRef*")]
		public delegate IntPtr PFN_av_buffer_create([NativeType("uint8_t*")] IntPtr data, int size, [MarshalAs(UnmanagedType.FunctionPtr)] AVFree free, IntPtr opaque, int flags);
		public delegate void PFN_av_buffer_default_free(IntPtr opaque, [NativeType("uint8_t*")] IntPtr data);
		[return: NativeType("AVBufferRef*")]
		public delegate IntPtr PFN_av_buffer_ref([NativeType("AVBufferRef*")] IntPtr buf);
		public delegate void PFN_av_buffer_unref([NativeType("AVBufferRef**")] IntPtr buf);
		public delegate bool PFN_av_buffer_is_writable([NativeType("AVBufferRef*")] IntPtr buf);
		public delegate AVError PFN_av_buffer_make_writable([NativeType("AVBufferRef**")] IntPtr buf);
		public delegate AVError PFN_av_buffer_realloc([NativeType("AVBufferRef**")] IntPtr buf, int size);
		[return: NativeType("AVBufferPool*")]
		public delegate IntPtr PFN_av_buffer_pool_init(int size, [MarshalAs(UnmanagedType.FunctionPtr)] AVBufferAlloc alloc);
		[return: NativeType("AVBufferPool*")]
		public delegate IntPtr PFN_av_buffer_pool_init2(int size, IntPtr opaque, [MarshalAs(UnmanagedType.FunctionPtr)] AVBufferAlloc2 alloc, [MarshalAs(UnmanagedType.FunctionPtr)] AVPoolFree poolFree);
		public delegate void PFN_av_buffer_pool_uninit([NativeType("AVBufferPool**")] IntPtr pool);
		public delegate IntPtr PFN_av_buffer_pool_get([NativeType("AVBufferPool*")] IntPtr pool);

		public PFN_av_buffer_alloc av_buffer_alloc;
		public PFN_av_buffer_allocz av_buffer_allocz;
		public PFN_av_buffer_create av_buffer_create;
		public PFN_av_buffer_default_free av_buffer_default_free;
		public PFN_av_buffer_ref av_buffer_ref;
		public PFN_av_buffer_unref av_buffer_unref;
		public PFN_av_buffer_is_writable av_buffer_is_writable;
		public PFN_av_buffer_make_writable av_buffer_make_writable;
		public PFN_av_buffer_realloc av_buffer_realloc;
		public PFN_av_buffer_pool_init av_buffer_pool_init;
		public PFN_av_buffer_pool_init2 av_buffer_pool_init2;
		public PFN_av_buffer_pool_uninit av_buffer_pool_uninit;
		public PFN_av_buffer_pool_get av_buffer_pool_get;

		// cpu.h

		public delegate AVCPUFlags PFN_av_get_cpu_flags();
		public delegate void PFN_av_set_cpu_flags_mask(AVCPUFlags mask);
		public delegate AVCPUFlags PFN_av_parse_cpu_flags([MarshalAs(UnmanagedType.LPStr)] string s);
		public delegate int PFN_av_cpu_count();
		public delegate nuint PFN_av_cpu_max_align();

		public PFN_av_get_cpu_flags av_get_cpu_flags;
		public PFN_av_set_cpu_flags_mask av_set_cpu_flags_mask;
		public PFN_av_parse_cpu_flags av_parse_cpu_flags;
		public PFN_av_cpu_count av_cpu_count;
		public PFN_av_cpu_max_align av_cpu_max_align;

		// dict.h

		[return: NativeType("AVDictionaryEntry*")]
		public delegate IntPtr PFN_av_dict_get([NativeType("const AVDictionary*")] IntPtr m, [MarshalAs(UnmanagedType.LPStr)] string key, [NativeType("const AVDictionaryEntry*")] IntPtr prev, AVDictionaryFlags flags);
		public delegate int PFN_av_dict_count([NativeType("const AVDictionary*")] IntPtr m);
		public delegate AVError PFN_av_dict_set([NativeType("AVDictionary**")] IntPtr pm, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value, AVDictionaryFlags flags);
		public delegate AVError PFN_av_dict_parse_string([NativeType("AVDictionary**")] IntPtr pm, [MarshalAs(UnmanagedType.LPStr)] string str, [MarshalAs(UnmanagedType.LPStr)] string keyValSep, [MarshalAs(UnmanagedType.LPStr)] string pairsSep, AVDictionaryFlags flags);
		public delegate AVError PFN_av_dict_copy([NativeType("AVDictionary**")] IntPtr dst, [NativeType("const AVDictionary*")] IntPtr src, AVDictionaryFlags flags);
		public delegate void PFN_av_dict_free([NativeType("AVDictionary**")] IntPtr m);

		public PFN_av_dict_get av_dict_get;
		public PFN_av_dict_count av_dict_count;
		public PFN_av_dict_set av_dict_set;
		public PFN_av_dict_parse_string av_dict_parse_string;
		public PFN_av_dict_copy av_dict_copy;
		public PFN_av_dict_free av_dict_free;

		// frame.h

		[return: NativeType("AVFrame*")]
		public delegate IntPtr PFN_av_frame_alloc();
		public delegate void PFN_av_frame_free([NativeType("AVFrame**")] IntPtr frame);
		public delegate AVError PFN_av_frame_ref([NativeType("AVFrame*")] IntPtr dst, [NativeType("const AVFrame*")] IntPtr src);
		[return: NativeType("AVFrame*")]
		public delegate IntPtr PFN_av_frame_clone([NativeType("const AVFrame*")] IntPtr src);
		public delegate void PFN_av_frame_unref([NativeType("AVFrame*")] IntPtr frame);
		public delegate void PFN_av_frame_move_ref([NativeType("AVFrame*")] IntPtr dst, [NativeType("AVFrame*")] IntPtr src);
		public delegate AVError PFN_av_frame_get_buffer([NativeType("AVFrame*")] IntPtr frame, int align);
		public delegate bool PFN_av_frame_is_writable([NativeType("AVFrame*")] IntPtr frame);
		public delegate AVError PFN_av_frame_make_writable([NativeType("AVFrame*")] IntPtr frame);
		public delegate int PFN_av_frame_copy([NativeType("AVFrame*")] IntPtr dst, [NativeType("const AVFrame*")] IntPtr src);
		public delegate int PFN_av_frame_copy_props([NativeType("AVFrame*")] IntPtr dst, [NativeType("const AVFrame*")] IntPtr src);
		[return: NativeType("AVBufferRef*")]
		public delegate IntPtr PFN_av_frame_get_plane_buffer([NativeType("AVFrame*")] IntPtr frame, int plane);
		[return: NativeType("AVFrameSideData*")]
		public delegate IntPtr PFN_av_frame_new_side_data([NativeType("AVFrame*")] IntPtr frame, AVFrameSideDataType type, int size);
		[return: NativeType("AVFrameSideData*")]
		public delegate IntPtr PFN_av_frame_get_side_data([NativeType("AVFrame*")] IntPtr frame, AVFrameSideDataType type);
		public delegate void PFN_av_frame_remove_side_data([NativeType("AVFrame*")] IntPtr frame, AVFrameSideDataType type);
		public delegate int PFN_av_frame_apply_cropping([NativeType("AVFrame*")] IntPtr frame, AVFrameCropFlags flags);

		public PFN_av_frame_alloc av_frame_alloc;
		public PFN_av_frame_free av_frame_free;
		public PFN_av_frame_ref av_frame_ref;
		public PFN_av_frame_clone av_frame_clone;
		public PFN_av_frame_unref av_frame_unref;
		public PFN_av_frame_move_ref av_frame_move_ref;
		public PFN_av_frame_get_buffer av_frame_get_buffer;
		public PFN_av_frame_is_writable av_frame_is_writable;
		public PFN_av_frame_make_writable av_frame_make_writable;
		public PFN_av_frame_copy av_frame_copy;
		public PFN_av_frame_copy_props av_frame_copy_props;
		public PFN_av_frame_get_plane_buffer av_frame_get_plane_buffer;
		public PFN_av_frame_new_side_data av_frame_new_side_data;
		public PFN_av_frame_get_side_data av_frame_get_side_data;
		public PFN_av_frame_remove_side_data av_frame_remove_side_data;
		public PFN_av_frame_apply_cropping av_frame_apply_cropping;

		// log.h

		// Missing log functions because varargs are hard
		// void av_log(void* avcl, int level, const char* fmt, ...)
		// void av_vlog(void* avcl, int level, const char* fmt, va_list vl)

		public delegate AVLogLevel PFN_av_log_get_level();
		public delegate void PFN_av_log_set_level(AVLogLevel level);

		// Y'know libav, varargs work a bit differently outside of the C/C++ world...
		// void av_log_set_callback(void (*callback)(void*, int, const char*, va_list))
		// void av_log_default_callback(void* avcl, int level, const char* fmt, va_list vl))

		[return: NativeType("const char*")]
		public delegate IntPtr PFN_av_default_item_name(IntPtr ctx);
		public delegate void PFN_av_log_set_flags(AVLogFlags arg);

		public PFN_av_log_get_level av_log_get_level;
		public PFN_av_log_set_level av_log_set_level;
		public PFN_av_default_item_name av_default_item_name;
		public PFN_av_log_set_flags av_log_set_flags;

		// hwcontext.h

		public delegate AVHWDeviceType PFN_av_hwdevice_find_type_by_name([MarshalAs(UnmanagedType.LPStr)] string name);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_av_hwdevice_get_type_name(AVHWDeviceType type);
		public delegate AVHWDeviceType PFN_av_hwdevice_iterate_types(AVHWDeviceType prev);
		[return: NativeType("AVBufferRef*")]
		public delegate IntPtr PFN_av_hwdevice_ctx_alloc(AVHWDeviceType type);
		public delegate AVError PFN_av_hwdevice_ctx_init([NativeType("AVBufferRef*")] IntPtr _ref);
		public delegate AVError PFN_av_hwdevice_ctx_create([NativeType("AVBufferRef**")] IntPtr deviceCtx, AVHWDeviceType type, [MarshalAs(UnmanagedType.LPStr)] string device, [NativeType("AVDictionary*")] IntPtr opts, int flags);
		public delegate AVError PFN_av_hwdevice_ctx_create_derived([NativeType("AVBufferRef**")] IntPtr dstCtx, AVHWDeviceType type, [NativeType("AVBufferRef*")] IntPtr srcCtx, int flags);
		[return: NativeType("AVBufferRef*")]
		public delegate IntPtr PFN_av_hwframe_ctx_alloc([NativeType("AVBufferRef*")] IntPtr deviceCtx);
		public delegate AVError PFN_av_hwframe_ctx_init([NativeType("AVBufferRef*")] IntPtr _ref);
		public delegate AVError PFN_av_hwframe_get_buffer([NativeType("AVBufferRef*")] IntPtr hwframeCtx, [NativeType("AVFrame*")] IntPtr frame, int flags);
		public delegate AVError PFN_av_hwframe_transfer_data([NativeType("AVFrame*")] IntPtr dst, [NativeType("const AVFrame*")] IntPtr src, int flags);
		public delegate AVError PFN_av_hwframe_transfer_get_formats([NativeType("AVBufferRef*")] IntPtr hwframeCtx, AVHWFrameTransferDirection dir, [NativeType("AVPixelFormat**")] IntPtr formats, int flags);
		public delegate IntPtr PFN_av_hwdevice_hwconfig_alloc([NativeType("AVBufferRef*")] IntPtr deviceCtx);
		[return: NativeType("AVHWFramesConstraints*")]
		public delegate IntPtr PFN_av_hwdevice_get_hwframe_constraints([NativeType("AVBufferRef*")] IntPtr _ref, IntPtr hwconfig);
		public delegate void PFN_av_hwframe_constraints_free([NativeType("AVHWFramesConstraints**")] IntPtr constraints);
		public delegate AVError PFN_av_hwframe_map([NativeType("AVFrame*")] IntPtr dst, [NativeType("const AVFrame*")] IntPtr src, AVHWFrameMapFlags flags);
		public delegate AVError PFN_av_hwframe_ctx_create_derived([NativeType("AVBufferRef**")] IntPtr derivedFrameCtx, AVPixelFormat format, [NativeType("AVBufferRef*")] IntPtr derivedDeviceCtx, [NativeType("AVBufferRef*")] IntPtr sourceFrameCtx, AVHWFrameMapFlags flags);

		public PFN_av_hwdevice_find_type_by_name av_hwdevice_find_type_by_name;
		public PFN_av_hwdevice_get_type_name av_hwdevice_get_type_name;
		public PFN_av_hwdevice_iterate_types av_hwdevice_iterate_types;
		public PFN_av_hwdevice_ctx_alloc av_hwdevice_ctx_alloc;
		public PFN_av_hwdevice_ctx_init av_hwdevice_ctx_init;
		public PFN_av_hwdevice_ctx_create av_hwdevice_ctx_create;
		public PFN_av_hwdevice_ctx_create_derived av_hwdevice_ctx_create_derived;
		public PFN_av_hwframe_ctx_alloc av_hwframe_ctx_alloc;
		public PFN_av_hwframe_ctx_init av_hwframe_ctx_init;
		public PFN_av_hwframe_get_buffer av_hwframe_get_buffer;
		public PFN_av_hwframe_transfer_data av_hwframe_transfer_data;
		public PFN_av_hwframe_transfer_get_formats av_hwframe_transfer_get_formats;
		public PFN_av_hwdevice_hwconfig_alloc av_hwdevice_hwconfig_alloc;
		public PFN_av_hwdevice_get_hwframe_constraints av_hwdevice_get_hwframe_constraints;
		public PFN_av_hwframe_constraints_free av_hwframe_constraints_free;
		public PFN_av_hwframe_map av_hwframe_map;
		public PFN_av_hwframe_ctx_create_derived av_hwframe_ctx_create_derived;

		// mathematics.h

		public delegate long PFN_av_gcd(long a, long b);
		public delegate long PFN_av_rescale(long a, long b, long c);
		public delegate long PFN_av_rescale_rnd(long a, long b, long c, AVRounding round);
		public delegate long PFN_av_rescale_q(long a, AVRational bq, AVRational cq);
		public delegate long PFN_av_rescale_q_rnd(long a, AVRational bq, AVRational cq, AVRounding round);
		public delegate int PFN_av_compare_ts(long tsA, AVRational tbA, long tsB, AVRational tbB);
		public delegate long PFN_av_compare_mod(ulong a, ulong b, ulong mod);

		public PFN_av_gcd av_gcd;
		public PFN_av_rescale av_rescale;
		public PFN_av_rescale_rnd av_rescale_rnd;
		public PFN_av_rescale_q av_rescale_q;
		public PFN_av_rescale_q_rnd av_rescale_q_rnd;
		public PFN_av_compare_ts av_compare_ts;
		public PFN_av_compare_mod av_compare_mod;

		// channel_layout.h

		public delegate AVChannelMask PFN_av_get_channel_layout([MarshalAs(UnmanagedType.LPStr)] string name);
		public delegate void PFN_av_get_channel_layout_string([NativeType("char*")] IntPtr buf, int bufSize, int nbChannels, AVChannelMask channelLayout);
		public delegate int PFN_av_get_channel_layout_nb_channels(AVChannelMask channelLayout);
		public delegate AVChannelMask PFN_av_get_default_channel_layout(int nbChannels);
		public delegate int PFN_av_get_channel_layout_channel_index(AVChannelMask channelLayout, AVChannelMask channel);
		public delegate AVChannelMask PFN_av_channel_layout_extract_channel(AVChannelMask channelLayout, int channel);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_av_get_channel_name(AVChannelMask channel);

		public PFN_av_get_channel_layout av_get_channel_layout;
		public PFN_av_get_channel_layout_string av_get_channel_layout_string;
		public PFN_av_get_channel_layout_nb_channels av_get_channel_layout_nb_channels;
		public PFN_av_get_default_channel_layout av_get_default_channel_layout;
		public PFN_av_get_channel_layout_channel_index av_get_channel_layout_channel_index;
		public PFN_av_channel_layout_extract_channel av_channel_layout_extract_channel;
		public PFN_av_get_channel_name av_get_channel_name;

	}

}

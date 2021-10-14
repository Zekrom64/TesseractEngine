using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.LibAV.Native;

namespace Tesseract.LibAV {


	public static class LibAVCodec {

		public static readonly LibrarySpec Spec = new() { Name = "avcodec" };
		public static readonly Library Library = LibraryManager.Load(Spec);
		public static LibAVCodecFunctions Functions { get; } = new();

		public const int InputBufferPaddingSize = 8;

		public const int InputBufferMinSize = 16384;

		public const uint GetBufferFlagRef = 1 << 0;

		public const int CompressionDefault = -1;

		public const int LevelUnknown = -99;


		public static int Version => (int)Functions.avcodec_version();

		public static string Configuration => MemoryUtil.GetASCII(Functions.avcodec_configuration());

		public static string License => MemoryUtil.GetASCII(Functions.avcodec_license());


		static LibAVCodec() {
			Library.LoadFunctions(Functions);

#if LIBAVCODEC_VERSION_58
			if (LibAVUtil.VersionMajor(Version) > 58) throw new AVException($"avcodec library has incompatible version >58");
#endif
		}

		public static IConstPointer<AVCodecHWConfig> GetHWConfig(IConstPointer<AVCodec> codec, int index) {
			IntPtr ptr = Functions.avcodec_get_hw_config(codec.Ptr, index);
			return ptr != IntPtr.Zero ? new ManagedPointer<AVCodecHWConfig>(ptr) : null;
		}

		public static void RegisterAll() => Functions.avcodec_register_all();

		// avcodec_get_class

		// avsubtitle_free

		public static AVCodecRef FindDecoder(AVCodecID id) {
			IntPtr pCodec = Functions.avcodec_find_decoder(id);
			return pCodec != IntPtr.Zero ? new AVCodecRef(pCodec) : null;
		}

		public static AVCodecRef FindDecoderByName(string name) {
			IntPtr pCodec = Functions.avcodec_find_decoder_by_name(name);
			return pCodec != IntPtr.Zero ? new AVCodecRef(pCodec) : null;
		}

		public static AVCodecRef FindEncoder(AVCodecID id) {
			IntPtr pCodec = Functions.avcodec_find_encoder(id);
			return pCodec != IntPtr.Zero ? new AVCodecRef(pCodec) : null;
		}

		public static AVCodecRef FIndEncoderByName(string name) {
			IntPtr pCodec = Functions.avcodec_find_encoder_by_name(name);
			return pCodec != IntPtr.Zero ? new AVCodecRef(pCodec) : null;
		}

		public static uint PixFmtToCodecTag(AVPixelFormat pixFmt) => Functions.avcodec_pix_fmt_to_codec_tag(pixFmt);

		public static AVLossFlags GetPixFmtLoss(AVPixelFormat dstFormat, AVPixelFormat srcFormat, bool hasAlpha) => Functions.avcodec_get_pix_fmt_loss(dstFormat, srcFormat, hasAlpha);

		public static AVPixelFormat FindBestPixFmt(in ReadOnlySpan<AVPixelFormat> fmtList, AVPixelFormat srcFormat, bool hasAlpha, out AVLossFlags losses) {
			if (fmtList[^1] != AVPixelFormat.None) { // Format list MUST be terminated by AV_PIX_FMT_NONE
				Span<AVPixelFormat> newFmtList = stackalloc AVPixelFormat[fmtList.Length + 1];
				fmtList.CopyTo(newFmtList);
				newFmtList[fmtList.Length] = AVPixelFormat.None;
				unsafe {
					fixed (AVPixelFormat* pFmtList = newFmtList) {
						return Functions.avcodec_find_best_pix_fmt2((IntPtr)pFmtList, srcFormat, hasAlpha, out losses);
					}
				}
			} else {
				unsafe {
					fixed(AVPixelFormat* pFmtList = fmtList) {
						return Functions.avcodec_find_best_pix_fmt2((IntPtr)pFmtList, srcFormat, hasAlpha, out losses);
					}
				}
			}
		}

		public static string GetCodecTagString(uint codecTag) {
			nuint len = Functions.av_get_codec_tag_string(IntPtr.Zero, 0, codecTag);
			Span<byte> buf = stackalloc byte[(int)len];
			unsafe {
				fixed(byte* pBuf = buf) {
					len = Functions.av_get_codec_tag_string((IntPtr)pBuf, len, codecTag);
					return MemoryUtil.GetASCII(buf[0..(int)len]);
				}
			}
		}

		public static string ProfileName(AVCodecID codecID, int profile) => MemoryUtil.GetASCII(Functions.avcodec_profile_name(codecID, profile));

		public static int GetBitsPerSample(AVCodecID codecID) => Functions.av_get_bits_per_sample(codecID);

		public static int GetExactBitsPerSample(AVCodecID codecID) => Functions.av_get_exact_bits_per_sample(codecID);

		public static ManagedPointer<byte> FastPaddedMalloc(IntPtr ptr, out uint size, nuint minSize) {
			unsafe {
				Functions.av_fast_padded_malloc((IntPtr)(&ptr), out size, minSize);
			}
			return new ManagedPointer<byte>(ptr, new(LibAVUtil.Functions.av_free), (int)size);
		}

		public static uint XiphLacing(Span<byte> s, uint v) {
			if (s.Length < ((v / 255) + 1)) throw new ArgumentException("Buffer not large enough to store result", nameof(s));
			unsafe {
				fixed(byte* pS = s) {
					return Functions.av_xiphlacing((IntPtr)pS, v);
				}
			}
		}

		public static AVMediaType GetMediaType(AVCodecID codecID) => Functions.avcodec_get_type(codecID);

		public static AVCodecDescriptorRef GetDescriptor(AVCodecID codecID) {
			IntPtr pDesc = Functions.avcodec_descriptor_get(codecID);
			return pDesc == IntPtr.Zero ? null : new AVCodecDescriptorRef(new(pDesc));
		}

		public static AVCodecDescriptorRef GetDescriptorByName(string name) {
			IntPtr pDesc = Functions.avcodec_descriptor_get_by_name(name);
			return pDesc == IntPtr.Zero ? null : new AVCodecDescriptorRef(new(pDesc));
		}

		// av_cpb_properties_alloc

	}

}

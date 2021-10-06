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

	}

}

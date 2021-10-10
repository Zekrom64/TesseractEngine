using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.LibAV.Native;

namespace Tesseract.LibAV {
	
	public class LibAVResample {

		public static LibrarySpec LibrarySpec { get; } = new() { Name = "avresample" };
		public static Library Library { get; } = LibraryManager.Load(LibrarySpec);
		public static LibAVResampleFunctions Functions { get; } = new();

		public LibAVResample() {
			Library.LoadFunctions(Functions);
		}

		public const int MaxChannels = 32;

		public static int Version => (int)Functions.avresample_version();

		public static string Configuration => MemoryUtil.GetASCII(Functions.avresample_configuration());

		public static string License => MemoryUtil.GetASCII(Functions.avresample_license());

		// avresample_get_class

		public static void BuildMatrix(AVChannelMask inLayout, AVChannelMask outLayout, double centerMixLevel, double surroundMixLevel, double lfeMixLevel, bool normalize, Span<double> matrix, int stride, AVMatrixEncoding encoding) {
			unsafe {
				fixed(double* pMatrix = matrix) {
					Functions.avresample_build_matrix(inLayout, outLayout, centerMixLevel, surroundMixLevel, lfeMixLevel, normalize, (IntPtr)pMatrix, stride, encoding);
				}
			}
		}

	}

}

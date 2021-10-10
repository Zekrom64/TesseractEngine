using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class AVAudioResampleContext : IDisposable {

		[NativeType("AVAudioResampleContext*")]
		public IntPtr Context { get; }

		public bool IsOpen => LibAVResample.Functions.avresample_is_open(Context);

		public AVAudioResampleContext() {
			Context = LibAVResample.Functions.avresample_alloc_context();
		}

		public void Open() {
			AVError err = LibAVResample.Functions.avresample_open(Context);
			if (err != AVError.None) throw new AVException("Failed to open audio resample context", err);
		}

		public void Close() => LibAVResample.Functions.avresample_close(Context);

		public void Dispose() {
			GC.SuppressFinalize(this);
			IntPtr ctx = Context;
			unsafe {
				LibAVResample.Functions.avresample_free((IntPtr)(&ctx));
			}
		}

		public void GetMatrix(Span<double> matrix, int stride) {
			unsafe {
				fixed(double* pMatrix = matrix) {
					AVError err = LibAVResample.Functions.avresample_get_matrix(Context, (IntPtr)pMatrix, stride);
					if (err != AVError.None) throw new AVException("Failed to get resample context matrix", err);
				}
			}
		}

		public void SetMatrix(in ReadOnlySpan<double> matrix, int stride) {
			unsafe {
				fixed (double* pMatrix = matrix) {
					AVError err = LibAVResample.Functions.avresample_get_matrix(Context, (IntPtr)pMatrix, stride);
					if (err != AVError.None) throw new AVException("Failed to get resample context matrix", err);
				}
			}
		}

		public void SetChannelMapping(in ReadOnlySpan<int> channelMap) {
			unsafe {
				fixed(int* pChannelMap = channelMap) {
					AVError err = LibAVResample.Functions.avresample_set_channel_mapping(Context, (IntPtr)pChannelMap);
					if (err != AVError.None) throw new AVException("Failed to set resample context channel mapping", err);
				}
			}
		}

		public void SetCompensation(int sampleDelta, int compensationDistance) {
			AVError err = LibAVResample.Functions.avresample_set_compensation(Context, sampleDelta, compensationDistance);
			if (err != AVError.None) throw new AVException("Failed to set resample context compensation", err);
		}

		public int GetOutSamples(int nbSamples) => LibAVResample.Functions.avresample_get_out_samples(Context, nbSamples);

		public int Convert(out ManagedPointer<byte>[] output, int outPlaneSize, int outSamples, IConstPointer<byte>[] input, int inPlaneSize, int inSamples) {
			Span<IntPtr> inputPtrs = stackalloc IntPtr[LibAVResample.MaxChannels];
			for (int i = 0; i < input.Length; i++) inputPtrs[i] = input[i].Ptr;
			Span<IntPtr> outputPtrs = stackalloc IntPtr[LibAVResample.MaxChannels];
			int ret;
			unsafe {
				fixed(IntPtr* pInputs = inputPtrs, pOutputs = outputPtrs) {
					ret = LibAVResample.Functions.avresample_convert(Context, (IntPtr)pOutputs, outPlaneSize, outSamples, (IntPtr)pInputs, inPlaneSize, inSamples);
				}
			}
			output = new ManagedPointer<byte>[outputPtrs.Length];
			for (int i = 0; i < output.Length; i++) output[i] = new ManagedPointer<byte>(outputPtrs[i], new(LibAVUtil.Functions.av_freep));
			return ret;
		}

		public int Delay => LibAVResample.Functions.avresample_get_delay(Context);

		public int Available => LibAVResample.Functions.avresample_available(Context);

		public int Read(out ManagedPointer<byte>[] output, int nbSamples) {
			Span<IntPtr> outputPtrs = stackalloc IntPtr[LibAVResample.MaxChannels];
			int ret;
			unsafe {
				fixed(IntPtr* pOutputs = outputPtrs) {
					ret = LibAVResample.Functions.avresample_read(Context, (IntPtr)pOutputs, nbSamples);
				}
			}
			output = new ManagedPointer<byte>[outputPtrs.Length];
			for (int i = 0; i < output.Length; i++) output[i] = new ManagedPointer<byte>(outputPtrs[i], new(LibAVUtil.Functions.av_freep));
			return ret;
		}

		// TODO:
		//public void ConvertFrame(AVFrame* output, AVFrame* input)

		// TODO:
		//public void Config(AVFrame* out, AVFrame* in)

	}
}

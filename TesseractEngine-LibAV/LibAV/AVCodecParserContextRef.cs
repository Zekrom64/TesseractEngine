using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class AVCodecParserContextRef : IDisposable {

		public ManagedPointer<AVCodecParserContext> ParserContext { get; }

		public AVCodecParserContextRef(AVCodecID codecID) {
			IntPtr pCtx = LibAVCodec.Functions.av_parser_init((int)codecID);
			if (pCtx == IntPtr.Zero) throw new AVException($"Could not create codec parser context for codec {codecID}");
			ParserContext = new ManagedPointer<AVCodecParserContext>(pCtx, new(LibAVCodec.Functions.av_parser_close));
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		public int Parse(AVCodecContextRef ctx, out Span<byte> outbuf, in ReadOnlySpan<byte> buf, long pts, long dts, long pos) {
			unsafe {
				fixed (byte* pBuf = buf) {
					IntPtr pOut = IntPtr.Zero;
					int outSize = 0;
					int ret = LibAVCodec.Functions.av_parser_parse2(ParserContext, ctx.Context, (IntPtr)(&pOut), (IntPtr)(&outSize), (IntPtr)pBuf, buf.Length, pts, dts, pos);
					outbuf = new Span<byte>((void*)pOut, outSize);
					return ret;
				}
			}
		}

		public int Parse(AVCodecContextRef ctx, in ReadOnlySpan<byte> buf, long pts, long dts, long pos) {
			unsafe {
				fixed (byte* pBuf = buf) {
					int ret = LibAVCodec.Functions.av_parser_parse2(ParserContext, ctx.Context, IntPtr.Zero, IntPtr.Zero, (IntPtr)pBuf, buf.Length, pts, dts, pos);
					return ret;
				}
			}
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {

	// buffer.h

	public delegate void AVFree(IntPtr opaque, [NativeType("uint8_t*")] IntPtr data);

	[return: NativeType("AVBufferRef*")]
	public delegate IntPtr AVBufferAlloc(int size);

	[return: NativeType("AVBufferRef*")]
	public delegate IntPtr AVBufferAlloc2(IntPtr opaque, int size);

	public delegate void AVPoolFree(IntPtr opaque);

	// log.h

	// void (*callback)(void*, int, const char*, va_list)

	// avcodec.h

	public delegate int AVDefaultExecuteFunc([NativeType("AVCodecContext*")] IntPtr c2, IntPtr arg2);
	public delegate int AVDefaultExecuteFunc2([NativeType("AVCodecContext*")] IntPtr c2, IntPtr arg2, int i1, int i2);

	public delegate int AVLockMgrCB(ref IntPtr mutex, AVLockOp op);

}

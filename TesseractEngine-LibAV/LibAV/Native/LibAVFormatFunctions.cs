using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV.Native {

	public class LibAVFormatFunctions {

		// avio.h

		public delegate int PFN_avio_check([MarshalAs(UnmanagedType.LPStr)] string url, AVIOOpenFlags flags);
		public PFN_avio_check avio_check;

		[return: NativeType("AVIOContext*")]
		public delegate IntPtr PFN_avio_alloc_context([NativeType("unsigned char*")] IntPtr buffer, int bufferSize, bool writeFlag, IntPtr opaque, [MarshalAs(UnmanagedType.FunctionPtr)] AVReadPacket readPacket, [MarshalAs(UnmanagedType.FunctionPtr)] AVWritePacket writePacket, [MarshalAs(UnmanagedType.FunctionPtr)] AVSeek seek);
		public delegate void PFN_avio_context_free([NativeType("AVIOContext**")] IntPtr s);

		public PFN_avio_alloc_context avio_alloc_context;
		public PFN_avio_context_free avio_context_free;

		public delegate void PFN_avio_w8([NativeType("AVIOContext*")] IntPtr s, int b);
		public delegate void PFN_avio_write([NativeType("AVIOContext*")] IntPtr s, [NativeType("const unsigned char*")] IntPtr buf, int size);
		public delegate void PFN_avio_wl64([NativeType("AVIOContext*")] IntPtr s, ulong val);
		public delegate void PFN_avio_wb64([NativeType("AVIOContext*")] IntPtr s, ulong val);
		public delegate void PFN_avio_wl32([NativeType("AVIOContext*")] IntPtr s, uint val);
		public delegate void PFN_avio_wb32([NativeType("AVIOContext*")] IntPtr s, uint val);
		public delegate void PFN_avio_wl24([NativeType("AVIOContext*")] IntPtr s, uint val);
		public delegate void PFN_avio_wb24([NativeType("AVIOContext*")] IntPtr s, uint val);
		public delegate void PFN_avio_wl16([NativeType("AVIOContext*")] IntPtr s, uint val);
		public delegate void PFN_avio_wb16([NativeType("AVIOContext*")] IntPtr s, uint val);

		public PFN_avio_w8 avio_w8;
		public PFN_avio_write avio_write;
		public PFN_avio_wl64 avio_wl64;
		public PFN_avio_wb64 avio_wb64;
		public PFN_avio_wl32 avio_wl32;
		public PFN_avio_wb32 avio_wb32;
		public PFN_avio_wl24 avio_wl24;
		public PFN_avio_wb24 avio_wb24;
		public PFN_avio_wl16 avio_wl16;
		public PFN_avio_wb16 avio_wb16;

		public delegate void PFN_avio_put_str([NativeType("AVIOContext*")] IntPtr s, [MarshalAs(UnmanagedType.LPStr)] string str);
		public delegate void PFN_avio_put_str16le([NativeType("AVIOContext*")] IntPtr s, [MarshalAs(UnmanagedType.LPStr)] string str);
		public delegate void PFN_avio_put_str16be([NativeType("AVIOContext*")] IntPtr s, [MarshalAs(UnmanagedType.LPStr)] string str);

		public PFN_avio_put_str avio_put_str;
		public PFN_avio_put_str16le avio_put_str16le;
		public PFN_avio_put_str16be avio_put_str16be;

		public delegate void PFN_avio_write_marker([NativeType("AVIOContext*")] IntPtr s, long time, AVIODataMarkerType type);
		public PFN_avio_write_marker avio_write_marker;

		public delegate long PFN_avio_seek([NativeType("AVIOContext*")] IntPtr s, long offset, int whence);
		public delegate long PFN_avio_size([NativeType("AVIOContext*")] IntPtr s);
		public delegate void PFN_avio_flush([NativeType("AVIOContext*")] IntPtr s);
		
		public PFN_avio_seek avio_seek;
		public PFN_avio_size avio_size;
		public PFN_avio_flush avio_flush;

		public delegate int PFN_avio_read([NativeType("AVIOContext*")] IntPtr s, [NativeType("unsigned char*")] IntPtr buf, int size);
		public delegate int PFN_avio_read_partial([NativeType("AVIOContext*")] IntPtr s, [NativeType("unsigned char*")] IntPtr buf, int size);

		public PFN_avio_read avio_read;
		public PFN_avio_read_partial avio_read_partial;

		public delegate int PFN_avio_r8([NativeType("AVIOContext*")] IntPtr s);
		public delegate uint PFN_avio_rl16([NativeType("AVIOContext*")] IntPtr s);
		public delegate uint PFN_avio_rl24([NativeType("AVIOContext*")] IntPtr s);
		public delegate uint PFN_avio_rl32([NativeType("AVIOContext*")] IntPtr s);
		public delegate ulong PFN_avio_rl64([NativeType("AVIOContext*")] IntPtr s);
		public delegate uint PFN_avio_rb16([NativeType("AVIOContext*")] IntPtr s);
		public delegate uint PFN_avio_rb24([NativeType("AVIOContext*")] IntPtr s);
		public delegate uint PFN_avio_rb32([NativeType("AVIOContext*")] IntPtr s);
		public delegate ulong PFN_avio_rb64([NativeType("AVIOContext*")] IntPtr s);

		public PFN_avio_r8 avio_r8;
		public PFN_avio_rl16 avio_rl16;
		public PFN_avio_rl24 avio_rl24;
		public PFN_avio_rl32 avio_rl32;
		public PFN_avio_rl64 avio_rl64;
		public PFN_avio_rb16 avio_rb16;
		public PFN_avio_rb24 avio_rb24;
		public PFN_avio_rb32 avio_rb32;
		public PFN_avio_rb64 avio_rb64;

		public delegate int PFN_avio_get_str([NativeType("AVIOContext*")] IntPtr s, int maxlen, [NativeType("char*")] IntPtr buf, int buflen);
		public delegate int PFN_avio_get_str16le([NativeType("AVIOContext*")] IntPtr s, int maxlen, [NativeType("char*")] IntPtr buf, int buflen);
		public delegate int PFN_avio_get_str16be([NativeType("AVIOContext*")] IntPtr s, int maxlen, [NativeType("char*")] IntPtr buf, int buflen);

		public PFN_avio_get_str avio_get_str;
		public PFN_avio_get_str16le avio_get_str16le;
		public PFN_avio_get_str16be avio_get_str16be;

		public delegate AVError PFN_avio_open([NativeType("AVIOContext**")] IntPtr s, [MarshalAs(UnmanagedType.LPStr)] string url, AVIOOpenFlags flags);
		public delegate AVError PFN_avio_open2([NativeType("AVIOContext**")] IntPtr s, [MarshalAs(UnmanagedType.LPStr)] string url, AVIOOpenFlags flags, [NativeType("const AVIOInterruptCB*")] IntPtr intCb, [NativeType("AVDictionary**")] IntPtr options);

		public PFN_avio_open avio_open;
		public PFN_avio_open2 avio_open2;

		public delegate AVError PFN_avio_close([NativeType("AVIOContext*")] IntPtr s);
		public delegate AVError PFN_avio_closep([NativeType("AVIOContext**")] IntPtr s);
		public delegate AVError PFN_avio_open_dyn_buf([NativeType("AVIOContext*")] IntPtr s);
		public delegate int PFN_avio_close_dyn_buf([NativeType("AVIOContext*")] IntPtr s, [NativeType("uint8_t**")] out IntPtr buffer);

		public PFN_avio_close avio_close;
		public PFN_avio_closep avio_closep;
		public PFN_avio_open_dyn_buf avio_open_dyn_buf;
		public PFN_avio_close_dyn_buf avio_close_dyn_buf;

		[return: NativeType("const char*")]
		public delegate IntPtr PFN_avio_enum_protocols([NativeType("void**")] IntPtr opaque, int output);
		public delegate int PFN_avio_pause([NativeType("AVIOContext*")] IntPtr s, bool pause);
		public delegate long PFN_avio_seek_time([NativeType("AVIOContext*")] IntPtr s, int streamIndex, long timestamp, int flags);

	}

}

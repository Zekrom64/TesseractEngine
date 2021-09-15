using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.LMDB {

	using mdb_mode_t = Int32;
	using mdb_size_t = UIntPtr;

	using MDB_dbi = UInt32;

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct MDBVal {

		private readonly nuint size;
		public nuint Size { get => size; init => size = value; }

		private readonly IntPtr data;
		public IntPtr Data { get => data; init => data = value; }

	}

	public delegate int MDBCmpFunc([NativeType("const MDB_val*")] IntPtr a, [NativeType("const MDB_val*")] IntPtr b);

	public delegate void MDBRelFunc([NativeType("MDB_val*")] IntPtr item, IntPtr oldptr, IntPtr newptr, IntPtr relctx);

	[Flags]
	public enum MDBEnvFlags : uint {
		FixedMap =     0x00000001,
		NoSubDir =     0x00004000,
		NoSync =       0x00010000,
		ReadOnly =     0x00020000,
		NoMetaSync =   0x00040000,
		WriteMap =     0x00080000,
		MapAsync =     0x00100000,
		NoTLS =        0x00200000,
		NoLock =       0x00400000,
		NoReadAhead =  0x00800000,
		NoMemInit =    0x01000000,
		PrevSnapshot = 0x02000000
	}

	[Flags]
	public enum MDBDBFlags : uint {
		ReverseKey = 0x02,
		DupSort =    0x04,
		IntegerKey = 0x08,
		DupFixed =   0x10,
		IntegerDup = 0x20,
		ReverseDup = 0x40,
		Create =     0x40000
	}

	[Flags]
	public enum MDBWriteFlags : uint {
		NoOverwrite = 0x10,
		NoDupData =  0x20,
		Current =    0x40,
		Reserve =    0x10000,
		Append =     0x20000,
		AppendDup =  0x40000,
		Multiple =   0x80000
	}

	[Flags]
	public enum MDBCopyFlags : uint {
		Compact = 0x01
	}

	public enum MDBCursorOp {
		First,
		FirstDup,

		GetBoth,
		GetBothRange,
		GetCurrent,
		GetMultiple,

		Last,
		LastDup,

		Next,
		NextDup,

		NextMultiple,

		NextNoDup,
		Prev,
		PrevDup,

		PrevNoDup,
		Set,
		SetKey,
		SetRange,
		PrevMultiple
	}

	public enum MDBResult {
		Success = 0,
		KeyExist = -30799,
		NotFound = -30798,
		PageNotFound = -30797,
		Corrupted = -30796,
		Panic = -30795,
		VersionMismatch = -30794,
		Invalid = -30793,
		MapFull = -30792,
		DBSFull = -30791,
		ReadersFull = -30790,
		TLSFull = -30789,
		TXNFull = -30788,
		CursorFull = -30787,
		PageFull = -30786,
		MapResized = -30785,
		Incompatible = -30784,
		BadRSlot = -30783,
		BadTXN = -30782,
		BadValSize = -30781,
		BadDBI = -30780,
		Problem = -30779
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MDBStat {

		public uint PageSize;

		public uint Depth;

		public mdb_size_t BranchPages;

		public mdb_size_t LeafPages;

		public mdb_size_t OverflowPages;

		public mdb_size_t Entries;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MDBEnvInfo {

		public IntPtr MapAddr;

		public mdb_size_t MapSize;

		public mdb_size_t LastPageNum;

		public mdb_size_t LastTXNID;

		public uint MaxReaders;

		public uint NumReaders;

	}

	public delegate void MDBAssertFunc([NativeType("MDB_env*")] IntPtr env, [MarshalAs(UnmanagedType.LPStr)] string msg);

	public delegate MDBResult MDBMsgFunc([MarshalAs(UnmanagedType.LPStr)] string msg, IntPtr ctx);

	public class MDBFunctions {

		[return: NativeType("char*")]
		public delegate IntPtr PFN_mdb_version(out int major, out int minor, out int patch);
		[return: NativeType("char*")]
		public delegate IntPtr PFN_mdb_strerror(MDBResult err);

		public PFN_mdb_version mdb_version;
		public PFN_mdb_strerror mdb_strerror;

		public delegate MDBResult PFN_mdb_env_create([NativeType("MDB_env**")] out IntPtr env);
		public delegate MDBResult PFN_mdb_env_open([NativeType("MDB_env*")] IntPtr env, [MarshalAs(UnmanagedType.LPStr)] string path, MDBEnvFlags flags, mdb_mode_t mode);
		public delegate MDBResult PFN_mdb_env_copy([NativeType("MDB_env*")] IntPtr env, [MarshalAs(UnmanagedType.LPStr)] string path);
		public delegate MDBResult PFN_mdb_env_copyfd_WIN32([NativeType("MDB_env*")] IntPtr env, IntPtr fd);
		public delegate MDBResult PFN_mdb_env_copyfd_POSIX([NativeType("MDB_env*")] IntPtr env, int fd);
		public delegate MDBResult PFN_mdb_env_copy2([NativeType("MDB_env*")] IntPtr env, [MarshalAs(UnmanagedType.LPStr)] string path, MDBCopyFlags flags);
		public delegate MDBResult PFN_mdb_env_copyfd2_WIN32([NativeType("MDB_env*")] IntPtr env, IntPtr fd, MDBEnvFlags flags);
		public delegate MDBResult PFN_mdb_env_copyfd2_POSIX([NativeType("MDB_env*")] IntPtr env, int fd, MDBEnvFlags flags);
		public delegate MDBResult PFN_mdb_env_stat([NativeType("MDB_env*")] IntPtr env, out MDBStat stat);
		public delegate MDBResult PFN_mdb_env_info([NativeType("MDB_env*")] IntPtr env, out MDBEnvInfo stat);
		public delegate MDBResult PFN_mdb_env_sync([NativeType("MDB_env*")] IntPtr env, bool force);
		public delegate MDBResult PFN_mdb_env_close([NativeType("MDB_env*")] IntPtr env);
		public delegate MDBResult PFN_mdb_env_set_flags([NativeType("MDB_env*")] IntPtr env, MDBEnvFlags flags, bool onoff);
		public delegate MDBResult PFN_mdb_env_get_flags([NativeType("MDB_env*")] IntPtr env, out MDBEnvFlags flags);
		public delegate MDBResult PFN_mdb_env_get_path([NativeType("MDB_env*")] IntPtr env, [NativeType("const char**")] out IntPtr path);
		public delegate MDBResult PFN_mdb_env_get_fd_WIN32([NativeType("MDB_env*")] IntPtr env, out IntPtr fd);
		public delegate MDBResult PFN_mdb_env_get_fd_POSIX([NativeType("MDB_env*")] IntPtr env, out int fd);
		public delegate MDBResult PFN_mdb_env_set_mapsize([NativeType("MDB_env*")] IntPtr env, mdb_size_t size);
		public delegate MDBResult PFN_mdb_env_set_maxreaders([NativeType("MDB_env*")] IntPtr env, uint readers);
		public delegate MDBResult PFN_mdb_env_get_maxreaders([NativeType("MDB_env*")] IntPtr env, out uint readers);
		public delegate MDBResult PFN_mdb_env_set_maxdbs([NativeType("MDB_env*")] IntPtr env, MDB_dbi dbs);
		public delegate int PFN_mdb_env_get_maxkeysize([NativeType("MDB_env*")] IntPtr env);
		public delegate MDBResult PFN_mdb_env_set_userctx([NativeType("MDB_env*")] IntPtr env, IntPtr ctx);
		public delegate IntPtr PFN_mdb_env_get_userctx([NativeType("MDB_env*")] IntPtr env);
		public delegate MDBResult PFN_mdb_env_set_assert([NativeType("MDB_env*")] IntPtr env, [MarshalAs(UnmanagedType.FunctionPtr)] MDBAssertFunc func);

		public PFN_mdb_env_create mdb_env_create;
		public PFN_mdb_env_open mdb_env_open;
		public PFN_mdb_env_copy mdb_env_copy;
		[ExternFunction(AltNames = new string[] { "mdb_env_copyfd" }, Platform = Core.PlatformType.Windows)]
		public PFN_mdb_env_copyfd_WIN32 mdb_env_copyfd_WIN32;
		[ExternFunction(AltNames = new string[] { "mdb_env_copyfd" }, Platform = Core.PlatformType.Linux)]
		public PFN_mdb_env_copyfd_POSIX mdb_env_copyfd_POSIX;
		public PFN_mdb_env_copy2 mdb_env_copy2;
		[ExternFunction(AltNames = new string[] { "mdb_env_copyfd2" }, Platform = Core.PlatformType.Windows)]
		public PFN_mdb_env_copyfd2_WIN32 mdb_env_copyfd2_WIN32;
		[ExternFunction(AltNames = new string[] { "mdb_env_copyfd2" }, Platform = Core.PlatformType.Linux)]
		public PFN_mdb_env_copyfd2_POSIX mdb_env_copyfd2_POSIX;
		public PFN_mdb_env_stat mdb_env_stat;
		public PFN_mdb_env_info mdb_env_info;
		public PFN_mdb_env_sync mdb_env_sync;
		public PFN_mdb_env_close mdb_env_close;
		public PFN_mdb_env_set_flags mdb_env_set_flags;
		public PFN_mdb_env_get_flags mdb_env_get_flags;
		public PFN_mdb_env_get_path mdb_env_get_path;
		[ExternFunction(AltNames = new string[] { "mdb_env_get_fd" }, Platform = Core.PlatformType.Windows)]
		public PFN_mdb_env_get_fd_WIN32 mdb_enf_get_fd_WIN32;
		[ExternFunction(AltNames = new string[] { "mdb_env_get_fd" }, Platform = Core.PlatformType.Windows)]
		public PFN_mdb_env_get_fd_POSIX mdb_enf_get_fd_POSIX;
		public PFN_mdb_env_set_mapsize mdb_env_set_mapsize;
		public PFN_mdb_env_set_maxreaders mdb_env_set_maxreaders;
		public PFN_mdb_env_get_maxreaders mdb_env_get_maxreaders;
		public PFN_mdb_env_set_maxdbs mdb_env_set_maxdbs;
		public PFN_mdb_env_get_maxkeysize mdb_env_get_maxkeysize;
		public PFN_mdb_env_set_userctx mdb_env_set_userctx;
		public PFN_mdb_env_get_userctx mdb_env_get_userctx;
		public PFN_mdb_env_set_assert mdb_env_set_assert;

		public delegate MDBResult PFN_mdb_txn_begin([NativeType("MDB_env*")] IntPtr env, [NativeType("MDB_txn*")] IntPtr parent, MDBEnvFlags flags, [NativeType("MDB_txn**")] out IntPtr txn);
		[return: NativeType("MDB_env*")]
		public delegate IntPtr PFN_mdb_txn_env([NativeType("MDB_txn*")] IntPtr txn);
		public delegate mdb_size_t PFN_mdb_txn_id([NativeType("MDB_txn*")] IntPtr txn);
		public delegate MDBResult PFN_mdb_txn_commit([NativeType("MDB_txn*")] IntPtr txn);
		public delegate MDBResult PFN_mdb_txn_abort([NativeType("MDB_txn*")] IntPtr txn);
		public delegate MDBResult PFN_mdb_txn_reset([NativeType("MDB_txn*")] IntPtr txn);
		public delegate MDBResult PFN_mdb_txn_renew([NativeType("MDB_txn*")] IntPtr txn);

		public PFN_mdb_txn_begin mdb_txn_begin;
		public PFN_mdb_txn_env mdb_txn_env;
		public PFN_mdb_txn_id mdb_txn_id;
		public PFN_mdb_txn_commit mdb_txn_commit;
		public PFN_mdb_txn_abort mdb_txn_abort;
		public PFN_mdb_txn_reset mdb_txn_reset;
		public PFN_mdb_txn_renew mdb_txn_renew;

		public delegate MDBResult PFN_mdb_dbi_open([NativeType("MDB_txn*")] IntPtr txn, [MarshalAs(UnmanagedType.LPStr)] string name, MDBDBFlags flags, out MDB_dbi dbi);
		public delegate MDBResult PFN_mdb_stat([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, out MDBStat stat);
		public delegate MDBResult PFN_mdb_dbi_flags([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, out MDBDBFlags flags);
		public delegate MDBResult PFN_mdb_dbi_close([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi);
		public delegate MDBResult PFN_mdb_drop([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, bool del);
		public delegate MDBResult PFN_mdb_set_compare([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, [MarshalAs(UnmanagedType.FunctionPtr)] MDBCmpFunc cmp);
		public delegate MDBResult PFN_mdb_set_dupsort([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, [MarshalAs(UnmanagedType.FunctionPtr)] MDBCmpFunc cmp);
		public delegate MDBResult PFN_mdb_set_relfunc([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, [MarshalAs(UnmanagedType.FunctionPtr)] MDBRelFunc rel);
		public delegate MDBResult PFN_mdb_set_relctx([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, IntPtr ctx);

		public PFN_mdb_dbi_open mdb_dbi_open;
		public PFN_mdb_stat mdb_stat;
		public PFN_mdb_dbi_flags mdb_dbi_flags;
		public PFN_mdb_dbi_close mdb_dbi_close;
		public PFN_mdb_drop mdb_drop;
		public PFN_mdb_set_compare mdb_set_compare;
		public PFN_mdb_set_dupsort mdb_set_dupsort;
		public PFN_mdb_set_relfunc mdb_set_relfunc;
		public PFN_mdb_set_relctx mdb_set_relctx;

		public delegate MDBResult PFN_mdb_get([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, in MDBVal key, out MDBVal data);
		public delegate MDBResult PFN_mdb_put([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, in MDBVal key, in MDBVal data, MDBWriteFlags flags);
		public delegate MDBResult PFN_mdb_del([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, in MDBVal key, [NativeType("MDB_val*")] IntPtr data);
		public delegate MDBResult PFN_mdb_cursor_open([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, [NativeType("MDB_cursor**")] out IntPtr cursor);
		public delegate void PFN_mdb_cursor_close([NativeType("MDB_cursor*")] IntPtr cursor);
		public delegate MDBResult PFN_mdb_cursor_renew([NativeType("MDB_txn*")] IntPtr txn, [NativeType("MDB_cursor*")] IntPtr cursor);
		[return: NativeType("MDB_txn*")]
		public delegate IntPtr PFN_mdb_cursor_txn([NativeType("MDB_cursor*")] IntPtr cursor);
		public delegate MDB_dbi PFN_mdb_cursor_dbi([NativeType("MDB_cursor*")] IntPtr cursor);
		public delegate MDBResult PFN_mdb_cursor_get([NativeType("MDB_cursor*")] IntPtr cursor, ref MDBVal key, out MDBVal data, MDBCursorOp op);
		public delegate MDBResult PFN_mdb_cursor_put([NativeType("MDB_cursor*")] IntPtr cursor, in MDBVal key, in MDBVal data, MDBWriteFlags flags);
		public delegate MDBResult PFN_mdb_cursor_del([NativeType("MDB_cursor*")] IntPtr cursor, MDBWriteFlags flags);
		public delegate MDBResult PFN_mdb_cursor_count([NativeType("MDB_cursor*")] IntPtr cursor, out mdb_size_t countp);
		public delegate int PFN_mdb_cmp([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, in MDBVal a, in MDBVal b);
		public delegate int PFN_mdb_dcmp([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, in MDBVal a, in MDBVal b);
		public delegate MDBResult PFN_mdb_reader_list([NativeType("MDB_env*")] IntPtr env, [MarshalAs(UnmanagedType.FunctionPtr)] MDBMsgFunc func, IntPtr ctx);
		public delegate MDBResult PFN_mdb_reader_check([NativeType("MDB_env*")] IntPtr env, out int dead);

		public PFN_mdb_get mdb_get;
		public PFN_mdb_put mdb_put;
		public PFN_mdb_del mdb_del;
		public PFN_mdb_cursor_open mdb_cursor_open;
		public PFN_mdb_cursor_close mdb_cursor_close;
		public PFN_mdb_cursor_renew mdb_cursor_renew;
		public PFN_mdb_cursor_txn mdb_cursor_txn;
		public PFN_mdb_cursor_dbi mdb_cursor_dbi;
		public PFN_mdb_cursor_get mdb_cursor_get;
		public PFN_mdb_cursor_put mdb_cursor_put;
		public PFN_mdb_cursor_del mdb_cursor_del;
		public PFN_mdb_cursor_count mdb_cursor_count;
		public PFN_mdb_cmp mdb_cmp;
		public PFN_mdb_dcmp mdb_dcmp;
		public PFN_mdb_reader_list mdb_reader_list;
		public PFN_mdb_reader_check mdb_reader_check;

	}

	public class MDB {

		public static readonly LibrarySpec Spec = new() { Name = "lmdb" };
		public static readonly Library Library = LibraryManager.Load(Spec);

		public static MDBFunctions Functions { get; } = new();

		static MDB() {
			Library.LoadFunctions(Functions);
		}

		public static int VerInt(int a, int b, int c) => (a << 24) | (b << 16) | c;

		public static string GetVersion(out int major, out int minor, out int patch) => MemoryUtil.GetASCII(Functions.mdb_version(out major, out minor, out patch));

		public static string StrError(MDBResult err) => MemoryUtil.GetASCII(Functions.mdb_strerror(err));

	}

	public class MDBException : Exception {

		public MDBException(string err) : base(err) { }

		public MDBException(string str, MDBResult err) : base(str + ": " + MDB.StrError(err)) { }

	}

}

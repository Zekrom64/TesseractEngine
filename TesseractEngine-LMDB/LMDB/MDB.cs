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

	/// <summary>
	/// <para>Generic structure used for passing keys and data in and out of the database.</para>
	/// <para>
	/// Values returned from the database are valid only until a subsequent update operation, or the end of the transaction.
	/// Do not modify or free them, they commonly point into the database itself.
	/// </para>
	/// <para>
	/// Key sizes must be between 1 and <see cref="MDBEnv.MaxKeySize"/> inclusive. The same applies to data sizes in databases
	/// with the <see cref="MDBDBFlags.DupSort"/> flag. Other data items can in theory be from 0 to 0xFFFFFFFF bytes long.
	/// </para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct MDBVal {

		private readonly nuint size;
		/// <summary>
		/// The size of the data item.
		/// </summary>
		public nuint Size { get => size; init => size = value; }

		private readonly IntPtr data;
		/// <summary>
		/// The address of the data item.
		/// </summary>
		public IntPtr Data { get => data; init => data = value; }

	}

	public delegate int MDBCmpFunc([NativeType("const MDB_val*")] IntPtr a, [NativeType("const MDB_val*")] IntPtr b);

	/// <summary>
	/// <para>A callback function used to relocate a position-dependent data item in a fixed-address database.</para>
	/// <para>
	/// The <i>newptr</i> gives the item's desired address in the memory map, and <i>oldptr</i> gives its previous address. The
	/// item's actual data resides at the address in <i>item</i>. This callback is expected to walk through the fields of
	/// the record in item and modify any values based at the <i>oldptr</i> address to be relative to the <i>newptr</i> address.
	/// </para>
	/// </summary>
	/// <param name="item">The item that is to be relocated</param>
	/// <param name="oldptr">The previous address</param>
	/// <param name="newptr">The new address to relocate to</param>
	/// <param name="relctx">An application provided context, set by <see cref="MDBDBI.RelocateContext"/></param>
	public delegate void MDBRelFunc([NativeType("MDB_val*")] IntPtr item, IntPtr oldptr, IntPtr newptr, IntPtr relctx);

	/// <summary>
	/// Flags specifying behavior for an <see cref="MDBEnv"/>.
	/// </summary>
	[Flags]
	public enum MDBEnvFlags : uint {
		/// <summary>
		/// Use a fixed address for the mmap region. This flag must be specified when creating the environment, and is
		/// stored persistently in the environment. If successful, the memory map will always reside at the same virtual
		/// address and pointers used to reference data items in the database will be constant across multiple invocations.
		/// This option may not always work, depending on how the operating system has allocated memory to shared libraries
		/// and other uses. The feature is highly experimental.
		/// </summary>
		FixedMap =     0x00000001,
		/// <summary>
		/// By default, LMDB creates its environment in a directory whose pathname is given in path, and creates its data and
		/// lock files under that directory. With this option, path is used as-is for the database main data file. The database
		/// lock file is the path with "-lock" appended.
		/// </summary>
		NoSubDir =     0x00004000,
		/// <summary>
		/// Don't flush system buffers to disk when committing a transaction. This optimization means a system crash can corrupt
		/// the database or lose the last transactions if buffers are not yet flushed to disk. The risk is governed by how often
		/// the system flushes dirty buffers to disk and how often <see cref="MDBEnv.Sync(bool)"/> is called. However, if the
		/// filesystem preserves write order and the <see cref="WriteMap"/> flag is not used, transactions exhibit ACI (atomicity,
		/// consistency, isolation) properties and only lose D (durability). I.e. database integrity is maintained, but a system
		/// crash may undo the final transactions. Note that (<see cref="NoSync"/> | <see cref="WriteMap"/>) leaves the system with
		/// no hint for when to write transactions to disk, unless <see cref="MDBEnv.Sync(bool)"/> is called. (<see cref="MapAsync"/>
		/// | <see cref="WriteMap"/>) may be preferable. This flag may be changed at any time using <see cref="MDBEnv.SetFlags(MDBEnvFlags, bool)"/>.
		/// </summary>
		NoSync =       0x00010000,
		/// <summary>
		/// Open the environment in read-only mode. No write operations will be allowed. LMDB will still modify the lock file -
		/// except on read-only filesystems, where LMDB does not use locks.
		/// </summary>
		ReadOnly =     0x00020000,
		/// <summary>
		/// Flush system buffers to disk only once per transaction, omit the metadata flush. Defer that until the system flushes
		/// files to disk, or next non-<see cref="ReadOnly"/> commit or <see cref="MDBEnv.Sync(bool)"/>. This optimization maintains
		/// database integrity, but a system crash may undo the last committed transaction. I.e. it preserves the ACI (atomicity,
		/// consistency, isolation) but not D (durability) database property. This flag may be changed at any time using
		/// <see cref="MDBEnv.SetFlags(MDBEnvFlags, bool)"/>.
		/// </summary>
		NoMetaSync =   0x00040000,
		/// <summary>
		/// Use a writeable memory map unless <see cref="ReadOnly"/> is set. This is faster and uses fewer mallocs, but loses
		/// protection from application bugs like wild pointer writes and other bad updates into the database. Incompatible with
		/// nested transactions. Do not mix processes with and without <see cref="WriteMap"/> on the same environment. This can
		/// defeat durability (<see cref="MDBEnv.Sync(bool)"/> etc).
		/// </summary>
		WriteMap =     0x00080000,
		/// <summary>
		/// When using <see cref="WriteMap"/>, use asynchronous flushes to disk. As with <see cref="NoSync"/>, a system crash can
		/// then corrupt the database or lose the last transactions. Calling <see cref="MDBEnv.Sync(bool)"/> ensures on-disk database
		/// integrity until next commit. This flag may be changed at any time using <see cref="MDBEnv.SetFlags(MDBEnvFlags, bool)"/>.
		/// </summary>
		MapAsync =     0x00100000,
		/// <summary>
		/// Don't use Thread-Local Storage. Tie reader locktable slots to <see cref="MDBTxn"/> objects instead of to threads. I.e.
		/// <see cref="MDBTxn.Reset"/> keeps the slot reseved for the <see cref="MDBTxn"/> object. A thread may use parallel read-only
		/// transactions. A read-only transaction may span threads if the user synchronizes its use. Applications that multiplex many
		/// user threads over individual OS threads need this option. Such an application must also serialize the write transactions in
		/// an OS thread, since LMDB's write locking is unaware of the user threads.
		/// </summary>
		NoTLS =        0x00200000,
		/// <summary>
		/// Don't do any locking. If concurrent access is anticipated, the caller must manage all concurrency itself. For proper
		/// operation the caller must enforce single-writer semantics, and must ensure that no readers are using old transactions
		/// while a writer is active. The simplest approach is to use an exclusive lock so that no readers may be active at all when
		/// a writer begins.
		/// </summary>
		NoLock =       0x00400000,
		/// <summary>
		/// Turn off readahead. Most operating systems perform readahead on read requests by default. This option turns it off if
		/// the OS supports it. Turning it off may help random read performance when the DB is larger than RAM and system RAM is
		/// full. The option is not implemented on Windows.
		/// </summary>
		NoReadAhead =  0x00800000,
		/// <summary>
		/// Don't initialize malloc'd memory before writing to unused spaces in the data file. By default, memory for pages written
		/// to the data file is obtained using malloc. While these pages may be reused in subsequent transactions, freshly malloc'd
		/// pages will be initialized to zeroes before use. This avoids persisting leftover data from other code (that used the heap
		/// and subsequently freed the memory) into the data file. Note that many other system libraries may allocate and free memory
		/// from the heap for arbitrary uses. E.g., stdio may use the heap for file I/O buffers. This initialization step has a
		/// modest performance cost so some applications may want to disable it using this flag. This option can be a problem for
		/// applications which handle sensitive data like passwords, and it makes memory checkers like Valgrind noisy. This flag is
		/// not needed with <see cref="WriteMap"/>, which writes directly to the mmap instead of using malloc for pages. The
		/// initialization is also skipped if <see cref="MDBWriteFlags.Reserve"/> is used; the caller is expected to overwrite all of
		/// the memory that was reserved in that case. This flag may be changed at any time using <see cref="MDBEnv.SetFlags(MDBEnvFlags, bool)"/>.
		/// </summary>
		NoMemInit =    0x01000000,
		PrevSnapshot = 0x02000000
	}

	[Flags]
	public enum MDBDBFlags : uint {
		/// <summary>
		/// Keys are strings to be compared in reverse order, from the end of the strings to the beginning. By default, Keys are treated as strings and compared from beginning to end.
		/// </summary>
		ReverseKey = 0x02,
		/// <summary>
		/// Duplicate keys may be used in the database. (Or, from another perspective, keys may have multiple data items, stored in sorted order.) By default keys must be unique and may have only a single data item.
		/// </summary>
		DupSort =    0x04,
		/// <summary>
		/// Keys are binary integers in native byte order, either unsigned int or size_t, and will be sorted as such. The keys must all be of the same size.
		/// </summary>
		IntegerKey = 0x08,
		/// <summary>
		/// This flag may only be used in combination with <see cref="DupSort"/>. This option tells the library that the data
		/// items for this database are all the same size, which allows further optimizations in storage and retrieval. When all
		/// data items are the same size, the <see cref="MDBCursorOp.GetMultiple"/> and <see cref="MDBCursorOp.NextMultiple"/>
		/// cursor operations may be used to retrieve multiple items at once.
		/// </summary>
		DupFixed =   0x10,
		/// <summary>
		/// This option specifies that duplicate data items are binary integers, similar to <see cref="IntegerKey"/> keys.
		/// </summary>
		IntegerDup = 0x20,
		/// <summary>
		/// This option specifies that duplicate data items should be compared as strings in reverse order.
		/// </summary>
		ReverseDup = 0x40,
		/// <summary>
		/// Create the named database if it doesn't exist. This option is not allowed in a read-only transaction or a read-only environment.
		/// </summary>
		Create =     0x40000
	}

	[Flags]
	public enum MDBWriteFlags : uint {
		/// <summary>
		/// Enter the new key/data pair only if the key does not already appear in the database.
		/// </summary>
		NoOverwrite = 0x10,
		/// <summary>
		/// <para>
		/// For writes, enter the new key/data pair only if it does not already appear in the database. This flag may only
		/// be specified if the database was opened with <see cref="MDBDBFlags.DupSort"/>. The return value will indicate
		/// if the key/data pair was stored.
		/// </para>
		/// <para>
		/// For deletes, delete all the data items for the current key. This flag may only be specified if the database
		/// was opeened with <see cref="MDBDBFlags.DupSort"/>.
		/// </para>
		/// </summary>
		NoDupData =  0x20,
		/// <summary>
		/// Replace the item at the current cursor position. The <i>key</i> parameter must still be provided, and must
		/// match it. If using sorted duplicates (<see cref="MDBDBFlags.DupSort"/>) the data item must still sort into
		/// the same place. This is intended to be used when the new data is the same size as the old. Otherwise it will
		/// simply perform a delete of the old record followed by an insert. Only supported for cursor operations.
		/// </summary>
		Current =    0x40,
		/// <summary>
		/// Reserve space for data of the given size, but don't copy the given data. Instead, return a pointer to the
		/// reserved space, which the caller can fill in later. This saves an extra memcpy if the data is being generated
		/// later. This flag must not be specified if the database was opened with <see cref="MDBDBFlags.DupSort"/>.
		/// </summary>
		Reserve =    0x10000,
		/// <summary>
		/// Append the given key/data pair to the end of the database. No key comparisons are performed. This option
		/// allows fast bulk loading when keys are already known to be in the correct order. Loading unsorted keys
		/// with this flag will return a false value.
		/// </summary>
		Append =     0x20000,
		/// <summary>
		/// Identical to <see cref="Append"/>, but for sorted dup data.
		/// </summary>
		AppendDup =  0x40000,
		/// <summary>
		/// Store multiple contiguous data elements in a single request. This flag may only be specified if the database was
		/// opened with <see cref="MDBDBFlags.DupFixed"/>. The <i>data</i> argument must be an array of two values. The size
		/// of the first value must be the size of a single data element. The data of the first value must point to the beginning
		/// of the array of contiguous data elements. The size of the second value must be the count of the number of data
		/// elements to store. On return this field will be set to the count of the number of elements actually written. The
		/// data of the second value is unused. Only supported for cursor operations.
		/// </summary>
		Multiple =   0x80000
	}

	[Flags]
	public enum MDBCopyFlags : uint {
		Compact = 0x01
	}

	/// <summary>
	/// Cursor get operations. This is the set of all operations for retrieving data using a cursor.
	/// </summary>
	public enum MDBCursorOp {
		/// <summary>
		/// Position at first key/data item.
		/// </summary>
		First,
		/// <summary>
		/// Position at first data item of current key. Only for <see cref="MDBDBFlags.DupSort"/>.
		/// </summary>
		FirstDup,

		/// <summary>
		/// Position at key/data pair. Only for <see cref="MDBDBFlags.DupSort"/>.
		/// </summary>
		GetBoth,
		/// <summary>
		/// Position at key, nearest data. Only for <see cref="MDBDBFlags.DupSort"/>.
		/// </summary>
		GetBothRange,
		/// <summary>
		/// Return key/data at current cursor position.
		/// </summary>
		GetCurrent,
		/// <summary>
		/// Return key and up to a page of duplicate data items from the current cursor position. Move cursor to
		/// prepare for <see cref="NextMultiple"/>. Only for <see cref="MDBDBFlags.DupFixed"/>.
		/// </summary>
		GetMultiple,

		/// <summary>
		/// Position at last key/data item.
		/// </summary>
		Last,
		/// <summary>
		/// Position at last data item of current key. Only for <see cref="MDBDBFlags.DupSort"/>.
		/// </summary>
		LastDup,

		/// <summary>
		/// Position at next data item.
		/// </summary>
		Next,
		/// <summary>
		/// Position at next data item of current key. Only for <see cref="MDBDBFlags.DupSort"/>.
		/// </summary>
		NextDup,

		/// <summary>
		/// Return key and up to a page of duplicate data items from next cursor position. Move cursor to prepare for
		/// <see cref="NextMultiple"/>. Only for <see cref="MDBDBFlags.DupFixed"/>.
		/// </summary>
		NextMultiple,

		/// <summary>
		/// Position at first data item of next key.
		/// </summary>
		NextNoDup,
		/// <summary>
		/// Position at previous data item.
		/// </summary>
		Prev,
		/// <summary>
		/// Position at previous data item of current key. Only for <see cref="MDBDBFlags.DupSort"/>.
		/// </summary>
		PrevDup,

		/// <summary>
		/// Position at last data item of previous key.
		/// </summary>
		PrevNoDup,
		/// <summary>
		/// Position at specified key.
		/// </summary>
		Set,
		/// <summary>
		/// Position at specified key.
		/// </summary>
		SetKey,
		/// <summary>
		/// Position at specified key, return key + data.
		/// </summary>
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

	/// <summary>
	/// Statistics for the database environment.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct MDBStat {

		/// <summary>
		/// Size of a database page. This is currently the same for all databases.
		/// </summary>
		public uint PageSize;

		/// <summary>
		/// Depth (height) of the B-tree.
		/// </summary>
		public uint Depth;

		/// <summary>
		/// Number of internal (non-leaf) pages.
		/// </summary>
		public mdb_size_t BranchPages;

		/// <summary>
		/// Number of leaf pages.
		/// </summary>
		public mdb_size_t LeafPages;

		/// <summary>
		/// Number of overflow pages.
		/// </summary>
		public mdb_size_t OverflowPages;

		/// <summary>
		/// Number of data items.
		/// </summary>
		public mdb_size_t Entries;

	}

	/// <summary>
	/// Information about the environment.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct MDBEnvInfo {

		/// <summary>
		/// Address of map, if fixed.
		/// </summary>
		public IntPtr MapAddr;

		/// <summary>
		/// Size of the data memory map.
		/// </summary>
		public mdb_size_t MapSize;

		/// <summary>
		/// ID of the last used page.
		/// </summary>
		public mdb_size_t LastPageNum;

		/// <summary>
		/// ID of the last committed transaction.
		/// </summary>
		public mdb_size_t LastTXNID;

		/// <summary>
		/// Max reader slots in the environment.
		/// </summary>
		public uint MaxReaders;

		/// <summary>
		/// Max reader slots used in the environment.
		/// </summary>
		public uint NumReaders;

	}

	public delegate void MDBAssertFunc([NativeType("MDB_env*")] IntPtr env, [MarshalAs(UnmanagedType.LPStr)] string msg);

	public delegate MDBResult MDBMsgFunc([MarshalAs(UnmanagedType.LPStr)] string msg, IntPtr ctx);

#nullable disable
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
		public delegate MDBResult PFN_mdb_put([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, in MDBVal key, ref MDBVal data, MDBWriteFlags flags);
		public delegate MDBResult PFN_mdb_del([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, in MDBVal key, [NativeType("MDB_val*")] IntPtr data);
		public delegate MDBResult PFN_mdb_cursor_open([NativeType("MDB_txn*")] IntPtr txn, MDB_dbi dbi, [NativeType("MDB_cursor**")] out IntPtr cursor);
		public delegate void PFN_mdb_cursor_close([NativeType("MDB_cursor*")] IntPtr cursor);
		public delegate MDBResult PFN_mdb_cursor_renew([NativeType("MDB_txn*")] IntPtr txn, [NativeType("MDB_cursor*")] IntPtr cursor);
		[return: NativeType("MDB_txn*")]
		public delegate IntPtr PFN_mdb_cursor_txn([NativeType("MDB_cursor*")] IntPtr cursor);
		public delegate MDB_dbi PFN_mdb_cursor_dbi([NativeType("MDB_cursor*")] IntPtr cursor);
		public delegate MDBResult PFN_mdb_cursor_get([NativeType("MDB_cursor*")] IntPtr cursor, ref MDBVal key, out MDBVal data, MDBCursorOp op);
		public delegate MDBResult PFN_mdb_cursor_put([NativeType("MDB_cursor*")] IntPtr cursor, in MDBVal key, [NativeType("MDB_val*")] IntPtr data, MDBWriteFlags flags);
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
#nullable restore

	public class MDB {

		public static readonly LibrarySpec Spec = new() { Name = "lmdb" };
		public static readonly Library Library = LibraryManager.Load(Spec);

		public static MDBFunctions Functions { get; } = new();

		static MDB() {
			Library.LoadFunctions(Functions);
		}

		public static int VerInt(int a, int b, int c) => (a << 24) | (b << 16) | c;

		/// <summary>
		/// Return LMDB library version information.
		/// </summary>
		/// <param name="major">The library major version number</param>
		/// <param name="minor">The library minor version number</param>
		/// <param name="patch">The library patch version number</param>
		/// <returns>The library version as a string</returns>
		public static string GetVersion(out int major, out int minor, out int patch) => MemoryUtil.GetASCII(Functions.mdb_version(out major, out minor, out patch))!;

		/// <summary>
		/// <para>Return a string describing a given error code.</para>
		/// <para>
		/// This function is a superset of the ANSI C X3.159-1989 (ANSI C) strerror(3) function. If the error code
		/// is greater than or equal to 0, then the string returned by the system function strerror(3) is returned.
		/// If the error code is less than 0, an error string corresponding to the LMDB library error is returned.
		/// </para>
		/// </summary>
		/// <param name="err">The error code</param>
		/// <returns>The description of the error</returns>
		public static string StrError(MDBResult err) => MemoryUtil.GetASCII(Functions.mdb_strerror(err))!;

	}

	public class MDBException : Exception {

		public MDBException(string err) : base(err) { }

		public MDBException(string str, MDBResult err) : base(str + ": " + MDB.StrError(err)) { }

	}

}

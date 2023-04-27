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

	public unsafe class MDBFunctions {

		[NativeType("const char* mdb_version(int* major, int* minor, int* patch)")]
		public delegate* unmanaged<out int, out int, out int, IntPtr> mdb_version;
		[NativeType("const char* mdb_strerror(int err)")]
		public delegate* unmanaged<MDBResult, IntPtr> mdb_strerror;

		[NativeType("int mdb_env_create(MDB_env** env)")]
		public delegate* unmanaged<out IntPtr, MDBResult> mdb_env_create;
		[NativeType("int mdb_env_open(MDB_env* env, const char* path, int flags, mdb_mode_t mode)")]
		public delegate* unmanaged<IntPtr, byte*, MDBEnvFlags, mdb_mode_t, MDBResult> mdb_env_open;
		[NativeType("int mdb_env_copy(MDB_env* env, const char* path)")]
		public delegate* unmanaged<IntPtr, byte*, MDBResult> mdb_env_copy;
		[ExternFunction(AltNames = new string[] { "mdb_env_copyfd" }, Platform = Core.PlatformType.Windows)]
		[NativeType("int mdb_env_copyfd(MDB_env* env, mdb_filehandle_t fd)")]
		public delegate* unmanaged<IntPtr, IntPtr, MDBResult> mdb_env_copyfd_WIN32;
		[ExternFunction(AltNames = new string[] { "mdb_env_copyfd" }, Platform = Core.PlatformType.Linux)]
		[NativeType("int mdb_env_copyfd(MDB_env* env, mdb_filehandle_t fd)")]
		public delegate* unmanaged<IntPtr, int, MDBResult> mdb_env_copyfd_POSIX;
		[NativeType("int mdb_env_copy2(MDB_env* env, const char* path, int flags)")]
		public delegate* unmanaged<IntPtr, byte*, MDBCopyFlags, MDBResult> mdb_env_copy2;
		[ExternFunction(AltNames = new string[] { "mdb_env_copyfd2" }, Platform = Core.PlatformType.Windows)]
		[NativeType("int mdb_env_copyfd2(MDB_env* env, mdb_filehandle_t fd, int flags)")]
		public delegate* unmanaged<IntPtr, IntPtr, MDBEnvFlags, MDBResult> mdb_env_copyfd2_WIN32;
		[ExternFunction(AltNames = new string[] { "mdb_env_copyfd2" }, Platform = Core.PlatformType.Linux)]
		[NativeType("int mdb_env_copyfd2(MDB_env* env, mdb_filehandle_t fd, int flags)")]
		public delegate* unmanaged<IntPtr, int, MDBEnvFlags, MDBResult> mdb_env_copyfd2_POSIX;
		[NativeType("int mdb_env_stat(MDB_env* env, MDB_stat* stat)")]
		public delegate* unmanaged<IntPtr, out MDBStat, MDBResult> mdb_env_stat;
		[NativeType("int mdb_env_info(MDB_env* env, MDB_envinfo* stat)")]
		public delegate* unmanaged<IntPtr, out MDBEnvInfo, MDBResult> mdb_env_info;
		[NativeType("int mdb_env_sync(MDB_env* env, int force)")]
		public delegate* unmanaged<IntPtr, bool, MDBResult> mdb_env_sync;
		[NativeType("int mdb_env_close(MDB_env* env)")]
		public delegate* unmanaged<IntPtr, MDBResult> mdb_env_close;
		[NativeType("int mdb_env_set_flags(MDB_env* env, int flags, int onoff)")]
		public delegate* unmanaged<IntPtr, MDBEnvFlags, bool, MDBResult> mdb_env_set_flags;
		[NativeType("int mdb_env_get_flags(MDB_env* env, int* flags)")]
		public delegate* unmanaged<IntPtr, out MDBEnvFlags, MDBResult> mdb_env_get_flags;
		[NativeType("int mdb_env_get_path(MDB_env* env, const char** path)")]
		public delegate* unmanaged<IntPtr, out IntPtr, MDBResult> mdb_env_get_path;
		[ExternFunction(AltNames = new string[] { "mdb_env_get_fd" }, Platform = Core.PlatformType.Windows)]
		[NativeType("int mdb_env_get_fd(MDB_env* env, mdb_filehandle_t* fd)")]
		public delegate* unmanaged<IntPtr, out IntPtr, MDBResult> mdb_enf_get_fd_WIN32;
		[ExternFunction(AltNames = new string[] { "mdb_env_get_fd" }, Platform = Core.PlatformType.Windows)]
		[NativeType("int mdb_env_get_fd(MDB_env* env, mdb_filehandle_t* fd)")]
		public delegate* unmanaged<IntPtr, out int, MDBResult> mdb_enf_get_fd_POSIX;
		[NativeType("int mdb_env_set_mapsize(MDB_env* env, mdb_size_t size)")]
		public delegate* unmanaged<IntPtr, mdb_size_t, MDBResult> mdb_env_set_mapsize;
		[NativeType("int mdb_env_set_maxreaders(MDB_env* env, unsigned int readers)")]
		public delegate* unmanaged<IntPtr, uint, MDBResult> mdb_env_set_maxreaders;
		[NativeType("int mdb_env_get_maxreaders(MDB_env* env, unsigned int* readers)")]
		public delegate* unmanaged<IntPtr, out uint, MDBResult> mdb_env_get_maxreaders;
		[NativeType("int mdb_env_set_maxdbs(MDB_env* env, MDB_dbi dbs)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, MDBResult> mdb_env_set_maxdbs;
		[NativeType("int mdb_env_get_maxkeysize(MDB_env* env)")]
		public delegate* unmanaged<IntPtr, int> mdb_env_get_maxkeysize;
		[NativeType("int mdb_env_set_userctx(MDB_env* env, void* ctx)")]
		public delegate* unmanaged<IntPtr, IntPtr, MDBResult> mdb_env_set_userctx;
		[NativeType("void* mdb_env_get_userctx(MDB_env* env)")]
		public delegate* unmanaged<IntPtr, IntPtr> mdb_env_get_userctx;
		[NativeType("int mdb_env_set_assert(MDB_env* env, MDB_assert_func func)")]
		public delegate* unmanaged<IntPtr, IntPtr, MDBResult> mdb_env_set_assert;

		[NativeType("int mdb_txn_begin(MDB_env* env, MDB_txn* parent, int flags, MDB_txn** txn)")]
		public delegate* unmanaged<IntPtr, IntPtr, MDBEnvFlags, out IntPtr, MDBResult> mdb_txn_begin;
		[NativeType("MDB_env* mdb_txn_env(MDB_txn* txn)")]
		public delegate* unmanaged<IntPtr, IntPtr> mdb_txn_env;
		[NativeType("mdb_size_t mdb_txn_id(MDB_txn* txn)")]
		public delegate* unmanaged<IntPtr, mdb_size_t> mdb_txn_id;
		[NativeType("int mdb_txn_commit(MDB_txn* txn)")]
		public delegate* unmanaged<IntPtr, MDBResult> mdb_txn_commit;
		[NativeType("int mdb_txn_abort(MDB_txn* txn)")]
		public delegate* unmanaged<IntPtr, MDBResult> mdb_txn_abort;
		[NativeType("int mdb_txn_reset(MDB_txn* txn)")]
		public delegate* unmanaged<IntPtr, MDBResult> mdb_txn_reset;
		[NativeType("int mdb_txn_renew(MDB_txn* txn)")]
		public delegate* unmanaged<IntPtr, MDBResult> mdb_txn_renew;

		[NativeType("int mdb_dbi_open(MDB_txn* txn, const char* name, int flags, MDB_dbi* dbi)")]
		public delegate* unmanaged<IntPtr, byte*, MDBDBFlags, out MDB_dbi, MDBResult> mdb_dbi_open;
		[NativeType("int mdb_stat(MDB_txn* txn, MDB_dbi dbi, MDB_stat* stat)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, out MDBStat, MDBResult> mdb_stat;
		[NativeType("int mdb_dbi_flags(MDB_txn* txn, MDB_dbi dbi, int* flags)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, out MDBDBFlags, MDBResult> mdb_dbi_flags;
		[NativeType("int mdb_dbi_close(MDB_txn* txn, MDB_dbi dbi)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, MDBResult> mdb_dbi_close;
		[NativeType("int mdb_drop(MDB_txn* txn, MDB_dbi dbi, int del)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, bool, MDBResult> mdb_drop;
		[NativeType("int mdb_set_compare(MDB_txn* txn, MDB_dbi dbi, MDB_cmp_func cmp)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, IntPtr, MDBResult> mdb_set_compare;
		[NativeType("int mdb_set_dupsort(MDB_txn* txn, MDB_dbi dbi, MDB_cmp_func cmp)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, IntPtr, MDBResult> mdb_set_dupsort;
		[NativeType("int mdb_set_relfunc(MDB_txn* txn, MDB_dbi dbi, MDB_rel_func rel)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, IntPtr, MDBResult> mdb_set_relfunc;
		[NativeType("int mdb_set_relctx(MDB_txn* txn, MDB_dbi dbi, void* ctx)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, IntPtr, MDBResult> mdb_set_relctx;

		[NativeType("int mdb_get(MDB_txn* txn, MDB_dbi dbi, const MDB_val* key, MDB_val* data)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, in MDBVal, out MDBVal, MDBResult> mdb_get;
		[NativeType("int mdb_put(MDB_txn* txn, MDB_dbi dbi, const MDB_val* key, MDB_val* data, int flags)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, in MDBVal, ref MDBVal, MDBWriteFlags, MDBResult> mdb_put;
		[NativeType("int mdb_del(MDB_txn* txn, MDB_dbi dbi, const MDB_val* key, MDB_val* data)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, in MDBVal, MDBVal*, MDBResult> mdb_del;
		[NativeType("int mdb_cursor_open(MDB_txn* txn, MDB_dbi dbi, MDB_cursor** cursor)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, out IntPtr, MDBResult> mdb_cursor_open;
		[NativeType("void mdb_cursor_close(MDB_cursor* cursor)")]
		public delegate* unmanaged<IntPtr, void> mdb_cursor_close;
		[NativeType("int mdb_cursor_renew(MDB_txn* txn, MDB_cursor* cursor)")]
		public delegate* unmanaged<IntPtr, IntPtr, MDBResult> mdb_cursor_renew;
		[NativeType("MDB_txn* mdb_cursor_txn(MDB_cursor* cursor)")]
		public delegate* unmanaged<IntPtr, IntPtr> mdb_cursor_txn;
		[NativeType("MDB_dbi mdb_cursor_dbi(MDB_cursor* cursor)")]
		public delegate* unmanaged<IntPtr, MDB_dbi> mdb_cursor_dbi;
		[NativeType("int mdb_cursor_get(MDB_cursor* cursor, MDB_val* key, MDB_val* data, int op)")]
		public delegate* unmanaged<IntPtr, ref MDBVal, out MDBVal, MDBCursorOp, MDBResult> mdb_cursor_get;
		[NativeType("int mdb_cursor_put(MDB_cursor* cursor, const MDB_val* key, MDB_val* data, int flags)")]
		public delegate* unmanaged<IntPtr, in MDBVal, MDBVal*, MDBWriteFlags, MDBResult> mdb_cursor_put;
		[NativeType("int mdb_cursor_del(MDB_cursor* cursor, int flags)")]
		public delegate* unmanaged<IntPtr, MDBWriteFlags, MDBResult> mdb_cursor_del;
		[NativeType("int mdb_cursor_count(MDB_cursor* cursor, mdb_size_t* countp)")]
		public delegate* unmanaged<IntPtr, out mdb_size_t, MDBResult> mdb_cursor_count;
		[NativeType("int mdb_cmp(MDB_txn* txn, MDB_dbi dbi, const MDB_val* a, const MDB_val* b)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, in MDBVal, in MDBVal, int> mdb_cmp;
		[NativeType("int mdb_dcmp(MDB_txn* txn, MDB_dbi dbi, const MDB_val* a, const MDB_val* b)")]
		public delegate* unmanaged<IntPtr, MDB_dbi, in MDBVal, in MDBVal, int> mdb_dcmp;
		[NativeType("int mdb_reader_list(MDB_env* env, MDB_msg_func func, void* ctx)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr, MDBResult> mdb_reader_list;
		[NativeType("int mdb_reader_check(MDB_env* env, int* dead)")]
		public delegate* unmanaged<IntPtr, out int, MDBResult> mdb_reader_check;

	}

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
		public static string GetVersion(out int major, out int minor, out int patch) {
			unsafe {
				return MemoryUtil.GetASCII(Functions.mdb_version(out major, out minor, out patch))!;
			}
		}

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
		public static string StrError(MDBResult err) {
			unsafe {
				return MemoryUtil.GetASCII(Functions.mdb_strerror(err))!;
			}
		}
	}

	public class MDBException : Exception {

		public MDBException(string err) : base(err) { }

		public MDBException(string str, MDBResult err) : base(str + ": " + MDB.StrError(err)) { }

	}

}

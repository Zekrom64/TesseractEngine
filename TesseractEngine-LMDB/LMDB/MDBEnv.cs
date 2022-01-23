using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LMDB {

	public class MDBEnv : IDisposable {

		[NativeType("MDB_env*")]
		public IntPtr Env { get; private set; }

		/// <summary>
		/// Statistics about the LMDB environment.
		/// </summary>
		public MDBStat Stat {
			get {
				MDBResult err = MDB.Functions.mdb_env_stat(Env, out MDBStat stat);
				if (err != MDBResult.Success) throw new MDBException("Failed to get environment statistics", err);
				return stat;
			}
		}

		/// <summary>
		/// Information about the LMDB environment.
		/// </summary>
		public MDBEnvInfo Info {
			get {
				MDBResult err = MDB.Functions.mdb_env_info(Env, out MDBEnvInfo info);
				if (err != MDBResult.Success) throw new MDBException("Failed to get environment info", err);
				return info;
			}
		}

		/// <summary>
		/// Environment flags.
		/// </summary>
		public MDBEnvFlags Flags {
			get {
				MDBResult err = MDB.Functions.mdb_env_get_flags(Env, out MDBEnvFlags flags);
				if (err != MDBResult.Success) throw new MDBException("Failed to get environment flags", err);
				return flags;
			}
		}

		/// <summary>
		/// The path that was used in <see cref="Open(string, MDBEnvFlags, int)"/>.
		/// </summary>
		public string? Path {
			get {
				MDBResult err = MDB.Functions.mdb_env_get_path(Env, out IntPtr path);
				if (err != MDBResult.Success) throw new MDBException("Failed to get environment path", err);
				return MemoryUtil.GetASCII(path);
			}
		}

		/// <summary>
		/// <para>The size of the memory map to use for this environment.</para>
		/// <para>
		/// The size should be a multiple of the OS page size. The default is 10485760 bytes.
		/// The size of the memory map is also the maximum size of the database. The value
		/// should be chosen as large as possible, to accommodate future growth of the database.
		/// This function should be called before <see cref="Open(string, MDBEnvFlags, int)"/>.
		/// It may be called at later times if no transactions are active in this process.
		/// Note that the library does not check for this condition, the caller must ensure
		/// it explicitly.
		/// </para>
		/// <para>
		/// The new size takes effect immediately for the current process but will not be persisted
		/// to any others until a write transaction has been committed by the current process. Also,
		/// only mapsize increases are persisted into the environment.
		/// </para>
		/// <para>
		/// If the mapsize is increased by another process, and data has grown beyond the range of the
		/// current mapsize, <see cref="Begin(MDBTxn?, MDBEnvFlags)"/> will return MDB_MAP_RESIZED. This
		/// function may be called with a size of zero to adopt the new size.
		/// </para>
		/// <para>
		/// Any attempt to set a size smaller than the space already consumed by the environment will be
		/// silently changed to the current size of the used space.
		/// </para>
		/// </summary>
		public nuint MapSize {
			set {
				MDBResult err = MDB.Functions.mdb_env_set_mapsize(Env, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set environment map size", err);
			}
		}

		/// <summary>
		/// <para>The maximum number of threads/reader slots for the environment.</para>
		/// <para>
		/// This defines the number of slots in the lock table that is used to track readers in the
		/// environment. The default is 126. Starting a read-only transaction normally ties a lock table
		/// slot to the current thread until the environment closes or the thread exits. If <see cref="MDBEnvFlags.NoTLS"/>
		/// is in use, <see cref="Begin(MDBTxn?, MDBEnvFlags)"/> instead ties the slot to the <see cref="MDBTxn"/> object 
		/// until it or the <see cref="MDBEnv"/> object is destroyed. This function may only be called before <see cref="Open(string, MDBEnvFlags, int)"/>.
		/// </para>
		/// </summary>
		public uint MaxReaders {
			get {
				MDBResult err = MDB.Functions.mdb_env_get_maxreaders(Env, out uint readers);
				if (err != MDBResult.Success) throw new MDBException("Failed to get environment max reader count", err);
				return readers;
			}
			set {
				MDBResult err = MDB.Functions.mdb_env_set_maxreaders(Env, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set environment max reader count", err);
			}
		}

		/// <summary>
		/// <para>The maximum number of named databases for the environment.</para>
		/// <para>
		/// This function is only needed if multiple databases will be used in the environment.
		/// Simpler applications that use the environment as a single unnamed database can ignore
		/// this option. This function may only be called before <see cref="Open(string, MDBEnvFlags, int)"/>.
		/// </para>
		/// <para>
		/// Currently a moderate number of slots are cheap but a huge number gets expensive: 7-120
		/// words per transaction, and every <see cref="MDBTxn.Open(string, MDBDBFlags)"/> does a linear
		/// search of the opened slots.
		/// </para>
		/// </summary>
		public uint MaxDBs {
			set {
				MDBResult err = MDB.Functions.mdb_env_set_maxdbs(Env, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set environment max DB count", err);
			}
		}

		/// <summary>
		/// <para>Get the maximum size of keys and <see cref="MDBDBFlags.DupSort"/> data we can write.</para>
		/// <para></para>
		/// </summary>
		public int MaxKeySize => MDB.Functions.mdb_env_get_maxkeysize(Env);

		/// <summary>
		/// <para>Application information associated with the <see cref="MDBEnv"/>.</para>
		/// </summary>
		public IntPtr UserContext {
			set {
				MDBResult err = MDB.Functions.mdb_env_set_userctx(Env, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set environment user context", err);
			}
			get => MDB.Functions.mdb_env_get_userctx(Env);
		}

		/// <summary>
		/// The assert() callback of the environment. Disabled if liblmdb is built with NDEBUG.
		/// </summary>
		public MDBAssertFunc Assert {
			set {
				MDBResult err = MDB.Functions.mdb_env_set_assert(Env, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set environment assert function", err);
			}
		}

		/// <summary>
		/// <para>Create an LMDB environment handle.</para>
		/// </summary>
		/// <exception cref="MDBException">If the LMDB environment could not be created</exception>
		public MDBEnv() {
			MDBResult err = MDB.Functions.mdb_env_create(out IntPtr env);
			if (err != MDBResult.Success) throw new MDBException("Failed to create environment", err);
			Env = env;
		}

		~MDBEnv() {
			Dispose();
		}

		/// <summary>
		/// <para>Open an environment handle.</para>
		/// <para>If the function fails, <see cref="Dispose"/> must be called to discard the environment.</para>
		/// </summary>
		/// <param name="path">The directory in which the database files reside. This directory must already exist and be writable.</param>
		/// <param name="flags">Special options for this environment.</param>
		/// <param name="mode">The UNIX permissions to set on created files and semaphores. This parameter is ignored on Windows.</param>
		/// <exception cref="MDBException">If the database could not be opened</exception>
		public void Open(string path, MDBEnvFlags flags, int mode = 0b110110000 /* -rw-rw---- */) {
			MDBResult err = MDB.Functions.mdb_env_open(Env, path, flags, mode);
			if (err != MDBResult.Success) throw new MDBException("Failed to open environment", err);
		}

		/// <summary>
		/// <para>Copy an LMDB environment to the specified path.</para>
		/// <para>This function may be used to make a backup of an existing environment. No lockfile is created, since it gets recreated at need.</para>
		/// <para>
		/// Note: This call can trigger significant file size growth if run in parallel with write transactions,
		/// because it employs a read-only transaction. See long-lived transactions under
		/// <see href="http://www.lmdb.tech/doc/index.html#caveats_sec">Caveats</see>.
		/// </para>
		/// </summary>
		/// <param name="path">The directory in which the copy will reside. This directory must already exist and be writable but must otherwise be empty.</param>
		/// <exception cref="MDBException">If an error occurs copying this database</exception>
		public void CopyTo(string path) {
			MDBResult err = MDB.Functions.mdb_env_copy(Env, path);
			if (err != MDBResult.Success) throw new MDBException("Failed to copy environment", err);
		}

		/// <summary>
		/// <para>Copy an LMDB environment to the specified path, with options.</para>
		/// <para>This function may be used to make a backup of an existing environment. No lockfile is created, since it gets recreated at need.</para>
		/// <para>
		/// Note: This call can trigger significant file size growth if run in parallel with write transactions,
		/// because it employs a read-only transaction. See long-lived transactions under
		/// <see href="http://www.lmdb.tech/doc/index.html#caveats_sec">Caveats</see>.
		/// </para>
		/// </summary>
		/// <param name="path">The directory in which the copy will reside. This directory must already exist and be writable but must otherwise be empty.</param>
		/// <param name="flags">Special options for this operation</param>
		/// <exception cref="MDBException">If an error occurs copying this database</exception>
		public void CopyTo(string path, MDBCopyFlags flags) {
			MDBResult err = MDB.Functions.mdb_env_copy2(Env, path, flags);
			if (err != MDBResult.Success) throw new MDBException("Failed to copy environment", err);
		}

		/// <summary>
		/// <para>Flush the data buffers to disk.</para>
		/// <para>
		/// Data is always written to disk when <see cref="MDBTxn.Commit"/> is called, but the operating system may
		/// keep it buffered. LMDB always flushes the OS buffers upon commit as well, unless the environment was
		/// opened with <see cref="MDBEnvFlags.NoSync"/> or in part <see cref="MDBEnvFlags.NoMetaSync"/>. This call
		/// is not valid if the environment was opened with <see cref="MDBEnvFlags.ReadOnly"/>.
		/// </para>
		/// </summary>
		/// <param name="force"></param>
		/// <exception cref="MDBException"></exception>
		public void Sync(bool force = false) {
			MDBResult err = MDB.Functions.mdb_env_sync(Env, force);
			if (err != MDBResult.Success) throw new MDBException("Failed to sync environment", err);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Env != IntPtr.Zero) {
				MDB.Functions.mdb_env_close(Env);
				Env = IntPtr.Zero;
			}
		}

		/// <summary>
		/// <para>Set environment flags.</para>
		/// <para>
		/// This may be used to set some flags in addition to those from <see cref="Open(string, MDBEnvFlags, int)"/>,
		/// or to unset these flags. If several threads change the flags at the same time, the result is undefined.
		/// </para>
		/// </summary>
		/// <param name="flags">The flags to change, bitwise OR'd together</param>
		/// <param name="onoff">The value to set the flag to</param>
		/// <exception cref="MDBException">If an error occurs setting the flags</exception>
		public void SetFlags(MDBEnvFlags flags, bool onoff) {
			MDBResult err = MDB.Functions.mdb_env_set_flags(Env, flags, onoff);
			if (err != MDBResult.Success) throw new MDBException("Failed to set environment flags", err);
		}

		/// <summary>
		/// <para>Create a transaction for use with the environment.</para>
		/// <para>The transaction handle may be discarded using <see cref="MDBTxn.Abort"/> or <see cref="MDBTxn.Commit"/>.</para>
		/// <para>
		/// Note: A transaction and its cursors must only be used by a single thread, and a
		/// thread may only have a single transaction at a time. If <see cref="MDBEnvFlags.NoTLS"/>
		/// is in use, this does not apply to read-only transactions. Cursors may not span transactions.
		/// </para>
		/// </summary>
		/// <param name="parent">
		/// If this parameter is non-null, the new transaction will be a nested transaction, with the transaction
		/// indicated by <i>parent</i> as its parent. Transactions may be nested to any level. A parent transaction and
		/// its cursors may not issue any other operations than <see cref="MDBTxn.Commit"/> and <see cref="MDBTxn.Abort"/>
		/// while it has active child transactions.
		/// </param>
		/// <param name="flags">Special options for this transaction, only <see cref="MDBEnvFlags.ReadOnly"/> is supported now</param>
		/// <returns>A new transaction</returns>
		/// <exception cref="MDBException">If an error occurs starting the transaction</exception>
		public MDBTxn Begin(MDBTxn? parent = null, MDBEnvFlags flags = 0) {
			MDBResult err = MDB.Functions.mdb_txn_begin(Env, parent != null ? parent.Txn : IntPtr.Zero, flags, out IntPtr txn);
			if (err != MDBResult.Success) throw new MDBException("Failed to begin transaction", err);
			return new MDBTxn(txn, this);
		}

		/// <summary>
		/// Dump the entries in the reader lock table.
		/// </summary>
		/// <param name="func">A <see cref="MDBMsgFunc"/> function</param>
		/// <param name="ctx">Anything the message function needs</param>
		/// <exception cref="MDBException">If an error occurs listing the readers</exception>
		public void ListReaders(MDBMsgFunc func, IntPtr ctx = default) {
			MDBResult err = MDB.Functions.mdb_reader_list(Env, func, ctx);
			if (err != MDBResult.Success) throw new MDBException("Failed to enumerate reader list", err);
		}

		/// <summary>
		/// Check for stale entries in the reader lock table.
		/// </summary>
		/// <returns>The number of stale slots that were cleared</returns>
		/// <exception cref="MDBException">If an error occurs checking the readers</exception>
		public int CheckReaders() {
			MDBResult err = MDB.Functions.mdb_reader_check(Env, out int dead);
			if (err != MDBResult.Success) throw new MDBException("Failed to check for stale readers", err);
			return dead;
		}

		public static implicit operator IntPtr(MDBEnv env) => env.Env;

	}

}

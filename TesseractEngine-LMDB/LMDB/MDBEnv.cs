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

		public MDBStat Stat {
			get {
				MDBResult err = MDB.Functions.mdb_env_stat(Env, out MDBStat stat);
				if (err != MDBResult.Success) throw new MDBException("Failed to get environment statistics", err);
				return stat;
			}
		}

		public MDBEnvInfo Info {
			get {
				MDBResult err = MDB.Functions.mdb_env_info(Env, out MDBEnvInfo info);
				if (err != MDBResult.Success) throw new MDBException("Failed to get environment info", err);
				return info;
			}
		}

		public MDBEnvFlags Flags {
			get {
				MDBResult err = MDB.Functions.mdb_env_get_flags(Env, out MDBEnvFlags flags);
				if (err != MDBResult.Success) throw new MDBException("Failed to get environment flags", err);
				return flags;
			}
		}

		public string Path {
			get {
				MDBResult err = MDB.Functions.mdb_env_get_path(Env, out IntPtr path);
				if (err != MDBResult.Success) throw new MDBException("Failed to get environment path", err);
				return MemoryUtil.GetASCII(path);
			}
		}

		public nuint MapSize {
			set {
				MDBResult err = MDB.Functions.mdb_env_set_mapsize(Env, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set environment map size", err);
			}
		}

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

		public uint MaxDBs {
			set {
				MDBResult err = MDB.Functions.mdb_env_set_maxdbs(Env, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set environment max DB count", err);
			}
		}

		public int MaxKeySize => MDB.Functions.mdb_env_get_maxkeysize(Env);

		public IntPtr UserContext {
			set {
				MDBResult err = MDB.Functions.mdb_env_set_userctx(Env, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set environment user context", err);
			}
			get => MDB.Functions.mdb_env_get_userctx(Env);
		}

		public MDBAssertFunc Assert {
			set {
				MDBResult err = MDB.Functions.mdb_env_set_assert(Env, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set environment assert function", err);
			}
		}

		public MDBEnv() {
			MDBResult err = MDB.Functions.mdb_env_create(out IntPtr env);
			if (err != MDBResult.Success) throw new MDBException("Failed to create environment", err);
			Env = env;
		}

		~MDBEnv() {
			Dispose();
		}

		public void Open(string path, MDBEnvFlags flags, int mode = 0b110110000 /* -rw-rw---- */) {
			MDBResult err = MDB.Functions.mdb_env_open(Env, path, flags, mode);
			if (err != MDBResult.Success) throw new MDBException("Failed to open environment", err);
		}

		public void CopyTo(string path) {
			MDBResult err = MDB.Functions.mdb_env_copy(Env, path);
			if (err != MDBResult.Success) throw new MDBException("Failed to copy environment", err);
		}

		public void CopyTo(string path, MDBCopyFlags flags) {
			MDBResult err = MDB.Functions.mdb_env_copy2(Env, path, flags);
			if (err != MDBResult.Success) throw new MDBException("Failed to copy environment", err);
		}

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

		public void SetFlags(MDBEnvFlags flags, bool onoff) {
			MDBResult err = MDB.Functions.mdb_env_set_flags(Env, flags, onoff);
			if (err != MDBResult.Success) throw new MDBException("Failed to set environment flags", err);
		}

		public MDBTxn Begin(MDBTxn parent, MDBEnvFlags flags) {
			MDBResult err = MDB.Functions.mdb_txn_begin(Env, parent != null ? parent.Txn : IntPtr.Zero, flags, out IntPtr txn);
			if (err != MDBResult.Success) throw new MDBException("Failed to begin transaction", err);
			return new MDBTxn(txn, this);
		}

		public void ListReaders(MDBMsgFunc func, IntPtr ctx = default) {
			MDBResult err = MDB.Functions.mdb_reader_list(Env, func, ctx);
			if (err != MDBResult.Success) throw new MDBException("Failed to enumerate reader list", err);
		}

		public int CheckReaders() {
			MDBResult err = MDB.Functions.mdb_reader_check(Env, out int dead);
			if (err != MDBResult.Success) throw new MDBException("Failed to check for stale readers", err);
			return dead;
		}

		public static implicit operator IntPtr(MDBEnv env) => env.Env;

	}

}

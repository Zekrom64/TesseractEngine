using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.LMDB {
	
	public readonly struct MDBDBI {

		public MDBTxn Txn { get; }

		public uint DBI { get; }

		public MDBStat Stat {
			get {
				MDBResult err = MDB.Functions.mdb_stat(Txn, DBI, out MDBStat stat);
				if (err != MDBResult.Success) throw new MDBException("Failed to database stats", err);
				return stat;
			}
		}

		public MDBDBFlags Flags {
			get {
				MDBResult err = MDB.Functions.mdb_dbi_flags(Txn, DBI, out MDBDBFlags flags);
				if (err != MDBResult.Success) throw new MDBException("Failed to get database flags", err);
				return flags;
			}
		}

		public MDBCmpFunc Compare {
			set {
				MDBResult err = MDB.Functions.mdb_set_compare(Txn, DBI, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set database compare function", err);
			}
		}

		public MDBCmpFunc DupSort {
			set {
				MDBResult err = MDB.Functions.mdb_set_dupsort(Txn, DBI, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set database dupsort function", err);
			}
		}

		public MDBRelFunc Relocate {
			set {
				MDBResult err = MDB.Functions.mdb_set_relfunc(Txn, DBI, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set database relocate function", err);
			}
		}

		public IntPtr RelocateContext {
			set {
				MDBResult err = MDB.Functions.mdb_set_relctx(Txn, DBI, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set database relocate function context", err);
			}
		}

		public MDBDBI(uint dbi, MDBTxn txn) {
			DBI = dbi;
			Txn = txn;
		}

		public void Close() => MDB.Functions.mdb_dbi_close(Txn, DBI);

		public void Drop(bool del = false) {
			MDBResult err = MDB.Functions.mdb_drop(Txn, DBI, del);
			if (err != MDBResult.Success) throw new MDBException("Failed to drop database", err);
		}

		public Span<byte> Get(in ReadOnlySpan<byte> key) {
			unsafe {
				fixed(byte* pKey = key) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBResult err = MDB.Functions.mdb_get(Txn, DBI, vkey, out MDBVal data);
					if (err != MDBResult.Success) throw new MDBException("Failed to get database entry", err);
					return new Span<byte>((void*)data.Data, (int)data.Size);
				}
			}
		}

		public void Put(in ReadOnlySpan<byte> key, in ReadOnlySpan<byte> data, MDBWriteFlags flags) {
			unsafe {
				fixed(byte* pKey = key, pData = data) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBVal vdata = new() {
						Size = (nuint)data.Length,
						Data = (IntPtr)pData
					};
					MDBResult err = MDB.Functions.mdb_put(Txn, DBI, vkey, vdata, flags);
					if (err != MDBResult.Success) throw new MDBException("Failed to put database entry", err);
				}
			}
		}

		public void Del(in ReadOnlySpan<byte> key) {
			unsafe {
				fixed (byte* pKey = key) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBResult err = MDB.Functions.mdb_del(Txn, DBI, vkey, IntPtr.Zero);
					if (err != MDBResult.Success) throw new MDBException("Failed to delete database entry", err);
				}
			}
		}

		public void Del(in ReadOnlySpan<byte> key, in ReadOnlySpan<byte> data) {
			unsafe {
				fixed (byte* pKey = key, pData = data) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBVal vdata = new() {
						Size = (nuint)data.Length,
						Data = (IntPtr)pData
					};
					MDBResult err = MDB.Functions.mdb_del(Txn, DBI, vkey, (IntPtr)(&vdata));
					if (err != MDBResult.Success) throw new MDBException("Failed to delete database entry", err);
				}
			}
		}

		public MDBCursor OpenCursor() {
			MDBResult err = MDB.Functions.mdb_cursor_open(Txn, DBI, out IntPtr cursor);
			if (err != MDBResult.Success) throw new MDBException("Failed to open database cursor", err);
			return new MDBCursor(this, cursor);
		}

		public int Cmp(in MDBVal a, in MDBVal b) => MDB.Functions.mdb_cmp(Txn, DBI, a, b);

		public int DCmp(in MDBVal a, in MDBVal b) => MDB.Functions.mdb_cmp(Txn, DBI, a, b);

	}
}

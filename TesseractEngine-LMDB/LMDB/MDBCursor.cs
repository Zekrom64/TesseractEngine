using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LMDB {

	public class MDBCursor : IDisposable {

		public MDBDBI DBI { get; }

		[NativeType("MDB_cursor*")]
		public IntPtr Cursor { get; private set; }

		public nuint DuplicateCount {
			get {
				MDBResult err = MDB.Functions.mdb_cursor_count(Cursor, out nuint countp);
				if (err != MDBResult.Success) throw new MDBException("Failed to get cursor duplicate count", err);
				return countp;
			}
		}

		public MDBCursor(MDBDBI dbi, IntPtr cursor) {
			DBI = dbi;
			Cursor = cursor;
		}

		~MDBCursor() {
			Dispose();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Cursor != IntPtr.Zero) {
				MDB.Functions.mdb_cursor_close(Cursor);
				Cursor = IntPtr.Zero;
			}
		}

		public void Renew() {
			MDBResult err = MDB.Functions.mdb_cursor_renew(DBI.Txn, Cursor);
			if (err != MDBResult.Success) throw new MDBException("Failed to renew cursor", err);
		}

		public void Get(ref Span<byte> key, out Span<byte> data, MDBCursorOp op) {
			unsafe {
				fixed(byte* pKey = key) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBResult err = MDB.Functions.mdb_cursor_get(Cursor, ref vkey, out MDBVal vdata, op);
					if (err != MDBResult.Success) throw new MDBException("Failed to get from cursor", err);
					if (vkey.Data != (IntPtr)pKey)
						key = new Span<byte>((void*)vkey.Data, (int)vkey.Size);
					data = new Span<byte>((void*)vdata.Data, (int)vdata.Size);
				}
			}
		}

		public void Put(in ReadOnlySpan<byte> key, in ReadOnlySpan<byte> data, MDBWriteFlags flags) {
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
					MDBResult err = MDB.Functions.mdb_cursor_put(Cursor, vkey, vdata, flags);
					if (err != MDBResult.Success) throw new MDBException("Failed to put at cursor", err);
				}
			}
		}

		public void Del(MDBWriteFlags flags) {
			MDBResult err = MDB.Functions.mdb_cursor_del(Cursor, flags);
			if (err != MDBResult.Success) throw new MDBException("Failed to delete at cursor", err);
		}

	}

}

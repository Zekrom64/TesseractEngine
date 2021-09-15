using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LMDB {

	/// <summary>
	/// <para>
	/// Note: While this class implements the disposable pattern, disposal will abort the transaction by
	/// default as a fail-safe. Commiting must be done explicitly using the appropriate method before
	/// the disposal is performed.
	/// </para>
	/// </summary>
	public class MDBTxn : IDisposable {

		public MDBEnv Env { get; }

		[NativeType("MDB_txn*")]
		public IntPtr Txn { get; private set; }

		public nuint ID => MDB.Functions.mdb_txn_id(Txn);

		public MDBTxn(IntPtr txn, MDBEnv env) {
			Txn = txn;
			Env = env;
		}

		~MDBTxn() {
			Dispose();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Txn != IntPtr.Zero) {
				MDB.Functions.mdb_txn_abort(Txn);
				Txn = IntPtr.Zero;
			}
		}

		public void Commit() {
			MDBResult err = MDB.Functions.mdb_txn_commit(Txn);
			if (err != MDBResult.Success) throw new MDBException("Failed to commit transaction", err);
			Txn = IntPtr.Zero;
		}

		public void Abort() {
			MDB.Functions.mdb_txn_abort(Txn);
			Txn = IntPtr.Zero;
		}

		public void Reset() => MDB.Functions.mdb_txn_reset(Txn);

		public void Renew() {
			MDBResult err = MDB.Functions.mdb_txn_renew(Txn);
			if (err != MDBResult.Success) throw new MDBException("Failed to renew transaction", err);
		}

		public static implicit operator IntPtr(MDBTxn txn) => txn.Txn;

	}

}

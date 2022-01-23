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

		/// <summary>
		/// The transaction's <see cref="MDBEnv"/>.
		/// </summary>
		public MDBEnv Env { get; }

		/// <summary>
		/// The underyling MDB_txn pointer.
		/// </summary>
		[NativeType("MDB_txn*")]
		public IntPtr Txn { get; private set; }

		/// <summary>
		/// The transaction's ID. For a read-only transaction, this corresponds to the snapshot being read; concurrent readers will frequently have the same transaction ID.
		/// </summary>
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

		/// <summary>
		/// <para>Open a database in the environment.</para>
		/// <para>
		/// A database handle denotes the name and parameters of a database, independently
		/// of whether such a database exists. The database handle may be discarded by calling
		/// <see cref="Dispose"/>. The old database handle is returned if the database was already
		/// open. The handle may only be closed once.
		/// </para>
		/// <para>
		/// The database handle will be private to the current transaction until the transaction is
		/// successfully committed. If the transaction is aborted the handle will be closed automatically.
		/// After a successful commit the handle will reside in the shared environment, and may be used by
		/// other transactions.
		/// </para>
		/// <para>
		/// This function must not be called from multiple concurrent transactions in the same process. A
		/// transaction that uses this function must finish (either commit or abort) before any other
		/// transaction in the process may use this function.
		/// </para>
		/// <para>
		/// To use named databases (with name != null), <see cref="MDBEnv.MaxDBs"/> must be set before opening
		/// the environment. Database names are keys in the unnamed database, and may be read but not written.
		/// </para>
		/// </summary>
		/// <param name="name">The name of the database to open. If only a single database is needed in the environment, this value may be null.</param>
		/// <param name="flags">Special options for this database. This parameter must be set to 0 or by bitwise OR'ing together one or more values.</param>
		/// <returns>The new <see cref="MDBDBI"/> handle</returns>
		/// <exception cref="MDBException">If an error occured opening the handle</exception>
		public MDBDBI Open(string? name, MDBDBFlags flags) {
			MDBResult err = MDB.Functions.mdb_dbi_open(Txn, name, flags, out uint dbi);
			if (err != MDBResult.Success) throw new MDBException("Failed to open database instance", err);
			return new MDBDBI(dbi, this);
		}

		/// <summary>
		/// <para>Commit all the operations of a transaction into the database.</para>
		/// <para>
		/// The transaction handle is freed. It and its cursors must not be used again after this call,
		/// except with <see cref="MDBCursor.Renew"/>.
		/// </para>
		/// <para>Note: Earlier documentation incorrectly said all cursors would be freed. Only write-transactions free cursors.</para>
		/// </summary>
		/// <exception cref="MDBException">If an error occured commiting the transaction</exception>
		public void Commit() {
			MDBResult err = MDB.Functions.mdb_txn_commit(Txn);
			if (err != MDBResult.Success) throw new MDBException("Failed to commit transaction", err);
			Txn = IntPtr.Zero;
		}

		/// <summary>
		/// <para>Abandon all the operations of the transaction instead of saving them.</para>
		/// <para>
		/// The transaction handle is freed. It and its cursors must not be used again after this call,
		/// except with <see cref="MDBCursor.Renew"/>.
		/// </para>
		/// <para>Note: Earlier documentation incorrectly said all cursors would be freed. Only write-transactions free cursors.</para>
		/// </summary>
		public void Abort() {
			MDB.Functions.mdb_txn_abort(Txn);
			Txn = IntPtr.Zero;
		}

		/// <summary>
		/// <para>Reset a read-only transaction.</para>
		/// <para>
		/// Abort the transaction like <see cref="Abort"/>, but keep the transaction handle. <see cref="Renew"/> may
		/// reuse the handle. This saves allocation overhead if the process will start a new read-only transaction soon,
		/// and also locking overhead if <see cref="MDBEnvFlags.NoTLS"/> is in use. The reader table lock is released,
		/// but the table slot stays tied to its thread or <see cref="MDBTxn"/>. <see cref="Abort"/> to discard a reset
		/// handle, and to free its lock table slot if <see cref="MDBEnvFlags.NoTLS"/> is in use. Cursors opened within
		/// the transaction must not be used again after this call, except with <see cref="MDBCursor.Renew"/>. Reader
		/// locks generally don't interfere with writers, but they keep old versions of database pages allocated. Thus
		/// they prevent the old pages from being reused when writers commit new data, and so under heavy load the
		/// database size may grow much more rapidly than otherwise.
		/// </para>
		/// </summary>
		public void Reset() => MDB.Functions.mdb_txn_reset(Txn);

		/// <summary>
		/// <para>Renew a read-only transaction.</para>
		/// <para>
		/// This acquires a new reader lock for a transaction handle that had been released by <see cref="Reset"/>. I
		/// must be called before a reset transaction may be used again.
		/// </para>
		/// </summary>
		/// <exception cref="MDBException">If an error occurs renewing the transaction</exception>
		public void Renew() {
			MDBResult err = MDB.Functions.mdb_txn_renew(Txn);
			if (err != MDBResult.Success) throw new MDBException("Failed to renew transaction", err);
		}

		public static implicit operator IntPtr(MDBTxn txn) => txn.Txn;

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LMDB {

	using MDBDBI = UInt32;

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
		public nuint ID {
			get {
				unsafe {
					return MDB.Functions.mdb_txn_id(Txn);
				}
			}
		}

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
				unsafe {
					MDB.Functions.mdb_txn_abort(Txn);
				}
				Txn = IntPtr.Zero;
			}
		}

		//=======================//
		// Transaction Functions //
		//=======================//

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
		/// <returns>The new database interface handle</returns>
		/// <exception cref="MDBException">If an error occured opening the handle</exception>
		public MDBDBI Open(string? name, MDBDBFlags flags) {
			unsafe {
				fixed(byte* pName = (name != null ? MemoryUtil.StackallocUTF8(name, stackalloc byte[256]) : stackalloc byte[] { 0 })) {
					MDBResult err = MDB.Functions.mdb_dbi_open(Txn, pName, flags, out MDBDBI dbi);
					if (err != MDBResult.Success) throw new MDBException("Failed to open database instance", err);
					return dbi;
				}
			}
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
			unsafe {
				MDBResult err = MDB.Functions.mdb_txn_commit(Txn);
				if (err != MDBResult.Success) throw new MDBException("Failed to commit transaction", err);
				Txn = IntPtr.Zero;
			}
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
			unsafe {
				MDB.Functions.mdb_txn_abort(Txn);
				Txn = IntPtr.Zero;
			}
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
		public void Reset() {
			unsafe {
				MDB.Functions.mdb_txn_reset(Txn);
			}
		}

		/// <summary>
		/// <para>Renew a read-only transaction.</para>
		/// <para>
		/// This acquires a new reader lock for a transaction handle that had been released by <see cref="Reset"/>. I
		/// must be called before a reset transaction may be used again.
		/// </para>
		/// </summary>
		/// <exception cref="MDBException">If an error occurs renewing the transaction</exception>
		public void Renew() {
			unsafe {
				MDBResult err = MDB.Functions.mdb_txn_renew(Txn);
				if (err != MDBResult.Success) throw new MDBException("Failed to renew transaction", err);
			}
		}

		//===============//
		// DBI Functions //
		//===============//

		/// <summary>
		/// Statistics for the database.
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		public MDBStat Stat(MDBDBI dbi) {
			unsafe {
				MDBResult err = MDB.Functions.mdb_stat(Txn, dbi, out MDBStat stat);
				if (err != MDBResult.Success) throw new MDBException("Failed to get database stats", err);
				return stat;
			}
		}

		/// <summary>
		/// Flags for the database.
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		public MDBDBFlags Flags(MDBDBI dbi) {
			unsafe {
				MDBResult err = MDB.Functions.mdb_dbi_flags(Txn, dbi, out MDBDBFlags flags);
				if (err != MDBResult.Success) throw new MDBException("Failed to get database flags", err);
				return flags;
			}
		}

		/// <summary>
		/// <para>A custom key comparison function for a database.</para>
		/// <para>
		/// The comparison function is called whenever it is necessary to compare a key specified by
		/// the application with a key currently stored in the database. If no comparison function is
		/// specified, and no special key flags were specified with <see cref="MDBTxn.Open(string?, MDBDBFlags)"/>,
		/// the keys are compared lexically, with shorter keys collating before longer keys.
		/// </para>
		/// <para>
		/// Warning: This function must be called before any data access functions are used, otherwise data
		/// corruption may occur. The same comparison function must be used by every program accessing the database,
		/// every time the database is used.
		/// </para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		public void SetCompare(MDBDBI dbi, MDBCmpFunc fn) {
			unsafe {
				MDBResult err = MDB.Functions.mdb_set_compare(Txn, dbi, Marshal.GetFunctionPointerForDelegate(fn));
				if (err != MDBResult.Success) throw new MDBException("Failed to set database compare function", err);
			}
		}

		/// <summary>
		/// <para>A custom data comparison function for a <see cref="MDBDBFlags.DupSort"/> database.</para>
		/// <para>
		/// This comparison function is called whenever it is necessary to compare a data item specified by
		/// the application with a data item currently stored in the database. This function only takes effect
		/// if the database was opened with the <see cref="MDBDBFlags.DupSort"/> flag. If no comparison function
		/// is specified, and no special key flags were specified with <see cref="MDBTxn.Open(string?, MDBDBFlags)"/>,
		/// the data items are compared lexically, with shorter items collating before longer items.
		/// </para>
		/// <para>
		/// Warning: This function must be called before any data access functions are used, otherwise data
		/// corruption may occur. The same comparison function must be used by every program accessing the database,
		/// every time the database is used.
		/// </para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		public void SetDupSort(MDBDBI dbi, MDBCmpFunc fn) {
			unsafe {
				MDBResult err = MDB.Functions.mdb_set_dupsort(Txn, dbi, Marshal.GetFunctionPointerForDelegate(fn));
				if (err != MDBResult.Success) throw new MDBException("Failed to set database dupsort function", err);
			}
		}

		/// <summary>
		/// <para>A relocation function for a <see cref="MDBEnvFlags.FixedMap"/> database.</para>
		/// <para>
		/// Todo: The relocation function is called whenever it is necessary to move the data of an item
		/// to a different position in the database (e.g. through tree balancing operations, shifts as a
		/// result of adds or deletes, etc.). It is intended to allow address/position-dependent data items
		/// to be stored in a database in an environment opened with the <see cref="MDBEnvFlags.FixedMap"/>
		/// option. Currently the relocation feature is unimplemented and setting this function has no effect.
		/// </para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		public void SetRelocateFunction(MDBDBI dbi, MDBRelFunc fn) {
			unsafe {
				MDBResult err = MDB.Functions.mdb_set_relfunc(Txn, dbi, Marshal.GetFunctionPointerForDelegate(fn));
				if (err != MDBResult.Success) throw new MDBException("Failed to set database relocate function", err);
			}
		}

		/// <summary>
		/// <para>A context pointer for a <see cref="MDBEnvFlags.FixedMap"/> database's relocation function.</para>
		/// <para>
		/// See <see cref="Relocate"/> and <see cref="MDBRelFunc"/> for more details.
		/// </para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		public void SetRelocateContext(MDBDBI dbi, IntPtr ptr) {
			unsafe {
				MDBResult err = MDB.Functions.mdb_set_relctx(Txn, dbi, ptr);
				if (err != MDBResult.Success) throw new MDBException("Failed to set database relocate function context", err);
			}
		}

		/// <summary>
		/// <para>Close a database handle. Normally unnecessary. Use with care:</para>
		/// <para>
		/// This call is not mutex protected. Handles should only be closed by a single thread,
		/// and only if no other threads are going to reference the database handle or one of its
		/// cursors any further. Do not close a handle if an existing transaction has modified its
		/// database. Doing so can cause misbehavior from database corruption to errors like MDB_BAD_VALSIZE
		/// (since the DB name is gone).
		/// </para>
		/// <para>
		/// Closing a database handle is not necessary, but lets <see cref="MDBTxn.Open(string?, MDBDBFlags)"/>
		/// reuse the handle value. Usually it's better to set a bigger <see cref="MDBEnv.MaxDBs"/>, unless
		/// that value would be large.
		/// </para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		public void Close(MDBDBI dbi) {
			unsafe {
				MDB.Functions.mdb_dbi_close(Txn, dbi);
			}
		}

		/// <summary>
		/// <para>Empty or delete+close a database.</para>
		/// <para>See <see cref="Close"/> for restrictions about closing the DB handle.</para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="del">If the database should be deleted from the environment or just emptied</param>
		/// <exception cref="MDBException">If an error occurs dropping the database</exception>
		public void Drop(MDBDBI dbi, bool del = false) {
			unsafe {
				MDBResult err = MDB.Functions.mdb_drop(Txn, dbi, del);
				if (err != MDBResult.Success) throw new MDBException("Failed to drop database", err);
			}
		}

		/// <summary>
		/// <para>Get items from a database.</para>
		/// <para>
		/// This function retrieves key/data pairs from the database. The address and length of the data
		/// associated with the specified key are returned in the structure to which data refers. If the database
		/// supports duplicate keys (<see cref="MDBDBFlags.DupSort"/>) then the first data item for the key will
		/// be returned. Retrieval of other items requires the use of <see cref="MDBCursor.Get(ref Span{byte}, out Span{byte}, MDBCursorOp)"/>.
		/// </para>
		/// <para>
		/// Note: The memory pointed to by the returned values is owned by the database. The caller need not
		/// dispose of the memory, and may not modify it in any way. For values returned in a read-only transaction
		/// any modification attempts will cause a SIGSEGV. Values returned from the database are valid only until a
		/// subsequent update operation, or the end of the transaction.
		/// </para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to search for in the database</param>
		/// <returns>The data corresponding to the key</returns>
		/// <exception cref="KeyNotFoundException">If the specified key was not found in the database</exception>
		/// <exception cref="MDBException">If an error occurs getting the data</exception>
		public Span<byte> Get(MDBDBI dbi, in ReadOnlySpan<byte> key) {
			unsafe {
				fixed (byte* pKey = key) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBResult err = MDB.Functions.mdb_get(Txn, dbi, vkey, out MDBVal data);
					if (err == MDBResult.NotFound) throw new KeyNotFoundException();
					if (err != MDBResult.Success) throw new MDBException("Failed to get database entry", err);
					return new Span<byte>((void*)data.Data, (int)data.Size);
				}
			}
		}

		/// <summary>
		/// Similar to <see cref="Get(in ReadOnlySpan{byte})"/>, but the return value will indicate
		/// if the entry was retrieved or not.
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to search for in the database</param>
		/// <param name="data">The data corresponding to the key</param>
		/// <returns>If the entry was retrieved for the given key</returns>
		/// <exception cref="MDBException">If an error occurs getting the data</exception>
		public bool TryGet(MDBDBI dbi, in ReadOnlySpan<byte> key, out Span<byte> data) {
			data = Span<byte>.Empty;
			unsafe {
				fixed (byte* pKey = key) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBResult err = MDB.Functions.mdb_get(Txn, dbi, vkey, out MDBVal data2);
					if (err == MDBResult.NotFound) return false;
					if (err != MDBResult.Success) throw new MDBException("Failed to get database entry", err);
					data = new Span<byte>((void*)data2.Data, (int)data2.Size);
					return true;
				}
			}
		}

		/// <summary>
		/// Similar to <see cref="Get(in ReadOnlySpan{byte})"/>, but uses the byte representation
		/// of a value as the key.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to search for in the database</param>
		/// <returns>The data corresponding to the key</returns>
		public Span<byte> Get<K>(MDBDBI dbi, K key) where K : unmanaged {
			unsafe {
				K* pKey = &key;
				MDBVal vkey = new() {
					Size = (nuint)Marshal.SizeOf<K>(),
					Data = (IntPtr)pKey
				};
				MDBResult err = MDB.Functions.mdb_get(Txn, dbi, vkey, out MDBVal data);
				if (err == MDBResult.NotFound) throw new KeyNotFoundException();
				if (err != MDBResult.Success) throw new MDBException("Failed to get database entry", err);
				return new Span<byte>((void*)data.Data, (int)data.Size);
			}
		}

		/// <summary>
		/// Similar to <see cref="TryGet(in ReadOnlySpan{byte}, out Span{byte})"/>, but uses the
		/// byte representation of a value as the key.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to search for in the database</param>
		/// <param name="data">The data corresponding to the key</param>
		/// <returns>If the entry was retrieved for the given key</returns>
		public bool TryGet<K>(MDBDBI dbi, K key, out Span<byte> data) where K : unmanaged {
			data = Span<byte>.Empty;
			unsafe {
				K* pKey = &key;
				MDBVal vkey = new() {
					Size = (nuint)Marshal.SizeOf<K>(),
					Data = (IntPtr)pKey
				};
				MDBResult err = MDB.Functions.mdb_get(Txn, dbi, vkey, out MDBVal data2);
				if (err == MDBResult.NotFound) return false;
				if (err != MDBResult.Success) throw new MDBException("Failed to get database entry", err);
				data = new Span<byte>((void*)data2.Data, (int)data2.Size);
				return true;
			}
		}

		/// <summary>
		/// <para>Store items into a database.</para>
		/// <para>
		/// This function stores key/data pairs in the database. The default behavior is to
		/// enter the new key/data pair, replacing any previously existing key if duplicates
		/// are disallowed, or adding a duplicate data item if duplicates are allowed (<see cref="MDBDBFlags.DupSort"/>).
		/// </para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to store in the database</param>
		/// <param name="data">The data to store</param>
		/// <param name="flags">Special options for this operation. This parameter must be set to 0 or by bitwise OR'ing together one or more values.</param>
		/// <returns>If the write operation succeeded</returns>
		/// <exception cref="MDBException">If an error occurs putting the item into the database</exception>
		public bool Put(MDBDBI dbi, in ReadOnlySpan<byte> key, ref Span<byte> data, MDBWriteFlags flags = 0) {
			if ((flags & MDBWriteFlags.Multiple) != 0) throw new MDBException("Invalid use of MDBWriteFlags.Multiple");
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
					MDBResult err = MDB.Functions.mdb_put(Txn, dbi, vkey, ref vdata, flags);
					if (err == MDBResult.KeyExist) return false;
					if (err != MDBResult.Success) throw new MDBException("Failed to put database entry", err);
					if ((flags & MDBWriteFlags.Reserve) != 0)
						data = new Span<byte>((void*)vdata.Data, (int)vdata.Size);
					return true;
				}
			}
		}

		/// <summary>
		/// Similar to <see cref="Put(in ReadOnlySpan{byte}, ref Span{byte}, MDBWriteFlags)"/>, but uses
		/// the byte representation of a value for the key.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to store in the database</param>
		/// <param name="data">The data to store</param>
		/// <param name="flags">Special options for this operation. This parameter must be set to 0 or by bitwise OR'ing together one or more values.</param>
		/// <returns>If the write operation succeeded</returns>
		public bool Put<K>(MDBDBI dbi, K key, ref Span<byte> data, MDBWriteFlags flags = 0) where K : unmanaged {
			unsafe {
				return Put(dbi, new ReadOnlySpan<byte>(&key, sizeof(K)), ref data, flags);
			}
		}

		/// <summary>
		/// Similar to <see cref="Put{K}(K, ref Span{byte}, MDBWriteFlags)"/>, but uses the byte representation
		/// of values for both the key and value.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <typeparam name="V">Value type</typeparam>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to store in the database</param>
		/// <param name="value">The data to store</param>
		/// <param name="flags">Special options for this operation. This parameter must be set to 0 or by bitwise OR'ing together one or more values.</param>
		/// <returns>If the write operation succeeded</returns>
		public bool Put<K, V>(MDBDBI dbi, K key, V value, MDBWriteFlags flags = 0) where K : unmanaged where V : unmanaged {
			unsafe {
				Span<byte> svalue = new(&value, sizeof(V));
				return Put(dbi, key, ref svalue, flags);
			}
		}

		/// <summary>
		/// Similar to <see cref="Del(in ReadOnlySpan{byte}, in ReadOnlySpan{byte})"/>, but passes NULL as the
		/// data parameter. Effectively performs the delete operation with a key only.
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to delete from the database</param>
		/// <returns>If the corresponding entry was deleted</returns>
		/// <exception cref="MDBException">If an error occurs deleting the item</exception>
		public bool Del(MDBDBI dbi, in ReadOnlySpan<byte> key) {
			unsafe {
				fixed (byte* pKey = key) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBResult err = MDB.Functions.mdb_del(Txn, dbi, vkey, (MDBVal*)0);
					if (err == MDBResult.NotFound) return false;
					if (err != MDBResult.Success) throw new MDBException("Failed to delete database entry", err);
					return true;
				}
			}
		}

		/// <summary>
		/// Similar to <see cref="Del(in ReadOnlySpan{byte})"/>, but uses the byte representation of
		/// the key value.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to delete from the database</param>
		/// <returns>If the corresponding entry was deleted</returns>
		/// <exception cref="MDBException">If an error occurs deleting the item</exception>
		public bool Del<K>(MDBDBI dbi, K key) where K : unmanaged {
			unsafe {
				return Del(dbi, new ReadOnlySpan<byte>(&key, sizeof(K)));
			}
		}

		/// <summary>
		/// <para>Delete items from a database.</para>
		/// <para>
		/// This function removes key/data pairs from the database. If the database does not support sorted
		/// duplicate data items (<see cref="MDBDBFlags.DupSort"/>) the data parameter is ignored. If the
		/// database supports sorted duplicates and the data parameter is NULL, all of the duplicate data
		/// items for the key will be deleted. Otherwise, if the data parameter is non-NULL only the matching
		/// data item will be deleted. This function will return false if the specified key/data pair is not in the database.
		/// </para>
		/// <para>
		/// Note: To pass NULL as a data parameter, use <see cref="Del(in ReadOnlySpan{byte})"/> or its derivatives.
		/// </para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to delete from the database</param>
		/// <param name="data">The data to delete</param>
		/// <returns>If the corresponding entry was deleted</returns>
		/// <exception cref="MDBException">If an error occurs deleting the item</exception>
		public bool Del(MDBDBI dbi, in ReadOnlySpan<byte> key, in ReadOnlySpan<byte> data) {
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
					MDBResult err = MDB.Functions.mdb_del(Txn, dbi, vkey, &vdata);
					if (err == MDBResult.NotFound) return false;
					if (err != MDBResult.Success) throw new MDBException("Failed to delete database entry", err);
					return true;
				}
			}
		}

		/// <summary>
		/// Similar to <see cref="Del(in ReadOnlySpan{byte}, in ReadOnlySpan{byte})"/>, but uses the byte representation
		/// of a value for the key.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to delete from the database</param>
		/// <param name="data">The data to delete</param>
		/// <returns>If the corresponding entry was deleted</returns>
		/// <exception cref="MDBException">If an error occurs deleting the item</exception>
		public bool Del<K>(MDBDBI dbi, K key, in ReadOnlySpan<byte> data) where K : unmanaged {
			unsafe {
				return Del(dbi, new ReadOnlySpan<byte>(&key, sizeof(K)), data);
			}
		}

		/// <summary>
		/// Similar to <see cref="Del(in ReadOnlySpan{byte}, in ReadOnlySpan{byte})"/>, but uses the byte representation
		/// of values for both the key and value.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <typeparam name="V">Value type</typeparam>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="key">The key to delete from the database</param>
		/// <param name="value">The data to delete</param>
		/// <returns>If the corresponding entry was deleted</returns>
		/// <exception cref="MDBException">If an error occurs deleting the item</exception>
		public bool Del<K, V>(MDBDBI dbi, K key, V value) where K : unmanaged where V : unmanaged {
			unsafe {
				return Del(dbi, key, new ReadOnlySpan<byte>(&value, sizeof(V)));
			}
		}

		/// <summary>
		/// <para>Create a cursor handle.</para>
		/// <para>
		/// A cursor is associated with a specific transaction and database. A cursor cannot be used
		/// when its database handle is closed. Nor when its transaction has ended, except with <see cref="MDBCursor.Renew"/>.
		/// It can be discarded with <see cref="MDBCursor.Dispose"/>. A cursor in a write-transaction can be closed before its
		/// transaction ends, and will otherwise be closed when its transaction ends. A cursor in a read-only transaction must
		/// be closed explicitly, before or after its transaction ends. It can be reused with <see cref="MDBCursor.Renew"/>
		/// before finally closing it.
		/// </para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		/// <returns>The new <see cref="MDBCursor"/>.</returns>
		/// <exception cref="MDBException">If an error occurs opening the cursor.</exception>
		public MDBCursor OpenCursor(MDBDBI dbi) {
			unsafe {
				MDBResult err = MDB.Functions.mdb_cursor_open(Txn, dbi, out IntPtr cursor);
				if (err != MDBResult.Success) throw new MDBException("Failed to open database cursor", err);
				return new MDBCursor(this, cursor);
			}
		}

		/// <summary>
		/// <para>Compare two data items according to a particular database.</para>
		/// <para>This returns a comparison as if the two data items were keys in the specified database.</para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="a">The first item to compare</param>
		/// <param name="b">The second item to compare</param>
		/// <returns>Less than 0 if a &lt; b, 0 if a == b, greater than 0 if a &gt; b</returns>
		public int Cmp(MDBDBI dbi, in MDBVal a, in MDBVal b) {
			unsafe {
				return MDB.Functions.mdb_cmp(Txn, dbi, a, b);
			}
		}

		/// <summary>
		/// <para>Compare two data items according to a particular database.</para>
		/// <para>This returns a comparison as if the two items were data items of the specified database. The database must have the <see cref="MDBDBFlags.DupSort"/> flag.</para>
		/// </summary>
		/// <param name="dbi">The database interface handle</param>
		/// <param name="a">The first item to compare</param>
		/// <param name="b">The second item to compare</param>
		/// <returns>Less than 0 if a &lt; b, 0 if a == b, greater than 0 if a &gt; b</returns>
		public int DCmp(MDBDBI dbi, in MDBVal a, in MDBVal b) {
			unsafe {
				return MDB.Functions.mdb_cmp(Txn, dbi, a, b);
			}
		}

		public static implicit operator IntPtr(MDBTxn txn) => txn.Txn;

	}

}

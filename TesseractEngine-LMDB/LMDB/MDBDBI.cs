using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.LMDB {
	
	/// <summary>
	/// A database interface (DBI) provides access to a specific database as part of a transaction.
	/// </summary>
	public readonly struct MDBDBI {

		public MDBTxn Txn { get; }

		public uint DBI { get; }

		/// <summary>
		/// Statistics for the database.
		/// </summary>
		public MDBStat Stat {
			get {
				MDBResult err = MDB.Functions.mdb_stat(Txn, DBI, out MDBStat stat);
				if (err != MDBResult.Success) throw new MDBException("Failed to get database stats", err);
				return stat;
			}
		}

		/// <summary>
		/// Flags for the database.
		/// </summary>
		public MDBDBFlags Flags {
			get {
				MDBResult err = MDB.Functions.mdb_dbi_flags(Txn, DBI, out MDBDBFlags flags);
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
		public MDBCmpFunc Compare {
			set {
				MDBResult err = MDB.Functions.mdb_set_compare(Txn, DBI, value);
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
		public MDBCmpFunc DupSort {
			set {
				MDBResult err = MDB.Functions.mdb_set_dupsort(Txn, DBI, value);
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
		public MDBRelFunc Relocate {
			set {
				MDBResult err = MDB.Functions.mdb_set_relfunc(Txn, DBI, value);
				if (err != MDBResult.Success) throw new MDBException("Failed to set database relocate function", err);
			}
		}

		/// <summary>
		/// <para>A context pointer for a <see cref="MDBEnvFlags.FixedMap"/> database's relocation function.</para>
		/// <para>
		/// See <see cref="Relocate"/> and <see cref="MDBRelFunc"/> for more details.
		/// </para>
		/// </summary>
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
		public void Close() => MDB.Functions.mdb_dbi_close(Txn, DBI);

		/// <summary>
		/// <para>Empty or delete+close a database.</para>
		/// <para>See <see cref="Close"/> for restrictions about closing the DB handle.</para>
		/// </summary>
		/// <param name="del">If the database should be deleted from the environment or just emptied</param>
		/// <exception cref="MDBException">If an error occurs dropping the database</exception>
		public void Drop(bool del = false) {
			MDBResult err = MDB.Functions.mdb_drop(Txn, DBI, del);
			if (err != MDBResult.Success) throw new MDBException("Failed to drop database", err);
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
		/// <param name="key">The key to search for in the database</param>
		/// <returns>The data corresponding to the key</returns>
		/// <exception cref="KeyNotFoundException">If the specified key was not found in the database</exception>
		/// <exception cref="MDBException">If an error occurs getting the data</exception>
		public Span<byte> Get(in ReadOnlySpan<byte> key) {
			unsafe {
				fixed(byte* pKey = key) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBResult err = MDB.Functions.mdb_get(Txn, DBI, vkey, out MDBVal data);
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
		/// <param name="key">The key to search for in the database</param>
		/// <param name="data">The data corresponding to the key</param>
		/// <returns>If the entry was retrieved for the given key</returns>
		/// <exception cref="MDBException">If an error occurs getting the data</exception>
		public bool TryGet(in ReadOnlySpan<byte> key, out Span<byte> data) {
			data = Span<byte>.Empty;
			unsafe {
				fixed (byte* pKey = key) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBResult err = MDB.Functions.mdb_get(Txn, DBI, vkey, out MDBVal data2);
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
		/// <param name="key">The key to search for in the database</param>
		/// <returns>The data corresponding to the key</returns>
		public Span<byte> Get<K>(K key) where K : unmanaged {
			unsafe {
				return Get(new ReadOnlySpan<byte>(&key, sizeof(K)));
			}
		}

		/// <summary>
		/// Similar to <see cref="TryGet(in ReadOnlySpan{byte}, out Span{byte})"/>, but uses the
		/// byte representation of a value as the key.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <param name="key">The key to search for in the database</param>
		/// <param name="data">The data corresponding to the key</param>
		/// <returns>If the entry was retrieved for the given key</returns>
		public bool TryGet<K>(K key, out Span<byte> data) where K : unmanaged {
			unsafe {
				return TryGet(new ReadOnlySpan<byte>(&key, sizeof(K)), out data);
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
		/// <param name="key">The key to store in the database</param>
		/// <param name="data">The data to store</param>
		/// <param name="flags">Special options for this operation. This parameter must be set to 0 or by bitwise OR'ing together one or more values.</param>
		/// <returns>If the write operation succeeded</returns>
		/// <exception cref="MDBException">If an error occurs putting the item into the database</exception>
		public bool Put(in ReadOnlySpan<byte> key, ref Span<byte> data, MDBWriteFlags flags = 0) {
			if ((flags & MDBWriteFlags.Multiple) != 0) throw new MDBException("Invalid use of MDBWriteFlags.Multiple");
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
					MDBResult err = MDB.Functions.mdb_put(Txn, DBI, vkey, ref vdata, flags);
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
		/// <param name="key">The key to store in the database</param>
		/// <param name="data">The data to store</param>
		/// <param name="flags">Special options for this operation. This parameter must be set to 0 or by bitwise OR'ing together one or more values.</param>
		/// <returns>If the write operation succeeded</returns>
		public bool Put<K>(K key, ref Span<byte> data, MDBWriteFlags flags = 0) where K : unmanaged {
			unsafe {
				return Put(new ReadOnlySpan<byte>(&key, sizeof(K)), ref data, flags);
			}
		}

		/// <summary>
		/// Similar to <see cref="Put{K}(K, ref Span{byte}, MDBWriteFlags)"/>, but uses the byte representation
		/// of values for both the key and value.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <typeparam name="V">Value type</typeparam>
		/// <param name="key">The key to store in the database</param>
		/// <param name="value">The data to store</param>
		/// <param name="flags">Special options for this operation. This parameter must be set to 0 or by bitwise OR'ing together one or more values.</param>
		/// <returns>If the write operation succeeded</returns>
		public bool Put<K,V>(K key, V value, MDBWriteFlags flags = 0) where K : unmanaged where V : unmanaged {
			unsafe {
				Span<byte> svalue = new(&value, sizeof(V));
				return Put(key, ref svalue, flags);
			}
		}

		/// <summary>
		/// Similar to <see cref="Del(in ReadOnlySpan{byte}, in ReadOnlySpan{byte})"/>, but passes NULL as the
		/// data parameter. Effectively performs the delete operation with a key only.
		/// </summary>
		/// <param name="key">The key to delete from the database</param>
		/// <returns>If the corresponding entry was deleted</returns>
		/// <exception cref="MDBException">If an error occurs deleting the item</exception>
		public bool Del(in ReadOnlySpan<byte> key) {
			unsafe {
				fixed (byte* pKey = key) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBResult err = MDB.Functions.mdb_del(Txn, DBI, vkey, IntPtr.Zero);
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
		/// <param name="key">The key to delete from the database</param>
		/// <returns>If the corresponding entry was deleted</returns>
		/// <exception cref="MDBException">If an error occurs deleting the item</exception>
		public bool Del<K>(K key) where K : unmanaged {
			unsafe {
				return Del(new ReadOnlySpan<byte>(&key, sizeof(K)));
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
		/// <param name="key">The key to delete from the database</param>
		/// <param name="data">The data to delete</param>
		/// <returns>If the corresponding entry was deleted</returns>
		/// <exception cref="MDBException">If an error occurs deleting the item</exception>
		public bool Del(in ReadOnlySpan<byte> key, in ReadOnlySpan<byte> data) {
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
		/// <param name="key">The key to delete from the database</param>
		/// <param name="data">The data to delete</param>
		/// <returns>If the corresponding entry was deleted</returns>
		/// <exception cref="MDBException">If an error occurs deleting the item</exception>
		public bool Del<K>(K key, in ReadOnlySpan<byte> data) where K : unmanaged {
			unsafe {
				return Del(new ReadOnlySpan<byte>(&key, sizeof(K)), data);
			}
		}

		/// <summary>
		/// Similar to <see cref="Del(in ReadOnlySpan{byte}, in ReadOnlySpan{byte})"/>, but uses the byte representation
		/// of values for both the key and value.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <typeparam name="V">Value type</typeparam>
		/// <param name="key">The key to delete from the database</param>
		/// <param name="value">The data to delete</param>
		/// <returns>If the corresponding entry was deleted</returns>
		/// <exception cref="MDBException">If an error occurs deleting the item</exception>
		public bool Del<K, V>(K key, V value) where K : unmanaged where V : unmanaged {
			unsafe {
				return Del(key, new ReadOnlySpan<byte>(&value, sizeof(V)));
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
		/// <returns>The new <see cref="MDBCursor"/>.</returns>
		/// <exception cref="MDBException">If an error occurs opening the cursor.</exception>
		public MDBCursor OpenCursor() {
			MDBResult err = MDB.Functions.mdb_cursor_open(Txn, DBI, out IntPtr cursor);
			if (err != MDBResult.Success) throw new MDBException("Failed to open database cursor", err);
			return new MDBCursor(this, cursor);
		}

		/// <summary>
		/// <para>Compare two data items according to a particular database.</para>
		/// <para>This returns a comparison as if the two data items were keys in the specified database.</para>
		/// </summary>
		/// <param name="a">The first item to compare</param>
		/// <param name="b">The second item to compare</param>
		/// <returns>Less than 0 if a &lt; b, 0 if a == b, greater than 0 if a &gt; b</returns>
		public int Cmp(in MDBVal a, in MDBVal b) => MDB.Functions.mdb_cmp(Txn, DBI, a, b);

		/// <summary>
		/// <para>Compare two data items according to a particular database.</para>
		/// <para>This returns a comparison as if the two items were data items of the specified database. The database must have the <see cref="MDBDBFlags.DupSort"/> flag.</para>
		/// </summary>
		/// <param name="a">The first item to compare</param>
		/// <param name="b">The second item to compare</param>
		/// <returns>Less than 0 if a &lt; b, 0 if a == b, greater than 0 if a &gt; b</returns>
		public int DCmp(in MDBVal a, in MDBVal b) => MDB.Functions.mdb_cmp(Txn, DBI, a, b);

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LMDB {

	public class MDBCursor : IDisposable {

		public MDBTxn Txn { get; }

		[NativeType("MDB_cursor*")]
		public IntPtr Cursor { get; private set; }

		/// <summary>
		/// <para>Count of duplicates for the current key.</para>
		/// <para>This call is only valid on databases that support sorted duplicate data items <see cref="MDBDBFlags.DupSort"/>.</para>
		/// </summary>
		public nuint DuplicateCount {
			get {
				MDBResult err = MDB.Functions.mdb_cursor_count(Cursor, out nuint countp);
				if (err != MDBResult.Success) throw new MDBException("Failed to get cursor duplicate count", err);
				return countp;
			}
		}

		public MDBCursor(MDBTxn txn, IntPtr cursor) {
			Txn = txn;
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

		/// <summary>
		/// <para>Renew a cursor handle.</para>
		/// <para>
		/// A cursor is associated with a specific transaction and database. Cursors that are only used in
		/// read-only transactions may be re-used, to avoid unnecessary malloc/free overhead. The cursor may
		/// be associated with a new read-only transaction, and referencing the same database handle as it was
		/// created with. This may be done whether the previous transaction is live or dead.
		/// </para>
		/// </summary>
		/// <exception cref="MDBException"></exception>
		public void Renew() {
			MDBResult err = MDB.Functions.mdb_cursor_renew(Txn, Cursor);
			if (err != MDBResult.Success) throw new MDBException("Failed to renew cursor", err);
		}

		/// <summary>
		/// <para>Retrieve by cursor.</para>
		/// <para>
		/// This function retrieves key/data pairs from the database. The address and length of the key
		/// are returned in the object to which key refers (except for the case of the <see cref="MDBCursorOp.Set"/>
		/// option, in which the key object is unchanged), and the address and length of the data are returned in the
		/// object to which data refers. <see cref="MDBDBI.Get(in ReadOnlySpan{byte})"/> for restrictions on using the
		/// output values.
		/// </para>
		/// </summary>
		/// <param name="key">The key for a retrieved item</param>
		/// <param name="data">The dat of a retrieved item</param>
		/// <param name="op">A cursor operation</param>
		/// <returns>If the matching key was found</returns>
		/// <exception cref="MDBException">If an error occurs getting the item from the database</exception>
		public bool Get(ref Span<byte> key, out Span<byte> data, MDBCursorOp op) {
			data = Span<byte>.Empty;
			unsafe {
				fixed(byte* pKey = key) {
					MDBVal vkey = new() {
						Size = (nuint)key.Length,
						Data = (IntPtr)pKey
					};
					MDBResult err = MDB.Functions.mdb_cursor_get(Cursor, ref vkey, out MDBVal vdata, op);
					if (err == MDBResult.NotFound) return false;
					if (err != MDBResult.Success) throw new MDBException("Failed to get from cursor", err);
					if (vkey.Data != (IntPtr)pKey)
						key = new Span<byte>((void*)vkey.Data, (int)vkey.Size);
					data = new Span<byte>((void*)vdata.Data, (int)vdata.Size);
					return true;
				}
			}
		}

		/// <summary>
		/// <para>Store by cursor.</para>
		/// <para>
		/// This function stores key/data pairs into the database. The cursor is positioned at the new item, or on failure usually near it.
		/// </para>
		/// </summary>
		/// <param name="key">The key operated on</param>
		/// <param name="data">The data operated on</param>
		/// <param name="flags">Options for this operation, This parameter must be set to 0 or one of the <see cref="MDBWriteFlags"/> values.</param>
		/// <returns>If the put operation succeeded</returns>
		/// <exception cref="MDBException">If an error occurs writing the data</exception>
		public bool Put(in ReadOnlySpan<byte> key, ref Span<byte> data, MDBWriteFlags flags = 0) {
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
					MDBResult err = MDB.Functions.mdb_cursor_put(Cursor, vkey, (IntPtr)(&vdata), flags);
					if (err == MDBResult.KeyExist) return false;
					if (err != MDBResult.Success) throw new MDBException("Failed to put at cursor", err);
					if ((flags & MDBWriteFlags.Reserve) != 0)
						data = new Span<byte>((void*)vdata.Data, (int)vdata.Size);
					return true;
				}
			}
		}

		/// <summary>
		/// Similar to <see cref="Put(in ReadOnlySpan{byte}, ref Span{byte}, MDBWriteFlags)"/>, but uses the byte
		/// representation of a value for the key.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <param name="key">The key operated on</param>
		/// <param name="data">The data operated on</param>
		/// <param name="flags">Options for this operation, This parameter must be set to 0 or one of the <see cref="MDBWriteFlags"/> values.</param>
		/// <returns>If the put operation succeeded</returns>
		/// <exception cref="MDBException">If an error occurs writing the data</exception>
		public bool Put<K>(K key, ref Span<byte> data, MDBWriteFlags flags = 0) where K : unmanaged {
			unsafe {
				return Put(new ReadOnlySpan<byte>(&key, sizeof(K)), ref data, flags);
			}
		}

		/// <summary>
		/// Similar to <see cref="Put{K}(K, ref Span{byte}, MDBWriteFlags)"/>, but uses the byte representation
		/// of values for both key and value.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <typeparam name="V">Value type</typeparam>
		/// <param name="key">The key operated on</param>
		/// <param name="value">The data operated on</param>
		/// <param name="flags">Options for this operation, This parameter must be set to 0 or one of the <see cref="MDBWriteFlags"/> values.</param>
		/// <returns>If the put operation succeeded</returns>
		/// <exception cref="MDBException">If an error occurs writing the data</exception>
		public bool Put<K, V>(K key, V value, MDBWriteFlags flags = 0) where K : unmanaged where V : unmanaged {
			unsafe {
				Span<byte> svalue = new(&value, sizeof(V));
				return Put(key, ref svalue, flags);
			}
		}

		/// <summary>
		/// Similar to <see cref="Put{K, V}(K, V, MDBWriteFlags)"/>, but implicitly uses the <see cref="MDBWriteFlags.Multiple"/> flag.
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <typeparam name="V">Value type</typeparam>
		/// <param name="key">The key to use when writing</param>
		/// <param name="values">The multiple values to write</param>
		/// <param name="flags">Write flags</param>
		/// <returns>If the write operation succeeded</returns>
		/// <exception cref="MDBException">If an error occurs when writing the data</exception>
		public bool Put<K, V>(K key, in ReadOnlySpan<V> values, MDBWriteFlags flags = 0) where K : unmanaged where V : unmanaged {
			flags |= MDBWriteFlags.Multiple;
			unsafe {
				unsafe {
					fixed (V* pValue = values) {
						MDBVal vkey = new() {
							Size = (nuint)sizeof(K),
							Data = (IntPtr)(&key)
						};
						Span<MDBVal> vdata = stackalloc MDBVal[] {
							new() { // mv_size = sizeof(V), mv_data = &values
								Size = (nuint)sizeof(V),
								Data = (IntPtr)pValue
							},
							new() { // mv_size = values.Length
								Size = (nuint)values.Length
							}
						};
						MDBResult err;
						fixed(MDBVal* pData = vdata) {
							err = MDB.Functions.mdb_cursor_put(Cursor, vkey, (IntPtr)pData, flags);
						}
						if (err == MDBResult.KeyExist) return false;
						if (err != MDBResult.Success) throw new MDBException("Failed to put at cursor", err);
						return true;
					}
				}
			}
		}

		/// <summary>
		/// <para>Delete current key/data pair.</para>
		/// <para>This function deletes the key/data pair to which the cursor refers.</para>
		/// </summary>
		/// <param name="flags">Options for this operation. This parameter must be set to 0 or <see cref="MDBWriteFlags.NoDupData"/>.</param>
		/// <exception cref="MDBException"></exception>
		public void Del(MDBWriteFlags flags) {
			MDBResult err = MDB.Functions.mdb_cursor_del(Cursor, flags);
			if (err != MDBResult.Success) throw new MDBException("Failed to delete at cursor", err);
		}

	}

}

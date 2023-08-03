using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities.Data;

namespace Tesseract.Core.Engine.Content {

	/// <summary>
	/// Generic interface for the data manager for an item. Implementations should use the
	/// <see cref="ItemDataManager{T}"/> class for type safety.
	/// </summary>
	public interface IItemDataManager {

		/// <summary>
		/// The default data for the item.
		/// </summary>
		public object? Default { get; }

		/// <summary>
		/// Loads data for this item from a data object.
		/// </summary>
		/// <param name="data">Data object to load item data from</param>
		/// <returns>The loaded item data</returns>
		public object? Load(DataObject data);

		/// <summary>
		/// Stores data for this item to a data object.
		/// </summary>
		/// <param name="value">Item data to store</param>
		/// <param name="data">Data object to store item data to</param>
		public void Store(object? value, IStreamingDataObject data);

		/// <summary>
		/// Checks that the given object is of the correct type for this item's data.
		/// </summary>
		/// <param name="data">The data to check</param>
		/// <returns>The checked data</returns>
		public object? Check(object? data);

	}

	/// <summary>
	/// Concrete implementation of <see cref="IItemDataManager"/> with type safety.
	/// </summary>
	/// <typeparam name="T">The item data type</typeparam>
	public abstract class ItemDataManager<T> : IItemDataManager {

		public object? Default { get; }

		public ItemDataManager(T? defaultValue = default) {
			Default = Check(defaultValue);
		}

		protected abstract T LoadData(DataObject data);

		protected abstract void StoreData(T value, IStreamingDataObject data);

		public object? Load(DataObject data) => LoadData(data);

		public void Store(object? value, IStreamingDataObject data) => StoreData((T)value!, data);

		public object? Check(object? data) => data is T tval ? tval : throw new InvalidCastException($"Invalid item data type (expected {typeof(T)}, got {data?.GetType()?.ToString() ?? "null"})");

	}

}

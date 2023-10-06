using System;
using System.Diagnostics.CodeAnalysis;
using Tesseract.Core.Engine.Registry;
using Tesseract.Core.Utilities.Data;

namespace Tesseract.Core.Engine.Content {

	/// <summary>
	/// <para>An item stack holds zero or more of a specific item.</para>
	/// <para>
	/// The item stack structure holds three primary fields; a reference to the
	/// item being stored in the stack, the number of items in the stack (as a
	/// signed 32-bit integer), and an optional item-specific data object. An item
	/// stack is considered empty when the item is null or the count is zero.
	/// </para>
	/// </summary>
	public struct ItemStack {

		/// <summary>
		/// The item this stack is holding, or null if the stack is empty.
		/// </summary>
		public Item? Item { get; } = null;

		private int count = 0;

		/// <summary>
		/// The number of items in the stack.
		/// </summary>
		public int Count {
			readonly get => count;
			set {
				if (Item == null) return;
				if (value > Item.Properties.MaxStackSize) value = Item.Properties.MaxStackSize;
				if (value < 0) value = 0;
				count = value;
			}
		}

		private object? data = null;

		/// <summary>
		/// The data for the item in the stack.
		/// </summary>
		public object? Data {
			readonly get => data;
			set {
				if (Item == null) return;
				if (Item.DataManager == null) return;
				data = Item.DataManager.Check(value);
			}
		}

		/// <summary>
		/// If the item stack is empty (when the item is null or the count is zero).
		/// </summary>
		[MemberNotNullWhen(false, nameof(Item))]
		public readonly bool IsEmpty => Item == null || count == 0;

		public ItemStack(Item item, int count = 1, object? data = null) {
			if (count > 0) {
				Item = item;
				this.count = count;
				if (item.DataManager != null) {
					this.data = data != null ? item.DataManager.Check(data) : item.DataManager.Default;
				}
			}
		}

		/// <summary>
		/// Loads an item stack from the given data.
		/// </summary>
		/// <param name="data">The data to load from</param>
		/// <param name="idmap">The item ID map to use, or null to use the current global map</param>
		/// <returns>The loaded item stack</returns>
		public static ItemStack Load(DataObject data, RegistryIDMap<Item>? idmap = null) {
			Item? item;
			int count;
			object? itemData = null;

			try {
				// Get the ID from the data
				var id = data["ID"];
				if (id.Type == DataType.String) {
					// If a string then load by name
					Item.Registry.TryGetByKey((string)id, out item);
				} else if (id.Type == DataType.Int) {
					// Else load by ID
					item = (idmap ?? Item.Registry.CurrentIDMap).Load((int)id);
				} else return default;

				if (item == null) return default;

				count = (int)data["Count"];
				if (count < 0) return default;
				if (count > item.Properties.MaxStackSize) count = item.Properties.MaxStackSize;

				if (item.DataManager != null) {
					if (data.TryGetValue("Data", out DataBox data2) && data2.Type == DataType.Object) {
						itemData = item.DataManager.Load((DataObject)data2);
					} else {
						itemData = item.DataManager.Default;
					}
				}

				return new ItemStack(item, count, itemData);
			} catch (Exception) {
				return default;
			}
		}

		/// <summary>
		/// Loads the item data for this item stack from the given data object.
		/// </summary>
		/// <param name="data">Data object to load from</param>
		public void LoadData(DataObject data) {
			if (Item != null && Item.DataManager != null) {
				this.data = Item.DataManager.Load(data);
			}
		}

		/// <summary>
		/// <para>Saves an item stack to a data object.</para>
		/// <para>
		/// If the stack saved in a portable form, the string name of it is used
		/// as the ID instead of its numeric ID from the registry. If it is saved
		/// as an integer ID the default ID map for items is used, although a custom
		/// one may be supplied.
		/// </para>
		/// </summary>
		/// <param name="data">Data object to save the stack to</param>
		/// <param name="portable">If the stack should be saved in a portable format</param>
		/// <param name="idmap">The ID map to use for saving non-portably, or null to use the default map</param>
		public readonly void Save(IStreamingDataObject data, bool portable = true, RegistryIDMap<Item>? idmap = null) {
			if (Item != null) {
				idmap ??= Item.Registry.CurrentIDMap;
				data["ID"] = portable ? Item.UnlocalizedName.ToString() : idmap.Store(Item);
				data["Count"] = count;
				if (Item.DataManager != null && this.data != null) {
					ItemStack self = this;
					data.Write("Data", data2 => self.Item.DataManager.Store(self.data, data2));
				}
			}
		}

		/// <summary>
		/// Tests if the item in this item stack is the same as the item in another.
		/// </summary>
		/// <param name="other">Item stack to check against</param>
		/// <returns>If both stacks hold the same item</returns>
		public readonly bool AreItemsEqual(ItemStack other) => Item == other.Item && Equals(Data, other.Data);

		/// <summary>
		/// "Applies" another item stack onto this stack, as if it were dropped onto this inside a player's inventory.
		/// </summary>
		/// <param name="other">The stack to apply to this stack</param>
		/// <returns>If any action was taken using the applied stack</returns>
		public bool Apply(ref ItemStack other) {
			// If this stack is empty move the other stack into it
			if (IsEmpty) {
				this = other;
				other = default;
				return true;
			}

			// If the items are the same try to combine the stacks
			if (AreItemsEqual(other)) {
				int count = other.count;
				Stack(ref other);
				return other.count != count;
			}

			// Defer to our item how the stack is applied
			return Item.ApplyItemStack(ref this, ref other);
		}

		/// <summary>
		/// Attempts to combine another item stack into this stack, leaving behind any remaining items.
		/// </summary>
		/// <param name="other">The other stack to merge into this</param>
		/// <returns>If the other stack was entirely consumed by this stack</returns>
		public bool Stack(ref ItemStack other) {
			// Stacking nothing into the stack changes nothing
			if (other.IsEmpty) return true;

			// If this stack is empty we take the entire other stack
			if (IsEmpty) {
				this = other;
				other = default;
				return true;
			}

			// If the items are not equal we cannot combine the stacks
			if (!AreItemsEqual(other)) return false;

			// Either consume the other stack entirely or move enough items to hit the maximum
			int space = Item.Properties.MaxStackSize - count;
			if (space > other.count) {
				count += other.count;
				other = default;
				return true;
			} else {
				count += space;
				other.count -= space;
				return false;
			}
		}

		/// <summary>
		/// Splits this item stack by removing a certain number of items.
		/// </summary>
		/// <param name="count">The maximum number of items to take from this stack</param>
		/// <returns>The stack split from this stack</returns>
		public ItemStack Split(int count) {
			if (IsEmpty || count < 1) return default;

			if (count > this.count) {
				ItemStack copy = this;
				this = default;
				return copy;
			}

			this.count -= count;
			ItemStack split =  new(Item, count, data);
			if (this.count <= 0) this = default;
			return split;
		}

	}

}

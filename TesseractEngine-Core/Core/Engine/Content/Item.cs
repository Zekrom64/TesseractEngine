using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Engine.Registry;

namespace Tesseract.Core.Engine.Content {

	/// <summary>
	/// <para>An item is an object which can exist inside an inventory.</para>
	/// <para>
	/// Items can override the default behavior by:
	/// <list type="number">
	/// <item>Overriding methods in the base class</item>
	/// <item>Implementing interfaces which define additional behavior</item>
	/// <item>Setting properties on the item class</item>
	/// </list>
	/// </para>
	/// </summary>
	public class Item : IRegistryObject {

		/// <summary>
		/// The registry for items.
		/// </summary>
		public static IRegistry<Item> Registry { get; } = new BaseRegistry<Item>("item");

		public string UnlocalizedName { get; }

		private int id = -1;

		public int ID {
			get => id >= 0 ? id : throw new InvalidOperationException("Item has not been initialized with an ID yet");
			set {
				if (id < 0) throw new ArgumentException("Cannot assign a negative ID to an item");
				if (id == -1) id = value;
				else throw new InvalidOperationException("Item has already been assigned an ID");
			}
		}

		/// <summary>
		/// The maximum number of this item that can exist in an <see cref="ItemStack"/>.
		/// </summary>
		public int MaxStackSize { get; protected init; } = 1;

		/// <summary>
		/// The data manager for this item. If null the item does not have additional data.
		/// </summary>
		public IItemDataManager? DataManager { get; protected init; } = null;


		protected Item(string unlocalizedName) {
			UnlocalizedName = unlocalizedName;
		}


		/// <summary>
		/// Invoked to "apply" and item stack with this item to another stack, as if the player dropped
		/// it onto the other stack in an inventory.
		/// </summary>
		/// <param name="thisStack">Item stack containing this item to apply</param>
		/// <param name="appliedStack">Item stack to apply this item to</param>
		/// <returns>If this item overrides the application behavior</returns>
		public virtual bool ApplyItemStack(ref ItemStack thisStack, ref ItemStack appliedStack) => false;

	}

}

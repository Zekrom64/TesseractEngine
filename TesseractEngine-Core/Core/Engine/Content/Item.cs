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
	public class Item : RegistryObject {

		/// <summary>
		/// The registry for items.
		/// </summary>
		public static IRegistry<Item> Registry { get; } = new BaseRegistry<Item>("item");


		/// <summary>
		/// The properties of this item.
		/// </summary>
		public ItemProperties Properties { get; }

		/// <summary>
		/// The data manager for this item. If null the item does not have additional data.
		/// </summary>
		public IItemDataManager? DataManager { get; protected init; } = null;


		protected Item(string unlocalizedName, ItemProperties properties) : base(unlocalizedName) {
			Properties = properties;
		}

		//===================//
		// Item Interactions //
		//===================//

		/// <summary>
		/// Invoked to "apply" an item stack with this item to another stack, as if the player dropped
		/// it onto the other stack in an inventory.
		/// </summary>
		/// <param name="thisStack">Item stack containing this item to apply</param>
		/// <param name="appliedStack">Item stack to apply this item to</param>
		/// <returns>If this item overrides the application behavior</returns>
		public virtual bool ApplyItemStack(ref ItemStack thisStack, ref ItemStack appliedStack) => false;

		//==========================//
		// Dynamic Property Getters //
		//==========================//

		/// <summary>
		/// Computes the currency value for a stack of this item.
		/// </summary>
		/// <param name="stack">The stack of this item</param>
		/// <returns>The currency value of the stack</returns>
		public virtual double GetCurrencyValue(ItemStack stack) => Properties.BaseCurrencyValue * stack.Count;

		/// <summary>
		/// Computes the mass of a stack of this item in kilograms.
		/// </summary>
		/// <param name="stack">The stack of this item</param>
		/// <returns>The mass of the stack</returns>
		public virtual double GetMass(ItemStack stack) => 0;

	}

}

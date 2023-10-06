using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Engine.Content {

	/// <summary>
	/// Record for properties of an item.
	/// </summary>
	public record ItemProperties {

		/// <summary>
		/// The maximum number of this item that can exist in an <see cref="ItemStack"/>.
		/// </summary>
		public int MaxStackSize { get; init; } = 100;

		/// <summary>
		/// The base currency value of this item.
		/// </summary>
		public double BaseCurrencyValue { get; init; } = 0;

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Native {

	/// <summary>
	/// Denotes the native type of a field or parameter that may be ambiguous before marshalling.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class NativeTypeAttribute : Attribute {

		public string Name { get; }

		public NativeTypeAttribute(string name) {
			Name = name;
		}

	}

	/// <summary>
	/// Denotes an "extern" function that will be loaded from some library.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public class ExternFunctionAttribute : Attribute {

		/// <summary>
		/// A list of alternative names to attempt to load the function with.
		/// </summary>
		public string[] AltNames { get; init; } = null;

		/// <summary>
		/// A generic predicate to determine if the function should be loaded.
		/// </summary>
		public Func<bool> Predicate { get; init; } = null;

		/// <summary>
		/// A platform the function should only be loaded on.
		/// </summary>
		public PlatformType? Platform { get; init; } = null;

		/// <summary>
		/// A sub-platform the function should only be loaded on.
		/// </summary>
		public SubplatformType? Subplatform { get; init; } = null;

		public ExternFunctionAttribute() { }

	}
}

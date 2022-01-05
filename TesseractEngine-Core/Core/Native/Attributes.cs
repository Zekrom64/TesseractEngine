using System;

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
		public string[]? AltNames { get; init; } = null;

		/// <summary>
		/// If the function should be manually loaded. If true this field will be ignored by
		/// <see cref="Library.LoadFunctions(Func{string, IntPtr}, object)"/>
		/// </summary>
		public bool Manual { get; init; } = false;

		/// <summary>
		/// A platform the function should only be loaded on.
		/// </summary>
		public PlatformType Platform { get; init; } = default;

		/// <summary>
		/// A sub-platform the function should only be loaded on.
		/// </summary>
		public SubplatformType Subplatform { get; init; } = default;

		/// <summary>
		/// If a relaxed approach should be taken to function loading (ie. no exception is thrown on failure).
		/// </summary>
		public bool Relaxed { get; init; } = false;

		public ExternFunctionAttribute() { }

	}
}

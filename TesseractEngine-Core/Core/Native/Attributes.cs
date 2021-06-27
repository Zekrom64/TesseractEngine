using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Native {

	/// <summary>
	/// Denotes the native type of a field or parameter that may be ambiguous before marshalling.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class NativeTypeAttribute : Attribute {

		public string Name { get; }

		public NativeTypeAttribute(string name) {
			Name = name;
		}

	}
}

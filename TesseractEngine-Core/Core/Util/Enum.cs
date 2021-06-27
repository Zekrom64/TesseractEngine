using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Util {

	public interface IValuedEnum<T> : IEquatable<IValuedEnum<T>> {
	
		public T Value { get; }
	
	}

}

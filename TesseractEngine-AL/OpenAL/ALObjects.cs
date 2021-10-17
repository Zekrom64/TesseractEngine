using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.OpenAL {
	
	public interface IALObject {

		public AL AL { get; }

	}

	public class ALException : Exception {

		public ALException(string msg) : base(msg) { }

	}

}

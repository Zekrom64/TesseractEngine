using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.OpenAL {
	
	public class AL {

		public ALCContext Context { get; }

		public AL11 AL11 { get; }

		public AL(ALCContext context) {
			Context = context;
			context.MakeContextCurrent();
		}

		public IntPtr GetProcAddress(string name) => Context.Device.GetProcAddress(name);

	}
}

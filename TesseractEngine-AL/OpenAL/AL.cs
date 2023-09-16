using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.OpenAL {
	
	public class AL {

		public ALCContext Context { get; }

		public AL11 AL11 { get; }

		//public SOFTBufferSamples? SOFTBufferSamples { get; }
		public EXTEFX? EXTEFX { get; }

		public AL(ALCContext context) {
			Context = context;
			context.MakeContextCurrent();

			AL11 = new(this);
			//if (AL11.IsExtensionPresent(SOFTBufferSamples.ExtensionName)) SOFTBufferSamples = new(this);
			if (AL11.IsExtensionPresent(EXTEFX.ExtensionName)) EXTEFX = new(this);
		}

		public IntPtr GetProcAddress(string name) => Context.Device.GetProcAddress(name);

	}
}

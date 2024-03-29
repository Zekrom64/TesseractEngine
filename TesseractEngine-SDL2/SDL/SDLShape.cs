﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Numerics;

namespace Tesseract.SDL {
	
	public enum WindowShapeMode {
		Default,
		BinarizeAlpha,
		ReverseBinarizeAlpha,
		ColorKey
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct WindowShapeParams {
		[FieldOffset(0)]
		public byte BinarizationCutoff;
		[FieldOffset(0)]
		public Vector4b ColorKey;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLWindowShapeMode {
		public WindowShapeMode Mode;
		public WindowShapeParams Parameters;
	}

}

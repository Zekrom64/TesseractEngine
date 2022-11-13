using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.SDL {
	
	public enum SDLMessageBoxFlags : uint {
		Error = 0x10,
		Warning = 0x20,
		Information = 0x40,
		ButtonsLeftToRight = 0x80,
		ButtonsRightToLeft = 0x100
	}

	public enum SDLMessageBoxButtonFlags : uint {
		ReturnKeyDefault = 0x1,
		EscapeKeyDefault = 0x2
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLMessageBoxButtonData {

		public SDLMessageBoxButtonFlags Flags;

		public int ButtonID;

		[MarshalAs(UnmanagedType.LPStr)]
		public string Text;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLMessageBoxColor {

		public byte R;
		public byte G;
		public byte B;

	}

	public enum SDLMessageBoxColorType : int {
		Background = 0,
		Text,
		ButtonBorder,
		ButtonBackground,
		ButtonSelected,
		Max
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLMessageBoxColorScheme {

		public SDLMessageBoxColor Background;
		public SDLMessageBoxColor Text;
		public SDLMessageBoxColor ButtonBorder;
		public SDLMessageBoxColor ButtonBackground;
		public SDLMessageBoxColor ButtonSelected;

		public SDLMessageBoxColor this[SDLMessageBoxColorType type] {
			get {
				if (type >= SDLMessageBoxColorType.Max) return default;
				ref SDLMessageBoxColor rcolor = ref Background;
				unsafe {
					fixed(SDLMessageBoxColor* pColor = &rcolor) {
						return pColor[(int)type];
					}
				}
			}
			set {
				if (type >= SDLMessageBoxColorType.Max) return;
				ref SDLMessageBoxColor rcolor = ref Background;
				unsafe {
					fixed (SDLMessageBoxColor* pColor = &rcolor) {
						pColor[(int)type] = value;
					}
				}
			}
		}

	}

	public record SDLMessageBoxData {

		public SDLMessageBoxFlags Flags { get; init; } = default;

		public SDLWindow? Window { get; init; } = null;

		public required string Title { get; init; }

		public required string Message { get; init; }

		public SDLMessageBoxButtonData[]? Buttons { get; init; } = null;

		public SDLMessageBoxColorScheme? ColorScheme { get; init; } = null;

	}

}

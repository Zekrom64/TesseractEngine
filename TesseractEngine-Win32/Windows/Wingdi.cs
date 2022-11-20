using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Windows {

	[StructLayout(LayoutKind.Sequential)]
	public struct LOGFONTW {

		public int Height;
		public int Width;
		public int Escapement;
		public int Orientation;
		public int Weight;
		private byte italic;
		public bool Italic {
			get => italic != 0;
			set => italic = (byte)(value ? 1 : 0);
		}
		private byte underline;
		public bool Underline {
			get => underline != 0;
			set => underline = (byte)(value ? 1 : 0);
		}
		private byte strikeOut;
		public bool StrikeOut {
			get => strikeOut != 0;
			set => strikeOut = (byte)(value ? 1 : 0);
		}
		public byte CharSet;
		public byte OutPrecision;
		public byte ClipPrecision;
		public byte Quality;
		public byte PitchAndFamily;
		private unsafe fixed char faceName[32];
		public string FaceName {
			get {
				unsafe {
					fixed(char* pFaceName = faceName) {
						return MemoryUtil.GetUTF16(new ReadOnlySpan<char>(pFaceName, 32));
					}
				}
			}
			set {
				unsafe {
					fixed(char* pFaceName = faceName) {
						MemoryUtil.PutUTF16(value, new Span<byte>(pFaceName, 32*sizeof(char)));
					}
				}
			}
		}

	}

}

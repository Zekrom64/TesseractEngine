using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {

	public class SDLCursor : IDisposable {

		public IPointer<SDL_Cursor> Cursor { get; private set;}

		public SDLCursor(in ReadOnlySpan<byte> data, in ReadOnlySpan<byte> mask, int w, int h, int hotX, int hotY) {
			unsafe {
				fixed(byte* pData = data) {
					fixed(byte* pMask = mask) {
						Cursor = new UnmanagedPointer<SDL_Cursor>(SDL2.Functions.SDL_CreateCursor((IntPtr)pData, (IntPtr)pMask, w, h, hotX, hotY));
					}
				}
			}
		}

		public SDLCursor(SDLSurface surface, int hotX, int hotY) {
			Cursor = new UnmanagedPointer<SDL_Cursor>(SDL2.Functions.SDL_CreateColorCursor(surface.Surface.Ptr, hotX, hotY));
		}

		public SDLCursor(SDLSystemCursor id) {
			Cursor = new UnmanagedPointer<SDL_Cursor>(SDL2.Functions.SDL_CreateSystemCursor(id));
		}

		public SDLCursor(IPointer<SDL_Cursor> cursor) {
			Cursor = cursor;
		}

		public static SDLCursor DefaultCursor => new(new UnmanagedPointer<SDL_Cursor>(SDL2.Functions.SDL_GetDefaultCursor()));

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Cursor != null && !Cursor.IsNull) {
				SDL2.Functions.SDL_FreeCursor(Cursor.Ptr);
				Cursor = null!;
			}
		}

	}

}

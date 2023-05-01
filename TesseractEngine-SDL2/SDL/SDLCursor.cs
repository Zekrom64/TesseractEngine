using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {

	public class SDLCursor : IDisposable {

		[NativeType("SDL_Cursor*")]
		public IntPtr Cursor { get; private set;}

		public SDLCursor(in ReadOnlySpan<byte> data, in ReadOnlySpan<byte> mask, int w, int h, int hotX, int hotY) {
			unsafe {
				fixed(byte* pData = data) {
					fixed(byte* pMask = mask) {
						Cursor = SDL2.Functions.SDL_CreateCursor(pData, pMask, w, h, hotX, hotY);
					}
				}
			}
		}

		public SDLCursor(SDLSurface surface, int hotX, int hotY) {
			unsafe {
				Cursor = SDL2.Functions.SDL_CreateColorCursor((SDL_Surface*)surface.Surface.Ptr, hotX, hotY);
			}
		}

		public SDLCursor(SDLSystemCursor id) {
			unsafe {
				Cursor = SDL2.Functions.SDL_CreateSystemCursor(id);
			}
		}

		public SDLCursor([NativeType("SDL_Cursor*")] IntPtr cursor) {
			Cursor = cursor;
		}

		public static SDLCursor DefaultCursor {
			get {
				unsafe {
					return new(SDL2.Functions.SDL_GetDefaultCursor());
				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Cursor != IntPtr.Zero) {
				unsafe {
					SDL2.Functions.SDL_FreeCursor(Cursor);
				}
				Cursor = IntPtr.Zero;
			}
		}

	}

}

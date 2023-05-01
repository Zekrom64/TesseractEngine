using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;
using System.Numerics;

namespace Tesseract.SDL.Native {

	using SDLPoint = Vector2i;
	using SDLFPoint = Vector2;
	using SDLRect = Recti;
	using SDLFRect = Rectf;
	using SDLColor = Vector4b;
	using SDL_SensorID = Int32;
	using SDL_GLContext = IntPtr;
	using SDL_JoystickGUID = Guid;

	public unsafe class SDLFunctions {

		// SDL_stdinc.h

		[NativeType("void* SDL_malloc(size_t size)")]
		public delegate* unmanaged<nuint, IntPtr> SDL_malloc;
		[NativeType("void* SDL_calloc(size_t nmemb, size_t size)")]
		public delegate* unmanaged<nuint, nuint, IntPtr> SDL_calloc;
		[NativeType("void* SDL_realloc(void* mem, size_t size)")]
		public delegate* unmanaged<IntPtr, nuint, IntPtr> SDL_realloc;
		[NativeType("void SDL_free(void* mem)")]
		public delegate* unmanaged<IntPtr, void> SDL_free;

		// SDL.h

		[NativeType("int SDL_Init(unsigned int flags)")]
		public delegate* unmanaged<uint, int> SDL_Init;
		[NativeType("int SDL_InitSubSystem(unsigned int flags)")]
		public delegate* unmanaged<uint, int> SDL_InitSubSystem;
		[NativeType("void SDL_QuitSubSystem(unsigned int flags)")]
		public delegate* unmanaged<uint, void> SDL_QuitSubSystem;
		[NativeType("unsigned int SDL_WasInit(unsigned int flags)")]
		public delegate* unmanaged<uint, uint> SDL_WasInit;
		[NativeType("void SDL_Quit()")]
		public delegate* unmanaged<void> SDL_Quit;

		// SDL_error.h

		[NativeType("int SDL_SetError(const char* fmt, ...)")]
		public delegate* unmanaged<byte*, int> SDL_SetError;
		[NativeType("const char* SDL_GetError()")]
		public delegate* unmanaged<IntPtr> SDL_GetError;
		[NativeType("void SDL_ClearError()")]
		public delegate* unmanaged<void> SDL_ClearError;
		[NativeType("int SDL_Error(int code)")]
		public delegate* unmanaged<int, int> SDL_Error;

		// SDL_pixels.h

		[NativeType("const char* SDL_GetPixelFormatName(unsigned int format)")]
		public delegate* unmanaged<uint, IntPtr> SDL_GetPixelFormatName;
		[NativeType("SDL_bool SDL_PixelFormatEnumToMasks(unsigned int format, int* bpp, Uint32* rMask, Uint32* gMask, Uint32* bMask, Uint32* aMask)")]
		public delegate* unmanaged<uint, out int, out uint, out uint, out uint, out uint, SDLBool> SDL_PixelFormatEnumToMasks;
		[NativeType("Uint32 SDL_MasksToPixelFormatEnum(int bpp, Uint32 rMask, Uint32 gMask, Uint32 bMask, Uint32 aMask)")]
		public delegate* unmanaged<int, uint, uint, uint, uint, uint> SDL_MasksToPixelFormatEnum;
		[NativeType("SDL_PixelFormat* SDL_AllocFormat(unsigned int pixelFormat)")]
		public delegate* unmanaged<uint, SDL_PixelFormat*> SDL_AllocFormat;
		[NativeType("void SDL_FreeFormat(SDL_PixelFormat* format)")]
		public delegate* unmanaged<SDL_PixelFormat*, void> SDL_FreeFormat;
		[NativeType("SDL_Palette* SDL_AllocPalette(int nColors)")]
		public delegate* unmanaged<int, SDL_Palette*> SDL_AllocPalette;
		[NativeType("int SDL_SetPixelFormatPalette(SDL_PixelFormat* format, SDL_Palette* palette")]
		public delegate* unmanaged<SDL_PixelFormat*, SDL_Palette*, int> SDL_SetPixelFormatPalette;
		[NativeType("int SDL_SetPaletteColors(SDL_Palette* palette, const SDL_Color* colors, int firstColor, int nColors)")]
		public delegate* unmanaged<SDL_Palette*, SDLColor*, int, int, int> SDL_SetPaletteColors;
		[NativeType("void SDL_FreePalette(SDL_Palette* palette)")]
		public delegate* unmanaged<SDL_Palette*, void> SDL_FreePalette;
		[NativeType("Uint32 SDL_MapRGB(const SDL_PixelFormat* format, Uint8 r, Uint8 g, Uint8 b)")]
		public delegate* unmanaged<SDL_PixelFormat*, byte, byte, byte, uint> SDL_MapRGB;
		[NativeType("Uint32 SDL_MapRGBA(const SDL_PixelFormat* format, Uint8 r, Uint8 g, Uint8 b, Uint8 a)")]
		public delegate* unmanaged<SDL_PixelFormat*, byte, byte, byte, byte, uint> SDL_MapRGBA;
		[NativeType("void SDL_GetRGB(Uint32 pixel, const SDL_PixelFormat* format, Uint8* r, Uint8* g, Uint8* b)")]
		public delegate* unmanaged<uint, SDL_PixelFormat*, out byte, out byte, out byte, void> SDL_GetRGB;
		[NativeType("void SDL_GetRGBA(Uint32 pixel, const SDL_PixelFormat* format, Uint8* r, Uint8* g, Uint8* b, Uint8* a)")]
		public delegate* unmanaged<uint, SDL_PixelFormat*, out byte, out byte, out byte, out byte, void> SDL_GetRGBA;
		[NativeType("void SDL_CalculateGammaRamp(float gamma, Uint16[256] ramp)")]
		public delegate* unmanaged<float, ushort*, void> SDL_CalculateGammaRamp;

		// SDL_rwops.h

		[NativeType("SDL_RWops* SDL_RWFromFile(const char* file, const char* mode)")]
		public delegate* unmanaged<byte*, byte*, SDL_RWops*> SDL_RWFromFile;
		[NativeType("SDL_RWops* SDL_RWFromMem(void* mem, int size)")]
		public delegate* unmanaged<IntPtr, int, SDL_RWops*> SDL_RWFromMem;
		[NativeType("SDL_RWops* SDL_RWFromConstMem(void* mem, int size)")]
		public delegate* unmanaged<IntPtr, int, SDL_RWops*> SDL_RWFromConstMem;
		[NativeType("SDL_RWops* SDL_AllocRW()")]
		public delegate* unmanaged<SDL_RWops*> SDL_AllocRW;
		[NativeType("void SDL_FreeRW(SDL_RWops* context)")]
		public delegate* unmanaged<SDL_RWops*, void> SDL_FreeRW;
		[NativeType("SInt64 SDL_RWsize(SDL_RWops* context)")]
		public delegate* unmanaged<SDL_RWops*, long> SDL_RWsize;
		[NativeType("SInt64 SDL_RWseek(SDL_RWops* context, SInt64 offset, int whence)")]
		public delegate* unmanaged<SDL_RWops*, long, SDLRWWhence, long> SDL_RWseek;
		[NativeType("SInt64 SDL_RWtell(SDL_RWops* context)")]
		public delegate* unmanaged<SDL_RWops*, long> SDL_RWtell;
		[NativeType("size_t SDL_RWread(SDL_RWops* context, void* ptr, size_t size, size_t maxnum)")]
		public delegate* unmanaged<SDL_RWops*, IntPtr, nuint, nuint, nuint> SDL_RWread;
		[NativeType("size_t SDL_RWwrite(SDL_RWops* context, void* ptr, size_t size, size_t num)")]
		public delegate* unmanaged<SDL_RWops*, IntPtr, nuint, nuint, nuint> SDL_RWwrite;
		[NativeType("int SDL_RWclose(SDL_RWops* context)")]
		public delegate* unmanaged<SDL_RWops*, int> SDL_RWclose;
		[NativeType("void* SDL_LoadFile_RW(SDL_RWops* src, size_t* datasize, int freesrc)")]
		public delegate* unmanaged<SDL_RWops*, out nuint, bool, IntPtr> SDL_LoadFile_RW;
		[NativeType("void* SDL_LoadFile(const char* file, size_t* datasize)")]
		public delegate* unmanaged<byte*, out nuint, IntPtr> SDL_LoadFile;

		[NativeType("Uint8 SDL_ReadU8(SDL_RWops* src)")]
		public delegate* unmanaged<SDL_RWops*, byte> SDL_ReadU8;
		[NativeType("Uint16 SDL_ReadLE16(SDL_RWops* src)")]
		public delegate* unmanaged<SDL_RWops*, ushort> SDL_ReadLE16;
		[NativeType("Uint16 SDL_ReadBE16(SDL_RWops* src)")]
		public delegate* unmanaged<SDL_RWops*, ushort> SDL_ReadBE16;
		[NativeType("Uint32 SDL_ReadLE32(SDL_RWops* src)")]
		public delegate* unmanaged<SDL_RWops*, uint> SDL_ReadLE32;
		[NativeType("Uint32 SDL_ReadBE32(SDL_RWops* src)")]
		public delegate* unmanaged<SDL_RWops*, uint> SDL_ReadBE32;
		[NativeType("UInt64 SDL_ReadLE64(SDL_RWops* src)")]
		public delegate* unmanaged<SDL_RWops*, ulong> SDL_ReadLE64;
		[NativeType("UInt64 SDL_ReadBE64(SDL_RWops* src)")]
		public delegate* unmanaged<SDL_RWops*, ulong> SDL_ReadBE64;
		[NativeType("size_t SDL_WriteU8(SDL_RWops* dst, Uint8 value)")]
		public delegate* unmanaged<SDL_RWops*, byte, nuint> SDL_WriteU8;
		[NativeType("size_t SDL_WriteLE16(SDL_RWops* dst, Uint16 value)")]
		public delegate* unmanaged<SDL_RWops*, ushort, nuint> SDL_WriteLE16;
		[NativeType("size_t SDL_WriteBE16(SDL_RWops* dst, Uint16 value)")]
		public delegate* unmanaged<SDL_RWops*, ushort, nuint> SDL_WriteBE16;
		[NativeType("size_t SDL_WriteLE32(SDL_RWops* dst, Uint32 value)")]
		public delegate* unmanaged<SDL_RWops*, uint, nuint> SDL_WriteLE32;
		[NativeType("size_t SDL_WriteBE32(SDL_RWops* dst, Uint32 value)")]
		public delegate* unmanaged<SDL_RWops*, uint, nuint> SDL_WriteBE32;
		[NativeType("size_t SDL_WriteLE64(SDL_RWops* dst, UInt64 value)")]
		public delegate* unmanaged<SDL_RWops*, ulong, nuint> SDL_WriteLE64;
		[NativeType("size_t SDL_WriteBE64(SDL_RWops* dst, UInt64 value)")]
		public delegate* unmanaged<SDL_RWops*, ulong, nuint> SDL_WriteBE64;

		// SDL_rect.h

		[NativeType("SDL_bool SDL_HasIntersection(const SDL_Rect* a, const SDL_Rect* b)")]
		public delegate* unmanaged<in SDLRect, in SDLRect, SDLBool> SDL_HasIntersection;
		[NativeType("SDL_bool SDL_IntersectRect(const SDL_Rect* a, const SDL_Rect* b, SDL_Rect* result)")]
		public delegate* unmanaged<in SDLRect, in SDLRect, out SDLRect, SDLBool> SDL_IntersectRect;
		[NativeType("void SDL_UnionRect(const SDL_Rect* a, const SDL_Rect* b, SDL_Rect* result)")]
		public delegate* unmanaged<in SDLRect, in SDLRect, out SDLRect, void> SDL_UnionRect;
		[NativeType("SDL_bool SDL_EnclosePoints(const SDL_Point* points, int count, const SDL_Rect* clip, SDL_Rect* result)")]
		public delegate* unmanaged<SDLPoint*, int, SDLRect*, out SDLRect, SDLBool> SDL_EnclosePoints;
		[NativeType("SDL_bool SDL_IntersectRectAndLine(const SDL_Rect* rect, int* x1, int* y1, int* x2, int* y2)")]
		public delegate* unmanaged<in SDLRect, ref int, ref int, ref int, ref int, SDLBool> SDL_IntersectRectAndLine;

		// SDL_blendmode.h

		[NativeType("SDL_BlendMode SDL_ComposeCustomBlendMode(SDL_BlendFactor srcColorFactor, SDL_BlendFactor dstColorFactor, SDL_BlendOperation colorOperation, SDL_BlendFactor srcAlphaFactor, SDL_BlendFactor dstAlphaFactor, SDL_BlendOperation alphaOperation)")]
		public delegate* unmanaged<SDLBlendFactor, SDLBlendFactor, SDLBlendOperation, SDLBlendFactor, SDLBlendFactor, SDLBlendOperation, SDLBlendMode> SDL_ComposeCustomBlendMode;

		// SDL_surface.h

		[NativeType("SDL_Surface* SDL_CreateRGBSurface(Uint32 flags, int width, int height, int depth, Uint32 rmask, Uint32 gmask, Uint32 bmask, Uint32 amask)")]
		public delegate* unmanaged<uint, int, int, int, uint, uint, uint, uint, SDL_Surface*> SDL_CreateRGBSurface;
		[NativeType("SDL_Surface* SDL_CreateRGBSurfaceWithFormat(Uint32 flags, int width, int height, int depth, Uint32 format)")]
		public delegate* unmanaged<uint, int, int, int, uint, SDL_Surface*> SDL_CreateRGBSurfaceWithFormat;
		[NativeType("SDL_Surface* SDL_CreateRGBSurfaceFrom(void* pixels, int width, int height, int depth, int pitch, Uint32 rmask, Uint32 gmask, Uint32 bmask, Uint32 amask)")]
		public delegate* unmanaged<IntPtr, int, int, int, int, uint, uint, uint, uint, SDL_Surface*> SDL_CreateRGBSurfaceFrom;
		[NativeType("SDL_Surface* SDL_CreateRGBSurfaceWithFormatFrom(void* pixels, int width, int height, int depth, int pitch, Uint32 format)")]
		public delegate* unmanaged<IntPtr, int, int, int, int, uint, SDL_Surface*> SDL_CreateRGBSurfaceWithFormatFrom;
		[NativeType("void SDL_FreeSurface(SDL_Surface* surface)")]
		public delegate* unmanaged<SDL_Surface*, void> SDL_FreeSurface;
		[NativeType("int SDL_SetSurfacePalette(SDL_Surface* surface, SDL_Palette* palette)")]
		public delegate* unmanaged<SDL_Surface*, SDL_Palette*, int> SDL_SetSurfacePalette;
		[NativeType("int SDL_LockSurface(SDL_Surface* surface)")]
		public delegate* unmanaged<SDL_Surface*, int> SDL_LockSurface;
		[NativeType("void SDL_UnlockSurface(SDL_Surface* surface)")]
		public delegate* unmanaged<SDL_Surface*, void> SDL_UnlockSurface;
		[NativeType("SDL_Surface* SDL_LoadBMP_RW(SDL_RWops* src, int freesrc)")]
		public delegate* unmanaged<SDL_RWops*, int, SDL_Surface*> SDL_LoadBMP_RW;
		[NativeType("int SDL_SaveBMP_RW(SDL_Surface* surface, SDL_RWops* dst, int freedst)")]
		public delegate* unmanaged<SDL_Surface*, SDL_RWops*, int, int> SDL_SaveBMP_RW;
		[NativeType("int SDL_SetSurfaceRLE(SDL_Surface* surface, int flag)")]
		public delegate* unmanaged<SDL_Surface*, bool, int> SDL_SetSurfaceRLE;
		[NativeType("int SDL_SetColorKey(SDL_Surface* surface, int flag, Uint32 key)")]
		public delegate* unmanaged<SDL_Surface*, bool, uint, int> SDL_SetColorKey;
		[NativeType("SDL_bool SDL_HasColorKey(SDL_Surface* surface)")]
		public delegate* unmanaged<SDL_Surface*, SDLBool> SDL_HasColorKey;
		[NativeType("int SDL_GetColorKey(SDL_Surface* surface, Uint32* key)")]
		public delegate* unmanaged<SDL_Surface*, out uint, int> SDL_GetColorKey;
		[NativeType("int SDL_SetSurfaceColorMod(SDL_Surface* surface, Uint8 r, Uint8 g, Uint8 b)")]
		public delegate* unmanaged<SDL_Surface*, byte, byte, byte, int> SDL_SetSurfaceColorMod;
		[NativeType("int SDL_GetSurfaceColorMod(SDL_Surface* surface, Uint8* r, Uint8* g, Uint8* b)")]
		public delegate* unmanaged<SDL_Surface*, out byte, out byte, out byte, int> SDL_GetSurfaceColorMod;
		[NativeType("int SDL_SetSurfaceAlphaMod(SDL_Surface* surface, Uint8 alpha)")]
		public delegate* unmanaged<SDL_Surface*, byte, int> SDL_SetSurfaceAlphaMod;
		[NativeType("int SDL_GetSurfaceAlphaMod(SDL_Surface* surface, Uint8* alpha)")]
		public delegate* unmanaged<SDL_Surface*, out byte, int> SDL_GetSurfaceAlphaMod;
		[NativeType("int SDL_SetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode blendMode)")]
		public delegate* unmanaged<SDL_Surface*, SDLBlendMode, int> SDL_SetSurfaceBlendMode;
		[NativeType("int SDL_GetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode* blendMode)")]
		public delegate* unmanaged<SDL_Surface*, out SDLBlendMode, int> SDL_GetSurfaceBlendMode;
		[NativeType("SDL_bool SDL_SetClipRect(SDL_Surface* surface, const SDL_Rect* rect)")]
		public delegate* unmanaged<SDL_Surface*, SDLRect*, SDLBool> SDL_SetClipRect;
		[NativeType("void SDL_GetClipRect(SDL_Surface* surface, SDL_Rect* rect)")]
		public delegate* unmanaged<SDL_Surface*, out SDLRect, void> SDL_GetClipRect;
		[NativeType("SDL_Surface* SDL_DuplicateSurface(SDL_Surface* surface)")]
		public delegate* unmanaged<SDL_Surface*, SDL_Surface*> SDL_DuplicateSurface;
		[NativeType("SDL_Surface* SDL_ConvertSurface(SDL_Surface* src, const SDL_PixelFormat* fmt, Uint32 flags)")]
		public delegate* unmanaged<SDL_Surface*, SDL_PixelFormat*, uint, SDL_Surface*> SDL_ConvertSurface;
		[NativeType("SDL_Surface* SDL_ConvertSurfaceFormat(SDL_Surface* src, Uint32 pixelFormat, Uint32 flags)")]
		public delegate* unmanaged<SDL_Surface*, uint, uint, SDL_Surface*> SDL_ConvertSurfaceFormat;
		[NativeType("int SDL_ConvertPixels(int width, int height, Uint32 srcFormat, void* src, int srcPitch, Uint32 dstFormat, void* dst, int dstPitch)")]
		public delegate* unmanaged<int, int, uint, IntPtr, int, uint, IntPtr, int, int> SDL_ConvertPixels;
		[NativeType("int SDL_FillRect(SDL_Surface* dst, const SDL_Rect* rect, Uint32 color)")]
		public delegate* unmanaged<SDL_Surface*, SDLRect*, uint, int> SDL_FillRect;
		[NativeType("int SDL_FillRects(SDL_Surface* dst, const SDL_Rect* rects, int count, Uint32 color)")]
		public delegate* unmanaged<SDL_Surface*, SDLRect*, int, uint, int> SDL_FillRects;
		[NativeType("int SDL_UpperBlit(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect)")]
		public delegate* unmanaged<SDL_Surface*, SDLRect*, SDL_Surface*, in SDLRect, int> SDL_UpperBlit;
		[NativeType("int SDL_LowerBlit(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect)")]
		public delegate* unmanaged<SDL_Surface*, SDLRect*, SDL_Surface*, in SDLRect, int> SDL_LowerBlit;
		[NativeType("int SDL_SoftStretch(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect)")]
		public delegate* unmanaged<SDL_Surface*, in SDLRect, SDL_Surface*, in SDLRect, int> SDL_SoftStretch;
		[NativeType("int SDL_UpperBlitScaled(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect)")]
		public delegate* unmanaged<SDL_Surface*, in SDLRect, SDL_Surface*, in SDLRect, int> SDL_UpperBlitScaled;
		[NativeType("int SDL_LowerBlitScaled(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect)")]
		public delegate* unmanaged<SDL_Surface*, in SDLRect, SDL_Surface*, in SDLRect, int>  SDL_LowerBlitScaled;
		[NativeType("void SDL_SetYUVConverisonMode(SDL_YUVConversionMode mode)")]
		public delegate* unmanaged<SDLYUVConversionMode, void> SDL_SetYUVConversionMode;
		[NativeType("SDL_YUVConversionMode SDL_GetYUVConversionMode()")]
		public delegate* unmanaged<SDLYUVConversionMode> SDL_GetYUVConversionMode;
		[NativeType("SDL_YUVConversionMode SDL_GetYUVConversionModeForResolution(int width, int height)")]
		public delegate* unmanaged<int, int, SDLYUVConversionMode> SDL_GetYUVConversionModeForResolution;

		// SDL_video.h

		[NativeType("int SDL_GetNumVideoDrivers()")]
		public delegate* unmanaged<int> SDL_GetNumVideoDrivers;
		[NativeType("const char* SDL_GetVideoDriver(int index)")]
		public delegate* unmanaged<int, IntPtr> SDL_GetVideoDriver;
		[NativeType("int SDL_VideoInit(const char* driverName)")]
		public delegate* unmanaged<byte*, int> SDL_VideoInit;
		[NativeType("void SDL_VideoQuit()")]
		public delegate* unmanaged<void> SDL_VideoQuit;
		[NativeType("const char* SDL_GetCurrentVideoDriver()")]
		public delegate* unmanaged<IntPtr> SDL_GetCurrentVideoDriver;
		[NativeType("int SDL_GetNumVideoDisplays()")]
		public delegate* unmanaged<int> SDL_GetNumVideoDisplays;
		[NativeType("const char* SDL_GetDisplayName(int displayIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_GetDisplayName;
		[NativeType("int SDL_GetDisplayBounds(int displayIndex, SDL_Rect* rect)")]
		public delegate* unmanaged<int, out SDLRect, int> SDL_GetDisplayBounds;
		[NativeType("int SDL_GetDisplayUsableBounds(int displayIndex, SDL_Rect* rect)")]
		public delegate* unmanaged<int, out SDLRect, int> SDL_GetDisplayUsableBounds;
		[NativeType("int SDL_GetDisplayDPI(int displayIndex, float* ddpi, float* hdpi, float* vdpi)")]
		public delegate* unmanaged<int, out float, out float, out float, int> SDL_GetDisplayDPI;
		[NativeType("SDL_DisplayOrientation SDL_GetDisplayOrientation(int displayIndex)")]
		public delegate* unmanaged<int, SDLDisplayOrientation> SDL_GetDisplayOrientation;
		[NativeType("int SDL_GetNumDisplayModes(int displayIndex)")]
		public delegate* unmanaged<int, int> SDL_GetNumDisplayModes;
		[NativeType("int SDL_GetDisplayMode(int displayIndex, int modeIndex, SDL_DisplayMode* mode)")]
		public delegate* unmanaged<int, int, out SDLDisplayMode, int> SDL_GetDisplayMode;
		[NativeType("int SDL_GetDesktopDisplayMode(int displayIndex, SDL_DisplayMode* mode)")]
		public delegate* unmanaged<int, out SDLDisplayMode, int> SDL_GetDesktopDisplayMode;
		[NativeType("int SDL_GetCurrentDisplayMode(int displayIndex, SDL_DisplayMode* mode)")]
		public delegate* unmanaged<int, out SDLDisplayMode, int> SDL_GetCurrentDisplayMode;
		[NativeType("SDL_DisplayMode* SDL_GetClosestDisplayMode(int displayIndex, const SDL_DisplayMode* mode, SDL_DisplayMode* closest)")]
		public delegate* unmanaged<int, in SDLDisplayMode, out SDLDisplayMode, SDLDisplayMode*> SDL_GetClosestDisplayMode;
		[NativeType("int SDL_GetWindowDisplayIndex(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, int> SDL_GetWindowDisplayIndex;
		[NativeType("int SDL_SetWindowDisplayMode(SDL_Window* window, const SDL_DisplayMode* mode)")]
		public delegate* unmanaged<IntPtr, in SDLDisplayMode, int> SDL_SetWindowDisplayMode;
		[NativeType("int SDL_GetWindowDisplayMode(SDL_Window* window, SDL_DisplayMode* mode)")]
		public delegate* unmanaged<IntPtr, out SDLDisplayMode, int> SDL_GetWindowDisplayMode;
		[NativeType("Uint32 SDL_GetWindowPixelFormat(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, uint> SDL_GetWindowPixelFormat;
		[NativeType("SDL_Window* SDL_CreateWindow(const char* title, int x, int y, int w, int h, Uint32 flags)")]
		public delegate* unmanaged<byte*, int, int, int, int, uint, IntPtr> SDL_CreateWindow;
		[NativeType("SDL_Window* SDL_CreateWindowFrom(void* data)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_CreateWindowFrom;
		[NativeType("Uint32 SDL_GetWindowID(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, uint> SDL_GetWindowID;
		[NativeType("SDL_Window* SDL_GetWindowFromID(Uint32 id)")]
		public delegate* unmanaged<uint, IntPtr> SDL_GetWindowFromID;
		[NativeType("Uint32 SDL_GetWindowFlags(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, uint> SDL_GetWindowFlags;
		[NativeType("void SDL_SetWindowTitle(SDL_Window* window, const char* title)")]
		public delegate* unmanaged<IntPtr, byte*, void> SDL_SetWindowTitle;
		[NativeType("const char* SDL_GetWindowTitle(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_GetWindowTitle;
		[NativeType("void SDL_SetWindowIcon(SDL_Window* window, SDL_Surface* icon)")]
		public delegate* unmanaged<IntPtr, SDL_Surface*, void> SDL_SetWindowIcon;
		[NativeType("void* SDL_SetWindowData(SDL_Window* window, const char* name, void* userdata)")]
		public delegate* unmanaged<IntPtr, byte*, IntPtr, IntPtr> SDL_SetWindowData;
		[NativeType("void* SDL_GetWindowData(SDL_Window* window, const char* name)")]
		public delegate* unmanaged<IntPtr, byte*, IntPtr> SDL_GetWindowData;
		[NativeType("void SDL_SetWindowPosition(SDL_Window* window, int x, int y)")]
		public delegate* unmanaged<IntPtr, int, int, void> SDL_SetWindowPosition;
		[NativeType("void SDL_GetWindowPosition(SDL_Window* window, int* x, int* y)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> SDL_GetWindowPosition;
		[NativeType("void SDL_SetWindowSize(SDL_Window* window, int w, int h)")]
		public delegate* unmanaged<IntPtr, int, int, void> SDL_SetWindowSize;
		[NativeType("void SDL_GetWindowSize(SDL_Window* window, int* w, int* h)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> SDL_GetWindowSize;
		[NativeType("int SDL_GetWindowBordersSize(SDL_Window* window, int* top, int* left, int* bottom, int* right)")]
		public delegate* unmanaged<IntPtr, out int, out int, out int, out int, int> SDL_GetWindowBordersSize;
		[NativeType("void SDL_SetWindowMinimumSize(SDL_Window* window, int minw, int minh)")]
		public delegate* unmanaged<IntPtr, int, int, void> SDL_SetWindowMinimumSize;
		[NativeType("void SDL_GetWindowMinimumSize(SDL_Window* window, int* minw, int* minh)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> SDL_GetWindowMinimumSize;
		[NativeType("void SDL_SetWindowMaximumSize(SDL_Window* window, int maxw, int maxh)")]
		public delegate* unmanaged<IntPtr, int, int, void> SDL_SetWindowMaximumSize;
		[NativeType("void SDL_GetWindowMaximumSize(SDL_Window* window, int* maxw, int* maxh)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> SDL_GetWindowMaximumSize;
		[NativeType("void SDL_SetWindowBordered(SDL_Window* window, SDL_bool bordered)")]
		public delegate* unmanaged<IntPtr, SDLBool, void> SDL_SetWindowBordered;
		[NativeType("void SDL_SetWindowResizable(SDL_Window* window, SDL_bool resizable)")]
		public delegate* unmanaged<IntPtr, SDLBool, void> SDL_SetWindowResizable;
		[NativeType("void SDL_ShowWindow(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, void> SDL_ShowWindow;
		[NativeType("void SDL_HideWindow(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, void> SDL_HideWindow;
		[NativeType("void SDL_RaiseWindow(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, void> SDL_RaiseWindow;
		[NativeType("void SDL_MaximizeWindow(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, void> SDL_MaximizeWindow;
		[NativeType("void SDL_MinimizeWindow(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, void> SDL_MinimizeWindow;
		[NativeType("void SDL_RestoreWindow(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, void> SDL_RestoreWindow;
		[NativeType("int SDL_SetWindowFullscreen(SDL_Window* window, Uint32 flags)")]
		public delegate* unmanaged<IntPtr, uint, int> SDL_SetWindowFullscreen;
		[NativeType("SDL_Surface* SDL_GetWindowSurface(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, SDL_Surface*> SDL_GetWindowSurface;
		[NativeType("int SDL_UpdateWindowSurface(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, int> SDL_UpdateWindowSurface;
		[NativeType("int SDL_UpdateWindowSurfaceRects(SDL_Window* window, const SDL_Rect* rects, int numRects)")]
		public delegate* unmanaged<IntPtr, SDLRect*, int, int> SDL_UpdateWindowSurfaceRects;
		[NativeType("void SDL_SetWindowGrab(SDL_Window* window, SDL_bool grabbed)")]
		public delegate* unmanaged<IntPtr, SDLBool, void> SDL_SetWindowGrab;
		[NativeType("SDL_bool SDL_GetWindowGrab(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, SDLBool> SDL_GetWindowGrab;
		[NativeType("SDL_Window* SDL_GetGrabbedWindow()")]
		public delegate* unmanaged<IntPtr> SDL_GetGrabbedWindow;
		[NativeType("int SDL_SetWindowBrightness(SDL_Window* window, float brightness)")]
		public delegate* unmanaged<IntPtr, float, int> SDL_SetWindowBrightness;
		[NativeType("float SDL_GetWindowBrightness(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, float> SDL_GetWindowBrightness;
		[NativeType("int SDL_SetWindowOpacity(SDL_Window* window, float opacity)")]
		public delegate* unmanaged<IntPtr, float, int> SDL_SetWindowOpacity;
		[NativeType("int SDL_GetWindowOpacity(SDL_Window* window, float* opacity)")]
		public delegate* unmanaged<IntPtr, out float, int> SDL_GetWindowOpacity;
		[NativeType("int SDL_SetWindowModalFor(SDL_Window* modalWindow, SDL_Window* parentWindow)")]
		public delegate* unmanaged<IntPtr, IntPtr, int> SDL_SetWindowModalFor;
		[NativeType("int SDL_SetWindowInputFocus(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, int> SDL_SetWindowInputFocus;
		[NativeType("int SDL_SetWindowGammaRamp(SDL_Window* window, const Uint16* red, const Uint16* green, const Uint16* blue)")]
		public delegate* unmanaged<IntPtr, ushort*, ushort*, ushort*, int> SDL_SetWindowGammaRamp;
		[NativeType("int SDL_GetWindowGammaRamp(SDL_Window* window, Uint16* red, Uint16* green, Uint16* blue)")]
		public delegate* unmanaged<IntPtr, ushort*, ushort*, ushort*, int> SDL_GetWindowGammaRamp;
		[NativeType("int SDL_SetWindowHitTest(SDL_Window* window, SDL_HitTest callback, void* callbackData)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr, int> SDL_SetWindowHitTest;
		[NativeType("void SDL_DestroyWindow(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, void> SDL_DestroyWindow;
		[NativeType("SDL_bool SDL_IsScreenSaverEnabled()")]
		public delegate* unmanaged<SDLBool> SDL_IsScreenSaverEnabled;
		[NativeType("void SDL_EnableScreenSaver()")]
		public delegate* unmanaged<void> SDL_EnableScreenSaver;
		[NativeType("void SDL_DisableScreenSaver()")]
		public delegate* unmanaged<void> SDL_DisableScreenSaver;

		[NativeType("int SDL_GL_LoadLibrary(const char* path)")]
		public delegate* unmanaged<byte*, int> SDL_GL_LoadLibrary;
		[NativeType("void* SDL_GL_GetProcAddress(const char* proc)")]
		public delegate* unmanaged<byte*, IntPtr> SDL_GL_GetProcAddress;
		[NativeType("void SDL_GL_UnloadLibrary()")]
		public delegate* unmanaged<void> SDL_GL_UnloadLibrary;
		[NativeType("SDL_bool SDL_GL_ExtensionSupported(const char* extension)")]
		public delegate* unmanaged<byte*, SDLBool> SDL_GL_ExtensionSupported;
		[NativeType("void SDL_GL_ResetAttributes()")]
		public delegate* unmanaged<void> SDL_GL_ResetAttributes;
		[NativeType("int SDL_GL_SetAttribute(SDL_GLAttr attr, int value)")]
		public delegate* unmanaged<SDLGLAttr, int, int> SDL_GL_SetAttribute;
		[NativeType("int SDL_GL_GetAttribute(SDL_GLAttr attr, int* value)")]
		public delegate* unmanaged<SDLGLAttr, out int, int> SDL_GL_GetAttribute;
		[NativeType("SDL_GLContext SDL_GL_CreateContext(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, SDL_GLContext> SDL_GL_CreateContext;
		[NativeType("int SDL_GL_MakeCurrent(SDL_Window* window, SDL_GLContext context)")]
		public delegate* unmanaged<IntPtr, SDL_GLContext, int> SDL_GL_MakeCurrent;
		[NativeType("SDL_Window* SDL_GL_GetCurrentWindow()")]
		public delegate* unmanaged<IntPtr> SDL_GL_GetCurrentWindow;
		[NativeType("SDL_GLContext SDL_GL_GetCurrentContext()")]
		public delegate* unmanaged<SDL_GLContext> SDL_GL_GetCurrentContext;
		[NativeType("void SDL_GL_GetDrawableSize(SDL_Window* window, int* w, int* h)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> SDL_GL_GetDrawableSize;
		[NativeType("int SDL_GL_SetSwapInterval(int interval)")]
		public delegate* unmanaged<int, int> SDL_GL_SetSwapInterval;
		[NativeType("int SDL_GL_GetSwapInterval()")]
		public delegate* unmanaged<int> SDL_GL_GetSwapInterval;
		[NativeType("void SDL_GL_SwapWindow(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, void> SDL_GL_SwapWindow;
		[NativeType("void SDL_GL_DeleteContext(SDL_GLContext context)")]
		public delegate* unmanaged<SDL_GLContext, void> SDL_GL_DeleteContext;

		// SDL_keyboard.h

		[NativeType("SDL_Window* SDL_GetKeyboardFocus()")]
		public delegate* unmanaged<IntPtr> SDL_GetKeyboardFocus;
		[NativeType("const Uint8* SDL_GetKeyboardState(int* numKeys)")]
		public delegate* unmanaged<out int, IntPtr> SDL_GetKeyboardState;
		[NativeType("SDL_Keymod SDL_GetModState()")]
		public delegate* unmanaged<SDLKeymod> SDL_GetModState;
		[NativeType("void SDL_SetModState(SDL_Keymod modstate)")]
		public delegate* unmanaged<SDLKeymod, void> SDL_SetModState;
		[NativeType("SDL_Keycode SDL_GetKeyFromScancode(SDL_Scancode scancode)")]
		public delegate* unmanaged<SDLScancode, SDLKeycode> SDL_GetKeyFromScancode;
		[NativeType("SDL_Scancode SDL_GetScancodeFromKey(SDL_Keycode key)")]
		public delegate* unmanaged<SDLKeycode, SDLScancode> SDL_GetScancodeFromKey;
		[NativeType("const char* SDL_GetScancodeName(SDL_Scancode scancode)")]
		public delegate* unmanaged<SDLScancode, IntPtr> SDL_GetScancodeName;
		[NativeType("SDL_Scancode SDL_GetScancodeFromName(const char* name)")]
		public delegate* unmanaged<byte*, SDLScancode> SDL_GetScancodeFromName;
		[NativeType("const char* SDL_GetKeyName(SDL_Keycode key)")]
		public delegate* unmanaged<SDLKeycode, IntPtr> SDL_GetKeyName;
		[NativeType("SDL_Keycode SDL_GetKeyFromName(const char* name)")]
		public delegate* unmanaged<byte*, SDLKeycode> SDL_GetKeyFromName;
		[NativeType("void SDL_StartTextInput()")]
		public delegate* unmanaged<void> SDL_StartTextInput;
		[NativeType("SDL_bool SDL_IsTextInputActive()")]
		public delegate* unmanaged<SDLBool> SDL_IsTextInputActive;
		[NativeType("void SDL_StopTextInput()")]
		public delegate* unmanaged<void> SDL_StopTextInput;
		[NativeType("void SDL_SetTextInputRect(const SDL_Rect* rect)")]
		public delegate* unmanaged<in SDLRect, void> SDL_SetTextInputRect;
		[NativeType("SDL_bool SDL_HasScreenKeyboardSupport()")]
		public delegate* unmanaged<SDLBool> SDL_HasScreenKeyboardSupport;
		[NativeType("SDL_bool SDL_IsScreenKeyboardDown(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, SDLBool> SDL_IsScreenKeyboardShown;

		// SDL_mouse.h

		[NativeType("SDL_Window* SDL_GetMouseFocus()")]
		public delegate* unmanaged<IntPtr> SDL_GetMouseFocus;
		[NativeType("Uint32 SDL_GetMouseState(int* x, int* y)")]
		public delegate* unmanaged<out int, out int, uint> SDL_GetMouseState;
		[NativeType("Uint32 SDL_GetGlobalMouseState(int* x, int* y)")]
		public delegate* unmanaged<out int, out int, uint> SDL_GetGlobalMouseState;
		[NativeType("Uint32 SDL_GetRelativeMouseState(int* x, int* y)")]
		public delegate* unmanaged<out int, out int, uint> SDL_GetRelativeMouseState;
		[NativeType("void SDL_WarpMouseInWindow(SDL_Window* window, int x, int y)")]
		public delegate* unmanaged<IntPtr, int, int, void> SDL_WarpMouseInWindow;
		[NativeType("int SDL_WarpMouseGlobal(int x, int y)")]
		public delegate* unmanaged<int, int, int> SDL_WarpMouseGlobal;
		[NativeType("int SDL_SetRelativeMouseMode(SDL_bool enabled)")]
		public delegate* unmanaged<SDLBool, int> SDL_SetRelativeMouseMode;
		[NativeType("int SDL_CaptureMouse(SDL_bool enabled)")]
		public delegate* unmanaged<SDLBool, int> SDL_CaptureMouse;
		[NativeType("SDL_bool SDL_GetRelativeMouseMode()")]
		public delegate* unmanaged<SDLBool> SDL_GetRelativeMouseMode;
		[NativeType("SDL_Cursor* SDL_CreateCursor(const Uint8* data, const Uint8* mask, int w, int h, int hotX, int hotY")]
		public delegate* unmanaged<byte*, byte*, int, int, int, int, IntPtr> SDL_CreateCursor;
		[NativeType("SDL_Cursor* SDL_CreateColorCursor(const SDL_Surface* surface, int hotX, int hotY)")]
		public delegate* unmanaged<SDL_Surface*, int, int, IntPtr> SDL_CreateColorCursor;
		[NativeType("SDL_Cursor* SDL_CreateSystemCursor(SDL_SystemCursor id)")]
		public delegate* unmanaged<SDLSystemCursor, IntPtr> SDL_CreateSystemCursor;
		[NativeType("void SDL_SetCursor(SDL_Cursor* cursor)")]
		public delegate* unmanaged<IntPtr, void> SDL_SetCursor;
		[NativeType("SDL_Cursor* SDL_GetCursor()")]
		public delegate* unmanaged<IntPtr> SDL_GetCursor;
		[NativeType("SDL_Cursor* SDL_GetDefaultCursor()")]
		public delegate* unmanaged<IntPtr> SDL_GetDefaultCursor;
		[NativeType("int SDL_ShowCursor(int toggle)")]
		public delegate* unmanaged<int, int> SDL_ShowCursor;
		[NativeType("void SDL_FreeCursor(SDL_Cursor* cursor)")]
		public delegate* unmanaged<IntPtr, void> SDL_FreeCursor;

		// SDL_joystick.h

		[NativeType("void SDL_LockJoysticks()")]
		public delegate* unmanaged<void> SDL_LockJoysticks;
		[NativeType("void SDL_UnlockJoysticks()")]
		public delegate* unmanaged<void> SDL_UnlockJoysticks;
		[NativeType("int SDL_NumJoysticks()")]
		public delegate* unmanaged<int> SDL_NumJoysticks;
		[NativeType("const char* SDL_JoystickNameForIndex(int deviceIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_JoystickNameForIndex;
		[NativeType("int SDL_JoystickGetDevicePlayerIndex(int deviceIndex)")]
		public delegate* unmanaged<int, int> SDL_JoystickGetDevicePlayerIndex;
		[NativeType("SDL_JoystickGUID SDL_JoystickGetDeviceGUID(int deviceIndex)")]
		public delegate* unmanaged<int, SDL_JoystickGUID> SDL_JoystickGetDeviceGUID;
		[NativeType("Uint16 SDL_JoystickGetDeviceVendor(int deviceIndex)")]
		public delegate* unmanaged<int, ushort> SDL_JoystickGetDeviceVendor;
		[NativeType("Uint16 SDL_JoystickGetDeviceProduct(int deviceIndex)")]
		public delegate* unmanaged<int, ushort> SDL_JoystickGetDeviceProduct;
		[NativeType("Uint16 SDL_JoystickGetDeviceProductVersion(int deviceIndex)")]
		public delegate* unmanaged<int, ushort> SDL_JoystickGetDeviceProductVersion;
		[NativeType("SDL_JoystickType SDL_JoystickGetDeviceType(int deviceIndex)")]
		public delegate* unmanaged<int, SDLJoystickType> SDL_JoystickGetDeviceType;
		[NativeType("int SDL_JoystickGetDeviceInstanceID(int deviceIndex)")]
		public delegate* unmanaged<int, int> SDL_JoystickGetDeviceInstanceID;
		[NativeType("SDL_Joystick* SDL_JoystickOpen(int deviceIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_JoystickOpen;
		[NativeType("SDL_Joystick* SDL_JoystickFromInstanceID(int instanceID)")]
		public delegate* unmanaged<int, IntPtr> SDL_JoystickFromInstanceID;
		[NativeType("SDL_Joystick* SDL_JoystickFromPlayerIndex(int playerIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_JoystickFromPlayerIndex;
		[NativeType("int SDL_JoystickAttachVirtual(SDL_JoystickType type, int nAxes, int nButtons, int nHats)")]
		public delegate* unmanaged<SDLJoystickType, int, int, int, int> SDL_JoystickAttachVirtual;
		[NativeType("int SDL_JoystickDetachVirtual(int deviceIndex)")]
		public delegate* unmanaged<int, int> SDL_JoystickDetachVirtual;
		[NativeType("SDL_bool SDL_JoystickIsVirtual(int deviceIndex)")]
		public delegate* unmanaged<int, SDLBool> SDL_JoystickIsVirtual;
		[NativeType("int SDL_JoystickSetVirtualAxis(SDL_Joystick* joystick, int axis, Sint16 value)")]
		public delegate* unmanaged<IntPtr, int, short, int> SDL_JoystickSetVirtualAxis;
		[NativeType("int SDL_JoystickSetVirtualButton(SDL_Joystick* joystick, int button, SDL_ButtonState state)")]
		public delegate* unmanaged<IntPtr, int, SDLButtonState, int> SDL_JoystickSetVirtualButton;
		[NativeType("int SDL_JoystickSetVirtualHat(SDL_Joystick* joystick, int hat, SDL_Hat state)")]
		public delegate* unmanaged<IntPtr, int, SDLHat, int> SDL_JoystickSetVirtualHat;
		[NativeType("const char* SDL_JoystickName(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_JoystickName;
		[NativeType("int SDL_JoystickGetPlayerIndex(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, int> SDL_JoystickGetPlayerIndex;
		[NativeType("void SDL_JoystickSetPlayerIndex(SDL_Joystick joystick, int playerIndex)")]
		public delegate* unmanaged<IntPtr, int, void> SDL_JoystickSetPlayerIndex;
		[NativeType("SDL_JoysticGUID SDL_JoystickGetGUID(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, SDL_JoystickGUID> SDL_JoystickGetGUID;
		[NativeType("Uint16 SDL_JoystickGetVendor(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, ushort> SDL_JoystickGetVendor;
		[NativeType("Uint16 SDL_JoystickGetProduct(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, ushort> SDL_JoystickGetProduct;
		[NativeType("Uint16 SDL_JoystickGetProductVersion(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, ushort> SDL_JoystickGetProductVersion;
		[NativeType("const char* SDL_JoystickGetSerial(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_JoystickGetSerial;
		[NativeType("SDL_JoystickType SDL_JoystickGetType(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, SDLJoystickType> SDL_JoystickGetType;
		[NativeType("void SDL_JoystickGetGUIDString(SDL_JoystickGUID guid, char* pszGUID, int cbGUID)")]
		public delegate* unmanaged<SDL_JoystickGUID, byte*, int, void> SDL_JoystickGetGUIDString;
		[NativeType("SDL_JoystickGUID SDL_JoystickGetGUIDFromString(const char* pchGUID)")]
		public delegate* unmanaged<byte*, SDL_JoystickGUID> SDL_JoystickGetGUIDFromString;
		[NativeType("SDL_bool SDL_JoystickGetAttached(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, SDLBool> SDL_JoystickGetAttached;
		[NativeType("int SDL_JoystickInstanceID(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, int> SDL_JoystickInstanceID;
		[NativeType("int SDL_JoystickNumAxes(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, int> SDL_JoystickNumAxes;
		[NativeType("int SDL_JoystickNumBalls(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, int> SDL_JoystickNumBalls;
		[NativeType("int SDL_JoystickNumHats(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, int> SDL_JoystickNumHats;
		[NativeType("int SDL_JoystickNumButtons(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, int> SDL_JoystickNumButtons;
		[NativeType("void SDL_JoystickUpdate()")]
		public delegate* unmanaged<void> SDL_JoystickUpdate;
		[NativeType("int SDL_JoystickEventState(int state)")]
		public delegate* unmanaged<int, int> SDL_JoystickEventState;
		[NativeType("Sint16 SDL_JoystickGetAxis(SDL_Joystick* joystick, int axis)")]
		public delegate* unmanaged<IntPtr, int, short> SDL_JoystickGetAxis;
		[NativeType("SDL_bool SDL_JoystickGetAxisInitialState(SDL_Joystick* joystick, int axis, Sint16* state)")]
		public delegate* unmanaged<IntPtr, int, out short, SDLBool> SDL_JoystickGetAxisInitialState;
		[NativeType("SDL_Hat SDL_JoystickGetHat(SDL_Joystick* joystick, int hat)")]
		public delegate* unmanaged<IntPtr, int, SDLHat> SDL_JoystickGetHat;
		[NativeType("int SDL_JoystickGetBall(SDL_Joystick* joystick, int ball, int* dx, int* dy)")]
		public delegate* unmanaged<IntPtr, int, out int, out int, int> SDL_JoystickGetBall;
		[NativeType("SDL_ButtonState SDL_JoystickGetButton(SDL_Joystick* joystick, int button)")]
		public delegate* unmanaged<IntPtr, int, SDLButtonState> SDL_JoystickGetButton;
		[NativeType("int SDL_JoystickRumble(SDL_Joystick* joystick, Uint16 lowFreqRumble, Uint16 highFreqRumble, Uint32 durationMS)")]
		public delegate* unmanaged<IntPtr, ushort, ushort, uint, int> SDL_JoystickRumble;
		[NativeType("int SDL_JoystickRumbleTriggers(SDL_Joystick* joystick, Uint16 leftRumble, Uint16 rightRumble, Uint32 durationMS)")]
		public delegate* unmanaged<IntPtr, ushort, ushort, uint, int> SDL_JoystickRumbleTriggers;
		[NativeType("SDL_bool SDL_JoystickHasLED(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, SDLBool> SDL_JoystickHasLED;
		[NativeType("int SDL_JoystickSetLED(SDL_Joystick* joystick, Uint8 red, Uint8 green, Uint8 blue)")]
		public delegate* unmanaged<IntPtr, byte, byte, byte, int> SDL_JoystickSetLED;
		[NativeType("void SDL_JoystickClose(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, void> SDL_JoystickClose;
		[NativeType("SDL_JoystickPowerLevel SDL_JoystickCurrentPowerLevel(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, SDLJoystickPowerLevel> SDL_JoystickCurrentPowerLevel;

		// SDL_gamecontroller.h

		[NativeType("int SDL_GameControllerAddMappingsFromRW(SDL_RWops* rw, int freerw)")]
		public delegate* unmanaged<SDL_RWops*, bool, int> SDL_GameControllerAddMappingsFromRW;
		[NativeType("int SDL_GameControllerAddMapping(const char* mappingStr)")]
		public delegate* unmanaged<byte*, int> SDL_GameControllerAddMapping;
		[NativeType("int SDL_GameControllerNumMappings()")]
		public delegate* unmanaged<int> SDL_GameControllerNumMappings;
		// Note: Must free after use
		[NativeType("const char* SDL_GameControllerMappingForIndex(int mappingIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_GameControllerMappingForIndex;
		// Note: Must free after use
		[NativeType("const char* SDL_GameControllerMappingForGUID(SDL_JoystickGUID)")]
		public delegate* unmanaged<SDL_JoystickGUID, IntPtr> SDL_GameControllerMappingForGUID;
		// Note: Must free after use
		[NativeType("const char* SDL_GameControllerMapping(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_GameControllerMapping;
		[NativeType("SDL_bool SDL_IsGameController(int joystickIndex)")]
		public delegate* unmanaged<int, SDLBool> SDL_IsGameController;
		[NativeType("const char* SDL_GameControllerNameForIndex(int joystickIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_GameControllerNameForIndex;
		[NativeType("SDL_GameControllerType SDL_GameControllerTypeForIndex(int joystickIndex)")]
		public delegate* unmanaged<int, SDLGameControllerType> SDL_GameControllerTypeForIndex;
		// Note: Must free after use
		[NativeType("const char* SDL_GameControllerMappingForDeviceIndex(int joystickIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_GameControllerMappingForDeviceIndex;
		[NativeType("SDL_GameController* SDL_GameControllerOpen(int joystickIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_GameControllerOpen;
		[NativeType("SDL_GameController* SDL_GameControllerFromInstanceID(int joystickID)")]
		public delegate* unmanaged<int, IntPtr> SDL_GameControllerFromInstanceID;
		[NativeType("SDL_GameController* SDL_GameControllerFromPlayerIndex(int playerIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_GameControllerFromPlayerIndex;
		[NativeType("const char* SDL_GameControllerName(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_GameControllerName;
		[NativeType("SDL_GameControllerType SDL_GameControllerGetType(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, SDLGameControllerType> SDL_GameControllerGetType;
		[NativeType("int SDL_GameControllerGetPlayerIndex(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, int> SDL_GameControllerGetPlayerIndex;
		[NativeType("void SDL_GameControllerSetPlayerIndex(SDL_GameController* gameController, int playerIndex)")]
		public delegate* unmanaged<IntPtr, int, void> SDL_GameControllerSetPlayerIndex;
		[NativeType("Uint16 SDL_GameControllerGetVendor(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, ushort> SDL_GameControllerGetVendor;
		[NativeType("Uint16 SDL_GameControllerGetProduct(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, ushort> SDL_GameControllerGetProduct;
		[NativeType("Uint16 SDL_GameControllerGetProductVersion(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, ushort> SDL_GameControllerGetProductVersion;
		[NativeType("const char* SDL_GameControllerGetSerial(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_GameControllerGetSerial;
		[NativeType("SDL_bool SDL_GameControllerGetAttached(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, SDLBool> SDL_GameControllerGetAttached;
		[NativeType("SDL_Joystick* SDL_GameControllerGetJoystick(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_GameControllerGetJoystick;
		[NativeType("int SDL_GameControllerEventState(int state)")]
		public delegate* unmanaged<int, int> SDL_GameControllerEventState;
		[NativeType("void SDL_GameControllerUpdate()")]
		public delegate* unmanaged<void> SDL_GameControllerUpdate;
		[NativeType("SDL_GameControllerAxis SDL_GameControllerGetAxisFromString(const char* pchString)")]
		public delegate* unmanaged<byte*, SDLGameControllerAxis> SDL_GameControllerGetAxisFromString;
		[NativeType("const char* SDL_GameControllerGetStringForAxis(SDL_GameControllerAxis axis)")]
		public delegate* unmanaged<SDLGameControllerAxis, IntPtr> SDL_GameControllerGetStringForAxis;
		[NativeType("SDL_GameControllerButtonBind SDL_GameControllerGetBindForAxis(SDL_GameController* gameController, SDL_GameControllerAxis axis)")]
		public delegate* unmanaged<IntPtr, SDLGameControllerAxis, SDLGameControllerButtonBind> SDL_GameControllerGetBindForAxis;
		[NativeType("SDL_bool SDL_GameControllerHasAxis(SDL_GameController* gameController, SDL_GameControllerAxis axis)")]
		public delegate* unmanaged<IntPtr, SDLGameControllerAxis, SDLBool> SDL_GameControllerHasAxis;
		[NativeType("Sint16 SDL_GameControllerGetAxis(SDL_GameController* gameController, SDL_GameControllerAxis axis)")]
		public delegate* unmanaged<IntPtr, SDLGameControllerAxis, short> SDL_GameControllerGetAxis;
		[NativeType("SDL_GameControllerButton SDL_GameControllerGetButtonFromString(const char* pchString)")]
		public delegate* unmanaged<byte*, SDLGameControllerButton> SDL_GameControllerGetButtonFromString;
		[NativeType("const char* SDL_GameControllerGetStringForButton(SDL_GameControllerButton button)")]
		public delegate* unmanaged<SDLGameControllerButton, IntPtr> SDL_GameControllerGetStringForButton;
		[NativeType("SDL_GameControllerButtonBind SDL_GameControllerGetBindForButton(SDL_GameController* gameController, SDL_GameControllerButton button)")]
		public delegate* unmanaged<IntPtr, SDLGameControllerButton, SDLGameControllerButtonBind> SDL_GameControllerGetBindForButton;
		[NativeType("SDL_bool SDL_GameControllerHasButton(SDL_GameController* gameController, SDL_GameControllerButton button)")]
		public delegate* unmanaged<IntPtr, SDLGameControllerButton, SDLBool> SDL_GameControllerHasButton;
		[NativeType("SDL_ButtonState SDL_GameControllerGetButton(SDL_GameController* gameController, SDL_GameControllerButton button)")]
		public delegate* unmanaged<IntPtr, SDLGameControllerButton, SDLButtonState> SDL_GameControllerGetButton;
		[NativeType("int SDL_GameControllerGetNumTouchpads(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, int> SDL_GameControllerGetNumTouchpads;
		[NativeType("int SDL_GameControllerGetNumTouchpadFingers(SDL_GameController* gameController, int touchpad)")]
		public delegate* unmanaged<IntPtr, int, int> SDL_GameControllerGetNumTouchpadFingers;
		[NativeType("int SDL_GameControllerGetTouchpadFinger(SDL_GameController* gameController, int touchpad, int finger, SDL_ButtonState* state, float* x, float* y, float* pressure)")]
		public delegate* unmanaged<IntPtr, int, int, out SDLButtonState, out float, out float, out float, int> SDL_GameControllerGetTouchpadFinger;
		[NativeType("SDL_bool SDL_GameControllerHasSensor(SDL_GameController* gameController, SDL_SensorType type)")]
		public delegate* unmanaged<IntPtr, SDLSensorType, SDLBool> SDL_GameControllerHasSensor;
		[NativeType("int SDL_GameControllerSetSensorEnabled(SDL_GameController* gameController, SDL_SensorType type, SDL_bool enabled)")]
		public delegate* unmanaged<IntPtr, SDLSensorType, SDLBool, int> SDL_GameControllerSetSensorEnabled;
		[NativeType("SDL_bool SDL_GameControllerIsSensorEnabled(SDL_GameController* gameController, SDL_SensorType type)")]
		public delegate* unmanaged<IntPtr, SDLSensorType, SDLBool> SDL_GameControllerIsSensorEnabled;
		[NativeType("int SDL_GameControllerGetSensorData(SDL_GameController* gameController, SDL_SensorType type, float* data, int numValues)")]
		public delegate* unmanaged<IntPtr, SDLSensorType, float*, int, int> SDL_GameControllerGetSensorData;
		[NativeType("int SDL_GameControllerRumble(SDL_GameController* gameController, Uint16 lowFreqRumble, Uint16 highFreqRumble, Uint32 durationMS)")]
		public delegate* unmanaged<IntPtr, ushort, ushort, uint, int> SDL_GameControllerRumble;
		[NativeType("int SDL_GameControllerRumbleTriggers(SDL_GameController* gameController, Uint16 leftRumble, Uint16 rightRumble, Uint32 durationMS)")]
		public delegate* unmanaged<IntPtr, ushort, ushort, uint, int> SDL_GameControllerRumbleTriggers;
		[NativeType("SDL_bool SDL_GameControllerHasLED(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, SDLBool> SDL_GameControllerHasLED;
		[NativeType("int SDL_GameControllerSetLED(SDL_GameController* gameController, Uint8 r, Uint8 g, Uint8 b)")]
		public delegate* unmanaged<IntPtr, byte, byte, byte, int> SDL_GameControllerSetLED;
		[NativeType("void SDL_GameControllerClose(SDL_GameController* gameController)")]
		public delegate* unmanaged<IntPtr, void> SDL_GameControllerClose;

		// SDL_touch.h

		[NativeType("int SDL_GetNumTouchDevices()")]
		public delegate* unmanaged<int> SDL_GetNumTouchDevices;
		[NativeType("SInt64 SDL_GetTouchDevice(int index)")]
		public delegate* unmanaged<int, long> SDL_GetTouchDevice;
		[NativeType("SDL_TouchDeviceType SDL_GetTouchDeviceType(SInt64 touchID)")]
		public delegate* unmanaged<long, SDLTouchDeviceType> SDL_GetTouchDeviceType;
		[NativeType("int SDL_GetNumTouchFingers(SInt64 touchID)")]
		public delegate* unmanaged<long, int> SDL_GetNumTouchFingers;
		[NativeType("SDL_Finger* SDL_GetTouchFinger(SInt64 touchID, int index)")]
		public delegate* unmanaged<long, int, SDLFinger*> SDL_GetTouchFinger;

		// SDL_events.h

		[NativeType("void SDL_PumpEvents()")]
		public delegate* unmanaged<void> SDL_PumpEvents;
		[NativeType("int SDL_PeepEvents(SDL_Event* events, int numEvents, SDL_EventAction action, Uint32 minType, Uint32 maxType)")]
		public delegate* unmanaged<SDLEvent*, int, SDLEventAction, uint, uint, int> SDL_PeepEvents;
		[NativeType("SDL_bool SDL_HasEvent(Uint32 type)")]
		public delegate* unmanaged<uint, SDLBool> SDL_HasEvent;
		[NativeType("SDL_bool SDL_HasEvents(Uint32 minType, Uint32 maxType)")]
		public delegate* unmanaged<uint, uint, SDLBool> SDL_HasEvents;
		[NativeType("void SDL_FlushEvent(Uint32 type)")]
		public delegate* unmanaged<uint, void> SDL_FlushEvent;
		[NativeType("void SDL_FlushEvents(Uint32 minType, Uint32 maxType)")]
		public delegate* unmanaged<uint, uint, void> SDL_FlushEvents;
		[NativeType("int SDL_PollEvent(SDL_Event* event)")]
		public delegate* unmanaged<out SDLEvent, int> SDL_PollEvent;
		[NativeType("int SDL_WaitEvent(SDL_Event* event)")]
		public delegate* unmanaged<out SDLEvent, int> SDL_WaitEvent;
		[NativeType("int SDL_WaitEventTimeout(SDL_Event* event, int timeout)")]
		public delegate* unmanaged<ref SDLEvent, int, int> SDL_WaitEventTimeout;
		[NativeType("int SDL_PushEvent(const SDL_Event* event)")]
		public delegate* unmanaged<in SDLEvent, int> SDL_PushEvent;
		[NativeType("void SDL_SetEventFilter(SDL_EventFilter filter, void* userdata)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> SDL_SetEventFilter;
		[NativeType("int SDL_SetEventFilter(SDL_EventFilter* filter, void** userdata)")]
		public delegate* unmanaged<out IntPtr, out IntPtr, bool> SDL_GetEventFilter;
		[NativeType("void SDL_AddEventWatch(SDL_EventFilter filter, void* userdata)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> SDL_AddEventWatch;
		[NativeType("void SDL_DelEventWatch(SDL_EventFilter filter, void* userdata)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> SDL_DelEventWatch;
		[NativeType("void SDL_FilterEvents(SDL_EventFilter filter, void* userdata)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> SDL_FilterEvents;
		[NativeType("Uint8 SDL_EventState(Uint32 type, int state)")]
		public delegate* unmanaged<uint, byte, byte> SDL_EventState;
		[NativeType("Uint32 SDL_RegisterEvents(int numEvents)")]
		public delegate* unmanaged<int, uint> SDL_RegisterEvents;

		// SDL_cpuinfo.h

		[NativeType("int SDL_GetCPUCount()")]
		public delegate* unmanaged<int> SDL_GetCPUCount;
		[NativeType("int SDL_GetCPUCacheLineSize()")]
		public delegate* unmanaged<int> SDL_GetCPUCacheLineSize;
		[NativeType("SDL_bool SDL_HasRDTSC()")]
		public delegate* unmanaged<SDLBool> SDL_HasRDTSC;
		[NativeType("SDL_bool SDL_HasAltiVec()")]
		public delegate* unmanaged<SDLBool> SDL_HasAltiVec;
		[NativeType("SDL_bool SDL_HasMMX()")]
		public delegate* unmanaged<SDLBool> SDL_HasMMX;
		[NativeType("SDL_bool SDL_Has3DNow()")]
		public delegate* unmanaged<SDLBool> SDL_Has3DNow;
		[NativeType("SDL_bool SDL_HasSSE()")]
		public delegate* unmanaged<SDLBool> SDL_HasSSE;
		[NativeType("SDL_bool SDL_HasSSE2()")]
		public delegate* unmanaged<SDLBool> SDL_HasSSE2;
		[NativeType("SDL_bool SDL_HasSSE3()")]
		public delegate* unmanaged<SDLBool> SDL_HasSSE3;
		[NativeType("SDL_bool SDL_HasSSE41()")]
		public delegate* unmanaged<SDLBool> SDL_HasSSE41;
		[NativeType("SDL_bool SDL_HasSSE42()")]
		public delegate* unmanaged<SDLBool> SDL_HasSSE42;
		[NativeType("SDL_bool SDL_HasAVX()")]
		public delegate* unmanaged<SDLBool> SDL_HasAVX;
		[NativeType("SDL_bool SDL_HasAVX2()")]
		public delegate* unmanaged<SDLBool> SDL_HasAVX2;
		[NativeType("SDL_bool SDL_HasAVX512F()")]
		public delegate* unmanaged<SDLBool> SDL_HasAVX512F;
		[NativeType("SDL_bool SDL_HasARMSIMD()")]
		public delegate* unmanaged<SDLBool> SDL_HasARMSIMD;
		[NativeType("SDL_bool SDL_HasNEON()")]
		public delegate* unmanaged<SDLBool> SDL_HasNEON;
		[NativeType("int SDL_GetSystemRAM()")]
		public delegate* unmanaged<int> SDL_GetSystemRAM;
		[NativeType("size_t SDL_SIMDGetAlignment()")]
		public delegate* unmanaged<nuint> SDL_SIMDGetAlignment;
		[NativeType("void* SDL_SIMDAlloc(size_t len)")]
		public delegate* unmanaged<nuint, IntPtr> SDL_SIMDAlloc;
		[NativeType("void* SDL_SIMDRealloc(void* mem, size_t len)")]
		public delegate* unmanaged<IntPtr, nuint, IntPtr> SDL_SIMDRealloc;
		[NativeType("void SDL_SIMDFree(void* mem)")]
		public delegate* unmanaged<IntPtr, void> SDL_SIMDFree;

		// SDL_audio.h

		[NativeType("int SDL_GetNumAudioDrivers()")]
		public delegate* unmanaged<int> SDL_GetNumAudioDrivers;
		[NativeType("const char* SDL_GetAudioDriver(int num)")]
		public delegate* unmanaged<int, IntPtr> SDL_GetAudioDriver;
		[NativeType("int SDL_AudioInit(const char* driverName)")]
		public delegate* unmanaged<byte*, int> SDL_AudioInit;
		[NativeType("void SDL_AudioQuit()")]
		public delegate* unmanaged<void> SDL_AudioQuit;
		[NativeType("const char* SDL_GetCurrentAudioDriver()")]
		public delegate* unmanaged<IntPtr> SDL_GetCurrentAudioDriver;
		[NativeType("int SDL_OpenAudio(const SDL_AudioSpec* desired, SDL_AudioSpec* obtained)")]
		public delegate* unmanaged<in SDLAudioSpec, out SDLAudioSpec, int> SDL_OpenAudio;
		[NativeType("int SDL_GetNumAudioDevices(int iscapture)")]
		public delegate* unmanaged<bool, int> SDL_GetNumAudioDevices;
		[NativeType("const char* SDL_GetAudioDeviceName(int index, int iscapture)")]
		public delegate* unmanaged<int, bool, IntPtr> SDL_GetAudioDeviceName;
		[NativeType("Uint32 SDL_OpenAudioDevice(const char* device, int iscapture, const SDL_AudioSpec* desired, SDL_AudioSpec* obtained, SDL_AudioAllowChange allowedChanges)")]
		public delegate* unmanaged<byte*, bool, in SDLAudioSpec, out SDLAudioSpec, SDLAudioAllowChange, uint> SDL_OpenAudioDevice;
		[NativeType("SDL_AudioStatus, SDL_GetAudioDeviceStatus(Uint32 dev)")]
		public delegate* unmanaged<uint, SDLAudioStatus> SDL_GetAudioDeviceStatus;
		[NativeType("void SDL_PauseAudio(int pauseOn)")]
		public delegate* unmanaged<bool, void> SDL_PauseAudio;
		[NativeType("void SDL_PauseAudioDevice(Uint32 dev, int pauseOn)")]
		public delegate* unmanaged<uint, bool, void> SDL_PauseAudioDevice;
		[NativeType("SDL_AudioSpec* SDL_LoadWAV_RW(SDL_RWops* src, int freesrc, SDL_AudioSpec* spec, Uint8** audioBuf, Uint32* audioLength)")]
		public delegate* unmanaged<SDL_RWops*, bool, out SDLAudioSpec, out byte*, out uint, SDLAudioSpec*> SDL_LoadWAV_RW;
		[NativeType("void SDL_FreeWAV(Uint8* audioBuf)")]
		public delegate* unmanaged<byte*, void> SDL_FreeWAV;
		[NativeType("int SDL_BuildAudioCVT(SDL_AudioCVT* cvt, SDL_AudioFormat srcFormat, Uint8 srcChannels, int srcRate, SDL_AudioFormat dstFormat, Uint8 dstChannels, int dstRate)")]
		public delegate* unmanaged<out SDLAudioCVT, SDLAudioFormat, byte, int, SDLAudioFormat, byte, int, int> SDL_BuildAudioCVT;
		[NativeType("int SDL_ConvertAudio(SDL_AudioCVT* cvt)")]
		public delegate* unmanaged<in SDLAudioCVT, int> SDL_ConvertAudio;
		[NativeType("SDL_AudioStream* SDL_NewAudioStream(SDL_AudioFormat srcFormat, Uint8 srcChannels, int srcRate, SDLAudioFormat dstFormat, Uint8 dstChannels, int dstRate)")]
		public delegate* unmanaged<SDLAudioFormat, byte, int, SDLAudioFormat, byte, int, IntPtr> SDL_NewAudioStream;
		[NativeType("int SDL_AudioStreamPut(SDL_AudioStream* stream, void* buf, int len)")]
		public delegate* unmanaged<IntPtr, IntPtr, int, int> SDL_AudioStreamPut;
		[NativeType("int SDL_AudioStreamGet(SDL_AudioStream* stream, void* buf, int len)")]
		public delegate* unmanaged<IntPtr, IntPtr, int, int> SDL_AudioStreamGet;
		[NativeType("int SDL_AudioStreamAvailable(SDL_AudioStream* stream)")]
		public delegate* unmanaged<IntPtr, int> SDL_AudioStreamAvailable;
		[NativeType("int SDL_AudioStreamFlush(SDL_AudioStream* stream)")]
		public delegate* unmanaged<IntPtr, int> SDL_AudioStreamFlush;
		[NativeType("void SDL_AudioStreamClear(SDL_AudioStream* stream)")]
		public delegate* unmanaged<IntPtr, void> SDL_AudioStreamClear;
		[NativeType("void SDL_FreeAudioStream(SDL_AudioStream* stream)")]
		public delegate* unmanaged<IntPtr, void> SDL_FreeAudioStream;
		[NativeType("void SDL_MixAudio(Uint8* dst, const Uint8* src, Uint32 len, int volume)")]
		public delegate* unmanaged<byte*, byte*, uint, int, void> SDL_MixAudio;
		[NativeType("void SDL_MixAudioFormat(Uint8* dst, const Uint8* src, SDL_AudioFormat format, Uint32 len, int volume)")]
		public delegate* unmanaged<byte*, byte*, SDLAudioFormat, uint, int, void> SDL_MixAudioFormat;
		[NativeType("int SDL_QueueAudio(Uint32 dev, void* data, Uint32 len)")]
		public delegate* unmanaged<uint, IntPtr, uint, int> SDL_QueueAudio;
		[NativeType("Uint32 SDL_DequeueAudio(Uint32 dev, void* data, Uint32 len)")]
		public delegate* unmanaged<uint, IntPtr, uint, uint> SDL_DequeueAudio;
		[NativeType("Uint32 SDL_GetQueuedAudioSize(Uint32 dev)")]
		public delegate* unmanaged<uint, uint> SDL_GetQueuedAudioSize;
		[NativeType("void SDL_ClearQueuedAudio(Uint32 dev)")]
		public delegate* unmanaged<uint, void> SDL_ClearQueuedAudio;
		[NativeType("void SDL_LockAudio()")]
		public delegate* unmanaged<void> SDL_LockAudio;
		[NativeType("void SDL_LockAudioDevice(Uint32 dev)")]
		public delegate* unmanaged<uint, void> SDL_LockAudioDevice;
		[NativeType("void SDL_UnlockAudio()")]
		public delegate* unmanaged<void> SDL_UnlockAudio;
		[NativeType("void SDL_UnlockAudioDevice(Uint32 dev)")]
		public delegate* unmanaged<uint, void> SDL_UnlockAudioDevice;
		[NativeType("void SDL_CloseAudio()")]
		public delegate* unmanaged<void> SDL_CloseAudio;
		[NativeType("void SDL_CloseAudioDevice(Uint32 dev)")]
		public delegate* unmanaged<uint, void> SDL_CloseAudioDevice;

		// SDL_clipboard.h

		[NativeType("int SDL_SetClipboardText(const char* text)")]
		public delegate* unmanaged<byte*, int> SDL_SetClipboardText;
		[NativeType("const char* SDL_GetClipboardText()")]
		public delegate* unmanaged<IntPtr> SDL_GetClipboardText;
		[NativeType("SDL_bool SDL_HasClipboardText()")]
		public delegate* unmanaged<SDLBool> SDL_HasClipboardText;

		// SDL_filesystem.h

		[NativeType("const char* SDL_GetBasePath()")]
		public delegate* unmanaged<IntPtr> SDL_GetBasePath;
		[NativeType("const char* SDL_GetPrefPath(const char* org, const char* app)")]
		public delegate* unmanaged<byte*, byte*, IntPtr> SDL_GetPrefPath;

		// SDL_gesture.h

		[NativeType("int SDL_RecordGesture(SInt64 touchID)")]
		public delegate* unmanaged<long, int> SDL_RecordGesture;
		[NativeType("int SDL_SaveAllDollarTemplates(SDL_RWops* dst)")]
		public delegate* unmanaged<SDL_RWops*, int> SDL_SaveAllDollarTemplates;
		[NativeType("int SDL_SaveDollarTemplate(SInt64 gestureID, SDL_RWops* dst)")]
		public delegate* unmanaged<long, SDL_RWops*, int> SDL_SaveDollarTemplate;
		[NativeType("int SDL_LoadDollarTemplates(SInt64 touchID, SDL_RWops* src)")]
		public delegate* unmanaged<long, SDL_RWops*, int> SDL_LoadDollarTemplates;

		// SDL_haptic.h

		[NativeType("int SDL_NumHaptics()")]
		public delegate* unmanaged<int> SDL_NumHaptics;
		[NativeType("const char* SDL_HapticName(int deviceIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_HapticName;
		[NativeType("SDL_Haptic* SDL_HapticOpen(int deviceIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_HapticOpen;
		[NativeType("int SDL_HapticOpened(int deviceIndex)")]
		public delegate* unmanaged<int, int> SDL_HapticOpened;
		[NativeType("int SDL_HapticIndex(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, int> SDL_HapticIndex;
		[NativeType("int SDL_MouseIsHaptic()")]
		public delegate* unmanaged<int> SDL_MouseIsHaptic;
		[NativeType("SDL_Haptic* SDL_HapticOpenFromMouse()")]
		public delegate* unmanaged<IntPtr> SDL_HapticOpenFromMouse;
		[NativeType("int SDL_JoystickIsHaptic(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, int> SDL_JoystickIsHaptic;
		[NativeType("SDL_Haptic* SDL_HapticOpenFromJoystick(SDL_Joystick* joystick)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_HapticOpenFromJoystick;
		[NativeType("void SDL_HapticClose(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, void> SDL_HapticClose;
		[NativeType("int SDL_HapticNumEffects(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, int> SDL_HapticNumEffects;
		[NativeType("int SDL_HapticNumEffectsPlaying(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, int> SDL_HapticNumEffectsPlaying;
		[NativeType("Uint32 SDL_HapticQuery(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, uint> SDL_HapticQuery;
		[NativeType("int SDL_HapticNumAxes(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, int> SDL_HapticNumAxes;
		[NativeType("int SDL_HapticEffectSupported(SDL_Haptic* haptic, const SDL_HapticEffect* effect)")]
		public delegate* unmanaged<IntPtr, in SDLHapticEffect, int> SDL_HapticEffectSupported;
		[NativeType("int SDL_HapticNewEffect(SDL_Haptic* haptic, const SDL_HapticEffect* effect)")]
		public delegate* unmanaged<IntPtr, in SDLHapticEffect, int> SDL_HapticNewEffect;
		[NativeType("int SDL_HapticUpdateEffect(SDL_Haptic* haptic, int effect, const SDL_HapticEffect* data)")]
		public delegate* unmanaged<IntPtr, int, in SDLHapticEffect, int> SDL_HapticUpdateEffect;
		[NativeType("int SDL_HapticRunEffect(SDL_Haptic* haptic, int effect, Uint32 iterations)")]
		public delegate* unmanaged<IntPtr, int, uint, int> SDL_HapticRunEffect;
		[NativeType("int SDL_HapticStopEffect(SDL_Haptic* haptic, int effect)")]
		public delegate* unmanaged<IntPtr, int, int> SDL_HapticStopEffect;
		[NativeType("void SDL_HapticDestroyEffect(SDL_Haptic* haptic, int effect)")]
		public delegate* unmanaged<IntPtr, int, void> SDL_HapticDestroyEffect;
		[NativeType("int SDL_HapticGetEffectStatus(SDL_Haptic* haptic, int effect)")]
		public delegate* unmanaged<IntPtr, int, int> SDL_HapticGetEffectStatus;
		[NativeType("int delegate* unmanaged<IntPtr, int, int>(SDL_Haptic* haptic, int gain)")]
		public delegate* unmanaged<IntPtr, int, int> SDL_HapticSetGain;
		[NativeType("int SDL_HapticSetAutocenter(SDL_Haptic* haptic, int autocenter)")]
		public delegate* unmanaged<IntPtr, int, int> SDL_HapticSetAutocenter;
		[NativeType("int SDL_HapticPause(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, int> SDL_HapticPause;
		[NativeType("int SDL_HapticUnpause(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, int> SDL_HapticUnpause;
		[NativeType("int SDL_HapticStopAll(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, int> SDL_HapticStopAll;
		[NativeType("int SDL_HapticRumbleSupported(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, int> SDL_HapticRumbleSupported;
		[NativeType("int SDL_HapticRumbleInit(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, int> SDL_HapticRumbleInit;
		[NativeType("int SDL_HapticRumblePlay(SDL_Haptic* haptic, float strength, Uint32 length)")]
		public delegate* unmanaged<IntPtr, float, uint, int> SDL_HapticRumblePlay;
		[NativeType("int SDL_HapticRumbleStop(SDL_Haptic* haptic)")]
		public delegate* unmanaged<IntPtr, int> SDL_HapticRumbleStop;

		// SDL_hints.h

		[NativeType("SDL_bool SDL_SetHintWithPriority(const char* name, const char* value, SDL_HintPriority priority)")]
		public delegate* unmanaged<byte*, byte*, SDLHintPriority, SDLBool> SDL_SetHintWithPriority;
		[NativeType("SDL_bool SDL_SetHint(const char* name, const char* value)")]
		public delegate* unmanaged<byte*, byte*, SDLBool> SDL_SetHint;
		[NativeType("const char* SDL_GetHint(const char* name)")]
		public delegate* unmanaged<byte*, IntPtr> SDL_GetHint;
		[NativeType("SDL_bool SDL_GetHintBoolean(const char* name, SDL_bool defaultValue)")]
		public delegate* unmanaged<byte*, SDLBool, SDLBool> SDL_GetHintBoolean;
		[NativeType("void SDL_AddHintCallback(const char* name, SDL_HintCallback callback, void* userdata)")]
		public delegate* unmanaged<byte*, IntPtr, IntPtr, void> SDL_AddHintCallback;
		[NativeType("void SDL_DelHintCallback(const char* name, SDL_HintCallback callback, void* userdata)")]
		public delegate* unmanaged<byte*, IntPtr, IntPtr, void> SDL_DelHintCallback;

		// SDL_locale.h

		[NativeType("SDL_Locale* SDL_GetPreferredLocales()")]
		public delegate* unmanaged<SDL_Locale*> SDL_GetPreferredLocales;

		// SDL_log.h

		[NativeType("void SDL_LogSetAllPriority(SDL_LogPriority priority)")]
		public delegate* unmanaged<SDLLogPriority, void> SDL_LogSetAllPriority;
		[NativeType("void SDL_LogSetPriority(int category, SDL_LogPriority priority)")]
		public delegate* unmanaged<int, SDLLogPriority, void> SDL_LogSetPriority;
		[NativeType("SDL_LogPriority SDL_LogGetPriority(int category)")]
		public delegate* unmanaged<int, SDLLogPriority> SDL_LogGetPriority;
		[NativeType("void SDL_LogResetPriorities()")]
		public delegate* unmanaged<void> SDL_LogResetPriorities;
		[NativeType("void SDL_Log(const char* fmt, ...)")]
		public delegate* unmanaged<byte*, byte*, void> SDL_Log;
		[NativeType("void SDL_LogVerbose(int category, const char* fmt, ...)")]
		public delegate* unmanaged<int, byte*, byte*, void> SDL_LogVerbose;
		[NativeType("void SDL_LogDebug(int category, const char* fmt, ...)")]
		public delegate* unmanaged<int, byte*, byte*, void> SDL_LogDebug;
		[NativeType("void SDL_LogInfo(int category, const char* fmt, ...)")]
		public delegate* unmanaged<int, byte*, byte*, void> SDL_LogInfo;
		[NativeType("void SDL_LogWarn(int category, const char* fmt, ...)")]
		public delegate* unmanaged<int, byte*, byte*, void> SDL_LogWarn;
		[NativeType("void SDL_LogError(int category, const char* fmt, ...)")]
		public delegate* unmanaged<int, byte*, byte*, void> SDL_LogError;
		[NativeType("void SDL_LogCritical(int category, const char* fmt, ...)")]
		public delegate* unmanaged<int, byte*, byte*, void> SDL_LogCritical;
		[NativeType("void SDL_LogMessage(int category, SDL_LogPriority priority, const char* fmt, ...)")]
		public delegate* unmanaged<int, SDLLogPriority, byte*, byte*, void> SDL_LogMessage;
		[NativeType("void SDL_LogGetOutputFunction(SDL_LogOutputFunction* callback, void** userdata)")]
		public delegate* unmanaged<out IntPtr, out IntPtr, void> SDL_LogGetOutputFunction;
		[NativeType("void SDL_LogSetOutputFunction(SDL_LogOutputFunction callback, void* userdata)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> SDL_LogSetOutputFunction;

		// SDL_messagebox.h

		[NativeType("int SDL_ShowMessageBox(const SDL_MessageBoxData* data, int* buttonID)")]
		public delegate* unmanaged<in SDL_MessageBoxData, out int, int> SDL_ShowMessageBox;
		[NativeType("int SDL_ShowSimpleMessageBox(int flags, const char* title, const char* message, SDL_Window* window)")]
		public delegate* unmanaged<SDLMessageBoxFlags, byte*, byte*, IntPtr, int> SDL_ShowSimpleMessageBox;

		// SDL_metal.h

		[NativeType("SDL_MetalView SDL_Metal_CreateView(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_Metal_CreateView;
		[NativeType("void SDL_Metal_DestroyView(SDL_MetalView view)")]
		public delegate* unmanaged<IntPtr, void> SDL_Metal_DestroyView;
		[NativeType("void* SDL_Metal_GetLayer(SDL_MetalView view)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_Metal_GetLayer;
		[NativeType("void SDL_Metal_GetDrawableSize(SDL_Window* window, int* w, int* h)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> SDL_Metal_GetDrawableSize;

		// SDL_misc.h

		[NativeType("int SDL_OpenURL(const char* url)")]
		public delegate* unmanaged<byte*, int> SDL_OpenURL;

		// SDL_platform.h

		[NativeType("const char* SDL_GetPlatform()")]
		public delegate* unmanaged<IntPtr> SDL_GetPlatform;

		// SDL_power.h

		[NativeType("SDL_PowerState SDL_GetPowerInfo(int* seconds, int* percent)")]
		public delegate* unmanaged<out int, out int, SDLPowerState> SDL_GetPowerInfo;

		// SDL_render.h

		[NativeType("int SDL_GetNumRenderDrivers()")]
		public delegate* unmanaged<int> SDL_GetNumRenderDrivers;
		[NativeType("int SDL_GetRenderDriverInfo(int index, SDL_RendererInfo* info)")]
		public delegate* unmanaged<int, out SDLRendererInfo, int> SDL_GetRenderDriverInfo;
		[NativeType("int SDL_CreateWindowAndRenderer(int width, int height, int windowFlags, SDL_Window** window, SDL_Renderer** renderer)")]
		public delegate* unmanaged<int, int, SDLWindowFlags, out IntPtr, out IntPtr, int> SDL_CreateWindowAndRenderer;
		[NativeType("SDL_Renderer* SDL_CreateRenderer(SDL_Window* window, int index, int flags)")]
		public delegate* unmanaged<IntPtr, int, SDLRendererFlags, IntPtr> SDL_CreateRenderer;
		[NativeType("SDL_Renderer* SDL_CreateSoftwareRenderer(SDL_Surface* surface)")]
		public delegate* unmanaged<SDL_Surface*, IntPtr> SDL_CreateSoftwareRenderer;
		[NativeType("SDL_Renderer* SDL_GetRenderer(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_GetRenderer;
		[NativeType("int SDL_GetRendererInfo(SDL_Renderer* renderer, SDL_RendererInfo* info)")]
		public delegate* unmanaged<IntPtr, out SDLRendererInfo, int> SDL_GetRendererInfo;
		[NativeType("int SDL_GetRendererOutputSize(SDL_Renderer* renderer, int* w, int* h)")]
		public delegate* unmanaged<IntPtr, out int, out int, int> SDL_GetRendererOutputSize;

		[NativeType("SDL_Texture* SDL_CreateTexture(SDL_Renderer* renderer, Uint32 format, SDL_TextureAccess access, int w, int h)")]
		public delegate* unmanaged<IntPtr, SDLPixelFormatEnum, SDLTextureAccess, int, int, IntPtr> SDL_CreateTexture;
		[NativeType("SDL_Texture* SDL_CreateTextureFromSurface(SDL_Renderer* renderer, SDL_Surface* surface)")]
		public delegate* unmanaged<IntPtr, SDL_Surface*, IntPtr> SDL_CreateTextureFromSurface;
		[NativeType("int SDL_QueryTexture(SDL_Texture* texture, Uint32* format, SDL_TextureAccess* access, int* w, int* h)")]
		public delegate* unmanaged<IntPtr, out SDLPixelFormatEnum, out SDLTextureAccess, out int, out int, int> SDL_QueryTexture;
		[NativeType("int SDL_SetTextureColorMod(SDL_Texture* texture, Uint8 r, Uint8 g, Uint8 b)")]
		public delegate* unmanaged<IntPtr, byte, byte, byte, int> SDL_SetTextureColorMod;
		[NativeType("int SDL_GetTextureColorMod(SDL_Texture* texture, Uint8* r, Uint8* g, Uint8* b)")]
		public delegate* unmanaged<IntPtr, out byte, out byte, out byte, int> SDL_GetTextureColorMod;
		[NativeType("int SDL_SetTextureAlphaMod(SDL_Texture* texture, Uint8 a)")]
		public delegate* unmanaged<IntPtr, byte, int> SDL_SetTextureAlphaMod;
		[NativeType("int SDL_GetTextureAlphaMod(SDL_Texture* texture, Uint8* a)")]
		public delegate* unmanaged<IntPtr, out byte, int> SDL_GetTextureAlphaMod;
		[NativeType("int SDL_SetTextureBlendMode(SDL_Texture* texture, SDL_BlendMode blendMode)")]
		public delegate* unmanaged<IntPtr, SDLBlendMode, int> SDL_SetTextureBlendMode;
		[NativeType("int SDL_GetTextureBlendMode(SDL_Texture* texture, SDL_BlendMode* blendMode)")]
		public delegate* unmanaged<IntPtr, out SDLBlendMode, int> SDL_GetTextureBlendMode;
		[NativeType("int SDL_SetTextureScaleMode(SDL_Texture* texture, SDL_ScaleMode scaleMode)")]
		public delegate* unmanaged<IntPtr, SDLScaleMode, int> SDL_SetTextureScaleMode;
		[NativeType("int SDL_GetTextureScaleMode(SDL_Texture* texture, SDL_ScaleMode* scaleMode)")]
		public delegate* unmanaged<IntPtr, out SDLScaleMode, int> SDL_GetTextureScaleMode;
		[NativeType("int SDL_UpdateTexture(SDL_Texture* texture, const SDL_Rect* rect, void* pixels, int pitch)")]
		public delegate* unmanaged<IntPtr, SDLRect*, IntPtr, int, int> SDL_UpdateTexture;
		[NativeType("int SDL_UpdateYUVTexture(SDL_Texture* texture, const SDL_Rect* rect, void* yplane, int ypitch, void* uplane, int upitch, void* vplane, int vpitch)")]
		public delegate* unmanaged<IntPtr, SDLRect*, IntPtr, int, IntPtr, int, IntPtr, int, int> SDL_UpdateYUVTexture;
		[NativeType("int SDL_LockTexture(SDL_Texture* texture, const SDL_Rect* rect, void** pixels, int* pitch)")]
		public delegate* unmanaged<IntPtr, SDLRect*, out IntPtr, out int, int> SDL_LockTexture;
		[NativeType("int SDL_LockTextureToSurface(SDL_Texture* texture, const SDL_Rect* rect, SDL_Surface** surface)")]
		public delegate* unmanaged<IntPtr, SDLRect*, out SDL_Surface*, int> SDL_LockTextureToSurface;
		[NativeType("void SDL_UnlockTexture(SDL_Texture* texture)")]
		public delegate* unmanaged<IntPtr, void> SDL_UnlockTexture;
		[NativeType("SDL_bool SDL_RenderTargetSupported(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, SDLBool> SDL_RenderTargetSupported;
		[NativeType("int SDL_SetRenderTarget(SDL_Renderer* renderer, SDL_Texture* texture)")]
		public delegate* unmanaged<IntPtr, IntPtr, int> SDL_SetRenderTarget;
		[NativeType("SDL_Texture* SDL_GetRenderTarget(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_GetRenderTarget;

		[NativeType("int SDL_RenderSetLogicalSize(SDL_Renderer* renderer, int w, int h)")]
		public delegate* unmanaged<IntPtr, int, int, int> SDL_RenderSetLogicalSize;
		[NativeType("int SDL_RenderGetLogicalSize(SDL_Renderer* renderer, int* w, int* h)")]
		public delegate* unmanaged<IntPtr, out int, out int, int> SDL_RenderGetLogicalSize;
		[NativeType("int SDL_RenderSetIntegerScale(SDL_Renderer* renderer, SDL_bool enable)")]
		public delegate* unmanaged<IntPtr, SDLBool, int> SDL_RenderSetIntegerScale;
		[NativeType("SDL_bool SDL_RenderGetIntegerScale(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, SDLBool> SDL_RenderGetIntegerScale;
		[NativeType("int SDL_RenderSetViewport(SDL_Renderer* renderer, const SDL_Rect* rect)")]
		public delegate* unmanaged<IntPtr, SDLRect*, int> SDL_RenderSetViewport;
		[NativeType("void SDL_RenderGetViewport(SDL_Renderer* renderer, SDL_Rect* rect)")]
		public delegate* unmanaged<IntPtr, out SDLRect, void> SDL_RenderGetViewport;
		[NativeType("int SDL_RenderSetClipRect(SDL_Renderer* renderer, const SDL_Rect* rect)")]
		public delegate* unmanaged<IntPtr, SDLRect*, int> SDL_RenderSetClipRect;
		[NativeType("void SDL_RenderGetClipRect(SDL_Renderer* renderer, SDL_Rect* rect)")]
		public delegate* unmanaged<IntPtr, out SDLRect, void> SDL_RenderGetClipRect;
		[NativeType("SDL_bool SDL_RenderIsClipEnabled(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, SDLBool> SDL_RenderIsClipEnabled;
		[NativeType("int SDL_RenderSetScale(SDL_Renderer* renderer, float scaleX, float scaleY)")]
		public delegate* unmanaged<IntPtr, float, float, int> SDL_RenderSetScale;
		[NativeType("void SDL_RenderGetScale(SDL_Renderer* renderer, float* scaleX, float* scaleY)")]
		public delegate* unmanaged<IntPtr, out float, out float, void> SDL_RenderGetScale;
		[NativeType("int SDL_SetRenderDrawColor(SDL_Renderer* renderer, Uint8 r, Uint8 g, Uint8 b, Uint8 a)")]
		public delegate* unmanaged<IntPtr, byte, byte, byte, byte, int> SDL_SetRenderDrawColor;
		[NativeType("int SDL_GetRenderDrawColor(SDL_Renderer* renderer, Uint8* r, Uint8* g, Uint8* b, Uint8* a)")]
		public delegate* unmanaged<IntPtr, out byte, out byte, out byte, out byte, int> SDL_GetRenderDrawColor;
		[NativeType("int SDL_SetRenderDrawBlendMode(SDL_Renderer* renderer, SDL_BlendMode blendMode)")]
		public delegate* unmanaged<IntPtr, SDLBlendMode, int> SDL_SetRenderDrawBlendMode;
		[NativeType("int SDL_GetRenderDrawBlendMode(SDL_Renderer* renderer, SDL_BlendMode* blendMode)")]
		public delegate* unmanaged<IntPtr, out SDLBlendMode, int> SDL_GetRenderDrawBlendMode;
		[NativeType("int SDL_RenderClear(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, int> SDL_RenderClear;

		[NativeType("int SDL_RenderDrawPoint(SDL_Renderer* renderer, int x, int y)")]
		public delegate* unmanaged<IntPtr, int, int, int> SDL_RenderDrawPoint;
		[NativeType("int SDL_RenderDrawPoints(SDL_Renderer* renderer, const SDL_Point* points, int count)")]
		public delegate* unmanaged<IntPtr, SDLPoint*, int, int> SDL_RenderDrawPoints;
		[NativeType("int SDL_RenderDrawLine(SDL_Renderer* renderer, int x1, int y1, int x2, int y2)")]
		public delegate* unmanaged<IntPtr, int, int, int, int, int> SDL_RenderDrawLine;
		[NativeType("int SDL_RenderDrawLines(SDL_Renderer* renderer, const SDL_Point* points, int count)")]
		public delegate* unmanaged<IntPtr, SDLPoint*, int, int> SDL_RenderDrawLines;
		[NativeType("int SDL_RenderDrawRect(SDL_Renderer* renderer, const SDL_Rect* rect)")]
		public delegate* unmanaged<IntPtr, in SDLRect, int> SDL_RenderDrawRect;
		[NativeType("int SDL_RenderDrawRects(SDL_Renderer* renderer, const SDL_Rect* rects, int count)")]
		public delegate* unmanaged<IntPtr, SDLRect*, int, int> SDL_RenderDrawRects;
		[NativeType("int SDL_RenderFillRect(SDL_Renderer* renderer, const SDL_Rect* rect)")]
		public delegate* unmanaged<IntPtr, in SDLRect, int> SDL_RenderFillRect;
		[NativeType("int SDL_RenderFillRects(SDL_Renderer* renderer, const SDL_Rect* rects, int count)")]
		public delegate* unmanaged<IntPtr, SDLRect*, int, int> SDL_RenderFillRects;

		[NativeType("int SDL_RenderCopy(SDL_Renderer* renderer, SDL_Texture* texture, const SDL_Rect* srcRect, const SDL_Rect* dstRect)")]
		public delegate* unmanaged<IntPtr, IntPtr, SDLRect*, SDLRect*, int> SDL_RenderCopy;
		[NativeType("int SDL_RenderCopyEx(SDL_Renderer* renderer, SDL_Texture* texture, const SDL_Rect* srcRect, const SDL_Rect* dstRect, double angle, const SDL_Point* center, SDL_RendererFlip flip)")]
		public delegate* unmanaged<IntPtr, IntPtr, SDLRect*, SDLRect*, double, SDLPoint*, SDLRendererFlip, int>  SDL_RenderCopyEx;
		[NativeType("int SDL_RenderDrawPointF(SDL_Renderer* renderer, float x, float y)")]
		public delegate* unmanaged<IntPtr, float, float, int> SDL_RenderDrawPointF;
		[NativeType("int SDL_RenderDrawPointsF(SDL_Renderer* renderer, const SDL_FPoint* points, int count)")]
		public delegate* unmanaged<IntPtr, SDLFPoint*, int, int> SDL_RenderDrawPointsF;
		[NativeType("int SDL_RenderDrawLineF(SDL_Renderer* renderer, float x1, float y1, float x2, float y2)")]
		public delegate* unmanaged<IntPtr, float, float, float, float, int> SDL_RenderDrawLineF;
		[NativeType("int SDL_RenderDrawLinesF(SDL_Renderer* renderer, const SDL_FPoint* points, int count)")]
		public delegate* unmanaged<IntPtr, SDLFPoint*, int, int> SDL_RenderDrawLinesF;
		[NativeType("int SDL_RenderDrawRectF(SDL_Renderer* renderer, const SDL_FRect* rect)")]
		public delegate* unmanaged<IntPtr, in SDLFRect, int> SDL_RenderDrawRectF;
		[NativeType("int SDL_RenderDrawRectsF(SDL_Renderer* renderer, const SDL_FRect* rects, int count)")]
		public delegate* unmanaged<IntPtr, SDLFRect*, int, int> SDL_RenderDrawRectsF;
		[NativeType("int SDL_RenderFillRectF(SDL_Renderer* renderer, const SDL_FRect* rect)")]
		public delegate* unmanaged<IntPtr, in SDLFRect, int> SDL_RenderFillRectF;
		[NativeType("int SDL_RenderFillRectsF(SDL_Renderer* renderer, const SDL_FRect* rects, int count)")]
		public delegate* unmanaged<IntPtr, SDLFRect*, int, int> SDL_RenderFillRectsF;

		[NativeType("int SDL_RenderCopyF(SDL_Renderer* renderer, SDL_Texture* texture, const SDL_Rect* srcRect, const SDL_FRect* dstRect)")]
		public delegate* unmanaged<IntPtr, IntPtr, SDLRect*, SDLFRect*, int> SDL_RenderCopyF;
		[NativeType("int SDL_RenderCopyExF(SDL_Renderer* renderer, SDL_Texture* texture, const SDL_Rect* srcRect, const SDL_FRect* dstRect, double angle, const SDL_FPoint* center, SDL_RenderFlip flip)")]
		public delegate* unmanaged<IntPtr, IntPtr, SDLRect*, SDLFRect*, double, in SDLFPoint, SDLRendererFlip, int> SDL_RenderCopyExF;
		[NativeType("int SDL_RenderGeometry(SDL_Renderer* renderer, SDL_Texture* texture, const SDL_Vertex* vertices, int numVertices, const int* indices, int numIndices)")]
		public delegate* unmanaged<IntPtr, IntPtr, SDLVertex*, int, int*, int, int> SDL_RenderGeometry;
		[NativeType("int SDL_RenderGeometryRaw(SDL_Renderer* renderer, SDL_Texture* texture, const float* xy, int xyStride, const SDL_Color* color, int colorStride, const float* uv, int uvStride, int numVertices, const void* indices, int numIndices, int sizeIndices)")]
		public delegate* unmanaged<IntPtr, IntPtr, float*, int, SDLColor*, int, float*, int, int, IntPtr, int, int, int> SDL_RenderGeometryRaw;
		[NativeType("int SDL_RenderReadPixels(SDL_Renderer* renderer, const SDL_Rect* rect, Uint32 format, void* pixels, int pitch)")]
		public delegate* unmanaged<IntPtr, SDLRect*, SDLPixelFormatEnum, IntPtr, int, int> SDL_RenderReadPixels;
		[NativeType("void SDL_RenderPresent(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, void> SDL_RenderPresent;
		[NativeType("void SDL_DestroyTexture(SDL_Texture* texture)")]
		public delegate* unmanaged<IntPtr, void> SDL_DestroyTexture;
		[NativeType("void SDL_DestroyRenderer(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, void> SDL_DestroyRenderer;
		[NativeType("int SDL_RenderFlush(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, int> SDL_RenderFlush;

		[NativeType("int SDL_GL_BindTexture(SDL_Texture* texture, float* texw, float* texh)")]
		public delegate* unmanaged<IntPtr, out float, out float, int> SDL_GL_BindTexture;
		[NativeType("int SDL_GL_UnbindTexture(SDL_Texture* texture)")]
		public delegate* unmanaged<IntPtr, int> SDL_GL_UnbindTexture;
		[NativeType("void* SDL_RenderGetMetalLayer(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_RenderGetMetalLayer;
		[NativeType("void* SDL_RenderGetMetalCommandEncoder(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_RenderGetMetalCommandEncoder;

		// SDL_shape.h

		[NativeType("SDL_Window* SDL_CreateShapedWindow(const char* title, Uint32 x, Uint32 y, Uint32 w, Uint32 h, int flags)")]
		public delegate* unmanaged<byte*, uint, uint, uint, uint, SDLWindowFlags, IntPtr> SDL_CreateShapedWindow;
		[NativeType("SDL_bool SDL_IsShapedWindow(SDL_Window* window)")]
		public delegate* unmanaged<IntPtr, SDLBool> SDL_IsShapedWindow;
		[NativeType("int SDL_SetWindowShape(SDL_Window* window, SDL_Surface* shape, const SDL_WindowShapeMode* shapeMode)")]
		public delegate* unmanaged<IntPtr, SDL_Surface*, in SDLWindowShapeMode, int> SDL_SetWindowShape;
		[NativeType("int SDL_GetShapedWindowMode(SDL_Window* window, SDL_WindowShapeMode* shapeMode)")]
		public delegate* unmanaged<IntPtr, out SDLWindowShapeMode, int> SDL_GetShapedWindowMode;

		// SDL_sensor.h

		[NativeType("void SDL_LockSensors()")]
		public delegate* unmanaged<void> SDL_LockSensors;
		[NativeType("void SDL_UnlockSensors()")]
		public delegate* unmanaged<void> SDL_UnlockSensors;
		[NativeType("int SDL_NumSensors()")]
		public delegate* unmanaged<int> SDL_NumSensors;
		[NativeType("const char* SDL_SensorGetDeviceName(int deviceIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_SensorGetDeviceName;
		[NativeType("SDL_SensorType SDL_SensorGetDeviceType(int deviceIndex)")]
		public delegate* unmanaged<int, SDLSensorType> SDL_SensorGetDeviceType;
		[NativeType("int SDL_SensorGetDeviceNonPortableType(int deviceIndex)")]
		public delegate* unmanaged<int, int> SDL_SensorGetDeviceNonPortableType;
		[NativeType("SDL_SensorID SDL_SensorGetDeviceInstanceID(int deviceIndex)")]
		public delegate* unmanaged<int, SDL_SensorID> SDL_SensorGetDeviceInstanceID;
		[NativeType("SDL_Sensor* SDL_SensorOpen(int deviceIndex)")]
		public delegate* unmanaged<int, IntPtr> SDL_SensorOpen;
		[NativeType("SDL_Sensor* SDL_SensorFromInstanceID(int instanceID)")]
		public delegate* unmanaged<int, IntPtr> SDL_SensorFromInstanceID;
		[NativeType("const char* SDL_SensorGetName(SDL_Sensor* sensor)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_SensorGetName;
		[NativeType("SDL_SensorType SDL_SensorGetType(SDL_Sensor* sensor)")]
		public delegate* unmanaged<IntPtr, SDLSensorType> SDL_SensorGetType;
		[NativeType("int SDL_SensorGetNonPortableType(SDL_Sensor* sensor)")]
		public delegate* unmanaged<IntPtr, int> SDL_SensorGetNonPortableType;
		[NativeType("int SDL_SensorGetInstanceID(SDL_Sensor* sensor)")]
		public delegate* unmanaged<IntPtr, int> SDL_SensorGetInstanceID;
		[NativeType("int SDL_SensorGetData(SDL_Sensor* sensor, float* data, int numValues)")]
		public delegate* unmanaged<IntPtr, float*, int, int> SDL_SensorGetData;
		[NativeType("void SDL_SensorClose(SDL_Sensor* sensor)")]
		public delegate* unmanaged<IntPtr, void> SDL_SensorClose;
		[NativeType("void SDL_SensorUpdate()")]
		public delegate* unmanaged<void> SDL_SensorUpdate;

		// SDL_system.h

		[ExternFunction(Platform = PlatformType.Windows)]
		[NativeType("void SDL_SetWindowsMessageHook(SDL_WindowsMessageHook callback, void* userdata)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> SDL_SetWindowsMessageHook;
		[ExternFunction(Platform = PlatformType.Windows)]
		[NativeType("int SDL_Direct3D9GetAdapterIndex(int displayIndex)")]
		public delegate* unmanaged<int, int> SDL_Direct3D9GetAdapterIndex;
		[ExternFunction(Platform = PlatformType.Windows)]
		[NativeType("IDirect3DDevice9* SDL_RenderGetD3D9Device(SDL_Renderer* renderer)")]
		public delegate* unmanaged<IntPtr, IntPtr> SDL_RenderGetD3D9Device;
		[ExternFunction(Platform = PlatformType.Windows)]
		[NativeType("SDL_bool SDL_DXGIGetOutputInfo(int displayIndex, int* adapterIndex, int* outputIndex)")]
		public delegate* unmanaged<int, out int, out int, SDLBool> SDL_DXGIGetOutputInfo;

		[ExternFunction(Platform = PlatformType.Linux)]
		[NativeType("int SDL_LinuxSetThreadPriority(SInt64 threasdID, int priority)")]
		public delegate* unmanaged<long, int, int> SDL_LinuxSetThreadPriority;

		/* iOS not yet supported
		public delegate int PFN_SDL_iPhoneSetAnimationCallback([NativeType("SDL_Window*")] IntPtr window, int interval, SDLiOSAnimationCallback callback, IntPtr callbackParam);
		public delegate void PFN_SDL_iPhoneSetEventPump(SDLBool enabled);
		public delegate void PFN_SDL_OnApplicationDidChangeStatusBarOrientation();

		public PFN_SDL_iPhoneSetAnimationCallback SDL_iPhoneSetAnimationCallback;
		public PFN_SDL_iPhoneSetEventPump SDL_iPhoneSetEventPump;
		public PFN_SDL_OnApplicationDidChangeStatusBarOrientation SDL_OnApplicationDidChangeStatusBarOrientation;
		*/

		/* Android not yet supported
		[return: NativeType("JNIEnv*")]
		public delegate IntPtr PFN_SDL_AndroidGetJNIEnv();
		[return: NativeType("jobject")]
		public delegate IntPtr PFN_SDL_AndroidGetActivity();
		public delegate int PFN_SDL_GetAndroidSDKVersion();
		public delegate SDLBool PFN_SDL_IsAndroidTV();
		public delegate SDLBool PFN_SDL_IsChromebook();
		public delegate SDLBool PFN_SDL_IsDeXMode();
		public delegate void PFN_SDL_AndroidBackButton();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_AndroidGetInternalStoragePath();
		public delegate SDLAndroidStorageState PFN_SDL_AndroidGetExternalStorageState();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_AndroidGetExternalStoragePath();
		public delegate SDLBool PFN_SDL_AndroidRequestPermission([MarshalAs(UnmanagedType.LPStr)] string permission);

		public PFN_SDL_AndroidGetJNIEnv SDL_AndroidGetJNIEnv;
		public PFN_SDL_AndroidGetActivity SDL_AndroidGetActivity;
		public PFN_SDL_GetAndroidSDKVersion SDL_GetAndroidSDKVersion;
		public PFN_SDL_IsAndroidTV SDL_IsAndroidTV;
		public PFN_SDL_IsChromebook SDL_IsChromebook;
		public PFN_SDL_IsDeXMode SDL_IsDeXMode;
		public PFN_SDL_AndroidBackButton SDL_AndroidBackButton;
		public PFN_SDL_AndroidGetInternalStoragePath SDL_AndroidGetInternalStoragePath;
		public PFN_SDL_AndroidGetExternalStorageState SDL_AndroidGetExternalStorageState;
		public PFN_SDL_AndroidGetExternalStoragePath SDL_AndroidGetExternalStoragePath;
		public PFN_SDL_AndroidRequestPermission SDL_AndroidRequestPermission;
		*/

		/* WinRT not yet sypported
		[return: NativeType("const wchar_t*")]
		public delegate IntPtr PFN_SDL_WinRTGetFSPathUNICODE(SDLWinRTPath path);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_WinRTGetFSPathUTF8(SDLWinRTPath path);
		public delegate SDLWinRTDeviceFamily PFN_SDL_WinRTGetDeviceFamily();

		public PFN_SDL_WinRTGetFSPathUNICODE SDL_WinRTGetFSPathUNICODE;
		public PFN_SDL_WinRTGetFSPathUTF8 SDL_WinRTGetFSPathUTF8;
		public PFN_SDL_WinRTGetDeviceFamily SDL_WinRTGetDeviceFamily;

		// iOS not supported
		public delegate void PFN_SDL_OnApplicationWillTerminate();
		public delegate void PFN_SDL_OnApplicationDidReceiveMemoryWarning();
		public delegate void PFN_SDL_OnApplicationWillResignActive();
		public delegate void PFN_SDL_OnApplicationDidEnterBackground();
		public delegate void PFN_SDL_OnApplicationWillEnterForeground();
		public delegate void PFN_SDL_OnApplicationDidBecomeActive();

		public PFN_SDL_OnApplicationWillTerminate SDL_OnApplicationWillTerminate;
		public PFN_SDL_OnApplicationDidReceiveMemoryWarning SDL_OnApplicationDidReceiveMemoryWarning;
		public PFN_SDL_OnApplicationWillResignActive SDL_OnApplicationWillResignActive;
		public PFN_SDL_OnApplicationDidEnterBackground SDL_OnApplicationDidEnterBackground;
		public PFN_SDL_OnApplicationWillEnterForeground SDL_OnApplicationWillEnterForeground;
		public PFN_SDL_OnApplicationDidBecomeActive SDL_OnApplicationDidBecomeActive;
		*/

		[NativeType("SDL_bool SDL_IsTablet()")]
		public delegate* unmanaged<SDLBool> SDL_IsTablet;

		// SDL_syswm.h

		[NativeType("SDL_bool SDL_GetWindowWMInfo(SDL_Window* window, SDL_SysWMinfo* info)")]
		public delegate* unmanaged<IntPtr, ref SDL_SysWMinfo, SDLBool> SDL_GetWindowWMInfo;

		// SDL_timer.h

		[NativeType("Uint32 SDL_GetTicks()")]
		public delegate* unmanaged<uint> SDL_GetTicks;
		[NativeType("UInt64 SDL_GetPerformanceCounter()")]
		public delegate* unmanaged<ulong> SDL_GetPerformanceCounter;
		[NativeType("UInt64 SDL_GetPerformanceFrequency()")]
		public delegate* unmanaged<ulong> SDL_GetPerformanceFrequency;
		[NativeType("void SDL_Delay(Uint32 ms)")]
		public delegate* unmanaged<uint, void> SDL_Delay;
		[NativeType("int SDL_AddTimer(Uint32 interval, SDL_TimerCallback callback, void* param)")]
		public delegate* unmanaged<uint, IntPtr, IntPtr, int> SDL_AddTimer;
		[NativeType("SDL_bool SDL_RemoveTimer(int id)")]
		public delegate* unmanaged<int, SDLBool> SDL_RemoveTimer;

		// SDL_version.h

		[NativeType("void SDL_GetVersion(SDL_Version* ver)")]
		public delegate* unmanaged<out SDLVersion, void> SDL_GetVersion;
		[NativeType("const char* SDL_GetRevision()")]
		public delegate* unmanaged<IntPtr> SDL_GetRevision;
		[NativeType("int SDL_GetRevisionNumber()")]
		public delegate* unmanaged<int> SDL_GetRevisionNumber;

		// SDL_vulkan.h

		[NativeType("void SDL_Vulkan_LoadLibrary(const char* path)")]
		public delegate* unmanaged<byte*, void> SDL_Vulkan_LoadLibrary;
		[NativeType("PFN_vkGetInstanceProcAddr SDL_Vulkan_GetVkGetInstanceProcAddr()")]
		public delegate* unmanaged<IntPtr> SDL_Vulkan_GetVkGetInstanceProcAddr;
		[NativeType("void SDL_Vulkan_UnloadLibrary()")]
		public delegate* unmanaged<void> SDL_Vulkan_UnloadLibrary;
		[NativeType("SDL_bool SDL_Vulkan_GetInstanceExtensions(SDL_Window* window, int* count, const char** pNames)")]
		public delegate* unmanaged<IntPtr, ref int, byte**, SDLBool> SDL_Vulkan_GetInstanceExtensions;
		[NativeType("SDL_bool SDL_Vulkan_CreateSurface(SDL_Window* window, VkInstance instance, VkSurfaceKHR* pSurface)")]
		public delegate* unmanaged<IntPtr, IntPtr, out ulong, SDLBool> SDL_Vulkan_CreateSurface;
		[NativeType("void SDL_Vulkan_GetDrawableSize(SDL_Window* window, int* w, int* h)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> SDL_Vulkan_GetDrawableSize;
		
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.SDL.Native {

	public class SDLFunctions {

		// SDL_stdinc.h

		public delegate IntPtr PFN_SDL_malloc(UIntPtr size);
		public delegate IntPtr PFN_SDL_calloc(UIntPtr nmemb, UIntPtr size);
		public delegate IntPtr PFN_SDL_realloc(IntPtr mem, UIntPtr size);
		public delegate void PFN_SDL_free(IntPtr mem);

		public PFN_SDL_malloc SDL_malloc;
		public PFN_SDL_calloc SDL_calloc;
		public PFN_SDL_realloc SDL_realloc;
		public PFN_SDL_free SDL_free;

		// SDL.h

		/// <summary>
		/// Initializes the subsystems specified by <c>flags</c>.
		/// </summary>
		/// <param name="flags">Flags specifying the subsystems to initialize</param>
		/// <returns>Zero if successful, negative on error. Call <c>SDL_GetError()</c> for detailed error information</returns>
		public delegate int PFN_SDL_Init(uint flags);
		/// <summary>
		/// Initializes specific SDL subsystems. Subsystems are ref-counted (ie. initializing increments, deinitializing decrements).
		/// </summary>
		/// <param name="flags">Flags specifying the subsystems to initialize</param>
		/// <returns>Zero if successful, negative on error. Call <c>SDL_GetError()</c> for detailed error information</returns>
		public delegate int PFN_SDL_InitSubSystem(uint flags);
		/// <summary>
		/// Deinitializes specific SDL subsystems.
		/// </summary>
		/// <param name="flags">Flags specifying the subsystems to deinitialize</param>
		public delegate void PFN_SDL_QuitSubSystem(uint flags);
		/// <summary>
		/// Returns a mask of specific subsystems which have been initialized.
		/// </summary>
		/// <param name="flags">Mask of subsystems to test, or return all subsystems if 0</param>
		/// <returns>Mask of initialized subsystems</returns>
		public delegate uint PFN_SDL_WasInit(uint flags);
		/// <summary>
		/// Cleans up all initialized subsystems.
		/// </summary>
		public delegate void PFN_SDL_Quit();

		public PFN_SDL_Init SDL_Init;
		public PFN_SDL_InitSubSystem SDL_InitSubSystem;
		public PFN_SDL_QuitSubSystem SDL_QuitSubSystem;
		public PFN_SDL_WasInit SDL_WasInit;
		public PFN_SDL_Quit SDL_Quit;

		// SDL_error.h

		/// <summary>
		/// Sets the current SDL error, unconditionally returning -1.
		/// </summary>
		/// <param name="fmt">String error message</param>
		/// <returns>-1</returns>
		public delegate int PFN_SDL_SetError([MarshalAs(UnmanagedType.LPStr)] string fmt);
		/// <summary>
		/// Gets the current SDL error.
		/// </summary>
		/// <returns>Current SDL error</returns>
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GetError();
		/// <summary>
		/// Clears the current SDL error.
		/// </summary>
		public delegate void PFN_SDL_ClearError();
		/// <summary>
		/// Sets the current SDL error to the given error code, unconditionally returning -1.
		/// </summary>
		/// <param name="code">Error code</param>
		/// <returns>-1</returns>
		public delegate int PFN_SDL_Error(SDLErrorCode code);

		public PFN_SDL_SetError SDL_SetError;
		public PFN_SDL_GetError SDL_GetError;
		public PFN_SDL_ClearError SDL_ClearError;
		public PFN_SDL_Error SDL_Error;

		// SDL_pixels.h

		/// <summary>
		/// Gets the name of a pixel format.
		/// </summary>
		/// <param name="format">Pixel format value</param>
		/// <returns>Pixel format name</returns>
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GetPixelFormatName(uint format);
		/// <summary>
		/// Gets the bitmasks and bits per pixel for a pixel format value.
		/// </summary>
		/// <param name="format">Pixel format value</param>
		/// <param name="bpp">Bits per pixel</param>
		/// <param name="rMask">Red component mask</param>
		/// <param name="gMask">Green component mask</param>
		/// <param name="bMask">Blue component mask</param>
		/// <param name="aMask">Alpha component mask</param>
		/// <returns>If the conversion was successful</returns>
		public delegate bool PFN_SDL_PixelFormatEnumToMasks(uint format, out int bpp, out uint rMask, out uint gMask, out uint bMask, out uint aMask);
		/// <summary>
		/// Gets a pixel format value given a set of bitmasks.
		/// </summary>
		/// <param name="bpp">Number of bits per pixel</param>
		/// <param name="rMask">Red component mask</param>
		/// <param name="gMask">Green component mask</param>
		/// <param name="bMask">Blue component mask</param>
		/// <param name="aMask">Alpha component mask</param>
		/// <returns>Pixel format value</returns>
		public delegate uint PFN_SDL_MasksToPixelFormatEnum(int bpp, uint rMask, uint gMask, uint bMask, uint aMask);
		/// <summary>
		/// Allocates a pixel format using the given pixel format value.
		/// </summary>
		/// <param name="pixelFormat">Pixel format value</param>
		/// <returns>Allocated pixel format</returns>
		[return: NativeType("SDL_PixelFormat*")]
		public delegate IntPtr PFN_SDL_AllocFormat(uint pixelFormat);
		/// <summary>
		/// Frees an allocated pixel format.
		/// </summary>
		/// <param name="format">Pixel format to free</param>
		public delegate void PFN_SDL_FreeFormat([NativeType("SDL_PixelFormat*")] IntPtr format);
		/// <summary>
		/// Allocates a color palette.
		/// </summary>
		/// <param name="ncolors">Number of colors in palette</param>
		/// <returns>Allocated color palette</returns>
		[return: NativeType("SDL_Palette*")]
		public delegate IntPtr PFN_SDL_AllocPalette(int ncolors);
		/// <summary>
		/// Sets the color palette of a format.
		/// </summary>
		/// <param name="format">Pixel format</param>
		/// <param name="palette">Color palette</param>
		/// <returns></returns>
		public delegate int PFN_SDL_SetPixelFormatPalette([NativeType("SDL_PixelFormat*")] IntPtr format, [NativeType("SDL_Palette*")] IntPtr palette);
		/// <summary>
		/// Sets the color values in a color palette.
		/// </summary>
		/// <param name="palette">Color palette</param>
		/// <param name="colors">Pointer to color values</param>
		/// <param name="firstcolor">Starting index to write color values to</param>
		/// <param name="ncolors">Number of colors to set</param>
		/// <returns>Zero on success, or -1 if not all colors could be set</returns>
		public delegate int PFN_SDL_SetPaletteColors([NativeType("SDL_PixelFormat*")] IntPtr palette, [NativeType("const SDL_Color*")] IntPtr colors, int firstcolor, int ncolors);
		/// <summary>
		/// Frees an allocated color palette.
		/// </summary>
		/// <param name="palette">Color palette to free</param>
		public delegate void PFN_SDL_FreePalette([NativeType("SDL_Palette*")] IntPtr palette);
		/// <summary>
		/// Maps a tuple of RGB values to a pixel value for a pixel format.
		/// </summary>
		/// <param name="format">Pixel format</param>
		/// <param name="r">Red component</param>
		/// <param name="g">Green component</param>
		/// <param name="b">Blue component</param>
		/// <returns>Pixel value</returns>
		public delegate uint PFN_SDL_MapRGB([NativeType("SDL_PixelFormat*")] IntPtr format, byte r, byte g, byte b);
		/// <summary>
		/// Maps a tuple of RGBA values to a pixel value for a pixel format.
		/// </summary>
		/// <param name="format">Pixel format</param>
		/// <param name="r">Red component</param>
		/// <param name="g">Green component</param>
		/// <param name="b">Blue component</param>
		/// <param name="a">Alpha component</param>
		/// <returns>Pixel value</returns>
		public delegate uint PFN_SDL_MapRGBA([NativeType("SDL_PixelFormat*")] IntPtr format, byte r, byte g, byte b, byte a);
		/// <summary>
		/// Gets the RGB components for a pixel value in the given format.
		/// </summary>
		/// <param name="pixel">Pixel value</param>
		/// <param name="format">Pixel format</param>
		/// <param name="r">Red component</param>
		/// <param name="g">Green component</param>
		/// <param name="b">Blue component</param>
		public delegate void PFN_SDL_GetRGB(uint pixel, [NativeType("SDL_PixelFormat*")] IntPtr format, out byte r, out byte g, out byte b);
		/// <summary>
		/// Gets the RGBA components for a pixel value in the given format.
		/// </summary>
		/// <param name="pixel">Pixel value</param>
		/// <param name="format">Pixel format</param>
		/// <param name="r">Red component</param>
		/// <param name="g">Green component</param>
		/// <param name="b">Blue component</param>
		/// <param name="a">Alpha component</param>
		public delegate void PFN_SDL_GetRGBA(uint pixel, [NativeType("SDL_PixelFormat*")] IntPtr format, out byte r, out byte g, out byte b, out byte a);
		/// <summary>
		/// Calculates an array of gamma ramp values for the given gamma value.
		/// </summary>
		/// <param name="gamma">Gamma value</param>
		/// <param name="ramp">Ramp to fill</param>
		public delegate void PFN_SDL_CalculateGammaRamp(float gamma, [Out][MarshalAs(UnmanagedType.LPArray, SizeConst = 256)] ushort[] ramp);

		public PFN_SDL_GetPixelFormatName SDL_GetPixelFormatName;
		public PFN_SDL_PixelFormatEnumToMasks SDL_PixelFormatEnumToMasks;
		public PFN_SDL_MasksToPixelFormatEnum SDL_MasksToPixelFormatEnum;
		public PFN_SDL_AllocFormat SDL_AllocFormat;
		public PFN_SDL_FreeFormat SDL_FreeFormat;
		public PFN_SDL_AllocPalette SDL_AllocPalette;
		public PFN_SDL_SetPixelFormatPalette SDL_SetPixelFormatPalette;
		public PFN_SDL_SetPaletteColors SDL_SetPaletteColors;
		public PFN_SDL_FreePalette SDL_FreePalette;
		public PFN_SDL_MapRGB SDL_MapRGB;
		public PFN_SDL_MapRGBA SDL_MapRGBA;
		public PFN_SDL_GetRGB SDL_GetRGB;
		public PFN_SDL_GetRGBA SDL_GetRGBA;
		public PFN_SDL_CalculateGammaRamp SDL_CalculateGammaRamp;

		// SDL_rwops.h

		/// <summary>
		/// Creates an RWOps stream from a file.
		/// </summary>
		/// <param name="file">Path to the file</param>
		/// <param name="mode">Mode to open file</param>
		/// <returns>File RWOps, or NULL on error</returns>
		[return: NativeType("SDL_RWops*")]
		public delegate IntPtr PFN_SDL_RWFromFile([MarshalAs(UnmanagedType.LPStr)] string file, [MarshalAs(UnmanagedType.LPStr)] string mode);
		/// <summary>
		/// Creates an RWOps stream from arbitrary memory.
		/// </summary>
		/// <param name="mem">Pointer to memory to wrap</param>
		/// <param name="size">Size of memory to wrap</param>
		/// <returns>Memory RWOps</returns>
		[return: NativeType("SDL_RWops*")]
		public delegate IntPtr PFN_SDL_RWFromMem(IntPtr mem, int size);
		/// <summary>
		/// Creates a read-only RWOps stream from arbitrary memory.
		/// </summary>
		/// <param name="mem">Pointer to memory to wrap</param>
		/// <param name="size">Size of memory to wrap</param>
		/// <returns>Constant memory RWOps</returns>
		[return: NativeType("SDL_RWops*")]
		public delegate IntPtr PFN_SDL_RWFromConstMem(IntPtr mem, int size);

		/// <summary>
		/// Allocates a new RWOps object.
		/// </summary>
		/// <returns>New RWOps, or NULL on error</returns>
		[return: NativeType("SDL_RWops*")]
		public delegate IntPtr PFN_SDL_AllocRW();
		/// <summary>
		/// Frees an allocated RWOps object.
		/// </summary>
		/// <param name="area">RWOps to free</param>
		public delegate void PFN_SDL_FreeRW([NativeType("SDL_RWops*")] IntPtr area);

		/// <summary>
		/// Gets the size of an RWOps stream, or -1 if unknown.
		/// </summary>
		/// <param name="context">RWOps stream</param>
		/// <returns>Stream size, or -1</returns>
		public delegate long PFN_SDL_RWsize([NativeType("SDL_RWops*")] IntPtr context);
		/// <summary>
		/// Seeks inside an RWOps stream.
		/// </summary>
		/// <param name="context">RWOps stream</param>
		/// <param name="offset">Offset to seek to</param>
		/// <param name="whence">Where to seek relative to</param>
		/// <returns>New stream position, or -1 on error</returns>
		public delegate long PFN_SDL_RWseek([NativeType("SDL_RWops*")] IntPtr context, long offset, SDLRWWhence whence);
		/// <summary>
		/// Gets the current offset inside an RWOps stream.
		/// </summary>
		/// <param name="context">RWOps stream</param>
		/// <returns>Current offset</returns>
		public delegate long PFN_SDL_RWtell([NativeType("SDL_RWops*")] IntPtr context);
		/// <summary>
		/// Reads a number of objects from an RWOps stream.
		/// </summary>
		/// <param name="context">RWOps stream</param>
		/// <param name="ptr">Pointer to read to</param>
		/// <param name="size">Size of objects to read</param>
		/// <param name="maxnum">Number of objects to read</param>
		/// <returns>The number of objects read, or 0 on error or end of file</returns>
		public delegate UIntPtr PFN_SDL_RWread([NativeType("SDL_RWops*")] IntPtr context, IntPtr ptr, UIntPtr size, UIntPtr maxnum);
		/// <summary>
		/// Writes a number of objects to an RWOps stream.
		/// </summary>
		/// <param name="context">RWOps stream</param>
		/// <param name="ptr">Pointer to write from</param>
		/// <param name="size">Size of objects to write</param>
		/// <param name="num">Number of objects to write</param>
		/// <returns>Number of objects written, or 0 on error or end of file</returns>
		public delegate UIntPtr PFN_SDL_RWwrite([NativeType("SDL_RWops*")] IntPtr context, IntPtr ptr, UIntPtr size, UIntPtr num);
		/// <summary>
		/// Closes and frees an RWOps stream.
		/// </summary>
		/// <param name="context">RWOps stream</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_RWclose([NativeType("SDL_RWops*")] IntPtr context);

		/// <summary>
		/// Loads all the data from an SDL data stream. The data is allocated with a zero byte at the end (null terminated). The data should
		/// be freed with <c>SDL_free()</c>.
		/// </summary>
		/// <param name="src">RWOps to read from</param>
		/// <param name="datasize">Size of data read</param>
		/// <param name="freesrc">If the RWOps should be freed once finished</param>
		/// <returns>The read data, or NULL if there was an error</returns>
		public delegate IntPtr PFN_SDL_LoadFile_RW([NativeType("SDL_RWops*")] IntPtr src, out UIntPtr datasize, int freesrc);
		/// <summary>
		/// Loads all the data from a file. The data is allocated with a zero byte at the end (null terminated). The data should be freed
		/// with <c>SDL_free()</c>
		/// </summary>
		/// <param name="file"></param>
		/// <param name="datasize"></param>
		/// <returns></returns>
		public delegate IntPtr PFN_SDL_LoadFile([MarshalAs(UnmanagedType.LPStr)] string file, out UIntPtr datasize);

		public delegate byte PFN_SDL_ReadU8([NativeType("SDL_RWops*")] IntPtr src);
		public delegate ushort PFN_SDL_ReadLE16([NativeType("SDL_RWops*")] IntPtr src);
		public delegate ushort PFN_SDL_ReadBE16([NativeType("SDL_RWops*")] IntPtr src);
		public delegate uint PFN_SDL_ReadLE32([NativeType("SDL_RWops*")] IntPtr src);
		public delegate uint PFN_SDL_ReadBE32([NativeType("SDL_RWops*")] IntPtr src);
		public delegate ulong PFN_SDL_ReadLE64([NativeType("SDL_RWops*")] IntPtr src);
		public delegate ulong PFN_SDL_ReadBE64([NativeType("SDL_RWops*")] IntPtr src);

		public delegate UIntPtr PFN_SDL_WriteU8([NativeType("SDL_RWops*")] IntPtr dst, byte value);
		public delegate UIntPtr PFN_SDL_WriteLE16([NativeType("SDL_RWops*")] IntPtr dst, ushort value);
		public delegate UIntPtr PFN_SDL_WriteBE16([NativeType("SDL_RWops*")] IntPtr dst, ushort value);
		public delegate UIntPtr PFN_SDL_WriteLE32([NativeType("SDL_RWops*")] IntPtr dst, uint value);
		public delegate UIntPtr PFN_SDL_WriteBE32([NativeType("SDL_RWops*")] IntPtr dst, uint value);
		public delegate UIntPtr PFN_SDL_WriteLE64([NativeType("SDL_RWops*")] IntPtr dst, ulong value);
		public delegate UIntPtr PFN_SDL_WriteBE64([NativeType("SDL_RWops*")] IntPtr dst, ulong value);

		public PFN_SDL_RWFromFile SDL_RWFromFile;
		public PFN_SDL_RWFromMem SDL_RWFromMem;
		public PFN_SDL_RWFromConstMem SDL_RWFromConstMem;
		public PFN_SDL_AllocRW SDL_AllocRW;
		public PFN_SDL_FreeRW SDL_FreeRW;
		public PFN_SDL_RWsize SDL_RWsize;
		public PFN_SDL_RWseek SDL_RWseek;
		public PFN_SDL_RWtell SDL_RWtell;
		public PFN_SDL_RWread SDL_RWread;
		public PFN_SDL_RWwrite SDL_RWwrite;
		public PFN_SDL_RWclose SDL_RWclose;
		public PFN_SDL_LoadFile_RW SDL_LoadFile_RW;
		public PFN_SDL_LoadFile SDL_LoadFile;
		public PFN_SDL_ReadU8 SDL_ReadU8;
		public PFN_SDL_ReadLE16 SDL_ReadLE16;
		public PFN_SDL_ReadBE16 SDL_ReadBE16;
		public PFN_SDL_ReadLE32 SDL_ReadLE32;
		public PFN_SDL_ReadBE32 SDL_ReadBE32;
		public PFN_SDL_ReadLE64 SDL_ReadLE64;
		public PFN_SDL_ReadBE64 SDL_ReadBE64;
		public PFN_SDL_WriteU8 SDL_WriteU8;
		public PFN_SDL_WriteLE16 SDL_WriteLE16;
		public PFN_SDL_WriteBE16 SDL_WriteBE16;
		public PFN_SDL_WriteLE32 SDL_WriteLE32;
		public PFN_SDL_WriteBE32 SDL_WriteBE32;
		public PFN_SDL_WriteLE64 SDL_WriteLE64;
		public PFN_SDL_WriteBE64 SDL_WriteBE64;

		// SDL_rect.h

		/// <summary>
		/// Tests if two rectangles intersect.
		/// </summary>
		/// <param name="a">First rectangle</param>
		/// <param name="b">Second rectangle</param>
		/// <returns>If the rectangles intersect</returns>
		public delegate SDLBool PFN_SDL_HasIntersection(in SDLRect a, in SDLRect b);
		/// <summary>
		/// Gets the intersection between two rectangles.
		/// </summary>
		/// <param name="a">First rectangle</param>
		/// <param name="b">Second rectangle</param>
		/// <param name="result">The area of intersection if there is one</param>
		/// <returns>If an intersection was detected</returns>
		public delegate SDLBool PFN_SDL_IntersectRect(in SDLRect a, in SDLRect b, out SDLRect result);
		/// <summary>
		/// Computes the union of two rectangles.
		/// </summary>
		/// <param name="a">First rectangle</param>
		/// <param name="b">Second rectangle</param>
		/// <param name="result">Union of two rectangles</param>
		public delegate void PFN_SDL_UnionRect(in SDLRect a, in SDLRect b, out SDLRect result);
		/// <summary>
		/// Computes a minimum rectangle to bound a set of points within an optional clipping rectangle.
		/// </summary>
		/// <param name="points">Array of points to check bounds</param>
		/// <param name="count">Number of points to check</param>
		/// <param name="clip">Clipping rectangle for points</param>
		/// <param name="result">Rectangle bounding clipped points</param>
		/// <returns>If any points were enclosed</returns>
		public delegate SDLBool PFN_SDL_EnclosePoints([NativeType("const SDL_Point*")] IntPtr points, int count, [NativeType("const SDL_Rect*")] IntPtr clip, out SDLRect result);
		/// <summary>
		/// Gets the intersection between a rectangle and a line, adjusting the points to within the rectangle boundary if so.
		/// </summary>
		/// <param name="rect">Rectangle</param>
		/// <param name="x1">First X position</param>
		/// <param name="y1">First Y position</param>
		/// <param name="x2">Second X position</param>
		/// <param name="y2">Second Y position</param>
		/// <returns>If the line and rectangle intersect</returns>
		public delegate SDLBool PFN_SDL_IntersectRectAndLine(in SDLRect rect, ref int x1, ref int y1, ref int x2, ref int y2);

		public PFN_SDL_HasIntersection SDL_HasIntersection;
		public PFN_SDL_IntersectRect SDL_IntersectRect;
		public PFN_SDL_UnionRect SDL_UnionRect;
		public PFN_SDL_EnclosePoints SDL_EnclosePoints;
		public PFN_SDL_IntersectRectAndLine SDL_IntersectRectAndLine;

		// SDL_blendmode.h

		public delegate SDLBlendMode PFN_SDL_ComposeCustomBlendMode(SDLBlendFactor srcColorFactor, SDLBlendFactor dstColorFactor, SDLBlendOperation colorOperation, SDLBlendFactor srcAlphaFactor, SDLBlendFactor dstAlphaFactor, SDLBlendOperation alphaOperation);

		public PFN_SDL_ComposeCustomBlendMode SDL_ComposeCustomBlendMode;

		// SDL_surface.h

		/// <summary>
		/// <para>Allocate an RGB surface.</para>
		/// <para>If the depth is 4 or 8 bits, an empty palette is allocated for the surface. If the depth
		/// is greater than 8 bits, the pixel format is set using the RGB mask.</para>
		/// </summary>
		/// <param name="flags">Obsolete, should be 0</param>
		/// <param name="width">Width of surface to create</param>
		/// <param name="height">Height of surface to create</param>
		/// <param name="depth">Bit depth of surface to create</param>
		/// <param name="rmask">Red mask</param>
		/// <param name="gmask">Green mask</param>
		/// <param name="bmask">Blue mask</param>
		/// <param name="amask">Alpha mask</param>
		/// <returns>Allocated surface, or NULL if out of memory</returns>
		[return: NativeType("SDL_Surface*")]
		public delegate IntPtr PFN_SDL_CreateRGBSurface(uint flags, int width, int height, int depth, uint rmask, uint gmask, uint bmask, uint amask);
		/// <summary>
		/// Allocates an RGB surface from a pixel format value.
		/// </summary>
		/// <param name="flags">Obsolete, should be 0</param>
		/// <param name="width">Width of surface to create</param>
		/// <param name="height">Height of surface to create</param>
		/// <param name="depth">Bit depth of surface to create</param>
		/// <param name="format">Pixel format of surface</param>
		/// <returns>Allocated surface, or NULL if out of memory</returns>
		[return: NativeType("SDL_Surface*")]
		public delegate IntPtr PFN_SDL_CreateRGBSurfaceWithFormat(uint flags, int width, int height, int depth, uint format);
		/// <summary>
		/// Creates an RGB surface like <c>SDL_CreateRGBSurface</c> but initialized to existing pixel data.
		/// </summary>
		/// <param name="pixels">Pointer to initial pixel data</param>
		/// <param name="width">Surface width</param>
		/// <param name="height">Surface height</param>
		/// <param name="depth">Surface bit depth</param>
		/// <param name="pitch">Surface pixel pitch</param>
		/// <param name="rmask">Red mask</param>
		/// <param name="gmask">Green mask</param>
		/// <param name="bmask">Blue mask</param>
		/// <param name="amask">Alpha mask</param>
		/// <returns>Allocated surface, or NULL if out of memory</returns>
		[return: NativeType("SDL_Surface*")]
		public delegate IntPtr PFN_SDL_CreateRGBSurfaceFrom(IntPtr pixels, int width, int height, int depth, int pitch, uint rmask, uint gmask, uint bmask, uint amask);
		/// <summary>
		/// Creates an RGB surface like <c>SDL_CreateRGBSurfaceWithFormat</c> but initialized to existing pixel data.
		/// </summary>
		/// <param name="pixels">Pointer to initial pixel data</param>
		/// <param name="width">Surface width</param>
		/// <param name="height">Surface height</param>
		/// <param name="depth">Surface bit depth</param>
		/// <param name="pitch">Surface pixel pitch</param>
		/// <param name="format">Pixel format of surface</param>
		/// <returns>Allocated surface, or NULL if out of memory</returns>
		[return: NativeType("SDL_Surface*")]
		public delegate IntPtr PFN_SDL_CreateRGBSurfaceWithFormatFrom(IntPtr pixels, int width, int height, int depth, int pitch, uint format);
		/// <summary>
		/// Frees a surface.
		/// </summary>
		/// <param name="surface">Surface to free</param>
		public delegate void PFN_SDL_FreeSurface([NativeType("SDL_Surface*")] IntPtr surface);
		/// <summary>
		/// Sets the palette used by a surface.
		/// </summary>
		/// <param name="surface">Surface to set palette on</param>
		/// <param name="palette">Palette to set</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_SetSurfacePalette([NativeType("SDL_Surface*")] IntPtr surface, [NativeType("SDL_Palette*")] IntPtr palette);
		/// <summary>
		/// Locks the pixels of a surface for direct access.
		/// </summary>
		/// <param name="surface">Surface to lock</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_LockSurface([NativeType("SDL_Surface*")] IntPtr surface);
		/// <summary>
		/// Unlocks the pixels of a surface.
		/// </summary>
		/// <param name="surface">Surface to unlock</param>
		public delegate void PFN_SDL_UnlockSurface([NativeType("SDL_Surface*")] IntPtr surface);
		/// <summary>
		/// Loads a surface from a BMP file using an RWOps stream.
		/// </summary>
		/// <param name="src">RWOps data stream</param>
		/// <param name="freesrc">If the stream should be freed after reading</param>
		/// <returns>The loaded surface, or NULL on error</returns>
		[return: NativeType("SDL_Surface*")]
		public delegate IntPtr PFN_SDL_LoadBMP_RW([NativeType("SDL_RWops*")] IntPtr src, int freesrc);
		/// <summary>
		/// Saves a surface to a BMP file using an RWOps stream.
		/// </summary>
		/// <param name="surface">Surface to save</param>
		/// <param name="dst">RWOps data stream</param>
		/// <param name="freedst">If the stream should be freed after reading</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_SaveBMP_RW([NativeType("SDL_Surface*")] IntPtr surface, [NativeType("SDL_RWops*")] IntPtr dst, int freedst);
		/// <summary>
		/// Sets the RLE acceleration hint for a surface.
		/// </summary>
		/// <param name="surface">Surface to set hint on</param>
		/// <param name="flag">RLE hint flag</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_SetSurfaceRLE([NativeType("SDL_Surface*")] IntPtr surface, int flag);
		/// <summary>
		/// Sets the color key (transparent pixel) on a blittable surface.
		/// </summary>
		/// <param name="surface">Surface to modify</param>
		/// <param name="flag">If the color key should be set or cleared</param>
		/// <param name="key">Color key to set</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_SetColorKey([NativeType("SDL_Surface*")] IntPtr surface, int flag, uint key);
		/// <summary>
		/// Test if a surface has a color key.
		/// </summary>
		/// <param name="surface">Surface to test</param>
		/// <returns>If the surface has a color key</returns>
		public delegate SDLBool PFN_SDL_HasColorKey([NativeType("SDL_Surface*")] IntPtr surface);
		/// <summary>
		/// Gets the color key for a surface.
		/// </summary>
		/// <param name="surface">Surface to get from</param>
		/// <param name="key">Color key of surface</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_GetColorKey([NativeType("SDL_Surface*")] IntPtr surface, out uint key);
		/// <summary>
		/// Sets the additional color value to use it blit operations.
		/// </summary>
		/// <param name="surface">Surface to modify</param>
		/// <param name="r">Red color value</param>
		/// <param name="g">Green color value</param>
		/// <param name="b">Blue color value</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_SetSurfaceColorMod([NativeType("SDL_Surface*")] IntPtr surface, byte r, byte g, byte b);
		/// <summary>
		/// Gets the additional color value to use in blit operations.
		/// </summary>
		/// <param name="surface">Surface to get from</param>
		/// <param name="r">Red color value</param>
		/// <param name="g">Green color value</param>
		/// <param name="b">Blue color value</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_GetSurfaceColorMod([NativeType("SDL_Surface*")] IntPtr surface, out byte r, out byte g, out byte b);
		/// <summary>
		/// Sets the additional alpha value to use in blit operations.
		/// </summary>
		/// <param name="surface">Surface to modify</param>
		/// <param name="alpha">Alpha value</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_SetSurfaceAlphaMod([NativeType("SDL_Surface*")] IntPtr surface, byte alpha);
		/// <summary>
		/// Gets the additional alpha value to use in blit operations.
		/// </summary>
		/// <param name="surface">Surface to get from</param>
		/// <param name="alpha">Alpha value</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_GetSurfaceAlphaMod([NativeType("SDL_Surface*")] IntPtr surface, out byte alpha);
		/// <summary>
		/// Sets the blend mode to use for blit operations.
		/// </summary>
		/// <param name="surface">Surface to modify</param>
		/// <param name="blendMode">Blend mode</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_SetSurfaceBlendMode([NativeType("SDL_Surface*")] IntPtr surface, SDLBlendMode blendMode);
		/// <summary>
		/// Gets the blend mode to use for blit operations.
		/// </summary>
		/// <param name="surface">Surface to get from</param>
		/// <param name="blendMode">Blend mode</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_GetSurfaceBlendMode([NativeType("SDL_Surface*")] IntPtr surface, out SDLBlendMode blendMode);
		/// <summary>
		/// Sets the clip area to use for blit operations.
		/// </summary>
		/// <param name="surface">Surface to modify</param>
		/// <param name="rect">Clip rectangle, or NULL to disable clipping</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate SDLBool PFN_SDL_SetClipRect([NativeType("SDL_Surface*")] IntPtr surface, [NativeType("SDL_Surface*")] IntPtr rect);
		/// <summary>
		/// Gets the clip area to use for blit operations.
		/// </summary>
		/// <param name="surface">Surface to get from</param>
		/// <param name="retc">Clip rectangle</param>
		public delegate void PFN_SDL_GetClipRect([NativeType("SDL_Surface*")] IntPtr surface, out SDLRect rect);
		/// <summary>
		/// Allocates and initializes a duplicate of a surface.
		/// </summary>
		/// <param name="surface">Surface to duplicate</param>
		/// <returns>Copy of surface, or NULL on error</returns>
		[return: NativeType("SDL_Surface*")]
		public delegate IntPtr PFN_SDL_DuplicateSurface([NativeType("SDL_Surface*")] IntPtr surface);
		/// <summary>
		/// Converts a surface to a new pixel format.
		/// </summary>
		/// <param name="src">Source surface</param>
		/// <param name="fmt">New pixel format</param>
		/// <param name="flags">Flags as passed to <c>SDL_CreateRGBSurface</c></param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate IntPtr PFN_SDL_ConvertSurface([NativeType("SDL_Surface*")] IntPtr src, [NativeType("const SDL_PixelFormat*")] IntPtr fmt, uint flags);
		/// <summary>
		/// Converts a surface to a new pixel format using a pixel format value.
		/// </summary>
		/// <param name="src">Source surface</param>
		/// <param name="pixelFormat">New pixel format</param>
		/// <param name="flags">Flags as passed to <c>SDL_CreateRGBSurface</c></param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate IntPtr PFN_SDL_ConvertSurfaceFormat([NativeType("SDL_Surface*")] IntPtr src, uint pixelFormat, uint flags);
		/// <summary>
		/// Converts a block of pixels between formats.
		/// </summary>
		/// <param name="width">Surface width</param>
		/// <param name="height">Surface height</param>
		/// <param name="srcFormat">Source format</param>
		/// <param name="src">Source pixels</param>
		/// <param name="srcPitch">Source pixel pitch</param>
		/// <param name="dstFormat">Destination format</param>
		/// <param name="dst">Destination pixels</param>
		/// <param name="dstPitch">Destination pixel pitch</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_ConvertPixels(int width, int height, uint srcFormat, IntPtr src, int srcPitch, uint dstFormat, IntPtr dst, int dstPitch);
		/// <summary>
		/// Fills a rectangle area on a surface with a constant color.
		/// </summary>
		/// <param name="dst">Destination surface</param>
		/// <param name="rect">Destination area</param>
		/// <param name="color">Color to fill</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_FillRect([NativeType("SDL_Surface*")] IntPtr dst, [NativeType("const SDL_Rect*")] IntPtr rect, uint color);
		/// <summary>
		/// Performs the same operation as <c>SDL_FillRect</c> on multiple areas.
		/// </summary>
		/// <param name="dst">Destination surface</param>
		/// <param name="rects">Areas to fill</param>
		/// <param name="count">Number of areas</param>
		/// <param name="color">Color to fill</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_FillRects([NativeType("SDL_Surface*")] IntPtr dst, [NativeType("const SDL_Rect*")] IntPtr rects, int count, uint color);
		/// <summary>
		/// <para>Performs a blit between surfaces.</para>
		/// <para>
		/// This assumes that source and destination areas are the same. If either srcrect or dstrect are NULL, the
		/// entire surface is copied. The final blit rectangles are saved in srcrect and dstrect after all clipping is performed.
		/// </para>
		/// </summary>
		/// <param name="src">Source surface</param>
		/// <param name="srcrect">Source area</param>
		/// <param name="dst">Destination surface</param>
		/// <param name="dstrect">Destination area</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_UpperBlit([NativeType("SDL_Surface*")] IntPtr src, [NativeType("const SDL_Rect*")] IntPtr srcrect, [NativeType("SDL_Surface*")] IntPtr dst, [NativeType("SDL_Rect*")] IntPtr dstrect);
		/// <summary>
		/// Performs low-level blitting. This should be avoided and <c>SDL_UpperBlit</c> should be used instead
		/// as it provides input validation and clipping.
		/// </summary>
		/// <param name="src">Source surface</param>
		/// <param name="srcrect">Source area</param>
		/// <param name="dst">Destination surface</param>
		/// <param name="dstrect">Destination area</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_LowerBlit([NativeType("SDL_Surface*")] IntPtr src, [NativeType("SDL_Rect*")] IntPtr srcrect, [NativeType("SDL_Surface*")] IntPtr dst, [NativeType("SDL_Rect*")] IntPtr dstrect);
		/// <summary>
		/// Performs a fast "soft strech" between surfaces.
		/// </summary>
		/// <param name="src">Source surface</param>
		/// <param name="srcrect">Source area</param>
		/// <param name="dst">Destination surface</param>
		/// <param name="dstrect">Destination area</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_SoftStretch([NativeType("SDL_Surface*")] IntPtr src, [NativeType("const SDL_Rect*")] IntPtr srcrect, [NativeType("SDL_Surface*")] IntPtr dst, [NativeType("const SDL_Rect*")] IntPtr dstrect);
		/// <summary>
		/// Performs a scaled blit between surfaces.
		/// </summary>
		/// <param name="src">Source surface</param>
		/// <param name="srcrect">Source area</param>
		/// <param name="dst">Destination surface</param>
		/// <param name="dstrect">Destination area</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_UpperBlitScaled([NativeType("SDL_Surface*")] IntPtr src, [NativeType("const SDL_Rect*")] IntPtr srcrect, [NativeType("SDL_Surface*")] IntPtr dst, [NativeType("SDL_Rect*")] IntPtr dstrect);
		/// <summary>
		/// Performs low-level scaled blitting. This should be avoided and <c>SDL_UpperBlitScaled</c> should be used instead
		/// as it provides input validation and clipping.
		/// </summary>
		/// <param name="src">Source surface</param>
		/// <param name="srcrect">Source area</param>
		/// <param name="dst">Destination surface</param>
		/// <param name="dstrect">Destination area</param>
		/// <returns>Zero on success, -1 on error</returns>
		public delegate int PFN_SDL_LowerBlitScaled([NativeType("SDL_Surface*")] IntPtr src, [NativeType("SDL_Rect*")] IntPtr srcrect, [NativeType("SDL_Surface*")] IntPtr dst, [NativeType("SDL_Rect*")] IntPtr dstrect);
		/// <summary>
		/// Sets the conversion mode for YUV surfaces.
		/// </summary>
		/// <param name="mode">YUV conversion mode</param>
		public delegate void PFN_SDL_SetYUVConversionMode(SDLYUVConversionMode mode);
		/// <summary>
		/// Gets the conversion mode for YUV surfaces.
		/// </summary>
		/// <returns>YUV conversion mode</returns>
		public delegate SDLYUVConversionMode PFN_SDL_GetYUVConversionMode();
		/// <summary>
		/// Gets the conversion mode for YUV surfaces given their size.
		/// </summary>
		/// <param name="width">Surface width</param>
		/// <param name="height">Surface height</param>
		/// <returns>YUV conversion mode</returns>
		public delegate SDLYUVConversionMode PFN_SDL_GetYUVConversionModeForResolution(int width, int height);

		public PFN_SDL_CreateRGBSurface SDL_CreateRGBSurface;
		public PFN_SDL_CreateRGBSurfaceWithFormat SDL_CreateRGBSurfaceWithFormat;
		public PFN_SDL_CreateRGBSurfaceFrom SDL_CreateRGBSurfaceFrom;
		public PFN_SDL_CreateRGBSurfaceWithFormatFrom SDL_CreateRGBSurfaceWithFormatFrom;
		public PFN_SDL_FreeSurface SDL_FreeSurface;
		public PFN_SDL_SetSurfacePalette SDL_SetSurfacePalette;
		public PFN_SDL_LockSurface SDL_LockSurface;
		public PFN_SDL_UnlockSurface SDL_UnlockSurface;
		public PFN_SDL_LoadBMP_RW SDL_LoadBMP_RW;
		public PFN_SDL_SaveBMP_RW SDL_SaveBMP_RW;
		public PFN_SDL_SetSurfaceRLE SDL_SetSurfaceRLE;
		public PFN_SDL_SetColorKey SDL_SetColorKey;
		public PFN_SDL_HasColorKey SDL_HasColorKey;
		public PFN_SDL_GetColorKey SDL_GetColorKey;
		public PFN_SDL_SetSurfaceColorMod SDL_SetSurfaceColorMod;
		public PFN_SDL_GetSurfaceColorMod SDL_GetSurfaceColorMod;
		public PFN_SDL_SetSurfaceAlphaMod SDL_SetSurfaceAlphaMod;
		public PFN_SDL_GetSurfaceAlphaMod SDL_GetSurfaceAlphaMod;
		public PFN_SDL_SetSurfaceBlendMode SDL_SetSurfaceBlendMode;
		public PFN_SDL_GetSurfaceBlendMode SDL_GetSurfaceBlendMode;
		public PFN_SDL_SetClipRect SDL_SetClipRect;
		public PFN_SDL_GetClipRect SDL_GetClipRect;
		public PFN_SDL_DuplicateSurface SDL_DuplicateSurface;
		public PFN_SDL_ConvertSurface SDL_ConvertSurface;
		public PFN_SDL_ConvertSurfaceFormat SDL_ConvertSurfaceFormat;
		public PFN_SDL_ConvertPixels SDL_ConvertPixels;
		public PFN_SDL_FillRect SDL_FillRect;
		public PFN_SDL_FillRects SDL_FillRects;
		public PFN_SDL_UpperBlit SDL_UpperBlit;
		public PFN_SDL_LowerBlit SDL_LowerBlit;
		public PFN_SDL_SoftStretch SDL_SoftStretch;
		public PFN_SDL_UpperBlitScaled SDL_UpperBlitScaled;
		public PFN_SDL_LowerBlitScaled SDL_LowerBlitScaled;
		public PFN_SDL_SetYUVConversionMode SDL_SetYUVConversionMode;
		public PFN_SDL_GetYUVConversionMode SDL_GetYUVConversionMode;
		public PFN_SDL_GetYUVConversionModeForResolution SDL_GetYUVConversionModeForResolution;

		// SDL_video.h

		/// <summary>
		/// Gets the number of video drivers compiled into SDL.
		/// </summary>
		/// <returns>Number of video drivers</returns>
		public delegate int PFN_SDL_GetNumVideoDrivers();
		/// <summary>
		/// Gets the name of a builtin video driver.
		/// </summary>
		/// <param name="index">Video driver index</param>
		/// <returns>Name of video driver</returns>
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GetVideoDriver(int index);
		/// <summary>
		/// Initializes the video subsystem, optionally with a specific video driver.
		/// </summary>
		/// <param name="driverName">Name of video driver to use, or null for default</param>
		/// <returns>Zero on success, -1 on error</returns>
		public delegate int PFN_SDL_VideoInit([MarshalAs(UnmanagedType.LPStr)] string driverName);
		/// <summary>
		/// Shuts down the video subsystem.
		/// </summary>
		public delegate void PFN_SDL_VideoQuit();
		/// <summary>
		/// Gets the current video driver name.
		/// </summary>
		/// <returns>Video driver name</returns>
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GetCurrentVideoDriver();
		/// <summary>
		/// Gets the number of video displays.
		/// </summary>
		/// <returns>Number of video displays</returns>
		public delegate int PFN_SDL_GetNumVideoDisplays();
		/// <summary>
		/// Gets the name of a display.
		/// </summary>
		/// <param name="displayIndex">Display index</param>
		/// <returns>Name of display</returns>
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GetDisplayName(int displayIndex);
		/// <summary>
		/// Gets the desktop area represented by a display.
		/// </summary>
		/// <param name="displayIndex">Display index</param>
		/// <param name="rect">Display desktop area</param>
		/// <returns>Zero on success, or -1 on error</returns>
		public delegate int PFN_SDL_GetDisplayBounds(int displayIndex, out SDLRect rect);
		public delegate int PFN_SDL_GetDisplayUsableBounds(int displayIndex, out SDLRect rect);
		public delegate int PFN_SDL_GetDisplayDPI(int displayIndex, out float ddpi, out float hdpi, out float vdpi);
		public delegate SDLDisplayOrientation PFN_SDL_GetDisplayOrientation(int displayIndex);
		public delegate int PFN_SDL_GetNumDisplayModes(int displayIndex);
		public delegate int PFN_SDL_GetDisplayMode(int displayIndex, int modeIndex, out SDLDisplayMode mode);
		public delegate int PFN_SDL_GetDesktopDisplayMode(int displayIndex, out SDLDisplayMode mode);
		public delegate int PFN_SDL_GetCurrentDisplayMode(int displayIndex, out SDLDisplayMode mode);
		[return: NativeType("SDL_DisplayMode*")]
		public delegate IntPtr PFN_SDL_GetClosestDisplayMode(int displayIndex, in SDLDisplayMode mode, out SDLDisplayMode closest);
		public delegate int PFN_SDL_GetWindowDisplayIndex([NativeType("SDL_Window*")] IntPtr window);
		public delegate int PFN_SDL_SetWindowDisplayMode([NativeType("SDL_Window*")] IntPtr window, in SDLDisplayMode mode);
		public delegate int PFN_SDL_GetWindowDisplayMode([NativeType("SDL_Window*")] IntPtr window, out SDLDisplayMode mode);
		public delegate uint PFN_SDL_GetWindowPixelFormat([NativeType("SDL_Window*")] IntPtr window);
		[return: NativeType("SDL_Window*")]
		public delegate IntPtr PFN_SDL_CreateWindow([MarshalAs(UnmanagedType.LPUTF8Str)] string title, int x, int y, int w, int h, uint flags);
		[return: NativeType("SDL_Window*")]
		public delegate IntPtr PFN_SDL_CreateWindowFrom(IntPtr data);
		public delegate uint PFN_SDL_GetWindowID([NativeType("SDL_Window*")] IntPtr window);
		[return: NativeType("SDL_Window*")]
		public delegate IntPtr PFN_SDL_GetWindowFromID(uint id);
		public delegate uint PFN_SDL_GetWindowFlags([NativeType("SDL_Window*")] IntPtr window);
		public delegate void PFN_SDL_SetWindowTitle([NativeType("SDL_Window*")] IntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string title);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GetWindowTitle([NativeType("SDL_Window*")] IntPtr window);
		public delegate void PFN_SDL_SetWindowIcon([NativeType("SDL_Window*")] IntPtr window, [NativeType("SDL_Surface*")] IntPtr icon);
		public delegate IntPtr PFN_SDL_SetWindowData([NativeType("SDL_Window*")] IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string name, IntPtr userdata);
		public delegate IntPtr PFN_SDL_GetWindowData([NativeType("SDL_Window*")] IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string name);
		public delegate void PFN_SDL_SetWindowPosition([NativeType("SDL_Window*")] IntPtr window, int x, int y);
		public delegate void PFN_SDL_GetWindowPosition([NativeType("SDL_Window*")] IntPtr window, out int x, out int y);
		public delegate void PFN_SDL_SetWindowSize([NativeType("SDL_Window*")] IntPtr window, int w, int h);
		public delegate void PFN_SDL_GetWindowSize([NativeType("SDL_Window*")] IntPtr window, out int w, out int h);
		public delegate int PFN_SDL_GetWindowBordersSize([NativeType("SDL_Window*")] IntPtr window, out int top, out int left, out int bottom, out int right);
		public delegate void PFN_SDL_SetWindowMinimumSize([NativeType("SDL_Window*")] IntPtr window, int minw, int minh);
		public delegate void PFN_SDL_GetWindowMinimumSize([NativeType("SDL_Window*")] IntPtr window, out int minw, out int minh);
		public delegate void PFN_SDL_SetWindowMaximumSize([NativeType("SDL_Window*")] IntPtr window, int maxw, int maxh);
		public delegate void PFN_SDL_GetWindowMaximumSize([NativeType("SDL_Window*")] IntPtr window, out int maxw, out int maxh);
		public delegate void PFN_SDL_SetWindowBordered([NativeType("SDL_Window*")] IntPtr window, SDLBool bordered);
		public delegate void PFN_SDL_SetWindowResizable([NativeType("SDL_Window*")] IntPtr window, SDLBool resizable);
		public delegate void PFN_SDL_ShowWindow([NativeType("SDL_Window*")] IntPtr window);
		public delegate void PFN_SDL_HideWindow([NativeType("SDL_Window*")] IntPtr window);
		public delegate void PFN_SDL_RaiseWindow([NativeType("SDL_Window*")] IntPtr window);
		public delegate void PFN_SDL_MaximizeWindow([NativeType("SDL_Window*")] IntPtr window);
		public delegate void PFN_SDL_MinimizeWindow([NativeType("SDL_Window*")] IntPtr window);
		public delegate void PFN_SDL_RestoreWindow([NativeType("SDL_Window*")] IntPtr window);
		public delegate int PFN_SDL_SetWindowFullscreen([NativeType("SDL_Window*")] IntPtr window, uint flags);
		[return: NativeType("SDL_Surface*")]
		public delegate IntPtr PFN_SDL_GetWindowSurface([NativeType("SDL_Window*")] IntPtr window);
		public delegate int PFN_SDL_UpdateWindowSurface([NativeType("SDL_Window*")] IntPtr window);
		public delegate int PFN_SDL_UpdateWindowSurfaceRects([NativeType("SDL_Window*")] IntPtr window, [NativeType("const SDL_Rect*")] IntPtr rects, int numrects);
		public delegate void PFN_SDL_SetWindowGrab([NativeType("SDL_Window*")] IntPtr window, SDLBool grabbed);
		public delegate SDLBool PFN_SDL_GetWindowGrab([NativeType("SDL_Window*")] IntPtr window);
		[return: NativeType("SDL_Window*")]
		public delegate IntPtr PFN_SDL_GetGrabbedWindow();
		public delegate int PFN_SDL_SetWindowBrightness([NativeType("SDL_Window*")] IntPtr window, float brightness);
		public delegate float PFN_SDL_GetWindowBrightness([NativeType("SDL_Window*")] IntPtr window);
		public delegate int PFN_SDL_SetWindowOpacity([NativeType("SDL_Window*")] IntPtr window, float opacity);
		public delegate int PFN_SDL_GetWindowOpacity([NativeType("SDL_Window*")] IntPtr window, out float opacity);
		public delegate int PFN_SDL_SetWindowModalFor([NativeType("SDL_Window*")] IntPtr modalWindow, [NativeType("SDL_Window*")] IntPtr parentWindow);
		public delegate int PFN_SDL_SetWindowInputFocus([NativeType("SDL_Window*")] IntPtr window);
		public delegate int PFN_SDL_SetWindowGammaRamp([NativeType("SDL_Window*")] IntPtr window, [NativeType("const Uint16*")] IntPtr red, [NativeType("const Uint16*")] IntPtr green, [NativeType("const Uint16*")] IntPtr blue);
		public delegate int PFN_SDL_GetWindowGammaRamp([NativeType("SDL_Window*")] IntPtr window, [NativeType("const Uint16*")] IntPtr red, [NativeType("const Uint16*")] IntPtr green, [NativeType("const Uint16*")] IntPtr blue);
		public delegate int PFN_SDL_SetWindowHitTest([NativeType("SDL_Window*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] SDLHitTest callback, IntPtr callbackData);
		public delegate void PFN_SDL_DestroyWindow([NativeType("SDL_Window*")] IntPtr window);
		public delegate SDLBool PFN_SDL_IsScreenSaverEnabled();
		public delegate void PFN_SDL_EnableScreenSaver();
		public delegate void PFN_SDL_DisableScreenSaver();
		public delegate int PFN_SDL_GL_LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string path);
		public delegate IntPtr PFN_SDL_GL_GetProcAddress([MarshalAs(UnmanagedType.LPStr)] string proc);
		public delegate void PFN_SDL_GL_UnloadLibrary();
		public delegate SDLBool PFN_SDL_GL_ExtensionSupported([MarshalAs(UnmanagedType.LPStr)] string extension);
		public delegate void PFN_SDL_GL_ResetAttributes();
		public delegate int PFN_SDL_GL_SetAttribute(SDLGLAttr attr, int value);
		public delegate int PFN_SDL_GL_GetAttribute(SDLGLAttr attr, out int value);
		[return: NativeType("SDL_GLContext")]
		public delegate IntPtr PFN_SDL_GL_CreateContext([NativeType("SDL_Window*")] IntPtr window);
		public delegate int PFN_SDL_GL_MakeCurrent([NativeType("SDL_Window*")] IntPtr window, [NativeType("SDL_GLContext")] IntPtr context);
		[return: NativeType("SDL_Window*")]
		public delegate IntPtr PFN_SDL_GL_GetCurrentWindow();
		[return: NativeType("SDL_GLContext")]
		public delegate IntPtr PFN_SDL_GL_GetCurrentContext();
		public delegate void PFN_SDL_GL_GetDrawableSize([NativeType("SDL_Window*")] IntPtr window, out int w, out int h);
		public delegate int PFN_SDL_GL_SetSwapInterval(int interval);
		public delegate int PFN_SDL_GL_GetSwapInterval();
		public delegate void PFN_SDL_GL_SwapWindow([NativeType("SDL_Window*")] IntPtr window);
		public delegate void PFN_SDL_GL_DeleteContext([NativeType("SDL_GLContext")] IntPtr context);

		public PFN_SDL_GetNumVideoDrivers SDL_GetNumVideoDrivers;
		public PFN_SDL_GetVideoDriver SDL_GetVideoDriver;
		public PFN_SDL_VideoInit SDL_VideoInit;
		public PFN_SDL_VideoQuit SDL_VideoQuit;
		public PFN_SDL_GetCurrentVideoDriver SDL_GetCurrentVideoDriver;
		public PFN_SDL_GetNumVideoDisplays SDL_GetNumVideoDisplays;
		public PFN_SDL_GetDisplayName SDL_GetDisplayName;
		public PFN_SDL_GetDisplayBounds SDL_GetDisplayBounds;
		public PFN_SDL_GetDisplayUsableBounds SDL_GetDisplayUsableBounds;
		public PFN_SDL_GetDisplayDPI SDL_GetDisplayDPI;
		public PFN_SDL_GetDisplayOrientation SDL_GetDisplayOrientation;
		public PFN_SDL_GetNumDisplayModes SDL_GetNumDisplayModes;
		public PFN_SDL_GetDisplayMode SDL_GetDisplayMode;
		public PFN_SDL_GetDesktopDisplayMode SDL_GetDesktopDisplayMode;
		public PFN_SDL_GetCurrentDisplayMode SDL_GetCurrentDisplayMode;
		public PFN_SDL_GetClosestDisplayMode SDL_GetClosestDisplayMode;
		public PFN_SDL_GetWindowDisplayIndex SDL_GetWindowDisplayIndex;
		public PFN_SDL_SetWindowDisplayMode SDL_SetWindowDisplayMode;
		public PFN_SDL_GetWindowDisplayMode SDL_GetWindowDisplayMode;
		public PFN_SDL_GetWindowPixelFormat SDL_GetWindowPixelFormat;
		public PFN_SDL_CreateWindow SDL_CreateWindow;
		public PFN_SDL_CreateWindowFrom SDL_CreateWindowFrom;
		public PFN_SDL_GetWindowID SDL_GetWindowID;
		public PFN_SDL_GetWindowFromID SDL_GetWindowFromID;
		public PFN_SDL_GetWindowFlags SDL_GetWindowFlags;
		public PFN_SDL_SetWindowTitle SDL_SetWindowTitle;
		public PFN_SDL_GetWindowTitle SDL_GetWindowTitle;
		public PFN_SDL_SetWindowIcon SDL_SetWindowIcon;
		public PFN_SDL_SetWindowData SDL_SetWindowData;
		public PFN_SDL_GetWindowData SDL_GetWindowData;
		public PFN_SDL_SetWindowPosition SDL_SetWindowPosition;
		public PFN_SDL_GetWindowPosition SDL_GetWindowPosition;
		public PFN_SDL_SetWindowSize SDL_SetWindowSize;
		public PFN_SDL_GetWindowSize SDL_GetWindowSize;
		public PFN_SDL_GetWindowBordersSize SDL_GetWindowBordersSize;
		public PFN_SDL_SetWindowMinimumSize SDL_SetWindowMinimumSize;
		public PFN_SDL_GetWindowMinimumSize SDL_GetWindowMinimumSize;
		public PFN_SDL_SetWindowMaximumSize SDL_SetWindowMaximumSize;
		public PFN_SDL_GetWindowMaximumSize SDL_GetWindowMaximumSize;
		public PFN_SDL_SetWindowBordered SDL_SetWindowBordered;
		public PFN_SDL_SetWindowResizable SDL_SetWindowResizable;
		public PFN_SDL_ShowWindow SDL_ShowWindow;
		public PFN_SDL_HideWindow SDL_HideWindow;
		public PFN_SDL_RaiseWindow SDL_RaiseWindow;
		public PFN_SDL_MaximizeWindow SDL_MaximizeWindow;
		public PFN_SDL_MinimizeWindow SDL_MinimizeWindow;
		public PFN_SDL_RestoreWindow SDL_RestoreWindow;
		public PFN_SDL_SetWindowFullscreen SDL_SetWindowFullscreen;
		public PFN_SDL_GetWindowSurface SDL_GetWindowSurface;
		public PFN_SDL_UpdateWindowSurface SDL_UpdateWindowSurface;
		public PFN_SDL_UpdateWindowSurfaceRects SDL_UpdateWindowSurfaceRects;
		public PFN_SDL_SetWindowGrab SDL_SetWindowGrab;
		public PFN_SDL_GetWindowGrab SDL_GetWindowGrab;
		public PFN_SDL_GetGrabbedWindow SDL_GetGrabbedWindow;
		public PFN_SDL_SetWindowBrightness SDL_SetWindowBrightness;
		public PFN_SDL_GetWindowBrightness SDL_GetWindowBrightness;
		public PFN_SDL_SetWindowOpacity SDL_SetWindowOpacity;
		public PFN_SDL_GetWindowOpacity SDL_GetWindowOpacity;
		public PFN_SDL_SetWindowModalFor SDL_SetWindowModalFor;
		public PFN_SDL_SetWindowInputFocus SDL_SetWindowInputFocus;
		public PFN_SDL_SetWindowGammaRamp SDL_SetWindowGammaRamp;
		public PFN_SDL_GetWindowGammaRamp SDL_GetWindowGammaRamp;
		public PFN_SDL_SetWindowHitTest SDL_SetWindowHitTest;
		public PFN_SDL_DestroyWindow SDL_DestroyWindow;
		public PFN_SDL_IsScreenSaverEnabled SDL_IsScreenSaverEnabled;
		public PFN_SDL_EnableScreenSaver SDL_EnableScreenSaver;
		public PFN_SDL_DisableScreenSaver SDL_DisableScreenSaver;
		public PFN_SDL_GL_LoadLibrary SDL_GL_LoadLibrary;
		public PFN_SDL_GL_GetProcAddress SDL_GL_GetProcAddress;
		public PFN_SDL_GL_UnloadLibrary SDL_GL_UnloadLibrary;
		public PFN_SDL_GL_ExtensionSupported SDL_GL_ExtensionSupported;
		public PFN_SDL_GL_ResetAttributes SDL_GL_ResetAttributes;
		public PFN_SDL_GL_SetAttribute SDL_GL_SetAttribute;
		public PFN_SDL_GL_GetAttribute SDL_GL_GetAttribute;
		public PFN_SDL_GL_CreateContext SDL_GL_CreateContext;
		public PFN_SDL_GL_MakeCurrent SDL_GL_MakeCurrent;
		public PFN_SDL_GL_GetCurrentWindow SDL_GL_GetCurrentWindow;
		public PFN_SDL_GL_GetCurrentContext SDL_GL_GetCurrentContext;
		public PFN_SDL_GL_GetDrawableSize SDL_GL_GetDrawableSize;
		public PFN_SDL_GL_SetSwapInterval SDL_GL_SetSwapInterval;
		public PFN_SDL_GL_GetSwapInterval SDL_GL_GetSwapInterval;
		public PFN_SDL_GL_SwapWindow SDL_GL_SwapWindow;
		public PFN_SDL_GL_DeleteContext SDL_GL_DeleteContext;

		// SDL_keyboard.h

		[return: NativeType("SDL_Window*")]
		public delegate IntPtr PFN_SDL_GetKeyboardFocus();
		[return: NativeType("const Uint8*")]
		public delegate IntPtr PFN_SDL_GetKeyboardState(out int numkeys);
		public delegate SDLKeymod PFN_SDL_GetModState();
		public delegate void PFN_SDL_SetModState(SDLKeymod modstate);
		public delegate SDLKeycode PFN_SDL_GetKeyFromScancode(SDLScancode scancode);
		public delegate SDLScancode PFN_SDL_GetScancodeFromKey(SDLKeycode key);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GetScancodeName(SDLScancode scancode);
		public delegate SDLScancode PFN_SDL_GetScancodeFromName([MarshalAs(UnmanagedType.LPStr)] string name);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GetKeyName(SDLKeycode key);
		public delegate SDLKeycode PFN_SDL_GetKeyFromName([MarshalAs(UnmanagedType.LPStr)] string name);
		public delegate void PFN_SDL_StartTextInput();
		public delegate SDLBool PFN_SDL_IsTextInputActive();
		public delegate void PFN_SDL_StopTextInput();
		public delegate void PFN_SDL_SetTextInputRect(in SDLRect rect);
		public delegate SDLBool PFN_SDL_HasScreenKeyboardSupport();
		public delegate SDLBool PFN_SDL_IsScreenKeyboardShown([NativeType("SDL_Window*")] IntPtr window);

		public PFN_SDL_GetKeyboardFocus SDL_GetKeyboardFocus;
		public PFN_SDL_GetKeyboardState SDL_GetKeyboardState;
		public PFN_SDL_GetModState SDL_GetModState;
		public PFN_SDL_SetModState SDL_SetModState;
		public PFN_SDL_GetKeyFromScancode SDL_GetKeyFromScancode;
		public PFN_SDL_GetScancodeFromKey SDL_GetScancodeFromKey;
		public PFN_SDL_GetScancodeName SDL_GetScancodeName;
		public PFN_SDL_GetScancodeFromName SDL_GetScancodeFromName;
		public PFN_SDL_GetKeyName SDL_GetKeyName;
		public PFN_SDL_GetKeyFromName SDL_GetKeyFromName;
		public PFN_SDL_StartTextInput SDL_StartTextInput;
		public PFN_SDL_IsTextInputActive SDL_IsTextInputActive;
		public PFN_SDL_StopTextInput SDL_StopTextInput;
		public PFN_SDL_SetTextInputRect SDL_SetTextInputRect;
		public PFN_SDL_HasScreenKeyboardSupport SDL_HasScreenKeyboardSupport;
		public PFN_SDL_IsScreenKeyboardShown SDL_IsScreenKeyboardShown;

		// SDL_mouse.h

		[return: NativeType("SDL_Window*")]
		public delegate IntPtr PFN_SDL_GetMouseFocus();
		public delegate uint PFN_SDL_GetMouseState(out int x, out int y);
		public delegate uint PFN_SDL_GetGlobalMouseState(out int x, out int y);
		public delegate uint PFN_SDL_GetRelativeMouseState(out int x, out int y);
		public delegate void PFN_SDL_WarpMouseInWindow([NativeType("SDL_Window*")] IntPtr window, int x, int y);
		public delegate int PFN_SDL_WarpMouseGlobal(int x, int y);
		public delegate int PFN_SDL_SetRelativeMouseMode(SDLBool enabled);
		public delegate int PFN_SDL_CaptureMouse(SDLBool enabled);
		public delegate SDLBool PFN_SDL_GetRelativeMouseMode();
		[return: NativeType("SDL_Cursor*")]
		public delegate IntPtr PFN_SDL_CreateCursor([NativeType("const Uint8*")] IntPtr data, [NativeType("const Uint8*")] IntPtr mask, int w, int h, int hot_x, int hot_y);
		[return: NativeType("SDL_Cursor*")]
		public delegate IntPtr PFN_SDL_CreateColorCursor([NativeType("const SDL_Surface*")] IntPtr surface, int hot_x, int hot_y);
		[return: NativeType("SDL_Cursor*")]
		public delegate IntPtr PFN_SDL_CreateSystemCursor(SDLSystemCursor id);
		public delegate void PFN_SDL_SetCursor([NativeType("SDL_Cursor*")] IntPtr cursor);
		[return: NativeType("SDL_Cursor*")]
		public delegate IntPtr PFN_SDL_GetCursor();
		[return: NativeType("SDL_Cursor*")]
		public delegate IntPtr PFN_SDL_GetDefaultCursor();
		public delegate void PFN_SDL_FreeCursor([NativeType("SDL_Cursor*")] IntPtr cursor);
		public delegate int PFN_SDL_ShowCursor(int toggle);

		public PFN_SDL_GetMouseFocus SDL_GetMouseFocus;
		public PFN_SDL_GetMouseState SDL_GetMouseState;
		public PFN_SDL_GetGlobalMouseState SDL_GetGlobalMouseState;
		public PFN_SDL_GetRelativeMouseState SDL_GetRelativeMouseState;
		public PFN_SDL_WarpMouseInWindow SDL_WarpMouseInWindow;
		public PFN_SDL_WarpMouseGlobal SDL_WarpMouseGlobal;
		public PFN_SDL_SetRelativeMouseMode SDL_SetRelativeMouseMode;
		public PFN_SDL_CaptureMouse SDL_CaptureMouse;
		public PFN_SDL_GetRelativeMouseMode SDL_GetRelativeMouseMode;
		public PFN_SDL_CreateCursor SDL_CreateCursor;
		public PFN_SDL_CreateColorCursor SDL_CreateColorCursor;
		public PFN_SDL_CreateSystemCursor SDL_CreateSystemCursor;
		public PFN_SDL_SetCursor SDL_SetCursor;
		public PFN_SDL_GetCursor SDL_GetCursor;
		public PFN_SDL_GetDefaultCursor SDL_GetDefaultCursor;
		public PFN_SDL_ShowCursor SDL_ShowCursor;
		public PFN_SDL_FreeCursor SDL_FreeCursor;

		// SDL_joystick.h

		public delegate void PFN_SDL_LockJoysticks();
		public delegate void PFN_SDL_UnlockJoysticks();
		public delegate int PFN_SDL_NumJoysticks();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_JoystickNameForIndex(int deviceIndex);
		public delegate int PFN_SDL_JoystickGetDevicePlayerIndex(int deviceIndex);
		public delegate Guid PFN_SDL_JoystickGetDeviceGUID(int deviceIndex);
		public delegate ushort PFN_SDL_JoystickGetDeviceVendor(int deviceIndex);
		public delegate ushort PFN_SDL_JoystickGetDeviceProduct(int deviceIndex);
		public delegate ushort PFN_SDL_JoystickGetDeviceProductVersion(int deviceIndex);
		public delegate SDLJoystickType PFN_SDL_JoystickGetDeviceType(int deviceIndex);
		public delegate int PFN_SDL_JoystickGetDeviceInstanceID(int deviceIndex);
		[return: NativeType("SDL_Joystick*")]
		public delegate IntPtr PFN_SDL_JoystickOpen(int deviceIndex);
		[return: NativeType("SDL_Joystick*")]
		public delegate IntPtr PFN_SDL_JoystickFromInstanceID(int instanceID);
		[return: NativeType("SDL_Joystick*")]
		public delegate IntPtr PFN_SDL_JoystickFromPlayerIndex(int playerIndex);
		public delegate int PFN_SDL_JoystickAttachVirtual(SDLJoystickType type, int naxes, int nbuttons, int nhats);
		public delegate int PFN_SDL_JoystickDetachVirtual(int deviceIndex);
		public delegate SDLBool PFN_SDL_JoystickIsVirtual(int deviceIndex);
		public delegate int PFN_SDL_JoystickSetVirtualAxis([NativeType("SDL_Joystick*")] IntPtr joystick, int axis, short value);
		public delegate int PFN_SDL_JoystickSetVirtualButton([NativeType("SDL_Joystick*")] IntPtr joystick, int button, SDLButtonState state);
		public delegate int PFN_SDL_JoystickSetVirtualHat([NativeType("SDL_Joystick*")] IntPtr joystick, int hat, SDLButtonState state);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_JoystickName([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate int PFN_SDL_JoystickGetPlayerIndex([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate void PFN_SDL_JoystickSetPlayerIndex([NativeType("SDL_Joystick*")] IntPtr joystick, int playerIndex);
		public delegate Guid PFN_SDL_JoystickGetGUID([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate ushort PFN_SDL_JoystickGetVendor([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate ushort PFN_SDL_JoystickGetProduct([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate ushort PFN_SDL_JoystickGetProductVersion([NativeType("SDL_Joystick*")] IntPtr joystick);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_JoystickGetSerial([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate SDLJoystickType PFN_SDL_JoystickGetType([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate void PFN_SDL_JoystickGetGUIDString(Guid guid, [NativeType("char*")] IntPtr pszGUID, int cbGUID);
		public delegate Guid PFN_SDL_JoystickGetGUIDFromString([MarshalAs(UnmanagedType.LPStr)] string pchGUID);
		public delegate SDLBool PFN_SDL_JoystickGetAttached([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate int PFN_SDL_JoystickInstanceID([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate int PFN_SDL_JoystickNumAxes([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate int PFN_SDL_JoystickNumBalls([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate int PFN_SDL_JoystickNumHats([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate int PFN_SDL_JoystickNumButtons([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate void PFN_SDL_JoystickUpdate();
		public delegate int PFN_SDL_JoystickEventState(int state);
		public delegate short PFN_SDL_JoystickGetAxis([NativeType("SDL_Joystick*")] IntPtr joystick, int axis);
		public delegate SDLBool PFN_SDL_JoystickGetAxisInitialState([NativeType("SDL_Joystick*")] IntPtr joystick, int axis, out short state);
		public delegate SDLHat PFN_SDL_JoystickGetHat([NativeType("SDL_Joystick*")] IntPtr joystick, int hat);
		public delegate int PFN_SDL_JoystickGetBall([NativeType("SDL_Joystick*")] IntPtr joystick, int ball, out int dx, out int dy);
		public delegate SDLButtonState PFN_SDL_JoystickGetButton([NativeType("SDL_Joystick*")] IntPtr joystick, int button);
		public delegate int PFN_SDL_JoystickRumble([NativeType("SDL_Joystick*")] IntPtr joystick, ushort lowFreqRumble, ushort highFreqRumble, uint durationMS);
		public delegate int PFN_SDL_JoystickRumbleTriggers([NativeType("SDL_Joystick*")] IntPtr joystick, ushort leftRumble, ushort rightRumble, uint durationMS);
		public delegate SDLBool PFN_SDL_JoystickHasLED([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate int PFN_SDL_JoystickSetLED([NativeType("SDL_Joystick*")] IntPtr joystick, byte red, byte green, byte blue);
		public delegate void PFN_SDL_JoystickClose([NativeType("SDL_Joystick*")] IntPtr joystick);
		public delegate SDLJoystickPowerLevel PFN_SDL_JoystickCurrentPowerLevel([NativeType("SDL_Joystick*")] IntPtr joystick);

		public PFN_SDL_LockJoysticks SDL_LockJoysticks;
		public PFN_SDL_UnlockJoysticks SDL_UnlockJoysticks;
		public PFN_SDL_NumJoysticks SDL_NumJoysticks;
		public PFN_SDL_JoystickNameForIndex SDL_JoystickNameForIndex;
		public PFN_SDL_JoystickGetDevicePlayerIndex SDL_JoystickGetDevicePlayerIndex;
		public PFN_SDL_JoystickGetDeviceGUID SDL_JoystickGetDeviceGUID;
		public PFN_SDL_JoystickGetDeviceVendor SDL_JoystickGetDeviceVendor;
		public PFN_SDL_JoystickGetDeviceProduct SDL_JoystickGetDeviceProduct;
		public PFN_SDL_JoystickGetDeviceProductVersion SDL_JoystickGetDeviceProductVersion;
		public PFN_SDL_JoystickGetDeviceType SDL_JoystickGetDeviceType;
		public PFN_SDL_JoystickGetDeviceInstanceID SDL_JoystickGetDeviceInstanceID;
		public PFN_SDL_JoystickOpen SDL_JoystickOpen;
		public PFN_SDL_JoystickFromInstanceID SDL_JoystickFromInstanceID;
		public PFN_SDL_JoystickFromPlayerIndex SDL_JoystickFromPlayerIndex;
		public PFN_SDL_JoystickAttachVirtual SDL_JoystickAttachVirtual;
		public PFN_SDL_JoystickDetachVirtual SDL_JoystickDetachVirtual;
		public PFN_SDL_JoystickIsVirtual SDL_JoystickIsVirtual;
		public PFN_SDL_JoystickSetVirtualAxis SDL_JoystickSetVirtualAxis;
		public PFN_SDL_JoystickSetVirtualButton SDL_JoystickSetVirtualButton;
		public PFN_SDL_JoystickSetVirtualHat SDL_JoystickSetVirtualHat;
		public PFN_SDL_JoystickName SDL_JoystickName;
		public PFN_SDL_JoystickGetPlayerIndex SDL_JoystickGetPlayerIndex;
		public PFN_SDL_JoystickSetPlayerIndex SDL_JoystickSetPlayerIndex;
		public PFN_SDL_JoystickGetGUID SDL_JoystickGetGUID;
		public PFN_SDL_JoystickGetVendor SDL_JoystickGetVendor;
		public PFN_SDL_JoystickGetProduct SDL_JoystickGetProduct;
		public PFN_SDL_JoystickGetProductVersion SDL_JoystickGetProductVersion;
		public PFN_SDL_JoystickGetSerial SDL_JoystickGetSerial;
		public PFN_SDL_JoystickGetType SDL_JoystickGetType;
		public PFN_SDL_JoystickGetGUIDString SDL_JoystickGetGUIDString;
		public PFN_SDL_JoystickGetGUIDFromString SDL_JoystickGetGUIDFromString;
		public PFN_SDL_JoystickGetAttached SDL_JoystickGetAttached;
		public PFN_SDL_JoystickInstanceID SDL_JoystickInstanceID;
		public PFN_SDL_JoystickNumAxes SDL_JoystickNumAxes;
		public PFN_SDL_JoystickNumBalls SDL_JoystickNumBalls;
		public PFN_SDL_JoystickNumHats SDL_JoystickNumHats;
		public PFN_SDL_JoystickNumButtons SDL_JoystickNumButtons;
		public PFN_SDL_JoystickUpdate SDL_JoystickUpdate;
		public PFN_SDL_JoystickEventState SDL_JoystickEventState;
		public PFN_SDL_JoystickGetAxis SDL_JoystickGetAxis;
		public PFN_SDL_JoystickGetAxisInitialState SDL_JoystickGetAxisInitialState;
		public PFN_SDL_JoystickGetHat SDL_JoystickGetHat;
		public PFN_SDL_JoystickGetBall SDL_JoystickGetBall;
		public PFN_SDL_JoystickGetButton SDL_JoystickGetButton;
		public PFN_SDL_JoystickRumble SDL_JoystickRumble;
		public PFN_SDL_JoystickRumbleTriggers SDL_JoystickRumbleTriggers;
		public PFN_SDL_JoystickHasLED SDL_JoystickHasLED;
		public PFN_SDL_JoystickSetLED SDL_JoystickSetLED;
		public PFN_SDL_JoystickClose SDL_JoystickClose;
		public PFN_SDL_JoystickCurrentPowerLevel SDL_JoystickCurrentPowerLevel;

		// SDL_gamecontroller.h

		public delegate int PFN_SDL_GameControllerAddMappingsFromRW([NativeType("SDL_RWops*")] IntPtr rw, int freerw);
		public delegate int PFN_SDL_GameControllerAddMapping([MarshalAs(UnmanagedType.LPStr)] string mappingstr);
		public delegate int PFN_SDL_GameControllerNumMappings();
		// Note: Must free after use
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GameControllerMappingForIndex(int mappingIndex);
		// Note: Must free after use
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GameControllerMappingForGUID(Guid guid);
		// Note: Must free after use
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GameControllerMapping([NativeType("SDL_GameController*")] IntPtr gameController);
		public delegate SDLBool PFN_SDL_IsGameController(int joystickIndex);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GameControllerNameForIndex(int joystickIndex);
		public delegate SDLGameControllerType PFN_SDL_GameControllerTypeForIndex(int joystickIndex);
		// Note: Must free after use
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GameControllerMappingForDeviceIndex(int joystickIndex);
		[return: NativeType("SDL_GameController*")]
		public delegate IntPtr PFN_SDL_GameControllerOpen(int joystickIndex);
		[return: NativeType("SDL_GameController*")]
		public delegate IntPtr PFN_SDL_GameControllerFromInstanceID(int joystickID);
		[return: NativeType("SDL_GameController*")]
		public delegate IntPtr PFN_SDL_GameControllerFromPlayerIndex(int playerIndex);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GameControllerName([NativeType("SDL_GameController*")] IntPtr gameController);
		public delegate SDLGameControllerType PFN_SDL_GameControllerGetType([NativeType("SDL_GameController*")] IntPtr gameController);
		public delegate int PFN_SDL_GameControllerGetPlayerIndex([NativeType("SDL_GameController*")] IntPtr gameController);
		public delegate void PFN_SDL_GameControllerSetPlayerIndex([NativeType("SDL_GameController*")] IntPtr gameController, int playerIndex);
		public delegate ushort PFN_SDL_GameControllerGetVendor([NativeType("SDL_GameController*")] IntPtr gameController);
		public delegate ushort PFN_SDL_GameControllerGetProduct([NativeType("SDL_GameController*")] IntPtr gameController);
		public delegate ushort PFN_SDL_GameControllerGetProductVersion([NativeType("SDL_GameController*")] IntPtr gameController);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GameControllerGetSerial([NativeType("SDL_GameController*")] IntPtr gameController);
		public delegate SDLBool PFN_SDL_GameControllerGetAttached([NativeType("SDL_GameController*")] IntPtr gameController);
		[return: NativeType("SDL_Joystick*")]
		public delegate IntPtr PFN_SDL_GameControllerGetJoystick([NativeType("SDL_GameController*")] IntPtr gameController);
		public delegate int PFN_SDL_GameControllerEventState(int state);
		public delegate void PFN_SDL_GameControllerUpdate();
		public delegate SDLGameControllerAxis PFN_SDL_GameControllerGetAxisFromString([MarshalAs(UnmanagedType.LPStr)] string pchString);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GameControllerGetStringForAxis(SDLGameControllerAxis axis);
		public delegate SDLGameControllerButtonBind PFN_SDL_GameControllerGetBindForAxis([NativeType("SDL_GameController*")] IntPtr gameController, SDLGameControllerAxis axis);
		public delegate SDLBool PFN_SDL_GameControllerHasAxis([NativeType("SDL_GameController*")] IntPtr gameController, SDLGameControllerAxis axis);
		public delegate short PFN_SDL_GameControllerGetAxis([NativeType("SDL_GameController*")] IntPtr gameController, SDLGameControllerAxis axis);
		public delegate SDLGameControllerButton PFN_SDL_GameControllerGetButtonFromString([MarshalAs(UnmanagedType.LPStr)] string pchString);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_SDL_GameControllerGetStringForButton(SDLGameControllerButton button);
		public delegate SDLGameControllerButtonBind PFN_SDL_GameControllerGetBindForButton([NativeType("SDL_GameController*")] IntPtr gameController, SDLGameControllerButton button);
		public delegate SDLBool PFN_SDL_GameControllerHasButton([NativeType("SDL_GameController*")] IntPtr gameController, SDLGameControllerButton button);
		public delegate SDLButtonState PFN_SDL_GameControllerGetButton([NativeType("SDL_GameController*")] IntPtr gameController, SDLGameControllerButton button);
		public delegate int PFN_SDL_GameControllerGetNumTouchpads([NativeType("SDL_GameController*")] IntPtr gameController);
		public delegate int PFN_SDL_GameControllerGetNumTouchpadFingers([NativeType("SDL_GameController*")] IntPtr gameController, int touchpad);
		public delegate int PFN_SDL_GameControllerGetTouchpadFinger([NativeType("SDL_GameController*")] IntPtr gameController, int touchpad, int finger, out SDLButtonState state, out float x, out float y, out float pressure);
		public delegate SDLBool PFN_SDL_GameControllerHasSensor([NativeType("SDL_GameController*")] IntPtr gameController, SDLSensorType type);
		public delegate int PFN_SDL_GameControllerSetSensorEnabled([NativeType("SDL_GameController*")] IntPtr gameController, SDLSensorType type, SDLBool enabled);
		public delegate SDLBool PFN_SDL_GameControllerIsSensorEnabled([NativeType("SDL_GameController*")] IntPtr gameController, SDLSensorType type);
		public delegate int PFN_SDL_GameControllerGetSensorData([NativeType("SDL_GameController*")] IntPtr gameController, SDLSensorType type, [NativeType("float*")] IntPtr data, int numValues);
		public delegate int PFN_SDL_GameControllerRumble([NativeType("SDL_GameController*")] IntPtr gameController, ushort lowFreqRumble, ushort highFreqRumble, uint durationMS);
		public delegate int PFN_SDL_GameControllerRumbleTriggers([NativeType("SDL_GameController*")] IntPtr gameController, ushort leftRumble, ushort rightRumble, uint durationMS);
		public delegate SDLBool PFN_SDL_GameControllerHasLED([NativeType("SDL_GameController*")] IntPtr gameController);
		public delegate int PFN_SDL_GameControllerSetLED([NativeType("SDL_GameController*")] IntPtr gameController, byte r, byte g, byte b);
		public delegate void PFN_SDL_GameControllerClose([NativeType("SDL_GameController*")] IntPtr gameController);

		public PFN_SDL_GameControllerAddMappingsFromRW SDL_GameControllerAddMappingsFromRW;
		public PFN_SDL_GameControllerAddMapping SDL_GameControllerAddMapping;
		public PFN_SDL_GameControllerNumMappings SDL_GameControllerNumMappings;
		public PFN_SDL_GameControllerMappingForIndex SDL_GameControllerMappingForIndex;
		public PFN_SDL_GameControllerMappingForGUID SDL_GameControllerMappingForGUID;
		public PFN_SDL_GameControllerMapping SDL_GameControllerMapping;
		public PFN_SDL_IsGameController SDL_IsGameController;
		public PFN_SDL_GameControllerNameForIndex SDL_GameControllerNameForIndex;
		public PFN_SDL_GameControllerTypeForIndex SDL_GameControllerTypeForIndex;
		public PFN_SDL_GameControllerMappingForDeviceIndex SDL_GameControllerMappingForDeviceIndex;
		public PFN_SDL_GameControllerOpen SDL_GameControllerOpen;
		public PFN_SDL_GameControllerFromInstanceID SDL_GameControllerFromInstanceID;
		public PFN_SDL_GameControllerFromPlayerIndex SDL_GameControllerFromPlayerIndex;
		public PFN_SDL_GameControllerName SDL_GameControllerName;
		public PFN_SDL_GameControllerGetType SDL_GameControllerGetType;
		public PFN_SDL_GameControllerGetPlayerIndex SDL_GameControllerGetPlayerIndex;
		public PFN_SDL_GameControllerSetPlayerIndex SDL_GameControllerSetPlayerIndex;
		public PFN_SDL_GameControllerGetVendor SDL_GameControllerGetVendor;
		public PFN_SDL_GameControllerGetProduct SDL_GameControllerGetProduct;
		public PFN_SDL_GameControllerGetProductVersion SDL_GameControllerGetProductVersion;
		public PFN_SDL_GameControllerGetSerial SDL_GameControllerGetSerial;
		public PFN_SDL_GameControllerGetAttached SDL_GameControllerGetAttached;
		public PFN_SDL_GameControllerGetJoystick SDL_GameControllerGetJoystick;
		public PFN_SDL_GameControllerEventState SDL_GameControllerEventState;
		public PFN_SDL_GameControllerUpdate SDL_GameControllerUpdate;
		public PFN_SDL_GameControllerGetAxisFromString SDL_GameControllerGetAxisFromString;
		public PFN_SDL_GameControllerGetStringForAxis SDL_GameControllerGetStringForAxis;
		public PFN_SDL_GameControllerGetBindForAxis SDL_GameControllerGetBindForAxis;
		public PFN_SDL_GameControllerHasAxis SDL_GameControllerHasAxis;
		public PFN_SDL_GameControllerGetAxis SDL_GameControllerGetAxis;
		public PFN_SDL_GameControllerGetButtonFromString SDL_GameControllerGetButtonFromString;
		public PFN_SDL_GameControllerGetStringForButton SDL_GameControllerGetStringForButton;
		public PFN_SDL_GameControllerGetBindForButton SDL_GameControllerGetBindForButton;
		public PFN_SDL_GameControllerHasButton SDL_GameControllerHasButton;
		public PFN_SDL_GameControllerGetButton SDL_GameControllerGetButton;
		public PFN_SDL_GameControllerGetNumTouchpads SDL_GameControllerGetNumTouchpads;
		public PFN_SDL_GameControllerGetNumTouchpadFingers SDL_GameControllerGetNumTouchpadFingers;
		public PFN_SDL_GameControllerGetTouchpadFinger SDL_GameControllerGetTouchpadFinger;
		public PFN_SDL_GameControllerHasSensor SDL_GameControllerHasSensor;
		public PFN_SDL_GameControllerSetSensorEnabled SDL_GameControllerSetSensorEnabled;
		public PFN_SDL_GameControllerIsSensorEnabled SDL_GameControllerIsSensorEnabled;
		public PFN_SDL_GameControllerGetSensorData SDL_GameControllerGetSensorData;
		public PFN_SDL_GameControllerRumble SDL_GameControllerRumble;
		public PFN_SDL_GameControllerRumbleTriggers SDL_GameControllerRumbleTriggers;
		public PFN_SDL_GameControllerHasLED SDL_GameControllerHasLED;
		public PFN_SDL_GameControllerSetLED SDL_GameControllerSetLED;
		public PFN_SDL_GameControllerClose SDL_GameControllerClose;

		// SDL_touch.h

		public delegate int PFN_SDL_GetNumTouchDevices();
		public delegate long PFN_SDL_GetTouchDevice(int index);
		public delegate SDLTouchDeviceType PFN_SDL_GetTouchDeviceType(long touchID);
		public delegate int PFN_SDL_GetNumTouchFingers(long touchID);
		[return: NativeType("SDL_Finger*")]
		public delegate IntPtr PFN_SDL_GetTouchFinger(long touchID, int index);

		public PFN_SDL_GetNumTouchDevices SDL_GetNumTouchDevices;
		public PFN_SDL_GetTouchDevice SDL_GetTouchDevice;
		public PFN_SDL_GetTouchDeviceType SDL_GetTouchDeviceType;
		public PFN_SDL_GetNumTouchFingers SDL_GetNumTouchFingers;
		public PFN_SDL_GetTouchFinger SDL_GetTouchFinger;

		// SDL_events.h

		public delegate void PFN_SDL_PumpEvents();
		public delegate int PFN_SDL_PeepEvents([NativeType("SDL_Event*")] IntPtr events, int numevents, SDLEventAction action, uint minType, uint maxType);
		public delegate SDLBool PFN_SDL_HasEvent(uint type);
		public delegate SDLBool PFN_SDL_HasEvents(uint minType, uint maxType);
		public delegate void PFN_SDL_FlushEvent(uint type);
		public delegate void PFN_SDL_FlushEvents(uint minType, uint maxType);
		public delegate int PFN_SDL_PollEvent(out SDLEvent _event);
		public delegate int PFN_SDL_WaitEvent(out SDLEvent _event);
		public delegate int PFN_SDL_WaitEventTimeout(ref SDLEvent _event, int timeout);
		public delegate int PFN_SDL_PushEvent(in SDLEvent _event);
		public delegate void PFN_SDL_SetEventFilter([MarshalAs(UnmanagedType.FunctionPtr)] SDLEventFilter filter, IntPtr userdata);
		public delegate bool PFN_SDL_GetEventFilter([NativeType("SDL_EventFilter*")] out IntPtr filter, [NativeType("void**")] out IntPtr userdata);
		public delegate void PFN_SDL_AddEventWatch([MarshalAs(UnmanagedType.FunctionPtr)] SDLEventFilter filter, IntPtr userdata);
		public delegate void PFN_SDL_DelEventWatch([MarshalAs(UnmanagedType.FunctionPtr)] SDLEventFilter filter, IntPtr userdata);
		public delegate void PFN_SDL_FilterEvents([MarshalAs(UnmanagedType.FunctionPtr)] SDLEventFilter filter, IntPtr userdata);
		public delegate byte PFN_SDL_EventState(uint type, byte state);
		public delegate uint PFN_SDL_RegisterEvents(int numEvents);

		public PFN_SDL_PumpEvents SDL_PumpEvents;
		public PFN_SDL_PeepEvents SDL_PeepEvents;
		public PFN_SDL_HasEvent SDL_HasEvent;
		public PFN_SDL_HasEvents SDL_HasEvents;
		public PFN_SDL_FlushEvent SDL_FlushEvent;
		public PFN_SDL_FlushEvents SDL_FlushEvents;
		public PFN_SDL_PollEvent SDL_PollEvent;
		public PFN_SDL_WaitEvent SDL_WaitEvent;
		public PFN_SDL_WaitEventTimeout SDL_WaitEventTimeout;
		public PFN_SDL_PushEvent SDL_PushEvent;
		public PFN_SDL_SetEventFilter SDL_SetEventFilter;
		public PFN_SDL_GetEventFilter SDL_GetEventFilter;
		public PFN_SDL_AddEventWatch SDL_AddEventWatch;
		public PFN_SDL_DelEventWatch SDL_DelEventWatch;
		public PFN_SDL_FilterEvents SDL_FilterEvents;
		public PFN_SDL_EventState SDL_EventState;
		public PFN_SDL_RegisterEvents SDL_RegisterEvents;

		// SDL_cpuinfo.h

		public delegate int PFN_SDL_GetCPUCount();
		public delegate int PFN_SDL_GetCPUCacheLineSize();
		public delegate SDLBool PFN_SDL_HasRDTSC();
		public delegate SDLBool PFN_SDL_HasAltiVec();
		public delegate SDLBool PFN_SDL_HasMMX();
		public delegate SDLBool PFN_SDL_Has3DNow();
		public delegate SDLBool PFN_SDL_HasSSE();
		public delegate SDLBool PFN_SDL_HasSSE2();
		public delegate SDLBool PFN_SDL_HasSSE3();
		public delegate SDLBool PFN_SDL_HasSSE41();
		public delegate SDLBool PFN_SDL_HasSSE42();
		public delegate SDLBool PFN_SDL_HasAVX();
		public delegate SDLBool PFN_SDL_HasAVX2();
		public delegate SDLBool PFN_SDL_HasAVX512F();
		public delegate SDLBool PFN_SDL_HasARMSIMD();
		public delegate SDLBool PFN_SDL_HasNEON();
		public delegate int PFN_SDL_GetSystemRAM();
		public delegate nuint PFN_SDL_SIMDGetAlignment();
		public delegate IntPtr PFN_SDL_SIMDAlloc(nuint len);
		public delegate IntPtr PFN_SDL_SIMDRealloc(IntPtr mem, nuint len);
		public delegate void PFN_SDL_SIMDFree(IntPtr mem);

		public PFN_SDL_GetCPUCount SDL_GetCPUCount;
		public PFN_SDL_GetCPUCacheLineSize SDL_GetCPUCacheLineSize;
		public PFN_SDL_HasRDTSC SDL_HasRDTSC;
		public PFN_SDL_HasAltiVec SDL_HasAltiVec;
		public PFN_SDL_HasMMX SDL_HasMMX;
		public PFN_SDL_Has3DNow SDL_Has3DNow;
		public PFN_SDL_HasSSE SDL_HasSSE;
		public PFN_SDL_HasSSE2 SDL_HasSSE2;
		public PFN_SDL_HasSSE3 SDL_HasSSE3;
		public PFN_SDL_HasSSE41 SDL_HasSSE41;
		public PFN_SDL_HasSSE42 SDL_HasSSE42;
		public PFN_SDL_HasAVX SDL_HasAVX;
		public PFN_SDL_HasAVX2 SDL_HasAVX2;
		public PFN_SDL_HasAVX512F SDL_HasAVX512F;
		public PFN_SDL_HasARMSIMD SDL_HasARMSIMD;
		public PFN_SDL_HasNEON SDL_HasNEON;
		public PFN_SDL_GetSystemRAM SDL_GetSystemRAM;
		public PFN_SDL_SIMDGetAlignment SDL_SIMDGetAlignment;
		public PFN_SDL_SIMDAlloc SDL_SIMDAlloc;
		public PFN_SDL_SIMDRealloc SDL_SIMDRealloc;
		public PFN_SDL_SIMDFree SDL_SIMDFree;

	}

}

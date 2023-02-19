using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Graphics;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;
using Tesseract.Core.Resource;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Engine {

	public class Texture : IEngineObject, IDisposable {

		public TesseractEngine Engine { get; }

		public ITexture RawTexture { get; }

		public ITextureView RawTextureView { get; }

		internal Texture(TesseractEngine engine) {
			Engine = engine;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Engine.ThreadSafety == ThreadSafetyLevel.Concurrent) DisposeImpl();
			else Engine.InvokeInRenderThread(DisposeImpl);
		}

		private void DisposeImpl() {
			RawTextureView.Dispose();
			RawTexture.Dispose();
		}

	}

	/// <summary>
	/// An atlas contains a set of sprites on a single texture.
	/// </summary>
	public class AtlasTexture : IEngineObject, IDisposable {

		public TesseractEngine Engine { get; }

		/// <summary>
		/// The size of the atlas in pixels.
		/// </summary>
		public Vector2i Size { get; private set; }

		/// <summary>
		/// The scale of an individual pixel in texel units.
		/// </summary>
		public Vector2 PixelScale { get; private set; }

		/// <summary>
		/// A small shift to apply to texel coordinates to ensure they cover the appropriate pixels
		/// </summary>
		public Vector2 TexelShift => PixelScale * 0.02f;

		/// <summary>
		/// The UV coordinates of a white pixel on the atlas, to allow drawing flat colors
		/// without needing to switch textures.
		/// </summary>
		public Vector2 WhitePixelUV { get; private set; }

		// The list of all sprites in the atlas
		private readonly List<AtlasTextureRef> sprites = new();

		/// <summary>
		/// The sprite to use for 
		/// </summary>
		public AtlasTextureRef? MissingTextureSprite { get; internal set; } = null;

		internal AtlasTexture(TesseractEngine engine) {
			Engine = engine;
		}

		/// <summary>
		/// Adds an image to the atlas, returning a sprite within the atlas which will be valid once
		/// the atlas is built. An area within the image to use may be specified.
		/// </summary>
		/// <param name="image">The image to add to the atlas</param>
		/// <param name="area">The area of the image to use, or null to use the whole image</param>
		/// <returns>Sprite within the atlas</returns>
		/// <exception cref="ArgumentException">If the image does not have a format of <see cref="PixelFormat.R8G8B8A8UNorm"/></exception>
		public AtlasTextureRef Add(IImage image, Recti? area = null) {
			if (!image.Format.Equals(PixelFormat.R8G8B8A8UNorm)) throw new ArgumentException("Image must have RGBA8 pixel format", nameof(image));
			AtlasTextureRef sprite = new(this, image, area ?? new Recti(image.Size.X, image.Size.Y));
			sprites.Add(sprite);
			return sprite;
		}

		/// <summary>
		/// Adds a magenta-black checkerboard sprite and sets the missing texture sprite to it.
		/// </summary>
		/// <returns>Missing texture sprite</returns>
		public AtlasTextureRef AddDefaultMissingTexture() {
			return (MissingTextureSprite = Add(new ArrayImage(2, 2, PixelFormat.R8G8B8A8UNorm, new byte[] {
				0xFF, 0, 0xFF, 0xFF,   0, 0, 0, 0xFF,
				0, 0, 0, 0xFF,   0xFF, 0, 0xFF, 0xFF
			})));
		}

		// The list of images owned by this atlas
		private readonly List<IImage> ownedImages = new();

		/// <summary>
		/// Adds a sprite using the image at the specified location.
		/// </summary>
		/// <param name="location">Sprite image location</param>
		/// <returns>Added sprite</returns>
		public AtlasTextureRef Add(ResourceLocation location) {
			IImage image = Engine.ImageIO.Load(location);
			ownedImages.Add(image);
			return Add(image);
		}

		private void SortSprites() {
			Vector2i size = default;

			// Sort sprites in descending size (largest first)
			sprites.Sort((a, b) => {
				int sizeA = a.Size.X * a.Size.Y;
				int sizeB = b.Size.X * b.Size.Y;
				return -sizeA.CompareTo(sizeB);
			});

			// List of areas on the in-progress atlas that are free
			List<Recti> freeAreas = new();

			// Sorts the free area list
			void SortFreeAreas() => freeAreas.Sort((a, b) => {
				// Sort free areas in ascending order (smallest first)
				int sizeA = a.Size.X * a.Size.Y;
				int sizeB = b.Size.X * b.Size.Y;
				return sizeA.CompareTo(sizeB);
			});

			foreach (AtlasTextureRef sprite in sprites) {
				Vector2i spriteSize = sprite.Size;

				// The first sprite always gets assigned at 0,0
				if (size.X == 0) {
					size = spriteSize;
					sprite.Position = default;
					continue;
				}

				// Try to find a free area that the sprite can be assigned to first
				bool hasAssigned = false;
				for (int i = 0; i < freeAreas.Count; i++) {
					Recti area = freeAreas[i];
					// If area is large enough to hold sprite
					if (area.Size.X >= spriteSize.X && area.Size.Y >= spriteSize.Y) {
						// Remove area and set sprite position to the origin of the area
						freeAreas.RemoveAt(i);
						sprite.Position = area.Position;
						// Compute remaining space outside the sprite
						Vector2i space = area.Size - sprite.Size;
						if (space.X > 0 || space.Y > 0) {
							// If free space is larger in width than height
							if (space.X > space.Y) {
								// Create free area to the right of the sprite
								freeAreas.Add(new Recti(area.Position.X + sprite.Size.X, area.Position.Y, sprite.Size.X - area.Size.X, area.Size.Y));
								// If sprite does not fill the whole height, add another free area below it
								if (space.Y > 0)
									freeAreas.Add(new Recti(area.Position.X, area.Position.Y + sprite.Size.Y, sprite.Size.X, area.Size.Y - sprite.Size.Y));
							} else {
								// Create free area below the sprite
								freeAreas.Add(new Recti(area.Position.X, area.Position.Y + sprite.Size.Y, area.Size.X, area.Size.Y - sprite.Size.Y));
								// If sprite does not fill the whole width, add another free area to its right
								if (space.X > 0)
									freeAreas.Add(new Recti(area.Position.X + sprite.Size.X, area.Position.Y, sprite.Size.X - area.Size.X, area.Size.Y));
							}
							SortFreeAreas();
						}
						hasAssigned = true;
						break;
					}
				}
				if (hasAssigned) continue;

				if (size.Y >= size.X) {
					// Expand in the X axis primarily
					sprite.Position = new Vector2i(size.X, 0);
					size.X += spriteSize.X;
					size.Y = Math.Max(size.Y, spriteSize.Y);
					// Add any free area created by expansion
					if (sprite.Size.Y < size.Y) {
						freeAreas.Add(new Recti(size.X, sprite.Size.Y, sprite.Size.X, size.Y - sprite.Size.Y));
						SortFreeAreas();
					} else if (sprite.Size.Y > size.Y) {
						freeAreas.Add(new Recti(0, size.Y, size.X, sprite.Size.Y - size.Y));
						SortFreeAreas();
					}
				} else {
					// Expand in the Y axis primarily
					sprite.Position = new Vector2i(0, size.Y);
					size.X = Math.Max(size.X, spriteSize.X);
					size.Y += spriteSize.Y;
					// Add any free area created by expansion
					if (sprite.Size.X < size.X) {
						freeAreas.Add(new Recti(sprite.Size.X, size.Y, size.X - sprite.Size.X, sprite.Size.Y));
						SortFreeAreas();
					} else if (sprite.Size.X > size.X) {
						freeAreas.Add(new Recti(size.X, 0, sprite.Size.X - size.X, size.Y));
						SortFreeAreas();
					}
				}
			}

			// Set the size and pixel scale of the atlas
			Size = size;
			PixelScale = new Vector2(1) / (Vector2)Size;

			// Generate the coordinates for all sprites
			sprites.ForEach(sprite => sprite.OnBuild());
		}

		/// <summary>
		/// Builds the atlas, updating all sprites with their locations and generating the atlas texture.
		/// </summary>
		/// <exception cref="InvalidOperationException">If the atlas has already been built</exception>
		public void Build() {
			// Add sprite for white pixel
			AtlasTextureRef whitePixel = new(this, new Vector2i(2, 2));
			sprites.Add(whitePixel);

			// Sort sprites and assign positions
			SortSprites();

			// Assign white pixel
			Span<uint> pixelSquare = stackalloc uint[4];
			pixelSquare.Fill(0xFFFFFFFF);
			WhitePixelUV = (whitePixel.MaxUV + whitePixel.MinUV) * 0.5f; // Coordinate is the center of the 2x2 square

			// Add each sprite to the atlas
			sprites.ForEach(sprite => {
				IImage? image = sprite.Image;
				if (image != null) {
					Recti area = sprite.ImageArea;
				}
			});

			// Cleanup any owned images
			foreach (IImage image in ownedImages) image.Dispose();
			ownedImages.Clear();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			sprites.Clear();
		}

	}

	/// <summary>
	/// A sprite is an image within a texture that can be used for rendering.
	/// </summary>
	public class AtlasTextureRef {

		/// <summary>
		/// The atlas this sprite belongs to.
		/// </summary>
		public AtlasTexture Atlas { get; }

		/// <summary>
		/// The pixel position of the sprite within its atlas.
		/// </summary>
		public Vector2i Position { get; internal set; }

		/// <summary>
		/// The pixel size of the sprite.
		/// </summary>
		public Vector2i Size { get; }

		/// <summary>
		/// The minimum (top-left) coordinates of the sprite within its atlas.
		/// </summary>
		public Vector2 MinUV { get; private set; }

		/// <summary>
		/// The maximum (bottom-right) coordinates of the sprite within its atlas.
		/// </summary>
		public Vector2 MaxUV { get; private set; }

		/// <summary>
		/// The list of frames this sprite will use.
		/// </summary>
		public TextureFrameList? FrameList { get; set; }

		internal IImage? Image;
		internal Recti ImageArea;

		internal AtlasTextureRef(AtlasTexture atlas, IImage image, Recti area) {
			Atlas = atlas;
			Image = image;
			// Constrain area to within the image
			area.Size = area.Size.Min((new Vector2i(image.Size) - area.Position).Max(default));
			ImageArea = area;
			Size = area.Size;
		}

		internal AtlasTextureRef(AtlasTexture atlas, Vector2i size) {
			Atlas = atlas;
			Image = null;
			Size = size;
		}

		internal void OnBuild() {
			Vector2 scale = Atlas.PixelScale;
			MinUV = (Vector2)Position * scale;
			MaxUV = MinUV + (Vector2)Size * scale;
			MinUV += Atlas.TexelShift;
			MaxUV -= Atlas.TexelShift;
			FrameList?.Generate(this);
		}

		public void UseAsMissingTexture() => Atlas.MissingTextureSprite = this;

	}

	/// <summary>
	/// A frame list specifies a list of 
	/// </summary>
	public class TextureFrameList {

		/// <summary>
		/// The pixel size of the frame.
		/// </summary>
		public Vector2i FrameSize { get; }

		/// <summary>
		/// The list of pixel offsets of each frame.
		/// </summary>
		public Vector2i[] FrameOffsets { get; }

		/// <summary>
		/// The texel (UV) size of the frame.
		/// </summary>
		public Vector2 FrameUVSize { get; internal set; }

		/// <summary>
		/// The list of UV offsets of each frame.
		/// </summary>
		public Vector2[] FrameUVOffsets { get; }

		/// <summary>
		/// Creates a new frame list with the given frame size and offset list.
		/// </summary>
		/// <param name="frameSize">The size of the sprite frame</param>
		/// <param name="frameOffsets">The list of frame offsets</param>
		public TextureFrameList(Vector2i frameSize, params Vector2i[] frameOffsets) {
			FrameSize = frameSize;
			FrameOffsets = frameOffsets;
			FrameUVOffsets = new Vector2[frameOffsets.Length];
		}

		internal void Generate(AtlasTextureRef sprite) {
			Vector2 scale = sprite.Atlas.PixelScale;
			FrameUVSize = (Vector2)FrameSize * scale;
			FrameUVSize -= sprite.Atlas.TexelShift * 2;
			Vector2 baseOffset = sprite.MinUV;
			for (int i = 0; i < FrameOffsets.Length; i++)
				FrameUVOffsets[i] = baseOffset + ((Vector2)FrameOffsets[i] * scale) + sprite.Atlas.TexelShift;
		}

	}

}

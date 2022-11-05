using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.Core.Graphics {

	/// <summary>
	/// An image atlas compiles a set of smaller images into one larger image.
	/// </summary>
	public class ImageAtlas {

		// The list of pixel offsets of subimages
		private Vector2i[] offsets = Array.Empty<Vector2i>();
		// The list of entries in the atlas
		private readonly List<(IImage Image, Recti Area)> entries = new();

		/// <summary>
		/// Adds an image to the atlas, returning the ID associated with it.
		/// </summary>
		/// <param name="image">The image to add to the atlas</param>
		/// <param name="srcArea">The area within the image to add, or null to add the entire image</param>
		/// <returns>The subimage's ID</returns>
		public int AddImage(IImage image, Recti? srcArea = null) {
			int id = entries.Count;
			entries.Add((image, srcArea ?? new Recti(image.Size)));
			return id;
		}

		/// <summary>
		/// Builds the atlas image and computes the offsets of all entries within the atlas.
		/// </summary>
		/// <returns>The built atlas image</returns>
		public IProcessableImage Build() {
			Vector2i size = default;
			offsets = new Vector2i[entries.Count];

			// Sort sprites in descending size (largest first)
			entries.Sort((a, b) => {
				int sizeA = a.Area.Size.X * a.Area.Size.Y;
				int sizeB = b.Area.Size.X * b.Area.Size.Y;
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

			for(int i = 0; i < entries.Count; i++) {
				Vector2i spriteSize = entries[i].Area.Size;
				ref Vector2i spritePosition = ref offsets[i];

				// The first sprite always gets assigned at 0,0
				if (size.X == 0) {
					size = spriteSize;
					spritePosition = default;
					continue;
				}

				// Try to find a free area that the sprite can be assigned to first
				bool hasAssigned = false;
				for (int j = 0; j < freeAreas.Count; j++) {
					Recti area = freeAreas[j];
					// If area is large enough to hold sprite
					if (area.Size.X >= spriteSize.X && area.Size.Y >= spriteSize.Y) {
						// Remove area and set sprite position to the origin of the area
						freeAreas.RemoveAt(j);
						spritePosition = area.Position;
						// Compute remaining space outside the sprite
						Vector2i space = area.Size - spriteSize;
						if (space.X > 0 || space.Y > 0) {
							// If free space is larger in width than height
							if (space.X > space.Y) {
								// Create free area to the right of the sprite
								freeAreas.Add(new Recti(area.Position.X + spriteSize.X, area.Position.Y, spriteSize.X - area.Size.X, area.Size.Y));
								// If sprite does not fill the whole height, add another free area below it
								if (space.Y > 0)
									freeAreas.Add(new Recti(area.Position.X, area.Position.Y + spriteSize.Y, spriteSize.X, area.Size.Y - spriteSize.Y));
							} else {
								// Create free area below the sprite
								freeAreas.Add(new Recti(area.Position.X, area.Position.Y + spriteSize.Y, area.Size.X, area.Size.Y - spriteSize.Y));
								// If sprite does not fill the whole width, add another free area to its right
								if (space.X > 0)
									freeAreas.Add(new Recti(area.Position.X + spriteSize.X, area.Position.Y, spriteSize.X - area.Size.X, area.Size.Y));
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
					spritePosition = new Vector2i(size.X, 0);
					size.X += spriteSize.X;
					size.Y = Math.Max(size.Y, spriteSize.Y);
					// Add any free area created by expansion
					if (spriteSize.Y < size.Y) {
						freeAreas.Add(new Recti(size.X, spriteSize.Y, spriteSize.X, size.Y - spriteSize.Y));
						SortFreeAreas();
					} else if (spriteSize.Y > size.Y) {
						freeAreas.Add(new Recti(0, size.Y, size.X, spriteSize.Y - size.Y));
						SortFreeAreas();
					}
				} else {
					// Expand in the Y axis primarily
					spritePosition = new Vector2i(0, size.Y);
					size.X = Math.Max(size.X, spriteSize.X);
					size.Y += spriteSize.Y;
					// Add any free area created by expansion
					if (spriteSize.X < size.X) {
						freeAreas.Add(new Recti(spriteSize.X, size.Y, size.X - spriteSize.X, spriteSize.Y));
						SortFreeAreas();
					} else if (spriteSize.X > size.X) {
						freeAreas.Add(new Recti(size.X, 0, spriteSize.X - size.X, size.Y));
						SortFreeAreas();
					}
				}
			}

			// Create the atlas and add all entries to it
			var atlasImage = ImageSharpImage<Rgba32>.Create(size.X, size.Y);
			for(int i = 0; i < entries.Count; i++) {
				var (image, area) = entries[i];
				var spriteSize = area.Size;
				var offset = offsets[i];
				atlasImage.Blit(new Recti(offset, spriteSize), image, area.Position);
			}

			return atlasImage;
		}

		/// <summary>
		/// Gets the pixel offset of an entry in the atlas.
		/// </summary>
		/// <param name="id">ID of the entry</param>
		/// <returns>Pixel offset of the entry</returns>
		public Vector2i GetOffset(int id) => offsets[id];

	}

}

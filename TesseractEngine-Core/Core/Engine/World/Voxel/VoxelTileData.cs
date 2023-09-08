using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Engine.World.Voxel {
	
	/// <summary>
	/// Structure containing tile data within a voxel world.
	/// </summary>
	public struct VoxelTileData {

		/// <summary>
		/// The numeric ID of the tile.
		/// </summary>
		public int TileID { get; set; }

		/// <summary>
		/// Game-specific metadata for this tile.
		/// </summary>
		public ushort TileMetadata { get; set; }

		/// <summary>
		/// Tile-specific data for this tile.
		/// </summary>
		public ushort TileUserdata { get; set; }

	}

}

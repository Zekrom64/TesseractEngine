using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Engine.World.Voxel {
	
	public interface IVoxelShard {

		public IEnumerator<int> GetWorldIDs();

		public IEnumerator<IVoxelWorld> GetWorlds();

		public bool TryGetWorld(int id, [NotNullWhen(true)] out IVoxelWorld? world);

		public IVoxelWorld this[int id] { get; }

	}

}

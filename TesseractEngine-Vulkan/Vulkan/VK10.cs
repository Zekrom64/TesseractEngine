using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Vulkan {
	public class VK10 {

		public static int MakeVersion(int major, int minor, int patch) => (major << 22) | (minor << 12) | patch;
		public static int VersionMajor(int version) => version >> 22;
		public static int VersionMinor(int version) => (version >> 12) & 0x3FF;
		public static int VersionPatch(int version) => version & 0xFFF;

		public static readonly int ApiVersion = MakeVersion(1, 0, 0);

		public const float LodClampNone = 1000.0f;
		public const uint RemainingMipLevels = ~0U;
		public const uint RemainingArrayLayers = ~0U;
		public const ulong WholeSize = ~0UL;
		public const uint AttachmentUnused = ~0U;
		public const uint QueueFamilyIgnored = ~0U;
		public const uint SubpassExternal = ~0U;
		public const int MaxPhysicalDeviceNameSize = 256;
		public const int UUIDSize = 16;
		public const int MaxMemoryTypes = 32;
		public const int MaxMemoryHeaps = 16;
		public const int MaxExtensionNameSize = 256;
		public const int MaxDescriptionSize = 256;

	}
}

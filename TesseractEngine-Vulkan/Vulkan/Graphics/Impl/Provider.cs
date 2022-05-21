using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Util;

namespace Tesseract.Vulkan.Graphics.Impl {

	public class VulkanGraphicsProvider : IGraphicsProvider {

		public static readonly Guid BaseID = new("3d7e57ba-ce53-44d1-aad1-cc2e1c30aa89");

		public IGraphicsProperites Properties { get; }

		public IGraphicsFeatures Features { get; }

		public IGraphicsLimits Limits { get; }

		public string Name { get; }

		public Guid UniqueID { get; }

		public bool MultiGraphics => true;

		public VulkanGraphicsProvider(VulkanGraphicsEnumerator enumerator, VKPhysicalDevice physicalDevice) {
			Properties = new VulkanGraphicsProperties();

			var properties = physicalDevice.Properties;
			Name = $"Vulkan [GPU: {properties.DeviceName}]";
			UniqueID = new GuidDigester(BaseID)
				.Digest(properties.PipelineCacheUUID)
				.CurrentGuid;
		}

		public IGraphics CreateGraphics(GraphicsCreateInfo createInfo) {
			throw new NotImplementedException();
		}

		public ISwapchain CreateSwapchain(IGraphics graphics, SwapchainCreateInfo createInfo) {
			throw new NotImplementedException();
		}

		public SwapchainSupportInfo GetSwapchainSupport(IGraphics graphics, IWindow window) {
			throw new NotImplementedException();
		}

	}

	[GraphicsEnumerator]
	public class VulkanGraphicsEnumerator : IGraphicsEnumerator {

		public static IGraphicsEnumerator GetEnumerator(GraphicsEnumeratorCreateInfo createInfo) {
			
		}

		public VK VK { get; }
		public VKInstance Instance { get; }

		public VulkanGraphicsEnumerator(IVKLoader loader) {
			using MemoryStack sp = MemoryStack.Push();
			VK = new(loader);

			VK.InstanceExtensionProperties

			ManagedPointer<VKApplicationInfo> appInfo = new(new VKApplicationInfo() {
				Type = VKStructureType.ApplicationInfo,
				APIVersion = VK.MaxInstanceVersion,
			});

			VKInstanceCreateInfo createInfo = new() {
				Type = VKStructureType.InstanceCreateInfo,
				ApplicationInfo = appInfo
			};
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		public IEnumerable<IGraphicsProvider> EnumerateProviders() {
			
		}

		public bool TryGetProvider(Guid uniqueID, out IGraphicsProvider provider) {
			throw new NotImplementedException();
		}

	}

}

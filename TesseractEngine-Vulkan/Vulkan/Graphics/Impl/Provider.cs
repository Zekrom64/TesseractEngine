using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace Tesseract.Vulkan.Graphics.Impl {

	public class VulkanGraphicsProvider : IGraphicsProvider {

		/// <summary>
		/// The base ID for all Vulkan graphics providers.
		/// </summary>
		public static readonly Guid BaseID = new("3d7e57ba-ce53-44d1-aad1-cc2e1c30aa89");

		/// <summary>
		/// The enumerator for this graphics provider.
		/// </summary>
		public readonly VulkanGraphicsEnumerator Enumerator;

		/// <summary>
		/// The physical device info for this graphics provider.
		/// </summary>
		public readonly VulkanPhysicalDeviceInfo PhysicalDeviceInfo;

		public IGraphicsProperites Properties { get; }

		public IGraphicsFeatures Features { get; }

		public IGraphicsLimits Limits { get; }

		public string Name { get; }

		public Guid UniqueID { get; }

		public bool MultiGraphics => true;

		public VulkanGraphicsProvider(VulkanGraphicsEnumerator enumerator, VKPhysicalDevice physicalDevice) {
			Enumerator = enumerator;
			PhysicalDeviceInfo = new VulkanPhysicalDeviceInfo(physicalDevice);
			Properties = new VulkanGraphicsProperties(PhysicalDeviceInfo);
			Features = new VulkanGraphicsFeatures(PhysicalDeviceInfo);
			Limits = new VulkanGraphicsLimits(PhysicalDeviceInfo);

			var properties = physicalDevice.Properties;
			Name = $"Vulkan [GPU: {properties.DeviceName}]";
			UniqueID = new GuidDigester(BaseID)
				.Digest(properties.PipelineCacheUUID)
				.CurrentGuid;
		}

		public IGraphics CreateGraphics(GraphicsCreateInfo createInfo) =>
			new VulkanGraphics(this, new VulkanDevice(PhysicalDeviceInfo, createInfo));

		public ISwapchain CreateSwapchain(IGraphics graphics, SwapchainCreateInfo createInfo) =>
			new VulkanSwapchain((VulkanGraphics)graphics, createInfo);

		public SwapchainSupportInfo? GetSwapchainSupport(IGraphics graphics, IWindow window) {
			if (window is IVKSurfaceProvider sp) {
				VKPhysicalDevice pd = PhysicalDeviceInfo.PhysicalDevice;
				VKSurfaceKHR surface = Enumerator.GetOrCreateSurface(sp);

				// Must be supported by at least one of the device queues.
				bool hasSupport = false;
				for (int queue = 0; queue < PhysicalDeviceInfo.QueueFamilyProperties.Length; queue++) {
					if (pd.GetSurfaceSupportKHR((uint)queue, surface)) {
						hasSupport = true;
						break;
					}
				}
				if (!hasSupport) return null;

				// Get surface properties
				var caps = pd.GetSurfaceCapabilitiesKHR(surface);
				var fmts = pd.GetSurfaceFormatsKHR(surface);
				var modes = pd.GetSurfacePresentModesKHR(surface);

				return new SwapchainSupportInfo() {
					ImageType = SwapchainImageType.Texture,
					SupportedImageUsage = VulkanConverter.Convert(caps.SupportedUsageFlags),
					SupportedPresentModes = modes.ConvertAll(VulkanConverter.Convert),
					SupportedFormats = (from fmt in fmts
									   where fmt.ColorSpace == VKColorSpaceKHR.SRGBNonlinear
									   select VulkanConverter.Convert(fmt.Format)).ToList()
				};
			} else return null;
		}

	}

	/// <summary>
	/// Extended graphics enumerator info for Vulkan-based backends.
	/// </summary>
	public record VulkanExtendedGraphicsEnumeratorInfo : IExtendedGraphicsEnumeratorInfo {

		/// <summary>
		/// The name of the application to pass to <see cref="VKApplicationInfo"/>.
		/// </summary>
		public string ApplicationName { get; init; } = "Tesseract";

		/// <summary>
		/// The version of the application to pass to <see cref="VKApplicationInfo"/>.
		/// </summary>
		public uint ApplicationVersion { get; init; } = VK10.MakeVersion(0, 1, 0);

		/// <summary>
		/// The name of the engine to pass to <see cref="VKApplicationInfo"/>.
		/// </summary>
		public string EngineName { get; init; } = "Tesseract";

		/// <summary>
		/// THe version of the engine to pass to <see cref="VKApplicationInfo"/>.
		/// </summary>
		public uint EngineVersion { get; init; } = VK10.MakeVersion(0, 1, 0);

		/// <summary>
		/// A loader for Vulkan functions to use when creating objects. If null, <see cref="VulkanPlatformLoader"/> is used.
		/// </summary>
		public IVKLoader? Loader { get; init; } = null;

		/// <summary>
		/// The amount of parallelism to use with command pools. Each command pool can only support recording on a
		/// single buffer at a time so this value determines the number of concurrent pools to use for command
		/// buffer allocation and recording. Values &lt;1 are interpreted to use the number of CPU cores as a default
		/// value (since having more buffers recording that hardware threads is already suboptimal).
		/// </summary>
		public int CommandPoolParallelism { get; init; } = -1;

		/// <summary>
		/// The number of orphaned command buffers above which to start disposing. Values &lt;1 are interpreted
		/// to use a default value.
		/// </summary>
		public int CommandBufferGCThreshold { get; init; } = -1;

		/// <summary>
		/// An enumerable list of required extension names to use when creating the Vulkan instance.
		/// </summary>
		public IEnumerable<string>? RequiredExtensions { get; init; } = null;

		/// <summary>
		/// An enumerable list of required layer names to use when creating the Vulkan instance.
		/// </summary>
		public IEnumerable<string>? RequiredLayers { get; init; } = null;

	}

	/// <summary>
	/// A graphics enumerator for the Vulkan API.
	/// </summary>
	[GraphicsEnumerator]
	public class VulkanGraphicsEnumerator : IGraphicsEnumerator {

		public static IGraphicsEnumerator GetEnumerator(GraphicsEnumeratorCreateInfo createInfo) {
			VulkanExtendedGraphicsEnumeratorInfo? exInfo = null;
			if (createInfo.ExtendedInfo != null)
				foreach (var info in createInfo.ExtendedInfo)
					if (info is VulkanExtendedGraphicsEnumeratorInfo ex) exInfo = ex;
			return new VulkanGraphicsEnumerator(createInfo, exInfo);
		}

		public VK VK { get; }
		public VKInstance Instance { get; }

		// The list of all graphics providers (GPUs)
		private readonly List<VulkanGraphicsProvider> providers = new();

		// A mapping of window surfaces to VkSurfaceKHR objects, this is managed by the enumerator as it holds the VkInstance
		private readonly Dictionary<IVKSurfaceProvider, VKSurfaceKHR> surfaces = new();

		public VulkanExtendedGraphicsEnumeratorInfo? ExtendedInfo { get; }

		public VulkanGraphicsEnumerator(GraphicsEnumeratorCreateInfo enumCreateInfo, VulkanExtendedGraphicsEnumeratorInfo? exInfo) {
			ExtendedInfo = exInfo;
			// The layer name for the standard validation layers
			const string KHRONOSValidation = "VK_LAYER_KHRONOS_validation";
			using MemoryStack sp = MemoryStack.Push();

			// Get the surface provider from the window associated with creation info, if possible
			IVKSurfaceProvider? windowSurface = null;
			if (enumCreateInfo.Window != null) {
				if (enumCreateInfo.Window is IVKSurfaceProvider surf) windowSurface = surf;
				else throw new ArgumentException("Provided window cannot create a Vulkan surface", nameof(enumCreateInfo));
			}

			// Get/create the loader and Vulkan API object
			IVKLoader loader = exInfo?.Loader ?? new VulkanPlatformLoader();
			VK = new(loader);

			{ // Gather instance layers/extensions and create instance
				HashSet<string> availExts = VK.InstanceExtensionProperties.ConvertAll(p => p.ExtensionName).ToHashSet();
				HashSet<string> availLayers = VK.InstanceLayerProperties.ConvertAll(p => p.LayerName).ToHashSet();
				List<string> exts = new(), layers = new();

				// If the window surface is provided add its required extensions
				if (windowSurface != null)
					exts.AddAll(windowSurface.RequiredInstanceExtensions);

				// 
				if (enumCreateInfo.EnableDebugExtensions) {
					if (availLayers.Contains(KHRONOSValidation)) layers.Add(KHRONOSValidation);
					if (availExts.Contains(EXTDebugUtils.ExtensionName)) exts.Add(EXTDebugUtils.ExtensionName);
				}

				VKApplicationInfo appInfo = new() {
					Type = VKStructureType.ApplicationInfo,
					APIVersion = VK.MaxInstanceVersion,
					ApplicationName = "Tesseract",
					ApplicationVersion = VK10.MakeVersion(0, 1, 0),
					EngineName = "Tesseract",
					EngineVersion = VK10.MakeVersion(0, 1, 0)
				};
				if (exInfo != null) {
					if (exInfo.ApplicationName != null) appInfo.ApplicationName = exInfo.ApplicationName;
					if (exInfo.ApplicationVersion != 0) appInfo.ApplicationVersion = exInfo.ApplicationVersion;
					if (exInfo.EngineName != null) appInfo.EngineName = exInfo.EngineName;
					if (exInfo.EngineVersion != 0) appInfo.EngineVersion = exInfo.EngineVersion;
				}
				ManagedPointer<VKApplicationInfo> pAppInfo = new(appInfo);

				VKInstanceCreateInfo createInfo = new() {
					Type = VKStructureType.InstanceCreateInfo,
					ApplicationInfo = pAppInfo,
					EnabledLayerCount = (uint)layers.Count,
					EnabledLayerNames = sp.UTF8Array(layers),
					EnabledExtensionCount = (uint)exts.Count,
					EnabledExtensionNames = sp.UTF8Array(exts)
				};

				Instance = VK.CreateInstance(createInfo);

				pAppInfo.Dispose();
			}

			// Create window surface as needed
			VKSurfaceKHR? surface = null;
			if (windowSurface != null) {
				surface = windowSurface.CreateSurface(Instance);
				surfaces[windowSurface] = surface;
			}

			{ // Add providers for each available physical device
				foreach (VKPhysicalDevice pd in Instance.PhysicalDevices) {
					// If created with a surface, we need 
					if (surface != null) {
						bool hasPresent = false;
						for (int queue = 0; queue < pd.QueueFamilyProperties.Length; queue++) {
							if (pd.GetSurfaceSupportKHR((uint)queue, surface)) {
								hasPresent = true;
								break;
							}
						}
						if (!hasPresent) continue;
					}

					providers.Add(new VulkanGraphicsProvider(this, pd));
				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Instance.Dispose();
			foreach (var surface in surfaces) surface.Value.Dispose();
		}

		public IEnumerable<IGraphicsProvider> EnumerateProviders() => providers;

		public bool TryGetProvider(Guid uniqueID, [NotNullWhen(true)] out IGraphicsProvider? provider) {
			foreach (IGraphicsProvider p in providers) {
				if (p.UniqueID == uniqueID) {
					provider = p;
					return true;
				}
			}
			provider = null;
			return false;
		}

		/// <summary>
		/// Gets or creates a Vulkan surface for the given surface provider.
		/// </summary>
		/// <param name="window">Surface provider to get surface for</param>
		/// <returns>Vulkan surface</returns>
		public VKSurfaceKHR GetOrCreateSurface(IVKSurfaceProvider window) {
			if (surfaces.TryGetValue(window, out VKSurfaceKHR? surface)) return surface;
			surface = window.CreateSurface(Instance);
			surfaces[window] = surface;
			return surface;
		}

		/// <summary>
		/// Deletes the given Vulkan surface.
		/// </summary>
		/// <param name="surface">Vulkan surface</param>
		public void DeleteSurface(VKSurfaceKHR surface) {
			foreach (var e in surfaces)
				if (e.Value == surface) surfaces.Remove(e.Key);
			surface.Dispose();
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;
using Tesseract.Core.Math;
using Tesseract.Core.Util;

namespace Tesseract.Vulkan.Graphics.Impl {

	public class VulkanPhysicalDeviceInfo {

		/*
		public const float UnsupportedDeviceScore = float.NegativeInfinity;
		*/

		public VKPhysicalDevice PhysicalDevice { get; }

		// Vulkan 1.0

		public IReadOnlySet<string> Extensions { get; }

		public IReadOnlySet<string> Layers { get; }

		public VKPhysicalDeviceFeatures Features { get; }

		public VKPhysicalDeviceProperties Properties { get; }

		public VKPhysicalDeviceLimits Limits => Properties.Limits;

		public VKPhysicalDeviceMemoryProperties MemoryProperties { get; }

		public VKQueueFamilyProperties[] QueueFamilyProperties { get; }

		// EXT_custom_border_color

		public VKPhysicalDeviceCustomBorderColorFeaturesEXT? CustomBorderColorFeaturesEXT { get; } = null;

		public VKPhysicalDeviceCustomBorderColorPropertiesEXT? CustomBorderColorPropertiesEXT { get; } = null;

		// EXT_line_rasterization

		public VKPhysicalDeviceLineRasterizationFeaturesEXT? LineRasterizationFeaturesEXT { get; } = null;

		public VKPhysicalDeviceLineRasterizationPropertiesEXT? LineRasterizationPropertiesEXT { get; } = null;

		// EXT_extended_dynamic_state

		public VKPhysicalDeviceExtendedDynamicStateFeaturesEXT? ExtendedDynamicStateFeaturesEXT { get; } = null;

		/*
		public float Score { get; } = UnsupportedDeviceScore;

		public bool IsSupported => Score != UnsupportedDeviceScore;
		*/

		public VulkanPhysicalDeviceInfo(VKPhysicalDevice physicalDevice) {
			PhysicalDevice = physicalDevice;

			HashSet<string> extensions = new(), layers = new();
			foreach (var ext in physicalDevice.DeviceExtensions) extensions.Add(ext.ExtensionName);
			Extensions = extensions;
			foreach (var lyr in physicalDevice.DeviceLayers) layers.Add(lyr.LayerName);
			Layers = layers;

			Features = physicalDevice.Features;
			Properties = physicalDevice.Properties;
			MemoryProperties = physicalDevice.MemoryProperties;
			QueueFamilyProperties = physicalDevice.QueueFamilyProperties;

			if (physicalDevice.Instance.APIVersion >= VK11.ApiVersion || physicalDevice.Instance.KHRGetPhysicalDeviceProperties2Functions != null) {
				using MemoryStack sp = MemoryStack.Push();
				int bp = sp.Pointer;

				bool extCustomBorderColor = Extensions.Contains(EXTCustomBorderColor.ExtensionName);
				bool extLineRasterization = Extensions.Contains(EXTLineRasterization.ExtensionName);
				bool extExtendedDynamicState = Extensions.Contains(EXTExtendedDynamicState.ExtensionName);

				{
					IntPtr next = IntPtr.Zero;
					
					UnmanagedPointer<VKPhysicalDeviceCustomBorderColorFeaturesEXT> pCustomBorderColorFeatures = default;
					if (extCustomBorderColor) {
						pCustomBorderColorFeatures = sp.Values(new VKPhysicalDeviceCustomBorderColorFeaturesEXT() {
							Type  = VKStructureType.PhysicalDeviceCustomBorderColorFeaturesEXT,
							Next = next
						});
						next = pCustomBorderColorFeatures;
					}

					UnmanagedPointer<VKPhysicalDeviceLineRasterizationFeaturesEXT> pLineRasterizationFeatures = default;
					if (extLineRasterization) {
						pLineRasterizationFeatures = sp.Values(new VKPhysicalDeviceLineRasterizationFeaturesEXT() {
							Type = VKStructureType.PhysicalDeviceLineRasterizationFeaturesEXT,
							Next = next
						});
						next = pLineRasterizationFeatures;
					}

					UnmanagedPointer<VKPhysicalDeviceExtendedDynamicStateFeaturesEXT> pExtendedDynamicStateFeatures = default;
					if (extExtendedDynamicState) {
						pExtendedDynamicStateFeatures = sp.Values(new VKPhysicalDeviceExtendedDynamicStateFeaturesEXT() {
							Type = VKStructureType.PhysicalDeviceExtendedDynamicStateFeaturesEXT,
							Next = next
						});
						next = pExtendedDynamicStateFeatures;
					}

					VKPhysicalDeviceFeatures2 features2 = new() { Type = VKStructureType.PhysicalDeviceFeatures2, Next = next };

					physicalDevice.GetFeatures2(ref features2);

					Features = features2.Features;

					if (pCustomBorderColorFeatures) CustomBorderColorFeaturesEXT = pCustomBorderColorFeatures.Value;
					if (pLineRasterizationFeatures) LineRasterizationFeaturesEXT = pLineRasterizationFeatures.Value;
					if (pExtendedDynamicStateFeatures) ExtendedDynamicStateFeaturesEXT = pExtendedDynamicStateFeatures.Value;
				}
				sp.Pointer = bp;
				{
					IntPtr next = IntPtr.Zero;

					UnmanagedPointer<VKPhysicalDeviceCustomBorderColorPropertiesEXT> pCustomBorderColorProperties = default;
					if (extCustomBorderColor) {
						pCustomBorderColorProperties = sp.Values(new VKPhysicalDeviceCustomBorderColorPropertiesEXT() {
							Type = VKStructureType.PhysicalDeviceCustomBorderColorPropertiesEXT,
							Next = next
						});
						next = pCustomBorderColorProperties;
					}

					UnmanagedPointer<VKPhysicalDeviceLineRasterizationPropertiesEXT> pLineRasterizationProperties = default;
					if (extLineRasterization) {
						pLineRasterizationProperties = sp.Values(new VKPhysicalDeviceLineRasterizationPropertiesEXT() {
							Type = VKStructureType.PhysicalDeviceLineRasterizationPropertiesEXT,
							Next = next
						});
						next = pLineRasterizationProperties;
					}

					VKPhysicalDeviceProperties2 properties2 = new() { Type = VKStructureType.PhysicalDeviceProperties2, Next = next };

					physicalDevice.GetProperties2(ref properties2);

					Properties = properties2.Properties;

					if (pCustomBorderColorProperties) CustomBorderColorPropertiesEXT = pCustomBorderColorProperties.Value;
					if (pLineRasterizationProperties) LineRasterizationPropertiesEXT = pLineRasterizationProperties.Value;
				}
				sp.Pointer = bp;
				{
					IntPtr next = IntPtr.Zero;
					VKPhysicalDeviceMemoryProperties2 properties2 = new() { Type = VKStructureType.PhysicalDeviceMemoryProperties2, Next = next };

					physicalDevice.GetMemoryProperties2(ref properties2);

					MemoryProperties = properties2.MemoryProperties;
				}
			}
			//Score = CalculateScore(context);
		}

		/*
		private static float GetDeviceTypeWeight(VKPhysicalDeviceType type) => type switch {
			VKPhysicalDeviceType.IntegratedGPU => 100.0f,
			VKPhysicalDeviceType.DiscreteGPU => 200.0f,
			VKPhysicalDeviceType.VirtualGPU => 50.0f,
			_ => 0.0f
		};

		private static float GetMemoryHeapWeight(VKMemoryHeap heap) {
			float weight = (heap.Size >> 27) * 10.0f; // Add 1 for each 128 MiB of memory
			if ((heap.Flags & VKMemoryHeapFlagBits.DeviceLocal) != 0) weight *= 1.5f; // Increase weight 50% if device local
			return weight;
		}

		private float CalculateScore(VulkanGraphicsContext context) {
			if (context.ScoreFunc != null) return context.ScoreFunc(this);
			float score = 0;

			if (context.RequiredDeviceExtensions != null)
				foreach (string reqext in context.RequiredDeviceExtensions) if (!Extensions.Contains(reqext)) return UnsupportedDeviceScore;

			if (context.RequiredCompatibleSurfaces != null) {
				foreach (var surface in context.RequiredCompatibleSurfaces) {
					bool support = false;
					for (uint queue = 0; queue < QueueFamilyProperties.Length; queue++) {
						if (PhysicalDevice.GetSurfaceSupportKHR(queue, surface)) {
							support = true;
							break;
						}
					}
					if (!support) return UnsupportedDeviceScore;
				}
			}

			if (context.PreferredDeviceExtensions != null)
				foreach (string prefext in context.PreferredDeviceExtensions) if (Extensions.Contains(prefext)) score += context.ExtensionWeight;

			score += GetDeviceTypeWeight(Properties.DeviceType);
			foreach (var heap in MemoryProperties.MemoryHeaps) score += GetMemoryHeapWeight(heap);

			return score;
		}
		*/

		public int FindQueue(VKQueueFlagBits bits, params int[] notPreferred) {
			int targetCount = BitOperations.PopCount((uint)bits);
			var primaryChoice = from family in LINQ.Seq(QueueFamilyProperties.Length) // For each queue family index
								// Ignore non-preferred queues
								where !notPreferred.Contains(family)
								// Number of shared bits the family has
								let familyBits = BitOperations.PopCount((uint)(bits & QueueFamilyProperties[family].QueueFlags))
								// Must have at least the target count, otherwise non compatible
								where familyBits >= targetCount
								// Sort by the number of bits ascending (prefer more specialized queues)
								orderby familyBits ascending
								// Select the family
								select family;
			if (primaryChoice.TryGetFirst(out int primaryFamily)) return primaryFamily;
			var secondaryChoice = from family in LINQ.Seq(QueueFamilyProperties.Length) // For each queue family index
								  // Number of shared bits the family has
								  let familyBits = BitOperations.PopCount((uint)(bits & QueueFamilyProperties[family].QueueFlags))
								  // Must have at least the target count, otherwise non compatible
								  where familyBits >= targetCount
								  // Sort by the number of bits ascending (prefer more specialized queues)
								  orderby familyBits ascending
								  // Select the family
								  select family;
			return secondaryChoice.FirstOrDefault(-1);
		}

	}

	public struct VulkanDeviceQueueInfo : IDisposable {

		public uint QueueFamily;

		public uint QueueIndex;

		public VKQueueFlagBits QueueFlags;

		public Vector3ui MinImageTransferGranularity;

		public VKQueue Queue;

		public ulong QueueID => (ulong)Queue.Queue;

		public SemaphoreSlim QueueSemaphore;

		public void Dispose() {
			QueueSemaphore?.Dispose();
			QueueSemaphore = null!;
		}

		public void InitQueue(VulkanDevice device) {
			if (Queue == null) Queue = device.Device.GetQueue(QueueFamily, QueueIndex);
		}

	}

	public class VulkanDevice : IDisposable {

		public VulkanPhysicalDeviceInfo PhysicalDevice { get; }


		public IReadOnlySet<string> EnabledExtensions { get; }
		
		public IReadOnlySet<string> EnabledLayers { get; }

		public VKSharingMode ResourceSharingMode { get; }

		public ManagedPointer<int> ResourceSharingIndices { get; }

		private VulkanDeviceQueueInfo queueGraphics;
		public VulkanDeviceQueueInfo QueueGraphics => queueGraphics;

		private VulkanDeviceQueueInfo queueTransfer;
		public VulkanDeviceQueueInfo QueueTransfer => queueTransfer;

		private VulkanDeviceQueueInfo queueCompute;
		public VulkanDeviceQueueInfo QueueCompute => queueCompute;


		public VKDevice Device { get; }

		/*
		private static VulkanPhysicalDeviceInfo SelectPhysicalDevice(VulkanGraphicsContext context) {
			// If a preferred device is given use that
			if (context.PreferredPhysicalDevice != null) {
				VulkanPhysicalDeviceInfo devinfo = new(context, context.PreferredPhysicalDevice);
				if (devinfo.IsSupported) return devinfo;
			}
			var devices = context.Instance.PhysicalDevices;
			// If no Vulkan devices throw exception
			if (devices.Length == 0) throw new VulkanException("No Vulkan-capable devices found!");
			// If only one Vulkan device we have no choice
			else if (devices.Length == 1) {
				VulkanPhysicalDeviceInfo devinfo = new(context, devices[0]);
				if (devinfo.Score == VulkanPhysicalDeviceInfo.UnsupportedDeviceScore) throw new VulkanException("No Vulkan-capable device available which satisfies requirements!");
				return devinfo;
			// Else enumerate all devices, sort by score, and select the best
			}  else {
				VulkanPhysicalDeviceInfo[] infos = Array.ConvertAll(devices, device => new VulkanPhysicalDeviceInfo(context, device));
				VulkanPhysicalDeviceInfo? devinfo = (
					from info in infos
					where info.IsSupported
					orderby info.Score descending
					select info
				).FirstOrDefault();
				if (devinfo == null) throw new VulkanException("No Vulkan-capable device available which satisfies requirements!");
				return devinfo;
			}
		}
		*/

		public VulkanDevice(VulkanGraphicsProvider provider, VulkanPhysicalDeviceInfo physicalDevice, GraphicsCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			PhysicalDevice = physicalDevice;
			var hwfeatures = createInfo.EnabledFeatures;

			// Enable extensions
			HashSet<string> enabledExts = new();

			EnabledExtensions = enabledExts;

			EnabledLayers = new HashSet<string>();

			// Find queue families for each type
			int graphicsQueueFamily = PhysicalDevice.FindQueue(VKQueueFlagBits.Graphics);
			if (graphicsQueueFamily < 0) throw new VulkanException("Could not find graphics queue");
			int transferQueueFamily = PhysicalDevice.FindQueue(VKQueueFlagBits.Transfer, graphicsQueueFamily);
			if (transferQueueFamily < 0) throw new VulkanException("Could not find transfer queue");
			int computeQueueFamily = PhysicalDevice.FindQueue(VKQueueFlagBits.Compute, graphicsQueueFamily, transferQueueFamily);
			if (computeQueueFamily < 0) throw new VulkanException("Could not find compute queue");

			// Determine sharing mode from queue families
			if (graphicsQueueFamily == transferQueueFamily && transferQueueFamily == computeQueueFamily) {
				ResourceSharingMode = VKSharingMode.Exclusive;
			} else {
				ResourceSharingMode = VKSharingMode.Concurrent;
				HashSet<int> families = new() { graphicsQueueFamily, transferQueueFamily, computeQueueFamily };
				ResourceSharingIndices = new ManagedPointer<int>(families);
			}

			// Initialize queue create infos
			// Start with graphics
			List<VKDeviceQueueCreateInfo> queueInfos = new() {
				new VKDeviceQueueCreateInfo() {
					Type = VKStructureType.DeviceQueueCreateInfo,
					QueueCount = 1,
					QueueFamilyIndex = (uint)graphicsQueueFamily
				}
			};
			var graphicsQueueProps = PhysicalDevice.QueueFamilyProperties[graphicsQueueFamily];
			queueGraphics = new() {
				QueueFamily = (uint)graphicsQueueFamily,
				QueueIndex = 0,
				QueueFlags = graphicsQueueProps.QueueFlags,
				MinImageTransferGranularity = graphicsQueueProps.MinImageTransferGranularity,
				QueueSemaphore = new(1)
			};

			// Then transfer
			VKQueueFamilyProperties transferQueueProps;
			if (transferQueueFamily == graphicsQueueFamily) {
				transferQueueProps = graphicsQueueProps;
				queueTransfer = queueGraphics;
				if (graphicsQueueProps.QueueCount > 1) {
					// Shares family, exclusive logical queue
					queueTransfer.QueueIndex = 1;
					queueTransfer.QueueSemaphore = new(1);
					var info = queueInfos[0];
					info.QueueCount++;
					queueInfos[0] = info;
				}
				// Else, shares family and logical queue
			} else {
				transferQueueProps = PhysicalDevice.QueueFamilyProperties[transferQueueFamily];
				queueTransfer = new() {
					QueueFamily = (uint)transferQueueFamily,
					QueueIndex = 0,
					QueueFlags = transferQueueProps.QueueFlags,
					MinImageTransferGranularity = transferQueueProps.MinImageTransferGranularity,
					QueueSemaphore = new(1)
				};
				queueInfos.Add(new VKDeviceQueueCreateInfo() {
					Type = VKStructureType.DeviceQueueCreateInfo,
					QueueCount = 1,
					QueueFamilyIndex = (uint)transferQueueFamily
				});
			}

			// Then compute
			if (computeQueueFamily == graphicsQueueFamily) {
				queueCompute = queueGraphics;
				var info = queueInfos[0];
				if (graphicsQueueProps.QueueCount > info.QueueCount) {
					queueCompute.QueueIndex = info.QueueCount++;
					queueInfos[0] = info;
					queueCompute.QueueSemaphore = new(1);
				}
			} else if (computeQueueFamily == transferQueueFamily) {
				queueCompute = queueTransfer;
				var info = queueInfos[1];
				if (transferQueueProps.QueueCount > info.QueueCount) {
					queueCompute.QueueIndex = info.QueueCount++;
					queueInfos[1] = info;
					queueCompute.QueueSemaphore = new(1);
				}
			} else {
				var computeQueueProps = PhysicalDevice.QueueFamilyProperties[computeQueueFamily];
				queueTransfer = new() {
					QueueFamily = (uint)computeQueueFamily,
					QueueIndex = 0,
					QueueFlags = computeQueueProps.QueueFlags,
					MinImageTransferGranularity = computeQueueProps.MinImageTransferGranularity,
					QueueSemaphore = new(1)
				};
				queueInfos.Add(new VKDeviceQueueCreateInfo() {
					Type = VKStructureType.DeviceQueueCreateInfo,
					QueueCount = 1,
					QueueFamilyIndex = (uint)computeQueueFamily
				});
			}

			// Initialize queue priorities
			for(int i = 0; i < queueInfos.Count; i++) {
				var info = queueInfos[i];
				info.QueuePriorities = sp.Values(LINQ.Dup(1.0f, (int)info.QueueCount));
				queueInfos[i] = info;
			}
 
			// Create device
			Device = PhysicalDevice.PhysicalDevice.CreateDevice(new VKDeviceCreateInfo() {
				Type = VKStructureType.DeviceCreateInfo,
				EnabledExtensionCount = (uint)enabledExts.Count,
				EnabledExtensionNames = sp.UTF8Array(enabledExts),
				QueueCreateInfoCount = (uint)queueInfos.Count,
				QueueCreateInfos = sp.Values(queueInfos)
			});

			// Initialize queues
			queueGraphics.InitQueue(this);
			queueTransfer.InitQueue(this);
			queueCompute.InitQueue(this);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			QueueGraphics.Dispose();
			QueueCompute.Dispose();
			QueueTransfer.Dispose();
			Device.Dispose();
			ResourceSharingIndices.Dispose();
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;
using Tesseract.Core.Utilities;

namespace Tesseract.Vulkan.Services.Objects {

	/// <summary>
	/// Vulkan swapchain implementation.
	/// </summary>
	public class VulkanSwapchain : ISwapchain {

		/// <summary>
		/// The graphics context this swapchain was created from.
		/// </summary>
		public VulkanGraphics Graphics { get; }

		/// <summary>
		/// The provider for the swapchain's surface.
		/// </summary>
		public IVKSurfaceProvider SurfaceProvider { get; }

		/// <summary>
		/// The underlying Vulkan surface this swapchain uses.
		/// </summary>
		public VKSurfaceKHR Surface { get; }

		/// <summary>
		/// The underlying Vulkan swapchain. This may change between rebuilds.
		/// </summary>
		public VKSwapchainKHR? Swapchain { get; private set; }

		public Vector2i Size { get; private set; }

		public PixelFormat Format { get; private set; }

		public SwapchainImageType ImageType => SwapchainImageType.Texture;

		public ISwapchainImage[] Images { get; private set; }

		public event Action? OnRebuild;


		// The base swapchain creation information
		private readonly SwapchainCreateInfo baseInfo;
		// The index of the currently acquired image
		private uint currentImageIndex;
		// If the last acquired image was successfully acquired, but suboptimal
		private bool suboptimal;

		// The command bank and corresponding queue to present using
		private readonly VulkanCommands.CommandBank commandBank;

		// Properties will be non-null after swapchain recreation, but the constructor doesn't know this so suppress nullable warnings
#nullable disable
		public VulkanSwapchain(VulkanGraphics graphics, SwapchainCreateInfo info) {
			Graphics = graphics;
			baseInfo = info;

			IVKSurfaceProvider sp = info.PresentWindow.GetService(VKServices.SurfaceProvider);
			SurfaceProvider = sp ?? throw new VulkanException("Cannot create Vulkan swapchain for window which is not a surface provider");
			Surface = graphics.Provider.Enumerator.GetOrCreateSurface(SurfaceProvider);

			VulkanCommands.CommandBank cmdbank = null;
			void CheckCommandBank(VulkanCommands.CommandBank bank) {
				if (cmdbank != null) return;
				if (graphics.Device.PhysicalDevice.PhysicalDevice.GetSurfaceSupportKHR(bank.QueueInfo.QueueFamily, Surface))
					cmdbank = bank;
			}
			CheckCommandBank(graphics.Commands.CommandBankTransfer);
			CheckCommandBank(graphics.Commands.CommandBankGraphics);
			CheckCommandBank(graphics.Commands.CommandBankCompute);
			commandBank = cmdbank ?? throw new VulkanException("No suitable queue exists for presentation in associated graphics context");

			RecreateSwapchain();
		}
#nullable restore

		private void RecreateSwapchain() {
			VulkanDevice vkdevice = Graphics.Device;
			VKDevice device = vkdevice.Device;
			VKPhysicalDevice pd = vkdevice.PhysicalDevice.PhysicalDevice;

			// If zero extent, avoid recreating
			Vector2ui size = (Vector2ui)SurfaceProvider.SurfaceExtent;

			var caps = pd.GetSurfaceCapabilitiesKHR(Surface);
			var fmts = pd.GetSurfaceFormatsKHR(Surface);

			// Select image size
			size = size.Max(caps.MinImageExtent).Min(caps.MinImageExtent);
			if (size.LengthSquared == 0) return;
			Size = (Vector2i)size;

			// Select present mode
			VKPresentModeKHR presentMode = VulkanConverter.Convert(baseInfo.PresentMode);

			// Select image count
			uint requestedImageCount = 2;
			if (presentMode == VKPresentModeKHR.Mailbox) requestedImageCount = 3;

			// Select format
			VKFormat preferredFormat = baseInfo.PreferredPixelFormat != null ? VulkanConverter.Convert(baseInfo.PreferredPixelFormat) : VKFormat.Undefined;

			int ScoreFormat(VKSurfaceFormatKHR fmt) {
				int score = 0;
				if (fmt.ColorSpace == VKColorSpaceKHR.SRGBNonlinear) score++;
				if (preferredFormat != VKFormat.Undefined && fmt.Format == preferredFormat) score += 10;
				else {
					switch(fmt.Format) {
						case VKFormat.R8G8B8A8UNorm:
						case VKFormat.B8G8R8A8UNorm:
							score += 5;
							break;
					}
				}
				return score;
			}

			VKSurfaceFormatKHR surfFmt = (from fmt in fmts
										 let score = ScoreFormat(fmt)
										 orderby score descending
										 select fmt).First();

			Format = VulkanConverter.Convert(surfFmt.Format)!;

			// Create swapchain
			VKSwapchainCreateInfoKHR createInfo = new() {
				Type = VKStructureType.SwapchainCreateInfoKHR,
				Surface = Surface.SurfaceKHR,
				MinImageCount = Math.Min(Math.Max(requestedImageCount, caps.MinImageCount), caps.MaxImageCount),
				ImageFormat = surfFmt.Format,
				ImageColorSpace = surfFmt.ColorSpace	,
				ImageExtent = size,
				ImageArrayLayers = 1,
				ImageUsage = VulkanConverter.Convert(baseInfo.ImageUsage),
				ImageSharingMode = vkdevice.ResourceSharingMode,
				QueueFamilyIndexCount = (uint)vkdevice.ResourceSharingIndices.ArraySize,
				QueueFamilyIndices = vkdevice.ResourceSharingIndices,
				PreTransform = caps.CurrentTransform,
				CompositeAlpha = VKCompositeAlphaFlagBitsKHR.Opaque,
				PresentMode = presentMode,
				Clipped = true,
				OldSwapchain = Swapchain?.SwapchainKHR ?? 0
			};

			VKSwapchainKHR newSwapchain = device.CreateSwapchainKHR(createInfo);
			Swapchain?.Dispose();
			Swapchain = newSwapchain;

			Images = Swapchain.Images.ConvertAll(img => new VulkanTexture(Graphics, img, false) { Format = Format });

			OnRebuild?.Invoke();
		}

		public int BeginFrame(ISync? signal) {
			// Convert the sync object if required
			VKSemaphore? semaphore = null;
			VKFence? fence = null;
			if (signal != null) {
				if (signal is VulkanSemaphoreSync vksem) semaphore = vksem.Semaphore;
				else if (signal is VulkanFenceSync vkfence) fence = vkfence.Fence;
				else throw new VulkanException("Unsupported sync object passed to BeginFrame");
			}

			VKResult ret;
			do {
				// Try to acquire the next image
				ret = Swapchain!.AcquireNextImage(ulong.MaxValue, semaphore, fence, out currentImageIndex);
				switch(ret) {
					// On success, break
					case VKResult.Success:
						break;
					// If out of date, recreate the swapchain and retry
					case VKResult.ErrorOutOfDateKHR:
						RecreateSwapchain();
						break;
					// If suboptimal, consider a success but flag as suboptimal
					case VKResult.SuboptimalKHR:
						suboptimal = true;
						ret = VKResult.Success;
						break;
					// Else it is some kind of error to check
					default:
						VK.CheckError(ret);
						break;
				}
			// Loop until success or error
			} while (ret != VKResult.Success);

			return (int)currentImageIndex;
		}

		public void EndFrame(ISync? signalFence, params ISync[] wait) {
			using MemoryStack sp = MemoryStack.Push();
			
			// Convert semaphores and fences
			Span<ulong> semaphores = stackalloc ulong[wait.Length];
			for(int i = 0; i < wait.Length; i++) {
				var waitobj = wait[i];
				if (waitobj is VulkanSemaphoreSync vksem) semaphores[i] = vksem.Semaphore.Semaphore;
				else throw new VulkanException("Cannot use non-semaphore sync object for EndFrame wait");
			}

			ulong fence = 0;
			if (signalFence != null) {
				if (signalFence is VulkanFenceSync vkfence) fence = vkfence.Fence.Fence;
				else throw new VulkanException("Cannot use non-fence sync object for EndFrame signaling");
			}

			UnmanagedPointer<VKResult> result = sp.Alloc<VKResult>();
			
			// Create present info
			VKPresentInfoKHR presentInfo = new() {
				Type = VKStructureType.PresentInfoKHR,
				WaitSemaphoreCount = (uint)semaphores.Length,
				WaitSemaphores = sp.Values<ulong>(semaphores),
				SwapchainCount = 1,
				Swapchains = sp.Values(Swapchain!.SwapchainKHR),
				ImageIndices = sp.Values(currentImageIndex),
				Results = result
			};

			// Present using the appropriate command bank
			VKResult err = commandBank.Present(presentInfo);
			if (err == VKResult.Success) err = result.Value;

			// If not success
			if (err != VKResult.Success) {
				// If the swapchain needs rebuilding
				if (err == VKResult.SuboptimalKHR || err == VKResult.ErrorOutOfDateKHR || suboptimal) {
					// Wait until all commands are done, then recreate
					commandBank.WaitIdle();
					RecreateSwapchain();
				// Else check error
				} else VK.CheckError(err);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Swapchain?.Dispose();
			Graphics.Provider.Enumerator.DeleteSurface(Surface);
		}

	}

}

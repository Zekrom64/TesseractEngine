using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Vulkan.Native;

namespace Tesseract.Vulkan.Graphics {

	public class VulkanFenceSync : ISync {

		public VKFence Fence { get; }

		public VulkanFenceSync(VKFence fence) {
			Fence = fence;
		}

		public SyncGranularity Granularity => SyncGranularity.CommandBuffer;

		public SyncDirection Direction => SyncDirection.GPUToHost;

		public SyncFeatures Features => SyncFeatures.GPUWorkSignaling | SyncFeatures.HostPolling | SyncFeatures.HostWaiting;

		public void Dispose() {
			GC.SuppressFinalize(this);
			Fence.Dispose();
		}

		public bool HostPoll() => Fence.Status;

		public void HostReset() => throw new NotSupportedException("Cannot modify a Vulkan fence from the host");

		public void HostSet() => throw new NotSupportedException("Cannot modify a Vulkan fence from the host");

		public bool HostWait(ulong timeout) => !Fence.WaitFor(timeout);

	}

	public class VulkanEventSync : ISync {

		public SyncGranularity Granularity => SyncGranularity.PipelineStage;

		public SyncDirection Direction => SyncDirection.Any;

		public SyncFeatures Features => SyncFeatures.GPUSignaling | SyncFeatures.GPUWaiting | SyncFeatures.HostPolling | SyncFeatures.HostSignaling | SyncFeatures.HostWaiting;

		public void Dispose() {
			GC.SuppressFinalize(this);
			throw new NotImplementedException();
		}

		public bool HostPoll() {
			throw new NotImplementedException();
		}

		public void HostReset() {
			throw new NotImplementedException();
		}

		public void HostSet() {
			throw new NotImplementedException();
		}

		public bool HostWait(ulong timeout) {
			throw new NotImplementedException();
		}
	}

	public class VulkanSemaphoreSync : ISync {
		public SyncGranularity Granularity => throw new NotImplementedException();

		public SyncDirection Direction => throw new NotImplementedException();

		public SyncFeatures Features => throw new NotImplementedException();

		public void Dispose() {
			GC.SuppressFinalize(this);
			throw new NotImplementedException();
		}

		public bool HostPoll() => throw new NotSupportedException("Cannot poll a Vulkan semaphore from the host");

		public void HostReset() => throw new NotSupportedException("Cannot modify a Vulkan semaphore from the host");

		public void HostSet() => throw new NotSupportedException("Cannot modify a Vulkan semaphore from the host");

		public bool HostWait(ulong timeout) => throw new NotSupportedException("Cannot wait on a Vulkan semaphore from the host");
	}

}

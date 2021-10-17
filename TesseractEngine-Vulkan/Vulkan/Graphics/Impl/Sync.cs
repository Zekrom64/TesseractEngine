using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Vulkan.Native;

namespace Tesseract.Vulkan.Graphics.Impl {

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

		public void HostReset() => Fence.Reset();

		public void HostSet() => throw new NotSupportedException("Cannot signal a Vulkan fence from the host");

		public bool HostWait(ulong timeout) => !Fence.WaitFor(timeout);

	}

	public class VulkanEventSync : ISync {

		public VKEvent Event { get; }

		public VulkanEventSync(VKEvent evt) {
			Event = evt;
		}

		public SyncGranularity Granularity => SyncGranularity.PipelineStage;

		public SyncDirection Direction => SyncDirection.Any;

		public SyncFeatures Features => SyncFeatures.GPUSignaling | SyncFeatures.GPUWaiting | SyncFeatures.HostPolling | SyncFeatures.HostSignaling | SyncFeatures.HostWaiting;

		public void Dispose() {
			GC.SuppressFinalize(this);
			Event.Dispose();
		}

		public bool HostPoll() => Event.Status;

		public void HostReset() => Event.Status = false;

		public void HostSet() => Event.Status = true;

		public bool HostWait(ulong timeout) {
			var spin = new SpinWait();
			Stopwatch sw = new();
			sw.Start();
			while(!Event.Status) {
				if ((ulong)sw.ElapsedMilliseconds > timeout) return true;
				spin.SpinOnce();
			}
			return false;
		}
	}

	public class VulkanSemaphoreSync : ISync {

		public VKSemaphore Semaphore { get; }

		public VulkanSemaphoreSync(VKSemaphore semaphore) {
			Semaphore = semaphore;
		}

		public SyncGranularity Granularity => SyncGranularity.CommandBuffer;

		public SyncDirection Direction => SyncDirection.GPUToGPU;

		public SyncFeatures Features => SyncFeatures.GPUWorkSignaling | SyncFeatures.GPUWorkWaiting | SyncFeatures.GPUMultiQueue;

		public void Dispose() {
			GC.SuppressFinalize(this);
			Semaphore.Dispose();
		}

		public bool HostPoll() => throw new NotSupportedException("Cannot poll a Vulkan semaphore from the host");

		public void HostReset() => throw new NotSupportedException("Cannot modify a Vulkan semaphore from the host");

		public void HostSet() => throw new NotSupportedException("Cannot modify a Vulkan semaphore from the host");

		public bool HostWait(ulong timeout) => throw new NotSupportedException("Cannot wait on a Vulkan semaphore from the host");
	}

}

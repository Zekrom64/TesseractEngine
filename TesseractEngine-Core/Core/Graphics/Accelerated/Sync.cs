using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Graphics.Accelerated {
	
	/// <summary>
	/// Synchronization granularity determines how the level of specificity of synchronization operations.
	/// </summary>
	public enum SyncGranularity {
		/// <summary>
		/// The synchronization operation happens before command buffer submission or after completion of all commands in the buffer.
		/// </summary>
		CommandBuffer,
		/// <summary>
		/// The synchronization operation happens at the boundary between individual commands.
		/// </summary>
		Command,
		/// <summary>
		/// The synchronization operation happens when a command executes at a specific stage in the pipeline.
		/// </summary>
		PipelineStage
	}

	/// <summary>
	/// Synchronization direction determines the flow of synchronization events from signaling to waiting or polling.
	/// </summary>
	public enum SyncDirection {
		/// <summary>
		/// The GPU sources events and the Host sinks events.
		/// </summary>
		GPUToHost,
		/// <summary>
		/// The GPU sources and sinks events.
		/// </summary>
		GPUToGPU,
		/// <summary>
		/// Either the GPU or Host may source or sink events.
		/// </summary>
		Any
	}

	/// <summary>
	/// Synchronization features define what specific operations a sync object supports.
	/// </summary>
	public enum SyncFeatures {
		/// <summary>
		/// The state of the sync object can be polled by the Host.
		/// </summary>
		HostPolling =      0x0001,
		/// <summary>
		/// The sync object may be waited on by the Host.
		/// </summary>
		HostWaiting =      0x0002,
		/// <summary>
		/// The sync object may be waited on by the GPU.
		/// </summary>
		GPUWaiting =       0x0004,
		/// <summary>
		/// The state of the sync object can be modified by the Host.
		/// </summary>
		HostSignaling =    0x0008,
		/// <summary>
		/// The state of the sync object can be modified by the GPU.
		/// </summary>
		GPUSignaling =     0x0010,
		/// <summary>
		/// The sync object can be signaled by work completion on the GPU.
		/// </summary>
		GPUWorkSignaling = 0x0020,
		/// <summary>
		/// The sync object can be waited on before work begins on the GPU.
		/// </summary>
		GPUWorkWaiting =   0x0040,
		/// <summary>
		/// The sync object may be signaled across GPU queues.
		/// </summary>
		GPUMultiQueue =    0x0080
	}

	/// <summary>
	/// A synchronization object manages synchronization between Host and GPU in accelerated
	/// graphics APIs. Some APIs such as OpenGL require no explicit synchronization while it
	/// is mandatory for other APIs like Vulkan.
	/// </summary>
	public interface ISync : IDisposable {

		/// <summary>
		/// The granularity of synchronization of this sync object.
		/// </summary>
		public SyncGranularity Granularity { get; }
		
		/// <summary>
		/// The direction of synchronization of this sync object.
		/// </summary>
		public SyncDirection Direction { get; }

		/// <summary>
		/// A bitmask of features provided by this synchronization object.
		/// </summary>
		public SyncFeatures Features { get; }

		/// <summary>
		/// Attempts to wait on the sync object on the host side until the given timeout.
		/// </summary>
		/// <param name="timeout">Timeout for waiting in milliseconds</param>
		/// <returns>If the sync object timed out during waiting</returns>
		public bool HostWait(ulong timeout);

		/// <summary>
		/// Sets the state of the sync object from the host side.
		/// </summary>
		public void HostSet();

		/// <summary>
		/// Resets the state of the sync object from the host side.
		/// </summary>
		public void HostReset();

		/// <summary>
		/// Polls the state of the sync object from the host side.
		/// </summary>
		/// <returns></returns>
		public bool HostPoll();

		public class SyncAwaiter : INotifyCompletion {

			private readonly ISync sync;
			private readonly ulong timeout;

			public bool IsComplete => sync.HostPoll();
			
			public void OnCompleted(Action continuation) {
				Task.Run(continuation);
			}

			public bool GetResult() => sync.HostWait(timeout);

			public SyncAwaiter(ISync sync, ulong timeout) {
				this.sync = sync;
				this.timeout = timeout;
			}

		}

		public SyncAwaiter GetAwaiter(ulong timeout = ulong.MaxValue) => new(this, timeout);

	}

	/// <summary>
	/// Sync object creation information structure.
	/// </summary>
	public record SyncCreateInfo {

		/// <summary>
		/// Required synchonrization granularity for the sync object.
		/// </summary>
		public SyncGranularity Granularity { get; init; }

		/// <summary>
		///  Required synchronization direction for the sync object.
		/// </summary>
		public SyncDirection Direction { get; init; }

		/// <summary>
		/// Bitmask of required features for the sync object.
		/// </summary>
		public SyncFeatures Features { get; init; }

	}

}

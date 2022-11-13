using System;
using System.Runtime.CompilerServices;
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
	[Flags]
	public enum SyncFeatures {
		/// <summary>
		/// The state of the sync object can be polled by the Host.
		/// </summary>
		HostPolling = 0x0001,
		/// <summary>
		/// The sync object may be waited on by the Host.
		/// </summary>
		HostWaiting = 0x0002,
		/// <summary>
		/// The sync object may be waited on by the GPU.
		/// </summary>
		GPUWaiting = 0x0004,
		/// <summary>
		/// The state of the sync object can be modified by the Host. All host-accessible
		/// objects may be reset, but signaling implies that the host can both set the object
		/// and that this change can signal something on the GPU.
		/// </summary>
		HostSignaling = 0x0008,
		/// <summary>
		/// The state of the sync object can be modified by the GPU.
		/// </summary>
		GPUSignaling = 0x0010,
		/// <summary>
		/// The sync object can be signaled by work completion on the GPU.
		/// </summary>
		GPUWorkSignaling = 0x0020,
		/// <summary>
		/// The sync object can be waited on before work begins on the GPU.
		/// </summary>
		GPUWorkWaiting = 0x0040,
		/// <summary>
		/// The sync object may be signaled across GPU queues.
		/// </summary>
		GPUMultiQueue = 0x0080
	}

	/// <summary>
	/// <para>
	/// A synchronization object manages synchronization between Host and GPU in accelerated
	/// graphics APIs. Some APIs such as OpenGL require no explicit synchronization while it
	/// is mandatory for other APIs like Vulkan. Sync objects have three main properties;
	/// granularity, direction, and features.
	/// </para>
	/// <para>
	/// The <i>granularity</i> of a sync object determines how detailed the synchronization
	/// operation performed is. At the finest level this can be the execution of a command
	/// in a particular pipeline stage. At a more coarse level this can be between the execution
	/// of 
	/// </para>
	/// <para>
	/// The <i>direction</i> of a sync object determines how synchronization events
	/// are sourced and sinked; from the GPU to the host, from the GPU to itself, or in
	/// from the GPU or host either direction.
	/// </para>
	/// <para>
	/// The <i>features</i> of a sync object determine what operations the object is capable
	/// of; host/GPU signaling and waiting, host polling, and GPU work waiting/signaling.
	/// Work waiting/signaling differs from regular waiting/signaling in that it is used to
	/// control the order of execution of command buffers submitted to the GPU.
	/// </para>
	/// <para>While backends will not support every combination of these properties, there
	/// are two major types of objects that are always supported:
	/// <list type="bullet">
	/// <item><b>Semaphores</b> - Only support <see cref="SyncFeatures.GPUWorkSignaling"/> and <see cref="SyncFeatures.GPUWorkWaiting"/>
	/// with a granularity of <see cref="SyncGranularity.CommandBuffer"/> and a direction <see cref="SyncDirection.GPUToGPU"/>,
	/// and are used to synchronize command execution on the GPU.</item>
	/// <item><b>Fences</b> - Only support <see cref="SyncFeatures.GPUWorkSignaling"/>, <see cref="SyncFeatures.HostWaiting"/> and
	/// <see cref="SyncFeatures.HostPolling"/> with a granularity of <see cref="SyncGranularity.CommandBuffer"/> and
	/// a direction <see cref="SyncDirection.GPUToHost"/>. and are used to signal completion of GPU work to the host.</item>
	/// </list>
	/// Most backends also support some variation of an <b>Event</b> object with features <see cref="SyncFeatures.GPUWaiting"/>,
	/// <see cref="SyncFeatures.GPUSignaling"/>, <see cref="SyncFeatures.HostPolling"/>, and <see cref="SyncFeatures.HostSignaling"/>,
	/// and direction <see cref="SyncDirection.Any"/>. Events allow more fine-grained control over when commands are executed on
	/// the GPU and are host-accessible, but the granularity of events may vary significantly between backends and they
	/// may be emulated on some such as OpenGL which has no such concept of events. Therefore they are not preferred as sync objects.
	/// </para>
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

		/// <summary>
		/// Awaiter object for a single instance of awaiting a sync object from the host side.
		/// </summary>
		public class SyncAwaiter : INotifyCompletion {

			private bool? status = null;
			private event Action? OnComplete;

			/// <summary>
			/// If the await operation is completed.
			/// </summary>
			public bool IsCompleted => status != null;

			/// <summary>
			/// Queues an action to be performed on completion of the await operation.
			/// </summary>
			/// <param name="continuation">Continuation to perform</param>
			public void OnCompleted(Action continuation) => OnComplete += continuation;

			/// <summary>
			/// Gets the result of the await operation. This is only valid if <see cref="IsCompleted"/> is set.
			/// </summary>
			/// <returns>The result of the awaited operation</returns>
			public bool GetResult() => status != null && status.Value;

			/// <summary>
			/// Begins a new await operation on a sync object with the given timeout.
			/// </summary>
			/// <param name="sync">Sync object to wait on</param>
			/// <param name="timeout">Timeout to wait for</param>
			public SyncAwaiter(ISync sync, ulong timeout) {
				// Spin off a new thread to do the actual host waiting and update this object on completion
				Task.Run(() => {
					status = sync.HostWait(timeout);
					OnComplete?.Invoke();
				});
			}

		}

		/// <summary>
		/// Intermediate ref struct that simply acts as a provider for an awaiter tied
		/// to a sync object and a timeout.
		/// </summary>
		public readonly ref struct SyncAwaitProvider {

			/// <summary>
			/// The referenced sync object.
			/// </summary>
			public required ISync Sync { get; init; }

			/// <summary>
			/// The referenced timeout.
			/// </summary>
			public ulong Timeout { get; init; }

			/// <summary>
			/// Gets the awaiter object for the given sync object and timeout.
			/// </summary>
			/// <returns>Sync awaiter</returns>
			public SyncAwaiter GetAwaiter() => new(Sync, Timeout);

		}

		/// <summary>
		/// Provides an awaitable object that will use <see cref="HostWait(ulong)"/> with the given timeout value. The
		/// return value indicates if the await timed out.
		/// </summary>
		/// <param name="timeout">The timeout of the await object</param>
		/// <returns>An awaitable object</returns>
		public SyncAwaitProvider AsAwait(ulong timeout = ulong.MaxValue) => new() { Sync = this, Timeout = timeout };

	}

	/// <summary>
	/// Sync object creation information structure.
	/// </summary>
	public record SyncCreateInfo {

		/// <summary>
		/// Required synchonrization granularity for the sync object.
		/// </summary>
		public required SyncGranularity Granularity { get; init; }

		/// <summary>
		///  Required synchronization direction for the sync object.
		/// </summary>
		public required SyncDirection Direction { get; init; }

		/// <summary>
		/// Bitmask of required features for the sync object.
		/// </summary>
		public required SyncFeatures Features { get; init; }

	}

}

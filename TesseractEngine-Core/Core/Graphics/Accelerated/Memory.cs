using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Util;

namespace Tesseract.Core.Graphics.Accelerated {
	
	/// <summary>
	/// Bitmask of flags to apply during memory mapping.
	/// </summary>
	public enum MemoryMapFlags {
		/// <summary>
		/// No memory mapping may be done. This may hint that memory using this flag is purely device local and not directly accessible by host.
		/// </summary>
		None = 0x00,
		/// <summary>
		/// The memory may be read from by the host. It is undefined behavior to attempt to write to read-only memory.
		/// </summary>
		Read = 0x01,
		/// <summary>
		/// The memory may be written to by the host. Mapping with write-only flags may hint to the backend that the previous contents of the
		/// mapped region can be discarded. It is undefined behavior to attempt to read from write-only memory.
		/// </summary>
		Write = 0x02,
		/// <summary>
		/// The memory may be read from or written to by the host.
		/// </summary>
		ReadWrite = Read | Write,
		/// <summary>
		/// The memory may be used in GPU operations while mapped (although additional synchronization may be required).
		/// </summary>
		Persistent = 0x04,
		/// <summary>
		/// The mapped memory supports coherent access (ie. modifications to the memory on the host or GPU will be automatically visible to the other).
		/// Note that if this is not present the changes to the memory must manually be flushed between the host and GPU.
		/// </summary>
		Coherent = 0x08
	}

	/// <summary>
	/// A memory range specifies a contiguous block of memory.
	/// </summary>
	public struct MemoryRange {

		/// <summary>
		/// The byte offset of the memory range.
		/// </summary>
		public ulong Offset { get; set; }

		/// <summary>
		/// The length of the memory range in bytes. Zero values are interpreted
		/// as mappings from the offset to the end of the memory being referenced.
		/// </summary>
		public ulong Length { get; set; }

	}

	/// <summary>
	/// Interface for explicit memory binding information.
	/// </summary>
	public interface IMemoryBinding { }

	/// <summary>
	/// Bitmask of buffer usage flags.
	/// </summary>
	public enum BufferUsage {
		/// <summary>
		/// The buffer may be used to to read data from.
		/// </summary>
		TransferSrc = 0x0001,
		/// <summary>
		/// The buffer may be used to write data to.
		/// </summary>
		TransferDst = 0x0002,
		/// <summary>
		/// The buffer may be used in a bind set as a 'Uniform Texel Buffer'.
		/// </summary>
		UniformTexelBuffer = 0x0004,
		/// <summary>
		/// The buffer may be used in a bind set as a 'Storage Texel Buffer'.
		/// </summary>
		StorageTexelBuffer = 0x0008,
		/// <summary>
		/// The buffer may be used in a bind set as a 'Uniform Buffer'.
		/// </summary>
		UniformBuffer = 0x0010,
		/// <summary>
		/// The buffer may be used in a bind set as a 'Storage Buffer'.
		/// </summary>
		StorageBuffer = 0x0020,
		/// <summary>
		/// The buffer may be used to provide indices for draw calls.
		/// </summary>
		IndexBuffer = 0x0040,
		/// <summary>
		/// The buffer may be used to provide vertices for draw calls.
		/// </summary>
		VertexBuffer = 0x0080,
		/// <summary>
		/// The buffer may be used to provide draw parameters for indirect draw calls.
		/// </summary>
		IndirectBuffer = 0x0100
	}

	/// <summary>
	/// Memory access type bitmask.
	/// </summary>
	public enum MemoryAccess {
		/// <summary>
		/// Memory read of indirect draw parameters.
		/// </summary>
		IndirectCommandRead = 0x00001,
		/// <summary>
		/// Memory read of vertex indices.
		/// </summary>
		IndexRead = 0x00002,
		/// <summary>
		/// Memory read of vertex attributes.
		/// </summary>
		VertexAttributeRead = 0x00004,
		/// <summary>
		/// Memory read of a uniform buffer.
		/// </summary>
		UniformRead = 0x00008,
		/// <summary>
		/// Memory read of an input attachment.
		/// </summary>
		InputAttachmentRead = 0x00010,
		/// <summary>
		/// Memory read from a shader (uniform/shader storage buffers, texel fetches/samples).
		/// </summary>
		ShaderRead = 0x00020,
		/// <summary>
		/// Memory write from a shader (shader storage buffers or images).
		/// </summary>
		ShaderWrite = 0x00040,
		/// <summary>
		/// Mmeory read of a color attachment.
		/// </summary>
		ColorAttachmentRead = 0x00080,
		/// <summary>
		/// Memory write to a color attachment.
		/// </summary>
		ColorAttachmentWrite = 0x00100,
		/// <summary>
		/// Memory read from a depth/stencil attachment.
		/// </summary>
		DepthStencilAttachmentRead = 0x00200,
		/// <summary>
		/// Memory write to a depth/stencil attachment.
		/// </summary>
		DepthStencilAttachmentWrite = 0x00400,
		/// <summary>
		/// Memory read by a transfer operation (copies/blits).
		/// </summary>
		TransferRead = 0x00800,
		/// <summary>
		/// Memory write by a transfer operation (copies/blits).
		/// </summary>
		TransferWrite = 0x01000,
		/// <summary>
		/// Memory read by the host.
		/// </summary>
		HostRead = 0x02000,
		/// <summary>
		/// Memory write by the host.
		/// </summary>
		HostWrite = 0x04000,
		/// <summary>
		/// Generic memory read, targeting all read operations.
		/// </summary>
		MemoryRead = 0x08000,
		/// <summary>
		/// Generic memory write, targeting all write operations.
		/// </summary>
		MemoryWrite = 0x10000
	}

	/// <summary>
	/// <para>
	/// A buffer is a contiguous region of memory accessible by the GPU. Buffers may be mapped to
	/// allow a host to access them or they may be only GPU accessible. Buffer contents are referenced
	/// by other components of the graphics system.
	/// </para>
	/// <para>
	/// During buffer creation the buffer is bound to underlying GPU memory, either explicitly provided
	/// through creation information or automatically determined by the backend. No guarentees are made
	/// about the underlying memory other than the constraints given by the binding information.
	/// </para>
	/// <para>
	/// Accessing memory from the host is done by mapping a region of the buffer memory into host memory
	/// using the <see cref="Map{T}(MemoryMapFlags, in MemoryRange)">Map</see> function. The flags provided
	/// to this function indicate how the mapped memory must behave. Note that the backend may provide memory
	/// that behaves slightly differently from the requested behavior but still fulfills the constraints provided
	/// during mapping, and this should be kept in mind to avoid backend-dependent behavior. Some examples are:
	/// <list type="bullet">
	/// <item>Mapped memory may be read-write even if read-only or write-only mapping is requested</item>
	/// <item>Mapped memory may be coherent depending on memory binding even if coherency is not requested
	/// during mapping. Flushes will be ignored as coherency is already enforced.</item>
	/// <item>Persistent mapping may be implicitly supported even if not requested.</item>
	/// </list>
	/// </para>
	/// <para>
	/// Regardless of how any particular implementation manages buffers, access on the GPU should be assumed to be unsynchronized;
	/// attempting to modify buffer memory while it is in use by executing GPU commands will result in undefined behavior.
	/// </para>
	/// </summary>
	public interface IBuffer : IDisposable {

		/// <summary>
		/// The size of the buffer in bytes.
		/// </summary>
		public ulong Size { get; }

		/// <summary>
		/// Bitmask of supported buffer usages.
		/// </summary>
		public BufferUsage Usage { get; }

		/// <summary>
		/// The memory binding information for the buffer (may be null).
		/// </summary>
		public IMemoryBinding MemoryBinding { get; }

		/// <summary>
		/// Bitmask of supported memory mapping flags.
		/// </summary>
		public MemoryMapFlags SupportedMappings { get; }

		/// <summary>
		/// Maps the memory of this buffer for access by the host.
		/// </summary>
		/// <typeparam name="T">Type to map memory as</typeparam>
		/// <param name="flags">Flags to map memory based on</param>
		/// <param name="range">Range to map</param>
		/// <returns>Pointer to mapped memory</returns>
		[ThreadSafety(ThreadSafetyLevel.SingleThread)]
		public IPointer<T> Map<T>(MemoryMapFlags flags, in MemoryRange range = default);

		/// <summary>
		/// Unmaps this buffer if mapped.
		/// </summary>
		[ThreadSafety(ThreadSafetyLevel.SingleThread)]
		public void Unmap();

		/// <summary>
		/// Flushes modifications made by the host to the GPU during non-coherent mapping.
		/// </summary>
		/// <param name="range">Range to flush</param>
		public void FlushHostToGPU(in MemoryRange range = default);

		/// <summary>
		/// Flushes modifications made by the GPU to the host during non-coherent mapping.
		/// </summary>
		/// <param name="range">Range to flush</param>
		public void FlushGPUToHost(in MemoryRange range = default);

	}

	/// <summary>
	/// Buffer creation information.
	/// </summary>
	public record BufferCreateInfo {

		/// <summary>
		/// The size of the buffer.
		/// </summary>
		public ulong Size { get; init; }

		/// <summary>
		/// The required usages of the buffer.
		/// </summary>
		public BufferUsage Usage { get; init; }

		/// <summary>
		/// Explicit memory binding information for the buffer, or <c>null</c> to let the backend
		/// decide how memory should be bound for the buffer.
		/// </summary>
		public IMemoryBinding MemoryBinding { get; init; }

		/// <summary>
		/// The required memory mapping flags to support.
		/// </summary>
		public MemoryMapFlags MapFlags { get; init; }

	}

	/// <summary>
	/// A buffer binding stores a buffer and memory range within it to bind to.
	/// </summary>
	public record BufferBinding {

		/// <summary>
		/// The buffer to bind.
		/// </summary>
		public IBuffer Buffer { get; init; }

		/// <summary>
		/// The memory range within the buffer to bind.
		/// </summary>
		public MemoryRange Range { get; init; }

	}

}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// Enumeration of types of bindings.
	/// </summary>
	public enum BindType {
		/// <summary>
		/// A uniform buffer.
		/// </summary>
		UniformBuffer,
		/// <summary>
		/// A shader storage buffer.
		/// </summary>
		StorageBuffer,

		/// <summary>
		/// A combined texture and sampler.
		/// </summary>
		CombinedTextureSampler,
		/// <summary>
		/// A storage texture.
		/// </summary>
		StorageTexture,
		/// <summary>
		/// An input attachment.
		/// </summary>
		InputAttachment
	}

	/// <summary>
	/// Texture binding information.
	/// </summary>
	public readonly struct TextureBinding {

		/// <summary>
		/// The sampler used with this texture binding.
		/// </summary>
		public ISampler? Sampler { get; init; } = null;

		/// <summary>
		/// The texture view used with this texture binding.
		/// </summary>
		public required ITextureView TextureView { get; init; }

		/// <summary>
		/// The expected layout of the texture that will be bound.
		/// </summary>
		public required TextureLayout TexureLayout { get; init; }

		public TextureBinding() { }

	}

	/// <summary>
	/// Information for a single binding inside a bind set layout.
	/// </summary>
	public struct BindSetLayoutBinding {

		/// <summary>
		/// The index of this binding.
		/// </summary>
		public uint Binding;

		/// <summary>
		/// The type of binding.
		/// </summary>
		public BindType Type;

		/* Field not used for now
		/// <summary>
		/// The number of array elements in the binding. A value of zero
		/// is interpreted as there being no array (or an array of size 1).
		/// </summary>
		public uint ArraySize;
		*/

		/// <summary>
		/// The shader stages this binding will be used by.
		/// </summary>
		public ShaderType Stages;

	}

	/// <summary>
	/// Bind set layout creation information.
	/// </summary>
	public record BindSetLayoutCreateInfo {

		/// <summary>
		/// The list of bindings in this bind set layout.
		/// </summary>
		public BindSetLayoutBinding[] Bindings { get; init; } = Array.Empty<BindSetLayoutBinding>();

	}

	/// <summary>
	/// A bind set layout describes the list of bindings used by a bind set.
	/// </summary>
	public interface IBindSetLayout : IDisposable {

		/// <summary>
		/// The list of bindings used in this bind set.
		/// </summary>
		public IReadOnlyList<BindSetLayoutBinding> Bindings { get; }

	}

	/// <summary>
	/// Bind pool creation information.
	/// </summary>
	public record BindPoolCreateInfo {

		/// <summary>
		/// The list of weights of binding types to prioritize in this pool. If
		/// a binding type is not described in this list it cannot be allocated
		/// from this pool.
		/// </summary>
		public (BindType Type, float Weight)[] BindTypeWeights { get; init; } = Array.Empty<(BindType, float)>();

		/// <summary>
		/// The target size of the number of entries in the bind pool.
		/// </summary>
		public int TargetPoolSize { get; init; } = 32;

	}

	/// <summary>
	/// A bind pool provides storage and allocation for bind sets.
	/// </summary>
	public interface IBindPool : IDisposable {

		/// <summary>
		/// Allocates a bind set from this pool using the given allocation information.
		/// </summary>
		/// <param name="allocateInfo">Bind set allocation information</param>
		/// <returns>Allocated bind set</returns>
		public IBindSet AllocSet(BindSetAllocateInfo allocateInfo);

	}

	/// <summary>
	/// Bind set allocation information.
	/// </summary>
	public record BindSetAllocateInfo {

		/// <summary>
		/// The list of layouts the allocated bind set will use.
		/// </summary>
		public IBindSetLayout[] Layouts { get; init; } = Array.Empty<IBindSetLayout>();

	}

	/// <summary>
	/// Bind set write information.
	/// </summary>
	public struct BindSetWrite {

		/// <summary>
		/// The binding to write to.
		/// </summary>
		public uint Binding;

		/* Fields may be reintroduced if necessary, but are not for current binding scheme
		/// <summary>
		/// The first array element to write at.
		/// </summary>
		public uint ArrayElement;

		/// <summary>
		/// The number of elements to write. If zero, only one element will be written.
		/// </summary>
		public uint Count;
		*/

		/// <summary>
		/// The type of binding to write.
		/// </summary>
		public BindType Type;

		/// <summary>
		/// The texture binding to write.
		/// </summary>
		public TextureBinding? TextureInfo;

		/// <summary>
		/// The buffer binding to write.
		/// </summary>
		public BufferBinding? BufferInfo;

	}

	/// <summary>
	/// A bind set holds a list of bindings to use in one or more shader stages in a pipeline.
	/// </summary>
	public interface IBindSet : IDisposable {

		/// <summary>
		/// Updates the bindings in this bind set.
		/// </summary>
		/// <param name="writes">Writes to apply to the bindings in this bind set</param>
		public void Update(IReadOnlyList<BindSetWrite> writes);

		/// <summary>
		/// Updates the bindings in this bind set.
		/// </summary>
		/// <param name="writes">Writes to apply to the bindings in this bind set</param>
		public void Update(params BindSetWrite[] writes) => Update((IReadOnlyList<BindSetWrite>)writes);

	}

}

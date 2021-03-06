using System;
using System.Collections.Generic;
using Tesseract.Core.Native;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// Enumeration of types of shader.
	/// </summary>
	[Flags]
	public enum ShaderType {
		/// <summary>
		/// Vertex shader.
		/// </summary>
		Vertex = 0x01,
		/// <summary>
		/// Tessellation control shader.
		/// </summary>
		TessellationControl = 0x02,
		/// <summary>
		/// Tessellation evaluation shader.
		/// </summary>
		TessellationEvaluation = 0x04,
		/// <summary>
		/// Geometry shader.
		/// </summary>
		Geometry = 0x08,
		/// <summary>
		/// Fragment shader (sometimes called pixel shader).
		/// </summary>
		Fragment = 0x10,
		/// <summary>
		/// Compute shader.
		/// </summary>
		Compute = 0x20
	}

	/// <summary>
	/// Enumeration of types of shader source types.
	/// </summary>
	public enum ShaderSourceType {
		/// <summary>
		/// SPIR-V binary. Recognized types (of <see cref="int"/>):
		/// <list type="bullet">
		/// <item><see cref="Array"/></item>
		/// <item><see cref="IReadOnlyList{T}"/></item>
		/// <item><see cref="ReadOnlyMemory{T}"/></item>
		/// <item><see cref="IConstPointer{T}"/> with explicit size</item>
		/// </list>
		/// </summary>
		SPIRV,
		/// <summary>
		/// GLSL source code. Recognized types:
		/// <list type="bullet">
		/// <item><see cref="string"/></item>
		/// <item>The object's <see cref="object.ToString()"/> method</item>
		/// <item><see cref="IConstPointer{T}"/> of <see cref="byte"/> (must have explicit size or be null-terminated)</item>
		/// </list>
		/// </summary>
		GLSL
	}

	/// <summary>
	/// A shader stores the code for a programmable stage in a graphics pipeline.
	/// </summary>
	public interface IShader : IDisposable {

		/// <summary>
		/// Attempts to find a binding by name that is used by this shader.
		/// </summary>
		/// <param name="name">The name of the binding</param>
		/// <param name="binding">The binding information</param>
		/// <returns>If the binding was found</returns>
		public bool TryFindBinding(string name, out BindSetLayoutBinding binding);

	}

	/// <summary>
	/// Shader creation information.
	/// </summary>
	public record ShaderCreateInfo {

		/// <summary>
		/// The type of shader to create.
		/// </summary>
		public ShaderType Type { get; init; }

		/// <summary>
		/// The type of source to create the shader from.
		/// </summary>
		public ShaderSourceType SourceType { get; init; }

		/// <summary>
		/// The object to use as the source code for the shader.
		/// </summary>
		public object Source { get; init; } = null!;

	}

}

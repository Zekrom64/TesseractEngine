using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Graphics.Accelerated {
	
	/// <summary>
	/// Enumeration of types of shader.
	/// </summary>
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
		/// <item><see cref="Span{T}"/></item>
		/// <item><see cref="ReadOnlySpan{T}"/></item>
		/// <item><see cref="Array"/></item>
		/// <item><see cref="IReadOnlyList{T}"/></item>
		/// </list>
		/// </summary>
		SPIRV,
		/// <summary>
		/// GLSL source code. Recognized types:
		/// <list type="bullet">
		/// <item><see cref="string"/></item>
		/// </list>
		/// </summary>
		GLSL
	}

	/// <summary>
	/// A shader stores the code for a programmable stage in a graphics pipeline.
	/// </summary>
	public interface IShader : IDisposable { }

	/// <summary>
	/// Shader creation information.
	/// </summary>
	public struct ShaderCreateInfo {

		/// <summary>
		/// The type of shader to create.
		/// </summary>
		public ShaderType Type { get; set; }

		/// <summary>
		/// The type of source to create the shader from.
		/// </summary>
		public ShaderSourceType SourceType { get; set; }

		/// <summary>
		/// The object to use as the source code for the shader.
		/// </summary>
		public object Source { get; set; }

	}

}

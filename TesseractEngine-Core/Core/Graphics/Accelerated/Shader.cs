using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
	/// <para>Enumeration of types of shader source types.</para>
	/// <para>
	/// All source types recognize source objects with types (of <see cref="byte"/>):
	/// <list type="bullet">
	/// <item><see cref="Array"/></item>
	/// <item><see cref="IConstPointer{T}"/> with an explicit size (or null-termination when treated as a string)</item>
	/// <item><see cref="ReadOnlyMemory{T}"/></item>
	/// </list>
	/// If any of these object types are used as a source they are reinterpreted as follows:
	/// <list type="table">
	/// <item>
	/// <term><see cref="SPIRV"/></term>
	/// <description>The underlying memory is treated as an array of <see cref="int"/>s in native byte order</description>
	/// </item>
	/// <item>
	/// <term><see cref="GLSL"/></term>
	/// <description>The underlying memory is treated as a (potentially null-terminated) ASCII string</description>
	/// </item>
	/// </list>
	/// </para>
	/// </summary>
	public enum ShaderSourceType {
		/// <summary>
		/// SPIR-V binary. Additionally recognizes types (of <see cref="int"/>):
		/// <list type="bullet">
		/// <item><see cref="Array"/></item>
		/// <item><see cref="IReadOnlyList{T}"/></item>
		/// <item><see cref="ReadOnlyMemory{T}"/></item>
		/// <item><see cref="IConstPointer{T}"/> with explicit size</item>
		/// </list>
		/// </summary>
		SPIRV,
		/// <summary>
		/// GLSL source code. Additionaly recognizes types:
		/// <list type="bullet">
		/// <item><see cref="string"/></item>
		/// <item><see cref="IReadOnlyList{T}"/> of <see cref="char"/></item>
		/// <item>The object's <see cref="object.ToString()"/> method</item>
		/// </list>
		/// </summary>
		GLSL
	}

	/// <summary>
	/// Utilities for reading shader sources.
	/// </summary>
	public static class ShaderSourceUtil {

		/// <summary>
		/// Gets a span containing the SPIR-V binary data for the given source object.
		/// </summary>
		/// <param name="obj">SPIR-V source object</param>
		/// <returns>SPIR-V binary</returns>
		/// <exception cref="ArgumentException">If the source object is invalid for SPIR-V</exception>
		public static ReadOnlySpan<int> GetSPIRV(object obj) {
			// Standard object types
			if (obj is byte[] barr) return MemoryMarshal.Cast<byte, int>(barr);
			else if (obj is IConstPointer<byte> cpb) {
				if (cpb.ArraySize < 0) throw new ArgumentException("Cannot get SPIR-V source from IConstPointer<byte> with no explicit size");
				return MemoryUtil.RecastAs<byte, int>(cpb).ReadOnlySpan;
			} else if (obj is ReadOnlyMemory<byte> romb) return MemoryMarshal.Cast<byte, int>(romb.Span);

			// SPIR-V object types
			if (obj is int[] iarr) return iarr;
			else if (obj is IReadOnlyList<int> lst) return lst.ToArray();
			else if (obj is ReadOnlyMemory<int> romi) return romi.Span;
			else if (obj is IConstPointer<int> cpi) {
				if (cpi.ArraySize < 0) throw new ArgumentException("Cannot get SPIR-V source from IConstPointer<int> with no explicit size");
				return cpi.ReadOnlySpan;
			}

			throw new ArgumentException($"Cannot convert shader source of type \"{obj.GetType()}\" to SPIR-V", nameof(obj));
		}

		/// <summary>
		/// Gets a span containing the GLSL text bytes for the given source object.
		/// </summary>
		/// <param name="obj">GLSL source object</param>
		/// <returns>GLSL source code bytes</returns>
		public static ReadOnlySpan<byte> GetGLSL(object obj) {
			// Standard object types
			if (obj is byte[] barr) return barr;
			else if (obj is IConstPointer<byte> cpbPtr) {
				int len = cpbPtr.ArraySize;
				if (len < 0) len = MemoryUtil.FindFirst(cpbPtr.Ptr, 0);
				unsafe {
					return new ReadOnlySpan<byte>((void*)cpbPtr.Ptr, len);
				}
			} else if (obj is ReadOnlyMemory<byte> romb) return romb.Span;

			// GLSL object types
			var ascii = Encoding.ASCII;
			if (obj is string s) return ascii.GetBytes(s);
			else if (obj is IReadOnlyList<char> rolc) return ascii.GetBytes(rolc.ToArray());
			else return ascii.GetBytes(obj.ToString() ?? throw new NullReferenceException("Cannot convert object with null ToString() to GLSL"));
			
			//throw new ArgumentException($"Cannot convert shader source of type \"{obj.GetType()}\" to GLSL", nameof(obj));
		}

	}

	/// <summary>
	/// A shader stores the code for a programmable stage in a graphics pipeline.
	/// </summary>
	public interface IShader : IDisposable { 
	
		/// <summary>
		/// The type of shader this module is.
		/// </summary>
		public ShaderType Type { get; }
	
	}

	/// <summary>
	/// Shader creation information.
	/// </summary>
	public record ShaderCreateInfo {

		/// <summary>
		/// The type of shader to create.
		/// </summary>
		public required ShaderType Type { get; init; }

		/// <summary>
		/// The type of source to create the shader from.
		/// </summary>
		public required ShaderSourceType SourceType { get; init; }

		/// <summary>
		/// The object to use as the source code for the shader.
		/// </summary>
		public required object Source { get; init; }

		/// <summary>
		/// The name of the entry point for this shader stage in the shader object.
		/// </summary>
		public string EntryPoint { get; init; } = "main";

	}

	/// <summary>
	/// A shader program links the code between different shader modules.
	/// </summary>
	public interface IShaderProgram : IDisposable {

		/// <summary>
		/// Attempts to get the binding layout information for a pipeline resource binding with the given name. If the
		/// binding was successfully retrieved the binding type and location are always valid, but if the stages the binding
		/// is used in cannot be determined <see cref="BindSetLayoutBinding.Stages"/> will be zero.
		/// </summary>
		/// <param name="name">Resource binding name</param>
		/// <param name="binding">Resource binding information</param>
		/// <returns>If the binding information was found</returns>
		public bool TryGetBinding(string name, out BindSetLayoutBinding binding);
	
	}

	/// <summary>
	/// Shader program creation information.
	/// </summary>
	public record ShaderProgramCreateInfo {

		/// <summary>
		/// The list of shader modules to link for the program.
		/// </summary>
		public required IReadOnlyList<IShader> Modules { get; init; }

	}

}

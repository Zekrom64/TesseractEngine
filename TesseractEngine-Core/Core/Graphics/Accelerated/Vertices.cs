﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using Tesseract.Core.Numerics;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// This attribute can be applied to struct fields to indicate they are vertex attributes.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class VertexAttribAttribute : Attribute {

		/// <summary>
		/// The location of the vertex attribute, or -1 to use the implied index given the order of fields.
		/// </summary>
		public int Location { get; init; } = -1;

		/// <summary>
		/// The format of the vertex attribute.
		/// </summary>
		public required PixelFormat Format { get; init; }

		/// <summary>
		/// The byte offset of the attribute
		/// </summary>
		public int Offset { get; init; } = -1;

		public VertexAttribAttribute() { }

	}

	/// <summary>
	/// Structure describing vertex attributes.
	/// </summary>
	public record struct VertexAttrib {

		/// <summary>
		/// The location of the attribute mapping to a shader input.
		/// </summary>
		public uint Location { get; set; }

		/// <summary>
		/// The binding index of the attribute mapping to a vertex buffer binding.
		/// </summary>
		public uint Binding { get; set; }

		/// <summary>
		/// The format of the attribute.
		/// </summary>
		public PixelFormat Format { get; set; }

		/// <summary>
		/// The byte offset of the attribute from the start of a vertex.
		/// </summary>
		public uint Offset { get; set; }

		/// <summary>
		/// Loads a list of attributes from the given type based on the type's fields.
		/// </summary>
		/// <param name="type">Type to load attributes from</param>
		/// <param name="binding">Binding index to apply to vertex attributes</param>
		/// <param name="nextLocation">First location to use for implied attribute locations</param>
		/// <returns>List of vertex attributes</returns>
		public static IReadOnlyList<VertexAttrib> LoadAttribs(Type type, uint binding, int nextLocation = 0) {
			List<VertexAttrib> attribs = new();
			foreach (FieldInfo field in type.GetFields().OrderBy(field => field.MetadataToken)) {
				if (field.GetCustomAttribute(typeof(VertexAttribAttribute)) is VertexAttribAttribute attrib) {
					PixelFormat format = attrib.Format;
					int location = attrib.Location;
					if (location == -1) {
						location = nextLocation++;
					}
					int offset = attrib.Offset;
					if (offset == -1) {
						offset = (int)Marshal.OffsetOf(type, field.Name);
					}
					attribs.Add(new VertexAttrib() {
						Binding = binding,
						Format = format,
						Location = (uint)location,
						Offset = (uint)offset
					});
				}
			}
			return attribs;
		}

	}

	/// <summary>
	/// Enumeration of input rates for vertex attributes.
	/// </summary>
	public enum VertexInputRate {
		/// <summary>
		/// The attribute is read for every vertex.
		/// </summary>
		PerVertex,
		/// <summary>
		/// The attribute is read for every instance.
		/// </summary>
		PerInstance
	}

	/// <summary>
	/// A vertex binding describes how groups of one or more vertex attributes are fetched from a vertex buffer.
	/// </summary>
	public record struct VertexBinding {

		/// <summary>
		/// The index of the vertex buffer to fetch attributes from.
		/// </summary>
		public uint Binding { get; set; }

		/// <summary>
		/// The stride between groups of attributes in the vertex buffer.
		/// </summary>
		public uint Stride { get; set; }

		/// <summary>
		/// The rate at which groups of attributes are fetched from the vertex buffer.
		/// </summary>
		public VertexInputRate InputRate { get; set; }

	}

	/// <summary>
	/// A vertex format describes how a set of vertex attributes are ordered and fetched from a set of vertex bindings.
	/// </summary>
	public record class VertexFormat : IEquatable<VertexFormat> {

		/// <summary>
		/// The collection of vertex attributes used in the format.
		/// </summary>
		public IReadOnlyCollection<VertexAttrib> Attributes { get; }

		/// <summary>
		/// The collection of vertex bindings used in the format.
		/// </summary>
		public IReadOnlyCollection<VertexBinding> Bindings { get; }

		/// <summary>
		/// Creates a vertex format using the given collections of attributes and bindings.
		/// </summary>
		/// <param name="attribs">Vertex attributes in the format</param>
		/// <param name="bindings">Vertex bindings in the format</param>
		public VertexFormat(IReadOnlyCollection<VertexAttrib> attribs, IReadOnlyCollection<VertexBinding> bindings) {
			Attributes = new List<VertexAttrib>(attribs);
			Bindings = new List<VertexBinding>(bindings);
		}

	}

	/// <summary>
	/// <para>
	/// A vertex array is an object that stores a vertex format and binding information
	/// for vertex and index buffers. Once created the vertex array cannot be modified, and
	/// changes to the drawn geometry should be done either using draw indirection or by
	/// modifying the underlying vertex and index buffers.
	/// </para>
	/// <para>
	/// Note that vertex format information still must be associated with pipelines during
	/// their creation as some APIs require it. No explicit test is performed when vertex arrays
	/// are bound to check if their vertex format matches that of the bound pipeline, and
	/// doing so with different vertex arrays will cause undefined behavior.
	/// </para>
	/// </summary>
	public interface IVertexArray : IDisposable {

		/// <summary>
		/// The format of the vertices in the vertex array.
		/// </summary>
		public VertexFormat Format { get; }

	}

	/// <summary>
	/// Enumeration of index value types.
	/// </summary>
	public enum IndexType {
		/// <summary>
		/// Unsigned 8-bit indices.
		/// </summary>
		UInt8,
		/// <summary>
		/// Unsigned 16-bit indices.
		/// </summary>
		UInt16,
		/// <summary>
		/// Unsigned 32-bit indices.
		/// </summary>
		UInt32
	}

	/// <summary>
	/// Creation information structure for vertex arrays.
	/// </summary>
	public record VertexArrayCreateInfo {

		/// <summary>
		/// The format of vertices in the vertex array.
		/// </summary>
		public required VertexFormat Format { get; init; }

		/// <summary>
		/// The list of buffer bindings for vertex buffers with their associated
		/// binding indices.
		/// </summary>
		public (BufferBinding Binding, uint Index)[] VertexBuffers { get; init; } = Array.Empty<(BufferBinding, uint)>();

		/// <summary>
		/// The buffer binding for an index buffer.
		/// </summary>
		public (BufferBinding Binding, IndexType Type)? IndexBuffer { get; init; } = null;

	}

}

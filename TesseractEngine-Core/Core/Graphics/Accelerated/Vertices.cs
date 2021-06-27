using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// Enumeration of supported vertex attribute formats.
	/// </summary>
	public enum VertexAttribFormat {
		/// <summary>
		/// Undefined vertex attribute format.
		/// </summary>
		Undefined,
		/// <summary>
		/// Scalar 32-bit float.
		/// </summary>
		X32SFloat,
		/// <summary>
		/// 2-component 32-bit float vector.
		/// </summary>
		X32Y32SFloat,
		/// <summary>
		/// 3-component 32-bit float vector.
		/// </summary>
		X32Y32Z32SFloat,
		/// <summary>
		/// 4-component 32-bit float vector.
		/// </summary>
		X32Y32Z32W32SFloat,
		/// <summary>
		/// Scalar 32-bit signed integer.
		/// </summary>
		X32SInt,
		/// <summary>
		/// 2-component 32-bit signed integer vector.
		/// </summary>
		X32Y32SInt,
		/// <summary>
		/// 3-component 32-bit signed integer vector.
		/// </summary>
		X32Y32Z32SInt,
		/// <summary>
		/// 4-component 32-bit signed integer vector.
		/// </summary>
		X32Y32Z32W32SInt
	}

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
		public VertexAttribFormat Format { get; init; } = VertexAttribFormat.Undefined;

		/// <summary>
		/// The byte offset of the attribute
		/// </summary>
		public int Offset { get; init; } = -1;

		public VertexAttribAttribute() { }

	}

	/// <summary>
	/// Structure describing vertex attributes.
	/// </summary>
	public struct VertexAttrib : IEquatable<VertexAttrib> {

		/// <summary>
		/// The location of the attribute mapping to a shader input.
		/// </summary>
		public uint Location;

		/// <summary>
		/// The binding index of the attribute mapping to a vertex buffer binding.
		/// </summary>
		public uint Binding;

		/// <summary>
		/// The format of the attribute.
		/// </summary>
		public VertexAttribFormat Format;

		/// <summary>
		/// The byte offset of the attribute from the start of a vertex.
		/// </summary>
		public uint Offset;

		public bool Equals(VertexAttrib attrib) =>
			Location == attrib.Location &&
			Binding == attrib.Binding &&
			Format == attrib.Format &&
			Offset == attrib.Offset;

		public override bool Equals(object obj) {
			if (obj is VertexAttrib attrib) return Equals(attrib);
			else return false;
		}

		public override int GetHashCode() => (int)((Location << 8) ^ (Binding << 6) ^ (uint)Format ^ Offset);

		public static bool operator ==(VertexAttrib a1, VertexAttrib a2) => a1.Equals(a2);

		public static bool operator !=(VertexAttrib a1, VertexAttrib a2) => !(a1 == a2);

		private static readonly Dictionary<Type, VertexAttribFormat> attribFormatTypes = new() {
			{ typeof(float), VertexAttribFormat.X32SFloat },
			{ typeof(Vector2), VertexAttribFormat.X32Y32SFloat },
			{ typeof(Vector3), VertexAttribFormat.X32Y32Z32SFloat },
			{ typeof(Vector4), VertexAttribFormat.X32Y32Z32W32SFloat },
			{ typeof(int), VertexAttribFormat.X32SInt },
			{ typeof(Vector2i), VertexAttribFormat.X32Y32SInt },
			{ typeof(Vector3i), VertexAttribFormat.X32Y32Z32SInt },
			{ typeof(Vector4i), VertexAttribFormat.X32Y32Z32W32SInt }
		};

		/// <summary>
		/// Loads a list of attributes from the given type based on the type's fields.
		/// </summary>
		/// <param name="type">Type to load attributes from</param>
		/// <param name="binding">Binding index to apply to vertex attributes</param>
		/// <param name="nextLocation">First location to use for implied attribute locations</param>
		/// <returns>List of vertex attributes</returns>
		public static IReadOnlyList<VertexAttrib> LoadAttribs(Type type, uint binding, int nextLocation = 0) {
			List<VertexAttrib> attribs = new();
			foreach(FieldInfo field in type.GetFields().OrderBy(field => field.MetadataToken)) {
				if (field.GetCustomAttribute(typeof(VertexAttribAttribute)) is VertexAttribAttribute attrib) {
					VertexAttribFormat format = attrib.Format;
					if (format == VertexAttribFormat.Undefined) {
						if (!attribFormatTypes.TryGetValue(field.FieldType, out format))
							throw new ArgumentException($"Attribute format cannot be derived from type of field \"{field}\"");
					}
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

	public struct VertexBinding : IEquatable<VertexBinding> {

		public uint Binding;
		public uint Stride;
		public VertexInputRate InputRate;

		public bool Equals(VertexBinding binding) =>
			Binding == binding.Binding &&
			Stride == binding.Stride &&
			InputRate == binding.InputRate;

		public override bool Equals(object obj) {
			if (obj is VertexBinding binding) return Equals(binding);
			else return false;
		}

		public override int GetHashCode() => (int)((Binding << 8) ^ Stride ^ ((uint)InputRate << 4));

		public static bool operator ==(VertexBinding b1, VertexBinding b2) => b1.Equals(b2);

		public static bool operator !=(VertexBinding b1, VertexBinding b2) => !(b1 == b2);

	}

	public class VertexFormat : IEquatable<VertexFormat> {

		public readonly IReadOnlyCollection<VertexAttrib> Attributes;
		public readonly IReadOnlyCollection<VertexBinding> Bindings;

		public VertexFormat(IReadOnlyCollection<VertexAttrib> attribs, IReadOnlyCollection<VertexBinding> bindings) {
			Attributes = new List<VertexAttrib>(attribs);
			Bindings = new List<VertexBinding>(bindings);
		}

		public bool Equals(VertexFormat format) {
			if (format == this) return true;
			if (format == null) return false;
			if (Attributes.Count != format.Attributes.Count) return false;
			if (Bindings.Count != format.Bindings.Count) return false;
			if (!Attributes.All(format.Attributes.Contains)) return false;
			if (!Bindings.All(format.Bindings.Contains)) return false;
			return true;
		}

		public override bool Equals(object obj) {
			if (obj is VertexFormat format) return Equals(format);
			else return false;
		}

		public override int GetHashCode() => Attributes.GetHashCode() ^ Bindings.GetHashCode();

		public static bool operator ==(VertexFormat f1, VertexFormat f2) {
			if (f1 == f2) return true;
			if (f1 == null) return false;
			return f1.Equals(f2);
		}

		public static bool operator !=(VertexFormat f1, VertexFormat f2) => !(f1 == f2);

	}

	/// <summary>
	/// A vertex array is an object that stores a vertex format and binding information
	/// for vertex and index buffers. Once created the vertex array cannot be modified, and
	/// changes to the drawn geometry should be done either using draw indirection or by
	/// modifying the underlying vertex and index buffers.
	/// </summary>
	public interface IVertexArray : IDisposable {

		/// <summary>
		/// The format of the vertices in the vertex array.
		/// </summary>
		public VertexFormat Format { get; }

	}

	/// <summary>
	/// Creation information structure for vertex arrays.
	/// </summary>
	public struct VertexArrayCreateInfo {

		/// <summary>
		/// The format of vertices in the vertex array.
		/// </summary>
		public VertexFormat Format { get; set; }

		/// <summary>
		/// The list of buffer bindings for vertex buffers.
		/// </summary>
		public BufferBinding[] VertexBuffers { get; set; }

		/// <summary>
		/// The buffer binding for an index buffer.
		/// </summary>
		public BufferBinding IndexBuffer { get; set; }

	}

}

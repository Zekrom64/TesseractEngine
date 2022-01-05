using System.Runtime.InteropServices;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// <para>
	/// A draw mode describes how vertices are fetched and assembled into primitives.
	/// </para>
	/// <para>
	/// The fundamental primitives supported are:
	/// <list type="bullet">
	/// <item>Points - Geometry defined by a single point in space.</item>
	/// <item>Lines - Geometry defined as a line between two points in space.</item>
	/// <item>Triangles - Geometry defined as a plane between three points in space.</item>
	/// </list>
	/// Additionally, tesselation shaders may take in patches of arbitrary numbers of vertices set by
	/// the pipeline tessellation settings.
	/// </para>
	/// <para>
	/// The exact rasterization of primitives into fragments is
	/// determined by the pipeline rasterization stage but in general; points are converted to fragments
	/// covering a square centered on the point position (the size being determined by the value assigned
	/// inside a vertex or tessellation shader), lines are converted into fragments intersecting
	/// the line as determined by an implementation-dependent line drawing algorithm (though this
	/// may be specified by a pipeline, the width of the line is specified by the current pipeline state),
	/// and triangles are converted into fragments covering the defined plane.
	/// </para>
	/// <para>
	/// For more information on primitive topologies or primitive rasterization see the respective sections
	/// in the OpenGL, Vulkan, or DirectX specifications.
	/// </para>
	/// </summary>
	public enum DrawMode {
		/// <summary>
		/// Vertices are fetched one at a time, with each vertex describing a single point in space.
		/// </summary>
		PointList,
		/// <summary>
		/// Vertices are fetched in pairs, with each pair describing a single line.
		/// </summary>
		LineList,
		/// <summary>
		/// An initial pair of vertices is fetched describing the first line segment, and each subsequent vertex describes a new line segment
		/// between itself and the previous vertex.
		/// </summary>
		LineStrip,
		/// <summary>
		/// Vertices are fetched three at a time, with each group describing a single triangle.
		/// </summary>
		TriangleList,
		/// <summary>
		/// An initial group of three vertices are fetched describing the first triangle, and each subsequent vertex describes a new triangle
		/// between itself and the previous two vertices.
		/// </summary>
		TriangleStrip,
		/// <summary>
		/// An initial group of three vertices are fetched describing the first triangle, and each subsequent vertex describes a new triangle
		/// between itself, the previous vertex, and the first vertex fetched.
		/// </summary>
		TriangleFan,
		/// <summary>
		/// Identical to <see cref="LineList"/>, but with adjacent vertices inserted before the first and
		/// after the second vertices.
		/// </summary>
		LineListWithAdjacency,
		/// <summary>
		/// Identical to <see cref="LineStrip"/>, but with adjacent vertices inserted before the first
		/// and after the last vertices.
		/// </summary>
		LineStripWithAdjacency,
		/// <summary>
		/// Identical to <see cref="TriangleList"/>, but with adjacent vertices inserted forming additional triangles along each edge of the
		/// triangles described in the list.
		/// </summary>
		TriangleListWithAdjacency,
		/// <summary>
		/// Identical to <see cref="TriangleStrip"/>, but with adjacent vertices inserted forming additional triangles along each external
		/// edge of the complete triangle strip.
		/// </summary>
		TriangleStripWithAdjacency,
		/// <summary>
		/// An arbitrary number of vertices are fetched, defined by the tessellation settings of the current pipeline. The
		/// actual primitive output is determined by the tessellation shaders.
		/// </summary>
		PatchList
	}

	/// <summary>
	/// A draw parameter structure stores parameters for a single non-indexed draw call. This
	/// structure may be stored into a GPU buffer and used for indirect drawing.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DrawParams {

		/// <summary>
		/// The number of vertices to draw.
		/// </summary>
		public uint VertexCount;
		/// <summary>
		/// The number of instances of vertices to draw.
		/// </summary>
		public uint InstanceCount;
		/// <summary>
		/// The offset of the first vertex in the current vertex array binding.
		/// </summary>
		public uint FirstVertex;
		/// <summary>
		/// The offset of the first instance for instanced rendering.
		/// </summary>
		public uint FirstInstance;

	}

	/// <summary>
	/// An indexed draw parameter structure stores parameters for a signle indexed draw call. This
	/// structure may be stored into a GPU buffer and used for indirect drawing.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DrawIndexedParams {

		/// <summary>
		/// The number of indices to draw.
		/// </summary>
		public uint IndexCount;
		/// <summary>
		/// The number of instances of indices to draw.
		/// </summary>
		public uint InstanceCount;
		/// <summary>
		/// The offset of the first index in the current vertex array binding.
		/// </summary>
		public uint FirstIndex;
		/// <summary>
		/// The offset to add to each index when vertices are fetched.
		/// </summary>
		public int VertexOffset;
		/// <summary>
		/// The offset of the first instance for instanced rendering.
		/// </summary>
		public uint FirstInstance;

	}

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Util;

namespace Tesseract.Core.Graphics.Model {

	/// <summary>
	/// A single draw call within a model.
	/// </summary>
	public struct ModelDrawCall {
	
		/// <summary>
		/// The mode to draw vertices with.
		/// </summary>
		public DrawMode Mode;

		/// <summary>
		/// The offset of the first unit (indices or vertices) to start drawing at.
		/// </summary>
		public int Offset;

		/// <summary>
		/// The number of units (indices or vertices) to draw.
		/// </summary>
		public int Length;
	
	}

	/// <summary>
	/// A node 
	/// </summary>
	public struct ModelNode {

		/// <summary>
		/// The string name of the node.
		/// </summary>
		public string Name;

		/// <summary>
		/// A matrix storing the transformation matrix of the meshes drawn
		/// at this node, relative to the model.
		/// </summary>
		public Matrix4x4 LocalTransform;

		/// <summary>
		/// The draw calls to perform at this node.
		/// </summary>
		public ModelDrawCall[] DrawCalls;

		/// <summary>
		/// The children of this node.
		/// </summary>
		public ModelNode[] Children;

	}

	/// <summary>
	/// A model buffer stores data in an opaque binary format for use elsewhere in a model.
	/// </summary>
	public interface IModelBuffer {

		/// <summary>
		/// Gets the data for this buffer as a span of bytes.
		/// </summary>
		public Span<byte> Bytes { get; }

	}

	public class ModelArrayBuffer<T> : IModelBuffer where T : unmanaged {

		public readonly Memory<T> Array;

		public Span<byte> Bytes => MemoryMarshal.AsBytes(Array.Span);

		public ModelArrayBuffer(Memory<T> array) {
			Array = array;
		}

		public ModelArrayBuffer(T[] array) {
			Array = new Memory<T>(array);
		}

	}

	public interface IModel {
		
		public IReadOnlyList<IModelBuffer> Buffers { get; }

		public ModelNode RootNode { get; }
		
	}

	public interface IModelLoadContext {

		public Stream OpenResource(string name);

	}

	public interface IModelSaveContext {

	}

	public interface IModelFormat {

		public bool CanSave { get; }

		public IModel Load(Stream stream, IModelLoadContext? context = null);

		public void Save(IModel model, Stream stream, IModelSaveContext? context = null);

	}

}

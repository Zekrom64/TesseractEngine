using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Resource;
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

	/// <summary>
	/// A model buffer implementation that wraps an array.
	/// </summary>
	/// <typeparam name="T">Element type</typeparam>
	public class ModelArrayBuffer<T> : IModelBuffer where T : unmanaged {

		private readonly Memory<T> Array;

		public Span<byte> Bytes => MemoryMarshal.AsBytes(Array.Span);

		/// <summary>
		/// Creates a new array buffer using the given memory.
		/// </summary>
		/// <param name="array">Array memory</param>
		public ModelArrayBuffer(Memory<T> array) {
			Array = array;
		}

		/// <summary>
		/// Creates a new array buffer using the given array.
		/// </summary>
		/// <param name="array">Array to wrap</param>
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

		public Stream OpenResource(string name);

	}

	public interface IModelFormat {

		/// <summary>
		/// An enumeratino of MIME types identifying this model format.
		/// </summary>
		public IEnumerable<string> MIMETypes { get; }

		/// <summary>
		/// If the format can save.
		/// </summary>
		public bool CanSave { get; }

		/// <summary>
		/// Loads a model 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public IModel Load(Stream stream, IModelLoadContext? context = null);

		public void Save(IModel model, Stream stream, IModelSaveContext? context = null);

	}

	public interface IModelFormatDetector {

		/// <summary>
		/// Gets the size of the header 
		/// </summary>
		public int HeaderSize { get; }

		public IModelFormat? DetectFormat(in ReadOnlySpan<byte> header);

	}

	public interface IModelIO {

		public bool CanLoad(string mimeType);

		public bool CanSave(string mimeType);

		public IModel Load(ResourceLocation resource);

		public IModel Load(Stream stream, IModelLoadContext? context = null);

		public void Save(IModel model, Stream stream, IModelSaveContext? context = null);

	}

	public class ModelIO : IModelIO {

		private readonly Dictionary<string, IModelFormat> mimeFormats = new();
		private readonly List<IModelFormatDetector> detectors = new();

		public void AddFormat(IModelFormat format, IModelFormatDetector? detector) {
			foreach (var mime in format.MIMETypes) mimeFormats[mime] = format;
			if (detector != null) detectors.Add(detector);
		}

		public bool CanLoad(string mimeType) => mimeFormats.ContainsKey(mimeType);

		public bool CanSave(string mimeType) {
			if (!mimeFormats.TryGetValue(mimeType, out var format)) return false;
			return format.CanSave;
		}

		private IModel Load(string? mime, Stream stream, IModelLoadContext? context) {
			IModelFormat? format = null;
			Stream loadStream = stream;
			if (mime == null) {
				int reqHeaderSize = 0;
				foreach (var detector in detectors) reqHeaderSize = System.Math.Max(reqHeaderSize, detector.HeaderSize);

				Span<byte> header = stackalloc byte[reqHeaderSize];
				int headerSize;

				if (stream.CanSeek) {
					headerSize = stream.ReadAny(header);
					stream.Position = 0;
				} else {
					PushbackStream pbStream = new(stream);
					loadStream = pbStream;
					headerSize = stream.ReadAny(header);
					pbStream.Unread(header[..headerSize]);
				}

				header = header[..headerSize];
				foreach (var detector in detectors) {
					format = detector.DetectFormat(header);
					if (format != null) break;
				}
			} else format = mimeFormats.GetValueOrDefault(mime);

			if (format == null) throw new NotSupportedException("Cannot load models in an unknown/unsupported format");

			return format.Load(loadStream, context);
		}

		private class ResourceModelLoadContext : IModelLoadContext {

			private readonly ResourceLocation directory;

			public ResourceModelLoadContext(ResourceLocation loc) {
				directory = loc.Parent;
			}

			public Stream OpenResource(string name) {
				return new ResourceLocation(directory, name).OpenStream();
			}
		}

		public IModel Load(ResourceLocation resource) {
			using Stream stream = resource.OpenStream();
			return Load(resource.Metadata.MIMEType, stream, new ResourceModelLoadContext(resource));
		}

		public IModel Load(Stream stream, IModelLoadContext? context = null) => Load(null, stream, context);

		public void Save(IModel model, Stream stream, IModelSaveContext? context = null) {
			throw new NotImplementedException();
		}
	}

}

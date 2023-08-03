using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Utilities.Data {

	/// <summary>
	/// Interface for objects that can write data objects in a streaming manner.
	/// </summary>
	public interface IStreamingDataObject {

		/// <summary>
		/// Writes a key-value pair for this object.
		/// </summary>
		/// <param name="key">Key to write</param>
		public DataBox this[string key] { set; }

		/// <summary>
		/// Writes a key-value pair where the value itself is a streaming object.
		/// </summary>
		/// <param name="key">Key to write</param>
		/// <param name="writer">Writer for the streaming object</param>
		public void Write(string key, Action<IStreamingDataObject> writer);

	}

	/// <summary>
	/// A data object stream can write the contents data objects without creating intermediate values.
	/// </summary>
	public class DataObjectStream : IStreamingDataObject, IDisposable {

		private readonly BinaryWriter writer;

		public DataObjectStream(BinaryWriter bw) {
			writer = bw;
			writer.Write((byte)DataType.ObjectStreaming);
		}

		public DataBox this[string key] {
			set {
				value.Write(writer);
				StructuredData.WriteString(key, writer);
			}
		}

		public void Write(string key, Action<IStreamingDataObject> wr) {
			writer.Write((byte)DataType.ObjectStreaming);
			wr(this);
			StructuredData.WriteString(key, writer);
			default(DataBox).Write(writer);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			default(DataBox).Write(writer);
		}

		public static DataObject Construct(BinaryReader br) {
			DataObject data = new();
			do {
				DataBox val = DataBox.Construct(br);
				if (val == default) break;
				data[StructuredData.ReadString(br)] = val;
			} while (true);
			return data;
		}

	}

}

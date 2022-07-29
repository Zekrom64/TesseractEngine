using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Utilities {
	
	/// <summary>
	/// Interface for objects that can be serialized to binary data.
	/// </summary>
	public interface IWritableData {

		/// <summary>
		/// Writes this object to a binary stream.
		/// </summary>
		/// <param name="bw">Binary stream writer</param>
		public void Write(BinaryWriter bw);

	}

	/// <summary>
	/// Interface for objects that can be serialized and deserialized as binary data.
	/// </summary>
	public interface IData : IWritableData {

		/// <summary>
		/// Reads this object from a binary stream.
		/// </summary>
		/// <param name="br">Binary stream reader</param>
		public void Read(BinaryReader br);

	}

}

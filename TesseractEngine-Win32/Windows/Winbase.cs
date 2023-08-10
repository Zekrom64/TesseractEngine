using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.Windows {

	/// <summary>
	/// The SECURITY_ATTRIBUTES structure contains the security descriptor for an object and specifies whether the handle
	/// retrieved by specifying this structure is inheritable.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SECURITY_ATTRIBUTES {

		/// <summary>
		/// The size, in bytes, of this structure. Set this value to the size of the <c>SECURITY_ATTRIBUTES</c> structure.
		/// </summary>
		public uint Length;

		/// <summary>
		/// A pointer to a <c>SECURITY_DESCRIPTOR</c> structure that controls access to the object. If the value of this member
		/// is null, the object is assigned the default security descriptor associated with the access token of the calling process.
		/// </summary>
		public IntPtr SecurityDescriptor;

		/// <summary>
		/// A boolean value that specifies whether the returned handle is inherited when a new process is created. If this member is
		/// true, the new process inherits the handle.
		/// </summary>
		public bool InheritHandle;

	}

}

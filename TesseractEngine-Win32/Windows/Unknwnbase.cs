using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.Windows {

	using ULONG = UInt32;

	/// <summary>
	/// <para>
	/// Enables clients to get pointers to other interfaces on a given object through the <see cref="QueryInterface{T}">QueryInterface</see>
	/// method, and manage the existence of the object through the <see cref="AddRef">AddRef</see> and <see cref="Release">Release</see>
	/// methods. All other COM interfaces are inherited, directly or indirectly, from <b>IUnknown</b>. Therefore, the three methods in
	/// <b>IUnknown</b> are the first entries in the vtable for every interface.
	/// </para>
	/// 
	/// <para>
	/// In the CLR this interface is not very useful on its own, as COM wrappers handle most conversions and abstract the
	/// details of reference counting and type coersion. This interface is included to provide access to the <b>IUnknown</b>
	/// methods of a COM object if there is an edge-case where they are needed.
	/// </para>
	/// </summary>
	[ComImport, Guid("00000000-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IUnknown : IDisposable {

		/// <summary>
		/// Queries a COM object for a pointer to one of its interface; identifying the interface by a reference to its interface
		/// identifier (IID). If the COM object implements the interface, then it returns a pointer to that interface after calling
		/// <see cref="AddRef"/> on it.
		/// </summary>
		/// <param name="riid"></param>
		/// <returns></returns>
		public IntPtr QueryInterface(in Guid riid);

		/// <summary>
		/// <para>Increments the reference count for an interface pointer to a COM object. You should call this method whenever you make a copy of an interface pointer.</para>
		/// 
		/// <para>
		/// A COM object uses a per-interface reference-counting mechanism to ensure that the object doesn't outlive references
		/// to it. You use AddRef to stabilize a copy of an interface pointer. It can also be called when the life of a cloned
		/// pointer must extend beyond the lifetime of the original pointer. The cloned pointer must be released by calling
		/// <see cref="Release"/> on it.
		/// </para>
		/// 
		/// <para>
		/// The internal reference counter that <b>AddRef</b> maintains should be a 32-bit unsigned integer.
		/// </para>
		/// 
		/// <para>
		/// <b>Notes to callers</b><br/>
		/// Call this method for every new copy of an interface pointer that you make. For example, if you return a copy of a
		/// pointer from a method, then you must call <b>AddRef on that pointer</b>. You must also call <b>AddRef</b> on a pointer
		/// before passing it as an in-out parameter to a method; the method will call <see cref="Release"/> before copying the
		/// out-value on top of it.
		/// </para>
		/// 
		/// </summary>
		/// <returns>The method returns the new reference count. This value is intended to be used only for test purposes.</returns>
		[PreserveSig]
		public ULONG AddRef();

		/// <summary>
		/// <para>Decrements the reference count for an interface on a COM object.</para>
		/// 
		/// <para>
		/// When the reference count on an object reaches zero, <b>Release</b> must cause the interface pointer to free itself.
		/// When the released pointer is the only (formerly) outstanding reference to an object (whether the object supports single
		/// or multiple interfaces), the implementation must free the object.
		/// </para>
		/// 
		/// <para>
		/// Note that aggregation of objects restricts the ability to recover interface pointers.
		/// </para>
		/// 
		/// <para>
		/// <b>Notes to callers</b><br/>
		/// Call this method when you no longer need to use an interface pointer. If you are writing a method that takes an in-out
		/// parameter, call <b>Release</b> on the pointer you are passing in before copying the out-value on top of it.
		/// </para>
		/// </summary>
		/// <returns>The method returns the new reference count. This value is intended to be used only for test purposes.</returns>
		[PreserveSig]
		public ULONG Release();

	}

}

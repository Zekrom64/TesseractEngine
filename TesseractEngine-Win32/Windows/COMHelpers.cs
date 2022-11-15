using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Tesseract.Windows {
	
	/// <summary>
	/// Delegate function commonly implemented by COM interfaces for getting subinterfaces.
	/// </summary>
	/// <param name="riid">The ID of the interface to get</param>
	/// <returns>COM object pointer for the given interface</returns>
	public delegate IntPtr COMObjectGetter(in Guid riid);

	/// <summary>
	/// Methods which help with COM interop.
	/// </summary>
	public static class COMHelpers {

		/// <summary>
		/// Gets the UUID of the given COM interface.
		/// </summary>
		/// <typeparam name="T">COM interface type</typeparam>
		/// <returns>COM interface UUID</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Guid GetCOMID<T>() => typeof(T).GUID;

		/// <summary>
		/// Converts a COM object pointer into a runtime callable wrapper (RCW) of the given type.
		/// </summary>
		/// <typeparam name="T">Wrapper type</typeparam>
		/// <param name="ppvObject">COM object pointer</param>
		/// <returns>Wrapped COM object</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetObjectForCOMPointer<T>(IntPtr ppvObject) where T : class =>
			ppvObject == IntPtr.Zero ? null : (T)Marshal.GetTypedObjectForIUnknown(ppvObject, typeof(T));

		/// <summary>
		/// Retrieves a COM object using an appropriate getter function.
		/// </summary>
		/// <typeparam name="T">Retrieved object wrapper type</typeparam>
		/// <param name="getter">Getter function</param>
		/// <returns>COM object retrieved from the getter</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetObjectFromCOMGetter<T>(COMObjectGetter getter) where T : class =>
			GetObjectForCOMPointer<T>(getter(GetCOMID<T>()));

		/// <summary>
		/// Casts a COM wrapper dervied from the <see cref="IUnknown"/> interface to a different
		/// wrapper type using the <see cref="IUnknown.QueryInterface(in Guid)"/> method.
		/// </summary>
		/// <typeparam name="T">Type to cast to</typeparam>
		/// <param name="obj">COM wrapper object to cast</param>
		/// <returns>Converted COM object</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		// QueryInterface will succeed or throw an exception so we can assume it will not return null
		public static T Cast<T>(IUnknown obj) where T : class => GetObjectFromCOMGetter<T>(obj.QueryInterface)!;

		/// <summary>
		/// Gets the corresponding COM object pointer for 
		/// </summary>
		/// <typeparam name="T">Type to convert</typeparam>
		/// <param name="obj">COM wrapper object to convert</param>
		/// <returns>COM object pointer for interface</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr GetCOMPointerForObject<T>(object obj) => Marshal.GetComInterfaceForObject(obj, typeof(T));

	}

}

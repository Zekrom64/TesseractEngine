using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.DirectX.Core;
using Tesseract.Windows;

namespace Tesseract.DirectX.Managed {

	/// <inheritdoc cref="IDXCoreAdapterFactory"/>
	public class DXCoreAdapterFactory : IDisposable {

		/// <summary>
		/// The underlying COM object.
		/// </summary>
		public IDXCoreAdapterFactory COM { get; }

		/// <summary>
		/// Constructs a managed DXCore adapter factory from a COM object.
		/// </summary>
		/// <param name="com">The COM object</param>
		public DXCoreAdapterFactory(IDXCoreAdapterFactory com) { COM = com; }

		/// <summary>
		/// Creates a managed DXCore adapter factory using the <see cref="DXCore.CreateAdapterFactory{T}"/> method.
		/// </summary>
		public DXCoreAdapterFactory() { COM = DXCore.CreateAdapterFactory<IDXCoreAdapterFactory>(); }

		public void Dispose() {
			GC.SuppressFinalize(this);
			COM.Release();
		}

		/// <inheritdoc cref="IDXCoreAdapterFactory.CreateAdapterList(uint, nint, in Guid, out nint)"/>
		public DXCoreAdapterList CreateAdapterList(in ReadOnlySpan<Guid> filterAttributes) {
			unsafe {
				fixed (Guid* pFilterAttributes = filterAttributes) {
					COM.CreateAdapterList((uint)filterAttributes.Length, (nint)pFilterAttributes, COMHelpers.GetCOMID<IDXCoreAdapterList>(), out IntPtr pAdapterList);
					return new DXCoreAdapterList(COMHelpers.GetObjectForCOMPointer<IDXCoreAdapterList>(pAdapterList)!, this);
				}
			}
		}

		/// <inheritdoc cref="IDXCoreAdapterFactory.GetAdapterByLuid(in LUID, in Guid, out nint)"/>
		public DXCoreAdapter GetAdapterByLuid(in LUID adapterLUID) {
			var riid = COMHelpers.GetCOMID<IDXCoreAdapter>();
			COM.GetAdapterByLuid(adapterLUID, riid, out nint pAdapter);
			return new(COMHelpers.GetObjectForCOMPointer<IDXCoreAdapter>(pAdapter)!, this);
		}

		/// <inheritdoc cref="IDXCoreAdapterFactory.IsNotificationTypeSupported(DXCoreNotificationType)"/>
		public bool IsNotificationTypeSupported(DXCoreNotificationType notificationType) => COM.IsNotificationTypeSupported(notificationType);

	}

	/// <inheritdoc cref="IDXCoreAdapterList"/>
	public class DXCoreAdapterList : IDisposable, IReadOnlyList<DXCoreAdapter> {

		/// <summary>
		/// The underlying COM object.
		/// </summary>
		public IDXCoreAdapterList COM { get; }

		/// <summary>
		/// The adapter factory that created this list.
		/// </summary>
		public DXCoreAdapterFactory Factory { get; }

		// Self reference for callback
		private readonly ObjectPointer<DXCoreAdapterList> SelfPtr;

		/// <summary>
		/// Event fired when the list is notified of becoming stale (ie. when the
		/// list of adapters in the system changes).
		/// </summary>
		public event Action<DXCoreAdapterList>? OnStale;

		// Stale list callback
		private static void NotifyStale(DXCoreNotificationType notificationType, object obj, IntPtr context) {
			var list = new ObjectPointer<DXCoreAdapterList>(context).Value!;
			list.OnStale?.Invoke(list);
		}

		/// <summary>
		/// Constructs a managed DXCore adapter list from a COM object, optionally supplying the factory 
		/// it was created with and constructing it from <see cref="IDXCoreAdapterList.GetFactory(in Guid, out nint)"/>
		/// otherwise.
		/// </summary>
		/// <param name="com">The COM object</param>
		/// <param name="factory">The adapter factory, or null</param>
		public DXCoreAdapterList(IDXCoreAdapterList com, DXCoreAdapterFactory? factory = null) {
			SelfPtr = new ObjectPointer<DXCoreAdapterList>(this);
			COM = com;
			Factory = factory ?? new DXCoreAdapterFactory(COMHelpers.GetObjectFromCOMGetter<IDXCoreAdapterFactory>(com.GetFactory)!);
			Factory.COM.RegisterEventNotification(COM, DXCoreNotificationType.AdapterListStale, NotifyStale, SelfPtr, out _);
			Count = (int)COM.GetAdapterCount();
			adapters = new DXCoreAdapter?[Count];
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			SelfPtr.Dispose();
			COM.Release();
		}


		/// <inheritdoc cref="IDXCoreAdapterList.IsStale"/>
		public bool IsStale => COM.IsStale();

		/// <inheritdoc cref="IDXCoreAdapterList.Sort(uint, nint)"/>
		public void Sort(in ReadOnlySpan<DXCoreAdapterPreference> preferences) {
			unsafe {
				fixed(DXCoreAdapterPreference* pPreferences = preferences) {
					COM.Sort((uint)preferences.Length, (nint)pPreferences);
				}
			}
		}

		/// <inheritdoc cref="IDXCoreAdapterList.IsAdapterPreferenceSupported(DXCoreAdapterPreference)"/>
		public bool IsAdapterPreferenceSupported(DXCoreAdapterPreference preference) => COM.IsAdapterPreferenceSupported(preference);

		// [IReadOnlyList<DXCoreAdapter>]

		// The list of adapter objects, lazily initialized
		private readonly DXCoreAdapter?[] adapters;

		public int Count { get; }

		public DXCoreAdapter this[int index] {
			get {
				var adapter = adapters[index];
				if (adapter == null) {
					adapter = new DXCoreAdapter(
						COMHelpers.GetObjectFromCOMGetter<IDXCoreAdapter>((in Guid riid, out IntPtr ptr) => COM.GetAdapter((uint)index, riid, out ptr))!,
						Factory
					);
					adapters[index] = adapter;
				}
				return adapter;
			}
		}

		public IEnumerator<DXCoreAdapter> GetEnumerator() {
			for (int i = 0; i < Count; i++) yield return this[i];
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	/// <inheritdoc cref="IDXCoreAdapter"/>
	public class DXCoreAdapter : IDisposable {

		/// <summary>
		/// The underlying COM object.
		/// </summary>
		public IDXCoreAdapter COM { get; }

		/// <summary>
		/// The factory that created this adapter.
		/// </summary>
		public DXCoreAdapterFactory Factory { get; }

		/// <summary>
		/// Constructs a managed DXCore adapter from a COM object, optionally supplying the factory 
		/// it was created with and constructing it from <see cref="IDXCoreAdapter.GetFactory(in Guid, out nint)"/>
		/// otherwise.
		/// </summary>
		/// <param name="com">The COM object</param>
		/// <param name="factory">The adapter factory, or null</param>
		public DXCoreAdapter(IDXCoreAdapter com, DXCoreAdapterFactory? factory = null) {
			COM = com;
			Factory = factory ?? new DXCoreAdapterFactory(COMHelpers.GetObjectFromCOMGetter<IDXCoreAdapterFactory>(COM.GetFactory)!);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			COM.Release();
		}

		/// <inheritdoc cref="IDXCoreAdapter.IsValid"/>
		public bool IsValid => COM.IsValid();

		/// <inheritdoc cref="IDXCoreAdapter.IsAttributeSupported(in Guid)"/>
		public bool IsAttributeSupported(in Guid attributeGuid) => COM.IsAttributeSupported(attributeGuid);

		/// <inheritdoc cref="IDXCoreAdapter.IsPropertySupported(DXCoreAdapterProperty)"/>
		public bool IsPropertySupported(DXCoreAdapterProperty property) => COM.IsPropertySupported(property);

		/// <inheritdoc cref="IDXCoreAdapter.IsQueryStateSupported(DXCoreAdapterState)"/>
		public bool IsQueryStateSupported(DXCoreAdapterState state) => COM.IsQueryStateSupported(state);

		/// <inheritdoc cref="IDXCoreAdapter.QueryState(DXCoreAdapterState, nuint, nint, nuint, nint)"/>
		public O QueryState<I,O>(DXCoreAdapterState state, I? input = null) where I : unmanaged where O : unmanaged {
			unsafe {
				I vinput = input ?? default;
				I* pInput = input.HasValue ? &vinput : (I*)0;
				O output = default;
				COM.QueryState(state, (nuint)(input.HasValue ? Marshal.SizeOf<I>() : 0), (nint)pInput, (nuint)Marshal.SizeOf<O>(), (nint)(&output));
				return output;
			}
		}

		/// <inheritdoc cref="IDXCoreAdapter.IsSetStateSupported(DXCoreAdapterState)"/>
		public bool IsSetStateSupported(DXCoreAdapterState state) => COM.IsSetStateSupported(state);

		/// <inheritdoc cref="IDXCoreAdapter.SetState(DXCoreAdapterState, nuint, nint, nuint, nint)"/>
		public O SetState<I, O>(DXCoreAdapterState state, I? input = null) where I : unmanaged where O : unmanaged {
			unsafe {
				I vinput = input ?? default;
				I* pInput = input.HasValue ? &vinput : (I*)0;
				O output = default;
				COM.SetState(state, (nuint)(input.HasValue ? Marshal.SizeOf<I>() : 0), (nint)pInput, (nuint)Marshal.SizeOf<O>(), (nint)(&output));
				return output;
			}
		}

		/// <inheritdoc cref="IDXCoreAdapter.GetProperty(DXCoreAdapterProperty, nuint, nint)"/>
		public byte[] GetProperty(DXCoreAdapterProperty property) {
			COM.GetPropertySize(property, out nuint size);
			byte[] data = new byte[(int)size];
			unsafe {
				fixed(byte* pData = data) {
					COM.GetProperty(property, (uint)data.Length, (nint)pData);
				}
			}
			return data;
		}

		/// <inheritdoc cref="IDXCoreAdapter.GetProperty(DXCoreAdapterProperty, nuint, nint)"/>
		public T GetProperty<T>(DXCoreAdapterProperty property) where T : unmanaged {
			unsafe {
				COM.GetPropertySize(property, out nuint size);
				if ((int)size != sizeof(T)) throw new ArgumentException("Sizeof mismatch for property", nameof(T));
				T value = default;
				COM.GetProperty(property, size, (IntPtr)(&value));
				return value;
			}
		}

	}

}

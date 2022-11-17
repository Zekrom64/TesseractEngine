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

	public class DXCoreAdapterFactory : IDisposable {

		public IDXCoreAdapterFactory COM { get; }

		public DXCoreAdapterFactory(IDXCoreAdapterFactory com) { COM = com; }

		public DXCoreAdapterFactory() { COM = DXCore.CreateAdapterFactory<IDXCoreAdapterFactory>(); }

		public void Dispose() {
			GC.SuppressFinalize(this);
			COM.Release();
		}

		public DXCoreAdapterList CreateAdapterList(in ReadOnlySpan<Guid> filterAttributes) {
			unsafe {
				fixed (Guid* pFilterAttributes = filterAttributes) {
					var list = COMHelpers.GetObjectForCOMPointer<IDXCoreAdapterList>(
						COM.CreateAdapterList((uint)filterAttributes.Length, (nint)pFilterAttributes, COMHelpers.GetCOMID<IDXCoreAdapterList>())
					)!;
					return new DXCoreAdapterList(list, this);
				}
			}
		}

		public DXCoreAdapter GetAdapterByLuid(in LUID adapterLUID) {
			var riid = COMHelpers.GetCOMID<IDXCoreAdapter>();
			nint pAdapter = COM.GetAdapterByLuid(adapterLUID, riid);
			return new(COMHelpers.GetObjectForCOMPointer<IDXCoreAdapter>(pAdapter)!, this);
		}

		public bool IsNotificationTypeSupported(DXCoreNotificationType notificationType) => COM.IsNotificationTypeSupported(notificationType);

	}

	public class DXCoreAdapterList : IDisposable, IReadOnlyList<DXCoreAdapter> {

		public IDXCoreAdapterList COM { get; }

		public DXCoreAdapterFactory Factory { get; }

		private readonly ObjectPointer<DXCoreAdapterList> SelfPtr;

		public event Action<DXCoreAdapterList>? OnStale;

		private static void NotifyStale(DXCoreNotificationType notificationType, object obj, IntPtr context) {
			var list = new ObjectPointer<DXCoreAdapterList>(context).Value!;
			list.OnStale?.Invoke(list);
		}

		public DXCoreAdapterList(IDXCoreAdapterList com, DXCoreAdapterFactory? factory = null) {
			SelfPtr = new ObjectPointer<DXCoreAdapterList>(this);
			COM = com;
			Factory = factory ?? new DXCoreAdapterFactory(COMHelpers.GetObjectFromCOMGetter<IDXCoreAdapterFactory>(com.GetFactory)!);
			Factory.COM.RegisterEventNotification(COM, DXCoreNotificationType.AdapterListStale, NotifyStale, SelfPtr);
			Count = (int)COM.GetAdapterCount();
			adapters.EnsureCapacity(Count);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			SelfPtr.Dispose();
			COM.Release();
		}


		public bool IsStale => COM.IsStale();

		public void Sort(in ReadOnlySpan<DXCoreAdapterPreference> preferences) {
			unsafe {
				fixed(DXCoreAdapterPreference* pPreferences = preferences) {
					COM.Sort((uint)preferences.Length, (nint)pPreferences);
				}
			}
		}

		public bool IsAdapterPreferenceSupported(DXCoreAdapterPreference preference) => COM.IsAdapterPreferenceSupported(preference);


		private readonly List<DXCoreAdapter?> adapters = new();

		public int Count { get; }

		public DXCoreAdapter this[int index] {
			get {
				var adapter = adapters[index];
				if (adapter == null) {
					adapter = new DXCoreAdapter(
						COMHelpers.GetObjectFromCOMGetter<IDXCoreAdapter>((in Guid riid) => COM.GetAdapter((uint)index, riid))!,
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

	public class DXCoreAdapter : IDisposable {

		public IDXCoreAdapter COM { get; }

		public DXCoreAdapterFactory Factory { get; }

		public DXCoreAdapter(IDXCoreAdapter com, DXCoreAdapterFactory? factory = null) {
			COM = com;
			Factory = factory ?? new DXCoreAdapterFactory(COMHelpers.GetObjectFromCOMGetter<IDXCoreAdapterFactory>(com.GetFactory)!);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			COM.Release();
		}


		public bool IsValid => COM.IsValid();

		public bool IsAttributeSupported(in Guid attributeGuid) => COM.IsAttributeSupported(attributeGuid);

		public bool IsPropertySupported(DXCoreAdapterProperty property) => COM.IsPropertySupported(property);

		public bool IsQueryStateSupported(DXCoreAdapterState state) => COM.IsQueryStateSupported(state);

		public O QueryState<I,O>(DXCoreAdapterState state, I? input = null) where I : unmanaged where O : unmanaged {
			unsafe {
				I vinput = input ?? default;
				I* pInput = input.HasValue ? &vinput : (I*)0;
				O output = default;
				COM.QueryState(state, (nuint)(input.HasValue ? Marshal.SizeOf<I>() : 0), (nint)pInput, (nuint)Marshal.SizeOf<O>(), (nint)(&output));
				return output;
			}
		}

		public bool IsSetStateSupported(DXCoreAdapterState state) => COM.IsSetStateSupported(state);

		public O SetState<I, O>(DXCoreAdapterState state, I? input = null) where I : unmanaged where O : unmanaged {
			unsafe {
				I vinput = input ?? default;
				I* pInput = input.HasValue ? &vinput : (I*)0;
				O output = default;
				COM.SetState(state, (nuint)(input.HasValue ? Marshal.SizeOf<I>() : 0), (nint)pInput, (nuint)Marshal.SizeOf<O>(), (nint)(&output));
				return output;
			}
		}

		public byte[] GetProperty(DXCoreAdapterProperty property) {
			byte[] data = new byte[(int)COM.GetPropertySize(property)];
			unsafe {
				fixed(byte* pData = data) {
					COM.GetProperty(property, (uint)data.Length, (nint)pData);
				}
			}
			return data;
		}

	}

}

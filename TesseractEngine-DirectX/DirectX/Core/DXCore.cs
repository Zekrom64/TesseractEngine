using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Windows;

namespace Tesseract.DirectX.Core {

	#region DXCore enums
	/// <summary>
	/// Defines constants that specify DXCore adapter properties. Pass one of these constants to the
	/// <see cref="IDXCoreAdapter.GetPropertySize(DXCoreAdapterProperty)">IDXCoreAdapter::GetPropertySize</see> method to
	/// retrieve the buffer size necessary to receive the value of the corresponding property; then pass the same constant
	/// to the <see cref="IDXCoreAdapter.GetProperty(DXCoreAdapterProperty, nuint, IntPtr)">IDXCoreAdapter::GetProperty</see>
	/// method to retrieve the property's value in a buffer that you allocate.
	/// </summary>
	public enum DXCoreAdapterProperty : uint {
		/// <summary>
		/// <para>
		/// Specifies the <i>InstanceLuid</i> adapter property, which contains a locally unique identifier representing the adapter.
		/// This value remains constant for the lifetime of this adapter. The LUID of an adapter changes on reboot, driver upgrade,
		/// or device disablement/enablement.
		/// </para>
		/// 
		/// <para>
		/// The <i>InstanceLuid</i> adapter property has type <see cref="Tesseract.Windows.LUID">LUID</see>.
		/// </para>
		/// </summary>
		InstanceLuid = 0,
		/// <summary>
		/// <para>
		/// Specifies the <i>DriverVersion</i> adapter property, which contains the driver version, represented in <b>WORD</b>s as
		/// a <see cref="LARGE_INTEGER">LARGE_INTEGER</see>.
		/// </para>
		/// 
		/// <para>
		/// The DriverVersion adapter property has type <b>uint64_t</b>, representing a Boolean value.
		/// </para>
		/// </summary>
		DriverVersion,
		/// <summary>
		/// <para>
		/// Specifies the <i>DriverDescription</i> adapter property, which contains a NULL-terminated array of <b>CHAR</b>s
		/// describing the driver, as specified by the driver, in UTF-8 encoding.
		/// </para>
		/// 
		/// <para>
		/// The <i>DriverDescription</i> adapter property has type <b>char*</b>.
		/// </para>
		/// </summary>
		DriverDescription,
		/// <summary>
		/// <para>
		/// Specifies the <i>HardwareID</i> adapter property, which represents the PnP hardware ID parts.
		/// </para>
		/// 
		/// <para>
		/// The <i>HardwareID</i> adapter property has type <see cref="DXCoreHardwareID">DXCoreHardwareID</see>.
		/// </para>
		/// </summary>
		HardwareID,
		/// <summary>
		/// <para>
		/// Specifies the <i>KmdModelVersion</i> adapter property, which represents the driver model.
		/// </para>
		/// 
		/// <para>
		/// The <i>KmdModelVersion</i> adapter property has type <see cref="Direct3D.D3DKMTDriverVersion">D3DKMT_DRIVERVERSION</see>.
		/// </para>
		/// </summary>
		KmdModelVersion,
		/// <summary>
		/// <para>
		/// Specifies the <i>ComputePreemptionGranularity</i> adapter property, which represents the compute preemption granularity.
		/// </para>
		/// 
		/// <para>
		/// The <i>ComputePreemptionGranularity</i> adapter property has type <b>uint16_t</b>, representing a
		/// <see cref="Direct3D.D3DKMDTComputePreemptionGranularity">D3DKMDT_COMPUTE_PREEMPTION_GRANULARITY</see> value.
		/// </para>
		/// </summary>
		ComputePreemptionGranularity,
		/// <summary>
		/// <para>
		/// Specifies the <i>GraphicsPreemptionGranularity</i> adapter property, which represents the graphics preemption granularity.
		/// </para>
		/// 
		/// <para>
		/// The <i>GraphicsPreemptionGranularity</i> adapter property has type <b>uint16_t</b>, representing a
		/// <see cref="Direct3D.D3DKMDTGraphicsPreemptionGranularity">D3DKMDT_GRAPHICS_PREEMPTION_GRANULARITY</see> value.
		/// </para>
		/// </summary>
		GraphicsPreemptionGranularity,
		/// <summary>
		/// <para>
		/// Specifies <i>the DedicatedAdapterMemory</i> adapter property, which represents the number of bytes of dedicated
		/// adapter memory that are not shared with the CPU.
		/// </para>
		/// 
		/// <para>
		/// The <i>DedicatedVideoMemory</i> adapter property has type <b>uint64_t</b>.
		/// </para>
		/// </summary>
		DedicatedAdapterMemory,
		/// <summary>
		/// <para>
		/// Specifies the <i>DedicatedSystemMemory</i> adapter property, which represents the number of bytes of dedicated
		/// system memory that are not shared with the CPU. This memory is allocated from available system memory at boot time.
		/// </para>
		/// 
		/// <para>
		/// The <i>DedicatedSystemMemory</i> adapter property has type <b>uint64_t</b>.
		/// </para>
		/// </summary>
		DedicatedSystemMemory,
		/// <summary>
		/// <para>
		/// Specifies the <i>SharedSystemMemory</i> adapter property, which represents the number of bytes of shared system memory.
		/// This is the maximum value of system memory that may be consumed by the adapter during operation. Any incidental memory
		/// consumed by the driver as it manages and uses video memory is additional.
		/// </para>
		/// 
		/// <para>
		/// The <i>SharedSystemMemory</i> adapter property has type <b>uint64_t</b>.
		/// </para>
		/// </summary>
		SharedSystemMemory,
		/// <summary>
		/// <para>
		/// Specifies the <i>AcgCompatible</i> adapter property, which indicates whether the adapter is compatible with processes
		/// that enforce Arbitrary Code Guard.
		/// </para>
		/// 
		/// <para>
		/// The <i>AcgCompatible</i> adapter property has type <b>bool</b>.
		/// </para>
		/// </summary>
		AcgCompatible,
		/// <summary>
		/// <para>
		/// Specifies the <i>IsHardware</i> adapter property, which determines whether or not this is a hardware adapter. An adapter
		/// that's not a hardware adapter is a software adapter.
		/// </para>
		/// 
		/// <para>
		/// The <i>IsHardware</i> adapter property has type <b>bool</b>.
		/// </para>
		/// </summary>
		IsHardware,
		/// <summary>
		/// <para>
		/// Specifies the <i>IsIntegrated</i> adapter property, which determines whether the adapter is reported to be an integrated graphics processor (iGPU).
		/// </para>
		/// 
		/// <para>
		/// The <i>IsIntegrated</i> adapter property has type <b>bool</b>.
		/// </para>
		/// </summary>
		IsIntegrated,
		/// <summary>
		/// <para>
		/// Specifies the <i>IsDetachable</i> adapter property, which determines whether the adapter has been reported to be detachable, or removable.
		/// </para>
		/// 
		/// <para>
		/// The <i>IsDetachable</i> adapter property has type <b>bool</b>.
		/// </para>
		/// 
		/// <para>
		/// <b>Note.</b> Even if <see cref="IDXCoreAdapter.GetProperty{T}(DXCoreAdapterProperty)">IDXCoreAdapter::GetProperty</see>
		/// indicates <c>false</c> for this property, the adapter still has the ability to be reported as removed, such as in the
		/// case of malfunction, or driver update.
		/// </para>
		/// </summary>
		IsDetachable,
		HardwareIDParts
	}

	/// <summary>
	/// Defines constants that specify kinds of DXCore adapter states. Pass one of these constants to the 
	/// <see cref="IDXCoreAdapter.QueryState{TIn, TOut}(DXCoreAdapterState, TIn?)">IDXCoreAdapter::QueryState</see> method to
	/// retrieve the adapter state item for a state kind; pass a constant to the
	/// <see cref="IDXCoreAdapter.SetState{TIn, TOut}(DXCoreAdapterState, TIn)">IDXCoreAdapter::SetState</see> method to set
	/// an adapter's info for a state item.
	/// </summary>
	public enum DXCoreAdapterState : uint {
		/// <summary>
		/// <para>
		/// Specifies the <i>IsDriverUpdateInProgress</i> adapter state, which when true indicates that a driver update has been
		/// initiated on the adapter but it has not yet completed. If the driver update has already completed, then the adapter
		/// will have been invalidated, and your <see cref="IDXCoreAdapter.QueryState{TIn, TOut}(DXCoreAdapterState, TIn?)">QueryState</see>
		/// call will return a <b>HRESULT</b> of <b>DXGI_ERROR_DEVICE_REMOVED</b>.
		/// </para>
		/// 
		/// <para>
		/// When calling <see cref="IDXCoreAdapter.QueryState{TIn, TOut}(DXCoreAdapterState, TIn?)">QueryState</see>, the
		/// <i>IsDriverUpdateInProgress</i> state item has type <b>uint8_t</b>, representing a Boolean value.
		/// </para>
		/// 
		/// <para>
		/// <b>Important.</b> This state item is not supported for <see cref="IDXCoreAdapter.SetState{TIn, TOut}(DXCoreAdapterState, TIn)">SetState</see>.
		/// </para>
		/// </summary>
		IsDriverUpdateInProgress = 0,
		/// <summary>
		/// <para>
		/// Specifies the <i>AdapterMemoryBudget</i> adapter state, which retrieves or requests the adapter memory budget on the adapter.
		/// </para>
		/// 
		/// <para>
		/// When calling QueryState, the <i>AdapterMemoryBudget</i> adapter state has type <see cref="DXCoreAdapterMemoryBudgetNodeSegmentGroup"/> for
		/// <i>inputStateDetails</i>, and type <see cref="DXCoreAdapterMemoryBudget"/> for <i>outputBuffer</i>.
		/// </para>
		/// 
		/// <para>
		/// <b>Important.</b> This state item is not supported for <see cref="IDXCoreAdapter.SetState{TIn, TOut}(DXCoreAdapterState, TIn)">SetState</see>.
		/// </para>
		/// </summary>
		AdapterMemoryBudget = 1
	}

	/// <summary>
	/// Defines constants that specify an adapter's memory segment grouping.
	/// </summary>
	public enum DXCoreSegmentGroup : uint {
		/// <summary>
		/// Specifies a grouping of segments that is considered local to the adapter, and represents the fastest memory available
		/// to the GPU. Your application should target the local segment group as the target size for its working set.
		/// </summary>
		Local = 0,
		/// <summary>
		/// Specifies a grouping of segments that is considered non-local to the adapter, and may have slower performance than
		/// the local segment group.
		/// </summary>
		NonLocal = 1
	}

	/// <summary>
	/// <para>
	/// Defines constants that specify types of notifications raised by <see cref="IDXCoreAdapter"/> or <see cref="IDXCoreAdapterList"/> objects.
	/// </para>
	/// 
	/// <para>
	/// You can register and unregister for these notifications by calling
	/// <see cref="IDXCoreAdapterFactory.RegisterEventNotification(object, DXCoreNotificationType, DXCoreNotificationCallback, IntPtr)">IDXCoreAdapterFactory::RegisterEventNotification</see>
	/// and
	/// <see cref="IDXCoreAdapterFactory.UnregisterEventNotification(uint)">IDXCoreAdapterFactory::UnregisterEventNotification</see>, respectively.
	/// </para>
	/// </summary>
	public enum DXCoreNotificationType : uint {
		/// <summary>
		/// This notification is raised by an <see cref="IDXCoreAdapterList"/> object when the adapter list becomes stale. The
		/// fresh-to-stale transition is one-way, and one-time, so this notification is raised at most one time.
		/// </summary>
		AdapterListStale = 0,
		/// <summary>
		/// This notification is raised by an <see cref="IDXCoreAdapter"/> object when the adapter becomes no longer valid. The
		/// valid-to-invalid transition is one-way, and one-time, so this notification is raised at most one time.
		/// </summary>
		AdapterNoLongerValid = 1,

		// TODO: Documentation links for DXGI/D3D11

		/// <summary>
		/// This notification is raised by an <see cref="IDXCoreAdapter"/> object when an adapter budget change occurs. This
		/// notification can occur many times. Using this notification is functionally similar to IDXGIAdapter3::RegisterVideoMemoryBudgetChangeNotificationEvent.
		/// In response to this event, you should call <see cref="IDXCoreAdapter.QueryState{TIn, TOut}(DXCoreAdapterState, TIn?)">IDXCoreAdapter::QueryState</see>
		/// (with <see cref="DXCoreAdapterState.AdapterMemoryBudget"/>) to evaluate the current memory budget state.
		/// </summary>
		AdapterBudgetChange = 2,
		/// <summary>
		/// This notification is raised by an <see cref="IDXCoreAdapter"/> object to notify of an adapter's hardware content
		/// protection teardown. This notification can occur many times. It is functionally similar to IDXGIAdapter3::RegisterHardwareContentProtectionTeardownStatusEvent.
		/// In response to this event, you should re-evaluate the current crypto session status; for example, by calling
		/// ID3D11VideoContext1::CheckCryptoSessionStatus to determine the impact of the hardware teardown for a specific
		/// ID3D11CryptoSession interface.
		/// </summary>
		AdapterContentProtectionTeardown = 3
	}

	/// <summary>
	/// Defines constants that specify DXCore adapter preferences to be used as list-sorting criteria. You can sort a
	/// DXCore adapter list by passing an array of <b>DXCoreAdapterPreference</b> to
	/// <see cref="IDXCoreAdapterList.Sort(in ReadOnlySpan{DXCoreAdapterPreference})">IDXCoreAdapterList::Sort</see>.
	/// </summary>
	public enum DXCoreAdapterPreference : uint {
		/// <summary>
		/// Specifies a preference for hardware adapters (as opposed to software adapters).
		/// </summary>
		Hardware = 0,
		/// <summary>
		/// Specifies a preference for the minimum-powered GPU (such as an integrated graphics processor, or iGPU).
		/// </summary>
		MinimumPower = 1,
		/// <summary>
		/// Specifies a preference for the highest-performance GPU, such as an external graphics processor (xGPU), if available, or discrete graphics processor (dGPU) if available.
		/// </summary>
		HighPerformance = 2,
	}
	#endregion

	#region DXCore structures
	/// <summary>
	/// Represents the PnP hardware ID parts for an adapter.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DXCoreHardwareID {

		public uint VendorID;
		public uint DeviceID;
		public uint SubSysID;
		public uint Revision;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXCoreHardwareIDParts {

		public uint VendorID;
		public uint DeviceID;
		public uint SubSysID;
		public uint SubVendorID;
		public uint Revision;

	}

	/// <summary>
	/// Describes a memory segment group for an adapter.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DXCoreAdapterMemoryBudgetNodeSegmentGroup {

		/// <summary>
		/// Specifies the device's physical adapter for which the adapter memory information is queried.
		/// For single-adapter operation, set this to zero. If there are multiple adapter nodes, then set
		/// this to the index of the node (the device's physical adapter) for which you want to query for
		/// adapter memory information (see <see href="https://docs.microsoft.com/en-us/windows/win32/direct3d12/multi-engine">Multi-adapter</see>).
		/// </summary>
		public uint NodeIndex;
		/// <summary>
		/// Specifies the adapter memory segment grouping that you want to query about.
		/// </summary>
		public DXCoreSegmentGroup SegmentGroup;

	}

	/// <summary>
	/// Describes the memory budget for an adapter.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DXCoreAdapterMemoryBudget {

		/// <summary>
		/// Specifies the OS-provided adapter memory budget, in bytes, that your application should target. If <i>currentUsage</i>
		/// is greater than <i>budget</i>, then your application may incur stuttering or performance penalties due to background
		/// activity by the OS, which is intended to provide other applications with a fair usage of adapter memory.
		/// </summary>
		public ulong Budget;
		public ulong CurrentUsage;
		/// <summary>
		/// Specifies the amount of adapter memory, in bytes, that your application has available for reservation. To reserve this
		/// adapter memory, your application should call <see cref="IDXCoreAdapter.SetState{TIn, TOut}(DXCoreAdapterState, TIn)">IDXCoreAdapter::SetState</see>
		/// with state set to <see cref="DXCoreAdapterState.AdapterMemoryBudget"/>.
		/// </summary>
		public ulong AvailableForReservation;
		public ulong CurrentReservation;

	}
	#endregion

	#region DXCore functions
	/// <summary>
	/// A callback function (implemented by your application), which is called by a DXCore object for notification events.
	/// </summary>
	/// 
	/// <param name="notificationType">The type of notification representing this invocation. See the table in <see cref="DXCoreNotificationType"/> for info about what types are valid with which kinds of objects.</param>
	/// <param name="obj">The <see cref="IDXCoreAdapter"/> or <see cref="IDXCoreAdapterList"/> object raising the notification.</param>
	/// <param name="context">A pointer, which may be <c>nullptr</c>, to an object containing context info.</param>
	public delegate void DXCoreNotificationCallback(DXCoreNotificationType notificationType, [MarshalAs(UnmanagedType.IUnknown)] object obj, IntPtr context);
	#endregion

	#region DXCore interfaces
	/// <summary>
	/// <para>
	/// The <b>IDXCoreAdapter</b> interface implements methods for retrieving details about an adapter item. <b>IDXCoreAdapter</b>
	/// inherits from the <see cref="IUnknown"/> interface. For programming guidance, and code examples, see
	/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
	/// </para>
	/// </summary>
	[ComImport, Guid("f0db4c7f-fe5a-42a2-bd62-f2a6cf6fc83e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXCoreAdapter : IUnknown {

		/// <summary>
		///  Specifies an adapter that supports being used with the <see href="https://docs.microsoft.com/en-us/windows/win32/direct3d11">Direct3D 11</see>
		///  graphics APIs. No guarantees are made about specific features, nor is a guarantee made that the OS in its current
		///  configuration supports these APIs.
		/// </summary>
		public static readonly Guid AttributeD3D11Graphics = new(0x8c47866b, 0x7583, 0x450d, 0xf0, 0xf0, 0x6b, 0xad, 0xa8, 0x95, 0xaf, 0x4b);
		/// <summary>
		/// Specifies an adapter that supports being used with the <see href="https://docs.microsoft.com/en-us/windows/win32/direct3d12">Direct3D 12</see>
		/// graphics APIs. No guarantees are made about specific features, nor is a guarantee made that the OS in its current
		/// configuration supports these APIs.
		/// </summary>
		public static readonly Guid AttributeD3D12Graphics = new(0x0c9ece4d, 0x2f6e, 0x4f01, 0x8c, 0x96, 0xe8, 0x9e, 0x33, 0x1b, 0x47, 0xb1);
		/// <summary>
		/// Specifies an adapter that supports being used with the <see href="https://docs.microsoft.com/en-us/windows/win32/direct3d12/core-feature-levels">Direct3D 12 Core</see>
		/// compute APIs. No guarantees are made about specific features, nor is a guarantee made that the OS in its current
		/// configuration supports these APIs.
		/// </summary>
		public static readonly Guid AttributeD3D12CoreCompute = new(0x248e2800, 0xa793, 0x4724, 0xab, 0xaa, 0x23, 0xa6, 0xde, 0x1b, 0xe0, 0x90);

		/// <summary>
		/// Determines whether this DXCore adapter object is still valid. For programming guidance, and code examples, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </summary>
		/// 
		/// <returns>Returns <c>true</c> if this DXCore adapter object is still valid. Otherwise, returns <c>false</c>.</returns>
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U1)]
		public bool IsValid();

		/// <summary>
		/// Determines whether this DXCore adapter object supports the specified adapter attribute.
		/// </summary>
		/// 
		/// <param name="attributeGuid">A reference to an adapter attribute GUID. For a list of attribute GUIDs, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-adapter-attribute-guids">DXCore adapter attribute GUIDs</see>.</param>
		/// <returns>Returns <c>true</c> if this DXCore adapter object supports the specified adapter attribute. Otherwise, returns <c>false</c>.</returns>
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U1)]
		public bool IsAttributeSupported(in Guid attributeGuid);

		/// <summary>
		/// Determines whether this DXCore adapter object and the current operating system (OS) support the specified adapter property.
		/// </summary>
		/// 
		/// <param name="property">The type of property that you're querying about support for. See the table in <see cref="DXCoreAdapterProperty"/> for more info about each adapter property.</param>
		/// <returns>Returns <c>true</c> if this DXCore adapter object and the current operating system (OS) support the specified adapter property. Otherwise, returns <c>false</c>.</returns>
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U1)]
		public bool IsPropertySupported(DXCoreAdapterProperty property);

		/// <summary>
		/// <para>
		/// Retrieves the value of the specified adapter property. Before calling <b>GetProperty</b> for a property type, call
		/// <see cref="IsPropertySupported(DXCoreAdapterProperty)">IsPropertySupported</see> to confirm that the property type
		/// is available for this adapter and operating system (OS). Also before calling <b>GetProperty</b>, call
		/// <see cref="GetPropertySize(DXCoreAdapterProperty)">GetPropertySize</see> to determine the necessary size of the buffer
		/// in which to receive the property value.
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// You can call <b>GetProperty</b> on an adapter that's no longer valid—the function won't fail as a result of that. This
		/// function zeros out the <i>propertyData</i> buffer prior to filling it in.
		/// </para>
		/// </remarks>
		/// 
		/// <param name="property">The type of the property whose value you wish to retrieve. See the table in <see cref="DXCoreAdapterProperty"/> for more info about each adapter property.</param>
		/// <param name="bufferSize">The size, in bytes, of the output buffer that you allocate and provide in <i>propertyData</i>.</param>
		/// <param name="propertyData">
		/// A pointer to an output buffer that you allocate in your application, and that the function fills in. Call
		/// <see cref="GetPropertySize(DXCoreAdapterProperty)">GetPropertySize</see> to determine the size that the
		/// <i>propertyData</i> buffer should be for a given adapter property.
		/// </param>
		public void GetProperty(DXCoreAdapterProperty property, nuint bufferSize, IntPtr propertyData);

		/*
		/// <summary>
		/// Template wrapper function for <see cref="GetProperty(DXCoreAdapterProperty, nuint, IntPtr)"/> to simplify retrieval of unmanaged types.
		/// </summary>
		/// 
		/// <typeparam name="T">The unmanaged property type</typeparam>
		/// <param name="property">The property to pass to <see cref="GetProperty(DXCoreAdapterProperty, nuint, IntPtr)"/></param>
		/// <returns>The resulting property value retrieved from <see cref="GetProperty(DXCoreAdapterProperty, nuint, IntPtr)"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T GetProperty<T>(DXCoreAdapterProperty property) where T : unmanaged {
			unsafe {
				T val = default;
				GetProperty(property, (nuint)sizeof(T), (IntPtr)(&val));
				return val;
			}
		}
		*/

		/// <summary>
		/// <para>
		/// For a specified adapter property, retrieves the size of buffer, in bytes, that is required for a call to
		/// <see cref="GetProperty(DXCoreAdapterProperty, nuint, IntPtr)">GetProperty</see>. Before calling <b>GetPropertySize</b>
		/// for a property type, call <see cref="IsPropertySupported(DXCoreAdapterProperty)">IsPropertySupported</see> to confirm
		/// that the property type is available for this adapter and operating system (OS).
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// You can call <b>GetPropertySize</b> on an adapter that's no longer valid—the function won't fail.
		/// </para>
		/// </remarks>
		/// 
		/// <param name="property">The type of the property whose size, in bytes, you wish to retrieve.</param>
		/// <returns>
		/// The size, in bytes, of the output buffer that you should allocate and pass as the <i>propertyData</i> argument in your
		/// call to <see cref="GetProperty(DXCoreAdapterProperty, nuint, IntPtr)">GetProperty</see>.
		/// </returns>
		public nuint GetPropertySize(DXCoreAdapterProperty property);

		/// <summary>
		/// Determines whether this DXCore adapter object and the current operating system (OS) support querying the value of the specified adapter state.
		/// </summary>
		/// 
		/// <param name="state">The kind of state item that you're querying about support for. See the table in <see cref="DXCoreAdapterState"/> for more info about each adapter state kind.</param>
		/// <returns>Returns <c>true</c> if this DXCore adapter object and the current operating system (OS) support querying the specified adapter state. Otherwise, returns <c>false</c>.</returns>
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U1)]
		public bool IsQueryStateSupported(DXCoreAdapterState state);

		/// <summary>
		/// <para>
		/// Retrieves the current state of the specified item on the adapter. Before calling <b>QueryState</b> for a property type,
		/// call <see cref="IsQueryStateSupported(DXCoreAdapterState)">IsQueryStateSupported</see> to confirm that querying the state
		/// kind is available for this adapter and operating system (OS).
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// See <see cref="DXCoreAdapterState"/> for more info about each adapter state kind, and what inputs and outputs are used.
		/// This function zeros out the <i>outputBuffer</i> buffer prior to filling it in.
		/// </para>
		/// </remarks>
		/// 
		/// <param name="state">The kind of state item on the adapter whose state you wish to retrieve. See the table in <see cref="DXCoreAdapterState"/> for more info about each adapter state kind.</param>
		/// <param name="inputStateDetailsSize">The size, in bytes, of the input state details buffer that you (optionally) allocate and provide in <i>inputStateDetails</i>.</param>
		/// <param name="inputStateDetails">
		/// An optional pointer to a constant input state details buffer that you allocate in your application, containing any
		/// information about your request that's required for the state kind you specify in <i>state</i>. See the table in
		/// <see cref="DXCoreAdapterState"/> for more info about any input buffer requirement for a given state kind.
		/// </param>
		/// <param name="outputBufferSize">The size, in bytes, of the output buffer that you allocate and provide in <i>outputBuffer</i>.</param>
		/// <param name="outputBuffer">
		/// A pointer to an output buffer that you allocate in your application, and that the function fills in. See the table in
		/// <see cref="DXCoreAdapterState"/> for more info about the output buffer requirement for a given state kind.
		/// </param>
		public void QueryState(DXCoreAdapterState state, nuint inputStateDetailsSize, IntPtr inputStateDetails, nuint outputBufferSize, IntPtr outputBuffer);

		/*
		/// <summary>
		/// Template wrapper function for <see cref="QueryState(DXCoreAdapterState, nuint, IntPtr, nuint, IntPtr)"/> to simplify querying of unmanaged types.
		/// </summary>
		/// 
		/// <typeparam name="TIn">The input state unmanaged type</typeparam>
		/// <typeparam name="TOut">The output state unmanaged type</typeparam>
		/// <param name="state">The state to pass to <see cref="QueryState(DXCoreAdapterState, nuint, IntPtr, nuint, IntPtr)"/></param>
		/// <param name="inputStateDetails">The input state details value, or null if no input details are passed</param>
		/// <returns>The resulting output state retrieved from <see cref="QueryState(DXCoreAdapterState, nuint, IntPtr, nuint, IntPtr)"/></returns>
		public TOut QueryState<TIn, TOut>(DXCoreAdapterState state, TIn? inputStateDetails = null) where TIn : unmanaged where TOut : unmanaged {
			unsafe {
				IntPtr pIsd = IntPtr.Zero;
				if (inputStateDetails.HasValue) {
					TIn isd = inputStateDetails.Value;
					pIsd = (IntPtr)(&isd);
				}
				TOut ob = default;
				QueryState(state, (nuint)sizeof(TIn), pIsd, (nuint)sizeof(TOut), (IntPtr)(&ob));
				return ob;
			}
		}
		*/

		/// <summary>
		/// Determines whether this DXCore adapter object and the current operating system (OS) support setting the value of the specified adapter state.
		/// </summary>
		/// 
		/// <param name="state">The kind of state item that you're querying about support for. See the table in <see cref="DXCoreAdapterState"/> for more info about each adapter state kind.</param>
		/// <returns>Returns <c>true</c> if this DXCore adapter object and the current operating system (OS) support setting the specified adapter state. Otherwise, returns <c>false</c>.</returns>
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U1)]
		public bool IsSetStateSupported(DXCoreAdapterState state);

		/// <summary>
		/// Sets the state of the specified item on the adapter. Before calling SetState for a property type, call
		/// <see cref="IsSetStateSupported(DXCoreAdapterState)">IsSetStateSupported</see> to confirm that setting the state kind is
		/// available for this adapter and operating system (OS).
		/// </summary>
		/// 
		/// <param name="state">The kind of state item on the adapter whose state you wish to set. See the table in <see cref="DXCoreAdapterState"/> for more info about each adapter state kind.</param>
		/// <param name="inputStateDetailsSize">The size, in bytes, of the input state details buffer that you (optionally) allocate and provide in <i>inputStateDetails</i>.</param>
		/// <param name="inputStateDetails">
		/// An optional pointer to a constant input state details buffer that you allocate in your application, containing any
		/// information about your request that's required for the state kind you specify in <i>state</i>. See the table in
		/// <see cref="DXCoreAdapterState"/> for more info about any input buffer requirement for a given state kind.
		/// </param>
		/// <param name="outputBufferSize">The size, in bytes, of the input buffer that you allocate and provide in <i>inputData</i>.</param>
		/// <param name="outputBuffer">
		/// A pointer to an input buffer that you allocate in your application, containing the state information to set for the state
		/// item whose kind you specify in <i>state</i>. See the table in <see cref="DXCoreAdapterState"/> for more info about the
		/// input buffer requirement for a given state kind.
		/// </param>
		public void SetState(DXCoreAdapterState state, nuint inputStateDetailsSize, IntPtr inputStateDetails, nuint outputBufferSize, IntPtr outputBuffer);

		/*
		/// <summary>
		/// Template wrapper function for <see cref="QueryState(DXCoreAdapterState, nuint, IntPtr, nuint, IntPtr)"/> to simplify setting of unmanaged types.
		/// </summary>
		/// 
		/// <typeparam name="TIn">Input state type</typeparam>
		/// <typeparam name="TOut">Output data size</typeparam>
		/// <param name="state">The state to pass to <see cref="SetState(DXCoreAdapterState, nuint, IntPtr, nuint, IntPtr)"/></param>
		/// <param name="inputStateDetails">The input state to pass to <see cref="SetState(DXCoreAdapterState, nuint, IntPtr, nuint, IntPtr)"/></param>
		/// <returns>The output data returned from <see cref="SetState(DXCoreAdapterState, nuint, IntPtr, nuint, IntPtr)"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TOut SetState<TIn, TOut>(DXCoreAdapterState state, TIn inputStateDetails) where TIn : unmanaged where TOut : unmanaged {
			unsafe {
				TOut ob = default;
				SetState(state, (nuint)sizeof(TIn), (IntPtr)(&inputStateDetails), (nuint)sizeof(TOut), (IntPtr)(&ob));
				return ob;
			}
		}
		*/

		/// <summary>
		/// <para>
		/// Retrieves an <see cref="IDXCoreAdapterFactory"/> interface pointer to the DXCore adapter factory object. For programming
		/// guidance, and code examples, see <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </para>
		/// </summary>
		/// 
		/// <param name="riid">
		/// A reference to the globally unique identifier (GUID) of the interface that you wish to be returned.
		/// This is expected to be the interface identifier (IID) of <see cref="IDXCoreAdapterFactory"/>.
		/// </param>
		/// <returns>
		/// A pointer to the existing DXCore adapter factory object. Before returning, the function increments the reference count
		/// on the factory object's <see cref="IDXCoreAdapterFactory"/> interface.
		/// </returns>
		public IntPtr GetFactory(in Guid riid);

		/*
		/// <summary>
		/// A helper function template that infers an interface identifier, and calls <see cref="GetFactory(in Guid)"/>.
		/// </summary>
		/// 
		/// <typeparam name="T">The COM interface type</typeparam>
		/// <returns>The returned value from <see cref="GetFactory(in Guid)"/>, converted to the given COM type</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T GetFactory<T>() => COMHelpers.GetObjectFromCOMGetter<T>(GetFactory);
		*/

	}

	/// <summary>
	/// The <b>IDXCoreAdapterList</b> interface implements methods for retrieving adapter items from a generated list, as
	/// well as details about the list. <b>IDXCoreAdapterList</b> inherits from the <see cref="IUnknown"/> interface. For
	/// programming guidance, and code examples, see
	/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
	/// </summary>
	[ComImport, Guid("526c7776-40e9-459b-b711-f32ad76dfc28")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXCoreAdapterList : IUnknown {

		/// <summary>
		/// <para>
		/// Retrieves a specific adapter by index from a DXCore adapter list object. For programming guidance, and code examples, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// Multiple calls passing an index that represents the same adapter return identical interface pointers, even across
		/// different adapter lists. As a result, it's safe to compare interface pointers to determine whether multiple pointers
		/// refer to the same adapter object.
		/// </para>
		/// </remarks>
		/// 
		/// <param name="index">A zero-based index, identifying an adapter instance within the DXCore adapter list.</param>
		/// <param name="riid">A reference to the globally unique identifier (GUID) of the interface that you wish to be returned. This is expected to be the interface identifier (IID) of <see cref="IDXCoreAdapter"/>.</param>
		public IntPtr GetAdapter(uint index, in Guid riid);

		/*
		/// <summary>
		/// A helper function template that infers an interface identifier, and calls <see cref="GetAdapter(uint, in Guid)"/>.
		/// </summary>
		/// 
		/// <typeparam name="T">The COM interface type</typeparam>
		/// <param name="index">The index to pass to <see cref="GetAdapter(uint, in Guid)"/></param>
		/// <returns>The returned value from <see cref="GetAdapter(uint, in Guid)"/>, converted to the given COM type</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T GetAdapter<T>(uint index) => COMHelpers.GetObjectFromCOMGetter<T>((in Guid riid) => GetAdapter(index, riid));
		*/

		/// <summary>
		/// Retrieves the number of adapters in a DXCore adapter list object. For programming guidance, and code examples, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </summary>
		/// 
		/// <returns>Returns the number of adapter items in the list.</returns>
		[PreserveSig]
		public uint GetAdapterCount();

		/// <summary>
		/// <para>
		/// Determines whether changes to this system have resulted in this DXCore adapter list object becoming out of date. For
		/// programming guidance, and code examples, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// You can poll <b>IsStale</b> to determine whether changing system conditions have led to this list becoming out of date. If
		/// <b>IsStale</b> returns <c>true</c> once, then it returns <c>true</c> for the remaining lifetime of the DXCore adapter
		/// list object. A stale list object is still considered stale even if system conditions return to a state that matches the
		/// list (the same conditions obtain now as did when the list was first generated).
		/// </para>
		/// </remarks>
		/// 
		/// <returns>
		/// Returns <c>true</c> if, since generating the list, changes to system conditions have occurred that would cause this
		/// adapter list to become stale. Otherwise, returns <c>false</c>.
		/// </returns>
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U1)]
		public bool IsStale();

		/// <summary>
		/// <para>
		/// Retrieves an <see cref="IDXCoreAdapterFactory"/> interface pointer to the DXCore adapter factory object. For programming
		/// guidance, and code examples, see <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </para>
		/// </summary>
		/// 
		/// <param name="riid">
		/// A reference to the globally unique identifier (GUID) of the interface that you wish to be returned.
		/// This is expected to be the interface identifier (IID) of <see cref="IDXCoreAdapterFactory"/>.
		/// </param>
		/// <returns>
		/// A pointer to the existing DXCore adapter factory object. Before returning, the function increments the reference count
		/// on the factory object's <see cref="IDXCoreAdapterFactory"/> interface.
		/// </returns>
		public IntPtr GetFactory(in Guid riid);

		/*
		/// <summary>
		/// A helper function template that infers an interface identifier, and calls <see cref="GetFactory(in Guid)"/>.
		/// </summary>
		/// 
		/// <typeparam name="T">The COM interface type</typeparam>
		/// <returns>The returned value from <see cref="GetFactory(in Guid)"/>, converted to the given COM type</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T GetFactory<T>() => COMHelpers.GetObjectFromCOMGetter<T>(GetFactory);
		*/

		/// <summary>
		/// <para>
		/// Sorts a DXCore adapter list object based on a provided input array of sort criteria, where array items earlier in the
		/// array of criteria are given a higher weight. <b>Sort</b> helps you to more easily find your ideal adapter in an adapter list.
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// In cases where a provided <see cref="DXCoreAdapterPreference"/> value isn't recognized by the operating system (OS), it
		/// is ignored, and won't cause the API to fail. Known <b>DXCoreAdapterPreference</b> values will still be considered in this
		/// case. To determine whether a sort type is understood by the API, call 
		/// <see cref="IsAdapterPreferenceSupported(DXCoreAdapterPreference)">IDXCoreAdapterList::IsAdapterPreferenceSupported</see>.
		/// </para>
		/// 
		/// <para>
		/// <b>DXCoreAdapterPreference</b> values that occur earlier in the provided <i>preferences</i> array are treated with higher priority.
		/// </para>
		/// 
		/// <para>
		/// Refer to the <b>DXCoreAdapterPreference</b> enumeration documentation for details about what logic is applied for each
		/// type. The internal logic for a type may develop as the OS develops.
		/// </para>
		/// 
		/// <para>
		/// When <b>Sort</b> returns, items in the DXCore adapter list will have been sorted from most preferable to least
		/// preferable. So, calling <see cref="GetAdapter{T}(uint)">IDXCoreAdapterList::GetAdapter</see> with index 0 retrieves
		/// the adapter that best matches the requested sort preference types; index 1 is the next best match, and so on.
		/// </para>
		/// </remarks>
		/// 
		/// <param name="numPreferences">The number of elements that are in the array pointed to by the <i>preferences</i> parameter.</param>
		/// <param name="preferences">A pointer to a constant array of <see cref="DXCoreAdapterPreference"/> values, representing sort criteria.</param>
		public void Sort(uint numPreferences, [NativeType("const DXCoreAdapterPreference*")] IntPtr preferences);

		/*
		/// <summary>
		/// A helper function that invokes <see cref="Sort(uint, IntPtr)"/> using a span of preference values.
		/// </summary>
		/// 
		/// <param name="preferences">Preferences to pass to <see cref="Sort(uint, IntPtr)"/></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Sort(in ReadOnlySpan<DXCoreAdapterPreference> preferences) {
			unsafe {
				fixed (DXCoreAdapterPreference* ptr = preferences) {
					Sort((uint)preferences.Length, (IntPtr)ptr);
				}
			}
		}
		*/

		/// <summary>
		/// Determines whether a specified <see cref="DXCoreAdapterPreference"/> value is understood by the current operating system
		/// (OS). You can call <b>IsAdapterPreferenceSupported</b> before calling <see cref="Sort(in ReadOnlySpan{DXCoreAdapterPreference})">IDXCoreAdapterList::Sort</see>.
		/// </summary>
		/// 
		/// <param name="preference">A <see cref="DXCoreAdapterPreference"/> value that will be checked to see whether it's supported by the OS.</param>
		/// <returns>Returns <c>true</c> if the sort type is understood by the current OS. Otherwise, returns <c>false</c>.</returns>
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U1)]
		public bool IsAdapterPreferenceSupported(DXCoreAdapterPreference preference);

	}

	/// <summary>
	/// The <b>IDXCoreAdapterFactory</b> interface implements methods for generating DXCore adapter enumeration objects, and for
	/// retrieving their details. <b>IDXCoreAdapterFactory</b> inherits from the <see cref="IUnknown"/> interface. For programming
	/// guidance, and code examples, see
	/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
	/// </summary>
	[ComImport, Guid("78ee5945-c36e-4b13-a669-005dd11c0f06")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDXCoreAdapterFactory : IUnknown {

		/// <summary>
		/// <para>
		/// Generates a list of adapter objects representing the current adapter state of the system, and meeting the criteria
		/// specified. For programming guidance, and code examples, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// Even if no adapters are found, as long as the arguments are valid, <b>CreateAdapterList</b> creates a valid
		/// <see cref="IDXCoreAdapterList"/> object, and returns <b>S_OK</b>. Once generated, the adapters in this specific list
		/// won't change. But the list will be considered stale if one of the adapters later becomes invalid, or if a new adapter
		/// arrives that meets the provided filter criteria. The list returned by <b>CreateAdapterList</b> is not ordered in any
		/// particular way, but the ordering of a list is consistent across multiple calls, and even across operating system
		/// restarts. The ordering may change upon system configuration changes, including the addition or removal of an adapter,
		/// or a driver update on an existing adapter. You can register for these changes with
		/// <see cref="RegisterEventNotification(object, DXCoreNotificationType, DXCoreNotificationCallback, IntPtr)">IDXCoreAdapterFactory::RegisterEventNotification</see>
		/// using the notification type <see cref="DXCoreNotificationType.AdapterListStale"/>.
		/// </para>
		/// </remarks>
		/// 
		/// <param name="numAttributes">The number of elements in the array pointed to by the <i>filterAttributes</i> argument.</param>
		/// <param name="filterAttributes">
		/// A pointer to an array of adapter attribute GUIDs. For a list of attribute GUIDs, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-adapter-attribute-guids">DXCore adapter attribute GUIDs</see>.
		/// At least one GUID must be provided. In the case that more than one GUID is provided in the array, only adapters that meet all
		/// of the requested attributes will be included in the returned list.
		/// </param>
		/// <param name="riid">A reference to the globally unique identifier (GUID) of the interface that you wish to be returned. This is expected to be the interface identifier (IID) of <see cref="IDXCoreAdapterList"/>.</param>
		/// <returns>A pointer to the adapter list created.</returns>
		public IntPtr CreateAdapterList(uint numAttributes, [NativeType("const GUID*")] IntPtr filterAttributes, in Guid riid);

		/*
		/// <summary>
		/// A helper function that infers an interface identifier and invokes <see cref="CreateAdapterList(uint, IntPtr, in Guid)"/>
		/// using a span of attribute values.
		/// </summary>
		/// 
		/// <typeparam name="T">The COM interface type</typeparam>
		/// <param name="attributes">The attributes to pass to <see cref="CreateAdapterList(uint, IntPtr, in Guid)"/></param>
		/// <returns>The returned value from <see cref="CreateAdapterList(uint, IntPtr, in Guid)"/>, converted to the given COM type</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T CreateAdapterList<T>(in ReadOnlySpan<Guid> attributes) {
			unsafe {
				fixed(Guid* pAttributes = attributes) {
					return COMHelpers.GetObjectForCOMPointer<T>(CreateAdapterList((uint)attributes.Length, (IntPtr)pAttributes, typeof(T).GUID));					
				}
			}
		}
		*/

		/// <summary>
		/// <para>
		/// Retrieves the DXCore adapter object (<see cref="IDXCoreAdapter"/>) for a specified LUID, if available. For programming
		/// guidance, and code examples, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// Multiple calls passing the same <see cref="LUID"/> return identical interface pointers. As a result, it's safe to compare
		/// interface pointers to determine whether multiple pointers refer to the same adapter object.
		/// </para>
		/// </remarks>
		/// 
		/// <param name="adapterLUID">The locally unique value that identifies the adapter instance.</param>
		/// <param name="riid">A reference to the globally unique identifier (GUID) of the interface that you wish to be returned. This is expected to be the interface identifier (IID) of <see cref="IDXCoreAdapter"/>.</param>
		/// <returns>a pointer to the DXCore adapter created.</returns>
		public IntPtr GetAdapterByLuid(in LUID adapterLUID, in Guid riid);

		/*
		/// <summary>
		/// A helper function that infers an interface identifier, and calls <see cref="GetAdapterByLuid(in LUID, in Guid, out IntPtr)"/>.
		/// </summary>
		/// 
		/// <typeparam name="T">The COM inteface type</typeparam>
		/// <param name="adapterLUID">The adapter LUID to pass to <see cref="GetAdapterByLuid(in LUID, in Guid, out IntPtr)"/></param>
		/// <returns>The returned value from <see cref="GetAdapterByLuid(in LUID, in Guid)"/>, converted to the given COM type</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T GetAdapterByLuid<T>(in LUID adapterLUID) => COMHelpers.GetObjectForCOMPointer<T>(GetAdapterByLuid(adapterLUID, typeof(T).GUID));
		*/

		/// <summary>
		/// <para>
		/// Determines whether a specified notification type is supported by the operating system (OS). For programming guidance,
		/// and code examples, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// You can call <b>IsNotificationTypeSupported</b> to determine whether a given notification type is known to this version
		/// of the OS. For example, a notification type that's introduced in a particular version of Windows is unknown to previous
		/// versions of Windows.
		/// </para>
		/// </remarks>
		/// 
		/// <param name="notificationType">The type of notification that you're querying about support for. See the table in <see cref="DXCoreNotificationType"/> for info about the notification types.</param>
		/// <returns>Returns <c>true</c> if the notification type is supported by the system. Otherwise, returns <c>false</c>.</returns>
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.U1)]
		public bool IsNotificationTypeSupported(DXCoreNotificationType notificationType);

		// TODO: Documentation links for D3D11

		/// <summary>
		/// <para>
		/// Registers to receive notifications of specific conditions from a DXCore adapter or adapter list. For programming
		/// guidance, and code examples, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// You use <b>RegisterEventNotification</b> to register for events raised by <see cref="IDXCoreAdapterList"/> and
		/// <see cref="IDXCoreAdapter"/> interfaces. These notification types are supported.
		/// <list type="table">
		///		<listheader>
		///			<term><see cref="DXCoreNotificationType"/></term>
		///			<description><b>Notes</b></description>
		///		</listheader>
		///		<item>
		///			<term><see cref="DXCoreNotificationType.AdapterListStale">AdapterListStale</see></term>
		///			<description>
		///				Indicates that the list of adapters meeting your filter criteria has changed. If the adapter list is stale
		///				at the time of registration, then your callback is immediately called. This callback occurs at most one time
		///				per registration (<i>dxCoreObject</i> must be <see cref="IDXCoreAdapterList"/>).
		///			</description>
		///		</item>
		///		<item>
		///			<term><see cref="DXCoreNotificationType.AdapterNoLongerValid">AdapterNoLongerValid</see></term>
		///			<description>
		///				Indicates that the adapter is no longer valid. If the adapter is invalid at registration time, then your
		///				callback is immediately called (<i>dxCoreObject</i> must be <see cref="IDXCoreAdapter"/>).
		///			</description>
		///		</item>
		///		<item>
		///			<term><see cref="DXCoreNotificationType.AdapterBudgetChange">AdapterBudgetChange</see></term>
		///			<description>
		///				Indicates that a memory budgeting event has occurred, and that you should call
		///				<see cref="IDXCoreAdapter.QueryState{TIn, TOut}(DXCoreAdapterState, TIn?)">IDXCoreAdapter::QueryState</see>
		///				(with <see cref="DXCoreAdapterState.AdapterMemoryBudget"/>) to evaluate the current memory budget state. Upon
		///				registration, an initial callback will always occur to allow you to query the initial state
		///				(<i>dxCoreObject</i> must be <see cref="IDXCoreAdapter"/>).
		///			</description>
		///		</item>
		///		<item>
		///			<term><see cref="DXCoreNotificationType.AdapterContentProtectionTeardown">AdapterContentProtectionTeardown</see></term>
		///			<description>
		///				Indicates that you should re-evaluate the current crypto session status; for example, by calling
		///				ID3D11VideoContext1::CheckCryptoSessionStatus to determine the impact of the hardware teardown for a
		///				specific ID3D11CryptoSession interface. Upon registration, an initial callback will always occur to allow
		///				you to query the initial state.
		///			</description>
		///		</item>
		/// </list>
		/// </para>
		/// 
		/// <para>
		/// A call to the function that you provide in <i>callbackFunction</i> is made asynchronously on a background thread by
		/// DXCore when the detected event occurs. No guarantee is made as to the ordering or timing of callbacks—multiple callbacks
		/// may occur in any order, or even simultaneously. It's even possible for your callback to be invoked before
		/// <b>RegisterEventNotification</b> has completed. In that case, DXCore guarantees that your <i>eventCookie</i> is set
		/// before your callback is called. Multiple callbacks for a specific registration will be serialized in order.
		/// </para>
		/// 
		/// <para>
		/// Callbacks may occur at any time until you call <see cref="UnregisterEventNotification(uint)">UnregisterEventNotification</see>,
		/// and it completes. Callbacks occur on their own threads, and you may make calls into the DXCore API on those threads, including
		/// <b>UnregisterEventNotification</b>. However, you must not release the last reference to the <i>dxCoreObject</i> on this thread.
		/// </para>
		/// </remarks>
		/// 
		/// <param name="dxCoreObject">The DXCore object (<see cref="IDXCoreAdapter"/> or <see cref="IDXCoreAdapterList"/>) whose notifications you're subscribing to.</param>
		/// <param name="notificationType">The type of notification that you're registering for. See the table in <see cref="DXCoreNotificationType"/> for info about what types are valid with which kinds of objects.</param>
		/// <param name="callbackFunction">
		/// A pointer to a callback function (implemented by your application), which is called by the DXCore object for notification
		/// events. For the signature of the function, see <see cref="DXCoreNotificationCallback"/>.
		/// </param>
		/// <param name="callbackContext">An optional pointer to an object containing context info. This object is passed to your callback function when the notification is raised.</param>
		/// <returns>
		/// a non-zero cookie value representing this registration. Use this cookie value to unregister from the notification by
		/// calling <see cref="UnregisterEventNotification(uint)">IDXCoreAdapterFactory::UnregisterEventNotification</see>. See Remarks.
		/// </returns>
		public uint RegisterEventNotification([MarshalAs(UnmanagedType.IUnknown)] object dxCoreObject, DXCoreNotificationType notificationType, [MarshalAs(UnmanagedType.FunctionPtr)] DXCoreNotificationCallback callbackFunction, IntPtr callbackContext);

		/// <summary>
		/// <para>
		/// Unregisters from a notification that you previously registered for. For programming guidance, and code examples, see
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore-enum-adapters">Using DXCore to enumerate adapters</see>.
		/// </para>
		/// </summary>
		/// 
		/// <remarks>
		/// <para>
		/// <b>UnregisterEventNotification</b> returns only after all pending/in-progress callbacks for this registration have
		/// completed. DXCore guarantees that no new callbacks will occur for this registration after <b>UnregisterEventNotification</b>
		/// has returned. However, to avoid a deadlock, if you call <b>UnregisterEventNotification</b> from within your callback, then
		/// <b>UnregisterEventNotification</b> doesn't wait for the active callback to complete.
		/// </para>
		/// 
		/// <para>
		/// Once you unregister a cookie value, that value is then eligible for being returned by a subsequent registration.
		/// </para>
		/// </remarks>
		/// 
		/// <param name="eventCookie">
		/// The cookie value (returned when you called <see cref="RegisterEventNotification(object, DXCoreNotificationType, DXCoreNotificationCallback, IntPtr)">IDXCoreAdapterFactory::RegisterEventNotification</see>)
		/// representing a prior registration that you now wish to unregister for.
		/// </param>
		public void UnregisterEventNotification(uint eventCookie);

	}
	#endregion

	/// <summary>
	/// <para>
	/// DXCore is an adapter enumeration API for graphics and compute devices, so some of its facilities overlap with those of DXGI. DXCore is used by Direct3D 12.
	/// </para>
	/// <para>See the <see href="https://docs.microsoft.com/en-us/windows/win32/dxcore/dxcore">MSDN</see> page for more details.</para>
	/// </summary>
	public static class DXCore {

		[DllImport("DXCore.dll", PreserveSig = false)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "DllImport required for PreserveSig attribute")]
		private static extern IntPtr DXCoreCreateAdapterFactory(in Guid riid);

		/// <summary>
		/// <para>
		/// Creates a DXCore adapter factory, which you can use to generate further DXCore objects.
		/// </para>
		/// <para>See the <see href="https://docs.microsoft.com/en-us/windows/win32/api/dxcore/nf-dxcore-dxcorecreateadapterfactory">MSDN</see> page for more details.</para>
		/// </summary>
		/// 
		/// <typeparam name="T">The factory type to create</typeparam>
		/// <returns>The created DXCore factory</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T CreateAdapterFactory<T>() => COMHelpers.GetObjectFromCOMGetter<T>(DXCoreCreateAdapterFactory);

	}

}

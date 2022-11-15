using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.DirectX.Direct3D {

	// d3dkmthk.h

	/// <summary>
	/// The D3DKMT_DRIVERVERSION enumeration type contains values that indicate the version of the display driver model
	/// that the display miniport driver supports.
	/// </summary>
	public enum D3DKMTDriverVersion {
		/// <summary>
		/// The display miniport driver supports the Windows Vista display driver model (WDDM) without Windows 7 features.
		/// </summary>
		WDDM_1_0,
		/// <summary>
		/// The display miniport driver supports the Windows Vista display driver model with prereleased Windows 7 features.
		/// </summary>
		WDDM_1_1Prerelease,
		/// <summary>
		/// The display miniport driver supports the Windows Vista display driver model with released Windows 7 features.
		/// </summary>
		WDDM_1_1,
		/// <summary>
		/// The display miniport driver supports the Windows Vista display driver model with released Windows 8 features.
		/// </summary>
		WDDM_1_2,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows 8.1 features.
		/// </summary>
		WDDM_1_3,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows 10 features.
		/// </summary>
		WDDM_2_0,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows 10, version 1607 features.
		/// </summary>
		WDDM_2_1,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows 10, version 1703 features.
		/// </summary>
		WDDM_2_2,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows 10, version 1709 features.
		/// </summary>
		WDDM_2_3,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows 10, version 1803 features.
		/// </summary>
		WDDM_2_4,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows 10, version 1809 features.
		/// </summary>
		WDDM_2_5,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows 10, version 1903 features.
		/// </summary>
		WDDM_2_6,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows 10, version 2004 features.
		/// </summary>
		WDDM_2_7,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows 10, version 2004 features.
		/// </summary>
		WDDM_2_8,
		/// <summary>
		/// The display miniport driver supports the Windows display driver model with released Windows Server 2022 features.
		/// </summary>
		WDDM_2_9,
		WDDM_3_0
	}

	// d3dkmdt.h

	/// <summary>
	/// <para>
	/// Specifies the capabilities for the preemption of graphic processing unit (GPU) compute shader operations that the display miniport driver supports.
	/// </para>
	/// 
	/// <para>
	/// Starting with Windows 8, display miniport drivers need to specify the level of preemption granularity supported by the GPU
	/// when executing compute shader operations. Because engines on the same adapter may potentially support different preemption
	/// levels, the driver should report the coarsest granularity among all engines capable of executing a particular type of compute
	/// shader requests.
	/// </para>
	/// 
	/// <para>
	/// For example, if one engine supports the preemption of primitive level graphics requests, and another engine supports the
	/// preemption of triangle level graphics requests, the driver should report primitive level graphics preemption capability for
	/// that adapter.
	/// </para>
	/// </summary>
	public enum D3DKMDTComputePreemptionGranularity {
		/// <summary>
		/// The driver does not support the preemption of GPU compute shader operations.
		/// </summary>
		None,
		/// <summary>
		/// The driver cannot stop currently running DMA buffers of a specified type but can prevent all pending DMA buffers in the hardware queue from running.
		/// </summary>
		DMABufferBoundary,
		/// <summary>
		/// The driver cannot stop currently executing compute shader commands that were dispatched from a thread group but can prevent all pending commands from being dispatched.
		/// </summary>
		DispatchBoundary,
		/// <summary>
		/// The driver cannot stop currently executing compute shader commands that were dispatched from a thread group but can prevent a thread group from dispatching other commands.
		/// </summary>
		ThreadGroupBoundary,
		/// <summary>
		/// The driver cannot stop currently executing compute shader commands that were dispatched from a thread in a thread group but can prevent a thread from dispatching other commands.
		/// </summary>
		ThreadBoundary,
		/// <summary>
		/// The driver cannot stop currently running shader commands of a specified type but can prevent all shader commands in the hardware queue from running.
		/// </summary>
		ShaderBoundary
	}

	/// <summary>
	/// <para>Specifies the capabilities for the preemption of graphic processing unit (GPU) graphics operations that the display miniport driver supports.</para>
	/// 
	/// <para>
	/// Starting with Windows 8, display miniport drivers need to specify the level of preemption granularity supported by the GPU
	/// when executing graphics operations. Because engines on the same adapter may potentially support different preemption levels,
	/// the driver should report the coarsest granularity among all engines capable of executing a particular type of graphics
	/// request.
	/// </para>
	/// 
	/// <para>
	/// For example, if one engine supports the preemption of primitive level graphics requests, and another engine supports the
	/// preemption of triangle level graphics requests, the driver should report a capability of
	/// <see cref="PrimitiveBoundary">D3DKMDT_GRAPHICS_PREEMPTION_PRIMITIVE_BOUNDARY</see> for the adapter.
	/// </para>
	/// </summary>
	public enum D3DKMDTGraphicsPreemptionGranularity {
		/// <summary>
		/// The driver does not support the preemption of GPU graphics operations.
		/// </summary>
		None,
		/// <summary>
		/// The driver cannot stop currently running DMA buffers of a specified type but can prevent all pending DMA buffers in the hardware queue from running.
		/// </summary>
		DMABufferBoundary,
		/// <summary>
		/// The driver cannot stop currently running primitive buffers of a specified type but can prevent all pending primitive buffers in the hardware queue from running.
		/// </summary>
		PrimitiveBoundary,
		/// <summary>
		/// The driver cannot stop currently running triangle buffers of a specified type but can prevent all pending triangle buffers in the hardware queue from running.
		/// </summary>
		TriangleBoundary,
		/// <summary>
		/// The driver cannot stop currently running pixel buffers of a specified type but can prevent all pending pixel buffers in the hardware queue from running.
		/// </summary>
		PixelBoundary,
		/// <summary>
		/// The driver cannot stop currently running shader instruction buffers of a specified type but can prevent all pending shader instruction buffers in the hardware queue from running.
		/// </summary>
		ShaderBoundary
	}

}

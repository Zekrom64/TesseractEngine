using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Engine.Registry;

namespace Tesseract.Core.Engine.Content {

	public class Material : RegistryObject {

		public Material(RegistryKey unlocalizedName) : base(unlocalizedName) { }

		//=====================//
		// Physical Properties //
		//=====================//

		/// <summary>
		/// The density of this material, in kilograms per cubic meter (kg/m³).
		/// </summary>
		public float Density { get; init; } = 0;

		/// <summary>
		/// The hardness of this material on the Mohs hardness scale.
		/// </summary>
		public float Hardness { get; init; } = 0;

		//=======================//
		// Electrical Properties //
		//=======================//

		/// <summary>
		/// If the material is feasible as a conductor of electricity.
		/// </summary>
		public bool IsElectricalConductor { get; init; } = false;

		/// <summary>
		/// If the material is feasible as an insulator of electricity.
		/// </summary>
		public bool IsElectricalInsulator { get; init; } = false;

		/// <summary>
		/// The resistivity of the material in ohm-meters (Ω⋅m; ohms per meter of
		/// conductor per square meter cross-section of conductor). This value is
		/// for resistivity at a standard 20°C.
		/// </summary>
		public float ElectricalResistivity { get; init; } = float.PositiveInfinity;

		/// <summary>
		/// The coefficient that determines how electrical resistivity changes in
		/// response to temperature per degree Celsius (°C).
		/// </summary>
		public float ResistanceTemperatureCoefficient { get; init; } = 0;

		//====================//
		// Thermal Properties //
		//====================//

		/// <summary>
		/// The thermal conductivity of the material in Watts per meter-degrees-Kelvin
		/// (W·m⁻¹·K⁻¹, Watts of thermal energy per meter of material and degree of
		/// temperature difference). Note that Celsius and Kelvin are interchangeable
		/// for this unit as it indicates differential instead of absolute temperature.
		/// </summary>
		public float ThermalConductivity { get; init; } = 0.026f; // Default value for air at 25°C

		//=======================//
		// Combustion Properties //
		//=======================//

		/// <summary>
		/// If this material can be ignited.
		/// </summary>
		public bool IsFlammable { get; init; } = false;

		/// <summary>
		/// The auto-ignition temperature of this material in Celsius (°C).
		/// </summary>
		public float IgnitionTemperature { get; init; } = float.PositiveInfinity;

		/// <summary>
		/// The energy produced by burning this material in Joules per kilogram (J/kg).
		/// </summary>
		public float CombustionEnergyDensity { get; init; } = 0;

		/// <summary>
		/// If this material promotes combustion as an oxidizer.
		/// </summary>
		public bool IsOxidizer { get; init; } = false;

		/// <summary>
		/// The increase in combustion temperature provided by this oxidizer in degrees Celcius (°C).
		/// </summary>
		public float OxidizerTemperatureBoost { get; init; } = 0;

		/// <summary>
		/// The increase in combustion energy output provided by this oxidizer in Joules per kilogram (J/kg).
		/// </summary>
		public float OxidizerEnergyBoost { get; init; } = 0;

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.Core {

	/// <summary>
	/// Enumeration of known platform types.
	/// </summary>
	public enum PlatformType {
		Unknown = default,

		/// <summary>
		/// Windows platform (any version of windows).
		/// </summary>
		Windows,
		/// <summary>
		/// Linux platform (Linux-based kernel or other Unix-like system).
		/// </summary>
		Linux,
		/// <summary>
		/// MacOSX platform.
		/// </summary>
		MacOSX,

		/// <summary>
		/// Xbox platform (Xbox 360 -> One, etc.)
		/// </summary>
		XBox,

		/// <summary>
		/// Webassembly platform (most likely Blazor)
		/// </summary>
		Webassembly
	}

	/// <summary>
	/// Enumeration of known sub-platform types.
	/// </summary>
	public enum SubplatformType {
		Unknown = default,

		/// <summary>
		/// Non-WindowsNT based version of Windows (no longer supported).
		/// </summary>
		NonNTWindows,
		/// <summary>
		/// Standard version of Windows (NT - 10+).
		/// </summary>
		Windows,

		/// <summary>
		/// Generic linux operating system.
		/// </summary>
		Linux,
		/// <summary>
		/// FreeBSD operating system.
		/// </summary>
		FreeBSD,

		/// <summary>
		/// Generic MacOSX version.
		/// </summary>
		MacOSX,

		/// <summary>
		/// Xbox360 system (no longer supported).
		/// </summary>
		Xbox360,

		/// <summary>
		/// Generic Webassembly system (probably Blazor).
		/// </summary>
		Webassembly
	}

	/// <summary>
	/// Enumeration of CPU architecture types.
	/// </summary>
	public enum ArchitectureType {
		Unknown = default,

		/// <summary>
		/// X86 architecture (32 or 64 bit)
		/// </summary>
		X86,
		/// <summary>
		/// ARM architecture (32 or 64 bit)
		/// </summary>
		ARM,
		/// <summary>
		/// Wasm architecture (Webassembly)
		/// </summary>
		Wasm
	}

	/// <summary>
	/// Enumeration of CPU architecture sub-types.
	/// </summary>
	public enum SubarchitectureType {
		Unknown = default,

		/// <summary>
		/// 32-bit X86 (i386).
		/// </summary>
		X86_32,
		/// <summary>
		/// 64-bit X86 (AMD64).
		/// </summary>
		X86_64,
		/// <summary>
		/// 32-bit ARM.
		/// </summary>
		ARM,
		/// <summary>
		/// 64-bit ARM.
		/// </summary>
		ARM64,
		/// <summary>
		/// Webassembly.
		/// </summary>
		Wasm
	}

	/// <summary>
	/// A platform info object describes the details of a particular platform.
	/// </summary>
	public record PlatformInfo {

		/// <summary>
		/// The general type of the platform.
		/// </summary>
		public PlatformType Type { get; init; }

		/// <summary>
		/// The more specific sub-type of the platform.
		/// </summary>
		public SubplatformType Subtype { get; init; }

		/// <summary>
		/// The CPU architecture of the platform.
		/// </summary>
		public ArchitectureType Architecture { get; init; }
		
		/// <summary>
		/// The CPU sub-architecture of the platform.
		/// </summary>
		public SubarchitectureType Subarchitecture { get; init; }

	}

	/// <summary>
	/// A platform provides some common information for different systems the engine might be running on.
	/// </summary>
	public abstract class Platform {

		/// <summary>
		/// The current sub-platform type.
		/// </summary>
		public static readonly SubplatformType CurrentSubplatformType = Environment.OSVersion.Platform switch {
			PlatformID.Win32S or PlatformID.Win32Windows or PlatformID.WinCE => SubplatformType.NonNTWindows,
			PlatformID.Win32NT => SubplatformType.Windows,
			PlatformID.Unix =>
				RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) ? SubplatformType.FreeBSD :
					RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? SubplatformType.MacOSX :
						SubplatformType.Linux,
			PlatformID.MacOSX => SubplatformType.MacOSX,
			PlatformID.Other => CurrentArchitecture == ArchitectureType.Wasm ? SubplatformType.Webassembly : SubplatformType.Unknown,
			_ => SubplatformType.Unknown
		};

		/// <summary>
		/// The current platform type.
		/// </summary>
		public static readonly PlatformType CurrentPlatformType = CurrentSubplatformType switch {
			SubplatformType.NonNTWindows or SubplatformType.Windows => PlatformType.Windows,
			SubplatformType.Linux or SubplatformType.FreeBSD => PlatformType.Linux,
			SubplatformType.MacOSX => PlatformType.MacOSX,
			SubplatformType.Webassembly => PlatformType.Webassembly,
			SubplatformType.Xbox360 => PlatformType.XBox,
			_ => PlatformType.Unknown
		};

		/// <summary>
		/// The current CPU sub-architecture type.
		/// </summary>
		public static readonly SubarchitectureType CurrentSubarchitecture = RuntimeInformation.ProcessArchitecture switch {
			Architecture.X64 => SubarchitectureType.X86_64,
			Architecture.X86 => SubarchitectureType.X86_32,
			Architecture.Arm => SubarchitectureType.ARM,
			Architecture.Arm64 => SubarchitectureType.ARM64,
			Architecture.Wasm => SubarchitectureType.Wasm,
			_ => SubarchitectureType.Unknown
		};

		/// <summary>
		/// The current CPU architecture type.
		/// </summary>
		public static readonly ArchitectureType CurrentArchitecture = CurrentSubarchitecture switch {
			SubarchitectureType.X86_32 or SubarchitectureType.X86_64 => ArchitectureType.X86,
			SubarchitectureType.ARM or SubarchitectureType.ARM64 => ArchitectureType.ARM,
			SubarchitectureType.Wasm => ArchitectureType.Wasm,
			_ => ArchitectureType.Unknown
		};

		/// <summary>
		/// The current platform information.
		/// </summary>
		public static readonly PlatformInfo CurrentPlatformInfo = new() {
			Type = CurrentPlatformType,
			Architecture = CurrentArchitecture,
			Subarchitecture = CurrentSubarchitecture
		};

		/// <summary>
		/// The current platform.
		/// </summary>
		public static readonly Platform CurrentPlatform = CurrentPlatformType switch {
			PlatformType.Windows => new PlatformWindows(),
			PlatformType.Linux => new PlatformLinux(),
			PlatformType.MacOSX => new PlatformMacOSX(),
			_ => null // TODO: Should current platform be null for unknown systems?
		};
		
		/// <summary>
		/// If the current process is 64-bit. If this is false it is assumed it is 32-bit.
		/// </summary>
		public static readonly bool Is64Bit = Environment.Is64BitProcess;

		/// <summary>
		/// The file extension for native libraries on this platform.
		/// </summary>
		public abstract string NativeLibraryExtension { get; }

	}

	public class PlatformWindows : Platform {

		public override string NativeLibraryExtension => "dll";

	}

	public class PlatformLinux : Platform {

		public override string NativeLibraryExtension => "so";

	}

	public class PlatformMacOSX : Platform {

		public override string NativeLibraryExtension => "dylib";

	}

}

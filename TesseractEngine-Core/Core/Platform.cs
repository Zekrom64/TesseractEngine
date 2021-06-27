using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core {

	public enum PlatformType {
		Unknown,

		Windows,
		Linux,
		MacOSX,
	}

	public abstract class Platform {

		public static readonly PlatformType CurrentPlatformType = Environment.OSVersion.Platform switch {
			PlatformID.Win32S or PlatformID.Win32Windows or PlatformID.Win32NT or PlatformID.WinCE => PlatformType.Windows,
			PlatformID.Xbox => PlatformType.Unknown,
			PlatformID.Unix => PlatformType.Linux,
			PlatformID.MacOSX => PlatformType.MacOSX,
			PlatformID.Other => PlatformType.Unknown,
			_ => PlatformType.Unknown
		};

		public static readonly Platform CurrentPlatform = CurrentPlatformType switch {
			PlatformType.Windows => new PlatformWindows(),
			PlatformType.Linux => new PlatformLinux(),
			PlatformType.MacOSX => new PlatformMacOSX(),
			_ => null // TODO: Should current platform be null for unknown systems?
		};

		public static readonly bool Is64Bit = Environment.Is64BitProcess;

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

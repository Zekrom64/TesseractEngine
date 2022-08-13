using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Native {

	public delegate string NativeLibraryLocator(string name);

	public record LibrarySpec {

		public string Name { get; init; } = null!;
		public string[] AltNames { get; init; } = Array.Empty<string>();
		public LibrarySpec[] Dependencies { get; init; } = Array.Empty<LibrarySpec>();

	}

	public class Library {

		public static void LoadFunctions(Func<string, IntPtr> loader, object funcs) {
			foreach (var field in funcs.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)) {
				string name = field.Name;
				try {
					ExternFunctionAttribute? efa = field.GetCustomAttribute<ExternFunctionAttribute>();
					if (efa != null) {
						if (efa.Manual) continue;
						if (efa.Platform != default && efa.Platform != Platform.CurrentPlatformType) continue;
						if (efa.Subplatform != default && efa.Subplatform != Platform.CurrentSubplatformType) continue;
					}
					Type delegateType = field.FieldType;
					IntPtr pfn = loader(name);
					if (pfn == IntPtr.Zero && efa?.AltNames != null) {
						foreach (string altname in efa.AltNames) {
							pfn = loader(altname);
							if (pfn != IntPtr.Zero) break;
						}
					}
					if (pfn == IntPtr.Zero && (efa == null || !efa.Relaxed)) throw new InvalidOperationException($"Could not load function \"{name}\"");
					Delegate? del = null;
					if (pfn != IntPtr.Zero) del = Marshal.GetDelegateForFunctionPointer(pfn, delegateType);
					field.SetValue(funcs, del);
				} catch (Exception e) {
					throw new MissingMethodException($"No valid export for function {name}", e);
				}
			}
		}

		public IntPtr Handle { get; }

		public Library(string name) {
			Handle = NativeLibrary.Load(name);
		}

		public Library(IntPtr handle) {
			Handle = handle;
		}

		public IntPtr GetExport(string name) {
			if (NativeLibrary.TryGetExport(Handle, name, out IntPtr address)) return address;
			else return IntPtr.Zero;
		}

		public T GetFunction<T>(string name) where T : Delegate => Marshal.GetDelegateForFunctionPointer<T>(GetExport(name));

		public void LoadFunctions(object funcs) => LoadFunctions(name => NativeLibrary.GetExport(Handle, name), funcs);

		public IntPtr this[string name] => GetExport(name);

	}

	public static class LibraryManager {

		private static readonly Dictionary<string, Library> loadedLibraries = new();

		static LibraryManager() {
			AppDomain.CurrentDomain.ProcessExit += (s, e) => {
				foreach (Library library in loadedLibraries.Values) NativeLibrary.Free(library.Handle);
			};
		}

		public static NativeLibraryLocator Locator { get; set; } = (string name) => {
			string path = "lib" + Path.DirectorySeparatorChar;
			path += Platform.CurrentPlatformType switch {
				PlatformType.Windows => "win" + (Platform.Is64Bit ? "64" : "32") + Path.DirectorySeparatorChar,
				PlatformType.Linux => "linux" + (Platform.Is64Bit ? "64" : "32") + Path.DirectorySeparatorChar,
				PlatformType.MacOSX => "macosx" + Path.DirectorySeparatorChar,
				_ => ""
			};
			path += name;
			string? extension = Platform.CurrentPlatform?.NativeLibraryExtension;
			if (extension != null) path += "." + extension;
			return path;
		};

		public static Library Load(LibrarySpec spec) {
			if (!loadedLibraries.TryGetValue(spec.Name, out Library? library)) {
				if (spec.Dependencies != null)
					foreach (LibrarySpec subspec in spec.Dependencies) Load(subspec);
				List<string> libnames = new() { spec.Name };
				if (spec.AltNames != null) foreach (string name in spec.AltNames) libnames.Add(name);
				foreach(string name in libnames) {
					try {
						IntPtr pLibrary = NativeLibrary.Load(Locator(spec.Name));
						if (pLibrary != IntPtr.Zero) {
							library = new Library(pLibrary);
							break;
						}
					} catch (DllNotFoundException) { }
				}
				loadedLibraries[spec.Name] = library ?? throw new DllNotFoundException($"Could not find library \"{spec.Name}\" (or under alternative names)");
			}
			return library;
		}

	}

}

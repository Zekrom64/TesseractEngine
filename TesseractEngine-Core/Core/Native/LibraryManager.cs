using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Native {

	public delegate string NativeLibraryLocator(string name);
	
	public class LibrarySpec {

		public string Name { get; init; }
		public LibrarySpec[] Dependencies { get; init; }

	}

	public class Library {

		public static void LoadFunctions(Func<string,IntPtr> loader, object funcs) {
			foreach (var field in funcs.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)) {
				ExternFunctionAttribute efa = field.GetCustomAttribute<ExternFunctionAttribute>();
				if (!(efa?.Predicate() ?? true)) continue;
				if (efa.Platform != null && efa.Platform.Value != Platform.CurrentPlatformType) continue;
				Type delegateType = field.FieldType;
				string name = field.Name;
				IntPtr pfn = loader(name);
				if (pfn == IntPtr.Zero && efa?.AltNames != null) {
					foreach(string altname in efa.AltNames) {
						pfn = loader(altname);
						if (pfn != IntPtr.Zero) break;
					}
				}
				Delegate del = null;
				if (pfn != IntPtr.Zero) del = Marshal.GetDelegateForFunctionPointer(pfn, delegateType);
				field.SetValue(funcs, del);
			}
		}

		public IntPtr Handle { get; }

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
			string extension = Platform.CurrentPlatform?.NativeLibraryExtension;
			if (extension != null) path += "." + extension;
			return path;
		};

		public static Library Load(LibrarySpec spec) {
			if (!loadedLibraries.TryGetValue(spec.Name, out Library library)) {
				if (spec.Dependencies != null)
					foreach (LibrarySpec subspec in spec.Dependencies) Load(subspec);
				library = new Library(NativeLibrary.Load(Locator(spec.Name)));
				loadedLibraries[spec.Name] = library;
			}
			return library;
		}

	}

}

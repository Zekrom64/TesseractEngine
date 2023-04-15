using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Tesseract.Core.Resource;

namespace Tesseract.Core.Native {

	/// <summary>
	/// A library locator determines the file path to a native library from its generic name (eg. 'glfw' or 'SDL2').
	/// </summary>
	/// <param name="name">The generic name of the library</param>
	/// <returns>The expected path to the native library</returns>
	public delegate string NativeLibraryLocator(string name);

	/// <summary>
	/// A library spec determines how a native library should be loaded.
	/// </summary>
	public record LibrarySpec {

		/// <summary>
		/// The name of the library.
		/// </summary>
		public required string Name { get; init; }
		/// <summary>
		/// A list of alternate names for the library.
		/// </summary>
		public string[] AltNames { get; init; } = Array.Empty<string>();
		public LibrarySpec[] Dependencies { get; init; } = Array.Empty<LibrarySpec>();

	}

	/// <summary>
	/// Interface for a native library.
	/// </summary>
	public class Library {

		/// <summary>
		/// Loads each field of the specified object with the corresponding function deletegate retrieved
		/// from a loader based on the name of the field.
		/// </summary>
		/// <param name="loader">The native function loader</param>
		/// <param name="funcs">The object to load functions for</param>
		/// <exception cref="MissingMethodException">If one of the fields could not be loaded with a function</exception>
		public static void LoadFunctions(Func<string, IntPtr> loader, object funcs) {
			foreach (var field in funcs.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)) {
				string name = field.Name;
				try {
					// Check if we have an attribute providing properties for the function
					ExternFunctionAttribute? efa = field.GetCustomAttribute<ExternFunctionAttribute>();
					if (efa != null) {
						if (efa.Manual) continue;
						if (efa.Platform != default && efa.Platform != Platform.CurrentPlatformType) continue;
						if (efa.Subplatform != default && efa.Subplatform != Platform.CurrentSubplatformType) continue;
					}
					// Import the pointer from the library
					Type functionType = field.FieldType;
					IntPtr pfn = loader(name);
					if (pfn == IntPtr.Zero && efa?.AltNames != null) {
						foreach (string altname in efa.AltNames) {
							pfn = loader(altname);
							if (pfn != IntPtr.Zero) break;
						}
					}
					// If not found and not relaxed loading, throw an exception
					if (pfn == IntPtr.Zero && (efa == null || !efa.Relaxed)) throw new InvalidOperationException($"Could not load function \"{name}\"");
					if (functionType.IsAssignableTo(typeof(Delegate))) {
						// If field is a delegate type get the delegate for the function pointer
						Delegate? del = null;
						if (pfn != IntPtr.Zero) del = Marshal.GetDelegateForFunctionPointer(pfn, functionType);
						field.SetValue(funcs, del);
					} else {
						// Else assume its an unmanaged delegate we can convert to directly
						field.SetValue(funcs, Convert.ChangeType(pfn, functionType));
					}
				} catch (Exception e) {
					throw new MissingMethodException($"No valid export for function {name}", e);
				}
			}
		}

		/// <summary>
		/// The native handle for the library.
		/// </summary>
		public IntPtr Handle { get; }

		/// <summary>
		/// Creates a library from the given name via <see cref="NativeLibrary.Load(string)"/>
		/// </summary>
		/// <param name="name">The name of the library to load</param>
		public Library(string name) {
			Handle = NativeLibrary.Load(name);
		}

		/// <summary>
		/// Creates a library from an existing handle.
		/// </summary>
		/// <param name="handle">The handle of the native library</param>
		public Library(IntPtr handle) {
			Handle = handle;
		}

		/// <summary>
		/// Gets an exported pointer from the library, returning <see cref="nint.Zero"/> if none was found.
		/// </summary>
		/// <param name="name">The name of the exported pointer</param>
		/// <returns>The exported pointer, or <see cref="nint.Zero"/></returns>
		public IntPtr GetExport(string name) {
			if (NativeLibrary.TryGetExport(Handle, name, out IntPtr address)) return address;
			else return IntPtr.Zero;
		}

		/// <summary>
		/// Gets an exported function from the library.
		/// </summary>
		/// <typeparam name="T">The delegate type of the function</typeparam>
		/// <param name="name">The name of the exported function</param>
		/// <returns>The exported function</returns>
		public T GetFunction<T>(string name) where T : Delegate => Marshal.GetDelegateForFunctionPointer<T>(GetExport(name));

		/// <summary>
		/// Loads functions from this library into the specified object using <see cref="LoadFunctions(Func{string, nint}, object)"/>.
		/// </summary>
		/// <param name="funcs">The object to load functions for</param>
		public void LoadFunctions(object funcs) => LoadFunctions(name => NativeLibrary.GetExport(Handle, name), funcs);

		/// <summary>
		/// Gets an exported pointer from this library.
		/// </summary>
		/// <seealso cref="GetExport(string)"/>
		/// <param name="name">The name of the exported pointer</param>
		/// <returns>The exported pointer, or <see cref="nint.Zero"/></returns>
		public IntPtr this[string name] => GetExport(name);

	}

	/// <summary>
	/// The library manager controls how native libraries are loaded.
	/// </summary>
	public static class LibraryManager {

		// Set of loaded libraries by their name
		private static readonly Dictionary<string, Library> loadedLibraries = new();

		static LibraryManager() {
			// Free all libraries at exit
			AppDomain.CurrentDomain.ProcessExit += (s, e) => {
				foreach (Library library in loadedLibraries.Values) NativeLibrary.Free(library.Handle);
			};
		}

		/// <summary>
		/// A resource domain which the manager can use to extract embedded native libraries.
		/// </summary>
		public static ResourceDomain? EmbeddedLibraryDomain { get; set; } = null;

		/// <summary>
		/// The locator for native libraries.
		/// </summary>
		public static NativeLibraryLocator Locator { get; set; } = (string name) => {
			string path = "lib/";
			path += Platform.CurrentPlatformType switch {
				PlatformType.Windows => $"win{(Platform.Is64Bit ? "64" : "32")}/",
				PlatformType.Linux => $"linux{(Platform.Is64Bit ? "64" : "32")}/",
				PlatformType.MacOSX => $"macosx/",
				_ => throw new PlatformNotSupportedException("Unsupported platform")
			};
			path += name;
			string? extension = Platform.CurrentPlatform?.NativeLibraryExtension;
			if (extension != null) path += "." + extension;
			return path.Replace('/', Path.DirectorySeparatorChar);
		};

		/// <summary>
		/// Loads a native library.
		/// </summary>
		/// <param name="spec">The specifications of the library</param>
		/// <returns>The loaded native library</returns>
		/// <exception cref="DllNotFoundException">If the native library could not be loaded</exception>
		public static Library Load(LibrarySpec spec) {
			if (!loadedLibraries.TryGetValue(spec.Name, out Library? library)) {
				// Load each dependency first
				if (spec.Dependencies != null)
					foreach (LibrarySpec subspec in spec.Dependencies) Load(subspec);
				List<string> libnames = new() { spec.Name };
				if (spec.AltNames != null) foreach (string name in spec.AltNames) libnames.Add(name);
				// For each possible name of the library
				foreach(string name in libnames) {
					try {
						// Find its location
						string location = Locator(spec.Name);
						// If it doesn't exist, try to load from the embedded library domain
						if (!File.Exists(location)) {
							if (EmbeddedLibraryDomain == null) throw new FileNotFoundException($"Missing native library at \"{location}\"");
							Resource.ResourceLocation resource = new(EmbeddedLibraryDomain, location.Replace(Path.DirectorySeparatorChar, '/'));
							if (resource.Exists) {
								Directory.CreateDirectory(location[..location.LastIndexOf(Path.DirectorySeparatorChar)]);
								using FileStream dstFile = File.Create(location);
								try {
									using Stream srcStream = resource.OpenStream();
									srcStream.CopyTo(dstFile);
								} catch(Exception ex) {
									File.Delete(location);
									throw new Exception("Failed to extract embedded native library", ex);
								}
							}
						}
						// Load the library if possible
						IntPtr pLibrary = NativeLibrary.Load(location);
						if (pLibrary != IntPtr.Zero) {
							library = new Library(pLibrary);
							break;
						}
					} catch (Exception) { 
						// TODO: Log failures instead of ignoring them
					}
				}
				// If loaded succesfully, register the library so we don't need to load it again
				loadedLibraries[spec.Name] = library ?? throw new DllNotFoundException($"Could not find library \"{spec.Name}\" (or under alternative names)");
			}
			return library;
		}

	}

}

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Tesseract.Core.Resource {

	/// <summary>
	/// A file resource domain provides resources in an accessible file system starting in a root directory.
	/// </summary>
	public class FileResourceDomain : ResourceDomain {

		/// <summary>
		/// Gets the extension of a file given its name, similar to <see cref="Path.GetFileName(string?)"/> but
		/// using the file name only instead of a path.
		/// </summary>
		/// <param name="name">File name</param>
		/// <returns>Extension of the file name, or an empty string if there is none</returns>
		public static string? GetExtensionFromFileName(string? name) {
			if (name == null) return null;
			string extension = string.Empty;
			int dotpos = name.LastIndexOf('.');
			if (dotpos != -1) extension = name[(dotpos + 1)..];
			return extension;
		}

		// The absolute path of the root directory of the domain
		private readonly string directory;

		/// <summary>
		/// Creates a new file resource domain targeting the given directory.
		/// </summary>
		/// <param name="name">Name of the resource domain</param>
		/// <param name="directory">Path to root directory of resource domain</param>
		public FileResourceDomain(string name, string directory) : base(name) {
			this.directory = Path.GetFullPath(directory);
		}

		// Converts a resource location to an absolute path to the file
		private string ToFilePath(ResourceLocation location) => directory + Path.DirectorySeparatorChar + location.Path.Replace('/', Path.DirectorySeparatorChar);

		// Converts an absolute path within the root directory of the domain to a resource location
		private ResourceLocation FromFilePath(string path) => new(this, path[(directory.Length + 1)..]);

		public override IEnumerable<ResourceLocation> EnumerateDirectory(ResourceLocation dir) {
			if (dir.Domain != this) throw new ArgumentException("Cannot operate on a resource location from a different domain", nameof(dir));
			DirectoryInfo dirInfo = new(ToFilePath(dir));
			foreach (FileInfo fileInfo in dirInfo.EnumerateFiles()) {
				yield return FromFilePath(fileInfo.FullName);
			}
		}

		public override Stream OpenStream(ResourceLocation file) {
			if (file.Domain != this) throw new ArgumentException("Cannot operate on a resource location from a different domain", nameof(file));
			return new FileStream(ToFilePath(file), FileMode.Open);
		}

		public override ResourceMetadata GetMetadata(ResourceLocation file) {
			if (file.Domain != this) throw new ArgumentException("Cannot operate on a resource location from a different domain", nameof(file));

			FileInfo info = new(ToFilePath(file));
			string? mime = null;
			if (MIME.TryGuessFromExtension(GetExtensionFromFileName(info.Name), out string? type)) mime = type;

			return new ResourceMetadata() {
				MIMEType = mime,
				Size = info.Length,
				Local = true
			};
		}

	}

	/// <summary>
	/// A zip resource domain provides resources stored in a Zip file.
	/// </summary>
	public class ZipResourceDomain : ResourceDomain, IDisposable {

		// The underlying archive
		private readonly ZipArchive archive;
		// Cache of known directory listings
		private readonly Dictionary<ResourceLocation, List<ResourceLocation>> cachedDirectories = new();

		/// <summary>
		/// Creates a new resource domain, targeting the given Zip archive.
		/// </summary>
		/// <param name="name">Name of the resource domain</param>
		/// <param name="archive">The Zip archive to target</param>
		public ZipResourceDomain(string name, ZipArchive archive) : base(name) {
			this.archive = archive;
		}

		// Gets the zip archive entry for a given resource location within this domain
		private ZipArchiveEntry? ToEntry(ResourceLocation location) => archive.GetEntry(location.Path);

		// Gets the resource location for a given zip archive entry
		private ResourceLocation FromEntry(ZipArchiveEntry entry) => new(this, entry.FullName);

		// Does the actual directory enumeration
		private IEnumerable<ResourceLocation> DoEnumerateDirectory(ResourceLocation dir) {
			foreach (ZipArchiveEntry entry in archive.Entries) {
				ResourceLocation location = FromEntry(entry);
				if (location.Parent == dir) yield return location;
			}
		}

		public override IEnumerable<ResourceLocation> EnumerateDirectory(ResourceLocation dir) {
			if (dir.Domain != this) throw new ArgumentException("Cannot operate on a resource location from a different domain", nameof(dir));
			if (cachedDirectories.TryGetValue(dir, out List<ResourceLocation>? entries)) return entries;
			entries = DoEnumerateDirectory(dir).ToList();
			cachedDirectories[dir] = entries;
			return entries;
		}

		public override Stream OpenStream(ResourceLocation file) {
			if (file.Domain != this) throw new ArgumentException("Cannot operate on a resource location from a different domain", nameof(file));
			ZipArchiveEntry? entry = ToEntry(file);
			if (entry == null) throw new IOException("Cannot open stream on a resource that does not exist");
			return entry.Open();
		}

		public override ResourceMetadata GetMetadata(ResourceLocation file) {
			if (file.Domain != this) throw new ArgumentException("Cannot operate on a resource location from a different domain", nameof(file));

			ZipArchiveEntry? entry = ToEntry(file);
			string? mime = null;
			if (entry != null && MIME.TryGuessFromExtension(FileResourceDomain.GetExtensionFromFileName(entry.Name), out string? type)) mime = type;

			return new ResourceMetadata() {
				MIMEType = mime,
				Size = entry != null ? entry.Length : -1,
				Local = true
			};
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			archive.Dispose();
		}

	}

}

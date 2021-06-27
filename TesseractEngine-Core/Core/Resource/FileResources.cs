using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Resource {
	
	/// <summary>
	/// A file resource domain provides resources in an accessible file system starting in a root directory.
	/// </summary>
	public class FileResourceDomain : ResourceDomain {

		private readonly string directory;

		/// <summary>
		/// Creates a new file resource domain targeting the given directory.
		/// </summary>
		/// <param name="name">Name of the resource domain</param>
		/// <param name="directory">Path to root directory of resource domain</param>
		public FileResourceDomain(string name, string directory) {
			Name = name;
			this.directory = Path.GetFullPath(directory);
		}

		private string ToFilePath(ResourceLocation location) => directory + Path.DirectorySeparatorChar + location.Path.Replace('/', Path.DirectorySeparatorChar);

		private ResourceLocation FromFilePath(string path) => new(this, path[(directory.Length + 1)..]);

		public override IEnumerable<ResourceLocation> EnumerateDirectory(ResourceLocation dir) {
			DirectoryInfo dirInfo = new(ToFilePath(dir));
			foreach(FileInfo fileInfo in dirInfo.EnumerateFiles()) {
				yield return FromFilePath(fileInfo.FullName);
			}
		}

		public override Stream OpenStream(ResourceLocation file) => new FileStream(ToFilePath(file), FileMode.Open);

		public override ResourceMetadata GetMetadata(ResourceLocation file) {
			FileInfo info = new(ToFilePath(file));
			string name = info.Name;
			string extension = null;
			int dotpos = name.LastIndexOf('.');
			if (dotpos != -1) extension = name[(dotpos + 1)..];

			string mime = null;
			if (MIME.TryGuessFromExtension(extension, out string type)) mime = type;

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

		private readonly ZipArchive archive;
		private readonly Dictionary<ResourceLocation, List<ResourceLocation>> cachedDirectories = new();

		/// <summary>
		/// Creates a new resource domain, targeting the given Zip archive.
		/// </summary>
		/// <param name="name">Name of the resource domain</param>
		/// <param name="archive">The Zip archive to target</param>
		public ZipResourceDomain(string name, ZipArchive archive) {
			Name = name;
			this.archive = archive;
		}

		private ZipArchiveEntry ToEntry(ResourceLocation location) => archive.GetEntry(location.Path);

		private ResourceLocation FromEntry(ZipArchiveEntry entry) => new (this, entry.FullName);

		private IEnumerable<ResourceLocation> DoEnumerateDirectory(ResourceLocation dir) {
			foreach(ZipArchiveEntry entry in archive.Entries) {
				ResourceLocation location = FromEntry(entry);
				if (location.Parent == dir) yield return location;
			}
		}

		public override IEnumerable<ResourceLocation> EnumerateDirectory(ResourceLocation dir) {
			if (cachedDirectories.TryGetValue(dir, out List<ResourceLocation> entries)) return entries;
			entries = DoEnumerateDirectory(dir).ToList();
			cachedDirectories[dir] = entries;
			return entries;
		}

		public override Stream OpenStream(ResourceLocation file) => ToEntry(file).Open();

		public override ResourceMetadata GetMetadata(ResourceLocation file) {
			ZipArchiveEntry entry = ToEntry(file);

			string name = entry.Name;
			string extension = null;
			int dotpos = name.LastIndexOf('.');
			if (dotpos != -1) extension = name[(dotpos + 1)..];

			string mime = null;
			if (MIME.TryGuessFromExtension(extension, out string type)) mime = type;

			return new ResourceMetadata() {
				MIMEType = mime,
				Size = entry.Length,
				Local = true
			};
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			archive.Dispose();
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Resource {
	
	/// <summary>
	/// An assembly resource domain will reflect manifest resources from an assembly as resources.
	/// </summary>
	public class AssemblyResourceDomain : ResourceDomain {

		/// <summary>
		/// The assembly referenced by this domain.
		/// </summary>
		public Assembly Assembly { get; }

		private struct EmbeddedResource {

			public string Name;

			public ResourceMetadata Metadata;

		}

		private readonly Dictionary<string, EmbeddedResource> resourceCache = new();
		private readonly Dictionary<string, List<string>> directoryCache = new();

		private static string ConvertResourcePath(string path) {
			string[] parts = path.Split('.');
			int namepart = parts.Length - 2;
			string ext = parts[^1];
			if (ext.ToLower() != ext) namepart++;
			string respath = "";
			for(int i = 0; i < parts.Length; i++) {
				respath += parts[i];
				if (i != parts.Length - 1) {
					respath += (i == namepart) ? '.' : '/';
				}
			}
			return respath;
		}
		
		/// <summary>
		/// Creates a new assembly resource domain with the given name using the given assembly.
		/// </summary>
		/// <param name="name">Resource domain name</param>
		/// <param name="assembly">Assembly to load resources from</param>
		public AssemblyResourceDomain(string name, Assembly assembly) : base(name) {
			Assembly = assembly;

			// Gather manifest resources based on names
			foreach(string resname in assembly.GetManifestResourceNames()) {
				string path = ConvertResourcePath(resname);
				
				string? ext = null;
				int extpos = path.LastIndexOf('.');
				if (extpos != -1) ext = path[(extpos + 1)..];

				MIME.TryGuessFromExtension(ext, out string? mime);

				ResourceMetadata meta = new() {
					Local = true,
					MIMEType = mime,
					Size = -1
				};

				resourceCache[path] = new EmbeddedResource() { Name = resname, Metadata = meta };
			}

			// Determine directories based on tree structure
			KeyedTree<string, string> fileTree = new();
			foreach(var file in resourceCache) {
				string path = file.Key;
				string[] parts = path.Split('/');
				fileTree.GetOrCreatePath(parts[..^1]).Leaf = parts[^1];
			}

			fileTree.Iterate((IReadOnlyList<string> path, KeyedTree<string, string>.Branch branch) => {
				string respath = path.Aggregate((string scur, string snew) => scur + '/' + snew);
				List<string> subpaths = new();
				if (branch.Leaf != null) subpaths.Add(respath + '/' + branch.Leaf);
				foreach (var subbranch in branch.Branches) subpaths.Add(respath + '/' + subbranch.Key);
				directoryCache[respath] = subpaths;
			});
		}

		public override Stream OpenStream(ResourceLocation file) {
			if (resourceCache.TryGetValue(file.Path, out EmbeddedResource resource)) {
				Stream? stream = Assembly.GetManifestResourceStream(resource.Name);
				if (stream == null) throw new IOException($"Failed to open resource \"{file.Name}\" from assembly {Assembly.FullName}");
				return stream;
			} else throw new IOException($"No such resource \"{file.Name}\" in assembly {Assembly.FullName}'s manifest");
		}

		public override IEnumerable<ResourceLocation> EnumerateDirectory(ResourceLocation dir) {
			if (directoryCache.TryGetValue(dir.Path, out List<string>? subpaths)) {
				foreach (string subpath in subpaths) yield return new ResourceLocation(this, subpath);
			}
		}

		public override ResourceMetadata GetMetadata(ResourceLocation file) {
			if (resourceCache.TryGetValue(file.Path, out EmbeddedResource resource)) return resource.Metadata;
			else return default;
		}

	}
}

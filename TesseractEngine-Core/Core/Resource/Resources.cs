using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Core.Util;

namespace Tesseract.Core.Resource {

	public abstract class ResourceDomain {

		private static readonly ThreadLocal<ResourceDomain> contextualDomain = new(() => Default);
		private static readonly Dictionary<string,ResourceDomain> allDomains = new();

		public static ResourceDomain Current {
			get => contextualDomain.Value;
			set {
				if (value != null) contextualDomain.Value = value;
			}
		}

		public static ResourceDomain Default { get; private set; }

		public static IReadOnlyDictionary<string, ResourceDomain> All {
			get {
				lock(allDomains) {
					// Return copy, domains *could* change at runtime
					return new Dictionary<string, ResourceDomain>(allDomains);
				}
			}
		}

		[ThreadSafety(ThreadSafetyLevel.SingleThread)]
		public static void SetDefaultDomain(ResourceDomain domain) {
			if (Default == null) Default = domain;
			else throw new InvalidOperationException("Cannot change default resource domain once set");
		}

		public static void AddDomain(ResourceDomain domain) {
			lock(allDomains) {
				string name = domain.Name;
				if (allDomains.ContainsKey(name)) throw new ArgumentException($"Resource domain \"{name}\" already exists", nameof(domain));
				allDomains[name] = domain;
			}
		}

		public static void SetCurrentDomain(string name) {
			lock(allDomains) {
				Current = allDomains.GetValueOrDefault(name, Default);
			}
		}

		public string Name { get; protected init; }

		public abstract Stream OpenStream(ResourceLocation file);

		public abstract IEnumerable<ResourceLocation> EnumerateDirectory(ResourceLocation dir);

		public abstract ResourceMetadata GetMetadata(ResourceLocation file);

		public override string ToString() => Name;

	}

	public struct ResourceLocation : IEquatable<ResourceLocation> {

		public ResourceDomain Domain { get; }
		public string Path { get; }

		public ResourceLocation Parent {
			get {
				int seppos = Path.LastIndexOf('/');
				if (seppos == -1) return new ResourceLocation(Domain, "");
				else return new ResourceLocation(Domain, Path.Substring(0, seppos));
			}
		}

		public string Name {
			get {
				int seppos = Path.LastIndexOf('/');
				if (seppos == -1) return Path;
				else return Path[(seppos + 1)..];
			}
		}

		public ResourceMetadata Metadata => Domain.GetMetadata(this);

		public ResourceLocation(ResourceDomain domain, string path) {
			Domain = domain;
			Path = path;
		}

		public ResourceLocation(ResourceLocation context, string subpath) {
			Domain = context.Domain;
			Path = context.Path + "/" + subpath;
		}

		public Stream OpenStream() => Domain.OpenStream(this);

		public byte[] ReadFully() {
			using Stream stream = OpenStream();
			using MemoryStream mstream = new MemoryStream();
			stream.CopyTo(mstream);
			return mstream.ToArray();
		}

		public IEnumerable<ResourceLocation> EnumerateDirectory() => Domain.EnumerateDirectory(this);

		public void RecurseDirectory(Action<ResourceLocation> forEach) {
			foreach(ResourceLocation subfile in EnumerateDirectory()) {
				forEach(subfile);
				subfile.RecurseDirectory(forEach);
			}
		}

		public bool Equals(ResourceLocation location) => Domain == location.Domain && Path == location.Path;

		public override string ToString() => Domain.ToString() + ":" + Path;

		public override bool Equals(object obj) => obj is ResourceLocation location && Equals(location);

		public override int GetHashCode() => Domain.GetHashCode() ^ Path.GetHashCode();

		public static bool operator ==(ResourceLocation left, ResourceLocation right) => left.Equals(right);

		public static bool operator !=(ResourceLocation left, ResourceLocation right) => !(left == right);

	}
}

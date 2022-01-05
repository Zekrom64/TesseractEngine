﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Tesseract.Core.Util;

namespace Tesseract.Core.Resource {

	/// <summary>
	/// <para>A resource domain specifies how resources are accessed from some particular source.</para>
	/// <para>
	/// The default resource domain is a globally set domain that will be used if no specific
	/// domain is specified as part of a resource location. It must only be set once, preferrably
	/// when the program is first started.
	/// </para>
	/// <para>
	/// Each thread also has its own 'contextual' domain which is initialized to the default domain
	/// and may be modified in a thread-local manner and will be used to initialize resource
	/// locations when no domain is specified. This contextual domain is useful in situations
	/// such as mod loading where the domain can be inferred as being the loaded mod.
	/// </para>
	/// </summary>
	public abstract class ResourceDomain {

		private static readonly ThreadLocal<ResourceDomain> contextualDomain = new(() => NullResourceDomain.Instance);
		private static readonly Dictionary<string, ResourceDomain> allDomains = new();

		/// <summary>
		/// The current contextual resource domain.
		/// </summary>
		public static ResourceDomain Current {
			get => contextualDomain.Value!;
			set {
				if (value != null) contextualDomain.Value = value;
			}
		}

		/// <summary>
		/// The default resource domain.
		/// </summary>
		public static ResourceDomain? Default { get; private set; }

		/// <summary>
		/// The global set of all registered resource domains.
		/// </summary>
		public static IReadOnlyDictionary<string, ResourceDomain> All {
			get {
				lock (allDomains) {
					// Return copy, domains *could* change at runtime
					return new Dictionary<string, ResourceDomain>(allDomains);
				}
			}
		}

		/// <summary>
		/// Sets the default resource domain.
		/// </summary>
		/// <param name="domain">The default resource domain</param>
		/// <exception cref="InvalidOperationException">If the default resource domain is already set</exception>
		[ThreadSafety(ThreadSafetyLevel.SingleThread)]
		public static void SetDefaultDomain(ResourceDomain domain) {
			if (Default == null) Default = domain;
			else throw new InvalidOperationException("Cannot change default resource domain once set");
		}

		/// <summary>
		/// Adds a domain to the global set of domains.
		/// </summary>
		/// <param name="domain">Domain to add</param>
		/// <exception cref="ArgumentException">If a resource domain with the same name already exists</exception>
		public static void AddDomain(ResourceDomain domain) {
			lock (allDomains) {
				string name = domain.Name;
				if (allDomains.ContainsKey(name)) throw new ArgumentException($"Resource domain \"{name}\" already exists", nameof(domain));
				allDomains[name] = domain;
			}
		}

		/// <summary>
		/// Removes a domain from the global set of domains.
		/// </summary>
		/// <param name="name">Name of the domain to remove</param>
		public static void RemoveDomain(string name) {
			lock (allDomains) {
				if (allDomains.Remove(name, out ResourceDomain? domain)) domain!.OnRemoved?.Invoke();
			}
		}

		/// <summary>
		/// Event called when the domain is removed from the global set of domains.
		/// </summary>
		public event Action? OnRemoved;

		/// <summary>
		/// Sets the current contextual domain given the name of a domain.
		/// </summary>
		/// <param name="name">The name of the domain to set</param>
		public static void SetCurrentDomain(string name) {
			lock (allDomains) {
				Current = allDomains!.GetValueOrDefault(name, Default)!;
			}
		}

		/// <summary>
		/// The name of the resource domain.
		/// </summary>
		public string Name { get; }

		protected ResourceDomain(string name) {
			Name = name;
		}

		/// <summary>
		/// Opens a resource location from this domain as a stream.
		/// </summary>
		/// <param name="file">The resource to open as a stream</param>
		/// <returns>The stream accessing the resource location</returns>
		/// <exception cref="ArgumentException">If the resource location is from a different domain</exception>
		public abstract Stream OpenStream(ResourceLocation file);

		/// <summary>
		/// Enumerates a resource location as a directory.
		/// </summary>
		/// <param name="dir">The resource to enumerate as a directory</param>
		/// <returns>An enumerable object of the resource locations in the directory</returns>
		/// <exception cref="ArgumentException">If the resource location is from a different domain</exception>
		public abstract IEnumerable<ResourceLocation> EnumerateDirectory(ResourceLocation dir);

		/// <summary>
		/// Gets the metadata for a resource.
		/// </summary>
		/// <param name="file">Resource to get metadata for</param>
		/// <returns>The metadata for the given resource</returns>
		/// <exception cref="ArgumentException">If the resource location is from a different domain</exception>
		public abstract ResourceMetadata GetMetadata(ResourceLocation file);

		public override string ToString() => Name;

	}

	public class NullResourceDomain : ResourceDomain {

		public static readonly NullResourceDomain Instance = new();

		private NullResourceDomain() : base("null") { }

		public override IEnumerable<ResourceLocation> EnumerateDirectory(ResourceLocation dir) => Collections<ResourceLocation>.EmptyList;

		public override ResourceMetadata GetMetadata(ResourceLocation file) => new() { Size = -1, Local = true };

		public override Stream OpenStream(ResourceLocation file) => throw new IOException("Cannot open stream from null resource domain");

	}

	/// <summary>
	/// <para>A resource location identifies where a resource is stored.</para>
	/// <para>
	/// Each location has a domain component which determines where the resources are stored and a
	/// path component which determines which resource within the domain is identified. These can
	/// be expressed as a string of the form <c>&lt;domain&gt;:&lt;path&gt;/&lt;to&gt;/&lt;resource&gt;</c>,
	/// or as only the path and the domain will be inferred from <see cref="ResourceDomain.Current"/>.
	/// </para>
	/// </summary>
	public struct ResourceLocation : IEquatable<ResourceLocation> {

		/// <summary>
		/// The domain component of this resource location.
		/// </summary>
		public ResourceDomain Domain { get; }

		/// <summary>
		/// The path component of this resource location.
		/// </summary>
		public string Path { get; }

		/// <summary>
		/// The parent directory of this resource.
		/// </summary>
		public ResourceLocation Parent {
			get {
				int seppos = Path.LastIndexOf('/');
				if (seppos == -1) return new ResourceLocation(Domain, "");
				else return new ResourceLocation(Domain, Path[..seppos]);
			}
		}

		/// <summary>
		/// The name of the resource.
		/// </summary>
		public string Name {
			get {
				int seppos = Path.LastIndexOf('/');
				if (seppos == -1) return Path;
				else return Path[(seppos + 1)..];
			}
		}

		/// <summary>
		/// The metadata of the resource.
		/// </summary>
		public ResourceMetadata Metadata => Domain.GetMetadata(this);

		/// <summary>
		/// Creates a new resource location from a resource domain and path.
		/// </summary>
		/// <param name="domain">The resource domain to use</param>
		/// <param name="path">Path to the resource</param>
		public ResourceLocation(ResourceDomain domain, string path) {
			Domain = domain;
			Path = path;
		}

		/// <summary>
		/// Creates a new resource location from a parent resource location and a subpath.
		/// The new resource location will be in the same domain as the parent.
		/// </summary>
		/// <param name="context">The parent resource</param>
		/// <param name="subpath">The subpath within the parent resource</param>
		public ResourceLocation(ResourceLocation context, string subpath) {
			Domain = context.Domain;
			Path = context.Path + "/" + subpath;
		}

		/// <summary>
		/// Creates a new resource location in the current domain from a path.
		/// </summary>
		/// <param name="path">Path to the resource</param>
		public ResourceLocation(string path) {
			Domain = ResourceDomain.Current;
			Path = path;
		}

		/// <summary>
		/// Opens a stream accessing this resource.
		/// </summary>
		/// <returns>Stream of resource data</returns>
		public Stream OpenStream() => Domain.OpenStream(this);

		/// <summary>
		/// Reads the resource fully and returns the resource data.
		/// </summary>
		/// <returns>Resuorce data</returns>
		public byte[] ReadFully() {
			using Stream stream = OpenStream();
			using MemoryStream mstream = new();
			stream.CopyTo(mstream);
			return mstream.ToArray();
		}

		/// <summary>
		/// Enumerates this resource location as a directory.
		/// </summary>
		/// <returns>Enumerable object of the child resources in this directory</returns>
		public IEnumerable<ResourceLocation> EnumerateDirectory() => Domain.EnumerateDirectory(this);

		/// <summary>
		/// Recursively enumerates this resource location as a directory, invoking the given callback
		/// for every resource found.
		/// </summary>
		/// <param name="forEach">Action to invoke for every resource</param>
		public void RecurseDirectory(Action<ResourceLocation> forEach) {
			foreach (ResourceLocation subfile in EnumerateDirectory()) {
				forEach(subfile);
				subfile.RecurseDirectory(forEach);
			}
		}

		public bool Equals(ResourceLocation location) => Domain == location.Domain && Path == location.Path;

		public override string ToString() => Domain.ToString() + ":" + Path;

		public override bool Equals(object? obj) => obj is ResourceLocation location && Equals(location);

		public override int GetHashCode() => Domain.GetHashCode() ^ Path.GetHashCode();

		public static bool operator ==(ResourceLocation left, ResourceLocation right) => left.Equals(right);

		public static bool operator !=(ResourceLocation left, ResourceLocation right) => !(left == right);

	}
}

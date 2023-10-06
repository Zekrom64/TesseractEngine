using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Resource;

namespace Tesseract.Core.Engine.Registry {

	/// <summary>
	/// A registry key is a string which identifies an entity mapped in a registry.
	/// </summary>
	public readonly struct RegistryKey : IEquatable<RegistryKey> {

		/// <summary>
		/// The name of the registry this key is associated with, or null if none is associated.
		/// This is mostly a semantic value but can differentiate between keys with identical
		/// paths in different registries.
		/// </summary>
		public string? RegistryName { get; init; }

		/// <summary>
		/// The namespace within the registry this key belongs to.
		/// </summary>
		public string NameSpace { get; }

		/// <summary>
		/// The path within the namespace of the registry this key has.
		/// </summary>
		public string Path { get; }

		// The complete string ID
		private readonly string key;

		/// <summary>
		/// Creates a registry key by parsing the given path using the format <tt>[&lt;registry&gt;:]&lt;namespace&gt;:&lt;path&gt;</tt>.
		/// If no namespace is specified the name of the current resource domain via <see cref="ResourceDomain.Current"/> is used.
		/// </summary>
		/// <param name="path">The path of the registry key</param>
		/// <exception cref="ArgumentException">If the path is in an invalid format</exception>
		public RegistryKey(string path) {
			string[] parts = path.Split(':');
			switch(parts.Length) {
				case 1:
					RegistryName = null;
					NameSpace = ResourceDomain.Current.Name;
					Path = parts[0];
					break;
				case 2:
					RegistryName = null;
					NameSpace = parts[0];
					Path = parts[1];
					break;
				case 3:
					RegistryName = parts[0];
					NameSpace = parts[1];
					Path = parts[2];
					break;
				default:
					throw new ArgumentException($"Invalid registry key \"{path}\"");
			}
			key = path;
		}

		/// <summary>
		/// Creates a registry key with no registry name, and a namespace and path.
		/// </summary>
		/// <param name="nameSpace">The namespace of the key</param>
		/// <param name="path">The path of the key</param>
		/// <exception cref="ArgumentException">If the namespace or path are invalid</exception>
		public RegistryKey(string nameSpace, string path) {
			if (nameSpace.Contains(':')) throw new ArgumentException("Name space cannot have a ':' in it", nameof(nameSpace));
			if (path.Contains(':')) throw new ArgumentException("Path cannot have a ':' in it", nameof(path));
			NameSpace = nameSpace;
			Path = path;
			key = nameSpace + ':' + path;
		}

		public bool Equals(RegistryKey other) {
			if (RegistryName != null && other.RegistryName != null && RegistryName != other.RegistryName) return false;
			return NameSpace == other.NameSpace && Path == other.Path;
		}

		/// <summary>
		/// Returns a registry key with the same namespace and path but with no defined the registry name.
		/// </summary>
		/// <returns>Key without registry name</returns>
		public RegistryKey WithoutRegistryName() {
			if (RegistryName == null) return this;
			else return new RegistryKey(NameSpace, Path);
		}

		public override string ToString() => key;

		public override bool Equals([NotNullWhen(true)] object? obj) => obj is RegistryKey key && Equals(key);

		public override int GetHashCode() => key.GetHashCode();

		public static bool operator==(RegistryKey left, RegistryKey right) => left.Equals(right);

		public static bool operator!=(RegistryKey left, RegistryKey right) => !(left == right);

		public static implicit operator RegistryKey(string str) => new(str);

	}

}

using System;
using System.Collections.Generic;

namespace Tesseract.Core.Services {

	public interface IService<T> { }

	public interface IServiceProvider {

		public T? GetService<T>(IService<T> service) => default;

	}

	public class OpaqueService<T> : IService<T> { }

	public class GlobalServices : IServiceProvider {

		private static readonly Dictionary<object, object> services = new();

		public static readonly GlobalServices Instance = new();

		private GlobalServices() { }

		public T? GetService<T>(IService<T> service) {
			if (services.TryGetValue(service, out object? val)) return (T)val;
			else return default;
		}

		public static void AddGlobalService<T>(IService<T> service, T value) {
			if (value == null) throw new ArgumentNullException(nameof(value), "Cannot register a global service");
			services[service] = value;
		}

	}

}

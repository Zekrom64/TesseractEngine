using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Services {

	public interface IService<T> { }

	public interface IServiceProvider {

		public T GetService<T>(IService<T> service);
	
	}

	public class OpaqueService<T> : IService<T> { }

	public class GlobalServices : IServiceProvider {

		private static readonly Dictionary<object, object> services = new();

		public static readonly GlobalServices Instance = new();

		private GlobalServices() { }

		public T GetService<T>(IService<T> service) {
			if (services.TryGetValue(service, out object val)) return (T)val;
			else return default;
		}

		public static void AddGlobalService<T>(IService<T> service, T value) => services[service] = value;

	}

}

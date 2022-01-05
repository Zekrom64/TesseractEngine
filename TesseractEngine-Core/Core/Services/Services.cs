using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Services {

	public interface IService<T> { }

	public interface IServiceProvider {

		public T GetService<T>(IService<T> service) {
			Func<IServiceProvider,T> injectedGetter = ServiceInjector.Lookup<T>(this, service);
			if (injectedGetter != null) return injectedGetter(this);
			else return default;
		}
	
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

	public class ServiceInjector {

		private static readonly Dictionary<(Type, Type), object> registry = new();

		public static void Inject<P,S,T>(Func<P,T> getter) where P : IServiceProvider where S : IService<T> {
			registry[(typeof(P),typeof(S))] = getter;
		}

		public static Func<IServiceProvider,T> Lookup<T>(IServiceProvider provider, IService<T> service) {
			Type tProvider = provider.GetType(), tService = service.GetType();
			if (registry.TryGetValue((tProvider, tService), out object fn)) {
				MethodInfo mInvoke = fn.GetType().GetMethod("Invoke");
				return (IServiceProvider p) => (T)mInvoke.Invoke(fn, new object[] { p });
			} else return null;
		}

	}

}

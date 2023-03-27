using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Tesseract.Core.Services {

	/// <summary>
	/// Non-generic interface for <see cref="IService{T}"/>.
	/// </summary>
	public interface IService { }

	/// <summary>
	/// A <i>service</i> identifies a type of object which provides certain functionality or features. Services
	/// can be retrieved from an <see cref="IServiceProvider"/>.
	/// </summary>
	/// <typeparam name="T">Service object type</typeparam>
	public interface IService<T> : IService where T : notnull { }

	/// <summary>
	/// An opaque service is a 
	/// </summary>
	/// <typeparam name="T">Service object type</typeparam>
	public class OpaqueService<T> : IService<T> where T : notnull { }

	/// <summary>
	/// This class holds references to 'global' services; services that are available globally and not tied to a single object.
	/// </summary>
	public class GlobalServices : IServiceProvider {

		private static readonly Dictionary<object, object> services = new();

		/// <summary>
		/// Instance of global services that can be used as an <see cref="IServiceProvider"/>.
		/// </summary>
		public static readonly GlobalServices Instance = new();

		public event Action? OnServicesUnload;

		private GlobalServices() {
			AppDomain.CurrentDomain.ProcessExit += (o, e) => OnServicesUnload?.Invoke();
		}

		public T? GetService<T>(IService<T> service) where T : notnull {
			if (services.TryGetValue(service, out object? val)) return (T)val;
			else return default;
		}

		/// <summary>
		/// Static shortcut for <see cref="GetService{T}(IService{T})"/>.
		/// </summary>
		/// <typeparam name="T">Service object type</typeparam>
		/// <param name="service">The global service to get</param>
		/// <returns>The registered global service, or null if none exists</returns>
		public static T? GetGlobalService<T>(IService<T> service) where T : notnull => Instance.GetService<T>(service);

		/// <summary>
		/// Adds a new global service.
		/// </summary>
		/// <typeparam name="T">Service object type</typeparam>
		/// <param name="service">The global service to add</param>
		/// <param name="value">The global service object</param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void AddGlobalService<T>(IService<T> service, T value) where T : notnull => services[service] = value;

	}

	/// <summary>
	/// A <i>service provider</i> is an object that can provide different services. Many objects which
	/// implement this interface will implement their services as interfaces themselves. However, the
	/// services architecture allows for 
	/// </summary>
	public interface IServiceProvider {

		/// <summary>
		/// Attempts to get a service from this provider. If no such service is found, null is returned.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="service">The service to get</param>
		/// <returns></returns>
		public virtual T? GetService<T>(IService<T> service) where T : notnull => ServiceInjector.Lookup(this, service);

	}

	/// <summary>
	/// The service injector provides functionality to inject new services into existing service providers.
	/// </summary>
	public static class ServiceInjector {

		private class InjectedRegistry {

			// Type of the associated object for this registry
			public required Type SelfType { get; init; }
			// Dictionary of injected objects for this object by service
			public readonly Dictionary<IService, object> InjectedObjects = new();

			private delegate object AbstractConstructor(IServiceProvider provider);

			public T? Lookup<T>(IService<T> service, IServiceProvider provider) where T : notnull {
				lock (this) {
					if (InjectedObjects.TryGetValue(service, out object? value)) return (T)value;
					else {
						if (constructors.TryGetValue((SelfType, service), out Func<IServiceProvider, object>? ctor)) {
							T tvalue = (T)ctor.Invoke(provider);
							InjectedObjects.Add(service, tvalue);
							return tvalue;
						} else return default;
					}
				}
			}

		}

		// Dictionary of injected service constructors by the provider type and service object
		private static readonly Dictionary<(Type, IService), Func<IServiceProvider, object>> constructors = new();
		// Dictionary of injected registries per provider
		private static readonly ConditionalWeakTable<IServiceProvider, InjectedRegistry> registry = new();

		/// <summary>
		/// Injects the ability to use service with a type of service provider.
		/// </summary>
		/// <typeparam name="P">Service provider type</typeparam>
		/// <typeparam name="T">Service object type</typeparam>
		/// <param name="service">The service to add</param>
		/// <param name="getter">The getter function for the injected service</param>
		public static void Inject<P, T>(IService<T> service, Func<P, T> getter)
			where P : IServiceProvider
			where T : notnull {
			constructors[(typeof(P), service)] = (IServiceProvider p) => getter((P)p);
		}

		/// <summary>
		/// Attempts to lookup an injected service for an object.
		/// </summary>
		/// <typeparam name="T">Service object type</typeparam>
		/// <param name="provider">The service provider to lookup for</param>
		/// <param name="service">The service to lookup</param>
		/// <returns>The retrieved injected service, or null if none was found</returns>
		public static T? Lookup<T>(IServiceProvider provider, IService<T> service) where T : notnull {
			// Fast lookup for objects that directly implement the service as an interface
			if (provider is T tval) return tval;
			lock (registry) {
				if (!registry.TryGetValue(provider, out InjectedRegistry? reg)) {
					reg = new() { SelfType = provider.GetType() };
					registry.Add(provider, reg);
				}
				return reg.Lookup(service, provider);
			}
		}

	}

}

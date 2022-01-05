using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Tesseract.Core.Util;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// A graphics provider offers a specific 
	/// </summary>
	public interface IGraphicsProvider {

		/// <summary>
		/// Graphics properties for the provided graphics.
		/// </summary>
		public IGraphicsProperites Properties { get; }

		/// <summary>
		/// Supported graphics features for the provided graphics.
		/// </summary>
		public IGraphicsFeatures Features { get; }

		/// <summary>
		/// Graphics limits for the provided graphics.
		/// </summary>
		public IGraphicsLimits Limits { get; }

		/// <summary>
		/// A human-readable name identifying this graphics provider.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// A unique ID for this graphics provider generated from its properties. This ID <b>could change
		/// due to driver changes</b>, but is generally stable and may be used to try and locate the same
		/// provider between program runs without enumerating every provider.
		/// </summary>
		public Guid UniqueID { get; }

		/// <summary>
		/// If multiple graphics instances may be instantiated from this provider. Otherwise only
		/// one graphics instance may be instantiated.
		/// </summary>
		public bool MultiGraphics { get; }

		/// <summary>
		/// Creates a new instance of these graphics from the current provider.
		/// </summary>
		/// <param name="createInfo">Graphics creation information</param>
		/// <returns></returns>
		public IGraphics CreateGraphics(GraphicsCreateInfo createInfo);

		/// <summary>
		/// Gets information describing swapchain support for the given window. If swapchain
		/// creation is not supported for the given window, null is returned.
		/// </summary>
		/// <param name="graphics">The graphics the swapchain will be used with</param>
		/// <param name="window">Window to check for swapchain support</param>
		/// <returns>Swapchain support information, or null</returns>
		public SwapchainSupportInfo? GetSwapchainSupport(IGraphics graphics, IWindow window);

		/// <summary>
		/// Creates a new swapchain compatible with graphics instantiated from this provider.
		/// </summary>
		/// <param name="graphics">The graphics the swapchain will be used with</param>
		/// <param name="createInfo">Swapchain creation information</param>
		/// <returns></returns>
		public ISwapchain CreateSwapchain(IGraphics graphics, SwapchainCreateInfo createInfo);

	}

	/// <summary>
	/// Graphics creation information.
	/// </summary>
	public record GraphicsCreateInfo {

		/// <summary>
		/// The hardware features that should be enabled during creation. If null no
		/// additional features will be enabled.
		/// </summary>
		public GraphicsHardwareFeatures? EnabledFeatures { get; init; }

		/// <summary>
		/// Extended graphics creation information, or null if none is given.
		/// </summary>
		public IExtendedGraphicsCreateInfo? ExtentedCreateInfo { get; init; } = null;

	}

	/// <summary>
	/// Swapchain support information.
	/// </summary>
	public record SwapchainSupportInfo {

		/// <summary>
		/// The supported presentation modes.
		/// </summary>
		public IReadOnlyCollection<SwapchainPresentMode> SupportedPresentModes { get; init; } = Collections<SwapchainPresentMode>.EmptyList;

		/// <summary>
		/// The type of images used by the swapchain.
		/// </summary>
		public SwapchainImageType ImageType { get; init; }

		/// <summary>
		/// Bitmask of supported usages of images in image-based swapchains.
		/// </summary>
		public TextureUsage SupportedImageUsage { get; init; }

	}

	/// <summary>
	/// Swapchain creation information.
	/// </summary>
	public record SwapchainCreateInfo {

		/// <summary>
		/// The window to present to.
		/// </summary>
		public IWindow PresentWindow { get; init; } = null!;

		/// <summary>
		/// The presentation mode to use.
		/// </summary>
		public SwapchainPresentMode PresentMode { get; init; }

		/// <summary>
		/// Bitmask of the required usages for each image in image-based swapchains.
		/// </summary>
		public TextureUsage ImageUsage { get; init; }

	}

	/// <summary>
	/// Interface for classes that provide extended graphics creation information.
	/// </summary>
	public interface IExtendedGraphicsCreateInfo { }

	/// <summary>
	/// A graphics enumerator manages the state for enumerating supported graphics providers. Because this
	/// may involve the creation of intermediate objects (contexts, window surfaces) this interface is also
	/// disposable and must be kept alive with any objects derived from it.
	/// </summary>
	public interface IGraphicsEnumerator : IDisposable {

		/// <summary>
		/// Enumerates all of the providers offered for the given enumeration information.
		/// </summary>
		/// <returns>Enumerable of graphics providers</returns>
		public IEnumerable<IGraphicsProvider> EnumerateProviders();

		/// <summary>
		/// Attempts to get a graphics provider from this enumerator based on its unique ID.
		/// </summary>
		/// <param name="uniqueID">Unique ID of the graphics provider</param>
		/// <param name="provider">The graphics provider identified by its unique ID, or null</param>
		/// <returns>If the graphics provider was found</returns>
		public bool TryGetProvider(Guid uniqueID, [NotNullWhen(true)] out IGraphicsProvider? provider);

	}

	/// <summary>
	/// A graphics enumerator that will always be empty.
	/// </summary>
	public class EmptyGraphicsEnumerator : IGraphicsEnumerator {

		[SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Dispose is a no-op, only used to fulfill the contract of IGraphicsEnumerator")]
		public void Dispose() { }

		public IEnumerable<IGraphicsProvider> EnumerateProviders() => Collections<IGraphicsProvider>.EmptyList;

		public bool TryGetProvider(Guid uniqueID, [NotNullWhen(true)] out IGraphicsProvider? provider) {
			provider = null;
			return false;
		}
	}

	/// <summary>
	/// <para>
	/// Attribute applied to classes that can act as graphics enumerators.
	/// </para>
	/// <para>
	/// Classes with this attribute must have a public static method called 'GetEnumerator' which
	/// takes a single parameter of the <see cref="GraphicsEnumeratorCreateInfo"/> class.
	/// </para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class GraphicsEnumeratorAttribute : Attribute { }

	/// <summary>
	/// Graphics provider enumerator creation information.
	/// </summary>
	public record GraphicsEnumeratorCreateInfo {

		/// <summary>
		/// The window the graphics will draw to. If null, this indicates that
		/// graphics will be requested in "headless" mode; nothing will be
		/// presented to the user.
		/// </summary>
		public IWindow? Window { get; init; }

	}

	/// <summary>
	/// Static class for managing creation of <see cref="IGraphicsEnumerator"/> instances. This will find classes
	/// that can provide enumerators as marked by <see cref="GraphicsEnumeratorAttribute"/> and will merge their
	/// outputs into a single graphics enumerator providing all known graphics instances for the given creation
	/// information.
	/// </summary>
	public static class GraphicsEnumerator {

		private static readonly List<Func<GraphicsEnumeratorCreateInfo,IGraphicsEnumerator>> enumerators = new();

		private static void LoadEnumerators(Assembly asm) {
			lock (enumerators) {
				foreach (Type type in asm.GetTypes()) {
					if (type.GetCustomAttribute<GraphicsEnumeratorAttribute>() != null) {
						MethodInfo? method = type.GetMethod("GetEnumerator", BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(GraphicsEnumeratorCreateInfo) });
						if (method != null) enumerators.Add((GraphicsEnumeratorCreateInfo createInfo) => {
							if (method.Invoke(null, new object[] { createInfo }) is not IGraphicsEnumerator e) throw new NullReferenceException("Created graphics enumerator must not be null");
							return e;
						});
					}
				}
			}
		}

		private static void OnAssemblyLoad(object? sender, AssemblyLoadEventArgs e) => LoadEnumerators(e.LoadedAssembly);

		static GraphicsEnumerator() {
			AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
			foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies()) LoadEnumerators(asm);
		}

		// Combines several graphics enumerators as one
		private class MultiGraphicsEnumerator : IGraphicsEnumerator {

			public List<IGraphicsEnumerator> Enumerators { get; init; } = null!;

			public void Dispose() {
				foreach (IGraphicsEnumerator e in Enumerators) e.Dispose();
			}

			public IEnumerable<IGraphicsProvider> EnumerateProviders() {
				foreach(IGraphicsEnumerator e in Enumerators)
					foreach(IGraphicsProvider p in e.EnumerateProviders()) yield return p;
			}

			public bool TryGetProvider(Guid uniqueID, [NotNullWhen(true)] out IGraphicsProvider? provider) {
				provider = null;
				foreach(IGraphicsEnumerator e in Enumerators) if (e.TryGetProvider(uniqueID, out provider)) return true;
				return false;
			}

		}

		/// <summary>
		/// Creates a graphics enumerator using the given create info.
		/// </summary>
		/// <param name="createInfo">Graphics enumerator creation information</param>
		/// <returns>Graphics enumerator</returns>
		public static IGraphicsEnumerator Create(GraphicsEnumeratorCreateInfo createInfo) {
			lock (enumerators) {
				List<IGraphicsEnumerator> enums = new();
				foreach (var func in enumerators) enums.Add(func(createInfo));
				return new MultiGraphicsEnumerator() { Enumerators = enums };
			}
		}

	}

}

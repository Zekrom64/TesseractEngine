using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Resource {

	/// <summary>
	/// An HTTP resource domain maps resources to HTTP requests at a specified URL.
	/// </summary>
	public class HTTPResourceDomain : ResourceDomain, IDisposable {

		/// <summary>
		/// The HTTP client used to fetch resources for this domain.
		/// </summary>
		public HttpClient HttpClient { get; } = new();

		/// <summary>
		/// The timeout to apply when reading resources from this domain.
		/// </summary>
		public TimeSpan Timeout { get; }

		/// <summary>
		/// The URL prefix to apply to paths within this domain.
		/// </summary>
		public string URLPrefix { get; }

		/// <summary>
		/// Creates a new HTTP resource domain.
		/// </summary>
		/// <param name="name">The name of the resource domain</param>
		/// <param name="prefix">The URL prefix to apply to paths in this domain</param>
		/// <param name="timeout">The timeout to use waiting for resources</param>
		/// <exception cref="ArgumentException">If an invalid argument was passed</exception>
		public HTTPResourceDomain(string name, string prefix, TimeSpan timeout) : base(name) {
			Timeout = timeout;
			URLPrefix = prefix;
			if (!prefix.StartsWith("http://") || !prefix.StartsWith("https://")) throw new ArgumentException($"Invalid URL prefix for HTTP resource domain: \"{prefix}\"", nameof(prefix));
		}

		public override IEnumerable<ResourceLocation> EnumerateDirectory(ResourceLocation dir) {
			if (dir.Domain != this) throw new ArgumentException("Cannot operate on a resource location from a different domain", nameof(dir));
			return Collections<ResourceLocation>.EmptyList;
		}

		public override ResourceMetadata GetMetadata(ResourceLocation file) {
			if (file.Domain != this) throw new ArgumentException("Cannot operate on a resource location from a different domain", nameof(file));
			MIME.TryGuessFromExtension(FileResourceDomain.GetExtensionFromFileName(file.Name), out string? mime);
			return new ResourceMetadata() {
				MIMEType = mime
			};
		}

		public override Stream OpenStream(ResourceLocation file) {
			if (file.Domain != this) throw new ArgumentException("Cannot operate on a resource location from a different domain", nameof(file));
			var areq = HttpClient.GetAsync(URLPrefix + PathPrefix + file.Path);
			if (!areq.Wait(Timeout)) throw new IOException("Timeout waiting for HTTP request");
			var request = areq.Result;
			if (request.StatusCode != HttpStatusCode.OK) throw new IOException($"URL resource returned non-ok status code {request.StatusCode}");
			return request.Content.ReadAsStream();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			HttpClient.Dispose();
		}

		public override bool Exists(ResourceLocation file) {
			var areq = HttpClient.GetAsync(URLPrefix + PathPrefix + file.Path);
			if (!areq.Wait(Timeout)) return false;
			return areq.Result.IsSuccessStatusCode;
		}
	}

}

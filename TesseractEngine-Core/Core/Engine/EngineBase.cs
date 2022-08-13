using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Resource;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Engine {
	
	public record class EngineCreateInfo {

		public IReadOnlyCollection<ResourceDomain> ResourceDomains { get; init; } = Collections<ResourceDomain>.EmptyList;

	}

	public abstract class EngineBase {

		protected readonly Dictionary<string, ResourceDomain> ResourceDomains = new();

		protected EngineBase(EngineCreateInfo createInfo) {
			foreach (ResourceDomain domain in createInfo.ResourceDomains) AddResourceDomain(domain);
		}

		public void AddResourceDomain(ResourceDomain domain) {
			if (ResourceDomains.ContainsKey(domain.Name)) throw new ArgumentException($"Engine already has resource domain \"{domain.Name}\"", nameof(domain));
			ResourceDomains[domain.Name] = domain;
		}

	}

}

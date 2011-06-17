using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;

namespace ManagedFusion.Security
{
	public class SectionSecurityProviderCollection : ProviderCollection
	{
		public new SectionSecurityProvider this[string name]
		{
			get { return base[name] as SectionSecurityProvider; }
		}

		public override void Add(ProviderBase provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (provider is SectionSecurityProvider == false)
				throw new ArgumentException("Invalid provider type.", "provider");

			base.Add(provider);
		}
	}
}

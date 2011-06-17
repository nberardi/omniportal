using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;

namespace ManagedFusion.Configuration
{
	public class CommunityConfigurationProviderCollection : ProviderCollection
	{
		public new CommunityConfigurationProvider this[string name]
		{
			get { return base[name] as CommunityConfigurationProvider; }
		}

		public override void Add(ProviderBase provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (provider is CommunityConfigurationProvider == false)
				throw new ArgumentException("Invalid provider type.", "provider");

			base.Add(provider);
		}
	}
}

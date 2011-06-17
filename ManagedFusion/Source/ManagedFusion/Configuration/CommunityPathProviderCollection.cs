using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;

namespace ManagedFusion.Configuration
{
	public class CommunityPathProviderCollection : ProviderCollection
	{
		public new CommunityPathProvider this[string name]
		{
			get { return base[name] as CommunityPathProvider; }
		}

		public override void Add(ProviderBase provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (provider is CommunityPathProvider == false)
				throw new ArgumentException("Invalid provider type.", "provider");

			base.Add(provider);
		}
	}
}

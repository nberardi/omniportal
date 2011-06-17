using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;

namespace ManagedFusion.Security
{
	public class PortletSecurityProviderCollection : ProviderCollection
	{
		public new PortletSecurityProvider this[string name]
		{
			get { return base[name] as PortletSecurityProvider; }
		}

		public override void Add(ProviderBase provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (provider is PortletSecurityProvider == false)
				throw new ArgumentException("Invalid provider type.", "provider");

			base.Add(provider);
		}
	}
}

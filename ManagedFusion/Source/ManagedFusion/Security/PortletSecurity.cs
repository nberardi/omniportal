using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace ManagedFusion.Security
{
	public class PortletSecurity
	{
		private static PortletSecurityProvider _provider;
		private static PortletSecurityProviderCollection _providers;
		private static object _lock = new object();

		public static PortletSecurityProvider Provider
		{
			get { return _provider; }
		}

		public static PortletSecurityProviderCollection Providers
		{
			get { return _providers; }
		}

		static PortletSecurity()
		{
			// avoid claiming lock if providers are already loaded
			if (_provider == null)
			{
				lock (_lock)
				{
					// do this again to make sure _provider is still null
					if (_provider == null)
					{
						// get a reference to the <configurationManager> section
						PortletSecurityManagerSection section = WebConfigurationManager.GetSection("managedFusion/portletSecurityManager") as PortletSecurityManagerSection;

						if (section != null)
						{
							// Load registered providers and point _provider to the default provider
							_providers = new PortletSecurityProviderCollection();
							ProvidersHelper.InstantiateProviders(section.Providers, _providers, typeof(PortletSecurityProvider));
							_provider = _providers[section.DefaultProvider];

							if (_provider == null)
								throw new ProviderException("Unable to load default PortletSecurityProvider");
						}
						else
						{
							_provider = new Portal.PortalPortletSecurityProvider();
							_providers = new PortletSecurityProviderCollection();
							_providers.Add(_provider);
						}
					}
				}
			}
		}
	}
}

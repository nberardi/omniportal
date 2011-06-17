using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace ManagedFusion.Security
{
	public class SectionSecurity
	{
		private static SectionSecurityProvider _provider;
		private static SectionSecurityProviderCollection _providers;
		private static object _lock = new object();

		public static SectionSecurityProvider Provider
		{
			get { return _provider; }
		}

		public static SectionSecurityProviderCollection Providers
		{
			get { return _providers; }
		}

		static SectionSecurity()
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
						SectionSecurityManagerSection section = WebConfigurationManager.GetSection("managedFusion/sectionSecurityManager") as SectionSecurityManagerSection;

						if (section != null)
						{
							// Load registered providers and point _provider to the default provider
							_providers = new SectionSecurityProviderCollection();
							ProvidersHelper.InstantiateProviders(section.Providers, _providers, typeof(SectionSecurityProvider));
							_provider = _providers[section.DefaultProvider];

							if (_provider == null)
								throw new ProviderException("Unable to load default SectionSecurityProvider");
						}
						else
						{
							_provider = new Portal.PortalSectionSecurityProvider();
							_providers = new SectionSecurityProviderCollection();
							_providers.Add(_provider);
						}
					}
				}
			}
		}
	}
}

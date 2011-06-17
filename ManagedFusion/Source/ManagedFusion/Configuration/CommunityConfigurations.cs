using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace ManagedFusion.Configuration
{
	public class CommunityConfigurations
	{
		private static CommunityConfigurationProvider _provider;
		private static CommunityConfigurationProviderCollection _providers;
		private static object _lock = new object();

		public static CommunityConfigurationProvider Provider
		{
			get { return _provider; }
		}

		public static CommunityConfigurationProviderCollection Providers
		{
			get { return _providers; }
		}

		public static CommunityConfiguration Default
		{
			get { return Provider.Default; }
		}

		public static CommunityConfiguration Current
		{
			get { return Provider.Current; }
		}

		public static CommunityConfiguration GetConfiguration(int id)
		{
			return Provider[id];
		}

		static CommunityConfigurations()
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
						CommunityConfigurationManagerSection section = WebConfigurationManager.GetSection("managedFusion/communityConfigurationManager") as CommunityConfigurationManagerSection;

						// Load registered providers and point _provider to the default provider
						_providers = new CommunityConfigurationProviderCollection();
						ProvidersHelper.InstantiateProviders(section.Providers, _providers, typeof(CommunityConfigurationProvider));
						_provider = _providers[section.DefaultProvider];

						if (_provider == null)
							throw new ProviderException("Unable to load default CommunityConfigurationProvider");
					}
				}
			}
		}
	}
}

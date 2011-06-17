using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace ManagedFusion.Configuration
{
	public class CommunityPaths
	{
		private static CommunityPathProvider _provider;
		private static CommunityPathProviderCollection _providers;
		private static object _lock = new object();

		public static CommunityPathProvider Provider
		{
			get { return _provider; }
		}

		public static CommunityPathProviderCollection Providers
		{
			get { return _providers; }
		}

		static CommunityPaths()
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
						CommunityPathManagerSection section = WebConfigurationManager.GetSection("managedFusion/communityPathManager") as CommunityPathManagerSection;

						// Load registered providers and point _provider to the default provider
						_providers = new CommunityPathProviderCollection();
						ProvidersHelper.InstantiateProviders(section.Providers, _providers, typeof(CommunityPathProvider));
						_provider = _providers[section.DefaultProvider];

						if (_provider == null)
							throw new ProviderException("Unable to load default CommunityPathProvider");
					}
				}
			}
		}
	}
}

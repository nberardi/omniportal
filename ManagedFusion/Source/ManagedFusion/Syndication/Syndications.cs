using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Configuration;

namespace ManagedFusion.Syndication
{
	public class Syndications
	{
		private static SyndicationProviderCollection _providers;
		private static object _lock = new object();

		public static SyndicationProviderCollection Providers
		{
			get { return _providers; }
		}

		static Syndications()
		{
			lock (_lock)
			{
				// get a reference to the <configurationManager> section
				SyndicationManagerSection section = WebConfigurationManager.GetSection("managedFusion/syndicationManager") as SyndicationManagerSection;

				// Load registered providers and point _provider to the default provider
				_providers = new SyndicationProviderCollection();
				ProvidersHelper.InstantiateProviders(section.Providers, _providers, typeof(SyndicationProvider));
			}
		}
	}
}

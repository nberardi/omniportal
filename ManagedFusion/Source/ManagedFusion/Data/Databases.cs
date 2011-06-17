using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace ManagedFusion.Data
{
	public static class Databases
	{
		private static DatabaseProvider _provider;
		private static DatabaseProviderCollection _providers;
		private static string _defaultConnectionStringName;
		private static object _lock = new object();

		public static DatabaseProvider Provider
		{
			get { return _provider; }
		}

		public static DatabaseProviderCollection Providers
		{
			get { return _providers; }
		}

		public static ConnectionStringSettings DefaultConnectionString
		{
			get { return ConfigurationManager.ConnectionStrings[_defaultConnectionStringName]; }
		}

		static Databases()
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
						DatabaseManagerSection section = WebConfigurationManager.GetSection("managedFusion/databaseManager") as DatabaseManagerSection;

						// set the connection string name
						_defaultConnectionStringName = section.DefaultConnectionStringName;

						// Load registered providers and point _provider to the default provider
						_providers = new DatabaseProviderCollection();
						ProvidersHelper.InstantiateProviders(section.Providers, _providers, typeof(DatabaseProvider));
						_provider = _providers[section.DefaultProvider];

						if (_provider == null)
							throw new ProviderException("Unable to load default DatabaseProvider");
					}
				}
			}
		}
	}
}

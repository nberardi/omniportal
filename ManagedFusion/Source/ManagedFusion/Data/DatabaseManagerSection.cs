using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ManagedFusion.Data
{
	public class DatabaseManagerSection : ConfigurationSection
	{
		[ConfigurationProperty("defaultProvider", IsRequired = true)]
		public string DefaultProvider
		{
			get { return base["defaultProvider"] as string; }
		}

		[ConfigurationProperty("defaultConnectionStringName", IsRequired = true)]
		public string DefaultConnectionStringName
		{
			get { return base["defaultConnectionStringName"] as string; }
		}

		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get { return base["providers"] as ProviderSettingsCollection; }
		}
	}
}

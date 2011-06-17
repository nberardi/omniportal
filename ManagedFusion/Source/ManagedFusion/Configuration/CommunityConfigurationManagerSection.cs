using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ManagedFusion.Configuration
{
	public class CommunityConfigurationManagerSection : ConfigurationSection
	{
		[ConfigurationProperty("defaultProvider", DefaultValue = "Folder", IsRequired = true)]
		public string DefaultProvider
		{
			get { return base["defaultProvider"] as string; }
		}

		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get { return base["providers"] as ProviderSettingsCollection; }
		}
	}
}

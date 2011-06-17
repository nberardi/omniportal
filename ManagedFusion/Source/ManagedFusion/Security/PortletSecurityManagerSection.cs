using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ManagedFusion.Security
{
	public class PortletSecurityManagerSection : ConfigurationSection
	{
		[ConfigurationProperty("defaultProvider", IsRequired = true)]
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ManagedFusion.Syndication
{
	public class SyndicationManagerSection : ConfigurationSection
	{
		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get { return base["providers"] as ProviderSettingsCollection; }
		}
	}
}

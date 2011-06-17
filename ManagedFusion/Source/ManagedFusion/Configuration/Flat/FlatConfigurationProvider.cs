using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Configuration.Flat
{
	internal class FlatConfigurationProvider : CommunityConfigurationProvider
	{
		private CommunityConfigurationCollection _collection;
		public override CommunityConfigurationCollection CommunityConfigurations
		{
			get
			{
				if (_collection == null)
					_collection = new CommunityConfigurationCollection();

				// this will always return the default configuration defined in the web.config
				return _collection;
			}
		}
	}
}

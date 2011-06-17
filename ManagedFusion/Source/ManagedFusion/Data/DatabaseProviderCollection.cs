using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;

namespace ManagedFusion.Data
{
	public class DatabaseProviderCollection : ProviderCollection
	{
		public new DatabaseProvider this[string name]
		{
			get { return base[name] as DatabaseProvider; }
		}

		public override void Add(ProviderBase provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (provider is DatabaseProvider == false)
				throw new ArgumentException("Invalid provider type.", "provider");

			base.Add(provider);
		}
	}
}

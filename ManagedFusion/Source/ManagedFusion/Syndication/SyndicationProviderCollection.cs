using System;
using System.Collections.Generic;
using System.Configuration.Provider;

namespace ManagedFusion.Syndication
{
	public class SyndicationProviderCollection : ProviderCollection
	{
		public new SyndicationProvider this[string name]
		{
			get
			{
				// check to see if any name was entered in
				// if the name is null or empty then nothing should be returned
				if (String.IsNullOrEmpty(name))
					return null;

				name = name.ToLower();

				// remove the question mark from the query string
				if (name[0] == '?')
					name = name.Substring(1);

				// gets the syndication provider name fromt he query string
				if (name.IndexOf('&') != -1)
					name = name.Substring(0, name.IndexOf('&') + 1);

				// returns the syndication provider that was specified in the query string
				return base[name.ToLower()] as SyndicationProvider;
			}
		}

		public override void Add(ProviderBase provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (provider is SyndicationProvider == false)
				throw new ArgumentException("Invalid provider type.", "provider");

			base.Add(provider);
		}
	}
}

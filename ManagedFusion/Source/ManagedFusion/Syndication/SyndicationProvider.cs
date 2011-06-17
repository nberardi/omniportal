using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Web;

namespace ManagedFusion.Syndication
{
	public abstract class SyndicationProvider : ProviderBase
	{
		public override string Name
		{
			get { return base.Name.ToLower(); }
		}

		public virtual bool IsSyndicated { get { return true; } }

		public abstract ISyndication Syndication { get; }

		public abstract IHttpHandler Handler { get; }
	}
}

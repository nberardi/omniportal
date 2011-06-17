using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ManagedFusion.Syndication.AtomFeed
{
	class AtomFeedProvider : SyndicationProvider
	{
		private AtomFeed _atomFeed;

		public AtomFeedProvider()
		{
			_atomFeed = new AtomFeed();
		}

		public override ISyndication Syndication { get { return _atomFeed; } }

		public override IHttpHandler Handler
		{
			get { return new AtomFeedHandler(this.Syndication); }
		}
	}
}

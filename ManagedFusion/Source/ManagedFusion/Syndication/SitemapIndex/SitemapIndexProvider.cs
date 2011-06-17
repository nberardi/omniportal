using System;
using System.Text;
using System.Web;

namespace ManagedFusion.Syndication.SitemapIndex
{
	internal class SitemapIndexProvider : SyndicationProvider
	{
		private SitemapIndex _sitemapIndex;

		public SitemapIndexProvider()
		{
			_sitemapIndex = new SitemapIndex();
		}

		public override ISyndication Syndication { get { return _sitemapIndex; } }

		public override IHttpHandler Handler
		{
			get { return new SitemapIndexHandler(this.Syndication); }
		}
	}
}

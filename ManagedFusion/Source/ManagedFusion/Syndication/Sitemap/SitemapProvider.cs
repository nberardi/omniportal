using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ManagedFusion.Syndication.Sitemap
{
	internal class SitemapProvider : SyndicationProvider
	{
		private Sitemap _sitemap;

		public SitemapProvider()
		{
			_sitemap = new Sitemap();
		}

		public override ISyndication Syndication { get { return _sitemap; } }

		public override IHttpHandler Handler
		{
			get { return new SitemapHandler(this.Syndication); }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication.Sitemap
{
	internal class SitemapHandler : BaseSyndicationHandler
	{
		ISyndication _syndication;

		public SitemapHandler(ISyndication syndication)
		{
			this._syndication = syndication;
		}

		protected override string FileName
		{
			get { return "sitemap.xml"; }
		}

		protected override string CacheKey
		{
			get { return Common.GetCacheKey("SitemapCacheKey"); }
		}

		protected override CacheItem CreateSyndication()
		{
			CacheItem item = new CacheItem();

			item.LastModified = this._syndication.LastModified;
			item.Xml = this._syndication.Serialize();

			return item;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication.SitemapIndex
{
	internal class SitemapIndexHandler : BaseSyndicationHandler
	{
		ISyndication _syndication;

		public SitemapIndexHandler(ISyndication syndication) 
		{
			this._syndication = syndication;
		}

		protected override string FileName
		{
			get { return "sitemapIndex.xml"; }
		}

		protected override string CacheKey
		{
			get { return Common.GetCacheKey("SitemapIndexCacheKey"); }
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

using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication.AtomFeed
{
	internal class AtomFeedHandler : BaseSyndicationHandler
	{
		ISyndication _syndication;

		public AtomFeedHandler(ISyndication syndication)
		{
			this._syndication = syndication;
		}

		protected override string ContentType
		{
			get { return "application/atom+xml"; }
		}

		protected override string FileName
		{
			get { return "atom.xml"; }
		}

		protected override string CacheKey
		{
			get { return Common.GetCacheKey("AtomFeedCacheKey"); }
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

#region Copyright © 2004, Nicholas Berardi
/*
 * ManagedFusion (www.ManagedFusion.net) Copyright © 2004, Nicholas Berardi
 * All rights reserved.
 * 
 * This code is protected under the Common Public License Version 1.0
 * The license in its entirety at <http://opensource.org/licenses/cpl.php>
 * 
 * ManagedFusion is freely available from <http://www.ManagedFusion.net/>
 */
#endregion

using System;

using ManagedFusion.Syndication;

namespace ManagedFusion.Modules.Syndication
{
	public class ModuleSitemapHandler : BaseSyndicationHandler
	{
		ISyndication _syndication;

		public ModuleSitemapHandler(ISyndication syn) 
		{
			this._syndication = syn;
		}

		protected override string CacheKey { get { return "Sitemap for " + this.SectionInformation.ID.ToString(); } }

		protected override CacheItem CreateSyndication()
		{
			CacheItem item = new CacheItem();

			item.LastModified = this._syndication.LastModified;
			item.Xml = this._syndication.Serialize();

			return item;
		}
	}
}

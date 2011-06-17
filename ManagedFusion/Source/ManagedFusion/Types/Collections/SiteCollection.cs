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
using System.Data;
using System.Collections;

namespace ManagedFusion
{
	public class SiteCollection : PortalTypeCollection<SiteInfo>
	{
		public SiteCollection(SiteInfo[] sites) : base(sites) { }

		public SiteInfo this [string host] 
		{
			get 
			{
				foreach (SiteInfo site in this.Collection) 
				{
					if (site.ToString() == host)
						// site found and returned
						return site;
				}

				// site not found and nothing returned
				return null;
			}
		}

		public bool Cotnains (string host) 
		{
			return (this[host] != null);
		}

		public override void CommitChanges()
		{
			Common.DatabaseProvider.Sites = this;
		}

		public override void Add(SiteInfo site) 
		{
			if (this.Contains(site.Identity) == false) 
			{
				site.SetState(State.Added);

				SiteInfo[] newSites = new SiteInfo[this.Collection.Length + 1];
				this.CopyTo(newSites, 0);
				newSites[newSites.Length -1] = site;
				this.Collection = newSites;
			}
		}

		public override void Remove(SiteInfo site) 
		{
			if (this.Contains(site.Identity) == true) 
			{
				site.SetState(State.Deleted);

				// notify subscribers of change
				Common.DatabaseProvider.OnSitesChanged();
			}
		}
	}
}
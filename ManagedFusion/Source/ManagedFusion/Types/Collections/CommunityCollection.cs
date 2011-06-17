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
	public class CommunityCollection : PortalTypeCollection<CommunityInfo>
	{
		public CommunityCollection(CommunityInfo[] communities) : base(communities) { }

		public CommunityInfo this [Guid id] 
		{
			get 
			{
				foreach (CommunityInfo community in this.Collection) 
				{
					if (community.UniversalID == id)
						// portal found and returned
						return community;
				}

				// portal not found and nothing returned
				return null;
			}
		}

		public bool Contains (Guid id) 
		{
			return (this[id] != null);
		}

		public override void CommitChanges()
		{
			Common.DatabaseProvider.Communities = this;
		}

		public override void Add(CommunityInfo community) 
		{
			if (this.Contains(community.Identity) == false) 
			{
				community.SetState(State.Added);

				CommunityInfo[] newCommunities = new CommunityInfo[this.Collection.Length + 1];
				this.CopyTo(newCommunities, 0);
				newCommunities[newCommunities.Length -1] = community;
				this.Collection = newCommunities;
			}
		}

		public override void Remove(CommunityInfo community) 
		{
			if (this.Contains(community.Identity)) 
			{
				community.SetState(State.Deleted);

				// notify subscribers of change
				Common.DatabaseProvider.OnCommunitiesChanged();
			}
		}
	}
}
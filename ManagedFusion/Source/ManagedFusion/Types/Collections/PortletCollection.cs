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
	public class PortletCollection : PortalTypeCollection<PortletInfo>
	{
		public PortletCollection(PortletInfo[] portlets) : base(portlets) { }

		public override void CommitChanges()
		{
			Common.DatabaseProvider.Portlets = this;
		}

		public override void Add(PortletInfo portlet) 
		{
			if (this.Contains(portlet.Identity) == false) 
			{
				portlet.SetState(State.Added);

				PortletInfo[] newPortlets = new PortletInfo[this.Collection.Length +1];
				this.CopyTo(newPortlets, 0);
				newPortlets[newPortlets.Length -1] = portlet;
				this.Collection = newPortlets;
			}
		}

		public override void Remove(PortletInfo portlet) 
		{
			if (this.Contains(portlet.Identity) == true) 
			{
				portlet.SetState(State.Deleted);

				// notify subscribers of change
				Common.DatabaseProvider.OnPortletsChanged();
			}
		}
	}
}
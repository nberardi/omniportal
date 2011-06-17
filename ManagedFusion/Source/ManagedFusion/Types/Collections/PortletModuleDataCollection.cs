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
using System.Collections.Specialized;

namespace ManagedFusion
{
	public class PortletModuleDataCollection : NameValueCollection
	{
		private PortletInfo _owner;

		public PortletModuleDataCollection(PortletInfo owner, NameValueCollection properties)
		{
			for(int i = 0; i < properties.Count; i++)
				base.Add(
					properties.GetKey(i),
					properties[i]
					);

			this._owner = owner;
		}

		public override void Add(string name, string value)
		{
			this.Set(name, value);
		}

		public override void Set(string name, string value)
		{
			if (base[name] == null) 
			{
				Common.DatabaseProvider.AddModuleDataForPortlet(name, value, _owner);
				base.Add (name, value);
			}
			else 
			{ 
				Common.DatabaseProvider.UpdateModuleDataForPortlet(name, value, _owner);
				base.Set (name, value);
			}
		}

		public override void Remove(string name)
		{
			if (base[name] != null) 
			{
				Common.DatabaseProvider.RemoveModuleDataForPortlet(name, _owner);
				base.Remove (name);
			}
		}
	}
}
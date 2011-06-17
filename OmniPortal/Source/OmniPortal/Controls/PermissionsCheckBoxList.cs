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
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Controls
{
	internal class PermissionsCheckBox : CheckBoxList
	{
		[Browsable(false)]
		public List<string> SelectedPermissions
		{
			get 
			{
				List<string> list = new List<string>();

				// get selected roles from currentRow list
				foreach(ListItem i in this.Items) 
					if (i.Selected)
						list.Add(i.Text);

				return list;
			}
			set { ViewState["SelectedPermissions"] = value; }
		}

		[Browsable(false)]
		public List<string> Permissions 
		{
			get
			{
				object state = ViewState["Permissions"];
				if (state == null) ViewState["Permissions"] = new List<string>();

				return ViewState["Permissions"] as List<string>;
			}
			set { ViewState["Permissions"] = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override void DataBind()
		{
			if (Permissions.Count == 0)
				throw new ApplicationException("You must enter at least one role in the property Roles.");

			this.DataSource = this.Permissions;
			base.DataBind ();
			
			// add OmniPortal System Roles to role list
			// select roles set in view state
			List<string> list = ViewState["SelectedPermissions"] as List<string>;
			for(int i = 0; i < this.Items.Count; i++)
				this.Items[i].Selected = list.Contains(this.Items[i].Text);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			// combines all strings and returns them
			return String.Join(Common.Delimiter.ToString(), this.SelectedPermissions.ToArray());
		}
	}
}
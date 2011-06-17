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
using MS = ManagedFusion.Security;

namespace OmniPortal.Controls
{
	[ToolboxData("<{0}:RolesGrid runat=server></{0}:RolesGrid>")]
	public class RolesGrid : System.Web.UI.WebControls.WebControl
	{
		private OmniPortal.Controls.PermissionsCheckBox taskList;
		private OmniPortal.Controls.SelectionList selectionList;

		public RolesGrid () 
		{
			this.AvailableTitle = "Available Roles";
			this.SelectedTitle = "Selected Roles";
		}

		#region Events

		public event RolesGridEventHandler RoleAdded;

		protected void OnRoleAdded (RolesGridEventArgs e) 
		{
			if (RoleAdded != null)
				RoleAdded(this, e);
		}

		public event RolesGridEventHandler RoleRemoved;

		protected void OnRoleRemoved(RolesGridEventArgs e) 
		{
			if (RoleRemoved != null)
				RoleRemoved(this, e);
		}

		public event RolesGridEventHandler RoleChanged;

		protected void OnRoleChanged(RolesGridEventArgs e) 
		{
			if (RoleChanged != null)
				RoleChanged(this, e);
		}


		#endregion

		#region Properties

		[Category("Behavior")]
		[DefaultValue(true)]
		[Browsable(true)]
		[Description("If the control is enabled or not.")]
		public override bool Enabled
		{
			get { return base.Enabled; }
			set 
			{
				base.Enabled = value;
				this.taskList.Enabled = value;
				this.selectionList.Enabled = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("Available Roles")]
		[Browsable(true)]
		[Description("Title of Available Roles that can be selected to the selected column.")]
		public string AvailableTitle
		{
			get { return ViewState["AvailableName"] as string; }
			set { ViewState["AvailableName"] = value; }
		}

		[Category("Appearance")]
		[DefaultValue("Selected Roles")]
		[Browsable(true)]
		[Description("Title of Selected Roles that can be un-selected back to the available column.")]
		public string SelectedTitle
		{
			get { return ViewState["SelectedTitle"] as string; }
			set { ViewState["SelectedTitle"] = value; }
		}

		[Category("Appearance")]
		[DefaultValue("Tasks")]
		[Browsable(true)]
		[Description("Title of the options in the lower list.")]
		public string OptionsTitle
		{
			get { return ViewState["OptionsTitle"] as string; }
			set { ViewState["OptionsTitle"] = value; }
		}

		[Browsable(false)]
		public IDictionary RolesInUse 
		{
			get 
			{
				if (ViewState["RolesInUse"] == null) 
					ViewState["RolesInUse"] = new Hashtable();

				return ViewState["RolesInUse"] as IDictionary;
			}
			set 
			{
				ViewState["RolesInUse"] = new Hashtable(value); 
			}
		}

		[Browsable(false)]
		public string SelectedRole 
		{
			get 
			{
				if (ViewState["SelectedRole"] == null) 
					ViewState["SelectedRole"] = String.Empty;
				
				return (string)ViewState["SelectedRole"];
			}
			set { ViewState["SelectedRole"] = value; }
		}

		[Browsable(false)]
		public List<string> Roles 
		{
			get 
			{
				if (ViewState["Roles"] == null) 
					ViewState["Roles"] = new List<string>();

				return ViewState["Roles"] as List<string>;
			}
			set 
			{
				ViewState["Roles"] = value; 
			}
		}

		[Browsable(false)]
		public List<string> Tasks 
		{
			get 
			{
				if (ViewState["Tasks"] == null) 
						ViewState["Tasks"] = new List<string>();

				return ViewState["Tasks"] as List<string>;
			} 
			set 
			{
				ViewState["Tasks"] = value; 
			}
		}

		#endregion

		#region WebControl Events

		protected override void OnInit(EventArgs e) 
		{
			taskList = new PermissionsCheckBox();
			selectionList = new SelectionList();
			
			// taskList
			//
			taskList.RepeatDirection = RepeatDirection.Horizontal;
			taskList.RepeatColumns = 5;
			taskList.AutoPostBack = true;
			taskList.RepeatLayout = RepeatLayout.Table;
			taskList.Width = this.Width;

			// selectionsList
			//
			selectionList.Width = this.Width;
			selectionList.RightListTitle = this.AvailableTitle;
			selectionList.LeftListTitle = this.SelectedTitle;

			this.Controls.Add(this.taskList);
			this.Controls.Add(this.selectionList);

			selectionList.AddButtonClick += new EventHandler(selectionsList_AddButtonClick);
			selectionList.RemoveButtonClick += new EventHandler(selectionsList_RemoveButtonClick);
			selectionList.LeftIndexChanged += new EventHandler(selectionsList_LeftIndexChanged);
			taskList.SelectedIndexChanged += new EventHandler(taskList_SelectedIndexChanged);

			base.OnInit (e);
		}

		protected override void OnPreRender(EventArgs e) 
		{
			if (this.Enabled == false) return;

			// set selected values
			List<string> permissions = this.RolesInUse[this.SelectedRole] as List<string>;

			if (permissions != null) 
				this.taskList.SelectedPermissions = permissions;

			base.OnPreRender (e);
		}

		#endregion

		public override void DataBind() 
		{
			if (this.Enabled == false) return;

			Hashtable tempRolesInUse = ViewState["RolesInUse"] as Hashtable;

			// gets the last selected currentRow to make sure the selected permisstions
			// are not lost and it checks to see if a role is selected
			this.EnsureData();

			// gets a list of roles that are not in use
			ArrayList collection = new ArrayList(0);
			foreach (string role in this.Roles) 
				if (tempRolesInUse.ContainsKey(role) == false)
					collection.Add(role);

			if (tempRolesInUse.Count > 0) 
			{
				// roles in use
				this.selectionList.LeftListDataSource = tempRolesInUse.Keys;
			}

			// roles not in use
			this.selectionList.RightListDataSource = collection;

			// set permissions
			taskList.Permissions = this.Tasks;

			// select the permission for the current role
			List<string> permissions = tempRolesInUse[this.SelectedRole] as List<string>;
			taskList.SelectedPermissions = (permissions != null) ? permissions : new List<string>();
			
			base.DataBind();
		}
		
		#region Render

		protected override void Render(HtmlTextWriter writer) 
		{
			// don't render if not enabled
			if (this.Enabled == false) return;

			// <table cellpadding=0 cellspacing=0 width=? height?>
			writer.WriteBeginTag("table");
			writer.WriteAttribute("cellpadding", "0");
			writer.WriteAttribute("cellspacing", "0");
			writer.WriteAttribute("width", Width.ToString());
			writer.WriteAttribute("height", Height.ToString());
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.WriteLine();

			// render body
			this.RenderContents(writer);

			// </table>
			writer.WriteEndTag("table");
			writer.WriteLine();
		}

		protected override void RenderContents(HtmlTextWriter writer) 
		{
			#region <tr><td class=?>[selectionsList]</td></tr>
			// <tr><td class=? colspan=2>
			writer.Indent++;
			writer.WriteFullBeginTag("tr");
			writer.WriteBeginTag("td");
			writer.WriteAttribute("class", this.CssClass);
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.WriteLine();

			// render roles list
			writer.Indent++;
			this.selectionList.RenderControl(writer);

			// </td></tr>
			writer.Indent--;
			writer.WriteEndTag("td");
			writer.WriteEndTag("tr");
			writer.WriteLine();
			#endregion

			#region <tr><td class=?>[permissionsList]</td></tr>
			// <tr><td class=?>
			writer.WriteFullBeginTag("tr");
			writer.WriteBeginTag("td");
			writer.WriteAttribute("class", this.CssClass);
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.WriteLine();

			// render permissions list
			writer.Indent++;
			this.taskList.RenderControl(writer);

			// </td></tr>
			writer.Indent--;
			writer.WriteEndTag("td");
			writer.WriteEndTag("tr");
			writer.WriteLine();
			#endregion
		}

		#endregion

		#region Event Methods

		private void selectionsList_AddButtonClick(object sender, EventArgs e)
		{
			// add this currentRow to the roles list
			this.RolesInUse.Add(this.selectionList.RightListSelectedItem.Text, new List<string>());

			// checks to see if a role is selected
			this.EnsureData();

			this.selectionList.LeftListNameSelected = this.selectionList.RightListSelectedItem.Text;
			this.SelectedRole = this.selectionList.RightListSelectedItem.Text;

			// notify that a role has been added
			this.OnRoleAdded(new RolesGridEventArgs(this.selectionList.RightListSelectedItem.Text, this.taskList.SelectedPermissions.ToArray()));

			// databind everything
			this.DataBind();
		}

		private void selectionsList_RemoveButtonClick(object sender, EventArgs e)
		{
			// remove this currentRow to the roles list
			this.RolesInUse.Remove(this.selectionList.LeftListSelectedItem.Text);
			this.selectionList.RightListNameSelected = this.selectionList.LeftListSelectedItem.Text;
			
			// notify that a role has been removed
			this.OnRoleRemoved(new RolesGridEventArgs(this.selectionList.LeftListSelectedItem.Text, null));

			// databind everything
			this.DataBind();
		}

		private void selectionsList_LeftIndexChanged(object sender, EventArgs e)
		{
			// checks to see if a role is selected
			this.EnsureData();

			// sets the selected role
			this.SelectedRole = this.selectionList.LeftListSelectedItem.Text;

			// gets the permissions for the selected role
			List<string> tasks = this.RolesInUse[this.SelectedRole] as List<string>;
			taskList.SelectedPermissions = (tasks == null) ? new List<string>() : tasks;

			// databind the permissions list
			taskList.DataBind();
		}

		private void taskList_SelectedIndexChanged(object sender, EventArgs e)
		{
			// sets the selected role
			this.SelectedRole = this.selectionList.LeftListSelectedItem.Text;

			// checks to see if a role is selected
			this.EnsureData();

			// notify that a role has been added
			this.OnRoleChanged(new RolesGridEventArgs(this.selectionList.LeftListSelectedItem.Text, this.taskList.SelectedPermissions.ToArray()));

			// databind everything
			this.DataBind();
		}

		#endregion

		public void EnsureData () 
		{
			// checks to see if a role is selected
			if (this.SelectedRole != String.Empty)
				this.RolesInUse[this.SelectedRole] = this.taskList.SelectedPermissions;
		}
	}
}
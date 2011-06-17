using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ManagedFusion;

namespace OmniPortal.Communities.Default.Modules.Admin
{
	public partial class Portlet : System.Web.UI.UserControl
	{
		private int _id;
		private const string PageAddress = "Portlet.aspx";
		private PortletInfo _info;

		public PortletInfo Info
		{
			get
			{
				if (_info == null)
					_info = PortletInfo.Collection[(int)ViewState["Info"]];

				return _info;
			}
			set
			{
				_info = null;
				ViewState["Info"] = value.Identity;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (this.Page.IsPostBack == false)
			{
				this.DataBind();
			}
		}

		private void RedirectDefaultState()
		{
			Response.Redirect(Common.Path.GetPortalUrl(PageAddress).ToString());
		}

		private void Redirect(string id)
		{
			Response.Redirect(Common.Path.GetPortalUrl(PageAddress + "?id=" + id).ToString());
		}

		private void ShowItemPanel(bool show)
		{
			this.ItemPanel.Visible = show;
			this.ListPanel.Visible = !show;
		}

		public override void DataBind()
		{
			// get the id for the community
			if (Request.QueryString["id"] != null)
			{
				try { this._id = Convert.ToInt32(Request.QueryString["id"]); }
				catch { this._id = PortletInfo.TempIdentity; }
			}
			else
				this._id = Int32.MinValue;

			if (this._id == PortletInfo.TempIdentity || PortletInfo.Collection.Contains(this._id))
			{
				if (this._id == PortletInfo.TempIdentity)
				{
					Info = PortletInfo.CreateNew();

					// change the buttons
					this.sendButton.Text = this.GetGlobalResourceObject("OmniPortal", "Add") as string;

					// disable delete button
					this.deleteButton.Enabled = false;
					this.RolesTableRow.Visible = false;
				}
				else
				{
					Info = PortletInfo.Collection[this._id];

					// check to see if the info exists
					if (Info == null)
					{
						RedirectDefaultState();
					}

					// common attributes
					this.typeID.Text = Info.Identity.ToString();
					this.lastTouched.Text = Info.Touched.ToString("F");

					// appearence attributes
					this.titleText.Text = Info.Title;

					// module attributes
					this.rolesGrid.Enabled = true;
					this.rolesGrid.Roles = new List<string>(Common.Role.GetAllRoles());
					this.rolesGrid.Roles.AddRange(new string[] { PortalRole.Everybody.ToString(), PortalRole.Authenticated.ToString(), PortalRole.NotAuthenticated.ToString() });
					this.rolesGrid.Tasks = new List<string>(Enum.GetNames(typeof(Permissions)));
					this.rolesGrid.RolesInUse = Info.Roles;

					// change the buttons
					this.sendButton.Text = this.GetGlobalResourceObject("OmniPortal", "Update") as string;
				}

				// modules attributes
				this.moduleList.Items.AddRange(this.GetModulesItemList(Info.Module));

				// show the currentRow panel
				this.ShowItemPanel(true);
			}
			else
			{
				this.portletList.Items.AddRange(this.GetItemList());

				// hide the currentRow panel
				this.ShowItemPanel(false);
			}

			base.DataBind();
		}

		private ListItem[] GetModulesItemList(ModuleInfo selectedModule)
		{
			ArrayList list = new ArrayList(PortletModule.Collection.Count);

			// add the default currentRow
			list.Add(new ListItem(
				"Select A Module",
				String.Empty
				));

			// add the module items
			foreach (PortletModule info in PortletModule.Collection)
			{
				ListItem item = new ListItem(info.Title, info.Identity.ToString());
				item.Selected = (info == selectedModule);
				list.Add(item);
			}

			// return the list of module values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetItemList()
		{
			ArrayList list = new ArrayList(PortletInfo.Collection.Count);

			// add the default currentRow
			list.Add(new ListItem(
				"Select A Portlet",
				String.Empty
				));

			// add the section items
			foreach (PortletInfo info in PortletInfo.Collection)
			{
				list.Add(new ListItem(
					info.Title,
					info.Identity.ToString()
					));
			}

			// return the list of section values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			// disabled roles list by default
			this.rolesGrid.Enabled = false;

			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.portletList.SelectedIndexChanged += new EventHandler(portletList_SelectedIndexChanged);
			this.rolesGrid.RoleRemoved += new OmniPortal.Controls.RolesGridEventHandler(this.rolesGrid_RoleRemoved);
			this.rolesGrid.RoleAdded += new OmniPortal.Controls.RolesGridEventHandler(this.rolesGrid_RoleAdded);
			this.rolesGrid.RoleChanged += new OmniPortal.Controls.RolesGridEventHandler(this.rolesGrid_RoleChanged);

		}
		#endregion

		protected void sendButton_Click(object sender, System.EventArgs e)
		{
			// set the values of the site
			Info.Title = this.titleText.Text;

			// set the module
			Info.Module = PortletModule.Collection[new Guid(this.moduleList.SelectedValue)] as PortletModule;

			// if the site is a temp site -- meaning it hasn't
			// yet been added to the database
			if (Info.Identity == PortletInfo.TempIdentity)
			{
				PortletInfo.Collection.Add(Info);
			}

			// commit the changes of this site to the database
			Info.CommitChanges();

			// redirect after changes
			RedirectDefaultState();
		}

		protected void deleteButton_Click(object sender, System.EventArgs e)
		{
			Info.SetForDeletion(true);

			// commit the changes of this site to the database
			Info.CommitChanges();

			// redirect after changes
			RedirectDefaultState();
		}

		protected void cancelButton_Click(object sender, System.EventArgs e)
		{
			// redirect back to the default state
			RedirectDefaultState();
		}

		private void portletList_SelectedIndexChanged(object sender, EventArgs e)
		{
			Redirect(this.portletList.SelectedValue);
		}

		private void rolesGrid_RoleAdded(object sender, OmniPortal.Controls.RolesGridEventArgs e)
		{
			// add role
			Info.AddRole(e.Role, e.Tasks);
		}

		private void rolesGrid_RoleRemoved(object sender, OmniPortal.Controls.RolesGridEventArgs e)
		{
			// remove role
			Info.RemoveRole(e.Role);
		}

		private void rolesGrid_RoleChanged(object sender, OmniPortal.Controls.RolesGridEventArgs e)
		{
			// update role
			Info.UpdateRole(e.Role, e.Tasks);
		}
	}
}
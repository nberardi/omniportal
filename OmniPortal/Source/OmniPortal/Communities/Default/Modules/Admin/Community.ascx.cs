using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Modules.Admin
{
	public partial class Community : System.Web.UI.UserControl
	{

		private int _id;
		private const string PageAddress = "Community.aspx";
		private CommunityInfo _info;

		public CommunityInfo Info 
		{
			get 
			{
				if (_info == null)
					_info = CommunityInfo.Collection[(int)ViewState["Info"]];

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

		private void RedirectDefaultState () 
		{
			Response.Redirect(Common.Path.GetPortalUrl(PageAddress).ToString());
		}

		private void Redirect(string id) 
		{
			Response.Redirect(Common.Path.GetPortalUrl(PageAddress + "?id=" + id).ToString());
		}

		private void ShowItemPanel (bool show) 
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
				catch { this._id = CommunityInfo.TempIdentity; }
			} 
			else
				this._id = Int32.MinValue;

			if (this._id == CommunityInfo.TempIdentity || CommunityInfo.Collection.Contains(this._id)) 
			{
				if (this._id == CommunityInfo.TempIdentity) 
				{
					Info = CommunityInfo.CreateNew();
				
					// set the value
					this.universalID.Text = Info.UniversalID.ToString();
			
					// change the buttons
					this.sendButton.Text = this.GetGlobalResourceObject("OmniPortal", "Add") as string;

					// disable delete button
					this.deleteButton.Enabled = false;
				}
				else 
				{
					Info = CommunityInfo.Collection[this._id];

					// check to see if the info exists
					if (Info == null) 
					{
						RedirectDefaultState();
					}

					// common attributes
					this.typeID.Text = Info.Identity.ToString();
					this.universalID.Text = Info.UniversalID.ToString();
					this.lastTouched.Text = Info.Touched.ToString("F");

					// appearence attributes
					this.titleText.Text   = Info.Title;

					// change the buttons
					this.sendButton.Text = this.GetGlobalResourceObject("OmniPortal", "Update") as string;
				} 

				// show the currentRow panel
				this.ShowItemPanel(true);
			} 
			else 
			{
				this.communityList.Items.AddRange(this.GetItemList());
				
				// hide the currentRow panel
				this.ShowItemPanel(false);
			}

			base.DataBind ();
		}

		private ListItem[] GetItemList () 
		{
			ArrayList list = new ArrayList(CommunityInfo.Collection.Count);

			// add the default currentRow
			list.Add(new ListItem(
				"Select A Community",
				String.Empty
				));

			// add the communities items
			foreach(CommunityInfo info in CommunityInfo.Collection) 
			{
				list.Add(new ListItem(
					info.Title,
					info.Identity.ToString()
					));
			}

			// return the list of community values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void sendButton_Click(object sender, System.EventArgs e)
		{
			// set the values of the community
			Info.Title = this.titleText.Text;

			// if the community is a temp community -- meaning it hasn't
			// yet been added to the database
			if (Info.Identity == CommunityInfo.TempIdentity) 
			{
				CommunityInfo.Collection.Add(Info);
			}

			// commit the changes of this community to the database
			Info.CommitChanges();

			// redirect after changes
			RedirectDefaultState();
		}

		protected void deleteButton_Click(object sender, EventArgs e)
		{
			Info.SetForDeletion(true);

			// commit the changes of this community to the database
			Info.CommitChanges();

			// redirect after changes
			RedirectDefaultState();
		}

		protected void cancelButton_Click(object sender, System.EventArgs e)
		{
			// redirect back to the default state
			RedirectDefaultState();
		}

		protected void communityList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Redirect(this.communityList.SelectedValue);
		}
	}
}

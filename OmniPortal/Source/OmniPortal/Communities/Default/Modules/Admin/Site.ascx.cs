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
	/// <summary>
	///		Summary description for Site.
	/// </summary>
	public partial class Site : System.Web.UI.UserControl
	{

		private int _id;
		private const string PageAddress = "Site.aspx";
		private SiteInfo _info;

		public SiteInfo Info 
		{
			get 
			{
				if (_info == null)
					_info = SiteInfo.Collection[(int)ViewState["Info"]];

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
				catch { this._id = SiteInfo.TempIdentity; }
			} 
			else
				this._id = Int32.MinValue;

			if (this._id == SiteInfo.TempIdentity || SiteInfo.Collection.Contains(this._id)) 
			{
				if (this._id == SiteInfo.TempIdentity) 
				{
					Info = SiteInfo.CreateNew();
			
					// change the buttons
					this.sendButton.Text = this.GetGlobalResourceObject("OmniPortal", "Add") as string;

					// disable delete button
					this.deleteButton.Enabled = false;
					this.ConnectedFieldSet.Visible = false;
				}
				else 
				{
					Info = SiteInfo.Collection[this._id];

					// check to see if the info exists
					if (Info == null) 
					{
						RedirectDefaultState();
					}

					// common attributes
					this.typeID.Text = Info.Identity.ToString();
					this.lastTouched.Text = Info.Touched.ToString("F");

					// domain attributes
					this.domainText.Text = Info.Domain;
					this.subDomainText.Text = Info.SubDomain;

					// connected attributes
					this.sectionsList.Items.AddRange(GetSectionsItemList(Info.ConnectedSection));

					// change the buttons
					this.sendButton.Text = this.GetGlobalResourceObject("OmniPortal", "Update") as string;
				} 

				// appearence attributes
				this.themeList.Items.AddRange(this.GetThemesItemList(Info.OriginalTheme));
				this.styleList.Items.AddRange(this.GetStylesItemList(Info.OriginalTheme, Info.OriginalStyle));

				// show the currentRow panel
				this.ShowItemPanel(true);
			} 
			else 
			{
				this.siteList.Items.AddRange(this.GetItemList());
				
				// hide the currentRow panel
				this.ShowItemPanel(false);
			}

			base.DataBind();
		}

		private ListItem[] GetSectionsItemList (SectionInfo selectedSection) 
		{
			ArrayList list = new ArrayList(SectionInfo.Collection.Count);

			// add the default currentRow
			list.Add(new ListItem(
				"Select A Section",
				String.Empty
				));

			// add the communities items
			foreach(SectionInfo info in SectionInfo.Collection) 
			{
				ListItem item = new ListItem(info.Path, info.Identity.ToString());
				item.Selected = (info == selectedSection);
				list.Add(item);
			}

			// return the list of community values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetThemesItemList (string selectedTheme) 
		{
			ArrayList list = new ArrayList(Info.ConnectedCommunity.Themes.Count);

			// add the theme items
			foreach(ThemeInfo info in Info.ConnectedCommunity.Themes) 
			{
				// don't show the NoTheme folder it is already account for above
				if (info.Name == ThemeInfo.NoTheme)
					continue;

				ListItem item = new ListItem((info.IsDefaultTheme ? "[default] " : String.Empty) + info.Name, info.Name);
				item.Selected = (info.Name == selectedTheme);
				list.Add(item);
			}

			// add the Inherited
			ListItem inherited = new ListItem("[system] " + ThemeInfo.Inherited, ThemeInfo.Inherited);
			inherited.Selected = (ThemeInfo.Inherited == selectedTheme);
			list.Add(inherited);

			// add the NoTheme
			ListItem noTheme = new ListItem("[system] " + ThemeInfo.NoTheme, ThemeInfo.NoTheme);
			noTheme.Selected = (ThemeInfo.NoTheme == selectedTheme);
			list.Add(noTheme);

			// return the list of theme values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetStylesItemList (string selectedTheme, string selectedStyles) 
		{
			ArrayList list = new ArrayList(Info.ConnectedCommunity.Themes.Count);

			if (ThemeInfo.NoTheme != selectedTheme)
			{
				if (ThemeInfo.Inherited != selectedTheme) 
				{
					// add the style items
					foreach(StyleInfo info in Info.ConnectedCommunity.Themes[selectedTheme].Styles) 
					{
						ListItem item = new ListItem(info.Name, info.Name);
						item.Selected = (info.Name == selectedStyles);
						list.Add(item);
					}
				}

				// add the Inherited
				ListItem inherited = new ListItem("[system] " + StyleInfo.Inherited, StyleInfo.Inherited);
				inherited.Selected = (StyleInfo.Inherited == selectedStyles);
				list.Add(inherited);
			}

			// add the NoTheme
			ListItem noStyle = new ListItem("[system] " + StyleInfo.NoStyle, StyleInfo.NoStyle);
			noStyle.Selected = (StyleInfo.NoStyle == selectedStyles);
			list.Add(noStyle);

			// return the list of style values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetItemList () 
		{
			ArrayList list = new ArrayList(SiteInfo.Collection.Count);

			// add the default currentRow
			list.Add(new ListItem(
				"Select A Site",
				String.Empty
				));

			// add the communities items
			foreach(SiteInfo info in SiteInfo.Collection) 
			{
				list.Add(new ListItem(
					info.FullDomain,
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void sendButton_Click(object sender, System.EventArgs e)
		{
			// set the values of the site
			Info.Domain = this.domainText.Text;
			Info.SubDomain = this.subDomainText.Text;
			Info.OriginalTheme = this.themeList.SelectedValue;
			Info.OriginalStyle = this.styleList.SelectedValue;

			// if the site is a temp site -- meaning it hasn't
			// yet been added to the database
			if (Info.Identity == SiteInfo.TempIdentity) 
			{
				SiteInfo.Collection.Add(Info);
			} 
			else
			{
				// set the connected section
				Info.ConnectedSection = SectionInfo.Collection[Convert.ToInt32(this.sectionsList.SelectedValue)];
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

		protected void siteList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Redirect(this.siteList.SelectedValue);
		}

		protected void themeList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.styleList.Items.Clear();
			this.styleList.Items.AddRange(this.GetStylesItemList(themeList.SelectedValue, Info.OriginalStyle));
		}
	}
}

using System;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ManagedFusion;

namespace OmniPortal.Communities.Default.Themes.Default.Skin.Pages
{
	public partial class Template : System.Web.UI.MasterPage
	{
		protected SectionInfo SectionInformation
		{
			get { return SectionInfo.Current; }
		}

		protected NameValueCollection InnerMenu
		{
			get
			{
				if (Context.Items["InnerMenu"] == null)
					Context.Items["InnerMenu"] = new NameValueCollection(0);
				return (NameValueCollection)Context.Items["InnerMenu"];
			}
		}

		protected void Page_Load (object sender, EventArgs e)
		{
			if (Page.IsPostBack == false) {
				this.topNavRepeater.DataBind();

				this.innerMenuRepeater.Visible = (InnerMenu.Count != 0);
				if (this.innerMenuRepeater.Visible)
					this.innerMenuRepeater.DataBind();

				this.childrenRepeater.Visible = (SectionInformation.Children.Count != 0 && SectionInformation.Parent != null);
				if (this.childrenRepeater.Visible)
					this.childrenRepeater.DataBind();
			}
		}
	}
}

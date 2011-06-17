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
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Modules;
using ManagedFusion.Data;

// OmniPortal Classes
using OmniPortal.Modules.Blog.Data;

namespace OmniPortal.Modules.Blog
{
	public partial class List : SkinnedUserControl
	{
		protected bool IsPoster = false;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			//BlogDatabaseProvider dbprovider = DatabaseManager.Providers["OmniPortalBlog"] as BlogDatabaseProvider;
			//BlogItem[] blogs = dbprovider.GetBlogs(this.Context);

			//IsPoster = this.IsInTask("Poster");

			//BlogList.DataSource = blogs;
			//BlogList.DataBind();
		}

		protected string GetPostUrl (string id) 
		{
			return ManagedFusion.Common.Path.GetPortalUrl(String.Format("Archive/{0}.aspx", id)).ToString();
		}

		protected string BlogTitle 
		{
			get { return this.Properties["BlogTitle"]; }
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
	}
}
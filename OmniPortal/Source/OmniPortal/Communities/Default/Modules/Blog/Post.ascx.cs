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
using System.Globalization;
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
	public partial class Post : System.Web.UI.UserControl
	{
		private BlogItem _post;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (this.Page.IsPostBack == false)
			{
				//int postID = Int32.Parse(Request.QueryString["post"], NumberStyles.Integer);

				//BlogDatabaseProvider dbprovider = DatabaseManager.Providers["OmniPortalBlog"] as BlogDatabaseProvider;
				//this._post = dbprovider.GetBlog(postID);
			}
		}

		protected BlogItem BlogPost
		{
			get { return this._post; }
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
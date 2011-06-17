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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Data;

// OmniPortal Classes
using OmniPortal.Modules.Blog.Data;

namespace OmniPortal.Modules.Blog.Admin
{
	public class AdminUserControl : SkinnedUserControl
	{
		private BlogDatabaseProvider _dbprovider;

		protected override void OnInit(EventArgs e)
		{
			// set db provider for the blog
			_dbprovider = Databases.Providers["OmniPortalBlog"] as BlogDatabaseProvider;

			// add admin style sheet
			Common.PageBuilder.StyleSheets.Add(this.Module.GetUrlPath("admin/admin.css").ToString());

			base.OnInit (e);
		}

		public BlogDatabaseProvider DatabaseProvider 
		{
			get { return this._dbprovider; }
		}
	}
}
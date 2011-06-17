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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

// OmniPortal Classes
using ManagedFusion.Security;
using ManagedFusion;
using ManagedFusion.Portlets;

namespace OmniPortal.Portlets.RssFeed
{
	[PortletAdmin("Edit.ascx", "Edit RSSFeed", "Edit's the location of the RSS Feed", Permissions.Read | Permissions.Edit)]
	public partial class RssFeedEdit : PortletUserControl
	{
		/// <summary>
		///	Required method for Designer support - do not modify
		///	the contents of this method with the code editor.
		/// </summary>
		protected override void OnInit(EventArgs e)
		{
			if (Properties["RssFeedAddress"] != null)
				this.rssUrlTextBox.Text = Properties["RssFeedAddress"];

			updateButton.Click += new EventHandler(updateButton_Click);
		
			base.OnInit (e);
		}

		private void updateButton_Click(object sender, EventArgs e)
		{
			this.Properties.Set("RssFeedAddress", rssUrlTextBox.Text);
		}
	}
}
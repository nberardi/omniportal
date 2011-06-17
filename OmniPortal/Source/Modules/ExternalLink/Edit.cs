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
using System.Web.UI;
using System.Web.UI.WebControls;

// OmniPortal Classes
using ManagedFusion.Security;
using ManagedFusion;
using ManagedFusion.Modules;

namespace OmniPortal.Modules.ExternalLink
{
	/// <summary>
	/// Summary description for Edit.
	/// </summary>
	[ModuleAdmin("Edit.aspx", "External Link", "Used to change the external link that this section links to.")]
	public class Edit : SkinnedUserControl
	{
		TextBox urlTextBox;
		Button updateButton;

		protected override void OnInit(EventArgs e)
		{
			urlTextBox = new TextBox();
			urlTextBox.Text = Properties["ExternalURL"];

			updateButton = new Button();
			updateButton.Text = "Update Link";
			updateButton.Click += new EventHandler(updateButton_Click);

			this.Controls.Add(new LiteralControl("Current Section Link:&nbsp;&nbsp;"));
			this.Controls.Add(urlTextBox);
			this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
			this.Controls.Add(updateButton);

			base.OnInit (e);
		}

		private void updateButton_Click(object sender, EventArgs e)
		{
			Properties["ExternalURL"] = urlTextBox.Text;
		}
	}
}
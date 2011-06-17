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
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Portlets;

namespace OmniPortal.Portlets.Login
{
	[Portlet("Login", "This is used to authenticate users.",
		"Login",
		"{F0E74CFD-C96C-4c1e-A9E0-CAF7BFC0CDB3}")]
	public partial class Read : PortletUserControl
	{
		protected System.Web.UI.HtmlControls.HtmlTable AuthenticationTable;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.IsAuthenticated) 
			{
				// hide the authentication table
				this.ShowForm(false);

				// show the message
				this.MessageLabel.Visible = true;
				this.MessageLabel.Text = String.Concat("Welcome ", Common.Context.User.Identity.Name, "!");
				this.MessageLabel.ForeColor = Color.Empty;
			} 
			else 
			{
				// show the authentication table
				this.ShowForm(true);

				// show message box if there is an error
				this.MessageLabel.Visible = (this.MessageLabel.Text.Length > 0);
			}
		}

		private void ShowForm (bool visible) 
		{
			this.LoggedOut.Visible = visible;
			this.LoggedIn.Visible = !visible;
		}

		protected override void OnInit(EventArgs e)
		{
			this.LoginButton.Text = this.GetGlobalResourceObject("OmniPortal", "Login") as string;
			this.LogoutButton.Text = this.GetGlobalResourceObject("OmniPortal", "Logout") as string;

			this.LogoutButton.Click += new System.EventHandler(this.LogoutButton_Click);
			this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
			this.Error += new System.EventHandler(this.Page_Error);
			this.Load += new System.EventHandler(this.Page_Load);

			base.OnInit(e);
		}

		private void LoginButton_Click(object sender, System.EventArgs e)
		{
			//FormAuthentication auth = Common.Security as FormAuthentication;

			//// check to see if username and password both have values
			//if (auth != null
			//    && this.UsernameTextBox.Text.Length > 0
			//    && this.PasswordTextBox.Text.Length > 0)
			//    // authenticate username and password
			//    if (auth.Login(this.UsernameTextBox.Text, this.PasswordTextBox.Text, this.KeepLoggedInCheckBox.Checked)) 
			//        this.Response.Redirect(FormsAuthentication.GetRedirectUrl(this.UsernameTextBox.Text, this.KeepLoggedInCheckBox.Checked));
			//    else
			//        throw new ManagedFusionException(ExceptionType.AccessDenied, this.UsernameTextBox.Text);
		}

		private void LogoutButton_Click(object sender, System.EventArgs e)
		{
			//FormAuthentication auth = Common.Security as FormAuthentication;

			//// logout if authentication is valid
			//if (auth != null)
			//    auth.Logout();

			//this.Response.Redirect(Common.Path.GetPortalUrl(String.Empty).ToString());
		}

		protected void Page_Error(object sender, EventArgs e)
		{
			this.MessageLabel.Text = "Username or Password is incorrect.";
			this.MessageLabel.ForeColor = Color.Red;
		}
	}
}
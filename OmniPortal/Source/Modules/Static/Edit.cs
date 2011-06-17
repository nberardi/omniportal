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
using System.Web.UI.HtmlControls;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;
using ManagedFusion.Modules;

namespace OmniPortal.Modules.Static
{
	/// <summary>
	/// Summary description for Edit.
	/// </summary>
	[ModuleAdmin("Edit.aspx", "Static Page", "Used to change the content of a static page with a WYSIWYG Editor")]
	public class Edit : SkinnedUserControl
	{
		protected HtmlTextArea contentText;
		protected Button saveButton;

		protected override void OnInit(EventArgs e)
		{
			this.Page.ClientScript.RegisterClientScriptInclude("TinyMCEInclude", "/jscripts/tiny_mce/tiny_mce.js");
			this.Page.ClientScript.RegisterClientScriptBlock(typeof(Edit), "TinyMCESetup", @"
tinyMCE.init({
	mode : ""textareas"",
	theme : ""advanced"",
	theme_advanced_buttons1 : ""bold,italic,underline,separator,strikethrough,justifyleft,justifycenter,justifyright,justifyfull,bullist,numlist,undo,redo,link,unlink"",
	theme_advanced_buttons2 : """",
	theme_advanced_buttons3 : """",
	theme_advanced_toolbar_location : ""top"",
	theme_advanced_toolbar_align : ""left"",
	theme_advanced_path_location : ""bottom"",
	extended_valid_elements : ""a[name|href|target|title|onclick],img[class|src|border=0|alt|title|hspace|vspace|width|height|align|onmouseover|onmouseout|name],hr[class|width|size|noshade],font[face|size|color|style],span[class|align|style]""
});");
			contentText = new HtmlTextArea();
			contentText.ID = "contentText";
			saveButton = new Button();
			saveButton.ID = "saveButton";

			// set styles
			contentText.Cols = 85;
			contentText.Rows = 30;
			contentText.Style.Add(HtmlTextWriterStyle.Width, "100%");
			contentText.Style.Add(HtmlTextWriterStyle.Height, "600px");

			// check to see if content has been defined
			if (Properties["Content"] != null)
				contentText.Value = Properties["Content"];

			// setup save button
			saveButton.Text = "Save Page Contents";
			saveButton.Click += new EventHandler(contentTextBox_SaveClick);			

			// add to page
			this.Controls.Add(saveButton);
			this.Controls.Add(contentText);

			base.OnInit (e);
		}

		private void contentTextBox_SaveClick (object sender, EventArgs e) 
		{
			this.Properties["Content"] = contentText.Value;
			
			// go back to original page
			Response.Redirect(Common.Path.GetPortalUrl(PortalProperties.DefaultPage).ToString());
		}
	}
}
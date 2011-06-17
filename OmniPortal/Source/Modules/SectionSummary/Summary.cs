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
using ManagedFusion;
using ManagedFusion.Modules;

namespace OmniPortal.Modules.SectionSummary
{
	/// <summary>
	/// Summary description for Summary.
	/// </summary>
	public class Summary : SkinnedUserControl
	{
		protected override void OnPreRender(EventArgs e)
		{
			// add the title to the page
			this.Controls.Add(new LiteralControl(String.Concat(
				"<h3>", SectionInformation.Title, "</h3>"
				)));

			foreach(SectionInfo section in SectionInformation.Children) 
			{
				HyperLink link = new HyperLink();
				link.Text = section.Title;
				link.NavigateUrl = section.UrlPath.ToString();
				
				this.Controls.Add(link);
				this.Controls.Add(new LiteralControl(String.Concat(
					"<br>", section.Title, "<br><br>"
					)));
			}

			base.OnPreRender (e);
		}
	}
}
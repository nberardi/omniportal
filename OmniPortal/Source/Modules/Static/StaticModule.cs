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

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;
using ManagedFusion.Syndication;
using ManagedFusion.Modules;

namespace OmniPortal.Modules.Static
{
	[Module("Static Page", 
			"The static module is used inorder to display static content to the page.", 
			"{8B61FD65-C060-46f2-AC29-5A4669D010AC}")]
	public class StaticModule : ModuleBase
	{
		protected override void OnLoadSyndication(LoadSyndicationEventArgs e)
		{
			// add the item
			Entry entry = new Entry(
				this.SectionInformation.Title,
				this.SectionInformation.UrlPath,
				this.SectionInformation.Touched
				);
			
			entry.Links.Add(new Link(this.SectionInformation.UrlPath, LinkRelationship.Self));
			entry.Content = new ManagedFusion.Syndication.Content("text/html", this.Properties["Content"]);

			// add entry to the feed
			e.Syndication.Items.Add(entry);
		}

		protected override void OnLoad(LoadModuleEventArgs e)
		{
#if DEBUG
			Context.Trace.Write("StaticModule", "QueryString Edit Present: " + (Context.Request.QueryString["edit"] != null));
#endif
			// checks to see if user is an administrator and is in edit mode
			if (Context.Request.QueryString["edit"] != null && IsInTask("Editor"))
				e.CenterTop.Add(new Edit());
			else 
			{
				// add edit button for users with access to edit
				if (IsInTask("Editor"))
				{
					HyperLink editLink = new HyperLink();
					editLink.Text = "Edit This Page's Content";
					editLink.NavigateUrl = Common.Path.GetPortalUrl("Edit.aspx").ToString();

					// add link to page
					e.CenterTop.AddAt(0, editLink);
				}

				// add body of page
				e.CenterTop.Add(new LiteralControl(this.Properties["Content"]));
			}
		}
	}
}
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
using OmniPortal;
using ManagedFusion.Modules;

namespace OmniPortal.Modules.ExternalLink
{
	/// <summary>
	/// Summary description for StaticModule.
	/// </summary>
	[Module("External Link Module", 
			"This module allows you to create an external link to a URL instead of the normal section.", 
			"{BE6DC7C6-1FA1-4a9f-8E51-AF016CCF0489}")]
	public class ExternalLinkModule : ModuleBase
	{
		protected override void OnLoad(LoadModuleEventArgs e)
		{
			if(this.InternalLocation.ToLower() == "edit.aspx")
				e.CenterTop.Add(new Edit());
			else
				Context.Response.Redirect(Properties["ExternalURL"]);
		}
	}
}
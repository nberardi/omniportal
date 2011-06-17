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

namespace OmniPortal.Modules.SectionSummary
{
	/// <summary>
	/// Summary description for StaticModule.
	/// </summary>
	[Module("Section Summary Module", 
			"This module gives you an overview of the sub-sections of the section this module is used in.", 
			"{C850333C-433E-432a-8AAC-161021524FF9}")]
	public class SectionSummaryModule : ModuleBase
	{
		protected override void OnLoad(LoadModuleEventArgs e)
		{
			e.CenterTop.Add(new Summary());
		}
	}
}
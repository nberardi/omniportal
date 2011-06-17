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
using System.Collections;
using System.Configuration;
using System.Security.Permissions;
using System.Web;

using ManagedFusion;
using ManagedFusion.Configuration;
using ManagedFusion.Syndication;
using ManagedFusion.Data;
using System.Web.Configuration;

namespace ManagedFusion
{
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ManagedFusionSectionGroup : ConfigurationSectionGroup
	{
	}
}
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
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// ManagedFusion Classes
using ManagedFusion;

namespace ManagedFusion.Portlets
{
	/// <summary>
	/// Summary description for ContainerUserControl.
	/// </summary>
	public class PortletUserControl : SkinnedUserControl, INamingContainer
	{
		/// <summary>The portlet id.</summary>
		protected internal int PortletID 
		{ 
			get { return this.PortletInformation.Identity; }
		}

		/// <summary>The title of the portlet.</summary>
		protected internal string Title 
		{ 
			get { return this.PortletInformation.Title; }
		}

		/// <summary>The portlet type.</summary>
		protected internal string PortletType 
		{
			get { return this.PortletInformation.Module.FolderName; }
		}

		/// <summary>Gets a list of all properties for module.</summary>
		public new NameValueCollection Properties 
		{ 
			get { return this.PortletInformation.ModuleData; } 
		}

		public PortletInfo PortletInformation 
		{ 
			get { return (PortletInfo)ViewState["PortletInfo"]; } 
			set { ViewState["PortletInfo"] = value; } 
		}
	}
}
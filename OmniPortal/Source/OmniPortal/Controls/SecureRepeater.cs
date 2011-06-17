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
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;

namespace OmniPortal.Controls
{
	/// <summary>
	/// Summary description for SecureRepeater.
	/// </summary>
	[ToolboxData("<{0}:SecureRepeater runat=server></{0}:SecureRepeater>")]
	public class SecureRepeater : Repeater
	{	
		private const char _deliminator = ';';

		public new SectionCollection DataSource
		{
			get { return (SectionCollection)base.DataSource; }
			set { base.DataSource = value; }
		}

		public override void DataBind()
		{
			ArrayList secureList = new ArrayList();

			// remove rows that user doesn't have access to
			foreach(SectionInfo section in this.DataSource)
			{
				// check to see if user has permission
				if (section.UserHasPermissions(Permissions.Read))
					secureList.Add(section);
			}

			// set new datasource
			base.DataSource = secureList;

			// databind new datasource
			base.DataBind ();
		}
	}
}
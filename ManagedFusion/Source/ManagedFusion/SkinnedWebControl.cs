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
using System.IO;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

// ManagedFusion Classes
using ManagedFusion;

namespace ManagedFusion
{
	/// <summary>
	/// This is the base class for all WebControls.  This classes primary use
	/// is to create a dynamic location for loading of controls in webpages
	/// without direct reference.
	/// </summary>
	public abstract class SkinnedWebControl : WebControl, INamingContainer
	{
		/// <summary>
		/// Initializes the Skinned Web Control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			LoadControl();
		}

		/// <summary>
		/// Run when control is being initialized.
		/// </summary>
		protected virtual void LoadControl()
		{
			// load the control
			this.Controls.Add(GetControl());
		}

		/// <summary>The Modules name.</summary>
		protected abstract string ModuleName { get; }

		/// <summary>The Controls location.</summary>
		protected abstract string ControlLocation { get; }

		/// <summary>A reference for <see cref="SiteInfo" />.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected SiteInfo SiteInformation { get { return SiteInfo.Current; } }

		/// <summary>A reference for <see cref="SectionInfo" />.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected SectionInfo SectionInformation { get { return SectionInfo.Current; } }

		/// <summary>A reference for <see cref="CommunityInfo" />.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected CommunityInfo CommunityInformation { get { return CommunityInfo.Current; } }

		/// <summary>Gets the control that is called from the module.</summary>
		/// <remarks>
		/// Gets the control that is called from the current module.  If the control cannot be found
		/// then it goes to the default theme and gets that version of the control.
		/// </remarks>
		/// <returns>The control that was loaded.</returns>
		protected virtual Control GetControl()
		{
			Control skin = new Control();

			string moduleString = Common.Path.GetModulePath(ModuleName, ControlLocation);

			// build skin key
			skin = Common.Path.GetControlFromLocation(moduleString);

			// changes the Identity names for compatibility
			skin.ID = this.ID;
			this.ID += "_WebControl";

			return skin;
		}
	}
}
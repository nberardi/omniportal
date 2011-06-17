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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.Security;
using System.Diagnostics;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Modules;

namespace ManagedFusion
{
	/// <summary>
	/// This is the base class of all controls in the ManagedFusion. This class 
	/// enables pages, content, and controls to load different skins.
	/// </summary>
	[ParseChildren(true)]
	public class SkinnedUserControl : UserControl, INamingContainer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ManagedFusion.SkinnedUserControl">SkinnedControl</see> class. 
		/// </summary>
		/// <remarks>
		/// Retrieves the UserInfo, SectionInfo, and SiteInfo from Context
		/// making these objects automatically available to all controls
		/// in the framework.
		/// </remarks>
		protected override void OnInit(EventArgs e)
		{
			// check this controls permissions
			this.CheckPermissions();

			base.OnInit(e);

			// write the trace of this control
			this.WriteTrace();
		}

		/// <summary>A reference for <see cref="SiteInfo" />.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected SiteInfo SiteInformation { get { return SiteInfo.Current; } }

		/// <summary>A reference for <see cref="SectionInfo" />.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected SectionInfo SectionInformation { get { return SectionInfo.Current; } }

		/// <summary>A reference for <see cref="CommunityInfo" />.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected CommunityInfo CommunityInformation { get { return CommunityInfo.Current; } }

		/// <summary>Gets the properties for the current module.</summary>
		protected NameValueCollection Properties { get { return Module.Properties; } }

		/// <summary>Gets the current executing module.</summary>
		protected ModuleBase Module { get { return Common.ExecutingModule; } }

		/// <summary></summary>
		/// <param name="tasks"></param>
		/// <returns></returns>
		protected bool IsInTask(string task)
		{
			return this.SectionInformation.UserInTasks(new string[] { task });
		}

		/// <summary></summary>
		private void CheckPermissions()
		{
			if (this.GetType().IsDefined(typeof(SecureSkinAttribute), true))
			{
				bool hasAccess = false;

				SecureSkinAttribute[] attrs = (SecureSkinAttribute[])this.GetType().GetCustomAttributes(typeof(SecureSkinAttribute), true);
				foreach (SecureSkinAttribute ssa in attrs)
					hasAccess = hasAccess && this.SectionInformation.UserHasPermissions(ssa.Permissions);

				if (hasAccess == false)
					throw new ManagedFusion.Security.UnauthorizedAccessException(
						Common.Context.User.Identity.Name,
						Common.Path.UrlPath
						);
			}
		}

		/// <summary>
		/// Used to write information to the Trace Context of the Web Page.
		/// </summary>
		/// <remarks>
		/// Called by <see cref="ManagedFusion.SkinnedUserControl.OnInit">SkinnedControl.OnInit</see>.  
		/// This method must be implemented by all derived controls, and will only
		/// be used when the Conditional Compilation Constant (<code>/define</code>) 
		/// of "DEBUG" is used.
		/// <seealso cref="System.Diagnostics.ConditionalAttribute"/>
		/// </remarks>
		[Conditional("DEBUG")]
		protected virtual void WriteTrace()
		{
			// nothing is written
		}
	}
}
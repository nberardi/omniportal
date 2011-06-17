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

using ManagedFusion.Security;

namespace ManagedFusion.Portlets
{
	/// <summary>
	/// Used for quick consuming of admin modules when needed to be displayed 
	/// in list form.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public sealed class PortletAdminAttribute : SecureSkinAttribute
	{
		private readonly string _location;

		/// <summary>Creates a new admin module attribute for an admin module.</summary>
		/// <param name="location">Location a URL should point to find module.</param>
		/// <param name="title">Title of module.</param>
		/// <param name="description">Description of module.</param>
		public PortletAdminAttribute(string location, string title, string description, Permissions permissions) : base(title, description, permissions) 
		{
			// checks to see if location is set
			if (location == null || location.Length == 0)
				throw new ArgumentException("Location needs to be set for PortletAdminAttribute.  Cannot be String.Empty or null.", "folderName");

			this._location = location;
		}

		/// <summary>Fully qualified or relative URL of location of module.</summary>
		public string Location { get { return this._location; } }
	}
}
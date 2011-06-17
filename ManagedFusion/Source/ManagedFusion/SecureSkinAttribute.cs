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

// ManagedFusion Classes
using ManagedFusion.Security;

namespace ManagedFusion
{
	/// <summary>
	/// Used for skin controls that need permissions.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public abstract class SecureSkinAttribute : SkinAttribute
	{
		private Permissions _permissions;

		/// <summary>Creates a new module attribute for a module.</summary>
		/// <param name="title">Title of module.</param>
		/// <param name="description">Description of module.</param>
		/// <param name="permissions">Permissions for skin control.</param>
		protected SecureSkinAttribute(string title, string description, Permissions permissions) : base(title, description) 
		{
			this._permissions = permissions;
		}

		/// <summary>Admin options for administration modules.</summary>
		public Permissions Permissions { get { return this._permissions; } }
	}
}
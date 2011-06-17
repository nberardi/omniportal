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

namespace ManagedFusion.Security
{
	/// <summary>Default Portal Roles available across all authentication types.</summary>
	public enum PortalRole
	{
		/// <summary>Un-Authenticated User for ManagedFusion.</summary>
		NotAuthenticated	= 0x0,

		/// <summary>Authenticated User for ManagedFusion.</summary>
		Authenticated		= 0x1,

		/// <summary>Super User for ManagedFusion.</summary>
		SuperUser			= 0x2,

		/// <summary>Everybody in ManagedFusion.</summary>
		Everybody			= 0x4
	}
}
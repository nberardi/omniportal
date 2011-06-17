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

namespace ManagedFusion
{
	/// <summary>The cache expiration types.</summary>
	public enum ExpirationType
	{
		/// <summary>For internal use only.</summary>
		NotSet,

		/// <summary>Expires in a set time after the last time it was accessed.</summary>
		Sliding,

		/// <summary>Expires in a set time.</summary>
		Absolute,

		/// <summary>Uses the default expiration for the cache.</summary>
		Default
	}
}
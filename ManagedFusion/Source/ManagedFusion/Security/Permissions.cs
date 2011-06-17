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
	/// <summary>The portal permissions for user roles.</summary>
	[Flags]
	public enum Permissions 
	{
		/// <summary>No Permission, cannot be combined.</summary>
		None						= 0x00,	// 00000 - 0
		
		/// <summary>Permission to add content.</summary>
		Add							= 0x01,	// 00001 - 1
		
		/// <summary>Permission to edit content.</summary>
		Edit						= 0x02,	// 00010 - 2
		
		/// <summary>Permission to read content.</summary>
		Read						= 0x04,	// 00100 - 4
		
		/// <summary>Permission to delete content.</summary>
		Delete						= 0x08,	// 01000 - 8
		
		/// <summary>Administrative Permission.</summary>
		Administrate				= 0x10,	// 10000 - 16

		/// <summary>Add and Edit Permission.</summary>
		Write = Add | Edit,					// 00011 - 3
		
		/// <summary>Read and Write Permission.</summary>
		ReadWrite = Read | Write,			// 00111 - 7
		
		/// <summary>Read/Write and Delete Permission.</summary>
		Modify = ReadWrite | Delete,		// 01111 - 15
		
		/// <summary>Modify and Administrate Permission.</summary>
		FullControl = Modify | Administrate	// 11111 - 31
	}
}
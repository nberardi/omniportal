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
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace ManagedFusion
{
	public class RolesPermissionsDictionary : DictionaryBase
	{
		public RolesPermissionsDictionary() { }

		/// <summary>Add a role and permissions set to the Dictionary.</summary>
		/// <param name="role">The role.</param>
		/// <param name="permissions">The permissions for the role.</param>
		public void Add (string role, params string[] permissions)
		{
			// check to see if the role has already been added
			// if it hasn't add the role and permissions to the hashtable
			if (this.InnerHashtable.ContainsKey(role) == false) 
				this.InnerHashtable.Add(role, permissions);
		}

		/// <summary>Gets all the roles contained in the Dictionary.</summary>
		public List<string> Roles 
		{
			get 
			{
				List<string> tasks = new List<string>();
				tasks.AddRange((new ArrayList(this.InnerHashtable.Keys)).ToArray(typeof(string)) as string[]);
				return tasks;
			}
		}

		/// <summary>Get the permissions for the selected role.</summary>
		/// <param name="role">The role to get permissions for.</param>
		/// <returns>Returns an <see cref="System.Collection.ArrayList"/> of permissions the role has.</returns>
		public List<string> GetPermissions (string role) 
		{
			return this.InnerHashtable[role] as List<string>;
		}

		/// <summary>Get roles for the permission set.</summary>
		/// <param name="permissions">The permission set to get roles for.</param>
		/// <returns>Returns a list of roles that have the <paramref name="permissions">permissions</paramref>.</returns>
		/// <remarks>
		/// It should be noted that this is a costly operation on large 
		/// <paramref name="permissions">permissions</paramref> array's or large
		/// roles list in this dictionary.  To asses the potential number of loops
		/// this method may go through you can multiply the following: 
		/// <code><paramref name="permissions">permissions</paramref>.Length * {RolesDictionary}.Count</code>
		/// </remarks>
		public List<string> GetRoles (params string[] permissions) 
		{
			return GetRoles(permissions);
		}

		public List<string> GetRoles (List<string> permissions) 
		{
			if (permissions == null) throw new ArgumentNullException("permissions");
			if (permissions.Count == 0) return this.Roles;

			List<string> roles = new List<string>();
			IList permissonList = null;
			bool granted = false;

			// search for the permissions and add the role if found
			foreach(string role in this.Roles) 
			{
				permissonList = this[role];
				granted = true;

				// go through each permission and check to see if it
				// is in the current permissions list, if granted
				// stays true then the role is allowed access to
				// permission set
				for (int i = 0; i < permissions.Count; i++)
					if (permissonList.Contains(permissions[i]) == false)
					{
						granted = false;
						break;
					}

				// if the role is granted add to the granted roles list
				if (granted) roles.Add(role);
			}

			return roles;
		}

		/// <summary>Get the permissions for the selected role.</summary>
		/// <param name="role">The role to get permissions for.</param>
		/// <returns>Returns an <see cref="System.Collection.ArrayList"/> of permissions the role has.</returns>
		public List<string> this [string role] 
		{
			get { return this.GetPermissions(role); }
		}

	}
}
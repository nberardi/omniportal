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
	public class RolesTasksDictionary : Dictionary<string, string[]>
	{
		public RolesTasksDictionary() { }

		/// <summary>Get the tasks for the selected role.</summary>
		/// <param name="role">The role to get tasks for.</param>
		/// <returns>Returns an <see cref="System.Collection.ArrayList"/> of tasks the role has.</returns>
		public string[] GetTasks (string role) 
		{
			string[] tasks = null;
			this.TryGetValue(role, out tasks);
			return tasks;
		}

		/// <summary>Get roles for the task set.</summary>
		/// <param name="tasks">The task set to get roles for.</param>
		/// <returns>Returns a list of roles that have the <paramref name="tasks">tasks</paramref>.</returns>
		/// <remarks>
		/// It should be noted that this is a costly operation on large 
		/// <paramref name="tasks">tasks</paramref> array's or large
		/// roles list in this dictionary.  To asses the potential number of loops
		/// this method may go through you can multiply the following: 
		/// <code><paramref name="tasks">tasks</paramref>.Length * {RolesDictionary}.Count</code>
		/// </remarks>
		public List<string> GetRoles (string[] tasks) 
		{
			if (tasks == null || tasks.Length == 0) return new List<string>(this.Keys);

			List<string> roles = new List<string>();
			IList taskList = null;
			bool granted = false;

			// search for the tasks and add the role if found
			foreach(string role in this.Keys) 
			{
				taskList = this[role];
				granted = false;

				// go through each task and check to see if it
				// is in the current tasks list, if granted
				// stays true then the role is allowed access to
				// task set
				for (int i = 0; i < tasks.Length; i++)
				{
					if (taskList.Contains(tasks[i]))
					{
						granted = true;
						break;
					}
				}

				// if the role is granted add to the granted roles list
				if (granted) roles.Add(role);
			}

			return roles;
		}
	}
}
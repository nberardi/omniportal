using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

using ManagedFusion.Security;

namespace OmniPortal.Security
{
	public class PortalRoleProvider : RoleProvider
	{
		private string _applicationName;
		private static readonly string[] _roles = new string[] { "Administrator", "User" };

		public override string Name
		{
			get { return "Portal"; }
		}

		public override string ApplicationName
		{
			get { return _applicationName; }
			set { _applicationName = value; }
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void CreateRole(string roleName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			return new string[0];
		}

		public override string[] GetAllRoles()
		{
			return _roles;
		}

		public override string[] GetRolesForUser(string username)
		{
			return new string[0];
		}

		public override string[] GetUsersInRole(string roleName)
		{
			return new string[0];
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			if (roleName == PortalRole.Everybody.ToString()) return true;

			if (roleName == PortalRole.Authenticated.ToString()
				&& username != PortalRole.NotAuthenticated.ToString()) return true;

			return username == roleName;
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool RoleExists(string roleName)
		{
			return true;
		}
	}
}

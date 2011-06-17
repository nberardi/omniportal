using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Web.Security;

namespace ManagedFusion.Security
{
	public abstract class PortletSecurityProvider : ProviderBase
	{
		public abstract string ApplicationName { get; set; }

		public abstract void AddRoleToPortlet(string role, Permissions permissions, PortletInfo portlet);
		public abstract void UpdateRoleForPortlet(string role, Permissions permissions, PortletInfo portlet);
		public abstract void RemoveRoleFromPortlet(string role, PortletInfo portlet);

		public abstract RolesPermissionsDictionary GetAllRoles(PortletInfo portlet);
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Security.Portal
{
	public class PortalPortletSecurityProvider : PortletSecurityProvider
	{
		private string _applicationName;

		public override string Name
		{
			get { return "Portal"; }
		}

		public override string ApplicationName
		{
			get { return _applicationName; }
			set { _applicationName = value; }
		}

		public override void AddRoleToPortlet(string role, Permissions permissions, PortletInfo portlet)
		{
			Common.DatabaseProvider.AddRoleForPortlet(role, permissions.ToString().Replace(", ", Common.Delimiter.ToString()).Split(Common.Delimiter), portlet);
		}

		public override void UpdateRoleForPortlet(string role, Permissions permissions, PortletInfo portlet)
		{
			Common.DatabaseProvider.UpdateRoleForPortlet(role, permissions.ToString().Replace(", ", Common.Delimiter.ToString()).Split(Common.Delimiter), portlet);
		}

		public override void RemoveRoleFromPortlet(string role, PortletInfo portlet)
		{
			Common.DatabaseProvider.RemoveRoleForPortlet(role, portlet);
		}

		public override RolesPermissionsDictionary GetAllRoles(PortletInfo portlet)
		{
			return Common.DatabaseProvider.GetRolesForPortlet(portlet);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Web.Security;

namespace ManagedFusion.Security
{
	public abstract class SectionSecurityProvider : ProviderBase
	{
		public abstract string ApplicationName { get; set; }

		public abstract void AddRoleToSection(string role, string[] taskNames, SectionInfo section);
		public abstract void UpdateRoleForSection(string role, string[] taskNames, SectionInfo section);
		public abstract void RemoveRoleFromSection(string role, SectionInfo section);

		public abstract RolesTasksDictionary GetAllRoles(SectionInfo section);
		public abstract string[] GetAllTasks(SectionInfo section);

		public abstract bool TaskExists(string taskName, SectionInfo section);
	}
}

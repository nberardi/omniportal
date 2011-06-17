using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Security.Portal
{
	public class PortalSectionSecurityProvider : SectionSecurityProvider
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

		public override void AddRoleToSection(string role, string[] taskNames, SectionInfo section)
		{
			Common.DatabaseProvider.AddRoleForSection(role, taskNames, section);
		}

		public override void UpdateRoleForSection(string role, string[] taskNames, SectionInfo section)
		{
			Common.DatabaseProvider.UpdateRoleForSection(role, taskNames, section);
		}

		public override void RemoveRoleFromSection(string role, SectionInfo section)
		{
			Common.DatabaseProvider.RemoveRoleForSection(role, section);
		}

		public override RolesTasksDictionary GetAllRoles(SectionInfo section)
		{
			return Common.DatabaseProvider.GetRolesForSection(section);
		}

		public override string[] GetAllTasks(SectionInfo section)
		{
			List<string> list = new List<string>();
			foreach (ManagedFusion.Modules.Configuration.ConfigurationTask task in section.Module.Config.Tasks)
				list.Add(task.Name);
			return list.ToArray();
		}

		public override bool TaskExists(string taskName, SectionInfo section)
		{
			ArrayList list = new ArrayList(GetAllTasks(section));
			return list.Contains(taskName);
		}
	}
}

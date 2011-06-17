using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Data.SqlServer2000
{
	public partial class ContainerPortletLink
	{
		public static List<ContainerPortletLink> GetByContainerID(int containerID)
		{
			return GetList("ContainerID = " + containerID);
		}
	}
}

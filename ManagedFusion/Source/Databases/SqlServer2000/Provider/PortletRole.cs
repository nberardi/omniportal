using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Data.SqlServer2000
{
	public partial class PortletRole
	{
		public static List<PortletRole> GetByPortletID(int portletID)
		{
			return GetList("PortletID = " + portletID);
		}
	}
}

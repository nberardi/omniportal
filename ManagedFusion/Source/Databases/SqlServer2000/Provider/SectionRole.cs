using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Data.SqlServer2000
{
	public partial class SectionRole
	{
		public static List<SectionRole> GetBySectionID(int sectionID)
		{
			return GetList("SectionID = " + sectionID);
		}
	}
}

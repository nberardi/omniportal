using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Data.SqlServer2000
{
	public partial class Community
	{
		public static implicit operator CommunityInfo(Community c)
		{
			return new CommunityInfo(
				c._communityID,
				c._universalID,
				c._name,
				c._touched
				);
		}

		public static explicit operator Community(CommunityInfo c)
		{
			Community community = new Community();
			community._communityID = c.Identity;
			community._universalID = c.UniversalID;
			community._name = c.Title;
			community._description = c.Title;
			community.Touched = c.Touched;
			return community;
		}
	}
}

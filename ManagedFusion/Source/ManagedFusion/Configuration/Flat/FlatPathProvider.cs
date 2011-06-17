using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Configuration.Flat
{
	internal class FlatPathProvider : CommunityPathProvider
	{
		protected override string GetCommunityPath(int communityID, string location)
		{
			return RemoveDoubleSeperators(PortalProperties.WebPathSeperator, "/" + location);
		}

		protected override string GetDefaultPath(string location)
		{
			return RemoveDoubleSeperators(PortalProperties.WebPathSeperator, "/" + location);
		}
	}
}

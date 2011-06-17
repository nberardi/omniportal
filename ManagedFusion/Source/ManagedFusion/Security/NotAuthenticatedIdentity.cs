using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace ManagedFusion.Security
{
	[Serializable]
	internal class NotAuthenticatedIdentity : IIdentity
	{
		#region IIdentity Members

		public string AuthenticationType
		{
			get { return "ManagedFusion NotAuthenticated Identity"; }
		}

		public bool IsAuthenticated
		{
			get { return false; }
		}

		public string Name
		{
			get { return PortalRole.NotAuthenticated.ToString(); }
		}

		#endregion
	}
}

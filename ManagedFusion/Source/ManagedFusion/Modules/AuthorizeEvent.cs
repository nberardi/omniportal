using System;
using System.Collections.Generic;
using System.Text;

using ManagedFusion.Security;

namespace ManagedFusion.Modules
{
	public delegate void AuthorizeEventHandler (object sender, AuthorizeEventArgs e);

	public class AuthorizeEventArgs
	{
		private PortalPrincipal _user;

		public AuthorizeEventArgs(PortalPrincipal user)
		{
			this._user = user;
		}

		public PortalPrincipal AuthenticatedUser
		{
			get { return _user; }
		}
	}
}

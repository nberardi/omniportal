#region Copyright © 2004, Nicholas Berardi
/*
 * ManagedFusion (www.ManagedFusion.net) Copyright © 2004, Nicholas Berardi
 * All rights reserved.
 * 
 * This code is protected under the Common Public License Version 1.0
 * The license in its entirety at <http://opensource.org/licenses/cpl.php>
 * 
 * ManagedFusion is freely available from <http://www.ManagedFusion.net/>
 */
#endregion

using System;
using System.Security.Principal;
using System.Web.Security;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;

namespace OmniPortal.Authentication.Windows
{
	//public class WindowsUserIdentity : PortalIdentity
	//{
	//    private WindowsIdentity _identity;

	//    public WindowsUserIdentity(Guid id, WindowsIdentity identity) : base(id, identity.Name)
	//    {
	//        if (identity == null) throw new ArgumentNullException("identity");

	//        this._identity = identity;
	//    }

	//    public override bool IsAuthenticated
	//    {
	//        get { return _identity.IsAuthenticated; }
	//    }
	//}
}
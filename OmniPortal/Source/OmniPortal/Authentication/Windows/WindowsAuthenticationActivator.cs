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
using System.Web;
using System.Security.Principal;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Authentication.Windows
{
	//public class WindowsAuthenticationActivator : ProviderActivator
	//{
	//    protected override object CreateInstance(Type type)
	//    {
	//        // get the current http context
	//        HttpContext context = HttpContext.Current;
			
	//        // check to make sure user and identity exisit
	//        if (context.User == null || context.User.Identity == null) return null;

	//        WindowsUserIdentity identity = new WindowsUserIdentity(Guid.NewGuid(), (WindowsIdentity)context.User.Identity);

	//        // create arguments to pass into the provider type
	//        object[] args = new object[] {
	//                                         identity
	//                                     };

	//        // create provider with the arguments from above
	//        return Activator.CreateInstance(type, args);
	//    }
	//}
}
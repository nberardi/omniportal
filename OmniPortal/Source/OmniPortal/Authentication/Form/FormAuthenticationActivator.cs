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
using System.Web.Security;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;

namespace OmniPortal.Authentication.Form
{
	//public class FormAuthenticationActivator : ProviderActivator
	//{
	//    protected override object CreateInstance(Type type)
	//    {
	//        // get the current http context
	//        HttpContext context = HttpContext.Current;

	//        // the identity of the current user
	//        FormUserIdentity identity = null;

	//        IAuthenticationProviderHandler auth = CommunityInfo.Current.Config.GetProvider("Authentication") as IAuthenticationProviderHandler;

	//        // check to make sure user and identity exisit
	//        if (context.User == null || context.User.Identity == null) 
	//        {
	//            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
	//                1,								// version
	//                PortalIdentity.AnonymousName,	// user name
	//                DateTime.Now,					// issue time
	//                DateTime.Now.AddHours(1),		// expires every hour
	//                false,							// don't persist cookie
	//                String.Empty					// roles
	//                );

	//            // get generic anonymous identity
	//            identity = new FormUserIdentity(Guid.Empty, new FormsIdentity(ticket));
	//        } 
	//        else
	//        {
	//            // get current identity
	//            identity = new FormUserIdentity(
	//                auth.GetUsersID(context.User.Identity.Name),
	//                context.User.Identity as FormsIdentity
	//                );
	//        }

	//        // create arguments to pass into the provider type
	//        object[] args = new object[] {
	//                                         identity,
	//                                         auth
	//                                     };

	//        // create provider with the arguments from above
	//        object o = Activator.CreateInstance(type, args);

	//        return o;
	//    }
	//}
}
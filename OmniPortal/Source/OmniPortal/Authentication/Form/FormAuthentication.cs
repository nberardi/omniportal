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
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;

namespace OmniPortal.Authentication.Form
{
	//[Provider("OmniPortalFormAuthentication", typeof(FormAuthenticationActivator))]
	//public class FormAuthentication : PortalPrincipal 
	//{
	//    private IAuthenticationProviderHandler _authentication;

	//    protected readonly string CookieName = CommunityInfo.Current.UniversalID.ToString();

	//    public FormAuthentication(IPortalIdentity user, IAuthenticationProviderHandler authentication) : base(user)
	//    {
	//        // get the provider for this authentication
	//        this._authentication = authentication;
	//    }

	//    public new FormUserIdentity Identity 
	//    {
	//        get { return (FormUserIdentity)base.Identity; }
	//    }

	//    #region Methods

	//    public bool Login(string name, string password, bool persistant)
	//    {
	//        FormsAuthentication.Initialize();

			
	//        ListDictionary parameters = new ListDictionary();
	//        parameters.Add("name", name);
	//        parameters.Add("password", password);

	//        IPortalIdentity user = this._authentication.GetAuthorizedUser(parameters);

	//        // set authentication cookie if valide
	//        if (user != null && user.IsAuthenticated)
	//            FormsAuthentication.SetAuthCookie(user.Name, persistant);
				
	//        return (user != null && user.IsAuthenticated);
	//    }

	//    public void Logout()
	//    {
	//        // clears the form authentication ticket
	//        FormsAuthentication.SignOut();
	//    }

	//    protected FormsAuthenticationTicket GetTicket (HttpContext context) 
	//    {
	//        FormsAuthenticationTicket ticket = null;

	//        if (context.Request.Cookies[CookieName] == null 
	//            || context.Request.Cookies[CookieName].Value.Length > 0) 
	//        {
	//            List<string> roles = this.Authorization.GetRoles(this.Identity);
	//            string[] userRoles = new string[roles.Count];
	//            roles.CopyTo(userRoles, 0);

	//            // Format string array
	//            string formattedUserRoles = String.Join(Common.Delimiter.ToString(), userRoles);

	//            // Create authentication ticket
	//            ticket = new FormsAuthenticationTicket(
	//                1,								// version
	//                context.User.Identity.Name,		// user name
	//                DateTime.Now,					// issue time
	//                DateTime.Now.AddHours(1),		// expires every hour
	//                false,							// don't persist cookie
	//                formattedUserRoles				// roles
	//                );
	//        } 
	//        else 
	//        {
	//            // decrypt the current cookie into a ticket
	//            ticket = FormsAuthentication.Decrypt(context.Request.Cookies[CookieName].Value);
	//        }

	//        return ticket;
	//    }
	//}

	//#endregion
}
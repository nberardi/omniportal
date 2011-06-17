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

using ManagedFusion;
using ManagedFusion.Security;

namespace OmniPortal.Users
{
	//public class UserInfo
	//{
	//    private IPortalIdentity _user;
	//    private UserProfile _profile;

	//    public static UserInfo Current 
	//    {
	//        get 
	//        {
	//            if (Common.Context.Items["OmniPortalUserInfo"] == null)
	//                Common.Context.Items["OmniPortalUserInfo"] = new UserInfo(Common.Security.Identity);

	//            return Common.Context.Items["OmniPortalUserInfo"] as UserInfo;
	//        }
	//    }

	//    public UserInfo (IPortalIdentity user) 
	//    {
	//        this._user = user;
	//        this._profile = new UserProfile(user.Profile);
	//    }

	//    public string UserName 
	//    {
	//        get { return this._user.Name; }
	//    }

	//    public Guid Identity 
	//    {
	//        get { return this._user.Identity; }
	//    }

	//    public UserProfile Profile 
	//    {
	//        get { return this._profile; }
	//    }
	//}
}
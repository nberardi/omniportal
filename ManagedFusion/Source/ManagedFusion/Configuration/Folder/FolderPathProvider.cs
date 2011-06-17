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
using System.IO;
using System.Collections.Generic;

namespace ManagedFusion.Configuration.Folder
{
	internal class FolderPathProvider : CommunityPathProvider
	{
		private List<string> Directories;

		public FolderPathProvider () 
		{
			// create instance of directories
			Directories = new List<string>();

			// portal directory reference
			string communityPath = Common.Context.Request.ApplicationPath + "/" + "Communities";
			DirectoryInfo portalDirectory = new DirectoryInfo(Common.Context.Server.MapPath(communityPath));

			// get list of all directories
			foreach(DirectoryInfo info in portalDirectory.GetDirectories())
				Directories.Add(info.Name);
		}

		protected override string GetCommunityPath(int communityID, string location)
		{
			location = location.Replace("\\", "/");

			if (Directories.Contains(communityID.ToString()) == false)
				throw new ApplicationException(String.Concat("The community with id ", communityID, " could not be found."));

			if (location.StartsWith("/Communities/" + communityID))
				return location;

			string path = "/Communities/" + communityID + "/" + location;
		
			path = RemoveDoubleSeperators(PortalProperties.WebPathSeperator, path);

			return path;
		}

		protected override string GetDefaultPath(string location)
		{
			location = location.Replace("\\", "/");

			if (location.StartsWith("/Communities/Default"))
				return location;

			string path = String.Concat("/Communities/Default/", location);
		
			path = RemoveDoubleSeperators(PortalProperties.WebPathSeperator, path);

			return path;
		}
	}
}
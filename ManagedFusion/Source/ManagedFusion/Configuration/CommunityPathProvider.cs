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
using System.Web;
using System.Web.UI;
using System.Configuration.Provider;

// ManagedFusion Classes
using ManagedFusion;

namespace ManagedFusion.Configuration
{
	public abstract class CommunityPathProvider : ProviderBase
	{
		protected abstract string GetCommunityPath (int communityID, string location);
		protected abstract string GetDefaultPath (string location);

		private string FormatPath (string path, string appPath) 
		{
			if (path == null) throw new ArgumentNullException("path");

			// checks to see if path is already formated
			if (path.StartsWith(appPath))
				return path;

			// checks to see if proposed path starts with a '/' and adds it if it doesn't
			if (path[0] != '/' && path[0] != '\\')
				path = String.Concat("/", path);

			// returns a relative path for the application
			return appPath + path;
		}

		protected string RemoveDoubleSeperators (char seperator, string path) 
		{
			string singleSeperator = seperator.ToString();
			string doubleSeperator = singleSeperator + singleSeperator;

			// remove all double seperators
			while(path.IndexOf(doubleSeperator) != -1)
				path = path.Replace(doubleSeperator, singleSeperator);

			return path;
		}

		#region Application Path

		protected internal readonly string ApplicationPath = "/";

		protected internal string WebApplicationPath 
		{ 
			get 
			{ 
				if (Common.Context.Request.ApplicationPath == ApplicationPath)
					return String.Empty;
				return Common.Context.Request.ApplicationPath;
			}
		}

		protected internal string PhysicalApplicationPath 
		{
			get { return Common.Context.Request.PhysicalApplicationPath; }
		}

		#endregion

		#region Verify

		public bool VerifyCommunityPath(int communityID, string location, out string path)
		{
			// combine community path with location
			path = GetCommunityPath(communityID, location);

			// verify file and directory
			if (this.VerifyFile (path) || this.VerifyDirectory (path))
				return true;

			// if all else fails reset path and return false
			path = null;
			return false;
		}

		public bool VerifyDefaultPath(string location, out string path)
		{
			// combine default path with location
			path = GetDefaultPath(location);

			// verify file and directory
			if (this.VerifyFile (path) || this.VerifyDirectory (path))
				return true;

			// if all else fails reset path and return false
			path = null;
			return false;
		}

		public bool VerifyDirectory (string path) 
		{
			// verify directory
			if (Directory.Exists(FormatDiskPath(path, PhysicalApplicationPath)))
				return true;

			return false;
		}

		public bool VerifyFile (string path) 
		{
			// veryify file
			if (File.Exists(FormatDiskPath(path, PhysicalApplicationPath)))
				return true;

			return false;
		}

		#endregion

		#region URL

		/// <summary>Gets the path that is pointed to with in the web application.</summary>
		/// <returns></returns>
		public string UrlPath
		{
			get { return GetUrlPath(Common.Context); }
		}

		/// <summary>Gets the path that is pointed to with in the web application.</summary>
		/// <returns></returns>
		public string GetUrlPath(HttpContext context)
		{
			// get URL information and base path
			string basePath = context.Request.PathInfo;

			// check to see if there was a path set
			// of if the path doesn't have a file specified and doesn't end in
			// a '/' mark
			if (basePath.Length == 0 || (
				basePath.Substring(basePath.LastIndexOf('/')).IndexOf('.') == -1 &&
				basePath[basePath.Length - 1] != '/'))
				basePath += '/';

			// check to see if there is a referenced page
			if (basePath[basePath.Length - 1] == '/')
				basePath += PortalProperties.DefaultPage;

			return basePath;
		}

		/// <summary>Gets the path to a community section.</summary>
		/// <returns></returns>
		public string BaseUrlPath
		{
			get { return GetBaseUrlPath(Common.Context); }
		}

		/// <summary>Gets the path that is pointed to with in the web application.</summary>
		/// <returns></returns>
		public string GetBaseUrlPath(HttpContext context)
		{
			string requestPath = GetUrlPath(context);
			int slashIndex = requestPath.LastIndexOf('/');

			// Remove page name
			if (slashIndex < requestPath.Length)
				requestPath = requestPath.Substring(0, slashIndex + 1);

			return requestPath;
		}
		
		/// <summary>
		/// Is the path in the URL part of ManagedFusion or an outside path.
		/// </summary>
		public bool IsManagedFusionPath 
		{
			get 
			{
				// gets base path of the request
				string currentPath = Common.Context.Request.Url.ToString().ToLower();
				string comparePath = GetPortalUrl(String.Empty).ToString().ToLower();

				// returns a boolean value if the FilePath ends with ApplicationPath + FileLocation
				return currentPath.StartsWith(comparePath);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="relativePath"></param>
		/// <returns></returns>
		public Uri GetAbsoluteUrl (string relativePath) 
		{
			return new Uri(
				Common.Context.Request.Url,
				FormatPath(relativePath, WebApplicationPath)
				);
		}

		/// <summary>Retreives an absolute Url from a relative path.</summary>
		/// <param name="relativePath">The relative path that needs to be processed.</param>
		/// <remarks>
		/// If the relativePath contains a '~' mark then the the '~' mark is removed and
		/// the path that follows is the path that gets used.  Otherwise a path is created from
		/// SectionInfo.Path and the relativePath.
		/// </remarks>
		/// <returns>Returns the absolute URL.</returns>
		public Uri GetPortalUrl (string relativePath) 
		{
			// check to see if this is an absolute path 
			if (relativePath.Length > 0 && relativePath[0] == '~')
				return this.GetAbsoluteUrl(relativePath.Remove(0, 1));
			
			relativePath = FormatPortalUrlPath(relativePath, WebApplicationPath);

			// returns a url with the base + the a relative part
			return new Uri(
				Common.Context.Request.Url,
				relativePath
				);
		}

		#region Protected Methods

		/// <summary>Retreives the relative path of the proposted path sent in.</summary>
		/// <param name="path">The proposted path.</param>
		/// <returns>The relative path of the proposedPath.</returns>
		protected string FormatPortalUrlPath (string path, string appPath) 
		{
			if (path == null || path.Length == 0)
				return appPath + "/Default.aspx";

			SectionInfo sectionToUse = SectionInfo.Current;

			// check to see if there is a path in the proposedPath
			if (path[0] != '/' || path.StartsWith("./"))
				path = sectionToUse.Path + path;

			// checks to see if proposed path starts with a '/' and adds it if it doesn't
			if (path[0] != '/')
				path = "/" + path;

			// returns the relative path for the proposed path
			return this.RemoveDoubleSeperators('/', String.Format("{0}/Default.aspx{1}", appPath, path));
		}

		#endregion

		#endregion

		#region Disk

		public string GetAbsoluteDiskPath (string relativePath) 
		{
			return this.GetDiskPath(relativePath, this.PhysicalApplicationPath);
		}

		public string GetRelativeDiskPath (string relativePath) 
		{
			return this.GetDiskPath(relativePath, this.ApplicationPath);
		}

		#region Protected Methods

		protected string GetDiskPath (string relativePath, string applicationPath) 
		{
			if (relativePath == null) throw new ArgumentNullException("relativePath");

			string path;
			
			if (this.VerifyCommunityPath(CommunityInfo.Current.Identity, relativePath, out path))
				goto ICanNotBeleiveIUsedAGoTo;

			if (this.VerifyDefaultPath(relativePath, out path))
				goto ICanNotBeleiveIUsedAGoTo;

			if (path == null)
				throw new ApplicationException(
					String.Concat("The path, ", FormatDiskPath(relativePath, applicationPath), ", is not valid or is not a relative path.")
					);

ICanNotBeleiveIUsedAGoTo:

			return FormatDiskPath(path, applicationPath);
		}

		protected string FormatDiskPath (string path, string appPath) 
		{
			if (path == null) throw new ArgumentNullException("path");
			if (appPath == null) throw new ArgumentNullException("appPath");

			path = FormatPath(path, appPath);
			path = path.Replace("/", @"\");
			path = RemoveDoubleSeperators('\\', path);

			return path;
		}

		#endregion

		#endregion

		#region Disk Layout

		#region Page

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public string GetPagePath (string location) 
		{
			if (location == null) throw new ArgumentNullException("folderName");

			return GetSkinPath(String.Concat("Pages/", location));
		}

		#endregion

		#region Module

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="location"></param>
		/// <returns></returns>
		public string GetModulePath (string moduleName, string location) 
		{
			if (moduleName == null) throw new ArgumentNullException("moduleName");
			if (location == null) throw new ArgumentNullException("folderName");

			return GetModulePath(String.Concat(moduleName, "/", location));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		internal string GetModulePath (string location)
		{
			if (location == null) throw new ArgumentNullException("folderName");

			return GetSkinPath(String.Concat("Modules/", location));
		}

		#endregion

		#region Portlet

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public string GetPortletPath (string portletName, string location) 
		{
			if (portletName == null) throw new ArgumentNullException("portletName");
			if (location == null) throw new ArgumentNullException("folderName");

			return GetPortletPath(String.Concat(portletName, "/", location));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		internal string GetPortletPath (string location) 
		{
			if (location == null) throw new ArgumentNullException("folderName");

			return GetSkinPath(String.Concat("Portlets/", location));
		}

		#endregion

		#region Style

		/// <summary>
		/// Gets the style path from the passed location.
		/// </summary>
		/// <param name="location">The relative location of the style.</param>
		/// <returns>Returns the style of the path.</returns>
		public string GetStylePath (string location) 
		{
			if (location == null) throw new ArgumentNullException("folderName");

			return GetStylePath(location, true);
		}

		/// <summary>
		/// Gets the style path from the passed location.
		/// </summary>
		/// <param name="location">The relative location of the style.</param>
		/// <returns>Returns the style of the path.</returns>
		public string GetStylePath (string location, bool forWeb) 
		{
			if (location == null) throw new ArgumentNullException("folderName");

			string path = GetThemedPath(PortalProperties.StylesDirectory, location);

			if (forWeb)
				return this.GetAbsoluteUrl(path).ToString();

			return this.GetRelativeDiskPath(path);
		}

		#endregion

		#region Image

		/// <summary>
		/// Gets an image location from the themes image directory.
		/// </summary>
		/// <param name="location">The relative location of the image.</param>
		/// <returns>Returns a valid URL for the image.</returns>
		public string GetImagePath (string location) 
		{
			if (location == null) throw new ArgumentNullException("folderName");

			return GetImagePath(location, true);
		}

		/// <summary>
		/// Gets an image location from the themes image directory.
		/// </summary>
		/// <param name="location">The relative location of the image.</param>
		/// <returns>Returns a valid URL for the image.</returns>
		public string GetImagePath (string location, bool forWeb) 
		{
			if (location == null) throw new ArgumentNullException("folderName");

			string path = GetThemedPath("Images", location);

			if (forWeb) 
			{
				// this is done so that the page will still render with a broken image
				// instead of throwing an error.
				if (path == null)
					path = this.GetUnverifiedThemedPath(SectionInfo.Current.Theme.Name, "Images", location);
				
				// get the path for the url
				return this.GetAbsoluteUrl(path).ToString();
			}

			// else get a relative disk path
			return this.GetRelativeDiskPath(path);
		}

		#endregion

		#region Skin

		/// <summary>
		/// Gets the relative path of the skin.
		/// </summary>
		/// <remarks>
		/// This method returns a path in the following format 
		/// "/Themes/<paramref name="skin">skin</paramref>/Skin/<paramref name="type">type</paramref>/[Mobile|Desktop]/<paramref name="file">file</paramref>".
		/// </remarks>
		/// <param name="location"></param>
		/// <returns></returns>
		internal string GetSkinPath (string location) 
		{
			if (location == null) throw new ArgumentNullException("folderName");

			return GetThemedPath("Skin", location);
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theme"></param>
		/// <param name="folder"></param>
		/// <param name="location"></param>
		/// <returns></returns>
		internal string GetThemedPath(ThemeInfo theme, string folder, string location) 
		{
			return this.GetThemedPath(theme, folder, location, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theme"></param>
		/// <param name="folder"></param>
		/// <param name="location"></param>
		/// <param name="forWeb"></param>
		/// <returns></returns>
		internal string GetThemedPath(ThemeInfo theme, string folder, string location, bool forWeb) 
		{
			if (theme == null) throw new ArgumentNullException("theme");
			if (folder == null) throw new ArgumentNullException("folder");
			if (location == null) throw new ArgumentNullException("folderName");

			string path;
			string unverifiedPath = this.GetUnverifiedThemedPath(theme.Name, folder, location);

			// check to see if the path is suppose to be from default
			if (theme.IsDefaultTheme && this.VerifyDefaultPath(unverifiedPath, out path))
				goto PathSuccess;
			// check to see if the path is suppose to be from community
			else if (theme.IsDefaultTheme == false && this.VerifyCommunityPath(theme.CommunityID, unverifiedPath, out path))
				goto PathSuccess;

			// if all else fails return null;
			return null;

PathSuccess:
			if (forWeb)
				return this.GetAbsoluteUrl(path).ToString();

			return GetRelativeDiskPath(path);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="location"></param>
		/// <returns></returns>
		protected string GetThemedPath(string folder, string location) 
		{
			if (folder == null) throw new ArgumentNullException("folder");
			if (location == null) throw new ArgumentNullException("folderName");

			string path;
			string currentLocation;
			int communityID = CommunityInfo.Current.Identity;

			// the purpose of the flags is to sync with the locations array and set the type of
			// check that should be preformed on the path
			//	1 = check community path
			//	2 = check default path
			//	3 = check both
			byte[] flags = new byte[4] { 3, 1, 2, 3 };

			string[] locations = new string[] {
					(SectionInfo.Current.Theme == null) ? null : this.GetUnverifiedThemedPath(SectionInfo.Current.Theme.Name, folder, location),
					this.GetUnverifiedThemedPath(CommunityInfo.Current.Config.DefaultTheme, folder, location),
					this.GetUnverifiedThemedPath(Common.Configuration.Default.DefaultTheme, folder, location),
					location
				};

			// try to find a valid path
			for(int i = 0; i < locations.Length; i++)
			{
				// get the location
				currentLocation = locations[i];

				// if the second item in the list is the same as the
				// first item skip it because the process has already
				// been done and failed
				if (String.IsNullOrEmpty(currentLocation) || (i == 1 && currentLocation == locations[0]))
					continue;

				// check community path
				if (Convert.ToBoolean(flags[i] & 1) && this.VerifyCommunityPath(communityID, currentLocation, out path))
					return path;

				// check default path
				if (Convert.ToBoolean(flags[i] & 2) && this.VerifyDefaultPath(currentLocation, out path))
					return path;
			}

			// return nothing found
			return null;
		}

		#region Get Style/Theme Directory

		public string GetCommunityStyleDirectory (int communityID, string skin) 
		{
			string path;
			if (this.VerifyCommunityPath(communityID, this.GetUnverifiedThemedPath(skin, PortalProperties.StylesDirectory, String.Empty), out path))
				return path;

			return null;
		}

		public string GetDefaultStyleDirectory (string skin) 
		{
			string path;
			if (this.VerifyDefaultPath(this.GetUnverifiedThemedPath(skin, PortalProperties.StylesDirectory, String.Empty), out path))
				return path;

			return null;
		}

		public string GetCommunitySkinDirectory (int communityID, string skin) 
		{
			string path;
			if (this.VerifyCommunityPath(communityID, this.GetUnverifiedThemedPath(skin, String.Empty, String.Empty), out path))
				return path;

			return null;
		}

		public string GetDefaultSkinDirectory (string skin) 
		{
			string path;
			if (this.VerifyDefaultPath(this.GetUnverifiedThemedPath(skin, String.Empty, String.Empty), out path))
				return path;

			return null;
		}

		public string GetCommunityThemeDirectory (int communityID) 
		{
			string path;
			if (this.VerifyCommunityPath(communityID, PortalProperties.ThemeDirectory, out path))
				return path;

			return null;
		}

		public string GetDefaultThemeDirectory () 
		{
			string path;
			if (this.VerifyDefaultPath(PortalProperties.ThemeDirectory, out path))
				return path;

			return null;
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="skin"></param>
		/// <param name="folder"></param>
		/// <param name="location"></param>
		/// <returns></returns>
		internal string GetUnverifiedThemedPath (string skin, string folder, string location) 
		{
			if (skin == null) throw new ArgumentNullException("skin");
			if (folder == null) throw new ArgumentNullException("folder");
			if (location == null) throw new ArgumentNullException("folderName");

			// concat together the path
			string path = String.Format("/{0}/{1}/{2}/{3}",
				PortalProperties.ThemeDirectory,
				skin,
				folder,
				location
				);

			// remove all double seperators
			return this.RemoveDoubleSeperators('/', path);
		}

		#endregion

		#region Control

		/// <summary>
		/// Get a <see cref="System.Web.UI.Control">Control</see> from the specified 
		/// loation.
		/// </summary>
		/// <param name="controlLocation">The location of the control.</param>
		/// <returns>The control to display on the screen.</returns>
		public Control GetControlFromLocation (string controlLocation) 
		{
			Control skin = new Control();
			TemplateControl template = new TemplateControl();
			template.AppRelativeVirtualPath = "~" + SectionInfo.Current.UrlPath.PathAndQuery;

			// checks to see if the application path was included in the controlLocation
			controlLocation = GetDiskPath(controlLocation, WebApplicationPath);

			// attempt to load control
			try { skin = template.LoadControl(controlLocation); } 
			catch (FileNotFoundException) 
			{
				// nothing worked code is giving up
				throw new  ApplicationException(controlLocation);
			}

			return skin;
		}

		/// <summary>A light weight version of the <see cref="TemplateControl"/>.</summary>
		private sealed class TemplateControl : System.Web.UI.TemplateControl {}

		#endregion
	}
}
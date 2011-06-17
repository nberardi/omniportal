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

namespace ManagedFusion
{
	/// <summary>The static properties of the portal.</summary>
	/// <remarks>
	/// This class has the static properties for the portal that will not change.  Context properties can be
	/// found in the class <see cref="PortalContext"/>.  Config properties from <c>Web.Config</c> can
	/// be found in the class <see cref="PortalConfig"/>.
	/// </remarks>
	public static class PortalProperties
	{
		/// <summary>The Default Page used for the main processor page as well as paths with no page listed.</summary>
		public const string DefaultPage = "Default.aspx";

		/// <summary>The path seperator for web URLs.</summary>
		public const char WebPathSeperator = '/';

		/// <summary>The path seperator for physical disks.</summary>
		public const char DiskPathSeperator = '\\';

		/// <summary>The Default Community directory name.</summary>
		public const string DefaultCommunity = "Default";

		/// <summary>Modules directory name.</summary>
		public const string PagesDirectory = "Pages";

		/// <summary>Modules directory name.</summary>
		public const string ModulesDirectory = "Modules";

		/// <summary>Portlets directory name.</summary>
		public const string PortletsDirectory = "Portlets";

		/// <summary>Images directory name.</summary>
		public const string ImagesDirectory = "Images";

		/// <summary>Skins directory name.</summary>
		public const string SkinsDirectory = "Skin";

		/// <summary>Styles directory name.</summary>
		public const string StylesDirectory = "Styles";

		/// <summary>The directory of where the themes are located in the current web application.</summary>
		public const string ThemeDirectory = "Themes";

		/// <summary>The page template file.</summary>
		public const string PageTemplateFile = "Template.master";

		/// <summary>The portlet template file.</summary>
		public const string PortletTemplateFile = "Template.ascx";

		/// <summary>The header file.</summary>
		public const string HeaderFile = "Head.html";

		/// <summary>The community config file.</summary>
		public const string CommunityConfigFile = "Community.config";

		/// <summary>The module config file.</summary>
		public const string ModuleConfigFile = "Module.config";

		/// <summary>The formal name of this portal software.</summary>
		public const string PortalName = "ManagedFusion";

		/// <summary>The formal <see cref="Uri"/> of this portal software.</summary>
		public static readonly Uri PortalUrl = new Uri("http://www.managedfusion.com");

		/// <summary>The copyright date of this portal software.</summary>
		public static readonly string PortalCopyrightDate = "2002-" + DateTime.Now.Year;

		/// <summary>The formal version of this portal software.</summary>
		public static Version PortalVersion
		{
			get { return typeof(PortalProperties).Assembly.GetName().Version; }
		}

		/// <summary>The copyright of this portal software.</summary>
		public static string PortalCopyright
		{
			get
			{
				return String.Format(
				  "{0} ({1}) Copyright {2}, Nicholas Berardi, All rights reserved.",
				  PortalName,
				  PortalUrl.Host,
				  PortalCopyrightDate
				  );
			}
		}

		/// <summary>The formal name of the software including the name and version.</summary>
		public static string SoftwareName
		{
			get { return String.Concat(PortalName, " ", PortalVersion.ToString(2)); }
		}
	}
}
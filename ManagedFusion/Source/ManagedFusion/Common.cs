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

#region References

using System;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using System.Web.UI;
using System.Text;
using System.Threading;
using System.Resources;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Web.Security;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Data;
using ManagedFusion.Properties;
using ManagedFusion.Modules;
using ManagedFusion.Security;
using ManagedFusion.Configuration;
using ManagedFusion.Syndication;

#endregion

namespace ManagedFusion
{
	/// <summary>
	/// The Global class is a collection of global methods that get used through
	/// out the ManagedFusion software.
	/// </summary>
	public static class Common
	{
		#region Fields

		private static CacheManager _cacheManager;
		private static PortalDatabaseProvider _databaseProvider;
		private static CommunityPathProvider _pathProvider;
		private static CommunityConfigurationProvider _configurationProvider;
		private static MembershipProvider _membershipProvider;
		private static RoleProvider _roleProvider;

		#endregion

		#region Constants

		/// <summary>The delimiter is ';'.</summary>
		public const char Delimiter = ';';

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		static Common()
		{
			/* ensure assemblies are loaded
			 * this loads assemblies that may not be loaded because they have not direct reference
			 * but need to be loaded because they may contain modules and portlets
			 */
			EnsureAssembliesAreLoaded();

			_cacheManager = new CacheManager();

			// providers
			_databaseProvider = Databases.Provider as PortalDatabaseProvider;
			_pathProvider = CommunityPaths.Provider;
			_configurationProvider = CommunityConfigurations.Provider;
			_membershipProvider = global::System.Web.Security.Membership.Provider;
			_roleProvider = global::System.Web.Security.Roles.Provider;
		}

		#endregion

		#region Properties

		#region Public

		/// <summary>The caching system used to manage the cache in the application.</summary>
		public static CacheManager Cache { get { return _cacheManager; } }

		/// <summary>The context for the current web application instance.</summary>
		public static HttpContext Context { get { return HttpContext.Current; } }

		/// <summary>A group of path related methods for creating and extending paths for the application.</summary>
		public static CommunityPathProvider Path { get { return _pathProvider; } }

		/// <summary>The configurations for each community and the default.</summary>
		public static CommunityConfigurationProvider Configuration { get { return _configurationProvider; } }

		/// <summary>The current executing <see cref="IModule">Module</see> for this instance of the page.</summary>
		public static ModuleBase ExecutingModule
		{
			get { return Common.Context.Items["ExecutingModule"] as ModuleBase; }
			internal set { Common.Context.Items["ExecutingModule"] = value; }
		}

		/// <summary>The membership provider for the portal.</summary>
		public static MembershipProvider Membership { get { return _membershipProvider; } }

		/// <summary>The role provider for the portal.</summary>
		public static RoleProvider Role { get { return _roleProvider; } }

		/// <summary>A representation of the current page properties, for this instance of the page.</summary>
		public static PageBuilder PageBuilder { get { return (PageBuilder)Common.Context.Items["PageBuilder"]; } }

		/// <summary>The current <see cref="CultureInfo"/> for this instance of the page.</summary>
		public static CultureInfo SelectedCulture
		{
			get { return Thread.CurrentThread.CurrentUICulture; }
			set
			{
				Thread.CurrentThread.CurrentCulture = value;
				Thread.CurrentThread.CurrentUICulture = value;
			}
		}

		#endregion

		#region Internal

		/// <summary>The ManagedFusion database for setup and creation of the layout and display.</summary>
		internal static PortalDatabaseProvider DatabaseProvider { get { return _databaseProvider; } }

		#endregion

		#endregion

		#region Methods

		/*****************************************************************************
		 * String Manipulation
		 */
		/// <summary>Works like VB's Left function.</summary>
		/// <param name="s">The string.</param>
		/// <param name="length">The number of characters on the left of the string to get.</param>
		/// <returns>Returns a new formated string.</returns>
		public static string Left (string s, int length)
		{
			// checks to see if length is less than 0
			if (length < 0)
				throw new ArgumentException("Argument 'length' must be greater or equal to zero.", "length");
			if ((s == null) || (s.Length == 0))
				return String.Empty; // VB.net does this.
			if (length >= s.Length)
				return s;

			return s.Substring(0, length);
		}

		/// <summary>Works like VB's Right function.</summary>
		/// <param name="s">The string.</param>
		/// <param name="length">The number of characters on the right of the string to get.</param>
		/// <returns>Returns a new formated string.</returns>
		public static string Right (string s, int length)
		{
			// checks to see if length is less than 0
			if (length < 0)
				throw new ArgumentException("Argument 'length' must be greater or equal to zero.", "length");
			if (s == null)
				return String.Empty;

			return s.Substring(s.Length - length);
		}

		/*
		 *****************************************************************************/

		/// <summary>
		/// Creates a cache key from the <see cref="ManagedFusion.Common.PortalID">Common.PortalID</see>.
		/// </summary>
		/// <param name="key">The key used to help create the cache key.</param>
		/// <returns>Returns the key that has been formed from the input.</returns>
		public static string GetCacheKey (string key)
		{
			try { return GetCacheKey(key, CommunityInfo.Current.UniversalID.ToString("B")); } catch (NullReferenceException exc) {
				throw new ApplicationException("Portal identifier has not been placed into Common.Context yet.", exc);
			}
		}

		/// <summary>Sends the <see cref="HttpStatusCode"/> for <see cref="HttpResponse"/>.</summary>
		/// <remarks>
		/// <note>No other processing in ASP.Net will happen after this method is called, because this method calls <see cref="HttpResponse.End"/>.</note>
		/// This method will sends a <see cref="HttpStatusCode"/>
		/// according to <see href="http://www.w3.org/Protocols/rfc2616/rfc2616.html">RFC 2616</see>.
		/// </remarks>
		/// <param name="statusCode">The status code to send to the client.</param>
		/// <param name="endRequest">If the request should end after setting the status code.</param>
		/// <seealso href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html">HTTP Status Codes</seealso>
		public static void SendHttpStatusCode (HttpStatusCode statusCode)
		{
			// set all of the status variables
			Context.Response.StatusCode = (int)statusCode;
			Context.Response.StatusDescription = statusCode.ToString();
			Context.Response.End();
		}

		/// <summary>
		/// Creates a cache key from the <see cref="ManagedFusion.Common.PortalID">Common.PortalID</see>.
		/// </summary>
		/// <param name="key">The key used to help create the cache key.</param>
		/// <param name="id">The id used to help create the cache key.</param>
		/// <returns>Returns the key that has been formed from the input.</returns>
		public static string GetCacheKey (string key, string id)
		{
			if (String.IsNullOrEmpty(id))
				return key;

			return String.Format("{0}:{1}", key, id);
		}

		/// <summary>
		/// Get value of a specified <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="node">Node to retreive value from.</param>
		/// <param name="attribute">Attribute to retreive.</param>
		/// <param name="required">The passed node is required by the program.</param>
		/// <returns>Returns the value of the attribute in the node.</returns>
		public static string GetValue (XmlNode node, string attribute, bool required)
		{
			XmlNode n = node.Attributes.GetNamedItem(attribute);

			// checks to see if the node was found
			if (n == null) {
				if (required)
					throw new ApplicationException(
						"Attribute Required: " + attribute
						);
				else
					return String.Empty;
			}

			// if node was found returns its value
			return n.Value;
		}

		/// <summary>Ensure <see cref="Assembly">Assemblies</see> are loaded from the /bin directory.</summary>
		private static void EnsureAssembliesAreLoaded ()
		{
			AppDomain domain = AppDomain.CurrentDomain;
			DirectoryInfo bin = new DirectoryInfo(Context.Server.MapPath("bin"));

			// goes through each assembly and removes the .dll
			foreach (FileInfo file in bin.GetFiles("*.dll")) {
				Assembly.LoadFrom(file.FullName, domain.Evidence);
			}
		}

		#endregion
	}
}
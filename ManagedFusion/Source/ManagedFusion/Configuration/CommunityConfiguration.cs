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
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace ManagedFusion.Configuration
{
	/// <summary>The config properties from <c>Web.Config</c> of the portal.</summary>
	/// <remarks>
	/// This class has the static properties for the portal that will not change.  Context properties can be
	/// found in the class <see cref="ManagedFusion.PortalContext"/>.  Static properties can be found in the 
	/// class <see cref="ManagedFusion.PortalProperties"/>.
	/// </remarks>
	public class CommunityConfiguration
	{
		private readonly string _location;
		private readonly int _communityID;

		#region Properties

		public string Location 
		{
			get { return this._location; }
		}

		public int AssociatedCommunityID 
		{
			get { return this._communityID; }
		}

		#region <settings>

		#region Private

		private Dictionary<string, string> _settings;

		private CultureInfo _defaultLanguage;
		private string _defaultTheme;
		private string _defaultStyle;
		private string _defaultSkinTemplate;
		private string _defaultPageHandler;
		private string _databaseConnectionString;
		private Type _defaultPageHandlerType;

		private ExpirationType _expirationType = ExpirationType.NotSet;
		private double _cacheTime = -1;
		private double _databaseCacheTime = -1;

		#endregion

		/// <summary>Gets a value from the settings.</summary>
		public string this [string key] 
		{
			get { return this._settings[key]; }
		}

		/// <summary>Gets the database connection string for the community.</summary>
		public string DatabaseConnectionString 
		{
			get 
			{
				// return value
				return _databaseConnectionString;
			}
		}

		/// <summary>Gets the default language for the community.</summary>
		public CultureInfo DefaultLanguage 
		{
			get 
			{
				// check to see if the value is set
				if (this._defaultLanguage == null)
					return PortalSettings.Default.DefaultLanguage;
				
				// return value
				return _defaultLanguage;
			}
		}

		/// <summary></summary>
		public string DefaultTheme 
		{
			get 
			{
				// check to see if the value is set
				if (this._defaultTheme == null)
					return PortalSettings.Default.DefaultTheme;
				
				// return value
				return _defaultTheme;
			}
		}

		/// <summary></summary>
		public string DefaultStyle 
		{
			get 
			{
				// check to see if the value is set
				if (this._defaultStyle == null)
					return PortalSettings.Default.DefaultStyle;
				
				// return value
				return _defaultStyle;
			}
		}

		/// <summary>The default skin control that is used when loading the page.</summary>
		public string DefaultSkinTemplate
		{
			get 
			{
				// check to see if the value is set
				if (this._defaultSkinTemplate == null)
					return PortalSettings.Default.DefaultSkinTemplate;
				
				// return value
				return _defaultSkinTemplate;
			}
		}

		/// <summary>The default handler to use to create the output.</summary>
		public IHttpHandler DefaultPageHandler
		{
			get 
			{
				// check to see if the type has been stored yet
				if (this._defaultPageHandlerType == null) 
				{
					string pageHandlerString;

					// check to see if the value is set
					if (this._defaultSkinTemplate == null)
						pageHandlerString = PortalSettings.Default.DefaultPageHandler;
					else
						pageHandlerString = this._defaultPageHandler;

					// check to see if the set type inherits from IHttpHandler
					try 
					{
						Type type = System.Type.GetType(pageHandlerString, false, true);

						if (type.IsSubclassOf(typeof(IHttpHandler)))
							throw new InvalidCastException("DefaultHandler must use the interface IHttpHandler.");
				
						// set the interface for the handler
						this._defaultPageHandlerType = type;
					} 
					catch (NullReferenceException exc) 
					{
						throw new TypeInitializationException(_defaultPageHandler, exc);
					}
				}

				return (IHttpHandler)Activator.CreateInstance(this._defaultPageHandlerType);
			}
		}

		/// <summary></summary>
		public ExpirationType ExpirationType 
		{
			get 
			{
				// check to see if the value is set
				if (this._expirationType == ExpirationType.NotSet)
					return PortalSettings.Default.ExpirationType;
				
				// return value
				return _expirationType;
			}
		}

		/// <summary></summary>
		public double CacheTime 
		{
			get 
			{
				// check to see if the value is set
				if (this._cacheTime == -1)
					return PortalSettings.Default.CacheTime;
				
				// return value
				return _cacheTime;
			}
		}

		/// <summary></summary>
		public double DatabaseCacheTime 
		{
			get 
			{
				// check to see if the value is set
				if (this._databaseCacheTime == -1)
					return PortalSettings.Default.DatabaseCacheTime;
				
				// return value
				return _databaseCacheTime;
			}
		}

		#endregion

		#endregion

		public CommunityConfiguration (XmlDocument config, string location, int communityID)
		{
			// set the location of this config file
			this._location = location;

			// set the community id that this config is associated with
			this._communityID = communityID;

			// collection of children in settings
			_settings = new Dictionary<string, string>();
			
			// get <settings>
			if (config != null)
				this.SetSettings(config.DocumentElement.SelectSingleNode("appSettings"));
		}

		#region <settings>

		private void SetSettings (XmlNode settings) 
		{
			foreach (XmlNode config in settings.SelectNodes("add"))
				_settings.Add(config.Attributes["key"].Value, config.Attributes["value"].Value);

			if (_settings.ContainsKey("DefaultLanguage"))
				try { _defaultLanguage = CultureInfo.CreateSpecificCulture(_settings["DefaultLanguage"]); }
				catch (ArgumentException) { }

			if (_settings.ContainsKey("DefaultTheme"))
				_defaultTheme = _settings["DefaultTheme"];

			if (_settings.ContainsKey("DefaultStyle"))
				_defaultStyle = _settings["DefaultStyle"];

			if (_settings.ContainsKey("DefaultSkinTemplate"))
				_defaultSkinTemplate = _settings["DefaultSkinTemplate"];

			if (_settings.ContainsKey("DefaultPageHandler"))
				_defaultPageHandler = _settings["DefaultPageHandler"];

			if (_settings.ContainsKey("ExpirationType"))
				try { _expirationType = (ExpirationType)Enum.Parse(typeof(ExpirationType), _settings["ExpirationType"], true); }
				catch (ArgumentException) { }

			if (_settings.ContainsKey("CacheTime"))
				_cacheTime = XmlConvert.ToDouble(_settings["CacheTime"]);

			if (_settings.ContainsKey("DatabaseConnectionString")) 
				_databaseConnectionString = _settings["DatabaseConnectionString"];

			if (_settings.ContainsKey("DatabaseCacheTime"))
				_databaseCacheTime = XmlConvert.ToDouble(_settings["DatabaseCacheTime"]);
		}

		#endregion
	}
}
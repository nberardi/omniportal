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
using System.ComponentModel;
using System.IO;
using System.Web.Caching;
using System.Xml.Serialization;
using System.Security.Permissions;

// ManagedFusion Classes
using ManagedFusion.Modules;
using ManagedFusion.Modules.Configuration;

namespace ManagedFusion
{
	public sealed class SectionModule : ModuleInfo
	{
		#region Static

		public static ModuleCollection Collection
		{
			get { return Common.DatabaseProvider.SectionModules; }
		}

		/// <summary>Gets the Error Module with GUID of {AACA68FA-4FA2-4a4a-A70C-BF400AFF278F}.</summary>
		public static readonly SectionModule ErrorModule = (SectionModule)Collection[new Guid(0xAACA68FA, 0x4FA2, 0x4a4a, 0xA7, 0x0C, 0xBF, 0x40, 0x0A, 0xFF, 0x27, 0x8F)];

		#endregion

		#region Fields

		private bool _traversable;
		private bool _folderBased;
		private bool _configInFolder;
		private String _folderName;
		private string _location;

		#endregion

		#region Constructor

		[EditorBrowsable(EditorBrowsableState.Never)]
		public SectionModule (ModuleAttribute attribute, Type type)
			: base(attribute.ModuleID, attribute.Title, attribute.Description, type)
		{
			this._traversable = attribute.TraversablePath;
			this._folderBased = attribute.FolderBased;
			this._folderName = attribute.FolderName;
			this._configInFolder = attribute.ConfigInFolder;
		}

		#endregion

		#region Properties

		/// <summary>Gets if the module's URL Path is traversable or not.</summary>
		public bool IsUrlPathTraversable { get { return this._traversable; } }

		/// <summary></summary>
		private string ConfigLocation
		{
			get
			{
				if (String.IsNullOrEmpty(this._location) == false)
					return this._location;

				// get the correct path to the config file
				if (this._configInFolder)
					this._location = Common.Path.GetModulePath(_folderName, PortalProperties.ModuleConfigFile);
				else
					this._location = Type.Namespace + "." + PortalProperties.ModuleConfigFile;

				return this._location;
			}
		}

		/// <summary></summary>
		public ModuleConfigurationDocument Config
		{
			get
			{
				string configLocation = this.ConfigLocation;
				string key = configLocation + " - ConfigFile";

				if (Common.Cache.IsCached(key, String.Empty) == false) {
					ModuleConfigurationDocument config = null;
					Stream stream = Stream.Null;
					StreamReader reader = StreamReader.Null;

					try {
						// check on where the application should look for a config file
						if (this._configInFolder) {
							configLocation = Common.Path.GetAbsoluteDiskPath(configLocation);
							reader = new StreamReader(configLocation);
						} else {
							stream = this.Type.Assembly.GetManifestResourceStream(configLocation);
							reader = new StreamReader(stream);
						}

						XmlSerializer ser = new XmlSerializer(typeof(ModuleConfigurationDocument));
						config = ser.Deserialize(reader) as ModuleConfigurationDocument;

						// add to the cache
						if (this._configInFolder) {
							Common.Cache.Add(key, String.Empty, config, Cache.NoSlidingExpiration, new CacheDependency(configLocation), CacheItemPriority.NotRemovable, null);
						} else {
							Common.Cache.Add(key, String.Empty, config, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
						}
					} finally {
						if (stream != Stream.Null && stream != null)
							stream.Close();

						if (reader != StreamReader.Null && reader != null)
							reader.Close();
					}
				}

				// gets the cached config collection
				return Common.Cache[key, String.Empty] as ModuleConfigurationDocument;
			}
		}
		#endregion
	}
}
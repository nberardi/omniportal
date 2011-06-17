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
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Collections;
using System.Web.Caching;
using System.Xml;

using ManagedFusion;
using ManagedFusion.Configuration;

namespace ManagedFusion.Configuration.Folder
{
	internal class FolderConfigurationProvider : CommunityConfigurationProvider
	{
		private CommunityConfigurationCollection _collection;
		public override CommunityConfigurationCollection CommunityConfigurations
		{
			get 
			{
				if (_collection != null) return _collection;

				// portal directory reference
				DirectoryInfo portalDirectory = new DirectoryInfo(Common.Context.Server.MapPath("Communities"));

				// create config collection
				CommunityConfigurationCollection collection = new CommunityConfigurationCollection();

				// portal directories
				DirectoryInfo[] Communities = portalDirectory.GetDirectories();

				// add each portal to dictionary
				FileInfo[] hosts = null;
				FileInfo host = null;
				int index = -1;

				try 
				{
					// process the rest of the communties
					foreach (DirectoryInfo community in Communities)
					{
						hosts = community.GetFiles("Community.config");

						// goto next directory if there is no config file
						if (hosts.Length < 1)
							continue;

						host = hosts[0];

						// get the index of the community id
						// if it is the default community just continue processing
						// because the default community has already been processed
						if (host.Directory.Name == "Default")
							continue;
						else
						{
							// try to convert the directory name to an index
							// if that doesn't work skip and keep processing
							try
							{
								index = Convert.ToInt32(host.Directory.Name);
							}
							catch (ApplicationException) { continue; }
						}

						XmlDocument config = new XmlDocument();
						config.Load(host.FullName);

						// add the configuration to the collection
						collection.Add(
							index,
							new CommunityConfiguration(config, host.FullName, index),
							host.FullName,
							new CacheDependency(host.FullName),
							new CacheItemRemovedCallback(ConfigurationCacheItemRemoved)
							);
					}
				} 
				catch (ApplicationException exc) 
				{
					throw new ApplicationException(
						"An error has occured in the portal configuration setup.",
						new ProviderException(exc.Message, exc)
					);
				}

				// check to see if there were any portal configurations created
				if (collection.Count == 0)
					throw new ApplicationException(
						"There are no portal configurations available in the Communities folder."
						);

				this._collection = collection;
				return _collection;
			}
		}

		private void ConfigurationCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
		{
			FileInfo host = new FileInfo(key);
			CommunityConfiguration communityConfig;

			// if key didn't relate to a file, then do nothing
			if (host == null)
				return;

			communityConfig = value as CommunityConfiguration;

			// if config isn't valid
			if (communityConfig == null)
				return;

			XmlDocument config = new XmlDocument();
			config.Load(host.FullName);

			// add the config to cache
			Common.Cache.Add(
				key,
				String.Empty,
				new CommunityConfiguration(config, communityConfig.Location, communityConfig.AssociatedCommunityID),
				TimeSpan.FromDays(1),
				new CacheDependency(host.FullName),
				CacheItemPriority.NotRemovable,
				new CacheItemRemovedCallback(ConfigurationCacheItemRemoved)
				);
		}
	}
}
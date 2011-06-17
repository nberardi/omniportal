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
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;

// ManagedFusion Classes
using ManagedFusion;

namespace ManagedFusion.Configuration
{
	public class CommunityConfigurationCollection : ICollection, IEnumerable<CommunityConfiguration>
	{
		private Dictionary<int, string> _innerDictionary;
		private static readonly CommunityConfiguration _defaultConfiguration = new CommunityConfiguration(null, null, -1);

		public CommunityConfigurationCollection() 
		{
			this._innerDictionary = new Dictionary<int, string>();
		}

		protected internal void Add(int id, CommunityConfiguration config, string cacheKey, CacheDependency dependency, CacheItemRemovedCallback callback)
		{
			if (this.InnerDictionary.ContainsKey(id))
				this.InnerDictionary.Remove(id);

			// add id/cacheKey mapping to the dictionary
			this.InnerDictionary.Add(id, cacheKey);

			// add the config to cache
			Common.Cache.Add(cacheKey, String.Empty, config, Cache.NoSlidingExpiration, dependency, CacheItemPriority.NotRemovable, callback);
		}

		public CommunityConfiguration DefaultConfiguration
		{
			get { return _defaultConfiguration; }
		}

		/// <summary>
		/// This contains a combination of the community id and the related location in the cache.
		/// </summary>
		protected Dictionary<int, string> InnerDictionary 
		{
			get { return _innerDictionary; }
		}

		public CommunityConfiguration this[int id] 
		{
			get 
			{
				// get the portal config key
				string key = null;

				// tries to get the value from the list, if it is not present
				// then the default configuration is used
				if (this.InnerDictionary.TryGetValue(id, out key) == false)
					return _defaultConfiguration;

				// get the portal config
				CommunityConfiguration config = Common.Cache[key, String.Empty] as CommunityConfiguration;

				// if config is null then config is default
				if (config == null)
					return _defaultConfiguration;

				return config;
			}
		}

		public bool Contains (int id) 
		{
			return this.InnerDictionary.ContainsKey(id);
		}

		#region IEnumerable<CommunityConfiguration> Members

		public IEnumerator<CommunityConfiguration> GetEnumerator()
		{
			foreach (string cacheKey in this.InnerDictionary.Values)
				yield return Common.Cache[cacheKey, String.Empty] as CommunityConfiguration;
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this.InnerDictionary).CopyTo(array, index);
		}

		public int Count
		{
			get { return this.InnerDictionary.Count; }
		}

		bool ICollection.IsSynchronized
		{
			get { return ((ICollection)this.InnerDictionary).IsSynchronized; }
		}

		object ICollection.SyncRoot
		{
			get { return ((ICollection)this.InnerDictionary).SyncRoot; }
		}

		#endregion
	}
}
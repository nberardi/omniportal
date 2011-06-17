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
using System.Web;
using System.Web.Caching;

namespace ManagedFusion
{
	/// <summary>
	/// This class is used to manipulate the <see cref="System.Web.HttpContext.Cache"/> in a
	/// constant way.
	/// </summary>
	/// <remarks>
	/// The purpose of this class is to apply a set of global policies to the caching of data with in
	/// the portal.
	/// </remarks>
	/// <example>
	/// The recommend way for implimenting the CacheManager is as follows:
	/// <code lang="C#" escaped="true">
	/// public DataTable GetData (int index)
	/// {
	/// 	// set cache key
	/// 	string key = String.Concat("GetData", index.ToString());
	/// 
	/// 	// check to see if the cache key has been previously cached
	/// 	if (Common.Cache.IsCached(key) == false) 
	/// 	{
	/// 		DataSet ds;
	/// 
	/// 		// work with DataSet here
	/// 
	/// 		// cache data table
	/// 		Common.Cache[key] = ds.Tables[0];
	/// 	}
	/// 
	/// 	return (DataTable)Common.Cache[key];
	/// }
	/// </code>
	/// <code lang="Visual Basic" escaped="true">
	/// Public Function GetData(ByVal index As Integer) As DataTable 
	///		' set cache key
	///		Dim key As String = String.Concat("GetData", index.ToString) 
	///		
	///		' check to see if the cache key has been previously cached
	///		If Common.Cache.IsCached(key) = False Then 
	///			Dim ds As DataSet 
	///			
	///			' work with DataSet here
	///			
	///			' cache data table
	///			Common.Cache(key) = ds.Tables(0) 
	///		End If 
	///		
	///		Return CType(Common.Cache(key), DataTable) 
	///	End Function
	/// </code>
	/// </example>
	public sealed class CacheManager
	{
		// only lets the internal assembly create the CacheManager
		internal CacheManager () { }

		/// <summary></summary>
		/// <param name="cacheKey"></param>
		/// <returns></returns>
		private bool _IsCached (string cacheKey)
		{
			// checks to see if results are being cached
			// if results aren't being cached false is always returned
			return _Get(cacheKey) != null;
		}

		/// <summary></summary>
		/// <param name="cacheKey"></param>
		/// <param name="value"></param>
		private void _Add (string cacheKey, object value)
		{
			// uses the specified expiration type
			switch (Common.Configuration.Current.ExpirationType) {
				// expires in "CacheTime" unless it gets used then expire time reset back to "CacheTime"
				case ExpirationType.Sliding:
					_Add(cacheKey, value,
						Cache.NoAbsoluteExpiration,
						TimeSpan.FromMinutes(Common.Configuration.Current.CacheTime)
						);
					break;
				// expires in exactly "CacheTime" from now
				case ExpirationType.Absolute:
					_Add(cacheKey, value,
						DateTime.Now.AddMinutes(Common.Configuration.Current.CacheTime),
						Cache.NoSlidingExpiration
						);
					break;
				// use default ASP.Net settings for cache expiration
				default:
					HttpContext.Current.Cache.Insert(cacheKey, value);
					break;
			}
		}

		/// <summary></summary>
		/// <param name="cacheKey"></param>
		/// <param name="value"></param>
		/// <param name="expire"></param>
		/// <param name="slide"></param>
		private void _Add (string cacheKey, object value, DateTime expire, TimeSpan slide)
		{
			HttpContext.Current.Cache.Insert(cacheKey, value, null, expire, slide);
		}

		/// <summary></summary>
		/// <param name="cacheKey"></param>
		/// <param name="value"></param>
		/// <param name="expire"></param>
		/// <param name="slide"></param>
		/// <param name="priority"></param>
		/// <param name="onRemoveCallback"></param>
		private void _Add (string cacheKey, object value, DateTime expire, TimeSpan slide, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			HttpContext.Current.Cache.Insert(cacheKey, value, null, expire, slide, priority, onRemoveCallback);
		}

		/// <summary></summary>
		/// <param name="cacheKey"></param>
		/// <param name="value"></param>
		/// <param name="dependencies"></param>
		/// <param name="expire"></param>
		/// <param name="slide"></param>
		/// <param name="priority"></param>
		/// <param name="onRemoveCallback"></param>
		private void _Add (string cacheKey, object value, CacheDependency dependencies, DateTime expire, TimeSpan slide, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			HttpContext.Current.Cache.Insert(cacheKey, value, dependencies, expire, slide, priority, onRemoveCallback);
		}

		/// <summary></summary>
		/// <param name="cacheKey"></param>
		/// <returns></returns>
		private object _Get (string cacheKey)
		{
			return HttpContext.Current.Cache.Get(cacheKey);
		}

		/// <summary></summary>
		/// <param name="cacheKey"></param>
		/// <returns></returns>
		private object _Remove (string cacheKey)
		{
			return HttpContext.Current.Cache.Remove(cacheKey);
		}

		#region Public Methods

		#region IsCached

		/// <overloads>
		/// <summary>Checks to see if the item is cached.</summary>
		/// <example>
		/// The recommend way for implimenting the CacheManager is as follows:
		/// <code lang="C#" escaped="true">
		/// public DataTable GetData (int index)
		/// {
		/// 	// set cache key
		/// 	string key = String.Concat("GetData", index.ToString());
		/// 
		/// 	// check to see if the cache key has been previously cached
		/// 	if (Common.Cache.IsCached(key) == false) 
		/// 	{
		/// 		DataSet ds;
		/// 
		/// 		// work with DataSet here
		/// 
		/// 		// cache data table
		/// 		Common.Cache[key] = ds.Tables[0];
		/// 	}
		/// 
		/// 	return (DataTable)Common.Cache[key];
		/// }
		/// </code>
		/// <code lang="Visual Basic" escaped="true">
		/// Public Function GetData(ByVal index As Integer) As DataTable 
		///		' set cache key
		///		Dim key As String = String.Concat("GetData", index.ToString) 
		///		
		///		' check to see if the cache key has been previously cached
		///		If Common.Cache.IsCached(key) = False Then 
		///			Dim ds As DataSet 
		///			
		///			' work with DataSet here
		///			
		///			' cache data table
		///			Common.Cache(key) = ds.Tables(0) 
		///		End If 
		///		
		///		Return CType(Common.Cache(key), DataTable) 
		///	End Function
		/// </code>
		/// </example>
		/// </overloads>
		/// <summary>
		/// Checks to see if the item is cached.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <returns>Returns a boolean value indicating if the key is cached for not.</returns>
		public bool IsCached (string key)
		{
			return _IsCached(Common.GetCacheKey(key));
		}

		/// <summary>
		/// Checks to see if the item is cached with a unique identifier.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <returns>Returns a boolean value indicating if the key is cached for not.</returns>
		public bool IsCached (string key, string uniqueIdentifier)
		{
			return _IsCached(Common.GetCacheKey(key, uniqueIdentifier));
		}

		#endregion

		#region Add

		/// <overloads>
		/// <summary>Inserts an item into the <see cref="System.Web.Caching.Cache"/> object. Use one of the versions of this method to overwrite an existing <b>Cache</b> item with the <i>same key</i> parameter.</summary>
		/// </overloads>
		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="value">The object to be inserted in the cache.</param>
		public void Add (string key, object value)
		{
			_Add(Common.GetCacheKey(key), value);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with an absolute expiration time.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="value">The object to be inserted in the cache.</param>
		/// <param name="expire">The time at which the inserted object expires and is removed from the cache.</param>
		public void Add (string key, object value, DateTime expire)
		{
			_Add(Common.GetCacheKey(key), value, expire, Cache.NoSlidingExpiration);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with an absolute expiration time.  
		/// That sets the <see cref="System.Web.Caching.CacheItemPriority"/> and has a callback to <see cref="System.Web.Caching.CacheItemRemovedCallback"/>.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="value">The object to be inserted in the cache.</param>
		/// <param name="expire">The time at which the inserted object expires and is removed from the cache.</param>
		/// <param name="priority">The cost of the object relative to other items stored in the cache, as expressed by the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration. This value is used by the cache when it evicts objects; objects with a lower cost are removed from the cache before objects with a higher cost.</param>
		/// <param name="onRemoveCallback">A delegate that, if provided, will be called when an object is removed from the cache. You can use this to notify applications when their objects are deleted from the cache. </param>
		public void Add (string key, object value, DateTime expire, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			_Add(Common.GetCacheKey(key), value, expire, Cache.NoSlidingExpiration, priority, onRemoveCallback);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a sliding expiration time.  
		/// That sets the <see cref="System.Web.Caching.CacheItemPriority"/> and has a callback to <see cref="System.Web.Caching.CacheItemRemovedCallback"/>.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="value">The object to be inserted in the cache.</param>
		/// <param name="expire">The time at which the inserted object expires and is removed from the cache.</param>
		/// <param name="dependencies">The file or cache key dependencies for the item. When any dependency changes, the object becomes invalid and is removed from the cache. If there are no dependencies, this parameter contains a <see langword="null"/> reference.</param>
		/// <param name="priority">The cost of the object relative to other items stored in the cache, as expressed by the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration. This value is used by the cache when it evicts objects; objects with a lower cost are removed from the cache before objects with a higher cost.</param>
		/// <param name="onRemoveCallback">A delegate that, if provided, will be called when an object is removed from the cache. You can use this to notify applications when their objects are deleted from the cache. </param>
		public void Add (string key, object value, DateTime expire, CacheDependency dependencies, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			_Add(Common.GetCacheKey(key), value, dependencies, expire, Cache.NoSlidingExpiration, priority, onRemoveCallback);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a sliding expiration time.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="value">The object to be inserted in the cache.</param>
		/// <param name="slide">The interval between the time the inserted object was last accessed and when that object expires. If this value is the equivalent of 20 minutes, the object will expire and be removed from the cache 20 minutes after it was last accessed.</param>
		public void Add (string key, object value, TimeSpan slide)
		{
			_Add(Common.GetCacheKey(key), value, Cache.NoAbsoluteExpiration, slide);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a sliding expiration time.  
		/// That sets the <see cref="System.Web.Caching.CacheItemPriority"/> and has a callback to <see cref="System.Web.Caching.CacheItemRemovedCallback"/>.
		/// </summary>
		/// <param name="key">The cache key.</param>
		/// <param name="value">The object that is to be cached.</param>
		/// <param name="slide">The time span that the object can remain inactive and still be in memory.</param>
		/// <param name="priority">The cost of the object relative to other items stored in the cache, as expressed by the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration. This value is used by the cache when it evicts objects; objects with a lower cost are removed from the cache before objects with a higher cost.</param>
		/// <param name="onRemoveCallback">A delegate that, if provided, will be called when an object is removed from the cache. You can use this to notify applications when their objects are deleted from the cache. </param>
		public void Add (string key, object value, TimeSpan slide, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			_Add(Common.GetCacheKey(key), value, Cache.NoAbsoluteExpiration, slide, priority, onRemoveCallback);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a sliding expiration time.  
		/// That sets the <see cref="System.Web.Caching.CacheItemPriority"/> and has a callback to <see cref="System.Web.Caching.CacheItemRemovedCallback"/>.
		/// </summary>
		/// <param name="key">The cache key.</param>
		/// <param name="value">The object that is to be cached.</param>
		/// <param name="slide">The time span that the object can remain inactive and still be in memory.</param>
		/// <param name="dependencies">The file or cache key dependencies for the item. When any dependency changes, the object becomes invalid and is removed from the cache. If there are no dependencies, this parameter contains a <see langword="null"/> reference.</param>
		/// <param name="priority">The cost of the object relative to other items stored in the cache, as expressed by the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration. This value is used by the cache when it evicts objects; objects with a lower cost are removed from the cache before objects with a higher cost.</param>
		/// <param name="onRemoveCallback">A delegate that, if provided, will be called when an object is removed from the cache. You can use this to notify applications when their objects are deleted from the cache. </param>
		public void Add (string key, object value, TimeSpan slide, CacheDependency dependencies, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			_Add(Common.GetCacheKey(key), value, dependencies, Cache.NoAbsoluteExpiration, slide, priority, onRemoveCallback);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a unique identifier.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <param name="value">The object to be inserted in the cache.</param>
		public void Add (string key, string uniqueIdentifier, object value)
		{
			_Add(Common.GetCacheKey(key, uniqueIdentifier), value);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a unique identifier and an absolute expiration time.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <param name="value">The object to be inserted in the cache.</param>
		/// <param name="expire">The time at which the inserted object expires and is removed from the cache.</param>
		public void Add (string key, string uniqueIdentifier, object value, DateTime expire)
		{
			_Add(Common.GetCacheKey(key, uniqueIdentifier), value, expire, Cache.NoSlidingExpiration);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a unique identifier and an absolute expiration time.  
		/// That sets the <see cref="System.Web.Caching.CacheItemPriority"/> and has a callback to <see cref="System.Web.Caching.CacheItemRemovedCallback"/>.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <param name="value">The object to be inserted in the cache.</param>
		/// <param name="expire">The time at which the inserted object expires and is removed from the cache.</param>
		/// <param name="priority">The cost of the object relative to other items stored in the cache, as expressed by the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration. This value is used by the cache when it evicts objects; objects with a lower cost are removed from the cache before objects with a higher cost.</param>
		/// <param name="onRemoveCallback">A delegate that, if provided, will be called when an object is removed from the cache. You can use this to notify applications when their objects are deleted from the cache. </param>
		public void Add (string key, string uniqueIdentifier, object value, DateTime expire, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			_Add(Common.GetCacheKey(key, uniqueIdentifier), value, expire, Cache.NoSlidingExpiration, priority, onRemoveCallback);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a sliding expiration time.  
		/// That sets the <see cref="System.Web.Caching.CacheItemPriority"/> and has a callback to <see cref="System.Web.Caching.CacheItemRemovedCallback"/>.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <param name="value">The object to be inserted in the cache.</param>
		/// <param name="expire">The time at which the inserted object expires and is removed from the cache.</param>
		/// <param name="dependencies">The file or cache key dependencies for the item. When any dependency changes, the object becomes invalid and is removed from the cache. If there are no dependencies, this parameter contains a <see langword="null"/> reference.</param>
		/// <param name="priority">The cost of the object relative to other items stored in the cache, as expressed by the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration. This value is used by the cache when it evicts objects; objects with a lower cost are removed from the cache before objects with a higher cost.</param>
		/// <param name="onRemoveCallback">A delegate that, if provided, will be called when an object is removed from the cache. You can use this to notify applications when their objects are deleted from the cache. </param>
		public void Add (string key, string uniqueIdentifier, object value, DateTime expire, CacheDependency dependencies, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			_Add(Common.GetCacheKey(key, uniqueIdentifier), value, dependencies, expire, Cache.NoSlidingExpiration, priority, onRemoveCallback);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a unique identifier and a sliding expiration time.
		/// </summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <param name="value">The object to be inserted in the cache.</param>
		/// <param name="slide">The interval between the time the inserted object was last accessed and when that object expires. If this value is the equivalent of 20 minutes, the object will expire and be removed from the cache 20 minutes after it was last accessed.</param>
		public void Add (string key, string uniqueIdentifier, object value, TimeSpan slide)
		{
			_Add(Common.GetCacheKey(key, uniqueIdentifier), value, Cache.NoAbsoluteExpiration, slide);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a unique identifier and a sliding expiration time.  
		/// That sets the <see cref="System.Web.Caching.CacheItemPriority"/> and has a callback to <see cref="System.Web.Caching.CacheItemRemovedCallback"/>.
		/// </summary>
		/// <param name="key">The cache key.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <param name="value">The object that is to be cached.</param>
		/// <param name="slide">The time span that the object can remain inactive and still be in memory.</param>
		/// <param name="priority">The cost of the object relative to other items stored in the cache, as expressed by the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration. This value is used by the cache when it evicts objects; objects with a lower cost are removed from the cache before objects with a higher cost.</param>
		/// <param name="onRemoveCallback">A delegate that, if provided, will be called when an object is removed from the cache. You can use this to notify applications when their objects are deleted from the cache. </param>
		public void Add (string key, string uniqueIdentifier, object value, TimeSpan slide, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			_Add(Common.GetCacheKey(key, uniqueIdentifier), value, Cache.NoAbsoluteExpiration, slide, priority, onRemoveCallback);
		}

		/// <summary>
		/// Inserts an item into the <see cref="System.Web.Caching.Cache"/> object with a sliding expiration time.  
		/// That sets the <see cref="System.Web.Caching.CacheItemPriority"/> and has a callback to <see cref="System.Web.Caching.CacheItemRemovedCallback"/>.
		/// </summary>
		/// <param name="key">The cache key.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <param name="value">The object that is to be cached.</param>
		/// <param name="slide">The time span that the object can remain inactive and still be in memory.</param>
		/// <param name="dependencies">The file or cache key dependencies for the item. When any dependency changes, the object becomes invalid and is removed from the cache. If there are no dependencies, this parameter contains a <see langword="null"/> reference.</param>
		/// <param name="priority">The cost of the object relative to other items stored in the cache, as expressed by the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration. This value is used by the cache when it evicts objects; objects with a lower cost are removed from the cache before objects with a higher cost.</param>
		/// <param name="onRemoveCallback">A delegate that, if provided, will be called when an object is removed from the cache. You can use this to notify applications when their objects are deleted from the cache. </param>
		public void Add (string key, string uniqueIdentifier, object value, TimeSpan slide, CacheDependency dependencies, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			_Add(Common.GetCacheKey(key, uniqueIdentifier), value, dependencies, Cache.NoAbsoluteExpiration, slide, priority, onRemoveCallback);
		}


		#endregion

		#region Get

		/// <overloads>
		/// <summary>Retreives an item from the cache.</summary>
		/// </overloads>
		/// <summary>Retreives an item from the cache.</summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <returns>The retrieved cache item, or a null reference if the key is not found.</returns>
		public object Get (string key)
		{
			return _Get(Common.GetCacheKey(key));
		}

		/// <summary>Retreives an item from the cache with a unique identifier.</summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <returns>The retrieved cache item, or a null reference if the key is not found.</returns>
		public object Get (string key, string uniqueIdentifier)
		{
			return _Get(Common.GetCacheKey(key, uniqueIdentifier));
		}

		#endregion

		#region Remove

		/// <overloads>
		/// <summary>Removes the specified item from the application's Cache object.</summary>
		/// </overloads>
		/// <summary>Removes the specified item from the application's Cache object.</summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <returns>The item removed from the Cache. If the value in the key parameter is not found, returns a null reference.</returns>
		public object Remove (string key)
		{
			return _Remove(Common.GetCacheKey(key));
		}

		/// <summary>Removes the specified item from the application's Cache object with a unique identifier.</summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <returns>The item removed from the Cache. If the value in the key parameter is not found, returns a null reference.</returns>
		public object Remove (string key, string uniqueIdentifier)
		{
			return _Remove(Common.GetCacheKey(key, uniqueIdentifier));
		}

		#endregion

		#region Indexer

		/// <overloads>
		/// <summary>Gets or sets the cache item at the specified key.</summary>
		/// </overloads>
		/// <summary>Gets or sets the cache item at the specified key.</summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <value>The object in the cache.</value>
		public object this[string key]
		{
			get { return this.Get(key); }
			set { this.Add(key, value); }
		}

		/// <summary>Gets or sets the cache item at the specified key with a unique identifier.</summary>
		/// <param name="key">The cache key used to reference the object.</param>
		/// <param name="uniqueIdentifier">The identifier used to make the cache key unique, so that it is not localized to a portal instance.</param>
		/// <value>The object in the cache.</value>
		public object this[string key, string uniqueIdentifier]
		{
			get { return this.Get(key, uniqueIdentifier); }
			set { this.Add(key, uniqueIdentifier, value); }
		}

		#endregion

		#endregion
	}
}
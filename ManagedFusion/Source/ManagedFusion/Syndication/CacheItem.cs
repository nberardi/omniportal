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

namespace ManagedFusion.Syndication
{
	/// <summary>
	/// Summary description for SyndicationCacheItem.
	/// </summary>
	public class CacheItem
	{
		public CacheItem() { }

		private DateTime _LastModified;
		public DateTime LastModified 
		{
			get { return this._LastModified; }
			set { this._LastModified = value; }
		}

		private string _Xml;
		public string Xml 
		{ 
			get { return this._Xml; }
			set { this._Xml = value; }
		}

		private string _Etag;
		public string Etag
		{
			get
			{
				// if Etag hasn't been sent then set the
				// Etag to the string of LastModified
				if (this._Etag == null)
					this._Etag = this.LastModified.ToString();

				return this._Etag;
			}
			set { this._Etag = value; }
		}
	}
}
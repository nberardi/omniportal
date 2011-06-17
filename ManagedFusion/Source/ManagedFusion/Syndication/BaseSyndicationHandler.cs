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
using System.Net;
using System.Web;
using System.Xml;
using System.Text;

// ManagedFusion Classes
using ManagedFusion;

namespace ManagedFusion.Syndication
{
	public abstract class BaseSyndicationHandler : IHttpHandler
	{
		public BaseSyndicationHandler() { }

		#region Properties

		private HttpContext _context;
		/// <summary>The web servers Context used in this application.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected HttpContext Context { get { return this._context; } }

		/// <summary>A reference for <see cref="SiteInfo">SiteInfo</see>.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected SiteInfo SiteInformation { get { return SiteInfo.Current; } }

		/// <summary>A reference for <see cref="SectionInfo">SectionInfo</see>.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected SectionInfo SectionInformation { get { return SectionInfo.Current; } }

		/// <summary>A reference for <see cref="CommunityInfo">CommunityInfo</see>.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected CommunityInfo CommunityInformation { get { return CommunityInfo.Current; } }

		private CacheItem _Feed;
		protected CacheItem Feed
		{
			get { return this._Feed; }
			set { this._Feed = value; }
		}

		protected DateTime LastVisited
		{
			get
			{
				// gets the last time the browser visited the site
				string lastModifiedString = this.Context.Request.Headers["If-Modified-Since"];
				if (lastModifiedString != null)
				{
					try
					{
						// return the datetime of the last time visited
						return DateTime.Parse(lastModifiedString);
					}
					catch { }
				}

				// if the datetime cannot be determind return the
				// min date availiable
				return DateTime.MinValue;
			}
		}

		protected virtual string ContentType
		{
			get { return "text/xml"; }
		}

		protected virtual string FileName
		{
			get { return "syndication.xml"; }
		}

		protected virtual bool IsAttachment
		{
			get { return false; }
		}

		protected virtual bool IsInLocalCache
		{
			get
			{
#if DEBUG
				return false;
#else
				return this.SectionInformation.Touched < this.LastVisited;
#endif
			}
		}

		protected virtual bool IsInServerCache
		{
			get
			{
#if !DEBUG
				// check to see if the item has been cached
				if (Common.Cache.IsCached(CacheKey)) 
				{
					SyndicationCacheItem cachedItem = Common.Cache[CacheKey] as SyndicationCacheItem;

					// check to see if the cached item is of the right type
					if (cachedItem != null) 
					{
						this.Feed = cachedItem;

						// if the cached item has more recent data than the section
						return this.SectionInformation.Touched < cachedItem.LastModified;
					}
				}
#endif
				// all checks indicate that it isn't in the server cache
				return false;
			}
		}

		protected abstract string CacheKey { get; }

		#endregion

		#region IHttpHandler Members

		public void ProcessRequest(HttpContext context)
		{
			this._context = context;
			this.ProcessSyndication();
		}

		public bool IsReusable { get { return false; } }

		#endregion

		protected abstract CacheItem CreateSyndication();

		protected virtual void CacheSyndication(CacheItem item)
		{
			Common.Cache.Add(this.CacheKey, item);
		}

		protected virtual void ProcessSyndication()
		{
			// if in local cache send a 304 status code
			// to tell the browser to look in the local
			// cache
			if (this.IsInLocalCache)
				this.Context.Response.StatusCode = (int)HttpStatusCode.NotModified;
			else if (!this.IsInServerCache)
			{
				this.Feed = this.CreateSyndication();
				if (this.Feed != null)
					this.CacheSyndication(this.Feed);
			}

			// write syndication
			this.WriteSyndication();
		}

		protected virtual void WriteSyndication()
		{
			if (this.Feed != null)
			{
				this.Context.Response.ContentEncoding = Encoding.UTF8;
				this.Context.Response.ContentType = this.ContentType;
				this.Context.Response.AddHeader(
					"Content-Disposition", 
					String.Format(@"{0};filename=""{1}""", this.IsAttachment ? "attachment" : "inline", this.FileName)
					);
				this.Context.Response.Cache.SetCacheability(HttpCacheability.Public);
				this.Context.Response.Cache.SetLastModified(this.Feed.LastModified);
				this.Context.Response.Cache.SetETag(this.Feed.Etag);
				this.Context.Response.Write(this.Feed.Xml);
			}
		}
	}
}
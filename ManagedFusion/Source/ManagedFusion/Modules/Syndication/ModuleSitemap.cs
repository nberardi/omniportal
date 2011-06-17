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
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.IO;

using ManagedFusion;
using ManagedFusion.Syndication;

namespace ManagedFusion.Modules.Syndication
{
	public class ModuleSitemap : ISyndication
	{
		private StringCollection _urls;

		public ModuleSitemap()
		{
			this._urls = new StringCollection();
		}

		private void AddUrl (string location, DateTime lastModified, bool useLastModified, ChangeFrequency changeFrequency, bool useChangeFrequency)
		{
			StringBuilder url = new StringBuilder();

			url.Append("	<url>");
			url.AppendFormat("		<loc>{0}</loc>", location);

			if (useLastModified)
				url.AppendFormat("		<lastmod>{0:s}{0:zzz}</lastmod>", lastModified);

			if (useChangeFrequency)
				url.AppendFormat("		<changefreq>{0}</changefreq>", changeFrequency);

			url.Append("	</url>");

			// add the new url to the collection
			this._urls.Add(url.ToString());
		}

		public void AddUrl(string location, DateTime lastModified, ChangeFrequency changeFrequency)
		{
			this.AddUrl(location, lastModified, true, changeFrequency, true);
		}

		public void AddUrl(string location, ChangeFrequency changeFrequency)
		{
			this.AddUrl(location, DateTime.MinValue, false, changeFrequency, true);
		}

		public void AddUrl(string location, DateTime lastModified)
		{
			this.AddUrl(location, lastModified, true, 0, false);
		}

		public void AddUrl(string location)
		{
			this.AddUrl(location, DateTime.MinValue, false, 0, false);
		}

		#region ISyndication Members

		public DateTime LastModified
		{
			get { return DateTime.Now; }
		}

		public string Serialize()
		{
			StringBuilder sitemap = new StringBuilder();

			sitemap.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.google.com/schemas/sitemap/0.84"">");

			foreach (string url in this._urls) {
				sitemap.Append(url);
			}

			sitemap.Append("</urlset>");

			// return the sitemap
			return sitemap.ToString();
		}

		#endregion
	}
}

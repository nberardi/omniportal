using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication.Sitemap
{
	internal class Sitemap : ISyndication
	{
		#region ISyndication Members

		public DateTime LastModified { get { return DateTime.Now; } }

		public string Serialize()
		{
			StringBuilder sb = new StringBuilder();
			using (SyndicationWriter writer = new SyndicationWriter(sb))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("urlset");
				writer.WriteAttributeString("xmlns", "http://www.google.com/schemas/sitemap/0.84");
				writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
				writer.WriteAttributeString("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://www.google.com/schemas/sitemap/0.84/sitemap.xsd");

				Feed feed = Common.ExecutingModule.Syndication;

				writer.WriteStartElement("url");

				writer.WriteStartElement("loc");
				writer.WriteValue(feed.Id);
				writer.WriteEndElement();

				writer.WriteStartElement("lastmod");
				writer.WriteValue(feed.Updated);
				writer.WriteEndElement();

				if (feed.ChangeFrequency != ChangeFrequency.NotDefined)
				{
					writer.WriteStartElement("changefreq");
					writer.WriteString(feed.ChangeFrequency.ToString().ToLower());
					writer.WriteEndElement();
				}

				if (feed.Priority.HasValue)
				{
					writer.WriteStartElement("priority");
					writer.WriteString(feed.Priority.Value.ToString("f1"));
					writer.WriteEndElement();
				}

				writer.WriteEndElement();

				foreach (Entry entry in feed.Items)
				{
					writer.WriteStartElement("url");

					writer.WriteStartElement("loc");
					writer.WriteValue(entry.Id);
					writer.WriteEndElement();

					writer.WriteStartElement("lastmod");
					writer.WriteValue(entry.Updated);
					writer.WriteEndElement();

					if (entry.ChangeFrequency != ChangeFrequency.NotDefined)
					{
						writer.WriteStartElement("changefreq");
						writer.WriteString(entry.ChangeFrequency.ToString().ToLower());
						writer.WriteEndElement();
					}

					if (entry.Priority.HasValue)
					{
						writer.WriteStartElement("priority");
						writer.WriteString(entry.Priority.Value.ToString("f1"));
						writer.WriteEndElement();
					}

					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
			return sb.ToString();
		}

		#endregion
	}
}

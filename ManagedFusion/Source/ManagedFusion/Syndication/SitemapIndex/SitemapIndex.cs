using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ManagedFusion.Syndication.SitemapIndex
{
	internal class SitemapIndex : ISyndication
	{
		internal SitemapIndex() { }

		#region ISyndication Members

		public DateTime LastModified { get { return DateTime.Now; } }

		public string Serialize()
		{
			StringBuilder sb = new StringBuilder();
			using (SyndicationWriter writer = new SyndicationWriter(sb))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("sitemapindex");
				writer.WriteAttributeString("xmlns", "http://www.google.com/schemas/sitemap/0.84");
				writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
				writer.WriteAttributeString("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://www.google.com/schemas/sitemap/0.84/siteindex.xsd");

				SiteMapNode node = SiteMap.CurrentNode;

				this.AddSiteMap(writer, node);
				this.AddSectionChildren(writer, node.ChildNodes);

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}

			// returns the generated sitemap index
			return sb.ToString();
		}

		private void AddSectionChildren (SyndicationWriter writer, SiteMapNodeCollection children)
		{
			foreach(SiteMapNode node in children)
			{
				this.AddSiteMap(writer, node);
				this.AddSectionChildren(writer, node.ChildNodes);
			}
		}

		private void AddSiteMap(SyndicationWriter writer, SiteMapNode node)
		{
			writer.WriteStartElement("sitemap");
			writer.WriteStartElement("loc");
			writer.WriteString(node.Url + "?sitemap");
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		#endregion
	}
}

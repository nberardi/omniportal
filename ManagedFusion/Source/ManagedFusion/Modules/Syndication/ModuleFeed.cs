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
using System.Text;
using System.Xml;
using System.IO;

using ManagedFusion;
using ManagedFusion.Syndication;

using Atom.Core;

namespace ManagedFusion.Modules.Syndication
{
	public class ModuleFeed : AtomFeed, ISyndication
	{
		private Guid _moduleID;

		public ModuleFeed(SectionInfo section)
		{
			// set the encoding to UTF8
			this.Encoding = Encoding.UTF8;

			// set the section specific information
			this.Id = section.UrlPath;
			this.Title = new AtomContentConstruct("title", section.Title);
			this.Links.Add(new AtomLink(section.UrlPath, Relationship.Alternate, MediaType.TextHtml));
			
			UriBuilder feed = new UriBuilder(section.UrlPath);
			feed.Query = "feed";
			
			this.Modified = new AtomDateConstruct("modified", section.Touched);

			// set the feed
			this.Links.Add(new AtomLink(feed.Uri, Relationship.ServiceFeed, MediaType.ApplicationXAtomXml));

			// set description if set
			if (section.MetaProperties["description"] != null)
				this.Info = new AtomContentConstruct("info", section.MetaProperties["description"]);

			// set copyright if set
			if (section.MetaProperties["copyright"] != null)
				this.Copyright = new AtomContentConstruct("copyright", section.MetaProperties["copyright"]);

			// set module specific data
			this._moduleID = section.Module.ID;
		}

		public Guid ModuleID 
		{
			get { return this._moduleID; }
		}

		#region ISyndication Members

		public DateTime LastModified 
		{ 
			get { return this.Modified.DateTime; }
		}

		public string Serialize()
		{
			StringBuilder feed = new StringBuilder();

			// create the elements for the feed
			ModuleElement moduleID = new ModuleElement("moduleid", this.ModuleID.ToString());
			ModuleElement generator = new ModuleElement("generator", Common.Properties.SoftwareName);

			// add addition elements to the feed
			this.AdditionalElements.Add(moduleID);
			this.AdditionalElements.Add(generator);

			using (SyndicationWriter writer = new SyndicationWriter(feed))
			{
				// get the feed text
				this.Save(writer);

				// clase the writer to free resources
				writer.Close();
			}

			// remove the elements added above
			this.AdditionalElements.Remove(moduleID);
			this.AdditionalElements.Remove(generator);

			// return the generated feed
			return feed.ToString();
		}

		#endregion
	}
}
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
using System.Data;
using System.Text.RegularExpressions;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Modules;
using ManagedFusion.Data;
using ManagedFusion.Syndication;

// OmniPortal Classes
using OmniPortal.Modules.Blog.Data;

namespace OmniPortal.Modules.Blog
{
	[Module("Blog Module",
		"This module is a web-log (blog), which is used to post a log about what you find informing.",
		"{A7181415-44BC-48aa-9519-2AFC69DCB92A}",
		"Blog",
		TraversablePath = true)]
	public class BlogModule : ModuleBase
	{
		#region OnLoadSyndication

		protected override void OnLoadSyndication(LoadSyndicationEventArgs e)
		{
			BlogDatabaseProvider dbprovider = Databases.Providers["OmniPortalBlog"] as BlogDatabaseProvider;
			BlogItem[] blogs = dbprovider.GetBlogs(this.Context);
			DateTime modified = DateTime.MinValue;

			// set the title for the feed
			e.Syndication.Title.InnerText = this.Properties["BlogTitle"];

			// populate content for syndication
			foreach(BlogItem blog in blogs) 
			{
				// check to see if this blog is suppose to be syndicated 
				// or has even been published yet
				if (blog.Syndicate == false || blog.Published == false)
					continue;

				// check to see if the blog date modified is more recent than
				// the previous modified value
				if (blog.Modified > modified)
					modified = blog.Modified;

				// get url id for the current entry
				Uri permalink = Common.Path.GetPortalUrl(String.Format("/archive/{0}.aspx", blog.ID));

				// add the item
				Entry entry = new Entry(
					blog.Title,
					permalink,
					blog.Modified
					);

				// set the author
				entry.Authors.Add(new Person("Fix Me")); // blog.Poster.FullName
				
				// set the title
				entry.Title.InnerText = blog.Title;
				
				// if there is a url for the title
				if (blog.TitleUrl != null)
					entry.Links.Add(new Link(blog.TitleUrl, blog.Title, LinkRelationship.Alternate, "text/html"));
				
				// if there is a source url
				if (blog.SourceUrl != null)
					entry.Links.Add(new Link(blog.SourceUrl, blog.Source, LinkRelationship.Related, "text/html"));

				// set the date time stamps for this entry
				entry.Published = blog.Issued;

				// set the body contents
				entry.Content = new Content("text/html", blog.Body);

				// add entry to the feed
				e.Syndication.Items.Add(entry);
			}

			// set the time the feed was last modified
			e.Syndication.Updated = modified;

			base.OnLoadSyndication (e);
		}

		#endregion

		protected override void OnLoad(LoadModuleEventArgs e)
		{
			// add the blog style sheet to the page.
			Common.PageBuilder.StyleSheets.Add(this.GetUrlPath("blog.css").ToString());

			// check to see if the current has a role with the task of Poster
			if (this.IsInTask("Poster")) 
			{
				// add admin links section to the portlets.
				e.GetHolder(0).Add(this.CreatePortlet(1, "Admin Links", this.GetControl("Portlets/Admin.ascx")));
			}

			// add public links section to the protlets
			e.GetHolder(0).Add(this.CreatePortlet(2, (this.IsInTask("Poster") ? "Public " : String.Empty) + "Links", this.GetControl("Portlets/Public.ascx")));

			// add calendar to the portlets
			e.GetHolder(0).Add(this.CreatePortlet(3, "Archive Calendar", this.GetControl("Portlets/Calendar.ascx")));

			base.OnLoad (e);
		}
	}
}
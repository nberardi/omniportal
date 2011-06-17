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
using System.Data.SqlClient;
using System.Collections;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Modules.Blog.Data.SqlServer
{
	public class BlogSqlServerProvider : BlogDatabaseProvider
	{
		private BlogItem BuildBlog (DataRow row) 
		{
			Uri titleUrl = null;
			Uri sourceUrl = null;
			
			try { titleUrl = new Uri(row["TitleUrl"] as string); }
			catch (SystemException) { }
		
			try { sourceUrl = new Uri(row["SourceUrl"] as string); }
			catch (SystemException) { }

			return new BlogItem(
				(int)row["Identity"],
				row["Title"] as string,
				row["Body"] as string,
				(bool)row["Published"],
				(bool)row["AllowComments"],
				(bool)row["Syndicate"],
				(Guid)row["User_ID"],
				titleUrl,
				row["Source"] as string,
				sourceUrl,
				(DateTime)row["Created"],
				(DateTime)row["Issued"],
				(DateTime)row["Modified"]
				);
		}

		public override void AddBlog(BlogItem item)
		{
			// create the post from the item
			Blog_Posts post = new Blog_Posts();
			post.Title = item.Title;
			post.Body = item.Body;
			post.Published = item.Published;
			post.AllowComments = item.AllowComments;
			post.Syndicate = item.Syndicate;
			post.User_ID = item.PosterID;
			post.TitleUrl = (item.TitleUrl != null) ? item.TitleUrl.ToString() : null;
			post.Source = item.Source;
			post.SourceUrl = (item.SourceUrl != null) ? item.SourceUrl.ToString() : null;
			post.Created = item.Created;
			post.Issued = item.Issued;
			post.Modified = item.Modified;

			// insert the post into the database
			post.Insert();
		}

		public override void UpdateBlog(BlogItem item)
		{
			// update the post from the item
			Blog_Posts post = new Blog_Posts();
			post.ID = item.ID;
			post.Title = item.Title;
			post.Body = item.Body;
			post.Published = item.Published;
			post.AllowComments = item.AllowComments;
			post.Syndicate = item.Syndicate;
			post.User_ID = item.PosterID;
			post.TitleUrl = (item.TitleUrl != null) ? item.TitleUrl.ToString() : null;
			post.Source = item.Source;
			post.SourceUrl = (item.SourceUrl != null) ? item.SourceUrl.ToString() : null;
			post.Created = item.Created;
			post.Issued = item.Issued;
			post.Modified = item.Modified;

			post.Update();
		}

		public override void RemoveBlog(int id)
		{
			Blog_Posts post = new Blog_Posts();
			post.ID = id;

			post.Delete();
		}

		public override BlogItem GetBlog(int id)
		{
			// create the post provider object
			Blog_Posts posts = new Blog_Posts();
			posts.ID = id;

			// get the post with the id
			DataTable table = posts.SelectOne();
			
			// check to see if a row was returned
			if (table.Rows.Count > 0)
				return this.BuildBlog(table.Rows[0]);

			// nothing was found return null
			return null;
		}

		public override BlogItem[] GetDateRange(DateTime startDate, DateTime stopDate)
		{
			// create the post provider object
			Posts posts = new Posts();

			// select all posts in a date range
			DataTable table = posts.SelectDateRange(startDate, stopDate);

			ArrayList blogs = new ArrayList(table.Rows.Count);

			foreach(DataRow row in table.Rows) 
			{
				blogs.Add(this.BuildBlog(row));
			}

			return blogs.ToArray(typeof(BlogItem)) as BlogItem[];
		}

		public override BlogItem[] GetTopBlogs(int recordsToReturn)
		{
			// create the post provider object
			Posts posts = new Posts();

			// select all posts in a date range
			DataTable table = posts.SelectTop(recordsToReturn);

			ArrayList blogs = new ArrayList(table.Rows.Count);

			foreach(DataRow row in table.Rows) 
			{
				blogs.Add(this.BuildBlog(row));
			}

			return blogs.ToArray(typeof(BlogItem)) as BlogItem[];
		}
	}
}
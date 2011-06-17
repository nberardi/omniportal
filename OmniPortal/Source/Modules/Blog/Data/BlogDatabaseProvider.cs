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
using System.Web;

using ManagedFusion;
using ManagedFusion.Data;

namespace OmniPortal.Modules.Blog.Data
{
	public abstract class BlogDatabaseProvider : DatabaseProvider
	{
		public abstract BlogItem GetBlog (int id);

		public abstract BlogItem[] GetDateRange (DateTime startDate, DateTime stopDate);

		public abstract BlogItem[] GetTopBlogs (int recordsToReturn);

		public abstract void UpdateBlog (BlogItem item);

		public abstract void AddBlog (BlogItem item);

		public abstract void RemoveBlog (int id);

		public BlogItem[] GetBlogs (HttpContext context) 
		{
			BlogItem[] blogs = null;

			// if the match was a sucess create the syndication
			if (context.Request.QueryString["year"] != null
				&& context.Request.QueryString["month"] != null) 
			{
				DateTime startDate, stopDate;
				int year, month, day;
				bool containsDay = context.Request.QueryString["day"] != null;

				year = Convert.ToInt32(context.Request.QueryString["year"]);
				month = Convert.ToInt32(context.Request.QueryString["month"]);

				// check for day, if not available set as 1
				day = (containsDay) ? Convert.ToInt32(context.Request.QueryString["day"]) : 1;

				// set the start time
				startDate = new DateTime(year, month, day, 0, 0, 0, 0);
			
				// if contains a day add 1 day to the start date
				if (containsDay)
					stopDate = startDate.AddDays(1);
					// if doesn't contain a day add 1 month to the start date
				else 
					stopDate = startDate.AddMonths(1);

				blogs = GetDateRange(startDate, stopDate);
			} 
			else 
			{
				blogs = GetTopBlogs(25);
			}

			return blogs;
		}
	}
}
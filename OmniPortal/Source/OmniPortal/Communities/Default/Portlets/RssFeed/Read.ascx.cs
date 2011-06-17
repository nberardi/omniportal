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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Portlets;

namespace OmniPortal.Portlets.RssFeed
{
	[Portlet("RSS Feed", "This is used to consume RSS Feeds from the web.",
		"RssFeed",
		"{8F369A68-564E-438b-B915-B4605EC12FE2}",
		EditPage = "Edit.ascx")]
	public partial class Read : PortletUserControl
	{
		protected string XmlDocument 
		{
			get { return this.Properties["RssFeedUrl"]; }
		}

		protected string XslDocument 
		{
			get 
			{
				string key = String.Concat(this.PortletID.ToString(), "RssFeed-XslDocument");
				
				if (Common.Cache.IsCached(key) == false) 
					Common.Cache.Add(key, Common.Path.GetAbsoluteUrl(Common.Path.GetPortletPath("RssFeed", "Rss2-0.xsl")).AbsolutePath);

				return (string)Common.Cache[key];
			}
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			string key = String.Concat("RssFeed", this.PortletID.ToString());
			
			try 
			{
				if (Common.Cache.IsCached(key) == false) 
				{
#if TRACE
					Context.Trace.Write("RssFeed", String.Concat("Fetching ", XmlDocument));
#endif
					// Creates a new XmlDocument object
					XmlDocument rss = new XmlDocument();

					// Loads the RSS Feed from the passed URL
					rss.Load(this.XmlDocument);

					// create writer
					System.IO.StringWriter stringWriter = new System.IO.StringWriter();

					// writes XML content to writer
					rss.WriteContentTo(new XmlTextWriter(stringWriter));

					// the object cast is needed so the method is not ambiguous
					Common.Cache.Add(key, (object)stringWriter.ToString(), DateTime.Now.AddDays(1));
				}

				// create XML transformation control
				System.Web.UI.WebControls.Xml rssfeed = new System.Web.UI.WebControls.Xml();

				// sets data for control
				rssfeed.DocumentContent = (string)Common.Cache[key];
				rssfeed.TransformSource = this.XslDocument;

				// write the HTML output to the writer.
				rssfeed.RenderControl(writer);
			} 
			catch (Exception exc) 
			{
				writer.Write("<center><strong>An error occured in the RSS Feed.</strong></center>");
				Trace.Warn("RssFeed", String.Concat("An error occured in ", this.Title, " \n\twith RSS URL ", this.XmlDocument, " \n\twith XSL URL ", this.XslDocument), exc);
				Common.Cache.Remove(key);
			}
		}
	}
}
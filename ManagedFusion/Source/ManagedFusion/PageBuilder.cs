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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace ManagedFusion
{
	public enum CssVersion 
	{
		One = 1,
		Two = 2,
		Default = Two
	}

	public class PageBuilder
	{
		internal PageBuilder () {}

		#region Properties

		private CssVersion _cssVersion = CssVersion.Default;
		/// <summary></summary>
		public CssVersion CssVersion 
		{
			get { return this._cssVersion; }
			set { this._cssVersion = value; }
		}

		/// <summary></summary>
		public List<string> StyleSheets 
		{ 
			get 
			{
				if (Common.Context.Items["StyleSheets"] == null)
					Common.Context.Items["StyleSheets"] = new List<string>();

				return Common.Context.Items["StyleSheets"] as List<string>; 
			}
		}

		/// <summary></summary>
		public List<string> PageContent 
		{
			get 
			{
				if (Common.Context.Items["PageContent"] == null)
					Common.Context.Items["PageContent"] = new List<string>();

				return Common.Context.Items["PageContent"] as List<string>;
			}
		}

		/// <summary></summary>
		public List<string> PageTitle 
		{
			get 
			{
				if (Common.Context.Items["PageTitle"] == null)
					Common.Context.Items["PageTitle"] = new List<string>();

				return Common.Context.Items["PageTitle"] as List<string>;
			}
		}

		/// <summary></summary>
		public List<HtmlMeta> PageMetaData 
		{
			get 
			{
				if (Common.Context.Items["PageMetaData"] == null)
					Common.Context.Items["PageMetaData"] = new List<HtmlMeta>();

				return Common.Context.Items["PageMetaData"] as List<HtmlMeta>;
			}
		}

		/// <summary></summary>
		public List<HtmlLink> PageLinks
		{
			get
			{
				if (Common.Context.Items["PageLinks"] == null)
					Common.Context.Items["PageLinks"] = new List<HtmlLink>();

				return Common.Context.Items["PageLinks"] as List<HtmlLink>;
			}
		}

		/*****************************************************************************
		 * Form Tag Manipulation
		 */
		/// <summary>
		/// Used to add attribute <c>enctype="multipart/form-data"</c> to the pages form,
		/// for uploading of data.
		/// </summary>
		public bool FormUploadMode 
		{
			get 
			{
				if (Common.Context.Items["FormUploadMode"] == null)
					FormUploadMode = false;
				return (bool)Common.Context.Items["FormUploadMode"];
			}
			set { Common.Context.Items["FormUploadMode"] = value; }
		}
		/*
		 *****************************************************************************/

		/*****************************************************************************
		 * Body Tag Manipulation
		 */

		/// <summary>
		/// Used to allow the use of <c>frameset</c> tag in the page.
		/// </summary>
		public bool FramesetMode 
		{
			get 
			{
				if (Common.Context.Items["FramesetMode"] == null)
					FramesetMode = false;
				return (bool)Common.Context.Items["FramesetMode"];
			}
			set { Common.Context.Items["FramesetMode"] = value; }
		}

		/*
		 *****************************************************************************/

		#endregion

		#region Methods

		internal void RenderHeader(Page page)
		{
			// get the <head>
			HtmlHead header = page.Header as HtmlHead;
			
			// add portal tag line to top of header
			header.Controls.AddAt(0, new LiteralControl("<!-- " + PortalProperties.PortalCopyright + " -->"));

			// add meta data to header
			foreach (HtmlMeta meta in this.PageMetaData)
			{
				header.Controls.Add(meta);
			}

			// add link data to header
			foreach (HtmlLink link in this.PageLinks)
			{
				header.Controls.Add(link);
			}

			// add the correct CSS style path depending on version
			switch ((int)this.CssVersion)
			{
				case 1:
					this.RenderCss1(header);
					break;

				case 2:
					this.RenderCss2(header);
					break;
			}

			if (this.PageContent.Count > 0)
			{
				header.Controls.Add(new LiteralControl("<!-- Start of Head Content -->"));

				// add content to the header
				foreach (string content in this.PageContent)
				{
					header.Controls.Add(new LiteralControl(content));
				}

				header.Controls.Add(new LiteralControl("<!-- End of Head Content -->"));
			}
		}

		internal void RenderTitle(Page page)
		{
			// set the page title
			page.Title = String.Join(" - ", this.PageTitle.ToArray());
		}

		private void RenderCss1(HtmlHead header)
		{
			// add all style sheets to page
			foreach (string style in this.StyleSheets)
			{
				HtmlLink link = new HtmlLink();
				link.Href = style;
				link.Attributes.Add("rel", "stylesheet");
				link.Attributes.Add("type", "text/css");

				// add this style to the header
				header.Controls.Add(link);
			}
		}

		private void RenderCss2(HtmlHead header)
		{
			StringBuilder builder = new StringBuilder();

			// add the following script to correct rendering problems in IE
			if (Common.Context.Request.Browser.Browser.ToUpper() == "IE"
				&& Common.Context.Request.Browser.MajorVersion <= 6)
			{
				builder.AppendLine("<!-- to correct the unsightly Flash of Unstyled Content. http://www.bluerobot.com/web/css/fouc.asp -->");
				builder.AppendLine(@"<script type=""text/javascript""></script>");
				builder.AppendLine();
			}

			builder.AppendLine(@"<style type=""text/css"" media=""all"" title=""OmniPortalStyles"">");

			// add all style sheets to page
			foreach (string style in this.StyleSheets)
				builder.AppendFormat("\t@import \"{0}\";{1}", style, Environment.NewLine);

			builder.AppendLine("</style>");

			// add the style to the header
			header.Controls.Add(new LiteralControl(builder.ToString()));
		}

		#endregion
	}
}
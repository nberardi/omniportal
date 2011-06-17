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
using System.Reflection;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Modules;

namespace ManagedFusion
{
	/// <summary>Main page for processing the web site.</summary>
	/// <seealso href="http://msdn2.microsoft.com/en-us/library/ms178472.aspx">Page Life Cycle Overview</seealso>
	public class PortalPage : Page
	{
		#region Private Methods

		private void SetContentTemplateCollection(IDictionary value)
		{
			Type targetType = typeof(Page);

			// get the reflected field reference for _contentTemplateControl
			FieldInfo contentTemplateCollection = targetType.GetField("_contentTemplateCollection", BindingFlags.Instance | BindingFlags.NonPublic);

			// set the content place holder id
			contentTemplateCollection.SetValue(this, value);
		}

		#endregion

		protected override void OnPreInit(EventArgs e)
		{
			IDictionary contentTemplateControl = new Hashtable();
			CommunityInfo community = CommunityInfo.Current;
			SectionInfo section = SectionInfo.Current;

			// load the page skin
			this.MasterPageFile = "~" + Common.Path.GetPagePath(community.Config.DefaultSkinTemplate);
			this.AppRelativeVirtualPath = "~" + section.UrlPath.PathAndQuery;

			// set the view state key to the name of the user
			this.ViewStateUserKey = Common.Context.User.Identity.Name;

			// add the main content holder
			contentTemplateControl.Add("Main", new CompiledTemplateBuilder(new BuildTemplateMethod((Common.ExecutingModule as ModuleBase).InstantiateContentIn)));

			// add the alternate content holders
			// HACK: Static representation of columns we need to create a way to specify max number of columns in config file.
			contentTemplateControl.Add("Column0", new CompiledTemplateBuilder(new BuildTemplateMethod((Common.ExecutingModule as ModuleBase).InstantiateContentIn)));

			// set the content template controls for use in the master page
			this.SetContentTemplateCollection(contentTemplateControl);
		}

		protected override void OnInit(EventArgs e)
		{
			CommunityInfo community = CommunityInfo.Current;
			SectionInfo section = SectionInfo.Current;

			// set CSS type that page supports
			Common.PageBuilder.CssVersion = CssVersion.Two;

			// add community header to page header
			if (String.IsNullOrEmpty(community.HeaderText) == false)
				Common.PageBuilder.PageContent.Add(community.HeaderText);

			// add theme header to page header
			if (String.IsNullOrEmpty(section.Theme.HeaderText) == false)
				Common.PageBuilder.PageContent.Add(section.Theme.HeaderText);

			// set page title elements
			Common.PageBuilder.PageTitle.Add(community.Title);
			Common.PageBuilder.PageTitle.Add(section.Title);

			// render the title for the page
			Common.PageBuilder.RenderTitle(this);

			// create generator meta data for header
			HtmlMeta generator = new HtmlMeta();
			generator.Name = "generator";
			generator.Content = PortalProperties.SoftwareName;

			// add the page generator meta property
			Common.PageBuilder.PageMetaData.Add(generator);

			// add section meta properties
			NameValueCollection sectionMetaProperties = section.MetaProperties;
			foreach (string key in sectionMetaProperties.Keys)
			{
				HtmlMeta meta = new HtmlMeta();
				meta.Name = key;
				meta.Content = sectionMetaProperties[key];

				Common.PageBuilder.PageMetaData.Add(meta);
			}

			// check to see if this section is syndicated
			if (section.Syndicated)
			{
				HtmlLink syndication = new HtmlLink();
				syndication.Href = section.UrlPath.ToString() + "?feed";
				syndication.Attributes.Add("title", this.Title);
				syndication.Attributes.Add("rel", "alternate");
				syndication.Attributes.Add("type", "application/atom+xml");

				// add syndication link to the page
				Common.PageBuilder.PageLinks.Add(syndication);
			}

			// add page style to style list, put this at the top of the style sheet list
			if (section.Style.Name != StyleInfo.NoStyle)
				Common.PageBuilder.StyleSheets.Insert(0, section.Style.Path);

			// render the header for the page
			Common.PageBuilder.RenderHeader(this);

			base.OnPreInit(e);
		}
	}
}
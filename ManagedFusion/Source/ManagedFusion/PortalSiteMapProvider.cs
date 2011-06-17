using System;
using System.Configuration.Provider;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Web;


namespace ManagedFusion
{
	[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class PortalSiteMapProvider : SiteMapProvider
	{
		public override string Name
		{
			get { return "Portal"; }
		}

		public override string Description
		{
			get { return "This provider intigrates ManagedFusion with the ASP.NET Site Map Provider."; }
		}

		public override SiteMapNode RootNode
		{
			get { return SiteInfo.Current.ConnectedSection; }
		}

		public override SiteMapNode CurrentNode
		{
			get { return SectionInfo.Current; }
		}

		public override SiteMapNode FindSiteMapNode(HttpContext context)
		{
			string requestPath = Common.Path.GetUrlPath(context);
			string requestBasePath = Common.Path.GetBaseUrlPath(context);

			// get site information for this request
			SiteInfo site = SiteInfo.GetSiteForHost(context);

			// get section information for this request
			return site.ConnectedSection.GetSectionForPath(requestBasePath);
		}

		public override SiteMapNode FindSiteMapNode(string rawUrl)
		{
			return SectionInfo.FindSection(rawUrl);
		}

		public override SiteMapNode FindSiteMapNodeFromKey(string key)
		{
			int i = 0;
			if (Int32.TryParse(key, out i))
				return SectionInfo.Collection[i];
			else
				return null;
		}

		public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
		{
			SectionInfo section = (SectionInfo)node;
			return section.Children;
		}

		public override SiteMapNode GetParentNode(SiteMapNode node)
		{
			SectionInfo section = (SectionInfo)node;
			return section.Parent;
		}

		protected override SiteMapNode GetRootNodeCore()
		{
			return SiteInfo.Current.ConnectedSection;
		}

		public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
		{
			SectionInfo section = (SectionInfo)node;
			return section.IsAccessibleToUser(context);
		}
	}
}

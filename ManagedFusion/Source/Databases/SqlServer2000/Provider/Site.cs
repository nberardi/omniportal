using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Data.SqlServer2000
{
	public partial class Site
	{
		public static implicit operator SiteInfo(Site s)
		{
			return new SiteInfo(
				s._siteID,
				s._domain,
				s._subDomain,
				s._touched,
				s._sectionID ?? SectionInfo.NullIdentity,
				s._theme,
				s._style
				);
		}

		public static explicit operator Site(SiteInfo s)
		{
			Site site = new Site();
			site._siteID = s.Identity;
			site._sectionID = (s.ConnectedSection != null) ? s.ConnectedSection.Identity : (int?)null;
			site._name = s.FullDomain;
			site._description = s.FullDomain;
			site._touched = s.Touched;
			site._subDomain = s.SubDomain;
			site._domain = s.Domain;
			site._theme = s.OriginalTheme;
			site._style = s.OriginalStyle;
			return site;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Data.SqlServer2000
{
	public partial class Section
	{
		public static implicit operator SectionInfo(Section s)
		{
			return new SectionInfo(
				s._sectionID,
				s._name,
				s._description,
				s._parentSectionID,
				s._sortOrder,
				s._isVisible,
				s._syndicateFeed,
				s._theme,
				s._style,
				s._moduleID,
				s._owner,
				s._touched,
				s._communityID
				);
		}

		public static explicit operator Section(SectionInfo s)
		{
			Section section = new Section();
			section._sectionID = s.Identity;
			section._parentSectionID = (s.Parent != null) ? s.Parent.Identity : RootSection.Identity;
			section._communityID = s.ConnectedCommunity.Identity;
			section._name = s.Name;
			section._description = s.OriginalTitle;
			section._touched = s.Touched;
			section._sortOrder = s.Order;
			section._isEnabled = s.Enabled;
			section._isVisible = s.Visible;
			section._syndicateFeed = s.Syndicated;
			section._syndicateSitemap = s.Syndicated;
			section._owner = s.OriginalOwner;
			section._moduleID = s.Module.Identity;
			section._theme = s.OriginalTheme;
			section._style = s.OriginalStyle;
			return section;
		}
	}
}

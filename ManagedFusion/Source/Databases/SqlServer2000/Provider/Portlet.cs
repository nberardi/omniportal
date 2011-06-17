using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Data.SqlServer2000
{
	public partial class Portlet
	{
		public static implicit operator PortletInfo(Portlet p)
		{
			return new PortletInfo(
				p._portletID,
				p._description,
				p._moduleID,
				p._touched
				);
		}

		public static explicit operator Portlet(PortletInfo p)
		{
			Portlet portlet = new Portlet();
			portlet._portletID = p.Identity;
			portlet._name = p.Title;
			portlet._description = p.Title;
			portlet._touched = p.Touched;
			portlet._moduleID = p.Module.Identity;
			return portlet;
		}
	}
}

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
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace OmniPortal.Modules.Blog.Portlets
{
	public partial class Calendar : System.Web.UI.UserControl
	{

		#region Web Form Designer generated code
		
		protected override void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.BlogCalendar.DayRender += new System.Web.UI.WebControls.DayRenderEventHandler(this.BlogCalendar_DayRender);

		}

		#endregion

		protected void BlogCalendar_SelectionChanged(object sender, System.EventArgs e)
		{
			DateTime selectedDate = BlogCalendar.SelectedDate;

			Response.Redirect(
				ManagedFusion.Common.Path.GetPortalUrl(
					String.Format("{0}/{1}/{2}/Default.aspx",
						selectedDate.Year,
						selectedDate.Month,
						selectedDate.Day
						)
					).ToString()
				);
		}

		private void BlogCalendar_DayRender(object sender, System.Web.UI.WebControls.DayRenderEventArgs e)
		{
			if (e.Day.Date > DateTime.Today) 
			{
				e.Cell.ForeColor = Color.FromArgb(0x99, 0x99, 0x99);
				e.Day.IsSelectable = false;
			}
		}
	}
}
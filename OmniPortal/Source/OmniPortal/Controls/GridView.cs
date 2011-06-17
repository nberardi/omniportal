using System;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace OmniPortal.Controls
{
	[
		ToolboxData("<{0}:GridView runat=\"server\"></{0}:GridView>"),
		DefaultEvent("SelectedIndexChanged"),
		SupportsEventValidation,
		Designer("System.Web.UI.Design.WebControls.GridViewDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
		ControlValueProperty("SelectedValue"),
		AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
		AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)
	]
	public class GridView : System.Web.UI.WebControls.GridView
	{
		/// <summary>
		/// Event fired for each row currentRow in the GridView, so you can customize the rendering of the grid.
		/// </summary>
		[Description("Event fired for each row currentRow in the GridView, so you can customize the rendering of the grid.")]
		[Category("Render")]
		public event GridViewRowRenderEventHandler RowRender;

		/// <summary>
		/// Fires the <see cref="OmniPortal.Components.Modules.Common.MasterDataGrid.DataGridItemRender">DataGridItemRender</see> event.
		/// </summary>
		/// <param name="e">Event Arguments.</param>
		protected virtual void OnRowRender(GridViewRowRenderEventArgs e)
		{
			if (RowRender != null)
				RowRender(this, e);
		}

		/// <summary>
		/// Renders the contents of the control into the specified writer. This method is used primarily by control developers.
		/// </summary>
		/// <param name="output"></param>
		protected override void RenderContents(HtmlTextWriter output)
		{
			if (HasControls())
			{
				// get the table from the control list
				Table table = this.Controls[0] as Table;

				for (int i = 0; i < table.Rows.Count; i++)
				{
					// sets the row for the current data grid currentRow
					GridViewRow currentRow = table.Rows[i] as GridViewRow;

					// set event agruments for DataGridItem Render
					GridViewRowRenderEventArgs args = new GridViewRowRenderEventArgs(currentRow);

					// raise RowRender event
					OnRowRender(args);

					// add rows before the current row
					foreach (TableRow row in args.RowsBeforeRow)
						table.Rows.AddAt(i++, row);

					// add rows after the current row
					foreach (TableRow row in args.RowsAfterRow)
						table.Rows.AddAt(++i, row);
				}
			}

			base.RenderContents(output);
		}
	}
}

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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Controls
{
	/// <summary>
	/// DataGrid that enables the creation of Master/Detail grids
	/// </summary>
	[
		ToolboxData("<{0}:DataGrid runat=\"server\"></{0}:DataGrid>"),
		Designer("System.Web.UI.Design.WebControls.DataGridDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
		Editor("System.Web.UI.Design.WebControls.DataGridComponentEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(ComponentEditor)),
		AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
		AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)
	]
	public class DataGrid : System.Web.UI.WebControls.DataGrid
	{
		/// <summary>
		/// Event fired for each row currentRow in the GridView, so you can customize the rendering of the grid.
		/// </summary>
		[Description("Event fired for each row currentRow in the GridView, so you can customize the rendering of the grid.")]
		[Category("Render")]
		public event DataGridItemRenderEventHandler RowRender;

		/// <summary>
		/// Fires the <see cref="OmniPortal.Components.Modules.Common.MasterDataGrid.DataGridItemRender">DataGridItemRender</see> event.
		/// </summary>
		/// <param name="e">Event Arguments.</param>
		protected virtual void OnRowRender(DataGridItemRenderEventArgs e)
		{
			if (RowRender != null)
				RowRender(this, e);
		}

		/// <summary>
		/// Renders the contents of the control into the specified writer. This method is used primarily by 
		/// control developers.
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
					DataGridItem currentRow = table.Rows[i] as DataGridItem;

					// set event agruments for DataGridItem Render
					DataGridItemRenderEventArgs args = new DataGridItemRenderEventArgs(currentRow);
						
					// raise DataGridItem Render
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
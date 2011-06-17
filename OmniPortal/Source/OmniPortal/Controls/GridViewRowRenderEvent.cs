using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace OmniPortal.Controls
{
	/// <summary>
	/// Delegate that defines the type of the <see cref="SuperGridView.RowRender"/> event
	/// </summary>
	public delegate void GridViewRowRenderEventHandler(object sender, GridViewRowRenderEventArgs e);

	/// <summary>
	/// Event arguments used by the event <see cref="SuperGridView.RowRender"/> in the <see cref="SuperGridView"/>
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough]
	public class GridViewRowRenderEventArgs : GridViewRowEventArgs
	{
		private List<TableRow> _beforeRow;
		private List<TableRow> _afterRow;

		public GridViewRowRenderEventArgs(GridViewRow row)
			: base(row)
		{
			this._beforeRow = new List<TableRow>();
			this._afterRow = new List<TableRow>();
		}

		public List<TableRow> RowsBeforeRow
		{
			get { return _beforeRow; }
		}

		public List<TableRow> RowsAfterRow
		{
			get { return _afterRow; }
		}
	}
}

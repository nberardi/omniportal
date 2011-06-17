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
using System.Web.UI.WebControls;

namespace OmniPortal.Controls
{
	/// <summary>
	/// Delegate that defines the type of the <see cref="MasterDetailGrid.DataGridItemRender"/> event
	/// </summary>
	public delegate void DataGridItemRenderEventHandler(object sender, DataGridItemRenderEventArgs e);

	/// <summary>
	/// Event arguments used by the event <see cref="MasterDetailGrid.DataGridItemRender"/> in the <see cref="MasterDetailGrid"/>
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough]
	public class DataGridItemRenderEventArgs : DataGridItemEventArgs
	{
		private ArrayList _beforeRow;
		private ArrayList _afterRow;

		public DataGridItemRenderEventArgs(DataGridItem item)
			: base(item)
		{
			this._beforeRow = new ArrayList();
			this._afterRow = new ArrayList();
		}

		public ArrayList RowsBeforeRow
		{
			get { return _beforeRow; }
		}

		public ArrayList RowsAfterRow
		{
			get { return _afterRow; }
		}
	}
}
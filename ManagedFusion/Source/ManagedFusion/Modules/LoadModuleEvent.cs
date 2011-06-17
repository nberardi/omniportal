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
using System.Web.UI;
using System.Web.UI.WebControls;

// ManagedFusion Classes
using ManagedFusion;

namespace ManagedFusion.Modules
{
	/// <summary>Event handler for PupulateHolders.</summary>
	public delegate void LoadModuleEventHandler (object sender, LoadModuleEventArgs e);

	/// <summary>Event argumnets for PopulateHolders.</summary>
	public class LoadModuleEventArgs : EventArgs 
	{
		private Content[] _holders;
		private Content _centerTop;
		private Content _centerBottom;

		/// <summary>
		/// Creates an instance of PopulateHoldersEventArgs.
		/// </summary>
		/// <param name="holders"></param>
		public LoadModuleEventArgs(Content[] holders, Content beforeModule, Content afterModule) 
		{
			this._holders = holders;
			this._centerTop = beforeModule;
			this._centerBottom = afterModule;
		}

		/// <summary>Selected column place holder.</summary>
		public ControlCollection GetHolder(int id)
		{
			if (id > this._holders.GetUpperBound(0))
				throw new ArgumentOutOfRangeException("id");

			return this._holders[id].Controls;
		}

		/// <summary>Center column place holder above module.</summary>
		public ControlCollection CenterTop { get { return this._centerTop.Controls; } }
		
		/// <summary>Center column place holder below module.</summary>
		public ControlCollection CenterBottom { get { return this._centerBottom.Controls; } }
	}
}
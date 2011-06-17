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

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Syndication;

namespace ManagedFusion.Modules
{
	/// <summary>Event handler for LoadSyndicationEventHandler.</summary>
	public delegate void LoadSyndicationEventHandler (object sender, LoadSyndicationEventArgs e);

	/// <summary>Event argumnets for LoadSyndicationEventArgs.</summary>
	public class LoadSyndicationEventArgs : EventArgs 
	{
		/// <summary>Creates an instance of LoadSyndicationEventArgs.</summary>
		public LoadSyndicationEventArgs(Feed syndication) 
		{
			this._syndication = syndication;
		}

		private Feed _syndication;
		public Feed Syndication 
		{ 
			get { return this._syndication; } 
		}
	}
}
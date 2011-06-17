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
using System.ComponentModel;

namespace ManagedFusion
{
	public abstract class ModuleInfo
	{
		#region Fields

		private readonly Guid _id;
		private readonly string _title;
		private readonly string _description;
		private readonly Type _type;

		#endregion

		#region Construstors

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ModuleInfo (Guid id, string title, string description, Type type)
		{
			this._id = id;
			this._title = title;
			this._description = description;
			this._type = type;
		}

		#endregion

		#region Properties

		public Guid Identity { get { return this._id; } }

		public string Title { get { return this._title; } }

		public string Description { get { return this._description; } }

		public Type Type { get { return this._type; } }

		#endregion

		#region Methods

		public override string ToString () { return Title.ToString(); }

		#endregion
	}
}
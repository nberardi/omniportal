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

namespace ManagedFusion
{
	/// <summary>
	/// Used for quick consuming of modules when needed to be displayed in list form.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public abstract class SkinAttribute : Attribute
	{
		private readonly string _title;
		private readonly string _description;

		/// <summary>Creates a new module attribute for a module.</summary>
		/// <param name="title">Title of module.</param>
		/// <param name="description">Description of module.</param>
		protected SkinAttribute(string title, string description)
		{
			// check to see if title has been set
			if (title == null || title.Length == 0)
				throw new ArgumentException("Title needs to be set for SkinAttribute.  Cannot be String.Empty or null.", "title");

			this._title = title;
			this._description = description;
		}

		/// <summary>Title of the module.</summary>
		public string Title { get { return this._title; } }

		/// <summary></summary>Description of the module.</summary>
		public string Description { get { return this._description; } }
	}
}
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

namespace ManagedFusion.Modules
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public sealed class ModuleAttribute : SkinAttribute
	{
		private bool _traversablePath = false;
		private bool _installable = false;
		private bool _configInFolder = false;

		private Guid _id;
		private string _folderName;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="title"></param>
		/// <param name="description"></param>
		/// <param name="id"></param>
		public ModuleAttribute(string title, string description, string id)
			: this(title, description, id, null) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="title"></param>
		/// <param name="description"></param>
		/// <param name="id"></param>
		/// <param name="folderBased"></param>
		/// <param name="folderName"></param>
		public ModuleAttribute(string title, string description, string id, string folderName)
			: base(title, description)
		{
			this._id = new Guid(id);
			this._folderName = folderName;
		}

		/// <summary>The Identity of the module to be used as a unique identifier.</summary>
		public Guid ModuleID { get { return this._id; } }

		public bool FolderBased { get { return String.IsNullOrEmpty(this._folderName) == false; } }

		public string FolderName { get { return this._folderName; } }

		public bool ConfigInFolder
		{
			get { return this._configInFolder; }
			set { this._configInFolder = (this.FolderBased && value); }
		}

		/// <summary>Indicates if the URL path after the module is part of the module.</summary>
		/// <remarks>
		/// <note>If the module is traversable then that means the module cannot have an child-modules.</note>
		/// <para>If set to <see langword="true"/> then everything in the URL path after the module, also
		/// belongs to the module.  For example: <c>http://www.mysite.com/default.aspx/MyModule/1980/03/14/0800.aspx</c>  
		/// The <c>1980/03/14/0800.aspx</c> belong to the module.  However, if set to <see langword="false"/> each
		/// wack (<c>/</c>) after <c>MyModule</c> would be an additional module.</para>
		/// </remarks>
		public bool TraversablePath 
		{ 
			get { return this._traversablePath; }
			set { this._traversablePath = value; }
		}

		/// <summary>Tells if this module has an install script.</summary>
		public bool Installable 
		{ 
			get { return this._installable; } 
			set { this._installable = value; }
		}
	}
}
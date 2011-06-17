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
using ManagedFusion.Portlets;

namespace ManagedFusion
{
	public sealed class PortletModule : ModuleInfo
	{
		private readonly string _folderName;
		private readonly string _readPage;
		private readonly string _addPage;
		private readonly string _editPage;
		private readonly string _deletePage;
		private readonly string _adminPage;

		internal PortletModule (PortletAttribute attribute, Type type)
			: base(attribute.PortletID, attribute.Title, attribute.Description, type)
		{
			this._folderName = attribute.FolderName;
			this._readPage = attribute.ReadPage;
			this._addPage = attribute.AddPage;
			this._editPage = attribute.EditPage;
			this._deletePage = attribute.DeletePage;
			this._adminPage = attribute.AdminPage;
		}

		public static ModuleCollection Collection
		{
			get { return Common.DatabaseProvider.PortletModules; }
		}

		public string FolderName { get { return this._folderName; } }

		public string ReadPage { get { return this._readPage; } }

		public string AddPage { get { return this._addPage; } }

		public string EditPage { get { return this._editPage; } }

		public string DeletePage { get { return this._deletePage; } }

		public string AdminPage { get { return this._adminPage; } }
	}
}
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

namespace ManagedFusion.Portlets
{
	/// <summary>
	/// Summary description for ContainerAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public sealed class PortletAttribute : SkinAttribute
	{
		private string _foldername;
		private string _addpage;
		private string _editpage;
		private string _deletepage;
		private string _adminpage;
		private string _readpage;
		private Guid _id;

		/// <summary>
		/// Creates a new admin module attribute for an admin module.
		/// </summary>
		/// <param name="title">Title of module.</param>
		/// <param name="description">Description of module.</param>
		/// <param name="filename">The folder name of the portlet.</param>
		/// <param name="id">The portlet id.</param>
		public PortletAttribute(string title, string description, string foldername, string id) : base(title, description) 
		{
			this._id = new Guid(id);
			this._foldername = foldername;
			this._readpage = "Read.ascx";
		}

		/// <summary>The folder name of the portlet.</summary>
		public string FolderName { get { return this._foldername; } }

		/// <summary>The Identity of the portlet to be used as a unique identifier.</summary>
		public Guid PortletID { get { return this._id; } }

		public string ReadPage { get { return this._readpage; } set { this._readpage = value; } }
		public string AddPage { get { return this._addpage; } set { this._addpage = value; } }
		public string EditPage { get { return this._editpage; } set { this._editpage = value; } }
		public string DeletePage { get { return this._deletepage; } set { this._deletepage = value; } }
		public string AdminPage { get { return this._adminpage; } set { this._adminpage = value; } }
	}
}
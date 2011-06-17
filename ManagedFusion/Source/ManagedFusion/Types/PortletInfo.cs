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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;
using ManagedFusion.Modules;

namespace ManagedFusion
{
	/// <summary>
	/// Summary description for Portlet.
	/// </summary>
	[Serializable]
	public class PortletInfo : PortalType
	{
		#region Static

		public static PortletCollection Collection
		{
			get { return Common.DatabaseProvider.Portlets; }
		}

		public static PortletInfo CreateNew ()
		{
			PortletInfo portlet = new PortletInfo(
				-1
				);

			portlet.SetState(State.Added);

			return portlet;
		}

		#endregion

		#region Fields

		private readonly int _id;
		private string _title;
		private Guid _moduleID;
		private DateTime _touched;

		#endregion

		#region Constructors

		[EditorBrowsable(EditorBrowsableState.Never)]
		public PortletInfo (int id, string title, Guid moduleID, DateTime touched)
		{
			this._id = id;
			this._title = title;
			this._moduleID = moduleID;
			this._touched = touched;

			// setup events
			this.SetupEvents();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public PortletInfo (int id, string title, PortletModule module, DateTime touched)
			: this(id, title, module.Identity, touched) { }

		private PortletInfo (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
			this._id = info.GetInt32("id");
			this._title = info.GetString("title");
			this._moduleID = new Guid(info.GetString("moduleID"));
			this._touched = info.GetDateTime("touched");

			// setup events
			this.SetupEvents();
		}

		private PortletInfo (int id)
		{
			this._id = id;
			this._touched = DateTime.Now;
		}

		private void SetupEvents ()
		{
		}

		#endregion

		#region Properties

		#region Properties In Database

		public override int Identity { get { return this._id; } }

		public string Title
		{
			get { return this._title; }
			set
			{
				this._title = value;
				this.ValueChanged();
			}
		}

		private Guid ModuleID
		{
			get { return this._moduleID; }
		}

		public override DateTime Touched
		{
			get { return this._touched; }
			set { this._touched = value; }
		}

		#endregion

		#region Properties Not In Database

		private bool _moduleChecked;
		private bool _enabled;

		public override bool Enabled
		{
			get
			{
				// check to see if the module was check
				if (_moduleChecked)
					return _enabled;

				// determine if this portlet should be enabled
				_enabled = this.Module != null;
				_moduleChecked = true;

				return _enabled;
			}
		}

		public PortletModule Module
		{
			get { return PortletModule.Collection[this._moduleID] as PortletModule; }
			set
			{
				this._moduleID = value.Identity;
				this.ValueChanged();
			}
		}

		private NameValueCollection _ModuleData;
		/// <summary>Get properties for portlets.</summary>
		/// <returns>Returns a collection of the properties.</returns>
		public NameValueCollection ModuleData
		{
			get
			{
				if (_ModuleData != null)
					return _ModuleData;

				this._ModuleData = Common.DatabaseProvider.GetModuleDataForPortlet(this);
				return this._ModuleData;
			}
		}

		private NameValueCollection _Properties;
		/// <summary>Get properties for portlets.</summary>
		/// <returns>Returns a collection of the properties.</returns>
		protected NameValueCollection Properties
		{
			get
			{
				if (_Properties != null)
					return _Properties;

				this._Properties = Common.DatabaseProvider.GetGeneralPropertiesForPortlet(this);
				return this._Properties;
			}
		}

		#endregion

		#endregion

		#region Methods

		# region Authorization

		public void AddRole (string role, Permissions permissions)
		{
			PortletSecurity.Provider.AddRoleToPortlet(role, permissions, this);

			// reset roles to refresh next time
			this._roles = null;
		}

		public void AddRole (string role, string[] permissions)
		{
			this.AddRole(role, (Permissions)Enum.Parse(typeof(Permissions), String.Join(", ", permissions), true));
		}

		public void UpdateRole (string role, Permissions permissions)
		{
			PortletSecurity.Provider.UpdateRoleForPortlet(role, permissions, this);

			// reset roles to refresh next time
			this._roles = null;
		}

		public void UpdateRole (string role, string[] permissions)
		{
			this.UpdateRole(role, (Permissions)Enum.Parse(typeof(Permissions), String.Join(", ", permissions), true));
		}

		public void RemoveRole (string role)
		{
			PortletSecurity.Provider.RemoveRoleFromPortlet(role, this);

			// reset roles to refresh next time
			this._roles = null;
		}

		private RolesPermissionsDictionary _roles;
		public RolesPermissionsDictionary Roles
		{
			get
			{
				if (this._roles != null)
					return this._roles;

				// get roles list for this section
				this._roles = PortletSecurity.Provider.GetAllRoles(this);
				return this._roles;
			}
		}

		public bool UserHasPermissions (Permissions p)
		{
			List<string> sc = this.GetRoles(p);

			foreach (string role in sc)
			{
				if (Common.Role.IsUserInRole(Common.Context.User.Identity.Name, role))
					return true;
			}

			return false;
		}

		/// <summary>Get the roles for selected permissions.</summary>
		/// <param name="permissions">The permissions to get the roles for.</param>
		/// <returns>Returns a list of roles for the <paramref name="permissions">permissions</paramref>.</returns>
		public List<string> GetRoles (Permissions permissions)
		{
			string key = String.Format("Portlet-Roles-{0}--{1}", this.Identity, permissions.ToString());

			// checks to see if the roles have been cached for these permissions
			// this is nessisary because RolesDictionary.GetRoles is a potentially
			// intensive operation
			if (Common.Cache.IsCached(key) == false) {
				List<string> rlist = this.Roles.GetRoles(permissions.ToString().Replace(", ", Common.Delimiter.ToString()).Split(Common.Delimiter));

				// add to cache
				Common.Cache.Add(key, rlist);
			}

			List<string> list = Common.Cache[key] as List<string>;

			// if it has no tasks is null then no tasks are returned
			return (list != null) ? list : new List<string>();
		}

		#endregion

		public int GetOrder (ContainerInfo container)
		{
			return Common.DatabaseProvider.GetContainerPortletLinkOrder(container, this);
		}

		protected override void CommitChangesToDatabase ()
		{
			Common.DatabaseProvider.CommitPortletChanges(this);
			Common.DatabaseProvider.ResetPortletCollection();
		}

		public override void GetObjectData (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("id", this._id);
			info.AddValue("title", this._title);
			info.AddValue("moduleID", this._moduleID.ToString());
			info.AddValue("touched", this._touched);
		}

		#endregion
	}
}
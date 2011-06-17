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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;
using ManagedFusion.Modules;
using ManagedFusion.Modules.Configuration;

namespace ManagedFusion
{
	/// <summary>
	/// The SectionInfo class represents all the information that is requested 
	/// by the PortalDesktop page on initialization to customize the over all page.
	/// </summary>
	[Serializable]
	public sealed class SectionInfo : PortalType
	{
		# region Static

		/// <summary></summary>
		public static SectionInfo Current { get { return (SectionInfo)Common.Context.Items["SectionInfo"]; } }

		public static SectionCollection Collection
		{
			get { return Common.DatabaseProvider.Sections; }
		}

		private static RootSection _Root = new RootSection();
		public static RootSection Root
		{
			get { return _Root; }
		}

		public static SectionInfo CreateNew ()
		{
			SectionInfo section = new SectionInfo(RootSection.Identity, CommunityInfo.NullIdentity);
			section.SetState(State.Added);

			return section;
		}

		public static SectionInfo CreateNew (SectionInfo parent)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");
			if (parent.Identity == TempIdentity)
				throw new ArgumentException("Must be commited to database before a child can be created.");
			if (parent.ConnectedCommunity == null)
				throw new ArgumentNullException("parent", "ConnectedCommunity must be set and commited to database before a child can be created.");
			if (parent.ConnectedCommunity.Identity == CommunityInfo.TempIdentity)
				throw new ArgumentException("ConnectedCommunity must be commited to database before a child can be created.");

			SectionInfo section = new SectionInfo(parent.Identity, parent.ConnectedCommunity.Identity);
			section.SetState(State.Added);

			return section;
		}

		public static SectionInfo FindSection(string rawUrl)
		{
			Uri url = new Uri(rawUrl);
			SiteInfo site = SiteInfo.GetSiteForHost(url.Host);
			string basePath = url.AbsolutePath;
			string[] pathSplit = basePath.Split(new string[] { PortalProperties.DefaultPage }, StringSplitOptions.RemoveEmptyEntries);
			return site.ConnectedSection.GetSectionForPath(pathSplit[1]);
		}

		#endregion

		#region Fields

		private readonly int _id;
		private string _name;			// name to display on buttons
		private string _title;			// title to display on browser
		private int _parentID;			// parent section id
		private int _order;
		private bool _visible;			// section is enabled for viewing
		private bool _syndicated;
		private string _originalTheme;
		private string _originalStyle;
		private string _owner;
		private Guid _moduleID;
		private DateTime _touched;
		private int _community_id;
		private int _community_idOld;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes the SectionInfo class by assigining values to its properties.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public SectionInfo (
			int id,
			string name,
			string title,
			int parentID,
			int order,
			bool visible,
			bool syndicated,
			string theme,
			string style,
			Guid moduleID,
			string owner,
			DateTime touched,
			int community_id
			)
		{
			if (id < 1)
				throw new ArgumentOutOfRangeException("id", id, "Identity must be greater than zero.");

			this._id = id;
			this._name = name;
			this._title = title;
			this._parentID = parentID;
			this._order = order;
			this._visible = visible;
			this._syndicated = syndicated;
			this._originalTheme = theme;
			this._originalStyle = style;
			this._moduleID = moduleID;
			this._owner = owner;
			this._touched = touched;
			this._community_id = community_id;
			this._community_idOld = this._community_id;

			// setup events
			this.SetupEvents();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public SectionInfo (
			int id,
			string name,
			string title,
			SectionInfo parent,
			int order,
			bool visible,
			bool syndicated,
			string theme,
			string style,
			SectionModule module,
			string owner,
			DateTime touched,
			CommunityInfo community
			)
			: this(
			id,
			name,
			title,
			(parent == null) ? RootSection.Identity : parent.Identity,
			order,
			visible,
			syndicated,
			theme,
			style,
			module.Identity,
			owner,
			touched,
			community.Identity
			)
		{
		}

		private SectionInfo (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
			this._id = info.GetInt32("id");
			this._name = info.GetString("name");
			this._title = info.GetString("title");
			this._parentID = info.GetInt32("parentID");
			this._order = info.GetInt32("order");
			this._visible = info.GetBoolean("visible");
			this._syndicated = info.GetBoolean("syndicated");
			this._originalTheme = info.GetString("originalTheme");
			this._originalStyle = info.GetString("originalStyle");
			this._owner = info.GetString("owner");
			this._moduleID = new Guid(info.GetString("moduleID"));
			this._touched = info.GetDateTime("touched");
			this._community_id = info.GetInt32("community_id");
			this._community_idOld = this._community_id;
		}

		private SectionInfo (int parentID, int community_id)
		{
			this._id = TempIdentity;
			this._parentID = parentID;
			this._visible = true;
			this._syndicated = true;
			this._originalTheme = ThemeInfo.Inherited;
			this._originalStyle = StyleInfo.Inherited;
			this._touched = DateTime.Now;
			this._community_id = community_id;
			this._community_idOld = this._community_id;

			// setup events
			this.SetupEvents();
		}

		private void SetupEvents ()
		{
			Common.DatabaseProvider.SitesChanged += new EventHandler(InvalidateExternalSitesCollections);
			Common.DatabaseProvider.CommunitiesChanged += new EventHandler(InvalidateExternalCommunityCollections);
			Common.DatabaseProvider.ContainersChanged += new EventHandler(InvalidateExternalContainersCollections);
		}

		#endregion

		#region Properties

		#region Properties In Database

		/// <summary>The Identity associated with the page.</summary>
		public override int Identity { get { return _id; } }

		/// <summary>A Pages Name.</summary>
		public string Name
		{
			get { return _name; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (this._name != value) {
					this._name = value;
					this.ValueChanged();
				}
			}
		}

		/// <summary>A Pages Title.</summary>
		public string Title
		{
			get
			{
				if (_title == null || _title.Length == 0)
					return _name;
				return _title;
			}
			set { this.OriginalTitle = value; }
		}

		/// <summary>The parent of the section.</summary>
		internal int ParentID { get { return this._parentID; } }

		/// <summary>The module that the section uses.</summary>
		private Guid ModuleID { get { return this._moduleID; } }

		/// <summary>The order of the section.</summary>
		public int Order
		{
			get { return this._order; }
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value", value, "The value must be great or equal to zero.");

				this._order = value;
				this.ValueChanged();
			}
		}

		/// <summary>Is the section visible?</summary>
		public bool Visible
		{
			get { return this._visible; }
			set
			{
				if (this._visible != value) {
					this._visible = value;
					this.ValueChanged();
				}
			}
		}

		/// <summary>Is the section syndicated?</summary>
		public bool Syndicated
		{
			get { return this._syndicated; }
			set
			{
				if (this._syndicated != value) {
					this._syndicated = value;
					this.ValueChanged();
				}
			}
		}

		public string OriginalTitle
		{
			get { return this._title; }
			set
			{
				if (this._title != value) {
					this._title = (value == null) ? String.Empty : value;
					this.ValueChanged();
				}
			}
		}

		/// <summary>The skin used in this section.</summary>
		public string OriginalTheme
		{
			get { return this._originalTheme; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (this._originalTheme != value) {
					this._originalTheme = value;
					this._theme = null;
					this._themeName = null;
					this.ValueChanged();
				}
			}
		}

		/// <summary>The style used in this section.</summary>
		public string OriginalStyle
		{
			get { return this._originalStyle; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (this._originalStyle != value) {
					this._originalStyle = value;
					this._style = null;
					this._styleName = null;
					this.ValueChanged();
				}
			}
		}

		/// <summary>The owner of this section.</summary>
		public string OriginalOwner
		{
			get { return this._owner; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (this._owner != value) {
					this._owner = value;
					this._owners = null;
					this.ValueChanged();
				}
			}
		}

		/// <summary>Last time the section was updated.</summary>
		public override DateTime Touched
		{
			get { return this._touched; }
			set { this._touched = value; }
		}

		#endregion

		#region Properties Not In Database

		private NameValueCollection _ModuleData;
		/// <summary></summary>
		public NameValueCollection ModuleData
		{
			get
			{
				if (_ModuleData != null)
					return this._ModuleData;

				this._ModuleData = Common.DatabaseProvider.GetModuleDataForSection(this);
				return this._ModuleData;
			}
		}

		private NameValueCollection _MetaProperties;
		/// <summary></summary>
		public NameValueCollection MetaProperties
		{
			get
			{
				if (_MetaProperties != null)
					return this._MetaProperties;

				this._MetaProperties = Common.DatabaseProvider.GetMetaPropertiesForSection(this);
				return this._MetaProperties;
			}
		}

		private NameValueCollection _Properties;
		/// <summary></summary>
		private NameValueCollection Properties
		{
			get
			{
				if (_Properties != null)
					return this._Properties;

				this._Properties = Common.DatabaseProvider.GetGeneralPropertiesForSection(this);
				return this._Properties;
			}
		}

		private List<string> _owners;
		/// <summary>The owners of this section.</summary>
		public List<string> Owners
		{
			get
			{
				if (this._owners != null)
					return this._owners;

				List<string> owners = new List<string>();

				SectionInfo parent = this.Parent;
				owners.Add(this.OriginalOwner);

				// keep working the way to primary parent to
				// create a list of owners
				while (parent != null) {
					// check to see if owner is already in list
					if (owners.Contains(parent.OriginalOwner) == false)
						owners.Add(parent.OriginalOwner);

					parent = parent.Parent;
				}

				this._owners = owners;
				return this._owners;
			}
		}

		/// <summary>The assembly content string.</summary>
		public SectionModule Module
		{
			get
			{
				ModuleInfo module = SectionModule.Collection[this._moduleID];

				// check to see if the module that was referenced is found
				// check to see if module is inherited from ModuleBase
				if (module == null || module.Type.IsSubclassOf(typeof(ModuleBase)) == false) {
					Common.Context.Trace.Warn("Module Failure", String.Format("Module, {0:B}, was not found.", this._moduleID));
					return SectionModule.ErrorModule;
				}

				return (SectionModule)module;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (this._moduleID != value.Identity) {
					this._moduleID = value.Identity;
					this.ValueChanged();
				}
			}
		}

		private ThemeInfo _theme;
		/// <summary></summary>
		public ThemeInfo Theme
		{
			get
			{
				if (this._theme != null)
					return this._theme;

				this._theme = ThemeInfo.Collection[ThemeName, this.ConnectedCommunity];
				return this._theme;
			}
		}

		private StyleInfo _style;
		/// <summary></summary>
		public StyleInfo Style
		{
			get
			{
				if (this._style != null)
					return this._style;

				this._style = Theme.Styles[StyleName];
				return this._style;
			}
		}

		private string _themeName;
		/// <summary>The skin used in this section.</summary>
		internal string ThemeName
		{
			get
			{
				if (this._themeName != null)
					return this._themeName;

				// check to see if this section uses an inherited skin
				// from parent
				if (this.OriginalTheme == ThemeInfo.Inherited) {
					SectionInfo parent = this.Parent;

					// check to see if parent exists
					if (parent != null) {
						// go up parent chain until theme is found
						while (parent.ThemeName == ThemeInfo.Inherited)
							parent = parent.Parent;

						this._themeName = parent.ThemeName;
					} else
						this._themeName = SiteInfo.Current.ThemeName;
				} else
					this._themeName = this.OriginalTheme;

				// if the theme is not contained in the collection use one of the defaults
				if (ThemeInfo.Collection.Contains(this._themeName, this.ConnectedCommunity) == false) {
					if (ThemeInfo.Collection.Contains(this.ConnectedCommunity.Config.DefaultTheme, this.ConnectedCommunity))
						this._themeName = this.ConnectedCommunity.Config.DefaultTheme;
					else
						this._themeName = Common.Configuration.Default.DefaultTheme;
				}


				return this._themeName;
			}
			set { this.OriginalTheme = value; }
		}

		private string _styleName;
		/// <summary>The style used in this section.</summary>
		internal string StyleName
		{
			get
			{
				if (this._styleName != null)
					return this._styleName;

				// check to see if this section uses an inherited style
				// from parent
				if (this.OriginalStyle == ThemeInfo.Inherited) {
					SectionInfo parent = this.Parent;

					// check to see if parent exists
					if (parent != null) {
						// go up parent chain until theme is found
						while (parent.StyleName == ThemeInfo.Inherited)
							parent = parent.Parent;

						this._styleName = parent.StyleName;
					} else
						this._styleName = SiteInfo.Current.StyleName;
				} else
					this._styleName = this.OriginalStyle;

				// if the style is not contained in the collection then use one of the defaults
				if (ThemeInfo.Collection.Contains(this._styleName) == false) {
					if (ThemeInfo.Collection.Contains(this.ConnectedCommunity.Config.DefaultStyle))
						this._styleName = this.ConnectedCommunity.Config.DefaultStyle;
					else
						this._styleName = Common.Configuration.Default.DefaultStyle;
				}

				return this._styleName;
			}
			set { this.OriginalStyle = value; }
		}

		private string _fullPath;
		/// <summary>The path to the section.</summary>
		private string FullPath
		{
			get
			{
				if (this._fullPath != null)
					return this._fullPath;

				Stack<string> pathList = new Stack<string>();

				SectionInfo parent = this.Parent;
				pathList.Push(this.Name);

				// keep working the way to primary parent to
				// create a list of names in path
				while (parent != null) {
					pathList.Push(parent.Name);
					parent = parent.Parent;
				}

				// remove primary parent from list because it is the root
				pathList.Pop();

				// create path
				this._fullPath = ("/" + String.Join("/", pathList.ToArray()) + "/").Replace("//", "/");
				return this._fullPath;
			}
		}

		/// <summary>The Uri to this section.</summary>
		public Uri UrlPath
		{
			get { return Common.Path.GetPortalUrl(this.Path); }
		}

		/// <summary>The Path to the section that is normalized depending on entry point through Portal.</summary>
		public string Path
		{
			get
			{
				return String.Concat("/",
					FullPath.Remove(0, SiteInfo.Current.ConnectedSection.FullPath.Length),
					"/").Replace("//", "/");
			}
		}

		/// <summary>The parent of the current section.</summary>
		public SectionInfo Parent
		{
			get { return Collection[this._parentID]; }
			set
			{
				if (value != null && value.Identity == TempIdentity)
					throw new ArgumentException("Must be commited to database before it can be connected.");

				if ((this._parentID == RootSection.Identity && value != null)
					&& this._parentID != value.Identity) {
					this._parentID = (value == null) ? RootSection.Identity : value.Identity;
					this.ValueChanged();
				}
			}
		}

		public void AddContainer (ContainerInfo container, int order, int position)
		{
			// add container to this section
			Common.DatabaseProvider.AddSectionContainerLink(this, container, order, position);

			// reset containers collection
			this._Containers = null;
		}

		public void UpdateContainer (ContainerInfo container, int order, int position)
		{
			// update container to this section
			Common.DatabaseProvider.UpdateSectionContainerLink(this, container, order, position);
		}

		public void RemoveContainer (ContainerInfo container)
		{
			// remove container from this section
			Common.DatabaseProvider.RemoveSectionContainerLink(this, container);

			// reset containers collection
			this._Containers = null;
		}

		private ContainerCollection _Containers;
		public ContainerCollection Containers
		{
			get
			{
				if (this._Containers != null)
					return this._Containers;

				List<ContainerInfo> list = new List<ContainerInfo>();

				// _Containers was null populate the variable
				foreach (int id in Common.DatabaseProvider.GetContainersForSection(this))
					list.Add(ContainerInfo.Collection[id]);

				this._Containers = new ContainerCollection(list.ToArray(), this);
				return this._Containers;
			}
		}

		private SectionCollection _Children;
		/// <summary>The children of the current section.</summary>
		public SectionCollection Children
		{
			get
			{
				if (this._Children != null)
					return this._Children;

				List<SectionInfo> list = new List<SectionInfo>();

				foreach (SectionInfo section in Collection) {
					if (section.ParentID == this.Identity)
						list.Add(section);
				}

				this._Children = new SectionCollection(list.ToArray());

				return this._Children;
			}
		}

		private SiteCollection _ConnectedSites;
		public SiteCollection ConnectedSites
		{
			get
			{
				if (this._ConnectedSites != null)
					return this._ConnectedSites;

				List<SiteInfo> list = new List<SiteInfo>();

				foreach (SiteInfo site in SiteInfo.Collection) {
					if (site.ConnectedSection.Identity == this.Identity)
						list.Add(site);
				}

				this._ConnectedSites = new SiteCollection(list.ToArray());
				return this._ConnectedSites;
			}
		}

		private CommunityInfo _ConnectedCommunity;
		/// <summary>Get and set Connected Community</summary>
		public CommunityInfo ConnectedCommunity
		{
			get
			{
				if (this._ConnectedCommunity == null)
					this._ConnectedCommunity = CommunityInfo.Collection[this._community_id];

				return this._ConnectedCommunity;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				if (value.Identity == CommunityInfo.TempIdentity)
					throw new ArgumentException("Must be commited to database before it can be connected.");
				if (this.Parent != null)
					throw new ArgumentException("Only sections that have a parent that is the root (or null (or Nothing)) can be changed.");

				if (this._community_id != value.Identity) {
					this.SetConnectedCommunity(value);
				}
			}
		}

		/// <summary>This is to by-pass the <see cref="Resources"/> thrown from set_ConnectedCommunity.</summary>
		/// <param name="community"></param>
		private void SetConnectedCommunity (CommunityInfo community)
		{
			this._ConnectedCommunity = community;
			this._community_id = community.Identity;
			this.ValueChanged();

			// set the community for all children
			foreach (SectionInfo child in this.Children)
				child.SetConnectedCommunity(community);
		}

		#endregion

		#endregion

		#region Methods

		# region Authorization

		public void AddRole (string role, string[] tasks)
		{
			SectionSecurity.Provider.AddRoleToSection(role, tasks, this);

			// reset roles to refresh next time
			this._roles = null;
		}

		public void UpdateRole (string role, string[] tasks)
		{
			SectionSecurity.Provider.UpdateRoleForSection(role, tasks, this);

			// reset roles to refresh next time
			this._roles = null;
		}

		public void RemoveRole (string role)
		{
			SectionSecurity.Provider.RemoveRoleFromSection(role, this);

			// reset roles to refresh next time
			this._roles = null;
		}

		private RolesTasksDictionary _roles;
		public RolesTasksDictionary Roles
		{
			get
			{
				if (this._roles != null)
					return this._roles;

				// get roles list for this section
				this._roles = SectionSecurity.Provider.GetAllRoles(this);
				return this._roles;
			}
		}

		public bool IsAccessibleToUser(HttpContext context)
		{
			return UserInTasks(new string[] { ConfigurationTask.ViewPageName }, context);
		}

		public bool UserHasPermissions (Permissions p)
		{
			return UserHasPermissions(p, Common.Context);
		}

		public bool UserHasPermissions(Permissions p, HttpContext context)
		{
			return UserInTasks(this.GetTasks(p).ToArray(), context);
		}

		public bool UserInTasks(string[] tasks)
		{
			return UserInTasks(tasks, Common.Context);
		}

		public bool UserInTasks(string[] tasks, HttpContext context)
		{
			if (context == null) throw new ArgumentNullException("context");

			// default user to not authenticated in case context user doesn't exisit
			string user = PortalRole.NotAuthenticated.ToString();

			// set the user name if it exisits
			if (context.User != null && context.User.Identity != null)
				user = context.User.Identity.Name;

			string[] roles = this.GetRoles(tasks).ToArray();
			foreach (string role in roles)
				if (Common.Role.IsUserInRole(user, role))
					return true;

			return false;
		}

		/// <summary>Get the tasks for selected <see cref="Permissions"/>.</summary>
		/// <param name="permissions">The permissions to get the tasks for.</param>
		/// <returns>Returns a list of tasks for the <paramref name="permissions" />.</returns>
		public List<string> GetTasks (Permissions permissions)
		{
			string key = String.Format("Section-Tasks-{0}--{1}", this.Identity, permissions.ToString());

			// checks to see if the tasks have been cached for these permissions
			// this is nessisary because this opperation is a potentially
			// intensive operation
			if (Common.Cache.IsCached(key) == false) {
				List<string> tlist = new List<string>();

				// add the view page task if it has the permissions
				// this is default for all sections
				ConfigurationTask viewPage = ConfigurationTask.ViewPageTask;
				if (viewPage.CheckAccess(permissions))
					tlist.Add(viewPage.Name);

				// go through each task and check the access
				foreach (ConfigurationTask task in this.Module.Config.Tasks) {
					if (task.CheckAccess(permissions))
						tlist.Add(task.Name);
				}

				// add to cache
				Common.Cache.Add(key, tlist);
			}

			List<string> list = Common.Cache[key] as List<string>;

			// if it has no tasks is null then no tasks are returned
			return (list != null) ? list : new List<string>();
		}

		/// <summary>Get the roles for selected tasks.</summary>
		/// <param name="tasks">The tasks to get the roles for.</param>
		/// <returns>Returns a list of roles for the <paramref name="tasks" />.</returns>
		public List<string> GetRoles (string[] tasks)
		{
			string key = String.Format("Section-Roles-{0}--{1}", this.Identity, tasks.GetHashCode());

			// checks to see if the roles have been cached for these tasks
			// this is nessisary because RolesDictionary.GetRoles is a potentially
			// intensive operation
			if (Common.Cache.IsCached(key) == false) {
				List<string> rlist = this.Roles.GetRoles(tasks);

				// add to cache
				Common.Cache.Add(key, rlist);
			}

			List<string> list = Common.Cache[key] as List<string>;

			// if it has no tasks is null then no roles are returned
			return (list != null) ? list : new List<string>();
		}

		#endregion

		#region Utilities

		public SectionInfo GetSectionForPath (string path)
		{
			// return if the module is Traversable
			if (this.Module.IsUrlPathTraversable)
				return this;

			// removes init whack if exception occures return this SectionInfo
			try {
				if (path[0] == '/')
					path = path.Substring(1);
			} catch (IndexOutOfRangeException) {
				return this;
			}

			// if there is nothing left return this SectionInfo
			if (path.Length == 0)
				return this;

			string[] split = path.Split(new char[] { '/' }, 2);

			// if the split URL doesn't get split
			if (split.Length != 2)
				return this;

			return this.Children[split[0]].GetSectionForPath(split[1]);
		}

		#endregion

		#region Invalidate Data Events

		private void InvalidateExternalSitesCollections (object sender, EventArgs e)
		{
			this._ConnectedSites = null;
		}

		private void InvalidateExternalContainersCollections (object sender, EventArgs e)
		{
			this._Containers = null;
		}

		private void InvalidateExternalCommunityCollections (object sender, EventArgs e)
		{
			this._ConnectedCommunity = null;
		}

		#endregion

		#region Commit Changes

		protected override void CommitChangesToDatabase ()
		{
			// if the community has changed, that means that other communities have changed
			// and all changes need to be commited, instead of just this section
			if (this._community_id != this._community_idOld) {
				Collection.CommitChanges();
			} else {
				Common.DatabaseProvider.CommitSectionChanges(this);
				Common.DatabaseProvider.ResetSectionCollection();
			}
		}

		#endregion

		public override void GetObjectData (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("id", this._id);
			info.AddValue("name", this._name);
			info.AddValue("title", this._title);
			info.AddValue("parentID", this._parentID);
			info.AddValue("order", this._order);
			info.AddValue("syndicated", this._syndicated);
			info.AddValue("visible", this._visible);
			info.AddValue("originalTheme", this._originalTheme);
			info.AddValue("originalStyle", this._originalStyle);
			info.AddValue("owner", this._owner);
			info.AddValue("moduleID", this._moduleID.ToString());
			info.AddValue("touched", this._touched);
			info.AddValue("community_id", this._community_id);
		}

		/// <summary>Writes the object to a string.</summary>
		/// <returns>Returns the name of the page.</returns>
		public override string ToString ()
		{
			return this.Name;
		}

		#endregion

		#region Operators

		/// <summary>Checks to see if parent has the child in its tree.</summary>
		/// <param name="child">The child to compare.</param>
		/// <param name="parent">The parent that is check for a child.</param>
		/// <param name="compare">Compare the parent and child to see if they are the same section.</param>
		/// <returns>Returns <see langword="true"/> the parent contains the child or are equal depending on the compare value, and <see langword="false"/> if the parent does not contain the child.</returns>
		private static bool IsChildFromParent (SectionInfo child, SectionInfo parent, bool compare)
		{
			// if the child is equal to the parent return true
			if (compare && child == parent)
				return true;

			// check all the parents children to see if they contain or are the children
			foreach (SectionInfo section in parent.Children) {
				if (IsChildFromParent(child, section, true))
					return true;
			}

			// the child was not found in the parents tree
			return false;
		}

		/// <summary>Checks to see if the first operand is the parent of the second operand.</summary>
		/// <param name="parent">Parent</param>
		/// <param name="child">Child</param>
		/// <remarks><note>A <see langword="null"/> object is assumed to be the <see cref="RooSection"/> and cannot be the child of any <see cref="SectionInfo"/>.</note></remarks>
		/// <returns>Returns <see langword="true"/> the parent contains the child, and <see langword="false"/> if the parent does not contain the child.</returns>
		public static bool operator > (SectionInfo parent, SectionInfo child)
		{
			// child cannot be root, root cannot be a child of any section
			if (child == null)
				return false;
			// parent is the root, so everything not null is a child of it
			if (parent == null)
				return true;

			return IsChildFromParent(child, parent, false);
		}

		/// <summary>Checks to see if the first operand is the child of the second operand.</summary>
		/// <param name="child">Child</param>
		/// <param name="parent">Parent</param>
		/// <remarks><note>A <see langword="null"/> object is assumed to be the <see cref="RooSection"/> and cannot be the child of any <see cref="SectionInfo"/>.</note></remarks>
		/// <returns>Returns <see langword="true"/> the parent contains the child, and <see langword="false"/> if the parent does not contain the child.</returns>
		public static bool operator < (SectionInfo child, SectionInfo parent)
		{
			// child cannot be root, root cannot be a child of any section
			if (child == null)
				return false;
			// parent is the root, so everything not null is a child of it
			if (parent == null)
				return true;

			return IsChildFromParent(child, parent, false);
		}

		/// <summary>Checks to see if the first operand is the parent or the same as the second operand.</summary>
		/// <param name="parent">Parent</param>
		/// <param name="child">Child or Same <see cref="SectionInfo"/></param>
		/// <remarks><note>A <see langword="null"/> object is assumed to be the <see cref="RooSection"/> and cannot be the child of any <see cref="SectionInfo"/>.</note></remarks>
		/// <returns>Returns <see langword="true"/> the parent contains the child or are equal, and <see langword="false"/> if the parent does not contain the child.</returns>
		public static bool operator >= (SectionInfo parent, SectionInfo child)
		{
			// child cannot be root, root cannot be a child of any section
			if (child == null)
				return false;
			// parent is the root, so everything not null is a child of it
			if (parent == null)
				return true;

			return IsChildFromParent(child, parent, true);
		}

		/// <summary>Checks to see if the first operand is the child or the same as the second operand.</summary>
		/// <param name="child">Child</param>
		/// <param name="parent">Parent or Same <see cref="SectionInfo"/></param>
		/// <remarks><note>A <see langword="null"/> object is assumed to be the <see cref="RooSection"/> and cannot be the child of any <see cref="SectionInfo"/>.</note></remarks>
		/// <returns>Returns <see langword="true"/> the parent contains the child or are equal, and <see langword="false"/> if the parent does not contain the child.</returns>
		public static bool operator <= (SectionInfo child, SectionInfo parent)
		{
			// child cannot be root, root cannot be a child of any section
			if (child == null)
				return false;
			// parent is the root, so everything not null is a child of it
			if (parent == null)
				return true;

			return IsChildFromParent(child, parent, true);
		}

		#endregion

		#region SiteMap

		public static implicit operator SiteMapNode (SectionInfo m)
		{
			return new SiteMapNode(
				SiteMap.Provider,
				m.Identity.ToString(),
				m.UrlPath.ToString(),
				m.Name,
				m.Title,
				m.Roles.GetRoles(null),
				m.MetaProperties,
				new NameValueCollection(),
				String.Empty
				);
		}

		public static explicit operator SectionInfo (SiteMapNode m)
		{
			return FindSection(m.Url);
		}

		#endregion
	}
}
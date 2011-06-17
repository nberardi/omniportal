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
using System.Web;
using System.ComponentModel;

namespace ManagedFusion
{
	/// <summary>
	/// The SiteInfo class represents all the data associated with a single site.
	/// </summary>
	[Serializable]
	public sealed class SiteInfo : PortalType
	{
		#region Static

		public static SiteCollection Collection
		{
			get { return Common.DatabaseProvider.Sites; }
		}

		public static SiteInfo CreateNew ()
		{
			SiteInfo site = new SiteInfo();
			site.SetState(State.Added);

			return site;
		}

		#endregion

		#region Fields

		private readonly int _id;
		private string _domain;			// can be * or domain
		private string _subDomain;		// can be * or sub-domain
		private DateTime _touched;		// date and time of creation in database
		private int _section_id;
		private string _originalTheme;
		private string _originalStyle;

		#endregion

		#region Constructors

		[EditorBrowsable(EditorBrowsableState.Never)]
		public SiteInfo (int id, string domain, string subDomain, DateTime touched, int connectedSection, string theme, string style)
		{
			if (id < 1)
				throw new ArgumentOutOfRangeException("id", id, "Identity must be greater than zero.");

			this._id = id;
			this._domain = (domain == null) ? "*" : domain;
			this._subDomain = (subDomain == null) ? "*" : subDomain;
			this._touched = (touched != DateTime.MinValue && touched != DateTime.MaxValue) ? touched : DateTime.Now;
			this._section_id = connectedSection;
			this._originalTheme = theme;
			this._originalStyle = style;

			// setup events
			this.SetupEvents();
		}

		private SiteInfo (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
			this._id = info.GetInt32("id");
			this._domain = info.GetString("domain");
			this._subDomain = info.GetString("subDomain");
			this._touched = info.GetDateTime("touched");
			this._section_id = info.GetInt32("section_id");
			this._originalTheme = info.GetString("originalTheme");
			this._originalStyle = info.GetString("originalStyle");
		}

		private SiteInfo ()
		{
			this._id = TempIdentity;
			this._domain = "*";
			this._subDomain = "*";
			this._section_id = SectionInfo.NullIdentity;
			this._originalTheme = ThemeInfo.Inherited;
			this._originalStyle = ThemeInfo.Inherited;
			this._touched = DateTime.Now;

			// setup events
			this.SetupEvents();
		}

		private void SetupEvents ()
		{
			Common.DatabaseProvider.SectionsChanged += new EventHandler(InvalidateExternalSectionsCollections);
		}

		#endregion

		#region Properties

		#region Properties In Database

		/// <summary>Gets Site Identity</summary>
		public override int Identity
		{
			get { return this._id; }
		}

		/// <summary>Get and sets Site Domain</summary>
		public string Domain
		{
			get { return this._domain; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (this._domain != value) {
					this._domain = value;
					this.ValueChanged();
				}
			}
		}

		/// <summary>Get and set Site SubDomain</summary>
		public string SubDomain
		{
			get { return this._subDomain; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (this._subDomain != value) {
					this._subDomain = value;
					this.ValueChanged();
				}
			}
		}

		/// <summary>The skin used in this site.</summary>
		public string OriginalTheme
		{
			get { return this._originalTheme; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				this._originalTheme = value;
				this._theme = null;
				this._themeName = null;
				this.ValueChanged();
			}
		}

		/// <summary>The style used in this site.</summary>
		public string OriginalStyle
		{
			get { return this._originalStyle; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				this._originalStyle = value;
				this._style = null;
				this._styleName = null;
				this.ValueChanged();
			}
		}

		/// <summary>Get last touched date.</summary>
		public override DateTime Touched
		{
			get { return this._touched; }
			set { this._touched = value; }
		}

		#endregion

		#region Properties Not In Database

		private SectionInfo _ConnectedSection;
		/// <summary>Get and set Connected Section</summary>
		public SectionInfo ConnectedSection
		{
			get
			{
				if (this._ConnectedSection == null)
					this._ConnectedSection = SectionInfo.Collection[this._section_id];

				return this._ConnectedSection;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				if (value.Identity == SectionInfo.TempIdentity)
					throw new ArgumentException("Must be commited to database before it can be connected.");

				this._ConnectedSection = value;
				this._section_id = value.Identity;
				this.ValueChanged();
			}
		}

		/// <summary>Get and set Connected Community</summary>
		public CommunityInfo ConnectedCommunity
		{
			get
			{
				// check for a connected section
				if (this.ConnectedSection != null)
					return this.ConnectedSection.ConnectedCommunity;

				// no section has been conntected so no community has been connected
				return null;
			}
		}

		public string FullDomain
		{
			get { return String.Concat(this.SubDomain, ".", this.Domain); }
		}

		private ThemeInfo _theme;
		/// <summary></summary>
		public ThemeInfo Theme
		{
			get
			{
				if (this._theme != null)
					return this._theme;

				this._theme = ThemeInfo.Collection[this.ThemeName];
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

				this._style = Theme.Styles[this.StyleName];
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

				// if the theme is inherited
				if (this.OriginalTheme == ThemeInfo.Inherited) {
					// if the a community is connected
					if (this.ConnectedCommunity != null) {
						this._themeName = this.ConnectedCommunity.Config.DefaultTheme;
					} else {
						this._themeName = Common.Configuration.Default.DefaultTheme;
					}
				} else {
					this._themeName = this.OriginalTheme;
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

				// if the style is inherited
				if (this.OriginalTheme == ThemeInfo.Inherited) {
					// if the a community is connected
					if (this.ConnectedCommunity != null) {
						this._styleName = this.ConnectedCommunity.Config.DefaultStyle;
					} else {
						this._styleName = Common.Configuration.Default.DefaultStyle;
					}
				} else {
					this._styleName = this.OriginalTheme;
				}

				return this._styleName;
			}
			set { this.OriginalStyle = value; }
		}

		#endregion

		#endregion

		#region Methods

		#region Get Site For Current URL

		/// <summary>
		/// Gets the <see cref="ManagedFusion.Site.SiteInfo">SiteInfo</see> according to the
		/// host of the current URL request.
		/// </summary>
		/// <returns>Returns the site information from the database.</returns>
		public static SiteInfo Current
		{
			get { return GetSiteForHost(Common.Context); }
		}

		/// <summary>
		/// Gets the <see cref="ManagedFusion.Site.SiteInfo">SiteInfo</see> according to the
		/// host of the current URL request.
		/// </summary>
		/// <returns>Returns the site information from the database.</returns>
		public static SiteInfo GetSiteForHost(HttpContext context)
		{
			// check to see if the context has already been created with the current site
			if (context.Items["SiteInfo"] != null)
				return (SiteInfo)context.Items["SiteInfo"];

			// get host for current portal
			string host = context.Request.Url.Host;

			return GetSiteForHost(host);
		}

		/// <summary>
		/// Gets the <see cref="ManagedFusion.Site.SiteInfo">SiteInfo</see> according to the
		/// host of the current URL request.
		/// </summary>
		/// <returns>Returns the site information from the database.</returns>
		/// <exception cref="ManagedFusion.SiteNotFoundException">Thrown when the current host is not found in sites.</exception>
		public static SiteInfo GetSiteForHost(string host)
		{
			// make sure the host is lowercase
			host = host.ToLower();

			// sets the portal key used for cache
			string siteKey = host;

			// see if portal is in cache
			if (Common.Cache.IsCached(siteKey, String.Empty) == true)
				return (SiteInfo)Common.Cache[siteKey, String.Empty];

			// get host parts
			string[] parts = host.Split('.');

			// get portal information
			string domain = String.Empty;
			string subDomain = "*";

			// get domain and subdomain
			switch (parts.Length)
			{
				case 3:		// in the form of subDomain.domain.com
					domain = String.Format("{0}.{1}", parts[1], parts[2]);
					subDomain = parts[0];
					break;
				case 4:
					domain = String.Concat(parts[1], ".", parts[2], ".", parts[3]);
					subDomain = parts[0];
					break;
				default:
					domain = host;
					break;
			}

			// get all site combinations
			SiteInfo[] sites = new SiteInfo[4];

			sites[0] = SiteInfo.Collection[subDomain + "." + domain];
			sites[1] = SiteInfo.Collection["*." + domain];
			sites[2] = SiteInfo.Collection[subDomain + ".*"];
			sites[3] = SiteInfo.Collection["*.*"];

			// create site info object
			SiteInfo cacheValue = null;

			foreach (SiteInfo site in sites)
				if (site != null)
				{
					cacheValue = site;
					break;
				}

			if (cacheValue == null)
				throw new ApplicationException("A site matching '" + host + "' could not be found.");

			// adds the portal to the cache
			Common.Cache.Add(siteKey, String.Empty, cacheValue);

			return cacheValue;
		}

		#endregion

		#region Invalidate

		private void InvalidateExternalSectionsCollections (object sender, EventArgs e)
		{
			this._ConnectedSection = null;
		}

		#endregion

		protected override void CommitChangesToDatabase ()
		{
			Common.DatabaseProvider.CommitSiteChanges(this);
			Common.DatabaseProvider.ResetSiteCollection();
		}

		/// <summary>Writes the object to a string.</summary>
		/// <returns>Returns the name of the page.</returns>
		public override string ToString ()
		{
			return this.FullDomain;
		}

		public override void GetObjectData (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("id", this._id);
			info.AddValue("domain", this._domain);
			info.AddValue("subDomain", this._subDomain);
			info.AddValue("touched", this._touched);
			info.AddValue("section_id", this._section_id);
			info.AddValue("originalTheme", this._originalTheme);
			info.AddValue("originalStyle", this._originalStyle);
		}

		#endregion
	}
}
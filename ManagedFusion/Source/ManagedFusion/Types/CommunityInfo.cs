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
using System.IO;
using System.Collections.Specialized;
using System.ComponentModel;

// ManagedFusion Classes
using ManagedFusion.Configuration;

namespace ManagedFusion
{
	/// <summary>
	/// The CommunityInfo class represents all the information, for the site, 
	/// that is requested by the DesktopDefault page on initialization.  It represents
	/// the current site.
	/// </summary>
	[Serializable]
	public sealed class CommunityInfo : PortalType
	{
		#region Static

		/// <summary></summary>
		public static CommunityInfo Current { get { return Common.Context.Items["CommunityInfo"] as CommunityInfo; } }

		/// <summary></summary>
		public static CommunityCollection Collection
		{
			get { return Common.DatabaseProvider.Communities; }
		}

		/// <summary></summary>
		/// <returns></returns>
		public static CommunityInfo CreateNew ()
		{
			CommunityInfo portal = new CommunityInfo();
			portal.SetState(State.Added);

			return portal;
		}

		/// <summary></summary>
		/// <param name="portal"></param>
		public static void AddCommunity (CommunityInfo community)
		{
			Collection.Add(community);
		}

		/// <summary></summary>
		/// <param name="portal"></param>
		public static void RemoveCommunity (CommunityInfo community)
		{
			Collection.Remove(community);
		}

		#endregion

		#region Fileds

		private readonly int _id;
		private readonly Guid _universalID;
		private string _title;
		private DateTime _touched;
		private string _head;

		#endregion

		#region Constructors

		[EditorBrowsable(EditorBrowsableState.Never)]
		public CommunityInfo (int id, Guid universalID, string title, DateTime touched)
		{
			if (id < 1)
				throw new ArgumentOutOfRangeException("id", id, "Identity must be greater than zero.");

			if (title == null || title.Length == 0)
				throw new ArgumentNullException("title");

			this._id = id;
			this._universalID = universalID;
			this._title = title;
			this._touched = touched;

			// setup events
			this.SetupEvents();
		}

		private CommunityInfo (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
			this._id = info.GetInt32("id");
			this._universalID = new Guid(info.GetString("universalID"));
			this._title = info.GetString("title");
			this._touched = info.GetDateTime("touched");

			// setup events
			this.SetupEvents();
		}

		private CommunityInfo ()
		{
			this._id = TempIdentity;
			this._universalID = Guid.NewGuid();
			this._touched = DateTime.Now;

			// setup events
			this.SetupEvents();
		}

		private void SetupEvents ()
		{
		}

		#endregion

		#region Properties

		/// <summary>The Identity associated with the portal.</summary>
		public override int Identity
		{
			get { return _id; }
		}

		/// <summary>The Universal Identity associated with the portal.</summary>
		public Guid UniversalID
		{
			get { return this._universalID; }
		}

		/// <summary>A Pages Title.</summary>
		public string Title
		{
			get { return _title; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (this._title != value) {
					this._title = value;
					this.ValueChanged();
				}
			}
		}

		/// <summary>The date the site was last updated.</summary>
		public override DateTime Touched
		{
			get { return this._touched; }
			set { this._touched = value; }
		}

		/// <summary></summary>
		public string HeaderText
		{
			get
			{
				if (this._head != null)
					return this._head;

				string path;

				// get the path for /Communities/{Identity}/Head.html and if it does
				// not exist then return an empty string
				if (Common.Path.VerifyCommunityPath(this.Identity, PortalProperties.HeaderFile, out path) == false)
					return String.Empty;

				// read the file from the path
				using (StreamReader reader = File.OpenText(Common.Path.GetAbsoluteDiskPath(path))) {
					// set the head
					this._head = reader.ReadToEnd();
					reader.Close();
				}

				return this._head;
			}
		}

		private NameValueCollection _Properties;
		/// <summary></summary>
		public NameValueCollection Properties
		{
			get
			{
				if (_Properties != null)
					return this._Properties;

				this._Properties = Common.DatabaseProvider.GetGeneralPropertiesForCommunity(this);
				return this._Properties;
			}
		}

		private CommunityThemeCollection _Themes;
		/// <summary></summary>
		public CommunityThemeCollection Themes
		{
			get
			{
				if (this._Themes != null)
					return this._Themes;

				this._Themes = ThemeInfo.Collection.GetThemes(this, true);
				return this._Themes;
			}
		}

		private CommunityThemeCollection _OnlyCommunityThemes;
		/// <summary></summary>
		public CommunityThemeCollection OnlyCommunityThemes
		{
			get
			{
				if (this._OnlyCommunityThemes != null)
					return this._OnlyCommunityThemes;

				this._OnlyCommunityThemes = ThemeInfo.Collection.GetThemes(this, false);
				return this._OnlyCommunityThemes;
			}
		}

		private ThemeInfo _DefaultTheme;
		/// <summary></summary>
		public ThemeInfo DefaultTheme
		{
			get
			{
				if (this._DefaultTheme != null)
					return this._DefaultTheme;

				this._DefaultTheme = ThemeInfo.Collection[Config.DefaultTheme, this];
				return this._DefaultTheme;
			}
		}

		/// <summary></summary>
		public CommunityConfiguration Config
		{
			get { return Common.Configuration.Current; }
		}

		#endregion

		#region Methods

		/// <summary>Writes the object to a string.</summary>
		/// <returns>Returns the name of the page.</returns>
		public override string ToString ()
		{
			return this.Title;
		}

		protected override void CommitChangesToDatabase ()
		{
			Common.DatabaseProvider.CommitPortalChanges(this);
			Common.DatabaseProvider.ResetCommunityCollection();
		}

		public override void GetObjectData (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("id", this._id);
			info.AddValue("universalID", this._universalID.ToString());
			info.AddValue("title", this._title);
			info.AddValue("touched", this._touched);
		}

		#endregion
	}
}
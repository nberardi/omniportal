using System;
using System.IO;
using System.Collections;

namespace ManagedFusion.Types
{
	/// <summary>
	/// Summary description for ThemeInfo.
	/// </summary>
	public class ThemeInfo
	{
		#region Themes/Styles

		// directories to be excluded
		private static string[] _excludedDirectories = new string[] {"CVS"};

		/// <summary>
		/// The theme should inherit it's parents theme or style.
		/// </summary>
		public const string Inherited = "Inherited";

		/// <summary>
		/// No theme has been set for the section.
		/// </summary>
		public const string NoTheme = "No Theme";

		/// <summary>
		/// A list of themes that are handled at the system level.
		/// </summary>
		public static string[] SystemThemes 
		{
			get { return new string[2] { Inherited, NoTheme }; }
		}

		#region Themes

		/// <summary>
		/// Get availiable themes in the Themes folder.
		/// </summary>
		/// <returns>Returns a string array of all themes availiable.</returns>
		private static string[] GetThemes (CommunityInfo community) 
		{
			// community theme directory reference
			DirectoryInfo themeDirectory = new DirectoryInfo(
				Global.Path.GetAbsoluteDiskPath(Global.Path.GetCommunityThemeDirectory(community.ID))
				);

			ArrayList themes = new ArrayList();

			// gets all the themes in the default directory
			foreach(DirectoryInfo dir in themeDirectory.GetDirectories()) 
				themes.Add(dir.Name);

			// remove directories to be excluded
			foreach(string s in _excludedDirectories)
				themes.Remove(s);

			// returns the list of themes
			return (string[])themes.ToArray(typeof(string));
		}

		private static string[] GetDefaultThemes () 
		{
			// default theme directory reference
			DirectoryInfo themeDirectory = new DirectoryInfo(
				Global.Path.GetAbsoluteDiskPath(Global.Path.GetDefaultThemeDirectory())
				);

			ArrayList themes = new ArrayList();

			// gets all the themes in the community directory
			foreach(DirectoryInfo dir in themeDirectory.GetDirectories())
				themes.Add(dir.Name);

			// remove directories to be excluded
			foreach(string s in _excludedDirectories)
				themes.Remove(s);

			// returns the list of themes
			return (string[])themes.ToArray(typeof(string));
		}

		#endregion

		#region Styles

		/// <summary>
		/// Get availiable styles for the passed theme.
		/// </summary>
		/// <param name="theme">The theme to get all the styles for.</param>
		/// <returns>Returns a string array of all styles availiable.</returns>
		private string[] GetStyles (string theme) 
		{
			// community theme directory reference
			DirectoryInfo stylesDirectory = new DirectoryInfo(
				Global.Path.GetAbsoluteDiskPath(Global.Path.GetCommunityStyleDirectory(this.CommunityID, theme))
				);

			if (stylesDirectory == null)
				return new string[0];

			ArrayList styles = new ArrayList();

			// gets all the themes in the directory
			foreach(FileInfo file in stylesDirectory.GetFiles("*.css"))
				styles.Add(file.Name.Substring(0, file.Name.LastIndexOf('.')));

			// returns the list of themes
			return (string[])styles.ToArray(typeof(string));
		}

		private string[] GetDefaultStyles (string theme) 
		{
			// community theme directory reference
			DirectoryInfo stylesDirectory = new DirectoryInfo(
				Global.Path.GetAbsoluteDiskPath(Global.Path.GetDefaultStyleDirectory(theme)))
				;

			if (stylesDirectory == null)
				return new string[0];

			ArrayList styles = new ArrayList();

			// gets all the themes in the directory
			foreach(FileInfo file in stylesDirectory.GetFiles("*.css"))
				styles.Add(file.Name.Substring(0, file.Name.LastIndexOf('.')));

			// returns the list of themes
			return (string[])styles.ToArray(typeof(string));
		}

		#endregion

		#endregion

		#region Static

		private static ThemeCollection _collection;
		
		static ThemeInfo()
		{
			ArrayList list = new ArrayList();

			// get the themes for each community
			foreach(CommunityInfo community in CommunityInfo.Collection)
				foreach(string themeName in GetThemes(community)) 
					list.Add(new ThemeInfo(community.ID, themeName));

			// get the default themes
			foreach(string themeName in GetDefaultThemes())
				list.Add(new ThemeInfo(themeName));

			// add to the collection
			_collection = new ThemeCollection(list.ToArray(typeof(ThemeInfo)) as ThemeInfo[]);
		}

		public static ThemeCollection Collection 
		{
			get { return _collection; }
		}

		#endregion

		#region Fields

		private bool _defaultTheme;
		private int _communityID;
		private string _name;
		private string _path;
		private StyleCollection _styles;
		private string _head;

		#endregion

		#region Constructors

		private ThemeInfo (string name) 
		{
			this._defaultTheme = true;
			this._communityID = 0;
			this._name = name;
			this._path = Global.Path.GetDefaultSkinDirectory(name);
		}

		private ThemeInfo (int communityID, string name) 
		{
			this._defaultTheme = false;
			this._communityID = communityID;
			this._name = name;
			this._path = Global.Path.GetCommunitySkinDirectory(communityID, name);
		}

		#endregion

		#region Properties
		
		public int CommunityID 
		{
			get { return this._communityID; }
		}

		public string Name 
		{
			get { return this._name; }
		}

		public string Path 
		{
			get { return this._path; }
		}

		/// <summary>Gets a value indicating if this is a theme from the default community.</summary>
		public bool IsDefaultTheme 
		{
			get { return this._defaultTheme; }
		}

		/// <summary></summary>
		public string HeaderText
		{
			get 
			{
				if (this._head != null)
					return this._head;

				// get the path for /Communities/{ID}/Themes/Skin/Pages/Head.html and if it does
				string path = Global.Path.GetThemedPath(this, String.Concat(PortalProperties.SkinsDirectory, "/", PortalProperties.PagesDirectory), PortalProperties.HeaderFile, false);

				// no Head.html was found so set the contents of Head.html to empty
				if (path == null)
				{
					this._head = String.Empty;
					return this._head;
				}

				// read the file from the path
				using (StreamReader reader = File.OpenText(Global.Path.GetAbsoluteDiskPath(path))) 
				{
					// set the head
					this._head = reader.ReadToEnd();
					reader.Close();
				}

				return this._head;
			}
		}

		public StyleCollection Styles 
		{
			get 
			{
				// check to see if styles has already been populated
				if (this._styles != null)
					return this._styles;

				ArrayList list = new ArrayList();

				// check to see if this theme is in the default community
				// and decide which method should be used to get the correct
				// styles
				string[] styles = (this.IsDefaultTheme) ? this.GetDefaultStyles(this.Name) : this.GetStyles(this.Name);

				foreach(string name in styles)
					list.Add(new StyleInfo(name, Global.Path.GetThemedPath(this, PortalProperties.StylesDirectory, String.Concat(name, ".css"))));
			
				this._styles = new StyleCollection(list.ToArray(typeof(StyleInfo)) as StyleInfo[]);
				return this._styles;
			}
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			return this.Path;
		}

		#endregion
	}
}

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
using System.Collections;

namespace ManagedFusion
{
	public class ThemeCollection : ICollection
	{
		private ThemeInfo[] _collection;

		public ThemeCollection(ThemeInfo[] themes)
		{
			this._collection = themes;
		}

		public ThemeInfo this [string name] 
		{
			get { return this.GetTheme(name); } 
		}

		public ThemeInfo this [string name, CommunityInfo community] 
		{
			get { return this.GetThemeByCommunity(name, community); }
		}

		public ThemeInfo GetTheme (string name) 
		{
			ThemeInfo defaultTheme = null;
			name = name.ToLower();

			// search through each theme for the specified theme
			foreach(ThemeInfo theme in this._collection)
			{
				// check to see if the theme meets the default community
				// and the name is what is being looked for
				if (theme.IsDefaultTheme && theme.Name.ToLower() == name) 
				{
					defaultTheme = theme;
					continue;
				}

				// check to see if this theme matches the community
				// and name this indexor is looking for
				if (theme.Name.ToLower() == name)
					return theme;
			}

			// returns default theme according to name if community theme was not found
			// returns null if nothing is found and returns
			return defaultTheme;
		}

		public ThemeInfo GetThemeByCommunity (string name, CommunityInfo community)
		{
			ThemeInfo defaultTheme = null;

			// search through each theme for the specified theme
			foreach(ThemeInfo theme in this._collection)
			{
				// check to see if the theme meets the default community
				// and the name is what is being looked for
				if (theme.IsDefaultTheme && theme.Name == name)
				{
					defaultTheme = theme;
					continue;
				}

				// check to see if this theme matches the community
				// and name this indexor is looking for
				if (theme.CommunityID == community.Identity && theme.Name == name)
					return theme;
			}

			// returns default theme according to name if community theme was not found
			// returns null if nothing is found and returns
			return defaultTheme;
		}

		public CommunityThemeCollection GetThemes (CommunityInfo community) 
		{
			return GetThemes(community, false);
		}

		public CommunityThemeCollection GetThemes (CommunityInfo community, bool includeDefaults)
		{
			ArrayList list = new ArrayList();

			// go through each theme and look for ones in the
			// community
			foreach (ThemeInfo theme in this._collection) 
			{
				if (theme.CommunityID == community.Identity)
					list.Add(theme);
			}

			// now check to see if defaults should be included
			if (includeDefaults) 
			{
				foreach(ThemeInfo theme in this.GetDefaultThemes())
					list.Add(theme);
			}

			// returns a list of the themes
			return new CommunityThemeCollection(list.ToArray(typeof(ThemeInfo)) as ThemeInfo[]);
		}

		public CommunityThemeCollection GetDefaultThemes () 
		{
			ArrayList list = new ArrayList();

			// go through each theme and look for ones in the
			// community
			foreach (ThemeInfo theme in this._collection) 
			{
				if (theme.IsDefaultTheme)
					list.Add(theme);
			}

			// returns a list of the themes
			return new CommunityThemeCollection(list.ToArray(typeof(ThemeInfo)) as ThemeInfo[]);
		}

		public bool Contains (string name) 
		{
			return (this[name] != null);
		}

		public bool Contains (string name, CommunityInfo community) 
		{
			return (this[name,community] != null);
		}

		#region ICollection Members

		bool ICollection.IsSynchronized { get { return this._collection.IsSynchronized; } }

		public int Count { get { return this._collection.Length; } }

		void ICollection.CopyTo(Array array, int index)
		{
			if (array is ThemeInfo[])
				this._collection.CopyTo(array, index);
			else 
				throw new InvalidCastException(String.Format("Can not cast {0} to ThemeInfo[]", array.GetType()));
		}

		object ICollection.SyncRoot { get { return this._collection.SyncRoot; } }

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return this._collection.GetEnumerator();
		}

		#endregion
	}
}
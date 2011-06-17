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
	public class CommunityThemeCollection : ICollection
	{
		private ThemeCollection _collection;

		internal CommunityThemeCollection (ThemeInfo[] themes) 
		{
			this._collection = new ThemeCollection(themes);
		}

		public bool Contains (string name) 
		{
			return this._collection.Contains(name);
		}

		public ThemeInfo this [string name] 
		{
			get { return this._collection[name] as ThemeInfo; }
		}

		#region ICollection Members

		bool ICollection.IsSynchronized { get { return ((ICollection)this._collection).IsSynchronized; } }

		public int Count { get { return this._collection.Count; } }

		void ICollection.CopyTo(Array array, int index)
		{
			if (array is ThemeInfo[])
				((ICollection)this._collection).CopyTo(array, index);
			else 
				throw new InvalidCastException(String.Format("Can not cast {0} to ThemeInfo[]", array.GetType()));
		}

		object ICollection.SyncRoot { get { return ((ICollection)this._collection).SyncRoot; } }

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return this._collection.GetEnumerator();
		}

		#endregion
	}
}
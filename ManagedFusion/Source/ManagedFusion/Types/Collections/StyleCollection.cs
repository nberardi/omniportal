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
	/// <summary>
	/// Summary description for StyleCollection.
	/// </summary>
	public class StyleCollection : ICollection
	{
		private StyleInfo[] _collection;

		public StyleCollection(StyleInfo[] styles)
		{
			this._collection = styles;
		}

		public StyleInfo this [string name] 
		{
			get { return this.GetStyle(name); }
		}

		public StyleInfo GetStyle (string name) 
		{
			// if NoStyle has been selected in the database
			if (name == StyleInfo.NoStyle) return StyleInfo.NoStyleClass;

			// change the name to all lower to normalize the formatting
			name = name.ToLower();

			foreach(StyleInfo style in this._collection)
			{
				// find the style that matches the name being searched for
				if (style.Name.ToLower() == name)
					return style;
			}

			// no style found
			return null;
		}

		#region ICollection Members

		bool ICollection.IsSynchronized { get { return this._collection.IsSynchronized; } }

		public int Count { get { return this._collection.Length; } }

		void ICollection.CopyTo(Array array, int index)
		{
			if (array is StyleInfo[])
				this._collection.CopyTo(array, index);
			else 
				throw new InvalidCastException(String.Format("Can not cast {0} to StyleInfo[]", array.GetType()));
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
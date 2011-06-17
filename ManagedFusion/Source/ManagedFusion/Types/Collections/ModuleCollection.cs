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
using System.Data;
using System.Collections;

namespace ManagedFusion
{
	public class ModuleCollection : ICollection
	{
		private ModuleInfo[] _collection;

		public ModuleCollection (ModuleInfo[] modules) 
		{
			this._collection = modules;
		}

		public ModuleInfo this [Guid id] 
		{
			get 
			{  
				foreach(ModuleInfo module in this._collection) 
				{
					if (module.Identity == id)
						// component found and returned
						return module;
				}

				// component not found and nothing returned
				return null;
			}
		}

		public void CopyTo (ModuleInfo[] array, int index) 
		{
			this._collection.CopyTo(array, index);
		}

		#region ICollection Members

		bool ICollection.IsSynchronized { get { return this._collection.IsSynchronized; } }

		public int Count { get { return this._collection.Length; } }

		void ICollection.CopyTo(Array array, int index)
		{
			if (array is ModuleInfo[])
				this._collection.CopyTo(array, index);
			else 
				throw new InvalidCastException(String.Format("Can not cast {0} to ModuleInfo[]", array.GetType()));
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
using System;
using System.Collections;
using System.Collections.Generic;

namespace ManagedFusion
{
	public abstract class PortalTypeCollection<T> : ICollection, IEnumerable<T>
		where T : PortalType
	{
		private T[] _collection;

		public PortalTypeCollection (T[] collection)
		{
			this._collection = collection;
		}

		protected T[] Collection
		{
			get { return this._collection; }
			set { this._collection = value; }
		}

		public T this[int id, bool onlyEnabled]
		{
			get
			{
				T type = this[id];

				// if onlyEnabled is false return type 
				// or if onlyEnabled is true and type is enabled return type
				// else return null
				if (onlyEnabled == false || (onlyEnabled && type.Enabled))
					return type;
				else
					return null;
			}
		}

		public virtual T this[int id]
		{
			get
			{
				foreach (T type in this._collection) {
					if (type.Identity == id)
						// type found and is return
						return type;
				}

				// type not found null is returned
				return null;
			}
		}

		public bool Contains (int id)
		{
			return this[id] != null;
		}

		public void CopyTo (T[] array, int index)
		{
			this._collection.CopyTo(array, index);
		}

		public T[] ToArray()
		{
			return this._collection;
		}

		#region ICollection Members

		public int Count
		{
			get { return this._collection.Length; }
		}

		void ICollection.CopyTo (Array array, int index)
		{
			if (array is T[])
				this.CopyTo(array as T[], index);
			else
				throw new ArrayTypeMismatchException();
		}

		bool ICollection.IsSynchronized
		{
			get { return this._collection.IsSynchronized; }
		}

		object ICollection.SyncRoot
		{
			get { return this._collection.SyncRoot; }
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return this.CommittedItems;
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Abstract Methods

		/// <summary>
		/// Commit the changes to the database.
		/// </summary>
		public abstract void CommitChanges ();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		public abstract void Add (T item);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public abstract void Remove (T item);

		#endregion

		#region Enumerable Methods

		/// <summary>
		/// Every item currently repsented in the collection.
		/// </summary>
		public IEnumerator<T> AllItems
		{
			get
			{
				foreach (T type in this._collection)
					yield return type;
			}
		}

		/// <summary>
		/// Items that have been changed in the database, added, or deleted.
		/// </summary>
		public IEnumerator<T> ChangedItems
		{
			get
			{
				foreach (T type in this._collection)
					if (type.State != State.NoChanges)
						yield return type;
			}
		}

		/// <summary>
		/// Items that are currently represented in the database.
		/// </summary>
		public IEnumerator<T> CommittedItems
		{
			get
			{
				foreach (T type in this._collection)
					if (type.State == State.Changed
						|| type.State == State.NoChanges)
						yield return type;
			}
		}

		#endregion
	}
}
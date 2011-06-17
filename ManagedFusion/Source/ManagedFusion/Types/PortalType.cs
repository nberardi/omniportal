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
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace ManagedFusion
{
	public enum State 
	{
		NoChanges,
		Changed,
		Added,
		Deleted
	}

	[Serializable]
	public abstract class PortalType : ISerializable
	{
		/// <summary></summary>
		public const int TempIdentity = -1;

		/// <summary></summary>
		public const int NullIdentity = 0;

		#region Properties

		/// <summary></summary>
		public abstract int Identity { get; }

		/// <summary></summary>
		public abstract DateTime Touched { get; set; }

		/// <summary></summary>
		public virtual bool Enabled { get { return true; } }

		private State _State = State.NoChanges;
		/// <summary></summary>
		public State State { get { return _State; } }

		#endregion

		#region Constructors

		protected PortalType () { }

		protected PortalType (SerializationInfo info, StreamingContext context) 
		{
			this.SetState((State)info.GetInt32("State"));
		}

		#endregion

		#region Methods

		/// <summary></summary>
		protected internal void SetState (State state) 
		{
			this._State = state;
		}

		/// <summary></summary>
		public void SetForDeletion (bool delete) 
		{
			_State = (delete) ? State.Deleted : State.Changed;
		}

		/// <summary></summary>
		protected void ValueChanged () 
		{
			if (_State != State.Added)
				_State = State.Changed;

			this.Touched = DateTime.Now;
		}

		/// <summary></summary>
		public void CommitChanges () 
		{
			// commit changes only if there are not changes
			if (_State != State.NoChanges)
				this.CommitChangesToDatabase();
		}

		protected abstract void CommitChangesToDatabase ();

		#endregion

		#region ISerializable Members

		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("State", (int)this.State);
		}

		#endregion

		#region Object Members

		/// <summary></summary>
		/// <returns></returns>
		public new virtual string ToString()
		{
			return this.Identity.ToString();
		}

		public override int GetHashCode()
		{
			// if the Identity is only a TempIdentity then the bases GetHashCode will have to create
			// the unique hash since the value must be unique across objects of the same 
			// type.
			if (this.Identity == TempIdentity) return base.GetHashCode();

			return this.Identity;
		}

		#endregion
	}
}
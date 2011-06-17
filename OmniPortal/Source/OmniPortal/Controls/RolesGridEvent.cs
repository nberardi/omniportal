using System;

namespace OmniPortal.Controls
{
	public delegate void RolesGridEventHandler (object sender, RolesGridEventArgs e);

	public class RolesGridEventArgs : EventArgs 
	{
		private string _role;
		private string[] _tasks;

		public RolesGridEventArgs (string role, string[] tasks) 
		{
			this._role = role;
			this._tasks = tasks;
		}

		public string Role { get { return this._role; } }

		public string[] Tasks { get { return this._tasks; } }
	}
}

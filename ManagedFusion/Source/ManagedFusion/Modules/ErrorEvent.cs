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
using System.Web.UI;

namespace ManagedFusion.Modules
{
	/// <summary>Event handler for GetControlUnsucessful.</summary>
	public delegate void ErrorEventHandler (object sender, ErrorEventArgs e);
	
	/// <summary>Event arguments for GetControlUnsuccessful.</summary>
	public class ErrorEventArgs : EventArgs 
	{
		private Exception _exception;
		private bool _throwException;
		private Control _errorControl;

		/// <summary>
		/// Creates an instance of GetControlUnsuccessfulEventArgs.
		/// </summary>
		/// <param name="exc"></param>
		public ErrorEventArgs(Exception exception) 
		{
			this._exception = exception;
#if DEBUG
			this._throwException = true;
#else
			this._throwException = false;
#endif
			this._errorControl = new LiteralControl("<center>Sorry we were unable to find the requested page.</center>");
		}

		/// <summary>The exception that was thrown.</summary>
		public Exception ExceptionReturned 
		{
			get { return this._exception; }
		}

		/// <summary>Throw the exception.</summary>
		public bool ThrowException 
		{
			get { return _throwException; }
			set { _throwException = value; }
		}

		/// <summary>The control that should be returned if the exception is not thrown.</summary>
		public Control ErrorControl 
		{
			get { return _errorControl; }
			set { _errorControl = value; }
		}
	}
}
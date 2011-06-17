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

namespace ManagedFusion
{
	public class StyleInfo
	{
		/// <summary>
		/// The theme should inherit it's parents theme or style.
		/// </summary>
		public const string Inherited = "Inherited";

		/// <summary>
		/// No style has been set for the section.
		/// </summary>
		public const string NoStyle = "NoStyle";

		/// <summary>
		/// A list of styles that are handled at the system level.
		/// </summary>
		public static string[] SystemStyles
		{
			get { return new string[2] { Inherited, NoStyle }; }
		}

		#region NoStyleInfo Class

		private class NoStyleInfo : StyleInfo
		{
			public NoStyleInfo () : base(StyleInfo.NoStyle, String.Empty) { }
		}

		internal static readonly StyleInfo NoStyleClass = new NoStyleInfo();

		#endregion

		private string _name;
		private string _path;

		internal StyleInfo (string name, string path)
		{
			this._name = name;
			this._path = path;
		}

		public string Name
		{
			get { return this._name; }
		}

		public string Path
		{
			get { return this._path; }
		}

		public override string ToString ()
		{
			return this.Path;
		}
	}
}
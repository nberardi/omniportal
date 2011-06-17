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
using System.Xml.Serialization;

// ManagedFusion Classes
using ManagedFusion.Modules;

namespace ManagedFusion.Modules.Configuration
{
	public class ConfigurationSetup
	{
		private string[] _sqlScripts;
		[	XmlArray("sql"),
			XmlArrayItem("file", typeof(string))]
		public string[] SqlScripts 
		{
			get { return _sqlScripts; }
			set { _sqlScripts = value; }
		}

		private string _type;
		[	XmlAttribute("type")]
		public string Type 
		{
			get { return _type; }
			set { _type = value; }
		}
	}
}
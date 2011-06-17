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
using ManagedFusion.Security;
using ManagedFusion.Modules;

namespace ManagedFusion.Modules.Configuration
{
	public class ConfigurationTask
	{
		internal const string ViewPageName = "ViewPage";

		public static ConfigurationTask ViewPageTask
		{
			get 
			{ 
				ConfigurationTask task = new ConfigurationTask();
				task._name = ViewPageName;
				task._permissionsString = Permissions.Read.ToString();
				return task;
			}
		}

		#region Properties

		#region Xml Attributes

		private string _name;
		[XmlAttribute("name")]
		public string Name 
		{ 
			get { return _name; }
			set { _name = value; }
		}

		private string _permissionsString;
		[XmlAttribute("permissions")]
		public string PermissionsString 
		{ 
			get { return _permissionsString; }
			set { _permissionsString = value; }
		}

		#endregion

		[XmlIgnore]
		public Permissions Permissions 
		{
			get 
			{ 
				try 
				{
					return (Permissions)Enum.Parse(typeof(Permissions), this.PermissionsString, true);
				} 
				catch (ArgumentException exc) 
				{
					throw new ApplicationException(
						String.Format("Permissions [{0}] are not valid for {1}.", this.PermissionsString, this.Name),
						exc
						);
				}
			}
		}

		#endregion

		public bool CheckAccess (Permissions permissions) 
		{
			return Convert.ToBoolean(this.Permissions & permissions);
		}

		public override string ToString()
		{
			return this.Name;
		}

	}
}
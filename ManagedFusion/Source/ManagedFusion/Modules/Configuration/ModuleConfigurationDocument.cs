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
using System.IO;
using System.Data;
using System.Xml.Serialization;

namespace ManagedFusion.Modules.Configuration
{
	[XmlRoot("module")]
	public class ModuleConfigurationDocument
	{
		private ConfigurationPage[] _pages;
		[	XmlArray("pages"),
			XmlArrayItem("page", typeof(ConfigurationPage))]
		public ConfigurationPage[] Pages 
		{ 
			get { return _pages; }
			set { _pages = value; }
		}

		private ConfigurationTask[] _tasks;
		[	XmlArray("tasks"),
			XmlArrayItem("task", typeof(ConfigurationTask))]
		public ConfigurationTask[] Tasks 
		{ 
			get { return _tasks; }
			set { _tasks = value; }
		}

		private ConfigurationSetup _install;
		[	XmlElement("install", typeof(ConfigurationSetup))]
		public ConfigurationSetup Install 
		{ 
			get { return _install; } 
			set { _install = value; }
		}

		private ConfigurationSetup _uninstall;
		[	XmlElement("uninstall", typeof(ConfigurationSetup))]
		public ConfigurationSetup Uninstall 
		{ 
			get { return _uninstall; } 
			set { _uninstall = value; }
		}

		public string this [string path] 
		{
			get 
			{
				ConfigurationPage p = FindPage(path); 

				return (p == null) ? String.Empty : p.Control;
			}
		}

		public ConfigurationPage FindPage (string path) 
		{
			foreach(ConfigurationPage p in Pages)
				if (p.IsMatch(path))
					return p;

			return null;
		}

		internal void InstallModule () 
		{
			this.ExecuteSetupScript(Install);
		}

		internal void UninstallModule () 
		{
			this.ExecuteSetupScript(Uninstall);
		}

		private void ExecuteSetupScript (ConfigurationSetup setup) 
		{ 
			foreach(string file in setup.SqlScripts) 
			{
				// Create an instance of StreamReader to read from a file.
				using (StreamReader reader = new StreamReader(file)) 
				{

				}
			}
		}
	}
}
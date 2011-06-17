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
using System.Collections.Specialized;
using System.Reflection;
using System.Configuration;
using System.Configuration.Provider;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Modules;
using ManagedFusion.Portlets;

namespace ManagedFusion.Data
{
	public abstract class DatabaseProvider : ProviderBase
	{
		private string _connectionStringName;

		public ConnectionStringSettings ConnectionString
		{
			get { return ConfigurationManager.ConnectionStrings[_connectionStringName]; }
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			// get the connection string name 
			if (config["connectionStringName"] != null)
			{
				_connectionStringName = config["connectionStringName"];
				config.Remove("connectionStringName");
			}
			else
			{
				_connectionStringName = Databases.DefaultConnectionString.Name;
			}

			base.Initialize(name, config);
		}
	}
}

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
using System.Xml;
using System.Configuration.Provider;

namespace ManagedFusion.Configuration
{
	public abstract class CommunityConfigurationProvider : ProviderBase
	{
		public abstract CommunityConfigurationCollection CommunityConfigurations { get; }

		public CommunityConfiguration Default
		{
			get { return CommunityConfigurations.DefaultConfiguration; }
		}

		public CommunityConfiguration Current
		{
			get
			{
				// get portal
				CommunityInfo community = CommunityInfo.Current;

				// if the portal hasn't been set yet use the default config
				if (community == null) 
					return this.Default;

				return this[community.Identity];
			}
		}

		public CommunityConfiguration this[int id]
		{
			get { return CommunityConfigurations[id]; }
		}
	}
}
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
using System.Web;

// ManagedFusion Classes
using ManagedFusion.Syndication;

namespace ManagedFusion.Modules.Syndication
{
	public class ModuleSitemapProvider : SyndicationProvider
	{
		public override ISyndication Syndication
		{
			get { return Common.ExecutingModule.Sitemap; }
		}

		public override IHttpHandler Handler
		{
			get { return new ModuleSitemapHandler(this.Syndication); }
		}
	}
}

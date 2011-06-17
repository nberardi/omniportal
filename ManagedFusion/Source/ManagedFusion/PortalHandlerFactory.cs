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
using System.Web.UI;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Syndication;

namespace ManagedFusion
{
	/// <summary>Returns an instance of a class that implements the IHttpHandler interface.</summary>
	/// <remarks>
	/// A class that implements the <b>IHttpHandlerFactory</b> interface has no behavior except to 
	/// dynamically manufacture new handler objects--that is, new instances of classes that implement 
	/// the <b>IHttpHandler</b> interface.
	/// </remarks>
	public class PortalHandlerFactory : IHttpHandlerFactory
	{
		#region IHttpHandlerFactory Members

		/// <summary>Enables a factory to reuse an existing handler instance.</summary>
		/// <param name="handler">The IHttpHandler object to reuse.</param>
		public void ReleaseHandler (IHttpHandler handler)
		{
		}

		/// <summary>Returns an instance of a class that implements the IHttpHandler interface.</summary>
		/// <param name="context">An instance of the <see cref="System.Web.HttpContext"/> class that provides references to intrinsic server objects (For example, <b>Request</b>, <b>Response</b>, <b>Session</b>, and <b>Server</b>) used to service HTTP requests.</param>
		/// <param name="requestType">The HTTP data transfer method (<b>GET</b> or <b>POST</b>) that the client uses.</param>
		/// <param name="url">The <see cref="System.Web.HttpRequest.RawUrl"/> of the requested resource.</param>
		/// <param name="pathTranslated">The <see cref="System.Web.HttpRequest.PhysicalApplicationPath"/> to the requested resource. </param>
		/// <returns>A new <b>IHttpHandler</b> object that processes the request.</returns>
		public IHttpHandler GetHandler (HttpContext context, string requestType, string url, string pathTranslated)
		{
			if (Common.Path.IsManagedFusionPath)
			{
				// preprocess the request by transforming the URL if the Module.config tells this process to do that
				Common.ExecutingModule.PreProcessRequest();

				SyndicationProvider syndication = Syndications.Providers[context.Request.Url.Query];

				// check to see if this request can be syndicated
				if (syndication != null && syndication.IsSyndicated)
					return syndication.Handler;

				// set handler that is going to get used
				return Common.ExecutingModule.Handler;
			}
			else
			{
				return PageParser.GetCompiledPageInstance(url, pathTranslated, context);
			}
		}

		#endregion
	}
}
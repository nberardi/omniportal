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
using System.Net;
using System.Threading;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Principal;
using System.Security.Permissions;
using System.Web;
using System.Web.Security;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;
using ManagedFusion.Modules;
using ManagedFusion.Configuration;

namespace ManagedFusion
{
	#region Module Comments
	/// <summary>Provides module initialization and disposal events to the inheriting class.</summary>
	/// <remarks>
	/// The following is the order in which events happen in the <see cref="IHttpModule"/>.
	/// <code>
	/// Applicaton:
	/// 
	/// 	These events are raised in a nondeterministic order.
	/// 
	///		  Start:						Call this event to notify the module that the application is starting.
	///		  End:							Call this event to notify the module that the application is ending.
	///		  Disposed:						Call this event to notify the module that the application is ending for some reason. Allows the module to perform internal cleanup.
	///		  Error:						Call this event to notify the module of an error that occurs during request processing.
	///		* PreSendRequestHeaders:		Call this event to notify the module that the HTTP headers are about to be sent to the client.
	///		* PreSendRequestContent:		Call this event to notify the module that content is about to be sent to the client.
	/// 
	///		These events are raised in a predetermind order.  (as listed below)
	/// 
	///		  BeginRequest:					Call this event to notify a module that new request is beginning.
	///		  AuthenticateRequest:			Call this event when a security module needs to authenticate the user before it processes the request.
	///		  AuthorizeRequest:				Call this event by a security module when the request needs to be authorized. Called after authentication.
	///		  ResolveRequestCache:			Call this event after authentication. Caching modules use this event to determine if the request should be processed by its cache or if a handler should process the request.
	/// 
	///	[handler is created]
	/// 
	///		  AcquireRequestState:			Call this event to allow the module to acquire or create the state (for example, session) for the request.
	/// 
	///		* Happens here if buffer is disabled.
	/// 
	///		  PreRequestHandlerExecute:		Call this event to notify the module that the handler for the request is about to be called.
	/// 
	///	[handler is executed]
	/// 
	///		  PostRequestHandlerExecute:	Call this event to notify the module that the handler has finished processing the request.
	///		  ReleaseRequestState:			Call this event to allow the module to release state because the handler has finished processing the request.
	///		  UpdateRequestCache:			Call this event after a response from the handler. Caching modules should update their cache with the response.
	///		  EndRequest:					Call this event to notify the module that the request is ending.
	/// 
	///		* Happens here if buffer is enabled.
	/// </code>
	/// </remarks>
	#endregion
	public class PortalModule : IHttpModule
	{
		#region IHttpModule Members

		/// <summary>Initializes a module and prepares it to handle requests.</summary>
		/// <param name="application">An <see cref="System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application.</param>
		public void Init (HttpApplication application)
		{
			application.BeginRequest += new EventHandler(Application_BeginRequest);
			application.AuthenticateRequest += new EventHandler(Application_AuthenticateRequest);
			application.AuthorizeRequest += new EventHandler(Application_AuthorizeRequest);
			application.PreRequestHandlerExecute += new EventHandler(Application_PreRequestHandlerExecute);
			application.ReleaseRequestState += new EventHandler(Application_ReleaseRequestState);
			application.PreSendRequestHeaders += new EventHandler(Application_PreSendRequestHeaders);
		}

		/// <summary>Disposes of the resources (other than memory) used by the module that implements <b>IHttpModule</b>.</summary>
		/// <remarks><b>Dispose</b> performs any final cleanup work prior to removal of the module from the execution pipeline.</remarks>
		public void Dispose ()
		{
		}

		#endregion

		#region BeginRequest
		/// <summary>
		/// Call this event to notify a module that new request is beginning.
		/// </summary>
		private void Application_BeginRequest (object sender, EventArgs e)
		{
			HttpContext Context = ((HttpApplication)sender).Context;
			Context.Trace.Warn("BeginRequest Start: " + DateTime.Now.ToLongTimeString());

			#region Query String Trace
#if DEBUG
			// checks to see if trace mode should be truned on
			if (Context.Request.Url.Query.ToLower().IndexOf("trace") > -1) {
				Context.Trace.IsEnabled = true;
				switch (Context.Request.QueryString["m"]) {
					case "c":
						Context.Trace.TraceMode = TraceMode.SortByCategory;
						break;

					case "t":
						Context.Trace.TraceMode = TraceMode.SortByTime;
						break;

					default:
						Context.Trace.TraceMode = TraceMode.Default;
						break;
				}
			}
#endif
			#endregion

			try
			{
				if (Common.Path.IsManagedFusionPath)
				{
					// set request paths
					string requestPath = Common.Path.UrlPath;
					string requestBasePath = Common.Path.BaseUrlPath;

					// get site information for this request
					SiteInfo site = SiteInfo.Current;
					Context.Items["SiteInfo"] = site;
					
					if (Context.Items["SiteInfo"] == null)
						throw new PortalInitializationException(
							"SiteInfo could not be determined from the URL."
							);

					// get section information for this request
					SectionInfo section = site.ConnectedSection.GetSectionForPath(requestBasePath);
					Context.Items["SectionInfo"] = section;

					if (Context.Items["SectionInfo"] == null)
						throw new PortalInitializationException(
							String.Format("SectionInfo could not be determined, because the requested path, {0}, isn't valid.", requestBasePath)
							);

					// get community information for this request
					CommunityInfo portal = section.ConnectedCommunity;
					Context.Items["CommunityInfo"] = portal;

					if (Context.Items["CommunityInfo"] == null)
						throw new PortalInitializationException(
							String.Format("CommunityInfo could not be determined, because the section, {0}, isn't connected to a valid community.", section.ToString())
							);

					try
					{
						// set current language to user default
						if (Context.Request.UserLanguages != null && Context.Request.UserLanguages.Length > 0)
							Common.SelectedCulture = CultureInfo.CreateSpecificCulture(Context.Request.UserLanguages[0]);
						else
							Common.SelectedCulture = CommunityInfo.Current.Config.DefaultLanguage;
					}
					catch (ArgumentException)
					{
						// there has been a problem with the browser language
						Common.SelectedCulture = CultureInfo.InvariantCulture;
					}

					// creates an instance of the Module to be used in the application
					Common.ExecutingModule = (ModuleBase)Activator.CreateInstance(SectionInfo.Current.Module.Type);
#if DEBUG
					Context.Trace.Write("PortalDesktop", String.Concat("Loading Module: ", SectionInfo.Current.Module));
					Context.Trace.Write("GlobalApplication", "Request Path = " + requestPath);
					Context.Trace.Write("GlobalApplication", "Request Base Path = " + requestBasePath);
					Context.Trace.Write("GlobalApplication", "Site Information = " + site.ToString());
					Context.Trace.Write("GlobalApplication", "Section Read = " + String.Join("; ", section.GetTasks(Permissions.Read).ToArray()));
					Context.Trace.Write("GlobalApplication", "Section Add = " + String.Join("; ", section.GetTasks(Permissions.Add).ToArray()));
					Context.Trace.Write("GlobalApplication", "Section Edit = " + String.Join("; ", section.GetTasks(Permissions.Edit).ToArray()));
					Context.Trace.Write("GlobalApplication", "Section Delete = " + String.Join("; ", section.GetTasks(Permissions.Delete).ToArray()));
					Context.Trace.Write("GlobalApplication", "Section Owners = " + String.Join("; ", section.Owners.ToArray()));
#endif
				}
			}
			catch (NullReferenceException exc)
			{
#if DEBUG
				throw;
#else
				// throw new error of HTTP 404 Not Found
				throw new HttpException((int)HttpStatusCode.NotFound, exc.Message, exc);
#endif
			}

			Context.Trace.Warn("BeginRequest End: " + DateTime.Now.ToLongTimeString());
		}

		#endregion

		#region AuthenticateRequest

		/// <summary>
		/// Call this event when a security module needs to authenticate the user before it processes the request.
		/// </summary>
		[SecurityPermission(SecurityAction.Assert, ControlPrincipal = true)]
		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		private void Application_AuthenticateRequest(object sender, EventArgs e)
		{
			HttpContext Context = ((HttpApplication)sender).Context;
			Context.Trace.Warn("AuthenticateRequest Start: " + DateTime.Now.ToLongTimeString());

			if (Common.Path.IsManagedFusionPath)
			{
				// if there is no valid user then the user is not authenticated
				if (Common.Context.User == null)
				{
					IPrincipal principal = new GenericPrincipal(new NotAuthenticatedIdentity(), new string[0]);
					Context.User = principal;
					Thread.CurrentPrincipal = principal;
				}
#if DEBUG
				Context.Trace.Write("Authenticate", "User Name: " + Context.User.Identity.Name);
				Context.Trace.Write("Authenticate", "Is User Authenticated: " + Context.User.Identity.IsAuthenticated);
				Context.Trace.Write("Authenticate", "User Roles: " + String.Join(Common.Delimiter.ToString(), Common.Role.GetRolesForUser(Common.Context.User.Identity.Name)));
#endif
			}

			Context.Trace.Warn("AuthenticateRequest End: " + DateTime.Now.ToLongTimeString());
		}

		#endregion

		#region AuthorizeRequest

		/// <summary>
		/// Call this event by a security module when the request needs to be authorized. Called after authentication.
		/// </summary>
		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		private void Application_AuthorizeRequest(object sender, EventArgs e)
		{
			HttpContext Context = ((HttpApplication)sender).Context;
			Context.Trace.Warn("AuthorizeRequest Start: " + DateTime.Now.ToLongTimeString());

			if (Common.Path.IsManagedFusionPath)
			{
#if DEBUG
				string[] sysRoles = Common.Role.GetRolesForUser(Common.Context.User.Identity.Name);

				Context.Trace.Write("Authorize", "System Roles: " + String.Join(Common.Delimiter.ToString(), sysRoles));

				string[] tasks = SectionInfo.Current.GetTasks(Permissions.Read).ToArray();
				string[] roles = SectionInfo.Current.GetRoles(tasks).ToArray();

				Context.Trace.Write("Authorize", "Section Tasks For Read: " + String.Join(Common.Delimiter.ToString(), tasks));
				Context.Trace.Write("Authorize", "Section Roles For Read: " + String.Join(Common.Delimiter.ToString(), roles));
#endif
				// check to see if user has permission to view this section
				if (Common.ExecutingModule.AccessGranted == false)
					throw new ManagedFusion.Security.UnauthorizedAccessException(Common.Context.User.Identity.Name, Common.Path.UrlPath);
			}
			
			Context.Trace.Warn("AuthorizeRequest End: " + DateTime.Now.ToLongTimeString());
		}

		#endregion

		#region PreRequestHandlerExecute

		/// <summary>
		/// Call this event to notify the module that the handler for the request is about to be called.
		/// </summary>
		/// <remarks>
		/// This was done to allow sessions to be used inside the module, because it occured after AcquireRequestState.
		/// </remarks>
		private void Application_PreRequestHandlerExecute (object sender, EventArgs e)
		{
			HttpContext Context = ((HttpApplication)sender).Context;
			Context.Trace.Warn("PreRequestHandlerExecute Start: " + DateTime.Now.ToLongTimeString());

			if (Common.Path.IsManagedFusionPath) {
				// set the builder for the page
				Context.Items["PageBuilder"] = new PageBuilder();

				// process request in current executing module
				Common.ExecutingModule.ProcessRequest();
			}

			Context.Trace.Warn("PreRequestHandlerExecute End: " + DateTime.Now.ToLongTimeString());
		}

		#endregion

		#region ReleaseRequestState

		/// <summary>
		/// Call this event to allow the module to release state because the handler has finished processing the request.
		/// </summary>
		private void Application_ReleaseRequestState (object sender, EventArgs e)
		{
			HttpContext Context = ((HttpApplication)sender).Context;
			Context.Trace.Warn("ReleaseRequestState Start: " + DateTime.Now.ToLongTimeString());

			if (Common.Path.IsManagedFusionPath)
			{
				// replace the action attribute of the form with a managed fusion complient form action value
				Context.Response.Filter = new FormActionFilter(Context.Response.Filter);
			}

			Context.Trace.Warn("ReleaseRequestState End: " + DateTime.Now.ToLongTimeString());
		}

		#endregion

		#region PreSendRequestHeaders

		private void Application_PreSendRequestHeaders (object sender, EventArgs e)
		{
			HttpContext Context = ((HttpApplication)sender).Context;
			Context.Trace.Warn("PreSendRequestHeaders Start: " + DateTime.Now.ToLongTimeString());

			// adds the portal version to the header
			Context.Response.AppendHeader("X-Powered-By", PortalProperties.PortalName);
			Context.Response.AppendHeader("X-" + PortalProperties.PortalName + "-Version", PortalProperties.PortalVersion.ToString(3));

			Context.Trace.Warn("PreSendRequestHeaders End: " + DateTime.Now.ToLongTimeString());
		}

		#endregion
	}
}
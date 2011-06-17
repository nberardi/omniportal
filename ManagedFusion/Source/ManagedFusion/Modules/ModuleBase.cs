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
using System.Web.UI.WebControls;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Resources;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;
using ManagedFusion.Containers;
using ManagedFusion.Modules.Configuration;

namespace ManagedFusion.Modules
{
	/// <summary>The base class for all modules created for ManagedFusion.</summary>
	public abstract class ModuleBase
	{
		#region Private Fields

		private ModuleContent[] _holders;
		private ModuleContent _centerHolder;
		private ModuleAttribute _attribute;
		private string _internalLocation;

		#endregion

		#region enum CenterPosition

		/// <summary>
		/// The position on the page.
		/// </summary>
		public enum CenterPosition
		{
			/// <summary>Center position before the module.</summary>
			BeforeModule = -1,

			/// <summary>Center position after the module.</summary>
			AfterModule = -2,
		}

		#endregion

		#region class ModuleContent

		private class ModuleContent : Content
		{
			public ModuleContent() { }

			public new string ContentPlaceHolderID
			{
				get { return base.ContentPlaceHolderID; }
				set
				{
					Type targetType = typeof(Content);

					// get the reflected field reference for _contentPlaceHolderID
					FieldInfo contentPlaceHolderID = targetType.GetField("_contentPlaceHolderID", BindingFlags.Instance | BindingFlags.NonPublic);

					// set the content place holder id
					contentPlaceHolderID.SetValue(this, value);
				}
			}
		}

		#endregion

		#region Events

		/// <summary>Occurs when the module cannot get a control from the passed location.</summary>
		public event ErrorEventHandler Error;

		/// <summary>Occurs when the module cannot get a control from the passed location.</summary>
		/// <param name="e">Events.</param>
		protected virtual void OnError(ErrorEventArgs e)
		{
			if (Error != null)
				Error(this, e);
		}

		/// <summary>Occurs when the module is initialized, which is the first step in the its lifecycle right after authentication is checked.</summary>
		public event EventHandler Init;

		/// <summary>Occurs when the module is initialized, which is the first step in the its lifecycle right after authentication is checked.</summary>
		/// <param name="e">Events.</param>
		protected virtual void OnInit(EventArgs e)
		{
			if (Init != null)
				Init(this, e);
		}

		public event LoadSyndicationEventHandler LoadSyndication;

		protected virtual void OnLoadSyndication(LoadSyndicationEventArgs e)
		{
			if (LoadSyndication != null)
				LoadSyndication(this, e);
		}

		/// <summary>Occurs when the module holders are getting populated with the module controls.</summary>
		public event LoadModuleEventHandler Load;

		/// <summary>Occurs when the module holders are getting populated with the module controls.</summary>
		/// <param name="e">Events.</param>
		protected virtual void OnLoad(LoadModuleEventArgs e)
		{
			if (Load != null)
				Load(this, e);
		}

		///// <summary>Occurs when the module authorizes a user to perform a certain action.</summary>
		//public event AuthorizeEventHandler Authorize;

		///// <summary>Occurs when the module authorizes a user to perform a certain action.</summary>
		///// <param name="e">Events.</param>
		//protected virtual void OnAuthorize(AuthorizeEventArgs e)
		//{
		//    if (Authorize != null)
		//        Authorize(this, e);
		//}

		#endregion

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		protected ModuleBase()
		{
			if (this.GetType().IsDefined(typeof(ModuleAttribute), false) == false)
				throw new ApplicationException(
					String.Format("{0} doesn't contain ModuleAttribute", this.GetType())
					);

			// get the module attribute
			this._attribute = this.GetType().GetCustomAttributes(typeof(ModuleAttribute), false)[0] as ModuleAttribute;

			// launch event Init
			this.OnInit(EventArgs.Empty);

			// setup content syndication
			this._syndication = new ManagedFusion.Syndication.Feed(this.SectionInformation);

			// create the center place holder
			this._centerHolder = new ModuleContent();
			this._centerHolder.ID = "MainContent";
			this._centerHolder.ContentPlaceHolderID = "Main";

			// configure modules place holder
			this._holders = new ModuleContent[] { new ModuleContent(), new ModuleContent() };

			// process config file
			this._config = this.SectionInformation.Module.Config;
		}

		#endregion

		#region Properties

		/// <summary>The web servers Context used in this application.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected HttpContext Context { get { return Common.Context; } }

		/// <summary>A reference for <see cref="SiteInfo">SiteInfo</see>.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected SiteInfo SiteInformation { get { return SiteInfo.Current; } }

		/// <summary>A reference for <see cref="SectionInfo">SectionInfo</see>.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected SectionInfo SectionInformation { get { return SectionInfo.Current; } }

		/// <summary>A reference for <see cref="CommunityInfo">CommunityInfo</see>.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected CommunityInfo CommunityInformation { get { return CommunityInfo.Current; } }

		#endregion

		#region Methods

		#region Setup Methods

		/// <summary></summary>
		private void ConfigureContainers()
		{
			ContainerCollection containers = this.SectionInformation.Containers;
#if DEBUG
			Context.Trace.Write("ModuleBase", String.Concat(containers.Count.ToString(), " Container(s) Found"));
#endif
			// left pane
			foreach (ContainerInfo c in containers.GetContainersForPosition(0))
				this._holders[0].Controls.Add(new ContainerHolder(c));

			// right pane
			foreach (ContainerInfo c in containers.GetContainersForPosition(1))
				this._holders[1].Controls.Add(new ContainerHolder(c));
		}

		/// <summary>Gets the controls that is called from the current module for the location.</summary>
		/// <remarks>Gets the controls that is called from the current module for the location.  
		/// This method splits the <see cref="ManagedFusion.Modules.ModuleBase.Location">Location</see>
		/// with the <see cref="ManagedFusion.Common.Delimiter">Delimiter</see> and then gets each control.
		/// If a control cannot be found then it goes to the default theme and gets that version 
		/// of the control.</remarks>
		/// <param name="page">The page to get the controls from.</param>
		/// <returns>The control that was loaded.</returns>
		private Control[] GetControls(ManagedFusion.Modules.Configuration.ConfigurationPage page)
		{
			List<Control> controls = new List<Control>(0);

			// add all the controls to the controls list
			if (page.Control != null)
				foreach (string controlLocation in page.Controls)
					controls.Add(GetControl(controlLocation));

			return controls.ToArray();
		}

		#endregion

		/// <summary>Gets the control that is called from the module.</summary>
		/// <remarks>Gets the control that is called from the current module.  If the control cannot be found
		/// then it goes to the default theme and gets that version of the control.</remarks>
		/// <param name="controlLocation">The controls location.</param>
		/// <returns>The control that was loaded.</returns>
		protected Control GetControl(string controlLocation)
		{
#if DEBUG
			Context.Trace.Write("ModuleBase", "Get Control : " + controlLocation);
#endif
			try
			{
				// go get control
				return Common.Path.GetControlFromLocation(Common.Path.GetModulePath(this._attribute.FolderName, controlLocation));
			}
			catch (Exception exc)
			{
				ErrorEventArgs e = new ErrorEventArgs(exc);

				this.OnError(e);

				// throw exception if event say to
				if (e.ThrowException == true)
					throw;

				// return default control
				return e.ErrorControl;
			}
		}

		#region Redirect

		/// <summary>
		/// 
		/// </summary>
		/// <param name="page"></param>
		private void Redirect(ManagedFusion.Modules.Configuration.ConfigurationPage page)
		{
			// checks to see if there is a transform attribute and a redirect attribute
			// sends the transformed path on to get rewritten
			// else just stops at this point and does nothing
			if (page.Transform != null && page.Redirect)
				Redirect(page.TransformPath(this.InternalLocation));
			else
				return;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		protected void Redirect(string path)
		{
			this.Context.Response.Redirect(CombineUrl(path).ToString());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="queryString"></param>
		protected void Redirect(string path, params string[] queryString)
		{
			this.Context.Response.Redirect(CombineUrl(path, queryString).ToString());
		}

		#endregion

		#region RewritePath

		/// <summary>
		/// Assigns a <see cref="HttpContext.RewritePath"/> using portal specific logic to
		/// determin the correct URL to rewrite.
		/// </summary>
		/// <param name="page">The page to rewrite the path for.</param>
		private void RewritePath(ManagedFusion.Modules.Configuration.ConfigurationPage page)
		{
			// checks to see if there is a transform attribute
			// sends the transformed path on to get rewritten
			// else just stops at this point and does nothing
			if (page.Transform != null)
				RewritePath(page.TransformPath(this.InternalLocation));
			else
				return;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		protected void RewritePath(string path)
		{
			this.Context.RewritePath(CombineUrl(path).PathAndQuery);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="queryStrings"></param>
		protected void RewritePath(string path, params string[] queryStrings)
		{
			this.Context.RewritePath(CombineUrl(path, queryStrings).PathAndQuery);
		}

		#endregion

		#region CombineUrl

		/// <summary>
		/// Assigns a <see cref="HttpContext.RewritePath"/> using portal specific logic to
		/// determin the correct URL to rewrite.
		/// </summary>
		/// <param name="url">The url path.</param>
		protected Uri CombineUrl(string url)
		{
			// get parts of paths
			string[] path = url.Split(new char[] { '?' }, 2);

			// if there is no query string just pass the first part
			// else pass the first part and split the second part by '&'
			if (path.Length == 1)
				return CombineUrl(path[0], new string[0]);
			else
				return CombineUrl(path[0], path[1].Split('&'));
		}

		/// <summary>
		/// Assigns a <see cref="HttpContext.RewritePath"/> using portal specific logic to
		/// determin the correct URL to rewrite.
		/// </summary>
		/// <param name="path">The path url.</param>
		/// <param name="queryStrings">The query strings to add to the old url.</param>
		protected Uri CombineUrl(string path, params string[] queryStrings)
		{
			string newUrl = path + "?";

			if (Context.Request.QueryString.Count > 0)
			{
				// write current query strings
				foreach (string key in Context.Request.QueryString)
					newUrl += String.Format("{0}={1}&", key, Context.Request.QueryString[key]);
			}

			if (queryStrings != null && queryStrings.Length > 0)
			{
				// write new query strings
				newUrl += String.Join("&", queryStrings);
			}

			// remove all trailing '&'
			newUrl.TrimEnd('&');

			// remove all trailing '?'
			newUrl.TrimEnd('?');
#if DEBUG
			Context.Trace.Write("ModuleBase", String.Concat("URL Combine : ", newUrl));
#endif
			return Common.Path.GetPortalUrl(newUrl);
		}

		#endregion

		/// <summary></summary>
		/// <param name="tasks"></param>
		/// <returns></returns>
		protected bool IsInTask(string task)
		{
			return this.SectionInformation.UserInTasks(new string[] { task });
		}

		/// <summary></summary>
		/// <param name="page"></param>
		/// <returns></returns>
		private bool HasAccess(ManagedFusion.Modules.Configuration.ConfigurationPage page)
		{
			if (page == null)
				return false;

			return this.SectionInformation.UserInTasks(page.Tasks);
		}

		protected Control CreatePortlet(int id, string title, Control controlToAdd)
		{
			// get Portlet Template
			Control template = ManagedFusion.Containers.ContainerHolder.PortletTemplate;
			template.ID = String.Format("{0}_portlet_{1}",
				title.Replace(" ", "_"),
				id
				);

			// set title
			((Label)template.FindControl("title")).Text = title;

			// add portlet content
			((PlaceHolder)template.FindControl("content")).Controls.Add(controlToAdd);

			return template;
		}

		/// <summary>Gets the <c>content</c> of the current module to be placed in the executing page.</summary>
		private Content GetContentHolder(int position)
		{
			if (position > this._holders.GetUpperBound(0))
				throw new ArgumentOutOfRangeException("position");

			ModuleContent holder = this._holders[position];
			holder.ID = "Column" + position;
			holder.ContentPlaceHolderID = holder.ID;

			return holder;
		}


		internal void InstantiateContentIn(Control container)
		{
			switch (container.ID.ToLower())
			{
				case "main":
					container.Controls.Add(this._centerHolder);
					break;

				default:
					string id = container.ID.ToLower().Replace("column", String.Empty);
					container.Controls.Add(this.GetContentHolder(Convert.ToInt32(id)));
					break;
			}
		}

		#endregion

		#region Get Path

		/// <summary></summary>
		/// <param name="localPath"></param>
		/// <returns></returns>
		private string GetPath(string localPath)
		{
			return Common.Path.GetModulePath(this._attribute.FolderName, localPath);
		}

		/// <summary></summary>
		/// <param name="localPath"></param>
		/// <returns></returns>
		public Uri GetUrlPath(string localPath)
		{
			return Common.Path.GetAbsoluteUrl(this.GetPath(localPath));
		}

		/// <summary></summary>
		/// <param name="localPath"></param>
		/// <returns></returns>
		public string GetDiskPath(string localPath)
		{
			return Common.Path.GetAbsoluteDiskPath(this.GetPath(localPath));
		}

		#endregion

		/// <summary>Gets a list of all properties for module.</summary>
		public NameValueCollection Properties { get { return this.SectionInformation.ModuleData; } }

		/// <summary></summary>
		public bool AccessGranted
		{
			get
			{
				// checks to see if the module is using a config file or not
				if (this.Config.Pages != null)
				{
					ManagedFusion.Modules.Configuration.ConfigurationPage page = this.Config.FindPage(InternalLocation);

					return this.HasAccess(page);
				}

				return false;
			}
		}

		/// <summary>Gets the left over URL after the section path is removed.</summary>
		/// <returns>Returns the local location of the module in the path info.</returns>
		public string InternalLocation
		{
			get
			{
				// this is done to keep the init location in memory, because the
				// PreProcessRequest() method has the possiblity of changing the
				// URL to something that is not recognized by the module.config
				if (_internalLocation != null)
					return _internalLocation;

				_internalLocation = Common.Path.UrlPath.Remove(0, this.SectionInformation.Path.Length);
				return _internalLocation;
			}
		}

		/// <summary>Gets the real left over URL after the section path is removed.</summary>
		/// <returns>Returns the local location of the module in the path info.</returns>
		public string RealInternalLocation
		{
			get { return Common.Path.UrlPath.Remove(0, this.SectionInformation.Path.Length); }
		}

		private ModuleConfigurationDocument _config;
		/// <summary>Config file for this module.</summary>
		public ModuleConfigurationDocument Config
		{
			get
			{
				if (this._config == null)
					throw new ApplicationException(
						String.Format("There has been a problem with the module.config for {0}.", this._attribute.Title)
						);

				return this._config;
			}
		}

		private ManagedFusion.Syndication.Feed _syndication;
		/// <summary></summary>
		public ManagedFusion.Syndication.Feed Syndication
		{
			get { return this._syndication; }
		}

		/// <summary></summary>
		public IHttpHandler Handler
		{
			get
			{
				// checks to see if the module is using a config file or not
				if (this.Config.Pages != null)
				{
					ManagedFusion.Modules.Configuration.ConfigurationPage page = this.Config.FindPage(InternalLocation);

					// load current handler according to the page address
					if (page != null && page.Handler != null)
						return page.HandlerObject;
				}

				// the default handler is going to get used
				return CommunityInfo.Current.Config.DefaultPageHandler;
			}
		}

		/// <summary></summary>
		public void PreProcessRequest()
		{
			ManagedFusion.Modules.Configuration.ConfigurationPage page = null;

			// checks to see if the center module has anything added to it
			if (this.Config.Pages != null)
			{
				page = this.Config.FindPage(InternalLocation);

				// redirect the path for the page if there is a transform attribute and redirect attribute
				if (page.Transform != null && page.Redirect)
					Redirect(page);

				// rewrite the path for the page if there is a transform attribute
				else if (page.Transform != null)
					RewritePath(page);
			}
		}

		/// <summary></summary>
		public void ProcessRequest()
		{
			ManagedFusion.Modules.Configuration.ConfigurationPage page = null;

			// checks to see if the center module has anything added to it
			if (this.Config.Pages != null)
			{
				page = this.Config.FindPage(InternalLocation);

				// an alternative handler is being loaded so no futher processing has to be done
				if (page.Handler != null)
					return;
			}

			// executes the correct even depending on if we are creating a page or syndication
			if (SectionInformation.Syndicated
				&& Context.Request.Url.Query.ToLower().IndexOf("feed") > -1)
			{
				LoadSyndicationEventArgs pcea = new LoadSyndicationEventArgs(this._syndication);
				this.OnLoadSyndication(pcea);
			}
			else
			{
				Content beforeModule = new Content();
				Content afterModule = new Content();

				// setup content holders
				LoadModuleEventArgs phea = new LoadModuleEventArgs(
					this._holders,
					beforeModule,
					afterModule
					);
				this.OnLoad(phea);

				#region Controls Before the Module

				// add the controls that are before the module
				if (beforeModule.Controls.Count > 0)
					this._centerHolder.Controls.Add(beforeModule);

				// center pane
				foreach (ContainerInfo c in this.SectionInformation.Containers.GetContainersForPosition((int)CenterPosition.BeforeModule))
					this._centerHolder.Controls.Add(new ContainerHolder(c));

				#endregion

				// checks to see if the center module has anything added to it
				if (page != null && page.Control != null)
				{
					// load current control according to the page address
					foreach (Control c in GetControls(page))
						this._centerHolder.Controls.Add(c);
				}

				#region Controls After the Module

				// add the controls that are after the module
				if (afterModule.Controls.Count > 0)
					this._centerHolder.Controls.Add(afterModule);

				// center pane
				foreach (ContainerInfo c in this.SectionInformation.Containers.GetContainersForPosition((int)CenterPosition.AfterModule))
					this._centerHolder.Controls.Add(new ContainerHolder(c));

				#endregion

				// configure the containers
				this.ConfigureContainers();
			}
		}
	}
}
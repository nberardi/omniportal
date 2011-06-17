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
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Security;
using ManagedFusion.Portlets;

namespace ManagedFusion.Containers
{
	public class ContainerHolder : PlaceHolder, INamingContainer
	{
		private ContainerInfo _container;

		public ContainerHolder (ContainerInfo container) 
		{
			if (container == null) throw new ArgumentNullException("container");

			this._container = container;
		}

		protected override void OnInit(EventArgs e)
		{
			this.ID = String.Concat(this._container.Title, "_container", this._container.Identity.ToString());

			// add all portlets to container
			foreach(PortletInfo portlet in this._container.Portlets)
				if (portlet.UserHasPermissions(Permissions.Read))
					this.Controls.Add(this.GetControl(portlet));

			base.OnInit (e);
		}

		/// <summary>
		/// Renders the container.
		/// </summary>
		/// <param name="writer">The HTML writer to use.</param>
		protected override void Render(HtmlTextWriter writer)
		{
#if DEBUG
			Context.Trace.Write("Container", String.Format("Container {0} has {1} portlets.", this.ClientID, this.Controls.Count));
#endif
			base.Render (writer);
		}

		/// <summary>
		/// The name of this container.
		/// </summary>
		public string Title 
		{
			get { return this._container.Title; }
		}

		/// <summary>
		/// Gets a collection of the portlets in this container.
		/// </summary>
		public ControlCollection Portlets
		{
			get	{ return this.Controls; }
		}

		/// <summary>
		/// Gets the template for the portlet.
		/// </summary>
		/// <returns>Returns a control of the template.</returns>
		public static Control PortletTemplate
		{
			get { return Common.Path.GetControlFromLocation(Common.Path.GetPortletPath(PortalProperties.PortletTemplateFile)); }
		}

		private Control GetControl (PortletInfo portlet) 
		{
			// get Portlet Template
			Control template = PortletTemplate;
			template.ID = String.Format("{0}_portlet{1}",
				portlet.Title.Replace(" ", "_"), 
				portlet.Identity
				);

			// set title
			((Label)template.FindControl("title")).Text = portlet.Title;

			// set the location of the control
			string location = Common.Path.GetPortletPath(portlet.Module.FolderName, portlet.Module.ReadPage);

			// get the location of the control
			PortletUserControl controlToAdd = Common.Path.GetControlFromLocation(location) as PortletUserControl;

			// set value for child portlet
			controlToAdd.PortletInformation = portlet;

			// add portlet content
			((PlaceHolder)template.FindControl("content")).Controls.Add(controlToAdd);

			return template;
		}
	}
}
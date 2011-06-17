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
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

// OmniPortal Classes
using ManagedFusion.Portlets;
using OmniPortal;

namespace OmniPortal.Portlets.Xml
{
	/// <summary>
	///	Summary description for rssfeed.
	/// </summary>
	[Portlet("XML", "This is used to apply styles to XML files.", "XmlTransform", "{DE524311-36A5-496e-935E-6E71B281E8A6}")]
	public abstract class Xml : PortletUserControl
	{
		/// <summary>
		///	Required method for Designer support - do not modify
		///	the contents of this method with the code editor.
		/// </summary>
		protected override void OnInit(EventArgs e)
		{
			System.Web.UI.WebControls.Xml control = new System.Web.UI.WebControls.Xml();
			control.DocumentSource = XmlDocument;
			control.TransformSource = XslDocument;
			this.Controls.Add(control);
		
			base.OnInit (e);
		}

		protected abstract string XmlDocument { get; }

		protected abstract string XslDocument { get; }
	}
}
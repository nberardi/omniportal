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
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Controls
{
	/// <summary>
	/// Summary description for TabControl.
	/// </summary>
	[ToolboxData("<{0}:TabControl runat=server></{0}:TabControl>")]
	public class TabControl : System.Web.UI.WebControls.WebControl
	{
		#region Properties

		[	Browsable(true),
			Category("Appearance")]
		public string CssTabSelected 
		{ 
			get 
			{
				if (ViewState["CssTabSelected"] == null)
					return String.Empty;
				return (string)ViewState["CssTabSelected"];
			}
			set { ViewState["CssTabSelected"] = value; } 
		}

		[	Browsable(true),
			Category("Appearance")]
		public string CssTabNotSelected 
		{ 
			get 
			{
				if (ViewState["CssTabNotSelected"] == null)
					return String.Empty;
				return (string)ViewState["CssTabNotSelected"];
			}
			set { ViewState["CssTabNotSelected"] = value; } 
		}

		[	Browsable(true),
			Category("Appearance")]
		public string CssSpace 
		{ 
			get 
			{
				if (ViewState["CssSpace"] == null)
					return String.Empty;
				return (string)ViewState["CssSpace"];
			}
			set { ViewState["CssSpace"] = value; } 
		}

		#endregion

		public void AddTab (string name, Control c)
		{
			PlaceHolder h = new PlaceHolder();

			// <span id=? style=display:?>
			h.Controls.Add(new LiteralControl(String.Format(
				"<span id=\"{0}\"{1}>",
				String.Concat("content_", this.ClientID),
				(this.Controls.Count == 0) ? String.Empty : " style=\"display:none\""
				)));

			// add control
			h.ID = name;
			h.Controls.Add (c);

			// </span>
			h.Controls.Add(new LiteralControl(String.Concat("</span>", Environment.NewLine)));

			this.Controls.Add(h);
		}

		protected override void OnPreRender(EventArgs e)
		{
			// get client ecma script
			string script = OmniPortal.Properties.Resources.TabControlScriptBlock;
			
			// replace script variables
			script = script.Replace("{ClassSelected}", this.CssTabSelected);
			script = script.Replace("{ClassNotSelected}", this.CssTabNotSelected);

			this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), PortalProperties.SoftwareName + "_TabControlScriptBlock", script, false);

			base.OnPreRender (e);
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContents(HtmlTextWriter writer)
		{
			writer.WriteLine();

			// <table cellpadding=0 cellspacing=0 width=? height=?>
			writer.WriteBeginTag("table");
			writer.WriteAttribute("cellpadding", "0");
			writer.WriteAttribute("cellspacing", "0");
			writer.WriteAttribute("width", Width.ToString());
			writer.WriteAttribute("height", Height.ToString());
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.WriteLine();

			// <tr>
			writer.Indent++;
			writer.WriteFullBeginTag("tr");
			writer.WriteLine();

			// <td class=? colspan=?>
			writer.Indent++;
			writer.WriteBeginTag("td");
			writer.WriteAttribute("class", this.CssClass);
			writer.WriteAttribute("colspan", Convert.ToString(Controls.Count +1));
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.WriteLine();

			int indent = writer.Indent;

			writer.WriteLine();
			writer.Indent = 0;
			this.RenderChildren(writer);
			writer.Indent = indent;
			writer.WriteLine();

			// </td>
			writer.WriteEndTag("td");
			writer.WriteLine();
			writer.Indent--;

			// </tr>
			writer.WriteEndTag("tr");
			writer.WriteLine();
			writer.Indent--;

			// <tr>
			writer.Indent++;
			writer.WriteFullBeginTag("tr");
			writer.WriteLine();

			writer.Indent++;
			for (int i = 0; i < this.Controls.Count; i++)
			{
				//  <td class=? id=?><a onclick=doClick(?, ?, ?)>
				writer.WriteBeginTag("td");
				writer.WriteAttribute("class", (i == 0) ? this.CssTabSelected : this.CssTabNotSelected);
				writer.WriteAttribute("id", String.Concat("tab_", this.ClientID));
				writer.Write(HtmlTextWriter.TagRightChar);
				writer.WriteBeginTag("a");
				writer.WriteAttribute("onclick", String.Format("doClick({0}, {1}, '{2}')", i, Controls.Count, this.ClientID));
				writer.Write(HtmlTextWriter.TagRightChar);

				// content
				writer.Write(Controls[i].ID);

				writer.WriteEndTag("a");
				writer.WriteEndTag("td");
				writer.WriteLine();
			}

			// <td class=TabNothing>&nbsp;</td>
			writer.WriteBeginTag("td");
			writer.WriteAttribute("class", this.CssSpace);
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.Write("&nbsp;");
			writer.WriteEndTag("td");
			writer.WriteLine();
			writer.Indent--;

			// </tr>
			writer.WriteEndTag("tr");
			writer.WriteLine();
			writer.Indent--;

			// </table>
			writer.WriteEndTag("table");
			writer.WriteLine();
		}
	}
}
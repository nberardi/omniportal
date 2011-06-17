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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Controls
{
	internal class SelectionList : System.Web.UI.WebControls.WebControl
	{
		protected System.Web.UI.WebControls.ListBox deniedListBox;
		protected System.Web.UI.WebControls.ListBox grantedListBox;
		protected System.Web.UI.WebControls.Button addButton;
		protected System.Web.UI.WebControls.Button removeButton;

		#region Events

		public event EventHandler AddButtonClick 
		{
			add { addButton.Click += value; }
			remove { addButton.Click -= value; }
		}

		public event EventHandler RemoveButtonClick 
		{
			add { removeButton.Click += value; }
			remove { removeButton.Click -= value; }
		}

		public event EventHandler LeftIndexChanged 
		{
			add 
			{
				grantedListBox.SelectedIndexChanged += value;
				grantedListBox.AutoPostBack = true;
			}
			remove 
			{
				grantedListBox.SelectedIndexChanged -= value;
				grantedListBox.AutoPostBack = false;
			}
		}

		public event EventHandler RightIndexChanged 
		{
			add 
			{
				deniedListBox.SelectedIndexChanged += value; 
				deniedListBox.AutoPostBack = true;
			}
			remove 
			{
				deniedListBox.SelectedIndexChanged -= value;
				deniedListBox.AutoPostBack = false;
			}
		}


		#endregion

		#region Properties
		
		public object RightListDataSource
		{
			get { return deniedListBox.DataSource; }
			set { deniedListBox.DataSource = value; }
		}

		public string RightListTitle
		{
			get { return (string)ViewState["RightListTitle"]; }
			set { ViewState["RightListTitle"] = value; }
		}

		public string RightListTextField		
		{
			get { return deniedListBox.DataTextField; }
			set { deniedListBox.DataTextField = value; }
		}

		public string RightListValueField		
		{
			get { return deniedListBox.DataValueField; }
			set { deniedListBox.DataValueField = value; }
		}

		public ListItem RightListSelectedItem 
		{
			get { return deniedListBox.SelectedItem; }
		}

		public string RightListNameSelected
		{
			get { return (string)ViewState["RightListNameSelected"]; }
			set { ViewState["RightListNameSelected"] = value; }
		}

		public string LeftListNameSelected
		{
			get { return (string)ViewState["LeftListNameSelected"]; }
			set { ViewState["LeftListNameSelected"] = value; }
		}

		public object LeftListDataSource
		{
			get { return grantedListBox.DataSource; }
			set { grantedListBox.DataSource = value; }
		}

		public string LeftListTitle
		{
			get { return (string)ViewState["LeftListTitle"]; }
			set { ViewState["LeftListTitle"] = value; }
		}

		public string LeftListTextField		
		{
			get { return grantedListBox.DataTextField; }
			set { grantedListBox.DataTextField = value; }
		}

		public string LeftListValueField		
		{
			get { return grantedListBox.DataValueField; }
			set { grantedListBox.DataValueField = value; }
		}

		public ListItem LeftListSelectedItem 
		{
			get { return grantedListBox.SelectedItem; }
		}

		#endregion

		protected override void OnInit(EventArgs e) 
		{
			this.addButton = new Button();
			this.removeButton = new Button();
			this.grantedListBox = new ListBox();
			this.deniedListBox = new ListBox();
			
			// addButton
			//
			this.addButton.Text = "<";

			// removeButton
			//
			this.removeButton.Text = ">";

			this.Controls.Add(this.addButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.grantedListBox);
			this.Controls.Add(this.deniedListBox);

			base.OnInit (e);
		}

		public override void DataBind()
		{
			this.deniedListBox.DataBind();
			this.grantedListBox.DataBind();

			if (this.RightListNameSelected != null)
				this.deniedListBox.Items.FindByText(this.RightListNameSelected).Selected = true;
			
			if (this.LeftListNameSelected != null)
				this.grantedListBox.Items.FindByText(this.LeftListNameSelected).Selected = true;
		}

		#region Render

		protected override void OnPreRender(EventArgs e)
		{
			this.grantedListBox.Width = Unit.Percentage(95D);
			this.deniedListBox.Width = Unit.Percentage(95D);

			base.OnPreRender(e);
		}

		protected override void Render(HtmlTextWriter writer) 
		{
			// <table cellpadding=0 cellspacing=0 width=? height?>
			writer.WriteBeginTag("table");
			writer.WriteAttribute("border", "0");
			writer.WriteAttribute("width", Width.ToString());
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.WriteLine();

			// render body
			this.RenderContents(writer);

			// </table>
			writer.WriteEndTag("table");
			writer.WriteLine();
		}

		protected override void RenderContents(HtmlTextWriter writer) 
		{
			writer.Indent++;
			#region Header
			writer.WriteFullBeginTag("tr");
			writer.WriteLine();

			writer.Indent++;
			writer.WriteBeginTag("td");
			writer.WriteAttribute("class", this.CssClass);
			writer.WriteAttribute("align", "center");
			writer.Write(HtmlTextWriter.TagRightChar);

			writer.Write(this.LeftListTitle);
			
			writer.WriteEndTag("td");
			writer.WriteLine();

			writer.WriteBeginTag("td");
			writer.WriteAttribute("class", this.CssClass);
			writer.WriteAttribute("align", "center");
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.WriteEndTag("td");
			writer.WriteLine();
			
			writer.WriteBeginTag("td");
			writer.WriteAttribute("class", this.CssClass);
			writer.WriteAttribute("align", "center");
			writer.Write(HtmlTextWriter.TagRightChar);

			writer.Write(this.RightListTitle);

			writer.WriteEndTag("td");
			writer.WriteLine();
			writer.Indent--;

			writer.WriteEndTag("tr");
			#endregion

			#region Content
			writer.Indent++;
			writer.WriteFullBeginTag("tr");
			writer.WriteLine();

			writer.Indent++;
			writer.WriteBeginTag("td");
			writer.WriteAttribute("rowspan", "2");
			writer.WriteAttribute("class", this.CssClass);
			writer.WriteAttribute("align", "center");
			writer.Write(HtmlTextWriter.TagRightChar);

			this.grantedListBox.RenderControl(writer);
			
			writer.WriteEndTag("td");
			writer.WriteLine();

			writer.WriteBeginTag("td");
			writer.WriteAttribute("class", this.CssClass);
			writer.WriteAttribute("align", "center");
			writer.Write(HtmlTextWriter.TagRightChar);

			this.addButton.RenderControl(writer);

			writer.WriteEndTag("td");
			writer.WriteLine();
			
			writer.WriteBeginTag("td");
			writer.WriteAttribute("rowspan", "2");
			writer.WriteAttribute("class", this.CssClass);
			writer.WriteAttribute("align", "center");
			writer.Write(HtmlTextWriter.TagRightChar);

			this.deniedListBox.RenderControl(writer);

			writer.WriteEndTag("td");
			writer.WriteLine();
			writer.Indent--;

			writer.WriteEndTag("tr");
			#endregion

			#region Footer
			writer.WriteFullBeginTag("tr");

			writer.WriteBeginTag("td");
			writer.WriteAttribute("class", this.CssClass);
			writer.WriteAttribute("align", "center");
			writer.Write(HtmlTextWriter.TagRightChar);

			this.removeButton.RenderControl(writer);

			writer.WriteEndTag("td");
			writer.WriteLine();

			writer.WriteEndTag("tr");
			#endregion
			writer.Indent--;
		}
		
		#endregion

		public override bool Enabled
		{
			get { return base.Enabled; }
			set 
			{
				base.Enabled = value;
				this.deniedListBox.Enabled = value;
				this.grantedListBox.Enabled = value;
				this.addButton.Enabled = value;
				this.removeButton.Enabled = value;
			}
		}
	}
}
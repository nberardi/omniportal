using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Modules.Admin
{
	public partial class Container : System.Web.UI.UserControl
	{

		private int _id;
		private const string PageAddress = "Container.aspx";
		private ContainerInfo _info;

		public ContainerInfo Info 
		{
			get 
			{
				if (_info == null)
					  _info = ContainerInfo.Collection[(int)ViewState["Info"]];

				return _info;
			}
			set 
			{
				_info = null;
				ViewState["Info"] = value.Identity; 
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (this.Page.IsPostBack == false) 
			{
				this.DataBind();
			}
		}

		private void RedirectDefaultState () 
		{
			Response.Redirect(Common.Path.GetPortalUrl(PageAddress).ToString());
		}

		private void Redirect(string id) 
		{
			Response.Redirect(Common.Path.GetPortalUrl(PageAddress + "?id=" + id).ToString());
		}

		private void ShowItemPanel (bool show) 
		{
			this.ItemPanel.Visible = show;
			this.ListPanel.Visible = !show;
		}

		public override void DataBind()
		{
			// get the id for the community
			if (Request.QueryString["id"] != null) 
			{
				try { this._id = Convert.ToInt32(Request.QueryString["id"]); } 
				catch { this._id = ContainerInfo.TempIdentity; }
			} 
			else
				this._id = Int32.MinValue;

			if (this._id == ContainerInfo.TempIdentity || ContainerInfo.Collection.Contains(this._id)) 
			{
				if (this._id == ContainerInfo.TempIdentity) 
				{
					Info = ContainerInfo.CreateNew();
			
					// change the buttons
					this.sendButton.Text = this.GetGlobalResourceObject("OmniPortal", "Add") as string;

					// disable delete button
					this.deleteButton.Enabled = false;
					this.ConnectedFieldSet.Visible = false;
				}
				else 
				{
					Info = ContainerInfo.Collection[this._id];

					// check to see if the info exists
					if (Info == null) 
					{
						RedirectDefaultState();
					}

					// common attributes
					this.typeID.Text = Info.Identity.ToString();
					this.lastTouched.Text = Info.Touched.ToString("F");

					// appearence attributes
					this.titleText.Text = Info.Title;

					// connected attributes
					this.ConnectedPortletsGrid.DataSource = Info.Portlets;
					this.ConnectedPortletsGrid.DataKeyField = "Identity";

					// change the buttons
					this.sendButton.Text = this.GetGlobalResourceObject("OmniPortal", "Update") as string;
				} 

				// show the currentRow panel
				this.ShowItemPanel(true);
			} 
			else 
			{
				this.containerList.Items.AddRange(this.GetItemList());
				
				// hide the currentRow panel
				this.ShowItemPanel(false);
			}

			base.DataBind();
		}

		private ListItem[] GetPortletItemList (PortletCollection excludedList) 
		{
			ArrayList list = new ArrayList(PortletInfo.Collection.Count - excludedList.Count);

			// add the container items
			foreach(PortletInfo info in PortletInfo.Collection) 
			{
				if (excludedList.Contains(info.Identity) == false) 
				{
					ListItem item = new ListItem(info.Title, info.Identity.ToString());
					list.Add(item);
				}
			}

			// return the list of container values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		protected ListItem[] GetOrderItemList (int id, int count, int selectedOrder) 
		{
			ArrayList list = new ArrayList(count);

			for (int i = 0; i < count; i++) 
			{
				// if id is a TempIdentity then no id is added to the ListItem
				ListItem item = new ListItem(i.ToString(), (id == ContainerInfo.TempIdentity) ? i.ToString() : String.Format("{0}:{1}", id, i));

				// set as selected if "i" is the selectedOrder or selectedOrder is outside the valid
				// range and "i" is on its last loop
				item.Selected = ((i == selectedOrder) || (selectedOrder >= count && i == count -1));
				list.Add(item);
			}

			// return the list of order values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetItemList () 
		{
			ArrayList list = new ArrayList(ContainerInfo.Collection.Count);

			// add the default currentRow
			list.Add(new ListItem(
				"Select A Container",
				String.Empty
				));

			// add the section items
			foreach(ContainerInfo info in ContainerInfo.Collection) 
			{
				list.Add(new ListItem(
					info.Title,
					info.Identity.ToString()
					));
			}

			// return the list of section values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.containerList.SelectedIndexChanged += new EventHandler(containerList_SelectedIndexChanged);
			this.ConnectedPortletsGrid.ItemCreated += new DataGridItemEventHandler(ConnectedPortletsGrid_ItemCreated);
			this.ConnectedPortletsGrid.UpdateCommand += new DataGridCommandEventHandler(ConnectedPortletsGrid_UpdateCommand);
			this.ConnectedPortletsGrid.DeleteCommand += new DataGridCommandEventHandler(ConnectedPortletsGrid_DeleteCommand);

		}
		#endregion

		private void ConnectedPortletsGrid_ItemCreated(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) 
			{
				PortletInfo currentPortlet = e.Item.DataItem as PortletInfo;

				// HACK: Hacked around the problem of the e.Item.DataItem not being binded (being set to null) on the OrderLists' AutoPostBack from below.
				if (currentPortlet == null) 
				{
					int portletId = (int)ConnectedPortletsGrid.DataKeys[e.Item.DataSetIndex];
					currentPortlet = PortletInfo.Collection[portletId];
				}

				// create the order drop down list
				DropDownList order = new DropDownList();
				order.ID = "OrderList";
				order.Width = Unit.Percentage(95D);
				order.Items.AddRange(this.GetOrderItemList(currentPortlet.Identity, Info.Portlets.Count, currentPortlet.GetOrder(Info)));
				order.AutoPostBack = true;
				order.SelectedIndexChanged += new EventHandler(ConnectedPortletsGrid_ItemOrderChanged);

				// add order to the cell
				e.Item.Cells[3].Controls.Add(order);
			}
			else if (e.Item.ItemType == ListItemType.Footer) 
			{
				if (PortletInfo.Collection.Count == Info.Portlets.Count) 
				{
					int width = e.Item.Cells.Count;

					// clear all the cells
					e.Item.Cells.Clear();

					// create a new portlet link
					HyperLink newPortletLink = new HyperLink();
					newPortletLink.ID = "NewPortletLink";
					newPortletLink.NavigateUrl = ManagedFusion.Common.Path.GetPortalUrl("Portlet.aspx?id=-1").ToString();
					newPortletLink.Text = "Click Here To Create New Portlet";
					newPortletLink.ForeColor = Color.White;
					newPortletLink.Style.Add("text-transform", "lowercase");

					// create a new long cell
					TableCell newCell = new TableCell();
					newCell.ColumnSpan = width;
					newCell.Controls.Add(newPortletLink);

					// add cell back to table
					e.Item.Cells.Add(newCell);
				} 
				else 
				{
					// create the portlet drop down list
					DropDownList portlets = new DropDownList();
					portlets.ID = "PortletList";
					portlets.Width = Unit.Percentage(95D);
					portlets.Items.AddRange(this.GetPortletItemList(Info.Portlets));
				
					// create the order drop down list
					DropDownList order = new DropDownList();
					order.ID = "OrderList";
					order.Width = Unit.Percentage(95D);
					order.Items.AddRange(this.GetOrderItemList(PortletInfo.TempIdentity, Info.Portlets.Count +1, Int32.MaxValue));

					// create the button to add a new site
					LinkButton addButton = new LinkButton();
					addButton.ID = "AddButton";
					addButton.Text = this.GetGlobalResourceObject("OmniPortal", "Add") as string;
					addButton.ForeColor = Color.White;
					addButton.Style.Add("text-transform", "lowercase");
					addButton.CommandName = "Update";

					// add controls to the cells
					e.Item.Cells[1].Controls.Add(addButton);
					e.Item.Cells[2].Controls.Add(portlets);
					e.Item.Cells[3].Controls.Add(order);
				}
			}
		}

		private void ConnectedPortletsGrid_UpdateCommand(object source, DataGridCommandEventArgs e)
		{
			// get the drop down lists
			DropDownList portlets = e.Item.Cells[2].FindControl("PortletList") as DropDownList;
			DropDownList orderList = e.Item.Cells[3].FindControl("OrderList") as DropDownList;

			// set the attributes of the connect
			PortletInfo portletInfo = PortletInfo.Collection[Convert.ToInt32(portlets.SelectedValue)];
			int order = Convert.ToInt32(orderList.SelectedValue);

			// add the connection to the section
			Info.AddPortlet(portletInfo, order);

			// give the datagrid the most recent data
			ConnectedPortletsGrid.DataSource = Info.Portlets;
			ConnectedPortletsGrid.DataBind();
		}

		private void ConnectedPortletsGrid_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			int portletId = (int)ConnectedPortletsGrid.DataKeys[e.Item.DataSetIndex];
			PortletInfo currentPortlet = PortletInfo.Collection[portletId];

			// remove the connection to the section
			Info.RemovePortlet(currentPortlet);

			// give the datagrid the most recent data
			ConnectedPortletsGrid.DataSource = Info.Portlets;
			ConnectedPortletsGrid.DataBind();
		}

		private void ConnectedPortletsGrid_ItemOrderChanged(object sender, EventArgs e)
		{
			DropDownList orderList = sender as DropDownList;
			string[] selectedOrder = orderList.SelectedValue.Split(':');

			// get the portlet whose order is going to change
			PortletInfo portletInfo = PortletInfo.Collection[Convert.ToInt32(selectedOrder[0])];
			
			// get the order of the portlet
			int order = Convert.ToInt32(selectedOrder[1]);

			// commit the changes of this portlet to the database
			Info.UpdatePortlet(portletInfo, order);
		}

		protected void sendButton_Click(object sender, System.EventArgs e)
		{
			// set the values of the site
			Info.Title = this.titleText.Text;

			// if the site is a temp site -- meaning it hasn't
			// yet been added to the database
			if (Info.Identity == ContainerInfo.TempIdentity) 
			{
				ContainerInfo.Collection.Add(Info);
			} 

			// commit the changes of this site to the database
			Info.CommitChanges();

			// redirect after changes
			RedirectDefaultState();
		}

		protected void deleteButton_Click(object sender, System.EventArgs e)
		{
			Info.SetForDeletion(true);

			// commit the changes of this site to the database
			Info.CommitChanges();

			// redirect after changes
			RedirectDefaultState();
		}

		protected void cancelButton_Click(object sender, System.EventArgs e)
		{
			// redirect back to the default state
			RedirectDefaultState();
		}

		private void containerList_SelectedIndexChanged(object sender, EventArgs e)
		{
			Redirect(this.containerList.SelectedValue);
		}
	}
}

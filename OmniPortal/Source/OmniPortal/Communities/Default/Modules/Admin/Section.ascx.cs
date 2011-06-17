using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ManagedFusion;
using ManagedFusion.Security;

namespace OmniPortal.Communities.Default.Modules.Admin
{
	public partial class Section : System.Web.UI.UserControl
	{
		private int _id;
		private int _parent;
		private const string PageAddress = "Section.aspx";
		private SectionInfo _info;

		public SectionInfo Info
		{
			get
			{
				if (_info == null)
					_info = SectionInfo.Collection[(int)ViewState["Info"]];

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

		private void RedirectDefaultState()
		{
			Response.Redirect(Common.Path.GetPortalUrl(PageAddress).ToString());
		}

		private void Redirect(string id)
		{
			Response.Redirect(Common.Path.GetPortalUrl(PageAddress + "?id=" + id).ToString());
		}

		private void ShowItemPanel(bool show)
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
				catch { this._id = SectionInfo.TempIdentity; }
			}
			else
				this._id = Int32.MinValue;

			if (Request.QueryString["parent"] != null)
			{
				try { this._parent = Convert.ToInt32(Request.QueryString["parent"]); }
				catch { this._parent = RootSection.Identity; }
			}
			else
				this._parent = Int32.MinValue;

			if (this._id == SectionInfo.TempIdentity || SectionInfo.Collection.Contains(this._id))
			{
				if (this._id == SiteInfo.TempIdentity)
				{
					Info = SectionInfo.CreateNew();
					Info.Parent = (SectionInfo.Collection.Contains(this._parent)) ? SectionInfo.Collection[this._parent] : null;

					// change the buttons
					this.sendButton.Text = this.GetGlobalResourceObject("OmniPortal", "Add") as string;

					// path attributes
					this.currentPath.Text = (Info.Parent != null) ? Info.Parent.Path + "{Name Field}" : "/";

					// disable delete button
					this.deleteButton.Enabled = false;
					this.ConnectedFieldSet.Visible = false;
					this.RolesTableRow.Visible = false;
				}
				else
				{
					Info = SectionInfo.Collection[this._id];

					// check to see if the info exists
					if (Info == null)
					{
						RedirectDefaultState();
					}

					// common attributes
					this.typeID.Text = Info.Identity.ToString();
					this.lastTouched.Text = Info.Touched.ToString("F");

					// path attributes
					this.currentPath.Text = Info.Path;

					// appearence attributes
					this.nameText.Text = Info.Name;
					this.titleText.Text = Info.OriginalTitle;

					// module attributes
					this.rolesGrid.Enabled = true;
					this.rolesGrid.Roles = new List<string>(Common.Role.GetAllRoles());
					this.rolesGrid.Roles.AddRange(new string[] { PortalRole.Everybody.ToString(), PortalRole.Authenticated.ToString(), PortalRole.NotAuthenticated.ToString() });
					this.rolesGrid.Tasks = this.ConvertTaskList(Info.Module.Config.Tasks);
					this.rolesGrid.Tasks.Add(ManagedFusion.Modules.Configuration.ConfigurationTask.ViewPageTask.ToString());
					this.rolesGrid.RolesInUse = Info.Roles;

					// connected attributes
					this.ChildrenGrid.DataSource = Info.Children;
					this.ChildrenGrid.DataKeyField = "Identity";
					this.ConnectedSitesGrid.DataSource = Info.ConnectedSites;
					this.ConnectedSitesGrid.DataKeyField = "Identity";
					this.communityList.Items.AddRange(this.GetCommunityItemList(Info.ConnectedCommunity));
					this.communityList.Enabled = (Info.Parent == null);
					this.ConnectedContainersGrid.DataSource = Info.Containers;
					this.ConnectedContainersGrid.DataKeyField = "Identity";

					// change the buttons
					this.sendButton.Text = this.GetGlobalResourceObject("OmniPortal", "Update") as string;
				}

				// path attributes
				this.parentList.Items.AddRange(this.GetParentSectionsItemList(Info.Parent, Info));

				// appearence attributes
				this.visibleCheckBox.Checked = Info.Visible;
				this.syndicatedCheckBox.Checked = Info.Syndicated;
				this.themeList.Items.AddRange(this.GetThemesItemList(Info.OriginalTheme));
				this.styleList.Items.AddRange(this.GetStylesItemList(Info.OriginalTheme, Info.OriginalStyle));
				this.ownerList.Items.AddRange(this.GetOwnersItemList(Info.OriginalOwner));

				// modules attributes
				this.moduleList.Items.AddRange(this.GetModulesItemList(Info.Module));

				// show the currentRow panel
				this.ShowItemPanel(true);
				this.parentLink.Visible = (Info.Parent != null);
				if (parentLink.Visible)
				{
					this.parentLink.NavigateUrl = Common.Path.GetPortalUrl("Section.aspx?id=" + Info.Parent.Identity).ToString();
					this.parentLink.Text = Info.Parent.Title;
				}
			}
			else
			{
				this.sectionList.Items.AddRange(this.GetItemList());

				// hide the currentRow panel
				this.ShowItemPanel(false);
			}

			base.DataBind();
		}

		private List<string> ConvertTaskList(object[] tasks)
		{
			List<string> list = new List<string>();

			// put each task name in to the list
			foreach (object task in tasks)
				list.Add(task.ToString());

			return list;
		}

		private ListItem[] GetContainerPositionItemList(int containerId, int selectedPosition)
		{
			ListItem[] list = new ListItem[4];

			list[0] = new ListItem("Right", (containerId == ContainerInfo.TempIdentity) ? "0" : String.Format("{0}:0", containerId));
			list[0].Selected = (selectedPosition == 0);
			list[1] = new ListItem("Left", (containerId == ContainerInfo.TempIdentity) ? "1" : String.Format("{0}:1", containerId));
			list[1].Selected = (selectedPosition == 1);
			list[2] = new ListItem("Above Module", (containerId == ContainerInfo.TempIdentity) ? "-1" : String.Format("{0}:-1", containerId));
			list[2].Selected = (selectedPosition == -1);
			list[3] = new ListItem("Below Module", (containerId == ContainerInfo.TempIdentity) ? "-2" : String.Format("{0}:-2", containerId));
			list[3].Selected = (selectedPosition == -2);

			// return the list of containers values
			return list;
		}

		private ListItem[] GetCommunityItemList(CommunityInfo selectedCommunity)
		{
			ArrayList list = new ArrayList(CommunityInfo.Collection.Count);

			// add the communities items
			foreach (CommunityInfo info in CommunityInfo.Collection)
			{
				ListItem item = new ListItem(info.Title, info.Identity.ToString());
				item.Selected = (info == selectedCommunity);
				list.Add(item);
			}

			// return the list of community values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetContainerItemList(ContainerCollection excludedList)
		{
			ArrayList list = new ArrayList(ContainerInfo.Collection.Count - excludedList.Count);

			// add the container items
			foreach (ContainerInfo info in ContainerInfo.Collection)
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

		private ListItem[] GetOwnersItemList(string selectedOwner)
		{
			ArrayList list = new ArrayList(Common.Role.GetAllRoles().Length);

			// add the owner items
			foreach (string role in Common.Role.GetAllRoles())
			{
				ListItem item = new ListItem(role);
				item.Selected = (role.ToLower() == selectedOwner.ToLower());
				list.Add(item);
			}

			// return the list of owner values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetModulesItemList(ModuleInfo selectedModule)
		{
			ArrayList list = new ArrayList(SectionModule.Collection.Count);

			// add the default currentRow
			list.Add(new ListItem(
				"Select A Module",
				String.Empty
				));

			// add the module items
			foreach (SectionModule info in SectionModule.Collection)
			{
				ListItem item = new ListItem(info.Title, info.Identity.ToString());
				item.Selected = (info == selectedModule);
				list.Add(item);
			}

			// return the list of module values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetThemesItemList(string selectedTheme)
		{
			ArrayList list = new ArrayList(Info.ConnectedCommunity.Themes.Count);

			// add the theme items
			foreach (ThemeInfo info in Info.ConnectedCommunity.Themes)
			{
				// don't show the NoTheme folder it is already account for above
				if (info.Name == ThemeInfo.NoTheme)
					continue;

				ListItem item = new ListItem((info.IsDefaultTheme ? "[default] " : String.Empty) + info.Name, info.Name);
				item.Selected = (info.Name == selectedTheme);
				list.Add(item);
			}

			// add the Inherited
			ListItem inherited = new ListItem("[system] " + ThemeInfo.Inherited, ThemeInfo.Inherited);
			inherited.Selected = (ThemeInfo.Inherited == selectedTheme);
			list.Add(inherited);

			// add the NoTheme
			ListItem noTheme = new ListItem("[system] " + ThemeInfo.NoTheme, ThemeInfo.NoTheme);
			noTheme.Selected = (ThemeInfo.NoTheme == selectedTheme);
			list.Add(noTheme);

			// return the list of theme values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetStylesItemList(string selectedTheme, string selectedStyles)
		{
			ArrayList list = new ArrayList(Info.ConnectedCommunity.Themes.Count);

			if (ThemeInfo.NoTheme != selectedTheme)
			{
				if (ThemeInfo.Inherited != selectedTheme)
				{
					// add the style items
					foreach (StyleInfo info in Info.ConnectedCommunity.Themes[selectedTheme].Styles)
					{
						ListItem item = new ListItem(info.Name, info.Name);
						item.Selected = (info.Name == selectedStyles);
						list.Add(item);
					}
				}

				// add the Inherited
				ListItem inherited = new ListItem("[system] " + StyleInfo.Inherited, StyleInfo.Inherited);
				inherited.Selected = (StyleInfo.Inherited == selectedStyles);
				list.Add(inherited);
			}

			// add the NoTheme
			ListItem noStyle = new ListItem("[system] " + StyleInfo.NoStyle, StyleInfo.NoStyle);
			noStyle.Selected = (StyleInfo.NoStyle == selectedStyles);
			list.Add(noStyle);

			// return the list of style values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		protected ListItem[] GetOrderItemList(int sectionID, int count, int selectedOrder)
		{
			ArrayList list = new ArrayList(count);

			for (int i = 0; i < count; i++)
			{
				// if sectionID is a TempIdentity then no sectionID is added to the ListItem
				ListItem item = new ListItem(i.ToString(), (sectionID == SectionInfo.TempIdentity) ? i.ToString() : String.Format("{0}:{1}", sectionID, i));

				// set as selected if "i" is the selectedOrder or selectedOrder is outside the valid
				// range and "i" is on its last loop
				item.Selected = ((i == selectedOrder) || (selectedOrder >= count && i == count - 1));
				list.Add(item);
			}

			// return the list of order values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetItemList()
		{
			ArrayList list = new ArrayList(RootSection.Children.Count);

			// add the default currentRow
			list.Add(new ListItem(
				"Select A Root Section",
				String.Empty
				));

			// add the section items
			foreach (SectionInfo info in RootSection.Children)
			{
				list.Add(new ListItem(
					info.Title,
					info.Identity.ToString()
					));
			}

			// return the list of section values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		private ListItem[] GetParentSectionsItemList(SectionInfo selectedSection, SectionInfo currentSection)
		{
			ArrayList list = new ArrayList(SectionInfo.Collection.Count);

			// add the root currentRow
			ListItem root = new ListItem("{root}", RootSection.Identity.ToString());
			root.Selected = (null == selectedSection);
			list.Add(root);

			// add the section items
			foreach (SectionInfo info in SectionInfo.Collection)
			{
				// if the selectedSection is a parent of info then it cannot be
				// used because you cannot move a parent to one of it's children
				if (currentSection >= info == false)
				{
					ListItem item = new ListItem(info.Path, info.Identity.ToString());
					item.Selected = (info == selectedSection);
					list.Add(item);
				}
			}

			// return the list of section values
			return list.ToArray(typeof(ListItem)) as ListItem[];
		}

		override protected void OnInit(EventArgs e)
		{
			// disabled roles list by default
			this.rolesGrid.Enabled = false;

			base.OnInit(e);
		}

		private void ChildrenGrid_ItemCreated(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				SectionInfo currentSection = e.Item.DataItem as SectionInfo;

				// HACK: Hacked around the problem of the e.Item.DataItem not being binded (being set to null) on the OrderLists' AutoPostBack from below.
				if (currentSection == null)
				{
					int sectionID = (int)ChildrenGrid.DataKeys[e.Item.DataSetIndex];
					currentSection = SectionInfo.Collection[sectionID];
				}

				// create the order drop down list
				DropDownList order = new DropDownList();
				order.ID = "OrderList";
				order.Width = Unit.Percentage(95D);
				order.Items.AddRange(this.GetOrderItemList(currentSection.Identity, Info.Children.Count, currentSection.Order));
				order.AutoPostBack = true;
				order.SelectedIndexChanged += new EventHandler(ChildrenGrid_ItemOrderChanged);

				// add order to the cell
				e.Item.Cells[4].Controls.Add(order);
			}
			else if (e.Item.ItemType == ListItemType.Footer)
			{
				// create the name text box
				TextBox name = new TextBox();
				name.ID = "NameText";
				name.MaxLength = 32;
				name.Width = Unit.Percentage(95D);

				// create the owner drop down list
				DropDownList owners = new DropDownList();
				owners.ID = "OwnersList";
				owners.Width = Unit.Percentage(95D);
				owners.Items.AddRange(this.GetOwnersItemList(String.Empty));

				// create the module drop down list
				DropDownList modules = new DropDownList();
				modules.ID = "ModulesList";
				modules.Width = Unit.Percentage(95D);
				modules.Items.AddRange(this.GetModulesItemList(null));

				// create the order drop down list
				DropDownList order = new DropDownList();
				order.ID = "OrderList";
				order.Width = Unit.Percentage(95D);
				order.Items.AddRange(this.GetOrderItemList(SectionInfo.TempIdentity, Info.Children.Count + 1, Int32.MaxValue));

				// create the button to add a new site
				LinkButton addButton = new LinkButton();
				addButton.ID = "AddButton";
				addButton.Text = this.GetGlobalResourceObject("OmniPortal", "Add") as string;
				addButton.ForeColor = Color.White;
				addButton.Style.Add("text-transform", "lowercase");
				addButton.CommandName = "Update";

				// add controls to the cells
				e.Item.Cells[0].Controls.Add(addButton);
				e.Item.Cells[1].Controls.Add(name);
				e.Item.Cells[2].Controls.Add(owners);
				e.Item.Cells[3].Controls.Add(modules);
				e.Item.Cells[4].Controls.Add(order);
			}
		}

		private void ChildrenGrid_UpdateCommand(object source, DataGridCommandEventArgs e)
		{
			SectionInfo sectionInfo = SectionInfo.CreateNew(this.Info);

			// get the web objects
			TextBox name = e.Item.Cells[1].FindControl("NameText") as TextBox;
			DropDownList owners = e.Item.Cells[2].FindControl("OwnersList") as DropDownList;
			DropDownList modules = e.Item.Cells[3].FindControl("ModulesList") as DropDownList;
			DropDownList order = e.Item.Cells[4].FindControl("OrderList") as DropDownList;

			// set the attributes of the section
			sectionInfo.Name = name.Text;
			sectionInfo.OriginalOwner = owners.SelectedValue;
			sectionInfo.Module = SectionModule.Collection[new Guid(modules.SelectedValue)] as SectionModule;
			sectionInfo.Order = Convert.ToInt32(order.SelectedValue);

			// commit the changes of this section to the database
			sectionInfo.CommitChanges();
		}

		protected void ChildrenGrid_ItemOrderChanged(object sender, System.EventArgs e)
		{
			DropDownList order = sender as DropDownList;
			string[] selectedOrder = order.SelectedValue.Split(':');

			// get the section whose order is going to change
			SectionInfo sectionInfo = SectionInfo.Collection[Convert.ToInt32(selectedOrder[0])];

			// set the order of the section
			sectionInfo.Order = Convert.ToInt32(selectedOrder[1]);

			// commit the changes of this section to the database
			sectionInfo.CommitChanges();
		}

		private void ConnectedSitesGrid_ItemCreated(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Footer)
			{
				// create the subDomain text box
				TextBox subDomain = new TextBox();
				subDomain.ID = "SubDomainText";
				subDomain.Width = Unit.Percentage(95D);
				subDomain.Text = "*";
				subDomain.MaxLength = 128;
				subDomain.Style.Add("text-align", "right");

				// create the domain text box
				TextBox domain = new TextBox();
				domain.ID = "DomainText";
				domain.MaxLength = 128;
				domain.Width = Unit.Percentage(95D);

				// create the button to add a new site
				LinkButton addButton = new LinkButton();
				addButton.ID = "AddButton";
				addButton.Text = this.GetGlobalResourceObject("OmniPortal", "Add") as string;
				addButton.ForeColor = Color.White;
				addButton.Style.Add("text-transform", "lowercase");
				addButton.CommandName = "Update";

				// add controls to the cells
				e.Item.Cells[0].Controls.Add(addButton);
				e.Item.Cells[1].Controls.Add(subDomain);
				e.Item.Cells[2].Text = "<b>.</b>";
				e.Item.Cells[3].Controls.Add(domain);
			}
		}

		private void ConnectedSitesGrid_UpdateCommand(object source, DataGridCommandEventArgs e)
		{
			SiteInfo siteInfo = SiteInfo.CreateNew();

			// get the text boxes
			TextBox subDomain = e.Item.Cells[1].FindControl("SubDomainText") as TextBox;
			TextBox domain = e.Item.Cells[3].FindControl("DomainText") as TextBox;

			// set the attributes of the site
			siteInfo.Domain = domain.Text;
			siteInfo.SubDomain = subDomain.Text;

			// set this community as the connected community
			siteInfo.ConnectedSection = Info;

			// commit the changes of this site to the database
			siteInfo.CommitChanges();
		}

		private void ConnectedContainersGrid_ItemCreated(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				ContainerInfo currentContainer = e.Item.DataItem as ContainerInfo;

				// HACK: Hacked around the problem of the e.Item.DataItem not being binded (being set to null) on the OrderLists' AutoPostBack from below.
				if (currentContainer == null)
				{
					int containerId = (int)ConnectedContainersGrid.DataKeys[e.Item.DataSetIndex];
					currentContainer = ContainerInfo.Collection[containerId];
				}

				// create the order drop down list
				DropDownList order = new DropDownList();
				order.ID = "OrderList";
				order.Width = Unit.Percentage(95D);
				order.Items.AddRange(this.GetOrderItemList(currentContainer.Identity, Info.Containers.Count, currentContainer.GetOrder(Info)));
				order.AutoPostBack = true;
				order.SelectedIndexChanged += new EventHandler(ConnectedContainersGrid_ItemOrderChanged);

				// create the position drop down list
				DropDownList position = new DropDownList();
				position.ID = "PositionList";
				position.Width = Unit.Percentage(95D);
				position.Items.AddRange(this.GetContainerPositionItemList(currentContainer.Identity, currentContainer.GetPosition(Info)));
				position.AutoPostBack = true;
				position.SelectedIndexChanged += new EventHandler(ConnectedContainersGrid_ItemPositionChanged);

				// add order to the cell
				e.Item.Cells[3].Controls.Add(order);
				e.Item.Cells[4].Controls.Add(position);
			}
			else if (e.Item.ItemType == ListItemType.Footer)
			{
				if (ContainerInfo.Collection.Count == Info.Containers.Count)
				{
					int width = e.Item.Cells.Count;

					// clear all the cells
					e.Item.Cells.Clear();

					// create a new container link
					HyperLink newContainerLink = new HyperLink();
					newContainerLink.ID = "NewContainerLink";
					newContainerLink.NavigateUrl = ManagedFusion.Common.Path.GetPortalUrl("Container.aspx?id=-1").ToString();
					newContainerLink.Text = "Click Here To Create New Container";
					newContainerLink.ForeColor = Color.White;
					newContainerLink.Style.Add("text-transform", "lowercase");

					// create a new long cell
					TableCell newCell = new TableCell();
					newCell.ColumnSpan = width;
					newCell.Controls.Add(newContainerLink);

					// add cell back to table
					e.Item.Cells.Add(newCell);
				}
				else
				{
					// create the container drop down list
					DropDownList containers = new DropDownList();
					containers.ID = "ContainerList";
					containers.Width = Unit.Percentage(95D);
					containers.Items.AddRange(this.GetContainerItemList(Info.Containers));

					// create the order drop down list
					DropDownList order = new DropDownList();
					order.ID = "OrderList";
					order.Width = Unit.Percentage(95D);
					order.Items.AddRange(this.GetOrderItemList(ContainerInfo.TempIdentity, Info.Containers.Count + 1, Int32.MaxValue));

					// create the position drop down list
					DropDownList position = new DropDownList();
					position.ID = "PositionList";
					position.Width = Unit.Percentage(95D);
					position.Items.AddRange(this.GetContainerPositionItemList(ContainerInfo.TempIdentity, Int32.MaxValue));

					// create the button to add a new site
					LinkButton addButton = new LinkButton();
					addButton.ID = "AddButton";
					addButton.Text = this.GetGlobalResourceObject("OmniPortal", "Add") as string;
					addButton.ForeColor = Color.White;
					addButton.Style.Add("text-transform", "lowercase");
					addButton.CommandName = "Update";

					// add controls to the cells
					e.Item.Cells[1].Controls.Add(addButton);
					e.Item.Cells[2].Controls.Add(containers);
					e.Item.Cells[3].Controls.Add(order);
					e.Item.Cells[4].Controls.Add(position);
				}
			}
		}

		private void ConnectedContainersGrid_UpdateCommand(object source, DataGridCommandEventArgs e)
		{
			// get the drop down lists
			DropDownList containers = e.Item.Cells[2].FindControl("ContainerList") as DropDownList;
			DropDownList orderList = e.Item.Cells[3].FindControl("OrderList") as DropDownList;
			DropDownList positionList = e.Item.Cells[4].FindControl("PositionList") as DropDownList;

			// set the attributes of the connect
			ContainerInfo containerInfo = ContainerInfo.Collection[Convert.ToInt32(containers.SelectedValue)];
			int order = Convert.ToInt32(orderList.SelectedValue);
			int position = Convert.ToInt32(positionList.SelectedValue);

			// add the connection to the section
			Info.AddContainer(containerInfo, order, position);

			// give the datagrid the most recent data
			ConnectedContainersGrid.DataSource = Info.Containers;
			ConnectedContainersGrid.DataBind();
		}

		private void ConnectedContainersGrid_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			int containerId = (int)ConnectedContainersGrid.DataKeys[e.Item.DataSetIndex];
			ContainerInfo currentContainer = ContainerInfo.Collection[containerId];

			// remove the connection to the section
			Info.RemoveContainer(currentContainer);

			// give the datagrid the most recent data
			ConnectedContainersGrid.DataSource = Info.Containers;
			ConnectedContainersGrid.DataBind();
		}

		private void ConnectedContainersGrid_ItemOrderChanged(object sender, EventArgs e)
		{
			DropDownList orderList = sender as DropDownList;
			string[] selectedOrder = orderList.SelectedValue.Split(':');

			// get the container whose order is going to change
			ContainerInfo containerInfo = ContainerInfo.Collection[Convert.ToInt32(selectedOrder[0])];

			// get the order of the container
			int order = Convert.ToInt32(selectedOrder[1]);

			// commit the changes of this section to the database
			Info.UpdateContainer(containerInfo, order, containerInfo.GetPosition(Info));
		}

		private void ConnectedContainersGrid_ItemPositionChanged(object sender, EventArgs e)
		{
			DropDownList positionList = sender as DropDownList;
			string[] selectedPosition = positionList.SelectedValue.Split(':');

			// get the container whose order is going to change
			ContainerInfo containerInfo = ContainerInfo.Collection[Convert.ToInt32(selectedPosition[0])];

			// get the position of the container
			int position = Convert.ToInt32(selectedPosition[1]);

			// commit the changes of this section to the database
			Info.UpdateContainer(containerInfo, containerInfo.GetOrder(Info), position);
		}

		protected void sendButton_Click(object sender, System.EventArgs e)
		{
			// set the parent
			Info.Parent = SectionInfo.Collection[Convert.ToInt32(this.parentList.SelectedValue)];

			// set the module
			Info.Module = SectionModule.Collection[new Guid(this.moduleList.SelectedValue)] as SectionModule;

			// set the values of the section
			Info.Name = this.nameText.Text;
			Info.Title = this.titleText.Text;
			Info.Visible = this.visibleCheckBox.Checked;
			Info.Syndicated = this.syndicatedCheckBox.Checked;
			Info.OriginalTheme = this.themeList.SelectedValue;
			Info.OriginalStyle = this.styleList.SelectedValue;
			Info.OriginalOwner = this.ownerList.SelectedValue;

			// if the site is a temp site -- meaning it hasn't
			// yet been added to the database
			if (Info.Identity == SectionInfo.TempIdentity)
			{
				SectionInfo.Collection.Add(Info);
			}
			else
			{
				// set the connected community if the parent is the root section
				if (Info.Parent == null)
					Info.ConnectedCommunity = CommunityInfo.Collection[Convert.ToInt32(this.communityList.SelectedValue)];
			}

			// commit the changes of this section to the database
			Info.CommitChanges();

			// redirect after changes
			RedirectDefaultState();
		}

		protected void deleteButton_Click(object sender, System.EventArgs e)
		{
			Info.SetForDeletion(true);

			// commit the changes of this section to the database
			Info.CommitChanges();

			// redirect after changes
			RedirectDefaultState();
		}

		protected void cancelButton_Click(object sender, System.EventArgs e)
		{
			// redirect back to the default state
			RedirectDefaultState();
		}

		protected void sectionList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Redirect(this.sectionList.SelectedValue);
		}

		protected void themeList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.styleList.Items.Clear();
			this.styleList.Items.AddRange(this.GetStylesItemList(themeList.SelectedValue, Info.OriginalStyle));
		}

		private void rolesGrid_RoleAdded(object sender, OmniPortal.Controls.RolesGridEventArgs e)
		{
			// add role
			Info.AddRole(e.Role, e.Tasks);
		}

		private void rolesGrid_RoleRemoved(object sender, OmniPortal.Controls.RolesGridEventArgs e)
		{
			// remove role
			Info.RemoveRole(e.Role);
		}

		private void rolesGrid_RoleChanged(object sender, OmniPortal.Controls.RolesGridEventArgs e)
		{
			// update role
			Info.UpdateRole(e.Role, e.Tasks);
		}
	}
}
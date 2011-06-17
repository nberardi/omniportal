<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Section.ascx.cs" Inherits="OmniPortal.Communities.Default.Modules.Admin.Section" %>
<%@ Register TagPrefix="controls" Namespace="OmniPortal.Controls" Assembly="OmniPortal" %>
<h1>Section</h1>
<div style="WIDTH: 750px">
	<asp:panel runat="server" id="ListPanel" HorizontalAlign="Center" Visible="True">
		<asp:dropdownlist id="sectionList" AutoPostBack="True" Runat="server" onselectedindexchanged="sectionList_SelectedIndexChanged"></asp:dropdownlist>
		<br>-- or --<br>
		<a href="Section.aspx?id=-1&amp;parent=0">Create New Root Section</a>
	</asp:panel>
	<asp:panel runat="server" id="ItemPanel" Visible="False">
		<fieldset>
			<table border="0">
				<tr>
					<td width="200">ID</td>
					<td><asp:label id="typeID" Runat="server"></asp:label></td>
				</tr>
				<tr>
					<td>Parent</td>
					<td><asp:hyperlink ID="parentLink" Runat="server" Visible="False" /></td>
				</tr>
				<tr>
					<td>Last Touched</td>
					<td><asp:label id="lastTouched" Runat="server"></asp:label></td>
				</tr>
			</table>
		</fieldset>
		<fieldset>
			<legend>Path</legend>
			<table border="0">
				<tr>
					<td width="200">Parent Path</td>
					<td><asp:dropdownlist id="parentList" Runat="server"></asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td>Current Path</td>
					<td><asp:label id="currentPath" Runat="server"></asp:label></td>
				</tr>
			</table>
		</fieldset>
		<fieldset>
			<legend>Appearence</legend>
			<table border="0">
				<tr>
					<td width="200">Name</td>
					<td><asp:textbox id="nameText" Runat="server" MaxLength="32"></asp:textbox></td>
				</tr>
				<tr>
					<td>Title</td>
					<td><asp:textbox id="titleText" Runat="server" MaxLength="128"></asp:textbox></td>
				</tr>
				<tr>
					<td>Visible</td>
					<td><asp:checkbox id="visibleCheckBox" Runat="server"></asp:checkbox></td>
				</tr>
				<tr>
					<td>Syndicated</td>
					<td><asp:checkbox id="syndicatedCheckBox" Runat="server"></asp:checkbox></td>
				</tr>
				<tr>
					<td>Theme &amp; Style</td>
					<td>
						<asp:dropdownlist id="themeList" AutoPostBack="True" Runat="server" onselectedindexchanged="themeList_SelectedIndexChanged"></asp:dropdownlist><br>
						<asp:dropdownlist id="styleList" Runat="server"></asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td>Owner</td>
					<td><asp:dropdownlist id="ownerList" Runat="server"></asp:dropdownlist></td>
				</tr>
			</table>
		</fieldset>
		<fieldset>
			<legend>Module</legend>
			<table border="0" width="100%">
				<tr>
					<td width="200">Module</td>
					<td><asp:dropdownlist id="moduleList" Runat="server"></asp:dropdownlist></td>
				</tr>
				<tr id="RolesTableRow" runat="server">
					<td>Access Roles</td>
					<td><controls:rolesgrid id="rolesGrid" runat="server" Width="100%"></controls:rolesgrid></td>
				</tr>
			</table>
		</fieldset>
		<fieldset id="ConnectedFieldSet" runat="server">
			<legend>Connected</legend>
			<table border="0">
				<tr>
					<td width="200">Children</td>
					<td>
						<asp:datagrid id="ChildrenGrid" Runat="server" AutoGenerateColumns="False" BorderColor="#006699" BorderWidth="1px" BorderStyle="Solid" GridLines="None" BackColor="White" CellPadding="3" ShowHeader="True" ShowFooter="True" Width="550px">
							<headerstyle forecolor="White" backcolor="#006699" horizontalalign="Center"></headerstyle>
							<footerstyle forecolor="White" backcolor="#006699" horizontalalign="Center"></footerstyle>
							<alternatingitemstyle backcolor="#E0E0E0"></alternatingitemstyle>
							<itemstyle forecolor="#000066" horizontalalign="Center"></itemstyle>
							<columns>
								<asp:templatecolumn>
									<headerstyle width="50px" />
									<itemtemplate><asp:hyperlink ID="Hyperlink1" NavigateUrl='<%# ManagedFusion.Common.Path.GetPortalUrl("Section.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID")) %>' Runat="server">edit</asp:hyperlink></itemtemplate>
								</asp:templatecolumn>
								<asp:boundcolumn DataField="Name" ReadOnly="True" />
								<asp:boundcolumn DataField="OriginalOwner" ReadOnly="True" />
								<asp:boundcolumn DataField="Module" ReadOnly="True" />
								<asp:templatecolumn>
									<headerstyle width="50px" />
								</asp:templatecolumn>
							</columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr>
					<td>Community</td>
					<td><asp:dropdownlist id="communityList" AutoPostBack="True" Runat="server"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td>Sites</td>
					<td>
						<asp:datagrid id="ConnectedSitesGrid" Runat="server" AutoGenerateColumns="False" BorderColor="#006699" BorderWidth="1px" BorderStyle="Solid" GridLines="None" BackColor="White" CellPadding="3" ShowHeader="True" ShowFooter="True" Width="550px">
							<headerstyle forecolor="White" backcolor="#006699" horizontalalign="Center"></headerstyle>
							<footerstyle forecolor="White" backcolor="#006699" horizontalalign="Center"></footerstyle>
							<alternatingitemstyle backcolor="#E0E0E0"></alternatingitemstyle>
							<itemstyle forecolor="#000066" horizontalalign="Center"></itemstyle>
							<columns>
								<asp:templatecolumn>
									<headerstyle width="50px" />
									<itemtemplate><asp:hyperlink ID="Hyperlink2" NavigateUrl='<%# ManagedFusion.Common.Path.GetPortalUrl("Site.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID")) %>' Runat="server">edit</asp:hyperlink></itemtemplate>
								</asp:templatecolumn>
								<asp:boundcolumn DataField="SubDomain" ReadOnly="True">
									<headerstyle width="100px" />
									<itemstyle horizontalalign="Right" />
									<footerstyle horizontalalign="Right" />
								</asp:boundcolumn>
								<asp:templatecolumn><itemtemplate>.</itemtemplate></asp:templatecolumn>
								<asp:boundcolumn DataField="Domain" ReadOnly="True">
									<headerstyle width="400px" />
									<itemstyle horizontalalign="Left" />
									<footerstyle horizontalalign="Left" />
								</asp:boundcolumn>
							</columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr>
					<td>Containers</td>
					<td>
						<asp:datagrid id="ConnectedContainersGrid" Runat="server" AutoGenerateColumns="False" BorderColor="#006699" BorderWidth="1px" BorderStyle="Solid" GridLines="None" BackColor="White" CellPadding="3" ShowHeader="True" ShowFooter="True" Width="550px">
							<headerstyle forecolor="White" backcolor="#006699" horizontalalign="Center"></headerstyle>
							<footerstyle forecolor="White" backcolor="#006699" horizontalalign="Center"></footerstyle>
							<alternatingitemstyle backcolor="#E0E0E0"></alternatingitemstyle>
							<itemstyle forecolor="#000066" horizontalalign="Center"></itemstyle>
							<columns>
								<asp:templatecolumn>
									<headerstyle width="50px" />
									<itemtemplate><asp:hyperlink ID="Hyperlink3" NavigateUrl='<%# ManagedFusion.Common.Path.GetPortalUrl("Container.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID")) %>' Runat="server">edit</asp:hyperlink></itemtemplate>
								</asp:templatecolumn>
								<asp:templatecolumn>
									<headerstyle width="50px" />
									<itemtemplate><asp:linkbutton CommandName="Delete" Runat="server" ID="Linkbutton1">delete</asp:linkbutton></itemtemplate>
								</asp:templatecolumn>
								<asp:boundcolumn DataField="Title" ReadOnly="True" />
								<asp:templatecolumn>
									<headerstyle width="50px" />
								</asp:templatecolumn>
								<asp:templatecolumn>
									<headerstyle width="125px" />
								</asp:templatecolumn>
							</columns>
						</asp:datagrid>
					</td>
				</tr>
			</table>
		</fieldset>
		<div id="page-buttons" align="center">
			<asp:button id="sendButton" Runat="server" Text="Add" onclick="sendButton_Click"></asp:button>&nbsp;&nbsp;
			<asp:button id="deleteButton" Runat="server" Text="Delete" onclick="deleteButton_Click"></asp:button>&nbsp;&nbsp;
			<asp:button id="cancelButton" Runat="server" Text="Cancel" onclick="cancelButton_Click"></asp:button>
		</div>
	</asp:panel>
</div>
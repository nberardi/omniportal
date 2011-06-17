<%@ Control Language="c#" Inherits="OmniPortal.Modules.Admin.Container" Codebehind="Container.ascx.cs" %>
<h1>Container</h1>
<div style="width: 750px">
	<asp:panel runat="server" id="ListPanel" HorizontalAlign="Center" Visible="True">
	<asp:dropdownlist id="containerList" Runat="server" AutoPostBack="True"></asp:dropdownlist>
	<br>-- or --<br>
	<a href="Site.aspx?id=-1">Create New Container</a>
</asp:panel>
	<asp:panel runat="server" id="ItemPanel" Visible="False">
		<fieldset>
			<table border="0">
				<tr>
					<td width="200">ID</td>
					<td><asp:label id="typeID" Runat="server"></asp:label></td>
				</tr>
				<tr>
					<td>Last Touched</td>
					<td><asp:label id="lastTouched" Runat="server"></asp:label></td>
				</tr>
			</table>
		</fieldset>
		<fieldset>
			<legend>Appearence</legend>
			<table border="0">
				<tr>
					<td width="200">Title</td>
					<td><asp:textbox ID="titleText" Runat="server" MaxLength="128"></asp:textbox></td>
				</tr>
			</table>
		</fieldset>
		<fieldset id="ConnectedFieldSet" runat="server">
			<legend>Connected</legend>
			<table border="0">
				<tr>
					<td width="200">Portlets</td>
					<td>
						<asp:datagrid id="ConnectedPortletsGrid" Runat="server" AutoGenerateColumns="False" BorderColor="#006699" BorderWidth="1px" BorderStyle="Solid" GridLines="None" BackColor="White" CellPadding="3" ShowHeader="True" ShowFooter="True" Width="550px">
							<headerstyle forecolor="White" backcolor="#006699" horizontalalign="Center"></headerstyle>
							<footerstyle forecolor="White" backcolor="#006699" horizontalalign="Center"></footerstyle>
							<alternatingitemstyle backcolor="#E0E0E0"></alternatingitemstyle>
							<itemstyle forecolor="#000066" horizontalalign="Center"></itemstyle>
							<columns>
								<asp:templatecolumn>
									<headerstyle width="50px" />
									<itemtemplate><asp:hyperlink NavigateUrl='<%# ManagedFusion.Common.Path.GetPortalUrl("Container.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID")) %>' Runat="server" ID="Hyperlink1">edit</asp:hyperlink></itemtemplate>
								</asp:templatecolumn>
								<asp:templatecolumn>
									<headerstyle width="50px" />
									<itemtemplate><asp:linkbutton CommandName="Delete" Runat="server" ID="Linkbutton1">delete</asp:linkbutton></itemtemplate>
								</asp:templatecolumn>
								<asp:boundcolumn DataField="Title" ReadOnly="True" />
								<asp:templatecolumn>
									<headerstyle width="50px" />
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
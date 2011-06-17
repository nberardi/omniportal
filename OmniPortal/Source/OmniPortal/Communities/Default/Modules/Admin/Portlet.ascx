<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Portlet.ascx.cs" Inherits="OmniPortal.Communities.Default.Modules.Admin.Portlet" %>
<%@ Register TagPrefix="controls" Namespace="OmniPortal.Controls" %>
<h1>Portlet</h1>
<div style="width: 750px">
	<asp:panel runat="server" id="ListPanel" HorizontalAlign="Center" Visible="True">
	<asp:dropdownlist id="portletList" Runat="server" AutoPostBack="True"></asp:dropdownlist>
	<br>-- or --<br>
	<a href="Site.aspx?id=-1">Create New Portlet</a>
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
		<div id="page-buttons" align="center">
			<asp:button id="sendButton" Runat="server" Text="Add" onclick="sendButton_Click"></asp:button>&nbsp;&nbsp;
			<asp:button id="deleteButton" Runat="server" Text="Delete" onclick="deleteButton_Click"></asp:button>&nbsp;&nbsp;
			<asp:button id="cancelButton" Runat="server" Text="Cancel" onclick="cancelButton_Click"></asp:button>
		</div>
	</asp:panel>
</div>
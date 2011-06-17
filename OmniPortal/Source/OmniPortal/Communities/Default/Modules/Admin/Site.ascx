<%@ Control Language="c#" Inherits="OmniPortal.Modules.Admin.Site" Codebehind="Site.ascx.cs" %>
<h1>Site</h1>
<div style="WIDTH: 750px">
	<asp:panel runat="server" id="ListPanel" HorizontalAlign="Center" Visible="True">
<asp:dropdownlist id="siteList" AutoPostBack="True" Runat="server" onselectedindexchanged="siteList_SelectedIndexChanged"></asp:dropdownlist><br>-- 
or --<br><a href="Site.aspx?id=-1">Create New Site</a>
</asp:panel>
	<asp:panel runat="server" id="ItemPanel" Visible="False">
		<fieldset>
			<table border="0">
				<tr>
					<td width="200">ID</td>
					<td>
						<asp:label id="typeID" Runat="server"></asp:label></td>
				</tr>
				<tr>
					<td>Last Touched</td>
					<td>
						<asp:label id="lastTouched" Runat="server"></asp:label></td>
				</tr>
			</table>
		</fieldset>
		<fieldset><legend>Domain</legend>
			<table border="0">
				<tr>
					<td width="200">Sub Domain</td>
					<td>
						<asp:textbox id="subDomainText" Runat="server" MaxLength="128"></asp:textbox></td>
				</tr>
				<tr>
					<td>Domain</td>
					<td>
						<asp:textbox id="domainText" Runat="server" MaxLength="128"></asp:textbox></td>
				</tr>
			</table>
		</fieldset>
		<fieldset><legend>Appearence</legend>
			<table border="0">
				<tr>
					<td width="200">Theme &amp; Style</td>
					<td>
						<asp:dropdownlist id="themeList" AutoPostBack="True" Runat="server" onselectedindexchanged="themeList_SelectedIndexChanged"></asp:dropdownlist><br>
						<asp:dropdownlist id="styleList" Runat="server"></asp:dropdownlist></td>
				</tr>
			</table>
		</fieldset>
		<fieldset id="ConnectedFieldSet" runat="server"><legend>Connected</legend>
			<table border="0">
				<tr>
					<td width="200">Section</td>
					<td>
						<asp:dropdownlist id="sectionsList" Runat="server"></asp:dropdownlist></td>
				</tr>
			</table>
		</fieldset>
		<div id="page-buttons" align="center">
			<asp:button id="sendButton" Runat="server" Text="Add" onclick="sendButton_Click"></asp:button>&nbsp;&nbsp;
			<asp:button id="deleteButton" Runat="server" Text="Delete" onclick="deleteButton_Click"></asp:button>&nbsp;&nbsp;
			<asp:button id="cancelButton" Runat="server" Text="Cancel" onclick="cancelButton_Click"></asp:button></div>
	</asp:panel>
</div>

<%@ Control language="c#" Inherits="OmniPortal.Modules.Admin.Community" Codebehind="Community.ascx.cs" %>
<h1>Community</h1>
<div style="width: 750px">
<asp:panel runat="server" id="ListPanel" HorizontalAlign="Center" Visible="True">
	<asp:dropdownlist id="communityList" Runat="server" AutoPostBack="True" onselectedindexchanged="communityList_SelectedIndexChanged"></asp:dropdownlist>
	<br>-- or --<br>
	<a href="Community.aspx?id=-1">Create New Community</a>
</asp:panel>
<asp:panel runat="server" id="ItemPanel" Visible="False">
	<fieldset>
		<table border="0">
			<tr>
				<td width="200">ID</td>
				<td><asp:label id="typeID" Runat="server"></asp:label></td>
			</tr>
			<tr>
				<td>Global ID</td>
				<td><asp:label id="universalID" Runat="server"></asp:label></td>
			</tr>
			<tr>
				<td>Last Touched</td>
				<td><asp:label id="lastTouched" Runat="server"></asp:label></td>
			</tr>
		</table>
	</fieldset>
	<fieldset>
		<legend>Appearance</legend>
		<table border="0">
			<tr>
				<td width="200">Title</td>
				<td><asp:textbox id="titleText" Runat="server" MaxLength="64"></asp:textbox></td>
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
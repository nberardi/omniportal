<%@ Control Language="c#" Inherits="OmniPortal.Portlets.Login.Read" Codebehind="Read.ascx.cs" %>
<div align="center"><asp:Label ID="MessageLabel" Runat="server" Visible="False" Font-Bold="True" /></div>
<asp:Panel Runat="server" ID="LoggedIn" Visible="False" HorizontalAlign="Center">
	<asp:LinkButton id="LogoutButton" Runat="server" Text="Logout"></asp:LinkButton>
</asp:Panel>
<asp:Panel Runat="server" ID="LoggedOut">
	<TABLE border="0">
		<TR>
			<TD>Username:</TD>
			<TD>
				<asp:TextBox id="UsernameTextBox" Runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD>Password:</TD>
			<TD>
				<asp:TextBox id="PasswordTextBox" Runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD colSpan="2">
				<asp:CheckBox id="KeepLoggedInCheckBox" Runat="server" Text="Remember Me"></asp:CheckBox></TD>
		</TR>
		<TR>
			<TD align="center" colSpan="2">
				<asp:Button id="LoginButton" Runat="server" Text="Login"></asp:Button></TD>
		</TR>
	</TABLE>
</asp:Panel>

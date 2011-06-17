<%@ Control Language="c#" AutoEventWireup="false" Inherits="OmniPortal.Portlets.Login.Read" Codebehind="~/Communities/Default/Portlets/Login/read.ascx.cs" %>
<style type="text/css">
	#login { text-align: center }
	#login .field { display: inline-block; width: 75px }
	#login .value { width: 125px }
</style>
<div id="login">
	<asp:Label ID="MessageLabel" Runat="server" Visible="False" Font-Bold="True" />
	<asp:Panel Runat="server" ID="LoggedIn" Visible="False">
		<asp:LinkButton id="LogoutButton" Runat="server" Text="Logout"></asp:LinkButton>
	</asp:Panel>
	<asp:Panel Runat="server" ID="LoggedOut">
		<div class="line"><span class="field">Username:</span>&nbsp;<asp:TextBox id="UsernameTextBox" Runat="server" CssClass="value" /></div>
		<div class="line"><span class="field">Password:</span>&nbsp;<asp:TextBox id="PasswordTextBox" Runat="server" CssClass="value" TextMode="Password" /></div>
		<asp:CheckBox id="KeepLoggedInCheckBox" Runat="server" Text="Remember Me"></asp:CheckBox><br/>
		<asp:Button id="LoginButton" Runat="server" Text="Login"></asp:Button>
	</asp:Panel>
</div>
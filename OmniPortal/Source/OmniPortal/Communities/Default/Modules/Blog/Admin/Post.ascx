<%@ Control Language="C#" CodeBehind="Post.ascx.cs" Inherits="OmniPortal.Communities.Default.Modules.Blog.Admin.Post" %>
<div id="blog-admin">
	<script language="javascript" type="text/javascript">
		function ShowHide(collapseContentString) {
			collapseContentObject = document.getElementById(collapseContentString);
			
			if (collapseContentObject.style.display != "none")
				collapseContentObject.style.display = "none";
			else
				collapseContentObject.style.display = "";
		}
	</script>
	<h1><%=GetTitle()%></h1>
	<div class="error" id="ErrorMessage" runat="server"><%=ErrorMessage%></div>
	<h2>Title:</h2>
	<asp:textbox id="TitleText" Runat="server" Width="95%"></asp:textbox>
	<h2>Body:</h2>
	<asp:TextBox ID="BodyText" runat="server" Rows="5" TextMode="MultiLine" Width="95%" />
	<h2>Categories:</h2>
	<asp:checkboxlist id="CategoriesList" Runat="server" Width="95%" RepeatColumns="5" RepeatDirection="Horizontal"></asp:checkboxlist>
	<br />
	<asp:button id="PostButton" Runat="server" Text="Post" CssClass="blog-post" onclick="PostButton_Click"></asp:button>
	<div id="advanced-options">
		<span class="title"><a href="javascript:ShowHide('advanced-options-content')">Advanced Options</a></span>
		<div id="advanced-options-content" title="Advanced Options" style="DISPLAY: none">
			<p>
				<asp:checkbox id="PublishCheckBox" Text="Publish" runat="server" Checked="True"></asp:checkbox>
				<asp:checkbox id="AllowCommentsCheckBox" Text="Allow Comments" runat="server" Checked="True"></asp:checkbox>
				<asp:checkbox id="SyndicateCheckBox" Text="Syndicate" runat="server" Checked="True"></asp:checkbox>
			</p>
			<h2>Title URL:</h2>
			<asp:textbox id="TitleUrlText" Runat="server" Width="95%"></asp:textbox>
			<h2>Source Name:</h2>
			<asp:textbox id="SourceText" Runat="server" Width="95%"></asp:textbox>
			<h2>Source URL:</h2>
			<asp:TextBox ID="SourceUrlText" Width="95%" Runat="server" OnTextChanged="SourceUrlText_TextChanged"></asp:TextBox>
		</div>
	</div>
</div>

<%@ Control Language="c#" Inherits="OmniPortal.Modules.Blog.List" Codebehind="List.ascx.cs" %>
<h1><%= BlogTitle %></h1>
<div id="blog">
<asp:Repeater ID="BlogList" Runat="server">
<ItemTemplate>
	<!-- start post <%# DataBinder.Eval(Container.DataItem, "ID") %> -->
	<div class="post">
		<div class="post-header">
			<h2 class="post-title"><a href="<%# DataBinder.Eval(Container.DataItem, "TitleUrl") %>"><%# DataBinder.Eval(Container.DataItem, "Title") %></a></h2>
			<div class="post-date"><%# DataBinder.Eval(Container.DataItem, "Issued", "{0:D}") %></div>
		</div>
		<div class="post-body">
			<%# DataBinder.Eval(Container.DataItem, "Body") %>
		</div>
		<div class="post-footer">
			<% if (IsPoster) {%>
			<a href="<%# ManagedFusion.Common.Path.GetPortalUrl(String.Format("Admin/Edit{0}.aspx", DataBinder.Eval(Container.DataItem, "ID"))).ToString() %>">edit</a> :
			<% } %>
			posted by <%# DataBinder.Eval(Container.DataItem, "Poster.FullName") %>
			at <a href="<%# GetPostUrl(DataBinder.Eval(Container.DataItem, "ID").ToString()) %>"><%# DataBinder.Eval(Container.DataItem, "Issued", "{0:t}") %></a>
		</div>
	</div>
	<!-- end post <%# DataBinder.Eval(Container.DataItem, "ID") %> -->
</ItemTemplate>
<SeparatorTemplate>
	<div class="blog-break"></div>
</SeparatorTemplate>
</asp:Repeater>
</div>
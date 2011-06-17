<%@ Control Language="c#" Inherits="OmniPortal.Modules.Blog.Admin._Default" Codebehind="Default.ascx.cs" %>
<h1>Current Posts</h1>
<asp:DataGrid id="BlogList" CssClass="admin-table" runat="server" AllowPaging="True" Width="100%" AutoGenerateColumns="False" CellPadding="2" CellSpacing="0" GridLines="Horizontal">
	<ItemStyle CssClass="admin-table-row" />
	<Columns>
		<asp:TemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<a href="<%# ManagedFusion.Common.Path.GetPortalUrl(String.Format("Admin/Edit{0}.aspx", DataBinder.Eval(Container.DataItem, "ID"))).ToString() %>">edit</a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Created" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "Created", "{0:g}") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Published" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "Published") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Poster" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<a href="mailto:<%# DataBinder.Eval(Container.DataItem, "Poster.Email") %>"><%# DataBinder.Eval(Container.DataItem, "Poster.FullName") %></a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Title" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "Title") %>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>

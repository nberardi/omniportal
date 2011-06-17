<%@ Control Language="c#" AutoEventWireup="false" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<ul class="links" title="Admin Links">
	<li><a href="<%= ManagedFusion.Common.Path.GetPortalUrl("Admin/Default.aspx").ToString() %>">Admin Home</a></li>
	<li><a href="<%= ManagedFusion.Common.Path.GetPortalUrl("Admin/AddPost.aspx").ToString() %>">Add Post</a></li>
</ul>
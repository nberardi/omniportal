<%@ Control Language="c#" Inherits="OmniPortal.Modules.Blog.Post" Codebehind="Post.ascx.cs" %>
<div id="blog">
	<!-- start post <%= BlogPost.ID %> -->
	<div class="post">
		<div class="post-header">
			<h2 class="post-title"><a href="<%= BlogPost.TitleUrl %>"><%= BlogPost.Title %></a></h2>
			<div class="post-date"><%= BlogPost.Modified.ToLongDateString() %></div>
		</div>
		<div class="post-body">
			<%= BlogPost.Body %>
		</div>
		<%-- <div class="post-footer">
			posted by <%= BlogPost.Poster.FullName %>
			at <%= BlogPost.Modified.ToShortTimeString() %>
		</div> --%>
	</div>
	<!-- end post <%= BlogPost.ID %> -->
</div>
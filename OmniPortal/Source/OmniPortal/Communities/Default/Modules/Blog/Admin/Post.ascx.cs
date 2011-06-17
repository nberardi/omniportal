using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

// OmniPortal Classes
using OmniPortal.Modules.Blog.Data;
using OmniPortal.Modules.Blog.Admin;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Communities.Default.Modules.Blog.Admin
{
	public partial class Post : AdminUserControl
	{
		private string _type;
		private int _postID;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// if type is not set then the form is set to add a new post
			if (Request.QueryString["type"] == null)
			{
				_type = "add";
				_postID = -1;
			}
			else
			{
				_type = Request.QueryString["type"];

				// check to see if looking for a post id is nessisary
				if (_type.ToLower() != "add")
				{
					try { _postID = Int32.Parse(Request.QueryString["post"], System.Globalization.NumberStyles.Integer); }
					catch (SystemException)
					{
						ErrorMessage.InnerText = String.Format("The Post Identity, {0}, is not a valid integer.", Request.QueryString["post"]);
						_type = "add";
						_postID = -1;
					}
				}
				else
					_postID = -1;
			}

			if (Page.IsPostBack == false)
			{
				this.DataBind();
			}
		}

		#region DataBind

		public override void DataBind()
		{
			// if there was a post then fill in the fields
			if (this._postID > -1)
			{
				BlogItem item = this.DatabaseProvider.GetBlog(this._postID);

				if (item != null)
				{
					this.TitleText.Text = item.Title;
					this.PublishCheckBox.Checked = item.Published;
					this.AllowCommentsCheckBox.Checked = item.AllowComments;
					this.SyndicateCheckBox.Checked = item.AllowComments;

					if (Common.Path.GetPortalUrl(String.Format("/archive/{0}.aspx", item.ID)).ToString() != item.TitleUrl.ToString())
						this.TitleUrlText.Text = item.TitleUrl.ToString();

					this.SourceText.Text = item.Source;

					if (item.SourceUrl != null)
						this.SourceUrlText.Text = item.SourceUrl.ToString();

					this.BodyText.Value = item.Body;
				}
			}

			base.DataBind();
		}

		#endregion

		protected string GetTitle()
		{
			return _type + " Post";
		}

		protected override void OnPreRender(EventArgs e)
		{
			// show error message if it has a value
			ErrorMessage.Visible = ErrorMessage.InnerText.Length > 0;

			base.OnPreRender(e);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		#region PostButton_Click

		protected void PostButton_Click(object sender, System.EventArgs e)
		{
			//BlogItem currentRow;
			string title = this.TitleText.Text;
			string body = this.BodyText.Value;
			bool publish = this.PublishCheckBox.Checked;
			bool allowComments = this.AllowCommentsCheckBox.Checked;
			bool syndicate = this.SyndicateCheckBox.Checked;
			//Guid uid;
			Uri titleUrl = null;
			string source = null;
			Uri sourceUrl = null;

			// check TitleUrlText for a valid variable
			if (this.TitleUrlText.Text.Length > 0)
			{
				try { titleUrl = new Uri(this.TitleUrlText.Text); }
				catch (UriFormatException) { }
			}

			// check SourceText for a valid variable
			if (this.SourceText.Text.Length > 0)
				source = this.SourceText.Text;

			// check SourceUrlText for a valid variable
			if (this.SourceUrlText.Text.Length > 0)
			{
				try { sourceUrl = new Uri(this.SourceUrlText.Text); }
				catch (UriFormatException) { }
			}

			//switch(this._type.ToLower()) 
			//{
			//    case "add" :
			//        // set the user
			//        uid = UserInfo.Current.Identity;

			//        // create the blog currentRow
			//        currentRow = new BlogItem(
			//            -1,
			//            title,
			//            body,
			//            publish,
			//            allowComments,
			//            syndicate,
			//            uid,
			//            titleUrl,
			//            source,
			//            sourceUrl,
			//            DateTime.Now,
			//            (publish) ? DateTime.Now : DateTime.MaxValue,
			//            DateTime.Now
			//            );

			//        // add the blog post
			//        this.DatabaseProvider.AddBlog(currentRow);
			//        break;

			//    case "edit" :
			//        // set the user
			//        uid = UserInfo.Current.Identity;

			//        BlogItem olditem = this.DatabaseProvider.GetBlog(this._postID);

			//        // create the blog currentRow
			//        currentRow = new BlogItem(
			//            this._postID,
			//            title,
			//            body,
			//            publish,
			//            allowComments,
			//            syndicate,
			//            uid,
			//            titleUrl,
			//            source,
			//            sourceUrl,
			//            olditem.Created,
			//            (publish && olditem.Published == false) ? DateTime.Now : olditem.Issued,
			//            DateTime.Now
			//            );

			//        // add the blog post
			//        this.DatabaseProvider.UpdateBlog(currentRow);
			//        break;
			//}

			Response.Redirect(this.SectionInformation.UrlPath.ToString());
		}

		#endregion
	}
}
#region Copyright © 2004, Nicholas Berardi
/*
 * ManagedFusion (www.ManagedFusion.net) Copyright © 2004, Nicholas Berardi
 * All rights reserved.
 * 
 * This code is protected under the Common Public License Version 1.0
 * The license in its entirety at <http://opensource.org/licenses/cpl.php>
 * 
 * ManagedFusion is freely available from <http://www.ManagedFusion.net/>
 */
#endregion

using System;
using System.Data;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Modules.Blog.Data
{
	public class BlogItem
	{
		#region Private Fields

		private bool _published, _allowComments, _syndicate;
		private int _id;
		private DateTime _created, _issued, _modified;
		private Guid _user;
		private Uri _titleUrl, _sourceUrl;
		private string _title, _body, _source;
		//private UserProfile _profile;

		#endregion

		#region Constructors

		public BlogItem(
			int id,
			string title,
			string body,
			bool published,
			bool allowComments,
			bool syndicate,
			Guid user,
			Uri titleUrl,
			string source,
			Uri sourceUrl,
			DateTime created,
			DateTime issued,
			DateTime modified
			)
		{
			this._id = id;
			this._title = title;
			this._body = body;
			this._published = published;
			this._allowComments = allowComments;
			this._syndicate = syndicate;
			this._user = user;

			if (titleUrl == null && id != -1)
				this._titleUrl = Common.Path.GetPortalUrl(String.Format("./archive/{0}.aspx", id));
			else
				this._titleUrl = titleUrl;

			this._source = source;
			this._sourceUrl = sourceUrl;
			this._created = created;
			this._issued = issued;
			this._modified = modified;
		}

		#endregion

		#region Properties

		//public UserProfile Poster 
		//{ 
		//    get 
		//    {
		//        if (_profile == null)
		//            _profile = new UserProfile((CommunityInfo.Current.Config.GetProvider("Profile") as IProfileProviderHandler).GetProfile(this._user));
			
		//        return _profile;
		//    }
		//}

		public int ID { get { return this._id; } }

		public string Title { get { return this._title; } }

		public string Body { get { return this._body; } }

		public bool Published { get { return this._published; } }

		public bool AllowComments { get { return this._allowComments; } }

		public bool Syndicate { get { return this._syndicate; } }

		public Guid PosterID { get { return this._user; } }

		public Uri TitleUrl { get { return this._titleUrl; } }

		public string Source { get { return this._source; } }

		public Uri SourceUrl { get { return this._sourceUrl; } }

		public DateTime Created { get { return this._created; } }

		public DateTime Issued { get { return this._issued; } }

		public DateTime Modified { get { return this._modified; } }

		#endregion
	}
}
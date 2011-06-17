using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ManagedFusion.Syndication
{
	public class Source
	{
		private List<Person> _authors = new List<Person>();
		private List<Category> _categories = new List<Category>();
		private List<Person> _contributors = new List<Person>();
		private Generator _generator = new Generator(PortalProperties.PortalVersion, new Uri("http://www.managedfusion.com"), PortalProperties.PortalCopyright);
		private Uri _icon;
		private string _id;
		private List<Link> _links = new List<Link>();
		private Uri _logo;
		private Text _rights;
		private Text _subtitle;
		private Text _title;
		private DateTime _updated;
		private List<CustomField> _customFields = new List<CustomField>();

		public Source() { }

		public Source(string title, Uri id, DateTime updated)
			: this(new Text("text", title), id, updated)
		{ }

		public Source(Text title, Uri id, DateTime updated)
			: this(title, HttpUtility.UrlEncode(id.ToString()), updated)
		{ }

		public Source(string title, string id, DateTime updated)
			: this(new Text("text", title), id, updated)
		{ }

		public Source(Text title, string id, DateTime updated)
		{
			_title = title;
			_id = id;
			_updated = updated;
		}

		public List<Person> Authors 
		{
			get { return _authors; }
		}

		public IList<Category> Categories 
		{
			get { return _categories; }
		}

		public List<Person> Contributors 
		{
			get { return _contributors; }
		}

		public Generator Generator 
		{
			get { return _generator; }
			set { _generator = value; }
		}

		public Uri Icon
		{
			get { return _icon; }
			set { _icon = value; }
		}

		public string Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public List<Link> Links
		{
			get { return _links; }
		}

		public Uri Logo
		{
			get { return _logo; }
			set { _logo = value; }
		}

		public Text Rights
		{
			get { return _rights; }
			set { _rights = value; }
		}

		public Text SubTitle
		{
			get { return _subtitle; }
			set { _subtitle = value; }
		}

		public Text Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public DateTime Updated
		{
			get { return _updated; }
			set { _updated = value; }
		}

		public List<CustomField> CustomFields
		{
			get { return _customFields; }
		}

		public IEnumerable<Link> GetLinks(LinkRelationship relationship)
		{
			foreach (Link link in this.Links)
				if (link.Relationship == relationship)
					yield return link;
		}

		public IEnumerable<CustomField> GetCustomField(string prefix)
		{
			foreach (CustomField field in this.CustomFields)
				if (field.Prefix == prefix)
					yield return field;
		}

		public CustomField GetCustomField(string prefix, string name)
		{
			foreach (CustomField field in this.CustomFields)
				if (field.Prefix == prefix && field.Name == name)
					return field;

			return null;
		}
	}
}

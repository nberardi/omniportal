using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ManagedFusion.Syndication
{
	public class Entry
	{
		private List<Person> _authors = new List<Person>();
		private List<Category> _categories = new List<Category>();
		private Content _content;
		private List<Person> _contributors = new List<Person>();
		private string _id;
		private List<Link> _links = new List<Link>();
		private DateTime? _published;
		private Text _rights;
		private Source _source;
		private Text _summary;
		private Text _title;
		private DateTime _updated;
		private List<CustomField> _customFields = new List<CustomField>();
		private ChangeFrequency _changeFrequency = ChangeFrequency.NotDefined;
		private float? _priority;

		public Entry(string title, Uri id, DateTime updated)
			: this(title, id, updated, (string)null)
		{ }

		public Entry(string title, Uri id, DateTime updated, string content)
			: this(new Text("text", title), id, updated, new Content("text", content))
		{ }

		public Entry(string title, Uri id, DateTime updated, Content content)
			: this(new Text("text", title), id, updated, content)
		{ }

		public Entry(Text title, Uri id, DateTime updated)
			: this(title, id, updated, null)
		{ }

		public Entry(Text title, Uri id, DateTime updated, Content content)
			: this(title, HttpUtility.UrlEncode(id.ToString()), updated, content)
		{ }

		public Entry(string title, string id, DateTime updated)
			: this(title, id, updated, (string)null)
		{ }

		public Entry(string title, string id, DateTime updated, string content)
			: this(new Text("text", title), id, updated, new Content("text", content))
		{ }

		public Entry(string title, string id, DateTime updated, Content content)
			: this(new Text("text", title), id, updated, content)
		{ }

		public Entry(Text title, string id, DateTime updated)
			: this(title, id, updated, null)
		{ }

		public Entry(Text title, string id, DateTime updated, Content content)
		{
			_title = title;
			_id = id;
			_updated = updated;
			_content = content;
		}

		public List<Person> Authors 
		{
			get { return _authors; }
		}

		public IList<Category> Categories
		{
			get { return _categories; }
		}

		public Content Content 
		{
			get { return _content; }
			set { _content = value; }
		}

		public List<Person> Contributors 
		{
			get { return _contributors; }
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

		public DateTime? Published
		{
			get { return _published; }
			set { _published = value; }
		}

		public Text Rights
		{
			get { return _rights; }
			set { _rights = value; }
		}

		public Source Source
		{
			get { return _source; }
			set { _source = value; }
		}

		public Text Summary
		{
			get { return _summary; }
			set { _summary = value; }
		}

		public Text Title
		{
			get { return _title; }
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

		public ChangeFrequency ChangeFrequency
		{
			get { return _changeFrequency; }
			set { _changeFrequency = value; }
		}

		public float? Priority
		{
			get { return _priority; }
			set
			{
				if (value != null && (value < 0F || value > 1F))
					throw new ArgumentOutOfRangeException("value", "The value of Priority must be between 0.0 and 1.0.");

				_priority = value;
			}
		}

		public IEnumerable<Link> GetLinks (LinkRelationship relationship)
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

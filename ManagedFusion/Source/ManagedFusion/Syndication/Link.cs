using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication
{
	public class Link
	{
		private Uri _href;
		private LinkRelationship _relationship = LinkRelationship.Alternate;
		private string _type;
		private string _lanaguage;
		private string _title;
		private uint? _length;

		public Link(Uri href)
			: this(href, null, LinkRelationship.NotDefined, null)
		{ }

		public Link(Uri href, string title)
			: this(href, title, LinkRelationship.NotDefined, null)
		{ }

		public Link(Uri href, LinkRelationship relationship)
			: this(href, null, relationship, null)
		{ }

		public Link(Uri href, string title, LinkRelationship relationship)
			: this(href, title, relationship, null)
		{ }

		public Link(Uri href, string title, LinkRelationship relationship, string type)
		{
			_href = href;
			_title = title;
			_relationship = relationship;
			_type = type;
		}

		public Uri Href
		{
			get { return _href; }
			set { _href = value; }
		}

		public LinkRelationship Relationship
		{
			get { return _relationship; }
			set { _relationship = value; }
		}

		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public string Language
		{
			get { return _lanaguage; }
			set { _lanaguage = value; }
		}

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		[CLSCompliant(false)]
		public uint? Length
		{
			get { return _length; }
			set { _length = value; }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication
{
	public class Feed : Source
	{
		private List<Entry> _items = new List<Entry>();
		private ChangeFrequency _changeFrequency = ChangeFrequency.NotDefined;
		private float? _priority;

		public Feed(SectionInfo section)
			: this(section.Title, section.UrlPath, DateTime.Now)
		{ }

		public Feed(Text title, Uri id, DateTime updated)
			: base(title, id, updated)
		{ }

		public Feed(string title, Uri id, DateTime updated)
			: base(title, id, updated)
		{ }

		public Feed(string title, string id, DateTime updated)
			: base(title, id, updated)
		{ }

		public Feed(Text title, string id, DateTime updated)
			: base(title, id, updated)
		{ }

		public List<Entry> Items
		{
			get { return _items; }
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
	}
}

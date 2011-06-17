using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication
{
	public class Content
	{
		private string _type;
		private Uri _sourceUrl;
		private string _innerText;

		public Content(string type, string text)
		{
			_type = type;
			_innerText = text;
		}

		public Content(string type, Uri sourceUrl)
		{
			_type = type;
			_sourceUrl = sourceUrl;
		}

		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public Uri SourceUrl
		{
			get { return _sourceUrl; }
			set { _sourceUrl = value; }
		}

		public string InnerText
		{
			get { return _innerText; }
			set { _innerText = value; }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication
{
	public class Generator
	{
		private Uri _url;
		private Version _version;
		private string _innerText;

		public Generator(string description)
		{
			_innerText = description;
		}

		public Generator(Version version, Uri url, string text)
		{
			_version = version;
			_url = url;
			_innerText = text;
		}

		public Uri Url
		{
			get { return _url; }
			set { _url = value; }
		}

		public Version Version
		{
			get { return _version; }
			set { _version = value; }
		}

		public string InnerText
		{
			get { return _innerText; }
			set { _innerText = value; }
		}
	}
}

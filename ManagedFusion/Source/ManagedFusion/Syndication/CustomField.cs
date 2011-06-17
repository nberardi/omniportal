using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ManagedFusion.Syndication
{
	public class CustomField
	{
		private string _prefix;
		private string _name;
		private string _innerText;

		public CustomField(string prefix, string name, string text)
		{
			_prefix = prefix;
			_name = name;
			_innerText = text;
		}

		public string Prefix
		{
			get { return _prefix; }
			set { _prefix = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string InnerText
		{
			get { return _innerText; }
			set { _innerText = value; }
		}

		public override string ToString()
		{
			return _innerText;
		}
	}
}

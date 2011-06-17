using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication
{
	public class Text
	{
		private string _type;
		private string _innerText;

		public Text(string type, string text)
		{
			_type = type;
			_innerText = text;
		}

		public string Type
		{
			get { return _type; }
			set { _type = value; }
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

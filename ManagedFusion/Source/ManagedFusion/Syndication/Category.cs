using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication
{
	public class Category
	{
		private string _term;
		private Uri _schemeUrl;
		private string _label;

		public Category(string term)
		{
			_term = term;
		}

		public string Term
		{
			get { return _term; }
			set { _term = value; }
		}

		public Uri SchemeUrl
		{
			get { return _schemeUrl; }
			set { _schemeUrl = value; }
		}

		public string Label
		{
			get
			{
				if (String.IsNullOrEmpty(_label))
					return Term;

				return _label;
			}
			set { _label = value; }
		}
	}
}

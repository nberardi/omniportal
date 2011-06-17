using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication
{
	public class Person
	{
		private string _name;
		private string _email;
		private Uri _link;

		public Person(string name)
		{
			_name = name;
		}

		public Person(string name, string email)
			: this(name)
		{
			_email = email;
		}

		public Person(string name, string email, Uri link)
			: this(name, email)
		{
			_link = link;
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Email
		{
			get { return _email; }
			set { _email = value; }
		}

		public Uri Link
		{
			get { return _link; }
			set { _link = value; }
		}
	}
}

#region Copyright © 2004, Nicholas Berardi
/*
 * ManagedFusion (www.ManagedFusion.net) Copyright © 2004, Nicholas Berardi
 * All rights reserved.
 * 
 * This code is protected under the Common Public License Version 1.0
 * The license in its entirety at <http://opensource.org/licenses/cpl.php>
 * 
 * ManagedFusion is freely available from <http://www.ManagedFusion.net/>
 */
#endregion

using System;
using System.Web;
using System.Xml.Serialization;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace ManagedFusion.Modules.Configuration
{
	public class ConfigurationPage
	{
		#region Properties

		#region Xml Attributes

		private bool _redirect;
		[XmlAttribute("redirect")]
		public bool Redirect 
		{
			get { return _redirect; }
			set { _redirect = value; }
		}

		private string _pattern;
		[XmlAttribute("pattern")]
		public string Pattern 
		{ 
			get { return _pattern; }
			set { _pattern = value; }
		}

		private string _control;
		[XmlAttribute("control")]
		public string Control 
		{ 
			get { return _control; }
			set { _control = value; }
		}

		private string _access;
		[XmlAttribute("access")]
		public string Access 
		{ 
			get { return _access; }
			set { _access = value; }
		}

		private string _handler;
		[XmlAttribute("handler")]
		public string Handler 
		{ 
			get { return _handler; }
			set { _handler = value; }
		}

		private string _transform;
		[XmlAttribute("transform")]
		public string Transform 
		{ 
			get { return _transform; }
			set { _transform = value; }
		}

		#endregion

		[XmlIgnore]
		public IHttpHandler HandlerObject
		{
			get 
			{
				try 
				{
					Type type = Type.GetType(Handler, false, true);

					// check to see if the set type inherits from IHttpHandler
					if (type.IsSubclassOf(typeof(IHttpHandler)) == false)
						throw new InvalidCastException("Page.Handler must use the interface IHttpHandler.");
				
					return (IHttpHandler)Activator.CreateInstance(type);
				} 
				catch (NullReferenceException exc) 
				{
					throw new TypeInitializationException(Handler, exc);
				}
			}
		}

		private Regex _pageRegex;
		[XmlIgnore]
		protected Regex Regex
		{
			get
			{
				if(_pageRegex == null)
					_pageRegex = new Regex(this.Pattern,RegexOptions.IgnoreCase|RegexOptions.Compiled);

				return _pageRegex;
			}
		}

		[XmlIgnore]
		public string[] Controls 
		{
			get { return Control.Split(Common.Delimiter); } 
		}

		[XmlIgnore]
		public string[] Tasks 
		{
			get 
			{
				if (Access != null)
					return Access.Split(Common.Delimiter); 
				else
					return new string[] { ConfigurationTask.ViewPageName };
			}
		}

		#endregion

		public bool IsMatch (string path)
		{
			return Regex.IsMatch(path);
		}

		public string TransformPath (string path) 
		{
			return Regex.Replace(path.ToLower(), this.Pattern.ToLower(), this.Transform);
		}
	}
}
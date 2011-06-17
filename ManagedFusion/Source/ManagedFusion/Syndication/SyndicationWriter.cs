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
using System.IO;
using System.Text;
using System.Xml;

namespace ManagedFusion.Syndication
{
	public class SyndicationWriter : XmlTextWriter
	{
		#region Utf8Writer Class

		private class UTF8Writer : StringWriter
		{
			public UTF8Writer(StringBuilder builder) : base(builder) { }

			public override Encoding Encoding
			{
				get { return Encoding.UTF8; }
			}
		}

		#endregion

		public SyndicationWriter(StringBuilder builder)
			: base(new UTF8Writer(builder))
		{
			this.Formatting = Formatting.Indented;
			this.Indentation = 1;
			this.IndentChar = (char)ConsoleKey.Tab;
		}
	}
}
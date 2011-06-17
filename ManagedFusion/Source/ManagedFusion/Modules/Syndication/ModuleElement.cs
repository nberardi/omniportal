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

using Atom.AdditionalElements;

namespace ManagedFusion.Modules.Syndication
{
	internal class ModuleElement : ScopedElement
	{
		public ModuleElement(string name) : this(name, String.Empty) { }

		public ModuleElement(string name, string content)
		{
			this.LocalName = name;
			this.Content = content;
		}

		public override string NamespacePrefix { get { return "mf"; } }

		public override Uri NamespaceUri { get { return new Uri("http://tempurl.org"); } }
	}
}
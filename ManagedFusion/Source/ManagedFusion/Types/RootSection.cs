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
using System.Collections.Generic;

namespace ManagedFusion
{
	/// <summary>
	/// Parent to all Primary Parent classes.
	/// </summary>
	/// <remarks>
	/// <para>The purpose of this class be a parent for all the sections that contain an Identity of 0 (zero).</para>
	/// <note>A <see cref="SectionInfo"/> that contains 0 (zero) is known as a Primary Parent.</note>
	/// </remarks>
	public sealed class RootSection
	{
		public const int Identity = 0;

		static RootSection ()
		{
			SetupEvents();
		}

		internal RootSection () { }

		private static void SetupEvents ()
		{
			Common.DatabaseProvider.SectionsChanged += new EventHandler(InvalidateExternalSectionsCollections);
		}

		private static SectionCollection _Children;
		/// <summary>The children of the current section.</summary>
		public static SectionCollection Children
		{
			get
			{
				if (_Children != null)
					return _Children;

				List<SectionInfo> list = new List<SectionInfo>();

				foreach (SectionInfo section in SectionInfo.Collection) {
					if (section.ParentID == Identity)
						list.Add(section);
				}

				_Children = new SectionCollection(list.ToArray());

				return _Children;
			}
		}

		private static void InvalidateExternalSectionsCollections (object sender, EventArgs e)
		{
			_Children = null;
		}
	}
}
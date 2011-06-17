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
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace ManagedFusion
{
    public class SectionCollection : PortalTypeCollection<SectionInfo>
    {
		public SectionCollection(SectionInfo[] sections) : base(sections) { }

        public SectionInfo this[string name]
        {
            get
            {
                // change name to lower format to normalize compare
                name = name.ToLower();

                foreach (SectionInfo section in this.Collection)
                {
                    if (section.Name.ToLower() == name)
                        // section found and returned
                        return section;
                }

                // section not found and nothing returned
                return null;
            }
        }

		public bool Contains(string name)
		{
			return (this[name] != null);
		}

		public override void CommitChanges()
		{
			Common.DatabaseProvider.Sections = this;
		}

        public override void Add(SectionInfo section)
        {
            if (this.Contains(section.Identity) == false)
            {
                section.SetState(State.Added);

				SectionInfo[] newSections = new SectionInfo[this.Collection.Length + 1];
                this.CopyTo(newSections, 0);
                newSections[newSections.Length - 1] = section;
				this.Collection = newSections;
            }
        }

        public override void Remove(SectionInfo section)
        {
            if (this.Contains(section.Identity) == true)
            {
                section.SetState(State.Deleted);

                // notify subscribers of change
                Common.DatabaseProvider.OnSectionsChanged();
            }
		}

		#region SiteMap

		public static implicit operator SiteMapNodeCollection(SectionCollection sections)
		{
			SiteMapNodeCollection collection = new SiteMapNodeCollection(sections.Count);

			foreach (SectionInfo section in sections)
				collection.Add(section);

			return collection;
		}

		#endregion
	}
}
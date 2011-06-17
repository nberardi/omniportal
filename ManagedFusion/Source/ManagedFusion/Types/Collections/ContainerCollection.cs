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
using System.Data;
using System.Collections;

namespace ManagedFusion
{
	public class ContainerCollection : PortalTypeCollection<ContainerInfo>
	{
		private SectionInfo _section;

		public ContainerCollection(ContainerInfo[] containers) : this(containers, null) { }

		public ContainerCollection (ContainerInfo[] containers, SectionInfo section) : base (containers)
		{
			this._section = section;
		}

		public override void CommitChanges()
		{
			Common.DatabaseProvider.Containers = this;
		}

		public override void Add(ContainerInfo container)
		{
			if (this.Contains(container.Identity) == false)
			{
				container.SetState(State.Added);

				ContainerInfo[] newContainers = new ContainerInfo[this.Collection.Length + 1];
				this.CopyTo(newContainers, 0);
				newContainers[newContainers.Length - 1] = container;
				this.Collection = newContainers;
			}
		}

		public override void Remove(ContainerInfo container)
		{
			if (this.Contains(container.Identity) == true)
			{
				container.SetState(State.Deleted);

				// notify subscribers of change
				Common.DatabaseProvider.OnContainersChanged();
			}
		}

		public ContainerCollection GetContainersForPosition (int position) 
		{
			if (this._section == null) throw new ApplicationException("Cannot get positions in the Global Container Collection.");

			// create the cache key
			string cacheKey = String.Format("{0}-Position_For_Section-{1}-Containers", position, this._section.Identity);
			int[] containers;

			// check to see if the value is cached and if it isn't cache the value
			if (Common.Cache.IsCached(cacheKey, String.Empty) == false) 
			{
				Common.Cache.Add(cacheKey, String.Empty, Common.DatabaseProvider.GetContainersInPositionForSection(this._section, position));
			}

			// get the containers from cache
			containers = Common.Cache[cacheKey, String.Empty] as int[];

			ArrayList list = new ArrayList();

			// if there are containes get the values
			if (containers != null)
				foreach(int id in containers)
					list.Add(this[id]);

			return new ContainerCollection(list.ToArray(typeof(ContainerInfo)) as ContainerInfo[], this._section);
		}
	}
}
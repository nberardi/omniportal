using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Data.SqlServer2000
{
	public partial class Container
	{
		public static implicit operator ContainerInfo(Container c)
		{
			return new ContainerInfo(
				c._containerID,
				c._description,
				c._touched
				);
		}

		public static explicit operator Container(ContainerInfo c)
		{
			Container container = new Container();
			container._containerID = c.Identity;
			container._name = c.Title;
			container._description = c.Title;
			container._touched = c.Touched;
			return container;
		}
	}
}

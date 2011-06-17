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
using System.ComponentModel;

namespace ManagedFusion
{
	/// <summary>
	/// Summary description for Container.
	/// </summary>
	[Serializable]
	public class ContainerInfo : PortalType
	{
		#region Static

		public static ContainerCollection Collection
		{
			get { return Common.DatabaseProvider.Containers; }
		}

		public static ContainerInfo CreateNew ()
		{
			ContainerInfo container = new ContainerInfo();
			container.SetState(State.Added);

			return container;
		}

		public static void AddContainer (ContainerInfo container)
		{
			Collection.Add(container);
		}

		public static void RemoveContainer (ContainerInfo container)
		{
			Collection.Remove(container);
		}

		#endregion

		#region Fields

		private readonly int _id;
		private string _title;
		private DateTime _touched;

		#endregion

		#region Constructors

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ContainerInfo (int id, string title, DateTime touched)
		{
			this._id = id;
			this._title = title;
			this._touched = touched;

			// setup events
			this.SetupEvents();
		}

		private ContainerInfo (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
			this._id = info.GetInt32("id");
			this._title = info.GetString("title");
			this._touched = info.GetDateTime("touched");

			// setup events
			this.SetupEvents();
		}

		private ContainerInfo ()
		{
			this._id = TempIdentity;
			this._touched = DateTime.Now;

			// setup events
			this.SetupEvents();
		}

		private void SetupEvents ()
		{
			Common.DatabaseProvider.ContainersChanged += new EventHandler(InvalidateExternalPortletsCollections);
		}

		#endregion

		#region Properties

		public override int Identity { get { return this._id; } }

		public string Title
		{
			get { return this._title; }
			set
			{
				this._title = value;
				this.ValueChanged();
			}
		}

		public override DateTime Touched
		{
			get { return this._touched; }
			set { this._touched = value; }
		}

		private PortletCollection _Portlets;
		public PortletCollection Portlets
		{
			get
			{
				if (this._Portlets != null)
					return this._Portlets;

				List<PortletInfo> list = new List<PortletInfo>();

				// _Portlets was null populate the variable
				foreach (int id in Common.DatabaseProvider.GetPortletsForContainer(this)) {
					PortletInfo p = PortletInfo.Collection[id, true];

					// if p was found and is enabled
					if (p != null)
						list.Add(p);
				}

				this._Portlets = new PortletCollection(list.ToArray());
				return this._Portlets;
			}
		}

		#endregion

		#region Methods

		public int GetOrder (SectionInfo section)
		{
			return Common.DatabaseProvider.GetSectionContainerLinkOrder(section, this);
		}

		public int GetPosition (SectionInfo section)
		{
			return Common.DatabaseProvider.GetSectionContainerLinkPosition(section, this);
		}

		public void AddPortlet (PortletInfo portlet, int order)
		{
			// add portlet to this container
			Common.DatabaseProvider.AddContainerPortletLink(this, portlet, order);

			// reset portlets collection
			this._Portlets = null;
		}

		public void UpdatePortlet (PortletInfo portlet, int order)
		{
			// update portlet to this container
			Common.DatabaseProvider.UpdateContainerPortletLink(this, portlet, order);
		}

		public void RemovePortlet (PortletInfo portlet)
		{
			// remove portlet from this container
			Common.DatabaseProvider.RemoveContainerPortletLink(this, portlet);

			// reset portlets collection
			this._Portlets = null;
		}

		private void InvalidateExternalPortletsCollections (object sender, EventArgs e)
		{
			this._Portlets = null;
		}

		protected override void CommitChangesToDatabase ()
		{
			Common.DatabaseProvider.CommitContainerChanges(this);
			Common.DatabaseProvider.ResetContainerCollection();
		}

		public override void GetObjectData (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("id", this._id);
			info.AddValue("title", this._title);
			info.AddValue("touched", this._touched);
		}

		#endregion
	}
}
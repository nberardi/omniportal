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
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Data;

namespace ManagedFusion.Data.SqlServer2000
{
	public class SqlServerProvider : PortalDatabaseProvider
	{
		#region Portlets

		private ManagedFusion.PortletCollection _Portlets;
		public override ManagedFusion.PortletCollection Portlets
		{
			#region get
			get
			{
				if (this._Portlets != null) return this._Portlets;

				// get all portlets
				List<Portlet> database = Portlet.GetList();

				// create protlet array
				PortletInfo[] portlets = new PortletInfo[database.Count];

				// create protlet for each row
				for (int i = 0; i < database.Count; i++)
					portlets[i] = database[i];

				this._Portlets = new ManagedFusion.PortletCollection(portlets);
				return this._Portlets;
			}
			#endregion
			#region set
			set
			{
				bool changed = false;

				IEnumerator<PortletInfo> enumerator = value.ChangedItems;

				while (enumerator.MoveNext()) {
					PortletInfo portlet = enumerator.Current;
					changed = true;

					this.CommitPortletChanges(portlet);
				}

				// update collection if changed
				if (changed) 
				{
					this.ResetPortletCollection();
				}
			}
			#endregion
		}

		public override void CommitPortletChanges(PortletInfo portlet)
		{
			switch (portlet.State) 
			{
				case State.Added :		// insert 
					Portlet.Insert((Portlet)portlet);
					break;
				case State.Changed :	// commit changes
					Portlet.Update((Portlet)portlet);
					break;
				case State.Deleted :	// remove
					Portlet.Delete((Portlet)portlet);
					break;
			}
		}

		protected override void ResetPortletCollection()
		{
			this._Portlets = null;
			this._Portlets = this.Portlets;

			// notify subscribers of change
			this.OnPortletsChanged();
		}

		#endregion

		#region Containers

		private ManagedFusion.ContainerCollection _Containers;
		public override ManagedFusion.ContainerCollection Containers
		{
			#region get
			get
			{
				if (this._Containers != null) return this._Containers;

				// get all containers
				List<Container> database = Container.GetList();

				// create container array
				ContainerInfo[] containers = new ContainerInfo[database.Count];

				// create container for each row
				for (int i = 0; i < database.Count; i++)
					containers[i] = database[i];

				this._Containers = new ManagedFusion.ContainerCollection(containers);
				return this._Containers;
			}
			#endregion
			#region set
			set
			{
				bool changed = false;

				IEnumerator<ContainerInfo> enumerator = value.ChangedItems;

				while (enumerator.MoveNext()) {
					ContainerInfo container = enumerator.Current;
					changed = true;

					this.CommitContainerChanges(container);
				}

				// update collection if changed
				if (changed) 
				{
					this.ResetContainerCollection();
				}
			}
			#endregion
		}

		public override void CommitContainerChanges(ContainerInfo container)
		{
			switch (container.State) 
			{
				case State.Added :		// insert 
					Container.Insert((Container)container);
					break;
				case State.Changed :	// commit changes
					Container.Update((Container)container);
					break;
				case State.Deleted :	// remove
					Container.Delete((Container)container);
					break;
			}
		}

		protected override void ResetContainerCollection()
		{
			this._Containers = null;
			this._Containers = this.Containers;

			// notify subscribers of change
			this.OnContainersChanged();
		}

		#endregion

		#region Sections

		private ManagedFusion.SectionCollection _Sections;
		public override ManagedFusion.SectionCollection Sections
		{
			#region get
			get
			{
				if (this._Sections != null) return this._Sections;

				// get all sections
				List<Section> database = Section.GetList();

				// create section array
				SectionInfo[] sections = new SectionInfo[database.Count];

				// create section for each row
				for(int i = 0; i < database.Count; i++)
					sections[i] = database[i];

				this._Sections = new ManagedFusion.SectionCollection(sections);
				return this._Sections;
			}
			#endregion
			#region set
			set
			{
				bool changed = false;

				IEnumerator<SectionInfo> enumerator = value.ChangedItems;

				while (enumerator.MoveNext()) {
					SectionInfo section = enumerator.Current;
					changed = true;

					this.CommitSectionChanges(section);
				}

				// update collection if changed
				if (changed) 
				{
					this.ResetSectionCollection();
				}
			}
			#endregion
		}

		public override void CommitSectionChanges(SectionInfo section)
		{
			switch (section.State) 
			{
				case State.Added :		// insert 
					Section.Insert((Section)section);
					break;
				case State.Changed :	// commit changes
					Section.Update((Section)section);
					break;
				case State.Deleted :	// remove
					Section.Delete((Section)section);
					break;
			}
		}

		protected override void ResetSectionCollection()
		{
			this._Sections = null;
			this._Sections = this.Sections;

			// notify subscribers of change
			this.OnSectionsChanged();
		}

		#endregion

		#region Communities

		private ManagedFusion.CommunityCollection _Communities;
		public override ManagedFusion.CommunityCollection Communities
		{
			#region get
			get
			{
				if (_Communities != null) return _Communities;

				// get all Communities
				List<Community> database = Community.GetList();

				// create community array
				CommunityInfo[] communities = new CommunityInfo[database.Count];

				// create community for each row
				for (int i = 0; i < database.Count; i++)
					communities[i] = database[i];

				this._Communities = new ManagedFusion.CommunityCollection(communities);
				return this._Communities;
			}
			#endregion
			#region set
			set
			{
				bool changed = false;

				IEnumerator<CommunityInfo> enumerator = value.ChangedItems;

				while(enumerator.MoveNext()) {
					CommunityInfo community = enumerator.Current;
					changed = true;

					this.CommitPortalChanges(community);
				}

				// update collection if changed
				if (changed) 
				{
					this.ResetCommunityCollection();
				}
			}
			#endregion
		}

		public override void CommitPortalChanges(CommunityInfo community)
		{
			switch (community.State) 
			{
				case State.Added :		// insert 
					Community.Insert((Community)community);
					break;
				case State.Changed :	// commit changes
					Community.Update((Community)community);
					break;
				case State.Deleted :	// remove
					Community.Delete((Community)community);
					break;
			}
		}

		protected override void ResetCommunityCollection()
		{
			this._Communities = null;
			this._Communities = this.Communities;

			// notify subscribers of change
			this.OnCommunitiesChanged();
		}

		#endregion

		#region Sites

		private ManagedFusion.SiteCollection _Sites;
		public override ManagedFusion.SiteCollection Sites
		{
			#region get
			get
			{
				if (_Sites != null) return _Sites;

				// get all sites
				List<Site> database = Site.GetList();

				// create site array
				SiteInfo[] sites = new SiteInfo[database.Count];

				// create site for each row
				for (int i = 0; i < database.Count; i++)
					sites[i] = database[i];

				this._Sites = new ManagedFusion.SiteCollection(sites);
				return this._Sites;
			}
			#endregion
			#region set
			set
			{
				bool changed = false;

				IEnumerator<SiteInfo> enumerator = value.ChangedItems;

				while(enumerator.MoveNext()) {
					SiteInfo site = enumerator.Current;
					changed = true;

					this.CommitSiteChanges(site);
				}

				// update collection if changed
				if (changed) 
				{
					this.ResetSiteCollection();
				}
			}
			#endregion
		}

		public override void CommitSiteChanges(SiteInfo site)
		{
			switch (site.State) 
			{
				case State.Added :		// insert 
					Site.Insert((Site)site);
					break;
				case State.Changed :	// commit changes
					Site.Update((Site)site);
					break;
				case State.Deleted :	// remove
					Site.Delete((Site)site);
					break;
			}
		}

		protected override void ResetSiteCollection()
		{
			this._Sites = null;
			this._Sites = this.Sites;

			// notify subscribers of change
			this.OnSitesChanged();
		}

		#endregion

		#region Properties

		#region SectionModuleData

		public override SectionModuleDataCollection GetModuleDataForSection (SectionInfo section)
		{
			List<SectionProperty> properties = SectionProperty.GetList("SectionID = " + section.Identity + " and PropertyGroup = '" + SectionModuleDataGroup + "'");
			
			// get all properties for section
			NameValueCollection collection = new NameValueCollection(properties.Count);

			// add all rows to NameValueCollection
			foreach (SectionProperty property in properties)
				collection.Add(
					property.Name,
					property.Value
					);

			// returns a property collection
			return new SectionModuleDataCollection(section, collection);
		}

		public override void AddModuleDataForSection (string name, string value, SectionInfo section)
		{
			SectionProperty property = new SectionProperty(
				section.Identity,
				SectionModuleDataGroup,
				name,
				value
				);

			try 
			{
				// insert new property
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void UpdateModuleDataForSection (string name, string value, SectionInfo section)
		{
			SectionProperty property = SectionProperty.GetBySectionIDAndPropertyGroupAndName(
				section.Identity,
				SectionModuleDataGroup,
				name
				);
			property.Value = value;

			try 
			{
				// update property
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void RemoveModuleDataForSection (string name, SectionInfo section)
		{
			SectionProperty property = SectionProperty.GetBySectionIDAndPropertyGroupAndName(
				section.Identity,
				SectionModuleDataGroup,
				name
				);		
			try 
			{
				// delete property
				property.Delete();
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		#endregion

		#region PortletModuleData

		public override PortletModuleDataCollection GetModuleDataForPortlet (PortletInfo portlet)
		{
			List<PortletProperty> properties = PortletProperty.GetList("PortletID = " + portlet.Identity + " and PropertyGroup = '" + PortletModuleDataGroup + "'");
			NameValueCollection collection = new NameValueCollection(properties.Count);

			// add all rows to NameValueCollection
			foreach (PortletProperty property in properties)
				collection.Add(
					property.Name,
					property.Value
					);

			// returns a property collection
			return new ManagedFusion.PortletModuleDataCollection(portlet, collection);
		}

		public override void AddModuleDataForPortlet (string name, string value, PortletInfo portlet)
		{
			PortletProperty property = new PortletProperty(
				portlet.Identity,
				PortletModuleDataGroup,
				name,
				value
				);

			try 
			{
				// insert new property
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void UpdateModuleDataForPortlet (string name, string value, PortletInfo portlet)
		{
			PortletProperty property = PortletProperty.GetByPortletIDAndPropertyGroupAndName(
				portlet.Identity,
				PortletModuleDataGroup,
				name
				);
			property.Value = value;

			try 
			{
				// update property
				property.CommitChanges();
			} 
			catch (Exception) { }
		}
		
		public override void RemoveModuleDataForPortlet (string name, PortletInfo portlet)
		{
			PortletProperty property = PortletProperty.GetByPortletIDAndPropertyGroupAndName(
				portlet.Identity,
				PortletModuleDataGroup,
				name
				);

			try 
			{
				// delete property
				property.Delete();
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		#endregion

		#region SectionMetaProperties

		public override SectionMetaPropertyCollection GetMetaPropertiesForSection (SectionInfo section)
		{
			List<SectionProperty> properties = SectionProperty.GetList("SectionID = " + section.Identity + " and PropertyGroup = '" + SectionMetaPropertiesGroup + "'");
			NameValueCollection collection = new NameValueCollection(properties.Count);

			// add all rows to NameValueCollection
			foreach (SectionProperty property in properties)
				collection.Add(
					property.Name,
					property.Value
					);

			// returns a property collection
			return new ManagedFusion.SectionMetaPropertyCollection(section, collection);
		}

		public override void AddMetaPropertyForSection (string name, string value, SectionInfo section)
		{
			SectionProperty property = new SectionProperty(
				section.Identity,
				SectionMetaPropertiesGroup,
				name,
				value
				);

			try 
			{
				// insert new property
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void UpdateMetaPropertyForSection (string name, string value, SectionInfo section)
		{
			SectionProperty property = SectionProperty.GetBySectionIDAndPropertyGroupAndName(
				section.Identity,
				SectionMetaPropertiesGroup,
				name
				);
			property.Value = value;

			try 
			{
				// update property
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void RemoveMetaPropertyForSection (string name, SectionInfo section)
		{
			SectionProperty property = SectionProperty.GetBySectionIDAndPropertyGroupAndName(
				section.Identity,
				SectionMetaPropertiesGroup,
				name
				);
		
			try 
			{
				// delete property
				property.Delete();
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		#endregion

		#region CommunityGeneralProperties

		public override ManagedFusion.CommunityPropertyCollection GetGeneralPropertiesForCommunity(CommunityInfo community)
		{
			List<CommunityProperty> properties = CommunityProperty.GetList("CommunityID = " + community.Identity + " and PropertyGroup = '" + CommunityGeneralPropertiesGroup + "'");
			NameValueCollection collection = new NameValueCollection(properties.Count);

			// add all rows to NameValueCollection
			foreach (CommunityProperty property in properties)
				collection.Add(
					property.Name,
					property.Value
					);

			// returns a property collection
			return new ManagedFusion.CommunityPropertyCollection(community, collection);
		}

		public override void AddGeneralPropertyForCommunity (string name, string value, CommunityInfo community)
		{
			CommunityProperty property = new CommunityProperty(
				community.Identity,
				CommunityGeneralPropertiesGroup,
				name,
				value
				);

			try 
			{
				// insert new property
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void UpdateGeneralPropertyForCommunity (string name, string value, CommunityInfo community)
		{
			CommunityProperty property = CommunityProperty.GetByCommunityIDAndPropertyGroupAndName(
				community.Identity,
				CommunityGeneralPropertiesGroup,
				name
				);
			property.Value = value;

			try 
			{
				// update property
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void RemoveGeneralPropertyForCommunity (string name, CommunityInfo community)
		{
			CommunityProperty property = CommunityProperty.GetByCommunityIDAndPropertyGroupAndName(
				community.Identity,
				CommunityGeneralPropertiesGroup,
				name
				);
		
			try 
			{
				// delete property
				property.Delete();
				property.CommitChanges();
			} 
			catch (Exception) { }
		}

		#endregion

		#region SectionGeneralProperties

		public override ManagedFusion.SectionPropertyCollection GetGeneralPropertiesForSection(SectionInfo section)
		{
			List<SectionProperty> properties = SectionProperty.GetList("SectionID = " + section.Identity + " and PropertyGroup = '" + SectionGeneralPropertiesGroup + "'");

			// get all properties for section
			NameValueCollection collection = new NameValueCollection(properties.Count);

			// add all rows to NameValueCollection
			foreach (SectionProperty property in properties)
				collection.Add(
					property.Name,
					property.Value
					);

			// returns a property collection
			return new ManagedFusion.SectionPropertyCollection(section, collection);
		}

		public override void AddGeneralPropertyForSection (string name, string value, SectionInfo section)
		{
			SectionProperty property = new SectionProperty(
				section.Identity,
				SectionModuleDataGroup,
				name,
				value
				);

			try
			{
				// insert new property
				property.CommitChanges();
			}
			catch (Exception) { }
		}

		public override void UpdateGeneralPropertyForSection (string name, string value, SectionInfo section)
		{
			SectionProperty property = SectionProperty.GetBySectionIDAndPropertyGroupAndName(
				section.Identity,
				SectionModuleDataGroup,
				name
				);
			property.Value = value;

			try
			{
				// update property
				property.CommitChanges();
			}
			catch (Exception) { }
		}

		public override void RemoveGeneralPropertyForSection (string name, SectionInfo section)
		{
			SectionProperty property = SectionProperty.GetBySectionIDAndPropertyGroupAndName(
				section.Identity,
				SectionModuleDataGroup,
				name
				);

			try
			{
				// delete property
				property.Delete();
				property.CommitChanges();
			}
			catch (Exception) { }
		}

		#endregion

		#region PortletGeneralProperties

		public override ManagedFusion.PortletPropertyCollection GetGeneralPropertiesForPortlet(PortletInfo portlet)
		{
			List<PortletProperty> properties = PortletProperty.GetList("PortletID = " + portlet.Identity + " and PropertyGroup = '" + PortletGeneralPropertiesGroup + "'");
			NameValueCollection collection = new NameValueCollection(properties.Count);

			// add all rows to NameValueCollection
			foreach (PortletProperty property in properties)
				collection.Add(
					property.Name,
					property.Value
					);

			// returns a property collection
			return new ManagedFusion.PortletPropertyCollection(portlet, collection);
		}

		public override void AddGeneralPropertyForPortlet (string name, string value, PortletInfo portlet)
		{
			PortletProperty property = new PortletProperty(
				portlet.Identity,
				PortletGeneralPropertiesGroup,
				name,
				value
				);

			try
			{
				// insert new property
				property.CommitChanges();
			}
			catch (Exception) { }
		}

		public override void UpdateGeneralPropertyForPortlet (string name, string value, PortletInfo portlet)
		{
			PortletProperty property = PortletProperty.GetByPortletIDAndPropertyGroupAndName(
				portlet.Identity,
				PortletGeneralPropertiesGroup,
				name
				);
			property.Value = value;

			try
			{
				// update property
				property.CommitChanges();
			}
			catch (Exception) { }
		}
		
		public override void RemoveGeneralPropertyForPortlet (string name, PortletInfo portlet)
		{
			PortletProperty property = PortletProperty.GetByPortletIDAndPropertyGroupAndName(
				portlet.Identity,
				PortletGeneralPropertiesGroup,
				name
				);

			try
			{
				// delete property
				property.Delete();
				property.CommitChanges();
			}
			catch (Exception) { }
		}

		#endregion

		#endregion

		#region Roles

		#region SectionRoles

		public override RolesTasksDictionary GetRolesForSection(SectionInfo section)
		{
			List<SectionRole> dbroles = SectionRole.GetBySectionID(section.Identity);

			// return list of roles
			RolesTasksDictionary roles = new RolesTasksDictionary();

			foreach (SectionRole role in dbroles)
				roles.Add(
					role.Role,
					role.Tasks.Split(Common.Delimiter)
					);

			return roles;
		}

		public override void AddRoleForSection(string role, string[] tasks, SectionInfo section)
		{
			SectionRole dbrole = new SectionRole(
				section.Identity,
				role,
				String.Join(Common.Delimiter.ToString(), tasks)
				);

			try 
			{
				// add role
				dbrole.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void UpdateRoleForSection(string role, string[] tasks, SectionInfo section)
		{
			SectionRole dbrole = SectionRole.GetBySectionIDAndRole(
				section.Identity,
				role
				);
			dbrole.Tasks = String.Join(Common.Delimiter.ToString(), tasks);

			try 
			{
				// update role
				dbrole.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void RemoveRoleForSection(string role, SectionInfo section)
		{
			SectionRole dbrole = SectionRole.GetBySectionIDAndRole(
				section.Identity,
				role
				);

			try 
			{
				// remove certain role
				dbrole.Delete();
				dbrole.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void RemoveAllRolesForSection(SectionInfo section)
		{
			List<SectionRole> dbroles = SectionRole.GetBySectionID(section.Identity);

			try 
			{
				// remove all roles for one section
				foreach (SectionRole role in dbroles)
				{
					role.Delete();
					role.CommitChanges();
				}
			} 
			catch (Exception) { }
		}

		#endregion

		#region PortletRoles

		public override RolesPermissionsDictionary GetRolesForPortlet(PortletInfo portlet)
		{
			List<PortletRole> dbroles = PortletRole.GetByPortletID(portlet.Identity);

			// return list of roles
			RolesPermissionsDictionary roles = new RolesPermissionsDictionary();

			foreach (PortletRole role in dbroles)
				roles.Add(
					role.Role,
					role.Permissions.Split(Common.Delimiter)
					);

			return roles;
		}

		public override void AddRoleForPortlet(string role, string[] permissions, PortletInfo portlet)
		{
			PortletRole dbrole = new PortletRole(
				portlet.Identity,
				role,
				String.Join(Common.Delimiter.ToString(), permissions)
				);

			try 
			{
				// add role
				dbrole.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void UpdateRoleForPortlet(string role, string[] permissions, PortletInfo portlet)
		{
			PortletRole dbrole = PortletRole.GetByPortletIDAndRole(
				portlet.Identity,
				role
				);
			dbrole.Permissions = String.Join(Common.Delimiter.ToString(), permissions);

			try 
			{
				// update role
				dbrole.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void RemoveRoleForPortlet(string role, PortletInfo portlet)
		{
			PortletRole dbrole = PortletRole.GetByPortletIDAndRole(
				portlet.Identity,
				role
				);

			try 
			{
				// remove certain role
				dbrole.Delete();
				dbrole.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void RemoveAllRolesForPortlet(PortletInfo portlet)
		{
			List<PortletRole> dbrole = PortletRole.GetByPortletID(portlet.Identity);

			try 
			{
				// remove all roles for one section
				foreach (PortletRole role in dbrole)
				{
					role.Delete();
					role.CommitChanges();
				}
			} 
			catch (Exception) { }
		}

		#endregion

		#endregion

		#region Links

		#region SectionContainerLink

		public override int[] GetContainersForSection (SectionInfo section)
		{
			// get containers for specific section
			List<SectionContainerLink> links = SectionContainerLink.GetList("SectionID = " + section.Identity);
			List<int> list = new List<int>(links.Count);

			foreach (SectionContainerLink link in links)
				list.Add(link.ContainerID);

			// return array of container ids
			return list.ToArray();
		}

		public override int[] GetContainersInPositionForSection(SectionInfo section, int position)
		{
			// get containers for specific section
			List<SectionContainerLink> links = SectionContainerLink.GetList("SectionID = " + section.Identity + " and Position = " + position);
			List<int> list = new List<int>(links.Count);

			foreach (SectionContainerLink link in links)
				list.Add(link.ContainerID);

			// return array of container ids
			return list.ToArray();
		}

		public override bool SectionContainerLinked (SectionInfo section, ContainerInfo container)
		{
			SectionContainerLink link = SectionContainerLink.GetBySectionIDAndContainerID(
				section.Identity,
				container.Identity
				);

			try 
			{
				// return if combination is found
				return link.SectionID != 0 && link.ContainerID != 0;
			} 
			catch (Exception) 
			{
				return false; 
			}
		}

		public override int GetSectionContainerLinkOrder(SectionInfo section, ContainerInfo container)
		{
			SectionContainerLink link = SectionContainerLink.GetBySectionIDAndContainerID(
				section.Identity,
				container.Identity
				);

			return (link.SectionID != 0 && link.ContainerID != 0) ? link.SortOrder : -1;
		}

		public override int GetSectionContainerLinkPosition(SectionInfo section, ContainerInfo container)
		{
			SectionContainerLink link = SectionContainerLink.GetBySectionIDAndContainerID(
				section.Identity,
				container.Identity
				);

			return (link.SectionID != 0 && link.ContainerID != 0) ? link.Position : Int32.MinValue;
		}

		public override void AddSectionContainerLink (SectionInfo section, ContainerInfo container, int order, int position)
		{
			SectionContainerLink link = new SectionContainerLink(
				section.Identity,
				container.Identity,
				order,
				position
				);

			try 
			{
				// add combination
				link.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void UpdateSectionContainerLink(SectionInfo section, ContainerInfo container, int order, int position)
		{
			SectionContainerLink link = SectionContainerLink.GetBySectionIDAndContainerID(
				section.Identity,
				container.Identity
				);
			link.SortOrder = order;
			link.Position = position;

			try 
			{
				// update combination
				link.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void RemoveSectionContainerLink (SectionInfo section, ContainerInfo container)
		{
			SectionContainerLink link = SectionContainerLink.GetBySectionIDAndContainerID(
				section.Identity,
				container.Identity
				);

			try 
			{
				// delete combination
				link.Delete();
				link.CommitChanges();
			} 
			catch (Exception) { }
		}

		#endregion

		#region ContainerPortletLink

		public override int[] GetPortletsForContainer (ContainerInfo container)
		{
			// get portets for specific container
			List<ContainerPortletLink> links = ContainerPortletLink.GetByContainerID(container.Identity);
			List<int> list = new List<int>(links.Count);

			foreach (ContainerPortletLink link in links)
				list.Add(link.PortletID);

			// return array of portlet ids
			return list.ToArray();
		}

		public override bool ContainerPortletLinked (ContainerInfo container, PortletInfo portlet)
		{
			ContainerPortletLink link = ContainerPortletLink.GetByContainerIDAndPortletID(
				container.Identity,
				portlet.Identity
				);
			
			try 
			{
				// return if combination is found
				return link.ContainerID != 0 && link.PortletID != 0;
			} 
			catch (Exception) 
			{
				return false; 
			}
		}

		public override int GetContainerPortletLinkOrder(ContainerInfo container, PortletInfo portlet)
		{
			ContainerPortletLink link = ContainerPortletLink.GetByContainerIDAndPortletID(
				container.Identity,
				portlet.Identity
				);

			return (link.ContainerID != 0 && link.PortletID != 0) ? link.SortOrder : -1;
		}

		public override void AddContainerPortletLink (ContainerInfo container, PortletInfo portlet, int order)
		{
			ContainerPortletLink link = new ContainerPortletLink(
				container.Identity,
				portlet.Identity,
				order
				);

			try 
			{
				// add combination
				link.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void UpdateContainerPortletLink(ContainerInfo container, PortletInfo portlet, int order)
		{
			ContainerPortletLink link = ContainerPortletLink.GetByContainerIDAndPortletID(
				container.Identity,
				portlet.Identity
				);
			link.SortOrder = order;

			try 
			{
				// update combination
				link.CommitChanges();
			} 
			catch (Exception) { }
		}

		public override void RemoveContainerPortletLink (ContainerInfo container, PortletInfo portlet)
		{
			ContainerPortletLink link = ContainerPortletLink.GetByContainerIDAndPortletID(
				container.Identity,
				portlet.Identity
				);

			try 
			{
				// delete combination
				link.Delete();
				link.CommitChanges();
			} 
			catch (Exception) { }
		}

		#endregion

		#endregion
	}
}
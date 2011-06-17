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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Configuration;
using System.Configuration.Provider;

// ManagedFusion Classes
using ManagedFusion;
using ManagedFusion.Modules;
using ManagedFusion.Portlets;

namespace ManagedFusion.Data
{
	public delegate ArrayList ModuleSearchEventHandler (Type searchFor);

	public abstract class PortalDatabaseProvider : DatabaseProvider
	{
		#region Portlets

		public event EventHandler PortletsChanged;
		public abstract ManagedFusion.PortletCollection Portlets { get; set; }
		
		public abstract void CommitPortletChanges (PortletInfo portlet);
		protected internal abstract void ResetPortletCollection ();

		protected internal void OnPortletsChanged () 
		{
			if (PortletsChanged != null)
				PortletsChanged(null, new EventArgs());
		}

		#endregion

		#region Containers

		public event EventHandler ContainersChanged;
		public abstract ContainerCollection Containers { get; set; }

		public abstract void CommitContainerChanges (ContainerInfo container);
		protected internal abstract void ResetContainerCollection ();

		protected internal void OnContainersChanged () 
		{
			if (ContainersChanged != null)
				ContainersChanged(null, new EventArgs());
		}

		#endregion

		#region Sections

		public event EventHandler SectionsChanged;
		public abstract SectionCollection Sections { get; set; }

		public abstract void CommitSectionChanges (SectionInfo section);
		protected internal abstract void ResetSectionCollection ();

		protected internal void OnSectionsChanged () 
		{
			if (SectionsChanged != null)
				SectionsChanged(null, new EventArgs());
		}

		#endregion

		#region Communities

		public event EventHandler CommunitiesChanged;
		public abstract CommunityCollection Communities { get; set; }

		public abstract void CommitPortalChanges (CommunityInfo portal);
		protected internal abstract void ResetCommunityCollection ();

		protected internal void OnCommunitiesChanged () 
		{
			if (CommunitiesChanged != null)
				CommunitiesChanged(null, new EventArgs());
		}

		#endregion

		#region Sites

		public event EventHandler SitesChanged;
		public abstract SiteCollection Sites { get; set; }

		public abstract void CommitSiteChanges (SiteInfo site);
		protected internal abstract void ResetSiteCollection ();

		protected internal void OnSitesChanged () 
		{
			if (SitesChanged != null)
				SitesChanged(null, new EventArgs());
		}

		#endregion

		#region Modules

		private ModuleCollection _SectionModule;
		public ModuleCollection SectionModules
		{
			get 
			{
				if (this._SectionModule != null)
					return this._SectionModule;

				SkinAttributeType[] attribs = ScourAssembly(typeof(ModuleAttribute));
				ModuleInfo[] collection = new ModuleInfo[attribs.Length];

				// add all modules
				for (int i = 0; i < attribs.Length; i++) 
				{
					collection[i] = new SectionModule(
						(ModuleAttribute)attribs[i].Attribute,
						attribs[i].Type
						);
				}

				this._SectionModule = new ModuleCollection(collection);
				return this._SectionModule;
			}
		}

		private ModuleCollection _PortletModules;
		public ModuleCollection PortletModules
		{
			get 
			{
				if (this._PortletModules != null) return this._PortletModules;

				SkinAttributeType[] attribs = ScourAssembly(typeof(PortletAttribute));
				PortletModule[] collection = new PortletModule[attribs.Length];

				// add all modules
				for (int i = 0; i < attribs.Length; i++) 
				{
					collection[i] = new PortletModule(
						(PortletAttribute)attribs[i].Attribute,
						attribs[i].Type
						);
				}

				this._PortletModules = new ModuleCollection(collection);
				return this._PortletModules;
			}
		}

		#region Scour Assemblies

		private struct SkinAttributeType 
		{
			public SkinAttribute Attribute;
			public Type Type;

			public SkinAttributeType (SkinAttribute attr, Type type)
			{
				Attribute = attr;
				Type = type;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">Handler to scour the assembly for.</param>
		/// <returns></returns>
		private SkinAttributeType[] ScourAssembly (Type type) 
		{
			ArrayList al = new ArrayList();

			// searches each array for Attribute
			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) 
			{
				if (a.FullName.StartsWith("System"))
					continue;

				ArrayList attribs = new ArrayList();

				// checks each type to see if Attribute is in it
				foreach (Type t in a.GetTypes()) 
				{
					// gets all attributes of Attribute
					object[] o = t.GetCustomAttributes(type, false);

					// if attributes are found add to array list
					if (o.Length > 0) 
					{
						attribs.Add(new SkinAttributeType((SkinAttribute)o[0], t));
					}
				}
#if DEBUG
				Common.Context.Trace.Write("ScourAssembly", String.Format("Found {0} of type {1} in {2}", attribs.Count, type.Name, a.FullName));
#endif
				al.AddRange(attribs);
			}

			// returns an array of objects that are of type "type"
			return (SkinAttributeType[])al.ToArray(typeof(SkinAttributeType));
		}

		#endregion

		#endregion

		#region Properties

		#region SectionModuleData

		public const string SectionModuleDataGroup = "Module";

		public abstract SectionModuleDataCollection GetModuleDataForSection (SectionInfo section);
		public abstract void AddModuleDataForSection (string name, string value, SectionInfo section);
		public abstract void UpdateModuleDataForSection (string name, string value, SectionInfo section);
		public abstract void RemoveModuleDataForSection (string name, SectionInfo section);

		#endregion

		#region PortletModuleData

		public const string PortletModuleDataGroup = "Module";

		public abstract PortletModuleDataCollection GetModuleDataForPortlet (PortletInfo portlet);
		public abstract void AddModuleDataForPortlet (string name, string value, PortletInfo portlet);
		public abstract void UpdateModuleDataForPortlet (string name, string value, PortletInfo portlet);
		public abstract void RemoveModuleDataForPortlet (string name, PortletInfo portlet);

		#endregion

		#region SectionMetaProperties

		public const string SectionMetaPropertiesGroup = "Meta";

		public abstract SectionMetaPropertyCollection GetMetaPropertiesForSection (SectionInfo section);
		public abstract void AddMetaPropertyForSection (string name, string value, SectionInfo section);
		public abstract void UpdateMetaPropertyForSection (string name, string value, SectionInfo section);
		public abstract void RemoveMetaPropertyForSection (string name, SectionInfo section);

		#endregion

		#region CommunityGeneralProperties

		public const string CommunityGeneralPropertiesGroup = "General";

		public abstract CommunityPropertyCollection GetGeneralPropertiesForCommunity (CommunityInfo section);
		public abstract void AddGeneralPropertyForCommunity (string name, string value, CommunityInfo section);
		public abstract void UpdateGeneralPropertyForCommunity  (string name, string value, CommunityInfo section);
		public abstract void RemoveGeneralPropertyForCommunity  (string name, CommunityInfo section);

		#endregion

		#region SectionGeneralProperties

		public const string SectionGeneralPropertiesGroup = "General";

		public abstract SectionPropertyCollection GetGeneralPropertiesForSection (SectionInfo section);
		public abstract void AddGeneralPropertyForSection (string name, string value, SectionInfo section);
		public abstract void UpdateGeneralPropertyForSection (string name, string value, SectionInfo section);
		public abstract void RemoveGeneralPropertyForSection (string name, SectionInfo section);

		#endregion

		#region PortletGeneralProperties

		public const string PortletGeneralPropertiesGroup = "General";

		public abstract PortletPropertyCollection GetGeneralPropertiesForPortlet (PortletInfo portlet);
		public abstract void AddGeneralPropertyForPortlet (string name, string value, PortletInfo portlet);
		public abstract void UpdateGeneralPropertyForPortlet (string name, string value, PortletInfo portlet);
		public abstract void RemoveGeneralPropertyForPortlet (string name, PortletInfo portlet);

		#endregion

		#endregion

		#region Roles

		#region SectionRoles

		public abstract RolesTasksDictionary GetRolesForSection (SectionInfo section);
		public abstract void AddRoleForSection (string role, string[] tasks, SectionInfo section);
		public abstract void UpdateRoleForSection (string role, string[] tasks, SectionInfo section);
		public abstract void RemoveRoleForSection (string role, SectionInfo section);
		public abstract void RemoveAllRolesForSection (SectionInfo section);

		#endregion

		#region PortletRoles

		public abstract RolesPermissionsDictionary GetRolesForPortlet (PortletInfo portlet);
		public abstract void AddRoleForPortlet (string role, string[] permissions, PortletInfo portlet);
		public abstract void UpdateRoleForPortlet (string role, string[] permissions, PortletInfo portlet);
		public abstract void RemoveRoleForPortlet (string role, PortletInfo portlet);
		public abstract void RemoveAllRolesForPortlet (PortletInfo portlet);

		#endregion

		#endregion

		#region Links

		#region SectionContainerLink

		public abstract int[] GetContainersForSection (SectionInfo section);
		public abstract int[] GetContainersInPositionForSection (SectionInfo section, int position);
		public abstract bool SectionContainerLinked (SectionInfo section, ContainerInfo container);

		public abstract int GetSectionContainerLinkPosition (SectionInfo section, ContainerInfo container);
		public abstract int GetSectionContainerLinkOrder (SectionInfo section, ContainerInfo container);

		public abstract void AddSectionContainerLink (SectionInfo section, ContainerInfo container, int order, int position);
		public abstract void UpdateSectionContainerLink (SectionInfo section, ContainerInfo container, int order, int position);
		public abstract void RemoveSectionContainerLink (SectionInfo section, ContainerInfo container);

		#endregion

		#region ContainerPortletLink

		public abstract int[] GetPortletsForContainer (ContainerInfo container);
		public abstract bool ContainerPortletLinked (ContainerInfo container, PortletInfo portlet);

		public abstract int GetContainerPortletLinkOrder (ContainerInfo container, PortletInfo portlet);

		public abstract void AddContainerPortletLink (ContainerInfo container, PortletInfo portlet, int order);
		public abstract void UpdateContainerPortletLink (ContainerInfo container, PortletInfo portlet, int order);
		public abstract void RemoveContainerPortletLink (ContainerInfo container, PortletInfo portlet);

		#endregion

		#endregion
	}
}
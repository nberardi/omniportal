/*
 * Community
 */

insert into Community ([Name]) values ('My First Site')

/*
 * Section
 */

insert into [Section] (ParentSectionID, CommunityID, [Name], SortOrder, ModuleID)
	values (0, 1, 'Home', 0, '8B61FD65-C060-46f2-AC29-5A4669D010AC')

insert into [Section] (ParentSectionID, CommunityID, [Name], Description, SortOrder, ModuleID)
	values (1, 1, 'Blog', 'My Blog', 0, 'A7181415-44BC-48aa-9519-2AFC69DCB92A')


/*
 * Site
 */

insert into Site (SectionID, [Name], [Description], SubDomain, [Domain]) 
	values (1, '*.*', '*.*', '*', '*')

/*
 * SectionRole
 */

insert into SectionRole (SectionID, Role, Tasks) values (1, 'Administrator', 'Editor')
insert into SectionRole (SectionID, Role, Tasks) values (1, 'Everybody', 'ViewPage')
insert into SectionRole (SectionID, Role, Tasks) values (2, 'Administrator', 'Poster')
insert into SectionRole (SectionID, Role, Tasks) values (2, 'Everybody', 'ViewPage;Commentor')

/*
 * SectionProperty
 */

insert into SectionProperty (SectionID, [PropertyGroup], [Name], [Value])
	values (1, 'Module', 'Content', '<h1>Welcome to OmniPortal</h1><p>This is your first instance of OmniPortal.  For more information please visit <a href="http://www.omniportal.net">OmniPortal.net</a> or <a href="http://sourceforge.net/projects/omniportal">OmniPortal@SourceForge</a>.')

/*
 * Containers
 */

insert into Container ([Name]) values ('MainPage')

/*
 * SectionContainerLink
 */

insert into SectionContainerLink (SectionID, ContainerID, SortOrder, [Position]) 
	values (1, 1, 0, 0)
insert into SectionContainerLink (SectionID, ContainerID, SortOrder, [Position]) 
	values (2, 1, 0, 0)

/*
 * Portlet
 */

insert into Portlet ([Name], ModuleID) 
	values ('Login', 'F0E74CFD-C96C-4C1E-A9E0-CAF7BFC0CDB3')
insert into Portlet ([Name], ModuleID) 
	values ('OmniPortal Summery', '8F369A68-564E-438B-B915-B4605EC12FE2')

/*
 * PortletRole
 */

insert into PortletRole (PortletID, Role, [Permissions]) 
	values (1, 'Everybody', 'Read')
insert into PortletRole (PortletID, Role, [Permissions]) 
	values (2, 'Everybody', 'Read')

/*
 * PortletProperty
 */

insert into PortletProperty (PortletID, [PropertyGroup], [Name], [Value])
	values (2, 'Module', 'RssFeedUrl', 'http://sourceforge.net/export/rss2_projsummary.php?group_id=54624')

/*
 * ContainerPortletLink
 */

insert into ContainerPortletLink (ContainerID, PortletID, SortOrder) 
	values (1, 1, 0)
insert into ContainerPortletLink (ContainerID, PortletID, SortOrder) 
	values (1, 2, 1)

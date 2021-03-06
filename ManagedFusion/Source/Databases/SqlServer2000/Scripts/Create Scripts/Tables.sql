/****** Object:  Table [dbo].[Community]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Community]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Community](
	[CommunityID] [int] IDENTITY(1,1) NOT NULL,
	[UniversalID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Communities_UniversalIdentity]  DEFAULT (newid()),
	[Name] [nvarchar](32) NOT NULL,
	[Description] [nvarchar](128) NULL,
	[Touched] [datetime] NOT NULL CONSTRAINT [DF_Communities_Touched]  DEFAULT (getdate()),
 CONSTRAINT [PK_Portals] PRIMARY KEY CLUSTERED 
(
	[CommunityID] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Communities_Title] UNIQUE NONCLUSTERED 
(
	[Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Index [IX_Communities_UniversalIdentity]    Script Date: 11/22/2006 15:59:38 ******/
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[Community]') AND name = N'IX_Communities_UniversalIdentity')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Communities_UniversalIdentity] ON [dbo].[Community] 
(
	[UniversalID] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Portlet]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Portlet]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Portlet](
	[PortletID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](32) NOT NULL,
	[Description] [nvarchar](128) NULL,
	[Touched] [datetime] NOT NULL CONSTRAINT [DF_Portlets_portlet_touched]  DEFAULT (getdate()),
	[ModuleID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Portlets] PRIMARY KEY CLUSTERED 
(
	[PortletID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Container]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Container]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Container](
	[ContainerID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](32) NOT NULL,
	[Description] [nvarchar](128) NULL,
	[Touched] [datetime] NOT NULL CONSTRAINT [DF_Containers_container_touched]  DEFAULT (getdate()),
 CONSTRAINT [PK_Containers] PRIMARY KEY CLUSTERED 
(
	[ContainerID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Section]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Section]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Section](
	[SectionID] [int] IDENTITY(1,1) NOT NULL,
	[ParentSectionID] [int] NOT NULL CONSTRAINT [DF_Sections_section_parentID]  DEFAULT ((0)),
	[CommunityID] [int] NOT NULL,
	[Name] [nvarchar](32) NOT NULL,
	[Description] [nvarchar](128) NULL,
	[Touched] [datetime] NOT NULL CONSTRAINT [DF_Sections_section_touched]  DEFAULT (getdate()),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_Sections_section_order]  DEFAULT ((0)),
	[IsEnabled] [bit] NOT NULL CONSTRAINT [DF_Section_IsEnabled]  DEFAULT ((1)),
	[IsVisible] [bit] NOT NULL CONSTRAINT [DF_Sections_section_visible]  DEFAULT ((1)),
	[SyndicateFeed] [bit] NOT NULL CONSTRAINT [DF_Sections_Syndicate]  DEFAULT ((1)),
	[SyndicateSitemap] [bit] NOT NULL CONSTRAINT [DF_Section_SyndicateSitemap]  DEFAULT ((1)),
	[Owner] [nvarchar](32) NOT NULL CONSTRAINT [DF_Sections_Owner]  DEFAULT (N'Administrator'),
	[ModuleID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Sections_Module]  DEFAULT ('8B61FD65-C060-46F2-AC29-5A4669D010AC'),
	[Theme] [nvarchar](64) NOT NULL CONSTRAINT [DF_Sections_Theme]  DEFAULT (N'Inherited'),
	[Style] [nvarchar](64) NOT NULL CONSTRAINT [DF_Sections_Style]  DEFAULT (N'Inherited'),
 CONSTRAINT [PK_Sections] PRIMARY KEY CLUSTERED 
(
	[SectionID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Index [IX_Sections_ParentID]    Script Date: 11/22/2006 15:59:38 ******/
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[Section]') AND name = N'IX_Sections_ParentID')
CREATE NONCLUSTERED INDEX [IX_Sections_ParentID] ON [dbo].[Section] 
(
	[ParentSectionID] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommunityProperty]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[CommunityProperty]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[CommunityProperty](
	[CommunityID] [int] NOT NULL,
	[PropertyGroup] [nvarchar](32) NOT NULL CONSTRAINT [DF_CommunityProperties_Group]  DEFAULT (N'General'),
	[Name] [nvarchar](32) NOT NULL,
	[Value] [ntext] NOT NULL,
 CONSTRAINT [PK_CommunityProperty] PRIMARY KEY CLUSTERED 
(
	[CommunityID] ASC,
	[PropertyGroup] ASC,
	[Name] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PortletRole]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PortletRole]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[PortletRole](
	[PortletID] [int] NOT NULL,
	[Role] [nvarchar](32) NOT NULL,
	[Permissions] [ntext] NOT NULL,
 CONSTRAINT [PK_PortletRole] PRIMARY KEY CLUSTERED 
(
	[PortletID] ASC,
	[Role] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PortletProperty]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[PortletProperty]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[PortletProperty](
	[PortletID] [int] NOT NULL,
	[PropertyGroup] [nvarchar](32) NOT NULL CONSTRAINT [DF_PortletProperties_Group]  DEFAULT (N'General'),
	[Name] [nvarchar](32) NOT NULL,
	[Value] [ntext] NOT NULL,
 CONSTRAINT [PK_PortletProperty] PRIMARY KEY CLUSTERED 
(
	[PortletID] ASC,
	[PropertyGroup] ASC,
	[Name] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ContainerPortletLink]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[ContainerPortletLink]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[ContainerPortletLink](
	[ContainerID] [int] NOT NULL,
	[PortletID] [int] NOT NULL,
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_ContainerPortletLink_Order]  DEFAULT ((0)),
 CONSTRAINT [PK_ContainerPortletLink] PRIMARY KEY CLUSTERED 
(
	[ContainerID] ASC,
	[PortletID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SectionRole]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SectionRole]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[SectionRole](
	[SectionID] [int] NOT NULL,
	[Role] [nvarchar](32) NOT NULL,
	[Tasks] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_SectionRole] PRIMARY KEY CLUSTERED 
(
	[SectionID] ASC,
	[Role] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SectionProperty]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SectionProperty]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[SectionProperty](
	[SectionID] [int] NOT NULL,
	[PropertyGroup] [nvarchar](32) NOT NULL CONSTRAINT [DF_SectionProperties_Group]  DEFAULT (N'General'),
	[Name] [nvarchar](32) NOT NULL,
	[Value] [ntext] NOT NULL,
 CONSTRAINT [PK_SectionProperty_1] PRIMARY KEY CLUSTERED 
(
	[SectionID] ASC,
	[PropertyGroup] ASC,
	[Name] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Site]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Site]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Site](
	[SiteID] [int] IDENTITY(1,1) NOT NULL,
	[SectionID] [int] NULL,
	[Name] [nvarchar](32) NOT NULL,
	[Description] [nvarchar](128) NULL,
	[Touched] [datetime] NOT NULL CONSTRAINT [DF_Sites_site_touched]  DEFAULT (getdate()),
	[SubDomain] [nvarchar](128) NOT NULL CONSTRAINT [DF_Sites_site_subDomain]  DEFAULT ('*'),
	[Domain] [nvarchar](128) NOT NULL CONSTRAINT [DF_Sites_site_domain]  DEFAULT ('*'),
	[Theme] [nvarchar](64) NOT NULL CONSTRAINT [DF_Sites_Theme]  DEFAULT (N'Inherited'),
	[Style] [nvarchar](64) NOT NULL CONSTRAINT [DF_Sites_Style]  DEFAULT (N'Inherited'),
 CONSTRAINT [PK_Sites] PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Sites_Domain] UNIQUE NONCLUSTERED 
(
	[Domain] ASC,
	[SubDomain] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SectionContainerLink]    Script Date: 11/22/2006 15:59:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SectionContainerLink]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[SectionContainerLink](
	[SectionID] [int] NOT NULL,
	[ContainerID] [int] NOT NULL,
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_SectionContainerLink_Order]  DEFAULT ((0)),
	[Position] [int] NOT NULL CONSTRAINT [DF_SectionContainerLink_Position]  DEFAULT ((2)),
 CONSTRAINT [PK_SectionContainerLink_1] PRIMARY KEY CLUSTERED 
(
	[SectionID] ASC,
	[ContainerID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Sections_Communities]') AND type = 'F')
ALTER TABLE [dbo].[Section]  WITH CHECK ADD  CONSTRAINT [FK_Sections_Communities] FOREIGN KEY([CommunityID])
REFERENCES [dbo].[Community] ([CommunityID])
GO
ALTER TABLE [dbo].[Section] CHECK CONSTRAINT [FK_Sections_Communities]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_CommunityProperties_Communities]') AND type = 'F')
ALTER TABLE [dbo].[CommunityProperty]  WITH CHECK ADD  CONSTRAINT [FK_CommunityProperties_Communities] FOREIGN KEY([CommunityID])
REFERENCES [dbo].[Community] ([CommunityID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CommunityProperty] CHECK CONSTRAINT [FK_CommunityProperties_Communities]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_PortletRoles_Portlets]') AND type = 'F')
ALTER TABLE [dbo].[PortletRole]  WITH CHECK ADD  CONSTRAINT [FK_PortletRoles_Portlets] FOREIGN KEY([PortletID])
REFERENCES [dbo].[Portlet] ([PortletID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PortletRole] CHECK CONSTRAINT [FK_PortletRoles_Portlets]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_PortletProperties_Portlets]') AND type = 'F')
ALTER TABLE [dbo].[PortletProperty]  WITH CHECK ADD  CONSTRAINT [FK_PortletProperties_Portlets] FOREIGN KEY([PortletID])
REFERENCES [dbo].[Portlet] ([PortletID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PortletProperty] CHECK CONSTRAINT [FK_PortletProperties_Portlets]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_ContainerPortletLink_Containers]') AND type = 'F')
ALTER TABLE [dbo].[ContainerPortletLink]  WITH CHECK ADD  CONSTRAINT [FK_ContainerPortletLink_Containers] FOREIGN KEY([ContainerID])
REFERENCES [dbo].[Container] ([ContainerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContainerPortletLink] CHECK CONSTRAINT [FK_ContainerPortletLink_Containers]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_ContainerPortletLink_Portlets]') AND type = 'F')
ALTER TABLE [dbo].[ContainerPortletLink]  WITH CHECK ADD  CONSTRAINT [FK_ContainerPortletLink_Portlets] FOREIGN KEY([PortletID])
REFERENCES [dbo].[Portlet] ([PortletID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContainerPortletLink] CHECK CONSTRAINT [FK_ContainerPortletLink_Portlets]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_SectionRoles_Sections]') AND type = 'F')
ALTER TABLE [dbo].[SectionRole]  WITH CHECK ADD  CONSTRAINT [FK_SectionRoles_Sections] FOREIGN KEY([SectionID])
REFERENCES [dbo].[Section] ([SectionID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SectionRole] CHECK CONSTRAINT [FK_SectionRoles_Sections]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_SectionProperties_Sections]') AND type = 'F')
ALTER TABLE [dbo].[SectionProperty]  WITH CHECK ADD  CONSTRAINT [FK_SectionProperties_Sections] FOREIGN KEY([SectionID])
REFERENCES [dbo].[Section] ([SectionID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SectionProperty] CHECK CONSTRAINT [FK_SectionProperties_Sections]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Sites_Sections]') AND type = 'F')
ALTER TABLE [dbo].[Site]  WITH CHECK ADD  CONSTRAINT [FK_Sites_Sections] FOREIGN KEY([SectionID])
REFERENCES [dbo].[Section] ([SectionID])
GO
ALTER TABLE [dbo].[Site] CHECK CONSTRAINT [FK_Sites_Sections]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_SectionContainerLink_Containers]') AND type = 'F')
ALTER TABLE [dbo].[SectionContainerLink]  WITH CHECK ADD  CONSTRAINT [FK_SectionContainerLink_Containers] FOREIGN KEY([ContainerID])
REFERENCES [dbo].[Container] ([ContainerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SectionContainerLink] CHECK CONSTRAINT [FK_SectionContainerLink_Containers]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_SectionContainerLink_Sections]') AND type = 'F')
ALTER TABLE [dbo].[SectionContainerLink]  WITH CHECK ADD  CONSTRAINT [FK_SectionContainerLink_Sections] FOREIGN KEY([SectionID])
REFERENCES [dbo].[Section] ([SectionID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SectionContainerLink] CHECK CONSTRAINT [FK_SectionContainerLink_Sections]

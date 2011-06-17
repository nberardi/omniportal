if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Blog_CategoryPostLink_Blog_Categories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Blog_CategoryPostLink] DROP CONSTRAINT FK_Blog_CategoryPostLink_Blog_Categories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Blog_CategoryPostLink_Blog_Posts]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Blog_CategoryPostLink] DROP CONSTRAINT FK_Blog_CategoryPostLink_Blog_Posts
GO

/****** Object:  Table [dbo].[Blog_Categories]    Script Date: 2/14/2005 1:05:48 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Blog_Categories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Blog_Categories]
GO

/****** Object:  Table [dbo].[Blog_CategoryPostLink]    Script Date: 2/14/2005 1:05:48 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Blog_CategoryPostLink]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Blog_CategoryPostLink]
GO

/****** Object:  Table [dbo].[Blog_Posts]    Script Date: 2/14/2005 1:05:48 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Blog_Posts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Blog_Posts]
GO

/****** Object:  Table [dbo].[Blog_Categories]    Script Date: 2/14/2005 1:05:50 PM ******/
CREATE TABLE [dbo].[Blog_Categories] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Name] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Blog_CategoryPostLink]    Script Date: 2/14/2005 1:05:50 PM ******/
CREATE TABLE [dbo].[Blog_CategoryPostLink] (
	[Post_ID] [int] NOT NULL ,
	[Category_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Blog_Posts]    Script Date: 2/14/2005 1:05:50 PM ******/
CREATE TABLE [dbo].[Blog_Posts] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Title] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Body] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Published] [bit] NOT NULL ,
	[AllowComments] [bit] NOT NULL ,
	[Syndicate] [bit] NOT NULL ,
	[User_ID] [uniqueidentifier] NOT NULL ,
	[TitleUrl] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Source] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[SourceUrl] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Created] [datetime] NOT NULL ,
	[Issued] [datetime] NOT NULL ,
	[Modified] [datetime] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Blog_Categories] WITH NOCHECK ADD 
	CONSTRAINT [PK_Blog_Categories] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Blog_CategoryPostLink] WITH NOCHECK ADD 
	CONSTRAINT [PK_Blog_CategoryPostLink] PRIMARY KEY  CLUSTERED 
	(
		[Post_ID],
		[Category_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Blog_Posts] WITH NOCHECK ADD 
	CONSTRAINT [PK_Blog_Posts] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Blog_Posts] ADD 
	CONSTRAINT [DF_Blog_Posts_Published] DEFAULT (1) FOR [Published],
	CONSTRAINT [DF_Blog_Posts_AllowComments] DEFAULT (1) FOR [AllowComments],
	CONSTRAINT [DF_Blog_Posts_Syndicate] DEFAULT (1) FOR [Syndicate]
GO

ALTER TABLE [dbo].[Blog_CategoryPostLink] ADD 
	CONSTRAINT [FK_Blog_CategoryPostLink_Blog_Categories] FOREIGN KEY 
	(
		[Category_ID]
	) REFERENCES [dbo].[Blog_Categories] (
		[ID]
	),
	CONSTRAINT [FK_Blog_CategoryPostLink_Blog_Posts] FOREIGN KEY 
	(
		[Post_ID]
	) REFERENCES [dbo].[Blog_Posts] (
		[ID]
	) ON DELETE CASCADE 
GO


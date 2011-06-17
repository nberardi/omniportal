/*
 * 	Template:		This code was generated by the ManagedFusion [http://www.managedfusion.com] Data Layer Template.
 * 	Created On :	11/22/2006
 * 	Remarks:		Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
 */

set nocount on

--------------------------------------------------------------------------------------------
--	Insert or Update entity
--------------------------------------------------------------------------------------------

if exists (select * from sysobjects where type = 'P' and name = 'ManagedFusion_Section')
	begin
		print 'Dropping Procedure ManagedFusion_Section'
		drop procedure ManagedFusion_Section
	end
go

print 'Creating Procedure ManagedFusion_Section'
go

create procedure ManagedFusion_Section
(
	@SectionID as int = null,
	@ParentSectionID as int,
	@CommunityID as int,
	@Name as nvarchar(32),
	@Description as nvarchar(128),
	@Touched as datetime,
	@SortOrder as int,
	@IsEnabled as bit,
	@IsVisible as bit,
	@SyndicateFeed as bit,
	@SyndicateSitemap as bit,
	@Owner as nvarchar(32),
	@ModuleID as uniqueidentifier,
	@Theme as nvarchar(64),
	@Style as nvarchar(64)
)
as

--	Author:		ManagedFusion [http://www.managedfusion.com]
--	Date:		11/22/2006
--	Purpose:	Inserts or updates an entity into Section.

-- check to make sure the record already doesn't exisit
if exists (	
	select *
	from [Section] 
	where 
		[SectionID] = @SectionID
) begin

	update 
		[Section]
	
	set
		[ParentSectionID] = @ParentSectionID,
		[CommunityID] = @CommunityID,
		[Name] = @Name,
		[Description] = @Description,
		[Touched] = @Touched,
		[SortOrder] = @SortOrder,
		[IsEnabled] = @IsEnabled,
		[IsVisible] = @IsVisible,
		[SyndicateFeed] = @SyndicateFeed,
		[SyndicateSitemap] = @SyndicateSitemap,
		[Owner] = @Owner,
		[ModuleID] = @ModuleID,
		[Theme] = @Theme,
		[Style] = @Style
	
	where
		[SectionID] = @SectionID

end else begin

	insert into [Section] (
		[SectionID],
		[ParentSectionID],
		[CommunityID],
		[Name],
		[Description],
		[Touched],
		[SortOrder],
		[IsEnabled],
		[IsVisible],
		[SyndicateFeed],
		[SyndicateSitemap],
		[Owner],
		[ModuleID],
		[Theme],
		[Style]
	) values (
		@SectionID,
		@ParentSectionID,
		@CommunityID,
		@Name,
		@Description,
		@Touched,
		@SortOrder,
		@IsEnabled,
		@IsVisible,
		@SyndicateFeed,
		@SyndicateSitemap,
		@Owner,
		@ModuleID,
		@Theme,
		@Style
	)

end

go

--------------------------------------------------------------------------------------------
--	Delete entity
--------------------------------------------------------------------------------------------
if exists (select * from sysobjects where type = 'P' and name = 'ManagedFusion_Section_Delete')
	begin
		print 'Dropping Procedure ManagedFusion_Section_Delete'
		drop procedure ManagedFusion_Section_Delete
	end
go

print 'Creating Procedure ManagedFusion_Section_Delete'
go

create procedure ManagedFusion_Section_Delete
(
	@SectionID as int
)
as

--	Author:		ManagedFusion [http://www.managedfusion.com]
--	Date:		11/22/2006
--	Purpose:	Delete the entity in Section.

delete from 
	[Section]

where
	[SectionID] = @SectionID

go

set nocount off
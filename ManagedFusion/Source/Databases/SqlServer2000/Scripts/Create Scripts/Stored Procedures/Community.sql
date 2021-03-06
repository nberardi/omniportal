/*
 * 	Template:		This code was generated by the ManagedFusion [http://www.managedfusion.com] Data Layer Template.
 * 	Created On :	11/22/2006
 * 	Remarks:		Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
 */

set nocount on

--------------------------------------------------------------------------------------------
--	Insert or Update entity
--------------------------------------------------------------------------------------------

if exists (select * from sysobjects where type = 'P' and name = 'ManagedFusion_Community')
	begin
		print 'Dropping Procedure ManagedFusion_Community'
		drop procedure ManagedFusion_Community
	end
go

print 'Creating Procedure ManagedFusion_Community'
go

create procedure ManagedFusion_Community
(
	@CommunityID as int = null,
	@UniversalID as uniqueidentifier,
	@Name as nvarchar(32),
	@Description as nvarchar(128),
	@Touched as datetime
)
as

--	Author:		ManagedFusion [http://www.managedfusion.com]
--	Date:		11/22/2006
--	Purpose:	Inserts or updates an entity into Community.

-- check to make sure the record already doesn't exisit
if exists (	
	select *
	from [Community] 
	where 
		[CommunityID] = @CommunityID
) begin

	update 
		[Community]
	
	set
		[UniversalID] = @UniversalID,
		[Name] = @Name,
		[Description] = @Description,
		[Touched] = @Touched
	
	where
		[CommunityID] = @CommunityID

end else begin

	insert into [Community] (
		[CommunityID],
		[UniversalID],
		[Name],
		[Description],
		[Touched]
	) values (
		@CommunityID,
		@UniversalID,
		@Name,
		@Description,
		@Touched
	)

end

go

--------------------------------------------------------------------------------------------
--	Delete entity
--------------------------------------------------------------------------------------------
if exists (select * from sysobjects where type = 'P' and name = 'ManagedFusion_Community_Delete')
	begin
		print 'Dropping Procedure ManagedFusion_Community_Delete'
		drop procedure ManagedFusion_Community_Delete
	end
go

print 'Creating Procedure ManagedFusion_Community_Delete'
go

create procedure ManagedFusion_Community_Delete
(
	@CommunityID as int
)
as

--	Author:		ManagedFusion [http://www.managedfusion.com]
--	Date:		11/22/2006
--	Purpose:	Delete the entity in Community.

delete from 
	[Community]

where
	[CommunityID] = @CommunityID

go

set nocount off
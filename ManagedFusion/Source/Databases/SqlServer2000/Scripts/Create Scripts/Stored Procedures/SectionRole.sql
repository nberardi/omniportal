/*
 * 	Template:		This code was generated by the ManagedFusion [http://www.managedfusion.com] Data Layer Template.
 * 	Created On :	11/22/2006
 * 	Remarks:		Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
 */

set nocount on

--------------------------------------------------------------------------------------------
--	Insert or Update entity
--------------------------------------------------------------------------------------------

if exists (select * from sysobjects where type = 'P' and name = 'ManagedFusion_SectionRole')
	begin
		print 'Dropping Procedure ManagedFusion_SectionRole'
		drop procedure ManagedFusion_SectionRole
	end
go

print 'Creating Procedure ManagedFusion_SectionRole'
go

create procedure ManagedFusion_SectionRole
(
	@SectionID as int = null,
	@Role as nvarchar(32) = null,
	@Tasks as nvarchar(32)
)
as

--	Author:		ManagedFusion [http://www.managedfusion.com]
--	Date:		11/22/2006
--	Purpose:	Inserts or updates an entity into SectionRole.

-- check to make sure the record already doesn't exisit
if exists (	
	select *
	from [SectionRole] 
	where 
		[SectionID] = @SectionID
		and [Role] = @Role
) begin

	update 
		[SectionRole]
	
	set
		[SectionID] = @SectionID,
		[Role] = @Role,
		[Tasks] = @Tasks
	
	where
		[SectionID] = @SectionID
		and [Role] = @Role

end else begin

	insert into [SectionRole] (
		[SectionID],
		[Role],
		[Tasks]
	) values (
		@SectionID,
		@Role,
		@Tasks
	)

end

go

--------------------------------------------------------------------------------------------
--	Delete entity
--------------------------------------------------------------------------------------------
if exists (select * from sysobjects where type = 'P' and name = 'ManagedFusion_SectionRole_Delete')
	begin
		print 'Dropping Procedure ManagedFusion_SectionRole_Delete'
		drop procedure ManagedFusion_SectionRole_Delete
	end
go

print 'Creating Procedure ManagedFusion_SectionRole_Delete'
go

create procedure ManagedFusion_SectionRole_Delete
(
	@SectionID as int,
	@Role as nvarchar(32)
)
as

--	Author:		ManagedFusion [http://www.managedfusion.com]
--	Date:		11/22/2006
--	Purpose:	Delete the entity in SectionRole.

delete from 
	[SectionRole]

where
	[SectionID] = @SectionID
	and [Role] = @Role

go

set nocount off
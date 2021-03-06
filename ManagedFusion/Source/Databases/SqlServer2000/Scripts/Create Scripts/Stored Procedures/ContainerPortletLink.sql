/*
 * 	Template:		This code was generated by the ManagedFusion [http://www.managedfusion.com] Data Layer Template.
 * 	Created On :	11/22/2006
 * 	Remarks:		Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
 */

set nocount on

--------------------------------------------------------------------------------------------
--	Insert or Update entity
--------------------------------------------------------------------------------------------

if exists (select * from sysobjects where type = 'P' and name = 'ManagedFusion_ContainerPortletLink')
	begin
		print 'Dropping Procedure ManagedFusion_ContainerPortletLink'
		drop procedure ManagedFusion_ContainerPortletLink
	end
go

print 'Creating Procedure ManagedFusion_ContainerPortletLink'
go

create procedure ManagedFusion_ContainerPortletLink
(
	@ContainerID as int = null,
	@PortletID as int = null,
	@SortOrder as int
)
as

--	Author:		ManagedFusion [http://www.managedfusion.com]
--	Date:		11/22/2006
--	Purpose:	Inserts or updates an entity into ContainerPortletLink.

-- check to make sure the record already doesn't exisit
if exists (	
	select *
	from [ContainerPortletLink] 
	where 
		[ContainerID] = @ContainerID
		and [PortletID] = @PortletID
) begin

	update 
		[ContainerPortletLink]
	
	set
		[ContainerID] = @ContainerID,
		[PortletID] = @PortletID,
		[SortOrder] = @SortOrder
	
	where
		[ContainerID] = @ContainerID
		and [PortletID] = @PortletID

end else begin

	insert into [ContainerPortletLink] (
		[ContainerID],
		[PortletID],
		[SortOrder]
	) values (
		@ContainerID,
		@PortletID,
		@SortOrder
	)

end

go

--------------------------------------------------------------------------------------------
--	Delete entity
--------------------------------------------------------------------------------------------
if exists (select * from sysobjects where type = 'P' and name = 'ManagedFusion_ContainerPortletLink_Delete')
	begin
		print 'Dropping Procedure ManagedFusion_ContainerPortletLink_Delete'
		drop procedure ManagedFusion_ContainerPortletLink_Delete
	end
go

print 'Creating Procedure ManagedFusion_ContainerPortletLink_Delete'
go

create procedure ManagedFusion_ContainerPortletLink_Delete
(
	@ContainerID as int,
	@PortletID as int
)
as

--	Author:		ManagedFusion [http://www.managedfusion.com]
--	Date:		11/22/2006
--	Purpose:	Delete the entity in ContainerPortletLink.

delete from 
	[ContainerPortletLink]

where
	[ContainerID] = @ContainerID
	and [PortletID] = @PortletID

go

set nocount off
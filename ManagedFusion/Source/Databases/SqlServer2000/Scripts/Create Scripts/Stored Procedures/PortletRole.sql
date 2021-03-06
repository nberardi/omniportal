/*
 * 	Template:		This code was generated by the ManagedFusion [http://www.managedfusion.com] Data Layer Template.
 * 	Created On :	11/22/2006
 * 	Remarks:		Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
 */

set nocount on

--------------------------------------------------------------------------------------------
--	Insert or Update entity
--------------------------------------------------------------------------------------------

if exists (select * from sysobjects where type = 'P' and name = 'ManagedFusion_PortletRole')
	begin
		print 'Dropping Procedure ManagedFusion_PortletRole'
		drop procedure ManagedFusion_PortletRole
	end
go

print 'Creating Procedure ManagedFusion_PortletRole'
go

create procedure ManagedFusion_PortletRole
(
	@PortletID as int = null,
	@Role as nvarchar(32) = null,
	@Permissions as ntext
)
as

--	Author:		ManagedFusion [http://www.managedfusion.com]
--	Date:		11/22/2006
--	Purpose:	Inserts or updates an entity into PortletRole.

-- check to make sure the record already doesn't exisit
if exists (	
	select *
	from [PortletRole] 
	where 
		[PortletID] = @PortletID
		and [Role] = @Role
) begin

	update 
		[PortletRole]
	
	set
		[PortletID] = @PortletID,
		[Role] = @Role,
		[Permissions] = @Permissions
	
	where
		[PortletID] = @PortletID
		and [Role] = @Role

end else begin

	insert into [PortletRole] (
		[PortletID],
		[Role],
		[Permissions]
	) values (
		@PortletID,
		@Role,
		@Permissions
	)

end

go

--------------------------------------------------------------------------------------------
--	Delete entity
--------------------------------------------------------------------------------------------
if exists (select * from sysobjects where type = 'P' and name = 'ManagedFusion_PortletRole_Delete')
	begin
		print 'Dropping Procedure ManagedFusion_PortletRole_Delete'
		drop procedure ManagedFusion_PortletRole_Delete
	end
go

print 'Creating Procedure ManagedFusion_PortletRole_Delete'
go

create procedure ManagedFusion_PortletRole_Delete
(
	@PortletID as int,
	@Role as nvarchar(32)
)
as

--	Author:		ManagedFusion [http://www.managedfusion.com]
--	Date:		11/22/2006
--	Purpose:	Delete the entity in PortletRole.

delete from 
	[PortletRole]

where
	[PortletID] = @PortletID
	and [Role] = @Role

go

set nocount off
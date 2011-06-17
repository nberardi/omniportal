-- //// Select All Stored procedure.
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Blog_Posts_SelectDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure [dbo].[Blog_Posts_SelectDateRange]
GO

---------------------------------------------------------------------------------
-- Stored procedure that will select all rows from the table 'Blog_Posts'
-- Returns: @ErrorCode int
---------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Blog_Posts_SelectDateRange]
	@StartDate datetime,
	@StopDate datetime,
	@ErrorCode int OUTPUT
AS
-- SELECT all rows from the table.
SELECT
	[posts].[ID],
	[posts].[Title],
	[posts].[Body],
	[posts].[Published],
	[posts].[AllowComments],
	[posts].[User_ID],
	[posts].[Syndicate],
	[posts].[TitleUrl],
	[posts].[Source],
	[posts].[SourceUrl],
	[posts].[Created],
	[posts].[Issued],
	[posts].[Modified]
FROM 
	[dbo].[Blog_Posts] AS posts
WHERE
	[posts].[Issued] BETWEEN @StartDate AND @StopDate
ORDER BY 
	[posts].[Issued] DESC
-- Get the Error Code for the statement just executed.
SELECT @ErrorCode=@@ERROR
GO

---------------------------------------------------------------------------------
-- Stored procedure that will select all rows from the table 'Blog_Posts'
-- Returns: @ErrorCode int
---------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[Blog_Posts_SelectAll]
	@ErrorCode int OUTPUT
AS
-- SELECT all rows from the table.
SELECT
	[ID],
	[Title],
	[Body],
	[Published],
	[AllowComments],
	[User_ID],
	[Syndicate],
	[TitleUrl],
	[Source],
	[SourceUrl],
	[Created],
	[Issued],
	[Modified]
FROM [dbo].[Blog_Posts]
ORDER BY 
	[Issued] DESC
-- Get the Error Code for the statement just executed.
SELECT @ErrorCode=@@ERROR
GO

-- //// Select All Stored procedure.
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Blog_Posts_SelectTop]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure [dbo].[Blog_Posts_SelectTop]
GO

---------------------------------------------------------------------------------
-- Stored procedure that will select all rows from the table 'Blog_Posts'
-- Returns: @ErrorCode int
---------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[Blog_Posts_SelectTop]
	@Top int,
	@ErrorCode int OUTPUT
AS
-- SELECT all rows from the table.
DECLARE @SQL nvarchar(500)
SET @SQL = 'SELECT
	TOP ' + convert(nvarchar, @Top) + '
	[ID],
	[Title],
	[Body],
	[Published],
	[AllowComments],
	[User_ID],
	[Syndicate],
	[TitleUrl],
	[Source],
	[SourceUrl],
	[Created],
	[Issued],
	[Modified]
FROM [dbo].[Blog_Posts]
ORDER BY 
	[Issued] DESC'

-- Execute SQL statement to select @top values	
EXEC(@SQL)
	
-- Get the Error Code for the statement just executed.
SELECT @ErrorCode=@@ERROR
GO
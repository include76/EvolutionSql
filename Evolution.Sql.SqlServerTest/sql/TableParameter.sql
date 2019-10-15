CREATE TYPE [dbo].[TstTableType] As Table
(
    UserId uniqueidentifier,
    FirstName NVARCHAR(50)
)
go;
CREATE PROCEDURE [dbo].[uspWithTableParameter]
(
    @myData As [dbo].[TstTableType] Readonly
)
AS

BEGIN
    SELECT * FROM @myData
END
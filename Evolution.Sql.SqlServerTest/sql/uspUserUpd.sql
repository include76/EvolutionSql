CREATE PROC uspUserUpd(
	@pUserId		uniqueidentifier,
	@pFirstName		varchar(256),
	@pLastName		varchar(256),
	@pUpdatedOn		datetime2
)
AS 
BEGIN
	UPDATE [user] SET FirstName = @pFirstName,
		LastName	=	@pLastName,
		UpdatedOn	=	@pUpdatedOn
	WHERE UserId = @pUserId
END
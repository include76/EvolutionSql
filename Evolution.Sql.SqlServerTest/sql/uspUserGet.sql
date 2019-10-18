create procedure uspUserGet(
	@userId uniqueidentifier,
	@totalCount int out
)
as
begin
	select * from [user] where userid = @userId
	select @totalCount = count(1) from [user]
end
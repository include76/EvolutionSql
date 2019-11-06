create procedure uspBlogIns(@Title nvarchar(1000),
	@content nvarchar(max),
	@CreatedBy uniqueidentifier,
	@CreatedOn datetime2,
	@UpdatedOn datetime2
)
as
begin
	insert into Blog values(@Title, @content, @CreatedBy, @CreatedOn, @UpdatedOn)
	select SCOPE_IDENTITY()
end